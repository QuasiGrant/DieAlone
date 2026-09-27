if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x;
var hillC = new UnityEngine.Vector2(300f, 280f);
// Rock wherever the ground is steep around the hill, including the trench walls; floor stays bare dirt.
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares);
int rockIdx = -1, mossIdx = -1; for (int i = 0; i < data.terrainLayers.Length; i++) { if (data.terrainLayers[i].name == "Layer_Rock") rockIdx = i; if (data.terrainLayers[i].name == "Layer_Moss") mossIdx = i; }
int painted = 0;
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    if (UnityEngine.Vector2.Distance(p, hillC) > 26f) continue;
    float steep = data.GetSteepness(x / size, z / size);
    float rock = UnityEngine.Mathf.Clamp01((steep - 25f) / 12f);
    if (rock <= 0.01f) continue;
    float rest = 1f - rock; float sum = 0f;
    for (int l = 0; l < alpha.GetLength(2); l++) if (l != rockIdx) sum += alpha[zi, xi, l];
    for (int l = 0; l < alpha.GetLength(2); l++) if (l != rockIdx) alpha[zi, xi, l] = sum > 0f ? alpha[zi, xi, l] / sum * rest : 0f;
    alpha[zi, xi, rockIdx] = rock; painted++;
}
data.SetAlphamaps(0, 0, alpha);
UnityEditor.EditorUtility.SetDirty(data);
// Extra hanging blocks under the ceiling so it reads as broken rock, not slabs.
var cave = UnityEngine.GameObject.Find("Cave").transform;
var rockMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/CaveRock.mat");
var rng = new System.Random(131); int added = 0;
void Hang(UnityEngine.Vector3 pos, UnityEngine.Vector3 sz)
{
    var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var g = pb.gameObject; g.name = "HangingRock"; g.transform.SetParent(cave, false);
    var pos2 = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    for (int i = 0; i < pos2.Count; i++) { var p = pos2[i]; float taper = p.y < 0f ? 0.55f : 1f; pos2[i] = new UnityEngine.Vector3(p.x * sz.x * taper * (1f + ((float)rng.NextDouble() - 0.5f) * 0.3f), p.y * sz.y, p.z * sz.z * taper * (1f + ((float)rng.NextDouble() - 0.5f) * 0.3f)); }
    pb.positions = pos2; pb.ToMesh(); pb.Refresh();
    g.transform.position = pos; g.transform.rotation = UnityEngine.Quaternion.Euler(((float)rng.NextDouble() - 0.5f) * 20f, (float)rng.NextDouble() * 360f, ((float)rng.NextDouble() - 0.5f) * 20f);
    g.GetComponent<UnityEngine.Renderer>().sharedMaterial = rockMat;
    var mc = g.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = g.GetComponent<UnityEngine.MeshFilter>().sharedMesh; added++;
}
for (float z = 260f; z <= 276f; z += 3.1f) Hang(V(300f + ((float)rng.NextDouble() - 0.5f) * 2.4f, 24f + 3.2f + (float)rng.NextDouble() * 0.5f, z), V(2.0f, 1.6f, 2.2f));
for (int i = 0; i < 9; i++) { float a = (float)rng.NextDouble() * 6.283f, r = (float)rng.NextDouble() * 4.5f; Hang(V(300f + UnityEngine.Mathf.Cos(a) * r, 24f + 3.6f + (float)rng.NextDouble() * 0.9f, 281f + UnityEngine.Mathf.Sin(a) * r), V(2.4f, 1.8f, 2.4f)); }
// The east mouth rock sat on the approach line; push it out of the way.
int movedRocks = 0;
foreach (UnityEngine.Transform c in cave) if (c.name == "MouthRock" && c.position.x > 302f && c.position.x < 304f && c.position.z < 260f) { c.position = V(306.2f, terrain.SampleHeight(V(306.2f, 0f, 258f)) - 0.3f, 258f); movedRocks++; }
UnityEngine.Physics.SyncTransforms();
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " rockCells=" + painted + " hangingRocks=" + added + " movedRocks=" + movedRocks;
