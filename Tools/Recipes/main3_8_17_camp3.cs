// Main3 task 8.17, Camp 3, the Artist's camp (Valley.md rev 11 1.4 and 6 M6; Check1_Story: he hides his work; the escape room happens
// inside one of his paintings, its own level later, DECISIONS 2026-09-30). Run after 8.16 in Main3, edit mode; rerunnable. Replaces
// 8.5's gray tent, fire, seat log and Snag lantern with owned ones, and adds the studio: canvases leaning face-in against the hollow's
// west bank (backs out), and one canvas on an easel, face out, with room to stand back and a stool in front of it (M6: which canvas is
// entered and how stays open for Grant). Easel and canvas frames are plank-textured boxes; the painted face is the owned BK cloud
// texture retinted to a dusk sky.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
var c3 = kit.Root("Campsites").transform.Find("Camp_3"); if (c3 == null) return "no Campsites/Camp_3";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
const float cx = 78f, cz = 146f;
foreach (var n in new[] { "Tent", "Fire", "SeatLog", "SnagLantern" }) PlaceKit.Remove(c3.Find(n));
PlaceKit.MarkerOnly(c3.Find("Resident/Resident_Camp3_Spot"));
var d = kit.Fresh("Dressing", c3, V(cx, kit.H(cx, cz), cz), 0f);
const string planks = "Assets/Materials/Planks023A_1.0x1.0.mat", concrete = "Assets/Materials/Concrete034_1.0x1.0.mat";
var easelWood = kit.Tinted("Places_EaselWood", planks, Hex("#7A5E42"), new UnityEngine.Vector2(0.25f, 2f));
var linen = kit.Tinted("Places_CanvasBack", concrete, Hex("#CFC6B0"), UnityEngine.Vector2.one);
var paint = kit.Tinted("Places_Painting", concrete, Hex("#E0A070"), UnityEngine.Vector2.one);
var clouds = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture>("Assets/BK/PureNature_Redwood/Textures/Clouds/Cloud_dist.png");
if (paint != null && clouds != null) { paint.SetTexture("_BaseMap", clouds); UnityEditor.EditorUtility.SetDirty(paint); } else kit.Missing.Add("Cloud_dist.png");

// ---- the tent, the fire, the seat log, the lantern on the Snag (8.5's places)
kit.Ground(PlaceKit.CS + "CS_Tent_Old_2", d, 71f, 151f, 60f, 1f, true);
var fire = kit.Group("Fire", d, V(73.5f, kit.H(73.5f, 141.5f), 141.5f), 0f);
kit.On(PlaceKit.CS + "CS_Campfire_2", fire, V(0f, 0f, 0f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Wood/CS_Firewood_Logs_Burnt", fire, V(0f, 0.05f, 0f), 80f, 1f, false, null, true);
kit.On(PlaceKit.CS + "CS_Campfire_Pot_Hanger_Wood", fire, V(0f, 0f, 0f), 30f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Cookware/CS_Cookware_Pot_3", fire, V(0.9f, 0f, -0.6f), 0f, 1f, false, null, true);
kit.Ground(PlaceKit.CS + "Wood/CS_Log_Large_Long_Seat_1", d, 71.2f, 139.6f, 70f, 1f, true);
var snag = kit.Root("Giants").transform.Find("Heroes/Snag");
if (snag != null)
{
    var toHollow = (V(cx, 0f, cz) - V(snag.position.x, 0f, snag.position.z)).normalized; var lp = V(snag.position.x, snag.position.y + 3f, snag.position.z) + toHollow * 3.3f;
    var lan = kit.Group("SnagLantern", d, lp, UnityEngine.Quaternion.LookRotation(toHollow).eulerAngles.y);
    kit.On(PlaceKit.CI + "Props/CITW_Wall_Hook", lan, V(0f, 0.25f, -0.15f), 180f, 1.5f, false, null, true);
    kit.On(PlaceKit.CS + "CS_Lantern_Old_Rusted", lan, V(0f, -0.2f, 0f), 0f, 1.4f, false, null, true);
    kit.Practical("LanternGlow", lan, V(0f, 0f, 0.2f), 4f, PracticalLight.Kind.Lantern, PracticalLight.ByDay.Off);
}
// ---- a camp mid-task: a paint box open by the fire, rags on the log, brushes in a jar, a bucket of water, his pack
kit.Ground(PlaceKit.CI + "Props/CITW_Crate", d, 75.5f, 140.4f, 20f, 0.8f, true);
kit.Ground(PlaceKit.CI + "Props/CITW_Jar", d, 75.4f, 140.9f, 0f, 1f, false, -0.35f);
kit.Ground(PlaceKit.CI + "Props/CITW_Bucket", d, 76.4f, 139.8f, 0f, 1f, false);
kit.Ground(PlaceKit.CS + "Bags/CS_Backpack_Old_3", d, 72.8f, 150.0f, 200f, 1f, false);
kit.Ground(PlaceKit.CS + "Beds/CS_Bedroll_Old_Rolled_2", d, 73.5f, 151.2f, 150f, 1f, false);

// ---- a canvas: a linen back and frame, the painted face on its front (local -z); bottom centre at the parent's origin
UnityEngine.Transform Canvas(string name, UnityEngine.Transform parent, UnityEngine.Vector3 lp, float yaw, float lean, float w, float h, bool faceOut)
{
    var g = kit.Group(name, parent, parent.TransformPoint(lp), 0f); g.localRotation = UnityEngine.Quaternion.Euler(lean, yaw, 0f);
    kit.Slab("Back", g, V(0f, h * 0.5f, 0.02f), V(w, h, 0.03f), linen);
    foreach (var s in new[] { (V(0f, 0.02f, 0f), V(w, 0.04f, 0.05f)), (V(0f, h - 0.02f, 0f), V(w, 0.04f, 0.05f)), (V(-w * 0.5f + 0.02f, h * 0.5f, 0f), V(0.04f, h, 0.05f)), (V(w * 0.5f - 0.02f, h * 0.5f, 0f), V(0.04f, h, 0.05f)) })
        kit.Slab("Stretcher", g, s.Item1, s.Item2, easelWood);
    if (faceOut) kit.Slab("Painting", g, V(0f, h * 0.5f, -0.005f), V(w - 0.06f, h - 0.06f, 0.005f), paint);
    return g;
}
// canvases faced to the rock of the west bank, backs to the camp (Valley 1.4)
// the bank foot: from the centre west along z 146.5 to where the ground rises bankRise over the floor, bankBack short of it
const float bankRise = 0.4f, bankBack = 0.5f, bankZ = 146.5f; float floorY = kit.H(cx, bankZ), footX = cx;
while (footX > cx - 14f && kit.H(footX - 0.25f, bankZ) < floorY + bankRise) footX -= 0.25f; footX += bankBack;
var stack = kit.Group("FacedCanvases", d, V(footX, kit.H(footX, bankZ), bankZ), 0f);
float bankYaw = -90f;   // the face toward the bank (west), backs toward the camp
for (int i = 0; i < 5; i++) Canvas("Canvas", stack, V(0.12f * i, kit.H(footX + 0.12f * i, bankZ - 1.6f + i * 0.75f) - stack.position.y, -1.6f + i * 0.75f), bankYaw + 180f + (i % 2) * 6f, -10f + i, 0.7f + 0.15f * (i % 3), 0.9f + 0.2f * (i % 2), false);
// the easel: two front legs, a back leg, a ledge; the one canvas on it faces the camp across open floor (M6), a stool 1.6 m in front
var easel = kit.Group("Easel", d, V(81.5f, kit.H(81.5f, 150.5f), 150.5f), 0f);
float faceYaw = UnityEngine.Quaternion.LookRotation(V(81.5f - cx, 0f, 150.5f - cz)).eulerAngles.y;   // the painting's face (local -z) looks back toward the camp centre
easel.localRotation = UnityEngine.Quaternion.Euler(0f, faceYaw, 0f);
foreach (var sx in new[] { -0.35f, 0.35f }) kit.Slab("Leg", easel, V(sx, 0.85f, 0f), V(0.05f, 1.75f, 0.05f), easelWood, V(-8f, 0f, sx > 0f ? -6f : 6f));
kit.Slab("BackLeg", easel, V(0f, 0.8f, 0.45f), V(0.05f, 1.7f, 0.05f), easelWood, V(22f, 0f, 0f));
kit.Slab("Ledge", easel, V(0f, 0.72f, -0.08f), V(0.9f, 0.04f, 0.08f), easelWood);
Canvas("TheCanvas", easel, V(0f, 0.74f, -0.02f), 0f, -8f, 0.9f, 1.1f, true);
kit.On(PlaceKit.CS + "CS_Stool_2", easel, V(0f, 0f, -1.6f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Mug", easel, V(0.3f, 0.76f, -0.08f), 0f, 1f, false, null, true);   // brushes in a mug on the ledge

// ---- 8.3's log steps down the rim into the hollow: each gray cylinder (colliderless; the StairRamp carries the player) becomes an
// owned log of the same length and girth in the same place
const float packLogLen = 2f, packLogGirth = 0.37f;
int logSteps = 0;
foreach (UnityEngine.Transform steps in kit.Root("PointsOfInterest").transform)
if (steps.name == "POI_Log_steps")
{
    var logs = kit.Fresh("OwnedLogs", steps, steps.position, 0f);
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(steps)))
    {
        if (t.name != "Log") continue;
        var lg = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", logs); if (lg == null) break;
        lg.transform.rotation = t.rotation * UnityEngine.Quaternion.Euler(0f, 0f, 90f);
        lg.transform.localScale = V(t.lossyScale.y * 2f / packLogLen, t.lossyScale.x / packLogGirth, t.lossyScale.z / packLogGirth);
        lg.transform.position = t.position; lg.transform.position += t.position - PlaceKit.MeshBounds(lg).center;
        PlaceKit.Remove(t); logSteps++;
    }
}

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " camp 3: log steps " + logSteps + ", renderers " + d.GetComponentsInChildren<UnityEngine.Renderer>().Length + ", easel faces " + faceYaw.ToString("F0") + " | " + kit.Report();
