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
    [SerializeField] private float sprintSpeed = 5.5f;
    [SerializeField] private float gravity = 20f;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float standHeight = 1.8f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float standEyeHeight = 1.6f;
    [SerializeField] private float crouchEyeHeight = 0.8f;
    [SerializeField] private float crouchTransitionSpeed = 6f;

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
    private InputAction sprintAction;
    private InputAction crouchAction;
    private bool isCrouching;
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
        sprintAction = map.FindAction("Sprint", throwIfNotFound: true);
        crouchAction = map.FindAction("Crouch", throwIfNotFound: true);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Look();
        Crouch();
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

    private void Crouch()
    {
        bool wantCrouch = crouchAction.IsPressed();

        if (wantCrouch != isCrouching)
        {
            // Standing up needs clear space above the crouched capsule.
            if (wantCrouch || HasHeadroom()) isCrouching = wantCrouch;
        }

        float targetHeight = isCrouching ? crouchHeight : standHeight;
        float step = crouchTransitionSpeed * Time.deltaTime;
        float h = Mathf.MoveTowards(controller.height, targetHeight, step);
        controller.height = h;
        controller.center = new Vector3(0f, h * 0.5f, 0f);

        float targetEye = isCrouching ? crouchEyeHeight : standEyeHeight;
        Vector3 camPos = cameraPivot.localPosition;
        camPos.y = Mathf.MoveTowards(camPos.y, targetEye, step);
        cameraPivot.localPosition = camPos;
    }

    // Sphere-casts up from the top of the crouched capsule to the standing height.
    // The cast starts inside our own CharacterController, which SphereCast ignores.
    private bool HasHeadroom()
    {
        float r = controller.radius - 0.05f;
        Vector3 origin = transform.position + Vector3.up * (crouchHeight - controller.radius);
        float distance = standHeight - crouchHeight;
        return !Physics.SphereCast(origin, r, Vector3.up, out _, distance, ~0, QueryTriggerInteraction.Ignore);
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 planar = transform.right * input.x + transform.forward * input.y;
        if (planar.sqrMagnitude > 1f) planar.Normalize();

        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        verticalVelocity -= gravity * Time.deltaTime;

        float speed = isCrouching ? crouchSpeed : (sprintAction.IsPressed() ? sprintSpeed : walkSpeed);
        Vector3 velocity = planar * speed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
