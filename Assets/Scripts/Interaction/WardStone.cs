using UnityEngine;

/// Stand-in for the Ward interaction: using the stone flips its rune glow between
/// dim and bright. Built on Interactable so the real Ward logic can replace Use later.
public class WardStone : Interactable
{
    [SerializeField] private Renderer target;
    [SerializeField] private Color dimGlow = new Color(0.05f, 0.25f, 0.2f);
    [SerializeField] private Color brightGlow = new Color(0.4f, 2.5f, 2.0f);

    private bool lit;
    private MaterialPropertyBlock block;

    public bool IsLit => lit;
    public override string Prompt => lit ? "Quiet the stone" : "Touch the stone";

    private void Awake()
    {
        if (target == null) target = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        ApplyGlow();
    }

    public override void Use(PlayerInteractor user)
    {
        lit = !lit;
        ApplyGlow();
    }

    private void ApplyGlow()
    {
        if (target == null) return;
        target.GetPropertyBlock(block);
        block.SetColor("_EmissionColor", lit ? brightGlow : dimGlow);
        target.SetPropertyBlock(block);
    }
}
