// Main3 task 8.3: trails flattened and painted along every route in Main3.md 4, points of interest as gray stand-ins,
// gray giant trees (hero trunks with mesh colliders), and the thicket that keeps walkers on the trails. Run after 8.2 in Main3, edit mode.
// Grades: every leg at 25 percent or less (DailyLoop.md, rev 13 3.4.7), the log steps excepted. J to Ward is built at 157 m with a 24 percent clamp
// (table 150, within 5 percent) so its climb to the last bend stays under 25 percent on the ground itself.
// Trails: the map curve (Main3_map.svg) is the centre line; ends and on-trail points of interest are anchors; between
// anchors the trail meanders (half-waves about 18 m long) with one amplitude factor per leg, solved so the walked
// length matches table 4 (Docs/Design/Main3_BuildNotes.md). Shore legs meander only to the land side.
// Prints each route's length against Main3.md and the distance along the leg of each point of interest.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Camp") == null) return "run 8.2 first";
if (Root("Trails") != null) return "Trails already exist; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float baseY = terrain.transform.position.y; var size = data.size;
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + baseY;
float SS(float a) => UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01(a));
float L(float a, float b, float t) => UnityEngine.Mathf.Lerp(a, b, t);
var lakeC = P(190, 60); const float lakeA = 54.8f, lakeB = 27.6f;
float LakeRe(UnityEngine.Vector2 p) { var q = p - lakeC; return UnityEngine.Mathf.Sqrt((q.x / lakeA) * (q.x / lakeA) + (q.y / lakeB) * (q.y / lakeB)); }

// ---------- legs: map control points (world metres), target length (table 4), points of interest ----------
// poi kind: "on" = the trail passes through it; "side" = stands beside the trail
var legs = new System.Collections.Generic.List<(string name, UnityEngine.Vector2[] ctrl, bool polyline, float target, bool shore, float maxGrade, (string n, UnityEngine.Vector2 p, string kind)[] pois)>
{
    ("Camp to pump", new[] { P(170,160), P(174,128), P(190,96) }, false, 80f, false, 0.25f, new[] { ("Water tank", P(182,128), "side") }),
    ("Pump to boathouse", new[] { P(190,96), P(232,92), P(247.6f,52.4f) }, false, 84f, true, 0.25f, new[] { ("Overturned rowboat", P(223.6f,83.2f), "side") }),
    ("Boathouse to Camp 2", new[] { P(247.6f,52.4f), P(280,64), P(292,108) }, false, 95f, false, 0.25f, new[] { ("Phone pole", P(272.8f,72f), "side") }),
    ("Camp 2 to T", new[] { P(292,108), P(324,128), P(340,170) }, false, 94f, false, 0.25f, new[] { ("Food lockers", P(320,133.6f), "side") }),
    ("Camp to Jg", new[] { P(170,160), P(200,136), P(232,152), P(262,172) }, false, 125f, false, 0.25f, new[] { ("Hollow Giant", P(202,140), "tree"), ("Forage patch A", P(240,162.8f), "side") }),
    ("Jg to T", new[] { P(262,172), P(288,196), P(316,148), P(340,170) }, false, 105f, false, 0.25f, new[] { ("Gate Tree", P(290,176), "tree"), ("First sight of the lot", P(328,164), "side") }),
    ("Jg to Camp 1", new[] { P(262,172), P(264,208), P(282,238) }, false, 83f, false, 0.25f, new[] { ("Latrine shed", P(268,206.4f), "side") }),
    ("Camp to Camp 3", new[] { P(170,160), P(144,184), P(108,128), P(78,146) }, false, 126f, false, 0.25f, new[] { ("Forage patch B", P(142.4f,163.6f), "side"), ("Log steps", P(96,144.8f), "steps") }),
    ("Pump to W1", new[] { P(190,96), P(156,92), P(128,70) }, false, 80f, true, 0.25f, new[] { ("Washed-out truck", P(157.6f,87.6f), "side"), ("Stepping stones", P(131.8f,72.7f), "on") }),
    ("W1 to Camp 3", new[] { P(128,70), P(96,104), P(78,146) }, false, 114f, false, 0.25f, new[] { ("Footbridge", P(114.8f,85.2f), "on"), ("Camper trailer", P(94,114.4f), "side") }),
    ("W1 to cave", new[] { P(128,70), P(108,48), P(84,64), P(52,37.5f) }, false, 109f, false, 0.25f, new[] { ("Rope handrail", P(106.8f,57.6f), "on"), ("Coloured bulbs", P(82,52), "side") }),
    ("Camp to J", new[] { P(170,160), P(136,196), P(104,206) }, false, 96f, false, 0.25f, new[] { ("Burn-map board", P(136.4f,189.6f), "side"), ("Plank bridge", P(104.8f,203.2f), "on") }),
    // rev 13 (3.6): round the Tor's south and west feet to the last bend (60, 250), then a straight run west over the low crest to the rock lip
    ("J to Ward", new[] { P(104,206), P(88,200), P(66,204), P(54,218), P(56,236), P(60,250), P(27,251), P(14.5f,252) }, true, 157f, false, 0.24f, new[] { ("Rune post", P(66,201.5f), "side"), ("Last bend", P(60,250), "bend") }),
};

// ---------- centre line sampling ----------
System.Collections.Generic.List<UnityEngine.Vector2> Centre((string name, UnityEngine.Vector2[] ctrl, bool polyline, float target, bool shore, float maxGrade, (string n, UnityEngine.Vector2 p, string kind)[] pois) leg)
{
    var pts = new System.Collections.Generic.List<UnityEngine.Vector2>(); var c = leg.ctrl;
    if (leg.polyline) { for (int i = 0; i < c.Length - 1; i++) for (int k = 0; k < 40; k++) pts.Add(UnityEngine.Vector2.Lerp(c[i], c[i + 1], k / 40f)); pts.Add(c[c.Length - 1]); return pts; }
    for (int k = 0; k <= 600; k++)
    {
        float t = k / 600f, u = 1 - t;
        pts.Add(c.Length == 3 ? u * u * c[0] + 2 * t * u * c[1] + t * t * c[2] : u * u * u * c[0] + 3 * t * u * u * c[1] + 3 * t * t * u * c[2] + t * t * t * c[3]);
    }
    return pts;
}
float[] Arc(System.Collections.Generic.List<UnityEngine.Vector2> pts) { var s = new float[pts.Count]; for (int i = 1; i < pts.Count; i++) s[i] = s[i - 1] + UnityEngine.Vector2.Distance(pts[i - 1], pts[i]); return s; }
UnityEngine.Vector2 At(System.Collections.Generic.List<UnityEngine.Vector2> pts, float[] s, float d, out UnityEngine.Vector2 tan)
{
    int i = 1; while (i < pts.Count - 1 && s[i] < d) i++;
    float t = UnityEngine.Mathf.InverseLerp(s[i - 1], s[i], d); tan = (pts[i] - pts[i - 1]).normalized;
    return UnityEngine.Vector2.Lerp(pts[i - 1], pts[i], t);
}
float Project(System.Collections.Generic.List<UnityEngine.Vector2> pts, float[] s, UnityEngine.Vector2 p, out float dist)
{
    float best = float.MaxValue, bs = 0f;
    for (int i = 0; i < pts.Count - 1; i++)
    {
        var a = pts[i]; var ab = pts[i + 1] - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-6f));
        float d = UnityEngine.Vector2.Distance(p, a + ab * t); if (d < best) { best = d; bs = s[i] + t * (s[i + 1] - s[i]); }
    }
    dist = best; return bs;
}
float PolyLen(System.Collections.Generic.List<UnityEngine.Vector2> p) { float l = 0; for (int i = 1; i < p.Count; i++) l += UnityEngine.Vector2.Distance(p[i - 1], p[i]); return l; }

// table 2.1 heights at the named trail ends; camp is the 15 m knoll top (rev 13, 8.1), so the camp ends meet it level
var namedEnds = new (UnityEngine.Vector2 p, float h)[] { (P(170,160), 15f), (P(190,96), -4.5f), (P(128,70), -4.5f), (P(104,206), 10f), (P(262,172), 5f), (P(340,170), 3f), (P(282,238), 5f), (P(292,108), 4f), (P(78,146), -4f), (P(52,37.5f), -6f), (P(14.5f,252), 36f) };
var built = new System.Collections.Generic.List<(string name, System.Collections.Generic.List<UnityEngine.Vector2> path, float[] prof, float target)>();
var poiPlaced = new System.Collections.Generic.List<(string leg, string n, UnityEngine.Vector2 at, UnityEngine.Vector2 tan, UnityEngine.Vector2 obj, string kind, float along, float height)>();
var report = new System.Text.StringBuilder();
foreach (var leg in legs)
{
    var C = Centre(leg); var S = Arc(C); float Lc = S[S.Length - 1];
    // anchors: ends and every "on"/"steps" point (projected); side points anchor at their projection too
    var anchors = new System.Collections.Generic.List<float> { 0f, Lc };
    foreach (var poi in leg.pois) { float s0 = Project(C, S, poi.p, out _); if (s0 > 2f && s0 < Lc - 2f) anchors.Add(s0); }
    anchors.Sort();
    int stepsSeg = -1;   // Camp to Camp 3: no meander from the log steps to the hollow floor
    int straightFrom = int.MaxValue;   // J to Ward: no meander after the last bend (the straight run west, rev 13)
    for (int i = 0; i < anchors.Count - 1; i++) foreach (var poi in leg.pois) if (poi.kind == "bend" && UnityEngine.Mathf.Abs(anchors[i] - Project(C, S, poi.p, out _)) < 0.01f) straightFrom = i;
    for (int i = 0; i < anchors.Count - 1; i++) foreach (var poi in leg.pois) if (poi.kind == "steps" && UnityEngine.Mathf.Abs(anchors[i] - Project(C, S, poi.p, out _)) < 0.01f) stepsSeg = i;
    // shore legs: pick the side away from the lake
    float side = 1f;
    if (leg.shore) { var mid = At(C, S, Lc * 0.5f, out var tm); var nrm = P(-tm.y, tm.x); side = LakeRe(mid + nrm * 3f) > LakeRe(mid - nrm * 3f) ? 1f : -1f; }
    // hero trees beside the trail: a smooth detour (window +-15 m) keeps the trail edge 2.5 m clear of the bark
    var detours = new System.Collections.Generic.List<(float s, float off)>();
    foreach (var poi in leg.pois) if (poi.kind == "tree")
    {
        float r = poi.n == "Hollow Giant" ? 4.5f : 4f;
        float s0 = Project(C, S, poi.p, out float dd); var c0 = At(C, S, s0, out var t0);
        float sideOf = UnityEngine.Mathf.Sign(t0.x * (poi.p.y - c0.y) - t0.y * (poi.p.x - c0.x));   // + = tree on the left
        float need = r + 2.5f - dd; if (need > 0f) detours.Add((s0, -sideOf * need));
    }
    System.Collections.Generic.List<UnityEngine.Vector2> Build(float k)
    {
        var outp = new System.Collections.Generic.List<UnityEngine.Vector2>();
        for (float d = 0f; d <= Lc + 1e-3f; d += 0.5f)
        {
            int seg = 0; while (seg < anchors.Count - 2 && d > anchors[seg + 1]) seg++;
            float a0 = anchors[seg], a1 = anchors[seg + 1], l = a1 - a0; float u = l > 1e-3f ? (d - a0) / l : 0f;
            int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(l / 18f)); float amp = seg == stepsSeg || seg >= straightFrom ? 0f : k * l / n;
            // times sin(pi u): zero offset and zero slope at every anchor, so the trail runs straight through anchored pieces
            float f = UnityEngine.Mathf.Sin(UnityEngine.Mathf.PI * n * u) * UnityEngine.Mathf.Sin(UnityEngine.Mathf.PI * u); if (leg.shore) f = UnityEngine.Mathf.Abs(f) * side;
            float off = amp * f;
            foreach (var dt in detours) { float x = (d - dt.s) / 15f; if (UnityEngine.Mathf.Abs(x) < 1f) { float w = UnityEngine.Mathf.Cos(x * UnityEngine.Mathf.PI * 0.5f); off += dt.off * w * w; } }
            var c = At(C, S, UnityEngine.Mathf.Min(d, Lc), out var tan); var q = c + P(-tan.y, tan.x) * off;
            // shore legs keep 6 percent of the lake radii off the water line, so no trail runs through the shallows (8.9a)
            if (leg.shore) { float re = LakeRe(q); if (re < 1.06f) q = lakeC + (q - lakeC) * (1.06f / re); }
            // the Ward climb keeps 21 m from the Tor's centre (its skirt is 18 m round at the foot, rev 13 route round it)
            if (leg.name == "J to Ward") { var tq = q - P(76, 223); if (tq.magnitude < 21f) q = P(76, 223) + tq.normalized * 21f; }
            outp.Add(q);
        }
        return outp;
    }
    float lo = 0f, hi = 1.5f; var path = Build(0f);
    if (PolyLen(path) < leg.target) { for (int it = 0; it < 40; it++) { float mid = (lo + hi) * 0.5f; if (PolyLen(Build(mid)) < leg.target) lo = mid; else hi = mid; } path = Build((lo + hi) * 0.5f); }
    // height profile: terrain along the path, smoothed over 8 m, grade clamped both ways; the log steps segment may reach 0.85
    int N = path.Count; var raw = new float[N]; var prof = new float[N];
    for (int i = 0; i < N; i++) raw[i] = H(path[i].x, path[i].y);
    for (int i = 0; i < N; i++) { float sum = 0; int cnt = 0; for (int j = UnityEngine.Mathf.Max(0, i - 8); j <= UnityEngine.Mathf.Min(N - 1, i + 8); j++) { sum += raw[j]; cnt++; } prof[i] = sum / cnt; }
    // pin both ends: to the table 2.1 height where the end is a named point (so J is 10 beside its creek and the pump -4.5), else the ground there; legs meeting at a point then agree
    float EndH(UnityEngine.Vector2 q) { foreach (var np in namedEnds) if (UnityEngine.Vector2.Distance(q, np.p) < 1f) return np.h; return H(q.x, q.y); }
    prof[0] = EndH(path[0]); prof[N - 1] = EndH(path[N - 1]);
    float stepsFrom = stepsSeg >= 0 ? anchors[stepsSeg] - 1f : float.MaxValue;
    // largest height change between samples i - 1 and i, from their real spacing (the meander stretches samples past 0.5 m)
    // gradeMargin keeps the built ground under the leg's grade after the terrain blend rounds the profile
    const float gradeMargin = 0.98f;
    float G(int i) => (i * 0.5f >= stepsFrom ? 0.85f : leg.maxGrade * gradeMargin) * UnityEngine.Vector2.Distance(path[i - 1], path[i]);
    // band first (8.9c re-walk 2): each point within reach of both pinned ends at the leg's grade, so the neighbour clamps
    // below cannot leave an end short (a tight leg such as camp to pump, 19.5 m down in 80 m, came out 6.7 m under the camp end)
    var fromA = new float[N]; var fromB = new float[N];
    for (int i = 1; i < N; i++) fromA[i] = fromA[i - 1] + G(i);
    for (int i = N - 2; i >= 0; i--) fromB[i] = fromB[i + 1] + G(i + 1);
    for (int i = 1; i < N - 1; i++)
    {
        float bLo = UnityEngine.Mathf.Max(prof[0] - fromA[i], prof[N - 1] - fromB[i]), bHi = UnityEngine.Mathf.Min(prof[0] + fromA[i], prof[N - 1] + fromB[i]);
        if (bLo > bHi) return leg.name + ": the ends are too far apart in height for the grade (" + prof[0] + " to " + prof[N - 1] + " over " + PolyLen(path).ToString("F1") + " m)";
        prof[i] = UnityEngine.Mathf.Clamp(prof[i], bLo, bHi);
    }
    for (int pass = 0; pass < 200; pass++)   // ends stay pinned; interior points clamp toward their neighbours until nothing moves
    {
        float moved = 0f;
        for (int i = 1; i < N - 1; i++) { float c = UnityEngine.Mathf.Clamp(prof[i], prof[i - 1] - G(i), prof[i - 1] + G(i)); moved = UnityEngine.Mathf.Max(moved, UnityEngine.Mathf.Abs(c - prof[i])); prof[i] = c; }
        for (int i = N - 2; i >= 1; i--) { float c = UnityEngine.Mathf.Clamp(prof[i], prof[i + 1] - G(i + 1), prof[i + 1] + G(i + 1)); moved = UnityEngine.Mathf.Max(moved, UnityEngine.Mathf.Abs(c - prof[i])); prof[i] = c; }
        if (moved < 0.0001f) break;
    }
    // log steps: one straight grade from the steps anchor to where the profile reaches the hollow floor, so terrain and ramp agree
    if (stepsSeg >= 0)
    {
        int i0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt(anchors[stepsSeg] / 0.5f), 0, N - 1); float floorH = prof[N - 1];
        int ie = i0; while (ie < N - 1 && prof[ie] > floorH + 0.05f) ie++;
        for (int i = i0; i <= ie; i++) prof[i] = L(prof[i0], prof[ie], (i - i0) / (float)UnityEngine.Mathf.Max(1, ie - i0));
    }
    built.Add((leg.name, path, prof, leg.target));
    float len = PolyLen(path);
    report.Append(leg.name + ": " + len.ToString("F1") + " m vs " + leg.target + " (" + (100f * (len - leg.target) / leg.target).ToString("+0.0;-0.0") + "%)");
    var ps = Arc(path);
    foreach (var poi in leg.pois)
    {
        // on-trail pieces sit on the anchor (where the meander crosses the centre line, so the trail runs straight through them)
        var aim = poi.kind == "on" || poi.kind == "steps" ? At(C, S, Project(C, S, poi.p, out _), out _) : poi.p;
        float s0 = Project(path, ps, aim, out float dd); var at = At(path, ps, s0, out var tan);
        int idx = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt(s0 / 0.5f), 0, N - 1);
        var obj = poi.p;
        if (poi.kind == "side" && dd < 3.5f) { var nrm = P(-tan.y, tan.x); float sgn = LakeRe(at + nrm * 4f) >= LakeRe(at - nrm * 4f) ? 1f : -1f; obj = at + nrm * sgn * 4f; }
        if (poi.kind == "on" || poi.kind == "steps") obj = at;
        poiPlaced.Add((leg.name, poi.n, at, tan, obj, poi.kind, s0, prof[idx]));
        report.Append(" | " + poi.n + " at " + s0.ToString("F0") + " m");
    }
    report.Append("\n");
}
// lot walks across the front zone (not painted; the lot is 8.6's gravel): straight segments to the doors
var lot = new (string n, UnityEngine.Vector2[] p, float target)[] {
    ("T to office", new[] { P(340,170), P(352,172), P(352,188), P(350,196) }, 35f),
    ("T to store", new[] { P(340,170), P(352,172), P(360,188), P(366,197.2f) }, 42f),
    ("T to booth", new[] { P(340,170), P(390,171), P(391.5f,174) }, 52f) };
foreach (var w in lot) { float l = PolyLen(new System.Collections.Generic.List<UnityEngine.Vector2>(w.p)); report.Append(w.n + " (lot walk): " + l.ToString("F1") + " m vs " + w.target + " (" + (100f * (l - w.target) / w.target).ToString("+0.0;-0.0") + "%)\n"); }

// inside the keeper's camp clearing the trails are not built: the tower and cabin stand there and the clearing is flat
bool InCamp(UnityEngine.Vector2 q) => UnityEngine.Vector2.Distance(q, P(170, 160)) < 15f;
// ---------- flatten: 1.5 m at the profile height, blend over 2.5 m ----------
int res = data.heightmapResolution; var hm = data.GetHeights(0, 0, res, res);
var bestD = new float[res, res]; var sumW = new float[res, res]; var sumH = new float[res, res];   // weighted blend of nearby samples, no cliffs where trails meet
for (int z = 0; z < res; z++) for (int x = 0; x < res; x++) bestD[z, x] = float.MaxValue;
float cellX = size.x / (res - 1), cellZ = size.z / (res - 1); const float flatR = 1.5f, blendR = 2.5f, reach = flatR + blendR;
foreach (var b in built)
    for (int i = 0; i < b.path.Count; i++)
    {
        var p = b.path[i]; if (InCamp(p)) continue;
        int x0 = UnityEngine.Mathf.Max(0, (int)((p.x - reach) / cellX)), x1 = UnityEngine.Mathf.Min(res - 1, (int)((p.x + reach) / cellX) + 1);
        int z0 = UnityEngine.Mathf.Max(0, (int)((p.y - reach) / cellZ)), z1 = UnityEngine.Mathf.Min(res - 1, (int)((p.y + reach) / cellZ) + 1);
        for (int z = z0; z <= z1; z++) for (int x = x0; x <= x1; x++)
        {
            float d = UnityEngine.Vector2.Distance(p, P(x * cellX, z * cellZ));
            if (d >= reach) continue;
            if (d < bestD[z, x]) bestD[z, x] = d;
            float wk = UnityEngine.Mathf.Exp(-(d * d) / (1.2f * 1.2f)); sumW[z, x] += wk; sumH[z, x] += wk * b.prof[i];
        }
    }
for (int z = 0; z < res; z++) for (int x = 0; x < res; x++)
{
    float d = bestD[z, x]; if (d >= reach) continue;
    float w = d <= flatR ? 1f : 1f - SS((d - flatR) / blendR);
    if (sumW[z, x] < 1e-6f) continue;
    float cur = hm[z, x] * size.y + baseY; float nh = L(cur, sumH[z, x] / sumW[z, x], w);
    hm[z, x] = UnityEngine.Mathf.Clamp01((nh - baseY) / size.y);
}
data.SetHeights(0, 0, hm);

// ---------- paint: Trail layer (index 3) 1.2 m, soft to 1.8 m ----------
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int layersN = alpha.GetLength(2);
float aX = size.x / ares, aZ = size.z / ares;
var aD = new float[ares, ares]; for (int z = 0; z < ares; z++) for (int x = 0; x < ares; x++) aD[z, x] = float.MaxValue;
foreach (var b in built) foreach (var p in b.path) if (!InCamp(p))
{
    int x0 = UnityEngine.Mathf.Max(0, (int)((p.x - 2f) / aX)), x1 = UnityEngine.Mathf.Min(ares - 1, (int)((p.x + 2f) / aX) + 1);
    int z0 = UnityEngine.Mathf.Max(0, (int)((p.y - 2f) / aZ)), z1 = UnityEngine.Mathf.Min(ares - 1, (int)((p.y + 2f) / aZ) + 1);
    for (int z = z0; z <= z1; z++) for (int x = x0; x <= x1; x++) { float d = UnityEngine.Vector2.Distance(p, P((x + 0.5f) * aX, (z + 0.5f) * aZ)); if (d < aD[z, x]) aD[z, x] = d; }
}
for (int z = 0; z < ares; z++) for (int x = 0; x < ares; x++)
{
    float w = 1f - UnityEngine.Mathf.Clamp01((aD[z, x] - 1.2f) / 0.6f); if (w <= 0f) continue;
    for (int k = 0; k < layersN; k++) alpha[z, x, k] *= 1f - w; alpha[z, x, 3] += w;
}
data.SetAlphamaps(0, 0, alpha);
UnityEditor.EditorUtility.SetDirty(data);

// ---------- gray stand-ins ----------
var trailsRoot = new UnityEngine.GameObject("Trails");
var poiRoot = new UnityEngine.GameObject("PointsOfInterest");
UnityEngine.GameObject Group(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, float yaw) { var g = new UnityEngine.GameObject(name); g.transform.SetParent(parent, false); g.transform.position = pos; g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); return g; }
UnityEngine.GameObject Prim(UnityEngine.PrimitiveType t, string name, UnityEngine.Transform parent, UnityEngine.Vector3 lp, UnityEngine.Vector3 sc, UnityEngine.Vector3? eul = null)
{
    var g = UnityEngine.GameObject.CreatePrimitive(t); g.name = name; g.transform.SetParent(parent, false); g.transform.localPosition = lp; g.transform.localScale = sc;
    if (eul.HasValue) g.transform.localRotation = UnityEngine.Quaternion.Euler(eul.Value); return g;
}
var Cube = UnityEngine.PrimitiveType.Cube; var Cyl = UnityEngine.PrimitiveType.Cylinder; var Sph = UnityEngine.PrimitiveType.Sphere;
// centre-line markers: one empty per leg holding its centre line as child points every 2 m (walk checks use them)
foreach (var b in built)
{
    var g = new UnityEngine.GameObject(b.name); g.transform.SetParent(trailsRoot.transform, false);
    for (int i = 0; i < b.path.Count; i += 4) { if (InCamp(b.path[i])) continue; var m = new UnityEngine.GameObject("P" + (i / 2)); m.transform.SetParent(g.transform, false); m.transform.position = V(b.path[i].x, b.prof[i], b.path[i].y); }
}
// side pieces keep their footprint radius plus 1.8 m clear of every trail centre line (meanders can loop back toward them)
var poiRadius = new System.Collections.Generic.Dictionary<string, float> {
    { "Water tank", 1.5f }, { "Overturned rowboat", 2f }, { "Phone pole", 1f }, { "Food lockers", 3.6f }, { "First sight of the lot", 0.3f },
    { "Latrine shed", 2.4f }, { "Forage patch A", 2f }, { "Forage patch B", 2f }, { "Washed-out truck", 2.6f }, { "Camper trailer", 2.8f },
    { "Coloured bulbs", 1.3f }, { "Burn-map board", 1f }, { "Rune post", 0.3f } };
var allTrail = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (var b in built) allTrail.AddRange(b.path);
for (int qi = 0; qi < poiPlaced.Count; qi++)
{
    var q = poiPlaced[qi]; if (q.kind != "side") continue;
    float need = poiRadius[q.n] + 1.8f; var o = q.obj;
    for (int it = 0; it < 30; it++)
    {
        float dn = float.MaxValue; var nearest = o;
        foreach (var t in allTrail) { float d = UnityEngine.Vector2.Distance(o, t); if (d < dn) { dn = d; nearest = t; } }
        if (dn >= need) break;
        var away = (o - nearest).sqrMagnitude > 1e-6f ? (o - nearest).normalized : P(1f, 0f);
        o += away * (need - dn + 0.2f);
    }
    q.obj = o; poiPlaced[qi] = q;
}
foreach (var q in poiPlaced)
{
    if (q.kind == "tree") continue;   // hero trees are built with the giants
    float yaw = UnityEngine.Mathf.Atan2(q.tan.x, q.tan.y) * UnityEngine.Mathf.Rad2Deg;   // local +z along the trail
    float gy = q.kind == "side" ? H(q.obj.x, q.obj.y) : q.height;
    var g = Group("POI_" + q.n.Replace(" ", "_").Replace("-", "_"), poiRoot.transform, V(q.obj.x, gy, q.obj.y), yaw).transform;
    switch (q.n)
    {
        case "Water tank": foreach (var sx in new[] { -1f, 1f }) foreach (var sz in new[] { -1f, 1f }) Prim(Cube, "Leg", g, V(sx, 1.5f, sz), V(0.15f, 3f, 0.15f)); Prim(Cyl, "Tank", g, V(0f, 4.2f, 0f), V(2.6f, 1.2f, 2.6f)); break;
        case "Overturned rowboat": Prim(Cube, "Hull", g, V(0f, 0.3f, 0f), V(1.3f, 0.6f, 3.8f), V(0f, 30f, 180f)); break;
        case "Phone pole": Prim(Cyl, "Pole", g, V(0f, 4f, 0f), V(0.3f, 4f, 0.3f)); Prim(Cube, "Handset", g, V(0f, 1.4f, 0.25f), V(0.4f, 0.5f, 0.25f)); Prim(Cube, "Crossarm", g, V(0f, 7.6f, 0f), V(1.8f, 0.12f, 0.12f)); break;
        case "Food lockers": for (int i = 0; i < 6; i++) { float a = i * 60f * UnityEngine.Mathf.Deg2Rad; Prim(Cube, "Locker", g, V(UnityEngine.Mathf.Sin(a) * 3f, 0.6f, UnityEngine.Mathf.Cos(a) * 3f), V(1f, 1.2f, 0.8f), V(0f, i * 60f, 0f)); } break;
        case "First sight of the lot": Prim(Cube, "Post", g, V(0f, 0.6f, 0f), V(0.2f, 1.2f, 0.2f)); break;
        case "Latrine shed": Prim(Cube, "Shed", g, V(0f, 1.1f, 0f), V(1.3f, 2.2f, 1.3f)); Prim(Cube, "WashStand", g, V(1.6f, 0.45f, 0f), V(0.8f, 0.9f, 0.5f)); break;
        case "Forage patch A": case "Forage patch B": for (int i = 0; i < 5; i++) { float a = i * 72f * UnityEngine.Mathf.Deg2Rad; Prim(Sph, "Bush", g, V(UnityEngine.Mathf.Sin(a) * 1.4f, 0.4f, UnityEngine.Mathf.Cos(a) * 1.4f), V(1.1f, 0.8f, 1.1f)); } break;
        case "Washed-out truck": Prim(Cube, "Bed", g, V(0f, 0.8f, -0.8f), V(2f, 1f, 3.2f), V(0f, 0f, 8f)); Prim(Cube, "Cab", g, V(0f, 1.1f, 1.6f), V(2f, 1.6f, 1.6f), V(0f, 0f, 8f)); break;
        case "Camper trailer": Prim(Cube, "Body", g, V(0f, 1.3f, 0f), V(2.3f, 2.2f, 5f), V(0f, 0f, -6f)); break;
        case "Coloured bulbs": Prim(Cyl, "DeadBranch", g, V(0f, 1.5f, 0f), V(0.25f, 1.5f, 0.25f)); Prim(Cube, "BulbString", g, V(0f, 2.6f, 1.2f), V(0.05f, 0.05f, 2.4f)); break;
        case "Burn-map board": Prim(Cube, "PostL", g, V(-0.8f, 0.9f, 0f), V(0.12f, 1.8f, 0.12f)); Prim(Cube, "PostR", g, V(0.8f, 0.9f, 0f), V(0.12f, 1.8f, 0.12f)); Prim(Cube, "Board", g, V(0f, 1.4f, 0f), V(1.8f, 1.0f, 0.08f)); break;
        case "Rune post": Prim(Cube, "Post", g, V(0f, 1f, 0f), V(0.3f, 2f, 0.3f)); break;
        // on-trail pieces are looks only: the player walks on the flattened trail under them, so no lip can stop the capsule
        case "Stepping stones": for (int i = -2; i <= 2; i++) UnityEngine.Object.DestroyImmediate(Prim(Cyl, "Stone", g, V(i * 1.1f, -0.1f, 0f), V(0.9f, 0.12f, 0.9f)).GetComponent<UnityEngine.Collider>()); break;
        case "Footbridge":
            UnityEngine.Object.DestroyImmediate(Prim(Cube, "Deck", g, V(0f, -0.06f, 0f), V(2.6f, 0.12f, 4.5f)).GetComponent<UnityEngine.Collider>());
            foreach (var sx in new[] { -1.3f, 1.3f }) UnityEngine.Object.DestroyImmediate(Prim(Cube, "Rail", g, V(sx, 0.5f, 0f), V(0.08f, 1.0f, 4.5f)).GetComponent<UnityEngine.Collider>()); break;   // looks only: the rails caught the player (Marlow finding 10); the thicket keeps walkers on the trail
        case "Plank bridge": UnityEngine.Object.DestroyImmediate(Prim(Cube, "Deck", g, V(0f, -0.06f, 0f), V(2.6f, 0.12f, 3.5f)).GetComponent<UnityEngine.Collider>()); break;   // low planks, no rails: J opens off its end
        case "Rope handrail": for (int i = -3; i <= 3; i++) Prim(Cube, "Post", g, V(1.2f, 0.5f, i * 2f), V(0.1f, 1.0f, 0.1f)); Prim(Cube, "Rope", g, V(1.2f, 0.95f, 0f), V(0.04f, 0.04f, 12f)); break;
        case "Log steps":
        {
            // STAIRS RULE: logs for looks, a collider-only ramp along the trail profile from the rim to the hollow floor
            var b = built.Find(x => x.name == q.leg); var ps = Arc(b.path); int i0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt(q.along / 0.5f), 0, b.path.Count - 1);
            var a = b.path[i0]; var e = b.path[b.path.Count - 1]; float ya = b.prof[i0], ye = b.prof[b.prof.Length - 1];
            int ie = i0; while (ie < b.path.Count - 1 && b.prof[ie] > ye + 0.05f) ie++;
            e = b.path[ie]; ye = b.prof[ie];
            var d3 = V(e.x - a.x, ye - ya, e.y - a.y); int logs = UnityEngine.Mathf.Max(2, UnityEngine.Mathf.RoundToInt(UnityEngine.Mathf.Abs(ye - ya) / 0.4f));
            g.position = V(a.x, ya, a.y); g.rotation = UnityEngine.Quaternion.LookRotation(V(d3.x, 0f, d3.z).normalized, UnityEngine.Vector3.up);
            float hl = new UnityEngine.Vector2(d3.x, d3.z).magnitude;
            for (int k = 1; k <= logs; k++) { var lg = Prim(Cyl, "Log", g, V(0f, d3.y * k / logs - 0.1f, hl * k / logs), V(0.3f, 0.9f, 0.3f), V(0f, 0f, 90f)); UnityEngine.Object.DestroyImmediate(lg.GetComponent<UnityEngine.Collider>()); }
            var ramp = new UnityEngine.GameObject("StairRamp"); ramp.transform.SetParent(g, false);
            float slopeLen = d3.magnitude, pitch = -UnityEngine.Mathf.Atan2(d3.y, hl) * UnityEngine.Mathf.Rad2Deg;
            ramp.transform.localPosition = V(0f, d3.y * 0.5f - 0.1f, hl * 0.5f); ramp.transform.localRotation = UnityEngine.Quaternion.Euler(pitch, 0f, 0f);
            ramp.transform.localScale = V(1.8f, 0.2f, slopeLen + 0.6f); ramp.AddComponent<UnityEngine.BoxCollider>();
            break;
        }
    }
}

// ---------- giants ----------
var giants = new UnityEngine.GameObject("Giants");
var heroes = new UnityEngine.GameObject("Heroes"); heroes.transform.SetParent(giants.transform, false);
// crown: horizontal radius cr, vertical half ch, centre offset co (the Hollow Giant's crown leans away from the Camp 2 view)
UnityEngine.GameObject Giant(string name, UnityEngine.Transform parent, UnityEngine.Vector2 p, float trunkD, float tall, bool crown, bool meshCollider, float cr = 9f, float ch = 7f, UnityEngine.Vector2 co = default)
{
    float gy = H(p.x, p.y);
    var g = Group(name, parent, V(p.x, gy, p.y), 0f).transform;
    var trunk = Prim(Cyl, "Trunk", g, V(0f, tall * 0.5f - 1f, 0f), V(trunkD, tall * 0.5f + 1f, trunkD));   // sunk 1 m into the ground
    if (meshCollider) { UnityEngine.Object.DestroyImmediate(trunk.GetComponent<UnityEngine.Collider>()); trunk.AddComponent<UnityEngine.MeshCollider>(); }
    if (crown) { var c = Prim(Sph, "Crown", g, V(co.x, tall - ch, co.y), V(cr * 2f, ch * 2f, cr * 2f)); UnityEngine.Object.DestroyImmediate(c.GetComponent<UnityEngine.Collider>()); }
    return g.gameObject;
}
// Hollow Giant: crown radius 5.5 m, half-height 5 m, centred 2.5 m south, so it clears the Camp 2 view by 3 m (8.9a)
Giant("Hollow_Giant", heroes.transform, P(202, 140), 9f, 50f - H(202, 140), true, true, 5.5f, 5f, P(0f, -2.5f));
Giant("Gate_Tree", heroes.transform, P(290, 176), 8f, 15f, false, true);
// Snag 6 m east of the map point (90, 146), still on the east rim (r 18), so the tower cab shows past it from the
// Camp 3 floor and the log steps trail keeps clear of its trunk (8.9a, build notes)
Giant("Snag", heroes.transform, P(96, 146.5f), 6f, 54f - H(96, 146.5f), false, true);

// giant field: at least 30 m apart, never on a grid, tops at most 50 absolute (46 in the cliff-edge band x < 32),
// 40 to 50 m tall, none on the spur or wherever that rule leaves no room, none in clearings, trails, the lake, the
// ravine, the hollow, the old burn, the front zone, or within 3 m (plus crown) of the five tower cones on the map.
var cones = new[] {
    new[] { P(164,166), P(226.7f,43.1f), P(253.3f,60.9f) }, new[] { P(164,166), P(297.6f,212.4f), P(266.4f,263.6f) },
    new[] { P(164,166), P(283.8f,89.8f), P(300.2f,126.2f) }, new[] { P(164,166), P(88.8f,150.6f), P(91.2f,141.4f) },
    new[] { P(164,166), P(382.8f,208f), P(382.8f,148f) } };
var burn = new[] { P(185,181), P(340,213), P(340,143), P(185,151) };
var ravine = new[] { P(20,20), P(80,25.2f), P(94.8f,50), P(74,60), P(70,62), P(30,55.2f) };
bool Inside(UnityEngine.Vector2 p, UnityEngine.Vector2[] poly)
{
    bool c = false;
    for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
        if (((poly[i].y > p.y) != (poly[j].y > p.y)) && (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) c = !c;
    return c;
}
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); return UnityEngine.Vector2.Distance(p, a + ab * t); }
float PolyDist(UnityEngine.Vector2 p, UnityEngine.Vector2[] poly) { if (Inside(p, poly)) return 0f; float d = float.MaxValue; for (int i = 0; i < poly.Length; i++) d = UnityEngine.Mathf.Min(d, SegD(p, poly[i], poly[(i + 1) % poly.Length])); return d; }
var clearings = new (UnityEngine.Vector2 c, float r)[] { (P(170,160), 18f), (P(282,238), 30f), (P(292,108), 20f), (P(78,146), 22f), (P(32,258), 12.5f), (P(104,206), 5f), (P(128,70), 5f), (P(262,172), 4f), (P(240,52), 8f), (P(52,34), 12f) };
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (var b in built) for (int i = 0; i < b.path.Count; i += 4) trailPts.Add(b.path[i]);
var placed = new System.Collections.Generic.List<UnityEngine.Vector2> { P(202, 140), P(290, 176), P(96, 146.5f) };
// junction sight lines kept clear of giants (next destination visible, 8.9a): pump to the Snag, Camp 2 to the boathouse, Camp 3 and the other junctions to the cab
var sights = new[] { (P(190, 97), P(96, 146.5f)), (P(286.5f, 102.2f), P(240, 52.4f)), (P(78, 146), P(164, 166)),
    (P(104, 206), P(164, 166)), (P(262, 172), P(164, 166)), (P(128, 70), P(164, 166)), (P(340, 170), P(164, 166)), (P(190, 97), P(164, 166)),
    (P(276, 232), P(164, 166)), (P(286, 99), P(164, 166)), (P(128, 70), P(96, 146.5f)) };   // every junction to the tower cab (4.1); W1 to the Snag (8.9b)
var rng = new System.Random(8003); int made = 0;
for (int attempt = 0; attempt < 20000; attempt++)
{
    var p = P(12f + (float)rng.NextDouble() * 323f, 6f + (float)rng.NextDouble() * 288f);
    float trunkD = 6f + (float)rng.NextDouble() * 4f, crownR = 9f, clear = trunkD * 0.5f + 3f;
    bool ok = true;
    foreach (var q in placed) if (UnityEngine.Vector2.Distance(p, q) < 30f) { ok = false; break; }
    if (!ok) continue;
    float g = H(p.x, p.y); float cap = p.x < 32f ? 46f : 50f; float tall = UnityEngine.Mathf.Min(40f + (float)rng.NextDouble() * 10f, cap - g);
    // on the knoll (ground up to 15, rev 13) giants are at most 35 m tall, still capped at 50 absolute
    bool onKnoll = UnityEngine.Vector2.Distance(p, P(170, 160)) < 63f && g > 8f;
    if (onKnoll) tall = UnityEngine.Mathf.Min(tall, 35f);
    if (tall < (onKnoll ? 30f : 40f)) continue;
    foreach (var c in clearings) if (UnityEngine.Vector2.Distance(p, c.c) < c.r + crownR) { ok = false; break; }
    if (!ok) continue;
    foreach (var cone in cones) if (PolyDist(p, cone) < crownR + 3f) { ok = false; break; }
    foreach (var s in sights) if (SegD(p, s.Item1, s.Item2) < crownR + 3f) { ok = false; break; }
    if (!ok || PolyDist(p, burn) < crownR || PolyDist(p, ravine) < clear || LakeRe(p) < 1.35f) continue;
    foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < clear + 1.5f) { ok = false; break; }
    if (!ok) continue;
    foreach (var q in poiPlaced) if (UnityEngine.Vector2.Distance(p, q.obj) < clear + 4f) { ok = false; break; }
    if (!ok) continue;
    placed.Add(p); made++;
    Giant("Giant_" + made.ToString("00"), giants.transform, p, trunkD, tall, true, false);
}


// ---------- thicket (8.9a): the ground keeps walkers on the trails (DailyLoop.md 1.2, Main3.md 2.10) ----------
// The walkable area is the union of trail corridors (2.2 m each side), clearings, the lake landings, the front zone's
// surfaces and buildings, the Ward ledge and the cave mouth approach (positions from Main3.md and the later recipes).
// Its outline is traced (marching squares on a 0.5 m grid) and simplified. Off-trail blocking in the blockout is invisible walls
// too high to jump with low gray markers (8.9b); real vegetation comes in Milestone 11. Meshes per 50 m tile under
// Assets/Terrain/Main3/Thicket: Wall_* (collider only, Ignore Raycast layer) and Marker_* (visible, no collider).
const float cellT = 0.5f; int GW = 800, GH = 600;
var fld = new float[GW + 1, GH + 1];
for (int i = 0; i <= GW; i++) for (int j = 0; j <= GH; j++) fld[i, j] = -10f;
void Stamp(float x0, float x1, float z0, float z1, System.Func<UnityEngine.Vector2, float> val)
{
    int i0 = UnityEngine.Mathf.Max(1, (int)(x0 / cellT) - 1), i1 = UnityEngine.Mathf.Min(GW - 1, (int)(x1 / cellT) + 1);
    int j0 = UnityEngine.Mathf.Max(1, (int)(z0 / cellT) - 1), j1 = UnityEngine.Mathf.Min(GH - 1, (int)(z1 / cellT) + 1);
    for (int i = i0; i <= i1; i++) for (int j = j0; j <= j1; j++) { float v = val(P(i * cellT, j * cellT)); if (v > fld[i, j]) fld[i, j] = v; }
}
void Circle(UnityEngine.Vector2 c, float r) => Stamp(c.x - r - 1f, c.x + r + 1f, c.y - r - 1f, c.y + r + 1f, q => r - UnityEngine.Vector2.Distance(q, c));
void Rect(float x0, float x1, float z0, float z1) => Stamp(x0 - 1f, x1 + 1f, z0 - 1f, z1 + 1f, q => UnityEngine.Mathf.Min(UnityEngine.Mathf.Min(q.x - x0, x1 - q.x), UnityEngine.Mathf.Min(q.y - z0, z1 - q.y)));
void Seg(UnityEngine.Vector2 a, UnityEngine.Vector2 b, float hw) => Stamp(UnityEngine.Mathf.Min(a.x, b.x) - hw - 1f, UnityEngine.Mathf.Max(a.x, b.x) + hw + 1f, UnityEngine.Mathf.Min(a.y, b.y) - hw - 1f, UnityEngine.Mathf.Max(a.y, b.y) + hw + 1f, q => hw - SegD(q, a, b));
void Ring(UnityEngine.Vector2 c, float r0, float r1) => Stamp(c.x - r1 - 1f, c.x + r1 + 1f, c.y - r1 - 1f, c.y + r1 + 1f, q => { float d = UnityEngine.Vector2.Distance(q, c); return UnityEngine.Mathf.Min(d - r0, r1 - d); });
foreach (var b in built) for (int i = 0; i < b.path.Count - 1; i++) Seg(b.path[i], b.path[i + 1], 2.2f);
Circle(P(170, 160), 18f); Circle(P(282, 238), 30f); Circle(P(292, 108), 20f); Circle(P(78, 146), 9f);
Circle(P(104, 206), 5f); Circle(P(128, 70), 5f); Circle(P(262, 172), 4f); Circle(P(340, 170), 4f);
Rect(13.6f, 30f, 245f, 271f);                                                      // Ward ledge (rev 13): up to the lip line (x 13.6), the low crest and the stones
Rect(49.5f, 54.5f, 30f, 41f);                                                      // in front of the cave mouth and into the passage (8.9b: no wall across it)
Rect(187.5f, 192.5f, 86f, 98f); Rect(236.3f, 248.3f, 49.1f, 55.7f);                // dock notch; boathouse and gangway
Rect(342f, 374f, 149f, 191f); Rect(373f, 395.6f, 166.5f, 173.5f); Circle(P(384, 160), 9f);   // lot, drive, turning circle
Rect(342f, 372f, 189f, 206f); Rect(388.5f, 395.6f, 172f, 179f); Rect(385f, 395.6f, 234f, 242f); // office and store, booth, chain
var spurW = new[] { P(385, 172.5f), P(385, 186), P(390, 196), P(390, 244) };
for (int i = 0; i < spurW.Length - 1; i++) Seg(spurW[i], spurW[i + 1], 3f);
Ring(P(372, 262), 9.5f, 21f);                                                      // loop road and pitches
Seg(P(390, 243), P(387, 255), 3f);                                                 // spur on into the loop (8.9b)
foreach (var q in poiPlaced) if (q.kind == "side") Circle(q.obj, poiRadius[q.n] + 1.5f);
// marching squares: segments between edge crossings, keyed by edge so neighbouring cells join
var ptOf = new System.Collections.Generic.Dictionary<long, UnityEngine.Vector2>();
var adj = new System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>>();
long EKey(int i, int j, int dir) => ((long)j * (GW + 1) + i) * 2 + dir;
UnityEngine.Vector2 EPoint(int i, int j, int dir)
{
    float va = fld[i, j], vb = dir == 0 ? fld[i + 1, j] : fld[i, j + 1]; float t = va / (va - vb);
    return dir == 0 ? P((i + t) * cellT, j * cellT) : P(i * cellT, (j + t) * cellT);
}
void Link(long a, long b, UnityEngine.Vector2 pa, UnityEngine.Vector2 pb)
{
    ptOf[a] = pa; ptOf[b] = pb;
    if (!adj.TryGetValue(a, out var la)) adj[a] = la = new System.Collections.Generic.List<long>(); la.Add(b);
    if (!adj.TryGetValue(b, out var lb)) adj[b] = lb = new System.Collections.Generic.List<long>(); lb.Add(a);
}
for (int i = 0; i < GW; i++) for (int j = 0; j < GH; j++)
{
    int cs = (fld[i, j] > 0 ? 1 : 0) | (fld[i + 1, j] > 0 ? 2 : 0) | (fld[i + 1, j + 1] > 0 ? 4 : 0) | (fld[i, j + 1] > 0 ? 8 : 0);
    if (cs == 0 || cs == 15) continue;
    long B = EKey(i, j, 0), R = EKey(i + 1, j, 1), T = EKey(i, j + 1, 0), Lf = EKey(i, j, 1);
    UnityEngine.Vector2 pB() => EPoint(i, j, 0); UnityEngine.Vector2 pR() => EPoint(i + 1, j, 1); UnityEngine.Vector2 pT() => EPoint(i, j + 1, 0); UnityEngine.Vector2 pL() => EPoint(i, j, 1);
    bool centre = (fld[i, j] + fld[i + 1, j] + fld[i + 1, j + 1] + fld[i, j + 1]) > 0f;
    switch (cs)
    {
        case 1: case 14: Link(Lf, B, pL(), pB()); break;
        case 2: case 13: Link(B, R, pB(), pR()); break;
        case 3: case 12: Link(Lf, R, pL(), pR()); break;
        case 4: case 11: Link(R, T, pR(), pT()); break;
        case 6: case 9: Link(B, T, pB(), pT()); break;
        case 7: case 8: Link(Lf, T, pL(), pT()); break;
        case 5: if (centre) { Link(B, R, pB(), pR()); Link(T, Lf, pT(), pL()); } else { Link(Lf, B, pL(), pB()); Link(R, T, pR(), pT()); } break;
        case 10: if (centre) { Link(Lf, B, pL(), pB()); Link(R, T, pR(), pT()); } else { Link(B, R, pB(), pR()); Link(T, Lf, pT(), pL()); } break;
    }
}
// chain the crossings into closed loops and simplify them (Douglas-Peucker, 0.3 m)
var loops = new System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>();
var seen = new System.Collections.Generic.HashSet<long>();
foreach (var k0 in adj.Keys)
{
    if (seen.Contains(k0)) continue;
    var loop = new System.Collections.Generic.List<UnityEngine.Vector2>(); long prev = -1, cur = k0;
    while (true)
    {
        seen.Add(cur); loop.Add(ptOf[cur]); long next = -1;
        foreach (var n in adj[cur]) if (n != prev && !seen.Contains(n)) { next = n; break; }
        if (next < 0) break; prev = cur; cur = next;
    }
    if (loop.Count > 2) { loop.Add(loop[0]); loops.Add(loop); }
}
System.Collections.Generic.List<UnityEngine.Vector2> Simplify(System.Collections.Generic.List<UnityEngine.Vector2> pts, float tol)
{
    var keep = new bool[pts.Count]; keep[0] = keep[pts.Count - 1] = true;
    var stack = new System.Collections.Generic.Stack<(int, int)>(); stack.Push((0, pts.Count - 1));
    while (stack.Count > 0)
    {
        var (a, b) = stack.Pop(); float best = 0f; int bi = -1;
        for (int i = a + 1; i < b; i++) { float d = SegD(pts[i], pts[a], pts[b]); if (d > best) { best = d; bi = i; } }
        if (bi >= 0 && best > tol) { keep[bi] = true; stack.Push((a, bi)); stack.Push((bi, b)); }
    }
    var outp = new System.Collections.Generic.List<UnityEngine.Vector2>(); for (int i = 0; i < pts.Count; i++) if (keep[i]) outp.Add(pts[i]); return outp;
}
// Each outline segment gets two boxes (8.9b): an invisible wall 4 m over the highest ground under it (the player jumps 0.6 m,
// so it cannot be climbed from a slope or a boulder), on the Ignore Raycast layer so no sight line or landmark check sees it;
// and a low visible marker 0.3 m over the lowest ground, no collider, so the edge reads in gray without hiding anything.
var tileWalls = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<UnityEngine.Matrix4x4>>();
var tileMarks = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<UnityEngine.Matrix4x4>>();
int boxes = 0;
foreach (var raw0 in loops)
{
    // a closed loop starts and ends on the same point, so split it at the point farthest from its start before simplifying
    int far = 0; for (int i = 1; i < raw0.Count; i++) if ((raw0[i] - raw0[0]).sqrMagnitude > (raw0[far] - raw0[0]).sqrMagnitude) far = i;
    var pl = Simplify(raw0.GetRange(0, far + 1), 0.3f); var back2 = Simplify(raw0.GetRange(far, raw0.Count - far), 0.3f); back2.RemoveAt(0); pl.AddRange(back2);
    for (int i = 0; i < pl.Count - 1; i++)
    {
        var a = pl[i]; var b = pl[i + 1]; float len = UnityEngine.Vector2.Distance(a, b); if (len < 0.05f) continue;
        int parts = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(len / 4f));   // at most 4 m per box so heights follow the ground
        for (int k = 0; k < parts; k++)
        {
            var sa = UnityEngine.Vector2.Lerp(a, b, k / (float)parts); var sb2 = UnityEngine.Vector2.Lerp(a, b, (k + 1) / (float)parts); var mid = (sa + sb2) * 0.5f;
            float g0 = H(sa.x, sa.y), g1 = H(sb2.x, sb2.y), gm = H(mid.x, mid.y);
            float gLow = UnityEngine.Mathf.Min(gm, UnityEngine.Mathf.Min(g0, g1)), gTop = UnityEngine.Mathf.Max(gm, UnityEngine.Mathf.Max(g0, g1));
            float lo = gLow - 0.4f, wallTop = gTop + 4f, markTop = gLow + 0.3f;
            var dir = sb2 - sa; float l = dir.magnitude + 0.5f; var rot = UnityEngine.Quaternion.LookRotation(V(dir.x, 0f, dir.y).normalized, UnityEngine.Vector3.up);
            int key = ((int)(mid.x / 50f)) * 100 + (int)(mid.y / 50f);
            if (!tileWalls.TryGetValue(key, out var lw)) { tileWalls[key] = lw = new System.Collections.Generic.List<UnityEngine.Matrix4x4>(); tileMarks[key] = new System.Collections.Generic.List<UnityEngine.Matrix4x4>(); }
            lw.Add(UnityEngine.Matrix4x4.TRS(V(mid.x, (lo + wallTop) * 0.5f, mid.y), rot, V(0.6f, wallTop - lo, l)));
            tileMarks[key].Add(UnityEngine.Matrix4x4.TRS(V(mid.x, (lo + markTop) * 0.5f, mid.y), rot, V(0.5f, markTop - lo, l)));
            boxes++;
        }
    }
}
const string thDir = "Assets/Terrain/Main3/Thicket";
if (!UnityEditor.AssetDatabase.IsValidFolder(thDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Terrain/Main3", "Thicket");
var cubeTmp = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
var cubeMesh = cubeTmp.GetComponent<UnityEngine.MeshFilter>().sharedMesh; var grayMat = cubeTmp.GetComponent<UnityEngine.MeshRenderer>().sharedMaterial;
var thicket = new UnityEngine.GameObject("Thicket"); long verts = 0;
UnityEngine.Mesh Combine(string name, System.Collections.Generic.List<UnityEngine.Matrix4x4> ms, bool keepNormals)
{
    var ci = new UnityEngine.CombineInstance[ms.Count];
    for (int i = 0; i < ci.Length; i++) ci[i] = new UnityEngine.CombineInstance { mesh = cubeMesh, transform = ms[i] };
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.CombineMeshes(ci, true, true); mesh.uv = null; mesh.tangents = null; if (!keepNormals) mesh.normals = null; mesh.RecalculateBounds(); verts += mesh.vertexCount;
    UnityEditor.AssetDatabase.CreateAsset(mesh, thDir + "/" + name + ".asset"); return mesh;
}
foreach (var kv in tileWalls)
{
    var wallMesh = Combine("Wall_" + kv.Key, kv.Value, false); var markMesh = Combine("Marker_" + kv.Key, tileMarks[kv.Key], true);
    var wgo = new UnityEngine.GameObject("Wall_" + kv.Key); wgo.transform.SetParent(thicket.transform, false); wgo.layer = 2;   // Ignore Raycast
    wgo.AddComponent<UnityEngine.MeshCollider>().sharedMesh = wallMesh;
    var mgo = new UnityEngine.GameObject("Marker_" + kv.Key); mgo.transform.SetParent(thicket.transform, false);
    mgo.AddComponent<UnityEngine.MeshFilter>().sharedMesh = markMesh; mgo.AddComponent<UnityEngine.MeshRenderer>().sharedMaterial = grayMat;
}
UnityEngine.Object.DestroyImmediate(cubeTmp);
report.Append("thicket: " + loops.Count + " outlines, " + boxes + " wall and marker pairs, " + tileWalls.Count + " tiles, " + verts + " vertices\n");
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " giants=" + made + " + 3 heroes, POIs=" + poiRoot.transform.childCount + "\n" + report;
