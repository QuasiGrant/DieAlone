/// The simulated player's day, from the tower check to its end: rounds by the player's
/// style, then the report, or an event that ends the day early. There is no time
/// budget (DECISIONS 2026-09-29). Events stay generic stand-ins until event content
/// exists: each CHECK left open at day end costs its own points through the event hook.
public class DayPlanner
{
    private static readonly Stat[] Stats = { Stat.Hp, Stat.Mind, Stat.Ward };
    private readonly SimulatorTuning tuning;

    public DayPlanner(SimulatorTuning tuning) => this.tuning = tuning;

    public void PlayDay(GameState state, PlayerStyle style, System.Random rng)
    {
        ResolveChecks(state, style, rng);

        bool eventsLive = state.Day >= state.Tuning.firstAnomalyDay;
        if (style == PlayerStyle.Typical && eventsLive && rng.NextDouble() < tuning.dayEndingEventChance)
        {
            state.EndDayByEvent(rng.NextDouble() < tuning.dayEndPaysNeedsChance);
            state.ApplyEventCost(RandomStat(rng), tuning.dayEndingEventCost);
        }
        else
        {
            MeetNeeds(state, style, rng);
            state.FileReport();
        }
        PayOpenChecks(state, rng);
    }

    private void ResolveChecks(GameState state, PlayerStyle style, System.Random rng)
    {
        if (style == PlayerStyle.Idle) return;
        for (int i = 0; i < state.LocationCount; i++)
        {
            if (!state.GetLocation(i).IsOpenCheck) continue;
            if (style == PlayerStyle.Perfect || rng.NextDouble() < tuning.resolveChance) state.ResolveCheck(i);
        }
    }

    private void MeetNeeds(GameState state, PlayerStyle style, System.Random rng)
    {
        if (style == PlayerStyle.Idle) return;
        TryNeed(state, Need.Food, tuning.choreChance, style, rng);
        TryNeed(state, Need.Water, tuning.choreChance, style, rng);
        TryNeed(state, Need.Warmth, tuning.choreChance, style, rng);
        TryNeed(state, Need.Social, tuning.socialChance, style, rng);
    }

    private static void TryNeed(GameState state, Need need, float chance, PlayerStyle style, System.Random rng)
    {
        if (style == PlayerStyle.Perfect || rng.NextDouble() < chance) state.MeetNeed(need);
    }

    private void PayOpenChecks(GameState state, System.Random rng)
    {
        int open = state.OpenChecks;
        for (int i = 0; i < open && !state.IsOver; i++)
            state.ApplyEventCost(RandomStat(rng), tuning.unresolvedCheckCost);
    }

    private static Stat RandomStat(System.Random rng) => Stats[rng.Next(Stats.Length)];
}
