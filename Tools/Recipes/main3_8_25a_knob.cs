// Main3 task 8.25a, Camp 2 rethink: his knob, to Camp2Layout.md draft 4 (Sable 2026-10-03; Marlow's 825a ground survey and paper check).
// Layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (the new pieces rebuilt under Campsites/Camp_2/Layout825a;
// absolute places). In the runner after main3_8_25_camp2.cs, whose column (GraniteStack, StackPath, StackTop) this replaces.
// REMOVALS (doc 3): GraniteStack; StackPath (its ramps, landings, rails and posts); StackTop and everything under it (his spot and the
//   letters under stones are kept, moved under Layout825a first); from Layout825 the Skirts, LandingRailN, WedgeFill, WedgeFillCorner,
//   BarrelStand and PaperSpots; Dressing's 8.17 talus (every BigBoulders_); in Forest/Grove_BurnEdge the plants inside the knob and Sequoia1;
//   on the deck lines to the lamp, Forest/Dense/Canopy/RedPine3 (261.6, 134.8) and Grove_BurnEdge/RedPine2 (268.7, 127.8) (Wren 2026-10-03).
// K1  the knob: footprint x kx0 to kx1, z kz0 to kz1, ground about 4.0, top kTop (9.0). A solid core of invisible collider boxes (cellStep m
//     columns merged along each row) to kTop, except the gully on leg 2, where each column stops under the ramp (its fill drawn). The core's
//     west face is at kx0, flush to leg 1's east edge. Drawn without colliders on the core's surfaces: the top, the west face, the gully's
//     walls and the other faces behind the skin. The skin (8.25a gate, Vesper 1): BigBoulders_0 to 5 at scale 1.3 to 1.6 on every face, tops
//     rising over kTop at the edges where the top's places, the gully and the chair's view allow, hulled; small Boulders high on the west
//     face over leg 1. One rock (Places_KnobRock, #6E6862) for the knob, steps, fill and rockfall.
// K2  the scramble (the STAIRS RULE): invisible ramp boxes (rampT thick) under owned Boulder_0 to 5 steps (scale stepScale) whose tops sit
//     stepUnder m under the ramp. Leg 1: the foot (288.0, 114.5) at 4.0 north along x 287.2 to 288.8 to the landing, 1.6 x 1.6 at (288.0,
//     121.5), 7.0. Leg 2: from the landing's east edge on the bearing-107 line (41 degrees, Wren 2026-10-03) to the head (291.0, 120.6) at 9.0, 1.6 wide. A rock fill under leg 1 and
//     the landing to the ground. The head's 1.0 m circle stays clear.
// K3  the top: his chair CS_Chair_2 at (292.0, 122.0) facing 65, a box from its mesh, `Talk`; his spot on it; the talk stand (291.3, 123.2)
//     facing 150 (the chair's bearing, Wren 2026-10-03); the tent CS_Tent_Modern_2, door north, its mesh box x 294.5 to 296.75, z 117.3 to 119.33, cut back doorCut short of the door
//     with a layer-2 doorway box over the cut; the ring box on a crate just inside the door, `Examine`, its reach an undrawn box ringMinDeg
//     wide from the door stand; the lamp on a 1.2 m pole at (297.0,
//     121.2) with its core (its glow LookTuning.camp2LampCoreIntensity, 8.25a gate: under clipped white up close, 40 grey or more from the
//     deck at night) and practical light (as 8.25); a mug and thermos by the chair; the letters by the tent's south side.
// K4  rockfall: BoulderField's Boulder_2 (287.80, 122.34) to (300.0, 126.5) and Boulder_2 (285.9, 115.6) to (300.5, 123.5), each touching
//     the east face (its west extent at kx1).
// K5  paper spots PaperSpot_K1 (289.4, 117.4) on the top, K2 (287.0, 119.0) on a step block off leg 1's west edge, K3 on the moved boulder
//     (300.0, 126.5); each a marker with a sheet.
// K6  the Camp_2_Top warp (293.0, 9.2, 123.6) facing 65.
// K7  the top's weathered cover, the gully head's stones and the cairn at the foot (below).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var notes = new System.Collections.Generic.List<string>();
var c2 = kit.Root("Campsites") != null ? kit.Root("Campsites").transform.Find("Camp_2") : null; var cd = c2 != null ? c2.Find("Dressing") : null;
if (c2 == null || cd == null || kit.Root("Forest") == null) return "run 8.5, 8.16, 8.17 and 8.25 first (Campsites/Camp_2/Dressing, Forest)";
float G(float x, float z) => kit.H(x, z);
int uses = 0; void Use(UnityEngine.GameObject g, string prompt, UnityEngine.Renderer target) { if (g == null) return; var u = g.GetComponent<ToggleColorInteractable>() ?? g.AddComponent<ToggleColorInteractable>(); var so = new UnityEditor.SerializedObject(u); so.FindProperty("prompt").stringValue = prompt; so.FindProperty("target").objectReferenceValue = target != null ? target : g.GetComponentInChildren<UnityEngine.Renderer>(); so.ApplyModifiedPropertiesWithoutUndo(); uses++; }
const string rocks = "Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Materials/Rocks.mat";
// one rock for the knob, its skin, the steps, the fill and the rockfall (8.25a gate, Vesper 2 and Wren 1): the BK boulder material at its pack
// value, retinted toward #6E6862
var knobMat = kit.Tinted("Places_KnobRock", rocks, Hex("#6E6862"), UnityEngine.Vector2.one);
void Rock(UnityEngine.GameObject g) { if (g == null) return; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>(true)) { var ms = r.sharedMaterials; for (int i = 0; i < ms.Length; i++) ms[i] = knobMat; r.sharedMaterials = ms; } }
// the letters (moved from StackTop on the first run) are kept across a rerun: out of Layout825a before it is rebuilt, back in after
var keptLetters = c2.Find("Layout825a/Letters"); if (keptLetters != null) keptLetters.SetParent(c2, true);
var L = kit.Fresh("Layout825a", c2, c2.position, 0f); if (keptLetters != null) keptLetters.SetParent(L, true);

// ================= REMOVALS =================
const float removeTol = 0.3f; int removed = 0; var missing = new System.Collections.Generic.List<string>();
UnityEngine.Transform Near(UnityEngine.Transform parent, string name, float x, float z, float tol) { UnityEngine.Transform best = null; float bd = tol; if (parent != null) foreach (UnityEngine.Transform t in parent) { if (t.name != name) continue; float d = UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(x, z)); if (d <= bd) { bd = d; best = t; } } return best; }
{
    var top = c2.Find("StackTop");
    if (top != null)
    {
        var spot = top.Find("Resident_Camp2_Spot"); if (spot != null) spot.SetParent(c2, true);
        var letters = keptLetters != null ? keptLetters : kit.Group("Letters", L, L.position, 0f); var td = top.Find("Dressing"); if (td != null) foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(td))) if (t.name.StartsWith("Paper") || t.name.StartsWith("CS_Stone_")) t.SetParent(letters, true);
    }
    foreach (var n in new[] { "GraniteStack", "StackPath", "StackTop" }) { var t = c2.Find(n); if (t != null) { PlaceKit.Remove(t); removed++; } }
    var l825 = c2.Find("Layout825"); foreach (var n in new[] { "Skirts", "LandingRailN", "WedgeFill", "WedgeFillCorner", "BarrelStand", "PaperSpots" }) { var t = l825 != null ? l825.Find(n) : null; if (t != null) { PlaceKit.Remove(t); removed++; } }
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(cd))) if (t.name.StartsWith("BigBoulders_")) { PlaceKit.Remove(t); removed++; }
    var grove = kit.Root("Forest").transform.Find("Grove_BurnEdge");
    foreach (var (n, x, z) in new[] { ("Sequoia1", 293.42f, 129.09f), ("Bush3", 289.65f, 122.91f), ("Bush3", 292.80f, 123.55f), ("ThinFern5", 290.44f, 121.55f), ("ThinFern3", 292.57f, 125.26f), ("ThinFern1", 288.65f, 126.55f), ("Bush1", 289.62f, 127.21f) })
    { var t = Near(grove, n, x, z, removeTol); if (t != null) { PlaceKit.Remove(t); removed++; } }
    // the two pines whose crowns stood on every deck line to the lamp's core (Wren 2026-10-03; their feet are KeepOuts.Camp2 zones)
    foreach (var (grp, n, x, z) in new[] { ("Dense/Canopy", "RedPine3", 261.6f, 134.8f), ("Grove_BurnEdge", "RedPine2", 268.7f, 127.8f) })
    { var t = Near(kit.Root("Forest").transform.Find(grp), n, x, z, removeTol); if (t != null) { PlaceKit.Remove(t); removed++; } }
}
UnityEngine.Physics.SyncTransforms();

// ================= K1, K2: the knob and the scramble =================
const float kx0 = 288.8f, kx1 = 299f, kz0 = 116f, kz1 = 127f, kTop = 9f, cellStep = 0.25f, rampW = 1.6f, rampT = 0.2f;
const float stepScale = 0.35f, stepUnder = 0.02f, stepPitch = 0.9f;
var foot = V(288.0f, 4.0f, 114.5f); var land = V(288.0f, 7.0f, 121.5f); var head = V(291.0f, kTop, 120.6f); float landHalf = 0.8f;
var landS = V(land.x, land.y, land.z - landHalf);   // leg 1 ends at the landing's south edge
// leg 2 starts where its line leaves the landing (its east edge), at 7.0, 41 degrees to the head (Wren 2026-10-03: from the landing's
// centre its south edge rose 0.2 to 0.3 m over leg 1's end and the walk stopped there); the gully's corridor still runs from the landing's
// centre, flat at the landing's height back of leg 2's start
var leg2Dir = (P(head.x, head.z) - P(land.x, land.z)).normalized; var leg2a = V(land.x + landHalf, land.y, land.z + leg2Dir.y * landHalf / leg2Dir.x);
// the ramp surface's height at a point on a leg (t along it from a to b)
float Lerp(float a, float b, float t) => a + (b - a) * UnityEngine.Mathf.Clamp01(t);
float Proj(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; return UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude); }
float Off(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(Proj(q, a, b)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
var A2 = P(land.x, land.z); var S2 = P(leg2a.x, leg2a.z); var H2 = P(head.x, head.z);
// a cell is gully when any of it lies in the corridor (its centre within the half width plus half its diagonal): a cell whose centre fell
// just outside stood its full height 0.18 m into the ramp's width and stopped a walk on the east lane and a sprint on leg 2's north edge
// (8.25a gate, Marlow 1)
// gullyHalf: the ramp's half width, half a cell's diagonal and wallClear more (8.25a gate rerun: a sprint on the east lane, the body 0.85 m off the
// line, caught a wall cell's corner 0.815 m off it)
const float wallClear = 0.1f; float gullyHalf = rampW * 0.5f + cellStep * 0.7072f + wallClear;
bool InGully(UnityEngine.Vector2 q) { float t = Proj(q, A2, H2); return t >= 0f && t <= 1f + cellStep / UnityEngine.Vector2.Distance(A2, H2) && Off(q, A2, H2) <= gullyHalf; }
// the gully floor's fill top: the landing's height back of leg 2's start, rampT under the ramp after it
float GullyTop(UnityEngine.Vector2 q) => Proj(q, S2, H2) < 0f ? land.y : Lerp(leg2a.y, head.y, Proj(q, S2, H2)) - rampT;
// the north bank (8.25a gate, Wren 2026-10-03: a player turns for the talk stand before the gully's end, and its north wall stood 0.15 to
// 0.3 m over the ramp there): from bankT0 of leg 2 to the head, the ground north of the ramp's edge rises from the ramp's height to the
// top's over bankW m, one drawn mesh collider; the core cells under it drop below it (as gully cells)
// bankT0: a body's width before the earliest turn tested (0.5 of leg 2); 37 degrees at its steepest
const float bankT0 = 0.25f, bankW = 2.0f, bankUnder = 0.03f, bankPast = 0.15f, bankBeyond = 0.4f; const int bankNT = 8, bankNS = 7;
var bankN = P(-(H2 - S2).normalized.y, (H2 - S2).normalized.x);   // square to leg 2, its north side
float RampY(float tS) => Lerp(leg2a.y, head.y, tS);
float BankS(UnityEngine.Vector2 q) { float tS = UnityEngine.Mathf.Clamp01(Proj(q, S2, H2)); return UnityEngine.Vector2.Dot(q - (S2 + (H2 - S2) * tS), bankN) - rampW * 0.5f; }
float BankH(float tS, float s) => UnityEngine.Mathf.Lerp(RampY(tS), kTop, UnityEngine.Mathf.Clamp01(s / bankW));
bool InBank(UnityEngine.Vector2 q) { float tS = Proj(q, S2, H2), s = BankS(q); return tS >= bankT0 && tS <= 1f && s > -cellStep && s < bankW + cellStep * 0.7072f; }
string skinNote = ""; int coreBoxes = 0, skinN = 0, stepN = 0, faces = 0; var skinDropped = new System.Collections.Generic.List<string>();   // by design: boulders that would stand in the scramble
const float faceT = 0.02f;
{
    var core = kit.Group("Core", L, L.position, 0f); var drawn = kit.Group("Faces", L, L.position, 0f);
    // whole cells by count (the footprint, 10.2 x 11, is no whole number of cells: a float walk to kx1 stepped past it and never closed
    // the row's run, so rows with no gully cell were left out); the last cell in each row and column is cut to the footprint
    int nx = UnityEngine.Mathf.CeilToInt((kx1 - kx0) / cellStep - 1e-3f), nz = UnityEngine.Mathf.CeilToInt((kz1 - kz0) / cellStep - 1e-3f);
    float CX0(int ix) => kx0 + ix * cellStep; float CX1(int ix) => UnityEngine.Mathf.Min(kx1, kx0 + (ix + 1) * cellStep);
    float CZ0(int iz) => kz0 + iz * cellStep; float CZ1(int iz) => UnityEngine.Mathf.Min(kz1, kz0 + (iz + 1) * cellStep);
    var gul = new bool[nx, nz]; var floorY = new float[nx, nz];
    for (int iz = 0; iz < nz; iz++) for (int ix = 0; ix < nx; ix++) { var cq = P((CX0(ix) + CX1(ix)) * 0.5f, (CZ0(iz) + CZ1(iz)) * 0.5f); gul[ix, iz] = InGully(cq) || InBank(cq); }
    // the core: invisible collider runs (8.25a gate, Vesper 1: its flat faces read as a black box); the gully's fill is drawn (its floor)
    for (int iz = 0; iz < nz; iz++)
    {
        float z0c = CZ0(iz), z1c = CZ1(iz), zc = (z0c + z1c) * 0.5f, dz = z1c - z0c, runX0 = float.NaN;
        for (int ix = 0; ix <= nx; ix++)
        {
            bool edge = ix == nx; float x = edge ? kx1 : CX0(ix), x1c = edge ? kx1 : CX1(ix), xc = (x + x1c) * 0.5f; bool gully = !edge && gul[ix, iz];
            if (!float.IsNaN(runX0) && (gully || edge)) { float gy = G((runX0 + x) * 0.5f, zc); var cb = Slab("Core", core, V((runX0 + x) * 0.5f, (gy - 0.3f + kTop) * 0.5f, zc), V(x - runX0, kTop - gy + 0.3f, dz), knobMat, true); cb.GetComponent<UnityEngine.Renderer>().enabled = false; coreBoxes++; runX0 = float.NaN; }
            if (edge) break;
            if (gully) { float ry = GullyTop(P(xc, zc)); if (!InGully(P(xc, zc)) || (Proj(P(xc, zc), S2, H2) >= bankT0 && BankS(P(xc, zc)) > 0f)) {   /* the bank's cells, and the gully's cells north of the ramp's edge alongside it (at the bank's height, under its mesh) */ ry = float.MaxValue; foreach (var cx in new[] { x, x1c }) foreach (var cz in new[] { z0c, z1c }) ry = UnityEngine.Mathf.Min(ry, BankH(UnityEngine.Mathf.Clamp01(Proj(P(cx, cz), S2, H2)), BankS(P(cx, cz)))); ry -= bankUnder; } else if (Proj(P(xc, zc), S2, H2) > 1f) { float tMin = float.MaxValue; foreach (var cx in new[] { x, x1c }) foreach (var cz in new[] { z0c, z1c }) tMin = UnityEngine.Mathf.Min(tMin, Proj(P(cx, cz), S2, H2)); ry = Lerp(leg2a.y, head.y, tMin); } floorY[ix, iz] = ry; float gy = G(xc, zc); if (ry - gy > 0.05f) { Slab("GullyFill", core, V(xc, (gy - 0.3f + ry) * 0.5f, zc), V(x1c - x, ry - gy + 0.3f, dz), knobMat, true).GetComponent<UnityEngine.Renderer>().enabled = false; coreBoxes++; } continue; }
            if (float.IsNaN(runX0)) runX0 = x;
        }
    }
    // the drawn rock where no boulder stands, no collider, faceT thick on the core's own surfaces: the top (every row's runs of core cells),
    // the west face beside leg 1 and the landing (rows whose first cell is core), the gully's walls (each gully cell's edge on a core cell),
    // and the south, north and east faces behind the skin (seen only between boulders)
    for (int iz = 0; iz < nz; iz++)
    {
        float zc = (CZ0(iz) + CZ1(iz)) * 0.5f, dz = CZ1(iz) - CZ0(iz), runX0 = float.NaN;
        for (int ix = 0; ix <= nx; ix++)
        {
            bool edge = ix == nx, gully = !edge && gul[ix, iz]; float x = edge ? kx1 : CX0(ix);
            if (!float.IsNaN(runX0) && (gully || edge)) { Slab("TopFace", drawn, V((runX0 + x) * 0.5f, kTop - faceT * 0.5f, zc), V(x - runX0, faceT, dz), knobMat); faces++; runX0 = float.NaN; }
            if (edge) break; if (!gully && float.IsNaN(runX0)) runX0 = x;
        }
        if (!gul[0, iz]) { float gy = G(kx0, zc); Slab("WestFace", drawn, V(kx0 + faceT * 0.5f, (gy - 0.3f + kTop) * 0.5f, zc), V(faceT, kTop - gy + 0.3f, dz), knobMat); faces++; }
        for (int ix = 0; ix < nx; ix++)
        {
            if (!gul[ix, iz]) continue; float fy = floorY[ix, iz] - 0.1f;
            if (ix + 1 < nx && !gul[ix + 1, iz]) { Slab("GullyWall", drawn, V(CX1(ix) - faceT * 0.5f, (fy + kTop) * 0.5f, zc), V(faceT, kTop - fy, dz), knobMat); faces++; }
            if (ix > 0 && !gul[ix - 1, iz]) { Slab("GullyWall", drawn, V(CX0(ix) + faceT * 0.5f, (fy + kTop) * 0.5f, zc), V(faceT, kTop - fy, dz), knobMat); faces++; }
            if (iz + 1 < nz && !gul[ix, iz + 1]) { Slab("GullyWall", drawn, V((CX0(ix) + CX1(ix)) * 0.5f, (fy + kTop) * 0.5f, CZ1(iz) - faceT * 0.5f), V(CX1(ix) - CX0(ix), kTop - fy, faceT), knobMat); faces++; }
            if (iz > 0 && !gul[ix, iz - 1]) { Slab("GullyWall", drawn, V((CX0(ix) + CX1(ix)) * 0.5f, (fy + kTop) * 0.5f, CZ0(iz) + faceT * 0.5f), V(CX1(ix) - CX0(ix), kTop - fy, faceT), knobMat); faces++; }
        }
    }
    foreach (var (n, a, b) in new[] { ("SouthFace", P(kx0, kz0 + faceT * 0.5f), P(kx1, kz0 + faceT * 0.5f)), ("NorthFace", P(kx0, kz1 - faceT * 0.5f), P(kx1, kz1 - faceT * 0.5f)), ("EastFace", P(kx1 - faceT * 0.5f, kz0), P(kx1 - faceT * 0.5f, kz1)) })
    { float gy = UnityEngine.Mathf.Min(G(a.x, a.y), G(b.x, b.y)); var c = (a + b) * 0.5f; bool alongX = UnityEngine.Mathf.Abs(b.x - a.x) > 0.1f; Slab(n, drawn, V(c.x, (gy - 0.3f + kTop) * 0.5f, c.y), alongX ? V(b.x - a.x, kTop - gy + 0.3f, faceT) : V(faceT, kTop - gy + 0.3f, b.y - a.y), knobMat); faces++; }
    // the flat top: thin slab colliders over the top's own footprint (doc K1), cut round the gully (one slab over it all roofed the gully:
    // the body stopped under it 2.4 m short of the head): the cut spans the widened corridor in z (its half width over the line's x share),
    // and the east slab starts past the corridor's far end (its last cell past the head and its far corner)
    const float tx0 = 289f, tx1 = 297.5f, tz0 = 117f, tz1 = 125.5f, slabT = 0.1f;
    var dirH = P(head.x - land.x, head.z - land.z).normalized; float wGully = gullyHalf, halfZ = wGully / UnityEngine.Mathf.Abs(dirH.x);
    float gz0 = UnityEngine.Mathf.Min(land.z, head.z) - halfZ, gz1 = UnityEngine.Mathf.Max(land.z, head.z) + halfZ, gxE = head.x + wGully * UnityEngine.Mathf.Abs(dirH.y) + cellStep * UnityEngine.Mathf.Abs(dirH.x);
    void TopSlab(string n, float x0, float x1, float z0, float z1) => kit.Blocker(n, L, L.InverseTransformPoint(V((x0 + x1) * 0.5f, kTop - slabT * 0.5f, (z0 + z1) * 0.5f)), V(x1 - x0, slabT, z1 - z0));
    // the bank's surface: a drawn mesh collider, bankNT by bankNS quads from the ramp's north edge (at the ramp's height) to bankW m out (at the
    // top's); the north top slab starts past it
    float bankZMax = gz1;
    {
        var verts = new UnityEngine.Vector3[(bankNT + 1) * (bankNS + 1)]; var tris = new System.Collections.Generic.List<int>(); var bankRoot = L.position;
        for (int i = 0; i <= bankNT; i++) for (int j = 0; j <= bankNS; j++)
        {
            // the mesh runs past the head by bankPast and past the bank's top edge by bankBeyond, both at the top's height (the cells under its edges)
            float tS = bankT0 + (1f + bankPast - bankT0) * i / bankNT, s = (bankW + bankBeyond) * j / bankNS; var lp = S2 + (H2 - S2) * tS + bankN * (rampW * 0.5f + s);
            verts[i * (bankNS + 1) + j] = V(lp.x, BankH(tS, s), lp.y) - bankRoot; bankZMax = UnityEngine.Mathf.Max(bankZMax, lp.y);
        }
        for (int i = 0; i < bankNT; i++) for (int j = 0; j < bankNS; j++) { int a = i * (bankNS + 1) + j, b = a + bankNS + 1; tris.AddRange(new[] { a, a + 1, b, b, a + 1, b + 1 }); }
        var mesh = new UnityEngine.Mesh { name = "KnobBank" }; mesh.vertices = verts; mesh.triangles = tris.ToArray(); mesh.RecalculateNormals(); mesh.RecalculateBounds();
        if (mesh.normals[0].y < 0f) { var tr = mesh.triangles; for (int k2 = 0; k2 < tr.Length; k2 += 3) { int sw = tr[k2 + 1]; tr[k2 + 1] = tr[k2 + 2]; tr[k2 + 2] = sw; } mesh.triangles = tr; mesh.RecalculateNormals(); }
        var bank = new UnityEngine.GameObject("Bank"); bank.transform.SetParent(L, false); bank.transform.position = bankRoot;
        bank.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; bank.AddComponent<UnityEngine.MeshRenderer>().sharedMaterial = knobMat; bank.AddComponent<UnityEngine.MeshCollider>().sharedMesh = mesh; faces++;
        bankZMax += 0.1f;
    }
    TopSlab("TopSlab", tx0, tx1, tz0, gz0); TopSlab("TopSlab_N", tx0, tx1, bankZMax, tz1); TopSlab("TopSlab_E", gxE, tx1, gz0, gz1);
    // the ramps: invisible boxes, rampT thick, their tops on the walking line
    void Ramp(string n, UnityEngine.Vector3 a, UnityEngine.Vector3 b) { var d = b - a; var mid = (a + b) * 0.5f; var rot = UnityEngine.Quaternion.LookRotation(d.normalized); var g = kit.Blocker(n, L, UnityEngine.Vector3.zero, V(rampW, rampT, d.magnitude)); g.transform.rotation = rot; g.transform.position = mid - (rot * UnityEngine.Vector3.up) * (rampT * 0.5f); }
    Ramp("Leg1Ramp", foot, landS); Ramp("Leg2Ramp", leg2a, head);
    // the scramble's drawn body (8.25a gate, Vesper: the fill's 0.25 m columns drew as stair treads): its fill and the gully's fill are
    // invisible colliders, and one rock body per leg is drawn instead, no collider: a box pitched along the leg, bodyDrop m under the walking
    // line (so the boulder steps show through it), its sides vertical, bodyDeep m deep (under the ground at the low end); the landing a box
    const float bodyDrop = 0.12f, bodyDeep = 6f, bodyDeepGully = 1f;   // the gully's body shallow: a deep one pitched back out under the landing
    void Body(string n, UnityEngine.Vector3 a, UnityEngine.Vector3 b, float w, float deep)
    {
        var d = b - a; var rot = UnityEngine.Quaternion.LookRotation(d.normalized); var g = kit.Slab(n, L, UnityEngine.Vector3.zero, V(w, deep, d.magnitude), knobMat);
        g.transform.rotation = rot; g.transform.position = (a + b) * 0.5f - rot * V(0f, bodyDrop + deep * 0.5f, 0f); faces++;
    }
    Body("Leg1Body", foot, landS, rampW, bodyDeep); Body("Leg2Body", leg2a, head, 2f * gullyHalf, bodyDeepGully);
    Slab("LandingBody", L, V(land.x, (G(land.x, land.z) - 0.3f + land.y - bodyDrop) * 0.5f, land.z), V(rampW, land.y - bodyDrop - G(land.x, land.z) + 0.3f, landHalf * 2f), knobMat);
    kit.Blocker("Landing", L, L.InverseTransformPoint(V(land.x, land.y - rampT * 0.5f, land.z)), V(rampW, rampT, landHalf * 2f));
    // the fill under leg 1 and the landing: rock columns to rampT under the walking line
    var fill = kit.Group("ScrambleFill", L, L.position, 0f);
    for (float z = foot.z; z < land.z + landHalf - 1e-3f; z += cellStep)
    {
        float zc = z + cellStep * 0.5f; float wy = zc >= landS.z ? land.y : Lerp(foot.y, landS.y, (zc - foot.z) / (landS.z - foot.z)); float top = wy - rampT, gy = G(foot.x, zc);
        if (top - gy > 0.05f) { Slab("Fill", fill, V(foot.x, (gy - 0.3f + top) * 0.5f, zc), V(rampW, top - gy + 0.3f, cellStep), knobMat, true).GetComponent<UnityEngine.Renderer>().enabled = false; coreBoxes++; }
    }
    // the steps: boulders along both legs, every LOD0 vertex stepUnder m or more under the walking line over it (a step set by its top alone
    // stood over the ramp on its downhill side, and 8.18a's solid pass then gave it a hull the body stopped on)
    var steps = kit.Group("Steps", L, L.position, 0f); int k = 0;
    void Step(UnityEngine.Vector3 at, UnityEngine.Vector3 a, UnityEngine.Vector3 b)
    {
        var g = kit.Spawn(PlaceKit.BK + "Rocks/Boulder_" + (k % 6), steps); if (g == null) return; PlaceKit.StripColliders(g); Rock(g); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, k * 67f, 0f); g.transform.localScale *= stepScale;
        var bx = Exact(g); g.transform.position += V(at.x - bx.center.x, 0f, at.z - bx.center.z);
        var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); var rs = lodg != null && lodg.GetLODs().Length > 0 ? lodg.GetLODs()[0].renderers : g.GetComponentsInChildren<UnityEngine.Renderer>(); float dy = float.MaxValue;
        foreach (var r in rs) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) continue; foreach (var v in mf.sharedMesh.vertices) { var w = r.transform.TransformPoint(v); float sy = Lerp(a.y, b.y, Proj(P(w.x, w.z), P(a.x, a.z), P(b.x, b.z))); dy = UnityEngine.Mathf.Min(dy, sy - stepUnder - w.y); } }
        if (dy < float.MaxValue) g.transform.position += V(0f, dy, 0f); k++; stepN++;
    }
    for (float s = stepPitch * 0.5f; s < UnityEngine.Vector3.Distance(foot, landS); s += stepPitch) Step(UnityEngine.Vector3.Lerp(foot, landS, s / UnityEngine.Vector3.Distance(foot, landS)), foot, landS);
    Step(land, land, land + V(0f, 0f, landHalf));
    for (float s = stepPitch * 0.5f; s < UnityEngine.Vector3.Distance(leg2a, head) - 0.3f; s += stepPitch) Step(UnityEngine.Vector3.Lerp(leg2a, head, s / UnityEngine.Vector3.Distance(leg2a, head)), leg2a, head);
    // the skin (8.25a gate, Vesper 1 and Wren 1): BK BigBoulders_0 to 5 at scale skinScaleLo up to skinScaleHi on every face, so boulders make
    // the knob's outline: each placed by its vertices, its outer extent skinOut m past its face (skinOutE on the east: the T tread), its top
    // at kTop plus the first of skinRises whose vertices over kTop + riseFree all stand clear of the top's places (keeps), within lipBand m
    // of the flat top's edge, off the gully, and riseUnder m or more under the seated eye's lines to the three S1 targets (lateral fanLat m);
    // hulled. A boulder in the scramble's walking space is dropped. The west face beside leg 1 gets Boulder_0 to 5 instead, from highOver m
    // over the ramp up to kTop + highRise, touching the face (Vesper: leg 1's clear width untouched).
    var skin = kit.Group("Skin", L, L.position, 0f); int si = 0;
    const float clearUp = 2.2f, clearOver = 0.05f, skinScaleLo = 1.3f, skinScaleStep = 0.1f, skinOut = 3.0f, skinOutE = 2.3f, skinOutFar = 4.0f, riseFree = 0.02f, lipBand = 1.2f, riseUnder = 0.3f, fanLat = 1.5f, gullyKeep = 1.3f;
    const float highOver = 2.8f, highRise = 0.3f, highMinH = 0.8f, highW = 1.4f, highOverGround = 2.5f, highWS = 2.0f, sHighX = 289.6f; const int skinSizes = 4;   // highW: its widest, so it reads set in the face, not hung over the path (F12)
    float[] skinRises = { 1.0f, 0.7f, 0.4f, 0.2f, -0.02f };
    var seat = V(292.0f, 10.2f, 122.0f); var s1 = new[] { V(430f, 4f, 185f), V(423.05f, 5.3f, 166f), V(389.3f, 4.5f, 169.5f) };
    // the top's places a rising boulder keeps clear of: circles (x, z, r) and the tent with its door and the ring box's stand
    var keepC = new (float x, float z, float r)[] { (292.0f, 122.0f, 1.2f), (291.3f, 123.2f, 0.8f), (297.0f, 121.2f, 0.7f), (291.0f, 120.6f, 1.3f), (293.0f, 123.6f, 0.8f), (289.4f, 117.4f, 0.6f), (295.6f, 120.3f, 0.8f) };
    const float keepTx0 = 294.3f, keepTx1 = 296.95f, keepTz0 = 117.1f, keepTz1 = 119.6f;   // the tent box plus 0.2; its door is the ring stand's circle
    System.Collections.Generic.IEnumerable<UnityEngine.Vector3> Verts(UnityEngine.GameObject g) { var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); var rs = lodg != null && lodg.GetLODs().Length > 0 ? lodg.GetLODs()[0].renderers : g.GetComponentsInChildren<UnityEngine.Renderer>(); foreach (var r in rs) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) continue; foreach (var v in mf.sharedMesh.vertices) yield return r.transform.TransformPoint(v); } }
    string RiseBlock(UnityEngine.GameObject g)
    {
        foreach (var w in Verts(g))
        {
            if (w.y <= kTop + riseFree) continue; var q = P(w.x, w.z);
            foreach (var kc in keepC) if (UnityEngine.Vector2.Distance(q, P(kc.x, kc.z)) < kc.r) return "a top place at (" + F(kc.x) + ", " + F(kc.z) + ")";
            if (q.x > keepTx0 && q.x < keepTx1 && q.y > keepTz0 && q.y < keepTz1) return "the tent";
            if (Off(q, A2, H2) < gullyKeep || InBank(q)) return "the gully";
            if (q.x > tx0 + lipBand && q.x < tx1 - lipBand && q.y > tz0 + lipBand && q.y < tz1 - lipBand) return "the top inside its lip";
            foreach (var t in s1)
            {
                var d = P(t.x - seat.x, t.z - seat.z); float len = d.magnitude; d /= len; var rel = q - P(seat.x, seat.z); float along = UnityEngine.Vector2.Dot(rel, d), lat = UnityEngine.Mathf.Abs(rel.x * d.y - rel.y * d.x);
                if (along > 0f && lat < fanLat && w.y > seat.y + (t.y - seat.y) * along / len - riseUnder) return "the chair's view";
            }
        }
        return null;
    }
    UnityEngine.GameObject Boulder(string prefab, float scale) { var g = kit.Spawn(PlaceKit.BK + "Rocks/" + prefab, skin); if (g == null) return null; PlaceKit.StripColliders(g); Rock(g); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, si * 61f, 0f); g.transform.localScale *= scale; return g; }
    // a boulder kept only if its hull leaves the scramble's walking space clear
    bool Hull(UnityEngine.GameObject g, string label, bool report)
    {
        var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); var hulls = new System.Collections.Generic.List<UnityEngine.Collider>();
        foreach (var r in g.GetComponentsInChildren<UnityEngine.MeshRenderer>()) { if (lodg != null && lodg.GetLODs().Length > 0 && System.Array.IndexOf(lodg.GetLODs()[0].renderers, r) < 0) continue; var mf = r.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; hulls.Add(mc); }
        UnityEngine.Physics.SyncTransforms(); string why = null;
        foreach (var (n, a, e) in new[] { ("leg 1", foot, landS), ("the landing", landS, land + V(0f, 0f, landHalf)), ("leg 2", leg2a, head) })
        {
            var d = e - a; var rot = UnityEngine.Quaternion.LookRotation(d.normalized); var c = (a + e) * 0.5f + rot * V(0f, clearUp * 0.5f + clearOver, 0f);
            foreach (var h in UnityEngine.Physics.OverlapBox(c, V(rampW * 0.5f, clearUp * 0.5f, d.magnitude * 0.5f), rot, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (hulls.Contains(h)) why = n;
        }
        if (why != null) { if (report) { skinDropped.Add(label + " in " + why); PlaceKit.Remove(g.transform); } else foreach (var h in hulls) UnityEngine.Object.DestroyImmediate(h); return false; }
        return true;
    }
    // each spot's outer extent past its face: skinOut, or skinOutE beside the T tread (east, south of z 119)
    var spots = new (char face, float along, float outM)[] { ('S', 293.0f, skinOut), ('S', 296.0f, skinOut), ('S', 299.0f, skinOut), ('E', 116.0f, skinOutE), ('E', 117.5f, skinOutE), ('E', 121.0f, skinOutFar), ('E', 124.5f, skinOutFar), ('N', 290.5f, skinOut), ('N', 293.5f, skinOut), ('N', 296.8f, skinOut), ('W', 125.5f, skinOut) };
    // 8.25a gate (Wren): one BigBoulders at 1.3 on the north-east corner, flat in F5 (the scale is fixed; the spots above cycle 1.3 to 1.6)
    var allSpots = new System.Collections.Generic.List<(char face, float along, float outM, float sc)>(); foreach (var (fc, al, om) in spots) allSpots.Add((fc, al, om, -1f)); allSpots.Add(('E', 127.5f, skinOutFar, skinScaleLo));
    var rises = new System.Collections.Generic.List<string>();
    // each spot: of skinYaws turns, the one whose first passing rise is highest (a single turn left a boulder flush where another rose)
    float[] skinYaws = { 0f, 60f, 120f, 180f, 240f, 300f };
    void PlaceAt(UnityEngine.GameObject g, char face, float along, float outM, float rise)
    {
        var b = Exact(g); float dy = kTop + rise - b.max.y;
        if (face == 'E') g.transform.position += V(kx1 + outM - b.max.x, dy, along - b.center.z);
        else if (face == 'N') g.transform.position += V(along - b.center.x, dy, kz1 + outM - b.max.z);
        else if (face == 'W') g.transform.position += V(kx0 - outM - b.min.x, dy, along - b.center.z);
        else g.transform.position += V(along - b.center.x, dy, kz0 - outM - b.min.z);
    }
    int spotI = 0;
    foreach (var (face, along, outM, sc) in allSpots)
    {
        var g = Boulder("BigBoulders_" + (spotI % 6), sc > 0f ? sc : skinScaleLo + skinScaleStep * (spotI % skinSizes)); spotI++; if (g == null) continue; string label = face + " " + F(along);
        float bestRise = float.NaN, bestYaw = 0f; string firstBlock = null, lastBlock = null;
        foreach (var yaw in skinYaws)
        {
            g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
            foreach (var rise in skinRises)
            {
                if (!float.IsNaN(bestRise) && rise <= bestRise) break;
                PlaceAt(g, face, along, outM, rise); var why = RiseBlock(g);
                if (why == null) { bestRise = rise; bestYaw = yaw; break; }
                if (rise == skinRises[0] && firstBlock == null) firstBlock = why; lastBlock = why;
            }
        }
        if (float.IsNaN(bestRise)) { skinDropped.Add(label + " rises into " + lastBlock); PlaceKit.Remove(g.transform); continue; }
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, bestYaw, 0f); PlaceAt(g, face, along, outM, bestRise);
        if (Hull(g, label, true)) { skinN++; rises.Add(label + " +" + F(bestRise) + (bestRise < skinRises[0] && firstBlock != null ? " (higher met " + firstBlock + ")" : "")); }
    }
    // high boulders set on a face, touching it from outside, of Boulder_0 to 5 and skinYaws the first that keeps the top's places and the
    // scramble's walking space clear: the west face beside leg 1 from highOver m over the ramp at its z; and (8.25a gate, Wren) the flat south
    // face at the south-west corner, under F12's perched one, from highOverGround m over the ground; each up to kTop + highRise, no wider than
    // its width cap
    var highs = new System.Collections.Generic.List<(string label, char face, float along, float bottom, float wCap)>();
    foreach (var al in new[] { 116.6f, 117.6f }) highs.Add(("W high " + F(al), 'W', al, Lerp(foot.y, landS.y, (al - foot.z) / (landS.z - foot.z)) + highOver, highW));
    highs.Add(("S high " + F(sHighX), 'S', sHighX, G(sHighX, kz0) + highOverGround, highWS));
    foreach (var (hl, hf, along, bottom, wCap) in highs)
    {
        float hgt = kTop + highRise - bottom; if (hgt < highMinH) continue;
        bool done = false; string why = null;
        for (int pi = 0; pi < 6 && !done; pi++)
            foreach (var yaw in skinYaws)
            {
                var g = Boulder("Boulder_" + pi, 1f); if (g == null) break; g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
                var b = Exact(g); g.transform.localScale *= UnityEngine.Mathf.Min(hgt / UnityEngine.Mathf.Max(0.1f, b.size.y), wCap / UnityEngine.Mathf.Max(0.1f, UnityEngine.Mathf.Max(b.size.x, b.size.z)));
                b = Exact(g);
                if (hf == 'W') g.transform.position += V(kx0 - b.max.x + 0.05f, bottom - b.min.y, along - b.center.z);
                else g.transform.position += V(along - b.center.x, bottom - b.min.y, kz0 - b.max.z + 0.05f);
                why = RiseBlock(g); if (why != null) { PlaceKit.Remove(g.transform); continue; }
                if (!Hull(g, hl, false)) { why = "the scramble's walking space"; PlaceKit.Remove(g.transform); continue; }
                skinN++; rises.Add(hl + " from " + F(bottom)); done = true; break;
            }
        if (!done) skinDropped.Add(hl + " (" + why + ")");
    }
    skinNote = string.Join(", ", rises);
}
UnityEngine.GameObject Slab(string n, UnityEngine.Transform parent, UnityEngine.Vector3 world, UnityEngine.Vector3 size, UnityEngine.Material m, bool collide = false) => kit.Slab(n, parent, parent.InverseTransformPoint(world), size, m, default, collide);
// a piece's true extent: its LOD0 meshes' vertices in world space (renderer bounds grow with a turn)
UnityEngine.Bounds Exact(UnityEngine.GameObject g) { var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); var rs = lodg != null && lodg.GetLODs().Length > 0 ? lodg.GetLODs()[0].renderers : g.GetComponentsInChildren<UnityEngine.Renderer>(); bool any = false; var b = new UnityEngine.Bounds(); foreach (var r in rs) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) continue; foreach (var v in mf.sharedMesh.vertices) { var w = r.transform.TransformPoint(v); if (!any) { b = new UnityEngine.Bounds(w, UnityEngine.Vector3.zero); any = true; } else b.Encapsulate(w); } } return any ? b : PlaceKit.MeshBounds(g); }

// ================= K3: the top =================
const float ringMinDeg = 8f, ringMargin = 1.1f; var ringStand = P(295.6f, 120.3f);
const float doorCut = 0.9f, crateH = 0.65f, crateW = 0.4f, crateIn = 0.65f, ringSide = 0.07f, poleH = 1.2f, coreH = 0.45f, coreW = 0.35f;
const float tentX0 = 294.5f, tentX1 = 296.75f, tentZ0 = 117.3f, tentZ1 = 119.33f;
var topG = kit.Group("Top", L, V(293f, kTop, 121f), 0f);
{
    var chair = kit.On(PlaceKit.CS + "CS_Chair_2", topG, topG.InverseTransformPoint(V(292.0f, kTop, 122.0f)), 65f, 1f, false, null, true); if (chair != null) { chair.name = "HisChair"; PlaceKit.FitExact(chair); Use(chair, "Talk", null); }
    var spot = c2.Find("Resident_Camp2_Spot"); if (spot != null) { spot.position = V(292.0f, kTop + 0.9f, 122.0f); spot.rotation = UnityEngine.Quaternion.Euler(0f, 65f, 0f); } else notes.Add("no Resident_Camp2_Spot");
    kit.Marker("TalkStand_Top", topG, topG.InverseTransformPoint(V(291.3f, kTop, 123.2f)), 150f);   // facing 150, the chair centre's bearing (Wren 2026-10-03; the doc's 120 met nothing)
    kit.On(PlaceKit.CS + "Drinks/CS_Drink_Thermos_1", topG, topG.InverseTransformPoint(V(292.8f, kTop, 121.3f)), 0f, 1f, false, null, true);
    kit.On(PlaceKit.CS + "Tableware/CS_Tableware_Mug_Metal_1", topG, topG.InverseTransformPoint(V(293.0f, kTop, 121.6f)), 30f, 1f, false, null, true);
    // the tent at yaw 0, door north (at 8.25's yaw 90 it faced east); the door side checked from its flap and net meshes, then placed by its box
    var tent = kit.On(PlaceKit.CS + "CS_Tent_Modern_2", topG, topG.InverseTransformPoint(V((tentX0 + tentX1) * 0.5f, kTop, (tentZ0 + tentZ1) * 0.5f)), 0f, 1f, false, null, true);
    if (tent != null) { var mb = PlaceKit.MeshBounds(tent); float door = 0f; foreach (var mf in tent.GetComponentsInChildren<UnityEngine.MeshFilter>(true)) if (mf.name.EndsWith("_Flap") || mf.name.EndsWith("_Net")) door = mf.GetComponent<UnityEngine.Renderer>().bounds.center.z - mb.center.z; if (door <= 0f) notes.Add("tent door not north (flap " + F(door) + " m in z)"); }
    if (tent == null) notes.Add("no CS_Tent_Modern_2");
    else
    {
        tent.name = "Tent"; foreach (var mf in tent.GetComponentsInChildren<UnityEngine.MeshFilter>(true)) if (mf.name.EndsWith("_Flap") || mf.name.EndsWith("_Net")) mf.gameObject.SetActive(false);
        var bc = PlaceKit.FitExact(tent, "Rope"); var wb = bc.bounds; tent.transform.position += V((tentX0 + tentX1) * 0.5f - wb.center.x, kTop - wb.min.y, (tentZ0 + tentZ1) * 0.5f - wb.center.z); UnityEngine.Physics.SyncTransforms(); wb = bc.bounds;
        float doorZ = wb.max.z, cutZ = doorZ - doorCut;
        var lo = tent.transform.InverseTransformPoint(V(wb.min.x, wb.min.y, wb.min.z)); var hi = tent.transform.InverseTransformPoint(V(wb.max.x, wb.max.y, cutZ));
        bc.center = (lo + hi) * 0.5f; bc.size = V(UnityEngine.Mathf.Abs(hi.x - lo.x), UnityEngine.Mathf.Abs(hi.y - lo.y), UnityEngine.Mathf.Abs(hi.z - lo.z));
        var doorway = kit.Blocker("Doorway", topG, topG.InverseTransformPoint(V(wb.center.x, wb.center.y, (cutZ + doorZ) * 0.5f)), V(wb.size.x, wb.size.y, doorZ - cutZ)); doorway.layer = 2;
        float crateZ = doorZ - crateIn; kit.Fill(PlaceKit.CI + "Props/CITW_Crate", topG, topG.InverseTransformPoint(V(295.6f, kTop, crateZ)), V(crateW, crateH, crateW));
        var ringMat = kit.Tinted("Places_RingBox", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#5A1E22"), UnityEngine.Vector2.one);
        var ringBox = kit.Slab("RingBox", topG, topG.InverseTransformPoint(V(295.6f, kTop + crateH + ringSide * 0.5f, crateZ)), V(ringSide, ringSide, ringSide), ringMat, default, true); Use(ringBox, "Examine", ringBox.GetComponent<UnityEngine.Renderer>());
        // its reach: an undrawn box round it, ringMinDeg wide from the door stand's eye (8.25a gate: the 7 cm box was 3 by 3 degrees from the door)
        float ringD = UnityEngine.Vector3.Distance(V(ringStand.x, kTop + 1.6f, ringStand.y), ringBox.transform.position), ringReach = 2f * ringD * UnityEngine.Mathf.Tan(ringMinDeg * 0.5f * UnityEngine.Mathf.Deg2Rad) * ringMargin;
        var reach = new UnityEngine.GameObject("RingBoxReach"); reach.transform.SetParent(ringBox.transform, false); reach.AddComponent<UnityEngine.BoxCollider>().size = UnityEngine.Vector3.one * (ringReach / ringSide);
        kit.On(PlaceKit.CS + "Beds/CS_Bedroll_Modern_1", topG, topG.InverseTransformPoint(V(wb.center.x, kTop, (wb.min.z + cutZ) * 0.5f)), 0f, 1f, false, null, true);
        // the letters under stones, by the tent's south side
        var letters = L.Find("Letters"); if (letters != null) { int n = 0; foreach (UnityEngine.Transform t in letters) { int slot = n / 2; t.position = V(tentX0 + 0.3f + (slot % 3) * 0.35f, kTop + (t.name.StartsWith("CS_Stone_") ? 0.01f : 0f), wb.min.z - 0.3f); n++; } }
    }
    // the lamp on a 1.2 m pole, its cold core and practical light (as 8.25)
    var lamp = kit.Group("Lamp", topG, V(297.0f, kTop, 121.2f), 0f);
    kit.Fill(PlaceKit.CI + "Building/CITW_Wood_Pillar", lamp, V(0f, 0f, 0f), V(0.09f, poleH, 0.09f)); kit.Blocker("PoleCollider", lamp, V(0f, poleH * 0.5f, 0f), V(0.09f, poleH, 0.09f));
    var lantern = kit.On(PlaceKit.CS + "CS_Lantern_Modern", lamp, V(0f, poleH, 0f), 0f, 1.4f, false, null, true);
    float coreY = lantern != null ? lamp.InverseTransformPoint(PlaceKit.MeshBounds(lantern).max).y + coreH * 0.5f : poleH + 0.1f;
    var core = kit.Slab("LampCore", lamp, V(0f, coreY, 0f), V(coreW, coreH, coreW), kit.Glow("Places_Camp2LampCore", Hex("#DDE6F0"), kit.Look.camp2LampCoreIntensity)); core.GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    kit.Practical("LampLight", lamp, V(0f, poleH + 0.1f, 0f), 6f, PracticalLight.Kind.Lantern, PracticalLight.ByDay.Off);
}

// ================= K4: rockfall =================
{
    var bf = c2.Find("BoulderField"); int moved = 0;
    foreach (var (fx, fz, tx, tz) in new[] { (287.80f, 122.34f, 300.0f, 126.5f), (285.9f, 115.6f, 300.5f, 123.5f) })
    {
        var t = Near(bf, "Boulder_2", fx, fz, 0.5f) ?? Near(bf, "Boulder_2", tx + 0.5f, tz, 2.5f); if (t == null) { notes.Add("no BoulderField/Boulder_2 at (" + F(fx) + ", " + F(fz) + ")"); continue; }
        float lift = t.position.y - G(t.position.x, t.position.z); t.position = V(tx, G(tx, tz) + lift, tz); UnityEngine.Physics.SyncTransforms(); var b = PlaceKit.MeshBounds(t.gameObject); t.position += V(kx1 - b.min.x, 0f, 0f); Rock(t.gameObject); moved++;
    }
    if (moved < 2) notes.Add("rockfall moved " + moved + " of 2");
}

// ================= K5: paper spots =================
{
    var ps = kit.Group("PaperSpots", L, L.position, 0f);
    // PS2's step block: its top psBelow m under leg 1's walking line at its z, off the tread's west edge (x 287.2)
    const float psBelow = 0.3f, psW = 0.4f; float ps2Top = Lerp(foot.y, landS.y, (119.0f - foot.z) / (landS.z - foot.z)) - psBelow;
    Slab("PS2Block", ps, V(287.0f, (G(287.0f, 119.0f) + ps2Top) * 0.5f, 119.0f), V(psW, ps2Top - G(287.0f, 119.0f), psW), knobMat, true);
    UnityEngine.Physics.SyncTransforms();
    foreach (var (n, x, z) in new[] { ("PaperSpot_K1", 289.4f, 117.4f), ("PaperSpot_K2", 287.0f, 119.0f), ("PaperSpot_K3", 300.0f, 126.5f) })
    {
        float y = G(x, z); foreach (var h in UnityEngine.Physics.RaycastAll(V(x, kTop + 3f, z), UnityEngine.Vector3.down, 30f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!h.collider.isTrigger && h.collider.gameObject.layer != 2) y = UnityEngine.Mathf.Max(y, h.point.y);
        var m = kit.Marker(n, ps, ps.InverseTransformPoint(V(x, y, z)), 0f); kit.On(PlaceKit.CE + "Decoration_Home/Paper", m, V(0f, 0.01f, 0f), 25f, 0.5f, false, null, true);
    }
}

// ================= K7: the top's weathered rock and the way up (8.25a gate) =================
// Vesper 3: the flat walk collider stays; its drawn top is covered with Boulder_0 to 5 at coverScale, laid flat (coverFlat of their height)
// and sunk so none stands over coverRise m (no collider: under a step, so no lip to stop on), every coverStep m off the top's places, and
// RubbleSparse_1 every rubbleStep m round the flat top's edge; two stones on the gully's walls by its head, headStoneRise m over the top,
// coming no nearer the head than its clear circle. Pim (Wren 2): a cairn of six pale stones (about 2 m), the ground's grass and ferns cleared cairnClear m round it and the foot, 1.6 m south-west of the scramble's foot, where
// both trail ends see it past the knob's south-west boulder,
// so the way up reads from the trail end; one capsule collider (it stands over the walk-into check's 0.5 m).
const float coverStep = 1.6f, coverJitter = 0.4f, coverScale = 0.22f, coverFlat = 0.35f, coverRise = 0.06f, coverPad = 0.3f, rubbleStep = 2.5f, rubbleScale = 0.5f;
const float headStoneScale = 0.3f, headStoneRise = 0.25f, headStoneOut = 0.35f, headStoneBackS = 0.4f, headStoneBackN = 1.8f, cairnX = 287.5f, cairnZ = 112.8f, cairnSink = 0.05f;
const float fx0 = 289f, fx1 = 297.5f, fz0 = 117f, fz1 = 125.5f; float[] cairnW = { 1.0f, 0.85f, 0.7f, 0.55f, 0.42f, 0.3f }, cairnH = { 0.45f, 0.4f, 0.36f, 0.32f, 0.28f, 0.24f }; const float cairnClear = 1.5f, cairnWMax = 1.4f;   // each stone cairnH tall (a width-only scale left the flat stones 0.7 m in all), no wider than cairnWMax of cairnW
var topKeeps = new (float x, float z, float r)[] { (292.0f, 122.0f, 0.9f), (291.3f, 123.2f, 0.6f), (297.0f, 121.2f, 0.5f), (291.0f, 120.6f, 1.0f), (293.0f, 123.6f, 0.6f), (289.4f, 117.4f, 0.5f), (295.6f, 120.3f, 0.6f) };
float wG = gullyHalf; var dirK = P(head.x - land.x, head.z - land.z).normalized; var perpK = P(-dirK.y, dirK.x);
bool TopKeep(UnityEngine.Vector2 q, float pad)
{
    foreach (var kc in topKeeps) if (UnityEngine.Vector2.Distance(q, P(kc.x, kc.z)) < kc.r + pad) return true;
    if (q.x > 294.5f - pad && q.x < 296.75f + pad && q.y > 116.7f - pad && q.y < 120.0f + pad) return true;   // the tent, its door and the letters
    return Off(q, A2, H2) < wG + pad || InBank(q) || BankS(q) < bankW + pad && Proj(q, S2, H2) >= bankT0 - 0.1f && Proj(q, S2, H2) <= 1f && BankS(q) > 0f;
}
int coverN = 0, rubbleN = 0;
{
    var cover = kit.Group("TopCover", L, L.position, 0f); var crng = new System.Random(8251); int n = 0;
    float Jit() => ((float)crng.NextDouble() * 2f - 1f) * coverJitter;
    void Sunk(UnityEngine.GameObject g, UnityEngine.Vector2 at, float rise) { var b = Exact(g); g.transform.position += V(at.x - b.center.x, kTop + rise - b.max.y, at.y - b.center.z); }
    for (float x = fx0 + coverStep * 0.5f; x < fx1; x += coverStep)
        for (float z = fz0 + coverStep * 0.5f; z < fz1; z += coverStep)
        {
            var q = P(x + Jit(), z + Jit()); if (TopKeep(q, coverPad)) continue;
            var g = kit.Spawn(PlaceKit.BK + "Rocks/Boulder_" + (n % 6), cover); n++; if (g == null) continue; PlaceKit.StripColliders(g); Rock(g);
            g.transform.rotation = UnityEngine.Quaternion.Euler(0f, (float)crng.NextDouble() * 360f, 0f); g.transform.localScale = V(g.transform.localScale.x * coverScale, g.transform.localScale.y * coverScale * coverFlat, g.transform.localScale.z * coverScale);
            Sunk(g, q, coverRise); coverN++;
        }
    var edgePts = new System.Collections.Generic.List<UnityEngine.Vector2>();
    for (float x = fx0; x <= fx1 + 1e-3f; x += rubbleStep) { edgePts.Add(P(x, fz0)); edgePts.Add(P(x, fz1)); }
    for (float z = fz0 + rubbleStep; z < fz1 - 1e-3f; z += rubbleStep) { edgePts.Add(P(fx0, z)); edgePts.Add(P(fx1, z)); }
    foreach (var q in edgePts)
    {
        if (TopKeep(q, 0f)) continue;
        var g = kit.Spawn(PlaceKit.BK + "Rocks/RubbleSparse_1", cover); if (g == null) continue; PlaceKit.StripColliders(g); Rock(g);
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, (float)crng.NextDouble() * 360f, 0f); g.transform.localScale *= rubbleScale; Sunk(g, q, coverRise); rubbleN++;
    }
    // each head stone's near side headStoneOut m (the body's radius) clear of the widened gully, so its hull (8.18a's solid pass gives one)
    // never stands in the walking line; the north one far enough back to keep off the walk from the head to the talk stand
    foreach (var (nm, back, side) in new[] { ("HeadStone_S", headStoneBackS, -1f), ("HeadStone_N", headStoneBackN, 1f) })
    {
        var g = kit.Spawn(PlaceKit.BK + "Rocks/Boulder_" + (n % 6), cover); n++; if (g == null) continue; g.name = nm; PlaceKit.StripColliders(g); Rock(g);
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, n * 47f, 0f); g.transform.localScale *= headStoneScale; var hb = Exact(g); float half = UnityEngine.Mathf.Max(hb.size.x, hb.size.z) * 0.5f;
        Sunk(g, P(head.x, head.z) - dirK * back + perpK * (side * (wG + headStoneOut + half)), headStoneRise);
    }
    // the cairn's stones a paler, worn grey than the knob (a marker someone stacked, read from the trail end at 15 m in the dusk frames)
    var cairnMat = kit.Tinted("Places_CairnStone", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#CFC9BD"), UnityEngine.Vector2.one);   // a pale weathered stone (the BK rock texture tinted pale still drew dark at dusk)
    var cairn = kit.Group("Cairn", L, V(cairnX, G(cairnX, cairnZ), cairnZ), 0f); float y = cairn.position.y;
    for (int i = 0; i < cairnW.Length; i++)
    {
        var g = kit.Spawn(PlaceKit.BK + "Rocks/Boulder_" + ((i + 2) % 6), cairn); if (g == null) continue; PlaceKit.StripColliders(g); foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>(true)) { var ms = r.sharedMaterials; for (int j = 0; j < ms.Length; j++) ms[j] = cairnMat; r.sharedMaterials = ms; }
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, i * 73f, 0f); var b = Exact(g); g.transform.localScale *= UnityEngine.Mathf.Min(cairnH[i] / UnityEngine.Mathf.Max(0.05f, b.size.y), cairnWMax * cairnW[i] / UnityEngine.Mathf.Max(0.05f, UnityEngine.Mathf.Max(b.size.x, b.size.z)));
        b = Exact(g); g.transform.position += V(cairnX - b.center.x, y - cairnSink - b.min.y, cairnZ - b.center.z); y = Exact(g).max.y;
    }
    kit.ClearDetail(V(cairnX, 0f, cairnZ), cairnClear); kit.ClearDetail(V(foot.x, 0f, foot.z), cairnClear);
    var cap = cairn.gameObject.AddComponent<UnityEngine.CapsuleCollider>(); float ch = y - cairn.position.y; cap.radius = cairnW[0] * 0.5f; cap.height = UnityEngine.Mathf.Max(ch, cairnW[0]); cap.center = V(0f, ch * 0.5f, 0f);
}

// ================= K6: the top warp =================
{ var w = kit.Root("DevWarps").transform.Find("Camp_2_Top"); if (w == null) notes.Add("no warp Camp_2_Top"); else { w.position = V(293.0f, kTop + 0.2f, 123.6f); w.rotation = UnityEngine.Quaternion.Euler(0f, 65f, 0f); } }

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | removed " + removed + " | core boxes " + coreBoxes + ", drawn faces " + faces + ", top cover " + coverN + ", rubble " + rubbleN + ", skin " + skinN + " (" + skinNote + ")" + (skinDropped.Count > 0 ? " (left out: " + string.Join(", ", skinDropped) + ")" : "") + ", steps " + stepN + " | usables " + uses + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
