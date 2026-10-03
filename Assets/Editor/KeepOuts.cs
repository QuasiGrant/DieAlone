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
        new Zone("N8 forage C", new Vector2(228.73f, 271.05f), 3f),   // 8.24 gate round 2: the ring moved to the tread edge (main3_8_24_north.cs)
        new Zone("N9 SS1", new Vector2(251.6f, 272.0f), 1.5f),
        new Zone("N9 SS1 found line", new Vector2(263.5f, 253.4f), new Vector2(251.6f, 272.0f), 1f),
        new Zone("N10 SS2", new Vector2(150.5f, 276.5f), 1.5f),
        new Zone("N10 SS2 found line", new Vector2(161.2f, 269.4f), new Vector2(150.5f, 276.5f), 1f),
        new Zone("N11 fallen giant", new Vector2(145.8f, 271.1f), new Vector2(131.5f, 257.3f), 3f),
        new Zone("N11 root plate", new Vector2(146.1f, 271.4f), 5f),   // the 4 m plate's radius 2 plus 3 m, on the axis's NE end
        new Zone("N13 N15 ruin and sky gap", new Vector2(172f, 281f), 8f),
        new Zone("N14 stovepipe line", new Vector2(192.7f, 276.6f), new Vector2(171.43f, 283.40f), 1.5f),
        new Zone("5a TS1 tower screen", new Vector2(169.1f, 260.9f), 1.5f),   // NorthLayout 5a (round 2): the screen firs keep their ground
        new Zone("5a TS2 tower screen", new Vector2(178.5f, 263.8f), 1.5f),
        new Zone("TS1b tower screen", new Vector2(169.0f, 254.0f), 1.5f),   // Wren 2026-10-02: backing fir behind TS1
    };

    /// Camp2Layout.md draft 2 (PLAN 8.25): K1 the phone wire (no trunk within 1 m of the line, pole to booth), K4 the barrel, K6 the booth.
    public static readonly Zone[] Camp2 =
    {
        new Zone("K1 phone wire", new Vector2(272.8f, 72.0f), new Vector2(300.0f, 99.0f), 1f),
        new Zone("K4 barrel", new Vector2(291.4f, 101.4f), 2.5f),
        new Zone("K6 booth", new Vector2(299.2f, 98.2f), 3f),
        new Zone("K-top", new Vector2(293.0f, 121.0f), 8f),   // Camp2Layout.md draft 4 (PLAN 8.25a): no trunk within 8 m of the knob top
        new Zone("K-lamp RedPine3", new Vector2(261.6f, 134.8f), 4f),   // Wren 2026-10-03: its crown stood on every deck line to the lamp
        new Zone("K-lamp RedPine2", new Vector2(268.7f, 127.8f), 4f),
    };

    /// Camp3Layout.md draft 2 (PLAN 8.26): K1 the creek, 1.5 m each side of the water (the water 0.6 m each side of its line; the lower run's
    /// shifted stretch, z 123 to 110, gets 0.5 m more for the tread rule's push), K2 the Snag line's cleared fir, K3 the lamppost spot, K4 the
    /// W1 sign, K5 the east rim spot plus 1 m.
    public static readonly Zone[] Camp3 =
    {
        new Zone("K1 creek, plank bridge", new Vector2(105.32f, 205.23f), new Vector2(104.8f, 203.2f), 2.1f),
        new Zone("K1 creek, upper", new Vector2(104.8f, 203.2f), new Vector2(100.0f, 175.2f), 2.1f),
        new Zone("K1 creek, ravine", new Vector2(100.0f, 175.2f), new Vector2(84.3f, 150.95f), 2.1f),
        new Zone("K1 pool", new Vector2(82.1f, 149.7f), new Vector2(85.1f, 149.7f), 2.75f),
        new Zone("K1 creek, spring", new Vector2(85.8f, 126.0f), new Vector2(89.57f, 123.99f), 2.6f),
        new Zone("K1 creek, shifted", new Vector2(89.57f, 123.99f), new Vector2(101.13f, 110.99f), 2.6f),
        new Zone("K1 creek, lower", new Vector2(101.13f, 110.99f), new Vector2(128.0f, 78.0f), 2.6f),
        new Zone("K1 creek, stones", new Vector2(128.0f, 78.0f), new Vector2(131.71f, 72.82f), 2.1f),
        new Zone("K1 creek, mouth", new Vector2(131.71f, 72.82f), new Vector2(136.5f, 66.0f), 2.1f),
        new Zone("K2 Snag line", new Vector2(96.52f, 155.38f), 2f),
        new Zone("K3 lamppost spot", new Vector2(58f, 150f), 3f),
        new Zone("K4 W1 sign", new Vector2(127.0f, 72.8f), 2f),
        new Zone("K5 east rim spot", new Vector2(93f, 138.5f), new Vector2(95f, 138.5f), 2.5f),   // x 92 to 96, z 137 to 140, plus 1 m
    };

    /// PLAN 8.24a (NorthLayout 5.4): the spots main3_8_24a_camp1_deck.cs moved three Grove_C1Ring sequoias from, so the deck sees Camp 1.
    public static readonly Zone[] Camp1 =
    {
        new Zone("C1 deck spot, Sequoia5 west", new Vector2(247.1f, 220.9f), 4f),
        new Zone("C1 deck spot, Sequoia5 south", new Vector2(253.7f, 211.7f), 4f),
        new Zone("C1 deck spot, Sequoia3", new Vector2(256.3f, 219.7f), 4f),
    };

    /// CaveLayout.md draft 2 doc 2.2 (PLAN 8.27; Wren 2026-10-03): C1 the mouth strip, x 52 to 54.5, z 38 to 46.5, kept clear of trees.
    public static readonly Zone[] Cave =
    {
        new Zone("C1 mouth strip", new Vector2(53.25f, 39.25f), new Vector2(53.25f, 45.25f), 1.25f),
    };

    public static bool Contains(Vector2 p) => Which(p) != null;
    public static bool Contains(Vector3 p) => Contains(new Vector2(p.x, p.z));
    public static string Which(Vector2 p) { foreach (var z in North) if (z.Contains(p)) return z.label; var c2 = WhichCamp2(p); return c2 ?? WhichCamp3(p) ?? WhichCave(p) ?? WhichCamp1(p); }
    public static string WhichCamp2(Vector2 p) { foreach (var z in Camp2) if (z.Contains(p)) return z.label; return null; }
    public static string WhichCamp3(Vector2 p) { foreach (var z in Camp3) if (z.Contains(p)) return z.label; return null; }
    public static string WhichCave(Vector2 p) { foreach (var z in Cave) if (z.Contains(p)) return z.label; return null; }
    public static string WhichCamp1(Vector2 p) { foreach (var z in Camp1) if (z.Contains(p)) return z.label; return null; }
}
