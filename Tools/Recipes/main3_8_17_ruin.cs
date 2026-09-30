// Main3 task 8.17, the north ruin: the previous keeper's cabin, no tower (Valley.md rev 11 1.3 and 6 E11; Wren's calls 2026-09-30).
// Run after 8.16 in Main3, edit mode; rerunnable. A low log cabin with its roof in, 6 x 4 m, its doorway facing the knob (south-west),
// in grove N1 at (172, 281): 4 m east and 3 m north of the map point (168, 278), where the grove leaves a gap (a giant stands 2.9 m from the
// map point; logged in Main3_BuildNotes.md). Broken walls from owned log modules with fitted colliders, the pale roof slab fallen in,
// the ridge beam down across it, a leaning stovepipe over a rusted stove, a trunk in the back corner (the cache), plank debris and
// leaves; a line of owned stones from the north loop to the doorway (no new trail paint). Adds the dev warp North_Loop_Ruin on the loop,
// facing the ruin (label in DevWarpLabels, Valley.md 14).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
UnityEngine.Physics.SyncTransforms();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
const float rx = 172f, rz = 281f, faceBearing = 225f, modW = 2f, openHalf = 0.6f, openHead = 2.15f, stoneStep = 1.6f;
const string planks = "Assets/Materials/Planks023A_1.0x1.0.mat";
var places = kit.Root("Places") ?? new UnityEngine.GameObject("Places");
var ruin = kit.Fresh("NorthRuin", places.transform, V(rx, kit.H(rx, rz), rz), faceBearing);   // local +z faces the knob
string LW(string n) => PlaceKit.CI + "Building/CITW_Log_" + n;
// a wall module with a collider round its mesh; a doorway keeps its opening clear
void Module(string kind, float lx, float lz, float yaw)
{
    var g = kit.On(LW(kind), ruin, V(lx, 0f, lz), yaw, 1f, false, null, true); if (g == null) return;
    var b = PlaceKit.LocalBounds(g, g.transform); var holder = kit.Group("Collider_" + kind, ruin, ruin.TransformPoint(V(lx, 0f, lz)), 0f); holder.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    float top = b.max.y * g.transform.localScale.y, thick = b.size.z;
    if (kind == "Doorway") { foreach (var sx in new[] { -1f, 1f }) kit.Blocker("Jamb", holder, V(sx * (modW * 0.5f + openHalf) * 0.5f, top * 0.5f, 0f), V(modW * 0.5f - openHalf, top, thick)); kit.Blocker("Lintel", holder, V(0f, (openHead + top) * 0.5f, 0f), V(openHalf * 2f, top - openHead, thick)); }
    else kit.Blocker("Wall", holder, V(0f, top * 0.5f, 0f), V(modW, top, thick));
}
// back (local -z) and sides stand to half height in places; the front's right module is gone, the right side has one half wall left
Module("Wall", -2f, -2f, 180f); Module("Half_Wall", 0f, -2f, 180f); Module("Half_Wall", 2f, -2f, 180f);
Module("Wall", -3f, -1f, 90f); Module("Half_Wall", -3f, 1f, 90f);
Module("Half_Wall", 3f, -1f, -90f);
Module("Half_Wall", -2f, 2f, 0f); Module("Doorway", 0f, 2f, 0f);
foreach (var c in new[] { V(-3f, 0f, -2f), V(-3f, 0f, 2f) }) kit.On(PlaceKit.CI + "Building/CITW_Wood_Pillar", ruin, c, 0f, 1f, false, null, true);
// floor boards, some gone
for (int i = 0; i < 6; i++) { if (i == 2 || i == 5) continue; kit.Fill(PlaceKit.CI + "Building/CITW_Floor", ruin, V(-2f + (i % 3) * 2f, -0.05f, -1f + (i / 3) * 2f), V(2f, 0.1f, 2f)); }
// the roof in: a pale plank slab from the back wall top to the floor, the ridge beam across it, a roof module slid off the right side
var pale = kit.Tinted("Places_RuinRoof", planks, Hex("#B4AC9C"), new UnityEngine.Vector2(3f, 2f));
kit.Slab("RoofSlab", ruin, V(0.5f, 1.3f, -0.4f), V(4.6f, 0.12f, 3.2f), pale, V(38f, 0f, 8f));
var beam = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", ruin);
if (beam != null) { PlaceKit.StripColliders(beam); beam.transform.localRotation = UnityEngine.Quaternion.Euler(0f, 15f, -22f); beam.transform.localScale = V(3.6f, 0.8f, 0.8f); beam.transform.localPosition = V(0.4f, 1.2f, 0.1f); }
var slid = kit.On(PlaceKit.CI + "Building/CITW_Roof_1", ruin, V(4.2f, 0f, 0.4f), 90f, 1f, false, V(0f, 0f, 55f), true);
// the stove and its leaning pipe, the cache trunk in the back left corner, debris and leaves
kit.On(PlaceKit.CE + "Decoration_Home/Potbelly_Stove", ruin, V(1.9f, 0f, -1.3f), 20f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Potbelly_Stove_Pipe_Long", ruin, V(2.1f, 1.1f, -1.3f), 0f, 1.6f, false, V(0f, 0f, -14f), true);
kit.On(PlaceKit.CI + "Props/CITW_Trunk_2", ruin, V(-2.3f, 0f, -1.4f), 90f, 1f, true, null, true);
kit.On(PlaceKit.CI + "Furniture/CITW_Rocking_Chair", ruin, V(-1.8f, 0f, 1.0f), 160f, 1f, false, V(0f, 0f, 80f), true);   // on its side
for (int i = 0; i < 5; i++) kit.On(PlaceKit.CC + "Props/C_Plank_B_Thick", ruin, V(-1.5f + i * 0.9f, 0.02f * i, 0.6f - (i % 2) * 1.1f), i * 41f, 1f, false, null, true);
foreach (var lp in new[] { V(-1f, 0f, 0.3f), V(1.2f, 0f, 1.2f), V(3.8f, 0f, -1.5f) }) kit.On(PlaceKit.BK + "Plants/DeadLeaves" + (1 + (int)(lp.x + 2f) % 2), ruin, lp, lp.x * 30f, 0.6f, false, null, true);
// the side path: owned stones every stoneStep m from the loop's nearest centre point to the doorway
var trails = kit.Root("Trails").transform; var door = ruin.TransformPoint(V(0f, 0f, 3f)); UnityEngine.Vector3 near = door; float best = float.MaxValue;
foreach (UnityEngine.Transform leg in trails) foreach (UnityEngine.Transform p in leg) { float dd = new UnityEngine.Vector2(p.position.x - door.x, p.position.z - door.z).magnitude; if (dd < best) { best = dd; near = p.position; } }
var path = kit.Group("SidePath", ruin, door, 0f); int stones = 0;
var run = new UnityEngine.Vector2(door.x - near.x, door.z - near.z); float len = run.magnitude;
for (float s = 1.5f; s < len - 0.5f; s += stoneStep) { var q = new UnityEngine.Vector2(near.x, near.z) + run.normalized * s; kit.Ground(PlaceKit.CS + "Rocks and Stones/CS_Stone_" + (1 + stones % 8), path, q.x, q.y, stones * 67f, 1f, false, 0.08f); stones++; }
// the warp on the loop, facing the ruin
var warps = kit.Root("DevWarps").transform; PlaceKit.Remove(warps.Find("North_Loop_Ruin"));
var w = new UnityEngine.GameObject("North_Loop_Ruin").transform; w.SetParent(warps, false); w.position = near + V(0f, 0.2f, 0f);
w.rotation = UnityEngine.Quaternion.LookRotation(V(rx - near.x, 0f, rz - near.z).normalized);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " ruin at (" + rx + ", " + rz + ") ground " + ruin.position.y.ToString("F1") + ", loop point " + best.ToString("F1") + " m from the door, stones " + stones + ", warp at " + w.position.ToString("F1") + " | " + kit.Report();
