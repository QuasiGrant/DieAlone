using NUnit.Framework;
using UnityEngine;

/// The run simulator (tasks 7.2 and 7.5): every run ends, a seed gives the same
/// results, and the longest runs match DailyLoop.md 4 (19 nights, 26 with recovery).
public class RunSimulatorTests
{
    private LoopTuning loop;
    private SimulatorTuning sim;

    [SetUp]
    public void SetUp()
    {
        loop = ScriptableObject.CreateInstance<LoopTuning>();
        sim = ScriptableObject.CreateInstance<SimulatorTuning>();
        sim.loop = loop;
        sim.runsPerStrategy = 100;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(sim);
        Object.DestroyImmediate(loop);
    }

    [Test]
    public void EveryStrategy_EveryRunEnds()
    {
        var simulator = new RunSimulator(sim);
        var strategies = RunSimulator.DefaultStrategies();
        for (int i = 0; i < strategies.Length; i++)
            foreach (var result in simulator.PlayRuns(strategies[i], i))
                Assert.AreNotEqual(Stat.None, result.EndedBy, strategies[i].Name);
    }

    [Test]
    public void SameSeed_SameResults()
    {
        var simulator = new RunSimulator(sim);
        var strategy = new BalanceStrategy("balance", PlayerStyle.Typical);
        var first = simulator.PlayRuns(strategy, 0);
        var second = simulator.PlayRuns(strategy, 0);
        CollectionAssert.AreEqual(first, second);
    }

    [Test]
    public void GiveNothing_NeverGivesPoints()
    {
        var state = GameState.NewRun(loop);
        Assert.AreEqual((0, 0), new FixedOfferingStrategy("nothing", 0, PlayerStyle.Perfect).ChooseOffering(state));
    }

    [Test]
    public void LongestRun_WithoutRecovery_EndsOnNight19()
    {
        var result = new RunSimulator(sim).PlayRun(
            new BalanceStrategy("perfect", PlayerStyle.Perfect), new System.Random(1));
        Assert.AreEqual(19, result.EndDay);
    }

    [Test]
    public void LongestRun_WithFullRecovery_EndsOnNight26()
    {
        var result = new RunSimulator(sim).PlayRun(
            new BalanceStrategy("perfect", PlayerStyle.Perfect, seeksRecovery: true), new System.Random(1));
        Assert.AreEqual(26, result.EndDay);
    }

    [Test]
    public void PerfectGiveNothing_WardGoneByNight10()
    {
        var result = new RunSimulator(sim).PlayRun(
            new FixedOfferingStrategy("nothing", 0, PlayerStyle.Perfect), new System.Random(1));
        Assert.AreEqual(Stat.Ward, result.EndedBy);
        Assert.AreEqual(10, result.EndDay);
    }
}
