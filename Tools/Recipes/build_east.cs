if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("EastTerrain") != null) return "EastTerrain already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var main = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var mainData = main.terrainData; float size = mainData.size.x;
// ---- Main terrain: the road runs on from the gate to the map edge, trees cleared along it.
var tail = new UnityEngine.Vector2[] { new(388,174), new(400,174) };
float TailDist(UnityEngine.Vector2 p) { var a = tail[0]; var b = tail[1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); return UnityEngine.Vector2.Distance(p, a + ab * t); }
int ares = mainData.alphamapResolution; var alpha = mainData.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; if (x < 386f) continue;
    float trail = 1f - UnityEngine.Mathf.Clamp01((TailDist(new UnityEngine.Vector2(x, z)) - 2.0f) / 0.7f); if (trail <= 0f) continue;
    float tw = UnityEngine.Mathf.Max(alpha[zi, xi, 1], trail); alpha[zi, xi, 1] = tw; if (L > 3) alpha[zi, xi, 3] = UnityEngine.Mathf.Min(alpha[zi, xi, 3], 1f - tw); alpha[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - tw - (L > 2 ? alpha[zi, xi, 2] : 0f) - (L > 3 ? alpha[zi, xi, 3] : 0f));
}
mainData.SetAlphamaps(0, 0, alpha);
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cut = 0;
foreach (var ti in mainData.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (p.x > 386f && TailDist(p) < 4f) { cut++; continue; } keep.Add(ti); }
mainData.SetTreeInstances(keep.ToArray(), true);
int res0 = mainData.heightmapResolution; var mh = mainData.GetHeights(0, 0, res0, res0);
for (int zi = 0; zi < res0; zi++) for (int xi = 0; xi < res0; xi++) { float x = xi * size / (res0 - 1), z = zi * size / (res0 - 1); if (x < 386f) continue; float w = 1f - UnityEngine.Mathf.Clamp01((TailDist(new UnityEngine.Vector2(x, z)) - 2.5f) / 3f); if (w > 0f) mh[zi, xi] = UnityEngine.Mathf.Lerp(mh[zi, xi] * mainData.size.y, 24f, w) / mainData.size.y; }
mainData.SetHeights(0, 0, mh);
UnityEditor.EditorUtility.SetDirty(mainData);

// ---- East terrain: 240 x 400 m beyond the map edge, flat-ish forest with the road running on into the fog. Never walked, no collider.
const int res = 257;
var data = new UnityEngine.TerrainData();
data.heightmapResolution = res; data.size = V(240f, 60f, 400f); data.alphamapResolution = 128; data.baseMapResolution = 128;
var road = new UnityEngine.Vector2[] { new(0,174), new(60,172), new(120,164), new(180,150), new(240,140) };   // local, x east of the edge
float RoadDist(UnityEngine.Vector2 p) { float best = float.MaxValue; for (int i = 0; i < road.Length - 1; i++) { var a = road[i]; var b = road[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }
var heights = new float[res, res];
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * 240f / (res - 1), z = zi * 400f / (res - 1);
    float h = 24f + (UnityEngine.Mathf.PerlinNoise(x / 55f + 9.3f, z / 55f + 4.4f) - 0.5f) * 3f + UnityEngine.Mathf.Clamp01((x - 40f) / 200f) * 6f * UnityEngine.Mathf.PerlinNoise(x / 80f, z / 80f);
    float rw = 1f - UnityEngine.Mathf.Clamp01((RoadDist(new UnityEngine.Vector2(x, z)) - 2.5f) / 3f);
    h = UnityEngine.Mathf.Lerp(h, 24f, rw);
    heights[zi, xi] = UnityEngine.Mathf.Clamp01(h / 60f);
}
data.SetHeights(0, 0, heights);
var floorLayer = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainLayer>("Assets/Terrain/Layer_ForestFloor.terrainlayer");
var trailLayer = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainLayer>("Assets/Terrain/Layer_Trail.terrainlayer");
var mossLayer = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainLayer>("Assets/Terrain/Layer_Moss.terrainlayer");
data.terrainLayers = new[] { floorLayer, trailLayer, mossLayer };
int ar = data.alphamapResolution; var ea = new float[ar, ar, 3];
for (int zi = 0; zi < ar; zi++) for (int xi = 0; xi < ar; xi++)
{
    float x = (xi + 0.5f) * 240f / ar, z = (zi + 0.5f) * 400f / ar;
    float trail = 1f - UnityEngine.Mathf.Clamp01((RoadDist(new UnityEngine.Vector2(x, z)) - 2.0f) / 0.7f);
    float moss = (1f - trail) * UnityEngine.Mathf.Clamp01((RoadDist(new UnityEngine.Vector2(x, z)) - 1.3f) / 3f);
    ea[zi, xi, 1] = trail; ea[zi, xi, 2] = moss; ea[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - trail - moss);
}
data.SetAlphamaps(0, 0, ea);
UnityEditor.AssetDatabase.CreateAsset(data, "Assets/Terrain/East_TerrainData.asset");
var protoNames = new[] { "Tree_Pine1", "Tree_Pine2", "Tree_Pine3", "Tree_Pine5", "Tree_Aspen1", "Tree_Birch1" };
var protos = new UnityEngine.TreePrototype[protoNames.Length];
for (int i = 0; i < protos.Length; i++) protos[i] = new UnityEngine.TreePrototype { prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Prefabs/Forest/" + protoNames[i] + ".prefab"), bendFactor = 0f };
data.treePrototypes = protos;
var rng = new System.Random(173); var trees = new System.Collections.Generic.List<UnityEngine.TreeInstance>();
for (float gz = 4f; gz < 396f; gz += 3.4f) for (float gx = 3f; gx < 237f; gx += 3.4f)
{
    float x = gx + (float)(rng.NextDouble() - 0.5) * 2.6f, z = gz + (float)(rng.NextDouble() - 0.5) * 2.6f;
    if (RoadDist(new UnityEngine.Vector2(x, z)) < 3.2f) continue;
    bool leaf = rng.NextDouble() < 0.18; float hs = leaf ? 1.3f + (float)rng.NextDouble() * 0.5f : 1.6f + (float)rng.NextDouble() * 0.8f;
    trees.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / 240f, 0f, z / 400f), prototypeIndex = leaf ? 4 + rng.Next(0, 2) : rng.Next(0, 4), heightScale = hs, widthScale = leaf ? hs : 1f + (float)rng.NextDouble() * 0.35f, rotation = (float)(rng.NextDouble() * 6.283), color = UnityEngine.Color.Lerp(new UnityEngine.Color(0.75f, 0.75f, 0.75f), UnityEngine.Color.white, (float)rng.NextDouble()), lightmapColor = UnityEngine.Color.white });
}
data.SetTreeInstances(trees.ToArray(), true);
var go = UnityEngine.Terrain.CreateTerrainGameObject(data);
go.name = "EastTerrain"; go.transform.position = V(400f, 0f, 0f);
var t = go.GetComponent<UnityEngine.Terrain>(); t.treeDistance = 220f; t.treeBillboardDistance = 220f; t.detailObjectDistance = 0f; t.drawInstanced = true; t.allowAutoConnect = false; t.heightmapPixelError = 8f;
var col = go.GetComponent<UnityEngine.TerrainCollider>(); if (col != null) UnityEngine.Object.DestroyImmediate(col);
main.allowAutoConnect = false;
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " mainTreesCut=" + cut + " eastTrees=" + trees.Count + " edgeMain=" + main.SampleHeight(V(399.5f, 0, 174f)).ToString("F2") + " edgeEast=" + t.SampleHeight(V(400.5f, 0, 174f)).ToString("F2");
