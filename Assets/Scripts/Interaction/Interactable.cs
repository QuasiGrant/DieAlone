using UnityEngine;

/// Base for anything the player can use with the Interact action.
/// Doors, pickups and switches derive from this and override Use.
public abstract class Interactable : MonoBehaviour
{
    [Tooltip("Short text shown when the player looks at this object.")]
    [SerializeField] private string prompt = "Use";

    public virtual string Prompt => prompt;

    /// Return false to hide the prompt and ignore Interact, e.g. a locked door.
    public virtual bool CanUse(PlayerInteractor user) => true;

    public abstract void Use(PlayerInteractor user);
}
