using UnityEngine;

/// All player-feel numbers in one asset. Edit it in the Inspector during Play mode
/// and the values stay after Play stops. Lives at Assets/Settings/PlayerTuning.asset.
[CreateAssetMenu(fileName = "PlayerTuning", menuName = "DieAlone/Player Tuning")]
public class PlayerTuning : ScriptableObject
{
    [Header("Movement (m/s)")]
    public float walkSpeed = 2.5f;
    public float sprintSpeed = 5.5f;
    public float crouchSpeed = 1.5f;
    public float gravity = 20f;

    [Header("Jump")]
    [Tooltip("Apex height of the hop in metres.")]
    public float jumpHeight = 0.6f;
    [Tooltip("Seconds after leaving the ground during which a jump still counts.")]
    public float coyoteTime = 0.1f;

    [Header("Crouch")]
    public float standHeight = 1.8f;
    public float crouchHeight = 1.0f;
    public float standEyeHeight = 1.6f;
    public float crouchEyeHeight = 0.8f;
    [Tooltip("Metres per second the capsule and camera move between stand and crouch.")]
    public float crouchTransitionSpeed = 6f;

    [Header("Look")]
    [Tooltip("Degrees per mouse pixel.")]
    public float mouseSensitivity = 0.1f;
    [Tooltip("Degrees per second at full stick deflection.")]
    public float gamepadLookSpeed = 120f;
    public float pitchLimit = 85f;

    [Header("Capsule (applied when Play starts)")]
    public float capsuleRadius = 0.35f;
    public float stepOffset = 0.1f;
    public float skinWidth = 0.035f;

    [Header("Interaction")]
    [Tooltip("How far the player can reach a usable object, in metres.")]
    public float interactReach = 2f;

    [Header("Doors")]
    public float doorOpenAngle = 90f;
    [Tooltip("Degrees per second.")]
    public float doorSwingSpeed = 150f;
}
