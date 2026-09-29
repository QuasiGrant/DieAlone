using UnityEngine;

/// Numbers for the run simulator (menu DieAlone > Simulate Runs). The loop rules
/// themselves are in LoopTuning; these describe the simulated players and stand-in
/// events. Lives at Assets/Settings/SimulatorTuning.asset.
[CreateAssetMenu(fileName = "SimulatorTuning", menuName = "DieAlone/Simulator Tuning")]
public class SimulatorTuning : ScriptableObject
{
    [Header("Runs")]
    public LoopTuning loop;
    public int runsPerStrategy = 1000;
    public int seed = 1;
    [Tooltip("A run still going after this many days is reported as unfinished.")]
    public int maxDays = 500;

    [Header("Typical player")]
    [Tooltip("Chance of meeting each of Food, Water and Warmth (a missed forage, a blocked option, a skipped chore).")]
    [Range(0f, 1f)] public float choreChance = 0.85f;
    [Tooltip("Chance a resident is home and visited.")]
    [Range(0f, 1f)] public float socialChance = 0.7f;
    [Tooltip("Chance of resolving each open CHECK.")]
    [Range(0f, 1f)] public float resolveChance = 0.75f;

    [Header("Stand-in events (until event content exists)")]
    [Tooltip("Cost of each CHECK still open at day end, to HP, MIND or WARD at random.")]
    public int unresolvedCheckCost = 1;
    [Tooltip("Chance per day, from the first anomaly day, that an event ends a typical player's day before the chores.")]
    [Range(0f, 1f)] public float dayEndingEventChance = 0.05f;
    [Tooltip("Cost of a day-ending event, to HP, MIND or WARD at random.")]
    public int dayEndingEventCost = 1;
    [Tooltip("Chance a day-ending event charges the needs still unmet; otherwise they are waived.")]
    [Range(0f, 1f)] public float dayEndPaysNeedsChance = 0.5f;

    [Header("Recovery")]
    [Tooltip("Points a recovering player is offered each day; GameState caps it at LoopTuning.recoveryCapPerDay.")]
    public int recoveryOffered = 1;
}
