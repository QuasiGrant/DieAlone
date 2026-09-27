var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
if (UnityEngine.GameObject.Find("TestCourse/TestRoom/Door") != null) return "doors already exist";

var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));

// Hinge at the west jamb of a 1.0 x 2.1 doorway; panel hangs east of it, closed in the wall plane.
UnityEngine.GameObject MakeDoor(UnityEngine.Transform parent, UnityEngine.Vector3 hingeLocalPos)
{
    var hinge = new UnityEngine.GameObject("Door");
    hinge.transform.SetParent(parent, false);
    hinge.transform.localPosition = hingeLocalPos;
    var rb = hinge.AddComponent<UnityEngine.Rigidbody>();
    rb.isKinematic = true;
    rb.useGravity = false;

    var panel = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    panel.name = "Panel";
    panel.transform.SetParent(hinge.transform, false);
    panel.transform.localPosition = V(0.49f, 1.05f, 0f);
    panel.transform.localScale = V(0.96f, 2.08f, 0.08f);

    var door = hinge.AddComponent<Door>();
    var so = new UnityEditor.SerializedObject(door);
    so.FindProperty("prompt").stringValue = "Open";
    so.FindProperty("panel").objectReferenceValue = panel.GetComponent<UnityEngine.BoxCollider>();
    so.ApplyModifiedPropertiesWithoutUndo();
    return hinge;
}

// Test room: doorway centered at local x 0 in the south wall (local z -2.1).
var testRoom = UnityEngine.GameObject.Find("TestCourse/TestRoom").transform;
var d1 = MakeDoor(testRoom, V(-0.5f, 0f, -2.1f));

// Tower room: doorway centered at local x -1.1 in the south wall (local z -2.1).
var topRoom = UnityEngine.GameObject.Find("Tower/TopRoom").transform;
var d2 = MakeDoor(topRoom, V(-1.6f, 0f, -2.1f));

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " testRoomDoor=" + d1.transform.position.ToString("F2") + " towerDoor=" + d2.transform.position.ToString("F2");
