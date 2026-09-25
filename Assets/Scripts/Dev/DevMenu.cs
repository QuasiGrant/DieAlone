using UnityEngine;

/// F1 toggles a panel listing every scene in the build list; picking one loads it.
/// Under the open scene it lists warp points (children of a "DevWarps" object).
/// Exists only in the Editor and development builds. In a release build this class
/// compiles to an empty component: no panel, no F1, nothing to find.
/// The panel is built from code at runtime so the prefab carries no UI for it.
public class DevMenu : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private GameObject panel;
    private Behaviour[] gameplay;
    private bool open;

    // Palette: near-black panel, bone text, one warm accent for the current scene.
    private static readonly Color PanelColor = new Color(0.06f, 0.06f, 0.07f, 0.94f);
    private static readonly Color EdgeColor = new Color(0.85f, 0.55f, 0.30f, 1f);
    private static readonly Color TextColor = new Color(0.92f, 0.88f, 0.80f, 1f);
    private static readonly Color DimColor = new Color(0.60f, 0.57f, 0.52f, 1f);
    private static readonly Color ButtonColor = new Color(0.16f, 0.16f, 0.18f, 1f);
    private static readonly Color ButtonHover = new Color(0.26f, 0.24f, 0.22f, 1f);
    private static readonly Color AccentColor = new Color(0.55f, 0.32f, 0.16f, 1f);
    private static readonly Color AccentHover = new Color(0.70f, 0.42f, 0.22f, 1f);
    private static readonly Color SubColor = new Color(0.11f, 0.11f, 0.12f, 1f);

    private void Update()
    {
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard == null || !keyboard.f1Key.wasPressedThisFrame) return;
        if (GamePause.Instance != null && GamePause.Instance.IsPaused) return;   // not over the pause menu
        Toggle();
    }

    private void Toggle()
    {
        if (open) Close(); else Open();
    }

    private void Open()
    {
        if (panel == null) Build();
        panel.SetActive(true);
        open = true;
        gameplay = FindGameplay();
        foreach (var b in gameplay) if (b != null) b.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Close()
    {
        if (panel != null) panel.SetActive(false);
        open = false;
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

    /// Friendly names for the build-list scenes.
    private static string DisplayName(string sceneName)
    {
        switch (sceneName)
        {
            case "Main": return "Main 1.0  (saved version)";
            case "Main2": return "Main 2.0  (current work)";
            case "Graybox": return "Graybox  (test course)";
            default: return sceneName;
        }
    }

    // ---- UI built from code: a dark left column with a title, the scene list, and warps under the open scene.

    private void Build()
    {
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        panel = new GameObject("DevMenuCanvas");
        panel.transform.SetParent(transform, false);
        var canvas = panel.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = panel.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 1f;
        panel.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Column background with a thin accent edge on its right side.
        var bg = Rect("Background", panel.transform, PanelColor);
        var bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 0f); bgRect.anchorMax = new Vector2(0f, 1f);
        bgRect.pivot = new Vector2(0f, 0.5f);
        bgRect.offsetMin = Vector2.zero; bgRect.offsetMax = Vector2.zero;
        bgRect.sizeDelta = new Vector2(340f, 0f);
        var edge = Rect("Edge", bg.transform, EdgeColor).GetComponent<RectTransform>();
        edge.anchorMin = new Vector2(1f, 0f); edge.anchorMax = new Vector2(1f, 1f); edge.pivot = new Vector2(1f, 0.5f);
        edge.offsetMin = Vector2.zero; edge.offsetMax = Vector2.zero; edge.sizeDelta = new Vector2(3f, 0f);

        var list = new GameObject("List", typeof(RectTransform));
        list.transform.SetParent(bg.transform, false);
        var listRect = list.GetComponent<RectTransform>();
        listRect.anchorMin = new Vector2(0f, 0f); listRect.anchorMax = new Vector2(1f, 1f);
        listRect.offsetMin = new Vector2(18f, 18f); listRect.offsetMax = new Vector2(-21f, -18f);
        var layout = list.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
        layout.spacing = 6f;
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.childControlWidth = true; layout.childControlHeight = true;
        layout.childForceExpandWidth = true; layout.childForceExpandHeight = false;

        AddText(list.transform, font, "DEV MENU", 26, FontStyle.Bold, TextColor, 38f);
        AddText(list.transform, font, "F1 closes.  Scenes load fresh; warps move the player.", 13, FontStyle.Normal, DimColor, 22f);
        AddGap(list.transform, 10f);
        AddText(list.transform, font, "SCENES", 12, FontStyle.Bold, DimColor, 20f);

        int count = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        for (int i = 0; i < count; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            bool current = path == active;
            AddButton(list.transform, font, (current ? "▶  " : "     ") + DisplayName(name), 17, 0f, current ? AccentColor : ButtonColor, current ? AccentHover : ButtonHover, current ? null : (UnityEngine.Events.UnityAction)(() => LoadScene(path)));
            if (!current) continue;
            var warps = GameObject.Find("DevWarps");
            if (warps == null || warps.transform.childCount == 0) continue;
            AddGap(list.transform, 4f);
            AddText(list.transform, font, "WARP TO", 12, FontStyle.Bold, DimColor, 20f);
            foreach (Transform w in warps.transform)
            {
                var target = w;
                AddButton(list.transform, font, w.name, 15, 22f, SubColor, ButtonHover, () => Warp(target));
            }
        }
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

    private static void AddText(Transform parent, Font font, string text, int size, FontStyle style, Color color, float height)
    {
        var go = new GameObject("Text", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<UnityEngine.UI.Text>();
        t.font = font; t.fontSize = size; t.fontStyle = style; t.color = color; t.text = text;
        t.alignment = TextAnchor.MiddleLeft; t.horizontalOverflow = HorizontalWrapMode.Overflow;
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = height;
    }

    private static void AddButton(Transform parent, Font font, string text, int size, float indent, Color normal, Color hover, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject("Button_" + text, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = normal;
        var button = go.AddComponent<UnityEngine.UI.Button>();
        var colors = button.colors;
        colors.normalColor = Color.white; colors.highlightedColor = new Color(hover.r / Mathf.Max(normal.r, 0.01f), hover.g / Mathf.Max(normal.g, 0.01f), hover.b / Mathf.Max(normal.b, 0.01f), 1f);
        colors.pressedColor = new Color(1.3f, 1.3f, 1.3f, 1f); colors.selectedColor = Color.white; colors.colorMultiplier = 1f;
        button.colors = colors;
        if (onClick != null) button.onClick.AddListener(onClick); else button.interactable = false;
        var le = go.AddComponent<UnityEngine.UI.LayoutElement>();
        le.preferredHeight = size + 16f;
        if (indent > 0f)
        {
            // Indented rows leave a margin on the left so warps read as children of the scene row.
            var pad = go.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
            pad.padding = new RectOffset((int)indent + 10, 10, 0, 0); pad.childAlignment = TextAnchor.MiddleLeft;
            pad.childControlWidth = true; pad.childControlHeight = true; pad.childForceExpandWidth = true; pad.childForceExpandHeight = true;
        }
        var label = new GameObject("Text", typeof(RectTransform));
        label.transform.SetParent(go.transform, false);
        var t = label.AddComponent<UnityEngine.UI.Text>();
        t.font = font; t.fontSize = size; t.color = TextColor; t.text = text; t.alignment = TextAnchor.MiddleLeft; t.horizontalOverflow = HorizontalWrapMode.Overflow;
        var r = label.GetComponent<RectTransform>();
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = new Vector2(indent > 0f ? 0f : 12f, 0f); r.offsetMax = new Vector2(-10f, 0f);
    }
#endif
}
