using System.Collections.Generic;
using System.Linq;
using System.Text;

/// Plays whole runs of GameState with a strategy and reports the night each run ended
/// and which stat ended it. Deterministic for a given seed.
public class RunSimulator
{
    public struct RunResult
    {
        public int EndDay;
        public Stat EndedBy;
    }

    private readonly SimulatorTuning tuning;
    private readonly DayPlanner planner;

    public RunSimulator(SimulatorTuning tuning)
    {
        this.tuning = tuning;
        planner = new DayPlanner(tuning);
    }

    /// The strategies the menu runs: the DailyLoop.md 4.5 play styles, the idle and
    /// typical players, and the longest runs with and without recovery.
    public static RunStrategy[] DefaultStrategies() => new RunStrategy[]
    {
        new FixedOfferingStrategy("Idle (no chores, bunk every night it may)", 0, PlayerStyle.Idle),
        new FixedOfferingStrategy("Perfect needs, gives nothing", 0, PlayerStyle.Perfect),
        new FixedOfferingStrategy("Perfect needs, feeds everything", int.MaxValue, PlayerStyle.Perfect),
        new BalanceStrategy("Perfect needs, balance, no recovery", PlayerStyle.Perfect),
        new BalanceStrategy("Perfect needs, balance, full recovery", PlayerStyle.Perfect, seeksRecovery: true),
        new FixedOfferingStrategy("Typical, gives 1", 1, PlayerStyle.Typical),
        new BalanceStrategy("Typical, balance", PlayerStyle.Typical),
        new BalanceStrategy("Typical, balance, full recovery", PlayerStyle.Typical, seeksRecovery: true),
    };

    public RunResult PlayRun(RunStrategy strategy, System.Random rng)
    {
        var state = GameState.NewRun(tuning.loop);
        while (!state.IsOver && state.Day <= tuning.maxDays)
        {
            planner.PlayDay(state, strategy.Style, rng);
            if (state.IsOver) break;

            if (strategy.Style == PlayerStyle.Idle && state.Day >= tuning.loop.firstBunkNight)
            {
                state.SleepInBunk(rng);
                continue;
            }

            // Recovery comes before the Ward when a stat has room, else after the hunger
            // (RESTORE events happen by day or night; the cap is per day).
            bool recovered = strategy.SeeksRecovery
                && state.Restore(LowestAfterSleep(state), tuning.recoveryOffered) > 0;
            var (hp, mind) = strategy.ChooseOffering(state);
            state.GiveToWard(hp, mind);
            if (state.IsOver) break;
            if (strategy.SeeksRecovery && !recovered) state.Restore(LowestAfterSleep(state), tuning.recoveryOffered);
            state.Sleep(rng);
        }
        return new RunResult { EndDay = state.Day, EndedBy = state.EndedBy };
    }

    public List<RunResult> PlayRuns(RunStrategy strategy, int strategyIndex)
    {
        var rng = new System.Random(tuning.seed + strategyIndex);
        var results = new List<RunResult>(tuning.runsPerStrategy);
        for (int i = 0; i < tuning.runsPerStrategy; i++) results.Add(PlayRun(strategy, rng));
        return results;
    }

    public string Summarize(RunStrategy strategy, List<RunResult> results)
    {
        var ended = results.Where(r => r.EndedBy != Stat.None).Select(r => r.EndDay).OrderBy(d => d).ToList();
        int unfinished = results.Count - ended.Count;
        var sb = new StringBuilder();
        sb.AppendLine($"{strategy.Name}: {results.Count} runs");
        if (ended.Count > 0)
        {
            sb.AppendLine($"  end night min {ended[0]}, median {ended[ended.Count / 2]}, "
                + $"mean {ended.Average():0.0}, max {ended[ended.Count - 1]}");
        }
        sb.AppendLine($"  ended by HP {Count(results, Stat.Hp)}, MIND {Count(results, Stat.Mind)}, "
            + $"WARD {Count(results, Stat.Ward)}, unfinished after {tuning.maxDays} days {unfinished}");
        sb.Append("  runs ending per night (night x count): ");
        sb.Append(string.Join(" ", ended.GroupBy(d => d).Select(g => $"{g.Key}x{g.Count()}")));
        return sb.ToString();
    }

    /// The stat that will be lowest after tonight's sleep costs.
    private static Stat LowestAfterSleep(GameState state)
    {
        int hp = state.Hp - state.SleepHpCost();
        int mind = state.Mind - state.SleepMindCost();
        if (hp <= mind && hp <= state.Ward) return Stat.Hp;
        return mind <= state.Ward ? Stat.Mind : Stat.Ward;
    }

    private static int Count(List<RunResult> results, Stat stat) => results.Count(r => r.EndedBy == stat);
}
