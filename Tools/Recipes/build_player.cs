var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene: " + scene.path;

// Remove the standalone camera from 1.1; the player owns the camera now.
var oldCam = UnityEngine.GameObject.Find("Main Camera");
if (oldCam != null) UnityEngine.Object.DestroyImmediate(oldCam);

var player = new UnityEngine.GameObject("Player");
player.transform.position = new UnityEngine.Vector3(-10f, 0f, -10f);
player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 30f, 0f);

var cc = player.AddComponent<UnityEngine.CharacterController>();
cc.height = 1.8f;
cc.radius = 0.35f;
cc.center = new UnityEngine.Vector3(0f, 0.9f, 0f);
cc.stepOffset = 0.3f;
cc.slopeLimit = 45f;

var camGo = new UnityEngine.GameObject("Main Camera");
camGo.tag = "MainCamera";
camGo.transform.SetParent(player.transform, false);
camGo.transform.localPosition = new UnityEngine.Vector3(0f, 1.6f, 0f);
var cam = camGo.AddComponent<UnityEngine.Camera>();
cam.nearClipPlane = 0.05f;
camGo.AddComponent<UnityEngine.AudioListener>();

var pc = player.AddComponent<PlayerController>();
var actions = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.InputSystem.InputActionAsset>("Assets/InputSystem_Actions.inputactions");
if (actions == null) return "input actions asset not found";

var so = new UnityEditor.SerializedObject(pc);
so.FindProperty("inputActions").objectReferenceValue = actions;
so.FindProperty("cameraPivot").objectReferenceValue = camGo.transform;
so.ApplyModifiedPropertiesWithoutUndo();

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " roots=" + scene.rootCount + " actions=" + (so.FindProperty("inputActions").objectReferenceValue != null) + " pivot=" + (so.FindProperty("cameraPivot").objectReferenceValue != null);
