// Main3 task 8.1: scene, terrain, fence, bounds, spawn, dev warps.
// Source: Docs/Design/Main3.md rev 16 table 2.1 for the valley floor; Docs/Design/Valley.md revision 5 (8.9j) for the ring of
// ridges, the Ward knob, the climb, the cleft and the ledge; resolutions in Docs/Design/Main3_BuildNotes.md.
// Run from Graybox (or any saved scene) in edit mode. Refuses if Main3.unity exists: delete it and its terrain folder to rebuild,
// then rerun every later Main3 recipe in task order (Tools/Recipes/main3_rebuild.sh).
// 8.9j: the terrain is placed at (-40, -200) and runs to (595, 500), so the W ridge's west face, the N, S and E ridges and their
// back slopes are real terrain with a collider; no world coordinate in any recipe changed. Later recipes that index the
// heightmap, alphamap or holes add the terrain origin.
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
        else if (dOut < 15f) h = L(-4.5f, h, (dOut - 1.5f) / 13.5f);   // linear bank, under 30 degrees
        if (dOut > 0f && z > 80f) h = UnityEngine.Mathf.Min(h, -4.5f + 0.7f * UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Abs(x - 190f) - 2.5f) + 0.5f * UnityEngine.Mathf.Max(0f, z - 96f));   // dock root notch at -4.5 to z 96, soft sides
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

// ---------- the ring of ridges (Valley.md rev 5, section 2) ----------
// Outer ground (2.7): gently rolling 0 to 10 outside the ridges. Floor() is copied in main3_8_9e_layers.cs (the outer ground
// mesh beyond the terrain); change both together. West of x 0 the ground steps down at 45 degrees to the -40 floor at x -40,
// the same slope in the terrain and the mesh, so no ray can pass under it.
float Floor(float x, float z) => UnityEngine.Mathf.Clamp(5f + 4.5f * (UnityEngine.Mathf.PerlinNoise(x / 280f + 11.3f, z / 280f + 4.7f) * 2f - 1f), 0f, 10f);
float Outer(float x, float z) => x >= 0f ? Floor(x, z) : L(-40f, Floor(0f, z), (x + 40f) / 40f);
const float backLen = 150f, westFloor = -40f, westEdge = -40f, wFade = 100f;
// W ridge: crest line and heights along it, the east foot; the knob and the NW corner are raised after
float[] wT = { -60f, 60f, 150f, 200f, 230f, 242f, 280f, 300f, 360f }; float[] wX = { 0f, 0f, 4f, 8f, 10f, 12f, 12f, 8f, 4f };
float[] wHT = { -60f, 0f, 60f, 150f, 200f, 235f, 242f, 280f, 300f, 360f }; float[] wH = { 95f, 100f, 95f, 108f, 106f, 106f, 113f, 113f, 105f, 103f };
float[] fT = { -60f, 85f, 100f, 180f, 195f, 360f }; float[] fX = { 38f, 38f, 46f, 46f, 80f, 80f };
float CrestX(float z) => PL(z, wT, wX);
float CrestW(float z)   // fades to the outer ground past both ends (z -60, z 360) so the hooks fold down onto the west step
{
    float out0 = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(-60f - z, z - 360f));
    return L(PL(z, wHT, wH), Floor(CrestX(z), z), SS(out0 / wFade));
}
float FootW(float z) => PL(z, fT, fX);
// N (crest z 350), S (crest z -50), E (crest x 445, road cut V at z 170)
float[] nT = { 0f, 20f, 60f, 90f, 120f, 170f, 250f, 320f, 400f, 440f }; float[] nH = { 103f, 103f, 88f, 84f, 94f, 72f, 50f, 70f, 62f, 58f };
float[] sT = { 0f, 60f, 110f, 170f, 240f, 320f, 400f, 440f }; float[] sH = { 95f, 90f, 75f, 50f, 70f, 58f, 52f, 50f };
float[] eT = { -50f, -40f, 60f, 120f, 250f, 340f, 350f }; float[] eH = { 45f, 45f, 57f, 47f, 60f, 55f, 55f };
const float crestN = 350f, crestS = -50f, crestE = 445f, footN = 300f, footS = 0f, footE = 400f, roadZ = 170f, roadHalf = 4f, roadLevel = 3f, cutSlope = 1f;
// every ridge fades to the outer ground within backLen past its ends, so the terrain meets the outer ground mesh at its edges
float EndFade(float over) => SS(UnityEngine.Mathf.Max(0f, over) / backLen);
float CrestN(float x) => L(PL(x, nT, nH), Floor(x, crestN), EndFade(x - crestE));
float CrestS(float x) => L(PL(x, sT, sH), Floor(x, crestS), EndFade(x - crestE));
float CrestE(float z) => L(UnityEngine.Mathf.Min(PL(z, eT, eH), roadLevel + UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Abs(z - roadZ) - roadHalf) * cutSlope), Floor(crestE, z), EndFade(UnityEngine.Mathf.Max(z - crestN, crestS - z)));
// a small roll on the faces, zero at crest and foot so both stay exact
float Rough(float x, float z, float t) => (UnityEngine.Mathf.PerlinNoise(x / 23f + 5.5f, z / 23f + 1.9f) - 0.5f) * 6f * t * (1f - t);
// one ridge side toward the valley: from the foot ground (t 0) up to the crest (t 1), steepest near the crest
float Face(float footG, float crest, float t, float x, float z) => footG + (crest - footG) * UnityEngine.Mathf.Pow(Clamp01(t), 1.6f) + Rough(x, z, Clamp01(t));
// one back slope: from the crest down to the outer ground within backLen
float Back(float crest, float outer, float d) => d >= backLen ? outer : outer + (crest - outer) * (1f - d / backLen) * (1f - d / backLen);

// ---------- the climb, the cleft, the ledge (Valley.md 5) ----------
// Four legs on the W face, each 85 m rising 20 m, benches 4 m wide; platforms at the turns come out of the leg heights (the
// two legs meeting at a turn have the same height there, so the ground between them is level).
float[] legX = { 72f, 59f, 46f, 33f }; const float benchHalf = 2f, legZ0 = 205f, legZ1 = 290f, zoneZ0 = 202.5f, zoneZ1 = 292.5f, zoneBlend = 4f;
float LegH(int i, float z)
{
    float t = (UnityEngine.Mathf.Clamp(z, legZ0, legZ1) - legZ0) / (legZ1 - legZ0);
    switch (i) { case 0: return 12f + 20f * t; case 1: return 52f - 20f * t; case 2: return 52f + 20f * t; default: return 92f - 20f * t; }
}
float ClimbFace(float x, float z, float footG)   // across the face from the crest down through the four benches to the foot
{
    var xs = new[] { CrestX(z), legX[3] - benchHalf, legX[3] + benchHalf, legX[2] - benchHalf, legX[2] + benchHalf, legX[1] - benchHalf, legX[1] + benchHalf, legX[0] - benchHalf, legX[0] + benchHalf, 80f };
    var hs = new[] { CrestW(z), LegH(3, z), LegH(3, z), LegH(2, z), LegH(2, z), LegH(1, z), LegH(1, z), LegH(0, z), LegH(0, z), footG };
    return PL(x, xs, hs);
}
// carved paths: J to leg 1, leg 5, the ramp from the cleft's west mouth to the path end on the ledge
var approach = new[] { (P(104f, 206f), 10f), (P(80f, 205.5f), 11.5f), (P(72f, 205f), 12f) };
// leg 5 leaves P4 west first (to (27, 206)), then runs north-west to the east mouth: straight from P4 it ran 27 degrees off leg 4 and
// stayed within 2 m of it for 5 m while the two parted in height, so the ground between them was a step (8.9j climb check)
var leg5 = new[] { (P(33f, 204f), 92f), (P(27f, 206f), 92.6f), (P(20f, 230f), 95f) };
var ramp = new[] { (P(4.25f, 238f), 95f), (P(-2f, 258f), 98f) };
float PathH((UnityEngine.Vector2, float)[] pts, UnityEngine.Vector2 p, out float dist)
{
    dist = float.MaxValue; float hBest = 0f;
    for (int i = 0; i < pts.Length - 1; i++) { float d = SegDist(p, pts[i].Item1, pts[i + 1].Item1, out float t); if (d < dist) { dist = d; hBest = L(pts[i].Item2, pts[i + 1].Item2, t); } }
    return hBest;
}
const float knobX0 = 8f, knobX1 = 18f, knobZ0 = 242f, knobZ1 = 280f, knobTop = 113f, knobSummit = 115f; var knobPeak = P(12f, 258f);
const float cleftFloor = 95f, cleftWall = 106f, ledgeH = 98f;
const float cellPad = 0.7f;   // raised areas reach one heightmap cell past their stated edges, so the ground at the edge itself holds the height
const float ledgeX0 = -9f, ledgeX1 = 4f, ledgeZ0 = 232f, ledgeZ1 = 275f;   // Valley 5.6 (x -8 to 4, z 232 to 272) widened 1 m west and 3 m north so the stones stand on it (Marlow R3.8)

float Height(float x, float z)
{
    var p = P(x, z);
    float uW = x - CrestX(z), uN = crestN - z, uS = z - crestS, uE = crestE - x;
    bool inside = uW > 0f && uN > 0f && uS > 0f && uE > 0f;
    float h = inside ? Valley(x, z) : Outer(x, z);
    // faces toward the valley (each meets the valley ground at its foot) and back slopes away from it
    if (uN >= 0f && uN < crestN - footN && uW > 0f) h = UnityEngine.Mathf.Max(h, Face(Valley(x, footN), CrestN(x), 1f - uN / (crestN - footN), x, z));
    else if (uN < 0f) h = UnityEngine.Mathf.Max(h, Back(CrestN(x), Outer(x, z), -uN));
    if (uS >= 0f && uS < footS - crestS && uW > 0f) h = UnityEngine.Mathf.Max(h, Face(Valley(x, footS), CrestS(x), 1f - uS / (footS - crestS), x, z));
    else if (uS < 0f) h = UnityEngine.Mathf.Max(h, Back(CrestS(x), Outer(x, z), -uS));
    if (uE >= 0f && uE < crestE - footE) h = UnityEngine.Mathf.Max(h, Face(Valley(footE, z), CrestE(z), 1f - uE / (crestE - footE), x, z));
    else if (uE < 0f) h = UnityEngine.Mathf.Max(h, Back(CrestE(z), Outer(x, z), -uE));
    float xc = CrestX(z), cw = CrestW(z);
    if (uW >= 0f)
    {
        float xf = FootW(z);
        if (x < xf)
        {
            float footG = Valley(xf, z);
            float face = Face(footG, cw, 1f - (x - xc) / (xf - xc), x, z);
            // the climb zone replaces the plain face with the benches, blending back at the zone's ends
            float zw = SS((z - (zoneZ0 - zoneBlend)) / zoneBlend) * SS(((zoneZ1 + zoneBlend) - z) / zoneBlend);
            if (zw > 0f) face = L(face, ClimbFace(x, z, footG), zw);
            h = UnityEngine.Mathf.Max(h, face);
        }
    }
    else
    {
        // west of the crest everything is capped by the west face: the crest down to -40 at x -40 (a cliff), so the hooks of the
        // N and S ridges fold down along it and no ridge stands over the -40 floor
        float out0 = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(-60f - z, z - 360f));
        float westFace = L(cw + (x - xc) * (cw - westFloor) / (xc - westEdge), Outer(x, z), SS(out0 / wFade));   // past the W ridge ends: the west step
        h = westFace;
    }
    // NW corner (2.6): 103 or more over x 0 to 20, z 340 to 360
    if (x >= 0f - cellPad && x <= 20f + cellPad && z >= 340f - cellPad && z <= 360f + cellPad) h = UnityEngine.Mathf.Max(h, 103f);
    // the ledge on the west face at 98, and the rock between it and the knob
    if (x >= ledgeX0 && x <= ledgeX1 && z >= ledgeZ0 && z <= ledgeZ1) h = ledgeH;
    if (x > ledgeX1 && x < knobX0 && z >= 240f && z <= knobZ1) h = UnityEngine.Mathf.Max(h, L(ledgeH, knobTop, (x - ledgeX1) / (knobX0 - ledgeX1)));
    // the Ward knob: flat-topped, square ends, 113 or more across all of it, summit 115
    if (x >= knobX0 - cellPad && x <= knobX1 + cellPad && z >= knobZ0 - cellPad && z <= knobZ1 + cellPad) h = UnityEngine.Mathf.Max(h, knobTop + (knobSummit - knobTop) * Clamp01(1f - UnityEngine.Vector2.Distance(p, knobPeak) / 15f));
    // the cleft's rock (walls 106 or higher), the fin west of part B, then the slot cut to 95
    if (x >= 3f && x <= 22f && z >= 225f && z <= 240f) h = UnityEngine.Mathf.Max(h, cleftWall);
    if (x >= 1f && x < 3f && z >= 225f && z <= 237.2f) h = UnityEngine.Mathf.Max(h, cleftWall);   // south wall and the fin (x 1 to 3, to z 237.2)
    if (x >= 3f && x <= 22f && z >= 228.75f && z <= 231.25f) h = cleftFloor;                           // part A along z 230
    if (x >= 3f && x <= 5.5f && z >= 228.75f && z <= 238.5f) h = cleftFloor;                            // part B north to the west mouth
    // carved paths: flat 2 m each side at the path height, blended 1.5 m (a trench where the ground is higher)
    { float ph = PathH(approach, p, out float d); if (d < 3.5f) h = L(ph, h, SS((d - 2f) / 1.5f)); }
    // leg 5 is flattened on the upper face; over bench 4 (x from 31) it only cuts down: near P4 it runs beside leg 4, which falls
    // north as leg 5 rises, so raising the bench to leg 5's height made a step across leg 4 (8.9j climb check)
    { float ph = PathH(leg5, p, out float d); if (d < 3.5f) { float c5 = L(ph, h, SS((d - 2f) / 1.5f)); h = x < legX[3] - benchHalf ? c5 : UnityEngine.Mathf.Min(h, c5); } }
    { float ph = PathH(ramp, p, out float d); if (d < 1.5f) h = UnityEngine.Mathf.Min(h, ph); }   // the ramp is sunk in the ledge (it rises 95 to 98)
    return h;
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
for (int zi = 0; zi < res; zi++)
    for (int xi = 0; xi < res; xi++)
        hm[zi, xi] = Clamp01((Height(originX + xi * sizeX / (res - 1), originZ + zi * sizeZ / (res - 1)) - baseY) / sizeY);
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

// ---------- bounds: invisible walls on the north and south map edges (the east edge is the fence; 8.6 builds the gate).
// The west is the W ridge: its face and the thicket keep walkers off it; the climb is the only way up (8.9j). ----------
var bounds = new UnityEngine.GameObject("Bounds");
void Wall(string name, UnityEngine.Vector3 c, UnityEngine.Vector3 s)
{
    var w = new UnityEngine.GameObject(name); w.transform.SetParent(bounds.transform, false);
    w.transform.position = c; w.AddComponent<UnityEngine.BoxCollider>().size = s;
}
Wall("Wall_North", V(200f, 25f, 300f), V(400f, 150f, 1f));
Wall("Wall_South", V(200f, 25f, 0f), V(400f, 150f, 1f));

// ---------- fence along x 396, gray, 2.1 m, gap for the gate lane z 167.5 to 172.5 ----------
var fence = new UnityEngine.GameObject("Fence");
void Cube(string name, UnityEngine.Transform parent, UnityEngine.Vector3 c, UnityEngine.Vector3 s)
{
    var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); g.name = name;
    g.transform.SetParent(parent, false); g.transform.position = c; g.transform.localScale = s;
}
void Run(float z0, float z1)
{
    int n = UnityEngine.Mathf.CeilToInt((z1 - z0) / 2.5f); float step = (z1 - z0) / n;
    for (int i = 0; i <= n; i++) { float z = z0 + i * step; Cube("Post", fence.transform, V(396f, H(396f, z) + 1.05f, z), V(0.12f, 2.1f, 0.12f)); }
    for (int i = 0; i < n; i++) { float z = z0 + (i + 0.5f) * step; Cube("Panel", fence.transform, V(396f, H(396f, z) + 1.05f, z), V(0.04f, 2.0f, step)); }
}
Run(0f, 167.5f); Run(172.5f, 300f);

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

// ---------- dev warps: every place in Main3.md plus the junctions, and the climb (8.9j) ----------
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
Warp("Junction_Jg", 262f, 168f, 340f, 170f);
Warp("Junction_J", 106f, 203f, 85f, 225f);
Warp("Junction_W1", 130f, 72f, 190f, 60f);
Warp("Cave_Mouth", 58f, 44f, 52f, 34f);
Warp("Ward_P3", 39f, 291f, 164f, 166f);    // the first platform above the tower deck, looking back east (Valley 5.3)
Warp("Ward_P4", 33f, 204f, 164f, 166f);    // the highest platform, looking back east
Warp("Ward", -2f, 258f, -40f, 258f);       // the path end on the ledge, facing west (Valley 5.6)
Warp("Old_Burn", 230f, 166f, 340f, 178f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
string F(float v) => v.ToString("F1");
var sb = new System.Text.StringBuilder("saved=" + saved + " warps=" + warps.transform.childCount + " fence pieces=" + fence.transform.childCount);
sb.Append(" | terrain " + originX + ".." + (originX + sizeX) + " x " + originZ + ".." + (originZ + sizeZ) + ", " + res + " heights");
sb.Append(" | ground: camp=" + F(H(170, 160)) + " tower=" + F(H(164, 166)) + " camp1=" + F(H(282, 238)) + " camp2=" + F(H(292, 108))
    + " hollow=" + F(H(78, 146)) + " J=" + F(H(104, 206)) + " bridge=" + F(H(104.8f, 203.2f)) + " lakeC=" + F(H(190, 60)) + " pump=" + F(H(190, 96)) + " W1=" + F(H(128, 70))
    + " mouthFloor=" + F(H(52, 38)) + " overPassage=" + F(H(52, 28)) + " overChamber=" + F(H(80, 12)) + " office=" + F(H(350, 200)));
sb.Append(" | W crest: z-60 " + F(H(0, -60)) + " z0 " + F(H(0, 0)) + " saddle(0,60) " + F(H(0, 60)) + " z150 " + F(H(4, 150)) + " z200 " + F(H(8, 200)) + " z300 " + F(H(8, 300)) + " z360 " + F(H(4, 360)));
sb.Append(" | knob: (8,242) " + F(H(8.2f, 242.2f)) + " (18,242) " + F(H(17.8f, 242.2f)) + " (18,280) " + F(H(17.8f, 279.8f)) + " (8,280) " + F(H(8.2f, 279.8f)) + " summit " + F(H(12, 258)) + " east edge (18,258) " + F(H(17.8f, 258)));
sb.Append(" | NW corner (6,350) " + F(H(6, 350)) + " (0,340) " + F(H(0.2f, 340.2f)) + " (20,360) " + F(H(19.8f, 359.8f)));
sb.Append(" | N saddle (250,350) " + F(H(250, 350)) + " S saddle (170,-50) " + F(H(170, -50)) + " E crest z-40 " + F(H(445, -40)) + " road cut (445,170) " + F(H(445, 170)) + " E z250 " + F(H(445, 250)));
sb.Append(" | climb: J " + F(H(104, 206)) + " leg1 start " + F(H(72, 205)) + " P1 " + F(H(65, 291)) + " P2 " + F(H(52, 204)) + " P3 " + F(H(39, 291)) + " P4 " + F(H(33, 204)) + " cleft east " + F(H(20, 230)) + " part A " + F(H(10, 230)) + " part B " + F(H(4.25f, 234)) + " west mouth " + F(H(4.25f, 238)) + " fin " + F(H(2, 233)) + " cleft wall " + F(H(12, 233)) + " path end " + F(H(-2, 258)) + " ledge " + F(H(-6, 250)));
sb.Append(" | west: x-40 z150 " + F(H(-39.8f, 150)) + " x-40 z480 " + F(H(-39.8f, 480)) + " edges N " + F(H(200, 499.5f)) + " S " + F(H(200, -199.5f)) + " E " + F(H(594.5f, 150)));
sb.Append(" | build list: "); foreach (var s in UnityEditor.EditorBuildSettings.scenes) sb.Append(System.IO.Path.GetFileNameWithoutExtension(s.path) + " ");
return sb.ToString();
