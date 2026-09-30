// Main3 task 8.1: scene, terrain, rock bands, fence, spawn, dev warps.
// Source: Docs/Design/Main3.md rev 16 table 2.1 for the valley floor; Docs/Design/Valley.md revision 10 (8.14) for the horseshoe:
// the W ridge (crest 80, knob 84, shoulder), the N and S arms, the open east with the highway, the Ward climb (four legs, the
// cleft with its dogleg and fin) and the ledge; resolutions in Docs/Design/Main3_BuildNotes.md. Wren 2026-09-30: the N arm holds 50
// at x 170. The rev 7 ring of ridges, the E ridge, the road cut, the benched climb and the Bounds walls are in git history
// (tag main3-rev7).
// Run from Graybox (or any saved scene) in edit mode. Refuses if Main3.unity exists: delete it and its terrain folder to rebuild,
// then rerun every later Main3 recipe in task order (Tools/Recipes/main3_rebuild.sh).
// The terrain is placed at (-40, -200) and runs to (595, 500); later recipes that index the heightmap, alphamap or holes add the
// terrain origin. Boundaries come from the land (Valley.md 8): a rock band with a collider along every ridge foot, the fence,
// and rock walls on the climb and the ledge ("Rock" root, visible, MeshColliders). No invisible wall is built here.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) return "active scene dirty, save first";
const string scenePath = "Assets/Scenes/Main3.unity";
const string dir = "Assets/Terrain/Main3";
if (System.IO.File.Exists(scenePath)) return "Main3.unity already exists";
if (!NewSceneMenu.CreateScene(scenePath)) return "recipe failed";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != scenePath) return "wrong scene after recipe: " + scene.path;
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
foreach (var r in scene.GetRootGameObjects()) if (r.name == "Ground") UnityEngine.Object.DestroyImmediate(r);
// A rebuild makes a new scene GUID; CreateScene skips a path already in the build list, so refresh that entry's GUID.
// Replacing the entry in place keeps the old GUID; it must be removed, saved, then added again.
{
    var list = new System.Collections.Generic.List<UnityEditor.EditorBuildSettingsScene>();
    foreach (var s in UnityEditor.EditorBuildSettings.scenes) if (s.path != scenePath) list.Add(s);
    UnityEditor.EditorBuildSettings.scenes = list.ToArray(); UnityEditor.AssetDatabase.SaveAssets();
    list.Add(new UnityEditor.EditorBuildSettingsScene(scenePath, true));
    UnityEditor.EditorBuildSettings.scenes = list.ToArray(); UnityEditor.AssetDatabase.SaveAssets();
}

// ---------- terrain extent (8.9j) ----------
const float originX = -40f, originZ = -200f, sizeX = 635f, sizeZ = 700f, baseY = -45f, sizeY = 165f;   // to x 595, z 500, heights -45 to 120
const int res = 1025, ares = 1024;   // 0.62 x 0.68 m cells, finer than rev 16's 0.78 m
// ---------- valley floor data (rev 16) ----------
var ravPoly = new[] { P(20,20), P(80,25.2f), P(94.8f,50), P(74,60), P(70,62), P(30,55.2f) };
var ravEdges = new[] { P(30,55.2f), P(20,20), P(80,25.2f), P(94.8f,50) };                           // west, south and east sides
var rim = new[] { P(30,55.2f), P(70,62), P(74,60), P(94.8f,50) };
var burnPoly = new[] { P(185,181), P(340,213), P(340,143), P(185,151) };
var creek = new[] { P(104,215.2f), P(104.8f,203.2f), P(100,175.2f), P(84,150), P(78,146), P(84,128), P(100,110), P(128,78), P(136.5f,66) };   // through the hollow centre, 11 m clear of the Snag
var creekBed = new[] { 9.5f, 8f, 3.5f, -4.1f, -4.1f, -4.3f, -4.8f, -5.3f, -5.8f };
var lakeC = P(190, 60); const float lakeA = 54.8f, lakeB = 27.6f;
const float lakeShore = 30f, notchSide = 0.7f, notchRise = 0.5f;   // the bank reaches the ground 30 m out (8.14a); the dock root notch
// named ground points: centre, flat radius, blend width, height (table 2.1)
var named = new (UnityEngine.Vector2 c, float r, float blend, float h)[] {
    (P(170,160), 18f, 45f, 15f),     // keeper's camp knoll top at 15 (rev 13), flanks to the ground by 63 m (clear of the pump notch)
    (P(282,238), 30f, 15f, 5f),      // Camp 1
    (P(292,108), 20f, 12f, 4f),      // Camp 2 boulder field
    (P(262,172), 4f, 12f, 5f),       // Jg
    (P(290,176), 3f, 10f, 4f),       // Gate Tree
    (P(202,140), 5f, 10f, 6f),       // Hollow Giant
    (P(240,162.8f), 4f, 10f, 5f),    // forage A
    (P(142.4f,163.6f), 4f, 10f, 6f), // forage B
    (P(104,206), 5f, 10f, 10f),      // J
    (P(128,70), 4f, 8f, -4.5f),      // W1
};
float BurnH(float x) => 12f - 9f * UnityEngine.Mathf.Clamp01((x - 185f) / 155f);   // about 12 on the knoll flank (rev 13)

float SegDist(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b, out float t)
{
    var ab = b - a; t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-4f));
    return UnityEngine.Vector2.Distance(p, a + ab * t);
}
float LineDist(UnityEngine.Vector2 p, UnityEngine.Vector2[] pts, out float s)
{
    float best = float.MaxValue, acc = 0f; s = 0f;
    for (int i = 0; i < pts.Length - 1; i++)
    {
        float d = SegDist(p, pts[i], pts[i + 1], out float t); float Lg = UnityEngine.Vector2.Distance(pts[i], pts[i + 1]);
        if (d < best) { best = d; s = acc + t * Lg; }
        acc += Lg;
    }
    return best;
}
float LineLen(UnityEngine.Vector2[] pts) { float Lg = 0; for (int i = 0; i < pts.Length - 1; i++) Lg += UnityEngine.Vector2.Distance(pts[i], pts[i + 1]); return Lg; }
bool Inside(UnityEngine.Vector2 p, UnityEngine.Vector2[] poly)
{
    bool c = false;
    for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
        if (((poly[i].y > p.y) != (poly[j].y > p.y)) && (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) c = !c;
    return c;
}
float SS(float a) => UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01(a));
float L(float a, float b, float t) => UnityEngine.Mathf.Lerp(a, b, t);
float Clamp01(float a) => UnityEngine.Mathf.Clamp01(a);
// piecewise linear through (ts[i], vs[i]), held flat past both ends
float PL(float t, float[] ts, float[] vs)
{
    if (t <= ts[0]) return vs[0];
    for (int i = 1; i < ts.Length; i++) if (t <= ts[i]) return L(vs[i - 1], vs[i], (t - ts[i - 1]) / (ts[i] - ts[i - 1]));
    return vs[vs.Length - 1];
}

// base: 0 in the south-west rolling up to 5 in the north-east
float Base(float x, float z) => 2.5f * (x / 400f + z / 300f);
var ctrl = new System.Collections.Generic.List<(UnityEngine.Vector2 c, float h)>();
foreach (var n in named) if (n.c != P(170, 160)) ctrl.Add((n.c, n.h));   // the knoll shapes itself (linear flank), so it does not lift the ground round the lake
for (float bx = 200f; bx <= 330f; bx += 32.5f) ctrl.Add((P(bx, 166f + (bx - 185f) * 0.08f), BurnH(bx)));
float rimLen = LineLen(rim);
float rimS18 = 45.1f;   // arc length of the vertex (74, 60) along the rim line

// the valley floor inside the ring of ridges (rev 16 steps 1 to 10; the Wall, plateau, pass, lip and cliff at x 10 are gone)
float Valley(float x, float z)
{
    var p = P(x, z);
    // 1. base plus smooth offsets toward the named heights, small roll
    float b = Base(x, z), num = 0f, den = 0.15f;
    foreach (var c in ctrl) { float w = UnityEngine.Mathf.Exp(-(UnityEngine.Vector2.SqrMagnitude(p - c.c)) / (40f * 40f)); num += w * (c.h - Base(c.c.x, c.c.y)); den += w; }
    float h = b + num / den + (UnityEngine.Mathf.PerlinNoise(x / 45f + 3.1f, z / 45f + 7.7f) - 0.5f) * 1.6f;
    float field = h;
    // 3. named flats: the knoll falls linearly (under 20 percent) so trails leaving the camp stay walkable; the other flats blend smoothly
    foreach (var n in named) { float d = UnityEngine.Vector2.Distance(p, n.c); bool knoll = n.c == P(170, 160); if (d < n.r + n.blend) h = L(n.h, h, knoll ? (d - n.r) / n.blend : SS((d - n.r) / n.blend)); }
    // 4. Camp 3 hollow
    {
        float d = UnityEngine.Vector2.Distance(p, P(78, 146));
        if (d < 30f) h = d < 8f ? -4f : d < 12f ? L(-4f, 4f, SS((d - 8f) / 4f)) : d < 20f ? 4f : L(4f, h, SS((d - 20f) / 10f));
    }
    // 5. lake: bed -8 at the centre shelving to -5.7 at the water line, bank -4.5 at 1.5 m, back to the ground within 15 m
    {
        var q = p - lakeC; float re = UnityEngine.Mathf.Sqrt((q.x / lakeA) * (q.x / lakeA) + (q.y / lakeB) * (q.y / lakeB));
        float Lr = re > 1e-4f ? q.magnitude / re : lakeB; float dOut = (re - 1f) * Lr;
        if (dOut <= 0f) h = -5.7f - 2.3f * (1f - re * re);
        else if (dOut < 1.5f) h = L(-5.7f, -4.5f, dOut / 1.5f);
        else if (dOut < lakeShore) h = L(-4.5f, h, SS((dOut - 1.5f) / (lakeShore - 1.5f)));   // 8.14a: a long gentle shore, not a pit (was 15 m linear)
        if (dOut > 0f && z > 80f) h = UnityEngine.Mathf.Min(h, -4.5f + notchSide * UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Abs(x - 190f) - 2.5f) + notchRise * UnityEngine.Mathf.Max(0f, z - 96f));   // dock root notch at -4.5 to z 96; with the long shore (8.14a) it is shallow
    }
    // 6. sill between lake and ravine
    {
        float sx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(100f - x, x - 120f)), sz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(45f - z, z - 65f));
        h = UnityEngine.Mathf.Max(h, -3f - 0.5f * UnityEngine.Mathf.Sqrt(sx * sx + sz * sz));
    }
    // 7. ravine: rim ridge on the north side (14, 18 near (74, 60)), floor -6 north of the mouth face
    {
        float dr = LineDist(p, rim, out float s);
        float rimTop = (s >= rimS18 - 20f && s <= rimS18 + 15f) ? 18f : 14f;
        if (s < rimS18 - 20f) rimTop = L(14f, 18f, SS((s - (rimS18 - 25f)) / 5f));
        float taper = SS(UnityEngine.Mathf.Min(s, rimLen - s) / 8f);
        if (Inside(p, ravPoly))
        {
            float de = LineDist(p, ravEdges, out _);
            float floorW = SS((z - 33f) / 4f) * SS(de / 10f);
            float fl = L(h, -6f, floorW);
            float rimSide = L(fl, L(field, rimTop, taper), 1f - SS((dr - 1.5f) / 12f));
            h = UnityEngine.Mathf.Max(fl, rimSide);
        }
        else if (dr < 22f) h = UnityEngine.Mathf.Max(h, L(L(h, rimTop, taper), h, SS((dr - 1.5f) / 20f)));
    }
    // 8. creek: channel above the hollow, a low walkable valley from the hollow to the lake
    {
        float dcr = float.MaxValue, bed = 0f; int seg = 0;
        for (int i = 0; i < creek.Length - 1; i++) { float d = SegDist(p, creek[i], creek[i + 1], out float t); if (d < dcr) { dcr = d; bed = L(creekBed[i], creekBed[i + 1], t); seg = i; } }
        float hollowT = SS((UnityEngine.Vector2.Distance(p, P(78, 146)) - 18f) / 8f);
        float hw = L(2.5f, seg <= 2 ? 1.2f : 4f, hollowT), slope = L(2.0f, seg <= 2 ? 1.0f : 0.45f, hollowT);
        float floorH = seg >= 5 && dcr >= 1.2f ? bed + 0.3f : bed;
        float carve = floorH + UnityEngine.Mathf.Max(0f, dcr - hw) * slope;
        float reach = 1f - SS((dcr - hw - 8f) / 4f);
        h = UnityEngine.Mathf.Min(h, L(h, carve, reach));
        float dW = UnityEngine.Vector2.Distance(p, P(128, 70));
        if (dW < 6f) h = L(-4.5f, h, SS((dW - 3f) / 3f));
    }
    // 9. ground over the cave: 0 over the chamber and ramp (x 44 to 89, z 3 to 22) and over the entrance passage (x 50 to 54, z 22 to 34)
    {
        float dx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(42f - x, x - 91f)), dz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(1f - z, z - 24f));
        float dRect = UnityEngine.Mathf.Sqrt(dx * dx + dz * dz);
        if (dRect < 4f) h = L(0f, h, SS(dRect / 4f));
        if (x >= 48f && x <= 56f && z >= 22f && z <= 34f) h = 0f;
    }
    // 9b. Camp 2 view cut (8.9a)
    {
        var va = P(286.5f, 102.2f); var vb = P(240f, 52.4f); var ab = vb - va;
        float t = Clamp01(UnityEngine.Vector2.Dot(p - va, ab) / ab.sqrMagnitude);
        float d = UnityEngine.Vector2.Distance(p, va + ab * t);
        if (d < 7f) { float cap = L(5.6f, -1.0f, t) - 1.3f; float w = 1f - SS((d - 3f) / 4f); if (h > cap) h = L(h, cap, w); }
    }
    // 10. front zone flat at 3
    if (x >= 330f) h = L(h, 3f, SS((x - 330f) / 10f));
    return h;
}

// ---------- outer ground (Valley.md 2.6 and 3.5) ----------
// Gently rolling 0 to 10 outside the horseshoe; east of the highway the land rolls up to 20 to 35 m from about 100 m past the
// road ("rolling forest", 3.5). Floor(), EastHills() and Outer() are copied in main3_8_9e_layers.cs (the outer ground mesh
// beyond the terrain); change both together. West of x 0 the ground steps down at 45 degrees to the -40 floor at x -40.
float Floor(float x, float z) => UnityEngine.Mathf.Clamp(5f + 4.5f * (UnityEngine.Mathf.PerlinNoise(x / 280f + 11.3f, z / 280f + 4.7f) * 2f - 1f), 0f, 10f);
const float hillX0 = 520f, hillRamp = 100f, hillLow = 20f, hillSpan = 15f, hillScale = 160f;
float EastHills(float x, float z) => SS((x - hillX0) / hillRamp) * (hillLow + hillSpan * UnityEngine.Mathf.PerlinNoise(x / hillScale + 2.2f, z / hillScale + 6.1f));
float Outer(float x, float z) => x >= 0f ? Floor(x, z) + EastHills(x, z) : L(-40f, Floor(0f, z), (x + 40f) / 40f);
const float backLen = 150f, westFloor = -40f, westEdge = -40f, wFade = 100f;

// ---------- the horseshoe (Valley.md rev 10, sections 2, 3, 4 and 8) ----------
// Ridge feet, the valley side edge of each ridge (Valley_map.svg rock bands). The chute's two rock arms step the W band out to
// x 86 between z 200 and 226 (IW2 fills the 3 m gap at the chute mouth, 8.7). Past the fence the arms end on a diagonal.
const float armEndSlope = 1f, fenceX = 396f;
float[] wfZ = { 10f, 85f, 100f, 180f, 195f, 290f }; float[] wfX = { 38f, 38f, 46f, 46f, 80f, 80f };
const float armZ0 = 200f, armZ1 = 226f, armX = 86f, gapZ0 = 211.5f, gapZ1 = 214.5f;
float FootW(float z) => (z >= armZ0 && z <= armZ1) ? armX : PL(z, wfZ, wfX);
float[] nfX = { 80f, 100f, 200f, 300f, 390f, 396f }; float[] nfZ = { 290f, 295f, 298f, 302f, 305f, 305f };
float FootN(float x) => x > fenceX ? 305f + (x - fenceX) * armEndSlope : PL(x, nfX, nfZ);
float[] sfX = { 38f, 120f, 200f, 300f, 390f, 396f }; float[] sfZ = { 10f, 2f, -4f, -2f, -8f, -8f };
float FootS(float x) => x > fenceX ? -8f - (x - fenceX) * armEndSlope : PL(x, sfX, sfZ);
bool InValley(float x, float z) => x >= FootW(z) && z <= FootN(x) && z >= FootS(x);
// the W foot apron (Valley.md 2.5: the W foot ground is about 12): within apronW of the W band between z 185 and 300 the valley
// rises to 12, so the band stands 4 m over the ground there and the chute mouth (12.6) is not a causeway up to the band top
const float apronH = 12f, apronW = 15f, apronZ0 = 185f, apronZ1 = 300f;
float ValleyA(float x, float z)
{
    float h = Valley(x, z); if (z < apronZ0 || z > apronZ1) return h;
    float d = x - FootW(z); return d < 0f || d > apronW ? h : UnityEngine.Mathf.Max(h, L(apronH, h, SS(d / apronW)));
}

// N and S arms: crest lines and heights (2.2; rev 10 N arm east half; Wren 2026-09-30: 50 held at x 170). Each arm falls to the
// valley at armK and away from it through its back slope; past the fence the heights fade to the verge by x 412.
const float armK = 1f;
float[] nCX = { 12f, 100f, 250f, 395f, 412f }; float[] nCZ = { 340f, 345f, 338f, 332f, 332f };
float[] nHX = { 15f, 40f, 60f, 80f, 120f, 170f, 250f, 320f, 380f, 400f, 412f }; float[] nHH = { 80f, 76f, 76f, 70f, 58f, 50f, 40f, 38f, 20f, 10f, 3f };
float[] sCX = { 14f, 100f, 240f, 395f, 412f }; float[] sCZ = { -40f, -45f, -38f, -30f, -30f };
float[] sHX = { 14f, 40f, 60f, 80f, 110f, 170f, 240f, 320f, 380f, 400f, 412f }; float[] sHH = { 76f, 72f, 72f, 66f, 50f, 32f, 45f, 30f, 16f, 8f, 3f };
float Back(float crest, float outer, float d) => d >= backLen ? outer : outer + (crest - outer) * (1f - d / backLen) * (1f - d / backLen);
// crests hold their height across a flat armCrestHalf either side (Wren 2026-09-30: the N arm holds 50 at x 170)
const float armCrestHalf = 1.5f;
float ArmN(float x, float z) { float cz = PL(x, nCX, nCZ), ch = PL(x, nHX, nHH); return z <= cz ? ch - armK * UnityEngine.Mathf.Max(0f, cz - armCrestHalf - z) : Back(ch, Outer(x, z), UnityEngine.Mathf.Max(0f, z - cz - armCrestHalf)); }
float ArmS(float x, float z) { float cz = PL(x, sCX, sCZ), ch = PL(x, sHX, sHH); return z >= cz ? ch - armK * UnityEngine.Mathf.Max(0f, z - cz - armCrestHalf) : Back(ch, Outer(x, z), UnityEngine.Mathf.Max(0f, cz - armCrestHalf - z)); }

// W ridge crest (2.2, 2.4): a flat top 6 m wide (x 10 to 16) at 80 from z 40 to 345, where daytime lines to the fire cross it;
// south of z 40 the top falls to the saddle 74 at z -10 and 76 at the SW corner (z -40). The knob and the shoulder sit on it.
float[] cLZ = { -45f, -40f, -10f, 40f }; float[] cLX = { 14f, 14f, 10f, 10f }; float[] cLH = { 76f, 76f, 74f, 80f };
const float crestTop = 80f, crestX0 = 10f, crestX1 = 16f, crestZ0 = 40f, crestZ1 = 345f;
float CrestH(float z) => z >= crestZ0 ? crestTop : PL(z, cLZ, cLH);
float CrestE(float z) => z >= crestZ0 ? crestX1 : PL(z, cLZ, cLX) + (crestX1 - crestX0);   // east edge of the crest top
const float knobX0 = 8f, knobX1 = 20f, knobZ0 = 200f, knobZ1 = 245f, knobTop = 84f;
const float shoulderX0 = 6f, shoulderX1 = 20f, shoulderZ0 = 245f, shoulderZ1 = 297f, shoulderTopS = 82f, shoulderTopN = 80f;
float CrestW(float z) => z >= shoulderZ0 && z <= shoulderZ1 ? shoulderX0 : z >= knobZ0 && z < shoulderZ0 ? knobX0 : z >= crestZ0 ? crestX0 : PL(z, cLZ, cLX);   // west edge of the top
float TopH(float z) => z >= shoulderZ0 && z <= shoulderZ1 ? L(shoulderTopS, shoulderTopN, (z - shoulderZ0) / (shoulderZ1 - shoulderZ0)) : z >= knobZ0 && z < shoulderZ0 ? knobTop : CrestH(z);

// ---------- the Ward climb (Valley.md rev 10 section 4) ----------
// Centre line with ground heights; the platforms are level (P1 4 x 4, P2 and P3 3 x 3, P4 6 m). P2 is 41.5 and P3 51.5 (each 0.5 m
// off table 4, inside WalkChecks 3's tolerance) so leg 3 holds 25 percent: at 41 and 52 its 41.8 m of slope would be 26.3 percent.
// Leg 1: from the chute mouth, ramps at 24.5 percent between three flights of 18 cut steps (0.25 rise, 0.3 run); the steps are
// looks only (8.7), the ground under them is their nosing line (40 degrees, walkable), so no StairRamp is needed.
const float mouthH = 12.6f, p1H = 30f, p2H = 41.5f, p3H = 51.5f, p4H = 60f, exitH = 61f, endH = 62f;
var pJ = P(104f, 206f); var pMouth = P(86f, 213f); var pP1 = P(52f, 216f); var pP2 = P(57f, 276f); var pL3 = P(44f, 278f);
// 8.14: leg 3 bends again at (34, 300) and meets P3 from the east; straight from (44, 278) it closed on leg 4 at 23 degrees and ran
// into leg 4's bench near P3
var pP3 = P(26f, 304f); var pL3b = P(34f, 300f); var pL4 = P(30f, 284f); var pP4 = P(26f, 262f);
UnityEngine.Vector2 Toward(UnityEngine.Vector2 a, UnityEngine.Vector2 b, float d) => a + (b - a).normalized * d;
const float p1Half = 2f, p23Half = 1.5f, p4Half = 3f;
var p1In = Toward(pP1, pMouth, p1Half); var p1Out = Toward(pP1, pP2, p1Half);
var p2In = Toward(pP2, pP1, p23Half); var p2Out = Toward(pP2, pL3, p23Half);
var p3In = Toward(pP3, pL3b, p23Half); var p3Out = Toward(pP3, pL4, p23Half);
var p4In = Toward(pP4, pL4, p4Half);
var slotIn = P(24.5f, 262f); var slotDog0 = P(18f, 262f); var slotDog1 = P(14.5f, 265.5f); var slotEnd = P(4f, 265.5f); var slotExit = P(4f, 257.3f);   // table 4 has (4, 262.5); 5.2 m further south so the fin covers the whole slot mouth (8.14 F-1: from the mouth's south corner a ray passed the fin's end at 262.4)
var pathEnd = P(-8.5f, 246f);
// leg 3 and leg 4 heights at their bends, by walked length between the platforms
float lenL3a = UnityEngine.Vector2.Distance(p2Out, pL3), lenL3b = UnityEngine.Vector2.Distance(pL3, pL3b), lenL3c = UnityEngine.Vector2.Distance(pL3b, p3In), lenL3 = lenL3a + lenL3b + lenL3c;
float hL3 = p2H + (p3H - p2H) * lenL3a / lenL3, hL3b = p2H + (p3H - p2H) * (lenL3a + lenL3b) / lenL3;
float lenL4a = UnityEngine.Vector2.Distance(p3Out, pL4), lenL4b = UnityEngine.Vector2.Distance(pL4, p4In);
float hL4 = p3H + (p4H - p3H) * lenL4a / (lenL4a + lenL4b);
float lenSlot = UnityEngine.Vector2.Distance(slotIn, slotDog0) + UnityEngine.Vector2.Distance(slotDog0, slotDog1) + UnityEngine.Vector2.Distance(slotDog1, slotEnd) + UnityEngine.Vector2.Distance(slotEnd, slotExit);
float SlotH(float s) => L(p4H, exitH, s / lenSlot);
float sDog0 = UnityEngine.Vector2.Distance(slotIn, slotDog0), sDog1 = sDog0 + UnityEngine.Vector2.Distance(slotDog0, slotDog1), sEnd = sDog1 + UnityEngine.Vector2.Distance(slotDog1, slotEnd);
// the climb as one polyline (8.3 lays the J to Ward trail on the same points; the check recipes walk it)
var climb = new (UnityEngine.Vector2 p, float h)[] {
    (pJ, 10f), (pMouth, mouthH), (p1In, p1H), (pP1, p1H), (p1Out, p1H), (p2In, p2H), (pP2, p2H), (p2Out, p2H), (pL3, hL3), (pL3b, hL3b), (p3In, p3H), (pP3, p3H), (p3Out, p3H),
    (pL4, hL4), (p4In, p4H), (pP4, p4H), (slotIn, p4H), (slotDog0, SlotH(sDog0)), (slotDog1, SlotH(sDog1)), (slotEnd, SlotH(sEnd)), (slotExit, exitH), (pathEnd, endH) };
// leg 1 ground along the chute, from the mouth: ramp, flight, ramp, flight, ramp, flight, ramp
const int stepsPerFlight = 18; const float stepRise = 0.25f, stepRun = 0.3f;
float leg1Len = UnityEngine.Vector2.Distance(pMouth, p1In), flightRun = stepsPerFlight * stepRun, flightRise = stepsPerFlight * stepRise;
float rampRun = (leg1Len - 3f * flightRun) / 4f, rampRise = (p1H - mouthH - 3f * flightRise) / 4f;
float Leg1H(float s)
{
    float h = mouthH;
    for (int i = 0; i < 7; i++)
    {
        bool flight = i % 2 == 1; float run = flight ? flightRun : rampRun, rise = flight ? flightRise : rampRise;
        if (s <= run) return h + rise * UnityEngine.Mathf.Clamp01(s / run);
        s -= run; h += rise;
    }
    return h;
}
// W ridge east face as shelves, west uphill (Valley.md 4.4 and 8): each shelf is level west of its east edge xb(z) inside its z
// range, and falls at k east of that edge and past its z ends. The face is the max of all shelves, so every bench runs level
// to the foot of the face above it (no gutter between) and every face between benches is at least 63 degrees (k 2).
const float faceK = 2f, cwmWallK = 4f, shoulderK = 5f, leg4K = 4f, legHalf = 2f, leg4Half = 1.5f;
float X2(float z) => L(p1Out.x, p2In.x, (z - p1Out.y) / (p2In.y - p1Out.y));
float H2(float z) => L(p1H, p2H, (z - p1Out.y) / (p2In.y - p1Out.y));
float X4(float z) => z >= pL4.y ? L(pL4.x, p3Out.x, (z - pL4.y) / (p3Out.y - pL4.y)) : L(p4In.x, pL4.x, (z - p4In.y) / (pL4.y - p4In.y));
float H4(float z) => z >= pL4.y ? L(hL4, p3H, (z - pL4.y) / (p3Out.y - pL4.y)) : L(p4H, hL4, (z - p4In.y) / (pL4.y - p4In.y));
var shelves = new (float z0, float z1, System.Func<float, float> xb, System.Func<float, float> h, float k)[] {
    (-45f, 350f, CrestE, CrestH, faceK),                                                                     // crest top (k 4 by the cwm, below)
    (knobZ0, knobZ1, z => knobX1, z => knobTop, faceK),                                                       // the knob
    (shoulderZ0, shoulderZ1, z => shoulderX1, z => L(shoulderTopS, shoulderTopN, (z - shoulderZ0) / (shoulderZ1 - shoulderZ0)), shoulderK),   // the shoulder: the crest wall over leg 4
    (pP1.y - p1Half, pP1.y + p1Half, z => pP1.x + p1Half, z => p1H, faceK),                                  // P1
    (p1Out.y, p2In.y, z => X2(z) + legHalf, H2, faceK),                                                       // leg 2, the exposed shelf
    (pP2.y - p23Half, pP2.y + p23Half, z => pP2.x + p23Half, z => p2H, faceK),                                // P2
    (p4In.y, p3Out.y, z => X4(z) + leg4Half, H4, leg4K),                                                      // leg 4, under the wall
    (pP3.y - p23Half, pP3.y + p23Half, z => pP3.x + p23Half, z => p3H, leg4K),                                // P3
    (pP4.y - p4Half, pP4.y + p4Half, z => pP4.x + 4f, z => p4H, faceK),                                       // P4, the look-back (x 24.5 to 30)
};
// the crest's east face is the wall over the whole climb (the knob, P4, leg 4 and the cwm's west wall) at k 4: at k 2 it buried leg 4's
// bench under the shoulder's north end (8.14, a 4 m step at z 297)
const float cwmZ0 = 195f, cwmZ1 = 350f;
float WEast(float x, float z)
{
    float best = float.MinValue;
    for (int i = 0; i < shelves.Length; i++)
    {
        var s = shelves[i]; float zc = UnityEngine.Mathf.Clamp(z, s.z0, s.z1), k = i == 0 && z > cwmZ0 && z < cwmZ1 ? cwmWallK : s.k;
        best = UnityEngine.Mathf.Max(best, s.h(zc) - k * (UnityEngine.Mathf.Max(0f, x - s.xb(zc)) + UnityEngine.Mathf.Abs(z - zc)));
    }
    return best;
}
// the burned cwm (leg 3): a bowl floor at the leg's height round the leg, rims all round (1.6): the crest wall west, the N hook top
// north, an east rim; each falls into the bowl at k 2
float SegDist2(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b, out float t) => SegDist(p, a, b, out t);
var cwmC = P(34f, 298f); const float cwmRX = 12f, cwmRZ = 14f;
float Leg3(float x, float z)
{
    var p = P(x, z);
    float d1 = SegDist2(p, p2Out, pL3, out float t1), d2 = SegDist2(p, pL3, pL3b, out float t2), d3 = SegDist2(p, pL3b, p3In, out float t3);
    float hq = d1 <= d2 && d1 <= d3 ? L(p2H, hL3, t1) : d2 <= d3 ? L(hL3, hL3b, t2) : L(hL3b, p3H, t3), d = UnityEngine.Mathf.Min(d1, UnityEngine.Mathf.Min(d2, d3)) - legHalf;
    var q = p - cwmC; float re = UnityEngine.Mathf.Sqrt((q.x / cwmRX) * (q.x / cwmRX) + (q.y / cwmRZ) * (q.y / cwmRZ));
    float dBowl = (re - 1f) * UnityEngine.Mathf.Min(cwmRX, cwmRZ);
    return hq - faceK * UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Min(d, dBowl));
}
const float nwX0 = 10f, nwX1 = 70f, nwZ0 = 322f, nwZ1 = 350f;
float[] nwHX = { 20f, 40f, 60f, 70f }; float[] nwHH = { 80f, 76f, 76f, 72f };
float NWTop(float x, float z)   // the N hook's top over the cwm
{
    float xc = UnityEngine.Mathf.Clamp(x, nwX0, nwX1), zc = UnityEngine.Mathf.Clamp(z, nwZ0, nwZ1);
    return PL(xc, nwHX, nwHH) - faceK * (UnityEngine.Mathf.Abs(x - xc) + UnityEngine.Mathf.Abs(z - zc));
}
var rimA = P(54f, 300f); var rimB = P(54f, 324f); const float rimHalf = 1.5f, rimHA = 72f, rimHB = 76f;
float CwmRim(float x, float z) { float d = SegDist2(P(x, z), rimA, rimB, out float t); return L(rimHA, rimHB, t) - faceK * UnityEngine.Mathf.Max(0f, d - rimHalf); }
// leg 1, the boulder chute: a trench from the mouth to P1, 3.2 m floor, walls 3 m (near vertical), on ribs that fall away at k 2
// the ribs peak at the wall top (ribHalf just past the wall), so their tops are not ground anyone walks along (8.14: a 2.4 m flat rib
// top ran level with the chute floor further up and joined it, so the climb ring left the chute's north side open)
const float chuteHalf = 1.6f, chuteWall = 3f, chuteWallK = 6f, ribHalf = chuteHalf + chuteWall / chuteWallK;
// both stop at P1: past the chute's end (t 1) P1 and leg 2 shape the ground
float ChuteRib(float x, float z) { float d = SegDist2(P(x, z), pMouth, p1In, out float t); if (t >= 1f) return float.MinValue; return Leg1H(t * leg1Len) + chuteWall - faceK * UnityEngine.Mathf.Max(0f, d - ribHalf); }
float ChuteCut(float x, float z) { float d = SegDist2(P(x, z), pMouth, p1In, out float t); if (t >= 1f) return float.MaxValue; return Leg1H(t * leg1Len) + chuteWallK * UnityEngine.Mathf.Max(0f, d - chuteHalf); }
// the cleft: a slot 2.5 m wide through the shoulder, walls to the top; the ledge; the fin and the rock round it are meshes (below)
var slotPts = new[] { slotIn, slotDog0, slotDog1, slotEnd, slotExit }; const float slotHalf = 1.25f, slotWallK = 25f;
float SlotCut(float x, float z)
{
    var p = P(x, z); float best = float.MaxValue, hs = p4H, acc = 0f;
    for (int i = 0; i < slotPts.Length - 1; i++)
    {
        float d = SegDist(p, slotPts[i], slotPts[i + 1], out float t), sl = UnityEngine.Vector2.Distance(slotPts[i], slotPts[i + 1]);
        if (d < best) { best = d; hs = SlotH(acc + t * sl); }
        acc += sl;
    }
    return hs + slotWallK * UnityEngine.Mathf.Max(0f, best - slotHalf);
}
const float ledgeX0 = -10f, ledgeX1 = 6f, ledgeZ0 = 215f, ledgeZ1 = 285f, ledgeBlend = 16f;
float LedgeH(float x, float z) => L(exitH, endH, SS(UnityEngine.Vector2.Distance(P(x, z), slotExit) / ledgeBlend));
// the approach, J to the chute mouth, carved on the valley side (flat 2 m each side, blended 1.5 m)
float ApproachH(UnityEngine.Vector2 p, out float d) { d = SegDist(p, pJ, pMouth, out float t); return L(10f, mouthH, t); }

// ---------- the highway (3.2): x 428, curving away west behind the arm ends at z 430 and z -130 ----------
const float roadX = 428f, roadN = 430f, roadS = -130f, roadR = 120f, roadY = 3f, roadHalf = 5f, ditchW = 2.5f, ditchD = 0.8f, roadBlend = 12f;
float RoadDist(float x, float z)
{
    if (z >= roadS && z <= roadN) return UnityEngine.Mathf.Abs(x - roadX);
    var c = z > roadN ? P(roadX - roadR, roadN) : P(roadX - roadR, roadS); var q = P(x, z) - c;
    if (q.x < 0f) return float.MaxValue;   // the arcs turn 90 degrees west; nothing past that
    return UnityEngine.Mathf.Abs(q.magnitude - roadR);
}
float RoadCarve(float x, float z, float h)
{
    float d = RoadDist(x, z); if (d >= roadHalf + ditchW + roadBlend) return h;
    if (d <= roadHalf) return roadY;
    if (d <= roadHalf + ditchW) return roadY - ditchD * UnityEngine.Mathf.Sin(UnityEngine.Mathf.PI * (d - roadHalf) / ditchW);
    return L(roadY, h, SS((d - roadHalf - ditchW) / roadBlend));
}

// band plateau: the land just behind each band is the band's top (foot ground plus 4 m west, 3 m north and south)
const float bandW = 4f, bandNS = 3f;
float Plateau(float x, float z, float footWG, float footNG, float footSG)
{
    float best = float.MinValue;
    if (x < FootW(z) && x > CrestE(z) && z > 10f && z < 290f) best = UnityEngine.Mathf.Max(best, footWG + bandW);
    if (z > FootN(x) && z < PL(x, nCX, nCZ) && x > 80f) best = UnityEngine.Mathf.Max(best, footNG + bandNS);
    if (z < FootS(x) && z > PL(x, sCX, sCZ) && x > 38f) best = UnityEngine.Mathf.Max(best, footSG + bandNS);
    return best;
}

// rock in the ridges (8.14a, Vesper and Marlow: a fortress of planes, the knob a pyramid, the arms smooth wedges): ridged noise that
// only ever raises the ground (so every F-1 and W-1 line keeps its cover), strong on high ground and gone near the feet; in the climb
// only above the highest bench (climbKeep), so the legs, platforms and slot keep their shape
const float rugBig = 30f, rugSmall = 9f, rugBigH = 7f, rugSmallH = 3f, rugFrom = 20f, rugRamp = 15f, climbKeep = 66f, climbKeepRamp = 10f;
bool InClimbZone(float x, float z) => x > -12f && x < 92f && z > 190f && z < 335f;
float Rugged(float x, float z, float h)
{
    float n1 = 1f - UnityEngine.Mathf.Abs(2f * UnityEngine.Mathf.PerlinNoise(x / rugBig + 1.7f, z / rugBig + 4.3f) - 1f);
    float n2 = 1f - UnityEngine.Mathf.Abs(2f * UnityEngine.Mathf.PerlinNoise(x / rugSmall + 8.2f, z / rugSmall + 2.6f) - 1f);
    float mask = InClimbZone(x, z) ? SS((h - climbKeep) / climbKeepRamp) : SS((h - rugFrom) / rugRamp);
    return h + (rugBigH * n1 * n1 + rugSmallH * n2) * mask;
}
const float roadEastX = 440f, roadEastRamp = 120f;
float Height(float x, float z, float footWG, float footNG, float footSG)
{
    var p = P(x, z); float h;
    if (InValley(x, z))
    {
        h = ValleyA(x, z);
        if (x > roadEastX) h = L(3f, Outer(x, z), SS((x - roadEastX) / roadEastRamp));   // past the road the land rolls up to the outer ground
        if (x > armX && x < 110f) { float ah = ApproachH(p, out float d); if (d < 3.5f) h = L(ah, h, SS((d - 2f) / 1.5f)); }
    }
    else
    {
        h = UnityEngine.Mathf.Max(Plateau(x, z, footWG, footNG, footSG), UnityEngine.Mathf.Max(ArmN(x, z), ArmS(x, z)));
        if (z > -60f && z < 360f) h = UnityEngine.Mathf.Max(h, WEast(x, z));
        if (x < 70f && z > 270f && z < 360f) h = UnityEngine.Mathf.Max(h, UnityEngine.Mathf.Max(Leg3(x, z), UnityEngine.Mathf.Max(NWTop(x, z), CwmRim(x, z))));
        h = Rugged(x, z, h);
        // the chute: ribs, then the trench cut into them and into the knob's face
        if (x < armX + 1f && x > 40f && z > 195f && z < 232f) { h = UnityEngine.Mathf.Max(h, ChuteRib(x, z)); h = UnityEngine.Mathf.Min(h, ChuteCut(x, z)); }
        // west of the top the W face falls to the -40 floor at x -40; past the ridge's ends it folds onto the outer ground
        float zc = UnityEngine.Mathf.Clamp(z, -45f, 350f), xw = CrestW(zc), th = TopH(zc);
        if (x < xw)
        {
            float out0 = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(-45f - z, z - 350f));
            float westFace = L(th + (x - xw) * (th - westFloor) / (xw - westEdge), Outer(x, z), SS(out0 / wFade));
            h = UnityEngine.Mathf.Min(h, westFace);
        }
        if (x >= ledgeX0 && x <= ledgeX1 && z >= ledgeZ0 && z <= ledgeZ1) h = LedgeH(x, z);
        if (x < 26f && x > 0f && z > 255f && z < 270f) h = UnityEngine.Mathf.Min(h, SlotCut(x, z));
    }
    return RoadCarve(x, z, h);
}


if (!UnityEditor.AssetDatabase.IsValidFolder(dir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Terrain", "Main3");
// Layers and their textures are imported before the terrain data exists: an import refresh can reload an unsaved TerrainData empty.
UnityEngine.TerrainLayer Layer(string name, float g, float warm, float tile)
{
    string texPath = dir + "/Gray_" + name + ".png";
    var tex = new UnityEngine.Texture2D(8, 8, UnityEngine.TextureFormat.RGB24, false);
    var col = new UnityEngine.Color(g + warm, g, g - warm);
    var px = new UnityEngine.Color[64];
    for (int i = 0; i < 64; i++) { float n = ((i * 7 + i / 8 * 3) % 5) * 0.006f; px[i] = col + new UnityEngine.Color(n, n, n); }
    tex.SetPixels(px); tex.Apply();
    System.IO.File.WriteAllBytes(texPath, tex.EncodeToPNG());
    UnityEngine.Object.DestroyImmediate(tex);
    UnityEditor.AssetDatabase.ImportAsset(texPath);
    var layer = new UnityEngine.TerrainLayer { diffuseTexture = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(texPath), tileSize = new UnityEngine.Vector2(tile, tile) };
    UnityEditor.AssetDatabase.CreateAsset(layer, dir + "/Layer_" + name + ".terrainlayer");
    return layer;
}
var layers = new[] { Layer("Ground", 0.50f, 0f, 4f), Layer("Rock", 0.33f, 0f, 4f), Layer("Burn", 0.42f, 0.025f, 4f), Layer("Trail", 0.64f, 0.01f, 2f), Layer("LakeBed", 0.36f, -0.03f, 4f) };
// ---------- terrain data ----------
var data = new UnityEngine.TerrainData();
data.heightmapResolution = res;
data.size = V(sizeX, sizeY, sizeZ);
data.alphamapResolution = ares;
data.baseMapResolution = 1024;
UnityEditor.AssetDatabase.CreateAsset(data, dir + "/Main3_TerrainData.asset");
var hm = new float[res, res];
// the valley ground at each foot, once per row (W) and column (N, S): the band plateau behind it is that plus the band height
var footWG = new float[res]; var footNG = new float[res]; var footSG = new float[res];
for (int i = 0; i < res; i++)
{
    float x = originX + i * sizeX / (res - 1), z = originZ + i * sizeZ / (res - 1);
    footWG[i] = ValleyA(FootW(z) + 1f, z); footNG[i] = ValleyA(x, FootN(x) - 1f); footSG[i] = ValleyA(x, FootS(x) + 1f);
}
for (int zi = 0; zi < res; zi++)
    for (int xi = 0; xi < res; xi++)
        hm[zi, xi] = Clamp01((Height(originX + xi * sizeX / (res - 1), originZ + zi * sizeZ / (res - 1), footWG[zi], footNG[xi], footSG[xi]) - baseY) / sizeY);
// the dirt spikes the gate found (8.14a: east of Camp 3, above the cave spur, west of W1; slivers where the rev 16 creek, ravine and
// hollow shapes meet): inside these circles no sample may stand more than spikeRise over the mean of its neighbours within
// spikeReach cells, repeated spikePasses times (it only lowers)
var spikeZones = new (UnityEngine.Vector2 c, float r)[] { (P(90f, 146f), 12f), (P(62f, 48f), 14f), (P(118f, 66f), 10f) };
const int spikeReach = 3, spikePasses = 4; const float spikeRise = 0.4f;
for (int pass = 0; pass < spikePasses; pass++)
    foreach (var zone in spikeZones)
    {
        int xi0 = UnityEngine.Mathf.Max(spikeReach, (int)((zone.c.x - zone.r - originX) / sizeX * (res - 1))), xi1 = UnityEngine.Mathf.Min(res - 1 - spikeReach, (int)((zone.c.x + zone.r - originX) / sizeX * (res - 1)));
        int zi0 = UnityEngine.Mathf.Max(spikeReach, (int)((zone.c.y - zone.r - originZ) / sizeZ * (res - 1))), zi1 = UnityEngine.Mathf.Min(res - 1 - spikeReach, (int)((zone.c.y + zone.r - originZ) / sizeZ * (res - 1)));
        for (int zi = zi0; zi <= zi1; zi++) for (int xi = xi0; xi <= xi1; xi++)
        {
            float x = originX + xi * sizeX / (res - 1), z = originZ + zi * sizeZ / (res - 1); if (UnityEngine.Vector2.Distance(P(x, z), zone.c) > zone.r) continue;
            float sum = 0f; int cnt = 0; for (int a = -spikeReach; a <= spikeReach; a++) for (int b = -spikeReach; b <= spikeReach; b++) if (a != 0 || b != 0) { sum += hm[zi + b, xi + a]; cnt++; }
            float cap = sum / cnt + spikeRise / sizeY; if (hm[zi, xi] > cap) hm[zi, xi] = cap;
        }
    }
data.SetHeights(0, 0, hm);
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();

data.terrainLayers = layers;
var alpha = new float[ares, ares, layers.Length];
for (int zi = 0; zi < ares; zi++)
    for (int xi = 0; xi < ares; xi++)
    {
        float x = originX + (xi + 0.5f) * sizeX / ares, z = originZ + (zi + 0.5f) * sizeZ / ares;
        float steep = data.GetSteepness((xi + 0.5f) / ares, (zi + 0.5f) / ares);
        var q = P(x, z) - lakeC; float re = UnityEngine.Mathf.Sqrt((q.x / lakeA) * (q.x / lakeA) + (q.y / lakeB) * (q.y / lakeB));
        int k = steep > 35f ? 1 : re < 1.03f ? 4 : Inside(P(x, z), burnPoly) ? 2 : 0;
        alpha[zi, xi, k] = 1f;
    }
data.SetAlphamaps(0, 0, alpha);
UnityEditor.EditorUtility.SetDirty(data);

var terrainGo = UnityEngine.Terrain.CreateTerrainGameObject(data);
terrainGo.name = "Terrain";
terrainGo.transform.position = V(originX, baseY, originZ);
var terrain = terrainGo.GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + baseY;

// ---------- rock: every stop the land makes is a visible rock mesh with a collider (Valley.md 8; root "Rock") ----------
// Rock bands along the ridge feet (4 m over the valley ground west, 3 m north and south), the two rock arms at the chute mouth,
// an outcrop tying each fence end into its band, the rock steps on the uphill side of every climb bench and along the chute and
// the cleft, the fin at the cleft mouth, the lip, the walls across the ledge ends and at the back of the ledge.
// F-1 counts this root as land (solid rock), never trees.
const string rockDir = dir + "/Rock";
if (!UnityEditor.AssetDatabase.IsValidFolder(rockDir)) UnityEditor.AssetDatabase.CreateFolder(dir, "Rock");
const string rockMatPath = "Assets/Materials/Blockout/Blockout_BandRock.mat";
const string rockTexPath = "Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Rocks_a.png";   // owned pack texture (git-ignored); plain granite if absent
const float rockTexLuma = 0.55f, rockTile = 4f, rockSmooth = 0.1f;
var rockMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(rockMatPath);
if (rockMat == null) { rockMat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Lit")); UnityEditor.AssetDatabase.CreateAsset(rockMat, rockMatPath); }
{
    UnityEngine.ColorUtility.TryParseHtmlString("#6E6660", out var granite);   // Style.md granite
    var rockTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(rockTexPath);
    rockMat.SetTexture("_BaseMap", rockTex);
    rockMat.SetColor("_BaseColor", rockTex != null ? new UnityEngine.Color(granite.r / rockTexLuma, granite.g / rockTexLuma, granite.b / rockTexLuma) : granite);
    rockMat.SetFloat("_Smoothness", rockSmooth); UnityEditor.EditorUtility.SetDirty(rockMat);
}
var rockRoot = new UnityEngine.GameObject("Rock");
var rockRng = new System.Random(8141);
var boulderRng = new System.Random(8142);   // owned boulders and rubble along the rock
float RR(float a, float b) => a + (float)rockRng.NextDouble() * (b - a);
int rockMeshes = 0; float rockLength = 0f;
System.Collections.Generic.List<UnityEngine.Vector2> Resample(UnityEngine.Vector2[] poly, float step)
{
    var outp = new System.Collections.Generic.List<UnityEngine.Vector2>();
    for (int i = 0; i < poly.Length - 1; i++)
    {
        float len = UnityEngine.Vector2.Distance(poly[i], poly[i + 1]); int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(len / step));
        for (int k = 0; k < n; k++) outp.Add(UnityEngine.Vector2.Lerp(poly[i], poly[i + 1], k / (float)n));
    }
    outp.Add(poly[poly.Length - 1]); return outp;
}
UnityEngine.Vector2 LeftN(System.Collections.Generic.List<UnityEngine.Vector2> pts, int i)
{
    var t = (pts[UnityEngine.Mathf.Min(i + 1, pts.Count - 1)] - pts[UnityEngine.Mathf.Max(i - 1, 0)]).normalized; return P(-t.y, t.x);
}
// one wall: its front face on the polyline (moved 'offset' toward the right, then pushed back into the rock by up to 'push'),
// its body 'thick' metres to the left, from yb to yt at each point. Faces are wound outward; the mesh is its own collider.
void RockWall(string name, UnityEngine.Transform parent, System.Collections.Generic.List<UnityEngine.Vector2> pts, float[] yb, float[] yt, float thick, float offset, float push, float lean = 0f)   // lean: how far the face's top sits back into the rock (a leaning face)
{
    int n = pts.Count; if (n < 2) return;
    var fb = new UnityEngine.Vector3[n]; var ft = new UnityEngine.Vector3[n]; var bt = new UnityEngine.Vector3[n]; var bb = new UnityEngine.Vector3[n]; var along = new float[n];
    for (int i = 0; i < n; i++)
    {
        var nl = LeftN(pts, i); var f = pts[i] - nl * offset + nl * RR(0f, push); var b = pts[i] + nl * thick;
        fb[i] = V(f.x, yb[i], f.y); var fl = f + nl * lean; ft[i] = V(fl.x, yt[i], fl.y); bt[i] = V(b.x, yt[i], b.y); bb[i] = V(b.x, yb[i], b.y);
        along[i] = i == 0 ? 0f : along[i - 1] + UnityEngine.Vector2.Distance(pts[i - 1], pts[i]);
    }
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var uvs = new System.Collections.Generic.List<UnityEngine.Vector2>(); var tris = new System.Collections.Generic.List<int>();
    // world-space UVs: sides by (x + z, y), tops by (x, z), one rock tile every rockTile metres
    void Quad(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Vector3 c, UnityEngine.Vector3 d, UnityEngine.Vector3 outward)
    {
        if (UnityEngine.Vector3.Dot(UnityEngine.Vector3.Cross(b - a, c - a), outward) < 0f) { var s = b; b = d; d = s; }
        bool top = outward.y > 0.5f; int i0 = vs.Count;
        foreach (var v in new[] { a, b, c, d }) { vs.Add(v); uvs.Add(top ? new UnityEngine.Vector2(v.x / rockTile, v.z / rockTile) : new UnityEngine.Vector2((v.x + v.z) / rockTile, v.y / rockTile)); }
        tris.AddRange(new[] { i0, i0 + 1, i0 + 2, i0, i0 + 2, i0 + 3 });
    }
    for (int i = 0; i < n - 1; i++)
    {
        var nl = LeftN(pts, i); var o = V(nl.x, 0f, nl.y);
        Quad(fb[i], fb[i + 1], ft[i + 1], ft[i], -o);
        Quad(bb[i], bb[i + 1], bt[i + 1], bt[i], o);
        Quad(ft[i], ft[i + 1], bt[i + 1], bt[i], UnityEngine.Vector3.up);
    }
    var t0 = pts[1] - pts[0]; var t1 = pts[n - 1] - pts[n - 2];
    Quad(fb[0], ft[0], bt[0], bb[0], -V(t0.x, 0f, t0.y));
    Quad(fb[n - 1], ft[n - 1], bt[n - 1], bb[n - 1], V(t1.x, 0f, t1.y));
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.SetVertices(vs); mesh.SetUVs(0, uvs); mesh.SetTriangles(tris, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, rockDir + "/" + name + ".asset");
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(parent, false);
    g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; g.AddComponent<UnityEngine.MeshRenderer>().sharedMaterial = rockMat;
    g.AddComponent<UnityEngine.MeshCollider>().sharedMesh = mesh;
    rockMeshes++; rockLength += along[n - 1];
}
// a wall from explicit end points: level top, base under the lowest ground along it
void FlatWall(string name, UnityEngine.Transform parent, UnityEngine.Vector2[] poly, float top, float thick, float jag)
{
    var pts = Resample(poly, 1f); var yb = new float[pts.Count]; var yt = new float[pts.Count];
    for (int i = 0; i < pts.Count; i++) { var nl = LeftN(pts, i); yb[i] = UnityEngine.Mathf.Min(H(pts[i].x, pts[i].y), H(pts[i].x + nl.x * thick, pts[i].y + nl.y * thick)) - 1f; yt[i] = top + RR(0f, jag); }
    RockWall(name, parent, pts, yb, yt, thick, 0f, 0.15f);
}
// bands: front face 0.7 m in front of the foot line, so the terrain's one-cell step up at the foot stays inside the rock
// 8.14a: the band face leans back bandLean over its height (about 75 degrees on a 4 m band), its top swells in bandWave-sample waves,
// the jag per metre is small, and owned rubble lies at its foot (Vesper: strata, not masonry)
const float bandThick = 3f, bandOffset = 0.7f, bandJag = 0.2f, bandFront = 1.5f, bandPush = 0.3f, bandLean = 1.1f, bandSwell = 1.5f, bandWave = 7f, screeOut = 1.2f;
const int screeStep = 10; int screeN = 0; var scree = new UnityEngine.GameObject("BandScree").transform; scree.SetParent(rockRoot.transform, false);
var bands = new UnityEngine.GameObject("Bands").transform; bands.SetParent(rockRoot.transform, false);
void Band(string name, UnityEngine.Vector2[] poly, System.Func<UnityEngine.Vector2, float> bandH)
{
    var pts = Resample(poly, 1f); var yb = new float[pts.Count]; var yt = new float[pts.Count];
    for (int i = 0; i < pts.Count; i++)
    {
        var nl = LeftN(pts, i); var front = pts[i] - nl * (bandOffset + bandFront); float g = H(front.x, front.y);
        yb[i] = UnityEngine.Mathf.Min(g, H(pts[i].x + nl.x * bandThick, pts[i].y + nl.y * bandThick)) - 1f; yt[i] = g + bandH(pts[i]) + bandSwell * UnityEngine.Mathf.PerlinNoise(i / bandWave + 0.5f, 3.3f) + RR(0f, bandJag);   // tops swell in long waves, not a battlement
    }
    RockWall(name, bands, pts, yb, yt, bandThick, bandOffset, bandPush, bandLean);
    // scree at the foot: owned rubble in front of the band every screeStep metres
    for (int i = 0; i < pts.Count; i += screeStep)
    {
        var nl = LeftN(pts, i); var q = pts[i] - nl * (bandOffset + screeOut + RR(0f, 1f));
        var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/BK/PureNature_Redwood/Prefabs/Rocks/RubbleSparse_" + (1 + boulderRng.Next(3)) + ".prefab"); if (pf == null) continue;
        var gs = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pf, scree); foreach (var c in gs.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
        gs.transform.rotation = UnityEngine.Quaternion.Euler(0f, RR(0f, 360f), 0f); gs.transform.position = V(q.x, H(q.x, q.y) - 0.1f, q.y); screeN++;
    }
}
float BandHAt(UnityEngine.Vector2 q) => q.y < FootS(q.x) + 1f || q.y > FootN(q.x) - 1f ? bandNS : bandW;   // N and S 3 m, W and the arms 4 m
// south band east to west, then the W band north to the south arm's end at the chute gap; the north arm from the gap to the N band
Band("Band_S_W", new[] { P(396f, -8f), P(390f, -8f), P(300f, -2f), P(200f, -4f), P(120f, 2f), P(38f, 10f), P(38f, 85f), P(46f, 100f), P(46f, 180f), P(80f, 195f), P(80f, 200f), P(armX, armZ0), P(armX, gapZ0) }, BandHAt);
Band("Band_W_N", new[] { P(armX, gapZ1), P(armX, armZ1), P(80f, armZ1), P(80f, 290f), P(100f, 295f), P(200f, 298f), P(300f, 302f), P(390f, 305f), P(396f, 305f) }, BandHAt);
// the fence ends tied into the bands (Valley.md 8): an outcrop over each end
const float outcropTop = 5f, outcropThick = 9f;
FlatWall("Outcrop_N", bands, new[] { P(391.5f, 302.5f), P(403f, 302.5f) }, H(fenceX, 302f) + outcropTop, outcropThick, bandJag);
FlatWall("Outcrop_S", bands, new[] { P(403f, -5.5f), P(391.5f, -5.5f) }, H(fenceX, -6f) + outcropTop, outcropThick, bandJag);
// the ledge (4.7 and 8, rev 10): the lip 0.8 m over the ledge along x -10; rock walls across both ends, 4.5 m over the ledge and
// past the lip into the west face; the back wall along the knob and shoulder faces, 3 m over the ledge, open at the cleft exit
// the lip is 1.1 m, not table 4.7's 0.8: in 8.14's Play check a sprint-jump rode the capsule over a 0.8 m rim (the capsule's round foot at the
// top of a 0.6 m hop meets the rim's edge below its centre and slides up); 1.1 clears the hop plus the 0.35 m radius
const float rimHeight = 1.1f;
// the ring's rims (8.14a, Wren) stand 1.2 m over the highest walkable ground beside them, in a line of boulders (8.14: a 0.5 m thick 1.2 m
// rim was sprint-jumped before the slide rule); its walls stand 3.5 m over the walkable ground beside them, tops broken in blocks
const float ringRimHeight = 1.2f;
var ledgeRock = new UnityEngine.GameObject("Ledge").transform; ledgeRock.SetParent(rockRoot.transform, false);
const float lipH = rimHeight, lipThick = 1.2f, endWallTop = 66.5f, endWallThick = 2f, backWallH = 3f, backWallThick = 1.5f, finTop = 78f;
{
    var lip = Resample(new[] { P(ledgeX0, ledgeZ0), P(ledgeX0, ledgeZ1) }, 1f); var yb = new float[lip.Count]; var yt = new float[lip.Count];
    for (int i = 0; i < lip.Count; i++) { yb[i] = H(ledgeX0 - lipThick, lip[i].y) - 1f; yt[i] = LedgeH(ledgeX0, lip[i].y) + lipH + RR(0f, 0.1f); }
    RockWall("Lip", ledgeRock, lip, yb, yt, lipThick, 0f, 0f);
}
FlatWall("EndWall_N", ledgeRock, new[] { P(-12f, ledgeZ1), P(ledgeX1 + 0.5f, ledgeZ1) }, endWallTop, endWallThick, 0.4f);
FlatWall("EndWall_S", ledgeRock, new[] { P(knobX0 + 0.5f, ledgeZ0), P(-12f, ledgeZ0) }, endWallTop, endWallThick, 0.4f);
// the back wall runs on north to the slot's south wall, so the terrain's one-cell slope at the slot mouth's south corner shows no sky
FlatWall("BackWall_S", ledgeRock, new[] { P(ledgeX1, slotEnd.y - slotHalf - 0.05f), P(ledgeX1, ledgeZ0 - 1f) }, endH + backWallH, backWallThick, 0.3f);
FlatWall("BackWall_N", ledgeRock, new[] { P(ledgeX1, ledgeZ1 + 0.5f), P(ledgeX1, 270f) }, endH + backWallH, backWallThick, 0.3f);
// the fin at the cleft mouth (4.6): x 1 to 3 from the exit north to z 270, and the rock over the slot's last metres (z 266.8 to 270)
// to the shoulder, top 78, so no flame top shows from inside the slot; the way runs south between the fin and the back wall and
// turns round the fin's end, just past the exit
FlatWall("Fin", ledgeRock, new[] { P(3f, slotExit.y + 0.2f), P(3f, 270f) }, finTop, 2f, 0.4f);
FlatWall("FinCap", ledgeRock, new[] { P(3f, slotEnd.y + slotHalf + 0.05f), P(ledgeX1 + 0.3f, slotEnd.y + slotHalf + 0.05f) }, finTop, 3.3f, 0.4f);
// the climb's own stops (4.4, 8). Measured in 8.14 (Play mode): the CharacterController jump-climbed terrain faces of any steepness;
// 8.14a gave PlayerController a slide off ground over the slope limit, so faces now hold and the ring only has to hold the edges
// (a 79 degree face 14 m, the 88 degree slot wall 4 m), so no terrain face stops anyone. Instead, the ground a player can walk
// to from the climb (cells on a 0.5 m grid reached from the J to Ward points with no step over walkStep between neighbours,
// valley cells left out) is ringed with visible rock at its edge, the rising ground behind the rock: where the ground beyond
// rises, a wall wallH over the walkable ground beside it; where it
// drops, a rim rimH high (over the 0.6 m jump and the capsule riding over an edge). Every bench, platform, the chute, the cwm,
// the cleft and the ledge are closed this way; the lip, fin and end walls above are the named pieces of the same ring.
var ring = new UnityEngine.GameObject("ClimbRing").transform; ring.SetParent(rockRoot.transform, false);
const float ringCell = 0.5f, ringX0 = -14f, ringX1 = 92f, ringZ0 = 190f, ringZ1 = 335f, walkStep = 0.45f, riseMin = 0.3f, wallH = 3.5f, rimH = ringRimHeight, wallThick = 0.8f;
int RW = UnityEngine.Mathf.RoundToInt((ringX1 - ringX0) / ringCell), RH = UnityEngine.Mathf.RoundToInt((ringZ1 - ringZ0) / ringCell);
var rh = new float[RW, RH]; var valleyCell = new bool[RW, RH]; var flooded = new bool[RW, RH];
float CX(int i) => ringX0 + (i + 0.5f) * ringCell; float CZ(int j) => ringZ0 + (j + 0.5f) * ringCell;
for (int i = 0; i < RW; i++) for (int j = 0; j < RH; j++) { rh[i, j] = H(CX(i), CZ(j)); valleyCell[i, j] = InValley(CX(i), CZ(j)); }
// a cell is ground to walk on only if its own slope (the gentler side on each axis) is under the slope limit: without this the
// fill crept along the contour lines of a steep face, where neighbours stand level (8.14)
const float walkSlope = 1.05f;   // tan 45 degrees, with room for the flights (0.83) and a bench cell beside a face
var gentle = new bool[RW, RH];
for (int i = 1; i < RW - 1; i++) for (int j = 1; j < RH - 1; j++)
{
    // the gentler side on each axis, so a bench cell beside a face still counts while a cell on the face does not
    float gx = UnityEngine.Mathf.Min(UnityEngine.Mathf.Abs(rh[i + 1, j] - rh[i, j]), UnityEngine.Mathf.Abs(rh[i, j] - rh[i - 1, j])) / ringCell;
    float gz = UnityEngine.Mathf.Min(UnityEngine.Mathf.Abs(rh[i, j + 1] - rh[i, j]), UnityEngine.Mathf.Abs(rh[i, j] - rh[i, j - 1])) / ringCell;
    gentle[i, j] = gx * gx + gz * gz <= walkSlope * walkSlope;
}
var queue = new System.Collections.Generic.Queue<(int, int)>();
for (int k = 1; k < climb.Length - 1; k++)   // the climb from the chute mouth on, sampled every half cell
{
    var a = climb[k].p; var b = climb[k + 1].p; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector2.Distance(a, b) / (ringCell * 0.5f)));
    for (int s = 0; s <= n; s++)
    {
        var q = UnityEngine.Vector2.Lerp(a, b, s / (float)n); int i = UnityEngine.Mathf.FloorToInt((q.x - ringX0) / ringCell), j = UnityEngine.Mathf.FloorToInt((q.y - ringZ0) / ringCell);
        if (i < 0 || j < 0 || i >= RW || j >= RH || valleyCell[i, j] || flooded[i, j]) continue;
        flooded[i, j] = true; queue.Enqueue((i, j));
    }
}
var n4 = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
while (queue.Count > 0)
{
    var (i, j) = queue.Dequeue();
    foreach (var (di, dj) in n4)
    {
        int a = i + di, b = j + dj; if (a < 0 || b < 0 || a >= RW || b >= RH || flooded[a, b] || valleyCell[a, b]) continue;
        if (!gentle[a, b] || UnityEngine.Mathf.Abs(rh[a, b] - rh[i, j]) > walkStep) continue;
        flooded[a, b] = true; queue.Enqueue((a, b));
    }
}
// a rim stands rimH over the highest walkable ground next to it (the fill takes in the first cell down a drop, so its own height
// can sit half a metre low, and a rim that low was jumped: P4's east edge, 8.14)
float LocalMax(int i, int j) { float m = rh[i, j]; for (int a = UnityEngine.Mathf.Max(0, i - 2); a <= UnityEngine.Mathf.Min(RW - 1, i + 2); a++) for (int b = UnityEngine.Mathf.Max(0, j - 2); b <= UnityEngine.Mathf.Min(RH - 1, j + 2); b++) if (flooded[a, b]) m = UnityEngine.Mathf.Max(m, rh[a, b]); return m; }
// uphill: rock walls whose tops break in blocks (wallBlock metres, wallBreak metres of height either way, one value per block), so
// they read as rock, not masonry; drops: a rim collider rimH over the highest walkable ground next to it, inside a broken line of
// owned boulders (Campsite CS_Rock, tilted, tops 1.15 to 1.3 m over the walkable ground, under the 1.6 m eye), so the valley side stays low and the views stay open (8.14a)
const float wallBlock = 3f, wallBreak = 0.8f, boulderStep = 1.2f, boulderLow = 1.35f, boulderHigh = 1.5f, boulderSink = 0.2f, boulderTilt = 14f, rimColThick = 0.8f;
float BlockNoise(float x, float z) => (UnityEngine.Mathf.PerlinNoise(UnityEngine.Mathf.Floor(x / wallBlock) * 0.37f + 3.1f, UnityEngine.Mathf.Floor(z / wallBlock) * 0.37f + 7.9f) * 2f - 1f) * wallBreak;
var ringBoxes = new System.Collections.Generic.List<UnityEngine.Matrix4x4>(); var rimBoxes = new System.Collections.Generic.List<UnityEngine.Matrix4x4>();
var rimSpots = new System.Collections.Generic.List<UnityEngine.Vector3>(); int wallEdges = 0, rimEdges = 0, floodCells = 0;
for (int i = 0; i < RW; i++) for (int j = 0; j < RH; j++)
{
    if (!flooded[i, j]) continue; floodCells++;
    foreach (var (di, dj) in n4)
    {
        int a = i + di, b = j + dj; if (a < 0 || b < 0 || a >= RW || b >= RH || flooded[a, b] || valleyCell[a, b]) continue;
        bool rise = rh[a, b] > rh[i, j] + riseMin;
        float ex = CX(i) + di * ringCell * 0.5f, ez = CZ(j) + dj * ringCell * 0.5f;
        // 8.14a: walls stand wallH over the walkable ground beside them (with the slide rule nobody climbs a face, so the 12 m reach that
        // made towers at the chute mouth is gone)
        float top = rise ? LocalMax(i, j) + wallH + BlockNoise(ex, ez) : LocalMax(i, j) + rimH, thick = rise ? wallThick : rimColThick;
        float bot = UnityEngine.Mathf.Min(rh[i, j], rh[a, b]) - 1f;
        // the rock stands on the far side of the shared edge: its face on the edge, its body over the neighbour cell
        float cxw = ex + di * thick * 0.5f, czw = ez + dj * thick * 0.5f;
        var size = di != 0 ? V(thick, top - bot, ringCell + 0.02f) : V(ringCell + 0.02f, top - bot, thick);
        (rise ? ringBoxes : rimBoxes).Add(UnityEngine.Matrix4x4.TRS(V(cxw, (top + bot) * 0.5f, czw), UnityEngine.Quaternion.identity, size));
        if (rise) wallEdges++; else { rimEdges++; rimSpots.Add(V(cxw, LocalMax(i, j), czw)); }
    }
}
UnityEngine.Mesh CombineBoxes(string name, System.Collections.Generic.List<UnityEngine.Matrix4x4> list)
{
    var cubeTmp = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); var cubeMesh = cubeTmp.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
    var ci = new UnityEngine.CombineInstance[list.Count]; for (int k = 0; k < ci.Length; k++) ci[k] = new UnityEngine.CombineInstance { mesh = cubeMesh, transform = list[k] };
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 }; mesh.CombineMeshes(ci, true, true);
    var vsR = mesh.vertices; var uvR = new UnityEngine.Vector2[vsR.Length]; var nrR = mesh.normals;   // world-space rock UVs, as RockWall
    for (int k = 0; k < vsR.Length; k++) { var v = vsR[k]; uvR[k] = nrR[k].y > 0.5f ? new UnityEngine.Vector2(v.x / rockTile, v.z / rockTile) : new UnityEngine.Vector2((v.x + v.z) / rockTile, v.y / rockTile); }
    mesh.uv = uvR; mesh.RecalculateBounds(); UnityEngine.Object.DestroyImmediate(cubeTmp);
    UnityEditor.AssetDatabase.CreateAsset(mesh, rockDir + "/" + name + ".asset"); return mesh;
}
{
    var wallMesh = CombineBoxes("ClimbRing", ringBoxes);
    var go = new UnityEngine.GameObject("ClimbRing_Rock"); go.transform.SetParent(ring, false);
    go.AddComponent<UnityEngine.MeshFilter>().sharedMesh = wallMesh; go.AddComponent<UnityEngine.MeshRenderer>().sharedMaterial = rockMat; go.AddComponent<UnityEngine.MeshCollider>().sharedMesh = wallMesh;
    var rimMesh = CombineBoxes("ClimbRim", rimBoxes);
    var rc = new UnityEngine.GameObject("ClimbRim_Collider"); rc.transform.SetParent(ring, false); rc.AddComponent<UnityEngine.MeshCollider>().sharedMesh = rimMesh;
    rockMeshes += 2;
}
// the boulders over the rim collider: one every boulderStep along it (the rim edges sorted by a coarse grid, the next spot too close skipped)
var boulders = new UnityEngine.GameObject("RimBoulders").transform; boulders.SetParent(ring, false); int boulderN = 0;
{
    const string rocksPath = "Assets/Revolving Pizza Games/Campsite/Prefabs/Rocks and Stones/CS_Rock_";
    var placed = new System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<UnityEngine.Vector3>>();
    long Key(float x, float z) => ((long)UnityEngine.Mathf.FloorToInt(x / boulderStep) << 32) ^ (uint)UnityEngine.Mathf.FloorToInt(z / boulderStep);
    foreach (var s in rimSpots)
    {
        bool near = false;
        for (int dx = -1; dx <= 1 && !near; dx++) for (int dz = -1; dz <= 1 && !near; dz++)
            if (placed.TryGetValue(Key(s.x + dx * boulderStep, s.z + dz * boulderStep), out var lst)) foreach (var q in lst) if ((new UnityEngine.Vector2(q.x - s.x, q.z - s.z)).magnitude < boulderStep) { near = true; break; }
        if (near) continue;
        var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(rocksPath + (1 + boulderRng.Next(8)) + ".prefab"); if (pf == null) continue;
        var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pf, boulders);
        foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
        g.transform.rotation = UnityEngine.Quaternion.Euler(RR(-boulderTilt, boulderTilt), RR(0f, 360f), RR(-boulderTilt, boulderTilt));
        float lo = float.MaxValue, hi = float.MinValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) { lo = UnityEngine.Mathf.Min(lo, r.bounds.min.y); hi = UnityEngine.Mathf.Max(hi, r.bounds.max.y); }
        float sc = RR(boulderLow, boulderHigh) / UnityEngine.Mathf.Max(0.2f, hi - lo); g.transform.localScale = V(sc, sc, sc);
        g.transform.position = V(s.x, 0f, s.z); lo = float.MaxValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) lo = UnityEngine.Mathf.Min(lo, r.bounds.min.y);
        g.transform.position = V(s.x, s.y - boulderSink - lo, s.z);   // tops about rimH over the walkable ground, under eye height, so views stay open
        long k = Key(s.x, s.z); if (!placed.TryGetValue(k, out var l2)) placed[k] = l2 = new System.Collections.Generic.List<UnityEngine.Vector3>(); l2.Add(s); boulderN++;
    }
}

// ---------- fence along x 396: owned chain-link panels (Modular Chain Link Fence, 2.1 m, with their own colliders), from the S band
// to the N band, gap for the gate lane z 167.5 to 172.5. See-through, so the road and the T read from the lot (8.14a; the rev 7 gray
// panels are in git history) ----------
const string fencePanelPath = "Assets/Modular Chain Link Fence/Prefabs/Fence_Frame_F.prefab";
var fencePanel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(fencePanelPath); if (fencePanel == null) return "missing " + fencePanelPath;
float panelLen; { var probe = (UnityEngine.GameObject)UnityEngine.Object.Instantiate(fencePanel); var b = new UnityEngine.Bounds(probe.transform.position, UnityEngine.Vector3.zero); foreach (var r in probe.GetComponentsInChildren<UnityEngine.Renderer>()) b.Encapsulate(r.bounds); panelLen = b.size.x; UnityEngine.Object.DestroyImmediate(probe); }
var fence = new UnityEngine.GameObject("Fence");
void Run(float z0, float z1)
{
    int n = UnityEngine.Mathf.CeilToInt((z1 - z0) / panelLen); float step = (z1 - z0) / n;
    for (int i = 0; i < n; i++)
    {
        float z = z0 + (i + 0.5f) * step; var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(fencePanel, fence.transform);
        g.name = "Panel"; g.transform.SetPositionAndRotation(V(fenceX, UnityEngine.Mathf.Min(H(fenceX, z - step * 0.5f), H(fenceX, z + step * 0.5f)), z), UnityEngine.Quaternion.Euler(0f, 90f, 0f));
        g.transform.localScale = V(step / panelLen, 1f, 1f);
    }
}
Run(-8f, 167.5f); Run(172.5f, 305f);

// ---------- lighting: day sun on (Main3.md: day is the sunset from waking), moon off ----------
foreach (var r in scene.GetRootGameObjects()) if (r.name == "NightLighting")
{
    var moon = r.transform.Find("Moon"); var sun = r.transform.Find("Sun");
    if (moon != null) moon.gameObject.SetActive(false);
    if (sun != null) { sun.gameObject.SetActive(true); sun.rotation = UnityEngine.Quaternion.Euler(9f, 250f, 0f); UnityEngine.RenderSettings.sun = sun.GetComponent<UnityEngine.Light>(); }
}

// ---------- spawn: inside the cabin position (178, 168), facing the tower and fire ----------
UnityEngine.GameObject player = null;
foreach (var r in scene.GetRootGameObjects()) if (r.name == "Player") player = r;
if (player == null) return "no Player in the new scene";
player.transform.position = V(178f, H(178f, 168f) + 0.1f, 168f);
player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 250f, 0f);

// ---------- dev warps: every place in Main3.md plus the junctions, the climb and the lot (Valley.md 14) ----------
var warps = new UnityEngine.GameObject("DevWarps");
void Warp(string name, float x, float z, float lx, float lz)
{
    var w = new UnityEngine.GameObject(name); w.transform.SetParent(warps.transform, false);
    w.transform.position = V(x, H(x, z) + 0.2f, z);
    w.transform.rotation = UnityEngine.Quaternion.LookRotation(V(lx - x, 0f, lz - z).normalized, UnityEngine.Vector3.up);
}
Warp("Keepers_Camp", 172f, 150f, 164f, 166f);
Warp("Lake_Pump", 190f, 99f, 190f, 60f);
Warp("Lake_Boathouse", 232f, 64f, 240f, 52f);
Warp("Camp_1", 268f, 226f, 282f, 238f);
Warp("Camp_2", 268f, 110f, 292f, 108f);
Warp("Camp_3", 74f, 142f, 78f, 146f);
Warp("Camp_3_Rim", 100f, 148f, 78f, 146f);
Warp("Office", 340f, 196f, 350f, 200f);
Warp("Store", 366f, 193f, 366f, 200f);
Warp("Gate_Booth", 388f, 176f, 396f, 170f);
Warp("Closed_Campground", 385f, 232f, 372f, 262f);
Warp("Trailhead_T", 337f, 170f, 358f, 170f);
Warp("Lot_Highway", 360f, 172f, roadX, 172f);                // the lot, facing the gate and the highway (Valley.md 14)
Warp("Junction_Jg", 262f, 168f, 340f, 170f);
Warp("Junction_J", 106f, 203f, pMouth.x, pMouth.y);           // facing the chute mouth
Warp("Junction_W1", 130f, 72f, 190f, 60f);
Warp("Cave_Mouth", 58f, 44f, 52f, 34f);
Warp("Ward_P3", pP3.x, pP3.y, cwmC.x + 8f, cwmC.y - 8f);     // the burned cwm, looking back down it
Warp("Ward_P4", pP4.x + 1.5f, pP4.y, 164f, 166f);              // the look-back: tower cab, cabin, highway
Warp("Ward", pathEnd.x, pathEnd.y, -40f, pathEnd.y);          // the path end on the ledge, facing west (Valley.md 4)
Warp("Old_Burn", 230f, 166f, 340f, 178f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
string F(float v) => v.ToString("F1");
float CrestLow(float z0, float z1, float xa, float xb, float skipZ0, float skipZ1)   // lowest of the highest ground across x xa to xb, z z0 to z1
{
    float low = float.MaxValue;
    for (float z = z0; z <= z1; z += 1f) { if (z > skipZ0 && z < skipZ1) continue; float hi = float.MinValue; for (float x = xa; x <= xb; x += 0.5f) hi = UnityEngine.Mathf.Max(hi, H(x, z)); low = UnityEngine.Mathf.Min(low, hi); }
    return low;
}
var sb = new System.Text.StringBuilder("saved=" + saved + " warps=" + warps.transform.childCount + " fence pieces=" + fence.transform.childCount);
sb.Append(" | terrain " + originX + ".." + (originX + sizeX) + " x " + originZ + ".." + (originZ + sizeZ) + ", " + res + " heights");
sb.Append(" | ground: camp=" + F(H(170, 160)) + " tower=" + F(H(164, 166)) + " camp1=" + F(H(282, 238)) + " camp2=" + F(H(292, 108))
    + " hollow=" + F(H(78, 146)) + " J=" + F(H(104, 206)) + " bridge=" + F(H(104.8f, 203.2f)) + " lakeC=" + F(H(190, 60)) + " pump=" + F(H(190, 96)) + " W1=" + F(H(128, 70))
    + " mouthFloor=" + F(H(52, 38)) + " overPassage=" + F(H(52, 28)) + " overChamber=" + F(H(80, 12)) + " office=" + F(H(350, 200)));
sb.Append(" | W crest lowest over x 5 to 20, z 40 to 345 (cleft left out) " + F(CrestLow(40f, 345f, 5f, 20f, 261f, 268f)) + "; z -10 " + F(H(10, -10)) + " z -40 " + F(H(14, -40)) + " knob " + F(H(14, 222)) + " shoulder " + F(H(13, 270)));
sb.Append(" | N arm x120 " + F(ArmN(120f, PL(120f, nCX, nCZ))) + " x170 " + F(H(170, PL(170f, nCX, nCZ))) + " x250 " + F(H(250, 338)) + " x320 " + F(H(320, 334)) + " x380 " + F(H(380, 332)) + " | S arm x110 " + F(H(110, -44)) + " x170 " + F(H(170, -42)) + " x240 " + F(H(240, -38)) + " x320 " + F(H(320, -34)));
sb.Append(" | climb: J " + F(H(pJ.x, pJ.y)) + " mouth " + F(H(pMouth.x + 0.5f, pMouth.y)) + " P1 " + F(H(pP1.x, pP1.y)) + " P2 " + F(H(pP2.x, pP2.y)) + " cwm bend " + F(H(pL3.x, pL3.y)) + " P3 " + F(H(pP3.x, pP3.y)) + " leg 4 bend " + F(H(pL4.x, pL4.y)) + " P4 " + F(H(pP4.x, pP4.y))
    + " dogleg " + F(H(slotDog1.x, slotDog1.y)) + " slot end " + F(H(slotEnd.x, slotEnd.y)) + " exit " + F(H(slotExit.x, slotExit.y)) + " path end " + F(H(pathEnd.x, pathEnd.y)) + " ledge " + F(H(-5f, 230f)) + " (leg 1 ramps " + (100f * rampRise / rampRun).ToString("F1") + " percent, leg 3 " + (100f * (p3H - p2H) / lenL3).ToString("F1") + ", leg 4 " + (100f * (p4H - p3H) / (lenL4a + lenL4b)).ToString("F1") + ")");
sb.Append(" | rock: " + rockMeshes + " meshes, walls " + rockLength.ToString("F0") + " m; climb ring: " + floodCells + " walkable cells, " + wallEdges + " wall and " + rimEdges + " rim edges, " + boulderN + " rim boulders, " + screeN + " rubble at the band feet");
sb.Append(" | road (428, 170) " + F(H(roadX, 170)) + " ditch " + F(H(roadX + roadHalf + ditchW * 0.5f, 170)) + " east hills (590,150) " + F(H(590, 150)) + " | west: x-40 z150 " + F(H(-39.8f, 150)) + " edges N " + F(H(200, 499.5f)) + " S " + F(H(200, -199.5f)) + " E " + F(H(594.5f, 150)));
sb.Append(" | build list: "); foreach (var s in UnityEditor.EditorBuildSettings.scenes) sb.Append(System.IO.Path.GetFileNameWithoutExtension(s.path) + " ");
return sb.ToString();
