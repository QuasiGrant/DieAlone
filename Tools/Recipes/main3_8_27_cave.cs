// Main3 task 8.27, the cave to CaveLayout.md draft 2 (Sable 2026-10-02, 6e94aba; CaveLayout_UI.md, CaveLayout_Story.md, CaveLayout_Sound.md,
// Marlow's 827 paper check with his S1 to S8 samples): layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (absolute
// places; new pieces rebuilt under Cave/Layout827, Ground815/Stops/Hedge_CaveRim and the spur's RopeRail827). In the runner after
// main3_8_26_camp3.cs and before main3_8_18a_solid.cs. Needs main3_8_27_warplabels.patch (Assets/Scripts/Dev/DevWarpLabels.cs) for the two
// new warps' rows in the dev panel; the warps themselves work without it.
// Props sit at yaw 0, 90, 180 or 270, or carry a box from their own meshes (PlaceKit.FitExact).
// REMOVALS (R), by name within removeTol m: the old rope rail POI (106.8, 57.6); TrailEdges CS_Stone_5 (53.22, 36.83) in the passage and
//   CS_Stone_8 (52.84, 41.94) in the mouth strip; ChamberDressing Boulder_1 (89.0, 5.51) (the seat shelf replaces it) and RubbleSparse_2
//   (82.86, 18.68) (under the battery bank); SideRoom Boulder_0 (97.50, 13.79) (in V9's opening).
// V1  the spur's rope rail on the drop side: posts railH over the tread every postStep m along the tread from P52 to P70 (W1 to cave), railOff m
//     south of the centre line (square to it), on their ground; three rope rails at railLevels; a box from the ground to the tops under each span.
// V2  the coloured bulbs (POI_Coloured_bulbs) moved to (82.15, 52.96) on the tread's ground; the dead branch keeps its own height.
// V3  the passage: the drip can (51.2, 28) at the west wall; the generator niche cut into the entrance's east wall, x 53.5 to 55.5,
//     z 31.6 to 33.4, floor -6, ceiling -4 (rock boxes, the old wall split round it), a cold generator and jugs in it; the cable, pale,
//     no collider and no light, from the niche along the east wall, round the inner walls of legs 1 to 3 and along the chamber's north wall
//     to the battery bank; the day-2 boards (a flat 0.3 m stack, x 50.55 to 51.35, z 34.0 to 37.2, off until the day system turns them on);
//     the load-down spot marker.
// V4  the toilet: a pit lid lidH high and a shovel lying flat at (47.64, 41.20).
// V5  the chamber: the seat shelf x 87.8 to 89.0, z 5.0 to 7.0, top 0.6 (rock); R7's spot on it facing the talk stand (86.9, 6.2), which
//     faces 98; the sleep (a mattress on a crate base, x 84 to 86, z 3.5 to 4.5, and a heater at (83.4, 4.0)); the food (a cooler and
//     crates, x 72.5 to 73.5, z 14.0 to 15.5); the stack (x 85 to 88.5, z 17 to 20.5, facing (80, 12)); the battery bank 1.0 x 0.6 flush to
//     its west side at the north wall (84.5, 20.5). Nothing over 0.3 m in z 10.8 to 13.2.
// V6  the side room: his chair to world yaw 270 (local 180 under RouletteTable's 90), keeping its convex hull; the crate at yaw 0 flush in
//     the north-west corner (89.75, 15.25), a box from its mesh; SideRoom BigBoulders_0 1.0 m east to (90.50, 9.51); the bulb string through
//     the doorway at bulbDoorH; the standing point (92.0, 12.0) facing 270.
// V9  the deeper passage: Wall_E_Future opens at z 13.2 to 14.4, deepH high, into x 97.5 to 101.5, turning north to a dead end at
//     (101.0, 15.5); a rock box (DeeperClosed) shuts it on other days; the inspect point (101.0, 14.0) facing 0.
// V10 the narrow: one row of owned BigBoulders on the bank side from x 72 to 61, their near faces narrowNear m north of the tread centre,
//     narrowH tall, convex hulls, continuous.
// V11 the rim band: a hedge box hedgeH tall (Ground815/Stops, a stop) with brush brushH tall, along the north wall's top, x 54 to 80 at the
//     rim's first ground over rimTop, and past x 54 to 80 on west to rimWest and east to rimEast along the face's top (round 2).
// WARPS: Cave_Mouth (58, 44) facing 223; Cave_Chamber (74, 12) facing 90; Cave_SideRoom (91.0, 12.0) facing 90 (new); Spur_Descent
//     (77.3, 48.6) facing 250 (new).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var notes = new System.Collections.Generic.List<string>();
var cave = kit.Root("Cave") != null ? kit.Root("Cave").transform : null; var poiRoot = kit.Root("PointsOfInterest") != null ? kit.Root("PointsOfInterest").transform : null; var trails = kit.Root("Trails") != null ? kit.Root("Trails").transform : null;
if (cave == null || poiRoot == null || trails == null || cave.Find("SideRoom") == null || cave.Find("ChamberDressing") == null) return "run 8.8 and 8.17 cave first (Cave, its SideRoom and ChamberDressing, PointsOfInterest, Trails)";
UnityEngine.Physics.SyncTransforms();
float G(float x, float z) => kit.H(x, z);
const float T = 0.5f, floorY = -18f;
const string rocks = "Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Materials/Rocks.mat", planksPath = "Assets/Materials/Planks023A_1.0x1.0.mat", concrete = "Assets/Materials/Concrete034_1.0x1.0.mat";
var rockMat = kit.Tinted("Places_CaveRock_2x2", rocks, Hex("#7A746C"), new UnityEngine.Vector2(2f, 2f)); var planks = kit.Tinted("Places_BoardPlank", planksPath, Hex("#6B5540"), new UnityEngine.Vector2(3f, 0.3f));
const string cableHex = "#9A968A"; const float cableGlow = 0.6f;
var rope = kit.Tinted("Places_Rope", planksPath, Hex("#8C7A58"), UnityEngine.Vector2.one); var cableMat = kit.Glow("Places_CableGlow", Hex(cableHex), cableGlow);   // unlit pale (round 2, Sable: the lit cable did not read on the passage wall; it reads cableOver grey or more, the cave frames)
var darkMat = kit.Tinted("Places_GearDark", concrete, Hex("#2A2C2E"), UnityEngine.Vector2.one); var greenMat = kit.Tinted("Places_Generator", concrete, Hex("#3E4A36"), UnityEngine.Vector2.one); var whiteMat = kit.Tinted("Places_Cooler", concrete, Hex("#C8CCC4"), UnityEngine.Vector2.one);
UnityEngine.GameObject Slab(string n, UnityEngine.Transform parent, UnityEngine.Vector3 world, UnityEngine.Vector3 size, UnityEngine.Material m, bool collide = false) => kit.Slab(n, parent, parent.InverseTransformPoint(world), size, m, default, collide);
UnityEngine.GameObject Rock(string n, UnityEngine.Transform parent, UnityEngine.Vector3 world, UnityEngine.Vector3 size) => Slab(n, parent, world, size, rockMat, true);
UnityEngine.Transform Child(UnityEngine.Transform parent, string name, float x, float z, float tol)
{
    UnityEngine.Transform best = null; float bd = tol;
    if (parent != null) foreach (var t in parent.GetComponentsInChildren<UnityEngine.Transform>(true)) { if (!(t.name == name || t.name.StartsWith(name + " ("))) continue; float dd = UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(x, z)); if (dd <= bd) { bd = dd; best = t; } }
    return best;
}
var L = kit.Fresh("Layout827", cave, cave.position, 0f);
// an axis-aligned box's world bounds from its transform (the cube's scale is its size), so a hidden original reads the same as a shown one
UnityEngine.Bounds BoxOf(UnityEngine.Transform t) => new UnityEngine.Bounds(t.position, t.lossyScale);
// the stand-in usables (Wren 2026-10-03, the standing prompt rule): ToggleColorInteractable with the CaveLayout_UI word on each interactable
int uses = 0; void Use(UnityEngine.GameObject g, string prompt, UnityEngine.Renderer target) { if (g == null) return; var u = g.GetComponent<ToggleColorInteractable>() ?? g.AddComponent<ToggleColorInteractable>(); var so = new UnityEditor.SerializedObject(u); so.FindProperty("prompt").stringValue = prompt; so.FindProperty("target").objectReferenceValue = target != null ? target : g.GetComponentInChildren<UnityEngine.Renderer>(); so.ApplyModifiedPropertiesWithoutUndo(); uses++; }

// ================= REMOVALS =================
const float removeTol = 0.3f; int removed = 0; var missing = new System.Collections.Generic.List<string>();
var ground815 = kit.Root("Ground815") != null ? kit.Root("Ground815").transform : null;
foreach (var (root, n, x, z) in new[] { (poiRoot, "POI_Rope_handrail", 106.8f, 57.6f), (ground815, "CS_Stone_5", 53.22f, 36.83f), (ground815, "CS_Stone_8", 52.84f, 41.94f),
    (cave.Find("ChamberDressing"), "Boulder_1", 89.0f, 5.51f), (cave.Find("ChamberDressing"), "RubbleSparse_2", 82.86f, 18.68f), (cave.Find("SideRoom"), "Boulder_0", 97.50f, 13.79f) })
{ var t = Child(root, n, x, z, n.StartsWith("POI_") ? 1.5f : removeTol); if (t != null) { PlaceKit.Remove(t); removed++; } else missing.Add(n + " (" + F(x) + ", " + F(z) + ")"); }
UnityEngine.Physics.SyncTransforms();

// the W1 to cave tread, its points in order
var leg = trails.Find("W1 to cave"); var tread = new System.Collections.Generic.List<(string name, UnityEngine.Vector3 p)>();
if (leg != null) foreach (UnityEngine.Transform p in leg) tread.Add((p.name, p.position)); else notes.Add("no Trails/W1 to cave");
int IndexOf(string n) { for (int i = 0; i < tread.Count; i++) if (tread[i].name == n) return i; return -1; }
// the tread's last point off the day-one board (round 2, Marlow: P84 sat inside a plank at z 37.6 and the W1 walk waited 200 s there): moved
// out to boardClear m past the board's outer face on the tread's own line
const float boardClear = 0.5f; string p84Note = "none";
{
    var board = cave.Find("Mouth/DayOneBoard"); int last = tread.Count - 1;
    if (board != null && last >= 1)
    {
        var bb = new UnityEngine.Bounds(); bool any = false; foreach (var r in board.GetComponentsInChildren<UnityEngine.Renderer>(true)) { if (!any) { bb = r.bounds; any = true; } else bb.Encapsulate(r.bounds); }
        var p = tread[last].p; bb.Expand(0.1f);
        if (any && bb.Contains(V(p.x, bb.center.y, p.z))) { var t = leg.Find(tread[last].name); var np = V(p.x, p.y, bb.max.z + boardClear); t.position = np; tread[last] = (tread[last].name, np); p84Note = tread[last].name + " moved to z " + F(np.z); }
    }
}

// ================= V1: the rope rail on the drop side =================
const float railH = 1.5f, postW = 0.1f, postStep = 2f, railOff = 1.2f, railT = 0.1f, railBar = 0.04f; var railLevels = new[] { 0.5f, 1.05f, 1.5f }; int posts = 0;   // round 2 (Wren 2026-10-03, Marlow: sprint-jumped at P58 to P68, open at P52 to P56): 1.5 m posts and rails from P52, as the Camp 2 screens
{
    var rail = kit.Fresh("RopeRail827", poiRoot, poiRoot.position, 0f); int i0 = IndexOf("P52"), i1 = IndexOf("P70");
    if (i0 < 0 || i1 <= i0) notes.Add("W1 to cave: no P52 to P70");
    else
    {
        // posts every postStep m along the tread polyline, each railOff m to its south (the drop's side), square to the tread
        var line = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (int i = i0; i <= i1; i++) line.Add(tread[i].p);
        float len = 0f; for (int i = 1; i < line.Count; i++) len += UnityEngine.Vector3.Distance(line[i - 1], line[i]); int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(len / postStep));
        UnityEngine.Vector3 At(float s, out UnityEngine.Vector3 dir) { float acc = 0f; for (int i = 1; i < line.Count; i++) { float l = UnityEngine.Vector3.Distance(line[i - 1], line[i]); dir = (line[i] - line[i - 1]).normalized; if (acc + l >= s || i == line.Count - 1) return UnityEngine.Vector3.Lerp(line[i - 1], line[i], UnityEngine.Mathf.Clamp01((s - acc) / UnityEngine.Mathf.Max(1e-4f, l))); acc += l; } dir = UnityEngine.Vector3.forward; return line[0]; }
        var feet = new System.Collections.Generic.List<UnityEngine.Vector3>(); var tops = new System.Collections.Generic.List<float>();
        for (int k = 0; k <= n; k++)
        {
            var q = At(len * k / n, out var dir); var side = V(-dir.z, 0f, dir.x).normalized; if (side.z > 0f) side = -side;   // the south side, where the drop is
            var f = q + side * railOff; f.y = G(f.x, f.z); feet.Add(f);
            // railH over the tread or the foot, whichever is higher: the drop side's low ground left the old rope 0.5 m over the tread
            float top = UnityEngine.Mathf.Max(f.y, q.y) + railH; tops.Add(top);
            Slab("Post", rail, V(f.x, (f.y + top) * 0.5f, f.z), V(postW, top - f.y, postW), planks, true); posts++;
        }
        for (int k = 1; k < feet.Count; k++)
        {
            var a = feet[k - 1]; var b = feet[k]; var mid = (a + b) * 0.5f; float l = UnityEngine.Vector3.Distance(a, b); var rot = UnityEngine.Quaternion.LookRotation(V(b.x - a.x, 0f, b.z - a.z).normalized);
            // three rope rails at railLevels under each post's top, and one box from the lower foot to the higher top
            foreach (var lv in railLevels) { var pa = V(a.x, tops[k - 1] - railH + lv, a.z); var pb = V(b.x, tops[k] - railH + lv, b.z); var r = Slab("Rope", rail, (pa + pb) * 0.5f, V(railBar, railBar, UnityEngine.Vector3.Distance(pa, pb)), rope); r.transform.rotation = UnityEngine.Quaternion.LookRotation((pb - pa).normalized); }
            float lo = UnityEngine.Mathf.Min(a.y, b.y), hi = UnityEngine.Mathf.Max(tops[k - 1], tops[k]);
            var box = kit.Blocker("RailBox", rail, rail.InverseTransformPoint(V(mid.x, (lo + hi) * 0.5f, mid.z)), V(railT, hi - lo, new UnityEngine.Vector2(b.x - a.x, b.z - a.z).magnitude)); box.transform.rotation = rot;
        }
    }
}

// ================= V2: the bulbs =================
const float bulbsOff = 1.8f, treeYaw = 40f, bulbTreeStep = 0.25f, treeMore = 2.5f, trunkClear = 1.5f, windLow = 1.2f, windHigh = 3.0f, windTurns = 3f, windOff = 0.03f, tipOut = 1.0f, tipLow = 2.6f, tipApart = 90f, tipOffLine = 0.6f, swag = 0.3f, bulbOverTread = 2.3f, bulbSize = 0.14f; const int bulbN = 14, windBulbsN = 6, windSegs = 36, wireSegs = 8; string bulbNote = "none";
{
    // beside the tread, bulbsOff m to its north (Wren 2026-10-03: V2 stood the dead branch on the centre line and its collider stopped the walk)
    var bulbsPoi = poiRoot.Find("POI_Coloured_bulbs"); var to = P(82.15f, 52.96f);
    { float bd = float.MaxValue; var side = P(0f, 1f); for (int i = 1; i < tread.Count; i++) { var a = P(tread[i - 1].p.x, tread[i - 1].p.z); var ab = P(tread[i].p.x, tread[i].p.z) - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(to - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); float d = UnityEngine.Vector2.Distance(to, a + ab * t); if (d < bd) { bd = d; side = P(-ab.y, ab.x).normalized; if (side.y < 0f) side = -side; } } to += side * bulbsOff; }
    if (bulbsPoi == null) notes.Add("no POI_Coloured_bulbs");
    else { var branch = bulbsPoi.Find("DeadBranch"); var foot = branch != null ? branch.position.y - branch.lossyScale.y : bulbsPoi.position.y;   // the cylinder's foot from its transform, so a hidden branch reads the same
           float rise = G(to.x, to.y) - foot; bulbsPoi.position += V(to.x - bulbsPoi.position.x, rise, to.y - bulbsPoi.position.z); }
    // ---- the bulb tree (CaveRock.md D, Vesper 8.27a): the dead tree, the hidden capsule and 8.3's grey string bar go; the map's own
    // Valley_Aspen1Leafless stands unscaled at yaw treeYaw on the bulbs' foot, its own collider, moved north (away from the drop) bulbTreeStep m
    // at a time, up to treeMore m, until its trunk is trunkClear m or more off the tread (CaveLayout 4).
    // the string (Day2String, off on day one; the day system turns it on, as the day-2 boards): bulbN bulbs, red, amber, green and blue in
    // turn. Run 1 winds the trunk from windLow to windHigh m in windTurns turns; runs 2 to 4 swag from the trunk at windHigh to the three
    // lowest branch tips (LOD0 vertices tipOut m or more off the trunk, tipLow m or more over the ground, tipApart degrees or more apart, none
    // over the tread line), sagging swag m. No bulb under bulbOverTread m over the tread; each bulbSize m, no collider, an unlit glow.
    if (bulbsPoi != null)
    {
        foreach (var n in new[] { "DeadTree", "DeadBranch", "BulbString", "BulbTree", "Day2String" }) PlaceKit.Remove(bulbsPoi.Find(n));
        float TreadDist(UnityEngine.Vector2 q) { float bd = float.MaxValue; for (int i = 1; i < tread.Count; i++) { var a = P(tread[i - 1].p.x, tread[i - 1].p.z); var ab = P(tread[i].p.x, tread[i].p.z) - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); bd = UnityEngine.Mathf.Min(bd, UnityEngine.Vector2.Distance(q, a + ab * t)); } return bd; }
        var foot2 = P(bulbsPoi.position.x, bulbsPoi.position.z); var north = P(0f, 1f);
        for (float more = 0f; more <= treeMore + 1e-3f && TreadDist(foot2) < trunkClear; more += bulbTreeStep) foot2 = P(bulbsPoi.position.x, bulbsPoi.position.z) + north * more;
        if (TreadDist(foot2) < trunkClear) notes.Add("bulb tree trunk " + F(TreadDist(foot2)) + " m off the tread after " + F(treeMore) + " m north");
        bulbsPoi.position = V(foot2.x, G(foot2.x, foot2.y), foot2.y);
        var tree = kit.Ground("Prefabs/Forest/Valley_Aspen1Leafless", bulbsPoi, foot2.x, foot2.y, treeYaw, 1f, false);
        if (tree == null) notes.Add("no Valley_Aspen1Leafless");
        else
        {
            tree.name = "BulbTree";
            // the trunk's axis and radius, and the branch tips, from the LOD0 meshes' vertices
            var lodg = tree.GetComponentInChildren<UnityEngine.LODGroup>(); var rs = lodg != null && lodg.GetLODs().Length > 0 ? lodg.GetLODs()[0].renderers : tree.GetComponentsInChildren<UnityEngine.Renderer>();
            var verts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (var r in rs) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) continue; foreach (var v in mf.sharedMesh.vertices) verts.Add(r.transform.TransformPoint(v)); }
            float gy = G(foot2.x, foot2.y); var axis = foot2; float trunkR = 0.15f;
            { var low = verts.FindAll(v => v.y > gy + windLow && v.y < gy + windLow + 0.5f); if (low.Count > 0) { var c = UnityEngine.Vector2.zero; foreach (var v in low) c += P(v.x, v.z); axis = c / low.Count; var ds = new System.Collections.Generic.List<float>(); foreach (var v in low) ds.Add(UnityEngine.Vector2.Distance(P(v.x, v.z), axis)); ds.Sort(); trunkR = ds[ds.Count / 2]; } }
            float treadY = tread.Count > 0 ? tread[0].p.y : gy; { float bd = float.MaxValue; foreach (var (n2, p) in tread) { float d = UnityEngine.Vector2.Distance(P(p.x, p.z), axis); if (d < bd) { bd = d; treadY = p.y; } } }
            var tips = new System.Collections.Generic.List<UnityEngine.Vector3>();
            foreach (var v in System.Linq.Enumerable.OrderBy(verts.FindAll(v => UnityEngine.Vector2.Distance(P(v.x, v.z), axis) >= tipOut && v.y >= gy + tipLow && v.y >= treadY + bulbOverTread && TreadDist(P(v.x, v.z)) >= tipOffLine), v => v.y))
            { float az = UnityEngine.Mathf.Atan2(v.z - axis.y, v.x - axis.x) * UnityEngine.Mathf.Rad2Deg; bool apart = true; foreach (var t in tips) if (UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(az, UnityEngine.Mathf.Atan2(t.z - axis.y, t.x - axis.x) * UnityEngine.Mathf.Rad2Deg)) < tipApart) apart = false; if (apart) tips.Add(v); if (tips.Count == 3) break; }
            if (tips.Count < 3) notes.Add("bulb tree: " + tips.Count + " branch tips found (3 wanted)");
            var str = new UnityEngine.GameObject("Day2String").transform; str.SetParent(bulbsPoi, true);
            var cols = new[] { Hex("#FF2A1E"), Hex("#FFA62B"), Hex("#3CFF5A"), Hex("#3C7CFF") }; var cn = new[] { "Red", "Amber", "Green", "Blue" };
            var mats = new UnityEngine.Material[4]; for (int i = 0; i < 4; i++) mats[i] = kit.Glow("Places_BulbGlow" + cn[i], cols[i], kit.Look.cabWindowGlowIntensity);
            var wire = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
            int nb = 0; void Bulb(UnityEngine.Vector3 p) { var b = Slab("Bulb", str, p, V(bulbSize, bulbSize, bulbSize), mats[nb % 4]); b.GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; nb++; }
            void Wire(UnityEngine.Vector3 a, UnityEngine.Vector3 b) { var w = Slab("Wire", str, (a + b) * 0.5f, V(0.015f, 0.015f, UnityEngine.Vector3.Distance(a, b)), wire); w.transform.rotation = UnityEngine.Quaternion.LookRotation((b - a).normalized); }
            // run 1: the wind round the trunk
            int windBulbs = windBulbsN; var prev = UnityEngine.Vector3.zero;
            for (int k = 0; k <= windSegs; k++) { float u = k / (float)windSegs, ang = u * windTurns * 360f * UnityEngine.Mathf.Deg2Rad; var p = V(axis.x + UnityEngine.Mathf.Cos(ang) * (trunkR + windOff), gy + UnityEngine.Mathf.Lerp(windLow, windHigh, u), axis.y + UnityEngine.Mathf.Sin(ang) * (trunkR + windOff)); if (k > 0) Wire(prev, p); prev = p; }
            for (int k = 0; k < windBulbs; k++) { float u = (k + 0.5f) / windBulbs, ang = u * windTurns * 360f * UnityEngine.Mathf.Deg2Rad; Bulb(V(axis.x + UnityEngine.Mathf.Cos(ang) * (trunkR + windOff + bulbSize * 0.5f), gy + UnityEngine.Mathf.Lerp(windLow, windHigh, u), axis.y + UnityEngine.Mathf.Sin(ang) * (trunkR + windOff + bulbSize * 0.5f))); }
            // runs 2 to 4: the swags, the bulbs left shared out
            var fork = V(axis.x, gy + windHigh, axis.y); int left = bulbN - windBulbs;
            for (int t = 0; t < tips.Count; t++)
            {
                int here = left / (tips.Count - t); left -= here; UnityEngine.Vector3 At(float u) => UnityEngine.Vector3.Lerp(fork, tips[t], u) - V(0f, swag * 4f * u * (1f - u), 0f);
                for (int s = 1; s <= wireSegs; s++) Wire(At((s - 1) / (float)wireSegs), At(s / (float)wireSegs));
                for (int k = 1; k <= here; k++) { var p = At(k / (here + 1f)); if (p.y < treadY + bulbOverTread) p.y = treadY + bulbOverTread; Bulb(p - V(0f, bulbSize * 0.6f, 0f)); }
            }
            str.gameObject.SetActive(false);
            bulbNote = "bulb tree at (" + F(foot2.x) + ", " + F(foot2.y) + "), trunk " + F(TreadDist(foot2)) + " m off the tread, " + nb + " bulbs on " + (1 + tips.Count) + " runs";
        }
    }
}

// ================= V3: the passage =================
const float nx0 = 53.5f, nx1 = 55.5f, nz0 = 31.6f, nz1 = 33.4f, nFloor = -6f, nCeil = -4f;
{
    // the niche: the entrance's east wall split round it, its own floor, ceiling, back and side walls
    var ent = cave.Find("Entrance"); UnityEngine.Transform eastWall = null; if (ent != null) foreach (UnityEngine.Transform t in ent) if (t.name == "Wall_E") { var b = BoxOf(t); if (b.min.z <= nz0 && b.max.z >= nz1) eastWall = t; }
    var niche = kit.Group("Niche", L, V((nx0 + nx1) * 0.5f, nFloor, (nz0 + nz1) * 0.5f), 0f);
    if (eastWall == null) notes.Add("no Entrance/Wall_E across the niche");
    if (eastWall != null)
    {
        var b = BoxOf(eastWall); float x = b.center.x, lo = b.min.y, hi = b.max.y;
        Rock("Wall_E_S", niche, V(x, (lo + hi) * 0.5f, (b.min.z + nz0) * 0.5f), V(b.size.x, hi - lo, nz0 - b.min.z));
        Rock("Wall_E_N", niche, V(x, (lo + hi) * 0.5f, (nz1 + b.max.z) * 0.5f), V(b.size.x, hi - lo, b.max.z - nz1));
        Rock("Wall_E_Over", niche, V(x, (nCeil + hi) * 0.5f, (nz0 + nz1) * 0.5f), V(b.size.x, hi - nCeil, nz1 - nz0));
        eastWall.gameObject.SetActive(false);   // kept, hidden: a rerun splits it again (Remove left a rerun nothing to split)
    }
    Rock("Floor", niche, V((nx0 + nx1) * 0.5f + T * 0.5f, nFloor - T * 0.5f, (nz0 + nz1) * 0.5f), V(nx1 - nx0 + T, T, nz1 - nz0 + 2f * T));
    Rock("Ceiling", niche, V((nx0 + nx1) * 0.5f + T * 0.5f, nCeil + T * 0.5f, (nz0 + nz1) * 0.5f), V(nx1 - nx0 + T, T, nz1 - nz0 + 2f * T));
    Rock("Back", niche, V(nx1 + T * 0.5f, (nFloor + nCeil) * 0.5f, (nz0 + nz1) * 0.5f), V(T, nCeil - nFloor + 2f * T, nz1 - nz0 + 2f * T));
    Rock("Side_S", niche, V((nx0 + nx1) * 0.5f, (nFloor + nCeil) * 0.5f, nz0 - T * 0.5f), V(nx1 - nx0, nCeil - nFloor, T));
    Rock("Side_N", niche, V((nx0 + nx1) * 0.5f, (nFloor + nCeil) * 0.5f, nz1 + T * 0.5f), V(nx1 - nx0, nCeil - nFloor, T));
    // the cold generator (a box) and the jugs beside it, the opening west
    var gen = Slab("Generator", niche, V(54.85f, nFloor + 0.25f, 32.2f), V(0.7f, 0.5f, 0.45f), greenMat, true);
    foreach (var (x, z) in new[] { (54.3f, 32.95f), (54.75f, 33.0f) }) kit.On(PlaceKit.CE + "Decoration_Out/Fuel_Can_Large", niche, niche.InverseTransformPoint(V(x, nFloor, z)), 0f, 1f, false, null, true);
    // the drip: a can against the west wall
    kit.On(PlaceKit.CS + "Food/CS_Food_Can_1_Open", L, L.InverseTransformPoint(V(51.2f, -6f, 28f)), 0f, 1.6f, false, null, true); kit.Marker("Drip", L, L.InverseTransformPoint(V(51.2f, -6f, 28f)), 0f);
    // the day-2 boards: a flat stack, off until the day system turns them on (the day-one board is solid on day 1)
    var boards = kit.Group("Day2Boards", L, V(50.95f, -6f, 35.6f), 0f);
    for (int i = 0; i < 3; i++) Slab("Board", boards, V(50.95f, -6f + 0.05f + i * 0.1f, 35.6f), V(0.8f, 0.08f, 3.2f), planks);
    boards.gameObject.SetActive(false);
    kit.Marker("LoadDown", L, L.InverseTransformPoint(V(52.7f, -6f, 36.0f)), 0f);
}
// the cable: pale, taped, no collider, no light; floor-level along the walls from the niche to the bank, each corner's floor height
// from the piece it stands in (8.8's legs fall 4 m each: leg 1 -6 to -10 eastward, leg 2 -10 to -14 westward, leg 3 -14 to -18 eastward)
const float cableUp = 0.05f, cableW = 0.04f; int cableRuns = 0;
{
    var c = new[] { V(53.35f, -6f, 31.5f), V(53.35f, -6f, 20.65f), V(66.5f, -10f, 20.65f), V(69.35f, -10f, 20.65f), V(69.35f, -10f, 18.35f), V(66.5f, -10f, 18.35f), V(53.5f, -14f, 18.35f), V(50.65f, -14f, 18.35f),
        V(50.65f, -14f, 13.35f), V(53.5f, -14f, 13.35f), V(66.5f, -18f, 13.35f), V(71.1f, -18f, 13.35f), V(71.1f, -18f, 20.85f), V(84.0f, -18f, 20.85f) };
    var cg = kit.Group("Cable", L, L.position, 0f);
    for (int i = 1; i < c.Length; i++) { var a = c[i - 1] + V(0f, cableUp, 0f); var b = c[i] + V(0f, cableUp, 0f); var s = Slab("Run", cg, (a + b) * 0.5f, V(cableW, 0.02f, UnityEngine.Vector3.Distance(a, b)), cableMat); s.transform.rotation = UnityEngine.Quaternion.LookRotation((b - a).normalized); s.GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; cableRuns++; }
}

// ================= V4: the toilet =================
const float lidH = 0.1f;
{
    var t = kit.Group("Toilet", L, V(47.64f, G(47.64f, 41.20f), 41.20f), 0f);
    Slab("PitLid", t, V(47.64f, G(47.64f, 41.20f) + lidH * 0.5f, 41.20f), V(0.6f, lidH, 0.6f), planks, true);
    kit.On(PlaceKit.FT + "Shovel", t, t.InverseTransformPoint(V(47.0f, G(47.0f, 41.4f), 41.4f)), 0f, 1f, false, V(0f, 0f, 90f), true);   // lying flat
}

// ================= V5: the chamber =================
const float r7BodyR = 0.3f, r7BodyH = 1.0f;
{
    var ch = kit.Group("Chamber", L, V(80f, floorY, 12f), 0f);
    // the seat shelf (rock) where Boulder_1 was, R7's spot on it, the talk stand
    Rock("SeatShelf", ch, V(88.4f, floorY + 0.3f, 6.0f), V(1.2f, 0.6f, 2.0f));
    var shelf = ch.Find("SeatShelf");
    var spot = cave.Find("Resident_Cave_Spot"); if (spot != null) { spot.position = V(88.4f, floorY + 0.6f, 6.0f); spot.rotation = UnityEngine.Quaternion.LookRotation(V(86.9f - 88.4f, 0f, 6.2f - 6.0f)); } else notes.Add("no Resident_Cave_Spot");
    // R7's stand-in body (no figure yet): a capsule r7BodyR by r7BodyH seated on the shelf, `Talk` (CaveLayout_UI 1.3); the shelf takes its colour
    if (spot != null) { PlaceKit.Remove(spot.Find("StandInBody")); var body = new UnityEngine.GameObject("StandInBody"); body.transform.SetParent(spot, false); body.transform.localPosition = V(0f, r7BodyH * 0.5f, 0f); var cap = body.AddComponent<UnityEngine.CapsuleCollider>(); cap.radius = r7BodyR; cap.height = r7BodyH; Use(body, "Talk", shelf != null ? shelf.GetComponent<UnityEngine.Renderer>() : null); }
    kit.Marker("TalkStand", ch, ch.InverseTransformPoint(V(86.9f, floorY, 6.2f)), 98f);
    // sleep: a mattress on a crate base, a heater
    var bed = kit.Group("Sleep", ch, V(85f, floorY, 4f), 0f);
    Slab("CrateBase", bed, V(85f, floorY + 0.15f, 4f), V(2f, 0.3f, 1f), planks); var mat = kit.Fill(PlaceKit.CE + "Furniture/Mattress", bed, bed.InverseTransformPoint(V(85f, floorY + 0.3f, 4f)), V(2f, 0.18f, 1f)); PlaceKit.FitExact(bed.gameObject);
    Slab("Heater", ch, V(83.4f, floorY + 0.25f, 4.0f), V(0.35f, 0.5f, 0.2f), darkMat, true);
    // food: a cooler and crates
    var food = kit.Group("Food", ch, V(73f, floorY, 14.75f), 0f);
    Slab("Cooler", food, V(73f, floorY + 0.25f, 14.35f), V(0.9f, 0.5f, 0.6f), whiteMat, true);
    Slab("Crate", food, V(73f, floorY + 0.2f, 15.15f), V(0.9f, 0.4f, 0.6f), planks, true);
    // the stack, its front toward (80, 12), and the battery bank flush to its west side at the north wall
    var stack = kit.Group("Stack", ch, V(86.75f, floorY, 18.75f), UnityEngine.Mathf.Atan2(80f - 86.75f, 12f - 18.75f) * UnityEngine.Mathf.Rad2Deg);
    for (int i = 0; i < 3; i++) kit.Slab("Speaker", stack, V(0f, 0.5f + i * 1.0f, 0f), V(1.6f, 0.95f, 0.9f), darkMat);
    PlaceKit.FitExact(stack.gameObject);
    Slab("BatteryBank", ch, V(84.5f, floorY + 0.4f, 20.7f), V(1.0f, 0.8f, 0.6f), darkMat, true);
}

// ================= V6: the side room =================
const float bulbDoorH = 2.05f, bulbChamberUp = 2.9f, bulbRoomUp = 2.8f, bbWestMin = 89.6f; int bulbs = 0;
{
    var room = cave.Find("SideRoom"); var tbl = room.Find("RouletteTable");
    // his chair: the chair on the far side (local z > 0) to local yaw 180, world 270
    UnityEngine.Transform his = null; if (tbl != null) foreach (UnityEngine.Transform t in tbl) if (t.name.StartsWith("Chair") && t.localPosition.z > 0f) his = t;
    if (his != null) { var c0 = PlaceKit.MeshBounds(his.gameObject).center; his.localRotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f); his.position += c0 - PlaceKit.MeshBounds(his.gameObject).center; } else notes.Add("no his chair under RouletteTable");
    // the guest chair (the near side, local z under 0), named GuestChair so its path is not his; `Sit` (CaveLayout_UI 1.5), a box from its
    // meshes when it has no collider
    UnityEngine.Transform guest = null; if (tbl != null) foreach (UnityEngine.Transform t in tbl) if ((t.name.StartsWith("Chair") || t.name == "GuestChair") && t.localPosition.z <= 0f) guest = t;
    if (guest != null) { guest.name = "GuestChair"; if (guest.GetComponentInChildren<UnityEngine.Collider>() == null) PlaceKit.FitExact(guest.gameObject); Use(guest.gameObject, "Sit", null); } else notes.Add("no guest chair under RouletteTable");
    // the crate at yaw 0, flush in the north-west corner, a box from its mesh
    var crate = Child(room, "C_Crate_Small_1", 96.6f, 14.6f, 1f) ?? Child(room, "C_Crate_Small_1", 89.75f, 15.25f, 1f);
    if (crate != null) { crate.rotation = UnityEngine.Quaternion.identity; var cb = PlaceKit.MeshBounds(crate.gameObject); crate.position += V(89.5f - cb.min.x, 0f, 15.5f - cb.max.z); PlaceKit.FitExact(crate.gameObject); } else notes.Add("no side-room crate");
    // BigBoulders_0 1.0 m east, out of the chamber
    var bb0 = Child(room, "BigBoulders_0", 89.50f, 9.51f, 0.3f) ?? Child(room, "BigBoulders_0", 90.50f, 9.51f, 1.5f);   // its mesh, not its pivot, clear of the chamber wall: west edge at bbWestMin (first run: 89.03)
    if (bb0 != null) { float west = PlaceKit.MeshBounds(bb0.gameObject).min.x; if (west < bbWestMin) bb0.position += V(bbWestMin - west, 0f, 0f); } else notes.Add("no SideRoom BigBoulders_0");
    // the bulb string: from the chamber through the doorway at bulbDoorH to over the table, two sagging runs
    foreach (var t in System.Linq.Enumerable.ToArray(room.GetComponentsInChildren<UnityEngine.Transform>())) if (t != null && (t.name == "BulbLine" || t.name == "Bulb")) PlaceKit.Remove(t);
    var bulbGlow = kit.Glow("Places_BulbGlow", kit.Look.practicalColor, kit.Look.cabWindowGlowIntensity); var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
    // the string over head wherever the ceiling allows (bulbs at bulbHigh or more), just under the lintel through the doorway (Wren
    // 2026-10-03: it has no collider; V6's 1.9 m hung bulbs at 1.81 to 1.98 m in the keep-clear strips)
    var pts = new[] { V(86.5f, floorY + bulbChamberUp, 12.3f), V(89.25f, floorY + bulbDoorH, 12.0f), V(96.8f, floorY + bulbRoomUp, 11.2f) }; const float sag = 0.2f; const int perRun = 5;
    for (int r = 1; r < pts.Length; r++)
    {
        var a = pts[r - 1]; var b = pts[r]; var ln = Slab("BulbLine", room, (a + b) * 0.5f - V(0f, sag * 0.5f, 0f), V(0.02f, 0.02f, UnityEngine.Vector3.Distance(a, b)), steel); ln.transform.rotation = UnityEngine.Quaternion.LookRotation(b - a);
        for (int i = 1; i <= perRun; i++) { float u = i / (perRun + 1f); var p = UnityEngine.Vector3.Lerp(a, b, u) - V(0f, sag * 4f * u * (1f - u) + 0.1f, 0f); Slab("Bulb", room, p, V(0.1f, 0.13f, 0.1f), bulbGlow).GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; bulbs++; }
    }
    kit.Marker("StandingPoint", L, L.InverseTransformPoint(V(92.0f, floorY, 12.0f)), 270f);
}

// ================= V9: the deeper passage =================
const float deadH = 2.0f, deadD = 0.3f, dz0 = 13.2f, dz1 = 14.4f, deepH = 2.1f, dxA0 = 97.5f, dxA1 = 101.6f, dxB0 = 100.4f, dzB1 = 16.1f;
{
    var room = cave.Find("SideRoom"); var fut = room.Find("Wall_E_Future"); var deep = kit.Group("Deeper", L, V(99.5f, floorY, 14f), 0f);
    if (fut != null)
    {
        var b = BoxOf(fut); float x = b.center.x, lo = b.min.y, hi = b.max.y;
        Rock("Wall_E_Future_S", deep, V(x, (lo + hi) * 0.5f, (b.min.z + dz0) * 0.5f), V(b.size.x, hi - lo, dz0 - b.min.z));
        Rock("Wall_E_Future_N", deep, V(x, (lo + hi) * 0.5f, (dz1 + b.max.z) * 0.5f), V(b.size.x, hi - lo, b.max.z - dz1));
        Rock("Wall_E_Future_Lintel", deep, V(x, (floorY + deepH + hi) * 0.5f, (dz0 + dz1) * 0.5f), V(b.size.x, hi - floorY - deepH, dz1 - dz0));
        fut.gameObject.SetActive(false);   // kept, hidden, as the niche's wall
    }
    else notes.Add("no SideRoom/Wall_E_Future");
    float xa0 = dxA0 + T; // the room's east wall face
    Rock("Floor", deep, V((xa0 + dxA1) * 0.5f, floorY - T * 0.5f, (dz0 + dzB1) * 0.5f), V(dxA1 - xa0 + T, T, dzB1 - dz0 + 2f * T));
    Rock("Ceiling", deep, V((xa0 + dxA1) * 0.5f, floorY + deepH + T * 0.5f, (dz0 + dzB1) * 0.5f), V(dxA1 - xa0 + T, T, dzB1 - dz0 + 2f * T));
    Rock("Wall_S", deep, V((xa0 + dxA1) * 0.5f, floorY + deepH * 0.5f, dz0 - T * 0.5f), V(dxA1 - xa0 + T, deepH, T));
    Rock("Wall_N_A", deep, V((xa0 + dxB0) * 0.5f, floorY + deepH * 0.5f, dz1 + T * 0.5f), V(dxB0 - xa0, deepH, T));
    Rock("Wall_W_B", deep, V(dxB0 - T * 0.5f, floorY + deepH * 0.5f, (dz1 + dzB1) * 0.5f + T * 0.5f), V(T, deepH, dzB1 - dz1 + T));
    Rock("Wall_E", deep, V(dxA1 + T * 0.5f, floorY + deepH * 0.5f, (dz0 + dzB1) * 0.5f), V(T, deepH, dzB1 - dz0 + 2f * T));
    Rock("Wall_End", deep, V((dxB0 + dxA1) * 0.5f, floorY + deepH * 0.5f, dzB1 + T * 0.5f), V(dxA1 - dxB0, deepH, T));
    Rock("DeeperClosed", deep, V(dxA0 + T * 0.5f, floorY + deepH * 0.5f, (dz0 + dz1) * 0.5f), V(T, deepH, dz1 - dz0));   // shut on other days; event 16 opens it
    kit.Marker("InspectPoint", deep, deep.InverseTransformPoint(V(101.0f, floorY, 14.0f)), 0f);
    // the dead end's face: a rock slab deadD deep on the end wall, `Examine` (CaveLayout_Story 23, Quill 2026-10-03; open days only, the
    // passage being shut on others); its face deadD nearer than the wall so the inspect point's ray meets it within reach
    var deadEnd = Rock("DeadEnd", deep, V((dxB0 + dxA1) * 0.5f, floorY + deadH * 0.5f, dzB1 - deadD * 0.5f), V(dxA1 - dxB0, deadH, deadD)); Use(deadEnd, "Examine", null);
}

// ================= V10: the narrow =================
const float narrowOver = 3.0f, narrowNear = 1.5f, narrowH = 4f, narrowStep = 2.6f, narrowX0 = 61f, narrowX1 = 72f; int narrowRocks = 0;
{
    var row = kit.Group("NarrowRow", L, L.position, 0f); var line = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (var (n, p) in tread) if (p.x >= narrowX0 - 1f && p.x <= narrowX1 + 1f) line.Add(p);
    line.Sort((a, b) => b.x.CompareTo(a.x));
    if (line.Count < 2) notes.Add("W1 to cave: no tread from x 72 to 61");
    else
    {
        float len = 0f; for (int i = 1; i < line.Count; i++) len += UnityEngine.Vector3.Distance(line[i - 1], line[i]); int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(len / narrowStep));
        for (int k = 0; k <= n; k++)
        {
            float s = len * k / n, acc = 0f; var q = line[line.Count - 1]; var dir = (line[line.Count - 1] - line[line.Count - 2]).normalized;
            for (int i = 1; i < line.Count; i++) { float l = UnityEngine.Vector3.Distance(line[i - 1], line[i]); if (acc + l >= s) { q = UnityEngine.Vector3.Lerp(line[i - 1], line[i], (s - acc) / UnityEngine.Mathf.Max(1e-4f, l)); dir = (line[i] - line[i - 1]).normalized; break; } acc += l; }
            var north = V(-dir.z, 0f, dir.x).normalized; if (north.z < 0f) north = -north;
            var g = kit.Spawn(PlaceKit.BK + "Rocks/BigBoulders_" + (k % 6), row); if (g == null) continue; PlaceKit.StripColliders(g);
            g.transform.rotation = UnityEngine.Quaternion.Euler(0f, q.x * 37f, 0f); var b0 = PlaceKit.MeshBounds(g); g.transform.localScale *= narrowH / UnityEngine.Mathf.Max(0.1f, b0.size.y);
            var b = PlaceKit.MeshBounds(g); float back = UnityEngine.Mathf.Abs(north.x) * b.extents.x + UnityEngine.Mathf.Abs(north.z) * b.extents.z;   // the box's half-depth across the tread
            var at = q + north * (narrowNear + back); g.transform.position += V(at.x - b.center.x, G(at.x, at.z) - 0.3f - b.min.y, at.z - b.center.z);
            // its top at least narrowOver over the tread (first run: east-end tops 1.1 m over the tread made a ledge with no way back, a trap):
            // scaled up about its foot, then set off again by its new half-depth
            { var nb = PlaceKit.MeshBounds(g); float want = q.y + narrowOver; if (nb.max.y < want) { g.transform.localScale *= (want - nb.min.y) / UnityEngine.Mathf.Max(0.1f, nb.size.y); var sb2 = PlaceKit.MeshBounds(g); float back2 = UnityEngine.Mathf.Abs(north.x) * sb2.extents.x + UnityEngine.Mathf.Abs(north.z) * sb2.extents.z; var at2 = q + north * (narrowNear + back2); g.transform.position += V(at2.x - sb2.center.x, nb.min.y - sb2.min.y, at2.z - sb2.center.z); } }
            foreach (var r in g.GetComponentsInChildren<UnityEngine.MeshRenderer>()) { var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); if (lodg != null && lodg.GetLODs().Length > 0 && System.Array.IndexOf(lodg.GetLODs()[0].renderers, r) < 0) continue; var mf = r.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; }
            narrowRocks++;
        }
    }
}

// ================= V11: the rim band =================
// hedgeT 1.2 (first run 0.6): the thin boxes left a one-place pocket where two met at an angle, (71.8, 57.8), a trap
const float rimWest = 26f, rimEast = 92f, rimLook = 6f, rimBelowTop = 1f, rimUp = 3f, rimUpStep = 0.5f, rimX0 = 54f, rimX1 = 80f, rimStep = 1f, rimZ0 = 55f, rimZ1 = 62f, rimTop = 15.5f, hedgeH = 2f, hedgeT = 1.2f, brushH = 1.5f, brushStep = 1.4f; int hedgeBoxes = 0, brush = 0;
{
    var stops = ground815 != null ? ground815.Find("Stops") : null; if (stops == null) { notes.Add("no Ground815/Stops"); }
    else
    {
        var hedge = kit.Fresh("Hedge_CaveRim", stops, V((rimWest + rimEast) * 0.5f, 0f, 58.5f), 0f);
        // a box's top hedgeH over the highest ground rimUp m north of it (uphill): the flood stood on boxes whose top was hedgeH over the
        // face's own lip (8.27 first run: 78 stands on Hedge_CaveRim)
        float Uphill(UnityEngine.Vector3 p) { float m = p.y; for (float dz = 0f; dz <= rimUp + 1e-3f; dz += rimUpStep) m = UnityEngine.Mathf.Max(m, G(p.x, p.z + dz)); return m; }
        // the band's line: at each x, the first z (going north from rimZ0) where the ground reaches rimTop, the top of the steep face
        var line = new System.Collections.Generic.List<UnityEngine.Vector3>();
        // past x rimX0 to rimX1 (where the face tops out at rimTop) the band runs on west to rimWest and east to rimEast (Wren 2026-10-03,
        // Marlow's 8.27 gate: open ends let a sprint slide down the north wall to the mouth): there its line is the first z north of rimZ0
        // within rimBelowTop of the highest ground up to rimLook m past rimZ1
        for (float x = rimWest; x <= rimEast + 1e-3f; x += rimStep)
        {
            float zz = rimZ1;
            if (x >= rimX0 && x <= rimX1) { for (float z = rimZ0; z <= rimZ1; z += 0.25f) if (G(x, z) >= rimTop) { zz = z; break; } }
            else { float top = float.MinValue; for (float z = rimZ0; z <= rimZ1 + rimLook; z += 0.25f) top = UnityEngine.Mathf.Max(top, G(x, z)); for (float z = rimZ0; z <= rimZ1 + rimLook; z += 0.25f) if (G(x, z) >= top - rimBelowTop) { zz = z; break; } }
            line.Add(V(x, G(x, zz), zz));
        }
        for (int i = 1; i < line.Count; i++)
        {
            var a = line[i - 1]; var b = line[i]; var mid = (a + b) * 0.5f; float l = new UnityEngine.Vector2(b.x - a.x, b.z - a.z).magnitude; float lo = UnityEngine.Mathf.Min(a.y, b.y) - 0.3f, hi = UnityEngine.Mathf.Max(Uphill(a), Uphill(b)) + hedgeH;
            var box = kit.Blocker("HedgeCollider", hedge, hedge.InverseTransformPoint(V(mid.x, (lo + hi) * 0.5f, mid.z)), V(hedgeT, hi - lo, l + 0.1f)); box.transform.rotation = UnityEngine.Quaternion.LookRotation(V(b.x - a.x, 0f, b.z - a.z).normalized); box.layer = 2; hedgeBoxes++;
        }
        float lastB = -brushStep; float run = 0f;
        for (int i = 1; i < line.Count; i++)
        {
            run += new UnityEngine.Vector2(line[i].x - line[i - 1].x, line[i].z - line[i - 1].z).magnitude; if (run - lastB < brushStep) continue; lastB = run;
            var g = kit.Ground(PlaceKit.CS + "Vegetation/CS_Bush_Large_" + (1 + brush % 2), hedge, line[i].x, line[i].z, i * 53f, 1f, false, 0.1f); if (g == null) continue;
            var gb = PlaceKit.MeshBounds(g); g.transform.localScale *= brushH / UnityEngine.Mathf.Max(0.1f, gb.size.y); PlaceKit.StripColliders(g); brush++;
        }
    }
}

// ================= V13: the cave reads as rock (round 2, Vesper and Sable, Wren 2026-10-03) =================
// the void: the near-black void material only on the first voidLen m inside the mouth (z voidZ0 to the mouth); the rest of the entrance
// passage takes the cave rock. The entrance's Floor, Ceiling and Wall_W span both, so each is hidden and built again as two boxes under
// Layout827/VoidSplit (rerun-safe: the originals are read from their transforms); every niche face takes the rock.
// the chamber floor: its own seamless ground material (Ground054; the cave rock texture is not seamless, and its tile edge ran down the walk
// line), tinted floorHex, tiled floorTile with an offset. The pack boulders take the cave's tint (boulderHex): untinted they read pale and floating.
// rock as the wall: CaveRock.md draft 1 (Vesper, 8.27a), 51 placed boulders and 5 rubble patches under Layout827/CaveRock, in place of
// round 2's RockBreakup; see the block for the method.
const float voidZ0 = 33.4f, floorTile = 3f, floorOffX = 0.21f, floorOffZ = 0.37f, hullUnder = 2.5f, rubbleW = 3f, rubbleH = 0.25f, stripZ0 = 10.8f, deadFront = 0.05f, shutS = 1.6f; int rockPieces = 0;
// the NARROWS pieces' depths and switches (CaveRock.md: if a check fails, cut d to 0.1, then drop the piece; never move it)
const float p1d = 0.8f, p2d = 0.4f, p4d = 0.4f, l14d = 0.4f, v1d = 0.1f, v2d = 0.1f, v3d = 0.1f, v5d = 0.1f; const float walkClear = 2.0f, jambTopD = 0.3f; const bool v1On = false, v2On = false, v3On = true, v5On = true, v6On = true; const string groundPath = "Assets/Materials/Ground054_25.0x25.0.mat", floorHex = "#3E3A36", boxHex = "#3E3A36", boulderHex = "#6E6862";
{
    var voidMat = kit.Tinted("Places_CaveVoid", rocks, Hex("#101214"), UnityEngine.Vector2.one);
    var vs = kit.Group("VoidSplit", L, L.position, 0f); var ent2 = cave.Find("Entrance");
    foreach (var n in new[] { "Floor", "Ceiling", "Wall_W" })
    {
        var t = ent2 != null ? ent2.Find(n) : null; if (t == null) { notes.Add("no Entrance/" + n); continue; }
        var b = BoxOf(t); t.gameObject.SetActive(false);
        if (b.max.z <= voidZ0 || b.min.z >= voidZ0) { var whole = Slab(n, vs, b.center, b.size, b.min.z >= voidZ0 ? voidMat : rockMat, true); continue; }
        Slab(n + "_Rock", vs, V(b.center.x, b.center.y, (b.min.z + voidZ0) * 0.5f), V(b.size.x, b.size.y, voidZ0 - b.min.z), rockMat, true);
        Slab(n + "_Void", vs, V(b.center.x, b.center.y, (voidZ0 + b.max.z) * 0.5f), V(b.size.x, b.size.y, b.max.z - voidZ0), voidMat, true);
    }
    // every other entrance box (the south end wall, the short east wall by leg 1): void only if it lies wholly in the first voidLen m
    if (ent2 != null) foreach (UnityEngine.Transform t in ent2) if (t.gameObject.activeSelf) { var r = t.GetComponent<UnityEngine.Renderer>(); if (r != null) r.sharedMaterial = BoxOf(t).min.z >= voidZ0 ? voidMat : rockMat; }
    // the niche's faces all take the cave rock (CaveRock.md A: a void face read as flat dark wall, not a recess)
    var wen = L.Find("Niche/Wall_E_N"); if (wen != null) wen.GetComponent<UnityEngine.Renderer>().sharedMaterial = rockMat;
    var cf = cave.Find("Chamber/Floor"); if (cf != null) { var fm = kit.Tinted("Places_CaveFloor_Chamber", groundPath, Hex(floorHex), new UnityEngine.Vector2(floorTile, floorTile)); fm.SetTextureOffset("_BaseMap", new UnityEngine.Vector2(floorOffX, floorOffZ)); cf.GetComponent<UnityEngine.Renderer>().sharedMaterial = fm; } else notes.Add("no Chamber/Floor");
    // the box falls back into shadow (Wren 2026-10-03, past the 8.27a cap): every Places_CaveRock_ material (8.17's walls, ceilings and floors,
    // this recipe's boxes) and the chamber floor tinted boxHex, the boulders kept at boulderHex so the rock reads as the lighter forms; the void stays
    foreach (var guid in UnityEditor.AssetDatabase.FindAssets("Places_CaveRock_ t:Material", new[] { "Assets/Materials/Places" })) { var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)); if (m == null || !m.name.StartsWith("Places_CaveRock_")) continue; m.SetColor("_BaseColor", Hex(boxHex)); UnityEditor.EditorUtility.SetDirty(m); }
    // ---- CaveRock.md draft 1 (Vesper, 2026-10-03; PLAN 8.27a): the rock is the wall. Each piece: the prefab turned to yaw, scaled so its mesh
    // bounds' largest side is S, then placed by its bounds: along the wall at its along coordinate, its centre at y (or its bottom at the given
    // bottom), and its face nearest the room d m inside the room from the wall face (a corner piece: from both faces). minBottom raises a
    // piece whose bottom would sit lower (the cable, the stack). Pieces whose bottom is under hullUnder m over the local floor get convex hulls
    // from LOD0; the rest have none. NARROWS pieces are judged by the area and cave checks (cut d to 0.1, then drop; never moved).
    var rg = kit.Group("CaveRock", L, L.position, 0f); var boulderMat = kit.Tinted("Places_CaveBoulder", rocks, Hex(boulderHex), UnityEngine.Vector2.one);
    void Tint(UnityEngine.GameObject g) { foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) { var ms = r.sharedMaterials; for (int i = 0; i < ms.Length; i++) ms[i] = boulderMat; r.sharedMaterials = ms; } }
    // a piece's true extent: its LOD0 meshes' vertices in world space (a renderer's bounds box the turned mesh box and grow with the yaw, so a
    // rock d m into the room by them sat inside the wall: Vesper's 8.27a gate, the wall rocks did not show)
    UnityEngine.Bounds Exact(UnityEngine.GameObject g) { var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); var rs = lodg != null && lodg.GetLODs().Length > 0 ? lodg.GetLODs()[0].renderers : g.GetComponentsInChildren<UnityEngine.Renderer>(); bool any = false; var b = new UnityEngine.Bounds(); foreach (var r in rs) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) continue; foreach (var v in mf.sharedMesh.vertices) { var w = r.transform.TransformPoint(v); if (!any) { b = new UnityEngine.Bounds(w, UnityEngine.Vector3.zero); any = true; } else b.Encapsulate(w); } } return any ? b : PlaceKit.MeshBounds(g); }
    UnityEngine.GameObject Piece(string id, string pf, float S, float yaw)
    {
        var g = kit.Spawn(PlaceKit.BK + "Rocks/" + pf, rg); if (g == null) { notes.Add("no " + pf); return null; } g.name = id + "_" + pf; PlaceKit.StripColliders(g); Tint(g);
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); var b0 = Exact(g); g.transform.localScale *= S / UnityEngine.Mathf.Max(0.1f, UnityEngine.Mathf.Max(b0.size.x, UnityEngine.Mathf.Max(b0.size.y, b0.size.z)));
        return g;
    }
    void Finish(UnityEngine.GameObject g, float floor, float minBottom, bool solidRule)
    {
        var b = Exact(g); if (!float.IsNaN(minBottom) && b.min.y < minBottom) g.transform.position += V(0f, minBottom - b.min.y, 0f);
        b = Exact(g);
        if (solidRule && b.min.y < floor + hullUnder) { var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); foreach (var r in g.GetComponentsInChildren<UnityEngine.MeshRenderer>()) { if (lodg != null && lodg.GetLODs().Length > 0 && System.Array.IndexOf(lodg.GetLODs()[0].renderers, r) < 0) continue; var mf = r.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; } }
        rockPieces++;
    }
    float Y(UnityEngine.Bounds b, float yc, float yb) => !float.IsNaN(yb) ? yb - b.min.y : yc - b.center.y;
    // a wall piece: axis 'x' (the face is x = face, along runs in z) or 'z' (the face is z = face, along runs in x); inward +1 or -1
    UnityEngine.GameObject Wall(string id, string pf, float S, float yaw, char axis, float face, float inward, float along, float yc, float yb, float d, float floor, float minBottom = float.NaN, bool solidRule = true)
    {
        var g = Piece(id, pf, S, yaw); if (g == null) return null; var b = Exact(g); float dy = Y(b, yc, yb);
        if (axis == 'x') { float nearX = inward > 0f ? b.max.x : b.min.x; g.transform.position += V(face + inward * d - nearX, dy, along - b.center.z); }
        else { float nearZ = inward > 0f ? b.max.z : b.min.z; g.transform.position += V(along - b.center.x, dy, face + inward * d - nearZ); }
        Finish(g, floor, minBottom, solidRule); return g;
    }
    UnityEngine.GameObject Corner(string id, string pf, float S, float yaw, float faceX, float inX, float faceZ, float inZ, float yc, float d, float floor, float minBottom = float.NaN)
    {
        var g = Piece(id, pf, S, yaw); if (g == null) return null; var b = Exact(g);
        float nearX = inX > 0f ? b.max.x : b.min.x, nearZ = inZ > 0f ? b.max.z : b.min.z; g.transform.position += V(faceX + inX * d - nearX, yc - b.center.y, faceZ + inZ * d - nearZ);
        Finish(g, floor, minBottom, true); return g;
    }
    UnityEngine.GameObject Ceil(string id, string pf, float S, float yaw, float x, float z, float bottom, float floor)
    {
        var g = Piece(id, pf, S, yaw); if (g == null) return null; var b = Exact(g); g.transform.position += V(x - b.center.x, bottom - b.min.y, z - b.center.z);
        Finish(g, floor, float.NaN, true); return g;
    }
    const float N = float.NaN, fE = -6f, fC = -18f;
    // A. entrance passage (box x 50.5 to 53.5, z 20.5 to 37.4, floor -6, ceiling -2)
    Corner("P1", "BigBoulders_1", 4.0f, 20f, 50.5f, 1f, 20.5f, 1f, -4.2f, p1d, fE);
    { var p2 = Wall("P2", "BigBoulders_5", 2.6f, 330f, 'x', 53.5f, -1f, 24.6f, -4.4f, N, p2d, fE); if (p2 != null) { var b = Exact(p2); if (b.min.z < 23.2f) p2.transform.position += V(0f, 0f, 23.2f - b.min.z); } }
    Ceil("P3", "Boulder_2", 3.5f, 0f, 54.25f, 22.0f, -2.8f, fE);
    Wall("P4", "BigBoulders_2", 3.0f, 70f, 'z', 20.5f, 1f, 54.5f, -5.0f, N, p4d, fE);
    Wall("P5", "BigBoulders_3", 4.0f, 110f, 'x', 50.5f, 1f, 25.0f, -4.0f, N, 0.4f, fE);
    Wall("P6", "BigBoulders_4", 4.0f, 250f, 'x', 50.5f, 1f, 31.0f, -4.0f, N, 0.4f, fE);
    Wall("P7", "BigBoulders_0", 3.0f, 200f, 'x', 53.5f, -1f, 30.0f, -4.5f, N, 0.4f, fE, -5.6f);
    Ceil("P8", "Boulder_4", 3.5f, 160f, 52.0f, 24.5f, -2.5f, fE); Ceil("P9", "Boulder_1", 3.5f, 300f, 52.0f, 29.5f, -2.5f, fE); Ceil("P10", "Boulder_2", 3.0f, 40f, 52.0f, 32.5f, -2.4f, fE);
    // B1. chamber low band (box x 71 to 89, z 3 to 21, floor -18, ceiling -10)
    Wall("L1", "BigBoulders_1", 6.0f, 15f, 'z', 3f, 1f, 74.0f, -15.8f, N, 1.0f, fC); Wall("L2", "BigBoulders_3", 5.5f, 95f, 'z', 3f, 1f, 78.5f, -16.0f, N, 0.9f, fC);
    Wall("L3", "Boulder_2", 5.0f, 200f, 'z', 3f, 1f, 82.8f, -16.2f, N, 0.3f, fC); Wall("L4", "BigBoulders_4", 6.0f, 140f, 'z', 3f, 1f, 87.6f, -15.8f, N, 0.3f, fC);
    Wall("L5", "BigBoulders_2", 6.0f, 220f, 'x', 71f, 1f, 6.0f, -15.8f, N, 1.0f, fC); Wall("L6", "BigBoulders_5", 4.5f, 300f, 'x', 71f, 1f, 8.0f, -16.3f, N, 0.8f, fC);
    Wall("L7", "BigBoulders_0", 4.5f, 30f, 'x', 71f, 1f, 15.8f, -16.3f, N, 0.4f, fC); Wall("L8", "BigBoulders_3", 6.0f, 175f, 'x', 71f, 1f, 19.2f, -15.8f, N, 0.9f, fC);
    Wall("L9", "BigBoulders_1", 6.0f, 250f, 'z', 21f, -1f, 75.5f, -15.6f, N, 0.2f, fC, -17.4f); Wall("L10", "BigBoulders_4", 5.5f, 35f, 'z', 21f, -1f, 80.5f, -15.8f, N, 0.2f, fC, -17.4f);
    Corner("L11", "BigBoulders_3", 6.0f, 175f, 89f, -1f, 21f, -1f, -14.8f, 0.2f, fC, -15.0f);
    Wall("L12", "BigBoulders_0", 5.0f, 60f, 'x', 89f, -1f, 6.0f, N, -17.4f, 0.3f, fC);
    Wall("L13", "BigBoulders_5", 4.5f, 280f, 'x', 89f, -1f, 8.4f, -16.3f, N, 0.6f, fC); Wall("L14", "BigBoulders_2", 4.5f, 45f, 'x', 89f, -1f, 15.6f, -16.3f, N, l14d, fC);
    { var l15 = Wall("L15", "BigBoulders_1", 3.0f, 10f, 'x', 89f, -1f, 9.3f, -16.6f, N, 0.3f, fC); if (l15 != null) { var b = Exact(l15); if (b.max.z > stripZ0) l15.transform.position += V(0f, 0f, stripZ0 - b.max.z); } }
    // B2. high band and lintels (over reach)
    Wall("H1", "Boulder_0", 7.0f, 0f, 'z', 3f, 1f, 75.5f, -12.0f, N, 0.8f, fC); Wall("H2", "Boulder_3", 7.0f, 180f, 'z', 3f, 1f, 83.5f, -11.8f, N, 0.8f, fC);
    Wall("H3", "Boulder_1", 7.0f, 90f, 'x', 71f, 1f, 5.5f, -11.5f, N, 0.8f, fC); Wall("H4", "Boulder_4", 5.0f, 90f, 'x', 71f, 1f, 12.0f, N, -14.3f, 0.5f, fC);
    Wall("H5", "Boulder_5", 7.0f, 270f, 'x', 71f, 1f, 18.5f, -11.5f, N, 0.8f, fC); Wall("H6", "Boulder_2", 7.0f, 0f, 'z', 21f, -1f, 76.0f, -11.5f, N, 0.9f, fC);
    Wall("H7", "Boulder_0", 7.0f, 180f, 'z', 21f, -1f, 84.0f, -11.5f, N, 0.9f, fC, -13.8f); Wall("H8", "Boulder_5", 7.0f, 90f, 'x', 89f, -1f, 6.0f, -11.5f, N, 0.8f, fC, -14.2f);
    Wall("H9", "Boulder_3", 4.5f, 90f, 'x', 89f, -1f, 12.0f, N, -15.5f, 0.4f, fC); Wall("H10", "Boulder_1", 7.0f, 270f, 'x', 89f, -1f, 18.0f, -11.5f, N, 0.8f, fC);
    // B3. ceiling
    Ceil("C1", "Boulder_0", 8.0f, 75f, 75.5f, 7.0f, -11.0f, fC); Ceil("C2", "Boulder_2", 8.0f, 10f, 82.5f, 7.5f, -11.2f, fC); Ceil("C3", "Boulder_4", 7.0f, 130f, 76.5f, 16.5f, -10.8f, fC);
    Ceil("C4", "Boulder_1", 8.0f, 200f, 84.0f, 16.0f, -11.0f, fC); Ceil("C5", "Boulder_3", 6.0f, 45f, 80.0f, 12.0f, -10.6f, fC);
    // B4. floor: rubble flattened to rubbleH at the wall feet, no collider
    {
        var rubble = new (string pf, float x, float z, float yaw)[] { ("RubbleSparse_1", 73.2f, 5.0f, 0f), ("RubbleSparse_2", 79.0f, 4.4f, 70f), ("RubbleSparse_3", 72.4f, 8.6f, 140f), ("RubbleSparse_1", 77.5f, 18.9f, 210f), ("RubbleSparse_2", 82.0f, 18.6f, 280f) };
        foreach (var (pf, x, z, yaw) in rubble) { var g = kit.Fill(PlaceKit.BK + "Rocks/" + pf, rg, rg.InverseTransformPoint(V(x, fC, z)), V(rubbleW, rubbleH, rubbleW), yaw); if (g != null) { Tint(g); rockPieces++; } }
    }
    // C. V9: jambs, lintel, the dead end's rock face, the turn, the deeper passage's lid
    if (v1On) Wall("V1", "BigBoulders_2", 2.4f, 30f, 'x', 97.5f, -1f, 12.4f, -16.8f, N, v1d, fC);
    if (v2On) Wall("V2", "BigBoulders_4", 2.4f, 200f, 'x', 97.5f, -1f, 15.2f, -16.8f, N, v2d, fC);
    // the jambs' tops (Wren 2026-10-03, past the cap: rock over the lintel and on the jambs' tops only, nothing under walkClear m in the doorway;
    // V1 and V2 at full height closed it): the same rocks with their bottoms walkClear over the floor
    Wall("V1T", "BigBoulders_2", 2.4f, 30f, 'x', 97.5f, -1f, 12.4f, N, fC + walkClear, jambTopD, fC); Wall("V2T", "BigBoulders_4", 2.4f, 200f, 'x', 97.5f, -1f, 15.2f, N, fC + walkClear, jambTopD, fC);
    if (v3On) Wall("V3", "Boulder_3", 3.0f, 90f, 'x', 97.5f, -1f, 13.8f, N, -16.0f, v3d, fC);
    var v4 = Wall("V4", "Boulder_5", 2.5f, 0f, 'z', 15.8f, -1f, 101.0f, fC + deepH * 0.5f, N, 0.15f, fC, float.NaN, false);
    if (v5On) Corner("V5", "BigBoulders_0", 2.0f, 120f, 101.6f, -1f, 13.2f, 1f, -17.0f, v5d, fC);
    if (v6On) Ceil("V6", "Boulder_1", 3.0f, 0f, 99.5f, 13.8f, fC + walkClear, fC);   // walkClear over the floor (was 1.95; Wren 2026-10-03)
    // the dead end slab: renderer off, its face moved deadFront m in front of V4's nearest point (the rock is its visible reason; Examine stays)
    { var de = L.Find("Deeper/DeadEnd"); if (de != null && v4 != null) { var vb = Exact(v4); var db = BoxOf(de); de.position += V(0f, 0f, (vb.min.z - deadFront) - db.min.z); de.GetComponent<UnityEngine.Renderer>().enabled = false; } }
    // shut days: DeeperClosed's renderer off; a Boulder_2 child at shutS fills the opening, its face 0.1 behind the wall face, and toggles with it
    {
        var dc = L.Find("Deeper/DeeperClosed");
        if (dc != null)
        {
            dc.GetComponent<UnityEngine.Renderer>().enabled = false; var g = kit.Spawn(PlaceKit.BK + "Rocks/Boulder_2", dc);
            if (g != null) { g.name = "ShutRock"; PlaceKit.StripColliders(g); Tint(g); g.transform.rotation = UnityEngine.Quaternion.identity; var b0 = Exact(g); g.transform.localScale *= shutS / UnityEngine.Mathf.Max(0.1f, UnityEngine.Mathf.Max(b0.size.x, UnityEngine.Mathf.Max(b0.size.y, b0.size.z))); var b = Exact(g); g.transform.position += V(97.6f - b.min.x, (fC + 1.05f) - b.center.y, 13.8f - b.center.z); rockPieces++; }
        }
    }
}

// ================= V12: the mouth strip kept clear of trees (KeepOuts C1; Wren 2026-10-03) =================
// a tree whose drawn mesh stands in the strip (x 52 to 54.5, z 38 to 46.5) between stripTop and stripHigh over the ground moves west by
// treeStep m at a time (its foot kept on the ground) until it no longer does, up to treeMax m (first run: RedPine1's low boughs, 1.93 m)
const float sx0 = 52f, sx1 = 54.5f, sz0 = 38f, sz1 = 46.5f, stripTop = 0.3f, stripHigh = 2f, stripStep = 0.25f, treeStep = 0.25f, treeMax = 5f, treeNear = 8f; string treeNote = "none";
{
    var canopy = kit.Root("Forest") != null ? kit.Root("Forest").transform.Find("Dense/Canopy") : null;
    if (canopy == null) notes.Add("no Forest/Dense/Canopy");
    else foreach (UnityEngine.Transform t in canopy)
    {
        if (UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P((sx0 + sx1) * 0.5f, (sz0 + sz1) * 0.5f)) > treeNear + (sz1 - sz0) * 0.5f) continue;
        var lod = t.GetComponentInChildren<UnityEngine.LODGroup>(); var rs = lod != null && lod.GetLODs().Length > 0 ? lod.GetLODs()[0].renderers : t.GetComponentsInChildren<UnityEngine.Renderer>();
        var mcs = new System.Collections.Generic.List<UnityEngine.MeshCollider>(); foreach (var r in rs) { if (r == null) continue; var mf = r.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mcs.Add(mc); }
        bool In() { UnityEngine.Physics.SyncTransforms(); for (float x = sx0; x <= sx1 + 1e-3f; x += stripStep) for (float z = sz0; z <= sz1 + 1e-3f; z += stripStep) { float g = G(x, z); var ray = new UnityEngine.Ray(V(x, g + stripHigh, z), UnityEngine.Vector3.down); foreach (var mc in mcs) if (mc.Raycast(ray, out var hh, stripHigh - stripTop)) return true; } return false; }
        try
        {
            if (!In()) continue; var start = t.position; float lift = start.y - G(start.x, start.z); float s = 0f;
            while (s < treeMax) { s += treeStep; var p = start - V(s, 0f, 0f); t.position = V(p.x, G(p.x, p.z) + lift, p.z); if (!In()) break; }
            treeNote = t.name + " moved " + F(s) + " m west to (" + F(t.position.x) + ", " + F(t.position.z) + ")" + (In() ? ", STILL IN THE STRIP" : "");
            if (In()) notes.Add(t.name + " still in the mouth strip after " + F(treeMax) + " m");
        }
        finally { foreach (var mc in mcs) if (mc != null) UnityEngine.Object.DestroyImmediate(mc); UnityEngine.Physics.SyncTransforms(); }
    }
}

// ================= WARPS =================
var warps = kit.Root("DevWarps").transform;
foreach (var (n, x, z, yaw, under) in new[] { ("Cave_Mouth", 58f, 44f, 223f, float.NaN), ("Cave_Chamber", 74f, 12f, 90f, floorY), ("Cave_SideRoom", 91.0f, 12.0f, 90f, floorY), ("Spur_Descent", 77.3f, 48.6f, 250f, float.NaN) })
{
    var w = warps.Find(n); if (w == null) { w = new UnityEngine.GameObject(n).transform; w.SetParent(warps, false); notes.Add("new warp " + n); }
    w.position = V(x, (float.IsNaN(under) ? G(x, z) : under) + 0.2f, z); w.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
}
notes.RemoveAll(s => s.StartsWith("new warp"));   // new warps are expected on the first run; the report says how many exist

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | removed " + removed + (missing.Count > 0 ? " (not found, likely gone already: " + string.Join(", ", missing) + ")" : "") + " | rail posts " + posts + " | cable runs " + cableRuns + " | bulbs " + bulbs + " | narrow rocks " + narrowRocks
    + " | rim hedge boxes " + hedgeBoxes + ", brush " + brush + " | usables " + uses + " | mouth strip trees: " + treeNote + " | rock pieces " + rockPieces + " | " + bulbNote + " | " + p84Note + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
