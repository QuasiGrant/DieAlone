/// How a simulated player spends the day.
/// Idle does nothing and sleeps in the bunk whenever it may. Typical meets needs and
/// resolves CHECKs by the odds in SimulatorTuning. Perfect meets every need and
/// resolves every CHECK.
public enum PlayerStyle { Idle, Typical, Perfect }

/// A simulated player: how they play the day, whether they take recovery, and what
/// they give the Ward each night.
public abstract class RunStrategy
{
    protected RunStrategy(string name, PlayerStyle style, bool seeksRecovery)
    {
        Name = name;
        Style = style;
        SeeksRecovery = seeksRecovery;
    }

    public string Name { get; }
    public PlayerStyle Style { get; }

    /// Takes all the recovery the day offers (SimulatorTuning.recoveryOffered), onto the lowest stat.
    public bool SeeksRecovery { get; }

    /// Points of HP and MIND to give tonight. (0, 0) is Give nothing.
    public abstract (int hp, int mind) ChooseOffering(GameState state);
}
