// 8.15a (edit mode): the carried night lamp on Assets/Prefabs/Player.prefab (DECISIONS 2026-09-30): a child "NightLamp" at the player's
// side, lampHeight up (PlayerTuning), with a Light and NightLamp (which switches it on only at night). Rerunnable: an existing child is
// reused. Saves only the prefab.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string prefabPath = "Assets/Prefabs/Player.prefab", tuningPath = "Assets/Settings/PlayerTuning.asset";
const float lampSide = 0.3f, lampForward = 0.15f;   // held at the right side, a little ahead, so the body does not hide it
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>(tuningPath); if (tuning == null) return "no " + tuningPath;
// the lamp's numbers (PlayerTuning, Night lamp): 8 m, bright enough that the pale trail dirt stands 20 grey over the floor 5 m ahead at
// night (Gate.md 4; measured at 5 m ahead on Camp to J and J to Ward: intensity 5 gave 6 to 20, 10 gave 14 to 36, 20 gave 20 to 79)
tuning.lampRange = 8f; tuning.lampIntensity = 20f; UnityEditor.EditorUtility.SetDirty(tuning); UnityEditor.AssetDatabase.SaveAssetIfDirty(tuning);
var root = UnityEditor.PrefabUtility.LoadPrefabContents(prefabPath);
try
{
    var t = root.transform.Find("NightLamp"); if (t == null) { t = new UnityEngine.GameObject("NightLamp").transform; t.SetParent(root.transform, false); }
    t.localPosition = new UnityEngine.Vector3(lampSide, tuning.lampHeight, lampForward); t.localRotation = UnityEngine.Quaternion.identity;
    var l = t.GetComponent<UnityEngine.Light>(); if (l == null) l = t.gameObject.AddComponent<UnityEngine.Light>();
    l.type = UnityEngine.LightType.Point; l.shadows = UnityEngine.LightShadows.None; l.color = tuning.lampColor; l.range = tuning.lampRange; l.intensity = tuning.lampIntensity;
    var lamp = t.GetComponent<NightLamp>(); if (lamp == null) lamp = t.gameObject.AddComponent<NightLamp>();
    var so = new UnityEditor.SerializedObject(lamp); so.FindProperty("tuning").objectReferenceValue = tuning; so.ApplyModifiedPropertiesWithoutUndo();
    UnityEditor.PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
}
finally { UnityEditor.PrefabUtility.UnloadPrefabContents(root); }
return "night lamp on " + prefabPath + ": #" + UnityEngine.ColorUtility.ToHtmlStringRGB(tuning.lampColor) + ", range " + tuning.lampRange + ", intensity " + tuning.lampIntensity + ", at " + tuning.lampHeight + " m";
