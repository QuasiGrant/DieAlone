using UnityEngine;

/// The one dev panel. F1 on the keyboard or View (Select) on a gamepad opens and closes it;
/// B on a gamepad also closes it. Three labelled sections, top to bottom (Pim, Check2_UI.md section 1):
/// TIME one row, selected on open; Left/Right steps the looks on LookPreview (Night, Day one, Day two) at once;
/// WARPS the open scene's warp points (children of "DevWarps"): the Ward first, then route-order groups
///   with plain names from DevWarpLabels;
/// SCENES every scene in the build list (picking one loads it).
/// The mouse wheel scrolls the list freely; the selection moves only on key, pad or click, and the list follows it.
/// A scrollbar and a "more below" mark show when rows are off screen. LB/RB and Page Up/Down jump between sections.
/// Text scales with the screen height from a 720 reference, never below 1 reference pixel per screen pixel.
/// Exists only in the Editor and development builds. In a release build this class
/// compiles to an empty component: no panel, no keys, nothing to find.
/// The panel is built from code each time it opens, so the prefab carries no UI for it.
public class DevMenu : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private const string WarpRootName = "DevWarps";

    // Layout, in reference pixels at 1280 x 720.
    private const float ColumnWidth = 360f;
    private const float EdgeWidth = 3f;
    private const float Margin = 18f;
    private const float RowSpacing = 6f;
    private const float SectionGap = 12f;
    private const float GroupGap = 4f;
    private const float WarpIndent = 22f;
    private const float ScrollbarWidth = 6f;
    private const float StepArrowWidth = 34f;
    private const int TitleSize = 26;
    private const int HintSize = 13;
    private const int HeaderSize = 13;
    private const int GroupSize = 11;
    private const float GroupHeight = 15f;
    private const int RowSize = 17;
    private const int SubRowSize = 15;
    private const int MinTextSize = 10;           // best-fit floor when a label is wider than a narrow column
    private const float ReferenceHeight = 720f;   // the canvas scales with screen height
    private const float MinScale = 1f;            // never smaller than 1 reference pixel per screen pixel, so short wide windows stay readable (8.9h)
    private const float ScrollSensitivity = 30f;
    private const float MoreBelowSlack = 1f;      // reference pixels of list hidden below the viewport before the mark shows
    private const int SortingOrder = 100;

    private GameObject panel;
    private Behaviour[] gameplay;
    private bool open;

    /// True while the panel is on screen, so other dev overlays (the LookPreview corner label) can stay out of its way.
    public static bool IsShowing { get; private set; }

    // Layout that follows the screen: the panel sits inside the safe area, the column never gets wider than it,
    // and the list scrolls (mouse wheel, or following the keyboard and gamepad selection) when it is taller.
    private UnityEngine.UI.CanvasScaler scaler;
    private UnityEngine.UI.ScrollRect scroll;
    private RectTransform safeRect;
    private RectTransform columnRect;
    private RectTransform viewportRect;
    private RectTransform listRect;
    private GameObject moreBelow;
    private Vector2Int fittedScreen;
    private Rect fittedSafeArea;
    private GameObject followed;   // the selection the list last scrolled to

    // Section heads in order, with the row each jump selects.
    private struct Section { public RectTransform Head; public UnityEngine.UI.Selectable First; }
    private readonly System.Collections.Generic.List<Section> sections = new System.Collections.Generic.List<Section>();

    // Palette: near-black panel, bone text, one warm accent for what is current.
    private static readonly Color PanelColor = new Color(0.06f, 0.06f, 0.07f, 0.94f);
    private static readonly Color EdgeColor = new Color(0.85f, 0.55f, 0.30f, 1f);
    private static readonly Color TextColor = new Color(0.92f, 0.88f, 0.80f, 1f);
    private static readonly Color DimColor = new Color(0.60f, 0.57f, 0.52f, 1f);
    private static readonly Color ButtonColor = new Color(0.16f, 0.16f, 0.18f, 1f);
    private static readonly Color ButtonHover = new Color(0.30f, 0.27f, 0.24f, 1f);
    private static readonly Color AccentColor = new Color(0.55f, 0.32f, 0.16f, 1f);
    private static readonly Color AccentHover = new Color(0.70f, 0.42f, 0.22f, 1f);
    private static readonly Color SubColor = new Color(0.11f, 0.11f, 0.12f, 1f);
    private static readonly Color RuleColor = new Color(0.85f, 0.55f, 0.30f, 0.35f);
    private static readonly Color TrackColor = new Color(0.85f, 0.55f, 0.30f, 0.15f);

    private void Update()
    {
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        var gamepad = UnityEngine.InputSystem.Gamepad.current;
        bool toggle = (keyboard != null && keyboard.f1Key.wasPressedThisFrame)
                   || (gamepad != null && gamepad.selectButton.wasPressedThisFrame);
        bool back = open && gamepad != null && gamepad.buttonEast.wasPressedThisFrame;
        if (back) { Close(); return; }
        if (open)
        {
            FitToScreen();
            bool next = (keyboard != null && keyboard.pageDownKey.wasPressedThisFrame) || (gamepad != null && gamepad.rightShoulder.wasPressedThisFrame);
            bool previous = (keyboard != null && keyboard.pageUpKey.wasPressedThisFrame) || (gamepad != null && gamepad.leftShoulder.wasPressedThisFrame);
            if (next != previous) JumpSection(next ? 1 : -1);
            FollowSelection();
            ShowMoreBelow();
        }
        if (!toggle) return;
        if (!open && GamePause.Instance != null && GamePause.Instance.IsPaused) return;   // not over the pause menu
        if (open) Close(); else Open();
    }

    private void Open()
    {
        Rebuild();
        open = true;
        IsShowing = true;
        gameplay = FindGameplay();
        foreach (var b in gameplay) if (b != null) b.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Close()
    {
        if (panel != null) Destroy(panel);
        panel = null;
        open = false;
        IsShowing = false;
        sections.Clear();
        followed = null;
        var events = UnityEngine.EventSystems.EventSystem.current;
        if (events != null) events.SetSelectedGameObject(null);
        if (gameplay != null) foreach (var b in gameplay) if (b != null) b.enabled = true;
        gameplay = null;
    }

    private void OnDisable()
    {
        if (open) Close();
    }

    private static Behaviour[] FindGameplay()
    {
        var list = new System.Collections.Generic.List<Behaviour>();
        var pc = FindFirstObjectByType<PlayerController>();
        var pi = FindFirstObjectByType<PlayerInteractor>();
        if (pc != null) list.Add(pc);
        if (pi != null) list.Add(pi);
        return list.ToArray();
    }

    private void LoadScene(string path)
    {
        Close();
        UnityEngine.SceneManagement.SceneManager.LoadScene(path);
    }

    private void Warp(Transform target)
    {
        var pc = FindFirstObjectByType<PlayerController>();
        if (pc == null) return;
        var cc = pc.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        pc.transform.position = target.position;
        pc.transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
        if (cc != null) cc.enabled = true;
        Close();
    }

    /// Steps the look by one (wrapping) and applies it at once; the panel stays open.
    private static void StepTime(LookPreview preview, UnityEngine.UI.Text label, int direction)
    {
        int count = preview.Count;
        preview.Select(((preview.Current + direction) % count + count) % count);
        label.text = TimeLabel(preview);
    }

    private static string TimeLabel(LookPreview preview) => "Time: " + preview.CurrentLabel;

    /// Friendly names for the build-list scenes.
    private static string SceneLabel(string sceneName)
    {
        switch (sceneName)
        {
            case "Main": return "Main 1.0  (saved version)";
            case "Main2": return "Main 2.0  (archived)";
            case "Graybox": return "Graybox  (test course)";
            default: return Readable(sceneName);
        }
    }

    /// "Campsite_1_Tents" or "CaveMouth" becomes "Campsite 1 Tents" or "Cave Mouth".
    private static string Readable(string name)
    {
        var sb = new System.Text.StringBuilder(name.Length + 8);
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            if (c == '_' || c == '-') { sb.Append(' '); continue; }
            bool wordStart = i > 0 && char.IsUpper(c) && char.IsLower(name[i - 1]);
            bool numberStart = i > 0 && char.IsDigit(c) && char.IsLetter(name[i - 1]);
            if (wordStart || numberStart) sb.Append(' ');
            sb.Append(c);
        }
        return System.Text.RegularExpressions.Regex.Replace(sb.ToString(), " {2,}", " ").Trim();
    }

    private static Transform FindWarpRoot()
    {
        foreach (var root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            if (root.name == WarpRootName) return root.transform;
        return null;
    }

    // ---- UI built from code: a dark left column with a title and three labelled sections.

    private void Rebuild()
    {
        if (panel != null) Destroy(panel);
        sections.Clear();
        followed = null;
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        panel = new GameObject("DevMenuCanvas");
        panel.transform.SetParent(transform, false);
        var canvas = panel.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = SortingOrder;
        scaler = panel.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPixelSize;   // FitToScreen sets the factor
        panel.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Safe area: everything sits inside Screen.safeArea (FitToScreen sets the anchors).
        var safe = new GameObject("SafeArea", typeof(RectTransform));
        safe.transform.SetParent(panel.transform, false);
        safeRect = safe.GetComponent<RectTransform>();
        safeRect.offsetMin = Vector2.zero; safeRect.offsetMax = Vector2.zero;

        // Column background down the left of the safe area, with a thin accent edge on its right side.
        var bg = Rect("Background", safe.transform, PanelColor);
        columnRect = bg.GetComponent<RectTransform>();
        columnRect.anchorMin = new Vector2(0f, 0f); columnRect.anchorMax = new Vector2(0f, 1f);
        columnRect.pivot = new Vector2(0f, 0.5f);
        columnRect.offsetMin = Vector2.zero; columnRect.offsetMax = Vector2.zero;
        columnRect.sizeDelta = new Vector2(ColumnWidth, 0f);
        var edge = Rect("Edge", bg.transform, EdgeColor).GetComponent<RectTransform>();
        edge.anchorMin = new Vector2(1f, 0f); edge.anchorMax = new Vector2(1f, 1f); edge.pivot = new Vector2(1f, 0.5f);
        edge.offsetMin = Vector2.zero; edge.offsetMax = Vector2.zero; edge.sizeDelta = new Vector2(EdgeWidth, 0f);

        // Scrolling list: a clipped viewport inside the column margins, the list growing downward from its top.
        var viewport = new GameObject("Viewport", typeof(RectTransform));
        viewport.transform.SetParent(bg.transform, false);
        viewportRect = viewport.GetComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero; viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(Margin, Margin); viewportRect.offsetMax = new Vector2(-Margin - EdgeWidth, -Margin);
        viewport.AddComponent<UnityEngine.UI.RectMask2D>();

        var list = new GameObject("List", typeof(RectTransform));
        list.transform.SetParent(viewport.transform, false);
        listRect = list.GetComponent<RectTransform>();
        listRect.anchorMin = new Vector2(0f, 1f); listRect.anchorMax = new Vector2(1f, 1f); listRect.pivot = new Vector2(0.5f, 1f);
        listRect.offsetMin = Vector2.zero; listRect.offsetMax = Vector2.zero;
        var layout = list.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
        layout.spacing = RowSpacing;
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.childControlWidth = true; layout.childControlHeight = true;
        layout.childForceExpandWidth = true; layout.childForceExpandHeight = false;
        list.AddComponent<UnityEngine.UI.ContentSizeFitter>().verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;

        scroll = bg.AddComponent<UnityEngine.UI.ScrollRect>();
        scroll.viewport = viewportRect; scroll.content = listRect;
        scroll.horizontal = false; scroll.vertical = true;
        scroll.movementType = UnityEngine.UI.ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = ScrollSensitivity;
        scroll.verticalScrollbar = AddScrollbar(bg.transform);
        scroll.verticalScrollbarVisibility = UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHide;
        moreBelow = AddMoreBelow(bg.transform, font);

        fittedScreen = Vector2Int.zero;   // force the first fit
        FitToScreen();

        AddText(list.transform, font, "DEV MENU", TitleSize, FontStyle.Bold, TextColor, 36f);
        AddText(list.transform, font, "Left/Right: time.  PgUp/PgDn or LB/RB: sections.  F1 or B: close.", HintSize, FontStyle.Normal, DimColor, 18f);

        // TIME: one row that steps the look in place.
        var timeHead = AddSection(list.transform, font, "TIME");
        var preview = FindFirstObjectByType<LookPreview>();
        UnityEngine.UI.Selectable timeRow = null;
        if (preview == null || preview.Count == 0)
            AddText(list.transform, font, "No look preview in this scene.", SubRowSize, FontStyle.Italic, DimColor, 22f);
        else
            timeRow = AddStepRow(list.transform, font, preview);
        sections.Add(new Section { Head = timeHead, First = timeRow });

        // WARPS: the Ward first, then the listed warps group by group in route order, then any others.
        var warpHead = AddSection(list.transform, font, "WARPS");
        UnityEngine.UI.Selectable firstWarp = null;
        void KeepWarp(UnityEngine.UI.Selectable s) { if (firstWarp == null) firstWarp = s; }
        var warps = FindWarpRoot();
        if (warps == null || warps.childCount == 0)
            AddText(list.transform, font, "No warp points in this scene.", SubRowSize, FontStyle.Italic, DimColor, 22f);
        else
        {
            var ward = warps.Find(DevWarpLabels.FirstWarp);
            if (ward != null) KeepWarp(AddWarp(list.transform, font, DevWarpLabels.FirstLabel, ward, SubColor, ButtonHover));
            string group = null;
            bool anyGroup = false;
            foreach (var row in DevWarpLabels.Rows)
            {
                var target = warps.Find(row.Name);
                if (target == null) continue;
                if (row.Group != group) { group = row.Group; anyGroup = true; AddGroup(list.transform, font, group); }
                KeepWarp(AddWarp(list.transform, font, row.Label, target, SubColor, ButtonHover));
            }
            bool otherHead = false;
            foreach (Transform w in warps)
            {
                if (DevWarpLabels.IsListed(w.name)) continue;
                if (anyGroup && !otherHead) { otherHead = true; AddGroup(list.transform, font, DevWarpLabels.OtherGroup); }
                KeepWarp(AddWarp(list.transform, font, Readable(w.name), w, SubColor, ButtonHover));
            }
        }
        sections.Add(new Section { Head = warpHead, First = firstWarp });

        // SCENES: every build-list scene, last because it is used least.
        var sceneHead = AddSection(list.transform, font, "SCENES");
        UnityEngine.UI.Selectable firstScene = null;
        int count = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        for (int i = 0; i < count; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            bool current = path == active;
            string label = SceneLabel(System.IO.Path.GetFileNameWithoutExtension(path)) + (current ? "   (reload)" : "");
            var b = AddButton(list.transform, font, label, RowSize, 0f, current ? AccentColor : ButtonColor, current ? AccentHover : ButtonHover, () => LoadScene(path));
            if (firstScene == null) firstScene = b;
        }
        sections.Add(new Section { Head = sceneHead, First = firstScene });

        // Keyboard and gamepad start on the TIME row (or the first row there is).
        UnityEngine.UI.Selectable first = null;
        foreach (var s in sections) if (first == null && s.First != null) first = s.First;
        var events = UnityEngine.EventSystems.EventSystem.current;
        if (events != null && first != null) events.SetSelectedGameObject(first.gameObject);
        followed = first != null ? first.gameObject : null;
    }

    /// Canvas scale: follows the screen height from the 720 reference, but never below MinScale.
    private static float CanvasScale(int screenHeight) => Mathf.Max(screenHeight / ReferenceHeight, MinScale);

    /// Keeps the panel inside the safe area and the column no wider than it, whenever the Game view or window changes size.
    private void FitToScreen()
    {
        if (safeRect == null) return;
        var screen = new Vector2Int(Screen.width, Screen.height);
        var area = Screen.safeArea;
        if (screen == fittedScreen && area == fittedSafeArea) return;
        fittedScreen = screen; fittedSafeArea = area;
        if (screen.x <= 0 || screen.y <= 0) return;
        safeRect.anchorMin = new Vector2(area.xMin / screen.x, area.yMin / screen.y);
        safeRect.anchorMax = new Vector2(area.xMax / screen.x, area.yMax / screen.y);
        float scale = CanvasScale(screen.y);
        scaler.scaleFactor = scale;
        float safeWidth = area.width / scale;
        columnRect.sizeDelta = new Vector2(Mathf.Min(ColumnWidth, safeWidth), 0f);
    }

    /// When the keyboard, gamepad or a click moves the selection, scrolls the list so the new row is inside the viewport.
    /// The wheel never changes the selection, so it scrolls freely.
    private void FollowSelection()
    {
        var events = UnityEngine.EventSystems.EventSystem.current;
        if (events == null || listRect == null) return;
        var selected = events.currentSelectedGameObject;
        if (selected == followed) return;
        followed = selected;
        if (selected == null || !selected.transform.IsChildOf(listRect)) return;
        var item = (RectTransform)selected.transform;
        var itemCorners = new Vector3[4]; var viewCorners = new Vector3[4];
        item.GetWorldCorners(itemCorners); viewportRect.GetWorldCorners(viewCorners);
        float above = itemCorners[1].y - viewCorners[1].y;   // item top over the viewport top
        float below = viewCorners[0].y - itemCorners[0].y;   // item bottom under the viewport bottom
        ScrollBy(above > 0f ? -above : below > 0f ? below : 0f);
    }

    /// LB/RB or Page Up/Down: selects the first row of the next or previous section (wrapping) and scrolls its head to the top.
    private void JumpSection(int direction)
    {
        if (sections.Count == 0) return;
        var events = UnityEngine.EventSystems.EventSystem.current;
        var selected = events != null ? events.currentSelectedGameObject : null;
        int current = -1;
        for (int i = 0; i < sections.Count; i++)
            if (selected != null && sections[i].Head.GetSiblingIndex() < selected.transform.GetSiblingIndex()) current = i;
        if (current < 0) current = direction > 0 ? -1 : 0;
        for (int step = 1; step <= sections.Count; step++)
        {
            var target = sections[((current + direction * step) % sections.Count + sections.Count) % sections.Count];
            if (target.First == null) continue;
            if (events != null) events.SetSelectedGameObject(target.First.gameObject);
            followed = target.First.gameObject;
            var headCorners = new Vector3[4]; var viewCorners = new Vector3[4];
            target.Head.GetWorldCorners(headCorners); viewportRect.GetWorldCorners(viewCorners);
            ScrollBy(viewCorners[1].y - headCorners[1].y);
            return;
        }
    }

    /// Moves the list up by a world-space distance (negative moves it down), kept inside the scroll range.
    private void ScrollBy(float worldDelta)
    {
        if (worldDelta == 0f) return;
        scroll.StopMovement();
        float scale = listRect.lossyScale.y > 0f ? listRect.lossyScale.y : 1f;
        float range = Mathf.Max(0f, listRect.rect.height - viewportRect.rect.height);
        float y = Mathf.Clamp(listRect.anchoredPosition.y + worldDelta / scale, 0f, range);
        listRect.anchoredPosition = new Vector2(listRect.anchoredPosition.x, y);
    }

    /// Shows the "more below" mark while any of the list is hidden under the viewport.
    private void ShowMoreBelow()
    {
        if (moreBelow == null) return;
        float hidden = listRect.rect.height - viewportRect.rect.height - listRect.anchoredPosition.y;
        bool show = hidden > MoreBelowSlack;
        if (moreBelow.activeSelf != show) moreBelow.SetActive(show);
    }

    private static RectTransform AddSection(Transform parent, Font font, string title)
    {
        AddGap(parent, SectionGap);
        var head = AddText(parent, font, title, HeaderSize, FontStyle.Bold, EdgeColor, 18f);
        var rule = Rect("Rule", parent, RuleColor);
        rule.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 1f;
        return head;
    }

    /// A small subhead inside WARPS (route-order group).
    private static void AddGroup(Transform parent, Font font, string title)
    {
        AddGap(parent, GroupGap);
        var go = new GameObject("Group_" + title, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = GroupHeight;
        var t = Label(go.transform, font, title, GroupSize, TextAnchor.MiddleLeft);
        t.fontStyle = FontStyle.Bold; t.color = DimColor;
        var r = t.rectTransform;
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = new Vector2(WarpIndent, 0f); r.offsetMax = Vector2.zero;
    }

    private UnityEngine.UI.Button AddWarp(Transform parent, Font font, string label, Transform target, Color normal, Color hover)
    {
        return AddButton(parent, font, label, SubRowSize, WarpIndent, normal, hover, () => Warp(target));
    }

    /// The TIME row: "Time: Night" on the left, clickable < and > on the right.
    private static DevStepRow AddStepRow(Transform parent, Font font, LookPreview preview)
    {
        var go = new GameObject("Row_Time", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = AccentColor;
        var row = go.AddComponent<DevStepRow>();
        row.targetGraphic = image;
        row.colors = Tints(AccentColor, AccentHover);
        var nav = row.navigation; nav.mode = UnityEngine.UI.Navigation.Mode.Vertical; row.navigation = nav;
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = RowSize + 16f;

        var label = Label(go.transform, font, TimeLabel(preview), RowSize, TextAnchor.MiddleLeft);
        var r = label.rectTransform;
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = new Vector2(12f, 0f); r.offsetMax = new Vector2(-10f - 2f * StepArrowWidth, 0f);
        row.Step = d => StepTime(preview, label, d);

        AddStepArrow(go.transform, font, "<", 1, () => row.Step(-1));
        AddStepArrow(go.transform, font, ">", 0, () => row.Step(1));
        return row;
    }

    /// A mouse-only arrow at the right end of a step row; slot 0 is the rightmost. It never takes keyboard or pad focus.
    private static void AddStepArrow(Transform row, Font font, string text, int slot, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject("Arrow_" + text, typeof(RectTransform));
        go.transform.SetParent(row, false);
        var r = go.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(1f, 0f); r.anchorMax = new Vector2(1f, 1f); r.pivot = new Vector2(1f, 0.5f);
        r.sizeDelta = new Vector2(StepArrowWidth, 0f); r.anchoredPosition = new Vector2(-slot * StepArrowWidth, 0f);
        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = AccentColor;
        var button = go.AddComponent<UnityEngine.UI.Button>();
        button.colors = Tints(AccentColor, AccentHover);
        var nav = button.navigation; nav.mode = UnityEngine.UI.Navigation.Mode.None; button.navigation = nav;
        button.onClick.AddListener(onClick);
        var t = Label(go.transform, font, text, RowSize, TextAnchor.MiddleCenter);
        t.fontStyle = FontStyle.Bold;
        t.rectTransform.anchorMin = Vector2.zero; t.rectTransform.anchorMax = Vector2.one;
        t.rectTransform.offsetMin = Vector2.zero; t.rectTransform.offsetMax = Vector2.zero;
    }

    /// A thin draggable scrollbar in the column's right margin; the ScrollRect hides it when the list fits.
    private static UnityEngine.UI.Scrollbar AddScrollbar(Transform column)
    {
        var track = Rect("Scrollbar", column, TrackColor);
        var r = track.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(1f, 0f); r.anchorMax = new Vector2(1f, 1f); r.pivot = new Vector2(1f, 0.5f);
        r.sizeDelta = new Vector2(ScrollbarWidth, -2f * Margin);
        r.anchoredPosition = new Vector2(-EdgeWidth - (Margin - ScrollbarWidth) * 0.5f, 0f);
        var area = new GameObject("SlidingArea", typeof(RectTransform));
        area.transform.SetParent(track.transform, false);
        var ar = area.GetComponent<RectTransform>();
        ar.anchorMin = Vector2.zero; ar.anchorMax = Vector2.one; ar.offsetMin = Vector2.zero; ar.offsetMax = Vector2.zero;
        var handle = Rect("Handle", area.transform, EdgeColor);
        var hr = handle.GetComponent<RectTransform>();
        hr.offsetMin = Vector2.zero; hr.offsetMax = Vector2.zero;
        var bar = track.AddComponent<UnityEngine.UI.Scrollbar>();
        bar.handleRect = hr;
        bar.targetGraphic = handle.GetComponent<UnityEngine.UI.Image>();
        bar.direction = UnityEngine.UI.Scrollbar.Direction.BottomToTop;
        var nav = bar.navigation; nav.mode = UnityEngine.UI.Navigation.Mode.None; bar.navigation = nav;
        return bar;
    }

    /// "more below" in the column's bottom margin, under the viewport.
    private static GameObject AddMoreBelow(Transform column, Font font)
    {
        var t = Label(column, font, "more below  v", HintSize, TextAnchor.MiddleCenter);
        t.gameObject.name = "MoreBelow";
        t.color = EdgeColor;
        var r = t.rectTransform;
        r.anchorMin = new Vector2(0f, 0f); r.anchorMax = new Vector2(1f, 0f); r.pivot = new Vector2(0.5f, 0f);
        r.offsetMin = new Vector2(Margin, 0f); r.offsetMax = new Vector2(-Margin - EdgeWidth, Margin);
        return t.gameObject;
    }

    private static GameObject Rect(string name, Transform parent, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        go.AddComponent<UnityEngine.UI.Image>().color = color;
        return go;
    }

    private static void AddGap(Transform parent, float height)
    {
        var go = new GameObject("Gap", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = height;
    }

    /// Labels stay on one line at their size and shrink (down to MinTextSize) instead of running past a narrow column.
    private static void FitText(UnityEngine.UI.Text t, int size)
    {
        t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Truncate;
        t.resizeTextForBestFit = true; t.resizeTextMinSize = Mathf.Min(MinTextSize, size); t.resizeTextMaxSize = size;
    }

    private static UnityEngine.UI.Text Label(Transform parent, Font font, string text, int size, TextAnchor alignment)
    {
        var go = new GameObject("Text", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<UnityEngine.UI.Text>();
        t.font = font; t.fontSize = size; t.color = TextColor; t.text = text; t.alignment = alignment; FitText(t, size);
        return t;
    }

    private static RectTransform AddText(Transform parent, Font font, string text, int size, FontStyle style, Color color, float height)
    {
        var t = Label(parent, font, text, size, TextAnchor.MiddleLeft);
        t.fontStyle = style; t.color = color;
        t.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = height;
        return t.rectTransform;
    }

    /// Tints multiply the image colour; hover and keyboard/gamepad focus share one tint.
    private static UnityEngine.UI.ColorBlock Tints(Color normal, Color hover)
    {
        var tint = new Color(hover.r / Mathf.Max(normal.r, 0.01f), hover.g / Mathf.Max(normal.g, 0.01f), hover.b / Mathf.Max(normal.b, 0.01f), 1f);
        var colors = UnityEngine.UI.ColorBlock.defaultColorBlock;
        colors.normalColor = Color.white; colors.highlightedColor = tint; colors.selectedColor = tint;
        colors.pressedColor = new Color(1.3f, 1.3f, 1.3f, 1f); colors.colorMultiplier = 1f;
        return colors;
    }

    private static UnityEngine.UI.Button AddButton(Transform parent, Font font, string text, int size, float indent, Color normal, Color hover, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject("Button_" + text, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = normal;
        var button = go.AddComponent<UnityEngine.UI.Button>();
        button.colors = Tints(normal, hover);
        var nav = button.navigation;
        nav.mode = UnityEngine.UI.Navigation.Mode.Vertical;
        button.navigation = nav;
        button.onClick.AddListener(onClick);
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = size + 16f;
        var t = Label(go.transform, font, text, size, TextAnchor.MiddleLeft);
        var r = t.rectTransform;
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = new Vector2(12f + indent, 0f); r.offsetMax = new Vector2(-10f, 0f);
        return button;
    }
#endif
}
