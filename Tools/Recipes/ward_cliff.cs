if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData;
int res = data.heightmapResolution; float sizeXZ = data.size.x, sizeY = data.size.y;
float cliffX = 40f;
// ---- Valley floor west of the cliff now uses the ValleyTerrain formula, so the two meet at x 0 with no step.
var heights = data.GetHeights(0, 0, res, res);
for (int zi = 0; zi < res; zi++)
    for (int xi = 0; xi < res; xi++)
    {
        float x = xi * sizeXZ / (res - 1), z = zi * sizeXZ / (res - 1);
        if (x >= cliffX) continue;
        float xl = x + 240f, zl = z + 160f;                                  // ValleyTerrain local coordinates
        float towardRidge = UnityEngine.Mathf.Clamp01(1f - xl / 240f);
        float valley = 2.5f + towardRidge * towardRidge * 16f + (UnityEngine.Mathf.PerlinNoise(xl / 40f + 2.2f, zl / 40f + 9.1f) - 0.5f) * 4f;
        // Undo the old lerp target: rebuild from the ledge height at the cliff line using the same smoothstep.
        float ledge = heights[zi, (int)UnityEngine.Mathf.Round(cliffX / sizeXZ * (res - 1))] * sizeY;
        float t = UnityEngine.Mathf.Clamp01((cliffX - x) / 14f);
        float h = UnityEngine.Mathf.Lerp(ledge, valley, UnityEngine.Mathf.SmoothStep(0f, 1f, t));
        heights[zi, xi] = UnityEngine.Mathf.Clamp01(h / sizeY);
    }
data.SetHeights(0, 0, heights);

// ---- Rock layer painted on steep ground (the cliff face), on both terrains' layer lists.
var rockLayer = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainLayer>("Assets/Terrain/Layer_Rock.terrainlayer");
if (rockLayer == null)
{
    var tex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>("Assets/Textures/Concrete034/Concrete034_Color.jpg");
    rockLayer = new UnityEngine.TerrainLayer { diffuseTexture = tex, tileSize = new UnityEngine.Vector2(6f, 6f), diffuseRemapMax = new UnityEngine.Vector4(0.45f, 0.42f, 0.4f, 1f) };
    UnityEditor.AssetDatabase.CreateAsset(rockLayer, "Assets/Terrain/Layer_Rock.terrainlayer");
}
var layers = new System.Collections.Generic.List<UnityEngine.TerrainLayer>(data.terrainLayers);
if (!layers.Contains(rockLayer)) { layers.Add(rockLayer); data.terrainLayers = layers.ToArray(); }
int rockIdx = layers.IndexOf(rockLayer);
int ares = data.alphamapResolution;
var alpha = data.GetAlphamaps(0, 0, ares, ares);
int painted = 0;
for (int zi = 0; zi < ares; zi++)
    for (int xi = 0; xi < ares; xi++)
    {
        float x = (xi + 0.5f) * sizeXZ / ares, z = (zi + 0.5f) * sizeXZ / ares;
        float steep = data.GetSteepness(x / sizeXZ, z / sizeXZ);           // degrees
        float rock = UnityEngine.Mathf.Clamp01((steep - 30f) / 15f);
        if (rock <= 0f) { alpha[zi, xi, rockIdx] = 0f; continue; }
        float rest = 1f - rock; float sum = 0f;
        for (int l = 0; l < layers.Count; l++) if (l != rockIdx) sum += alpha[zi, xi, l];
        for (int l = 0; l < layers.Count; l++) if (l != rockIdx) alpha[zi, xi, l] = sum > 0f ? alpha[zi, xi, l] / sum * rest : 0f;
        alpha[zi, xi, rockIdx] = rock; painted++;
    }
data.SetAlphamaps(0, 0, alpha);

// ---- Ward clearing: trees and cover come in to 15 m around the stones instead of 27 m around the old ring.
var wardOld = new UnityEngine.Vector2(66f, 323f); var wardNew = new UnityEngine.Vector2(54f, 322f);
var pathA = new UnityEngine.Vector2[] { new(75,315), new(110,300), new(130,270), new(115,240), new(140,215), new(175,225), new(200,200), new(225,195), new(238,185) };
float TrailDist(UnityEngine.Vector2 p)
{
    float best = float.MaxValue;
    for (int i = 0; i < pathA.Length - 1; i++) { var a = pathA[i]; var b = pathA[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); }
    return best;
}
var rng = new System.Random(31);
var trees = new System.Collections.Generic.List<UnityEngine.TreeInstance>(data.treeInstances);
int added = 0, valleyAdded = 0;
for (float gz = 290f; gz < 356f; gz += 3.0f)
    for (float gx = 45f; gx < 96f; gx += 3.0f)
    {
        float x = gx + (float)(rng.NextDouble() - 0.5) * 2.4f, z = gz + (float)(rng.NextDouble() - 0.5) * 2.4f;
        var p = new UnityEngine.Vector2(x, z);
        if (x < 45f || UnityEngine.Vector2.Distance(p, wardOld) >= 27f || UnityEngine.Vector2.Distance(p, wardNew) < 15f || TrailDist(p) < 2.2f) continue;
        bool leaf = rng.NextDouble() < 0.2; float hs = leaf ? 1.3f + (float)rng.NextDouble() * 0.5f : 1.6f + (float)rng.NextDouble() * 0.8f;
        trees.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / sizeXZ, 0f, z / sizeXZ), prototypeIndex = leaf ? 4 + rng.Next(0, 2) : rng.Next(0, 4), heightScale = hs, widthScale = leaf ? hs : 1.0f + (float)rng.NextDouble() * 0.35f, rotation = (float)(rng.NextDouble() * 6.283), color = UnityEngine.Color.Lerp(new UnityEngine.Color(0.75f, 0.75f, 0.75f), UnityEngine.Color.white, (float)rng.NextDouble()), lightmapColor = UnityEngine.Color.white });
        added++;
    }
// ---- Below the cliff: trees on the strip between the cliff foot and the valley terrain, using the valley prototypes.
var protos = new System.Collections.Generic.List<UnityEngine.TreePrototype>(data.treePrototypes);
int firstValley = protos.Count;
foreach (var n in new[] { "Valley_Pine1", "Valley_Pine2", "Valley_Pine4", "Valley_Aspen1Leafless", "Valley_Birch1Leafless" })
    protos.Add(new UnityEngine.TreePrototype { prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Prefabs/Forest/" + n + ".prefab"), bendFactor = 0f });
data.treePrototypes = protos.ToArray();
for (float gz = 4f; gz < 396f; gz += 5.5f)
    for (float gx = 3f; gx < 30f; gx += 5.5f)
    {
        float x = gx + (float)(rng.NextDouble() - 0.5) * 4f, z = gz + (float)(rng.NextDouble() - 0.5) * 4f;
        if (x < 2f || x > 31f) continue;
        bool burnt = rng.NextDouble() < 0.3; float hs = burnt ? 1.2f + (float)rng.NextDouble() * 0.5f : 1.3f + (float)rng.NextDouble() * 0.7f;
        var tint = burnt ? new UnityEngine.Color(0.25f, 0.2f, 0.18f) : UnityEngine.Color.Lerp(new UnityEngine.Color(0.55f, 0.5f, 0.45f), new UnityEngine.Color(0.85f, 0.8f, 0.75f), (float)rng.NextDouble());
        trees.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / sizeXZ, 0f, z / sizeXZ), prototypeIndex = firstValley + (burnt ? 3 + rng.Next(0, 2) : rng.Next(0, 3)), heightScale = hs, widthScale = 0.9f + (float)rng.NextDouble() * 0.3f, rotation = (float)(rng.NextDouble() * 6.283), color = tint, lightmapColor = UnityEngine.Color.white });
        valleyAdded++;
    }
data.SetTreeInstances(trees.ToArray(), true);
// Ground cover in the ring that used to be bare.
int dres = data.detailResolution; int coverAdded = 0;
for (int layer = 0; layer < 3; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
    {
        float x = (xi + 0.5f) * sizeXZ / dres, z = (zi + 0.5f) * sizeXZ / dres; var p = new UnityEngine.Vector2(x, z);
        if (x < 44f || UnityEngine.Vector2.Distance(p, wardOld) >= 27f || UnityEngine.Vector2.Distance(p, wardNew) < 13f || TrailDist(p) < 1.0f) continue;
        if (rng.NextDouble() < 0.33) { map[zi, xi] = 1 + rng.Next(0, 2); changed = true; coverAdded++; }
    }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var valleyT = UnityEngine.GameObject.Find("ValleyTerrain").GetComponent<UnityEngine.Terrain>();
return "saved=" + saved + " rockCells=" + painted + " ringTrees=" + added + " valleyStripTrees=" + valleyAdded + " cover=" + coverAdded + " seam: main(0.5,300)=" + H(0.5f, 300f).ToString("F2") + " valley(-0.5,300)=" + (valleyT.transform.position.y + valleyT.SampleHeight(V(-0.5f, 0, 300f))).ToString("F2") + " main(0.5,120)=" + H(0.5f, 120f).ToString("F2") + " valley(-0.5,120)=" + (valleyT.transform.position.y + valleyT.SampleHeight(V(-0.5f, 0, 120f))).ToString("F2") + " ledge(40,322)=" + H(40f, 322f).ToString("F1") + " foot(26,322)=" + H(26f, 322f).ToString("F1");
