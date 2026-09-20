using UnityEngine;
using UnityEngine.InputSystem;

/// First-person walk and look. Reads Move and Look from the Player map in the
/// project's InputActionAsset so keyboard/mouse and gamepad both work.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float gravity = 20f;

    [Header("Look")]
    [SerializeField] private Transform cameraPivot;
    [Tooltip("Degrees per mouse pixel.")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [Tooltip("Degrees per second at full stick deflection.")]
    [SerializeField] private float gamepadLookSpeed = 120f;
    [SerializeField] private float pitchLimit = 85f;

    private CharacterController controller;
    private InputAction moveAction;
    private InputAction lookAction;
    private float pitch;
    private float verticalVelocity;
    private bool lockedLastFrame;
    private int lockSettleFrames;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        var map = inputActions.FindActionMap("Player", throwIfNotFound: true);
        moveAction = map.FindAction("Move", throwIfNotFound: true);
        lookAction = map.FindAction("Look", throwIfNotFound: true);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void Look()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        // Mouse delta is already per-frame pixels. Stick input is -1..1 and needs time scaling.
        bool fromPointer = lookAction.activeControl != null && lookAction.activeControl.device is Pointer;

        // Locking warps the cursor to screen center, which shows up as one huge delta.
        // Ignore mouse look while unlocked and for a couple of frames after locking.
        bool locked = Cursor.lockState == CursorLockMode.Locked;
        if (locked && !lockedLastFrame) lockSettleFrames = 2;
        lockedLastFrame = locked;
        if (fromPointer)
        {
            if (!locked) return;
            if (lockSettleFrames > 0) { lockSettleFrames--; return; }
        }
        Vector2 delta = fromPointer
            ? look * mouseSensitivity
            : look * gamepadLookSpeed * Time.deltaTime;

        transform.Rotate(0f, delta.x, 0f);
        pitch = Mathf.Clamp(pitch - delta.y, -pitchLimit, pitchLimit);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 planar = transform.right * input.x + transform.forward * input.y;
        if (planar.sqrMagnitude > 1f) planar.Normalize();

        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        verticalVelocity -= gravity * Time.deltaTime;

        Vector3 velocity = planar * walkSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
