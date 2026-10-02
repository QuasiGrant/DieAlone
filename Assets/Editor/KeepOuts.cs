using UnityEngine;

/// Ground the forest recipes leave bare (NorthLayout.md draft 2, PLAN 8.24): no fir, sapling, bush, fern, log or stump of 8.16's grove
/// feet or 8.19's dense forest stands inside a zone, and main3_8_24_north.cs clears anything left in one. Giants are never removed by a
/// zone. Each zone is a circle, or a band of half width r round a segment a to b (a circle when a equals b). Editor only.
public static class KeepOuts
{
    public struct Zone
    {
        public string label; public Vector2 a, b; public float r;
        public Zone(string label, Vector2 a, Vector2 b, float r) { this.label = label; this.a = a; this.b = b; this.r = r; }
        public Zone(string label, Vector2 c, float r) : this(label, c, c, r) { }
        public bool Contains(Vector2 p)
        {
            var ab = b - a; float t = ab.sqrMagnitude < 1e-6f ? 0f : Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
            return Vector2.Distance(p, a + ab * t) <= r;
        }
    }

    /// NorthLayout.md draft 2 section 3 (K entries): N4 tent, N7 drag lane, N8 forage C, N9 SS1 and its found line, N10 SS2 and its line,
    /// N11 the fallen giant and its root plate, N13 and N15 the ruin and its sky gap, N14 the stovepipe approach line.
    public static readonly Zone[] North =
    {
        new Zone("N4 tent", new Vector2(271f, 234.5f), 6f),
        new Zone("N7 drag lane", new Vector2(292f, 247f), new Vector2(304f, 252f), 2.5f),
        new Zone("N8 forage C", new Vector2(229.5f, 273.5f), 3f),
        new Zone("N9 SS1", new Vector2(251.6f, 272.0f), 1.5f),
        new Zone("N9 SS1 found line", new Vector2(263.5f, 253.4f), new Vector2(251.6f, 272.0f), 1f),
        new Zone("N10 SS2", new Vector2(150.5f, 276.5f), 1.5f),
        new Zone("N10 SS2 found line", new Vector2(161.2f, 269.4f), new Vector2(150.5f, 276.5f), 1f),
        new Zone("N11 fallen giant", new Vector2(145.8f, 271.1f), new Vector2(131.5f, 257.3f), 3f),
        new Zone("N11 root plate", new Vector2(146.1f, 271.4f), 5f),   // the 4 m plate's radius 2 plus 3 m, on the axis's NE end
        new Zone("N13 N15 ruin and sky gap", new Vector2(172f, 281f), 8f),
        new Zone("N14 stovepipe line", new Vector2(192.7f, 276.6f), new Vector2(171.43f, 283.40f), 1.5f),
    };

    public static bool Contains(Vector2 p) { foreach (var z in North) if (z.Contains(p)) return true; return false; }
    public static bool Contains(Vector3 p) => Contains(new Vector2(p.x, p.z));
    public static string Which(Vector2 p) { foreach (var z in North) if (z.Contains(p)) return z.label; return null; }
}
