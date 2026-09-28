using UnityEngine;

/// Numbers for the run simulator (menu DieAlone > Simulate Runs). Chore costs are
/// duty seconds with the walk included, rough fits to Docs/Design/Main3.md section 7.
/// Lives at Assets/Settings/SimulatorTuning.asset.
[CreateAssetMenu(fileName = "SimulatorTuning", menuName = "DieAlone/Simulator Tuning")]
public class SimulatorTuning : ScriptableObject
{
    [Header("Runs")]
    public LoopTuning loop;
    public int runsPerStrategy = 1000;
    public int seed = 1;
    [Tooltip("A run still going after this many days is reported as unfinished.")]
    public int maxDays = 500;

    [Header("Duty time (seconds)")]
    [Tooltip("540 s of duty minus the tower (100 s) and the report (20 s).")]
    public int freeSeconds = 420;

    [Header("Chore costs, walk included (seconds)")]
    public int waterSeconds = 60;
    public int warmthSeconds = 48;
    public int socialSeconds = 100;
    public int forageSeconds = 80;
    [Tooltip("Second forage try at the other patch after a miss.")]
    public int forageRetrySeconds = 40;
    [Range(0f, 1f)] public float forageChance = 0.6f;
    [Tooltip("The sure store at the far edge of the map.")]
    public int storeSeconds = 180;

    [Header("Resolving a CHECK (seconds)")]
    public int resolveSeconds = 90;
    public int resolveWalkSeconds = 60;
    [Tooltip("Extra seconds per stage a CHECK has been left open.")]
    public int resolveSecondsPerStage = 30;

    public int ResolveCost(int stage) => resolveSeconds + resolveWalkSeconds + resolveSecondsPerStage * (stage - 1);
}
