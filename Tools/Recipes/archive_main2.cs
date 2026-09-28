// Task 5.11: archive Main2. Removes Main2 from the build list and deletes the crash leftover Assets/_Recovery.
// Run through the Editor bridge eval (method body, fully qualified names).
var kept = new System.Collections.Generic.List<UnityEditor.EditorBuildSettingsScene>();
foreach (var s in UnityEditor.EditorBuildSettings.scenes)
    if (s.path != "Assets/Scenes/Main2.unity") kept.Add(s);
UnityEditor.EditorBuildSettings.scenes = kept.ToArray();
bool deleted = UnityEditor.AssetDatabase.DeleteAsset("Assets/_Recovery");
UnityEditor.AssetDatabase.SaveAssets();
var paths = new System.Collections.Generic.List<string>();
foreach (var s in UnityEditor.EditorBuildSettings.scenes) paths.Add(s.path);
return "build list: " + string.Join(", ", paths) + " | _Recovery deleted: " + deleted;
