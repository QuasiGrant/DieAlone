using UnityEngine;

/// Fire pit stand-in: Use toggles the flame effects and light on and off.
/// Built on Interactable so the day-loop fire logic can replace Use later.
public class FirePit : Interactable
{
    [Tooltip("Objects switched on while the fire burns: flame and smoke effects, the light.")]
    [SerializeField] private GameObject[] burningObjects;
    [SerializeField] private bool startsLit = true;

    private bool lit;

    public bool IsLit => lit;
    public override string Prompt => lit ? "Put out the fire" : "Light the fire";

    private void Awake()
    {
        lit = startsLit;
        Apply();
    }

    public override void Use(PlayerInteractor user)
    {
        lit = !lit;
        Apply();
    }

    private void Apply()
    {
        if (burningObjects == null) return;
        foreach (var go in burningObjects) if (go != null) go.SetActive(lit);
    }
}
