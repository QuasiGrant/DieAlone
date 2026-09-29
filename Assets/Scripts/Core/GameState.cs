using System;
using UnityEngine;

/// The whole state of one run and the rules that move it: HP, MIND, WARD, the day's
/// needs, day and night, and the checked locations. Plain C#, no scene access.
/// Rules from Docs/Design/DailyLoop.md revision 6. A run ends the first time any stat
/// reaches 0 (DECISIONS 2026-09-28); the Ward's weekly hunger makes every run end.
/// Day order: MeetNeed / ResolveCheck, then FileReport or EndDayByEvent; at night
/// GiveToWard then Sleep, or SleepInBunk. Events apply their own costs and recovery
/// through ApplyEventCost and Restore at any time.
[Serializable]
public class GameState
{
    private const int ChoreCount = (int)Need.Safety;

    [SerializeField] private int day;
    [SerializeField] private DayPhase phase;
    [SerializeField] private int hp;
    [SerializeField] private int mind;
    [SerializeField] private int ward;
    [SerializeField] private bool[] choresMet;
    [SerializeField] private LocationState[] locations;
    [SerializeField] private bool needsWaived;
    [SerializeField] private bool gaveTonight;
    [SerializeField] private int recoveredToday;
    [SerializeField] private Stat endedBy;

    [NonSerialized] private LoopTuning tuning;

    /// Raised when Sleep finishes, including a sleep that ended the run. Saves hook here.
    [field: NonSerialized] public event Action<GameState> Slept;

    public int Day => day;
    public DayPhase Phase => phase;
    public int Hp => hp;
    public int Mind => mind;
    public int Ward => ward;
    public bool GaveTonight => gaveTonight;
    /// True when an event ended the day and waived the unmet needs.
    public bool NeedsWaived => needsWaived;
    public int RecoveredToday => recoveredToday;
    public bool IsOver => endedBy != Stat.None;
    public Stat EndedBy => endedBy;
    public int LocationCount => locations.Length;
    public LoopTuning Tuning => tuning;

    /// WARD the Ward takes tonight.
    public int Hunger => tuning.Hunger(day);

    public static GameState NewRun(LoopTuning tuning)
    {
        if (tuning == null) throw new ArgumentNullException(nameof(tuning));
        var state = new GameState
        {
            tuning = tuning,
            day = 1,
            phase = DayPhase.Day,
            hp = Mathf.Min(tuning.startHp, tuning.statMax),
            mind = Mathf.Min(tuning.startMind, tuning.statMax),
            ward = Mathf.Min(tuning.startWard, tuning.statMax),
            choresMet = new bool[ChoreCount],
            locations = new LocationState[tuning.checkedLocations.Length],
        };
        for (int i = 0; i < state.locations.Length; i++) state.locations[i] = new LocationState();
        return state;
    }

    public LocationState GetLocation(int index) => locations[index];

    public int OpenChecks
    {
        get
        {
            int count = 0;
            foreach (var location in locations) if (location.IsOpenCheck) count++;
            return count;
        }
    }

    public bool IsNeedMet(Need need) =>
        need == Need.Safety ? OpenChecks == 0 : choresMet[(int)need];

    public int GetStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.Hp: return hp;
            case Stat.Mind: return mind;
            case Stat.Ward: return ward;
            default: throw new ArgumentOutOfRangeException(nameof(stat));
        }
    }

    // ---------- Day ----------

    public void MeetNeed(Need need)
    {
        RequirePhase(DayPhase.Day);
        if (need == Need.Safety)
            throw new ArgumentException("Safety is met by resolving CHECKs, not directly.", nameof(need));
        choresMet[(int)need] = true;
    }

    public void ResolveCheck(int locationIndex)
    {
        RequirePhase(DayPhase.Day);
        var location = locations[locationIndex];
        if (!location.IsOpenCheck)
            throw new InvalidOperationException($"Location {locationIndex} has no open CHECK.");
        location.Clear();
    }

    public void FileReport()
    {
        RequirePhase(DayPhase.Day);
        phase = DayPhase.Night;
    }

    /// An event ended the day: the player wakes into night at the Ward and the report
    /// counts as filed. Each event says whether the needs still unmet are paid.
    public void EndDayByEvent(bool payUnmetNeeds)
    {
        RequirePhase(DayPhase.Day);
        needsWaived = !payUnmetNeeds;
        phase = DayPhase.Night;
    }

    // ---------- Events, any time ----------

    /// An event's own cost. Reaching 0 ends the run on the spot.
    public void ApplyEventCost(Stat stat, int amount)
    {
        RequireRunning();
        if (amount < 0) throw new ArgumentException("A cost cannot be negative.", nameof(amount));
        Change(stat, -amount);
    }

    /// Gives back up to the amount, within the daily recovery cap and the stat cap.
    /// Returns the points actually restored.
    public int Restore(Stat stat, int amount)
    {
        RequireRunning();
        if (amount < 0) throw new ArgumentException("Recovery cannot be negative.", nameof(amount));
        int room = tuning.statMax - GetStat(stat);
        int granted = Mathf.Max(0, Mathf.Min(amount, Mathf.Min(tuning.recoveryCapPerDay - recoveredToday, room)));
        recoveredToday += granted;
        Change(stat, granted);
        return granted;
    }

    // ---------- Night ----------

    /// The most points the Ward screen accepts tonight: the nightly limit, and never a
    /// point that would push WARD past the cap.
    public int MostPointsAccepted
    {
        get
        {
            int room = tuning.statMax - ward + Hunger;
            return Mathf.Clamp(room / tuning.wardPerPointGiven, 0, tuning.maxPointsGiven);
        }
    }

    /// The Ward screen. Give nothing is GiveToWard(0, 0). WARD changes by the WARD the
    /// points buy minus tonight's hunger. Giving a last point is allowed and ends the run.
    public void GiveToWard(int hpGiven, int mindGiven)
    {
        RequirePhase(DayPhase.Night);
        if (gaveTonight) throw new InvalidOperationException("The Ward was already answered tonight.");
        if (hpGiven < 0 || mindGiven < 0) throw new ArgumentException("Points given cannot be negative.");
        int total = hpGiven + mindGiven;
        if (total > MostPointsAccepted)
            throw new ArgumentException($"The Ward accepts at most {MostPointsAccepted} points tonight.");
        if (hpGiven > hp || mindGiven > mind)
            throw new ArgumentException("Cannot give more than the stat holds.");

        gaveTonight = true;
        if (Change(Stat.Hp, -hpGiven)) return;
        if (Change(Stat.Mind, -mindGiven)) return;
        Change(Stat.Ward, total * tuning.wardPerPointGiven - Hunger);
    }

    public int SleepHpCost() => needsWaived ? 0 :
        (choresMet[(int)Need.Food] ? 0 : tuning.foodMissHp)
        + (choresMet[(int)Need.Water] ? 0 : tuning.waterMissHp)
        + (choresMet[(int)Need.Warmth] ? 0 : tuning.warmthMissHp);

    public int SleepMindCost() => needsWaived ? 0 :
        (choresMet[(int)Need.Social] ? 0 : tuning.socialMissMind)
        + (IsNeedMet(Need.Safety) ? 0 : tuning.safetyMissMind);

    /// Pays the day's missed needs and starts the next day with its anomaly draw.
    /// The Ward must have been answered first.
    public void Sleep(System.Random rng)
    {
        RequirePhase(DayPhase.Night);
        if (!gaveTonight) throw new InvalidOperationException("The Ward screen comes before sleep.");

        if (!PayNeeds()) StartNextDay(rng);
        Slept?.Invoke(this);
    }

    /// Lying in the bunk instead of going to the Ward: counts as Give nothing, then sleep.
    /// Not allowed before tuning.firstBunkNight (night 1 is the reveal).
    public void SleepInBunk(System.Random rng)
    {
        RequirePhase(DayPhase.Night);
        if (day < tuning.firstBunkNight)
            throw new InvalidOperationException($"The bunk is not an option before night {tuning.firstBunkNight}.");
        GiveToWard(0, 0);
        if (IsOver) return;
        Sleep(rng);
    }

    /// Reattaches the tuning after the state was rebuilt from a save. Returns false when
    /// the saved arrays do not match the tuning (for example a changed location list).
    internal bool Bind(LoopTuning loopTuning)
    {
        if (loopTuning == null) throw new ArgumentNullException(nameof(loopTuning));
        if (choresMet == null || choresMet.Length != ChoreCount) return false;
        if (locations == null || locations.Length != loopTuning.checkedLocations.Length) return false;
        foreach (var location in locations) if (location == null) return false;
        tuning = loopTuning;
        return true;
    }

    // ---------- Rules ----------

    /// Returns true when paying the needs ended the run.
    private bool PayNeeds()
    {
        if (Change(Stat.Hp, -SleepHpCost())) return true;
        return Change(Stat.Mind, -SleepMindCost());
    }

    private void StartNextDay(System.Random rng)
    {
        day++;
        phase = DayPhase.Day;
        gaveTonight = false;
        needsWaived = false;
        recoveredToday = 0;
        Array.Clear(choresMet, 0, choresMet.Length);
        foreach (var location in locations) location.Clear();
        DrawAnomalies(rng);
    }

    private void DrawAnomalies(System.Random rng)
    {
        float chance = tuning.AnomalyChance(day);
        if (chance <= 0f) return;

        int[] order = new int[locations.Length];
        for (int i = 0; i < order.Length; i++) order[i] = i;
        for (int i = order.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (order[i], order[j]) = (order[j], order[i]);
        }

        foreach (int index in order)
        {
            if (OpenChecks >= tuning.maxOpenChecks) return;
            var location = locations[index];
            if (!location.IsOpenCheck && rng.NextDouble() < chance) location.Open();
        }
    }

    /// Applies a change to one stat, kept between 0 and the cap. Returns true when it ended the run.
    private bool Change(Stat stat, int delta)
    {
        int value = Mathf.Clamp(GetStat(stat) + delta, 0, tuning.statMax);
        switch (stat)
        {
            case Stat.Hp: hp = value; break;
            case Stat.Mind: mind = value; break;
            default: ward = value; break;
        }
        if (value > 0) return false;
        endedBy = stat;
        return true;
    }

    private void RequireRunning()
    {
        if (IsOver) throw new InvalidOperationException($"The run is over ({endedBy} reached 0).");
    }

    private void RequirePhase(DayPhase required)
    {
        RequireRunning();
        if (phase != required) throw new InvalidOperationException($"Only allowed during {required}.");
    }
}
