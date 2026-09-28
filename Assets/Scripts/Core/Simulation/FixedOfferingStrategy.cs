/// Gives the same number of points every night, one at a time from whichever of HP
/// and MIND is higher. Zero points is the always-Give-nothing player.
public class FixedOfferingStrategy : RunStrategy
{
    private readonly string name;
    private readonly int points;
    private readonly bool doesChores;

    public FixedOfferingStrategy(string name, int points, bool doesChores = true)
    {
        this.name = name;
        this.points = points;
        this.doesChores = doesChores;
    }

    public override string Name => name;
    public override bool DoesChores => doesChores;

    public override (int hp, int mind) ChooseOffering(GameState state)
    {
        int hp = 0, mind = 0;
        for (int i = 0; i < points; i++)
        {
            if (state.Hp - hp >= state.Mind - mind) hp++;
            else mind++;
        }
        return (hp, mind);
    }
}
