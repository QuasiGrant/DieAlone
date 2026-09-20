using UnityEngine;

/// Holds one Carryable in front of the camera. Interact sets it down on the surface
/// the player is looking at, or drops it to fall with physics when there is no
/// valid surface. Not an inventory: one object, always visible.
public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private PlayerTuning tuning;
    [Tooltip("Ray origin and direction. Normally the player camera.")]
    [SerializeField] private Transform eye;
    [Tooltip("Child of the camera. The carried object follows this point.")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private LayerMask mask = ~0;

    private CharacterController controller;
    private Carryable held;
    private Collider heldCollider;
    private Rigidbody heldBody;

    public bool IsCarrying => held != null;
    public Carryable Held => held;

    private void Awake() => controller = GetComponent<CharacterController>();

    public void PickUp(Carryable item)
    {
        if (held != null || item == null) return;
        held = item;
        heldCollider = item.GetComponent<Collider>();
        heldBody = item.GetComponent<Rigidbody>();
        if (heldCollider != null) heldCollider.enabled = false;
        if (heldBody != null) heldBody.isKinematic = true;
        item.transform.SetParent(holdPoint, true);
    }

    private void LateUpdate()
    {
        if (held == null) return;
        holdPoint.localPosition = tuning.carryHoldOffset;
        float t = 1f - Mathf.Exp(-tuning.carryFollowSpeed * Time.deltaTime);
        held.transform.position = Vector3.Lerp(held.transform.position, holdPoint.position, t);
        held.transform.rotation = Quaternion.Euler(0f, eye.eulerAngles.y, 0f);
    }

    public bool CanSetDown() => held != null && TryFindPlacement(out _, out _);

    /// Place the object on the surface under the crosshair. Does nothing if there is none.
    public void SetDown()
    {
        if (held == null || !TryFindPlacement(out Vector3 pos, out Quaternion rot)) return;
        Release(pos, rot, Vector3.zero);
    }

    /// Let go where it is. It keeps the player's motion plus a small forward push and falls.
    public void Drop()
    {
        if (held == null) return;
        Vector3 velocity = (controller != null ? controller.velocity : Vector3.zero) + eye.forward * tuning.dropForwardSpeed;
        Release(held.transform.position, held.transform.rotation, velocity);
    }

    /// Fling the object along the look direction with a little lift.
    public void Throw()
    {
        if (held == null) return;
        Vector3 velocity = (controller != null ? controller.velocity : Vector3.zero)
                         + eye.forward * tuning.throwSpeed
                         + Vector3.up * tuning.throwLift;
        Release(held.transform.position, held.transform.rotation, velocity);
    }

    private void Release(
Vector3 pos, Quaternion rot, Vector3 velocity)
    {
        held.transform.SetParent(null, true);
        held.transform.SetPositionAndRotation(pos, rot);
        if (heldCollider != null) heldCollider.enabled = true;
        if (heldBody != null)
        {
            heldBody.isKinematic = false;
            heldBody.linearVelocity = velocity;
            heldBody.angularVelocity = Vector3.zero;
        }
        held = null;
        heldCollider = null;
        heldBody = null;
    }

    // A spot is valid when the look ray hits a mostly-upward surface within reach and
    // the object's box fits there without overlapping anything but the player.
    private bool TryFindPlacement(out Vector3 pos, out Quaternion rot)
    {
        rot = Quaternion.Euler(0f, eye.eulerAngles.y, 0f);
        pos = default;

        if (!Physics.Raycast(eye.position, eye.forward, out RaycastHit hit, tuning.placeReach, mask, QueryTriggerInteraction.Ignore))
            return false;
        if (hit.normal.y < tuning.placeMinUpNormal) return false;

        Vector3 half = HeldHalfExtents();
        pos = hit.point + Vector3.up * (half.y + 0.005f);

        var hits = Physics.OverlapBox(pos, half - Vector3.one * 0.01f, rot, mask, QueryTriggerInteraction.Ignore);
        foreach (var h in hits)
            if (!(h is CharacterController) && h != heldCollider) return false;
        return true;
    }

    private Vector3 HeldHalfExtents()
    {
        if (heldCollider is BoxCollider box)
            return Vector3.Scale(box.size, box.transform.lossyScale) * 0.5f;
        return held.transform.lossyScale * 0.5f;
    }
}
