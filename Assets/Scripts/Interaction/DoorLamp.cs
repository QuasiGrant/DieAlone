using UnityEngine;

/// A lamp that is lit only while its door stands open (FrontLayout.md 2.7: the office west door must read open or shut from the
/// tower deck; the porch lamp over it shows which). Shows or hides the lamp object, its glow and its light, to match the door.
public class DoorLamp : MonoBehaviour
{
    [SerializeField] private Door door;
    [Tooltip("The lamp's glow and light; shown while the door is open.")]
    [SerializeField] private GameObject lamp;

    private void LateUpdate() => Sync();

    /// Matches the lamp to the door now (checks that move the door between frames call it).
    public void Sync()
    {
        if (door == null || lamp == null) return;
        if (lamp.activeSelf != door.IsOpen) lamp.SetActive(door.IsOpen);
    }
}
