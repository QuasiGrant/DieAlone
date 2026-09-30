// 8.9i (Play mode, Main3): poses the camera at S1 (LookSlice.md 6) and switches the look by clicking a LOOK row in the
// dev panel (opens it, invokes that row's Button.onClick, which is the panel's own handler and closes it). Edit look to
// the row label ("Night", "Day one", "Day two"). Returns what the scene now shows, as evidence the switch took:
// override, fog, ambient, sun, the stand-in fire and the cab lamp. Then capture_game_view source=camera to
// Docs/Look/DayNight/<look>_S1.png. The controller stays off; leave Play mode after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
string look = "Night";
UnityEngine.Application.runInBackground = true;
var F = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
var dm = UnityEngine.Object.FindFirstObjectByType<DevMenu>();
if (!(bool)typeof(DevMenu).GetField("open", F).GetValue(dm)) typeof(DevMenu).GetMethod("Open", F).Invoke(dm, null);
UnityEngine.UI.Button row = null;
foreach (var b in dm.GetComponentsInChildren<UnityEngine.UI.Button>()) if (b.name == "Button_" + look) row = b;
if (row == null) return "no LOOK row " + look;
row.onClick.Invoke();

var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false; cc.enabled = false;
var at = new UnityEngine.Vector3(156f, 16.6f, 148f); var to = new UnityEngine.Vector3(178f, 16f, 168f);   // S1, as main3_8_9f_pose.cs
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition;
var dir = to - at; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z);
pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized);
pc.transform.position = at - pc.transform.rotation * camLocal;
cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);

UnityEngine.GameObject Root(string n) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == n) return r; return null; }
var ward = Root("Ward"); var fire = ward != null ? ward.transform.Find("StandInFire") : null;
int lampsOn = 0, lamps = 0; foreach (var p in UnityEngine.Object.FindObjectsByType<PracticalLight>(UnityEngine.FindObjectsSortMode.None)) { lamps++; if (p.GetComponent<UnityEngine.Light>().enabled) lampsOn++; }
var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
var sun = UnityEngine.RenderSettings.sun;
return "clicked " + look + " -> panel look " + preview.CurrentLabel + " | override " + (LookOverride.Tuning != null ? LookOverride.Tuning.name : "none (scene look)")
    + " | fog " + UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.RenderSettings.fogColor) + " " + UnityEngine.RenderSettings.fogStartDistance.ToString("F0") + " to " + UnityEngine.RenderSettings.fogEndDistance.ToString("F0")
    + " | ambient #" + UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.RenderSettings.ambientLight) + " | sun " + (sun != null ? sun.name + " " + sun.intensity.ToString("F2") : "none")
    + " | stand-in fire " + (fire != null ? fire.gameObject.activeSelf.ToString() : "missing") + " | practical lights on " + lampsOn + "/" + lamps + " | camera " + cam.transform.position.ToString("F1");
