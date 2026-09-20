using UnityEngine;

/// Holds one Carryable in front of the camera and sets it down on the surface
/// the player is looking at. Not an inventory: one object, always visible.
public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private PlayerTuning tuning;
    [Tooltip("Ray origin and direction. Normally the player camera.")]
    [SerializeField] private Transform eye;
    [Tooltip("Child of the camera. The carried object follows this point.")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private LayerMask mask = ~0;

    private Carryable held;
    private Collider heldCollider;

    public bool IsCarrying => held != null;
    public Carryable Held => held;

    public void PickUp(Carryable item)
    {
        if (held != null || item == null) return;
        held = item;
        heldCollider = item.GetComponent<Collider>();
        if (heldCollider != null) heldCollider.enabled = false;
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

    public void SetDown()
    {
        if (held == null || !TryFindPlacement(out Vector3 pos, out Quaternion rot)) return;
        held.transform.SetParent(null, true);
        held.transform.SetPositionAndRotation(pos, rot);
        if (heldCollider != null) heldCollider.enabled = true;
        held = null;
        heldCollider = null;
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
