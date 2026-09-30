using UnityEngine;

/// Daylight fill inside a room (the cabin): a no-shadow light under the ceiling so its planks read by day.
/// Colour and strength come from the active LookTuning (interiorFillColor, interiorFillIntensity); an intensity of 0,
/// as in the night look, switches the light off.
[RequireComponent(typeof(Light))]
public class InteriorFill : MonoBehaviour
{
    [SerializeField] private LookTuning tuning;

    private Light lightSource;

    private void Awake() => lightSource = GetComponent<Light>();
    private void OnEnable() => Apply();
    private void Update() => Apply();

    private void Apply()
    {
        var t = LookOverride.Resolve(tuning);
        if (t == null || lightSource == null) return;
        lightSource.enabled = t.interiorFillIntensity > 0f;
        lightSource.color = t.interiorFillColor;
        lightSource.intensity = t.interiorFillIntensity;
    }
}
