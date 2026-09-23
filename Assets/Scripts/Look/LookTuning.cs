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
}
