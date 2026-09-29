using UnityEngine;

/// Every number of the daily loop rules in one asset. Values come from
/// Docs/Design/DailyLoop.md revision 6 (approved for now, DECISIONS 2026-09-29).
/// Lives at Assets/Settings/LoopTuning.asset.
[CreateAssetMenu(fileName = "LoopTuning", menuName = "DieAlone/Loop Tuning")]
public class LoopTuning : ScriptableObject
{
    [Header("Start values and cap")]
    public int startHp = 12;
    public int startMind = 12;
    public int startWard = 12;
    [Tooltip("HP, MIND and WARD never go above this.")]
    public int statMax = 12;

    [Header("Missed needs, paid at sleep")]
    public int foodMissHp = 1;
    public int waterMissHp = 1;
    public int warmthMissHp = 1;
    public int socialMissMind = 1;
    [Tooltip("MIND lost once when any CHECK is still open at sleep. Each event's own cost is separate.")]
    public int safetyMissMind = 1;

    [Header("The Ward")]
    [Tooltip("WARD the Ward takes every night of the first week.")]
    public int hungerStart = 1;
    [Tooltip("Hunger added each week after the first.")]
    public int hungerRisePerWeek = 1;
    public int daysPerWeek = 7;
    [Tooltip("WARD bought by each point of HP or MIND given.")]
    public int wardPerPointGiven = 1;
    [Tooltip("Most points of HP and MIND together given in one night.")]
    public int maxPointsGiven = 3;
    [Tooltip("First night the bunk may be chosen instead of the Ward. It counts as Give nothing.")]
    public int firstBunkNight = 2;

    [Header("Recovery")]
    [Tooltip("Most points all recovery together gives back in one day (RESTORE events, later memories and items).")]
    public int recoveryCapPerDay = 1;

    [Header("Locations checked from the tower")]
    public string[] checkedLocations = { "Lake", "Camp 1", "Camp 2", "Camp 3", "Office" };

    [Header("Anomaly odds per checked location per day")]
    [Tooltip("First day an anomaly can be drawn. Day 1 is always all SAFE when this is 2.")]
    public int firstAnomalyDay = 2;
    [Range(0f, 1f)] public float anomalyChanceStart = 0.10f;
    [Range(0f, 1f)] public float anomalyChancePerDay = 0.02f;
    [Range(0f, 1f)] public float anomalyChanceCap = 0.50f;
    [Tooltip("No new CHECK is drawn while this many are open.")]
    public int maxOpenChecks = 2;

    /// Chance that one checked location draws an anomaly on the given day.
    public float AnomalyChance(int day)
    {
        if (day < firstAnomalyDay) return 0f;
        float chance = anomalyChanceStart + anomalyChancePerDay * (day - firstAnomalyDay);
        return Mathf.Min(chance, anomalyChanceCap);
    }

    /// WARD the Ward takes on the night of the given day: rises each week, never stops.
    public int Hunger(int day) => hungerStart + hungerRisePerWeek * ((day - 1) / daysPerWeek);
}
