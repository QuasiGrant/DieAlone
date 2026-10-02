using UnityEngine;

/// Hinged door. This object is the hinge; the panel is a child collider.
/// Opens away from whoever used it, or to one set side (fixedSwing), and waits instead of
/// pushing when the player is in the way, so it can never shove or trap them.
/// startOpen (CampLayout.md 4.5): the scene pose is the closed pose; the door opens at load
/// and again at every wake (WakeSignal), so the keeper always wakes to it standing open.
/// noPrompt (FrontLayout.md O1, the office west door): the player cannot use it; only the world
/// shuts it (HoldShut, for an Office CHECK) and opens it again when it lets go.
public class Door : Interactable
{
    [SerializeField] private PlayerTuning tuning;
    [SerializeField] private BoxCollider panel;
    [SerializeField] private LayerMask blockers = ~0;
    [Tooltip("Open at load and at every wake; the scene pose is the closed pose.")]
    [SerializeField] private bool startOpen;
    [Tooltip("Always open to the same side, whoever uses it.")]
    [SerializeField] private bool fixedSwing;
    [Tooltip("With fixedSwing: +1 swings toward the hinge's back (against its forward), -1 toward its forward.")]
    [SerializeField] private float fixedSide = -1f;
    [Tooltip("No prompt and no Interact: only the world opens or shuts it (HoldShut).")]
    [SerializeField] private bool noPrompt;

    private bool isOpen;
    private bool heldShut;
    private float currentAngle;
    private float targetAngle;
    private Quaternion closedRotation = Quaternion.identity;   // hinge's own local rotation when shut, so doors on any wall swing from where they stand

    private void Awake()
    {
        closedRotation = transform.localRotation;
        if (startOpen) OpenNow();
    }

    private void OnEnable() { if (startOpen) WakeSignal.Woke += OnWake; }
    private void OnDisable() { WakeSignal.Woke -= OnWake; }

    public bool IsOpen => isOpen;
    public bool IsHeldShut => heldShut;
    public float CurrentAngle => currentAngle;
    public override string Prompt => isOpen ? "Close" : "Open";
    public override bool CanUse(PlayerInteractor user) => !noPrompt;

    public override void Use(PlayerInteractor user)
    {
        isOpen = !isOpen;
        if (!isOpen)
        {
            targetAngle = 0f;
            return;
        }
        targetAngle = OpenAngle(user.transform.position);
    }

    /// The world shuts the door (true) or lets it go (false), for a state such as an Office CHECK. Let go, a
    /// startOpen door opens again; a held door stays shut through a wake. instant skips the swing (nobody in the doorway).
    public void HoldShut(bool shut, bool instant = false)
    {
        heldShut = shut;
        if (shut)
        {
            isOpen = false;
            targetAngle = 0f;
            if (instant) SetAngle(0f);
            return;
        }
        if (!startOpen) return;
        if (instant) { OpenNow(); return; }
        isOpen = true;
        targetAngle = OpenAngle(transform.position - transform.forward);
    }

    private void OnWake() { if (!heldShut) OpenNow(); }

    /// Opens at once, without the swing (load and wake: nobody stands in the doorway then).
    private void OpenNow()
    {
        isOpen = true;
        targetAngle = OpenAngle(transform.position - transform.forward);
        SetAngle(targetAngle);
    }

    private void SetAngle(float angle)
    {
        currentAngle = angle;
        transform.localRotation = closedRotation * Quaternion.Euler(0f, currentAngle, 0f);
    }

    private float OpenAngle(Vector3 userPosition)
    {
        if (fixedSwing) return Mathf.Sign(fixedSide) * tuning.doorOpenAngle;
        // Swing away from the user. The hinge forward is the closed panel normal.
        Vector3 toUser = userPosition - transform.position;
        float side = Vector3.Dot(transform.forward, toUser);
        return side >= 0f ? tuning.doorOpenAngle : -tuning.doorOpenAngle;
    }

    private void Update()
    {
        if (Mathf.Approximately(currentAngle, targetAngle)) return;

        float next = Mathf.MoveTowardsAngle(currentAngle, targetAngle, tuning.doorSwingSpeed * Time.deltaTime);
        if (WouldHitPlayer(next)) return;   // hold this frame, try again next frame

        currentAngle = next;
        transform.localRotation = closedRotation * Quaternion.Euler(0f, currentAngle, 0f);
    }

    private bool WouldHitPlayer(float angle)
    {
        Quaternion parentRot = transform.parent != null ? transform.parent.rotation : Quaternion.identity;
        Quaternion rot = parentRot * closedRotation * Quaternion.Euler(0f, angle, 0f);
        Vector3 center = transform.position + rot * (panel.transform.localPosition + panel.center);
        Vector3 half = Vector3.Scale(panel.size, panel.transform.localScale) * 0.5f;

        var hits = Physics.OverlapBox(center, half, rot, blockers, QueryTriggerInteraction.Ignore);
        foreach (var h in hits)
            if (h is CharacterController) return true;
        return false;
    }
}
