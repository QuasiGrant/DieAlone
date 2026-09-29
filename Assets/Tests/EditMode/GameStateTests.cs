using System;
using NUnit.Framework;
using UnityEngine;

/// Rules of the daily loop (tasks 7.1 and 7.5, DailyLoop.md revision 6). Values come
/// from a fresh LoopTuning, so the tests follow the tuning defaults; tests that need a
/// sure anomaly set the odds to 1.
public class GameStateTests
{
    private LoopTuning tuning;

    [SetUp]
    public void SetUp() => tuning = ScriptableObject.CreateInstance<LoopTuning>();

    [TearDown]
    public void TearDown() => UnityEngine.Object.DestroyImmediate(tuning);

    private static readonly System.Random Rng = new System.Random(1);

    private static void MeetChores(GameState state)
    {
        state.MeetNeed(Need.Food);
        state.MeetNeed(Need.Water);
        state.MeetNeed(Need.Warmth);
        state.MeetNeed(Need.Social);
    }

    private static void ResolveAll(GameState state)
    {
        for (int i = 0; i < state.LocationCount; i++)
            if (state.GetLocation(i).IsOpenCheck) state.ResolveCheck(i);
    }

    private static void EndDay(GameState state, int hpGiven = 0, int mindGiven = 0)
    {
        state.FileReport();
        state.GiveToWard(hpGiven, mindGiven);
        state.Sleep(Rng);
    }

    private void SureAnomalies()
    {
        tuning.anomalyChanceStart = 1f;
        tuning.anomalyChanceCap = 1f;
    }

    [Test]
    public void NewRun_StartsFromTuning()
    {
        var state = GameState.NewRun(tuning);
        Assert.AreEqual(1, state.Day);
        Assert.AreEqual(DayPhase.Day, state.Phase);
        Assert.AreEqual(tuning.startHp, state.Hp);
        Assert.AreEqual(tuning.startMind, state.Mind);
        Assert.AreEqual(tuning.startWard, state.Ward);
        Assert.AreEqual(tuning.checkedLocations.Length, state.LocationCount);
        Assert.AreEqual(0, state.OpenChecks);
        Assert.IsFalse(state.IsOver);
    }

    [Test]
    public void NewRun_StartAboveCap_IsCapped()
    {
        tuning.startHp = tuning.statMax + 5;
        Assert.AreEqual(tuning.statMax, GameState.NewRun(tuning).Hp);
    }

    [Test]
    public void AnomalyChance_FollowsCurveAndCap()
    {
        Assert.AreEqual(0f, tuning.AnomalyChance(1));
        Assert.AreEqual(0.10f, tuning.AnomalyChance(2), 1e-5f);
        Assert.AreEqual(0.26f, tuning.AnomalyChance(10), 1e-5f);
        Assert.AreEqual(0.50f, tuning.AnomalyChance(40), 1e-5f);
    }

    [TestCase(1, 1)]
    [TestCase(7, 1)]
    [TestCase(8, 2)]
    [TestCase(14, 2)]
    [TestCase(15, 3)]
    [TestCase(21, 3)]
    [TestCase(22, 4)]
    public void Hunger_RisesEachWeek(int day, int hunger)
    {
        Assert.AreEqual(hunger, tuning.Hunger(day));
    }

    [Test]
    public void DayOne_IsAllSafeEvenWithSureOdds()
    {
        SureAnomalies();
        var state = GameState.NewRun(tuning);
        Assert.AreEqual(0, state.OpenChecks);
        Assert.IsTrue(state.IsNeedMet(Need.Safety));
    }

    [Test]
    public void AnomalyDraw_StopsAtMaxOpenChecks()
    {
        SureAnomalies();
        var state = GameState.NewRun(tuning);
        MeetChores(state);
        EndDay(state);
        Assert.AreEqual(2, state.Day);
        Assert.AreEqual(tuning.maxOpenChecks, state.OpenChecks);
        Assert.IsFalse(state.IsNeedMet(Need.Safety));
    }

    [Test]
    public void MissedNeeds_ArePaidAtSleep()
    {
        var state = GameState.NewRun(tuning);
        state.FileReport();
        Assert.AreEqual(tuning.foodMissHp + tuning.waterMissHp + tuning.warmthMissHp, state.SleepHpCost());
        Assert.AreEqual(tuning.socialMissMind, state.SleepMindCost());
        state.GiveToWard(0, 0);
        state.Sleep(Rng);
        Assert.AreEqual(tuning.startHp - 3, state.Hp);
        Assert.AreEqual(tuning.startMind - 1, state.Mind);
    }

    [Test]
    public void MetNeeds_CostNothingAndRestoreNothing()
    {
        var state = GameState.NewRun(tuning);
        MeetChores(state);
        Assert.IsTrue(state.IsNeedMet(Need.Food));
        EndDay(state, 1, 0);
        Assert.AreEqual(tuning.startHp - 1, state.Hp);
        Assert.AreEqual(tuning.startMind, state.Mind);
        Assert.AreEqual(tuning.startWard + 1 - tuning.Hunger(1), state.Ward);
    }

    [Test]
    public void Needs_ResetEachDay()
    {
        var state = GameState.NewRun(tuning);
        MeetChores(state);
        EndDay(state);
        Assert.IsFalse(state.IsNeedMet(Need.Food));
        Assert.IsFalse(state.IsNeedMet(Need.Social));
    }

    [Test]
    public void MeetNeed_SafetyDirectly_Throws()
    {
        var state = GameState.NewRun(tuning);
        Assert.Throws<ArgumentException>(() => state.MeetNeed(Need.Safety));
    }

    [TestCase(0, 0)]
    [TestCase(1, 0)]
    [TestCase(0, 1)]
    [TestCase(1, 1)]
    [TestCase(2, 1)]
    [TestCase(0, 3)]
    public void GiveToWard_EachPointBuysWardMinusHunger(int hpGiven, int mindGiven)
    {
        tuning.startWard = 6;
        var state = GameState.NewRun(tuning);
        state.FileReport();
        state.GiveToWard(hpGiven, mindGiven);
        Assert.AreEqual(tuning.startHp - hpGiven, state.Hp);
        Assert.AreEqual(tuning.startMind - mindGiven, state.Mind);
        int bought = (hpGiven + mindGiven) * tuning.wardPerPointGiven;
        Assert.AreEqual(tuning.startWard + bought - tuning.Hunger(1), state.Ward);
    }

    [Test]
    public void GiveToWard_MoreThanNightlyMax_Throws()
    {
        tuning.startWard = 1;
        var state = GameState.NewRun(tuning);
        state.FileReport();
        Assert.AreEqual(tuning.maxPointsGiven, state.MostPointsAccepted);
        Assert.Throws<ArgumentException>(() => state.GiveToWard(2, 2));
    }

    [Test]
    public void GiveToWard_PastWardCap_Throws()
    {
        var state = GameState.NewRun(tuning);
        state.FileReport();
        Assert.AreEqual(tuning.startWard, tuning.statMax);
        Assert.AreEqual(tuning.Hunger(1) / tuning.wardPerPointGiven, state.MostPointsAccepted);
        Assert.Throws<ArgumentException>(() => state.GiveToWard(2, 0));
        state.GiveToWard(1, 0);
        Assert.AreEqual(tuning.statMax, state.Ward);
    }

    [Test]
    public void GiveToWard_Twice_Throws()
    {
        var state = GameState.NewRun(tuning);
        state.FileReport();
        state.GiveToWard(0, 0);
        Assert.Throws<InvalidOperationException>(() => state.GiveToWard(0, 0));
    }

    [Test]
    public void GiveNothing_WardFallsByHunger()
    {
        var state = GameState.NewRun(tuning);
        for (int day = 1; day <= 8; day++)
        {
            MeetChores(state);
            int before = state.Ward;
            state.FileReport();
            state.GiveToWard(0, 0);
            Assert.AreEqual(before - tuning.Hunger(day), state.Ward, $"night {day}");
            state.Sleep(Rng);
        }
    }

    [Test]
    public void PhaseOrder_IsEnforced()
    {
        var state = GameState.NewRun(tuning);
        Assert.Throws<InvalidOperationException>(() => state.GiveToWard(0, 0));
        Assert.Throws<InvalidOperationException>(() => state.Sleep(Rng));
        state.FileReport();
        Assert.AreEqual(DayPhase.Night, state.Phase);
        Assert.Throws<InvalidOperationException>(() => state.MeetNeed(Need.Food));
        Assert.Throws<InvalidOperationException>(() => state.EndDayByEvent(true));
        Assert.Throws<InvalidOperationException>(() => state.Sleep(Rng));
        state.GiveToWard(0, 0);
        state.Sleep(Rng);
        Assert.AreEqual(DayPhase.Day, state.Phase);
        Assert.AreEqual(2, state.Day);
    }

    [Test]
    public void Bunk_NotOnNightOne()
    {
        var state = GameState.NewRun(tuning);
        state.FileReport();
        Assert.Throws<InvalidOperationException>(() => state.SleepInBunk(Rng));
    }

    [Test]
    public void Bunk_CountsAsGiveNothing()
    {
        var state = GameState.NewRun(tuning);
        MeetChores(state);
        EndDay(state);
        MeetChores(state);
        ResolveAll(state);
        int hp = state.Hp, mind = state.Mind, ward = state.Ward;
        state.FileReport();
        state.SleepInBunk(Rng);
        Assert.AreEqual(3, state.Day);
        Assert.AreEqual(hp, state.Hp);
        Assert.AreEqual(mind, state.Mind);
        Assert.AreEqual(ward - tuning.Hunger(2), state.Ward);
    }

    [Test]
    public void GivingLastHp_EndsRunOnHp()
    {
        tuning.startHp = 1;
        var state = GameState.NewRun(tuning);
        state.FileReport();
        state.GiveToWard(1, 0);
        Assert.IsTrue(state.IsOver);
        Assert.AreEqual(Stat.Hp, state.EndedBy);
        Assert.Throws<InvalidOperationException>(() => state.Sleep(Rng));
    }

    [Test]
    public void MindAtZeroFromNeeds_EndsRunOnMind()
    {
        tuning.startMind = 1;
        var state = GameState.NewRun(tuning);
        state.MeetNeed(Need.Food);
        state.MeetNeed(Need.Water);
        state.MeetNeed(Need.Warmth);
        EndDay(state, 1, 0);
        Assert.AreEqual(0, state.Mind);
        Assert.AreEqual(Stat.Mind, state.EndedBy);
        Assert.AreEqual(1, state.Day, "A run that ends at sleep does not start the next day.");
    }

    [Test]
    public void WardAtZero_EndsRunOnWard()
    {
        tuning.startWard = 1;
        var state = GameState.NewRun(tuning);
        state.FileReport();
        state.GiveToWard(0, 0);
        Assert.AreEqual(0, state.Ward);
        Assert.AreEqual(Stat.Ward, state.EndedBy);
    }

    [Test]
    public void ResolveCheck_MakesLocationSafe()
    {
        SureAnomalies();
        var state = GameState.NewRun(tuning);
        MeetChores(state);
        EndDay(state);
        ResolveAll(state);
        Assert.AreEqual(0, state.OpenChecks);
        Assert.IsTrue(state.IsNeedMet(Need.Safety));
        Assert.Throws<InvalidOperationException>(() => state.ResolveCheck(0));
    }

    [Test]
    public void OpenChecks_CostSafetyOnceAndNeverAge()
    {
        SureAnomalies();
        var state = GameState.NewRun(tuning);
        MeetChores(state);
        EndDay(state);
        Assert.AreEqual(2, state.OpenChecks);

        for (int night = 2; night <= 5; night++)
        {
            MeetChores(state);
            int mind = state.Mind, ward = state.Ward;
            state.FileReport();
            Assert.AreEqual(tuning.safetyMissMind, state.SleepMindCost(), "one Safety miss, however many are open");
            state.GiveToWard(0, 0);
            state.Sleep(Rng);
            Assert.AreEqual(mind - tuning.safetyMissMind, state.Mind);
            Assert.AreEqual(ward - tuning.Hunger(night), state.Ward, "no WARD cost for a CHECK left open");
        }
    }

    [Test]
    public void EventCost_EndsRunOnTheSpot()
    {
        var state = GameState.NewRun(tuning);
        state.ApplyEventCost(Stat.Mind, 2);
        Assert.AreEqual(tuning.startMind - 2, state.Mind);
        state.ApplyEventCost(Stat.Hp, tuning.startHp);
        Assert.IsTrue(state.IsOver);
        Assert.AreEqual(Stat.Hp, state.EndedBy);
        Assert.AreEqual(DayPhase.Day, state.Phase);
        Assert.Throws<InvalidOperationException>(() => state.ApplyEventCost(Stat.Ward, 1));
    }

    [Test]
    public void EventCost_Negative_Throws()
    {
        var state = GameState.NewRun(tuning);
        Assert.Throws<ArgumentException>(() => state.ApplyEventCost(Stat.Ward, -1));
    }

    [Test]
    public void EndDayByEvent_PaysNeeds()
    {
        var state = GameState.NewRun(tuning);
        state.EndDayByEvent(payUnmetNeeds: true);
        Assert.AreEqual(DayPhase.Night, state.Phase);
        Assert.IsFalse(state.NeedsWaived);
        state.GiveToWard(0, 0);
        state.Sleep(Rng);
        Assert.AreEqual(tuning.startHp - 3, state.Hp);
        Assert.AreEqual(tuning.startMind - 1, state.Mind);
    }

    [Test]
    public void EndDayByEvent_WaivesNeeds()
    {
        var state = GameState.NewRun(tuning);
        state.EndDayByEvent(payUnmetNeeds: false);
        Assert.IsTrue(state.NeedsWaived);
        Assert.AreEqual(0, state.SleepHpCost());
        Assert.AreEqual(0, state.SleepMindCost());
        state.GiveToWard(0, 0);
        state.Sleep(Rng);
        Assert.AreEqual(tuning.startHp, state.Hp);
        Assert.AreEqual(tuning.startMind, state.Mind);
        Assert.IsFalse(state.NeedsWaived, "the waiver lasts one day");
    }

    [Test]
    public void Restore_CappedPerDayCombinedAndAtStatMax()
    {
        var state = GameState.NewRun(tuning);
        Assert.AreEqual(0, state.Restore(Stat.Hp, 1), "HP is already at the cap");
        state.ApplyEventCost(Stat.Hp, 3);
        state.ApplyEventCost(Stat.Mind, 3);
        Assert.AreEqual(tuning.recoveryCapPerDay, state.Restore(Stat.Hp, 5));
        Assert.AreEqual(0, state.Restore(Stat.Mind, 1), "the cap is for all stats together");
        Assert.AreEqual(tuning.startHp - 3 + tuning.recoveryCapPerDay, state.Hp);

        MeetChores(state);
        state.FileReport();
        state.GiveToWard(0, 0);
        state.Sleep(Rng);
        Assert.AreEqual(tuning.recoveryCapPerDay, state.Restore(Stat.Mind, 1), "the cap resets each day");
    }

    [Test]
    public void NullTuning_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => GameState.NewRun(null));
    }
}
