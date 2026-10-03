// Main3 task 8.25, Camp 2 to Camp2Layout.md draft 2 (Sable 2026-10-02; Camp2Layout_UI.md, Camp2Layout_Story.md, Marlow's 825 paper check):
// layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (absolute places; the new pieces rebuilt under
// Campsites/Camp_2/Layout825 and StackTop/Layout825; trail points set by rule). In the runner after main3_8_24_north.cs.
// Every turned prop gets its box from its own meshes in its own axes (PlaceKit.FitExact; FitCollider inflates turned props, Marlow 825 3).
// REMOVALS (R): each named piece by its name within removeTol m (Marlow's read); then every fir, sapling, plant, log or stump left in a
//   Camp 2 keep-out zone (KeepOuts: K1 the phone wire, K4 the barrel, K6 the booth) under Forest, SliceLook or Ground815; C4: Camp_2/Ladder.
// C1  the top: a rope rail on rock, an octagon 0.25 m inside the top's faces (faces topRailFace, vertices topRailVertex from the centre, at
//     the stack's own bearings), posts every postStep m, rope at 0.5 and 1.05, a 1.05 x 0.1 box run as its collider, open only at the
//     landing mouth, (295.90, 106.5) to the vertex (296.52, 108); LandingTop_RailS cut back to start at x 295.90; his chair CS_Chair_2 at
//     (294.94, 110.30) facing 66 (slid 0.18 m from the doc's (294.76, 110.31) so the T signpost clears, Wren 2026-10-03), a box from its mesh (never a hull: a hulled seat is a perch over the rail); the tent CS_Tent_Modern_2
//     at (288.86, 108.0), door east, box from its fabric; the ring box inside it; the lamp on a 1.2 m pole at (291.6, 107.4), cold core;
//     the letters under stones by the tent; his mug and thermos by his chair; his spot on the chair.
// C2  the table cluster beside the booth: CS_Table_Small_Modern_2 at 1.3, yaw 0, x 298.10 to 299.01, z 97.30 to 98.21; his chair east facing
//     west, yours west facing east, the third place south facing north, each flush; his hand fanned on his side, a hand dealt face down by a
//     clean upturned mug at the third place; the handset (kitbash) on the booth's south wall inner face, hook (299.63, 1.35 up, 98.40).
// C3  the landing rail: LandingTop's north edge from the vertex (296.52, 108) to RailOverLaneA (298.2), 1.05.
// C5  the barrel CITW_Barrel_3 at 1.2, yaw 0, at (291.4, 101.4); its stand (292.3, 100.4) facing 318.
// C6  the T leg end: points P2 to P12 dropped, new points (300.4, 105.3), (302.4, 106.0), (303.6, 109.5), (304.0, 114.0) from the ramp foot
//     to P14, every trailStep m, the tread painted (copied from the leg's own tread) and the old stretch repainted as floor; skirts (planks
//     from the ground to whatever is overhead, no cap (Wren 2026-10-02: the 3 m cap left standable tops); bareSkirt m where nothing is overhead) round the stair's ground, the way-in planks; StartBlaze by position (304.4, 106.4).
// C7  the paper spots PS1 to PS3 (markers with a sheet each). C8 the phone wire from the pole's crossarm to a 1 m standoff on the booth
//     roof, sag wireSag, no collider. C9 Boulder_1 to (303.4, 97.9).
// WARPS: Camp_2 (294.3, 95.0) facing 55; Camp_2_Top (294.5, 24.2, 107.2) facing 20.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var notes = new System.Collections.Generic.List<string>();
var c2 = kit.Root("Campsites") != null ? kit.Root("Campsites").transform.Find("Camp_2") : null; var cd = c2 != null ? c2.Find("Dressing") : null; var top = c2 != null ? c2.Find("StackTop") : null; var path2 = c2 != null ? c2.Find("StackPath") : null;
if (cd == null || top == null || path2 == null || kit.Root("Forest") == null) return "run 8.5, 8.16 and 8.17 camp2 first (Campsites/Camp_2/Dressing, StackTop, StackPath, Forest)";
UnityEngine.Physics.SyncTransforms();
float G(float x, float z) => kit.H(x, z);
const float sx = 292f, sz = 108f, topY = 24f, hookUp = 1.35f;
var rope = kit.Tinted("Places_Rope", "Assets/Materials/Planks023A_1.0x1.0.mat", Hex("#8C7A58"), UnityEngine.Vector2.one);
var L = kit.Fresh("Layout825", c2, c2.position, 0f); var LT = kit.Fresh("Layout825", top, top.position, 0f);

// ================= REMOVALS =================
const float removeTol = 0.3f;
var named = new (string n, float x, float z, string why)[] {
    ("RedPine2", 288.87f, 86.44f, "C8"), ("RedPine3", 291.27f, 87.60f, "C8"), ("RedPine1", 300.78f, 95.76f, "C8"), ("Grass2", 291.27f, 100.11f, "C5"),
    ("GrassMoss", 303.25f, 107.36f, "C6"), ("CS_Stone_6", 300.71f, 118.92f, "C6"), ("Branchs", 298.91f, 112.05f, "C6"), ("Branchs", 301.13f, 115.20f, "C6"), ("Grass3", 298.63f, 99.41f, "C9") };
string[] keepOutKinds = { "RedFir", "RedPine", "Bush", "CS_Bush", "ThinFern", "Fern", "DeadLeaves", "Branchs", "RedwoodHollowLog", "CITW_Tree_Stump", "Grass", "Nettle", "Mushroom" };
bool Kind(string n) { foreach (var k in keepOutKinds) if (n.StartsWith(k)) return true; return false; }
var pieces = new System.Collections.Generic.List<UnityEngine.Transform>();
foreach (var rn in new[] { "Forest", "SliceLook", "Ground815" }) { var r = kit.Root(rn); if (r == null) continue; foreach (var t in r.GetComponentsInChildren<UnityEngine.Transform>(true)) { if (t == r.transform || WalkIns.PathOf(t).StartsWith("Ground815/Stops") || WalkIns.PathOf(t).StartsWith("Ground815/Pockets")) continue; if (UnityEditor.PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject) || !UnityEditor.PrefabUtility.IsPartOfPrefabInstance(t.gameObject)) pieces.Add(t); } }
var gone = new System.Collections.Generic.HashSet<UnityEngine.Transform>(); int namedGone = 0, zoneGone = 0; var namedMissing = new System.Collections.Generic.List<string>();
foreach (var (n, x, z, why) in named)
{
    UnityEngine.Transform hit = null; float best = removeTol;
    foreach (var t in pieces) { if (t == null || gone.Contains(t) || !(t.name == n || t.name.StartsWith(n + " ("))) continue; float d = UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(x, z)); if (d <= best) { best = d; hit = t; } }
    if (hit == null) { namedMissing.Add(why + " " + n); continue; } gone.Add(hit); UnityEngine.Object.DestroyImmediate(hit.gameObject); namedGone++;
}
foreach (var t in pieces)
{
    if (t == null || gone.Contains(t) || !Kind(t.name) || t.name.StartsWith("Sequoia")) continue; var z = KeepOuts.WhichCamp2(P(t.position.x, t.position.z)); if (z == null) continue;
    gone.Add(t); UnityEngine.Object.DestroyImmediate(t.gameObject); zoneGone++;
}
PlaceKit.Remove(c2.Find("Ladder"));   // C4: the ladder promised a climb the game does not have (8.17's 5 m talus keep stays)
UnityEngine.Physics.SyncTransforms();

// ================= C1: the top =================
const float topRailFace = 4.18f, topRailVertex = 4.52f, postStep = 2f, railH = 1.05f, railT = 0.1f, ropeLow = 0.5f, postW = 0.08f;
int railRuns = 0;
void RailRun(UnityEngine.Transform parent, UnityEngine.Vector3 a, UnityEngine.Vector3 b, string name)   // a rope rail from a to b on its floor (a.y)
{
    var d = b - a; d.y = 0f; float len = d.magnitude; if (len < 0.05f) return; var mid = (a + b) * 0.5f; float yaw = UnityEngine.Mathf.Atan2(d.x, d.z) * UnityEngine.Mathf.Rad2Deg;
    var run = kit.Group(name, parent, V(mid.x, a.y, mid.z), yaw);
    kit.Blocker("Collider", run, V(0f, railH * 0.5f, 0f), V(railT, railH, len));
    int posts = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(len / postStep));
    for (int k = 0; k <= posts; k++) { float u = -len * 0.5f + len * k / posts; kit.Fill(PlaceKit.CI + "Building/CITW_Wood_Pillar", run, V(0f, 0f, u), V(postW, railH + 0.05f, postW)); }
    foreach (var h in new[] { ropeLow, railH }) kit.Slab("Rope", run, V(0f, h, 0f), V(0.03f, 0.03f, len), rope);
    railRuns++;
}
{
    UnityEngine.Vector3 Vert(float bearing) => V(sx + UnityEngine.Mathf.Sin(bearing * UnityEngine.Mathf.Deg2Rad) * topRailVertex, topY, sz + UnityEngine.Mathf.Cos(bearing * UnityEngine.Mathf.Deg2Rad) * topRailVertex);
    var rail = kit.Group("TopRail", LT, LT.position, 0f);
    var pts = new System.Collections.Generic.List<UnityEngine.Vector3> { V(295.90f, topY, 106.5f) };   // the landing mouth, on the 90 to 135 edge
    for (float b = 135f; b <= 405f + 1e-3f; b += 45f) pts.Add(Vert(b));   // round by the south, west and north to the 90 vertex (450 = 90)
    pts.Add(Vert(90f));
    for (int i = 1; i < pts.Count; i++) RailRun(rail, pts[i - 1], pts[i], "Run" + i);
    // LandingTop_RailS cut back to start at x 295.90 (Marlow 11b's wedge)
    var rs = path2.Find("LandingTop_RailS"); if (rs != null) { float x0 = 295.90f, x1 = 301.1f; rs.position = V((x0 + x1) * 0.5f, rs.position.y, rs.position.z); rs.localScale = V(x1 - x0, rs.localScale.y, rs.localScale.z); } else notes.Add("no LandingTop_RailS");
    // his chair at the east rail facing the T (66), a box from its mesh; his mug and thermos beside it
    var topD = top.Find("Dressing"); foreach (var n in new[] { "CS_Chair_2", "CS_Tent_Modern_2", "CS_Lantern_Modern", "LampCore", "CS_Drink_Thermos_1", "CS_Tableware_Mug_Metal_1" }) if (topD != null) PlaceKit.Remove(topD.Find(n));
    var chair = kit.On(PlaceKit.CS + "CS_Chair_2", LT, LT.InverseTransformPoint(V(294.94f, topY, 110.30f)), 66f, 1f, false, null, true); if (chair != null) { chair.name = "HisChair"; PlaceKit.FitExact(chair); }
    kit.On(PlaceKit.CS + "Drinks/CS_Drink_Thermos_1", LT, LT.InverseTransformPoint(V(294.2f, topY, 110.9f)), 0f, 1f, false, null, true);
    kit.On(PlaceKit.CS + "Tableware/CS_Tableware_Mug_Metal_1", LT, LT.InverseTransformPoint(V(294.0f, topY, 110.6f)), 30f, 1f, false, null, true);
    // the tent, door east (its door is on its own -z: yaw 270 faces it east), box from its fabric; the ring box inside it, by the door
    var tent = kit.On(PlaceKit.CS + "CS_Tent_Modern_2", LT, LT.InverseTransformPoint(V(288.86f, topY, 108.0f)), 270f, 1f, false, null, true); if (tent != null) { tent.name = "Tent"; PlaceKit.FitExact(tent, "Rope"); }
    var ringMat = kit.Tinted("Places_RingBox", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#5A1E22"), UnityEngine.Vector2.one);
    kit.Slab("RingBox", LT, LT.InverseTransformPoint(V(289.35f, topY + 0.035f, 108.0f)), V(0.07f, 0.07f, 0.07f), ringMat);
    // the lamp on a 1.2 m pole 2.0 m east of the door, its cold core the deck's hard target
    const float poleH = 1.2f; var lamp = kit.Group("Lamp", LT, V(291.6f, topY, 107.4f), 0f);
    kit.Fill(PlaceKit.CI + "Building/CITW_Wood_Pillar", lamp, V(0f, 0f, 0f), V(0.09f, poleH, 0.09f)); kit.Blocker("PoleCollider", lamp, V(0f, poleH * 0.5f, 0f), V(0.09f, poleH, 0.09f));
    kit.On(PlaceKit.CS + "CS_Lantern_Modern", lamp, V(0f, poleH, 0f), 0f, 1.4f, false, null, true);
    var core = kit.Slab("LampCore", lamp, V(0f, poleH + 0.1f, 0f), V(0.12f, 0.18f, 0.12f), kit.Glow("Places_ColdLamp", Hex("#DDE6F0"), kit.Look.farMarkerIntensity)); core.GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    // the letters under stones, by the tent's south side
    if (topD != null) { int k = 0; foreach (UnityEngine.Transform t in topD) if (t.name.StartsWith("Paper") || t.name.StartsWith("CS_Stone_")) { int slot = k / 2; t.position = V(288.4f + (slot % 3) * 0.35f, t.position.y, 106.75f - (slot / 3) * 0.3f + (t.name.StartsWith("CS_Stone_") ? 0.01f : 0f)); k++; } }
    var spot = top.Find("Resident_Camp2_Spot"); if (spot != null) { spot.position = V(294.94f, topY + 0.9f, 110.30f); spot.rotation = UnityEngine.Quaternion.Euler(0f, 66f, 0f); }
    kit.Marker("TalkStand_Top", LT, LT.InverseTransformPoint(V(293.8f, topY, 109.5f)), 50f);
}
// ================= C3: the landing's north edge =================
RailRun(L, V(296.52f, topY, 108.0f), V(298.2f, topY, 108.0f), "LandingRailN");

// ================= C2: the booth's table cluster and the handset =================
const float tblX0 = 298.10f, tblX1 = 299.01f, tblZ0 = 97.30f, tblZ1 = 98.21f;
{
    PlaceKit.Remove(cd.Find("CardTable"));
    var cards = kit.Group("CardTable", L, V((tblX0 + tblX1) * 0.5f, G((tblX0 + tblX1) * 0.5f, (tblZ0 + tblZ1) * 0.5f), (tblZ0 + tblZ1) * 0.5f), 0f);
    var tbl = kit.On(PlaceKit.CS + "CS_Table_Small_Modern_2", cards, V(0f, 0f, 0f), 0f, 1.3f, false, null, true); if (tbl != null) PlaceKit.FitExact(tbl);
    float tTop = tbl != null ? PlaceKit.LocalBounds(tbl, cards).max.y : 0.78f;
    // the three places, each flush to the table (centres from the doc's boxes), yaw 0, 90 or 270 so the boxes are true
    foreach (var (n, path, x, z, yaw) in new[] { ("HisChair", "CS_Chair_1", 299.40f, 97.76f, 270f), ("YourChair", "CS_Chair_4", 297.71f, 97.76f, 90f), ("ThirdPlace", "CS_Chair_1", 298.55f, 96.91f, 0f) })
    { var ch = kit.On(PlaceKit.CS + path, cards, cards.InverseTransformPoint(V(x, G(x, z), z)), yaw, 1f, false, null, true); if (ch != null) { ch.name = n; PlaceKit.FitExact(ch); } }
    // his hand fanned on his side (east), a hand dealt face down and squared by a clean upturned mug at the third place (south)
    for (int i = 0; i < 5; i++) kit.On(PlaceKit.CE + "Decoration_Home/Paper", cards, V(0.24f, tTop, -0.16f + i * 0.08f), 90f + i * 23f, 0.3f, false, null, true);
    for (int i = 0; i < 5; i++) kit.On(PlaceKit.CE + "Decoration_Home/Paper", cards, V(-0.05f + i * 0.002f, tTop + i * 0.003f, -0.28f), i * 2f, 0.3f, false, null, true);
    kit.On(PlaceKit.CS + "Tableware/CS_Tableware_Mug_Metal_2", cards, V(0.16f, tTop, -0.32f), 0f, 1f, false, V(180f, 0f, 0f), true);
    kit.On(PlaceKit.CS + "CS_Lantern_Old", cards, V(0.32f, tTop, 0.3f), 0f, 1f, false, null, true);
    kit.On(PlaceKit.CE + "Decoration_Home/Ashtray", cards, V(0.32f, tTop, -0.36f), 0f, 1f, false, null, true);   // matches for a tally
    kit.On(PlaceKit.CS + "Bags/CS_Backpack_Old_2", cards, cards.InverseTransformPoint(V(300.1f, G(300.1f, 96.9f), 96.9f)), 60f, 1f, false, null, true);
    // the handset (kitbash: no owned payphone), on the booth's south wall's inner face; its hook; a solid box so the eye ray can meet it
    var phoneMat = kit.Tinted("Places_Handset", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#1C1C1E"), UnityEngine.Vector2.one);
    var hs = kit.Group("Handset", L, V(299.63f, G(299.63f, 98.40f) + hookUp, 98.40f), 0f);
    kit.Slab("Hook", hs, V(0f, 0.08f, 0.02f), V(0.12f, 0.2f, 0.03f), phoneMat);
    var rec = kit.Slab("Receiver", hs, V(0f, 0f, 0.05f), V(0.07f, 0.22f, 0.06f), phoneMat, default, true);
    kit.Slab("Cord", hs, V(0f, -0.25f, 0.05f), V(0.015f, 0.3f, 0.015f), phoneMat);
}

// ================= C5: the barrel =================
{
    var old = cd.Find("CITW_Barrel_3"); PlaceKit.Remove(old);
    var b = kit.Ground(PlaceKit.CI + "Props/CITW_Barrel_3", L, 291.4f, 101.4f, 0f, 1.2f, false); if (b != null) { b.name = "Barrel"; PlaceKit.FitExact(b); }
    kit.Marker("BarrelStand", L, L.InverseTransformPoint(V(292.3f, G(292.3f, 100.4f), 100.4f)), 318f);
}

// ================= C6: the T leg's end, the skirts, the blaze =================
const float trailStep = 2f, treadPaint = 1.5f, bareSkirt = 3f, skirtReach = 30f, boardW = 0.22f, boardGap = 0.06f, boardT = 0.05f;
int newPts = 0, paintCells = 0, boardN = 0;
{
    var leg = kit.Root("Trails").transform.Find("Camp 2 to T"); if (leg == null) return "no Trails/Camp 2 to T";
    // drop the points under the stair (P2 to P12, Marlow 825: (300.3, 107.1) to (301.5, 117.6)), then the new ones from the ramp foot to P14
    UnityEngine.Transform p0 = leg.childCount > 0 ? leg.GetChild(0) : null, p14 = null;
    foreach (UnityEngine.Transform p in leg) if (UnityEngine.Vector2.Distance(P(p.position.x, p.position.z), P(303.77f, 117.97f)) < 0.5f) p14 = p;
    var oldStretch = new System.Collections.Generic.List<UnityEngine.Vector2>();
    if (p0 == null || p14 == null) notes.Add("Camp 2 to T: no ramp foot point or P14");
    else
    {
        int i14 = p14.GetSiblingIndex();
        for (int i = i14 - 1; i >= 1; i--) { var t = leg.GetChild(i); oldStretch.Add(P(t.position.x, t.position.z)); UnityEngine.Object.DestroyImmediate(t.gameObject); }
        var route = new[] { P(p0.position.x, p0.position.z), P(300.4f, 105.3f), P(302.4f, 106.0f), P(303.6f, 109.5f), P(304.0f, 114.0f), P(p14.position.x, p14.position.z) };
        int at = 1;
        for (int s = 1; s < route.Length; s++)
        {
            var a = route[s - 1]; var b = route[s]; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector2.Distance(a, b) / trailStep));
            for (int k = 1; k <= n; k++) { if (s == route.Length - 1 && k == n) break; var q = UnityEngine.Vector2.Lerp(a, b, k / (float)n); var pt = new UnityEngine.GameObject("C6_" + (newPts + 1)).transform; pt.SetParent(leg, false); pt.position = V(q.x, G(q.x, q.y), q.y); pt.SetSiblingIndex(at++); newPts++; }
        }
        // the tread: alphamap cells within treadPaint m of the new route take the weights of the leg's own tread (P16), the dropped
        // stretch's cells (and not the new tread's, nor the boathouse leg's) take the camp floor's (at (296, 103))
        var ter = kit.Terrain; var td = ter.terrainData; var to = ter.transform.position; int ar = td.alphamapResolution; float cw = td.size.x / ar, ch = td.size.z / ar;
        float[] Weights(float x, float z) { int i = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt((x - to.x) / cw), 0, ar - 1), j = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt((z - to.z) / ch), 0, ar - 1); var a = td.GetAlphamaps(i, j, 1, 1); var w = new float[a.GetLength(2)]; for (int l = 0; l < w.Length; l++) w[l] = a[0, 0, l]; return w; }
        var p16 = leg.GetChild(UnityEngine.Mathf.Min(leg.childCount - 1, p14.GetSiblingIndex() + 1)).position; var treadW = Weights(p16.x, p16.z); var floorW = Weights(296f, 103f);
        var other = new System.Collections.Generic.List<UnityEngine.Vector2>(); var bl = kit.Root("Trails").transform.Find("Boathouse to Camp 2"); if (bl != null) foreach (UnityEngine.Transform p in bl) other.Add(P(p.position.x, p.position.z));
        float SegDist(UnityEngine.Vector2 q, System.Collections.Generic.IList<UnityEngine.Vector2> line) { float best = float.MaxValue; for (int i = 1; i < line.Count; i++) { var a = line[i - 1]; var ab = line[i] - a; float tt = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(q, a + ab * tt)); } return best; }
        var oldLine = new System.Collections.Generic.List<UnityEngine.Vector2> { route[0] }; oldStretch.Reverse(); oldLine.AddRange(oldStretch); oldLine.Add(route[route.Length - 1]);
        int i0 = UnityEngine.Mathf.FloorToInt((295f - to.x) / cw), i1 = UnityEngine.Mathf.CeilToInt((308f - to.x) / cw), j0 = UnityEngine.Mathf.FloorToInt((102f - to.z) / ch), j1 = UnityEngine.Mathf.CeilToInt((121f - to.z) / ch);
        var am = td.GetAlphamaps(i0, j0, i1 - i0 + 1, j1 - j0 + 1);
        for (int j = 0; j <= j1 - j0; j++) for (int i = 0; i <= i1 - i0; i++)
        {
            var q = P(to.x + (i0 + i + 0.5f) * cw, to.z + (j0 + j + 0.5f) * ch); float dNew = SegDist(q, route), dOld = SegDist(q, oldLine), dOther = other.Count > 1 ? SegDist(q, other) : float.MaxValue;
            float[] w = dNew <= treadPaint ? treadW : (dOld <= treadPaint && dOther > treadPaint) ? floorW : null; if (w == null) continue;
            for (int l = 0; l < w.Length; l++) am[j, i, l] = w[l]; paintCells++;
        }
        td.SetAlphamaps(i0, j0, am); UnityEditor.EditorUtility.SetDirty(td);
        for (int s = 1; s < route.Length; s++) for (float u = 0f; u <= 1f; u += 0.25f) { var q = UnityEngine.Vector2.Lerp(route[s - 1], route[s], u); kit.ClearDetail(V(q.x, 0f, q.y), treadPaint); }
    }
    // skirts: upright boards over a box collider each, from the ground to whatever is overhead (no cap), along the stair's ground
    var sk = kit.Group("Skirts", L, L.position, 0f);
    void Board(UnityEngine.Vector3 c, float h, bool alongX)
    {
        var g = kit.Fill(PlaceKit.CC + "Props/C_Plank_A_Thick", sk, sk.InverseTransformPoint(c), alongX ? V(boardW, boardT, h) : V(h, boardT, boardW), alongX ? 0f : 90f); if (g == null) return;
        var mb = PlaceKit.MeshBounds(g); g.transform.RotateAround(mb.center, alongX ? UnityEngine.Vector3.right : UnityEngine.Vector3.forward, 90f); mb = PlaceKit.MeshBounds(g); g.transform.position += c - mb.center; boardN++;
    }
    void Skirt(float x0, float z0, float x1, float z1, bool bare = false)   // bare: bareSkirt m whatever is overhead (the way-in planks)
    {
        bool alongX = UnityEngine.Mathf.Abs(x1 - x0) >= UnityEngine.Mathf.Abs(z1 - z0); float len = alongX ? UnityEngine.Mathf.Abs(x1 - x0) : UnityEngine.Mathf.Abs(z1 - z0); if (len < 0.05f) return;
        int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(len / (boardW + boardGap))); float pitch = len / n;
        for (int k = 0; k < n; k++)
        {
            float u = (k + 0.5f) / n; float x = UnityEngine.Mathf.Lerp(x0, x1, u), z = UnityEngine.Mathf.Lerp(z0, z1, u), gy = G(x, z), over = float.MaxValue;
            foreach (var side in new[] { -0.1f, 0.1f }) { float ox = alongX ? 0f : side, oz = alongX ? side : 0f; foreach (var hit in UnityEngine.Physics.RaycastAll(V(x + ox, gy + 0.05f, z + oz), UnityEngine.Vector3.up, skirtReach, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (hit.collider.transform.IsChildOf(path2)) over = UnityEngine.Mathf.Min(over, hit.distance + 0.05f); }   // both sides of the line: the overhead edge sits on it; only the stair (StackPath) counts overhead: way-in planks run to the stack's overhang left a standable top at 12.4 (8.25 round 2)
            float h = over < float.MaxValue && !bare ? over : bareSkirt;
            if (h < 0.1f) continue; var c = V(x, gy + h * 0.5f, z);
            Board(c, h, alongX); kit.Blocker("Board", sk, sk.InverseTransformPoint(c), alongX ? V(pitch, h, 0.1f) : V(0.1f, h, pitch));
        }
    }
    Skirt(301.1f, 108f, 301.1f, 119.5f); Skirt(298.2f, 119.5f, 301.1f, 119.5f); Skirt(298.2f, 112f, 298.2f, 119.5f); Skirt(296.28f, 112f, 298.2f, 112f); Skirt(299.6f, 108f, 301.1f, 108f);
    Skirt(297.32f, 106.5f, 298.2f, 106.5f, true); Skirt(298.2f, 106.5f, 298.2f, 108f, true);   // the way-in planks (Marlow 11a, 11c), bareSkirt high: run up to LandingS1 they closed the notch under a stack ledge the stair's rails let a jump reach (8.25 round 2 trap at (297.4, 12.4, 106.9))
    // the gap between Ramp1 and Ramp2 (Wren 2026-10-02, the 8.25 trap under Ramp2: a sprint-jump off Ramp1 over its east rail went under
    // Ramp2): boards over a box each, in the gap (gapX), from Ramp1's top to Ramp2's underside, every board pitch along the ramps
    var r1 = path2.Find("Ramp1") != null ? path2.Find("Ramp1").GetComponent<UnityEngine.Collider>() : null; var r2 = path2.Find("Ramp2") != null ? path2.Find("Ramp2").GetComponent<UnityEngine.Collider>() : null;
    if (r1 == null || r2 == null) notes.Add("no Ramp1 or Ramp2 collider");
    else
    {
        float gapX = (r1.bounds.max.x + r2.bounds.min.x) * 0.5f, z0 = UnityEngine.Mathf.Max(r1.bounds.min.z, r2.bounds.min.z), z1 = UnityEngine.Mathf.Min(r1.bounds.max.z, r2.bounds.max.z);
        int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt((z1 - z0) / (boardW + boardGap))); float pitch = (z1 - z0) / n;
        for (int k = 0; k < n; k++)
        {
            float z = z0 + (k + 0.5f) * pitch;
            if (!r1.Raycast(new UnityEngine.Ray(V(r1.bounds.max.x - 0.05f, r1.bounds.max.y + 1f, z), UnityEngine.Vector3.down), out var lo, 40f) || !r2.Raycast(new UnityEngine.Ray(V(r2.bounds.min.x + 0.05f, r2.bounds.min.y - 1f, z), UnityEngine.Vector3.up), out var hi, 40f)) continue;
            float b = lo.point.y, t = hi.point.y + 0.05f; if (t - b < 0.1f) continue; var c = V(gapX, (b + t) * 0.5f, z);
            Board(c, t - b, false); kit.Blocker("RampGap", sk, sk.InverseTransformPoint(c), V(r2.bounds.min.x - r1.bounds.max.x + 0.04f, t - b, pitch));
        }
    }
    // Ramp2's and Ramp4's east rails (the stair's outer side, Wren 2026-10-02): railH (1.05) over the ramp's top, measured square to the
    // ramp; their feet stay where they are. The rails were 0.93 square to the ramp (1.04 plumb); 14 sprint-jumps went over them.
    foreach (var rn in new[] { "Ramp2", "Ramp4" })
    {
        var ramp = path2.Find(rn); UnityEngine.Transform east = null; foreach (UnityEngine.Transform t in path2) if (t.name == rn + "_Rail" && (east == null || t.position.x > east.position.x)) east = t;
        if (ramp == null || east == null) { notes.Add("no " + rn + " or its east rail"); continue; }
        var up = ramp.up; float rampTop = ramp.localScale.y * 0.5f, o = UnityEngine.Vector3.Dot(east.position - ramp.position, up), foot = o - east.localScale.y * 0.5f, head = rampTop + railH;
        east.localScale = V(east.localScale.x, head - foot, east.localScale.z); east.position += up * ((head + foot) * 0.5f - o);
    }
    // plank screens on both sides of every ramp (Wren 2026-10-03: 13 downhill sprint-jumps still went over the raised rails): upright boards
    // over a box each, on each rail's line, from the ramp's top screenH m plumb, every board pitch along the ramp
    const float screenH = 1.5f; int screenBoards = 0;
    foreach (var rn in new[] { "Ramp1", "Ramp2", "Ramp3", "Ramp4" })
    {
        var ramp = path2.Find(rn); var rc = ramp != null ? ramp.GetComponent<UnityEngine.Collider>() : null; if (rc == null) { notes.Add("no " + rn + " collider"); continue; }
        foreach (UnityEngine.Transform rail in path2)
        {
            if (rail.name != rn + "_Rail") continue; float rx = rail.position.x, inward = rx < rc.bounds.center.x ? 0.1f : -0.1f;
            float z0 = rc.bounds.min.z, z1 = rc.bounds.max.z; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt((z1 - z0) / (boardW + boardGap))); float pitch = (z1 - z0) / n;
            for (int k = 0; k < n; k++)
            {
                float z = z0 + (k + 0.5f) * pitch; if (!rc.Raycast(new UnityEngine.Ray(V(rx + inward, rc.bounds.max.y + 1f, z), UnityEngine.Vector3.down), out var hit, 40f)) continue;
                var c = V(rx, hit.point.y + screenH * 0.5f, z);
                Board(c, screenH, false); kit.Blocker("Screen", sk, sk.InverseTransformPoint(c), V(0.1f, screenH, pitch)); screenBoards++;
            }
        }
    }
    // and on every landing rail (8.25 round 3: two sprint-jumps down Ramp4 went over LandingN2's 1.0 m rails onto the screens' stepped
    // tops): a screen on each rail's line, from the landing's top screenH m
    foreach (UnityEngine.Transform rail in path2)
    {
        if (!rail.name.StartsWith("Landing") || !rail.name.Contains("_Rail")) continue; var rb = rail.GetComponent<UnityEngine.Collider>() != null ? rail.GetComponent<UnityEngine.Collider>().bounds : new UnityEngine.Bounds(rail.position, UnityEngine.Vector3.zero);
        bool alongX = rb.size.x >= rb.size.z; float len = alongX ? rb.size.x : rb.size.z; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(len / (boardW + boardGap))); float pitch = len / n;
        for (int k = 0; k < n; k++)
        {
            float u = -len * 0.5f + (k + 0.5f) * pitch; var c = V(rb.center.x + (alongX ? u : 0f), rb.min.y + screenH * 0.5f, rb.center.z + (alongX ? 0f : u));
            Board(c, screenH, alongX); kit.Blocker("Screen", sk, sk.InverseTransformPoint(c), alongX ? V(pitch, screenH, 0.1f) : V(0.1f, screenH, pitch)); screenBoards++;
        }
    }
    // the wedge between the stack and Ramp1, south of the z 112 skirt (Wren 2026-10-03, the 8.25 notch trap): a fill block of stack rock to
    // over the skirt's top, so nothing falls into it
    {
        var stoneMat = kit.Tinted("Places_WedgeFill", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#6E6A64"), UnityEngine.Vector2.one);
        const float wx0 = 296.28f, wx1 = 298.15f, wz0 = 107.8f, wz1 = 112.06f, overSkirt = 0.1f; float gy = G((wx0 + wx1) * 0.5f, (wz0 + wz1) * 0.5f), wy1 = gy + bareSkirt + overSkirt;
        kit.Slab("WedgeFill", L, L.InverseTransformPoint(V((wx0 + wx1) * 0.5f, (gy + wy1) * 0.5f - 0.5f, (wz0 + wz1) * 0.5f)), V(wx1 - wx0, wy1 - gy + 1f, wz1 - wz0), stoneMat, default, true);
        // and the corner south of it, behind the way-in planks (8.25 round 3: a 5-place pocket at (297.8, 5.6, 106.9) on the stack's foot), over
        // the planks' tops so none is stood on
        const float cx0 = 297.0f, cx1 = 298.26f, cz0 = 106.44f, cz1 = 107.8f;
        kit.Slab("WedgeFillCorner", L, L.InverseTransformPoint(V((cx0 + cx1) * 0.5f, (gy + wy1) * 0.5f - 0.5f, (cz0 + cz1) * 0.5f)), V(cx1 - cx0, wy1 - gy + 1f, cz1 - cz0), stoneMat, default, true);
    }
    // the blaze by position, not child index (Marlow 825 2)
    var blaze = cd.Find("StartBlaze"); if (blaze != null) blaze.position = V(304.4f, G(304.4f, 106.4f), 106.4f); else notes.Add("no StartBlaze");
}

// ================= C7, C8, C9 =================
{
    var ps = kit.Group("PaperSpots", L, L.position, 0f);
    foreach (var (n, x, z) in new[] { ("PS1", 288.14f, 109.6f), ("PS2", 288.8f, 111.2f), ("PS3", 283.70f, 107.90f) })   // PS3 moved 2.83 m from the doc's (285.7, 109.9), which no trail or walk line found (main3_8_25_search.cs, Wren 2026-10-02)
    {
        float y = G(x, z); foreach (var h in UnityEngine.Physics.RaycastAll(V(x, topY + 3f, z), UnityEngine.Vector3.down, 30f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!h.collider.isTrigger && h.collider.gameObject.layer != 2) y = UnityEngine.Mathf.Max(y, h.point.y);
        var m = kit.Marker(n, ps, ps.InverseTransformPoint(V(x, y, z)), 0f); kit.On(PlaceKit.CE + "Decoration_Home/Paper", m, V(0f, 0.01f, 0f), 25f, 0.5f, false, null, true);
    }
    // the phone wire: the pole's crossarm to a 1 m standoff on the booth roof, a parabola with wireSag at mid span, in wireSegs pieces
    const float wireSag = 1f, standoff = 1f; const int wireSegs = 24;
    var pole = kit.Root("PointsOfInterest") != null ? kit.Root("PointsOfInterest").transform.Find("POI_Phone_pole/Crossarm") : null; var roof = cd.Find("Payphone/BoothRoof");
    if (pole == null || roof == null) notes.Add("no phone pole crossarm or booth roof");
    else
    {
        var a = pole.position; var rb = roof.GetComponent<UnityEngine.Collider>().bounds; var b = V(rb.center.x, rb.max.y + standoff, rb.center.z);
        kit.Fill(PlaceKit.CI + "Building/CITW_Wood_Pillar", L, L.InverseTransformPoint(V(b.x, rb.max.y, b.z)), V(0.06f, standoff, 0.06f));
        var wire = kit.Group("PhoneWire", L, L.position, 0f); var wireMat = kit.Tinted("Places_Wire", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#151515"), UnityEngine.Vector2.one);
        UnityEngine.Vector3 W(float u) { var p = UnityEngine.Vector3.Lerp(a, b, u); p.y -= wireSag * 4f * u * (1f - u); return p; }
        for (int i = 0; i < wireSegs; i++) { var p = W(i / (float)wireSegs); var q = W((i + 1) / (float)wireSegs); var s = kit.Slab("Wire", wire, wire.InverseTransformPoint((p + q) * 0.5f), V(0.02f, 0.02f, UnityEngine.Vector3.Distance(p, q)), wireMat); s.transform.rotation = UnityEngine.Quaternion.LookRotation((q - p).normalized); }
    }
    // C9: Boulder_1 out of the booth's way, 1.91 m off it
    var bf = c2.Find("BoulderField"); UnityEngine.Transform b1 = null; if (bf != null) foreach (UnityEngine.Transform t in bf) if (t.name.StartsWith("Boulder_1") && (UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(302.52f, 97.74f)) < 0.5f || UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(303.4f, 97.9f)) < 0.5f)) b1 = t;
    if (b1 != null) { var d = P(303.4f, 97.9f) - P(b1.position.x, b1.position.z); b1.position += V(d.x, G(303.4f, 97.9f) - G(b1.position.x, b1.position.z), d.y); } else notes.Add("no BoulderField/Boulder_1");
    kit.ClearDetail(V(299.0f, 0f, 99.0f), 1.0f);   // the door lane
}

// ================= FOOD LOCKERS (forward fix, Wren 2026-10-02) =================
// main3_8_3_trails_giants.cs fits each of the six crates to lockerSize but its Fit stretched them to 15 m bars (scale x 19.98) that cross
// the T leg's tread; each Locker is set to lockerSize in its own axes from its mesh and given a box from that mesh (PlaceKit.FitExact)
var lockerSize = V(1f, 1.2f, 0.8f); int lockersFixed = 0;
{
    var poi = kit.Root("PointsOfInterest") != null ? kit.Root("PointsOfInterest").transform.Find("POI_Food_lockers") : null;
    if (poi == null) notes.Add("no POI_Food_lockers");
    else foreach (UnityEngine.Transform t in poi)
    {
        var mf = t.GetComponent<UnityEngine.MeshFilter>(); if (t.name != "Locker" || mf == null || mf.sharedMesh == null) continue; var mb = mf.sharedMesh.bounds.size;
        t.localScale = V(lockerSize.x / mb.x, lockerSize.y / mb.y, lockerSize.z / mb.z);
        var wb = PlaceKit.MeshBounds(t.gameObject); t.position += V(0f, G(t.position.x, t.position.z) - wb.min.y, 0f); PlaceKit.FitExact(t.gameObject); lockersFixed++;
    }
}

// ================= WARPS =================
var warps = kit.Root("DevWarps").transform;
foreach (var (n, x, y, z, yaw) in new[] { ("Camp_2", 294.3f, float.NaN, 95.0f, 55f), ("Camp_2_Top", 294.5f, topY + 0.2f, 107.2f, 20f) })
{ var w = warps.Find(n); if (w == null) { notes.Add("no warp " + n); continue; } w.position = V(x, float.IsNaN(y) ? G(x, z) + 0.2f : y, z); w.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); }

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | removed by name " + namedGone + " of " + named.Length + (namedMissing.Count > 0 ? " (not found, likely gone already: " + string.Join(", ", namedMissing) + ")" : "") + ", in keep-out zones " + zoneGone
    + " | rail runs " + railRuns + " | T leg points " + newPts + ", tread cells " + paintCells + ", skirt boards " + boardN + " | lockers refit " + lockersFixed + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
