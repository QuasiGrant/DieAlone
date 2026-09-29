using UnityEngine;

/// A practical light (lamp, lantern, stove, fire pit) that follows the look. Its brightness
/// comes from LookTuning by kind; in a daylight look it drops to the day share, or goes off
/// if it is a night-only light. Brightness is carried in the light colour, so a component that
/// animates the intensity (FirePit's flicker) keeps working on top of it.
[RequireComponent(typeof(Light))]
public class PracticalLight : MonoBehaviour
{
    public enum Kind { FirePit, Stove, Lamp, Lantern }
    public enum ByDay { Full, Dimmed, Off }

    [SerializeField] private LookTuning tuning;
    [SerializeField] private Kind kind = Kind.Lamp;
    [Tooltip("What the light does in a daylight look (LookSlice.md 4).")]
    [SerializeField] private ByDay byDay = ByDay.Dimmed;

    private Light lightSource;

    private void Awake() => lightSource = GetComponent<Light>();
    private void OnEnable() => Apply();
    private void Update() => Apply();

    /// Night brightness for a kind, relative to the fire pit, so the fire pit stays the brightest.
    private static float Strength(LookTuning t, Kind k)
    {
        float top = Mathf.Max(t.firePitIntensity, 0.001f);
        switch (k)
        {
            case Kind.FirePit: return 1f;
            case Kind.Stove: return t.stoveIntensity / top;
            case Kind.Lantern: return t.lanternIntensity / top;
            default: return t.lampIntensity / top;
        }
    }

    private void Apply()
    {
        var t = LookOverride.Resolve(tuning);
        if (t == null || lightSource == null) return;
        bool day = LookOverride.ForceSunset;
        lightSource.enabled = !(day && byDay == ByDay.Off);
        float scale = Strength(t, kind) * (day && byDay == ByDay.Dimmed ? t.practicalDayScale : 1f);
        lightSource.color = t.practicalColor * scale;
    }
}
