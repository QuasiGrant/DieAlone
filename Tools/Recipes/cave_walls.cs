if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Transform cave = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "Cave") cave = r.transform;
if (cave == null) return "no root Cave";
var rockMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/CaveRock.mat");
var rng = new System.Random(151); int added = 0;
void Wall(UnityEngine.Vector3 pos, UnityEngine.Vector3 sz, float yaw)
{
    var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var g = pb.gameObject; g.name = "WallRock"; g.transform.SetParent(cave, false);
    var pos2 = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    for (int i = 0; i < pos2.Count; i++) { var p = pos2[i]; pos2[i] = new UnityEngine.Vector3(p.x * sz.x * (1f + ((float)rng.NextDouble() - 0.5f) * 0.2f), p.y * sz.y * (1f + ((float)rng.NextDouble() - 0.5f) * 0.15f), p.z * sz.z * (1f + ((float)rng.NextDouble() - 0.5f) * 0.2f)); }
    pb.positions = pos2; pb.ToMesh(); pb.Refresh();
    g.transform.position = pos; g.transform.rotation = UnityEngine.Quaternion.Euler(((float)rng.NextDouble() - 0.5f) * 6f, yaw + ((float)rng.NextDouble() - 0.5f) * 8f, ((float)rng.NextDouble() - 0.5f) * 6f);
    g.GetComponent<UnityEngine.Renderer>().sharedMaterial = rockMat;
    var mc = g.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = g.GetComponent<UnityEngine.MeshFilter>().sharedMesh; added++;
}
float floorY = 24f;
// Tunnel: blocks each side, inner faces about 1.6 m from the centre line, from the mouth to the chamber.
for (float z = 259f; z <= 275f; z += 2.4f)
    foreach (var side in new[] { -1f, 1f })
        Wall(V(300f + side * 2.6f + UnityEngine.Mathf.Sin(z * 0.35f) * 0.5f, floorY + 1.9f, z), V(2.0f, 4.2f, 2.9f), 0f);
// Chamber: ring of blocks with inner faces about 6.2 m out, gap left where the tunnel enters (south).
var chamberC = new UnityEngine.Vector2(300f, 281f);
for (int i = 0; i < 14; i++)
{
    float a = (i + 0.5f) / 14f * 360f;
    if (a > 240f && a < 300f) continue;   // tunnel mouth into the chamber sits at 270 degrees (south)
    float rad = a * UnityEngine.Mathf.Deg2Rad; float rr = 7.2f;
    Wall(V(chamberC.x + UnityEngine.Mathf.Cos(rad) * rr, floorY + 2.0f, chamberC.y + UnityEngine.Mathf.Sin(rad) * rr), V(2.2f, 4.6f, 3.6f), -a);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " wallRocks=" + added + " caveChildren=" + cave.childCount;
