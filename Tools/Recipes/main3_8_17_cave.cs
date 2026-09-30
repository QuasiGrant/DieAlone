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
    if (piece.name == "Resident_Cave_Spot" || piece.name == "SideRoom" || piece.name == "MouthDressing") continue;
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
// ---- outside the mouth: boulders and rubble on both sides of the opening (x 50.5 to 53.5), none in it
var md = kit.Fresh("MouthDressing", cave.transform, V(52f, kit.H(52f, 39f), 39f), 0f);
kit.Ground(PlaceKit.BK + "Rocks/BigBoulders_1", md, 47.6f, 38.6f, 30f, 0.55f, true, 0.5f);
kit.Ground(PlaceKit.BK + "Rocks/Boulder_2", md, 56.2f, 38.4f, 200f, 0.45f, true, 0.3f);
kit.Ground(PlaceKit.BK + "Rocks/Boulder_4", md, 48.9f, 41.2f, 110f, 0.3f, true, 0.2f);
kit.Ground(PlaceKit.BK + "Rocks/RubbleSparse_1", md, 52f, 40.5f, 15f, 0.6f, false, 0.1f);
// over the opening: two boulders on the top ground either side of the cap rock, overhanging its edges, so the lintel reads as rock
kit.Ground(PlaceKit.BK + "Rocks/Boulder_1", md, 49.2f, 35.2f, 70f, 0.5f, true, 0.4f);
kit.Ground(PlaceKit.BK + "Rocks/Boulder_5", md, 54.9f, 35.6f, 250f, 0.45f, true, 0.4f);
kit.Ground(PlaceKit.CC + "Props/C_Candle_2", md, 50.2f, 38.2f, 0f, 1f, false);   // stubs the cult left at the door
kit.Ground(PlaceKit.CC + "Props/C_Candle_4", md, 53.9f, 38.1f, 0f, 1f, false);

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
kit.On(PlaceKit.CE + "Furniture/Chair", tbl, V(0f, 0f, -0.95f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Furniture/Chair", tbl, V(0f, 0f, 0.95f), 180f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Weapons/Pistol", tbl, V(0.1f, tTop, 0f), 70f, 1f, false, V(0f, 0f, 90f), true);   // lying on its side
kit.On(PlaceKit.CC + "Props/C_Candle_1", tbl, V(-0.45f, tTop, 0.2f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Ashtray", tbl, V(0.4f, tTop, -0.25f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Lamps/Cage_Light", tbl, V(0f, roomH - 0.05f, 0f), 0f);
kit.Practical("TableLamp", tbl, V(0f, roomH - 0.5f, 0f), 5f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Full);
kit.On(PlaceKit.CC + "Props/C_Crate_Small_1", room, room.InverseTransformPoint(V(96.6f, floorY, 14.6f)), 20f, 1f, true, null, true);
PlaceKit.MarkerOnly(cave.transform.Find("Resident_Cave_Spot"));
// check: the terrain over the room stays above its ceiling rock (0 over it, Valley M10)
float lowest = float.MaxValue; for (float x = rx0; x <= rx1; x += 1f) for (float z = rz0; z <= rz1; z += 1f) lowest = UnityEngine.Mathf.Min(lowest, kit.H(x, z));

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " cave: rock boxes " + retex + ", side room ground over it lowest " + lowest.ToString("F1") + " (ceiling rock top " + (floorY + roomH + T).ToString("F1") + "), clear " + (lowest > floorY + roomH + T) + " | " + kit.Report();
