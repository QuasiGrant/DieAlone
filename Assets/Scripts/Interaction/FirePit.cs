using UnityEngine;

/// Fire pit stand-in: Use toggles the flame effects and light on and off.
/// Built on Interactable so the day-loop fire logic can replace Use later.
public class FirePit : Interactable
{
    [Tooltip("Objects switched on while the fire burns: flame and smoke effects, the light.")]
    [SerializeField] private GameObject[] burningObjects;
    [SerializeField] private bool startsLit = true;
    [Tooltip("Light that flickers while the fire burns. Optional.")]
    [SerializeField] private Light fireLight;
    [Tooltip("How far the light wanders around its set intensity. 0 is steady.")]
    [Range(0f, 1f)] [SerializeField] private float flicker = 0.35f;

    private bool lit;
    private float baseIntensity, baseRange;

    private void Update()
    {
        if (!lit || fireLight == null) return;
        // Two noise curves at different speeds: a slow breathe and a quick crackle.
        float t = Time.time;
        float n = (Mathf.PerlinNoise(t * 1.3f, 0.37f) - 0.5f) * 1.2f + (Mathf.PerlinNoise(t * 9f, 4.1f) - 0.5f) * 0.8f;
        fireLight.intensity = baseIntensity * (1f + flicker * n);
        fireLight.range = baseRange * (1f + flicker * 0.3f * n);
    }

    public bool IsLit => lit;
    public override string Prompt => lit ? "Put out the fire" : "Light the fire";

    private void Awake()
    {
        lit = startsLit;
        if (fireLight == null) fireLight = GetComponentInChildren<Light>(true);
        if (fireLight != null) { baseIntensity = fireLight.intensity; baseRange = fireLight.range; }
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
