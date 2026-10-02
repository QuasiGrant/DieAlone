using UnityEngine;

/// IW3, the invisible wall across the campground spur (FrontLayout.md 2.5 and 4.2; DECISIONS 2026-09-29, Wren 2026-10-02):
/// solid while any admitted car is on the spur, from the barrier lifting for it until it is parked, whatever the gate shift
/// does; open otherwise, so the player can walk up and find the parked cars. The car code (Milestone 10) calls
/// CarAdmitted and CarParked; nothing else turns it on.
[RequireComponent(typeof(BoxCollider))]
public class SpurWall : MonoBehaviour
{
    private BoxCollider wall;
    private int carsOnSpur;

    public bool IsOn => wall != null && wall.enabled;
    public int CarsOnSpur => carsOnSpur;

    private void Awake()
    {
        wall = GetComponent<BoxCollider>();
        Apply();
    }

    /// The barrier lifts for an admitted car.
    public void CarAdmitted()
    {
        carsOnSpur++;
        Apply();
    }

    /// An admitted car has stopped on its pitch.
    public void CarParked()
    {
        carsOnSpur = Mathf.Max(0, carsOnSpur - 1);
        Apply();
    }

    private void Apply()
    {
        if (wall != null) wall.enabled = carsOnSpur > 0;
    }
}
