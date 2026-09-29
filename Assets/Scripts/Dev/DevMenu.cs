using UnityEngine;

/// The one dev panel. F1 on the keyboard or View (Select) on a gamepad opens and closes it;
/// B on a gamepad also closes it. Three labelled sections:
/// SCENES every scene in the build list (picking one loads it),
/// WARPS the open scene's warp points (children of a "DevWarps" object, shown with readable names),
/// LOOK the looks listed on LookPreview (Current, Day one, Day two).
/// Mouse clicks, arrow keys or WASD with Enter, and stick or d-pad with A all work.
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
    private const float WarpIndent = 22f;
    private const int TitleSize = 26;
    private const int HintSize = 13;
    private const int HeaderSize = 13;
    private const int RowSize = 17;
    private const int SubRowSize = 15;
    private const int MinTextSize = 10;           // best-fit floor when a label is wider than a narrow column
    private const float ReferenceHeight = 720f;   // the canvas scales with screen height
    private const float ScrollSensitivity = 30f;
    private const int SortingOrder = 100;

    private GameObject panel;
    private Behaviour[] gameplay;
    private bool open;

    // Layout that follows the screen: the panel sits inside the safe area, the column never gets wider than it,
    // and the list scrolls (mouse wheel, or following the keyboard and gamepad selection) when it is taller.
    private RectTransform safeRect;
    private RectTransform columnRect;
    private RectTransform viewportRect;
    private RectTransform listRect;
    private Vector2Int fittedScreen;
    private Rect fittedSafeArea;

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

    private void Update()
    {
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        var gamepad = UnityEngine.InputSystem.Gamepad.current;
        bool toggle = (keyboard != null && keyboard.f1Key.wasPressedThisFrame)
                   || (gamepad != null && gamepad.selectButton.wasPressedThisFrame);
        bool back = open && gamepad != null && gamepad.buttonEast.wasPressedThisFrame;
        if (back) { Close(); return; }
        if (open) { FitToScreen(); KeepSelectionVisible(); }
        if (!toggle) return;
        if (!open && GamePause.Instance != null && GamePause.Instance.IsPaused) return;   // not over the pause menu
        if (open) Close(); else Open();
    }

    private void Open()
    {
        Rebuild();
        open = true;
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

    private void PickLook(LookPreview preview, int index)
    {
        preview.Select(index);
        Close();
    }

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
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        panel = new GameObject("DevMenuCanvas");
        panel.transform.SetParent(transform, false);
        var canvas = panel.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = SortingOrder;
        var scaler = panel.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(ReferenceHeight * 16f / 9f, ReferenceHeight);
        scaler.matchWidthOrHeight = 1f;
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

        var scroll = bg.AddComponent<UnityEngine.UI.ScrollRect>();
        scroll.viewport = viewportRect; scroll.content = listRect;
        scroll.horizontal = false; scroll.vertical = true;
        scroll.movementType = UnityEngine.UI.ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = ScrollSensitivity;

        fittedScreen = Vector2Int.zero;   // force the first fit
        FitToScreen();

        AddText(list.transform, font, "DEV MENU", TitleSize, FontStyle.Bold, TextColor, 36f);
        AddText(list.transform, font, "F1 or gamepad View opens and closes.  B closes.", HintSize, FontStyle.Normal, DimColor, 18f);
        AddText(list.transform, font, "Mouse, arrows + Enter, or stick + A to pick.", HintSize, FontStyle.Normal, DimColor, 18f);

        UnityEngine.UI.Selectable first = null;
        void Keep(UnityEngine.UI.Selectable s) { if (first == null && s != null && s.interactable) first = s; }

        // SCENES
        AddSection(list.transform, font, "SCENES");
        int count = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        for (int i = 0; i < count; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            bool current = path == active;
            string label = SceneLabel(System.IO.Path.GetFileNameWithoutExtension(path)) + (current ? "   (reload)" : "");
            Keep(AddButton(list.transform, font, label, RowSize, 0f, current ? AccentColor : ButtonColor, current ? AccentHover : ButtonHover, () => LoadScene(path)));
        }

        // WARPS
        AddSection(list.transform, font, "WARPS");
        var warps = FindWarpRoot();
        if (warps == null || warps.childCount == 0)
            AddText(list.transform, font, "No warp points in this scene.", SubRowSize, FontStyle.Italic, DimColor, 22f);
        else
            foreach (Transform w in warps)
            {
                var target = w;
                Keep(AddButton(list.transform, font, Readable(w.name), SubRowSize, WarpIndent, SubColor, ButtonHover, () => Warp(target)));
            }

        // LOOK
        AddSection(list.transform, font, "LOOK");
        var preview = FindFirstObjectByType<LookPreview>();
        if (preview == null || preview.Count == 0)
            AddText(list.transform, font, "No look preview in this scene.", SubRowSize, FontStyle.Italic, DimColor, 22f);
        else
            for (int i = 0; i < preview.Count; i++)
            {
                int index = i;
                bool current = i == preview.Current;
                Keep(AddButton(list.transform, font, preview.Label(i), SubRowSize, WarpIndent, current ? AccentColor : SubColor, current ? AccentHover : ButtonHover, () => PickLook(preview, index)));
            }

        // Keyboard and gamepad start on the first row.
        var events = UnityEngine.EventSystems.EventSystem.current;
        if (events != null && first != null) events.SetSelectedGameObject(first.gameObject);
    }

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
        float scale = screen.y / ReferenceHeight;   // the CanvasScaler's factor when it matches height
        float safeWidth = area.width / scale;
        columnRect.sizeDelta = new Vector2(Mathf.Min(ColumnWidth, safeWidth), 0f);
    }

    /// Scrolls the list so the row picked with the keyboard or gamepad is always inside the viewport.
    private void KeepSelectionVisible()
    {
        var events = UnityEngine.EventSystems.EventSystem.current;
        if (events == null || listRect == null) return;
        var selected = events.currentSelectedGameObject;
        if (selected == null || !selected.transform.IsChildOf(listRect)) return;
        var item = (RectTransform)selected.transform;
        var itemCorners = new Vector3[4]; var viewCorners = new Vector3[4];
        item.GetWorldCorners(itemCorners); viewportRect.GetWorldCorners(viewCorners);
        float above = itemCorners[1].y - viewCorners[1].y;   // item top over the viewport top
        float below = viewCorners[0].y - itemCorners[0].y;   // item bottom under the viewport bottom
        float delta = above > 0f ? -above : below > 0f ? below : 0f;
        if (delta == 0f) return;
        float scale = listRect.lossyScale.y > 0f ? listRect.lossyScale.y : 1f;
        listRect.anchoredPosition += new Vector2(0f, delta / scale);
    }

    private static void AddSection(Transform parent, Font font, string title)
    {
        AddGap(parent, SectionGap);
        AddText(parent, font, title, HeaderSize, FontStyle.Bold, EdgeColor, 18f);
        var rule = Rect("Rule", parent, RuleColor);
        rule.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 1f;
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

    private static void AddText(Transform parent, Font font, string text, int size, FontStyle style, Color color, float height)
    {
        var go = new GameObject("Text", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<UnityEngine.UI.Text>();
        t.font = font; t.fontSize = size; t.fontStyle = style; t.color = color; t.text = text;
        t.alignment = TextAnchor.MiddleLeft; FitText(t, size);
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = height;
    }

    private static UnityEngine.UI.Button AddButton(Transform parent, Font font, string text, int size, float indent, Color normal, Color hover, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject("Button_" + text, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = normal;
        var button = go.AddComponent<UnityEngine.UI.Button>();
        // Tints multiply the image colour; hover and keyboard/gamepad focus share one tint.
        var tint = new Color(hover.r / Mathf.Max(normal.r, 0.01f), hover.g / Mathf.Max(normal.g, 0.01f), hover.b / Mathf.Max(normal.b, 0.01f), 1f);
        var colors = button.colors;
        colors.normalColor = Color.white; colors.highlightedColor = tint; colors.selectedColor = tint;
        colors.pressedColor = new Color(1.3f, 1.3f, 1.3f, 1f); colors.colorMultiplier = 1f;
        button.colors = colors;
        var nav = button.navigation;
        nav.mode = UnityEngine.UI.Navigation.Mode.Vertical;
        button.navigation = nav;
        button.onClick.AddListener(onClick);
        var le = go.AddComponent<UnityEngine.UI.LayoutElement>();
        le.preferredHeight = size + 16f;
        var label = new GameObject("Text", typeof(RectTransform));
        label.transform.SetParent(go.transform, false);
        var t = label.AddComponent<UnityEngine.UI.Text>();
        t.font = font; t.fontSize = size; t.color = TextColor; t.text = text; t.alignment = TextAnchor.MiddleLeft; FitText(t, size);
        var r = label.GetComponent<RectTransform>();
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = new Vector2(12f + indent, 0f); r.offsetMax = new Vector2(-10f, 0f);
        return button;
    }
#endif
}
