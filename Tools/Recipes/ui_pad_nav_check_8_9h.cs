// 8.9h follow-up (Pim) (Play mode): drives the dev panel with a virtual gamepad. Adds a Gamepad device, opens the panel
// with View (select), then presses and releases d-pad down, one press every few frames, until the "Day two" LOOK row is
// selected or MaxPresses is reached (bounded; runs from EditorApplication.update and removes itself). After each press it
// logs the selected row and whether that row sits fully inside the scroll viewport. The log goes to Temp/ui_pad_nav.txt
// (read it with a second eval once "done" is in it); the device is removed at the end and the panel left open on Day two
// for a screenshot.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
const int MaxPresses = 60, FramesPerStep = 4;
const string Goal = "Button_Day two";
UnityEngine.Application.runInBackground = true;
string logPath = System.IO.Path.GetFullPath("Temp/ui_pad_nav.txt");
System.IO.File.WriteAllText(logPath, "");
var pad = UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Gamepad>("PadCheck");
var dm = UnityEngine.Object.FindFirstObjectByType<DevMenu>();
var F = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
if ((bool)typeof(DevMenu).GetField("open", F).GetValue(dm)) typeof(DevMenu).GetMethod("Close", F).Invoke(dm, null);   // View toggles: start closed
if (GamePause.Instance != null && GamePause.Instance.IsPaused) GamePause.Instance.Resume();
int step = 0, presses = 0, lastFrame = UnityEngine.Time.frameCount; bool down = false;
var log = new System.Text.StringBuilder();
bool InView(UnityEngine.GameObject g)
{
    var vp = (UnityEngine.RectTransform)typeof(DevMenu).GetField("viewportRect", F).GetValue(dm); if (vp == null) return false;
    var a = new UnityEngine.Vector3[4]; var b = new UnityEngine.Vector3[4]; ((UnityEngine.RectTransform)g.transform).GetWorldCorners(a); vp.GetWorldCorners(b);
    return a[0].y >= b[0].y - 0.5f && a[1].y <= b[1].y + 0.5f;
}
void Set(bool select, bool dpadDown)
{
    var state = new UnityEngine.InputSystem.LowLevel.GamepadState();
    if (select) state = state.WithButton(UnityEngine.InputSystem.LowLevel.GamepadButton.Select);
    if (dpadDown) state = state.WithButton(UnityEngine.InputSystem.LowLevel.GamepadButton.DpadDown);
    UnityEngine.InputSystem.InputSystem.QueueStateEvent(pad, state);
}
UnityEditor.EditorApplication.CallbackFunction tick = null;
tick = () =>
{
    bool finish = !UnityEngine.Application.isPlaying;
    if (!finish && UnityEngine.Time.frameCount - lastFrame >= FramesPerStep)
    {
        lastFrame = UnityEngine.Time.frameCount;
        if (step == 0) Set(true, false);                // press View: the panel opens
        else if (step == 1) Set(false, false);
        else
        {
            down = !down; Set(false, down);
            if (!down)
            {
                presses++;
                var sel = UnityEngine.EventSystems.EventSystem.current != null ? UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject : null;
                string n = sel != null ? sel.name : "none";
                log.AppendLine(presses + " " + n + " in view " + (sel != null && InView(sel)));
                if (n == Goal || presses >= MaxPresses) finish = true;
            }
        }
        step++;
    }
    if (finish)
    {
        UnityEditor.EditorApplication.update -= tick;
        if (pad != null && pad.added) UnityEngine.InputSystem.InputSystem.RemoveDevice(pad);
        log.AppendLine("done after " + presses + " presses");
        System.IO.File.WriteAllText(logPath, log.ToString());
    }
};
UnityEditor.EditorApplication.update += tick;
return "started; log " + logPath;
