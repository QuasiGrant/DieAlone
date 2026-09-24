using UnityEngine;

/// Applies the scene-side parts of the look every frame: distance fog and the
/// matching sky color. Lives on the Game object. Values come from LookTuning unless
/// this scene ticks Override Fog, which is how a sunset scene keeps a warm fog while
/// a night scene keeps a dark one from the same tuning asset.
public class LookEnvironment : MonoBehaviour
{
    [SerializeField] private LookTuning tuning;
    [SerializeField] private Camera targetCamera;

    [Header("Per-scene override")]
    [Tooltip("Use the fog values below for this scene instead of LookTuning.")]
    [SerializeField] private bool overrideFog;
    [SerializeField] private Color fogColor = new Color(0.02f, 0.03f, 0.05f);
    [SerializeField] private float fogStart = 6f;
    [SerializeField] private float fogEnd = 40f;

    private void OnEnable() => Apply();
    private void Update() => Apply();

    private void Apply()
    {
        if (tuning == null) return;
        bool enabled = tuning.fogEnabled;
        Color color = overrideFog ? fogColor : tuning.fogColor;
        float start = overrideFog ? fogStart : tuning.fogStart;
        float end = overrideFog ? fogEnd : tuning.fogEnd;

        RenderSettings.fog = enabled;
        if (enabled)
        {
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = color;
            RenderSettings.fogStartDistance = start;
            RenderSettings.fogEndDistance = Mathf.Max(end, start + 0.1f);
        }
        var cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam != null && enabled) cam.backgroundColor = color;
    }
}
