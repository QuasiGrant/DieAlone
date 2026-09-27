if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var sb = new System.Text.StringBuilder();

// ---- Stones on the ledge edge, north of the approach line.
UnityEngine.Transform ward = null; foreach (var r0 in scene.GetRootGameObjects()) if (r0.name == "Ward") ward = r0.transform;
var spots = new (string name, float x, float z)[] { ("Stone_1", 42.4f, 339.4f), ("Stone_2", 43.1f, 336.1f), ("Stone_3", 42.6f, 332.8f) };
foreach (var s in spots) { var st = ward.Find(s.name); st.position = V(s.x, H(s.x, s.z), s.z); }
// ---- Sign gone.
var signs = UnityEngine.GameObject.Find("Signs").transform;
int removed = 0;
for (int i = signs.childCount - 1; i >= 0; i--) { var c = signs.GetChild(i); if (c.name == "Sign_Ward" || (c.name == "Board" && c.position.x < 100f)) { UnityEngine.Object.DestroyImmediate(c.gameObject); removed++; } }
// ---- Warp point looks at the stones from the clearing.
var warp = UnityEngine.GameObject.Find("DevWarps").transform.Find("Ward");
warp.position = V(58f, H(58f, 324f) + 0.2f, 324f); warp.rotation = UnityEngine.Quaternion.LookRotation(V(43f, 0f, 336f) - V(58f, 0f, 324f), UnityEngine.Vector3.up);

// ---- Path A runs on into the clearing: paint and flatten the last leg (75,315) to (58,323).
var pathA = new UnityEngine.Vector2[] { new(75,315), new(110,300), new(130,270), new(115,240), new(140,215), new(175,225), new(200,200), new(225,195), new(238,185) };
var tail = new UnityEngine.Vector2[] { new(58,323), new(75,315) };
float SegDist(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); return UnityEngine.Vector2.Distance(p, a + ab * t); }
float TailDist(UnityEngine.Vector2 p) => SegDist(p, tail[0], tail[1]);
float PathDist(UnityEngine.Vector2 p) { float best = TailDist(p); for (int i = 0; i < pathA.Length - 1; i++) best = UnityEngine.Mathf.Min(best, SegDist(p, pathA[i], pathA[i + 1])); return best; }
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int painted = 0;
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares;
    float trail = 1f - UnityEngine.Mathf.Clamp01((TailDist(new UnityEngine.Vector2(x, z)) - 0.7f) / 0.6f);
    if (trail <= 0f) continue;
    float floor = alpha[zi, xi, 0], old = alpha[zi, xi, 1], rock = alpha.GetLength(2) > 2 ? alpha[zi, xi, 2] : 0f;
    float t = UnityEngine.Mathf.Max(old, trail * (1f - rock));
    alpha[zi, xi, 1] = t; alpha[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - t - rock); painted++;
}
data.SetAlphamaps(0, 0, alpha);

// ---- Clearing: trees and cover out of the new leg, out of the stone corner, plus one tree in ten thinned everywhere.
var rng = new System.Random(53);
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cutLeg = 0, cutCorner = 0, thinned = 0;
var corner = new UnityEngine.Vector2(45f, 336f);
foreach (var ti in data.treeInstances)
{
    var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size);
    if (TailDist(p) < 2.4f) { cutLeg++; continue; }
    if (UnityEngine.Vector2.Distance(p, corner) < 11f) { cutCorner++; continue; }
    if (ti.prototypeIndex <= 5 && rng.NextDouble() < 0.10) { thinned++; continue; }
    keep.Add(ti);
}
data.SetTreeInstances(keep.ToArray(), true);
int dres = data.detailResolution; int coverCut = 0;
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
    {
        if (map[zi, xi] == 0) continue;
        var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres);
        if (TailDist(p) < 1.0f || UnityEngine.Vector2.Distance(p, corner) < 9f) { map[zi, xi] = 0; changed = true; coverCut++; }
    }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " signRemoved=" + removed + " painted=" + painted + " treesCutLeg=" + cutLeg + " cutCorner=" + cutCorner + " thinned=" + thinned + " coverCut=" + coverCut + " treesLeft=" + keep.Count + " stone2=" + ward.Find("Stone_2").position.ToString("F1");
