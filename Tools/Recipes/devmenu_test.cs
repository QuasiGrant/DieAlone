UnityEngine.Application.runInBackground = true;
var sb = new System.Text.StringBuilder();
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var dev = UnityEngine.Object.FindFirstObjectByType<DevMenu>();
sb.Append("scene=" + scene.name + " devMenu=" + (dev != null));
if (dev == null) return sb.ToString();
// Press F1 through the Input System and run the dev menu's Update this frame.
var kb = UnityEngine.InputSystem.Keyboard.current;
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.F1));
UnityEngine.InputSystem.InputSystem.Update();
typeof(DevMenu).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(dev, null);
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState());
UnityEngine.InputSystem.InputSystem.Update();
var canvas = dev.transform.Find("DevMenuCanvas");
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
sb.Append(" | afterF1: panel=" + (canvas != null && canvas.gameObject.activeSelf) + " cursor=" + UnityEngine.Cursor.lockState + " playerEnabled=" + pc.enabled);
var buttons = canvas != null ? canvas.GetComponentsInChildren<UnityEngine.UI.Button>(true) : new UnityEngine.UI.Button[0];
sb.Append(" buttons=");
foreach (var b in buttons) sb.Append(b.name.Replace("Button_", "") + " ");
// Pick the other scene.
UnityEngine.UI.Button target = null;
foreach (var b in buttons) if (!b.name.EndsWith(scene.name)) target = b;
if (target != null) { target.onClick.Invoke(); sb.Append("| clicked " + target.name); }
return sb.ToString();
