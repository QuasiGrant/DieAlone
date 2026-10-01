// Main3 task 8.17, the cave mouth and its side room (Valley.md rev 11 1.5 and 6 M10; Check1_Story: gray outside, a party inside; the
// roulette table room). Run after 8.16 in Main3, edit mode; rerunnable. Every 8.8 rock box (floors, ceilings, walls, the rock over the
// mouth) takes the owned BK rock texture at about one tile per rockTile m; the day-one board takes planks and its CLOSED sign; owned
// boulders and rubble frame the mouth outside, clear of the 3 m opening. The side room (M10): x 89.5 to 97.5, z 7.5 to 15.5, floor
// -18, 3 m high, one doorway (1.2 x 2.1 m) through the chamber's east wall at z 12, its east wall a piece of its own so a passage can
// go on east later; one table, two chairs, a revolver, one lamp.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
var cave = kit.Root("Cave"); if (cave == null) return "no Cave";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
const float T = 0.5f, rockTile = 4f, floorY = -18f, roomH = 3f, rx0 = 89.5f, rx1 = 97.5f, rz0 = 7.5f, rz1 = 15.5f, doorZ = 12f, doorW = 1.2f, doorH = 2.1f;
const string rocks = "Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Materials/Rocks.mat", planks = "Assets/Materials/Planks023A_1.0x1.0.mat";
int Step(float m) { foreach (var s in new[] { 1, 2, 3, 4, 6, 8 }) if (m / rockTile <= s * 1.3f) return s; return 8; }
UnityEngine.Material RockFor(UnityEngine.Vector3 size) { var d = new[] { size.x, size.y, size.z }; System.Array.Sort(d); int u = Step(d[1]), v = Step(d[2]); return kit.Tinted("Places_CaveRock_" + u + "x" + v, rocks, Hex("#7A746C"), new UnityEngine.Vector2(u, v)); }
UnityEngine.GameObject RockBox(string name, UnityEngine.Transform parent, UnityEngine.Vector3 c, UnityEngine.Vector3 s)
{
    var g = kit.Slab(name, parent, parent.InverseTransformPoint(c), s, null, default, true); g.GetComponent<UnityEngine.Renderer>().sharedMaterial = RockFor(s); return g;
}

// ---- rock on every 8.8 box
int retex = 0;
foreach (UnityEngine.Transform piece in cave.transform)
{
    if (piece.name == "Resident_Cave_Spot" || piece.name == "SideRoom" || piece.name == "MouthDressing" || piece.name == "ChamberDressing") continue;
    foreach (var r in piece.GetComponentsInChildren<UnityEngine.MeshRenderer>())
    {
        if (r.transform.parent != null && r.transform.parent.name == "DayOneBoard") continue;
        r.sharedMaterial = RockFor(r.transform.lossyScale); retex++;
    }
}
// ---- the day-one board: plank boards and the sign
var board = cave.transform.Find("Mouth/DayOneBoard");
if (board != null)
{
    foreach (UnityEngine.Transform t in board)
    {
        var r = t.GetComponent<UnityEngine.Renderer>(); if (r == null) continue;
        if (t.name == "Plank") r.sharedMaterial = kit.Tinted("Places_BoardPlank", planks, Hex("#6B5540"), new UnityEngine.Vector2(3f, 0.3f));
        if (t.name == "Sign") { r.sharedMaterial = kit.Tinted("Places_BoardSign", planks, Hex("#B8A888"), UnityEngine.Vector2.one); if (t.parent.Find("Label") == null) kit.Label(t, "CLOSED - UNSAFE", Hex("#6A1E14"), 30, true); }
    }
}
// ---- outside the mouth (RebuildSpecs 3.2 and 3.3, Vesper 2026-10-01: the mouth still read as a rectangle). The 8.8 Entrance opening is
// x 50.5 to 53.5 at z mouthZ, floor about -6. Two BK BigBoulders lean in as jambs (leanDeg, tops toward the opening), their inner faces
// jambGap m apart at the floor, overlapping the box edges, so the opening narrows upward; one BigBoulder overhang over them, its underside
// openH m over the mouth floor and its front overhangProud m proud of the face; the Entrance boxes take a near-black rock (the void) so no
// straight box edge shows behind the rock; scree and brush at the jambs' feet, none in the opening. Every mouth rock collides as its
// convex hull. (8.17 gate: the two top-ground boulders over the lintel are gone; the hand walk found a pocket between the east one, the
// flank and the box.)
var md = kit.Fresh("MouthDressing", cave.transform, V(52f, kit.H(52f, 39f), 39f), 0f);
const float mouthX = 52f, mouthZ = 37.9f, jambGap = 3.4f, leanDeg = 10f, jambW = 6.2f, openH = 2.25f, overhangProud = 1.5f, overhangW = 7f, footStep = 1.6f;
float mouthFloor = -6f; { var mf = cave.transform.Find("Entrance/Floor"); if (mf != null) mouthFloor = mf.GetComponent<UnityEngine.Collider>().bounds.max.y; }
foreach (var side in new[] { -1f, 1f })
{
    var j = kit.Spawn(PlaceKit.BK + "Rocks/BigBoulders_" + (side < 0f ? "4" : "5"), md); if (j == null) continue;
    j.transform.rotation = UnityEngine.Quaternion.Euler(0f, mouthX * 7f * side, 0f); j.transform.localScale = UnityEngine.Vector3.one * (jambW / 6.2f);
    j.transform.rotation = UnityEngine.Quaternion.AngleAxis(side * leanDeg, UnityEngine.Vector3.forward) * j.transform.rotation;   // the top leans toward the opening
    var jb = PlaceKit.MeshBounds(j); float inner = mouthX + side * jambGap * 0.5f;   // the inner face at the floor
    j.transform.position += V(inner - (side < 0f ? jb.max.x : jb.min.x), mouthFloor - 0.4f - jb.min.y, mouthZ - jb.center.z);
}
var over = kit.Spawn(PlaceKit.BK + "Rocks/BigBoulders_2", md);
if (over != null) { PlaceKit.StripColliders(over); over.transform.rotation = UnityEngine.Quaternion.Euler(0f, 40f, 180f); float s0 = overhangW / 6.2f; over.transform.localScale = V(s0, s0 * 0.8f, s0); var ob = PlaceKit.MeshBounds(over); over.transform.position += V(mouthX - ob.center.x, mouthFloor + openH - ob.min.y, mouthZ + overhangProud - ob.max.z); }
var voidMat = kit.Tinted("Places_CaveVoid", rocks, Hex("#101214"), UnityEngine.Vector2.one);
var entrance = cave.transform.Find("Entrance"); if (entrance != null) foreach (var r in entrance.GetComponentsInChildren<UnityEngine.MeshRenderer>()) r.sharedMaterial = voidMat;
kit.Ground(PlaceKit.BK + "Rocks/Boulder_4", md, 48.9f, 41.2f, 110f, 0.3f, true, 0.2f);
foreach (var side in new[] { -1f, 1f }) for (int i = 0; i < 3; i++)   // scree and brush at each jamb's foot, outside the opening
{
    float fx = mouthX + side * (jambGap * 0.5f + 1f + i * footStep), fz = mouthZ + 1f + (i % 2) * 0.8f;
    kit.Ground(PlaceKit.BK + "Rocks/RubbleSparse_" + (1 + i % 2), md, fx, fz, fx * 13f, 0.5f, false, 0.1f);
    kit.Ground(PlaceKit.BK + "Plants/ThinFern" + (1 + i), md, fx + side * 0.6f, fz + 0.6f, fx * 29f, 1.2f, false, 0.05f);
}
kit.Ground(PlaceKit.CC + "Props/C_Candle_2", md, 50.2f, 38.2f, 0f, 1f, false);   // stubs the cult left at the door

// ---- the side room (M10): the chamber's east wall is rebuilt round a doorway at z 12
var chamber = cave.transform.Find("Chamber"); if (chamber == null) return "no Cave/Chamber";
var oldE = chamber.Find("Wall_E");
float wallTop = -9.5f, wallBottom = -18.5f, wx = 89.25f, wz0 = 2.5f, wz1 = 21.5f;
if (oldE != null) { var b = oldE.GetComponent<UnityEngine.Collider>().bounds; wallTop = b.max.y; wallBottom = b.min.y; wz0 = b.min.z; wz1 = b.max.z; PlaceKit.Remove(oldE); }
var room = kit.Fresh("SideRoom", cave.transform, V((rx0 + rx1) * 0.5f, floorY, (rz0 + rz1) * 0.5f), 0f);
float dz0 = doorZ - doorW * 0.5f, dz1 = doorZ + doorW * 0.5f;
RockBox("Chamber_Wall_E_S", room, V(wx, (wallTop + wallBottom) * 0.5f, (wz0 + dz0) * 0.5f), V(T, wallTop - wallBottom, dz0 - wz0));
RockBox("Chamber_Wall_E_N", room, V(wx, (wallTop + wallBottom) * 0.5f, (dz1 + wz1) * 0.5f), V(T, wallTop - wallBottom, wz1 - dz1));
RockBox("Chamber_Wall_E_Lintel", room, V(wx, (wallTop + floorY + doorH) * 0.5f, doorZ), V(T, wallTop - floorY - doorH, doorW));
RockBox("Floor", room, V((rx0 + rx1) * 0.5f, floorY - T * 0.5f, (rz0 + rz1) * 0.5f), V(rx1 - rx0 + T, T, rz1 - rz0 + 2f * T));
RockBox("Ceiling", room, V((rx0 + rx1) * 0.5f, floorY + roomH + T * 0.5f, (rz0 + rz1) * 0.5f), V(rx1 - rx0 + T, T, rz1 - rz0 + 2f * T));
RockBox("Wall_N", room, V((rx0 + rx1) * 0.5f, floorY + roomH * 0.5f, rz1 + T * 0.5f), V(rx1 - rx0 + T, roomH + 2f * T, T));
RockBox("Wall_S", room, V((rx0 + rx1) * 0.5f, floorY + roomH * 0.5f, rz0 - T * 0.5f), V(rx1 - rx0 + T, roomH + 2f * T, T));
RockBox("Wall_E_Future", room, V(rx1 + T * 0.5f, floorY + roomH * 0.5f, (rz0 + rz1) * 0.5f), V(T, roomH + 2f * T, rz1 - rz0 + 2f * T));   // a passage may open here later
// the table: one table in the middle, a chair each side, the revolver and a lamp on it, a single cage light over it
var tbl = kit.Group("RouletteTable", room, V(93.8f, floorY, 11.5f), 90f);
var tblMesh = kit.On(PlaceKit.CE + "Furniture/Table", tbl, V(0f, 0f, 0f), 0f, 0.6f, true, null, true);
float tTop = tblMesh != null ? PlaceKit.LocalBounds(tblMesh, tbl).max.y : 0.58f;
// 8.17 gate (Quill): the guest's chair (toward the door) squared up to the table, his own turned easy and pushed back
kit.On(PlaceKit.CE + "Furniture/Chair", tbl, V(0f, 0f, -0.95f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Furniture/Chair", tbl, V(0.2f, 0f, 1.25f), 205f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Weapons/Pistol", tbl, V(0.1f, tTop, 0f), 70f, 1f, false, V(0f, 0f, 90f), true);   // lying on its side
kit.On(PlaceKit.CC + "Props/C_Candle_1", tbl, V(-0.45f, tTop, 0.2f), 0f, 1f, false, null, true);
// a party carried in (Quill): a bottle and two glasses, the guest's poured; the table edge kept free
kit.On(PlaceKit.CE + "Decoration_Out/Glass_Bottle", tbl, V(-0.1f, tTop, 0.25f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Glass_Cup_1", tbl, V(0.15f, tTop, -0.3f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Glass_Cup_2", tbl, V(-0.2f, tTop, 0.35f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Lamps/Cage_Light", tbl, V(0f, roomH - 0.05f, 0f), 0f);
kit.Practical("TableLamp", tbl, V(0f, roomH - 0.5f, 0f), 5f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Full);
kit.On(PlaceKit.CC + "Props/C_Crate_Small_1", room, room.InverseTransformPoint(V(96.6f, floorY, 14.6f)), 20f, 1f, true, null, true);
// the bulb string, carried in from the chamber through the doorway and hung over the table (Quill)
var bulbGlow = kit.Glow("Places_BulbGlow", kit.Look.practicalColor, kit.Look.cabWindowGlowIntensity);
var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
var bulbA = V(86.5f, floorY + 2.4f, 12.3f); var bulbB = V(96.8f, floorY + 2.6f, 11.2f); const int roomBulbs = 9; const float roomSag = 0.35f;
var line = kit.Slab("BulbLine", room, room.InverseTransformPoint((bulbA + bulbB) * 0.5f - V(0f, roomSag * 0.5f, 0f)), V(0.02f, 0.02f, UnityEngine.Vector3.Distance(bulbA, bulbB)), steel);
line.transform.rotation = UnityEngine.Quaternion.LookRotation(bulbB - bulbA);
for (int i = 1; i <= roomBulbs; i++) { float u = i / (roomBulbs + 1f); var p = UnityEngine.Vector3.Lerp(bulbA, bulbB, u) - V(0f, roomSag * 4f * u * (1f - u) + 0.1f, 0f); kit.Slab("Bulb", room, room.InverseTransformPoint(p), V(0.1f, 0.13f, 0.1f), bulbGlow).GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; }
// a loose rock goes to the nearest cave wall (rays on wallRays bearings within wallReach m), its centre on the wall face, and down onto the
// floor under it, sunk rockSink: rockfall at the wall's foot (8.17 gate, Marlow 10: Boulder_1 hung 2.8 m over the chamber floor;
// RebuildSpecs 3.4: in the side room boulders sit on the floor or the walls, the ceiling is the rock resting on the walls)
const float wallReach = 8f, rockSink = 0.3f; const int wallRays = 16; int seated = 0; float airMax = 0f;
void Seat(UnityEngine.GameObject g, UnityEngine.Vector3 at)
{
    UnityEngine.Physics.SyncTransforms(); float bestD = wallReach; UnityEngine.Vector3 wallAt = at;
    for (int k = 0; k < wallRays; k++) { var dir = UnityEngine.Quaternion.Euler(0f, k * 360f / wallRays, 0f) * UnityEngine.Vector3.forward; if (UnityEngine.Physics.Raycast(at, dir, out var wh, bestD, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore) && wh.collider.transform.IsChildOf(cave.transform)) { bestD = wh.distance; wallAt = wh.point; } }
    var gb = PlaceKit.MeshBounds(g); g.transform.position += V(wallAt.x - gb.center.x, 0f, wallAt.z - gb.center.z);
    var from = V(wallAt.x, at.y, wallAt.z) - V(wallAt.x - at.x, 0f, wallAt.z - at.z).normalized * 0.3f;
    if (UnityEngine.Physics.Raycast(from, UnityEngine.Vector3.down, out var fh, 20f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore))
    { gb = PlaceKit.MeshBounds(g); g.transform.position += V(0f, fh.point.y - rockSink - gb.min.y, 0f); seated++; airMax = UnityEngine.Mathf.Max(airMax, PlaceKit.MeshBounds(g).min.y - fh.point.y); }
}
// 8.17 gate (Vesper): rock broken in along the room's north and east walls, a timber prop under a ceiling beam
foreach (var rb in new[] { (V(92f, floorY + 1.2f, rz1 - 0.6f), 1.6f, "BigBoulders_1"), (V(95.5f, floorY + 0.8f, rz1 - 0.5f), 1.3f, "Boulder_3"), (V(rx1 - 0.6f, floorY + 1.4f, 9.5f), 1.7f, "BigBoulders_3"), (V(rx1 - 0.5f, floorY + 1.8f, 13.8f), 1.2f, "Boulder_0"), (V(91f, floorY + 1.5f, 9.5f), 2f, "BigBoulders_0"), (V(95f, floorY + 1.5f, 14f), 1.8f, "BigBoulders_5") })
{
    var g = kit.Spawn(PlaceKit.BK + "Rocks/" + rb.Item3, room); if (g == null) continue; PlaceKit.StripColliders(g);
    g.transform.localScale = UnityEngine.Vector3.one * (rb.Item2 / 3f); g.transform.rotation = UnityEngine.Quaternion.Euler(rb.Item1.x * 13f, rb.Item1.z * 29f, 20f); g.transform.position += rb.Item1 - PlaceKit.MeshBounds(g).center;
    Seat(g, rb.Item1);
}
kit.Fill(PlaceKit.CI + "Building/CITW_Wood_Pillar", room, room.InverseTransformPoint(V(96.2f, floorY, 8.4f)), V(0.25f, roomH, 0.25f));
var beam = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", room); if (beam != null) { PlaceKit.StripColliders(beam); beam.transform.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); beam.transform.localScale = V(3.6f, 0.8f, 0.8f); beam.transform.position += V(96.2f, floorY + roomH - 0.15f, 11.5f) - PlaceKit.MeshBounds(beam).center; }
// the chamber (Vesper: near black and flat): a faint lantern by the side room door, rubble on the floor, rock ledges on its walls
var ch = kit.Fresh("ChamberDressing", cave.transform, V(80f, floorY, 12f), 0f);
kit.On(PlaceKit.CS + "CS_Lantern_Old", ch, ch.InverseTransformPoint(V(87.8f, floorY, 13.6f)), 0f, 1f, false, null, true);
kit.Practical("ChamberLantern", ch, ch.InverseTransformPoint(V(87.8f, floorY + 0.5f, 13.6f)), 9f, PracticalLight.Kind.Lantern, PracticalLight.ByDay.Full);
foreach (var rp in new[] { V(76f, floorY, 6f), V(83f, floorY, 19f), V(74f, floorY, 18f) }) kit.On(PlaceKit.BK + "Rocks/RubbleSparse_2", ch, ch.InverseTransformPoint(rp), rp.x * 11f, 0.6f, false, null, true);
foreach (var lp in new[] { (V(71.6f, floorY + 2.5f, 5f), "BigBoulders_2"), (V(80f, floorY + 3f, 20.9f), "BigBoulders_4"), (V(88.6f, floorY + 4.5f, 5.5f), "Boulder_1"), (V(79f, floorY + 1.2f, 3.4f), "Boulder_5") })
{
    var g = kit.Spawn(PlaceKit.BK + "Rocks/" + lp.Item2, ch); if (g == null) continue; PlaceKit.StripColliders(g);
    g.transform.localScale = UnityEngine.Vector3.one * 0.6f; g.transform.rotation = UnityEngine.Quaternion.Euler(10f, lp.Item1.x * 17f, -12f); g.transform.position += lp.Item1 - PlaceKit.MeshBounds(g).center;
    Seat(g, lp.Item1);
}
// RebuildSpecs 3.1 (Vesper, Style 2.3: fill #4A5058, light #9AA3AD, no glow): one roof crack over the side room with a cold shaft, a
// no-shadow spot light (LookTuning caveCrack*) from just under it aimed at the floor crackOff m west of the table, toward the door the
// chamber looks through; the crack a pale sliver in the ceiling rock. Fill: a no-shadow point light (LookTuning caveFill*) low in the
// chamber and one in the side room, their range short of the ground above. Grey targets are graded on the capture.
const float crackOff = 2f, crackLen = 2.2f, crackWidth = 0.25f, fillUp = 1.5f;
var look = kit.Look; var aim = V(93.8f - crackOff, floorY, 11.5f);
var crackMat = kit.Tinted("Places_CaveCrack", rocks, Hex("#9AA3AD"), UnityEngine.Vector2.one);
kit.Slab("RoofCrack", room, room.InverseTransformPoint(V(aim.x, floorY + roomH + 0.02f, aim.z)), V(crackWidth, 0.06f, crackLen), crackMat, V(0f, 25f, 0f));
UnityEngine.Light NewLight(string name, UnityEngine.Transform parent, UnityEngine.Vector3 at, UnityEngine.LightType type, UnityEngine.Color c, float intensity, float range)
{
    var go = new UnityEngine.GameObject(name); go.transform.SetParent(parent, false); go.transform.position = at;
    var l = go.AddComponent<UnityEngine.Light>(); l.type = type; l.color = c; l.intensity = intensity; l.range = range; l.shadows = UnityEngine.LightShadows.None; return l;
}
var crack = NewLight("CrackShaft", room, V(aim.x, floorY + roomH - 0.1f, aim.z), UnityEngine.LightType.Spot, look.caveCrackColor, look.caveCrackIntensity, look.caveCrackRange);
crack.spotAngle = look.caveCrackAngle; crack.transform.rotation = UnityEngine.Quaternion.LookRotation(UnityEngine.Vector3.down);
NewLight("CaveFill", ch, V(80f, floorY + fillUp, 12f), UnityEngine.LightType.Point, look.caveFillColor, look.caveFillIntensity, look.caveFillRange);
NewLight("CaveFill", room, V((rx0 + rx1) * 0.5f, floorY + fillUp, (rz0 + rz1) * 0.5f), UnityEngine.LightType.Point, look.caveFillColor, look.caveFillIntensity, look.caveFillRange);
PlaceKit.MarkerOnly(cave.transform.Find("Resident_Cave_Spot"));
// check: the terrain over the room stays above its ceiling rock (0 over it, Valley M10)
float lowest = float.MaxValue; for (float x = rx0; x <= rx1; x += 1f) for (float z = rz0; z <= rz1; z += 1f) lowest = UnityEngine.Mathf.Min(lowest, kit.H(x, z));

// 8.17 gate (hand-walk BOULDER POCKETS 2026-10-01: a pocket by the east flank at (54.4, 37) held the player): the mouth rocks collide as
// their convex hulls, so the pack meshes' creases cannot wedge the capsule
foreach (var mc in md.GetComponentsInChildren<UnityEngine.MeshCollider>()) mc.convex = true;
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " cave: rock boxes " + retex + ", cave rocks seated " + seated + " of 10 (most air under one " + airMax.ToString("F1") + " m), side room ground over it lowest " + lowest.ToString("F1") + " (ceiling rock top " + (floorY + roomH + T).ToString("F1") + "), clear " + (lowest > floorY + roomH + T) + " | " + kit.Report();
