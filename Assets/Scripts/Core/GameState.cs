using System;
using UnityEngine;

/// The whole state of one run and the rules that move it: HP, MIND, WARD, the day's
/// needs, day and night, and the checked locations. Plain C#, no scene access.
/// A run ends the first time any stat reaches 0 (DECISIONS 2026-09-28).
/// Day order: MeetNeed / ResolveCheck, FileReport, GiveToWard, Sleep.
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
    [SerializeField] private bool gaveTonight;
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
    public bool IsOver => endedBy != Stat.None;
    public Stat EndedBy => endedBy;
    public int LocationCount => locations.Length;
    public LoopTuning Tuning => tuning;

    public static GameState NewRun(LoopTuning tuning)
    {
        if (tuning == null) throw new ArgumentNullException(nameof(tuning));
        var state = new GameState
        {
            tuning = tuning,
            day = 1,
            phase = DayPhase.Day,
            hp = tuning.startHp,
            mind = tuning.startMind,
            ward = tuning.startWard,
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

    // ---------- Night ----------

    /// The Ward screen. Give nothing is GiveToWard(0, 0). Giving a last point is
    /// allowed and ends the run.
    public void GiveToWard(int hpGiven, int mindGiven)
    {
        RequirePhase(DayPhase.Night);
        if (gaveTonight) throw new InvalidOperationException("The Ward was already answered tonight.");
        if (hpGiven < 0 || mindGiven < 0) throw new ArgumentException("Points given cannot be negative.");
        int total = hpGiven + mindGiven;
        if (total > tuning.MaxPointsGiven)
            throw new ArgumentException($"At most {tuning.MaxPointsGiven} points a night.");
        if (hpGiven > hp || mindGiven > mind)
            throw new ArgumentException("Cannot give more than the stat holds.");

        gaveTonight = true;
        if (Change(Stat.Hp, -hpGiven)) return;
        if (Change(Stat.Mind, -mindGiven)) return;
        Change(Stat.Ward, tuning.wardChangeByPointsGiven[total]);
    }

    public int SleepHpCost() =>
        (choresMet[(int)Need.Food] ? 0 : tuning.foodMissHp)
        + (choresMet[(int)Need.Water] ? 0 : tuning.waterMissHp)
        + (choresMet[(int)Need.Warmth] ? 0 : tuning.warmthMissHp);

    public int SleepMindCost() =>
        (choresMet[(int)Need.Social] ? 0 : tuning.socialMissMind)
        + OpenChecks * tuning.openCheckMind;

    public int SleepWardCost()
    {
        int expiring = 0;
        foreach (var location in locations)
            if (location.IsOpenCheck && location.Stage >= tuning.checkLastStage) expiring++;
        return expiring * tuning.expiredCheckWard;
    }

    /// Pays the day's missed needs, ages open CHECKs, and starts the next day with its
    /// anomaly draw. The Ward must have been answered first.
    public void Sleep(System.Random rng)
    {
        RequirePhase(DayPhase.Night);
        if (!gaveTonight) throw new InvalidOperationException("The Ward screen comes before sleep.");

        if (!PayNight()) StartNextDay(rng);
        Slept?.Invoke(this);
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

    /// Returns true when paying the night ended the run.
    private bool PayNight()
    {
        if (Change(Stat.Hp, -SleepHpCost())) return true;
        if (Change(Stat.Mind, -SleepMindCost())) return true;
        return AgeChecks();
    }

    private void StartNextDay(System.Random rng)
    {
        day++;
        phase = DayPhase.Day;
        gaveTonight = false;
        Array.Clear(choresMet, 0, choresMet.Length);
        DrawAnomalies(rng);
    }

    /// Returns true when an open CHECK ending on its own ended the run.
    private bool AgeChecks()
    {
        foreach (var location in locations)
        {
            if (!location.IsOpenCheck) continue;
            if (location.Stage >= tuning.checkLastStage)
            {
                location.Clear();
                if (Change(Stat.Ward, -tuning.expiredCheckWard)) return true;
            }
            else
            {
                location.Advance();
            }
        }
        return false;
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

    /// Applies a change to one stat, clamped at 0. Returns true when it ended the run.
    private bool Change(Stat stat, int delta)
    {
        switch (stat)
        {
            case Stat.Hp: hp = Mathf.Max(0, hp + delta); break;
            case Stat.Mind: mind = Mathf.Max(0, mind + delta); break;
            case Stat.Ward: ward = Mathf.Max(0, ward + delta); break;
            default: throw new ArgumentOutOfRangeException(nameof(stat));
        }
        int value = stat == Stat.Hp ? hp : stat == Stat.Mind ? mind : ward;
        if (value > 0) return false;
        endedBy = stat;
        return true;
    }

    private void RequirePhase(DayPhase required)
    {
        if (IsOver) throw new InvalidOperationException($"The run is over ({endedBy} reached 0).");
        if (phase != required) throw new InvalidOperationException($"Only allowed during {required}.");
    }
}
