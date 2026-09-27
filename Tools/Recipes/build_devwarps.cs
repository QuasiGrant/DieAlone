if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("DevWarps") != null) return "DevWarps already exists";
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0, z));
var root = new UnityEngine.GameObject("DevWarps");
// Ward: on the ledge just past the warning sign, looking at the stone ring.
var ward = new UnityEngine.GameObject("Ward");
ward.transform.SetParent(root.transform, false);
ward.transform.position = new UnityEngine.Vector3(80f, H(80f, 313f) + 0.2f, 313f);
var look = new UnityEngine.Vector3(66f, 0f, 323f) - new UnityEngine.Vector3(80f, 0f, 313f);
ward.transform.rotation = UnityEngine.Quaternion.LookRotation(look.normalized, UnityEngine.Vector3.up);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " ward=" + ward.transform.position + " yaw=" + ward.transform.eulerAngles.y.ToString("F0");
