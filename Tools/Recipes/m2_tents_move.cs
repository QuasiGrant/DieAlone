if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main2.unity") return "open Main2 first: " + scene.path;
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x, sizeY = data.size.y;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
// Lake outline (same control points as build_lake.cs) for keeping the new clearing off the bank.
var ctrl = new UnityEngine.Vector2[] { new(232,104), new(236,120), new(222,134), new(200,136), new(178,138), new(168,128), new(150,132), new(130,136), new(118,122), new(122,104), new(124,90), new(112,78), new(124,66), new(138,52), new(160,60), new(176,54), new(194,48), new(214,58), new(220,74), new(224,86), new(228,94), new(232,104) };
var poly = new System.Collections.Generic.List<UnityEngine.Vector2>();
for (int seg = 0; seg < 7; seg++) { var p0 = ctrl[seg * 3]; var p1 = ctrl[seg * 3 + 1]; var p2 = ctrl[seg * 3 + 2]; var p3 = ctrl[seg * 3 + 3]; for (int k = 0; k < 12; k++) { float t = k / 12f, u = 1f - t; poly.Add(u * u * u * p0 + 3f * u * u * t * p1 + 3f * u * t * t * p2 + t * t * t * p3); } }
bool Inside(UnityEngine.Vector2 p) { bool inside = false; for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++) { if ((poly[i].y > p.y) != (poly[j].y > p.y) && p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x) inside = !inside; } return inside; }
float EdgeDist(UnityEngine.Vector2 p) { float best = float.MaxValue; for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++) { var a = poly[j]; var b = poly[i]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }
float Signed(UnityEngine.Vector2 p) => Inside(p) ? EdgeDist(p) : -EdgeDist(p);
var oldFire = new UnityEngine.Vector2(314f, 123f); var newFire = new UnityEngine.Vector2(208f, 147f);
var oldBranch = new UnityEngine.Vector2[] { new(270,130), new(295,128), new(312,124) };
var newBranch = new UnityEngine.Vector2[] { new(270,130), new(250,138), new(228,146), new(216,148) };
var otherTrails = new[] { new UnityEngine.Vector2[] { new(255,168), new(265,145), new(270,130) }, new UnityEngine.Vector2[] { new(270,130), new(258,108), new(246,88) }, new UnityEngine.Vector2[] { new(270,130), new(290,100), new(312,75), new(322,60) }, new UnityEngine.Vector2[] { new(258,108), new(246,106), new(236,104) }, new UnityEngine.Vector2[] { new(249,80), new(262,73), new(280,65), new(300,60), new(314,62) }, new UnityEngine.Vector2[] { new(330,63), new(342,86), new(352,112), new(356,140), new(356,172) } };
float PD(UnityEngine.Vector2 p, UnityEngine.Vector2[] pts) { float best = float.MaxValue; for (int i = 0; i < pts.Length - 1; i++) { var a = pts[i]; var b = pts[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }
float OtherDist(UnityEngine.Vector2 p) { float d = PD(p, newBranch); foreach (var t in otherTrails) d = UnityEngine.Mathf.Min(d, PD(p, t)); return d; }
float NewClear(UnityEngine.Vector2 p) => UnityEngine.Vector2.Distance(p, newFire);

// ---- Heights: new clearing flat at 24 (never inside the bank), new branch flat, old clearing left as it is.
int res = data.heightmapResolution; var heights = data.GetHeights(0, 0, res, res);
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * size / (res - 1), z = zi * size / (res - 1); var p = new UnityEngine.Vector2(x, z);
    if (x < 190f || x > 280f || z < 120f || z > 170f) continue;
    float w = UnityEngine.Mathf.Max(1f - UnityEngine.Mathf.Clamp01((NewClear(p) - 13f) / 5f), 1f - UnityEngine.Mathf.Clamp01((PD(p, newBranch) - 1.2f) / 2.5f));
    if (Signed(p) > -3f) w = 0f;
    if (w <= 0f) continue;
    heights[zi, xi] = UnityEngine.Mathf.Lerp(heights[zi, xi] * sizeY, 24f, w) / sizeY;
}
data.SetHeights(0, 0, heights);
// ---- Paint: new branch as trail, old branch back to floor and moss, new clearing dirt, old clearing mossy.
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    if (x < 190f || x > 335f || z < 100f || z > 170f) continue;
    float rock = L > 2 ? alpha[zi, xi, 2] : 0f;
    bool oldZone = (PD(p, oldBranch) < 3f || UnityEngine.Vector2.Distance(p, oldFire) < 15f) && OtherDist(p) > 3f;
    float trail = alpha[zi, xi, 1];
    if (oldZone) trail = 0f;
    trail = UnityEngine.Mathf.Max(trail, 1f - UnityEngine.Mathf.Clamp01((PD(p, newBranch) - 0.7f) / 0.6f));
    if (NewClear(p) < 13f && Signed(p) < -3f) trail = UnityEngine.Mathf.Max(trail, 0.6f);
    float moss = oldZone ? UnityEngine.Mathf.Clamp01((OtherDist(p) - 1.3f) / 3f) : (L > 3 ? alpha[zi, xi, 3] : 0f);
    if (NewClear(p) < 16f) moss = UnityEngine.Mathf.Min(moss, 1f - UnityEngine.Mathf.Clamp01((16f - NewClear(p)) / 4f));
    trail = UnityEngine.Mathf.Min(trail, 1f - rock); moss = UnityEngine.Mathf.Min(moss, 1f - rock - trail);
    alpha[zi, xi, 1] = trail; if (L > 3) alpha[zi, xi, 3] = moss; alpha[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - trail - rock - moss);
}
data.SetAlphamaps(0, 0, alpha);
// ---- Trees: out of the new clearing and branch, back into the old spur and clearing.
var rng = new System.Random(233);
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cut = 0, added = 0;
foreach (var ti in data.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (NewClear(p) < 14f || PD(p, newBranch) < 2.2f) { cut++; continue; } keep.Add(ti); }
for (float gz = 105f; gz < 142f; gz += 3f) for (float gx = 272f; gx < 332f; gx += 3f)
{
    float x = gx + (float)(rng.NextDouble() - 0.5) * 2.4f, z = gz + (float)(rng.NextDouble() - 0.5) * 2.4f; var p = new UnityEngine.Vector2(x, z);
    bool oldZone = PD(p, oldBranch) < 3.5f || UnityEngine.Vector2.Distance(p, oldFire) < 14.5f; if (!oldZone || OtherDist(p) < 2.2f) continue;
    bool leaf = rng.NextDouble() < 0.2; float hs = leaf ? 1.3f + (float)rng.NextDouble() * 0.5f : 1.6f + (float)rng.NextDouble() * 0.8f;
    keep.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / size, 0f, z / size), prototypeIndex = leaf ? 4 + rng.Next(0, 2) : rng.Next(0, 4), heightScale = hs, widthScale = leaf ? hs : 1f + (float)rng.NextDouble() * 0.35f, rotation = (float)(rng.NextDouble() * 6.283), color = UnityEngine.Color.Lerp(new UnityEngine.Color(0.75f, 0.75f, 0.75f), UnityEngine.Color.white, (float)rng.NextDouble()), lightmapColor = UnityEngine.Color.white }); added++;
}
data.SetTreeInstances(keep.ToArray(), true);
int dres = data.detailResolution;
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
    {
        var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres);
        if (p.x < 190f || p.x > 335f || p.y < 100f || p.y > 170f) continue;
        if (NewClear(p) < 6f || PD(p, newBranch) < 1.0f) { if (map[zi, xi] != 0) { map[zi, xi] = 0; changed = true; } }
        else if (NewClear(p) < 14f && rng.NextDouble() < 0.6 && map[zi, xi] != 0) { map[zi, xi] = 0; changed = true; }
        else if (layer < 5 && (PD(p, oldBranch) < 3f || UnityEngine.Vector2.Distance(p, oldFire) < 14f) && OtherDist(p) > 1.3f && rng.NextDouble() < 0.2) { map[zi, xi] = 1 + rng.Next(0, 2); changed = true; }
    }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);

// ---- Move the whole site: rotate 180 degrees about the fire so the open side faces the new trail from the east.
var site = UnityEngine.GameObject.Find("Campsites/Campsite_1_Tents").transform;
var rot = UnityEngine.Quaternion.Euler(0f, 180f, 0f); int movedObjs = 0;
foreach (UnityEngine.Transform c in site)
{
    var p = c.position; float lift = p.y - H(p.x, p.z);
    var rel = rot * V(p.x - oldFire.x, 0f, p.z - oldFire.y);
    float nx = newFire.x + rel.x, nz = newFire.y + rel.z;
    c.position = V(nx, H(nx, nz) + lift, nz); c.rotation = rot * c.rotation; movedObjs++;
}
// Fork sign: the top board now points along the new branch.
var signs = UnityEngine.GameObject.Find("Signs").transform; int boards = 0;
foreach (UnityEngine.Transform b in signs)
{
    if (b.name != "Board" || UnityEngine.Vector2.Distance(new UnityEngine.Vector2(b.position.x, b.position.z), new UnityEngine.Vector2(271.5f, 131.5f)) > 1.5f) continue;
    if (b.position.y < 25.8f) continue;   // the top board is the campsite 1 board
    var origin = V(271.5f, b.position.y, 131.5f); var dir = V(250f, 0f, 138f) - V(271.5f, 0f, 131.5f); dir.y = 0f; dir.Normalize();
    b.position = origin + dir * 0.55f; b.rotation = UnityEngine.Quaternion.LookRotation(UnityEngine.Vector3.Cross(dir, UnityEngine.Vector3.up), UnityEngine.Vector3.up); boards++;
}
var warp = UnityEngine.GameObject.Find("DevWarps").transform.Find("Campsite 1 tents");
warp.position = V(222f, H(222f, 147f) + 0.2f, 147f); warp.rotation = UnityEngine.Quaternion.LookRotation(V(newFire.x, 0f, newFire.y) - V(222f, 0f, 147f), UnityEngine.Vector3.up);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " treesCut=" + cut + " treesAdded=" + added + " siteObjects=" + movedObjs + " boardsReaimed=" + boards + " fire=" + site.Find("FirePit").position.ToString("F1") + " shoreFromFire=" + (-Signed(newFire)).ToString("F1");
