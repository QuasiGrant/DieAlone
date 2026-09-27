var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene: " + scene.path;
if (UnityEngine.GameObject.Find("TestCourse") != null) return "TestCourse already exists";

var root = new UnityEngine.GameObject("TestCourse");

UnityEngine.GameObject Group(string name, UnityEngine.Transform parent)
{
    var g = new UnityEngine.GameObject(name);
    g.transform.SetParent(parent, false);
    return g;
}

UnityEngine.GameObject Prim(UnityEngine.PrimitiveType type, string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(type);
    go.name = name;
    go.transform.SetParent(parent, false);
    go.transform.localPosition = pos;
    go.transform.localScale = size;
    if (euler.HasValue) go.transform.localRotation = UnityEngine.Quaternion.Euler(euler.Value);
    return go;
}

UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Vector3? euler = null)
    => Prim(UnityEngine.PrimitiveType.Cube, name, parent, pos, size, euler);

var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));

// ---- Sprint lane: 40 m straight strip along the west edge (x = -22), with end posts.
var lane = Group("SprintLane", root.transform);
Box("LaneFloor", lane.transform, V(-22f, 0.025f, 0f), V(3f, 0.05f, 40f));
Box("PostStartL", lane.transform, V(-23.5f, 0.5f, -20f), V(0.2f, 1f, 0.2f));
Box("PostStartR", lane.transform, V(-20.5f, 0.5f, -20f), V(0.2f, 1f, 0.2f));
Box("PostEndL",   lane.transform, V(-23.5f, 0.5f,  20f), V(0.2f, 1f, 0.2f));
Box("PostEndR",   lane.transform, V(-20.5f, 0.5f,  20f), V(0.2f, 1f, 0.2f));

// ---- Obstacle column at x = -17, running south to north.
// Low tunnel: 1.2 m clear height, 1.8 m clear width, 4 m long.
var tunnel = Group("LowTunnel", root.transform);
Box("WallW", tunnel.transform, V(-18f, 0.6f, -20f), V(0.2f, 1.2f, 4f));
Box("WallE", tunnel.transform, V(-16f, 0.6f, -20f), V(0.2f, 1.2f, 4f));
Box("Roof",  tunnel.transform, V(-17f, 1.3f, -20f), V(2.2f, 0.2f, 4f));

// Low step: 0.4 m tall. Too tall to walk up (step offset 0.3), low enough to hop.
Box("LowStep", root.transform, V(-17f, 0.2f, -14f), V(3f, 0.4f, 1f));

// Log: cylinder lying along x, 0.4 m diameter, 3 m long.
Prim(UnityEngine.PrimitiveType.Cylinder, "Log", root.transform, V(-17f, 0.2f, -10f), V(0.4f, 1.5f, 0.4f), V(0f, 0f, 90f));

// Fence: 1.5 m tall, thin, 4 m wide. Too tall for a 0.6 m hop.
Box("Fence", root.transform, V(-17f, 0.75f, -6f), V(4f, 1.5f, 0.1f));

// Ramp up to a 1.5 m platform, then stairs down the far side.
var climb = Group("RampAndStairs", root.transform);
float rise = 1.5f, run = 6f;
float slopeLen = UnityEngine.Mathf.Sqrt(rise * rise + run * run);
float angle = UnityEngine.Mathf.Atan2(rise, run) * UnityEngine.Mathf.Rad2Deg;
Box("Ramp", climb.transform, V(-17f, rise * 0.5f, 0f), V(3f, 0.2f, slopeLen), V(-angle, 0f, 0f));
Box("Platform", climb.transform, V(-17f, 0.75f, 4.5f), V(3f, 1.5f, 3f));
for (int i = 0; i < 5; i++)
{
    float top = 1.25f - 0.25f * i;
    Box("Step" + (i + 1), climb.transform, V(-17f, top * 0.5f, 6.15f + 0.3f * i), V(2f, top, 0.3f));
}

// ---- Small room with a doorway in the south wall. Interior 4 x 4 m, walls 2.5 m, no roof.
var room = Group("TestRoom", root.transform);
room.transform.localPosition = V(18f, 0f, 14f);
Box("WallN", room.transform, V(0f, 1.25f, 2.1f), V(4.4f, 2.5f, 0.2f));
Box("WallE", room.transform, V(2.1f, 1.25f, 0f), V(0.2f, 2.5f, 4f));
Box("WallW", room.transform, V(-2.1f, 1.25f, 0f), V(0.2f, 2.5f, 4f));
// South wall split around a 1.0 m wide, 2.1 m tall doorway.
Box("WallS_Left",  room.transform, V(-1.35f, 1.25f, -2.1f), V(1.7f, 2.5f, 0.2f));
Box("WallS_Right", room.transform, V( 1.35f, 1.25f, -2.1f), V(1.7f, 2.5f, 0.2f));
Box("WallS_Lintel", room.transform, V(0f, 2.3f, -2.1f), V(1.0f, 0.4f, 0.2f));

// ---- Table with loose objects.
var table = Group("Table", root.transform);
table.transform.localPosition = V(18f, 0f, 8f);
Box("Top", table.transform, V(0f, 0.75f, 0f), V(1.2f, 0.05f, 0.6f));
Box("LegA", table.transform, V(-0.55f, 0.375f, -0.25f), V(0.05f, 0.75f, 0.05f));
Box("LegB", table.transform, V( 0.55f, 0.375f, -0.25f), V(0.05f, 0.75f, 0.05f));
Box("LegC", table.transform, V(-0.55f, 0.375f,  0.25f), V(0.05f, 0.75f, 0.05f));
Box("LegD", table.transform, V( 0.55f, 0.375f,  0.25f), V(0.05f, 0.75f, 0.05f));
Box("Loose_Cube",  table.transform, V(-0.3f, 0.775f + 0.075f, 0f), V(0.15f, 0.15f, 0.15f));
Box("Loose_Brick", table.transform, V( 0.2f, 0.775f + 0.05f, 0.1f), V(0.25f, 0.1f, 0.12f));

// ---- Shelf with a loose object. Back board, two side boards, two shelves.
var shelf = Group("Shelf", root.transform);
shelf.transform.localPosition = V(21.5f, 0f, 8f);
Box("Back",  shelf.transform, V(0f, 0.75f, 0.15f), V(1.0f, 1.5f, 0.05f));
Box("SideL", shelf.transform, V(-0.5f, 0.75f, 0f), V(0.03f, 1.5f, 0.3f));
Box("SideR", shelf.transform, V( 0.5f, 0.75f, 0f), V(0.03f, 1.5f, 0.3f));
Box("ShelfLow",  shelf.transform, V(0f, 0.5f, 0f), V(1.0f, 0.03f, 0.3f));
Box("ShelfHigh", shelf.transform, V(0f, 1.0f, 0f), V(1.0f, 0.03f, 0.3f));
Box("Loose_Tin", shelf.transform, V(0.2f, 1.015f + 0.06f, 0f), V(0.12f, 0.12f, 0.12f));

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
int count = 0;
foreach (var t in root.GetComponentsInChildren<UnityEngine.Transform>()) count++;
return "saved=" + saved + " roots=" + scene.rootCount + " courseObjects=" + count + " rampAngle=" + angle.ToString("F1");
