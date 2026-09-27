if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main2.unity") return "open Main2 first: " + scene.path;
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x, sizeY = data.size.y;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
// ---- Everything that must stay seated: record its height above the ground before the terrain changes.
var seated = new System.Collections.Generic.List<(UnityEngine.Transform t, float off)>();
foreach (var rootName in new[] { "Signs", "DevWarps", "Camp", "Campsites", "Lake", "Entrance", "Cave", "Beats", "Player" })
{
    UnityEngine.Transform root = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == rootName) root = r.transform;
    if (root == null) continue;
    if (rootName == "Player") { seated.Add((root, root.position.y - H(root.position.x, root.position.z))); continue; }
    foreach (UnityEngine.Transform c in root)
    {
        // Groups of world-placed children (campsites, beat groups) seat each child; everything else seats as a unit.
        bool group = (rootName == "Campsites" || rootName == "Beats") && c.GetComponent<UnityEngine.Renderer>() == null;
        if (group) foreach (UnityEngine.Transform gc in c) seated.Add((gc, gc.position.y - H(gc.position.x, gc.position.z)));
        else seated.Add((c, c.position.y - H(c.position.x, c.position.z)));
    }
}
// ---- Where the ground may roll: outside clearings, the lake bank, the ward plateau, the hill and the cliff.
var flat = new (UnityEngine.Vector2 c, float r)[] { (new(250,180), 20f), (new(208,147), 16f), (new(247,84.5f), 11f), (new(325,56.5f), 12f), (new(373,181), 27f), (new(300,280), 26f), (new(66,323), 60f), (new(316,64), 6f), (new(236,200), 12f) };
var ctrl = new UnityEngine.Vector2[] { new(232,104), new(236,120), new(222,134), new(200,136), new(178,138), new(168,128), new(150,132), new(130,136), new(118,122), new(122,104), new(124,90), new(112,78), new(124,66), new(138,52), new(160,60), new(176,54), new(194,48), new(214,58), new(220,74), new(224,86), new(228,94), new(232,104) };
var poly = new System.Collections.Generic.List<UnityEngine.Vector2>();
for (int seg = 0; seg < 7; seg++) { var p0 = ctrl[seg * 3]; var p1 = ctrl[seg * 3 + 1]; var p2 = ctrl[seg * 3 + 2]; var p3 = ctrl[seg * 3 + 3]; for (int k = 0; k < 12; k++) { float t = k / 12f, u = 1f - t; poly.Add(u * u * u * p0 + 3f * u * u * t * p1 + 3f * u * t * t * p2 + t * t * t * p3); } }
bool Inside(UnityEngine.Vector2 p) { bool inside = false; for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++) { if ((poly[i].y > p.y) != (poly[j].y > p.y) && p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x) inside = !inside; } return inside; }
float EdgeDist(UnityEngine.Vector2 p) { float best = float.MaxValue; for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++) { var a = poly[j]; var b = poly[i]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }
float LakeSigned(UnityEngine.Vector2 p) => Inside(p) ? EdgeDist(p) : -EdgeDist(p);
float FlatWeight(UnityEngine.Vector2 p)
{
    float w = 0f; foreach (var f in flat) w = UnityEngine.Mathf.Max(w, 1f - UnityEngine.Mathf.Clamp01((UnityEngine.Vector2.Distance(p, f.c) - f.r) / 8f));
    w = UnityEngine.Mathf.Max(w, 1f - UnityEngine.Mathf.Clamp01((-LakeSigned(p) - 8f) / 8f));
    if (p.x < 60f) w = 1f;
    return w;
}
float Roll(UnityEngine.Vector2 p)
{
    float r = (UnityEngine.Mathf.PerlinNoise(p.x / 48f + 11.3f, p.y / 48f + 5.7f) - 0.5f) * 5f + (UnityEngine.Mathf.PerlinNoise(p.x / 17f + 3.1f, p.y / 17f + 8.2f) - 0.5f) * 1.2f;
    r += 3.2f * UnityEngine.Mathf.SmoothStep(0f, 1f, 1f - UnityEngine.Mathf.Clamp01((UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(240f, 122f)) - 6f) / 20f));   // low rise between the camp and the lake
    return r * (1f - FlatWeight(p));
}
// All trails, so each stays flat across and follows its centre line.
var trails = new[] {
    new UnityEngine.Vector2[] { new(58,323), new(75,315), new(110,300), new(130,270), new(115,240), new(140,215), new(175,225), new(200,200), new(225,195), new(238,185) },
    new UnityEngine.Vector2[] { new(255,168), new(265,145), new(270,130) }, new UnityEngine.Vector2[] { new(270,130), new(250,138), new(228,146), new(216,148) },
    new UnityEngine.Vector2[] { new(270,130), new(258,108), new(246,88) }, new UnityEngine.Vector2[] { new(270,130), new(290,100), new(312,75), new(322,60) },
    new UnityEngine.Vector2[] { new(258,108), new(246,106), new(236,104) }, new UnityEngine.Vector2[] { new(236,104), new(235.5f,95), new(238,87), new(243,82.5f), new(249,80) },
    new UnityEngine.Vector2[] { new(249,80), new(262,73), new(280,65), new(300,60), new(314,62) }, new UnityEngine.Vector2[] { new(330,63), new(342,86), new(352,112), new(356,140), new(356,172) },
    new UnityEngine.Vector2[] { new(266,178), new(300,176), new(340,174), new(388,174), new(400,174) }, new UnityEngine.Vector2[] { new(305,176), new(314,212), new(306,248), new(300,262) } };
(float d, UnityEngine.Vector2 q) Nearest(UnityEngine.Vector2 p)
{
    float best = float.MaxValue; var bq = p;
    foreach (var pts in trails) for (int i = 0; i < pts.Length - 1; i++) { var a = pts[i]; var b = pts[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); var q = a + ab * t; float d = UnityEngine.Vector2.Distance(p, q); if (d < best) { best = d; bq = q; } }
    return (best, bq);
}
// ---- Apply: new height = old height + roll, with the trail band taking the centre-line roll.
int res = data.heightmapResolution; var heights = data.GetHeights(0, 0, res, res); int changed = 0;
var old = (float[,])heights.Clone();
float OldAt(UnityEngine.Vector2 p) { float fx = p.x / size * (res - 1), fz = p.y / size * (res - 1); int x0 = UnityEngine.Mathf.Clamp((int)fx, 0, res - 2), z0 = UnityEngine.Mathf.Clamp((int)fz, 0, res - 2); float tx = fx - x0, tz = fz - z0; return UnityEngine.Mathf.Lerp(UnityEngine.Mathf.Lerp(old[z0, x0], old[z0, x0 + 1], tx), UnityEngine.Mathf.Lerp(old[z0 + 1, x0], old[z0 + 1, x0 + 1], tx), tz) * sizeY; }
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * size / (res - 1), z = zi * size / (res - 1); var p = new UnityEngine.Vector2(x, z);
    if (x < 60f) continue;
    var n = Nearest(p); float tw = 1f - UnityEngine.Mathf.Clamp01((n.d - 1.2f) / 2.5f);
    float target = tw > 0f ? UnityEngine.Mathf.Lerp(old[zi, xi] * sizeY + Roll(p), OldAt(n.q) + Roll(n.q), tw) : old[zi, xi] * sizeY + Roll(p);
    if (UnityEngine.Mathf.Abs(target - old[zi, xi] * sizeY) > 0.01f) changed++;
    heights[zi, xi] = UnityEngine.Mathf.Clamp01(target / sizeY);
}
data.SetHeights(0, 0, heights);
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);
// ---- Re-seat.
int reseated = 0; float maxShift = 0f;
foreach (var s in seated) { var p = s.t.position; float ny = H(p.x, p.z) + s.off; maxShift = UnityEngine.Mathf.Max(maxShift, UnityEngine.Mathf.Abs(ny - p.y)); s.t.position = V(p.x, ny, p.z); reseated++; }
UnityEngine.Physics.SyncTransforms();
// ---- Cave hill: trees back on the lower slopes and rocks up the sides, keeping the spur and the cut clear.
var rng = new System.Random(257); var hillC = new UnityEngine.Vector2(300f, 280f);
var trees = new System.Collections.Generic.List<UnityEngine.TreeInstance>(data.treeInstances); int hillTrees = 0;
for (float gz = 256f; gz < 304f; gz += 3.2f) for (float gx = 276f; gx < 324f; gx += 3.2f)
{
    float x = gx + (float)(rng.NextDouble() - 0.5) * 2.4f, z = gz + (float)(rng.NextDouble() - 0.5) * 2.4f; var p = new UnityEngine.Vector2(x, z);
    float d = UnityEngine.Vector2.Distance(p, hillC); if (d < 13.5f || d > 23f) continue;
    if (Nearest(p).d < 2.4f) continue;
    if (UnityEngine.Mathf.Abs(x - 300f) < 5.5f && z < 270f) continue;   // keep the mouth approach open
    float hs = 1.4f + (float)rng.NextDouble() * 0.7f;
    trees.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / size, 0f, z / size), prototypeIndex = rng.Next(0, 4), heightScale = hs, widthScale = 1f + (float)rng.NextDouble() * 0.3f, rotation = (float)(rng.NextDouble() * 6.283), color = UnityEngine.Color.Lerp(new UnityEngine.Color(0.75f, 0.75f, 0.75f), UnityEngine.Color.white, (float)rng.NextDouble()), lightmapColor = UnityEngine.Color.white }); hillTrees++;
}
data.SetTreeInstances(trees.ToArray(), true);
UnityEngine.Transform cave = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "Cave") cave = r.transform;
string rockRoot = "Assets/Revolving Pizza Games/Campsite/Prefabs/Rocks and Stones/"; var tintMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/CaveRockPack.mat"); int hillRocks = 0;
for (int i = 0; i < 16; i++)
{
    float a = (float)rng.NextDouble() * 6.283f, d = 9f + (float)rng.NextDouble() * 10f; float x = hillC.x + UnityEngine.Mathf.Cos(a) * d, z = hillC.y + UnityEngine.Mathf.Sin(a) * d;
    if (UnityEngine.Mathf.Abs(x - 300f) < 6f && z < 275f) continue;
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(rockRoot + "CS_Rock_" + (1 + rng.Next(0, 8)) + ".prefab"), scene);
    go.name = "HillRock"; go.transform.SetParent(cave, false); go.transform.position = V(x, H(x, z) - 0.4f, z); go.transform.rotation = UnityEngine.Quaternion.Euler((float)rng.NextDouble() * 30f, (float)rng.NextDouble() * 360f, (float)rng.NextDouble() * 30f); float s = 1.4f + (float)rng.NextDouble() * 1.4f; go.transform.localScale = V(s, s * 0.8f, s);
    foreach (var mr in go.GetComponentsInChildren<UnityEngine.MeshRenderer>()) { mr.sharedMaterial = tintMat; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf != null) { var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; } }
    hillRocks++;
}
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cellsChanged=" + changed + " reseated=" + reseated + " maxShift=" + maxShift.ToString("F2") + " hillTrees=" + hillTrees + " hillRocks=" + hillRocks + " rise@(240,122)=" + H(240f, 122f).ToString("F1") + " spur@(246,106)=" + H(246f, 106f).ToString("F1") + " forest@(180,180)=" + H(180f, 180f).ToString("F1");
