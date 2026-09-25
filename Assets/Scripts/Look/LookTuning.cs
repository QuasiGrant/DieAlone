using UnityEngine;

/// All numbers for the VHS look in one asset, same pattern as PlayerTuning.
/// Edit during Play mode and the values stay. Lives at Assets/Settings/LookTuning.asset.
[CreateAssetMenu(fileName = "LookTuning", menuName = "DieAlone/Look Tuning")]
public class LookTuning : ScriptableObject
{
    [Header("Filter")]
    [Tooltip("Master switch for the whole full-screen filter.")]
    public bool filterEnabled = true;

    [Header("Low resolution")]
    [Tooltip("Internal picture height in pixels. Width follows the screen aspect. The picture is scaled back up softly.")]
    [Range(64, 1080)] public int lowResHeight = 480;

    [Header("Tape color")]
    [Tooltip("How far colors smear sideways past their edges. 0 is off, 1 is heavy.")]
    [Range(0f, 1f)] public float colorBleed = 0.5f;
    [Tooltip("How much color drains toward gray. 0 is full color, 1 is nearly gray.")]
    [Range(0f, 1f)] public float washOut = 0.35f;
    [Tooltip("How much shadow detail collapses into flat black. 0 is off.")]
    [Range(0f, 1f)] public float crushBlacks = 0.3f;

    [Header("Grain and tape noise")]
    [Tooltip("Strength of the moving grain over the whole picture. 0 is off.")]
    [Range(0f, 1f)] public float grainStrength = 0.35f;
    [Tooltip("How many times per second the grain pattern changes.")]
    [Range(1f, 60f)] public float grainSpeed = 24f;
    [Tooltip("How strong the noise band is when it appears. 0 turns bands off.")]
    [Range(0f, 1f)] public float noiseBandStrength = 0.7f;
    [Tooltip("How fast the band rolls up the screen, in screen heights per second.")]
    [Range(0.1f, 3f)] public float noiseBandSpeed = 0.6f;
    [Tooltip("Average seconds between bands.")]
    [Range(1f, 60f)] public float noiseBandInterval = 6f;

    [Header("Scan lines, blur, dark corners")]
    [Tooltip("Darkening of every other picture row. 0 is off.")]
    [Range(0f, 1f)] public float scanLines = 0.3f;
    [Tooltip("Sideways softness on top of the low resolution. 0 is off.")]
    [Range(0f, 1f)] public float blur = 0.3f;
    [Tooltip("How dark the corners of the picture go. 0 is off.")]
    [Range(0f, 1f)] public float darkCorners = 0.4f;

    [Header("Fog and darkness")]
    [Tooltip("Master switch for distance fog.")]
    public bool fogEnabled = true;
    [Tooltip("Color things fade into. The sky takes this color too, so the far end blends away.")]
    public Color fogColor = new Color(0.02f, 0.03f, 0.05f);
    [Tooltip("Distance in metres where fog starts.")]
    [Range(0f, 100f)] public float fogStart = 6f;
    [Tooltip("Distance in metres where things are fully hidden.")]
    [Range(1f, 200f)] public float fogEnd = 40f;

    [Header("Fog (sunset scenes)")]
    [Tooltip("Haze color for scenes set to the Sunset fog set. The far ridge fades into this.")]
    public Color sunsetFogColor = new Color(0.62f, 0.36f, 0.22f);
    [Range(0f, 200f)] public float sunsetFogStart = 25f;
    [Range(1f, 600f)] public float sunsetFogEnd = 320f;
    [Tooltip("Flat ambient light for sunset scenes. Lifts the forest floor under the canopy where the low sun never reaches.")]
    public Color sunsetAmbient = new Color(0.5f, 0.34f, 0.28f);

    [Header("Sunset sky")]
    public Color skyTop = new Color(0.22f, 0.11f, 0.10f);
    public Color skyHorizon = new Color(0.85f, 0.40f, 0.18f);
    public Color skyGround = new Color(0.30f, 0.16f, 0.10f);
    public Color sunGlowColor = new Color(1.0f, 0.55f, 0.25f);
    [Tooltip("Higher is a tighter glow around the sun.")]
    [Range(2f, 200f)] public float sunGlowSize = 24f;

    [Header("Horizon fire")]
    public Color fireGlowColor = new Color(1.0f, 0.42f, 0.10f);
    [Tooltip("Brightness of the glow strips along the burning ridge. 0 hides them.")]
    [Range(0f, 6f)] public float fireGlowIntensity = 2.0f;
    [Tooltip("Smoke columns over the ridge. 0 is none, 1 is the built amount.")]
    [Range(0f, 2f)] public float fireSmoke = 1.0f;
    [Tooltip("Embers and ash drifting over the ridge. 0 is none, 1 is the built amount.")]
    [Range(0f, 2f)] public float fireEmbers = 1.0f;
}
