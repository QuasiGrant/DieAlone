using UnityEngine;

/// Hinged door. This object is the hinge; the panel is a child collider.
/// Opens away from whoever used it, and waits instead of pushing when the
/// player is in the way, so it can never shove or trap them.
public class Door : Interactable
{
    [SerializeField] private PlayerTuning tuning;
    [SerializeField] private BoxCollider panel;
    [SerializeField] private LayerMask blockers = ~0;

    private bool isOpen;
    private float currentAngle;
    private float targetAngle;

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

        // Swing away from the user. The hinge forward is the closed panel normal.
        Vector3 toUser = user.transform.position - transform.position;
        float side = Vector3.Dot(transform.forward, toUser);
        targetAngle = side >= 0f ? tuning.doorOpenAngle : -tuning.doorOpenAngle;
    }

    private void Update()
    {
        if (Mathf.Approximately(currentAngle, targetAngle)) return;

        float next = Mathf.MoveTowardsAngle(currentAngle, targetAngle, tuning.doorSwingSpeed * Time.deltaTime);
        if (WouldHitPlayer(next)) return;   // hold this frame, try again next frame

        currentAngle = next;
        transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }

    private bool WouldHitPlayer(float angle)
    {
        Quaternion parentRot = transform.parent != null ? transform.parent.rotation : Quaternion.identity;
        Quaternion rot = parentRot * Quaternion.Euler(0f, angle, 0f);
        Vector3 center = transform.position + rot * (panel.transform.localPosition + panel.center);
        Vector3 half = Vector3.Scale(panel.size, panel.transform.localScale) * 0.5f;

        var hits = Physics.OverlapBox(center, half, rot, blockers, QueryTriggerInteraction.Ignore);
        foreach (var h in hits)
            if (h is CharacterController) return true;
        return false;
    }
}
