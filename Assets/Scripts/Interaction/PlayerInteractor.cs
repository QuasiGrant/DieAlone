using UnityEngine;
using UnityEngine.InputSystem;

/// Looks along the eye for an Interactable within reach, shows its prompt,
/// and calls Use on it when the Interact action is pressed. While the player
/// is carrying something, Interact sets it down instead.
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private PlayerTuning tuning;
    [SerializeField] private InputActionAsset inputActions;
    [Tooltip("Ray origin and direction. Normally the player camera.")]
    [SerializeField] private Transform eye;
    [SerializeField] private LayerMask mask = ~0;
    [SerializeField] private InteractPromptUI promptUI;

    private InputAction interactAction;
    private InputAction throwAction;
    private Interactable current;
    private PlayerCarry carry;

    /// The usable object under the crosshair this frame, or null.
    public Interactable Current => current;
    public Transform Eye => eye;

    private void Awake()
    {
        var map = inputActions.FindActionMap("Player", throwIfNotFound: true);
        interactAction = map.FindAction("Interact", throwIfNotFound: true);
        throwAction = map.FindAction("Throw", throwIfNotFound: true);
        carry = GetComponent<PlayerCarry>();
    }

    private void OnEnable() { interactAction.Enable(); throwAction.Enable(); }
    private void OnDisable() { interactAction.Disable(); throwAction.Disable(); }

    private void Update()
    {
        bool pressed = interactAction.WasPressedThisFrame();

        if (carry != null && carry.IsCarrying)
        {
            current = null;
            if (throwAction.WasPressedThisFrame())
            {
                carry.Throw();
                if (promptUI != null) promptUI.SetPrompt(null);
                return;
            }
            bool canPlace = carry.CanSetDown();
            if (promptUI != null) promptUI.SetPrompt(canPlace ? "Set down" : null);
            if (pressed)
            {
                if (canPlace) carry.SetDown();
                else carry.Drop();
            }
            return;
        }

        current = FindTarget();
        if (promptUI != null) promptUI.SetPrompt(current != null ? current.Prompt : null);

        if (current != null && pressed)
            current.Use(this);
    }

    private Interactable FindTarget()
    {
        if (!Physics.Raycast(eye.position, eye.forward, out RaycastHit hit, tuning.interactReach, mask, QueryTriggerInteraction.Ignore))
            return null;

        var target = hit.collider.GetComponentInParent<Interactable>();
        if (target == null || !target.CanUse(this)) return null;
        return target;
    }
}
