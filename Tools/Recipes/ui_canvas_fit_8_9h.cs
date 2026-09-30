// 8.9h (edit mode): adds CanvasFit to the PauseMenu and HUD canvases in Assets/Prefabs/GameSystems.prefab so both stay
// readable and on screen at any window size. PauseMenu fits its Panel/Content box; the HUD has no fit box (its prompt
// and dot follow the screen; the prompt box spans the screen width). Reference height 720 and floor 1 match the dev panel.
// Saves only the prefab.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string PrefabPath = "Assets/Prefabs/GameSystems.prefab";
const float ReferenceHeight = 720f, MinScale = 1f, FitMargin = 16f;
var root = UnityEditor.PrefabUtility.LoadPrefabContents(PrefabPath);
var report = new System.Text.StringBuilder();
try
{
    foreach (var name in new[] { "PauseMenu", "HUD" })
    {
        UnityEngine.Transform t = null;
        foreach (var c in root.GetComponentsInChildren<UnityEngine.Canvas>(true)) if (c.name == name && c.isRootCanvas) t = c.transform;
        if (t == null) return "no canvas " + name;
        var fit = t.GetComponent<CanvasFit>(); if (fit == null) fit = t.gameObject.AddComponent<CanvasFit>();
        var so = new UnityEditor.SerializedObject(fit);
        so.FindProperty("referenceHeight").floatValue = ReferenceHeight;
        so.FindProperty("minScale").floatValue = MinScale;
        so.FindProperty("fitMargin").floatValue = FitMargin;
        so.FindProperty("fitTarget").objectReferenceValue = name == "PauseMenu" ? t.Find("Panel/Content") : null;
        so.ApplyModifiedPropertiesWithoutUndo();
        report.Append(name + " fit target " + (so.FindProperty("fitTarget").objectReferenceValue != null ? "Panel/Content" : "none") + "; ");
    }
    // HUD/Prompt was a fixed 400-unit box, wider than a narrow screen: it now spans the screen width less a margin each
    // side, at the same height and offset under the centre (the text is centred and wraps)
    UnityEngine.RectTransform prompt = null;
    foreach (var t in root.GetComponentsInChildren<UnityEngine.UI.Text>(true)) if (t.name == "Prompt") prompt = t.rectTransform;
    if (prompt == null) return "no HUD/Prompt";
    float promptY = prompt.anchoredPosition.y, promptH = prompt.sizeDelta.y;
    prompt.anchorMin = new UnityEngine.Vector2(0f, 0.5f); prompt.anchorMax = new UnityEngine.Vector2(1f, 0.5f);
    prompt.sizeDelta = new UnityEngine.Vector2(-2f * FitMargin, promptH); prompt.anchoredPosition = new UnityEngine.Vector2(0f, promptY);
    report.Append("prompt spans width less " + FitMargin + " each side; ");
    UnityEditor.PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
}
finally { UnityEditor.PrefabUtility.UnloadPrefabContents(root); }
return report + "saved " + PrefabPath;
