using UnityEngine;

/// The kerosene lamp the keeper carries at night (DECISIONS 2026-09-30): a warm point light, no beam, on only in the night look,
/// with no toggle and no fuel. Scares dim it or put it out through SetStrength. Numbers come from PlayerTuning (Night lamp).
[RequireComponent(typeof(Light))]
public class NightLamp : MonoBehaviour
{
    [SerializeField] private PlayerTuning tuning;

    private Light lampLight;
    private float strength = 1f;

    /// Share of the lamp's full brightness, 0 (out) to 1 (full). Scares set it; nothing restores it but a later call.
    public float Strength => strength;

    /// For scares: 1 is the full lamp, 0 puts it out, anything between dims it. The lamp still only shows at night.
    public void SetStrength(float share) => strength = Mathf.Clamp01(share);

    private void Awake()
    {
        lampLight = GetComponent<Light>();
        lampLight.type = LightType.Point;
        lampLight.shadows = LightShadows.None;
    }

    private void OnEnable() => Apply();
    private void Update() => Apply();

    private void Apply()
    {
        bool night = !LookOverride.ForceSunset;   // the daylight looks force the sunset lighting; every other look is night
        lampLight.color = tuning.lampColor;
        lampLight.range = tuning.lampRange;
        lampLight.intensity = tuning.lampIntensity * strength;
        lampLight.enabled = night && strength > 0f;
    }
}
