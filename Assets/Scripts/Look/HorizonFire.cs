using UnityEngine;

/// Drives the burning ridge from LookTuning: glow strip brightness and color, and the
/// smoke and ember emission amounts. Put it on the Horizon root and fill the lists.
public class HorizonFire : MonoBehaviour
{
    [SerializeField] private LookTuning tuning;
    [SerializeField] private Renderer[] glowStrips;
    [SerializeField] private ParticleSystem[] smoke;
    [SerializeField] private ParticleSystem[] embers;

    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int IntensityId = Shader.PropertyToID("_Intensity");
    private MaterialPropertyBlock block;
    private float[] smokeBase, emberBase;

    private void Awake()
    {
        block = new MaterialPropertyBlock();
        smokeBase = BaseRates(smoke);
        emberBase = BaseRates(embers);
    }

    private static float[] BaseRates(ParticleSystem[] systems)
    {
        var rates = new float[systems.Length];
        for (int i = 0; i < systems.Length; i++) rates[i] = systems[i] != null ? systems[i].emission.rateOverTime.constant : 0f;
        return rates;
    }

    private void Update()
    {
        if (tuning == null) return;
        foreach (var r in glowStrips)
        {
            if (r == null) continue;
            r.GetPropertyBlock(block);
            block.SetColor(ColorId, tuning.fireGlowColor);
            block.SetFloat(IntensityId, tuning.fireGlowIntensity);
            r.SetPropertyBlock(block);
            r.enabled = tuning.fireGlowIntensity > 0.001f;
        }
        ApplyRates(smoke, smokeBase, tuning.fireSmoke);
        ApplyRates(embers, emberBase, tuning.fireEmbers);
    }

    private static void ApplyRates(ParticleSystem[] systems, float[] bases, float amount)
    {
        for (int i = 0; i < systems.Length; i++)
        {
            if (systems[i] == null) continue;
            var em = systems[i].emission;
            em.rateOverTime = bases[i] * amount;
        }
    }
}
