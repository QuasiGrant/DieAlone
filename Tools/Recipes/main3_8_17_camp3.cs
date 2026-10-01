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
// 8.17 gate (Vesper, Camp 3 D: no tent or fire in any frame): tent and fire on the flat 8 to 13 m in front of the Camp 3 warp (74, 142, facing
// north-east), clear of the trail end
// 8.18a (Vesper, Pim: no tent in any frame; proof frame 2026-10-01: it stood there, but the pack canvas on Particles/Lit drew as a pale
// grey see-through sheet against the grey boulders, with open and closed flaps both shown): the canvas on an opaque two-sided URP Lit copy
// tinted tentCanvasHex, the open flaps hidden, the tent tentScale times the pack size
const string tentCanvasHex = "#7E6A44"; const float tentScale = 1.25f;
var tent = kit.Ground(PlaceKit.CS + "CS_Tent_Old_2", d, 80.5f, 153f, 215f, tentScale, true);
if (tent != null)
{
    var canvas = kit.Tinted("Places_TentCanvas", "Assets/Revolving Pizza Games/Campsite/Materials/Tents/CS_Tent_Old.mat", Hex(tentCanvasHex), UnityEngine.Vector2.one);
    var lit = UnityEngine.Shader.Find("Universal Render Pipeline/Lit");
    if (canvas != null && lit != null) { canvas.shader = lit; canvas.SetFloat("_Surface", 0f); canvas.SetFloat("_Cull", 0f); canvas.SetColor("_BaseColor", Hex(tentCanvasHex)); UnityEditor.EditorUtility.SetDirty(canvas); }
    foreach (var r in tent.GetComponentsInChildren<UnityEngine.Renderer>(true))
    {
        if (r.name.EndsWith("_Open")) { r.gameObject.SetActive(false); continue; }
        var ms = r.sharedMaterials; for (int i = 0; i < ms.Length; i++) if (ms[i] != null && ms[i].name == "CS_Tent_Old") ms[i] = canvas; r.sharedMaterials = ms;
    }
}
var fire = kit.Group("Fire", d, V(76.5f, kit.H(76.5f, 149.5f), 149.5f), 0f);
kit.On(PlaceKit.CS + "CS_Campfire_2", fire, V(0f, 0f, 0f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Wood/CS_Firewood_Logs_Burnt", fire, V(0f, 0.05f, 0f), 80f, 1f, false, null, true);
kit.On(PlaceKit.CS + "CS_Campfire_Pot_Hanger_Wood", fire, V(0f, 0f, 0f), 30f, 1f, false, null, true);
kit.On(PlaceKit.CS + "Cookware/CS_Cookware_Pot_3", fire, V(0f, 0.12f, 0f), 0f, 1f, false, null, true);   // a pot on the cold fire
kit.Ground(PlaceKit.CS + "Wood/CS_Log_Large_Long_Seat_1", d, 74.6f, 151.2f, 40f, 1f, true);
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
// the paint box, jar and rag at the easel foot
kit.Ground(PlaceKit.CI + "Props/CITW_Crate", d, 84.6f, 149.8f, 20f, 0.6f, true);
kit.Ground(PlaceKit.CI + "Props/CITW_Jar", d, 84.6f, 149.8f, 0f, 1f, false, -0.5f);
kit.Ground(PlaceKit.CS + "CS_Bedroll_Old_1", d, 83.1f, 150.4f, 70f, 0.35f, false);   // a rag
kit.Ground(PlaceKit.CI + "Props/CITW_Bucket", d, 77.8f, 150.6f, 0f, 1f, false);
kit.Ground(PlaceKit.CS + "Bags/CS_Backpack_Old_3", d, 79.0f, 154.0f, 200f, 1f, false);
kit.Ground(PlaceKit.CS + "Beds/CS_Bedroll_Old_Rolled_2", d, 82.6f, 151.4f, 150f, 1f, false);

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
// 8.17 gate (Quill: the canvases did not read, boulders stood in front of them): the bank foot is searched round the hollow for a
// stretch of bare rock: from the centre out along each bearing (every bankStep degrees from bankFrom) to where the ground rises bankRise
// over the floor, bankBack short of it, and the first with no 8.15 face rock within rockClear m and no trail within trailClear m wins
const float bankRise = 0.4f, bankBack = 0.5f, bankStep = 10f, bankFrom = 270f, rockClear = 3f, trailClear = 3.5f; float floorY = kit.H(cx, cz);
var faceRocks = new System.Collections.Generic.List<UnityEngine.Bounds>(); var fr = kit.Root("Ground815") != null ? kit.Root("Ground815").transform.Find("Stops/FaceRock") : null;
if (fr != null) foreach (var r in fr.GetComponentsInChildren<UnityEngine.Renderer>()) faceRocks.Add(r.bounds);
var trailP = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform leg in kit.Root("Trails").transform) foreach (UnityEngine.Transform p in leg) trailP.Add(new UnityEngine.Vector2(p.position.x, p.position.z));
UnityEngine.Vector3 foot = V(cx - 8f, 0f, cz); float bankYaw = -90f; bool bare = false;
for (float k = 0f; k < 360f && !bare; k += bankStep)
{
    foreach (var sgn in new[] { 1f, -1f })
    {
        float b = bankFrom + sgn * k; var dir = V(UnityEngine.Mathf.Sin(b * UnityEngine.Mathf.Deg2Rad), 0f, UnityEngine.Mathf.Cos(b * UnityEngine.Mathf.Deg2Rad)); float s = 0f;
        while (s < 14f && kit.H(cx + dir.x * (s + 0.25f), cz + dir.z * (s + 0.25f)) < floorY + bankRise) s += 0.25f;
        var q = V(cx + dir.x * (s - bankBack), 0f, cz + dir.z * (s - bankBack)); bool ok = true;
        foreach (var rb in faceRocks) if (rb.SqrDistance(V(q.x, rb.center.y, q.z)) < rockClear * rockClear) { ok = false; break; }
        foreach (var t in trailP) if ((t - new UnityEngine.Vector2(q.x, q.z)).sqrMagnitude < trailClear * trailClear) { ok = false; break; }
        if (ok) { foot = q; bankYaw = b; bare = true; break; }
    }
}
var stack = kit.Group("FacedCanvases", d, V(foot.x, kit.H(foot.x, foot.z), foot.z), bankYaw);   // local +z toward the bank
for (int i = 0; i < 4; i++) Canvas("Canvas", stack, V(-1.5f + i * 0.95f, kit.H(stack.TransformPoint(V(-1.5f + i * 0.95f, 0f, 0f)).x, stack.TransformPoint(V(-1.5f + i * 0.95f, 0f, 0f)).z) - stack.position.y, 0f), 180f + (i % 2) * 6f, -12f + i, 1.0f + 0.15f * (i % 3), 1.3f + 0.25f * (i % 2), false);
// the job form (Quill): one white sheet weighted with a mug, on the flat by the tent, nothing else near it
var form = kit.Group("JobForm", d, V(77.6f, kit.H(77.6f, 155.4f), 155.4f), 20f);
kit.On(PlaceKit.CE + "Decoration_Home/Paper", form, V(0f, 0f, 0f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Mug", form, V(0.05f, 0.005f, 0.04f), 0f, 1f, false, null, true);
// the easel: two front legs, a back leg, a ledge; the one canvas on it faces the camp across open floor (M6), a stool 1.6 m in front
var easel = kit.Group("Easel", d, V(83.5f, kit.H(83.5f, 149f), 149f), 0f);
float faceYaw = UnityEngine.Quaternion.LookRotation(V(83.5f - cx, 0f, 149f - cz)).eulerAngles.y;   // the painting's face (local -z) looks back toward the camp centre
easel.localRotation = UnityEngine.Quaternion.Euler(0f, faceYaw, 0f);
foreach (var sx in new[] { -0.35f, 0.35f }) kit.Slab("Leg", easel, V(sx, 0.85f, 0f), V(0.07f, 1.75f, 0.07f), easelWood, V(-8f, 0f, sx > 0f ? -6f : 6f));
kit.Slab("BackLeg", easel, V(0f, 0.8f, 0.45f), V(0.07f, 1.7f, 0.07f), easelWood, V(22f, 0f, 0f));
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
