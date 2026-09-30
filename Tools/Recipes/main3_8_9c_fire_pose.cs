// Main3 8.9c (Play mode), retargeted in 8.9j: stands the player at the Ward path end on the ledge (-2, 258, Valley.md 5.6), eyes
// level, facing yaw, for the fire frame. The fire is always on (8.9j). Edit yaw for the turn shots (270 west, 195 and 345 for 75
// degrees either side). The player controller stays off so the pose holds for a Game view capture; re-enable it or leave Play mode after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
float yaw = 270f;
const float standX = -2f, standZ = 258f;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var terrain = UnityEngine.Terrain.activeTerrain; float gy = terrain.SampleHeight(new UnityEngine.Vector3(standX, 0f, standZ)) + terrain.transform.position.y;
cc.enabled = false; pc.transform.SetPositionAndRotation(new UnityEngine.Vector3(standX, gy + 0.05f, standZ), UnityEngine.Quaternion.Euler(0f, yaw, 0f)); cc.enabled = true;
var cam = UnityEngine.Camera.main; cam.transform.localRotation = UnityEngine.Quaternion.identity;   // the camera is the pitch pivot: level eyes
return "player at the ledge path end facing " + yaw + ", camera at " + cam.transform.position.ToString("F1") + " far clip " + cam.farClipPlane + " fov " + cam.fieldOfView + ", fog " + UnityEngine.RenderSettings.fog;
