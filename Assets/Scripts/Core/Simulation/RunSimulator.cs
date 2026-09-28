using System.Collections.Generic;
using System.Linq;
using System.Text;

/// Plays whole runs of GameState with a strategy and reports the day each run ended
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

    /// The strategies the menu runs: an idle player, fixed offerings of 0, 1 and 2, and Balance.
    public static RunStrategy[] DefaultStrategies() => new RunStrategy[]
    {
        new FixedOfferingStrategy("Idle (no chores, gives nothing)", 0, doesChores: false),
        new FixedOfferingStrategy("Always gives nothing", 0),
        new FixedOfferingStrategy("Always gives 1", 1),
        new FixedOfferingStrategy("Always gives 2", 2),
        new BalanceStrategy(),
    };

    public RunResult PlayRun(RunStrategy strategy, System.Random rng)
    {
        var state = GameState.NewRun(tuning.loop);
        while (!state.IsOver && state.Day <= tuning.maxDays)
        {
            if (strategy.DoesChores) planner.PlayDay(state, rng);
            state.FileReport();
            var (hp, mind) = strategy.ChooseOffering(state);
            state.GiveToWard(hp, mind);
            if (state.IsOver) break;
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
            sb.AppendLine($"  end day min {ended[0]}, median {ended[ended.Count / 2]}, "
                + $"mean {ended.Average():0.0}, max {ended[ended.Count - 1]}");
        }
        sb.AppendLine($"  ended by HP {Count(results, Stat.Hp)}, MIND {Count(results, Stat.Mind)}, "
            + $"WARD {Count(results, Stat.Ward)}, unfinished after {tuning.maxDays} days {unfinished}");
        sb.Append("  runs ending per day (day x count): ");
        sb.Append(string.Join(" ", ended.GroupBy(d => d).Select(g => $"{g.Key}x{g.Count()}")));
        return sb.ToString();
    }

    private static int Count(List<RunResult> results, Stat stat) => results.Count(r => r.EndedBy == stat);
}
