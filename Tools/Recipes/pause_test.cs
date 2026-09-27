UnityEngine.Application.runInBackground = true;
var sb = new System.Text.StringBuilder();
var gp = GamePause.Instance;
var menu = UnityEngine.Object.FindFirstObjectByType<PauseMenu>();
var player = UnityEngine.GameObject.Find("Player");
var pc = player.GetComponent<PlayerController>();
var panel = UnityEngine.GameObject.Find("PauseMenu").transform.Find("Panel").gameObject;
var slider = panel.GetComponentInChildren<UnityEngine.UI.Slider>(true);
var toggle = panel.GetComponentInChildren<UnityEngine.UI.Toggle>(true);
var valueText = panel.transform.Find("Content/LookSensitivityRow/Value").GetComponent<UnityEngine.UI.Text>();
string settingsPath = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, "settings.json");

// A. Press Escape through the Input System and let the action fire.
var kb = UnityEngine.InputSystem.Keyboard.current;
if (kb == null) return "no keyboard device";
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.Escape));
UnityEngine.InputSystem.InputSystem.Update();
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState());
UnityEngine.InputSystem.InputSystem.Update();
sb.Append("A escape: paused=" + gp.IsPaused + " timeScale=" + UnityEngine.Time.timeScale + " panel=" + panel.activeSelf + " playerEnabled=" + pc.enabled + " cursor=" + UnityEngine.Cursor.lockState + " selected=" + (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null ? UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name : "none") + " | ");

// B. Change both settings through the controls.
slider.value = 2.0f;
toggle.isOn = true;
sb.Append("B set: sens=" + PlayerSettings.Current.lookSensitivity + " label=" + valueText.text + " invert=" + PlayerSettings.Current.invertLook + " fileExists=" + System.IO.File.Exists(settingsPath) + " | ");

// C. Resume via the button.
panel.transform.Find("Content/ResumeButton").GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
sb.Append("C resume: paused=" + gp.IsPaused + " timeScale=" + UnityEngine.Time.timeScale + " panel=" + panel.activeSelf + " playerEnabled=" + pc.enabled + " cursor=" + UnityEngine.Cursor.lockState + " | ");

// D. Escape again pauses, Escape once more resumes.
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.Escape));
UnityEngine.InputSystem.InputSystem.Update();
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState());
UnityEngine.InputSystem.InputSystem.Update();
bool p1 = gp.IsPaused;
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.Escape));
UnityEngine.InputSystem.InputSystem.Update();
UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState());
UnityEngine.InputSystem.InputSystem.Update();
sb.Append("D toggle: pausedAfterFirst=" + p1 + " pausedAfterSecond=" + gp.IsPaused + " | ");

// E. What is on disk.
sb.Append("E file: " + (System.IO.File.Exists(settingsPath) ? System.IO.File.ReadAllText(settingsPath).Replace("\n", " ").Replace("\r", "") : "missing") + " path=" + settingsPath);
return sb.ToString();
