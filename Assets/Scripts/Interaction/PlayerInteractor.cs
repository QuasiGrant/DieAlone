using UnityEngine;
using UnityEngine.InputSystem;

/// Looks along the eye for an Interactable within reach, shows its prompt,
/// and calls Use on it when the Interact action is pressed.
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [Tooltip("Ray origin and direction. Normally the player camera.")]
    [SerializeField] private Transform eye;
    [SerializeField] private float reach = 2f;
    [SerializeField] private LayerMask mask = ~0;
    [SerializeField] private InteractPromptUI promptUI;

    private InputAction interactAction;
    private Interactable current;

    /// The usable object under the crosshair this frame, or null.
    public Interactable Current => current;
    public Transform Eye => eye;

    private void Awake()
    {
        var map = inputActions.FindActionMap("Player", throwIfNotFound: true);
        interactAction = map.FindAction("Interact", throwIfNotFound: true);
    }

    private void OnEnable() => interactAction.Enable();
    private void OnDisable() => interactAction.Disable();

    private void Update()
    {
        current = FindTarget();
        if (promptUI != null) promptUI.SetPrompt(current != null ? current.Prompt : null);

        if (current != null && interactAction.WasPressedThisFrame())
            current.Use(this);
    }

    private Interactable FindTarget()
    {
        if (!Physics.Raycast(eye.position, eye.forward, out RaycastHit hit, reach, mask, QueryTriggerInteraction.Ignore))
            return null;

        var target = hit.collider.GetComponentInParent<Interactable>();
        if (target == null || !target.CanUse(this)) return null;
        return target;
    }
}
