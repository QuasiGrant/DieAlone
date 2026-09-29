// 8.9g (edit mode): makes Play start in the "Day one" look by setting startLook on LookPreview in
// Assets/Prefabs/GameSystems.prefab. The day-one values themselves (sun 28, crushed blacks 0.15, dark corners 0.3,
// fill #998A73) live in their owning recipe, main3_8_9d_dress_camp.cs, so a runner rebuild keeps them; the night look
// keeps its own crush and corners in main3_8_9f_look.cs. This recipe only reads them back. It saves only the prefab,
// never a blanket SaveAssets, so the URP global settings are not re-saved. Reports any scene instance overriding startLook.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string DayOnePath = "Assets/Settings/LookTuning_DayOne.asset", NightPath = "Assets/Settings/LookTuning.asset", PrefabPath = "Assets/Prefabs/GameSystems.prefab", StartLabel = "Day one";
var dayOne = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(DayOnePath);
var night = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(NightPath);
if (dayOne == null || night == null) return "missing look assets";

var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(PrefabPath);
var preview = prefab != null ? prefab.GetComponentInChildren<LookPreview>(true) : null;
if (preview == null) return "no LookPreview in " + PrefabPath;
var so = new UnityEditor.SerializedObject(preview);
var looks = so.FindProperty("looks");
int index = -1;
for (int i = 0; i < looks.arraySize; i++) if (looks.GetArrayElementAtIndex(i).FindPropertyRelative("label").stringValue == StartLabel) index = i;
if (index < 0) return "no look labelled " + StartLabel;
so.FindProperty("startLook").intValue = index;
if (so.ApplyModifiedPropertiesWithoutUndo()) UnityEditor.PrefabUtility.SavePrefabAsset(prefab);

var overrides = new System.Collections.Generic.List<string>();
foreach (var lp in UnityEngine.Object.FindObjectsByType<LookPreview>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    var p = new UnityEditor.SerializedObject(lp).FindProperty("startLook");
    if (p.prefabOverride) overrides.Add(lp.gameObject.scene.name + "/" + lp.name + "=" + p.intValue);
}
return "day one sun " + dayOne.sunElevation + " crush " + dayOne.crushBlacks + " corners " + dayOne.darkCorners + " fill #" + UnityEngine.ColorUtility.ToHtmlStringRGB(dayOne.sunsetAmbient)
    + " | night crush " + night.crushBlacks + " corners " + night.darkCorners
    + " | GameSystems startLook " + index + " (" + StartLabel + ") | open-scene overrides: " + (overrides.Count == 0 ? "none" : string.Join(", ", overrides));
