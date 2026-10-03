// Main3 task 8.27, the cave to CaveLayout.md draft 2 (Sable 2026-10-02, 6e94aba; CaveLayout_UI.md, CaveLayout_Story.md, CaveLayout_Sound.md,
// Marlow's 827 paper check with his S1 to S8 samples): layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (absolute
// places; new pieces rebuilt under Cave/Layout827, Ground815/Stops/Hedge_CaveRim and the spur's RopeRail827). In the runner after
// main3_8_26_camp3.cs and before main3_8_18a_solid.cs. Needs main3_8_27_warplabels.patch (Assets/Scripts/Dev/DevWarpLabels.cs) for the two
// new warps' rows in the dev panel; the warps themselves work without it.
// Props sit at yaw 0, 90, 180 or 270, or carry a box from their own meshes (PlaceKit.FitExact).
// REMOVALS (R), by name within removeTol m: the old rope rail POI (106.8, 57.6); TrailEdges CS_Stone_5 (53.22, 36.83) in the passage and
//   CS_Stone_8 (52.84, 41.94) in the mouth strip; ChamberDressing Boulder_1 (89.0, 5.51) (the seat shelf replaces it) and RubbleSparse_2
//   (82.86, 18.68) (under the battery bank); SideRoom Boulder_0 (97.50, 13.79) (in V9's opening).
// V1  the spur's rope rail on the drop side: posts postH tall every postStep m along the tread from P56 to P70 (W1 to cave), railOff m
//     south of the centre line (square to it), on the tread's ground; a rope at ropeH; a box from the ground to postH under each span.
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
//     rim's first ground over rimTop, tied into the slope at each end (rimTie m past it).
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
var rope = kit.Tinted("Places_Rope", planksPath, Hex("#8C7A58"), UnityEngine.Vector2.one); var cableMat = kit.Tinted("Places_Cable", concrete, Hex("#D8D2C0"), UnityEngine.Vector2.one);
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

// ================= V1: the rope rail on the drop side =================
const float postH = 1.0f, ropeH = 0.6f, postW = 0.1f, postStep = 2f, railOff = 1.2f, railT = 0.1f; int posts = 0;
{
    var rail = kit.Fresh("RopeRail827", poiRoot, poiRoot.position, 0f); int i0 = IndexOf("P56"), i1 = IndexOf("P70");
    if (i0 < 0 || i1 <= i0) notes.Add("W1 to cave: no P56 to P70");
    else
    {
        // posts every postStep m along the tread polyline, each railOff m to its south (the drop's side), square to the tread
        var line = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (int i = i0; i <= i1; i++) line.Add(tread[i].p);
        float len = 0f; for (int i = 1; i < line.Count; i++) len += UnityEngine.Vector3.Distance(line[i - 1], line[i]); int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(len / postStep));
        UnityEngine.Vector3 At(float s, out UnityEngine.Vector3 dir) { float acc = 0f; for (int i = 1; i < line.Count; i++) { float l = UnityEngine.Vector3.Distance(line[i - 1], line[i]); dir = (line[i] - line[i - 1]).normalized; if (acc + l >= s || i == line.Count - 1) return UnityEngine.Vector3.Lerp(line[i - 1], line[i], UnityEngine.Mathf.Clamp01((s - acc) / UnityEngine.Mathf.Max(1e-4f, l))); acc += l; } dir = UnityEngine.Vector3.forward; return line[0]; }
        var feet = new System.Collections.Generic.List<UnityEngine.Vector3>();
        for (int k = 0; k <= n; k++)
        {
            var q = At(len * k / n, out var dir); var side = V(-dir.z, 0f, dir.x).normalized; if (side.z > 0f) side = -side;   // the south side, where the drop is
            var f = q + side * railOff; f.y = G(f.x, f.z); feet.Add(f);
            Slab("Post", rail, f + V(0f, postH * 0.5f, 0f), V(postW, postH, postW), planks, true); posts++;
        }
        for (int k = 1; k < feet.Count; k++)
        {
            var a = feet[k - 1]; var b = feet[k]; var mid = (a + b) * 0.5f; float l = UnityEngine.Vector3.Distance(a, b); var rot = UnityEngine.Quaternion.LookRotation(V(b.x - a.x, 0f, b.z - a.z).normalized);
            var r = Slab("Rope", rail, mid + V(0f, ropeH, 0f), V(0.03f, 0.03f, l), rope); r.transform.rotation = UnityEngine.Quaternion.LookRotation((b - a).normalized);
            var box = kit.Blocker("RailBox", rail, rail.InverseTransformPoint(mid + V(0f, postH * 0.5f, 0f)), V(railT, postH, new UnityEngine.Vector2(b.x - a.x, b.z - a.z).magnitude)); box.transform.rotation = rot;
        }
    }
}

// ================= V2: the bulbs =================
{
    var bulbs = poiRoot.Find("POI_Coloured_bulbs"); var to = P(82.15f, 52.96f);
    if (bulbs == null) notes.Add("no POI_Coloured_bulbs");
    else { var branch = bulbs.Find("DeadBranch"); var foot = branch != null ? branch.GetComponent<UnityEngine.Collider>().bounds.min.y : bulbs.position.y; float rise = G(to.x, to.y) - foot; bulbs.position += V(to.x - bulbs.position.x, rise, to.y - bulbs.position.z); }
}

// ================= V3: the passage =================
const float nx0 = 53.5f, nx1 = 55.5f, nz0 = 31.6f, nz1 = 33.4f, nFloor = -6f, nCeil = -4f;
{
    // the niche: the entrance's east wall split round it, its own floor, ceiling, back and side walls
    var ent = cave.Find("Entrance"); UnityEngine.Transform eastWall = null; if (ent != null) foreach (UnityEngine.Transform t in ent) if (t.name == "Wall_E") { var b = t.GetComponent<UnityEngine.Collider>().bounds; if (b.min.z <= nz0 && b.max.z >= nz1) eastWall = t; }
    var niche = kit.Group("Niche", L, V((nx0 + nx1) * 0.5f, nFloor, (nz0 + nz1) * 0.5f), 0f);
    if (eastWall == null && L.Find("Niche/Wall_E_S") == null) notes.Add("no Entrance/Wall_E across the niche");
    if (eastWall != null)
    {
        var b = eastWall.GetComponent<UnityEngine.Collider>().bounds; float x = b.center.x, lo = b.min.y, hi = b.max.y;
        Rock("Wall_E_S", niche, V(x, (lo + hi) * 0.5f, (b.min.z + nz0) * 0.5f), V(b.size.x, hi - lo, nz0 - b.min.z));
        Rock("Wall_E_N", niche, V(x, (lo + hi) * 0.5f, (nz1 + b.max.z) * 0.5f), V(b.size.x, hi - lo, b.max.z - nz1));
        Rock("Wall_E_Over", niche, V(x, (nCeil + hi) * 0.5f, (nz0 + nz1) * 0.5f), V(b.size.x, hi - nCeil, nz1 - nz0));
        PlaceKit.Remove(eastWall);
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
const float bulbDoorH = 1.9f; int bulbs = 0;
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
    var bb0 = Child(room, "BigBoulders_0", 89.50f, 9.51f, 0.3f); if (bb0 != null) bb0.position += V(1.0f, 0f, 0f); else if (Child(room, "BigBoulders_0", 90.50f, 9.51f, 0.3f) == null) notes.Add("no SideRoom BigBoulders_0");
    // the bulb string: from the chamber through the doorway at bulbDoorH to over the table, two sagging runs
    foreach (var t in System.Linq.Enumerable.ToArray(room.GetComponentsInChildren<UnityEngine.Transform>())) if (t != null && (t.name == "BulbLine" || t.name == "Bulb")) PlaceKit.Remove(t);
    var bulbGlow = kit.Glow("Places_BulbGlow", kit.Look.practicalColor, kit.Look.cabWindowGlowIntensity); var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
    var pts = new[] { V(86.5f, floorY + 2.4f, 12.3f), V(89.25f, floorY + bulbDoorH, 12.0f), V(96.8f, floorY + 2.6f, 11.2f) }; const float sag = 0.25f; const int perRun = 5;
    for (int r = 1; r < pts.Length; r++)
    {
        var a = pts[r - 1]; var b = pts[r]; var ln = Slab("BulbLine", room, (a + b) * 0.5f - V(0f, sag * 0.5f, 0f), V(0.02f, 0.02f, UnityEngine.Vector3.Distance(a, b)), steel); ln.transform.rotation = UnityEngine.Quaternion.LookRotation(b - a);
        for (int i = 1; i <= perRun; i++) { float u = i / (perRun + 1f); var p = UnityEngine.Vector3.Lerp(a, b, u) - V(0f, sag * 4f * u * (1f - u) + 0.1f, 0f); Slab("Bulb", room, p, V(0.1f, 0.13f, 0.1f), bulbGlow).GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; bulbs++; }
    }
    kit.Marker("StandingPoint", L, L.InverseTransformPoint(V(92.0f, floorY, 12.0f)), 270f);
}

// ================= V9: the deeper passage =================
const float dz0 = 13.2f, dz1 = 14.4f, deepH = 2.1f, dxA0 = 97.5f, dxA1 = 101.6f, dxB0 = 100.4f, dzB1 = 16.1f;
{
    var room = cave.Find("SideRoom"); var fut = room.Find("Wall_E_Future"); var deep = kit.Group("Deeper", L, V(99.5f, floorY, 14f), 0f);
    if (fut != null)
    {
        var b = fut.GetComponent<UnityEngine.Collider>().bounds; float x = b.center.x, lo = b.min.y, hi = b.max.y;
        Rock("Wall_E_Future_S", deep, V(x, (lo + hi) * 0.5f, (b.min.z + dz0) * 0.5f), V(b.size.x, hi - lo, dz0 - b.min.z));
        Rock("Wall_E_Future_N", deep, V(x, (lo + hi) * 0.5f, (dz1 + b.max.z) * 0.5f), V(b.size.x, hi - lo, b.max.z - dz1));
        Rock("Wall_E_Future_Lintel", deep, V(x, (floorY + deepH + hi) * 0.5f, (dz0 + dz1) * 0.5f), V(b.size.x, hi - floorY - deepH, dz1 - dz0));
        PlaceKit.Remove(fut);
    }
    else if (L.Find("Deeper/Wall_E_Future_S") == null) notes.Add("no SideRoom/Wall_E_Future");
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
}

// ================= V10: the narrow =================
const float narrowNear = 1.5f, narrowH = 4f, narrowStep = 2.6f, narrowX0 = 61f, narrowX1 = 72f; int narrowRocks = 0;
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
            foreach (var r in g.GetComponentsInChildren<UnityEngine.MeshRenderer>()) { var lodg = g.GetComponentInChildren<UnityEngine.LODGroup>(); if (lodg != null && lodg.GetLODs().Length > 0 && System.Array.IndexOf(lodg.GetLODs()[0].renderers, r) < 0) continue; var mf = r.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; }
            narrowRocks++;
        }
    }
}

// ================= V11: the rim band =================
const float rimX0 = 54f, rimX1 = 80f, rimStep = 1f, rimZ0 = 55f, rimZ1 = 62f, rimTop = 15.5f, rimTie = 2f, hedgeH = 2f, hedgeT = 0.6f, brushH = 1.5f, brushStep = 1.4f; int hedgeBoxes = 0, brush = 0;
{
    var stops = ground815 != null ? ground815.Find("Stops") : null; if (stops == null) { notes.Add("no Ground815/Stops"); }
    else
    {
        var hedge = kit.Fresh("Hedge_CaveRim", stops, V((rimX0 + rimX1) * 0.5f, 0f, 58.5f), 0f);
        // the band's line: at each x, the first z (going north from rimZ0) where the ground reaches rimTop, the top of the steep face
        var line = new System.Collections.Generic.List<UnityEngine.Vector3>();
        for (float x = rimX0 - rimTie; x <= rimX1 + rimTie + 1e-3f; x += rimStep) { float zz = rimZ1; for (float z = rimZ0; z <= rimZ1; z += 0.25f) if (G(UnityEngine.Mathf.Clamp(x, rimX0, rimX1), z) >= rimTop) { zz = z; break; } line.Add(V(x, G(x, zz), zz)); }
        for (int i = 1; i < line.Count; i++)
        {
            var a = line[i - 1]; var b = line[i]; var mid = (a + b) * 0.5f; float l = new UnityEngine.Vector2(b.x - a.x, b.z - a.z).magnitude; float lo = UnityEngine.Mathf.Min(a.y, b.y) - 0.3f, hi = UnityEngine.Mathf.Max(a.y, b.y) + hedgeH;
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
    + " | rim hedge boxes " + hedgeBoxes + ", brush " + brush + " | usables " + uses + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
