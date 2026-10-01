using UnityEngine;

/// Shapes of the Main3 valley that several recipes share (Tools/Recipes/main3_*.cs), so each reads the same outline. Editor only.
public static class ValleyShapes
{
    /// The old burn's four corners (Valley.md rev 11; 8.1 to 8.16 used these as a straight-edged quad).
    static readonly Vector2[] BurnCorners = { new Vector2(185f, 181f), new Vector2(340f, 213f), new Vector2(340f, 143f), new Vector2(185f, 151f) };
    /// RebuildSpecs.md 1.7 (Vesper, 2026-10-01): no straight burn edge over 20 m. Each side is cut into BurnStep m pieces and every
    /// point moved along the side's normal by up to BurnWobble m (Perlin noise, fixed offsets, so every recipe gets the same outline).
    const float BurnEastX = 340f, BurnStep = 5f, BurnWobble = 8f, BurnNoiseScale = 0.045f, BurnNoiseSeedX = 17.3f, BurnNoiseSeedY = 4.1f;
    static Vector2[] burn;

    public static Vector2[] BurnOutline()
    {
        if (burn != null) return burn;
        var pts = new System.Collections.Generic.List<Vector2>(); float run = 0f;
        for (int e = 0; e < BurnCorners.Length; e++)
        {
            Vector2 a = BurnCorners[e], b = BurnCorners[(e + 1) % BurnCorners.Length]; float len = Vector2.Distance(a, b);
            var t = (b - a) / len; var n = new Vector2(-t.y, t.x);
            for (float s = 0f; s < len; s += BurnStep, run += BurnStep)
            {
                float w = (Mathf.PerlinNoise(run * BurnNoiseScale + BurnNoiseSeedX, BurnNoiseSeedY) * 2f - 1f) * BurnWobble;
                if (a.x >= BurnEastX && b.x >= BurnEastX) w = -Mathf.Abs(w);
                var q = a + t * s + n * w; q.x = Mathf.Min(q.x, BurnEastX);   // the east side wobbles inward only: the lot and office stay out
                pts.Add(q);
            }
        }
        burn = pts.ToArray(); return burn;
    }

    public static bool InBurn(Vector2 p)
    {
        var poly = BurnOutline(); bool c = false;
        for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
            if (((poly[i].y > p.y) != (poly[j].y > p.y)) && (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) c = !c;
        return c;
    }

    /// Distance from p to the burn outline, positive inside the burn, negative outside.
    public static float BurnDepth(Vector2 p)
    {
        var poly = BurnOutline(); float d = float.MaxValue;
        for (int i = 0; i < poly.Length; i++)
        {
            Vector2 a = poly[i], b = poly[(i + 1) % poly.Length], ab = b - a;
            float u = Mathf.Clamp01(Vector2.Dot(p - a, ab) / Mathf.Max(ab.sqrMagnitude, 1e-4f)); d = Mathf.Min(d, Vector2.Distance(p, a + ab * u));
        }
        return InBurn(p) ? d : -d;
    }
}
