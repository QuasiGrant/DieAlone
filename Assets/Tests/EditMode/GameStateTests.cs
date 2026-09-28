using System;
using NUnit.Framework;
using UnityEngine;

/// Rules of the daily loop (task 7.1). Values come from a fresh LoopTuning, so the
/// tests follow the tuning defaults; tests that need a sure anomaly set the odds to 1.
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

    private static void EndDay(GameState state, int hpGiven = 1, int mindGiven = 0)
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
    public void AnomalyChance_FollowsCurveAndCap()
    {
        Assert.AreEqual(0f, tuning.AnomalyChance(1));
        Assert.AreEqual(0.10f, tuning.AnomalyChance(2), 1e-5f);
        Assert.AreEqual(0.26f, tuning.AnomalyChance(10), 1e-5f);
        Assert.AreEqual(0.50f, tuning.AnomalyChance(40), 1e-5f);
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

    [TestCase(0, 0, -1)]
    [TestCase(1, 0, 0)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(2, 0, 1)]
    public void GiveToWard_UsesRateTable(int hpGiven, int mindGiven, int wardChange)
    {
        var state = GameState.NewRun(tuning);
        state.FileReport();
        state.GiveToWard(hpGiven, mindGiven);
        Assert.AreEqual(tuning.startHp - hpGiven, state.Hp);
        Assert.AreEqual(tuning.startMind - mindGiven, state.Mind);
        Assert.AreEqual(tuning.startWard + wardChange, state.Ward);
    }

    [Test]
    public void GiveToWard_MoreThanMax_Throws()
    {
        var state = GameState.NewRun(tuning);
        state.FileReport();
        Assert.Throws<ArgumentException>(() => state.GiveToWard(2, 1));
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
    public void PhaseOrder_IsEnforced()
    {
        var state = GameState.NewRun(tuning);
        Assert.Throws<InvalidOperationException>(() => state.GiveToWard(0, 0));
        Assert.Throws<InvalidOperationException>(() => state.Sleep(Rng));
        state.FileReport();
        Assert.AreEqual(DayPhase.Night, state.Phase);
        Assert.Throws<InvalidOperationException>(() => state.MeetNeed(Need.Food));
        Assert.Throws<InvalidOperationException>(() => state.Sleep(Rng));
        state.GiveToWard(0, 0);
        state.Sleep(Rng);
        Assert.AreEqual(DayPhase.Day, state.Phase);
        Assert.AreEqual(2, state.Day);
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
        for (int i = 0; i < state.LocationCount; i++)
            if (state.GetLocation(i).IsOpenCheck) state.ResolveCheck(i);
        Assert.AreEqual(0, state.OpenChecks);
        Assert.IsTrue(state.IsNeedMet(Need.Safety));
        Assert.Throws<InvalidOperationException>(() => state.ResolveCheck(0));
    }

    [Test]
    public void OpenCheck_CostsMindEachNightAndExpiresOnWard()
    {
        SureAnomalies();
        tuning.maxOpenChecks = 1;
        var state = GameState.NewRun(tuning);
        MeetChores(state);
        EndDay(state);

        int index = -1;
        for (int i = 0; i < state.LocationCount; i++) if (state.GetLocation(i).IsOpenCheck) index = i;
        Assert.AreEqual(1, state.GetLocation(index).Stage);

        for (int stage = 1; stage <= tuning.checkLastStage; stage++)
        {
            Assert.AreEqual(stage, state.GetLocation(index).Stage);
            int mindBefore = state.Mind;
            int wardBefore = state.Ward;
            MeetChores(state);
            state.FileReport();
            state.GiveToWard(1, 0);
            int wardAfterGift = state.Ward;
            state.Sleep(Rng);
            Assert.AreEqual(mindBefore - tuning.openCheckMind, state.Mind);
            bool expired = stage == tuning.checkLastStage;
            Assert.AreEqual(expired ? wardAfterGift - tuning.expiredCheckWard : wardBefore, state.Ward);
        }
        // The expired CHECK cleared; with sure odds a new one is drawn, so check the count stays capped.
        Assert.AreEqual(1, state.OpenChecks);
    }

    [Test]
    public void NullTuning_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => GameState.NewRun(null));
    }
}
