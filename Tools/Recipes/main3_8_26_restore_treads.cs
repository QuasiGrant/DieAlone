// Main3 8.26 tread restore (edit mode, Main3; 2026-10-03): the first 8.26 runs cut the creek through trail treads (1.3 m at J by the plank
// bridge). The recipe now never cuts a tread; this puts the tread cells near the water back to their height before 8.26: every terrain
// cell within treadHalf + treadKeep of a trail centre line and within nearWater m of the water (the Creek strips' vertices and the pool)
// takes the larger of its height now and its height in 'from' (Main3_TerrainData.asset at f5f63bd, read through a temporary import that
// is deleted after). One-off; saves the scene and the terrain.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
const string from = "Temp/terrain_head.asset", tmp = "Assets/_RookTemp/terrain_head.asset"; const float treadHalf = 1.2f, treadKeep = 0.3f, nearWater = 3f;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
float SegDist(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
var water = new System.Collections.Generic.List<UnityEngine.Vector2>(); var creek = Root("Campsites").transform.Find("Camp_3/Layout826/Creek");
if (creek == null) return "no Layout826/Creek";
foreach (var mf in creek.GetComponentsInChildren<UnityEngine.MeshFilter>()) { if (mf.sharedMesh == null) continue; if (mf.name == "Pool") { var b = mf.GetComponent<UnityEngine.Renderer>().bounds; for (float x = b.min.x; x <= b.max.x; x += 0.5f) for (float z = b.min.z; z <= b.max.z; z += 0.5f) water.Add(new UnityEngine.Vector2(x, z)); continue; } foreach (var v in mf.sharedMesh.vertices) { var w = mf.transform.TransformPoint(v); water.Add(new UnityEngine.Vector2(w.x, w.z)); } }
var lines = new System.Collections.Generic.List<UnityEngine.Vector2[]>(); foreach (UnityEngine.Transform lg in Root("Trails").transform) { var l = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform p in lg) l.Add(new UnityEngine.Vector2(p.position.x, p.position.z)); if (l.Count > 1) lines.Add(l.ToArray()); }
System.IO.Directory.CreateDirectory("Assets/_RookTemp"); System.IO.File.Copy(from, tmp, true); UnityEditor.AssetDatabase.ImportAsset(tmp, UnityEditor.ImportAssetOptions.ForceSynchronousImport);
var head = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainData>(tmp); int raised = 0; float most = 0f; string where = "";
try
{
    if (head == null) return "could not load " + tmp;
    var ter = UnityEngine.Terrain.activeTerrain; var data = ter.terrainData; var org = ter.transform.position; int res = data.heightmapResolution; float cx = data.size.x / (res - 1), cz = data.size.z / (res - 1);
    if (head.heightmapResolution != res || head.size != data.size) return "the terrain's size changed since " + from;
    float x0 = float.MaxValue, x1 = float.MinValue, z0 = float.MaxValue, z1 = float.MinValue; foreach (var p in water) { x0 = UnityEngine.Mathf.Min(x0, p.x); x1 = UnityEngine.Mathf.Max(x1, p.x); z0 = UnityEngine.Mathf.Min(z0, p.y); z1 = UnityEngine.Mathf.Max(z1, p.y); }
    int i0 = UnityEngine.Mathf.FloorToInt((x0 - nearWater - org.x) / cx), i1 = UnityEngine.Mathf.CeilToInt((x1 + nearWater - org.x) / cx), j0 = UnityEngine.Mathf.FloorToInt((z0 - nearWater - org.z) / cz), j1 = UnityEngine.Mathf.CeilToInt((z1 + nearWater - org.z) / cz);
    int w = i1 - i0 + 1, h = j1 - j0 + 1; var now = data.GetHeights(i0, j0, w, h); var was = head.GetHeights(i0, j0, w, h);
    for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
    {
        if (was[j, i] <= now[j, i]) continue; var q = new UnityEngine.Vector2(org.x + (i0 + i) * cx, org.z + (j0 + j) * cz);
        float td = float.MaxValue; foreach (var l in lines) for (int k = 1; k < l.Length; k++) td = UnityEngine.Mathf.Min(td, SegDist(q, l[k - 1], l[k])); if (td >= treadHalf + treadKeep) continue;
        bool near = false; foreach (var p in water) if ((p - q).sqrMagnitude < nearWater * nearWater) { near = true; break; } if (!near) continue;
        float dm = (was[j, i] - now[j, i]) * data.size.y; if (dm > most) { most = dm; where = "(" + q.x.ToString("F1") + ", " + q.y.ToString("F1") + ")"; }
        now[j, i] = was[j, i]; raised++;
    }
    data.SetHeights(i0, j0, now); UnityEditor.EditorUtility.SetDirty(data); UnityEngine.Physics.SyncTransforms();
}
finally { UnityEditor.AssetDatabase.DeleteAsset(tmp); UnityEditor.AssetDatabase.DeleteAsset("Assets/_RookTemp"); }
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene); bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | tread cells put back " + raised + ", the most " + most.ToString("F2") + " m at " + where;
