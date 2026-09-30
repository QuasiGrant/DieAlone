// 8.11 (Play mode, Main3): Pim's task test for the dev panel (Check2_UI.md section 1), by keyboard and by pad, with
// virtual devices. Each test starts with the panel closed, the look on Day one and the player at the Cabin warp, then
// counts its presses: night (F1, Left / View, d-pad Left) and the Ward (F1, Down, Enter / View, d-pad Down, A).
// Also checks the open state (TIME row selected, TIME and Ward rows inside the viewport, "more below" and the
// scrollbar showing when rows are hidden), Right and wrap on the TIME row, free wheel scrolling (the list stays
// where the wheel put it and the selection does not move), and section jumps (Page Up/Down, LB/RB).
// 8.14a: also J at night (F1, Left, Down, Down, Enter / View, d-pad Left, Down, Down, A), 5 presses or fewer.
// Runs from EditorApplication.update and removes itself. Log: Temp/dev_panel_8_11.txt ("done" on the last line).
// Leaves the panel open on Day one for the opening screenshot (capture_game_view source=screen).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
const int FramesPerStep = 4, SettleSteps = 3, MaxPressesPerTask = 5;
UnityEngine.Application.runInBackground = true;
string logPath = System.IO.Path.GetFullPath("Temp/dev_panel_8_11.txt");
System.IO.File.WriteAllText(logPath, "");
var F = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
var dm = UnityEngine.Object.FindFirstObjectByType<DevMenu>();
var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
UnityEngine.Transform warps = null;
foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == "DevWarps") warps = r.transform;
if (dm == null || preview == null || pc == null || warps == null) return "missing DevMenu, LookPreview, player or DevWarps";
var ward = warps.Find("Ward"); var cabin = warps.Find("Cabin");
int dayOne = -1, night = -1;
for (int i = 0; i < preview.Count; i++) { if (preview.Label(i) == "Day one") dayOne = i; if (preview.Label(i) == "Night") night = i; }
if (GamePause.Instance != null && GamePause.Instance.IsPaused) GamePause.Instance.Resume();

var kb = UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Keyboard>("KeyCheck811");
var pad = UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Gamepad>("PadCheck811");
var mouse = UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Mouse>("MouseCheck811");
var log = new System.Text.StringBuilder();
var steps = new System.Collections.Generic.List<System.Action>();
int presses = 0, failures = 0;

bool IsOpen() => (bool)typeof(DevMenu).GetField("open", F).GetValue(dm);
UnityEngine.GameObject Selected() => UnityEngine.EventSystems.EventSystem.current != null ? UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject : null;
UnityEngine.RectTransform Field(string n) => (UnityEngine.RectTransform)typeof(DevMenu).GetField(n, F).GetValue(dm);
UnityEngine.GameObject Row(string name) { foreach (var t in dm.GetComponentsInChildren<UnityEngine.RectTransform>(true)) if (t.name == name) return t.gameObject; return null; }
bool InView(UnityEngine.GameObject g)
{
    var vp = Field("viewportRect"); if (vp == null || g == null) return false;
    var a = new UnityEngine.Vector3[4]; var b = new UnityEngine.Vector3[4]; ((UnityEngine.RectTransform)g.transform).GetWorldCorners(a); vp.GetWorldCorners(b);
    return a[0].y >= b[0].y - 0.5f && a[1].y <= b[1].y + 0.5f && a[1].y >= 0f && a[0].y <= UnityEngine.Screen.height;
}
void Expect(string what, bool ok, string detail) { if (!ok) failures++; log.AppendLine((ok ? "PASS " : "FAIL ") + what + (detail != null ? "  (" + detail + ")" : "")); }
void Wait(int n) { for (int i = 0; i < n; i++) steps.Add(() => { }); }
void Key(UnityEngine.InputSystem.Key k) { steps.Add(() => { presses++; UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState(k)); }); steps.Add(() => UnityEngine.InputSystem.InputSystem.QueueStateEvent(kb, new UnityEngine.InputSystem.LowLevel.KeyboardState())); }
void Pad(UnityEngine.InputSystem.LowLevel.GamepadButton b) { steps.Add(() => { presses++; UnityEngine.InputSystem.InputSystem.QueueStateEvent(pad, new UnityEngine.InputSystem.LowLevel.GamepadState().WithButton(b)); }); steps.Add(() => UnityEngine.InputSystem.InputSystem.QueueStateEvent(pad, new UnityEngine.InputSystem.LowLevel.GamepadState())); }
void Reset(string title)
{
    steps.Add(() =>
    {
        if (IsOpen()) typeof(DevMenu).GetMethod("Close", F).Invoke(dm, null);
        preview.Select(dayOne);
        var cc = pc.GetComponent<UnityEngine.CharacterController>(); cc.enabled = false;
        pc.transform.position = cabin.position; cc.enabled = true;
        presses = 0; log.AppendLine("-- " + title);
    });
    Wait(SettleSteps);
}
void Check(string what, System.Func<(bool, string)> test) { steps.Add(() => { var (ok, detail) = test(); Expect(what, ok, detail); }); }
void CheckTask(string task) { steps.Add(() => Expect(task + " in " + presses + " presses (limit " + MaxPressesPerTask + ")", presses <= MaxPressesPerTask, null)); }
(bool, string) NightOn() => (preview.Current == night && IsOpen(), "look " + preview.CurrentLabel + ", panel open " + IsOpen());
(bool, string) AtWard() { float d = UnityEngine.Vector3.Distance(pc.transform.position, ward.position); return (d < 0.5f && !IsOpen(), "distance " + d.ToString("F2") + " m, panel open " + IsOpen()); }
(bool, string) OpenState()
{
    var sel = Selected(); var time = Row("Row_Time"); var wardRow = Row("Button_Ward: stones on the ledge");
    var more = Row("MoreBelow"); var bar = Row("Scrollbar");
    bool ok = IsOpen() && sel == time && InView(time) && InView(wardRow) && more != null && more.activeInHierarchy && bar != null && bar.activeInHierarchy;
    return (ok, "selected " + (sel != null ? sel.name : "none") + ", time in view " + InView(time) + ", ward in view " + InView(wardRow) + ", more below " + (more != null && more.activeInHierarchy) + ", scrollbar " + (bar != null && bar.activeInHierarchy));
}

// Keyboard: night.
Reset("keyboard: night");
Key(UnityEngine.InputSystem.Key.F1); Wait(SettleSteps);
Check("open state", OpenState);
Key(UnityEngine.InputSystem.Key.LeftArrow); Wait(1);
Check("Left sets Night at once", NightOn); CheckTask("keyboard night");
Key(UnityEngine.InputSystem.Key.LeftArrow); Wait(1);   // wraps
Check("Left from Night wraps to the last look row", () => (preview.Current == preview.Count - 1, preview.CurrentLabel));   // 8.12 added the lighting options after Day two
Key(UnityEngine.InputSystem.Key.RightArrow); Wait(1);
Check("Right steps back to Night", () => (preview.Current == night, preview.CurrentLabel));
Check("TIME row label follows", () => { var t = Row("Row_Time").GetComponentInChildren<UnityEngine.UI.Text>(); return (t.text == "Time: Night", t.text); });

// Keyboard: the Ward.
Reset("keyboard: Ward");
Key(UnityEngine.InputSystem.Key.F1); Wait(SettleSteps);
Key(UnityEngine.InputSystem.Key.DownArrow); Key(UnityEngine.InputSystem.Key.Enter); Wait(SettleSteps);
Check("warped to the Ward", AtWard); CheckTask("keyboard Ward");

// Pad: night.
Reset("pad: night");
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.Select); Wait(SettleSteps);
Check("open state", OpenState);
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.DpadLeft); Wait(1);
Check("d-pad Left sets Night at once", NightOn); CheckTask("pad night");

// Pad: the Ward.
Reset("pad: Ward");
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.Select); Wait(SettleSteps);
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.DpadDown); Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.South); Wait(SettleSteps);
Check("warped to the Ward", AtWard); CheckTask("pad Ward");

// J at night (8.14a, Wren: the WARD CLIMB rows sit under the Ward row): F1, Left, Down, Down, Enter / View, d-pad Left, Down, Down, A.
var junctionJ = warps.Find("Junction_J");
(bool, string) AtJNight() { if (junctionJ == null) return (false, "no Junction_J warp"); float d = UnityEngine.Vector3.Distance(pc.transform.position, junctionJ.position); return (d < 0.5f && !IsOpen() && preview.Current == night, "distance " + d.ToString("F2") + " m, look " + preview.CurrentLabel + ", panel open " + IsOpen()); }
Reset("keyboard: J at night");
Key(UnityEngine.InputSystem.Key.F1); Wait(SettleSteps);
Key(UnityEngine.InputSystem.Key.LeftArrow); Key(UnityEngine.InputSystem.Key.DownArrow); Key(UnityEngine.InputSystem.Key.DownArrow); Key(UnityEngine.InputSystem.Key.Enter); Wait(SettleSteps);
Check("warped to J at night", AtJNight); CheckTask("keyboard J at night");
Reset("pad: J at night");
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.Select); Wait(SettleSteps);
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.DpadLeft); Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.DpadDown); Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.DpadDown); Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.South); Wait(SettleSteps);
Check("warped to J at night", AtJNight); CheckTask("pad J at night");

// Wheel: scrolls freely, selection stays.
Reset("mouse wheel");
Key(UnityEngine.InputSystem.Key.F1); Wait(SettleSteps);
float scrolled = 0f;
steps.Add(() => UnityEngine.InputSystem.InputSystem.QueueStateEvent(mouse, new UnityEngine.InputSystem.LowLevel.MouseState { position = new UnityEngine.Vector2(UnityEngine.Screen.height * 0.2f, UnityEngine.Screen.height * 0.5f) }));
for (int i = 0; i < 6; i++)
{
    steps.Add(() => UnityEngine.InputSystem.InputSystem.QueueStateEvent(mouse, new UnityEngine.InputSystem.LowLevel.MouseState { position = new UnityEngine.Vector2(UnityEngine.Screen.height * 0.2f, UnityEngine.Screen.height * 0.5f), scroll = new UnityEngine.Vector2(0f, -120f) }));
    steps.Add(() => UnityEngine.InputSystem.InputSystem.QueueStateEvent(mouse, new UnityEngine.InputSystem.LowLevel.MouseState { position = new UnityEngine.Vector2(UnityEngine.Screen.height * 0.2f, UnityEngine.Screen.height * 0.5f) }));
}
steps.Add(() => scrolled = Field("listRect").anchoredPosition.y);
Wait(SettleSteps * 3);
Check("wheel scrolled and stayed", () => { float y = Field("listRect").anchoredPosition.y; return (scrolled > 0f && UnityEngine.Mathf.Abs(y - scrolled) < 0.5f, "after wheel " + scrolled.ToString("F1") + ", later " + y.ToString("F1")); });
Check("selection did not move", () => (Selected() == Row("Row_Time"), Selected() != null ? Selected().name : "none"));

// Section jumps.
Reset("section jumps");
Key(UnityEngine.InputSystem.Key.F1); Wait(SettleSteps);
Key(UnityEngine.InputSystem.Key.PageDown); Wait(1);
Check("Page Down to WARPS", () => (Selected() != null && Selected().name == "Button_Ward: stones on the ledge", Selected() != null ? Selected().name : "none"));
Key(UnityEngine.InputSystem.Key.PageDown); Wait(1);
Check("Page Down to SCENES, row in view", () => (Selected() != null && Selected().name.StartsWith("Button_Graybox") && InView(Selected()), Selected() != null ? Selected().name : "none"));
Key(UnityEngine.InputSystem.Key.PageUp); Wait(1);
Check("Page Up to WARPS", () => (Selected() != null && Selected().name == "Button_Ward: stones on the ledge" && InView(Selected()), Selected() != null ? Selected().name : "none"));
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.LeftShoulder); Wait(1);
Check("LB to TIME", () => (Selected() == Row("Row_Time") && InView(Selected()), Selected() != null ? Selected().name : "none"));
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.LeftShoulder); Wait(1);
Check("LB wraps to SCENES", () => (Selected() != null && Selected().name.StartsWith("Button_Graybox"), Selected() != null ? Selected().name : "none"));
Pad(UnityEngine.InputSystem.LowLevel.GamepadButton.RightShoulder); Wait(1);
Check("RB wraps to TIME", () => (Selected() == Row("Row_Time"), Selected() != null ? Selected().name : "none"));

// Leave the panel open on Day one for the screenshot.
Reset("screenshot");
Key(UnityEngine.InputSystem.Key.F1); Wait(SettleSteps);
Check("open state for the screenshot", OpenState);

int index = 0, lastFrame = UnityEngine.Time.frameCount;
UnityEditor.EditorApplication.CallbackFunction tick = null;
tick = () =>
{
    bool finish = !UnityEngine.Application.isPlaying || index >= steps.Count;
    if (!finish && UnityEngine.Time.frameCount - lastFrame >= FramesPerStep)
    {
        lastFrame = UnityEngine.Time.frameCount;
        try { steps[index](); } catch (System.Exception e) { failures++; log.AppendLine("FAIL step " + index + ": " + e.Message); }
        index++;
    }
    if (finish)
    {
        UnityEditor.EditorApplication.update -= tick;
        foreach (var d in new UnityEngine.InputSystem.InputDevice[] { kb, pad, mouse }) if (d != null && d.added) UnityEngine.InputSystem.InputSystem.RemoveDevice(d);
        log.AppendLine("done: " + failures + " failures, screen " + UnityEngine.Screen.width + " x " + UnityEngine.Screen.height);
        System.IO.File.WriteAllText(logPath, log.ToString());
    }
};
UnityEditor.EditorApplication.update += tick;
return "started " + steps.Count + " steps; log " + logPath;
