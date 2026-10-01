// Main3 task 8.17, the boathouse and the dock (Valley.md rev 11 1.2 and 6 M7, M8, E6; Check1_Story: the resident's step and bowl, a
// chair beside them). Run after 8.16 in Main3, edit mode; rerunnable. The 8.4 gray pieces stop drawing and keep their colliders (they
// are the walk surfaces and walls; the owned meshes stand on the same lines, 8.9d's pattern); the tin roof and the gangway, pocket decks
// and skirts take project textures in place. Boathouse: owned plank wall modules (a doorway on the east, windows north and west), a
// plank floor, log stilts, a hanging lamp, crates and a barrel inside; on the north side over the water, facing the tower, a plank step
// with a bowl and a chair (M8); reeds on the west side (M7). Dock (no dock pack, Wren 2026-09-30): planks across the deck on the same
// profile, log posts and log rails; the pump kitbashed from owned pipe parts with a bucket under the spout (E6).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
var lake = kit.Root("Lake"); if (lake == null) return "no Lake";
var B = lake.transform.Find("Boathouse"); var D = lake.transform.Find("Dock"); if (B == null || D == null) return "no Boathouse or Dock";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
const float bx0 = 236.8f, bx1 = 243.2f, bz0 = 49.6f, bz1 = 55.2f, floorY = -3.8f, wallH = 2.6f, doorZ = 52.4f, water = -5.5f;   // 8.4
const float modW = 2f, modH = 3f, plankW = 0.3f, plankT = 0.06f, packLogLen = 2f, packLogGirth = 0.37f, railGirth = 0.12f;
const string planks = "Assets/Materials/Planks023A_1.0x1.0.mat";
var deckWood = kit.Tinted("Places_DockPlank", planks, Hex("#6E5A44"), new UnityEngine.Vector2(1f, 3f));
var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
void HideDrawn(UnityEngine.Transform t) { foreach (var r in t.GetComponentsInChildren<UnityEngine.Renderer>()) r.enabled = false; }
// a pack log from a to b (world), girth g
UnityEngine.GameObject Log(UnityEngine.Transform parent, UnityEngine.Vector3 a, UnityEngine.Vector3 b, float g)
{
    var lg = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", parent); if (lg == null) return null; PlaceKit.StripColliders(lg);
    var dir = b - a; lg.transform.rotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.right, dir.normalized);
    lg.transform.localScale = V(dir.magnitude / packLogLen, g / packLogGirth, g / packLogGirth);
    lg.transform.position = (a + b) * 0.5f; lg.transform.position += (a + b) * 0.5f - PlaceKit.MeshBounds(lg).center; return lg;
}

// ================= BOATHOUSE =================
foreach (UnityEngine.Transform t in B) if (t.name != "Dressing" && t.name != "Roof" && !t.name.StartsWith("Gangway") && !t.name.StartsWith("PocketDeck") && !t.name.StartsWith("PocketSkirt")) HideDrawn(t);
foreach (UnityEngine.Transform t in B) if (t.name == "Roof") t.GetComponent<UnityEngine.Renderer>().sharedMaterial = steel;
foreach (UnityEngine.Transform t in B) if (t.name.StartsWith("Gangway") || t.name.StartsWith("PocketDeck") || t.name.StartsWith("PocketSkirt")) { var r = t.GetComponent<UnityEngine.Renderer>(); if (r != null) { r.enabled = true; r.sharedMaterial = deckWood; } }
PlaceKit.MarkerOnly(B.Find("Resident_Lake_Spot"));
var bd = kit.Fresh("Dressing", B, V((bx0 + bx1) * 0.5f, floorY, (bz0 + bz1) * 0.5f), 0f);
float W = bx1 - bx0, Dd = bz1 - bz0, sy = wallH / modH;
// walls: three modules a side, stretched to the 8.4 footprint and height; doorway east (at doorZ, the middle module), windows N and W
void Run(string[] kinds, UnityEngine.Vector3 start, UnityEngine.Vector3 along, float len, float yaw)
{
    float w = len / kinds.Length;
    for (int i = 0; i < kinds.Length; i++) { var g = kit.On(PlaceKit.CI + "Building/CITW_Plank_" + kinds[i], bd, bd.InverseTransformPoint(start + along * (w * (i + 0.5f))), yaw, 1f, false, null, true); if (g != null) g.transform.localScale = V(w / modW, sy, 1f); }
}
Run(new[] { "Wall", "Wall", "Wall" }, V(bx0, floorY, bz0 + 0.1f), V(1f, 0f, 0f), W, 0f);
Run(new[] { "Wall", "Doorway", "Wall" }, V(bx0, floorY, bz1 - 0.1f), V(1f, 0f, 0f), W, 180f);
Run(new[] { "Wall", "Window_Wall", "Wall" }, V(bx0 + 0.1f, floorY, bz0), V(0f, 0f, 1f), Dd, 90f);
Run(new[] { "Wall", "Doorway", "Wall" }, V(bx1 - 0.1f, floorY, bz0), V(0f, 0f, 1f), Dd, -90f);
foreach (var cx in new[] { bx0, bx1 }) foreach (var cz in new[] { bz0, bz1 }) { var p = kit.On(PlaceKit.CI + "Building/CITW_Wood_Pillar", bd, bd.InverseTransformPoint(V(cx, floorY, cz)), 0f, 1f, false, null, true); if (p != null) p.transform.localScale = V(1f, sy, 1f); }
for (float x = bx0 + plankW * 0.5f; x < bx1; x += plankW) kit.Fill(PlaceKit.CC + "Props/C_Plank_A_Thick", bd, bd.InverseTransformPoint(V(x, floorY - plankT, (bz0 + bz1) * 0.5f)), V(plankW - 0.02f, plankT, Dd));
foreach (UnityEngine.Transform t in B) if (t.name == "Stilt") { float h = t.lossyScale.y; Log(bd, t.position - V(0f, h * 0.5f, 0f), t.position + V(0f, h * 0.5f, 0f), 0.3f); }
// inside: a hanging lamp, crates, a barrel, a coil of rope, a net of oars against the wall
kit.On(PlaceKit.CI + "Props/CITW_Hanging_Oil_Lamp", bd, bd.InverseTransformPoint(V(239.5f, floorY + wallH - 0.9f, 52.4f)), 0f, 0.6f);
kit.Practical("Lamp", bd, bd.InverseTransformPoint(V(239.5f, floorY + wallH - 1.0f, 52.4f)), 5f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
kit.On(PlaceKit.CI + "Props/CITW_Crate", bd, bd.InverseTransformPoint(V(237.6f, floorY, 50.5f)), 10f, 0.8f, true, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Crate", bd, bd.InverseTransformPoint(V(237.7f, floorY + 0.8f, 50.5f)), 35f, 0.6f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Barrel_2", bd, bd.InverseTransformPoint(V(237.6f, floorY, 54.4f)), 0f, 1f, true, null, true);
kit.On(PlaceKit.FT + "Rope", bd, bd.InverseTransformPoint(V(240.5f, floorY, 54.6f)), 0f, 1.5f, false, null, true);
kit.On(PlaceKit.FT + "Bucket", bd, bd.InverseTransformPoint(V(242.3f, floorY, 50.3f)), 0f, 1f, false, null, true);
// the step on the north side over the water, facing the tower (M8): planks on two log legs, a bowl and a chair
// 8.17 gate (Wren: the step must be reachable, the cat is fed there): a doorway in the north wall (its 8.4 box cut round it) onto a
// plank step stepW x stepD m at the floor level, a box under it to stand on and rails on its three open sides
const float stepW = 3.6f, stepD = 1.8f, stepRail = 1.05f, doorHalf = 0.64f, doorHead = 2.15f * (2.6f / 3f);
var wallN = B.Find("Wall_N"); if (wallN != null && wallN.GetComponent<UnityEngine.BoxCollider>() != null) UnityEngine.Object.DestroyImmediate(wallN.GetComponent<UnityEngine.BoxCollider>());
foreach (var seg in new[] { (bx0, (bx0 + bx1) * 0.5f - doorHalf), ((bx0 + bx1) * 0.5f + doorHalf, bx1) }) kit.Blocker("WallN_Solid", bd, bd.InverseTransformPoint(V((seg.Item1 + seg.Item2) * 0.5f, floorY + wallH * 0.5f, bz1 - 0.075f)), V(seg.Item2 - seg.Item1, wallH, 0.15f));
kit.Blocker("WallN_Lintel", bd, bd.InverseTransformPoint(V((bx0 + bx1) * 0.5f, floorY + (doorHead + wallH) * 0.5f, bz1 - 0.075f)), V(doorHalf * 2f, wallH - doorHead, 0.15f));
var step = kit.Group("Step", bd, V(240f, floorY, bz1 + stepD * 0.5f), 0f);
kit.Blocker("StepFloor", step, V(0f, -0.15f, 0f), V(stepW, 0.3f, stepD));
foreach (var rs in new[] { (V(-stepW * 0.5f, 0f, 0f), V(0.12f, stepRail, stepD), 90f), (V(stepW * 0.5f, 0f, 0f), V(0.12f, stepRail, stepD), 90f), (V(0f, 0f, stepD * 0.5f), V(stepW, stepRail, 0.12f), 0f) })
{ var rl = kit.On(PlaceKit.CI + "Building/CITW_Railing", step, rs.Item1, rs.Item3, 1f, false, null, true); if (rl != null) rl.transform.localScale = V((rs.Item3 == 0f ? stepW : stepD) / 2f, 1f, 1f); kit.Blocker("StepRail", step, rs.Item1 + V(0f, stepRail * 0.5f, 0f), rs.Item2); }
for (float x = -stepW * 0.5f + plankW * 0.5f; x < stepW * 0.5f; x += plankW) kit.Fill(PlaceKit.CC + "Props/C_Plank_A_Thick", step, V(x, -plankT, 0f), V(plankW - 0.02f, plankT, stepD));
foreach (var lx in new[] { -stepW * 0.5f + 0.1f, stepW * 0.5f - 0.1f }) Log(step, step.TransformPoint(V(lx, -plankT, stepD * 0.4f)), V(step.position.x + lx, water - 0.8f, step.position.z + stepD * 0.4f), 0.22f);
float towerYaw = UnityEngine.Quaternion.LookRotation(V(164f - 240f, 0f, 166f - 57f)).eulerAngles.y;
kit.On(PlaceKit.CS + "CS_Chair_2", step, V(-1.2f, 0f, 0.3f), towerYaw, 1f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Bowl_Small", step, V(1.2f, 0f, 0.4f), 0f, 1f, false, null, true);   // empty
// 8.17 gate (Quill): her name hand-lettered on the bowl (a small painted tag on its side; the name is DECISIONS 2026-09-29's Tuesday,
// read as the lake resident: unverified, Wren to confirm), and a small folded blanket on the step
const string bowlName = "TUESDAY";
var tag = kit.Slab("BowlTag", step, V(1.2f, 0.05f, 0.28f), V(0.14f, 0.04f, 0.004f), kit.Tinted("Places_BowlTag", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#D8D0BC"), UnityEngine.Vector2.one), V(0f, 180f, 0f));
kit.Label(tag.transform, bowlName, Hex("#3A2A1E"), 30);
kit.On(PlaceKit.CI + "Furniture/CITW_Blanket", step, V(0.7f, 0f, 0.55f), 15f, 0.35f, false, null, true);
// reeds on the west side (M7), in the shallows
var rng = new System.Random(8017); var reeds = kit.Group("Reeds", bd, V(232f, water, 55f), 0f);
for (int i = 0; i < 22; i++) { float x = 233.8f + (float)rng.NextDouble() * 2.8f, z = 49.8f + (float)rng.NextDouble() * 7f; { float h = 1.4f + 0.6f * (float)rng.NextDouble(); var rd = kit.Fill(PlaceKit.CS + "Vegetation/CS_Grass_Long_" + (1 + i % 2), reeds, reeds.InverseTransformPoint(V(x, water - 0.15f, z)), V(0.9f, h, 0.9f), (float)rng.NextDouble() * 360f); } }   // stood up as reeds }

// ================= DOCK =================
foreach (UnityEngine.Transform t in D) if (t.name != "Dressing") HideDrawn(t);
var dd = kit.Fresh("Dressing", D, V(190f, -4.8f, 91f), 0f);
var flat = D.Find("DeckFlat"); var ramp = D.Find("DeckRamp");
// planks across the deck, following the deck's top at each z (the flat part and the ramp)
float DeckTop(float z) { var o = V(190f, 5f, z); foreach (var h in UnityEngine.Physics.RaycastAll(o, UnityEngine.Vector3.down, 20f)) if (h.collider.transform == flat || h.collider.transform == ramp) return h.point.y; return float.NaN; }
UnityEngine.Physics.SyncTransforms();
int dockPlanks = 0;
for (float z = 86.4f + plankW * 0.5f; z < 96f; z += plankW)
{
    float y = DeckTop(z); if (float.IsNaN(y)) continue;
    var pl = kit.Fill(PlaceKit.CC + "Props/C_Plank_A_Thick", dd, dd.InverseTransformPoint(V(190f, y - plankT + 0.005f, z)), V(3.2f, plankT, plankW - 0.02f)); dockPlanks++;
}
foreach (UnityEngine.Transform t in D)
{
    if (t.name == "Post") { float h = t.lossyScale.y; Log(dd, t.position - V(0f, h * 0.5f, 0f), t.position + V(0f, h * 0.5f, 0f), 0.24f); }
    if (t.name == "Rail" || t.name == "RailEnd") { var s = t.lossyScale; bool alongZ = s.z > s.x; float len = alongZ ? s.z : s.x; var ax = alongZ ? V(0f, 0f, len * 0.5f) : V(len * 0.5f, 0f, 0f); var top = t.position + V(0f, s.y * 0.5f - railGirth * 0.5f, 0f); Log(dd, top - ax, top + ax, railGirth); Log(dd, top - ax - V(0f, s.y * 0.45f, 0f), top + ax - V(0f, s.y * 0.45f, 0f), railGirth); }
}
// the pump (E6): a green pipe column, an elbow spout, a lever handle, the bucket under the spout
var pumpT = D.Find("Pump"); var pp = pumpT != null ? pumpT.position : V(190f, -4.5f, 94.8f);
var pump = kit.Group("Pump", dd, pp, 0f);
float pumpBase = DeckTop(pp.z); if (float.IsNaN(pumpBase)) pumpBase = pp.y;
kit.Fill(PlaceKit.CE + "Pipes/Pipe_Green_Straight_Medium", pump, V(0f, pumpBase - pp.y, 0f), V(0.16f, 1.0f, 0.16f));
kit.Fill(PlaceKit.CE + "Pipes/Pipe_Green_Elbow_90_Short_Smooth", pump, V(0f, pumpBase - pp.y + 0.8f, -0.12f), V(0.16f, 0.22f, 0.3f));   // the spout
var handle = kit.Fill(PlaceKit.CE + "Pipes/Pipe_Black_Straight_Short", pump, V(0f, pumpBase - pp.y + 1.0f, 0.25f), V(0.06f, 0.06f, 0.6f));   // the lever handle
if (handle != null) handle.transform.localRotation *= UnityEngine.Quaternion.Euler(-20f, 0f, 0f);
kit.On(PlaceKit.CI + "Props/CITW_Bucket", pump, V(0f, pumpBase - pp.y, -0.45f), 0f, 1f, false, null, true);
// the carrying bucket (E6), beside the trail at the dock root: no collider (the Celestia pack gives it two, which stalled the
// Pump to boathouse walk at (191.5, 96.1))
var carry = kit.Ground(PlaceKit.CE + "Decoration_Bathroom/Bucket", dd, 192f, 96f, 30f, 1f, false); if (carry != null) PlaceKit.StripColliders(carry);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " lake: boathouse renderers " + bd.GetComponentsInChildren<UnityEngine.Renderer>().Length + ", dock planks " + dockPlanks + " | " + kit.Report();
