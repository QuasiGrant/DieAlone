using UnityEngine;

/// Applies the scene-side parts of the look from LookTuning every frame:
/// distance fog and the matching sky color. Lives on the Game object.
/// Add future scene-level look settings (ambient, exposure) here the same way.
public class LookEnvironment : MonoBehaviour
{
    [SerializeField] private LookTuning tuning;
    [SerializeField] private Camera targetCamera;

    private void OnEnable() => Apply();
    private void Update() => Apply();

    private void Apply()
    {
        if (tuning == null) return;
        RenderSettings.fog = tuning.fogEnabled;
        if (tuning.fogEnabled)
        {
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = tuning.fogColor;
            RenderSettings.fogStartDistance = tuning.fogStart;
            RenderSettings.fogEndDistance = Mathf.Max(tuning.fogEnd, tuning.fogStart + 0.1f);
        }
        var cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam != null && tuning.fogEnabled) cam.backgroundColor = tuning.fogColor;
    }
}
