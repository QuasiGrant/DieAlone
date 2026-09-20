using UnityEngine;

/// A loose object the player can pick up with Interact and carry, one at a time.
public class Carryable : Interactable
{
    public override bool CanUse(PlayerInteractor user)
    {
        var carry = user.GetComponent<PlayerCarry>();
        return carry != null && !carry.IsCarrying;
    }

    public override void Use(PlayerInteractor user)
    {
        var carry = user.GetComponent<PlayerCarry>();
        if (carry != null) carry.PickUp(this);
    }
}
