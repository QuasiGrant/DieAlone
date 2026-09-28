using UnityEditor;
using UnityEngine;

/// Menu DieAlone > Simulate Runs: plays SimulatorTuning.runsPerStrategy runs for each
/// default strategy and logs the day each run ended and which stat ended it.
public static class RunSimulatorMenu
{
    private const string TuningPath = "Assets/Settings/SimulatorTuning.asset";

    [MenuItem("DieAlone/Simulate Runs")]
    public static void SimulateRuns() => Debug.Log(Run());

    /// Runs the simulation and returns the report text (also callable from eval).
    public static string Run()
    {
        var tuning = AssetDatabase.LoadAssetAtPath<SimulatorTuning>(TuningPath);
        if (tuning == null || tuning.loop == null)
            return $"Run simulator: missing {TuningPath} or its LoopTuning reference.";

        var simulator = new RunSimulator(tuning);
        var strategies = RunSimulator.DefaultStrategies();
        var report = new System.Text.StringBuilder("Run simulator\n");
        for (int i = 0; i < strategies.Length; i++)
            report.AppendLine(simulator.Summarize(strategies[i], simulator.PlayRuns(strategies[i], i)));
        return report.ToString();
    }
}
