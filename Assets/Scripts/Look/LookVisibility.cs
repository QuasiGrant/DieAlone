using UnityEngine;

/// Shows its target objects only in some looks: the stand-in fire at night and on day two (never on day one by day,
/// DECISIONS 2026-09-29), falling ash on day two, the cab's lit windows at night. A daylight look is day two when its
/// LookTuning is the day-two asset; every other daylight look counts as day one. Lives on an object that stays active.
public class LookVisibility : MonoBehaviour
{
    public enum Show { Night, DayOne, DayTwo, NightAndDayTwo }

    [SerializeField] private Show show = Show.Night;
    [Tooltip("The day-two look asset, to tell day two from day one.")]
    [SerializeField] private LookTuning dayTwo;
    [SerializeField] private GameObject[] targets;

    private void OnEnable() => Apply();
    private void Update() => Apply();

    private void Apply()
    {
        bool day = LookOverride.ForceSunset;
        bool isDayTwo = day && dayTwo != null && LookOverride.Tuning == dayTwo;
        bool isDayOne = day && !isDayTwo;
        bool on = show switch
        {
            Show.Night => !day,
            Show.DayOne => isDayOne,
            Show.DayTwo => isDayTwo,
            _ => !day || isDayTwo,
        };
        foreach (var t in targets) if (t != null && t.activeSelf != on) t.SetActive(on);
    }
}
