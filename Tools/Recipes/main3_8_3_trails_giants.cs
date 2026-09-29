// Main3 task 8.3: trails flattened and painted along every route in Main3.md 4, points of interest as gray stand-ins,
// gray giant trees (hero trunks with mesh colliders). Run after 8.2 in Main3, edit mode.
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
    ("Camp to pump", new[] { P(170,160), P(174,128), P(190,96) }, false, 80f, false, 0.3f, new[] { ("Water tank", P(182,128), "side") }),
    ("Pump to boathouse", new[] { P(190,96), P(232,92), P(246,57) }, false, 84f, true, 0.3f, new[] { ("Overturned rowboat", P(223.6f,83.2f), "side") }),
    ("Boathouse to Camp 2", new[] { P(246,57), P(280,64), P(292,108) }, false, 95f, false, 0.3f, new[] { ("Phone pole", P(272.8f,72f), "side") }),
    ("Camp 2 to T", new[] { P(292,108), P(324,128), P(340,170) }, false, 94f, false, 0.3f, new[] { ("Food lockers", P(320,133.6f), "side") }),
    ("Camp to Jg", new[] { P(170,160), P(200,136), P(232,152), P(262,172) }, false, 125f, false, 0.3f, new[] { ("Hollow Giant", P(202,140), "tree"), ("Forage patch A", P(240,162.8f), "side") }),
    ("Jg to T", new[] { P(262,172), P(288,196), P(316,148), P(340,170) }, false, 105f, false, 0.3f, new[] { ("Gate Tree", P(290,176), "tree"), ("First sight of the lot", P(328,164), "side") }),
    ("Jg to Camp 1", new[] { P(262,172), P(264,208), P(282,238) }, false, 83f, false, 0.3f, new[] { ("Latrine shed", P(268,206.4f), "side") }),
    ("Camp to Camp 3", new[] { P(170,160), P(144,184), P(108,128), P(78,146) }, false, 126f, false, 0.3f, new[] { ("Forage patch B", P(142.4f,163.6f), "side"), ("Log steps", P(96,144.8f), "steps") }),
    ("Pump to W1", new[] { P(190,96), P(156,92), P(128,70) }, false, 80f, true, 0.3f, new[] { ("Washed-out truck", P(157.6f,87.6f), "side"), ("Stepping stones", P(131.8f,72.7f), "on") }),
    ("W1 to Camp 3", new[] { P(128,70), P(96,104), P(78,146) }, false, 114f, false, 0.3f, new[] { ("Footbridge", P(114.8f,85.2f), "on"), ("Camper trailer", P(94,114.4f), "side") }),
    ("W1 to cave", new[] { P(128,70), P(108,48), P(84,64), P(52,37.5f) }, false, 109f, false, 0.3f, new[] { ("Rope handrail", P(106.8f,57.6f), "on"), ("Coloured bulbs", P(82,52), "side") }),
    ("Camp to J", new[] { P(170,160), P(136,196), P(104,206) }, false, 96f, false, 0.3f, new[] { ("Burn-map board", P(136.4f,189.6f), "side"), ("Plank bridge", P(104.8f,203.2f), "on") }),
    ("J to Ward", new[] { P(104,206), P(110,228), P(98,242), P(76,248), P(62,260), P(46,254), P(32,258) }, true, 130f, false, 0.3f, new[] { ("Rune post", P(92,243.6f), "side") }),
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

// table 2.1 heights at the named trail ends
var namedEnds = new (UnityEngine.Vector2 p, float h)[] { (P(170,160), 8f), (P(190,96), -4.5f), (P(128,70), -4.5f), (P(104,206), 10f), (P(262,172), 5f), (P(340,170), 3f), (P(282,238), 5f), (P(292,108), 4f), (P(78,146), -4f), (P(52,37.5f), -6f), (P(32,258), 36f) };
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
            int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(l / 18f)); float amp = seg == stepsSeg ? 0f : k * l / n;
            // times sin(pi u): zero offset and zero slope at every anchor, so the trail runs straight through anchored pieces
            float f = UnityEngine.Mathf.Sin(UnityEngine.Mathf.PI * n * u) * UnityEngine.Mathf.Sin(UnityEngine.Mathf.PI * u); if (leg.shore) f = UnityEngine.Mathf.Abs(f) * side;
            float off = amp * f;
            foreach (var dt in detours) { float x = (d - dt.s) / 15f; if (UnityEngine.Mathf.Abs(x) < 1f) { float w = UnityEngine.Mathf.Cos(x * UnityEngine.Mathf.PI * 0.5f); off += dt.off * w * w; } }
            var c = At(C, S, UnityEngine.Mathf.Min(d, Lc), out var tan); outp.Add(c + P(-tan.y, tan.x) * off);
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
    for (int pass = 0; pass < 6; pass++)   // ends stay pinned; interior points clamp toward them
    {
        for (int i = 1; i < N - 1; i++) { float g = (i * 0.5f >= stepsFrom ? 0.85f : leg.maxGrade) * 0.5f; prof[i] = UnityEngine.Mathf.Clamp(prof[i], prof[i - 1] - g, prof[i - 1] + g); }
        for (int i = N - 2; i >= 1; i--) { float g = (i * 0.5f >= stepsFrom ? 0.85f : leg.maxGrade) * 0.5f; prof[i] = UnityEngine.Mathf.Clamp(prof[i], prof[i + 1] - g, prof[i + 1] + g); }
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
            foreach (var sx in new[] { -1.3f, 1.3f }) Prim(Cube, "Rail", g, V(sx, 0.5f, 0f), V(0.08f, 1.0f, 4.5f)); break;
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
UnityEngine.GameObject Giant(string name, UnityEngine.Transform parent, UnityEngine.Vector2 p, float trunkD, float tall, bool crown, bool meshCollider)
{
    float gy = H(p.x, p.y);
    var g = Group(name, parent, V(p.x, gy, p.y), 0f).transform;
    var trunk = Prim(Cyl, "Trunk", g, V(0f, tall * 0.5f - 1f, 0f), V(trunkD, tall * 0.5f + 1f, trunkD));   // sunk 1 m into the ground
    if (meshCollider) { UnityEngine.Object.DestroyImmediate(trunk.GetComponent<UnityEngine.Collider>()); trunk.AddComponent<UnityEngine.MeshCollider>(); }
    if (crown) { var c = Prim(Sph, "Crown", g, V(0f, tall - 7f, 0f), V(18f, 14f, 18f)); UnityEngine.Object.DestroyImmediate(c.GetComponent<UnityEngine.Collider>()); }
    return g.gameObject;
}
Giant("Hollow_Giant", heroes.transform, P(202, 140), 9f, 50f - H(202, 140), true, true);
Giant("Gate_Tree", heroes.transform, P(290, 176), 8f, 15f, false, true);
Giant("Snag", heroes.transform, P(90, 146), 6f, 54f - H(90, 146), false, true);

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
var placed = new System.Collections.Generic.List<UnityEngine.Vector2> { P(202, 140), P(290, 176), P(90, 146) };
var rng = new System.Random(8003); int made = 0;
for (int attempt = 0; attempt < 20000; attempt++)
{
    var p = P(12f + (float)rng.NextDouble() * 323f, 6f + (float)rng.NextDouble() * 288f);
    float trunkD = 6f + (float)rng.NextDouble() * 4f, crownR = 9f, clear = trunkD * 0.5f + 3f;
    bool ok = true;
    foreach (var q in placed) if (UnityEngine.Vector2.Distance(p, q) < 30f) { ok = false; break; }
    if (!ok) continue;
    float g = H(p.x, p.y); float cap = p.x < 32f ? 46f : 50f; float tall = UnityEngine.Mathf.Min(40f + (float)rng.NextDouble() * 10f, cap - g);
    if (tall < 40f) continue;
    if (UnityEngine.Vector2.Distance(p, P(170, 160)) < 45f && tall > 42f) tall = 42f;
    foreach (var c in clearings) if (UnityEngine.Vector2.Distance(p, c.c) < c.r + crownR) { ok = false; break; }
    if (!ok) continue;
    foreach (var cone in cones) if (PolyDist(p, cone) < crownR + 3f) { ok = false; break; }
    if (!ok || PolyDist(p, burn) < crownR || PolyDist(p, ravine) < clear || LakeRe(p) < 1.35f) continue;
    foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < clear + 1.5f) { ok = false; break; }
    if (!ok) continue;
    foreach (var q in poiPlaced) if (UnityEngine.Vector2.Distance(p, q.obj) < clear + 4f) { ok = false; break; }
    if (!ok) continue;
    placed.Add(p); made++;
    Giant("Giant_" + made.ToString("00"), giants.transform, p, trunkD, tall, true, false);
}

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " giants=" + made + " + 3 heroes, POIs=" + poiRoot.transform.childCount + "\n" + report;
