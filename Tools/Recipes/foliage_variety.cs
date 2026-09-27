if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
string packRoot = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
var detailMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Foliage_Detail.mat");
UnityEngine.GameObject DetailVariant(string packName)
{
    string outPath = "Assets/Prefabs/Forest/Detail_" + packName + ".prefab";
    var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(outPath);
    if (existing != null) return existing;
    var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + packName + ".prefab");
    if (src == null) throw new System.Exception("missing " + packName);
    var inst = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src);
    foreach (var r in inst.GetComponentsInChildren<UnityEngine.Renderer>()) { var mats = r.sharedMaterials; for (int i = 0; i < mats.Length; i++) mats[i] = detailMat; r.sharedMaterials = mats; }
    var v = UnityEditor.PrefabUtility.SaveAsPrefabAsset(inst, outPath); UnityEngine.Object.DestroyImmediate(inst); return v;
}
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData;
// Twelve kinds: short and tall grasses, ferns, nettles, mushrooms. Size ranges differ so clumps read as different plants.
var kinds = new (string name, float minS, float maxS, UnityEngine.Color healthy, UnityEngine.Color dry, float weight)[] {
    ("Grass1", 1.0f, 1.8f, new UnityEngine.Color(0.75f, 0.8f, 0.6f), new UnityEngine.Color(0.85f, 0.7f, 0.45f), 0.16f),
    ("Grass2", 1.4f, 2.6f, new UnityEngine.Color(0.7f, 0.78f, 0.55f), new UnityEngine.Color(0.8f, 0.65f, 0.4f), 0.14f),
    ("Grass3", 0.8f, 1.5f, new UnityEngine.Color(0.8f, 0.85f, 0.65f), new UnityEngine.Color(0.9f, 0.75f, 0.5f), 0.12f),
    ("Grass4", 1.2f, 2.2f, new UnityEngine.Color(0.7f, 0.75f, 0.5f), new UnityEngine.Color(0.85f, 0.7f, 0.45f), 0.12f),
    ("Grass5", 1.0f, 2.0f, new UnityEngine.Color(0.75f, 0.8f, 0.6f), new UnityEngine.Color(0.8f, 0.6f, 0.4f), 0.10f),
    ("Fern1", 1.0f, 1.9f, new UnityEngine.Color(0.6f, 0.75f, 0.5f), new UnityEngine.Color(0.75f, 0.7f, 0.45f), 0.08f),
    ("Fern2", 0.9f, 1.6f, new UnityEngine.Color(0.6f, 0.75f, 0.5f), new UnityEngine.Color(0.7f, 0.65f, 0.4f), 0.06f),
    ("Fern3", 1.1f, 2.0f, new UnityEngine.Color(0.55f, 0.7f, 0.45f), new UnityEngine.Color(0.7f, 0.6f, 0.4f), 0.05f),
    ("Nettle1", 1.0f, 1.8f, new UnityEngine.Color(0.65f, 0.75f, 0.5f), new UnityEngine.Color(0.75f, 0.7f, 0.5f), 0.06f),
    ("Nettle2", 0.9f, 1.6f, new UnityEngine.Color(0.65f, 0.75f, 0.5f), new UnityEngine.Color(0.75f, 0.7f, 0.5f), 0.05f),
    ("Mushroom1", 0.9f, 1.6f, UnityEngine.Color.white, UnityEngine.Color.white, 0.03f),
    ("Mushroom3", 0.9f, 1.6f, UnityEngine.Color.white, UnityEngine.Color.white, 0.03f),
};
// Existing layers first so their maps stay valid, then the new ones.
var old = data.detailPrototypes; int oldCount = old.Length;
var oldMaps = new int[oldCount][,]; int dres = data.detailResolution;
for (int i = 0; i < oldCount; i++) oldMaps[i] = data.GetDetailLayer(0, 0, dres, dres, i);
var protos = new UnityEngine.DetailPrototype[kinds.Length];
for (int i = 0; i < kinds.Length; i++)
{
    var k = kinds[i];
    protos[i] = new UnityEngine.DetailPrototype { prototype = DetailVariant(k.name), usePrototypeMesh = true, renderMode = UnityEngine.DetailRenderMode.VertexLit, minHeight = k.minS, maxHeight = k.maxS, minWidth = k.minS, maxWidth = k.maxS, useInstancing = true, noiseSpread = 0.25f, healthyColor = k.healthy, dryColor = k.dry, alignToGround = 0.3f };
}
data.detailPrototypes = protos;
// Reassign every existing clump to a kind by weight, so the mix is spread and no kind repeats in blocks.
var rng = new System.Random(17);
var maps = new int[kinds.Length][,]; for (int i = 0; i < maps.Length; i++) maps[i] = new int[dres, dres];
float total = 0f; foreach (var k in kinds) total += k.weight;
long moved = 0;
for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
{
    int count = 0; for (int i = 0; i < oldCount; i++) count += oldMaps[i][zi, xi];
    if (count == 0) continue;
    float r = (float)rng.NextDouble() * total; int pick = 0; float acc = 0f;
    for (int i = 0; i < kinds.Length; i++) { acc += kinds[i].weight; if (r <= acc) { pick = i; break; } }
    maps[pick][zi, xi] = count; moved += count;
}
for (int i = 0; i < maps.Length; i++) data.SetDetailLayer(0, 0, i, maps[i]);
UnityEditor.EditorUtility.SetDirty(data);
// Tall flames a touch smaller.
var tall = UnityEngine.GameObject.Find("Camp/FirePit/FX_Flames_Tall"); if (tall != null) tall.transform.localScale = new UnityEngine.Vector3(0.45f, 0.45f, 0.45f);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " kinds=" + kinds.Length + " clumps=" + moved;
