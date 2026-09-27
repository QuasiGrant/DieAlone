if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
string packRoot = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
var copies = new System.Collections.Generic.Dictionary<UnityEngine.Material, UnityEngine.Material>();
UnityEngine.Material Instanced(UnityEngine.Material src)
{
    if (src == null) return null;
    if (copies.TryGetValue(src, out var m)) return m;
    string path = "Assets/Materials/Forest/" + src.name + "_Instanced.mat";
    m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(src); m.enableInstancing = true; UnityEditor.AssetDatabase.CreateAsset(m, path); }
    copies[src] = m; return m;
}
int swapped = 0;
UnityEngine.GameObject Variant(string packName)
{
    string outPath = "Assets/Prefabs/Forest/Valley_" + packName + ".prefab";
    var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(outPath);
    if (existing != null) return existing;
    var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + packName + ".prefab");
    var inst = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src);
    foreach (var r in inst.GetComponentsInChildren<UnityEngine.Renderer>(true))
    {
        var mats = r.sharedMaterials;
        for (int i = 0; i < mats.Length; i++) if (mats[i] != null && !mats[i].enableInstancing) { mats[i] = Instanced(mats[i]); swapped++; }
        r.sharedMaterials = mats;
    }
    var v = UnityEditor.PrefabUtility.SaveAsPrefabAsset(inst, outPath); UnityEngine.Object.DestroyImmediate(inst); return v;
}
var terrain = UnityEngine.GameObject.Find("ValleyTerrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData;
var protos = data.treePrototypes;
for (int i = 0; i < protos.Length; i++) if (UnityEditor.AssetDatabase.GetAssetPath(protos[i].prefab).StartsWith("Assets/suffercord")) protos[i].prefab = Variant(protos[i].prefab.name);
data.treePrototypes = protos;
terrain.treeDistance = 400f; terrain.treeBillboardDistance = 400f;
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var sb = new System.Text.StringBuilder("saved=" + saved + " swapped=" + swapped + " protos=");
foreach (var pr in data.treePrototypes) sb.Append(pr.prefab.name + " ");
return sb.ToString();
