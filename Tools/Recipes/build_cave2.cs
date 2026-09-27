if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
UnityEngine.Transform cave = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "Cave") cave = r.transform;
if (cave == null) return "no root Cave";
// ---- Out with the boxes.
int removed = 0;
for (int i = cave.childCount - 1; i >= 0; i--) { var c = cave.GetChild(i); if (c.name == "Rock" || c.name == "HangingRock" || c.name == "WallRock" || c.name.StartsWith("MouthRock")) { UnityEngine.Object.DestroyImmediate(c.gameObject); removed++; } }
// ---- In with pack rocks: organic shapes, convex mesh colliders, dark tint.
string rockRoot = "Assets/Revolving Pizza Games/Campsite/Prefabs/Rocks and Stones/";
var rockPrefabs = new UnityEngine.GameObject[8]; for (int i = 0; i < 8; i++) rockPrefabs[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(rockRoot + "CS_Rock_" + (i + 1) + ".prefab");
var rng = new System.Random(211); int placed = 0;
var tintMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/CaveRockPack.mat");
UnityEngine.GameObject Rock(UnityEngine.Vector3 pos, float scale, string name)
{
    var p = rockPrefabs[rng.Next(0, 8)];
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(p, scene);
    go.name = name; go.transform.SetParent(cave, false); go.transform.position = pos;
    go.transform.rotation = UnityEngine.Quaternion.Euler((float)rng.NextDouble() * 360f, (float)rng.NextDouble() * 360f, (float)rng.NextDouble() * 360f);
    go.transform.localScale = V(scale * (0.85f + (float)rng.NextDouble() * 0.3f), scale * (0.85f + (float)rng.NextDouble() * 0.3f), scale * (0.85f + (float)rng.NextDouble() * 0.3f));
    foreach (var mr in go.GetComponentsInChildren<UnityEngine.MeshRenderer>())
    {
        if (tintMat == null) { tintMat = new UnityEngine.Material(mr.sharedMaterial); tintMat.SetColor("_BaseColor", new UnityEngine.Color(0.55f, 0.5f, 0.48f)); UnityEditor.AssetDatabase.CreateAsset(tintMat, "Assets/Materials/CaveRockPack.mat"); }
        mr.sharedMaterial = tintMat;
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf != null && mr.GetComponent<UnityEngine.Collider>() == null) { var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; }
    }
    placed++; return go;
}
float floorY = 24f;
// Tunnel walls: two rows each side, inner faces about 1.5 m from the centre line.
for (float z = 257.5f; z <= 276.5f; z += 1.5f)
    foreach (var side in new[] { -1f, 1f })
    {
        float wobble = UnityEngine.Mathf.Sin(z * 0.35f) * 0.5f;
        Rock(V(300f + wobble + side * 3.1f, floorY + 0.7f, z), 1.35f, "TunnelRock");
        Rock(V(300f + wobble + side * 3.0f, floorY + 2.4f, z + 0.7f), 1.3f, "TunnelRock");
    }
// Tunnel ceiling: three lanes of rocks hanging at about 3.6 m, heavy overlap so no sky shows.
for (float z = 257f; z <= 277f; z += 1.3f)
    foreach (var lane in new[] { -1.4f, 0f, 1.4f })
        Rock(V(300f + lane + UnityEngine.Mathf.Sin(z * 0.35f) * 0.5f, floorY + 4.1f + (float)rng.NextDouble() * 0.4f, z), 1.55f, "CeilingRock");
// Chamber: ring walls in two rows, gap to the south for the tunnel; ceiling rocks in rings and a cap.
var cc = new UnityEngine.Vector2(300f, 281f);
for (int i = 0; i < 24; i++)
{
    float a = i * 15f + 7.5f; if (a > 250f && a < 290f) continue;
    float rad = a * UnityEngine.Mathf.Deg2Rad;
    Rock(V(cc.x + UnityEngine.Mathf.Cos(rad) * 7.6f, floorY + 0.8f, cc.y + UnityEngine.Mathf.Sin(rad) * 7.6f), 1.6f, "ChamberRock");
    Rock(V(cc.x + UnityEngine.Mathf.Cos(rad + 0.13f) * 7.3f, floorY + 2.7f, cc.y + UnityEngine.Mathf.Sin(rad + 0.13f) * 7.3f), 1.5f, "ChamberRock");
}
foreach (var (rr, count, y, s) in new[] { (5.6f, 16, 4.0f, 1.7f), (3.4f, 10, 4.8f, 1.8f), (1.2f, 4, 5.3f, 1.9f) })
    for (int i = 0; i < count; i++) { float rad = (i + 0.5f) / count * 6.283f; Rock(V(cc.x + UnityEngine.Mathf.Cos(rad) * rr, floorY + y, cc.y + UnityEngine.Mathf.Sin(rad) * rr), s, "ChamberCeiling"); }
Rock(V(cc.x, floorY + 5.6f, cc.y), 2.0f, "ChamberCeiling");
// Mouth: a few big rocks framing the entrance, clear of x 298 to 302.
Rock(V(295.6f, H(295.6f, 257.5f) + 0.6f, 257.5f), 2.2f, "MouthRock");
Rock(V(304.6f, H(304.6f, 257.5f) + 0.6f, 257.5f), 2.0f, "MouthRock");
Rock(V(300f, floorY + 5.4f, 257.5f), 2.1f, "MouthRock");
Rock(V(297.4f, floorY + 4.6f, 256.2f), 1.5f, "MouthRock");
Rock(V(302.8f, floorY + 4.7f, 256.4f), 1.5f, "MouthRock");
UnityEngine.Physics.SyncTransforms();
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " removed=" + removed + " rocksPlaced=" + placed + " caveChildren=" + cave.childCount;
