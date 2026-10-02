// 8.31: the VHS noise band (the rolling static bar) off in every LookTuning look (Grant 2026-10-02). Edit mode; rerunnable.
// main3_8_9f_look.cs copies Day one's band to the Night look, so a rebuild keeps it off.
var names = new System.Collections.Generic.List<string>();
foreach (var g in UnityEditor.AssetDatabase.FindAssets("t:LookTuning", new[] { "Assets" }))
{
    var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); var t = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(p); if (t == null) continue;
    t.noiseBandStrength = 0f; UnityEditor.EditorUtility.SetDirty(t); names.Add(System.IO.Path.GetFileNameWithoutExtension(p));
}
UnityEditor.AssetDatabase.SaveAssets();
int on = 0; foreach (var g in UnityEditor.AssetDatabase.FindAssets("t:LookTuning", new[] { "Assets" })) if (UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(UnityEditor.AssetDatabase.GUIDToAssetPath(g)).noiseBandStrength != 0f) on++;
return "band off in " + names.Count + " looks, still on " + on + " (" + string.Join(", ", names) + ")";
