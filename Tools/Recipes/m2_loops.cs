if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main2.unity") return "open Main2 first: " + scene.path;
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x, sizeY = data.size.y;
// Three connectors: lake shore to cabin 2, cabin 2 to cabin 3, cabin 3 up to the road by the office.
var L1 = new UnityEngine.Vector2[] { new(236,104), new(235.5f,95), new(238,87), new(243,82.5f), new(249,80) };
var L2 = new UnityEngine.Vector2[] { new(249,80), new(262,73), new(280,65), new(300,60), new(314,62) };
var L3 = new UnityEngine.Vector2[] { new(330,63), new(342,86), new(352,112), new(356,140), new(356,172) };
var trails = new[] { L1, L2, L3 };
float DistToPolyline(UnityEngine.Vector2 p, UnityEngine.Vector2[] pts) { float best = float.MaxValue; for (int i = 0; i < pts.Length - 1; i++) { var a = pts[i]; var b = pts[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }
(float d, UnityEngine.Vector2 q) Nearest(UnityEngine.Vector2 p)
{
    float best = float.MaxValue; var bq = p;
    foreach (var pts in trails) for (int i = 0; i < pts.Length - 1; i++) { var a = pts[i]; var b = pts[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); var q = a + ab * t; float d = UnityEngine.Vector2.Distance(p, q); if (d < best) { best = d; bq = q; } }
    return (best, bq);
}
// Heights: flatten across the trail at the height already there along its centre line (keeps any slope), 1.2 m flat, 2.5 m blend.
int res = data.heightmapResolution; var heights = data.GetHeights(0, 0, res, res); int touched = 0;
float HAt(UnityEngine.Vector2 q) => terrain.SampleHeight(V(q.x, 0, q.y));
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * size / (res - 1), z = zi * size / (res - 1); var p = new UnityEngine.Vector2(x, z);
    if (x < 225f || x > 365f || z < 55f || z > 180f) continue;
    var n = Nearest(p); float w = 1f - UnityEngine.Mathf.Clamp01((n.d - 1.2f) / 2.5f); if (w <= 0f) continue;
    float h = heights[zi, xi] * sizeY; float target = UnityEngine.Mathf.Min(HAt(n.q), 24.2f); if (target < 23.6f) target = 24f;   // never dip into the lake bed
    heights[zi, xi] = UnityEngine.Mathf.Lerp(h, target, w) / sizeY; touched++;
}
data.SetHeights(0, 0, heights);
// Paint: same trail width as the others.
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    if (x < 225f || x > 365f || z < 55f || z > 180f) continue;
    float trail = 1f - UnityEngine.Mathf.Clamp01((Nearest(p).d - 0.7f) / 0.6f); if (trail <= 0f) continue;
    float rock = L > 2 ? alpha[zi, xi, 2] : 0f; float t = UnityEngine.Mathf.Max(alpha[zi, xi, 1], trail * (1f - rock));
    alpha[zi, xi, 1] = t; if (L > 3) alpha[zi, xi, 3] = UnityEngine.Mathf.Min(alpha[zi, xi, 3], 1f - t - rock); alpha[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - t - rock - (L > 3 ? alpha[zi, xi, 3] : 0f));
}
data.SetAlphamaps(0, 0, alpha);
// Trees off the trails, cover off the core.
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cut = 0;
foreach (var ti in data.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (Nearest(p).d < 2.2f) { cut++; continue; } keep.Add(ti); }
data.SetTreeInstances(keep.ToArray(), true);
int dres = data.detailResolution;
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++) { if (map[zi, xi] == 0) continue; var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres); if (p.x < 225f || p.x > 365f || p.y < 55f || p.y > 180f) continue; if (Nearest(p).d < 1.0f) { map[zi, xi] = 0; changed = true; } }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
float len = 0f; foreach (var pts in trails) for (int i = 0; i < pts.Length - 1; i++) len += UnityEngine.Vector2.Distance(pts[i], pts[i + 1]);
return "saved=" + saved + " cellsFlattened=" + touched + " treesCut=" + cut + " newTrailMetres=" + len.ToString("F0");
