// 8.9h follow-up (Pim; MainMenu.md 2.6): player canvases (PauseMenu, HUD in Assets/Prefabs/GameSystems.prefab) move to a
// 1080-row reference with a floor of 0.667 (720 rows), so Style.md 7.2 sizes are typed as written. The existing layout was
// authored at 720 rows, so every size under those canvases is multiplied once by 1080 / 720 (rect sizes and positions,
// font sizes, layout spacing, padding and preferred sizes): the menus look the same at any window size as before. Runs once:
// a canvas whose CanvasFit already has the 1080 reference is left alone. The dev panel is not touched. Saves only the prefab.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string PrefabPath = "Assets/Prefabs/GameSystems.prefab";
const float OldReference = 720f, NewReference = 1080f, MinScale = 0.667f, FitMargin = 24f;
const float K = NewReference / OldReference;
var root = UnityEditor.PrefabUtility.LoadPrefabContents(PrefabPath);
var report = new System.Text.StringBuilder();
try
{
    foreach (var name in new[] { "PauseMenu", "HUD" })
    {
        UnityEngine.Canvas canvas = null;
        foreach (var c in root.GetComponentsInChildren<UnityEngine.Canvas>(true)) if (c.name == name && c.isRootCanvas) canvas = c;
        if (canvas == null) return "no canvas " + name;
        var fit = canvas.GetComponent<CanvasFit>(); if (fit == null) return name + " has no CanvasFit; run ui_canvas_fit_8_9h.cs first";
        var so = new UnityEditor.SerializedObject(fit);
        if (UnityEngine.Mathf.Approximately(so.FindProperty("referenceHeight").floatValue, NewReference)) { report.Append(name + " already at 1080; "); continue; }
        int rects = 0, texts = 0;
        foreach (var rt in canvas.GetComponentsInChildren<UnityEngine.RectTransform>(true))
        {
            if (rt == canvas.transform) continue;
            rt.sizeDelta *= K; rt.anchoredPosition *= K; rects++;
        }
        foreach (var t in canvas.GetComponentsInChildren<UnityEngine.UI.Text>(true))
        {
            t.fontSize = UnityEngine.Mathf.RoundToInt(t.fontSize * K);
            t.resizeTextMinSize = UnityEngine.Mathf.RoundToInt(t.resizeTextMinSize * K); t.resizeTextMaxSize = UnityEngine.Mathf.RoundToInt(t.resizeTextMaxSize * K); texts++;
        }
        foreach (var lg in canvas.GetComponentsInChildren<UnityEngine.UI.HorizontalOrVerticalLayoutGroup>(true))
        {
            lg.spacing *= K;
            lg.padding = new UnityEngine.RectOffset(UnityEngine.Mathf.RoundToInt(lg.padding.left * K), UnityEngine.Mathf.RoundToInt(lg.padding.right * K), UnityEngine.Mathf.RoundToInt(lg.padding.top * K), UnityEngine.Mathf.RoundToInt(lg.padding.bottom * K));
        }
        foreach (var le in canvas.GetComponentsInChildren<UnityEngine.UI.LayoutElement>(true))
        {
            if (le.minWidth > 0f) le.minWidth *= K; if (le.minHeight > 0f) le.minHeight *= K;
            if (le.preferredWidth > 0f) le.preferredWidth *= K; if (le.preferredHeight > 0f) le.preferredHeight *= K;
        }
        so.FindProperty("referenceHeight").floatValue = NewReference;
        so.FindProperty("minScale").floatValue = MinScale;
        so.FindProperty("fitMargin").floatValue = FitMargin;
        so.ApplyModifiedPropertiesWithoutUndo();
        report.Append(name + ": " + rects + " rects and " + texts + " texts x" + K.ToString("F2") + ", reference 1080, floor " + MinScale + "; ");
    }
    UnityEditor.PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
}
finally { UnityEditor.PrefabUtility.UnloadPrefabContents(root); }
return report + "saved " + PrefabPath;
