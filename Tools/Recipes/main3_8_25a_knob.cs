// Main3 task 8.25a, Camp 2 rethink: his knob, to Camp2Layout.md draft 4 (Sable 2026-10-03; Marlow's 825a ground survey and paper check).
// Layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (the new pieces rebuilt under Campsites/Camp_2/Layout825a;
// absolute places). In the runner after main3_8_25_camp2.cs, whose column (GraniteStack, StackPath, StackTop) this replaces.
// REMOVALS (doc 3): GraniteStack; StackPath (its ramps, landings, rails and posts); StackTop and everything under it (his spot and the
//   letters under stones are kept, moved under Layout825a first); from Layout825 the Skirts, LandingRailN, WedgeFill, WedgeFillCorner,
//   BarrelStand and PaperSpots; Dressing's 8.17 talus (every BigBoulders_); in Forest/Grove_BurnEdge the plants inside the knob and Sequoia1.
// K1  the knob: footprint x kx0 to kx1, z kz0 to kz1, ground about 4.0, top kTop (9.0). A solid core of rock boxes (cellStep m columns merged
//     along each row) to kTop, except the gully on leg 2, where each column stops under the ramp. The core's west face is at kx0, flush to
//     leg 1's east edge. Skinned on its north, east and south faces with BK BigBoulders_0 to 5 (scale skinScale0 up), each placed by its
//     vertices: its outer face skinOut m past the core face, its top just under kTop, hulled; none in the gully or on the west face.
// K2  the scramble (the STAIRS RULE): invisible ramp boxes (rampT thick) under owned Boulder_0 to 5 steps (scale stepScale) whose tops sit
//     stepUnder m under the ramp. Leg 1: the foot (288.0, 114.5) at 4.0 north along x 287.2 to 288.8 to the landing, 1.6 x 1.6 at (288.0,
//     121.5), 7.0. Leg 2: from the landing's centre at bearing 107 to the head (291.0, 120.6) at 9.0, 1.6 wide. A rock fill under leg 1 and
//     the landing to the ground. The head's 1.0 m circle stays clear.
// K3  the top: his chair CS_Chair_2 at (292.0, 122.0) facing 65, a box from its mesh, `Talk`; his spot on it; the talk stand (291.3, 123.2)
//     facing 120; the tent CS_Tent_Modern_2, door north, its mesh box x 294.5 to 296.75, z 117.3 to 119.33, cut back doorCut short of the door
//     with a layer-2 doorway box over the cut; the ring box on a crate just inside the door, `Examine`; the lamp on a 1.2 m pole at (297.0,
//     121.2) with its core and practical light (as 8.25); a mug and thermos by the chair; the letters by the tent's south side.
// K4  rockfall: BoulderField's Boulder_2 (287.80, 122.34) to (300.0, 126.5) and Boulder_2 (285.9, 115.6) to (300.5, 123.5), each touching
//     the east face (its west extent at kx1).
// K5  paper spots PaperSpot_K1 (289.4, 117.4) on the top, K2 (287.0, 119.0) on a step block off leg 1's west edge, K3 on the moved boulder
//     (300.0, 126.5); each a marker with a sheet.
// K6  the Camp_2_Top warp (293.0, 9.2, 123.6) facing 65.
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
var knobMat = kit.Tinted("Places_KnobRock", rocks, Hex("#7E7A72"), new UnityEngine.Vector2(2f, 2f)); var skinMat = kit.Tinted("Places_KnobSkin", rocks, Hex("#8C877E"), UnityEngine.Vector2.one);
// the letters (moved from StackTop on the first run) are kept across a rerun: out of Layout825a before it is rebuilt, back in after
var keptLetters = c2.Find("Layout825a/Letters"); if (keptLetters != null) keptLetters.SetParent(c2, true);
var L = kit.Fresh("Layout825a", c2, c2.position, 0f); if (keptLetters != null) keptLetters.SetParent(L, true);

// ================= REMOVALS =================
const float removeTol = 0.3f; int removed = 0; var missing = new System.Collections.Generic.List<string>();
void Gone(UnityEngine.Transform t, string label) { if (t == null) { missing.Add(label); return; } PlaceKit.Remove(t); removed++; }
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
}
UnityEngine.Physics.SyncTransforms();

// ================= K1, K2: the knob and the scramble =================
const float kx0 = 288.8f, kx1 = 299f, kz0 = 116f, kz1 = 127f, kTop = 9f, cellStep = 0.25f, rampW = 1.6f, rampT = 0.2f;
const float skinOut = 0.6f, skinScale0 = 1.0f, skinScaleStep = 0.06f, skinUnderTop = 0.02f, stepScale = 0.35f, stepUnder = 0.02f, stepPitch = 0.9f;
var foot = V(288.0f, 4.0f, 114.5f); var land = V(288.0f, 7.0f, 121.5f); var head = V(291.0f, kTop, 120.6f); float landHalf = 0.8f;
var landS = V(land.x, land.y, land.z - landHalf);   // leg 1 ends at the landing's south edge
// the ramp surface's height at a point on a leg (t along it from a to b)
float Lerp(float a, float b, float t) => a + (b - a) * UnityEngine.Mathf.Clamp01(t);
float Proj(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; return UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude); }
float Off(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(Proj(q, a, b)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
var A2 = P(land.x, land.z); var H2 = P(head.x, head.z);
bool InGully(UnityEngine.Vector2 q) { float t = Proj(q, A2, H2); return t >= 0f && t <= 1f && Off(q, A2, H2) <= rampW * 0.5f; }
float GullyY(UnityEngine.Vector2 q) => Lerp(land.y, head.y, Proj(q, A2, H2));
int coreBoxes = 0, skinN = 0, stepN = 0;
{
    var core = kit.Group("Core", L, L.position, 0f);
    for (float z = kz0; z < kz1 - 1e-3f; z += cellStep)
    {
        float zc = z + cellStep * 0.5f, runX0 = float.NaN;
        for (float x = kx0; x <= kx1 + 1e-3f; x += cellStep)
        {
            bool edge = x >= kx1 - 1e-3f; float xc = x + cellStep * 0.5f; bool gully = !edge && InGully(P(xc, zc));
            if (!float.IsNaN(runX0) && (gully || edge)) { float gy = G((runX0 + x) * 0.5f, zc); Slab("Core", core, V((runX0 + x) * 0.5f, (gy - 0.3f + kTop) * 0.5f, zc), V(x - runX0, kTop - gy + 0.3f, cellStep), knobMat, true); coreBoxes++; runX0 = float.NaN; }
            if (edge) break;
            if (gully) { float ry = GullyY(P(xc, zc)) - rampT; float gy = G(xc, zc); if (ry - gy > 0.05f) { Slab("GullyFill", core, V(xc, (gy - 0.3f + ry) * 0.5f, zc), V(cellStep, ry - gy + 0.3f, cellStep), knobMat, true); coreBoxes++; } continue; }
            if (float.IsNaN(runX0)) runX0 = x;
        }
    }
    // the flat top: one thin slab collider over the top's own footprint (doc K1)
    kit.Blocker("TopSlab", L, L.InverseTransformPoint(V((289f + 297.5f) * 0.5f, kTop - 0.05f, (117f + 125.5f) * 0.5f)), V(297.5f - 289f, 0.1f, 125.5f - 117f));
    // the ramps: invisible boxes, rampT thick, their tops on the walking line
    void Ramp(string n, UnityEngine.Vector3 a, UnityEngine.Vector3 b) { var d = b - a; var mid = (a + b) * 0.5f; var rot = UnityEngine.Quaternion.LookRotation(d.normalized); var g = kit.Blocker(n, L, UnityEngine.Vector3.zero, V(rampW, rampT, d.magnitude)); g.transform.rotation = rot; g.transform.position = mid - (rot * UnityEngine.Vector3.up) * (rampT * 0.5f); }
    Ramp("Leg1Ramp", foot, landS); Ramp("Leg2Ramp", land, head);
    kit.Blocker("Landing", L, L.InverseTransformPoint(V(land.x, land.y - rampT * 0.5f, land.z)), V(rampW, rampT, landHalf * 2f));
    // the fill under leg 1 and the landing: rock columns to rampT under the walking line
    var fill = kit.Group("ScrambleFill", L, L.position, 0f);
    for (float z = foot.z; z < land.z + landHalf - 1e-3f; z += cellStep)
    {
        float zc = z + cellStep * 0.5f; float wy = zc >= landS.z ? land.y : Lerp(foot.y, landS.y, (zc - foot.z) / (landS.z - foot.z)); float top = wy - rampT, gy = G(foot.x, zc);
        if (top - gy > 0.05f) { Slab("Fill", fill, V(foot.x, (gy - 0.3f + top) * 0.5f, zc), V(rampW, top - gy + 0.3f, cellStep), knobMat, true); coreBoxes++; }
    }
    // the steps: boulders along both legs, their tops stepUnder m under the walking line
    var steps = kit.Group("Steps", L, L.position, 0f); int k = 0;
    void Step(UnityEngine.Vector3 at)
    {
        var g = kit.Spawn(PlaceKit.BK + "Rocks/Boulder_" + (k % 6), steps); if (g == null) return; PlaceKit.StripColliders(g); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, k * 67f, 0f); g.transform.localScale *= stepScale;
        var b = Exact(g); g.transform.position += V(at.x - b.center.x, at.y - stepUnder - b.max.y, at.z - b.center.z); k++; stepN++;
    }
    for (float s = stepPitch * 0.5f; s < UnityEngine.Vector3.Distance(foot, landS); s += stepPitch) Step(UnityEngine.Vector3.Lerp(foot, landS, s / UnityEngine.Vector3.Distance(foot, landS)));
    Step(land);
    for (float s = landHalf + stepPitch * 0.5f; s < UnityEngine.Vector3.Distance(land, head) - 0.3f; s += stepPitch) Step(UnityEngine.Vector3.Lerp(land, head, s / UnityEngine.Vector3.Distance(land, head)));
    // the skin: north, east and south faces, none on the west face or in the gully
    var skin = kit.Group("Skin", L, L.position, 0f); int si = 0;
    // clearUp: the walking space kept clear over the ramp and landing, from clearOver m over their surface (the body's height plus a margin)
    const float clearUp = 2.2f, clearOver = 0.05f; const int skinSizes = 6;
    var spots = new (char face, float along)[] { ('S', 292.5f), ('S', 296.5f), ('E', 117.5f), ('E', 120.5f), ('E', 123.5f), ('E', 126.0f), ('N', 293.5f), ('N', 296.8f) };
    foreach (var (face, along) in spots)
    {
        var g = kit.Spawn(PlaceKit.BK + "Rocks/BigBoulders_" + (si % 6), skin); if (g == null) continue; PlaceKit.StripColliders(g);
        foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) { var ms = r.sharedMaterials; for (int i = 0; i < ms.Length; i++) ms[i] = skinMat; r.sharedMaterials = ms; }
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, si * 61f, 0f); g.transform.localScale *= skinScale0 + skinScaleStep * (si % skinSizes); var b = Exact(g); float dy = kTop - skinUnderTop - b.max.y;
        if (face == 'E') g.transform.position += V(kx1 + skinOut - b.max.x, dy, along - b.center.z);
        else if (face == 'N') g.transform.position += V(along - b.center.x, dy, kz1 + skinOut - b.max.z);
        else g.transform.position += V(along - b.center.x, dy, kz0 - skinOut - b.min.z);
        b = Exact(g);
        var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); var hulls = new System.Collections.Generic.List<UnityEngine.Collider>();
        foreach (var r in g.GetComponentsInChildren<UnityEngine.MeshRenderer>()) { if (lodg != null && lodg.GetLODs().Length > 0 && System.Array.IndexOf(lodg.GetLODs()[0].renderers, r) < 0) continue; var mf = r.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; hulls.Add(mc); }
        UnityEngine.Physics.SyncTransforms(); string why = b.min.x < kx0 ? "the west face" : null;
        foreach (var (n, a, e) in new[] { ("leg 1", foot, landS), ("the landing", landS, land + V(0f, 0f, landHalf)), ("leg 2", land, head) })
        {
            var d = e - a; var rot = UnityEngine.Quaternion.LookRotation(d.normalized); var c = (a + e) * 0.5f + rot * V(0f, clearUp * 0.5f + clearOver, 0f);
            foreach (var h in UnityEngine.Physics.OverlapBox(c, V(rampW * 0.5f, clearUp * 0.5f, d.magnitude * 0.5f), rot, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (hulls.Contains(h)) why = n;
        }
        if (why != null) { notes.Add("skin " + face + " " + F(along) + " dropped: in " + why); PlaceKit.Remove(g.transform); si++; continue; }
        si++; skinN++;
    }
}
UnityEngine.GameObject Slab(string n, UnityEngine.Transform parent, UnityEngine.Vector3 world, UnityEngine.Vector3 size, UnityEngine.Material m, bool collide = false) => kit.Slab(n, parent, parent.InverseTransformPoint(world), size, m, default, collide);
// a piece's true extent: its LOD0 meshes' vertices in world space (renderer bounds grow with a turn)
UnityEngine.Bounds Exact(UnityEngine.GameObject g) { var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); var rs = lodg != null && lodg.GetLODs().Length > 0 ? lodg.GetLODs()[0].renderers : g.GetComponentsInChildren<UnityEngine.Renderer>(); bool any = false; var b = new UnityEngine.Bounds(); foreach (var r in rs) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) continue; foreach (var v in mf.sharedMesh.vertices) { var w = r.transform.TransformPoint(v); if (!any) { b = new UnityEngine.Bounds(w, UnityEngine.Vector3.zero); any = true; } else b.Encapsulate(w); } } return any ? b : PlaceKit.MeshBounds(g); }

// ================= K3: the top =================
const float doorCut = 0.9f, crateH = 0.65f, crateW = 0.4f, crateIn = 0.65f, ringSide = 0.07f, poleH = 1.2f, lampGlow = 12f, coreH = 0.45f, coreW = 0.35f;
const float tentX0 = 294.5f, tentX1 = 296.75f, tentZ0 = 117.3f, tentZ1 = 119.33f;
var topG = kit.Group("Top", L, V(293f, kTop, 121f), 0f);
{
    var chair = kit.On(PlaceKit.CS + "CS_Chair_2", topG, topG.InverseTransformPoint(V(292.0f, kTop, 122.0f)), 65f, 1f, false, null, true); if (chair != null) { chair.name = "HisChair"; PlaceKit.FitExact(chair); Use(chair, "Talk", null); }
    var spot = c2.Find("Resident_Camp2_Spot"); if (spot != null) { spot.position = V(292.0f, kTop + 0.9f, 122.0f); spot.rotation = UnityEngine.Quaternion.Euler(0f, 65f, 0f); } else notes.Add("no Resident_Camp2_Spot");
    kit.Marker("TalkStand_Top", topG, topG.InverseTransformPoint(V(291.3f, kTop, 123.2f)), 120f);
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
        kit.On(PlaceKit.CS + "Beds/CS_Bedroll_Modern_1", topG, topG.InverseTransformPoint(V(wb.center.x, kTop, (wb.min.z + cutZ) * 0.5f)), 0f, 1f, false, null, true);
        // the letters under stones, by the tent's south side
        var letters = L.Find("Letters"); if (letters != null) { int n = 0; foreach (UnityEngine.Transform t in letters) { int slot = n / 2; t.position = V(tentX0 + 0.3f + (slot % 3) * 0.35f, kTop + (t.name.StartsWith("CS_Stone_") ? 0.01f : 0f), wb.min.z - 0.3f); n++; } }
    }
    // the lamp on a 1.2 m pole, its cold core and practical light (as 8.25)
    var lamp = kit.Group("Lamp", topG, V(297.0f, kTop, 121.2f), 0f);
    kit.Fill(PlaceKit.CI + "Building/CITW_Wood_Pillar", lamp, V(0f, 0f, 0f), V(0.09f, poleH, 0.09f)); kit.Blocker("PoleCollider", lamp, V(0f, poleH * 0.5f, 0f), V(0.09f, poleH, 0.09f));
    var lantern = kit.On(PlaceKit.CS + "CS_Lantern_Modern", lamp, V(0f, poleH, 0f), 0f, 1.4f, false, null, true);
    float coreY = lantern != null ? lamp.InverseTransformPoint(PlaceKit.MeshBounds(lantern).max).y + coreH * 0.5f : poleH + 0.1f;
    var core = kit.Slab("LampCore", lamp, V(0f, coreY, 0f), V(coreW, coreH, coreW), kit.Glow("Places_Camp2LampCore", Hex("#DDE6F0"), lampGlow)); core.GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    kit.Practical("LampLight", lamp, V(0f, poleH + 0.1f, 0f), 6f, PracticalLight.Kind.Lantern, PracticalLight.ByDay.Off);
}

// ================= K4: rockfall =================
{
    var bf = c2.Find("BoulderField"); int moved = 0;
    foreach (var (fx, fz, tx, tz) in new[] { (287.80f, 122.34f, 300.0f, 126.5f), (285.9f, 115.6f, 300.5f, 123.5f) })
    {
        var t = Near(bf, "Boulder_2", fx, fz, 0.5f) ?? Near(bf, "Boulder_2", tx + 0.5f, tz, 2.5f); if (t == null) { notes.Add("no BoulderField/Boulder_2 at (" + F(fx) + ", " + F(fz) + ")"); continue; }
        float lift = t.position.y - G(t.position.x, t.position.z); t.position = V(tx, G(tx, tz) + lift, tz); UnityEngine.Physics.SyncTransforms(); var b = PlaceKit.MeshBounds(t.gameObject); t.position += V(kx1 - b.min.x, 0f, 0f); moved++;
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

// ================= K6: the top warp =================
{ var w = kit.Root("DevWarps").transform.Find("Camp_2_Top"); if (w == null) notes.Add("no warp Camp_2_Top"); else { w.position = V(293.0f, kTop + 0.2f, 123.6f); w.rotation = UnityEngine.Quaternion.Euler(0f, 65f, 0f); } }

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | removed " + removed + " | core boxes " + coreBoxes + ", skin " + skinN + ", steps " + stepN + " | usables " + uses + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
