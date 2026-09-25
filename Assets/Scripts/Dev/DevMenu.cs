using UnityEngine;

/// F1 toggles a panel listing every scene in the build list; picking one loads it.
/// Exists only in the Editor and development builds. In a release build this class
/// compiles to an empty component: no panel, no F1, nothing to find.
/// The panel is built from code at runtime so the prefab carries no UI for it.
public class DevMenu : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private GameObject panel;
    private Behaviour[] gameplay;
    private bool open;

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

    // ---- UI built from code: a dark canvas with a title and one button per scene.

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
        panel.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        var bg = new GameObject("Background", typeof(RectTransform));
        bg.transform.SetParent(panel.transform, false);
        var bgImage = bg.AddComponent<UnityEngine.UI.Image>();
        bgImage.color = new Color(0f, 0f, 0f, 0.8f);
        var bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 0f);
        bgRect.anchorMax = new Vector2(0.3f, 1f);
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        var list = new GameObject("List", typeof(RectTransform));
        list.transform.SetParent(bg.transform, false);
        var listRect = list.GetComponent<RectTransform>();
        listRect.anchorMin = new Vector2(0f, 0f);
        listRect.anchorMax = new Vector2(1f, 1f);
        listRect.offsetMin = new Vector2(16f, 16f);
        listRect.offsetMax = new Vector2(-16f, -16f);
        var layout = list.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
        layout.spacing = 8f;
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        AddLabel(list.transform, font, "DEV MENU  (F1 closes)", 22);
        AddLabel(list.transform, font, "Scenes in build list:", 16);

        int count = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string active = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        for (int i = 0; i < count; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            AddButton(list.transform, font, name, () => LoadScene(path));
            if (path != active) continue;
            // Warp points: children of a "DevWarps" object in the open scene, listed under it.
            var warps = GameObject.Find("DevWarps");
            if (warps == null) continue;
            foreach (Transform w in warps.transform)
            {
                var target = w;
                AddButton(list.transform, font, "    > " + w.name, () => Warp(target));
            }
        }
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

    private static void AddLabel(Transform parent, Font font, string text, int size)
    {
        var go = new GameObject("Label", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<UnityEngine.UI.Text>();
        t.font = font; t.fontSize = size; t.color = Color.white; t.text = text;
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = size + 12;
    }

    private static void AddButton(Transform parent, Font font, string text, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject("Button_" + text, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = new Color(0.25f, 0.25f, 0.25f, 1f);
        var button = go.AddComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(onClick);
        go.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 36f;

        var label = new GameObject("Text", typeof(RectTransform));
        label.transform.SetParent(go.transform, false);
        var t = label.AddComponent<UnityEngine.UI.Text>();
        t.font = font; t.fontSize = 18; t.color = Color.white; t.text = text; t.alignment = TextAnchor.MiddleCenter;
        var r = label.GetComponent<RectTransform>();
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = Vector2.zero; r.offsetMax = Vector2.zero;
    }
#endif
}
