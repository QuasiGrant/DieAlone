// Task 7.1: creates Assets/Settings/LoopTuning.asset with the class defaults
// (placeholder numbers from Docs/Design/DailyLoop.md). Refuses to overwrite.
const string path = "Assets/Settings/LoopTuning.asset";
if (UnityEditor.AssetDatabase.LoadAssetAtPath<LoopTuning>(path) != null) return "exists: " + path;
var tuning = UnityEngine.ScriptableObject.CreateInstance<LoopTuning>();
UnityEditor.AssetDatabase.CreateAsset(tuning, path);
UnityEditor.AssetDatabase.SaveAssets();
return "created: " + path;
