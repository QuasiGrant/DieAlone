// 8.9h (Play mode): opens the dev panel (or menu = "pause" the pause menu, "hud" the HUD with a sample prompt) and checks it fits the current Game
// view: the panel's background and every visible text inside the screen, and every visible text at least MinReadablePx
// tall on screen. "Visible" means inside its scroll viewport; rows scrolled out of view are fine (the list scrolls).
// Run after ui_game_view_size_8_9h.cs has set the size and one frame has passed; then capture_game_view source=screen.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
string menu = "dev";
// readable floor: player menus at least 14.7 px (Style.md 7.2: 22 px at 1080 rows, so 14.7 at 720); the dev panel 11 px
float MinReadablePx = menu == "dev" ? 11f : 14.7f; string smallestName = "";
UnityEngine.Application.runInBackground = true;
var F = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
UnityEngine.Canvas target = null;
var dm = UnityEngine.Object.FindFirstObjectByType<DevMenu>(); var gp = GamePause.Instance;
bool devOpen = (bool)typeof(DevMenu).GetField("open", F).GetValue(dm);
if (menu == "dev")
{
    if (gp.IsPaused) gp.Resume();
    if (!devOpen) typeof(DevMenu).GetMethod("Open", F).Invoke(dm, null);
    foreach (var c in dm.GetComponentsInChildren<UnityEngine.Canvas>()) if (c.isRootCanvas) target = c;
}
else
{
    if (devOpen) typeof(DevMenu).GetMethod("Close", F).Invoke(dm, null);
    if (menu == "pause" && !gp.IsPaused) gp.Pause();
    if (menu == "hud" && gp.IsPaused) gp.Resume();
    string canvasName = menu == "pause" ? "PauseMenu" : "HUD";
    foreach (var c in UnityEngine.Object.FindObjectsByType<UnityEngine.Canvas>(UnityEngine.FindObjectsSortMode.None)) if (c.isRootCanvas && c.name == canvasName) target = c;
    // the HUD prompt is empty unless the player looks at something: show a long sample prompt for the check (runtime only)
    if (menu == "hud" && target != null) foreach (var t in target.GetComponentsInChildren<UnityEngine.UI.Text>(true)) if (t.name == "Prompt") { t.text = "Look inside the tent  [E]"; t.gameObject.SetActive(true); }
}
if (target == null) return "no " + menu + " canvas";
UnityEngine.Canvas.ForceUpdateCanvases();
var screen = new UnityEngine.Rect(0f, 0f, UnityEngine.Screen.width, UnityEngine.Screen.height);
var corners = new UnityEngine.Vector3[4];
UnityEngine.Rect Box(UnityEngine.RectTransform r) { r.GetWorldCorners(corners); return UnityEngine.Rect.MinMaxRect(corners[0].x, corners[0].y, corners[2].x, corners[2].y); }
bool Inside(UnityEngine.Rect r) => r.xMin >= screen.xMin - 0.5f && r.yMin >= screen.yMin - 0.5f && r.xMax <= screen.xMax + 0.5f && r.yMax <= screen.yMax + 0.5f;
var bad = new System.Collections.Generic.List<string>(); float smallest = float.MaxValue; int texts = 0;
foreach (var img in target.GetComponentsInChildren<UnityEngine.UI.Image>())
    if (img.transform.parent == null || img.transform.parent.GetComponent<UnityEngine.UI.LayoutGroup>() == null)   // panels and backgrounds, not list rows
        if (!Inside(Box(img.rectTransform))) bad.Add("off screen: " + img.name + " " + Box(img.rectTransform));
foreach (var t in target.GetComponentsInChildren<UnityEngine.UI.Text>())
{
    if (string.IsNullOrEmpty(t.text)) continue;
    var box = Box(t.rectTransform);
    var mask = t.GetComponentInParent<UnityEngine.UI.RectMask2D>();
    if (mask != null) { var m = Box(mask.rectTransform); if (!m.Overlaps(box)) continue; box = UnityEngine.Rect.MinMaxRect(UnityEngine.Mathf.Max(box.xMin, m.xMin), UnityEngine.Mathf.Max(box.yMin, m.yMin), UnityEngine.Mathf.Min(box.xMax, m.xMax), UnityEngine.Mathf.Min(box.yMax, m.yMax)); }   // scrolled out of view, or the part the viewport shows
    texts++;
    if (!Inside(box)) bad.Add("text off screen: " + t.text + " " + box);
    // on-screen size: the font size, shrunk as best fit would when the one-line label is wider than its box (never under
    // its minimum), times the canvas scale; read from the layout, since the render generator can lag a size change
    float units = t.fontSize; if (t.resizeTextForBestFit && t.preferredWidth > t.rectTransform.rect.width) units = UnityEngine.Mathf.Max(t.resizeTextMinSize, t.fontSize * t.rectTransform.rect.width / t.preferredWidth);
    float px = units * target.scaleFactor;
    if (px < smallest) { smallest = px; smallestName = t.text; }
    if (px < MinReadablePx) bad.Add("too small (" + px.ToString("F1") + " px): " + t.text);
}
return menu + " at " + screen.width + " x " + screen.height + " canvas scale " + target.scaleFactor.ToString("F2") + " | visible texts " + texts + " smallest " + smallest.ToString("F1") + " px (" + smallestName + ", floor " + MinReadablePx + ") | "
    + (bad.Count == 0 ? "fits: YES" : "fits: NO\n" + string.Join("\n", bad));
