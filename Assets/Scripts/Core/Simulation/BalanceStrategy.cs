/// Picks the offering that leaves the lowest stat highest after tonight's hunger and
/// sleep costs. Ties go to the offering that keeps the most points in total.
public class BalanceStrategy : RunStrategy
{
    public BalanceStrategy(string name, PlayerStyle style, bool seeksRecovery = false)
        : base(name, style, seeksRecovery) { }

    public override (int hp, int mind) ChooseOffering(GameState state)
    {
        LoopTuning tuning = state.Tuning;
        int hpAfterNeeds = state.Hp - state.SleepHpCost();
        int mindAfterNeeds = state.Mind - state.SleepMindCost();
        int most = state.MostPointsAccepted;

        (int hp, int mind) best = (0, 0);
        int bestMin = int.MinValue, bestSum = int.MinValue;
        for (int hpGiven = 0; hpGiven <= most; hpGiven++)
        {
            for (int mindGiven = 0; hpGiven + mindGiven <= most; mindGiven++)
            {
                if (hpGiven > state.Hp || mindGiven > state.Mind) continue;
                int hp = hpAfterNeeds - hpGiven;
                int mind = mindAfterNeeds - mindGiven;
                int ward = state.Ward + (hpGiven + mindGiven) * tuning.wardPerPointGiven - state.Hunger;
                int min = System.Math.Min(hp, System.Math.Min(mind, ward));
                int sum = hp + mind + ward;
                if (min > bestMin || (min == bestMin && sum > bestSum))
                {
                    best = (hpGiven, mindGiven);
                    bestMin = min;
                    bestSum = sum;
                }
            }
        }
        return best;
    }
}
