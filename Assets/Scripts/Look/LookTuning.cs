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
}
