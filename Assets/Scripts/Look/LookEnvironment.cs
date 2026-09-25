using UnityEngine;

/// Applies the scene-side parts of the look every frame from LookTuning: fog, the
/// camera background, and for sunset scenes the gradient sky. Lives on the Game object.
/// Each scene picks a fog set; every number for both sets lives in LookTuning so
/// Play-mode edits persist.
public class LookEnvironment : MonoBehaviour
{
    public enum FogSet { Night, Sunset }

    [SerializeField] private LookTuning tuning;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private FogSet fogSet = FogSet.Night;
    [Tooltip("Material using DieAlone/SkyGradient. Used when the fog set is Sunset.")]
    [SerializeField] private Material skyMaterial;

    private static readonly int TopId = Shader.PropertyToID("_TopColor");
    private static readonly int HorizonId = Shader.PropertyToID("_HorizonColor");
    private static readonly int GroundId = Shader.PropertyToID("_GroundColor");
    private static readonly int SunGlowId = Shader.PropertyToID("_SunGlowColor");
    private static readonly int SunSizeId = Shader.PropertyToID("_SunGlowSize");
    private static readonly int SunDirId = Shader.PropertyToID("_SunDir");

    private void OnEnable() => Apply();
    private void Update() => Apply();

    private void Apply()
    {
        if (tuning == null) return;
        bool sunset = fogSet == FogSet.Sunset;
        Color color = sunset ? tuning.sunsetFogColor : tuning.fogColor;
        float start = sunset ? tuning.sunsetFogStart : tuning.fogStart;
        float end = sunset ? tuning.sunsetFogEnd : tuning.fogEnd;

        RenderSettings.fog = tuning.fogEnabled;
        if (tuning.fogEnabled)
        {
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = color;
            RenderSettings.fogStartDistance = start;
            RenderSettings.fogEndDistance = Mathf.Max(end, start + 0.1f);
        }

        var cam = targetCamera != null ? targetCamera : Camera.main;
        if (sunset && skyMaterial != null)
        {
            skyMaterial.SetColor(TopId, tuning.skyTop);
            skyMaterial.SetColor(HorizonId, tuning.skyHorizon);
            skyMaterial.SetColor(GroundId, tuning.skyGround);
            skyMaterial.SetColor(SunGlowId, tuning.sunGlowColor);
            skyMaterial.SetFloat(SunSizeId, tuning.sunGlowSize);
            var sun = RenderSettings.sun;
            skyMaterial.SetVector(SunDirId, sun != null ? (Vector4)sun.transform.forward : new Vector4(0f, -0.25f, 1f, 0f));
            RenderSettings.skybox = skyMaterial;
            if (cam != null) cam.clearFlags = CameraClearFlags.Skybox;
        }
        else if (cam != null)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            if (tuning.fogEnabled) cam.backgroundColor = color;
        }
    }
}
