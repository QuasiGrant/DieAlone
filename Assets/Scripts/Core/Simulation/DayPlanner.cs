using System.Collections.Generic;

/// The simulated player's day: spends the free duty seconds greedily, open CHECKs
/// first (oldest stage first), then Water, Warmth, Food, Social. An action is taken
/// only if its whole cost fits in the time left.
public class DayPlanner
{
    private readonly SimulatorTuning tuning;

    public DayPlanner(SimulatorTuning tuning) => this.tuning = tuning;

    public void PlayDay(GameState state, System.Random rng)
    {
        int timeLeft = tuning.freeSeconds;

        var checks = new List<int>();
        for (int i = 0; i < state.LocationCount; i++)
            if (state.GetLocation(i).IsOpenCheck) checks.Add(i);
        checks.Sort((a, b) => state.GetLocation(b).Stage.CompareTo(state.GetLocation(a).Stage));
        foreach (int index in checks)
        {
            int cost = tuning.ResolveCost(state.GetLocation(index).Stage);
            if (cost > timeLeft) continue;
            timeLeft -= cost;
            state.ResolveCheck(index);
        }

        TryChore(state, Need.Water, tuning.waterSeconds, ref timeLeft);
        TryChore(state, Need.Warmth, tuning.warmthSeconds, ref timeLeft);
        TryFood(state, rng, ref timeLeft);
        TryChore(state, Need.Social, tuning.socialSeconds, ref timeLeft);
    }

    private static void TryChore(GameState state, Need need, int cost, ref int timeLeft)
    {
        if (cost > timeLeft) return;
        timeLeft -= cost;
        state.MeetNeed(need);
    }

    private void TryFood(GameState state, System.Random rng, ref int timeLeft)
    {
        if (tuning.forageSeconds <= timeLeft)
        {
            timeLeft -= tuning.forageSeconds;
            if (rng.NextDouble() < tuning.forageChance) { state.MeetNeed(Need.Food); return; }
            if (tuning.forageRetrySeconds <= timeLeft)
            {
                timeLeft -= tuning.forageRetrySeconds;
                if (rng.NextDouble() < tuning.forageChance) { state.MeetNeed(Need.Food); return; }
            }
        }
        TryChore(state, Need.Food, tuning.storeSeconds, ref timeLeft);
    }
}
