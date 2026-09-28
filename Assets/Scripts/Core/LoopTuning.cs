using UnityEngine;

/// Every number of the daily loop rules in one asset. Placeholder values come from
/// Docs/Design/DailyLoop.md (draft); nothing here is decided until DECISIONS.md says so.
/// Lives at Assets/Settings/LoopTuning.asset.
[CreateAssetMenu(fileName = "LoopTuning", menuName = "DieAlone/Loop Tuning")]
public class LoopTuning : ScriptableObject
{
    [Header("Start values")]
    public int startHp = 12;
    public int startMind = 12;
    public int startWard = 12;

    [Header("Missed needs, paid at sleep")]
    public int foodMissHp = 1;
    public int waterMissHp = 1;
    public int warmthMissHp = 1;
    public int socialMissMind = 1;
    [Tooltip("MIND lost per CHECK still open at sleep.")]
    public int openCheckMind = 1;

    [Header("Ward offering")]
    [Tooltip("WARD change by points given tonight: element 0 is Give nothing. The array length minus one is the most a player can give in one night.")]
    public int[] wardChangeByPointsGiven = { -1, 0, 1 };

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

    [Header("Open CHECKs")]
    [Tooltip("An open CHECK moves up one stage each night. Open at sleep on this stage, it ends on its own, badly.")]
    public int checkLastStage = 3;
    [Tooltip("WARD lost once when an open CHECK ends on its own.")]
    public int expiredCheckWard = 1;

    /// Chance that one checked location draws an anomaly on the given day.
    public float AnomalyChance(int day)
    {
        if (day < firstAnomalyDay) return 0f;
        float chance = anomalyChanceStart + anomalyChancePerDay * (day - firstAnomalyDay);
        return Mathf.Min(chance, anomalyChanceCap);
    }

    public int MaxPointsGiven => wardChangeByPointsGiven.Length - 1;
}
