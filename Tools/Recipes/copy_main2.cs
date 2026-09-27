if (UnityEngine.Application.isPlaying) return "stop play mode first";
var cur = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (cur.isDirty) UnityEditor.SceneManagement.EditorSceneManager.SaveScene(cur);
if (System.IO.File.Exists("Assets/Scenes/Main2.unity")) return "Main2 already exists";
// Copy the scene and every terrain data asset it uses, so 2.0 edits never touch 1.0.
if (!UnityEditor.AssetDatabase.CopyAsset("Assets/Scenes/Main.unity", "Assets/Scenes/Main2.unity")) return "scene copy failed";
var copies = new (string src, string dst)[] {
    ("Assets/Terrain/Main_TerrainData.asset", "Assets/Terrain/Main2_TerrainData.asset"),
    ("Assets/Terrain/Valley_TerrainData.asset", "Assets/Terrain/Main2_Valley_TerrainData.asset"),
    ("Assets/Terrain/East_TerrainData.asset", "Assets/Terrain/Main2_East_TerrainData.asset") };
foreach (var c in copies) if (!UnityEditor.AssetDatabase.CopyAsset(c.src, c.dst)) return "copy failed: " + c.src;
UnityEditor.AssetDatabase.Refresh();
var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Main2.unity", UnityEditor.SceneManagement.OpenSceneMode.Single);
var map = new System.Collections.Generic.Dictionary<string, string> { { "Terrain", copies[0].dst }, { "ValleyTerrain", copies[1].dst }, { "EastTerrain", copies[2].dst } };
var sb = new System.Text.StringBuilder();
foreach (var kv in map)
{
    var go = UnityEngine.GameObject.Find(kv.Key); if (go == null) { sb.Append(kv.Key + " missing; "); continue; }
    var td = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainData>(kv.Value);
    var t = go.GetComponent<UnityEngine.Terrain>(); t.terrainData = td;
    var col = go.GetComponent<UnityEngine.TerrainCollider>(); if (col != null) col.terrainData = td;
    sb.Append(kv.Key + " -> " + td.name + "; ");
}
// Build list: add Main2 after Main.
var scenes = new System.Collections.Generic.List<UnityEditor.EditorBuildSettingsScene>(UnityEditor.EditorBuildSettings.scenes);
bool present = false; foreach (var s in scenes) if (s.path == "Assets/Scenes/Main2.unity") present = true;
if (!present) { int idx = scenes.FindIndex(s => s.path == "Assets/Scenes/Main.unity"); scenes.Insert(idx < 0 ? scenes.Count : idx + 1, new UnityEditor.EditorBuildSettingsScene("Assets/Scenes/Main2.unity", true)); UnityEditor.EditorBuildSettings.scenes = scenes.ToArray(); }
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var bl = new System.Text.StringBuilder(); foreach (var s in UnityEditor.EditorBuildSettings.scenes) bl.Append(System.IO.Path.GetFileNameWithoutExtension(s.path) + " ");
return "saved=" + saved + " open=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " | " + sb + "| build: " + bl;
