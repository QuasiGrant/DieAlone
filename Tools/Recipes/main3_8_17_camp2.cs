// Main3 task 8.17, Camp 2 (Valley.md rev 11 1.2 and 6 M5; Check1_Story: watching for someone from as high as he can get; the payphone
// and card table at the stack foot). Run after 8.16 in Main3, edit mode; rerunnable. The stack, its switchback path and ladder stay
// (8.5, textured by 8.9f); this adds owned granite talus round the stack's foot (clear of the path, ladder, trails and the lake view line),
// swaps 8.5's gray boulder field for owned BK boulders in the same places and sizes, the gray tent and lamp on top for an owned tent and
// a lantern with a cold core (the tower's marker), the barrel for an owned one, and builds the payphone (Celestia booth, own hood
// light) at (300, 99) and the card table set for two at (297, 98), clear of the ramp foot (298.9, 107.8).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
var c2 = kit.Root("Campsites").transform.Find("Camp_2"); if (c2 == null) return "no Campsites/Camp_2";
UnityEngine.Physics.SyncTransforms();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
const float sx = 292f, sz = 108f, stackR = 6f, talusR = 7.2f, talusScale = 0.55f, trailKeep = 3.5f, boulderWidth = 6f, boulderSink = 0.3f;
var d = kit.Fresh("Dressing", c2, V(sx, kit.H(sx, sz), sz), 0f);
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform leg in kit.Root("Trails").transform) foreach (UnityEngine.Transform p in leg) trailPts.Add(P(p.position.x, p.position.z));
bool NearTrail(UnityEngine.Vector2 p, float r) { foreach (var t in trailPts) if ((t - p).sqrMagnitude < r * r) return true; return false; }
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); return UnityEngine.Vector2.Distance(p, a + ab * t); }
var lakeView = (P(286.5f, 102.2f), P(240f, 52.4f));   // 8.9's Camp 2 to boathouse line (8.5 kept it clear)
string[] bk = { "Boulder_0", "Boulder_1", "Boulder_2", "Boulder_3", "Boulder_4", "Boulder_5" }, big = { "BigBoulders_0", "BigBoulders_1", "BigBoulders_2", "BigBoulders_3", "BigBoulders_4", "BigBoulders_5" };

// ---- 8.5's boulder field: each gray sphere becomes an owned boulder of the same width, where it stood
// the swapped boulders live in their own group, which a rerun keeps (the gray ones are gone after the first run)
var field = c2.Find("BoulderField") ?? kit.Group("BoulderField", c2, c2.position, 0f); int swapped = 0;
foreach (var b in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(c2)))
{
    if (b.name != "Boulder") continue; var rock = b.Find("Rock"); float w = rock != null ? rock.localScale.x : 2f;
    kit.Ground(PlaceKit.BK + "Rocks/" + bk[swapped % bk.Length], field, b.position.x, b.position.z, b.eulerAngles.y, w / boulderWidth, true, boulderSink * w / boulderWidth);
    PlaceKit.Remove(b); swapped++;
}
// ---- talus round the stack's foot: west, north and south faces only (the path climbs the east face), each clear of the ladder, barrel,
// payphone corner, trails and the lake view line
var ladder = c2.Find("Ladder"); var ladderP = ladder != null ? P(ladder.position.x, ladder.position.z) : P(289.8f, 102.1f);
var keep = new[] { (ladderP, 3f), (P(286.5f, 101.5f), 2.5f), (P(298.5f, 99f), 4.5f) };
int talus = 0;
for (float a = 100f; a <= 355f; a += 14f)   // bearings from north, clockwise; east (10 to 100) is the path
{
    var p = P(sx + UnityEngine.Mathf.Sin(a * UnityEngine.Mathf.Deg2Rad) * talusR, sz + UnityEngine.Mathf.Cos(a * UnityEngine.Mathf.Deg2Rad) * talusR);
    bool ok = !NearTrail(p, trailKeep + 2f) && SegD(p, lakeView.Item1, lakeView.Item2) > 3.5f; foreach (var k in keep) if (UnityEngine.Vector2.Distance(p, k.Item1) < k.Item2 + 2f) ok = false;
    if (!ok) continue;
    kit.Ground(PlaceKit.BK + "Rocks/" + big[talus % big.Length], d, p.x, p.y, a * 2.3f, talusScale + 0.1f * (talus % 3), true, 0.6f); talus++;
}
// ---- the stack itself takes the owned BK rock texture (the flat granite tint read as a brown pillar, Check3_Quality 4)
// 8.5 bent the ProBuilder cylinder's points, which leaves its UVs flat: a cylinder projection, one texture tile per stackTile m
const float stackTile = 4f;
var stackR2 = c2.Find("GraniteStack");
if (stackR2 != null)
{
    stackR2.GetComponent<UnityEngine.Renderer>().sharedMaterial = kit.Tinted("Places_StackRock", "Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Materials/Rocks.mat", Hex("#9A948A"), UnityEngine.Vector2.one);
    var mesh = stackR2.GetComponent<UnityEngine.MeshFilter>().sharedMesh; var vs = mesh.vertices; var uv = new UnityEngine.Vector2[vs.Length];
    for (int i = 0; i < vs.Length; i++) { float r = new UnityEngine.Vector2(vs[i].x, vs[i].z).magnitude; float ang = UnityEngine.Mathf.Atan2(vs[i].z, vs[i].x); uv[i] = r < stackR * 0.5f ? new UnityEngine.Vector2(vs[i].x, vs[i].z) / stackTile : new UnityEngine.Vector2(ang * stackR / stackTile, vs[i].y / stackTile); }
    mesh.uv = uv;
}
// ---- the top: an owned tent, the resident's marker, a lantern with a cold core that the tower sees at night
var topT = c2.Find("StackTop"); if (topT == null) return "no StackTop";
PlaceKit.Remove(topT.Find("Tent")); PlaceKit.Remove(topT.Find("TentLamp")); PlaceKit.MarkerOnly(topT.Find("Resident_Camp2_Spot"));
var topD = kit.Fresh("Dressing", topT, topT.position, 0f);
kit.On(PlaceKit.CS + "CS_Tent_Modern_2", topD, V(0.5f, 0f, 0.8f), 200f, 1f, true, null, true);
kit.On(PlaceKit.CS + "CS_Lantern_Modern", topD, V(0.5f, 0f, -1.0f), 0f, 1.4f, false, null, true);
var core = kit.Slab("LampCore", topD, V(0.5f, 0.3f, -1.0f), V(0.12f, 0.18f, 0.12f), kit.Glow("Places_ColdLamp", Hex("#DDE6F0"), kit.Look.farMarkerIntensity));
core.GetComponent<UnityEngine.Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
kit.On(PlaceKit.CS + "CS_Chair_2", topD, V(-1.8f, 0f, -0.6f), 70f, 1f, false, null, true);   // his chair at the edge, facing the road
kit.On(PlaceKit.CE + "Decoration_Out/Glass_Bottle", topD, V(-1.3f, 0f, -0.9f), 0f, 1f, false, null, true);
// ---- the barrel
PlaceKit.Remove(c2.Find("RainBarrel"));
kit.Ground(PlaceKit.CI + "Props/CITW_Barrel_3", d, 286.5f, 101.5f, 20f, 1.2f, true);
// ---- the payphone: the booth at (300, 99) with its door toward the table, a hood lamp; the card table at (297, 98) set for two
var phone = kit.Group("Payphone", d, V(300f, kit.H(300f, 99f), 99f), -90f);
kit.On(PlaceKit.CE + "Decoration_Out/Telephone_Booth", phone, V(0f, 0f, 0f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Lamps/Cage_Light", phone, V(0f, 2.9f, 0.9f), 0f);
kit.Practical("HoodLight", phone, V(0f, 2.6f, 1.0f), 5f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Off);
var cards = kit.Group("CardTable", d, V(297f, kit.H(297f, 98f), 98f), 15f);
var tbl = kit.On(PlaceKit.CS + "CS_Table_Small_Modern_2", cards, V(0f, 0f, 0f), 0f, 1.3f, true, null, true);
float tTop = tbl != null ? PlaceKit.LocalBounds(tbl, cards).max.y : 0.78f;
kit.On(PlaceKit.CS + "CS_Chair_1", cards, V(0f, 0f, -0.95f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CS + "CS_Chair_4", cards, V(0f, 0f, 0.95f), 180f, 1f, true, null, true);   // the second chair, never sat in
for (int i = 0; i < 5; i++) kit.On(PlaceKit.CE + "Decoration_Home/Paper", cards, V(-0.2f + i * 0.08f, tTop, -0.15f + (i % 2) * 0.1f), i * 23f, 0.35f, false, null, true);   // a dealt hand
kit.On(PlaceKit.CS + "Drinks/CS_Drink_Beer", cards, V(0.3f, tTop, 0.2f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Drinks/CS_Drink_Beer", cards, V(-0.3f, tTop, 0.25f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Ashtray", cards, V(0.25f, tTop, -0.25f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "CS_Lantern_Old", cards, V(-0.3f, tTop, -0.2f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Bags/CS_Backpack_Old_2", cards, V(0.9f, 0f, -0.9f), 60f, 1f, false, null, true);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " camp 2: boulders swapped " + swapped + ", talus " + talus + ", renderers " + d.GetComponentsInChildren<UnityEngine.Renderer>().Length + " | " + kit.Report();
