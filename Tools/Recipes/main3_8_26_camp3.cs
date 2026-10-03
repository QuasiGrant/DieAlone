// Main3 task 8.26, Camp 3 to Camp3Layout.md draft 2 (Sable 2026-10-02, 81acd02; Camp3Layout_UI.md, Camp3Layout_Story.md, Marlow's 826
// paper check): layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (absolute places; terrain cuts are functions
// of place, min'd in; the new pieces rebuilt under Campsites/Camp_3/Layout826 and PointsOfInterest/POI_Log_steps). In the runner after
// main3_8_25_camp2.cs (after 8.19, so the removals by name hold) and before main3_8_18a_solid.cs (whose hull pass then takes the FaceRock
// round the hollow, the moved easel, fire and canvases: T1, T2).
// NEEDS main3_8_26_keepouts.patch applied to Assets/Editor/KeepOuts.cs first (KeepOuts.Camp3, WhichCamp3): written while the 8.25 gate
// held the Editor, so it was kept out of Assets.
// Every turned prop gets its box from its own meshes in its own axes (PlaceKit.FitExact; FitCollider inflated the tent to 7.44 x 7.59 m,
// Marlow 826 block 1).
// REMOVALS (R): each named piece by its name within removeTol m (Marlow's read): 13 trees and 1 log for the creek and the line (C3, C5);
//   then every fir, sapling, plant, log or stump left in a Camp 3 keep-out zone (KeepOuts.Camp3: K1 the creek, 1.5 m each side of the
//   water; K2 r 2 at the line's RedFir8; K3 the lamppost spot; K4 the W1 sign; K5 the east rim spot plus 1 m) under Forest, SliceLook or
//   Ground815, except Floor/RedwoodHollowLog_2 (114.97, 93.11), which stays as a log across the water. C2's leftover props by name and
//   place. C8: PointsOfInterest/POI_Footbridge (main3_8_18a_solid.cs and main3_areas_setup.cs drop it in the same commit).
// RIM SPUR (block 2): draft 1's spur over the ravine was never built (Main3.unity holds no spur, plank or path there); this recipe builds
//   none, and the 8.26 check proves no trail point stands in the ravine.
// C1  the Camp_3 warp (75.0, 143.6) facing 30.
// C2  the floor: the fire at (76.0, 150.5); the seat log at yaw 90 (north-south), mesh centre (73.8, 150.2), z 149.2 to 151.2; R3's spot on
//     its south end; the easel at (80.3, 148.8), its painting facing 284, the stool 1.6 m in front; a paint box at (80.6, 147.6); a plank
//     table 1.2 x 0.6, top 0.75, x 77.9 to 79.1, z 147.0 to 147.6, the job form on it under its mug; the tent CS_Tent_Old_2 at scale 1.0,
//     long side east-west, door north, mesh centre (75.0, 140.7), one box from its mesh's local bounds, no ropes; the ground sheet 1.5 x 1.5
//     at its door, no collider; the faced canvases at x 70.3, z 144.5 to 147.4, painted faces toward the rock (so the must-hide check has
//     faces to test).
// C3  the Snag line: a rope from the Snag's trunk collider face, snagRopeUp over the ground, to a stake on the north rim (80, 163),
//     stakeUp (3.6) tall; pegs at the four points; pieces pieceSide square, pieceDrop under the rope, painted faces east; the hoist: a
//     pulley hoistUp up the Snag's tower face, a cleat at (98.86, 144.85), a rope between.
// C4  log steps on Camp to Camp 3 P80 to P94 (placed by those points): logSteps logs, looks only, and a collider-only StairRamp from P80 to
//     P94 (the STAIRS RULE); 8.3's two POI_Log_steps go (8.3's W1 leg entry is deleted in the same commit); the Camp_3_Rim warp to the
//     steps' top (97.8, 142.0) facing 268.
// C5  the creek on 8.1's carved line (main3_8_1_scene_ground.cs's creek and creekBed arrays, copied below): three runs, each bed the
//     running minimum (from its source down) of 8.1's carved bed less bedUnder8 and the bank ground less bankCut, so it never rises; the
//     ground within waterHalf + bankW of the water cut to the bed plus bankSlope per metre past waterHalf (a min, so reruns hold); a
//     water strip 2 x waterHalf wide, waterDepth over the bed, no collider; stones every stoneStep m and rocks where the bed drops
//     cascadeDrop or more within 0.5 m, no colliders. (a) upper, from POI_Plank_bridge down 8.1's line and the ravine to the pool inlet;
//     (b) the pool, x 82.1 to 85.1, z 148.45 to 150.95, surface -4.1, bed -4.4, its sink stone at the south lip (83.6, 148.45), the dam
//     stand (82.6, 147.2) facing 45; (c) lower, from the spring at (85.8, 126.0) along 8.1's line, shifted shiftNE m north-east from z 123
//     to z 110, then pushed further from any trail until its water edge stands edgeToTread m or more from the tread edge (Wren
//     2026-10-03: water's edge at least 1.0 m from the tread edge; the two designed crossings, the plank bridge and the stepping stones,
//     are exempt within crossR m), never more than valleyHalf from 8.1's line, to the stepping stones and the mouth (136.5, 66).
// C6  the east rim spot: x 92 to 96, z 137 to 140, the ground set to its median. C7 the lamppost spot (58, 150): a marker. C9 the W1 sign
//     (127.0, 72.8): a post and two arms on yaw pivots (toward the Camp 3 branch and the pump).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var notes = new System.Collections.Generic.List<string>(); string tentNote = "none";
var c3 = kit.Root("Campsites") != null ? kit.Root("Campsites").transform.Find("Camp_3") : null; var d = c3 != null ? c3.Find("Dressing") : null;
var poiRoot = kit.Root("PointsOfInterest") != null ? kit.Root("PointsOfInterest").transform : null; var trails = kit.Root("Trails") != null ? kit.Root("Trails").transform : null;
if (d == null || poiRoot == null || trails == null || kit.Root("Forest") == null) return "run 8.3, 8.16 and 8.17 camp3 first (Campsites/Camp_3/Dressing, PointsOfInterest, Trails, Forest)";
UnityEngine.Physics.SyncTransforms();
float G(float x, float z) => kit.H(x, z);
var L = kit.Fresh("Layout826", c3, c3.position, 0f);
var planks = kit.Tinted("Places_EaselWood", "Assets/Materials/Planks023A_1.0x1.0.mat", Hex("#7A5E42"), new UnityEngine.Vector2(0.25f, 2f));
var linen = kit.Tinted("Places_CanvasBack", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#CFC6B0"), UnityEngine.Vector2.one);
var paint = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Places/Places_Painting.mat"); if (paint == null) { notes.Add("no Places_Painting (8.17)"); paint = linen; }
var rope = kit.Tinted("Places_Rope", "Assets/Materials/Planks023A_1.0x1.0.mat", Hex("#8C7A58"), UnityEngine.Vector2.one);
var sheetMat = kit.Tinted("Places_GroundSheet", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#4E5A3C"), UnityEngine.Vector2.one);
var water = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_Water.mat"); if (water == null) notes.Add("no Blockout_Water (8.4)");
UnityEngine.GameObject Slab(string n, UnityEngine.Transform parent, UnityEngine.Vector3 world, UnityEngine.Vector3 size, UnityEngine.Material m, bool collide = false) => kit.Slab(n, parent, parent.InverseTransformPoint(world), size, m, default, collide);
UnityEngine.Transform Child(UnityEngine.Transform parent, string name, float x, float z, float tol)   // a direct child by name (or "name (n)") within tol m of (x, z)
{
    UnityEngine.Transform best = null; float bd = tol;
    foreach (UnityEngine.Transform t in parent) { if (!(t.name == name || t.name.StartsWith(name + " ("))) continue; float dd = UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(x, z)); if (dd <= bd) { bd = dd; best = t; } }
    return best;
}
void PlaceMeshCentre(UnityEngine.Transform t, float x, float z, float yaw)   // yaw, then the mesh's centre over (x, z) and its foot on the lowest ground under it
{
    t.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); var b = PlaceKit.MeshBounds(t.gameObject); t.position += V(x - b.center.x, 0f, z - b.center.z);
    b = PlaceKit.MeshBounds(t.gameObject); float gy = UnityEngine.Mathf.Min(UnityEngine.Mathf.Min(G(b.min.x, b.min.z), G(b.max.x, b.max.z)), UnityEngine.Mathf.Min(G(b.min.x, b.max.z), G(b.max.x, b.min.z)));
    t.position += V(0f, gy - b.min.y, 0f);
}

// ================= REMOVALS =================
const float removeTol = 0.3f;
var named = new (string n, float x, float z, string why)[] {
    ("RedFir8", 96.52f, 155.38f, "C3"), ("RedFir7", 113.01f, 94.22f, "C5"), ("RedFir5", 101.99f, 107.99f, "C5"), ("RedPine4", 105.75f, 103.63f, "C5"), ("RedFir6", 106.67f, 101.34f, "C5"),
    ("RedFir8", 121.07f, 84.02f, "C5"), ("RedPine4", 123.53f, 83.08f, "C5"), ("RedPine2", 121.03f, 86.24f, "C5"), ("RedPine3", 107.85f, 98.90f, "C5"), ("RedFir5", 101.47f, 110.04f, "C5"),
    ("RedFir8", 88.32f, 124.22f, "C5"), ("RedFir8", 98.20f, 115.78f, "C5"), ("RedPine5", 103.25f, 194.99f, "C5"), ("RedwoodHollowLog_0", 129.05f, 75.92f, "C5") };
var keep = new[] { ("RedwoodHollowLog_2", 114.97f, 93.11f), ("CITW_Tree_Stump", 56.25f, 151.14f) };   // the log stays across the water, 9 m from the tread (C5); the stump 2.09 m from the lamppost spot stays (C7)
string[] keepOutKinds = { "RedFir", "RedPine", "Bush", "CS_Bush", "ThinFern", "Fern", "DeadLeaves", "Branchs", "RedwoodHollowLog", "CITW_Tree_Stump", "Grass", "Nettle", "Mushroom" };
bool Kind(string n) { foreach (var k in keepOutKinds) if (n.StartsWith(k)) return true; return false; }
bool Kept(UnityEngine.Transform t) { foreach (var (n, x, z) in keep) if (t.name.StartsWith(n) && UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(x, z)) < removeTol) return true; return false; }
var pieces = new System.Collections.Generic.List<UnityEngine.Transform>();
foreach (var rn in new[] { "Forest", "SliceLook", "Ground815" }) { var r = kit.Root(rn); if (r == null) continue; foreach (var t in r.GetComponentsInChildren<UnityEngine.Transform>(true)) { var path = WalkIns.PathOf(t); if (t == r.transform || path.StartsWith("Ground815/Stops") || path.StartsWith("Ground815/Pockets")) continue; if (UnityEditor.PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject) || !UnityEditor.PrefabUtility.IsPartOfPrefabInstance(t.gameObject)) pieces.Add(t); } }
var gone = new System.Collections.Generic.HashSet<UnityEngine.Transform>(); int namedGone = 0, zoneGone = 0; var namedMissing = new System.Collections.Generic.List<string>(); var zoneNames = new System.Collections.Generic.List<string>();
foreach (var (n, x, z, why) in named)
{
    UnityEngine.Transform hit = null; float best = removeTol;
    foreach (var t in pieces) { if (t == null || gone.Contains(t) || !(t.name == n || t.name.StartsWith(n + " ("))) continue; float dd = UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(x, z)); if (dd <= best) { best = dd; hit = t; } }
    if (hit == null) { namedMissing.Add(why + " " + n + " (" + F(x) + ", " + F(z) + ")"); continue; } gone.Add(hit); UnityEngine.Object.DestroyImmediate(hit.gameObject); namedGone++;
}
foreach (var t in pieces)
{
    if (t == null || gone.Contains(t) || !Kind(t.name) || t.name.StartsWith("Sequoia") || Kept(t)) continue; var zn = KeepOuts.WhichCamp3(P(t.position.x, t.position.z)); if (zn == null) continue;
    zoneNames.Add(zn + ": " + t.name + " (" + F(t.position.x) + ", " + F(t.position.z) + ")"); gone.Add(t); UnityEngine.Object.DestroyImmediate(t.gameObject); zoneGone++;
}
// C2's leftovers (Milestone 11 dresses again) and the old crate and jar
int propsGone = 0;
foreach (var (n, x, z) in new[] { ("CS_Bedroll_Old_Rolled_2", 82.60f, 151.40f), ("CS_Bedroll_Old_1", 83.10f, 150.40f), ("CS_Backpack_Old_3", 79.00f, 154.00f), ("CITW_Bucket", 77.80f, 150.60f), ("CITW_Crate", 84.60f, 149.80f), ("CITW_Jar", 84.60f, 149.80f) })
{ var t = Child(d, n, x, z, 0.5f); if (t != null) { PlaceKit.Remove(t); propsGone++; } }
// C8: the footbridge
bool bridgeGone = false; { var fb = poiRoot.Find("POI_Footbridge"); if (fb != null) { PlaceKit.Remove(fb); bridgeGone = true; } }
UnityEngine.Physics.SyncTransforms();

// ================= C2: the floor =================
{
    // the fire, the seat log (north-south) and R3's spot on its south end
    var fire = d.Find("Fire"); if (fire != null) fire.position = V(76.0f, G(76.0f, 150.5f), 150.5f); else notes.Add("no Dressing/Fire");
    var log = Child(d, "CS_Log_Large_Long_Seat_1", 74.6f, 151.2f, 2.5f); if (log == null) log = Child(d, "CS_Log_Large_Long_Seat_1", 73.8f, 150.2f, 2.5f);
    if (log != null) { PlaceMeshCentre(log, 73.8f, 150.2f, 90f); PlaceKit.FitExact(log.gameObject); } else notes.Add("no seat log");
    var r3 = c3.Find("Resident/Resident_Camp3_Spot");
    if (r3 != null) { var lb = log != null ? PlaceKit.MeshBounds(log.gameObject) : new UnityEngine.Bounds(V(73.8f, G(73.8f, 149.5f), 149.5f), UnityEngine.Vector3.zero); r3.position = V(73.8f, lb.max.y, 149.5f); r3.rotation = UnityEngine.Quaternion.LookRotation(V(76.0f - 73.8f, 0f, 150.5f - 149.5f)); }
    else notes.Add("no Resident_Camp3_Spot");
    // the easel (its painting is local -z: facing 284 puts its local +z on 104), the stool 1.6 m in front, a paint box
    var easel = d.Find("Easel"); if (easel != null) { easel.rotation = UnityEngine.Quaternion.Euler(0f, 284f - 180f, 0f); easel.position = V(80.3f, G(80.3f, 148.8f), 148.8f); } else notes.Add("no Dressing/Easel");
    PlaceKit.Remove(L.Find("PaintBox")); var box = kit.Ground(PlaceKit.CI + "Props/CITW_Crate", L, 80.6f, 147.6f, 0f, 0.6f, false); if (box != null) { box.name = "PaintBox"; PlaceKit.FitExact(box); }
    // the plank table, one box; the job form on it under its mug
    const float tblX0 = 77.9f, tblX1 = 79.1f, tblZ0 = 147.0f, tblZ1 = 147.6f, tblTop = 0.75f, tblT = 0.04f, legW = 0.05f;
    float tx = (tblX0 + tblX1) * 0.5f, tz = (tblZ0 + tblZ1) * 0.5f, tg = G(tx, tz);
    var table = kit.Group("PlankTable", L, V(tx, tg, tz), 0f);
    Slab("Top", table, V(tx, tg + tblTop - tblT * 0.5f, tz), V(tblX1 - tblX0, tblT, tblZ1 - tblZ0), planks);
    foreach (var (lx, lz) in new[] { (tblX0 + legW, tblZ0 + legW), (tblX1 - legW, tblZ0 + legW), (tblX0 + legW, tblZ1 - legW), (tblX1 - legW, tblZ1 - legW) }) Slab("Leg", table, V(lx, tg + (tblTop - tblT) * 0.5f, lz), V(legW, tblTop - tblT, legW), planks);
    PlaceKit.FitExact(table.gameObject);
    var form = d.Find("JobForm"); if (form != null) { form.position = V(tx, tg + tblTop, tz); form.rotation = UnityEngine.Quaternion.Euler(0f, 10f, 0f); } else notes.Add("no Dressing/JobForm");
    // the tent at scale 1.0: the yaw that lays its long side east-west (the doc's box, x 72.97 to 77.04, z 139.64 to 141.77) with its door (the
    // hidden _Open flaps, on a short end of the pack mesh, so "door north" cannot also hold) toward the fire
    var tent = Child(d, "CS_Tent_Old_2", 80.5f, 153f, 3f) ?? Child(d, "CS_Tent_Old_2", 75.0f, 140.7f, 3f);
    if (tent == null) notes.Add("no tent");
    else
    {
        tent.localScale = UnityEngine.Vector3.one; float pick = float.NaN, bestDot = float.MinValue; var toFire = new UnityEngine.Vector2(76.0f - 75.0f, 150.5f - 140.7f).normalized;
        foreach (var yaw in new[] { 0f, 90f, 180f, 270f })
        {
            tent.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); var b = PlaceKit.MeshBounds(tent.gameObject); if (b.size.x < b.size.z) continue;
            var door = UnityEngine.Vector3.zero; int n = 0; foreach (var mf in tent.GetComponentsInChildren<UnityEngine.MeshFilter>(true)) if (mf.name.EndsWith("_Open") && mf.sharedMesh != null) { door += mf.transform.TransformPoint(mf.sharedMesh.bounds.center); n++; }
            if (n == 0) continue; var dd = new UnityEngine.Vector2(door.x / n - b.center.x, door.z / n - b.center.z).normalized; float dot = UnityEngine.Vector2.Dot(dd, toFire); if (dot > bestDot) { bestDot = dot; pick = yaw; }
        }
        if (float.IsNaN(pick)) { notes.Add("tent: no yaw lays its long side east-west"); pick = 90f; }
        PlaceMeshCentre(tent, 75.0f, 140.7f, pick); PlaceKit.FitExact(tent.gameObject, "Rope");
        var tb = tent.GetComponent<UnityEngine.BoxCollider>().bounds; tentNote = ("yaw " + F(pick) + ", box x " + F(tb.min.x) + " to " + F(tb.max.x) + ", z " + F(tb.min.z) + " to " + F(tb.max.z) + ", top " + F(tb.max.y - tb.min.y));
    }
    // the ground sheet at the door, no collider
    const float shX0 = 74.25f, shX1 = 75.75f, shZ0 = 141.77f, shZ1 = 143.27f; float shG = G((shX0 + shX1) * 0.5f, (shZ0 + shZ1) * 0.5f);
    Slab("GroundSheet", L, V((shX0 + shX1) * 0.5f, shG + 0.01f, (shZ0 + shZ1) * 0.5f), V(shX1 - shX0, 0.02f, shZ1 - shZ0), sheetMat);
    // the faced canvases: four on the west wall's foot, painted faces (local -z) to the rock (west: yaw 90), tops leaning onto it
    PlaceKit.Remove(d.Find("FacedCanvases"));
    const float fcX = 70.3f, fcZ0 = 144.5f, fcZ1 = 147.4f, fcLean = -12f; const int fcN = 4;
    var fc = kit.Group("FacedCanvases", L, V(fcX, G(fcX, (fcZ0 + fcZ1) * 0.5f), (fcZ0 + fcZ1) * 0.5f), 0f);
    for (int i = 0; i < fcN; i++)
    {
        float w = 0.7f + 0.1f * (i % 2), h = 1.1f + 0.2f * (i % 3), z = UnityEngine.Mathf.Lerp(fcZ0 + w * 0.5f, fcZ1 - w * 0.5f, i / (float)(fcN - 1));
        var g = kit.Group("Canvas", fc, V(fcX, G(fcX, z), z), 0f); g.rotation = UnityEngine.Quaternion.Euler(fcLean, 90f, 0f);
        kit.Slab("Back", g, V(0f, h * 0.5f, 0.02f), V(w, h, 0.03f), linen);
        foreach (var (c, s) in new[] { (V(0f, 0.02f, 0f), V(w, 0.04f, 0.05f)), (V(0f, h - 0.02f, 0f), V(w, 0.04f, 0.05f)), (V(-w * 0.5f + 0.02f, h * 0.5f, 0f), V(0.04f, h, 0.05f)), (V(w * 0.5f - 0.02f, h * 0.5f, 0f), V(0.04f, h, 0.05f)) }) kit.Slab("Stretcher", g, c, s, planks);
        kit.Slab("Painting", g, V(0f, h * 0.5f, -0.005f), V(w - 0.06f, h - 0.06f, 0.005f), paint);
    }
    // the painted faces under one group, so the must-hide pixel check tests the faces and not the backs (the backs are what the deck reads)
    var faces = kit.Group("Faces", fc, fc.position, 0f); foreach (var p in System.Linq.Enumerable.ToArray(fc.GetComponentsInChildren<UnityEngine.Transform>())) if (p.name == "Painting") p.SetParent(faces, true);
    // behind the canvases, to the rock: a box as high as the canvases (the canvases and the rock are its reason; a 2.2 m Ignore Raycast box
    // was stood on from the rocks), so no body gets in behind them (the
    // first area flood found a 20-place pocket at (69.5, -3.2, 146.0) between the canvases and the west wall's hulls)
    const float behindX0 = 68.0f; var fcb = PlaceKit.MeshBounds(fc.gameObject); float behindH = fcb.max.y - fcb.min.y;
    var behind = kit.Blocker("BehindCanvases", L, L.InverseTransformPoint(V((behindX0 + fcb.max.x) * 0.5f, fcb.min.y + behindH * 0.5f, fcb.center.z)), V(fcb.max.x - behindX0, behindH, fcb.size.z + 0.2f));
    // the fire: one box from its meshes, so its interaction ray has something to meet
    if (fire != null) PlaceKit.FitExact(fire.gameObject);
    // the slot along the tent's south face from its west end, between it and the hulls of FaceRock Boulder_2 and BigBoulders_3 (the area flood
    // found traps at (72.6, -3.3, 139.6) and, after a first fill there, (73.4, -3.3, 139.3)): a box filled to the rocks' height
    { const float gx0 = 72.55f, gx1 = 75.5f, gz0 = 138.0f, gz1 = 139.64f, gTop = 1.7f; float gg = G((gx0 + gx1) * 0.5f, (gz0 + gz1) * 0.5f); kit.Blocker("TentRockGap", L, L.InverseTransformPoint(V((gx0 + gx1) * 0.5f, gg + gTop * 0.5f - 0.25f, (gz0 + gz1) * 0.5f)), V(gx1 - gx0, gTop + 0.5f, gz1 - gz0)); }   // over the rock's top too (then a 5-place trap at (74.6, -2.6, 138.6)), open east to the floor
    { const float wx0 = 71.4f, wx1 = 72.96f, wz0 = 138.0f, wz1 = 141.8f, wTop = 1.7f; float wg = G((wx0 + wx1) * 0.5f, (wz0 + wz1) * 0.5f); kit.Blocker("TentRockGapWest", L, L.InverseTransformPoint(V((wx0 + wx1) * 0.5f, wg + wTop * 0.5f - 0.25f, (wz0 + wz1) * 0.5f)), V(wx1 - wx0, wTop + 0.5f, wz1 - wz0)); }   // and along its west face over Boulder_2's foot (then traps at (72.6, -3.1, 140.0) and (72.2, -2.8, 139.6) in the rocks' hollows)
}

// ================= C3: the Snag line and the hoist =================
const float pieceClear = 2.85f; string ropeNote = "none"; const float snagRopeUp = 3.4f, stakeUp = 3.6f, stakeW = 0.12f, pieceSide = 0.5f, pieceDrop = 0.2f, ropeT = 0.03f, hoistUp = 12f, cleatH = 1.2f, cleatW = 0.12f, cleatOff = 0.3f, barkR = 2.5f;
var pegs = new[] { P(92.8f, 149.8f), P(89.6f, 153.1f), P(86.4f, 156.4f), P(83.2f, 159.7f) }; var stakeAt = P(80f, 163f); var cleatAt = P(98.86f, 144.85f);
int pieceN = 0;
{
    var snag = kit.Root("Giants") != null ? kit.Root("Giants").transform.Find("Heroes/Snag") : null; var trunk = snag != null ? snag.Find("Trunk") : null;
    if (snag == null || trunk == null) notes.Add("no Giants/Heroes/Snag/Trunk");
    else
    {
        var tc = trunk.GetComponent<UnityEngine.Collider>().bounds; var sc = P(tc.center.x, tc.center.z); float trunkR = tc.extents.x;
        var line = kit.Group("SnagLine", L, L.position, 0f);
        var toStake = (stakeAt - sc).normalized; var a2 = sc + toStake * trunkR; var a = V(a2.x, G(a2.x, a2.y) + snagRopeUp, a2.y);
        var stake = Slab("Stake", line, V(stakeAt.x, G(stakeAt.x, stakeAt.y) + stakeUp * 0.5f, stakeAt.y), V(stakeW, stakeUp, stakeW), planks, true);
        var b = V(stakeAt.x, G(stakeAt.x, stakeAt.y) + stakeUp, stakeAt.y);
        // the rope's Snag end no lower than every piece needs to hang pieceClear over the ground under it (at the doc's 3.4 m piece 1 hung 2.72)
        foreach (var q in pegs) { var ab2 = P(b.x - a.x, b.z - a.z); float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - P(a.x, a.z), ab2) / ab2.sqrMagnitude); if (t >= 0.999f) continue; float need = G(q.x, q.y) + pieceClear + pieceDrop + pieceSide + 0.02f; float ay = (need - t * b.y) / (1f - t); if (ay > a.y) { a.y = ay; ropeNote = "Snag end raised to " + F(a.y - G(a2.x, a2.y)) + " m over its ground"; } }
        void Rope(UnityEngine.Transform parent, UnityEngine.Vector3 p, UnityEngine.Vector3 q, string n) { var s = Slab(n, parent, (p + q) * 0.5f, V(ropeT, ropeT, UnityEngine.Vector3.Distance(p, q)), rope); s.transform.rotation = UnityEngine.Quaternion.LookRotation((q - p).normalized); }
        Rope(line, a, b, "Rope");
        UnityEngine.Vector3 OnRope(UnityEngine.Vector2 q) { var ab = P(b.x - a.x, b.z - a.z); float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - P(a.x, a.z), ab) / ab.sqrMagnitude); return UnityEngine.Vector3.Lerp(a, b, t); }
        for (int i = 0; i < pegs.Length; i++)
        {
            var r = OnRope(pegs[i]); Slab("Peg", line, r, V(0.03f, 0.08f, 0.03f), planks);
            // a piece: linen back west, his sketch on its east face (local -z faces east at yaw 270)
            var g = kit.Group("Piece" + (i + 1), line, r - V(0f, pieceDrop + pieceSide * 0.5f, 0f), 270f);
            kit.Slab("Back", g, V(0f, 0f, 0.005f), V(pieceSide, pieceSide, 0.005f), linen); kit.Slab("Sketch", g, V(0f, 0f, -0.002f), V(pieceSide - 0.04f, pieceSide - 0.04f, 0.003f), paint); pieceN++;
        }
        // the hoist: a pulley hoistUp up the Snag's face toward the tower, a cleat (a box) on its south-east side, a rope between
        var tower = kit.Root("Camp") != null ? kit.Root("Camp").transform.Find("Tower") : null; var toTower = tower != null ? (P(tower.position.x, tower.position.z) - sc).normalized : P(1f, 0.3f).normalized;
        var hoist = kit.Group("Hoist", L, L.position, 0f); var pa = sc + toTower * barkR; var pulley = V(pa.x, G(sc.x, sc.y) + hoistUp, pa.y);
        Slab("Pulley", hoist, pulley, V(0.2f, 0.2f, 0.1f), planks);
        cleatAt = sc + (cleatAt - sc).normalized * (trunkR + cleatOff + cleatW * 0.5f);   // the doc's bearing, cleatOff off the trunk collider as built
        float cg = G(cleatAt.x, cleatAt.y); Slab("Cleat", hoist, V(cleatAt.x, cg + cleatH * 0.5f, cleatAt.y), V(cleatW, cleatH, cleatW), planks, true);
        Rope(hoist, pulley, V(cleatAt.x, cg + cleatH, cleatAt.y), "Rope");

    }
}

// ================= C4: the log steps =================
const int logSteps = 19; const float rampExtra = 0.6f, logLen = 1.8f, logGirth = 0.3f, rampW = 1.8f, rampT = 0.2f, packLogLen = 2f, packLogGirth = 0.37f;
int logsMade = 0;
{
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(poiRoot))) if (t.name == "POI_Log_steps") PlaceKit.Remove(t);
    var leg = trails.Find("Camp to Camp 3"); var pts = new System.Collections.Generic.List<UnityEngine.Vector3>();
    if (leg != null) for (int i = 80; i <= 94; i++) { var p = leg.Find("P" + i); if (p != null) pts.Add(p.position); }
    if (pts.Count < 2) notes.Add("Camp to Camp 3: no P80 to P94");
    else
    {
        var steps = new UnityEngine.GameObject("POI_Log_steps").transform; steps.SetParent(poiRoot, false); steps.position = pts[0];
        var lens = new float[pts.Count]; for (int i = 1; i < pts.Count; i++) lens[i] = lens[i - 1] + UnityEngine.Vector3.Distance(pts[i - 1], pts[i]);
        UnityEngine.Vector3 At(float s, out UnityEngine.Vector3 dir) { int i = 1; while (i < pts.Count - 1 && lens[i] < s) i++; float t = UnityEngine.Mathf.InverseLerp(lens[i - 1], lens[i], s); dir = (pts[i] - pts[i - 1]).normalized; return UnityEngine.Vector3.Lerp(pts[i - 1], pts[i], t); }
        var logs = kit.Group("OwnedLogs", steps, steps.position, 0f);
        for (int k = 0; k < logSteps; k++)
        {
            float s = lens[pts.Count - 1] * (k + 0.5f) / logSteps; var q = At(s, out var dir); var flat = V(dir.x, 0f, dir.z).normalized;
            var lg = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", logs); if (lg == null) break;
            lg.transform.rotation = UnityEngine.Quaternion.LookRotation(flat) * UnityEngine.Quaternion.Euler(0f, 90f, 0f);   // the mesh's length (local x) across the path
            lg.transform.localScale = V(logLen / packLogLen, logGirth / packLogGirth, logGirth / packLogGirth);
            var mb = PlaceKit.MeshBounds(lg); lg.transform.position += V(q.x, G(q.x, q.z) + logGirth * 0.3f, q.z) - mb.center; logsMade++;
        }
        // the StairRamp: a collider-only box whose top face runs from P80 to P94 (the trail points are on the walking surface)
        var top = pts[0]; var bot = pts[pts.Count - 1]; var run = bot - top; var ramp = new UnityEngine.GameObject("StairRamp").transform; ramp.SetParent(steps, false);
        var up = UnityEngine.Vector3.Cross(run.normalized, UnityEngine.Vector3.Cross(UnityEngine.Vector3.up, run.normalized)).normalized; if (up.y < 0f) up = -up;
        ramp.rotation = UnityEngine.Quaternion.LookRotation(run.normalized, up); ramp.position = (top + bot) * 0.5f + run.normalized * (rampExtra * 0.5f) - up * (rampT * 0.5f); ramp.localScale = V(rampW, rampT, run.magnitude + rampExtra);   // its top end on P80 (a lip past it stopped the walk), the extra length under the floor past P94
        ramp.gameObject.AddComponent<UnityEngine.BoxCollider>();
    }
}

// ================= C5: the creek =================
// 8.1's carved line and bed (main3_8_1_scene_ground.cs lines 42 and 43): change both together
var creek8 = new[] { P(104, 215.2f), P(104.8f, 203.2f), P(100, 175.2f), P(84, 150), P(78, 146), P(84, 128), P(100, 110), P(128, 78), P(136.5f, 66) };
var creekBed8 = new[] { 9.5f, 8f, 3.5f, -4.1f, -4.1f, -4.3f, -4.8f, -5.3f, -5.8f };
const float bedUnder8 = 0.1f, bankCut = 0.4f, bankProbe = 2.5f, waterHalf = 0.6f, bankW = 1.0f, bankSlope = 0.6f, waterDepth = 0.25f, sampleStep = 0.25f, stoneStep = 4f, cascadeDrop = 0.3f;
const float poolX0 = 82.1f, poolX1 = 85.1f, poolZ0 = 148.45f, poolZ1 = 150.95f, poolSurf = -4.1f, poolBed = -4.4f, poolBank = 0.5f;
const float treadKeep = 0.3f; const float shiftNE = 1.5f, shiftZHi = 123f, shiftZLo = 110f, shiftTaper = 2f, treadHalf = 1.2f, edgeToTread = 1.0f, crossR = 3f, valleyHalf = 4f, heightTol = 0.005f;
float SegDist(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b, out float t) { var ab = b - a; t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
float Bed8(UnityEngine.Vector2 q, out float off) { float best = float.MaxValue, bed = 0f; for (int i = 0; i < creek8.Length - 1; i++) { float dd = SegDist(q, creek8[i], creek8[i + 1], out float t); if (dd < best) { best = dd; bed = UnityEngine.Mathf.Lerp(creekBed8[i], creekBed8[i + 1], t); } } off = best; return bed; }
var plank = poiRoot.Find("POI_Plank_bridge"); var plankAt = plank != null ? P(plank.position.x, plank.position.z) : P(105.32f, 205.23f);
var stonesPoi = poiRoot.Find("POI_Stepping_stones"); var stonesAt = stonesPoi != null ? P(stonesPoi.position.x, stonesPoi.position.z) : P(131.71f, 72.82f);
var crossings = new[] { plankAt, stonesAt };
// the trail centre lines, for the tread rule
var treadLines = new System.Collections.Generic.List<UnityEngine.Vector2[]>(); foreach (UnityEngine.Transform lg in trails) { var l = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform p in lg) l.Add(P(p.position.x, p.position.z)); if (l.Count > 1) treadLines.Add(l.ToArray()); }
float TrailDist(UnityEngine.Vector2 q, out UnityEngine.Vector2 away) { float best = float.MaxValue; away = UnityEngine.Vector2.zero; foreach (var l in treadLines) for (int i = 1; i < l.Length; i++) { float dd = SegDist(q, l[i - 1], l[i], out float t); if (dd < best) { best = dd; var n = q - (l[i - 1] + (l[i] - l[i - 1]) * t); away = n.sqrMagnitude > 1e-6f ? n.normalized : UnityEngine.Vector2.zero; } } return best; }
bool NearCrossing(UnityEngine.Vector2 q) { foreach (var c in crossings) if (UnityEngine.Vector2.Distance(q, c) < crossR) return true; return false; }
// a run: dense samples along its control points, each with its bed (the running minimum) and water height
System.Collections.Generic.List<(UnityEngine.Vector2 p, float bed)> Run(UnityEngine.Vector2[] ctrl, bool shiftLower, System.Collections.Generic.List<string> pushNotes)
{
    var pts = new System.Collections.Generic.List<UnityEngine.Vector2>();
    for (int i = 1; i < ctrl.Length; i++) { int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector2.Distance(ctrl[i - 1], ctrl[i]) / sampleStep)); for (int k = (i == 1 ? 0 : 1); k <= n; k++) pts.Add(UnityEngine.Vector2.Lerp(ctrl[i - 1], ctrl[i], k / (float)n)); }
    if (shiftLower)
    {
        var ne = P(0.75f, 0.66f).normalized;   // square to 8.1's (84, 128) to (100, 110) segment, toward the north-east
        for (int i = 0; i < pts.Count; i++)
        {
            var q = pts[i]; float w = q.y <= shiftZHi && q.y >= shiftZLo ? 1f : UnityEngine.Mathf.Clamp01(1f - UnityEngine.Mathf.Min(UnityEngine.Mathf.Abs(q.y - shiftZHi), UnityEngine.Mathf.Abs(q.y - shiftZLo)) / shiftTaper);
            q += ne * shiftNE * w;
            // the tread rule: the water's edge edgeToTread m or more from the tread edge, pushed away from the nearest trail, inside 8.1's valley floor
            for (int it = 0; it < 20 && !NearCrossing(q); it++) { float td = TrailDist(q, out var away); float need = treadHalf + edgeToTread + waterHalf; if (td >= need) break; Bed8(q + away * 0.1f, out float off8); if (off8 > valleyHalf - waterHalf) { pushNotes.Add("(" + F(q.x) + ", " + F(q.y) + ") " + F(td) + " m from a trail centre, at 8.1's valley edge"); break; } q += away * UnityEngine.Mathf.Min(0.1f, need - td); }
            pts[i] = q;
        }
    }
    var outp = new System.Collections.Generic.List<(UnityEngine.Vector2, float)>(); float run = float.MaxValue;
    for (int i = 0; i < pts.Count; i++)
    {
        var q = pts[i]; var tan = (pts[UnityEngine.Mathf.Min(pts.Count - 1, i + 1)] - pts[UnityEngine.Mathf.Max(0, i - 1)]).normalized; var nrm = P(-tan.y, tan.x);
        float bank = UnityEngine.Mathf.Min(G(q.x + nrm.x * bankProbe, q.y + nrm.y * bankProbe), G(q.x - nrm.x * bankProbe, q.y - nrm.y * bankProbe));
        float want = UnityEngine.Mathf.Min(Bed8(q, out _) - bedUnder8, bank - bankCut); run = UnityEngine.Mathf.Min(run, want); outp.Add((q, run));
    }
    return outp;
}
var pushNotes = new System.Collections.Generic.List<string>();
var poolInlet = P(84.3f, 150.95f); var spring = P(85.8f, 126.0f); var mouth = P(136.5f, 66f);
var upper = Run(new[] { plankAt, P(104.8f, 203.2f), P(100f, 175.2f), poolInlet }, false, pushNotes);
var lower = Run(new[] { spring, P(100f, 110f), P(128f, 78f), stonesAt, mouth }, true, pushNotes);
// the upper run ends in the pool, never under its surface's rim
int carved = 0, stonesN = 0, rocksN = 0;
{
    var ter = kit.Terrain; var data = ter.terrainData; var org = ter.transform.position; int res = data.heightmapResolution; float cx = data.size.x / (res - 1), cz = data.size.z / (res - 1);
    float reach = waterHalf + bankW;
    void Carve(System.Collections.Generic.List<(UnityEngine.Vector2 p, float bed)> run)
    {
        float x0 = float.MaxValue, x1 = float.MinValue, z0 = float.MaxValue, z1 = float.MinValue; foreach (var (p, _) in run) { x0 = UnityEngine.Mathf.Min(x0, p.x); x1 = UnityEngine.Mathf.Max(x1, p.x); z0 = UnityEngine.Mathf.Min(z0, p.y); z1 = UnityEngine.Mathf.Max(z1, p.y); }
        int i0 = UnityEngine.Mathf.FloorToInt((x0 - reach - org.x) / cx), i1 = UnityEngine.Mathf.CeilToInt((x1 + reach - org.x) / cx), j0 = UnityEngine.Mathf.FloorToInt((z0 - reach - org.z) / cz), j1 = UnityEngine.Mathf.CeilToInt((z1 + reach - org.z) / cz);
        int w = i1 - i0 + 1, h = j1 - j0 + 1; var hs = data.GetHeights(i0, j0, w, h);
        for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
        {
            var q = P(org.x + (i0 + i) * cx, org.z + (j0 + j) * cz); float best = float.MaxValue, bed = 0f;
            for (int k = 0; k < run.Count; k++) { float dd = (run[k].p - q).sqrMagnitude; if (dd < best) { best = dd; bed = run[k].bed; } }
            float dist = UnityEngine.Mathf.Sqrt(best); if (dist > reach) continue; if (TrailDist(q, out _) < treadHalf + treadKeep) continue;   // a tread is never cut: at the plank bridge and the stepping stones the trail stays over the water (the first run cut 1.3 m at J)
            float want = bed + UnityEngine.Mathf.Max(0f, dist - waterHalf) * bankSlope, now = hs[j, i] * data.size.y + org.y;
            if (now <= want + heightTol) continue; hs[j, i] = (want - org.y) / data.size.y; carved++;
        }
        data.SetHeights(i0, j0, hs);
    }
    Carve(upper); Carve(lower);
    // the pool: its bed inside the rectangle, its banks poolBank wide
    {
        int i0 = UnityEngine.Mathf.FloorToInt((poolX0 - poolBank - org.x) / cx), i1 = UnityEngine.Mathf.CeilToInt((poolX1 + poolBank - org.x) / cx), j0 = UnityEngine.Mathf.FloorToInt((poolZ0 - poolBank - org.z) / cz), j1 = UnityEngine.Mathf.CeilToInt((poolZ1 + poolBank - org.z) / cz);
        int w = i1 - i0 + 1, h = j1 - j0 + 1; var hs = data.GetHeights(i0, j0, w, h);
        for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
        {
            float x = org.x + (i0 + i) * cx, z = org.z + (j0 + j) * cz; float dx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(poolX0 - x, x - poolX1)), dz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(poolZ0 - z, z - poolZ1)); float dist = UnityEngine.Mathf.Sqrt(dx * dx + dz * dz);
            if (dist > poolBank) continue; float want = UnityEngine.Mathf.Lerp(poolBed, poolSurf + 0.15f, dist / poolBank), now = hs[j, i] * data.size.y + org.y;
            if (now <= want + heightTol) continue; hs[j, i] = (want - org.y) / data.size.y; carved++;
        }
        data.SetHeights(i0, j0, hs);
    }
    UnityEditor.EditorUtility.SetDirty(data); UnityEngine.Physics.SyncTransforms();
    // the water: a strip per run, waterDepth over its bed, and the pool's sheet; stones and cascade rocks, no colliders
    var creekG = kit.Group("Creek", L, L.position, 0f);
    void Strip(string n, System.Collections.Generic.List<(UnityEngine.Vector2 p, float bed)> run)
    {
        var verts = new System.Collections.Generic.List<UnityEngine.Vector3>(); var tris = new System.Collections.Generic.List<int>();
        for (int i = 0; i < run.Count; i++)
        {
            var tan = (run[UnityEngine.Mathf.Min(run.Count - 1, i + 1)].p - run[UnityEngine.Mathf.Max(0, i - 1)].p).normalized; var nrm = P(-tan.y, tan.x); float y = run[i].bed + waterDepth;
            verts.Add(V(run[i].p.x + nrm.x * waterHalf, y, run[i].p.y + nrm.y * waterHalf) - creekG.position); verts.Add(V(run[i].p.x - nrm.x * waterHalf, y, run[i].p.y - nrm.y * waterHalf) - creekG.position);
            if (i > 0) { int b = i * 2; tris.AddRange(new[] { b - 2, b, b - 1, b - 1, b, b + 1 }); }
        }
        var mesh = new UnityEngine.Mesh { name = "Creek_" + n }; mesh.SetVertices(verts); mesh.SetTriangles(tris, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
        var g = new UnityEngine.GameObject(n); g.transform.SetParent(creekG, false); g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; g.AddComponent<UnityEngine.MeshRenderer>().sharedMaterial = water;
        float last = -stoneStep; for (int i = 0; i < run.Count; i++)
        {
            float s = i * sampleStep; if (s - last >= stoneStep) { last = s; var side = (stonesN % 2 == 0 ? 1f : -1f) * waterHalf * 0.5f; var tan = (run[UnityEngine.Mathf.Min(run.Count - 1, i + 1)].p - run[UnityEngine.Mathf.Max(0, i - 1)].p).normalized; var q = run[i].p + P(-tan.y, tan.x) * side; if (kit.Ground(PlaceKit.CS + "Rocks and Stones/CS_Stone_" + (1 + stonesN % 8), creekG, q.x, q.y, stonesN * 47f, 0.35f, false, 0.05f) != null) stonesN++; }
            int ahead = UnityEngine.Mathf.Min(run.Count - 1, i + UnityEngine.Mathf.RoundToInt(0.5f / sampleStep)); if (run[i].bed - run[ahead].bed >= cascadeDrop && i % 4 == 0) { var q = run[ahead].p; if (kit.Ground(PlaceKit.CS + "Rocks and Stones/CS_Stone_" + (1 + rocksN % 8), creekG, q.x, q.y, rocksN * 61f, 0.7f, false, 0.1f) != null) rocksN++; }
        }
    }
    Strip("Upper", upper); Strip("Lower", lower);
    var pool = Slab("Pool", creekG, V((poolX0 + poolX1) * 0.5f, poolSurf, (poolZ0 + poolZ1) * 0.5f), V(poolX1 - poolX0, 0.01f, poolZ1 - poolZ0), water);
    // the sink: a flat stone over a gravel throat at the pool's south lip (the dam point), a low box so the eye ray meets it; the dam stand
    var sinkAt = P(83.6f, 148.45f); var sink = kit.Ground(PlaceKit.CS + "Rocks and Stones/CS_Stone_2", L, sinkAt.x, sinkAt.y, 0f, 0.6f, false, 0.05f); if (sink != null) { sink.name = "Sink"; PlaceKit.FitExact(sink); }
    kit.Marker("DamStand", L, L.InverseTransformPoint(V(82.6f, G(82.6f, 147.2f), 147.2f)), 45f);
    // the spring: water out of the rocks at the south trench's foot
    var springG = kit.Group("Spring", L, V(spring.x, G(spring.x, spring.y), spring.y), 0f);
    for (int i = 0; i < 3; i++) kit.Ground(PlaceKit.CS + "Rocks and Stones/CS_Stone_" + (5 + i), springG, spring.x - 0.6f + i * 0.6f, spring.y - 0.5f, i * 70f, 0.8f, false, 0.1f);
    for (int k = 0; k < upper.Count; k += 4) kit.ClearDetail(V(upper[k].p.x, 0f, upper[k].p.y), waterHalf + 0.5f);
    for (int k = 0; k < lower.Count; k += 4) kit.ClearDetail(V(lower[k].p.x, 0f, lower[k].p.y), waterHalf + 0.5f);
    kit.ClearDetail(V((poolX0 + poolX1) * 0.5f, 0f, (poolZ0 + poolZ1) * 0.5f), 2.2f);
}

// ================= C6, C7, C9 =================
{
    // the east rim spot: the ground set to its median (a function of the spot's own ground, so a rerun holds)
    const float rx0 = 92f, rx1 = 96f, rz0 = 137f, rz1 = 140f;
    var ter = kit.Terrain; var data = ter.terrainData; var org = ter.transform.position; int res = data.heightmapResolution; float cx = data.size.x / (res - 1), cz = data.size.z / (res - 1);
    int i0 = UnityEngine.Mathf.CeilToInt((rx0 - org.x) / cx), i1 = UnityEngine.Mathf.FloorToInt((rx1 - org.x) / cx), j0 = UnityEngine.Mathf.CeilToInt((rz0 - org.z) / cz), j1 = UnityEngine.Mathf.FloorToInt((rz1 - org.z) / cz);
    var hs = data.GetHeights(i0, j0, i1 - i0 + 1, j1 - j0 + 1); var all = new System.Collections.Generic.List<float>(); foreach (var v in hs) all.Add(v); all.Sort(); float med = all[all.Count / 2];
    for (int j = 0; j <= j1 - j0; j++) for (int i = 0; i <= i1 - i0; i++) hs[j, i] = med; data.SetHeights(i0, j0, hs); UnityEditor.EditorUtility.SetDirty(data);
    kit.Marker("EastRimSpot", L, L.InverseTransformPoint(V((rx0 + rx1) * 0.5f, med * data.size.y + org.y, (rz0 + rz1) * 0.5f)), 270f);
    kit.Marker("LamppostSpot", L, L.InverseTransformPoint(V(58f, G(58f, 150f), 150f)), 0f);
    // the W1 sign: a post (a box) and two arms on yaw pivots, toward the Camp 3 branch and the pump
    const float signX = 127.0f, signZ = 72.8f, postH = 2.0f, postW = 0.12f, armL = 0.8f, armH = 0.15f;
    var sign = kit.Group("W1Sign", L, V(signX, G(signX, signZ), signZ), 0f); float sg = G(signX, signZ);
    Slab("Post", sign, V(signX, sg + postH * 0.5f, signZ), V(postW, postH, postW), planks, true);
    foreach (var (n, tx, tz, up) in new[] { ("ArmCamp3", 96f, 104f, 1.75f), ("ArmPump", 156f, 92f, 1.5f) })
    {
        var piv = kit.Group(n, sign, V(signX, sg + up, signZ), UnityEngine.Mathf.Atan2(tx - signX, tz - signZ) * UnityEngine.Mathf.Rad2Deg);
        kit.Slab("Arm", piv, V(0f, 0f, armL * 0.5f + postW * 0.5f), V(0.03f, armH, armL), planks);
    }
}

// ================= T1: the FaceRock round the hollow =================
// the Ground815/Stops/FaceRock rocks within hollowR of the hollow's centre stand on the floor's edge, not on a stop edge: each LOD0 mesh gets
// its convex hull here (main3_8_18a_solid.cs then makes exact or drops any hull that enters a trail tread, and no longer strips them)
const float hollowR = 14f; int hullsAdded = 0;
{
    var fr = kit.Root("Ground815") != null ? kit.Root("Ground815").transform.Find("Stops/FaceRock") : null;
    if (fr == null) notes.Add("no Ground815/Stops/FaceRock");
    else foreach (var lod in fr.GetComponentsInChildren<UnityEngine.LODGroup>())
    {
        if (new UnityEngine.Vector2(lod.transform.position.x - 78f, lod.transform.position.z - 146f).magnitude > hollowR) continue; var lods = lod.GetLODs(); if (lods.Length == 0) continue;
        foreach (var r in lods[0].renderers)
        {
            var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null || r.GetComponent<UnityEngine.Collider>() != null) continue;
            var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; hullsAdded++;
        }
    }
}

// ================= WARPS =================
var warps = kit.Root("DevWarps").transform;
foreach (var (n, x, z, yaw) in new[] { ("Camp_3", 75.0f, 143.6f, 30f), ("Camp_3_Rim", 97.8f, 142.0f, 268f) })
{ var w = warps.Find(n); if (w == null) { notes.Add("no warp " + n); continue; } w.position = V(x, G(x, z) + 0.2f, z); w.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); }

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | removed by name " + namedGone + " of " + named.Length + (namedMissing.Count > 0 ? " (not found: " + string.Join(", ", namedMissing) + ")" : "") + ", in keep-out zones " + zoneGone + (zoneNames.Count > 0 ? " (" + string.Join("; ", zoneNames) + ")" : "")
    + ", leftover props " + propsGone + ", footbridge " + (bridgeGone ? "gone" : "absent") + " | Snag line pieces " + pieceN + " (rope " + ropeNote + ") | FaceRock hulls added " + hullsAdded + " | log steps " + logsMade + " | creek: upper " + upper.Count + " and lower " + lower.Count + " samples, cells cut " + carved + ", stones " + stonesN + ", cascade rocks " + rocksN
    + (pushNotes.Count > 0 ? ", AT THE VALLEY EDGE: " + string.Join("; ", pushNotes) : "") + " | tent " + tentNote + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
