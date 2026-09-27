if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
string packRoot = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Materials/Forest")) UnityEditor.AssetDatabase.CreateFolder("Assets/Materials", "Forest");
// One instanced copy per pack material, keyed by name, kept in the project.
var copies = new System.Collections.Generic.Dictionary<UnityEngine.Material, UnityEngine.Material>();
UnityEngine.Material Instanced(UnityEngine.Material src)
{
    if (src == null) return null;
    if (copies.TryGetValue(src, out var m)) return m;
    string path = "Assets/Materials/Forest/" + src.name + "_Instanced.mat";
    m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(src); m.enableInstancing = true; UnityEditor.AssetDatabase.CreateAsset(m, path); }
    else { m.CopyPropertiesFromMaterial(src); m.enableInstancing = true; UnityEditor.EditorUtility.SetDirty(m); }
    copies[src] = m; return m;
}
int swapped = 0;
void Retarget(UnityEngine.GameObject prefab)
{
    var root = UnityEditor.PrefabUtility.LoadPrefabContents(UnityEditor.AssetDatabase.GetAssetPath(prefab));
    foreach (var r in root.GetComponentsInChildren<UnityEngine.Renderer>(true))
    {
        var mats = r.sharedMaterials;
        for (int i = 0; i < mats.Length; i++) { if (mats[i] != null && !mats[i].enableInstancing) { mats[i] = Instanced(mats[i]); swapped++; } }
        r.sharedMaterials = mats;
    }
    UnityEditor.PrefabUtility.SaveAsPrefabAsset(root, UnityEditor.AssetDatabase.GetAssetPath(prefab));
    UnityEditor.PrefabUtility.UnloadPrefabContents(root);
}
// Tree variants already exist; make bush variants so the pack prefabs stay untouched.
UnityEngine.GameObject BushVariant(string packName)
{
    string outPath = "Assets/Prefabs/Forest/Bush_" + packName + ".prefab";
    var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(outPath);
    if (existing != null) return existing;
    var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + packName + ".prefab");
    var inst = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src);
    var v = UnityEditor.PrefabUtility.SaveAsPrefabAsset(inst, outPath); UnityEngine.Object.DestroyImmediate(inst); return v;
}
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData;
var protos = data.treePrototypes;
for (int i = 0; i < protos.Length; i++)
{
    var p = protos[i].prefab;
    string path = UnityEditor.AssetDatabase.GetAssetPath(p);
    if (path.StartsWith("Assets/suffercord")) { p = BushVariant(p.name); protos[i].prefab = p; }
    Retarget(p);
}
data.treePrototypes = protos;
// Detail prototypes already use the instanced Foliage_Detail material.
terrain.treeDistance = 220f; terrain.treeBillboardDistance = 220f; terrain.detailObjectDistance = 50f;
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var sb = new System.Text.StringBuilder("saved=" + saved + " materialsSwapped=" + swapped + " instancedMats=" + copies.Count + " protos=");
foreach (var pr in data.treePrototypes) sb.Append(pr.prefab.name + " ");
return sb.ToString();
