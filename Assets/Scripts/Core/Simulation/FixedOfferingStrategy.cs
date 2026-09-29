/// Gives the same number of points every night, or as many as the Ward accepts if
/// fewer, one at a time from whichever of HP and MIND is higher. Zero points is the
/// always-Give-nothing player.
public class FixedOfferingStrategy : RunStrategy
{
    private readonly int points;

    public FixedOfferingStrategy(string name, int points, PlayerStyle style, bool seeksRecovery = false)
        : base(name, style, seeksRecovery) => this.points = points;

    public override (int hp, int mind) ChooseOffering(GameState state)
    {
        int count = System.Math.Min(points, System.Math.Min(state.MostPointsAccepted, state.Hp + state.Mind));
        int hp = 0, mind = 0;
        for (int i = 0; i < count; i++)
        {
            if (state.Hp - hp >= state.Mind - mind) hp++;
            else mind++;
        }
        return (hp, mind);
    }
}
