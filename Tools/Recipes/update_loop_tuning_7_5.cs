// Task 7.5: brings Assets/Settings/LoopTuning.asset and SimulatorTuning.asset to
// Docs/Design/DailyLoop.md revision 6 after the field changes: copies every value from
// a fresh instance (the class defaults hold the doc numbers), keeps the asset GUIDs and
// the SimulatorTuning.loop link, and reserializes so dropped fields leave the files.
var loop = UnityEditor.AssetDatabase.LoadAssetAtPath<LoopTuning>("Assets/Settings/LoopTuning.asset");
var sim = UnityEditor.AssetDatabase.LoadAssetAtPath<SimulatorTuning>("Assets/Settings/SimulatorTuning.asset");
if (loop == null || sim == null) return "missing LoopTuning.asset or SimulatorTuning.asset";

var freshLoop = UnityEngine.ScriptableObject.CreateInstance<LoopTuning>();
UnityEditor.EditorUtility.CopySerialized(freshLoop, loop);
UnityEngine.Object.DestroyImmediate(freshLoop);
loop.name = "LoopTuning";

var freshSim = UnityEngine.ScriptableObject.CreateInstance<SimulatorTuning>();
UnityEditor.EditorUtility.CopySerialized(freshSim, sim);
UnityEngine.Object.DestroyImmediate(freshSim);
sim.name = "SimulatorTuning";
sim.loop = loop;

UnityEditor.EditorUtility.SetDirty(loop);
UnityEditor.EditorUtility.SetDirty(sim);
UnityEditor.AssetDatabase.SaveAssets();
UnityEditor.AssetDatabase.ForceReserializeAssets(new[] { "Assets/Settings/LoopTuning.asset", "Assets/Settings/SimulatorTuning.asset" });
return $"updated: hunger {loop.hungerStart}+{loop.hungerRisePerWeek}/week, cap {loop.statMax}, max given {loop.maxPointsGiven}, recovery {loop.recoveryCapPerDay}/day, sim loop linked {sim.loop == loop}";
