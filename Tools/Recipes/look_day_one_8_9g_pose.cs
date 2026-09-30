// 8.9g (Play mode, Main3): poses the player camera on one of the three day-one brightness shots, in the look Play started
// in (day one since 8.9g; nothing is selected here, so the shot also proves the start look). Then capture_game_view with
// source=camera at the Game view's own size (Screen.width x Screen.height, returned below) to Docs/Look/DayOneFix/<shot>.png.
// Edit shot: 1 under the giants (8.9j: inside the grove east of the Camp 2 to T leg, looking south-east),
// 2 in the cabin (the player spawn, facing the door), 3 S1 (LookSlice.md 6, same pose as main3_8_9f_pose.cs).
// The controller stays off; leave Play mode after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
int shot = 1;
UnityEngine.Application.runInBackground = true;
var terrain = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
const float eye = 1.6f;
// (name, camera position, look-at); NaN y means ground plus eye
var shots = new (string name, UnityEngine.Vector3 at, UnityEngine.Vector3 look)[] {
    ("Giants", new UnityEngine.Vector3(318f, float.NaN, 112f), new UnityEngine.Vector3(334f, 16f, 100f)),   // 8.9j: the groves were re-placed with the valley; inside the grove east of the Camp 2 to T leg, past its sight-break boulder
    ("Cabin", new UnityEngine.Vector3(176.1f, 17.24f, 169.8f), new UnityEngine.Vector3(176.1f, 16.9f, 164.8f)),
    ("S1", new UnityEngine.Vector3(156f, 16.6f, 148f), new UnityEngine.Vector3(178f, 16f, 168f)) };
var s = shots[shot - 1]; var at = s.at; if (float.IsNaN(at.y)) at.y = H(at.x, at.z) + eye;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false; cc.enabled = false;
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition;
var dir = s.look - at; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z);
pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized);
pc.transform.position = at - pc.transform.rotation * camLocal;
cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
return s.name + " look " + (preview != null ? preview.CurrentLabel : "none") + " (" + (LookOverride.Tuning != null ? LookOverride.Tuning.name : "scene") + ") camera " + cam.transform.position.ToString("F1")
    + " toward " + s.look + " | capture at " + UnityEngine.Screen.width + " x " + UnityEngine.Screen.height;
