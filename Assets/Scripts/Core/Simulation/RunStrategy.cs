/// A simulated player: whether they do chores, and what they give the Ward each night.
public abstract class RunStrategy
{
    public abstract string Name { get; }
    public virtual bool DoesChores => true;

    /// Points of HP and MIND to give tonight. (0, 0) is Give nothing.
    public abstract (int hp, int mind) ChooseOffering(GameState state);
}
