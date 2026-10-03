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
var notes = new System.Collections.Generic.List<string>(); string tentNote = "none", fillNote = "";
var c3 = kit.Root("Campsites") != null ? kit.Root("Campsites").transform.Find("Camp_3") : null; var d = c3 != null ? c3.Find("Dressing") : null;
var poiRoot = kit.Root("PointsOfInterest") != null ? kit.Root("PointsOfInterest").transform : null; var trails = kit.Root("Trails") != null ? kit.Root("Trails").transform : null;
if (d == null || poiRoot == null || trails == null || kit.Root("Forest") == null) return "run 8.3, 8.16 and 8.17 camp3 first (Campsites/Camp_3/Dressing, PointsOfInterest, Trails, Forest)";
UnityEngine.Physics.SyncTransforms();
float G(float x, float z) => kit.H(x, z);
var L = kit.Fresh("Layout826", c3, c3.position, 0f);
// the stand-in usable (8.5's ToggleColorInteractable) with Camp3Layout_UI's word, on every interactable (Wren 2026-10-03, a standing rule)
const float r3BodyR = 0.3f, r3BodyH = 1.0f; int uses = 0; void Use(UnityEngine.GameObject g, string prompt, UnityEngine.Renderer target) { if (g == null) return; var u = g.GetComponent<ToggleColorInteractable>() ?? g.AddComponent<ToggleColorInteractable>(); var so = new UnityEditor.SerializedObject(u); so.FindProperty("prompt").stringValue = prompt; so.FindProperty("target").objectReferenceValue = target != null ? target : g.GetComponentInChildren<UnityEngine.Renderer>(); so.ApplyModifiedPropertiesWithoutUndo(); uses++; }
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
    // R3's stand-in body on the log's south end (Wren 2026-10-03: R3 Talk, the spot exists): a seated capsule with the usable
    if (r3 != null) { PlaceKit.Remove(r3.Find("StandInBody")); var body = new UnityEngine.GameObject("StandInBody"); body.transform.SetParent(r3, false); body.transform.localPosition = V(0f, r3BodyH * 0.5f, 0f); var cap = body.AddComponent<UnityEngine.CapsuleCollider>(); cap.radius = r3BodyR; cap.height = r3BodyH; Use(body, "Talk", log != null ? log.GetComponentInChildren<UnityEngine.Renderer>() : null); }   // no figure yet: the seat log takes the stand-in's colour
    // the easel (its painting is local -z: facing 284 puts its local +z on 104), the stool 1.6 m in front, a paint box
    var easel = d.Find("Easel"); if (easel != null) { easel.rotation = UnityEngine.Quaternion.Euler(0f, 284f - 180f, 0f); easel.position = V(80.3f, G(80.3f, 148.8f), 148.8f); } else notes.Add("no Dressing/Easel");
    // the canvas's box (8.26 gate, Pim 1: the eye ray from the easel stand met nothing): 0.9 x 1.1 x 0.05 on its face, with the usable
    var theCanvas = easel != null ? easel.Find("TheCanvas") : null;
    if (theCanvas != null) { PlaceKit.Remove(theCanvas.Find("CanvasBox")); var cbx = new UnityEngine.GameObject("CanvasBox"); cbx.transform.SetParent(theCanvas, false); cbx.transform.localPosition = V(0f, 0.55f, 0f); var bcx = cbx.AddComponent<UnityEngine.BoxCollider>(); bcx.size = V(0.9f, 1.1f, 0.05f); var paintR = theCanvas.Find("Painting") != null ? theCanvas.Find("Painting").GetComponent<UnityEngine.Renderer>() : null; Use(cbx, "Study the painting", paintR); }
    else notes.Add("no Easel/TheCanvas");
    PlaceKit.Remove(L.Find("PaintBox")); var box = kit.Ground(PlaceKit.CI + "Props/CITW_Crate", L, 80.6f, 147.6f, 0f, 0.6f, false); if (box != null) { box.name = "PaintBox"; PlaceKit.FitExact(box); }
    // the plank table, one box; the job form on it under its mug
    const float tblX0 = 77.9f, tblX1 = 79.1f, tblZ0 = 147.0f, tblZ1 = 147.6f, tblTop = 0.75f, tblT = 0.04f, legW = 0.05f;
    float tx = (tblX0 + tblX1) * 0.5f, tz = (tblZ0 + tblZ1) * 0.5f, tg = G(tx, tz);
    var table = kit.Group("PlankTable", L, V(tx, tg, tz), 0f);
    Slab("Top", table, V(tx, tg + tblTop - tblT * 0.5f, tz), V(tblX1 - tblX0, tblT, tblZ1 - tblZ0), planks);
    foreach (var (lx, lz) in new[] { (tblX0 + legW, tblZ0 + legW), (tblX1 - legW, tblZ0 + legW), (tblX0 + legW, tblZ1 - legW), (tblX1 - legW, tblZ1 - legW) }) Slab("Leg", table, V(lx, tg + (tblTop - tblT) * 0.5f, lz), V(legW, tblTop - tblT, legW), planks);
    PlaceKit.FitExact(table.gameObject); Use(table.gameObject, "Examine", null);   // the job form on it (Camp3Layout_UI: Examine)
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
    // the fills (Marlow's area flood found pockets between the new hulls, the tent and the canvases): collider-only boxes (no renderer:
    // Vesper's grey faces at the bank are the canvas backs), each top no more than fillOver over the highest rock beside it (Wren 2026-10-03)
    const float fillOver = 0.3f, fillNear = 0.5f, fillStep = 0.25f;
    // the visible surface beside a fill: the first drawn mesh or terrain under wantTop, every fillStep m over the footprint widened by fillNear
    // (rays from wantTop down against temporary exact colliders on the drawn meshes there; colliders and blockers do not count)
    float RockTop(float x0, float x1, float z0, float z1, float from)
    {
        float top = float.MinValue; var temps = new System.Collections.Generic.HashSet<UnityEngine.Collider>();
        var near = new UnityEngine.Bounds(V((x0 + x1) * 0.5f, from, (z0 + z1) * 0.5f), V(x1 - x0 + fillNear * 2f, 40f, z1 - z0 + fillNear * 2f));
        foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
        { if (!mr.enabled || !mr.gameObject.activeInHierarchy || !mr.bounds.Intersects(near)) continue; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
        UnityEngine.Physics.SyncTransforms();
        try
        {
            for (float x = x0 - fillNear; x <= x1 + fillNear + 1e-3f; x += fillStep) for (float z = z0 - fillNear; z <= z1 + fillNear + 1e-3f; z += fillStep)
            {
                float best = float.MinValue; foreach (var h in UnityEngine.Physics.RaycastAll(V(x, from, z), UnityEngine.Vector3.down, 40f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
                    if ((temps.Contains(h.collider) || h.collider is UnityEngine.TerrainCollider) && h.point.y > best) best = h.point.y;
                top = UnityEngine.Mathf.Max(top, best);
            }
        }
        finally { foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); UnityEngine.Physics.SyncTransforms(); }
        return top;
    }
    void Fill(string n, float x0, float x1, float z0, float z1, float bottom, float wantTop)
    {
        float rt = RockTop(x0, x1, z0, z1, wantTop), top = rt > float.MinValue ? UnityEngine.Mathf.Min(wantTop, rt + fillOver) : wantTop; if (top - bottom < 0.1f) return;
        kit.Blocker(n, L, L.InverseTransformPoint(V((x0 + x1) * 0.5f, (bottom + top) * 0.5f, (z0 + z1) * 0.5f)), V(x1 - x0, top - bottom, z1 - z0)); fillNote += n + " top " + F(top) + " (rock " + F(rt) + "); ";
    }
    // behind the canvases, to the rock (the first flood found a 20-place pocket at (69.5, -3.2, 146.0) there)
    var fcb = PlaceKit.MeshBounds(fc.gameObject); Fill("BehindCanvases", 68.0f, fcb.max.x, fcb.min.z - 0.1f, fcb.max.z + 0.1f, fcb.min.y, fcb.max.y);
    // the fire: one box from its meshes, so its interaction ray has something to meet
    if (fire != null) PlaceKit.FitExact(fire.gameObject); if (fire != null) Use(fire.gameObject, "Sit by the fire", null);
    // the slot along the tent's south face over BigBoulders_3, and along its west face over Boulder_2's foot (traps at (72.6, -3.3, 139.6),
    // (73.4, -3.3, 139.3), (74.6, -2.6, 138.6), (72.6, -3.1, 140.0) and (72.2, -2.8, 139.6) in the rocks' hollows), open east to the floor
    { float gg = G(74.0f, 138.8f); Fill("TentRockGap", 72.55f, 75.5f, 138.0f, 139.64f, gg - 0.5f, gg + 1.7f); }
    { float wg = G(72.2f, 139.9f); Fill("TentRockGapWest", 71.4f, 72.96f, 138.0f, 141.8f, wg - 0.5f, wg + 1.7f); }
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
const int logSteps = 19; const float rampExtra = 0.6f, logLen = 1.8f, logGirth = 0.3f, rampW = 1.8f, rampT = 0.2f;
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
            // across the tread, one step each (8.26 gate, Marlow 5 and Vesper: they lay along it as one beam): the mesh's long local axis
            // found from its own bounds and turned square to the path; its top on the StairRamp's top line, so no foot sinks into it
            var ms = lg.GetComponentInChildren<UnityEngine.MeshFilter>().sharedMesh.bounds.size; bool longX = ms.x >= ms.z;
            lg.transform.rotation = UnityEngine.Quaternion.LookRotation(flat) * UnityEngine.Quaternion.Euler(0f, longX ? 0f : 90f, 0f);   // the long axis along local x lies across the path at yaw 0 from the path's frame
            lg.transform.localScale = longX ? V(logLen / ms.x, logGirth / ms.y, logGirth / ms.z) : V(logGirth / ms.x, logGirth / ms.y, logLen / ms.z);
            var mfT = lg.GetComponentInChildren<UnityEngine.MeshFilter>().transform; var longW = mfT.TransformDirection(longX ? UnityEngine.Vector3.right : UnityEngine.Vector3.forward); longW.y = 0f;
            var across = V(-flat.z, 0f, flat.x); if (longW.sqrMagnitude > 1e-6f) lg.transform.rotation = UnityEngine.Quaternion.FromToRotation(longW.normalized, UnityEngine.Vector3.Dot(longW, across) >= 0f ? across : -across) * lg.transform.rotation;   // a child turned inside the prefab still ends across
            float stepTop = UnityEngine.Mathf.Lerp(pts[0].y, pts[pts.Count - 1].y, s / lens[pts.Count - 1]);
            var mb = PlaceKit.MeshBounds(lg); lg.transform.position += V(q.x - mb.center.x, stepTop - mb.max.y, q.z - mb.center.z); logsMade++;
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
const float bedUnder8 = 0.1f, bankCut = 0.4f, bankProbe = 2.5f, waterHalf = 0.6f, bankW = 1.0f, bankSlope = 0.6f, waterDepth = 0.1f, sampleStep = 0.25f, stoneStep = 4f, cascadeDrop = 0.3f;
const float poolX0 = 82.1f, poolX1 = 85.1f, poolZ0 = 148.45f, poolZ1 = 150.95f, poolSurf = -4.25f, poolBed = -4.4f, poolBank = 0.5f;
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
    // the dam: debris over the sink, debrisW x debrisH x debrisD with its top debrisH over the floor, a box with the usable (8.26 gate, Pim 2:
    // the sink's box sat under the floor, out of the stand's reach); reached from the dam stand facing 45, about 40 degrees down
    const float debrisW = 0.8f, debrisH = 0.4f, debrisD = 0.6f; float floorAt = G(sinkAt.x, sinkAt.y - 0.6f);   // the floor south of the lip
    var dam = kit.Group("Dam", L, V(sinkAt.x, floorAt, sinkAt.y), 0f);
    var rub = kit.Fill(PlaceKit.BK + "Rocks/RubbleSparse_1", dam, V(0f, 0f, 0f), V(debrisW, debrisH, debrisD));
    var dbx = dam.gameObject.AddComponent<UnityEngine.BoxCollider>(); dbx.center = V(0f, debrisH * 0.5f, 0f); dbx.size = V(debrisW, debrisH, debrisD); Use(dam.gameObject, "Clear the dam", rub != null ? rub.GetComponentInChildren<UnityEngine.Renderer>() : null);
    kit.Marker("DamStand", L, L.InverseTransformPoint(V(82.6f, G(82.6f, 147.2f), 147.2f)), 45f);
    // the spring: water out of the rocks at the south trench's foot
    var springG = kit.Group("Spring", L, V(spring.x, G(spring.x, spring.y), spring.y), 0f);
    for (int i = 0; i < 3; i++) kit.Ground(PlaceKit.CS + "Rocks and Stones/CS_Stone_" + (5 + i), springG, spring.x - 0.6f + i * 0.6f, spring.y - 0.5f, i * 70f, 0.8f, false, 0.1f);
    for (int k = 0; k < upper.Count; k += 4) kit.ClearDetail(V(upper[k].p.x, 0f, upper[k].p.y), waterHalf + 0.5f);
    for (int k = 0; k < lower.Count; k += 4) kit.ClearDetail(V(lower[k].p.x, 0f, lower[k].p.y), waterHalf + 0.5f);
    kit.ClearDetail(V((poolX0 + poolX1) * 0.5f, 0f, (poolZ0 + poolZ1) * 0.5f), 2.2f);
    // frame F6's line, its eye on the W1 leg to the spring, cleared of fern (the 8.26 gate: a fern filled the frame)
    var f6Eye = P(84.43f, 122.24f); const float f6Step = 0.75f, f6Clear = 0.9f; float f6Len = UnityEngine.Vector2.Distance(f6Eye, spring);
    for (float s = 0f; s <= f6Len; s += f6Step) { var q = UnityEngine.Vector2.Lerp(f6Eye, spring, s / f6Len); kit.ClearDetail(V(q.x, 0f, q.y), f6Clear); }
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

// ================= the west rim's stop (8.26 gate, Marlow 4, Wren 2026-10-03) =================
// a sprint off the west rim landed on the tent's, the fire's and the fills' tops: a brush band with a hedge box (Ground815/Stops, a stop, as
// the other hedges) along the rim's crest over the camp, from bearing rimFrom to rimTo round the hollow's centre: on each bearing (every
// rimStep degrees) the first ground at rimLevel or over going out from rimIn m, set rimBack m further out
const float rimFrom = 200f, rimTo = 340f, rimStepDeg = 4f, rimIn = 8f, rimOut = 22f, rimLevel = 3.5f, rimBack = 0.6f, hedgeH = 2f, hedgeT = 0.6f, brushH = 1.5f, brushStep = 1.4f; int rimBoxes = 0, rimBrush = 0;
{
    var stops = kit.Root("Ground815") != null ? kit.Root("Ground815").transform.Find("Stops") : null;
    if (stops == null) notes.Add("no Ground815/Stops");
    else
    {
        var hedge = kit.Fresh("Hedge_Camp3Rim", stops, V(78f, 0f, 146f), 0f); var crest = new System.Collections.Generic.List<UnityEngine.Vector3>();
        for (float b = rimFrom; b <= rimTo + 1e-3f; b += rimStepDeg)
        {
            var dir = V(UnityEngine.Mathf.Sin(b * UnityEngine.Mathf.Deg2Rad), 0f, UnityEngine.Mathf.Cos(b * UnityEngine.Mathf.Deg2Rad));
            for (float r = rimIn; r <= rimOut; r += 0.25f) { var q = V(78f, 0f, 146f) + dir * r; if (G(q.x, q.z) >= rimLevel) { q += dir * rimBack; crest.Add(V(q.x, G(q.x, q.z), q.z)); break; } }
        }
        for (int i = 1; i < crest.Count; i++)
        {
            var a = crest[i - 1]; var c = crest[i]; var mid = (a + c) * 0.5f; float l = new UnityEngine.Vector2(c.x - a.x, c.z - a.z).magnitude; if (l < 0.05f) continue; float lo = UnityEngine.Mathf.Min(a.y, c.y) - 0.3f, hi = UnityEngine.Mathf.Max(a.y, c.y) + hedgeH;
            var box = kit.Blocker("HedgeCollider", hedge, hedge.InverseTransformPoint(V(mid.x, (lo + hi) * 0.5f, mid.z)), V(hedgeT, hi - lo, l + 0.1f)); box.transform.rotation = UnityEngine.Quaternion.LookRotation(V(c.x - a.x, 0f, c.z - a.z).normalized); box.layer = 2; rimBoxes++;
        }
        float run = 0f, last = -brushStep;
        for (int i = 1; i < crest.Count; i++)
        {
            float seg = new UnityEngine.Vector2(crest[i].x - crest[i - 1].x, crest[i].z - crest[i - 1].z).magnitude;
            for (float u = 0f; u < seg; u += 0.2f) { if (run + u - last < brushStep) continue; last = run + u; var q = UnityEngine.Vector3.Lerp(crest[i - 1], crest[i], u / seg); var g = kit.Ground(PlaceKit.CS + "Vegetation/CS_Bush_Large_" + (1 + rimBrush % 2), hedge, q.x, q.z, q.x * 53f, 1f, false, 0.1f); if (g == null) continue; var gb = PlaceKit.MeshBounds(g); g.transform.localScale *= brushH / UnityEngine.Mathf.Max(0.1f, gb.size.y); PlaceKit.StripColliders(g); rimBrush++; }
            run += seg;
        }
    }
}

// ================= the Snag's bark (8.26 gate, cheap only: its texture stretched over the scaled trunk) =================
// a copy of each of its materials with the main texture tiled snagTile times up the trunk, on the Snag's renderers
const float snagTileX = 2f, snagTileY = 6f; int snagMats = 0;
{
    var snag = kit.Root("Giants") != null ? kit.Root("Giants").transform.Find("Heroes/Snag") : null;
    if (snag != null) foreach (var r in snag.GetComponentsInChildren<UnityEngine.MeshRenderer>())
    {
        var ms = r.sharedMaterials; for (int i = 0; i < ms.Length; i++) { var m = ms[i]; if (m == null || m.name.StartsWith("Places_SnagBark")) continue; var nm = kit.Tinted("Places_SnagBark_" + m.name, UnityEditor.AssetDatabase.GetAssetPath(m), m.HasProperty("_BaseColor") ? m.GetColor("_BaseColor") : UnityEngine.Color.white, new UnityEngine.Vector2(snagTileX, snagTileY)); if (nm != null) { ms[i] = nm; snagMats++; } }
        r.sharedMaterials = ms;
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
    + (pushNotes.Count > 0 ? ", AT THE VALLEY EDGE: " + string.Join("; ", pushNotes) : "") + " | tent " + tentNote + " | fills " + fillNote + "| usables " + uses + " | west rim hedge boxes " + rimBoxes + ", brush " + rimBrush + " | Snag bark materials " + snagMats + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
