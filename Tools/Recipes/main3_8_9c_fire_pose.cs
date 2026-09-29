// Main3 8.9c (Play mode): switches the stand-in fire on and stands the player at the Ward lip (x 14.2, z 252, 1.2 m in
// from the lip), eyes level, facing yaw. Edit yaw for the turn shots (270 west, 195 and 345 for 75 degrees either side).
// The player controller stays off so the pose holds for a Game view capture; re-enable it or leave Play mode after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
float yaw = 270f;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var fire = Root("Ward").transform.Find("StandInFire"); fire.gameObject.SetActive(true);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var terrain = UnityEngine.Terrain.activeTerrain; float gy = terrain.SampleHeight(new UnityEngine.Vector3(14.2f, 0f, 252f)) + terrain.transform.position.y;
cc.enabled = false; pc.transform.SetPositionAndRotation(new UnityEngine.Vector3(14.2f, gy + 0.05f, 252f), UnityEngine.Quaternion.Euler(0f, yaw, 0f)); cc.enabled = true;
var cam = UnityEngine.Camera.main; cam.transform.localRotation = UnityEngine.Quaternion.identity;   // the camera is the pitch pivot: level eyes
return "fire on, player at the lip facing " + yaw + ", camera at " + cam.transform.position.ToString("F1") + " far clip " + cam.farClipPlane + " fov " + cam.fieldOfView + ", fog " + UnityEngine.RenderSettings.fog;
