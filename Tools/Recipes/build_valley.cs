if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("ValleyTerrain") != null) return "valley terrain already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));

// ---- Terrain data: 240 m wide (x -240..0), 720 m long (z -160..560), low ground rising toward the ridge.
const int res = 257;
var data = new UnityEngine.TerrainData();
data.heightmapResolution = res;
data.size = V(240f, 60f, 720f);
data.alphamapResolution = 64; data.baseMapResolution = 64;
var heights = new float[res, res];
for (int zi = 0; zi < res; zi++)
    for (int xi = 0; xi < res; xi++)
    {
        float x = xi * 240f / (res - 1), z = zi * 720f / (res - 1);         // local: x 0 is the far west edge, 240 is the cliff foot
        float towardRidge = 1f - x / 240f;                                     // 0 at the cliff foot, 1 at the ridge
        float h = 2.5f + towardRidge * towardRidge * 16f
                + (UnityEngine.Mathf.PerlinNoise(x / 40f + 2.2f, z / 40f + 9.1f) - 0.5f) * 4f;
        heights[zi, xi] = UnityEngine.Mathf.Clamp01(h / 60f);
    }
data.SetHeights(0, 0, heights);
var floorLayer = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainLayer>("Assets/Terrain/Layer_ForestFloor.terrainlayer");
data.terrainLayers = new[] { floorLayer };
UnityEditor.AssetDatabase.CreateAsset(data, "Assets/Terrain/Valley_TerrainData.asset");

// ---- Trees: pines near the cliff, leafless trunks toward the fire.
string packRoot = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
UnityEngine.GameObject P(string n) { var p = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + n + ".prefab"); if (p == null) throw new System.Exception("missing " + n); return p; }
var protos = new[] { P("Pine1"), P("Pine2"), P("Pine4"), P("Aspen1Leafless"), P("Birch1Leafless"), P("Aspen3Leafless") };
var tp = new UnityEngine.TreePrototype[protos.Length];
for (int i = 0; i < tp.Length; i++) tp[i] = new UnityEngine.TreePrototype { prefab = protos[i], bendFactor = 0f };
data.treePrototypes = tp;
var rng = new System.Random(21);
var trees = new System.Collections.Generic.List<UnityEngine.TreeInstance>();
for (float gz = 6f; gz < 714f; gz += 6.5f)
    for (float gx = 30f; gx < 236f; gx += 6.5f)
    {
        float x = gx + (float)(rng.NextDouble() - 0.5) * 5f, z = gz + (float)(rng.NextDouble() - 0.5) * 5f;
        float towardRidge = 1f - x / 240f;
        bool burnt = rng.NextDouble() < UnityEngine.Mathf.Clamp01((towardRidge - 0.25f) * 1.6f);
        int proto = burnt ? 3 + rng.Next(0, 3) : rng.Next(0, 3);
        float hs = burnt ? 1.2f + (float)rng.NextDouble() * 0.5f : 1.3f + (float)rng.NextDouble() * 0.7f;
        var tint = burnt ? new UnityEngine.Color(0.25f, 0.2f, 0.18f) : UnityEngine.Color.Lerp(new UnityEngine.Color(0.55f, 0.5f, 0.45f), new UnityEngine.Color(0.85f, 0.8f, 0.75f), (float)rng.NextDouble());
        trees.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / 240f, 0f, z / 720f), prototypeIndex = proto, heightScale = hs, widthScale = 0.9f + (float)rng.NextDouble() * 0.3f, rotation = (float)(rng.NextDouble() * 6.283), color = tint, lightmapColor = UnityEngine.Color.white });
    }
data.SetTreeInstances(trees.ToArray(), true);

var go = UnityEngine.Terrain.CreateTerrainGameObject(data);
go.name = "ValleyTerrain"; go.transform.position = V(-240f, 0f, -160f);
var t = go.GetComponent<UnityEngine.Terrain>(); t.treeDistance = 600f; t.treeBillboardDistance = 600f; t.detailObjectDistance = 0f; t.drawInstanced = true;
var col = go.GetComponent<UnityEngine.TerrainCollider>(); if (col != null) UnityEngine.Object.DestroyImmediate(col);   // never walked on

// Glow patches lower and closer, in among the burnt trees.
var glowRoot = UnityEngine.GameObject.Find("Horizon/Glow").transform;
var glowMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/HorizonGlow.mat");
var hf = UnityEngine.GameObject.Find("Horizon").GetComponent<HorizonFire>();
var so = new UnityEditor.SerializedObject(hf); var arr = so.FindProperty("glowStrips"); int start = arr.arraySize;
int added = 0;
for (int i = 0; i < 16; i++)
{
    var q = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Quad);
    q.name = "FirePatch_Valley"; q.transform.SetParent(glowRoot, false);
    float wx = -170f + (float)rng.NextDouble() * 90f, wz = -120f + (float)rng.NextDouble() * 640f;
    float ground = t.SampleHeight(V(wx, 0, wz));
    q.transform.position = V(wx, ground + 4f, wz); q.transform.rotation = UnityEngine.Quaternion.Euler(0f, -90f, 0f);
    q.transform.localScale = V(10f + (float)rng.NextDouble() * 14f, 7f + (float)rng.NextDouble() * 8f, 1f);
    var r = q.GetComponent<UnityEngine.Renderer>(); r.sharedMaterial = glowMat; r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    UnityEngine.Object.DestroyImmediate(q.GetComponent<UnityEngine.Collider>());
    arr.arraySize++; arr.GetArrayElementAtIndex(arr.arraySize - 1).objectReferenceValue = r; added++;
}
so.ApplyModifiedPropertiesWithoutUndo();

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " valleyTrees=" + trees.Count + " glowPatchesAdded=" + added + " heightAtCliffFoot=" + t.SampleHeight(V(-5f, 0, 300f)).ToString("F1") + " heightNearRidge=" + t.SampleHeight(V(-200f, 0, 300f)).ToString("F1");
