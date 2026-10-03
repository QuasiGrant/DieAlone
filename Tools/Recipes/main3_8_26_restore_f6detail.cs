// Main3 8.26 F6 detail restore (edit mode, Main3; Wren 2026-10-03). Round 2 cleared grass and fern round F6's first eye (84.43, 122.24) on
// the W1 leg (up to oldEyeR m round it, and oldLineR m along the line to the spring) to clear a frame whose "fern cards" were in fact
// RedFir6's lower boughs. This puts every detail layer's cells in that area back to their values in 'from' (Main3_TerrainData.asset at
// 002400f, before any F6 clearing, read through a temporary import that is deleted after). Then rerun main3_8_26_camp3.cs, which clears
// only its own areas again (the creek's band and the new F6 line at P72). One-off; saves the terrain and the scene.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
const string from = "Temp/terrain_002400f.asset", tmp = "Assets/_RookTemp/terrain_002400f.asset"; const float oldEyeR = 4.5f, oldLineR = 2.5f;
var oldEye = new UnityEngine.Vector2(84.43f, 122.24f); var spring = new UnityEngine.Vector2(85.8f, 126.0f);
float SegDist(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
var ter = UnityEngine.Terrain.activeTerrain; var data = ter.terrainData; var org = ter.transform.position;
System.IO.Directory.CreateDirectory("Assets/_RookTemp"); System.IO.File.Copy(from, tmp, true); UnityEditor.AssetDatabase.ImportAsset(tmp, UnityEditor.ImportAssetOptions.ForceSynchronousImport);
var old = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainData>(tmp); int restored = 0, cells = 0;
try
{
    if (old == null) return "could not load " + tmp;
    if (old.detailResolution != data.detailResolution || old.detailPrototypes.Length != data.detailPrototypes.Length) return "detail layout differs: resolution " + old.detailResolution + " vs " + data.detailResolution + ", layers " + old.detailPrototypes.Length + " vs " + data.detailPrototypes.Length;
    int res = data.detailResolution; float cw = data.size.x / res, ch = data.size.z / res; float reach = oldEyeR + UnityEngine.Vector2.Distance(oldEye, spring);
    int x0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt((oldEye.x - reach - org.x) / cw), 0, res - 1), x1 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.CeilToInt((oldEye.x + reach - org.x) / cw), 0, res - 1);
    int z0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt((oldEye.y - reach - org.z) / ch), 0, res - 1), z1 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.CeilToInt((oldEye.y + reach - org.z) / ch), 0, res - 1);
    int w = x1 - x0 + 1, h = z1 - z0 + 1;
    for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
    {
        var now = data.GetDetailLayer(x0, z0, w, h, layer); var was = old.GetDetailLayer(x0, z0, w, h, layer); bool changed = false;
        for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
        {
            var q = new UnityEngine.Vector2(org.x + (x0 + i + 0.5f) * cw, org.z + (z0 + j + 0.5f) * ch);
            if (UnityEngine.Vector2.Distance(q, oldEye) > oldEyeR && SegDist(q, oldEye, spring) > oldLineR) continue; if (layer == 0) cells++;
            if (now[j, i] == was[j, i]) continue; now[j, i] = was[j, i]; restored++; changed = true;
        }
        if (changed) data.SetDetailLayer(x0, z0, layer, now);
    }
}
finally { UnityEditor.AssetDatabase.DeleteAsset(tmp); UnityEditor.AssetDatabase.DeleteAsset("Assets/_RookTemp"); }
UnityEditor.EditorUtility.SetDirty(data); UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | cells in the area " + cells + " | layer cells restored " + restored;
