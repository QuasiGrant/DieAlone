var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
var player = UnityEngine.GameObject.Find("Player");
if (player.GetComponent<PlayerCarry>() != null) return "already built";

var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var cam = player.transform.Find("Main Camera");

var hold = new UnityEngine.GameObject("HoldPoint");
hold.transform.SetParent(cam, false);
hold.transform.localPosition = tuning.carryHoldOffset;

var carry = player.AddComponent<PlayerCarry>();
var so = new UnityEditor.SerializedObject(carry);
so.FindProperty("tuning").objectReferenceValue = tuning;
so.FindProperty("eye").objectReferenceValue = cam;
so.FindProperty("holdPoint").objectReferenceValue = hold.transform;
so.ApplyModifiedPropertiesWithoutUndo();

int count = 0;
foreach (var name in new[] { "TestCourse/Table/Loose_Cube", "TestCourse/Table/Loose_Brick", "TestCourse/Shelf/Loose_Tin" })
{
    var go = UnityEngine.GameObject.Find(name);
    if (go == null) return "missing " + name;
    var c = go.AddComponent<Carryable>();
    var soC = new UnityEditor.SerializedObject(c);
    soC.FindProperty("prompt").stringValue = "Pick up";
    soC.ApplyModifiedPropertiesWithoutUndo();
    count++;
}
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " carryables=" + count + " hold=" + hold.transform.localPosition.ToString("F2");
