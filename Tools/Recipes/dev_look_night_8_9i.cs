// 8.9i (edit mode): names the scene's own look "Night" in the dev panel's LOOK section (LookPreview looks[0] in
// Assets/Prefabs/GameSystems.prefab). Both build-list scenes (Graybox, Main3) have a night scene look: LookTuning.asset,
// the Night fog set and ambient, the NightLighting fire fill, LookVisibility (stand-in fire, cab glow) and PracticalLight
// at full. Picking Night clears the day override, so all of those return; no new system. Saves only the prefab.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string PrefabPath = "Assets/Prefabs/GameSystems.prefab", NightLabel = "Night";
var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(PrefabPath);
var preview = prefab != null ? prefab.GetComponentInChildren<LookPreview>(true) : null;
if (preview == null) return "no LookPreview in " + PrefabPath;
var so = new UnityEditor.SerializedObject(preview);
var looks = so.FindProperty("looks");
int scene = -1;
for (int i = 0; i < looks.arraySize; i++) if (looks.GetArrayElementAtIndex(i).FindPropertyRelative("tuning").objectReferenceValue == null) scene = i;
if (scene < 0) return "no scene look (empty tuning) in the list";
looks.GetArrayElementAtIndex(scene).FindPropertyRelative("label").stringValue = NightLabel;
if (so.ApplyModifiedPropertiesWithoutUndo()) UnityEditor.PrefabUtility.SavePrefabAsset(prefab);
var labels = new System.Collections.Generic.List<string>();
for (int i = 0; i < looks.arraySize; i++) labels.Add(looks.GetArrayElementAtIndex(i).FindPropertyRelative("label").stringValue);
return "LOOK rows: " + string.Join(", ", labels) + " | start " + so.FindProperty("startLook").intValue;
