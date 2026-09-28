using NUnit.Framework;
using UnityEngine;

/// The run simulator (task 7.2): every run ends, and a seed gives the same results.
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
        var strategy = new BalanceStrategy();
        var first = simulator.PlayRuns(strategy, 0);
        var second = simulator.PlayRuns(strategy, 0);
        CollectionAssert.AreEqual(first, second);
    }

    [Test]
    public void GiveNothing_NeverGivesPoints()
    {
        var state = GameState.NewRun(loop);
        Assert.AreEqual((0, 0), new FixedOfferingStrategy("nothing", 0).ChooseOffering(state));
    }
}
