using UnityEngine;

/// Test object: each use flips its color and gives a short scale pop.
public class ToggleColorInteractable : Interactable
{
    [SerializeField] private Renderer target;
    [SerializeField] private Color onColor = new Color(0.9f, 0.3f, 0.2f);
    [SerializeField] private float popScale = 1.15f;
    [SerializeField] private float popSeconds = 0.15f;

    private Color offColor;
    private bool isOn;
    private Vector3 baseScale;
    private float popUntil;

    public bool IsOn => isOn;

    private void Awake()
    {
        if (target == null) target = GetComponent<Renderer>();
        offColor = target.material.color;
        baseScale = transform.localScale;
    }

    public override void Use(PlayerInteractor user)
    {
        isOn = !isOn;
        target.material.color = isOn ? onColor : offColor;
        popUntil = Time.time + popSeconds;
    }

    private void Update()
    {
        float s = Time.time < popUntil ? popScale : 1f;
        transform.localScale = baseScale * s;
    }
}
