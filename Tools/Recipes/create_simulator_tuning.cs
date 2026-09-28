// Task 7.2: creates Assets/Settings/SimulatorTuning.asset with the class defaults and
// points it at LoopTuning.asset. Refuses to overwrite.
const string path = "Assets/Settings/SimulatorTuning.asset";
if (UnityEditor.AssetDatabase.LoadAssetAtPath<SimulatorTuning>(path) != null) return "exists: " + path;
var tuning = UnityEngine.ScriptableObject.CreateInstance<SimulatorTuning>();
tuning.loop = UnityEditor.AssetDatabase.LoadAssetAtPath<LoopTuning>("Assets/Settings/LoopTuning.asset");
if (tuning.loop == null) return "missing Assets/Settings/LoopTuning.asset";
UnityEditor.AssetDatabase.CreateAsset(tuning, path);
UnityEditor.AssetDatabase.SaveAssets();
return "created: " + path;
