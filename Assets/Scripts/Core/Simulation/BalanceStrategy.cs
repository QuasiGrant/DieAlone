/// Picks the offering that leaves the lowest stat highest after tonight's sleep costs.
/// Ties go to the offering that keeps the most points in total.
public class BalanceStrategy : RunStrategy
{
    public override string Name => "Balance";

    public override (int hp, int mind) ChooseOffering(GameState state)
    {
        LoopTuning tuning = state.Tuning;
        int hpAfterNeeds = state.Hp - state.SleepHpCost();
        int mindAfterNeeds = state.Mind - state.SleepMindCost();
        int wardAfterNeeds = state.Ward - state.SleepWardCost();

        (int hp, int mind) best = (0, 0);
        int bestMin = int.MinValue, bestSum = int.MinValue;
        for (int hpGiven = 0; hpGiven <= tuning.MaxPointsGiven; hpGiven++)
        {
            for (int mindGiven = 0; hpGiven + mindGiven <= tuning.MaxPointsGiven; mindGiven++)
            {
                if (hpGiven > state.Hp || mindGiven > state.Mind) continue;
                int hp = hpAfterNeeds - hpGiven;
                int mind = mindAfterNeeds - mindGiven;
                int ward = wardAfterNeeds + tuning.wardChangeByPointsGiven[hpGiven + mindGiven];
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
