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

    [Header("Carry")]
    [Tooltip("Where the carried object sits, relative to the camera: right, down, forward.")]
    public Vector3 carryHoldOffset = new Vector3(0.3f, -0.25f, 0.7f);
    [Tooltip("How quickly the carried object follows the camera. Higher is stiffer.")]
    public float carryFollowSpeed = 14f;
    [Tooltip("How far away the player can set an object down, in metres.")]
    public float placeReach = 2f;
    [Tooltip("How level a surface must be to set an object on it. 1 is flat, 0 allows walls.")]
    [Range(0f, 1f)] public float placeMinUpNormal = 0.7f;
    [Tooltip("Forward speed given to a dropped object, in m/s, on top of the player's own motion.")]
    public float dropForwardSpeed = 1f;
    [Tooltip("How hard the player shoves loose physics objects when walking into them.")]
    public float pushPower = 2f;
}
