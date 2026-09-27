var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
const string path = "Assets/Settings/PlayerTuning.asset";
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>(path);
if (tuning == null)
{
    tuning = UnityEngine.ScriptableObject.CreateInstance<PlayerTuning>();
    UnityEditor.AssetDatabase.CreateAsset(tuning, path);
    UnityEditor.AssetDatabase.SaveAssets();
}
void Wire(UnityEngine.Component c)
{
    var so = new UnityEditor.SerializedObject(c);
    so.FindProperty("tuning").objectReferenceValue = tuning;
    so.ApplyModifiedPropertiesWithoutUndo();
}
var player = UnityEngine.GameObject.Find("Player");
Wire(player.GetComponent<PlayerController>());
Wire(player.GetComponent<PlayerInteractor>());
int doors = 0;
foreach (var d in UnityEngine.Object.FindObjectsByType<Door>(UnityEngine.FindObjectsSortMode.None)) { Wire(d); doors++; }
var cc = player.GetComponent<UnityEngine.CharacterController>();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " asset=" + UnityEditor.AssetDatabase.GetAssetPath(tuning) + " doors=" + doors + " values: walk=" + tuning.walkSpeed + " sprint=" + tuning.sprintSpeed + " jump=" + tuning.jumpHeight + " step=" + tuning.stepOffset + " skin=" + tuning.skinWidth + " reach=" + tuning.interactReach + " ccStep=" + cc.stepOffset + " ccSkin=" + cc.skinWidth + " ccRadius=" + cc.radius;
