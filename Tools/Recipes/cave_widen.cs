if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x, sizeY = data.size.y;
var hillC = new UnityEngine.Vector2(300f, 280f); var chamberC = new UnityEngine.Vector2(300f, 281f);
float floorY = 24f;
// Wider cut: 7 m tunnel, 9 m chamber, so pack rocks can line the walls inside the trench.
float CutInside(UnityEngine.Vector2 p)
{
    float dz = UnityEngine.Mathf.Clamp(p.y, 256f, 276f); float tun = 3.5f - UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(300f + UnityEngine.Mathf.Sin(p.y * 0.35f) * 0.5f, dz));
    float cham = 9f + UnityEngine.Mathf.PerlinNoise(p.x * 0.4f, p.y * 0.4f) * 1.2f - UnityEngine.Vector2.Distance(p, chamberC);
    return UnityEngine.Mathf.Max(tun, cham);
}
float Hill(UnityEngine.Vector2 p) { float d = UnityEngine.Vector2.Distance(p, hillC); return 12f * UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01((22f - d) / 10f)); }
int res = data.heightmapResolution; var heights = data.GetHeights(0, 0, res, res); int cut = 0;
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * size / (res - 1), z = zi * size / (res - 1); var p = new UnityEngine.Vector2(x, z);
    if (UnityEngine.Vector2.Distance(p, hillC) > 26f) continue;
    float inside = CutInside(p);
    if (inside > -0.8f)
    {
        float h = heights[zi, xi] * sizeY; float w = UnityEngine.Mathf.Clamp01((inside + 0.8f) / 0.8f);
        heights[zi, xi] = UnityEngine.Mathf.Lerp(h, floorY + (UnityEngine.Mathf.PerlinNoise(x / 3f, z / 3f) - 0.5f) * 0.15f, w) / sizeY; cut++;
    }
}
data.SetHeights(0, 0, heights);
// Paint the whole cut and its walls rock so anything that shows between rocks is dark.
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
int rockIdx = -1; for (int i = 0; i < data.terrainLayers.Length; i++) if (data.terrainLayers[i].name == "Layer_Rock") rockIdx = i;
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    if (UnityEngine.Vector2.Distance(p, hillC) > 26f) continue;
    float inside = CutInside(p);
    float rock = inside > -3f ? 1f : (Hill(p) > 2.5f ? 0.7f : UnityEngine.Mathf.Clamp01((data.GetSteepness(x / size, z / size) - 25f) / 12f));
    if (inside > 1.2f) rock = 0.35f;   // floor: mostly dirt with a little rock
    for (int l = 0; l < L; l++) if (l != rockIdx) alpha[zi, xi, l] = l == 1 ? 1f - rock : 0f;
    alpha[zi, xi, rockIdx] = rock;
}
data.SetAlphamaps(0, 0, alpha);
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cutCells=" + cut + " wallAt(303.2,266)=" + terrain.SampleHeight(V(303.2f, 0, 266f)).ToString("F1") + " wallAt(304.2,266)=" + terrain.SampleHeight(V(304.2f, 0, 266f)).ToString("F1");
