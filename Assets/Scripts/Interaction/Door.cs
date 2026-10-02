using UnityEngine;

/// Hinged door. This object is the hinge; the panel is a child collider.
/// Opens away from whoever used it, or to one set side (fixedSwing), and waits instead of
/// pushing when the player is in the way, so it can never shove or trap them.
/// startOpen (CampLayout.md 4.5): the scene pose is the closed pose; the door opens at load
/// and again at every wake (WakeSignal), so the keeper always wakes to it standing open.
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

    private bool isOpen;
    private float currentAngle;
    private float targetAngle;
    private Quaternion closedRotation = Quaternion.identity;   // hinge's own local rotation when shut, so doors on any wall swing from where they stand

    private void Awake()
    {
        closedRotation = transform.localRotation;
        if (startOpen) OpenNow();
    }

    private void OnEnable() { if (startOpen) WakeSignal.Woke += OpenNow; }
    private void OnDisable() { WakeSignal.Woke -= OpenNow; }

    public bool IsOpen => isOpen;
    public float CurrentAngle => currentAngle;
    public override string Prompt => isOpen ? "Close" : "Open";

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

    /// Opens at once, without the swing (load and wake: nobody stands in the doorway then).
    private void OpenNow()
    {
        isOpen = true;
        targetAngle = currentAngle = OpenAngle(transform.position - transform.forward);
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
