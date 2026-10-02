using System;

/// Raised each time the keeper wakes in the cabin (the day system calls Raise at every dawn; the dev menu may too).
/// Things that reset at every wake subscribe to Woke: the cabin door stands open (Door.startOpen, CampLayout.md 4.5).
public static class WakeSignal
{
    public static event Action Woke;

    public static void Raise() => Woke?.Invoke();
}
