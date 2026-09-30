using UnityEngine;
using UnityEngine.InputSystem;

/// First-person walk, sprint, crouch, jump and look. Reads actions from the Player map
/// in the project's InputActionAsset so keyboard/mouse and gamepad both work.
/// All feel numbers come from the PlayerTuning asset.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerTuning tuning;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Transform cameraPivot;

    private CharacterController controller;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction jumpAction;
    private float lastGroundedTime = -999f;
    private float clock;   // seconds of Step time, so Play-mode checks that drive Step keep the coyote window
    private bool isCrouching;
    private float pitch;
    private float verticalVelocity;
    private bool lockedLastFrame;
    private int lockSettleFrames;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.radius = tuning.capsuleRadius;
        controller.stepOffset = tuning.stepOffset;
        controller.skinWidth = tuning.skinWidth;
        controller.height = tuning.standHeight;
        controller.center = new Vector3(0f, tuning.standHeight * 0.5f, 0f);

        var map = inputActions.FindActionMap("Player", throwIfNotFound: true);
        moveAction = map.FindAction("Move", throwIfNotFound: true);
        lookAction = map.FindAction("Look", throwIfNotFound: true);
        sprintAction = map.FindAction("Sprint", throwIfNotFound: true);
        crouchAction = map.FindAction("Crouch", throwIfNotFound: true);
        jumpAction = map.FindAction("Jump", throwIfNotFound: true);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        jumpAction.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
        jumpAction.Disable();
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
            ? look * tuning.mouseSensitivity
            : look * tuning.gamepadLookSpeed * Time.deltaTime;

        // Player settings: sensitivity multiplier and invert, chosen in the pause menu.
        var settings = PlayerSettings.Current;
        delta *= settings.lookSensitivity;
        if (settings.invertLook) delta.y = -delta.y;

        transform.Rotate(0f, delta.x, 0f);
        pitch = Mathf.Clamp(pitch - delta.y, -tuning.pitchLimit, tuning.pitchLimit);
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

        float targetHeight = isCrouching ? tuning.crouchHeight : tuning.standHeight;
        float step = tuning.crouchTransitionSpeed * Time.deltaTime;
        float h = Mathf.MoveTowards(controller.height, targetHeight, step);
        controller.height = h;
        controller.center = new Vector3(0f, h * 0.5f, 0f);

        float targetEye = isCrouching ? tuning.crouchEyeHeight : tuning.standEyeHeight;
        Vector3 camPos = cameraPivot.localPosition;
        camPos.y = Mathf.MoveTowards(camPos.y, targetEye, step);
        cameraPivot.localPosition = camPos;
    }

    // Sphere-casts up from the top of the crouched capsule to the standing height.
    // The cast starts inside our own CharacterController, which SphereCast ignores.
    private bool HasHeadroom()
    {
        float r = controller.radius - 0.05f;
        Vector3 origin = transform.position + Vector3.up * (tuning.crouchHeight - controller.radius);
        float distance = tuning.standHeight - tuning.crouchHeight;
        return !Physics.SphereCast(origin, r, Vector3.up, out _, distance, ~0, QueryTriggerInteraction.Ignore);
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 planar = transform.right * input.x + transform.forward * input.y;
        Step(planar, jumpAction.WasPressedThisFrame(), sprintAction.IsPressed(), Time.deltaTime);
    }

    /// One movement step: planar is the wished direction in world space (length 0 to 1). Public so Play-mode checks can drive
    /// the real movement rules with the component disabled. On ground steeper than the controller's slope limit the player
    /// is not grounded for jumping and slides down it, so no face over the limit can be climbed by hopping.
    public void Step(Vector3 planar, bool jumpPressed, bool sprint, float dt)
    {
        clock += dt;
        if (planar.sqrMagnitude > 1f) planar.Normalize();

        bool steep = false; Vector3 downhill = Vector3.zero;
        if (controller.isGrounded && SteepGround(out Vector3 normal, out Vector3 away))
        {
            steep = true;
            // down the face, and out over the drop: on a near-vertical face edge the down-the-face part alone points into the edge
            downhill = (Vector3.ProjectOnPlane(Vector3.down, normal).normalized + away).normalized;
            Vector3 uphill = new Vector3(-downhill.x, 0f, -downhill.z).normalized;
            float into = Vector3.Dot(planar, uphill);
            if (into > 0f) planar -= uphill * into;   // no walking up it either
        }

        if (controller.isGrounded && !steep)
        {
            lastGroundedTime = clock;
            if (verticalVelocity < 0f) verticalVelocity = -2f;
        }

        bool canJump = clock - lastGroundedTime <= tuning.coyoteTime;
        if (canJump && jumpPressed)
        {
            verticalVelocity = Mathf.Sqrt(2f * tuning.gravity * tuning.jumpHeight);
            lastGroundedTime = -999f;
        }

        verticalVelocity -= tuning.gravity * dt;

        float speed = isCrouching ? tuning.crouchSpeed : (sprint ? tuning.sprintSpeed : tuning.walkSpeed);
        Vector3 velocity = planar * speed + Vector3.up * verticalVelocity + downhill * tuning.steepSlideSpeed;
        controller.Move(velocity * dt);
    }

    // The surface under the capsule's foot, found by a sphere cast from the foot sphere's centre; true when it is steeper than
    // the controller's slope limit. An edge under the foot gives a tilted normal, so the player also slides off rims and walls.
    // away: level direction from the contact point to the foot's centre, so a player perched on a steep face's edge slides off it.
    private bool SteepGround(out Vector3 normal, out Vector3 away)
    {
        float r = controller.radius * 0.9f;
        Vector3 origin = transform.position + Vector3.up * (controller.radius + controller.skinWidth);
        if (Physics.SphereCast(origin, r, Vector3.down, out RaycastHit hit, tuning.groundProbeDistance + controller.skinWidth, ~0, QueryTriggerInteraction.Ignore))
        {
            normal = hit.normal;
            Vector3 centre = origin + Vector3.down * hit.distance;
            away = hit.distance > 0f ? Vector3.ProjectOnPlane(centre - hit.point, Vector3.up).normalized : Vector3.zero;
            return Vector3.Angle(normal, Vector3.up) > controller.slopeLimit;
        }
        normal = Vector3.up; away = Vector3.zero;
        return false;
    }

    // Shove loose physics objects aside instead of being blocked by them.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.rigidbody;
        if (body == null || body.isKinematic) return;
        if (hit.moveDirection.y < -0.3f) return;   // standing on it, not walking into it

        Vector3 push = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        body.linearVelocity = push * tuning.pushPower;
    }
}
