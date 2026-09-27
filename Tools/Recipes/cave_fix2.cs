if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x;
UnityEngine.Transform cave = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "Cave") cave = r.transform;
// Mouth rocks: keep them off the passage. Anything whose collider crosses x 298.4..301.6 below y 26 gets shrunk and pushed out.
var sb = new System.Text.StringBuilder(); int fixedRocks = 0;
foreach (UnityEngine.Transform c in cave)
{
    if (!c.name.StartsWith("MouthRock")) continue;
    var col = c.GetComponent<UnityEngine.Collider>(); if (col == null) continue;
    var b = col.bounds;
    bool crosses = b.max.x > 298.4f && b.min.x < 301.6f && b.min.y < 26f && b.max.z > 256f && b.min.z < 266f;
    if (!crosses) continue;
    float side = c.position.x < 300f ? -1f : 1f;
    c.localScale = c.localScale * 0.6f;
    c.position = V(300f + side * 4.6f, terrain.SampleHeight(V(300f + side * 4.6f, 0f, 258.5f)) - 0.2f, 258.5f);
    UnityEngine.Physics.SyncTransforms();
    sb.Append(c.name + " -> " + c.position.ToString("F1") + " bounds " + col.bounds.min.ToString("F1") + ".." + col.bounds.max.ToString("F1") + "; "); fixedRocks++;
}
// Trench walls: rock at full weight where the ground is inside the cut band but not on the floor.
var hillC = new UnityEngine.Vector2(300f, 280f); var chamberC = new UnityEngine.Vector2(300f, 281f);
float CutInside(UnityEngine.Vector2 p)
{
    float dz = UnityEngine.Mathf.Clamp(p.y, 258f, 276f); float tun = 1.6f - UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(300f + UnityEngine.Mathf.Sin(p.y * 0.35f) * 0.5f, dz));
    float cham = 6f + UnityEngine.Mathf.PerlinNoise(p.x * 0.4f, p.y * 0.4f) * 1.2f - UnityEngine.Vector2.Distance(p, chamberC);
    return UnityEngine.Mathf.Max(tun, cham);
}
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
int rockIdx = -1; for (int i = 0; i < data.terrainLayers.Length; i++) if (data.terrainLayers[i].name == "Layer_Rock") rockIdx = i;
int painted = 0;
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    if (UnityEngine.Vector2.Distance(p, hillC) > 26f) continue;
    float inside = CutInside(p);
    float rock = inside > 0.7f ? 0f : (inside > -3f ? 1f : UnityEngine.Mathf.Clamp01((data.GetSteepness(x / size, z / size) - 25f) / 12f));
    if (Hill(p) > 2.5f) rock = UnityEngine.Mathf.Max(rock, 0.7f);
    for (int l = 0; l < L; l++) if (l != rockIdx) alpha[zi, xi, l] = alpha[zi, xi, l] * (1f - rock) / UnityEngine.Mathf.Max(0.0001f, 1f - alpha[zi, xi, rockIdx]);
    alpha[zi, xi, rockIdx] = rock; painted++;
    float sum = 0f; for (int l = 0; l < L; l++) sum += alpha[zi, xi, l]; if (sum > 0.0001f) for (int l = 0; l < L; l++) alpha[zi, xi, l] /= sum; else alpha[zi, xi, 0] = 1f;
}
float Hill(UnityEngine.Vector2 p) { float d = UnityEngine.Vector2.Distance(p, hillC); return 12f * UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01((22f - d) / 10f)); }
data.SetAlphamaps(0, 0, alpha);
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " fixedRocks=" + fixedRocks + " " + sb + " painted=" + painted;
