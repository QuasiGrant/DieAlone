if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("Cave") != null) return "Cave already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x, sizeY = data.size.y;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
    throw new System.Exception("no prefab named " + name);
}
UnityEngine.GameObject Place(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, float yaw, bool collider = true, string rename = null)
{
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(name), scene);
    go.transform.SetParent(parent, false); go.transform.position = pos; go.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    if (rename != null) go.name = rename;
    if (collider && go.GetComponentInChildren<UnityEngine.Collider>() == null)
    {
        var rends = go.GetComponentsInChildren<UnityEngine.Renderer>();
        if (rends.Length > 0) { var b = rends[0].bounds; foreach (var r in rends) b.Encapsulate(r.bounds); var box = go.AddComponent<UnityEngine.BoxCollider>(); box.center = go.transform.InverseTransformPoint(b.center); var s = go.transform.InverseTransformVector(b.size); box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(s.x), UnityEngine.Mathf.Abs(s.y), UnityEngine.Mathf.Abs(s.z)); }
    }
    return go;
}
// ---- Shape: hill centred at (300, 280), tunnel from the mouth at z 262 north to the chamber at (300, 281).
var hillC = new UnityEngine.Vector2(300f, 280f); float hillR = 22f, hillH = 12f, floorY = 24f;
var chamberC = new UnityEngine.Vector2(300f, 281f); float chamberR = 6f;
float tunnelW = 3.2f, tunnelZ0 = 258f, tunnelZ1 = 276f, tunnelX = 300f;
float Hill(UnityEngine.Vector2 p) { float d = UnityEngine.Vector2.Distance(p, hillC); return hillH * UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01((hillR - d) / 10f)); }
// Signed distance to the cut (positive inside): tunnel is a capsule along z, chamber a disc.
float CutInside(UnityEngine.Vector2 p)
{
    float dz = UnityEngine.Mathf.Clamp(p.y, tunnelZ0, tunnelZ1); float tun = tunnelW * 0.5f - UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(tunnelX + UnityEngine.Mathf.Sin(p.y * 0.35f) * 0.5f, dz));
    float cham = chamberR + UnityEngine.Mathf.PerlinNoise(p.x * 0.4f, p.y * 0.4f) * 1.2f - UnityEngine.Vector2.Distance(p, chamberC);
    return UnityEngine.Mathf.Max(tun, cham);
}
var spur = new UnityEngine.Vector2[] { new(305,176), new(314,212), new(306,248), new(300,262) };
float SpurDist(UnityEngine.Vector2 p) { float best = float.MaxValue; for (int i = 0; i < spur.Length - 1; i++) { var a = spur[i]; var b = spur[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }

int res = data.heightmapResolution; var heights = data.GetHeights(0, 0, res, res); int cutCells = 0;
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * size / (res - 1), z = zi * size / (res - 1); var p = new UnityEngine.Vector2(x, z);
    if (x < 270f || x > 330f || z < 170f || z > 310f) continue;
    float h = heights[zi, xi] * sizeY;
    float hill = Hill(p);
    if (hill > 0f) h = 24f + hill + (UnityEngine.Mathf.PerlinNoise(x / 6f, z / 6f) - 0.5f) * 0.8f * UnityEngine.Mathf.Clamp01(hill / 3f);
    float inside = CutInside(p);
    if (inside > -0.9f) { float w = UnityEngine.Mathf.Clamp01((inside + 0.9f) / 0.9f); h = UnityEngine.Mathf.Lerp(h, floorY + (UnityEngine.Mathf.PerlinNoise(x / 3f, z / 3f) - 0.5f) * 0.15f, w); cutCells++; }
    float sw = 1f - UnityEngine.Mathf.Clamp01((SpurDist(p) - 1.0f) / 2.0f);
    if (sw > 0f && z < 262f) h = UnityEngine.Mathf.Lerp(h, 24f, sw);
    heights[zi, xi] = UnityEngine.Mathf.Clamp01(h / sizeY);
}
data.SetHeights(0, 0, heights);
// ---- Paint: rock where steep (the trench walls and the hill), faint trail on the spur, bare floor inside.
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
int rockIdx = -1, mossIdx = -1; for (int i = 0; i < data.terrainLayers.Length; i++) { if (data.terrainLayers[i].name == "Layer_Rock") rockIdx = i; if (data.terrainLayers[i].name == "Layer_Moss") mossIdx = i; }
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    if (x < 270f || x > 330f || z < 170f || z > 310f) continue;
    float steep = data.GetSteepness(x / size, z / size);
    float rock = UnityEngine.Mathf.Clamp01((steep - 30f) / 15f);
    if (Hill(p) > 2f) rock = UnityEngine.Mathf.Max(rock, 0.6f);
    float trail = UnityEngine.Mathf.Max(alpha[zi, xi, 1], (1f - UnityEngine.Mathf.Clamp01((SpurDist(p) - 0.6f) / 0.6f)) * 0.55f);
    if (CutInside(p) > -0.5f) { trail = 1f; rock = 0f; }
    float moss = mossIdx >= 0 ? UnityEngine.Mathf.Min(alpha[zi, xi, mossIdx], 1f - trail - rock) : 0f;
    alpha[zi, xi, 1] = UnityEngine.Mathf.Min(trail, 1f - rock); if (rockIdx >= 0) alpha[zi, xi, rockIdx] = rock; if (mossIdx >= 0) alpha[zi, xi, mossIdx] = UnityEngine.Mathf.Max(0f, moss);
    alpha[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - alpha[zi, xi, 1] - rock - (mossIdx >= 0 ? alpha[zi, xi, mossIdx] : 0f));
}
data.SetAlphamaps(0, 0, alpha);
// ---- Trees off the hill and the spur; cover off the cut and the spur.
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cut = 0;
foreach (var ti in data.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (UnityEngine.Vector2.Distance(p, hillC) < hillR + 1f || SpurDist(p) < 2.2f) { cut++; continue; } keep.Add(ti); }
data.SetTreeInstances(keep.ToArray(), true);
int dres = data.detailResolution;
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++) { if (map[zi, xi] == 0) continue; var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres); if (CutInside(p) > -1.5f || SpurDist(p) < 0.9f || Hill(p) > 2.5f) { map[zi, xi] = 0; changed = true; } }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);

// ---- Cave object: rock material, ceiling blocks over the trench, rocks at the mouth, props, light, warp.
var rockMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/CaveRock.mat");
if (rockMat == null)
{
    var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Concrete034_1.0x1.0.mat");
    rockMat = new UnityEngine.Material(src); rockMat.SetColor("_BaseColor", new UnityEngine.Color(0.32f, 0.3f, 0.29f)); rockMat.SetTextureScale("_BaseMap", new UnityEngine.Vector2(0.5f, 0.5f)); rockMat.SetFloat("_Smoothness", 0.05f);
    UnityEditor.AssetDatabase.CreateAsset(rockMat, "Assets/Materials/CaveRock.mat");
}
var cave = new UnityEngine.GameObject("Cave"); var Cv = cave.transform;
var rng = new System.Random(97); int blocks = 0;
UnityEngine.GameObject Block(UnityEngine.Vector3 pos, UnityEngine.Vector3 sz, UnityEngine.Vector3 euler)
{
    var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var g = pb.gameObject; g.name = "Rock"; g.transform.SetParent(Cv, false);
    var pos2 = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    for (int i = 0; i < pos2.Count; i++) { var p = pos2[i]; float j = 1f + ((float)rng.NextDouble() - 0.5f) * 0.25f; pos2[i] = new UnityEngine.Vector3(p.x * sz.x * j, p.y * sz.y, p.z * sz.z * (1f + ((float)rng.NextDouble() - 0.5f) * 0.25f)); }
    pb.positions = pos2; pb.ToMesh(); pb.Refresh();
    g.transform.position = pos; g.transform.rotation = UnityEngine.Quaternion.Euler(euler);
    g.GetComponent<UnityEngine.Renderer>().sharedMaterial = rockMat;
    var mc = g.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = g.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
    blocks++; return g;
}
float ceilY = floorY + 3.4f;
// Tunnel ceiling: overlapping blocks every 2.2 m, wider than the cut so they bite into the walls.
for (float z = 259f; z <= tunnelZ1 + 1f; z += 2.2f)
    Block(V(tunnelX, ceilY + 1.6f + (float)rng.NextDouble() * 0.4f, z), V(6.5f, 3.2f, 3.0f), V(((float)rng.NextDouble() - 0.5f) * 8f, ((float)rng.NextDouble() - 0.5f) * 10f, ((float)rng.NextDouble() - 0.5f) * 6f));
// Chamber ceiling: a ring of blocks and a higher centre slab, so the room opens up.
for (int i = 0; i < 8; i++) { float a = i * 45f * UnityEngine.Mathf.Deg2Rad; Block(V(chamberC.x + UnityEngine.Mathf.Cos(a) * 4.6f, ceilY + 2.2f, chamberC.y + UnityEngine.Mathf.Sin(a) * 4.6f), V(6f, 3.4f, 5f), V(((float)rng.NextDouble() - 0.5f) * 10f, i * 45f + ((float)rng.NextDouble() - 0.5f) * 20f, ((float)rng.NextDouble() - 0.5f) * 10f)); }
Block(V(chamberC.x, ceilY + 3.8f, chamberC.y), V(9f, 3f, 9f), V(4f, 30f, -3f));
// Mouth: big pack rocks either side and one over the top.
foreach (var (rx, rz, s, yaw) in new[] { (297.2f, 259.5f, 3.2f, 20f), (302.9f, 259.0f, 2.8f, 200f), (296.5f, 263.5f, 2.6f, 90f), (303.6f, 264f, 2.4f, 300f), (300f, 262.5f, 2.2f, 0f) })
{
    var r = Place("CS_Rock_" + (1 + blocks % 8), Cv, V(rx, H(rx, rz) - 0.3f + (rx == 300f ? 3.9f : 0f), rz), yaw, rename: "MouthRock"); r.transform.localScale = V(s, s * 0.9f, s); blocks++;
}
// Inside: cold fire ring, fur bedroll, candles on a crate, a red glow.
Place("CS_Campfire_2", Cv, V(chamberC.x, floorY, chamberC.y + 1.5f), 0f, rename: "ColdFire");
Place("CS_Bedroll_Fur_2", Cv, V(chamberC.x - 3.2f, floorY, chamberC.y - 1.5f), 70f, collider: false, rename: "FurBed");
var crate = Place("CITW_Crate", Cv, V(chamberC.x + 3.4f, floorY, chamberC.y + 1.0f), 25f, rename: "Crate");
Place("CITW_Candle_1", Cv, crate.transform.position + V(0.15f, 0.62f, 0.1f), 0f, collider: false, rename: "Candle_1");
Place("CITW_Candle_2", Cv, crate.transform.position + V(-0.2f, 0.62f, -0.15f), 0f, collider: false, rename: "Candle_2");
Place("CITW_Candle_1", Cv, V(chamberC.x + 1.2f, floorY, chamberC.y - 4.2f), 0f, collider: false, rename: "Candle_3");
Place("CITW_Candle_2", Cv, V(chamberC.x - 4.0f, floorY, chamberC.y + 2.6f), 0f, collider: false, rename: "Candle_4");
var glow = new UnityEngine.GameObject("CaveGlow"); glow.transform.SetParent(Cv, false); glow.transform.position = V(chamberC.x + 2.5f, floorY + 1.0f, chamberC.y + 0.5f);
var gl = glow.AddComponent<UnityEngine.Light>(); gl.type = UnityEngine.LightType.Point; gl.color = new UnityEngine.Color(1f, 0.45f, 0.25f); gl.intensity = 1.4f; gl.range = 9f;
var warps = UnityEngine.GameObject.Find("DevWarps").transform;
var warp = new UnityEngine.GameObject("Cave"); warp.transform.SetParent(warps, false); warp.transform.position = V(300f, H(300f, 252f) + 0.2f, 252f); warp.transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cutCells=" + cutCells + " treesCut=" + cut + " blocks=" + blocks + " floor@chamber=" + H(300f, 281f).ToString("F1") + " hillTop=" + H(300f, 290f).ToString("F1") + " mouthWall=" + H(297.5f, 262f).ToString("F1") + " spurMid=" + H(310f, 230f).ToString("F1");
