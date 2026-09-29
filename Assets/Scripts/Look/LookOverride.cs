using UnityEngine;

/// A look that temporarily replaces the one every look component has assigned, without
/// touching any asset or scene. Only the dev look preview sets it; when nothing is set,
/// each component uses its own LookTuning exactly as before.
public static class LookOverride
{
    /// Replaces every component's own LookTuning while set.
    public static LookTuning Tuning { get; private set; }

    /// When true, LookEnvironment uses the Sunset fog set and this sky, whatever the scene picked.
    public static bool ForceSunset { get; private set; }
    public static Material SkyMaterial { get; private set; }

    public static LookTuning Resolve(LookTuning own) => Tuning != null ? Tuning : own;

    public static void Set(LookTuning tuning, bool forceSunset, Material skyMaterial)
    {
        Tuning = tuning;
        ForceSunset = forceSunset;
        SkyMaterial = skyMaterial;
    }

    public static void Clear() => Set(null, false, null);
}
