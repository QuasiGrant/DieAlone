// Main3 8.26 gate round 2 (Wren 2026-10-03, A): the camper trailer and the washed-out pickup as kitbashed stand-ins from owned pieces
// (slabs in retinted project materials; the pack's Door_Plain and Car_Door read as a thin line and a white lattice), in place of 8.3's grey primitives (the W1 leg's "grey slab" was the
// trailer's Body). Nothing imported; the real models are Grant's M11 choice. Edit mode, Main3; rerunnable (each POI's Kit is rebuilt,
// 8.3's Body, Bed and Cab removed). In the runner after main3_8_26_camp3.cs.
// Each POI keeps its own place and yaw (local +z along the trail); a Kit child carries the lean and the sink, every piece in its axes.
// TRAILER (POI_Camper_trailer): a cream body trW x trH x trL on its wheels (trClear over the ground), a rust belt, a pale roof cap, dark
//   window insets (two a side and the front), a door panel with a window on the +x side, two tyres, the A-frame hitch and jack at +z; leaning trLean.
// PICKUP (POI_Washed_out_truck): a faded red cab and hood, side windows and door seams, windscreen and rear window insets, the bed's walls round a
//   silt fill nearly to their tops (no pocket to stand in), bumpers, headlights and four tyres, sunk tkSink into the ground, leaning tkLean.
// Colliders: the trailer body, the pickup cab and hood, a box over the bed, and a box over each tyre (its own mesh bounds); the rest is
// small or inside those.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var notes = new System.Collections.Generic.List<string>();
var poi = kit.Root("PointsOfInterest") != null ? kit.Root("PointsOfInterest").transform : null; if (poi == null) return "no PointsOfInterest (run 8.3)";
const string concrete = "Assets/Materials/Concrete034_1.0x1.0.mat";
var skin = kit.Tinted("Places_TrailerSkin", concrete, Hex("#BDB49A"), UnityEngine.Vector2.one); var belt = kit.Tinted("Places_TrailerBelt", concrete, Hex("#6A4E34"), UnityEngine.Vector2.one);
var roofM = kit.Tinted("Places_TrailerRoof", concrete, Hex("#9A968C"), UnityEngine.Vector2.one); var glass = kit.Tinted("Places_VehicleGlass", concrete, Hex("#1B1F22"), UnityEngine.Vector2.one);
var rubber = kit.Tinted("Places_Tyre", concrete, Hex("#1A1A1A"), UnityEngine.Vector2.one); var steelM = kit.Tinted("Places_VehicleSteel", concrete, Hex("#55585A"), UnityEngine.Vector2.one);
var paint = kit.Tinted("Places_PickupPaint", concrete, Hex("#7A3B2E"), UnityEngine.Vector2.one); var silt = kit.Tinted("Places_Silt", concrete, Hex("#6B5E4C"), new UnityEngine.Vector2(2f, 2f));
var lamp = kit.Tinted("Places_Headlamp", concrete, Hex("#C9C6B8"), UnityEngine.Vector2.one);
UnityEngine.GameObject S(string n, UnityEngine.Transform p, UnityEngine.Vector3 lp, UnityEngine.Vector3 size, UnityEngine.Material m, bool collide = false) => kit.Slab(n, p, lp, size, m, default, collide);
// a tyre: a cylinder on its side (axis x), radius r, width w, a box collider on its own mesh bounds
int tyres = 0;
void Tyre(UnityEngine.Transform p, UnityEngine.Vector3 lp, float r, float w)
{
    var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); g.name = "Tyre"; g.transform.SetParent(p, false); g.transform.localPosition = lp;
    g.transform.localRotation = UnityEngine.Quaternion.Euler(0f, 0f, 90f); g.transform.localScale = V(r * 2f, w * 0.5f, r * 2f);
    UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>()); g.AddComponent<UnityEngine.BoxCollider>(); g.GetComponent<UnityEngine.Renderer>().sharedMaterial = rubber; tyres++;
}
UnityEngine.Transform KitOf(string poiName, float lean, float sink, params string[] old)
{
    var g = poi.Find(poiName); if (g == null) { notes.Add("no " + poiName); return null; }
    foreach (var n in old) PlaceKit.Remove(g.Find(n));
    var k = kit.Fresh("Kit", g, g.position, 0f); k.localPosition = V(0f, -sink, 0f); k.localRotation = UnityEngine.Quaternion.Euler(0f, 0f, lean); return k;
}

// ================= TRAILER =================
const float trW = 2.3f, trH = 2.0f, trL = 5.0f, trClear = 0.3f, trLean = -6f, trTyreR = 0.33f, trTyreW = 0.22f, inset = 0.02f;
{
    var k = KitOf("POI_Camper_trailer", trLean, 0f, "Body");
    if (k != null)
    {
        float yMid = trClear + trH * 0.5f, side = trW * 0.5f + inset * 0.5f, front = trL * 0.5f + inset * 0.5f;
        S("Body", k, V(0f, yMid, 0f), V(trW, trH, trL), skin, true);
        foreach (var sx in new[] { -1f, 1f }) S("Belt", k, V(sx * side, trClear + 0.45f, 0f), V(inset, 0.25f, trL), belt);
        S("RoofCap", k, V(0f, trClear + trH + 0.06f, 0f), V(trW + 0.06f, 0.12f, trL + 0.06f), roofM);
        foreach (var sx in new[] { -1f, 1f }) foreach (var wz in new[] { 1.4f, -1.3f }) S("Window", k, V(sx * side, trClear + 1.35f, wz), V(inset, 0.5f, 0.9f), glass);
        S("WindowFront", k, V(0f, trClear + 1.35f, front), V(1.4f, 0.45f, inset), glass);
        S("Door", k, V(side + inset, trClear + 0.05f + 0.875f, 0.1f), V(inset, 1.75f, 0.7f), roofM); S("DoorWindow", k, V(side + inset * 2f, trClear + 1.4f, 0.1f), V(inset, 0.35f, 0.4f), glass);
        foreach (var sx in new[] { -1f, 1f }) Tyre(k, V(sx * (trW * 0.5f - trTyreW * 0.5f), trTyreR, -0.3f), trTyreR, trTyreW);
        // the A-frame hitch: two drawbars from the front corners to the coupler, a jack post; all under 0.5 m
        var tip = V(0f, 0.4f, trL * 0.5f + 1.1f);
        foreach (var sx in new[] { -0.55f, 0.55f }) { var a = V(sx, 0.4f, trL * 0.5f); var bar = S("Drawbar", k, (a + tip) * 0.5f, V(0.08f, 0.08f, UnityEngine.Vector3.Distance(a, tip)), steelM); bar.transform.localRotation = UnityEngine.Quaternion.LookRotation(tip - a); }
        S("Coupler", k, tip + V(0f, 0f, 0.12f), V(0.12f, 0.1f, 0.25f), steelM); S("Jack", k, V(0f, 0.2f, trL * 0.5f + 0.6f), V(0.06f, 0.4f, 0.06f), steelM);
    }
}

// ================= PICKUP =================
const float tkSink = 0.25f, tkLean = 8f, tkW = 1.9f, tkTyreR = 0.38f, tkTyreW = 0.25f, bedZ0 = -1.9f, bedZ1 = 0.7f, bedY0 = 0.55f, bedY1 = 1.1f, wallT = 0.08f, siltBelow = 0.08f;
{
    var k = KitOf("POI_Washed_out_truck", tkLean, tkSink, "Bed", "Cab");
    if (k != null)
    {
        float half = tkW * 0.5f;
        S("Cab", k, V(0f, 1.25f, 1.65f), V(tkW, 1.3f, 1.5f), paint, true);
        S("Hood", k, V(0f, 0.8f, 3.05f), V(tkW, 0.75f, 1.3f), paint, true);
        S("Windscreen", k, V(0f, 1.6f, 2.4f + inset * 0.5f), V(1.6f, 0.5f, inset), glass); S("RearWindow", k, V(0f, 1.55f, 0.9f - inset * 0.5f), V(1.4f, 0.4f, inset), glass);
        foreach (var sx in new[] { -1f, 1f }) { S("SideWindow", k, V(sx * (half + inset * 0.5f), 1.6f, 1.6f), V(inset, 0.45f, 1.0f), glass); foreach (var sz in new[] { 1.0f, 2.3f }) S("DoorSeam", k, V(sx * (half + inset * 0.5f), 1.0f, sz), V(inset, 0.8f, 0.03f), glass); }
        // the bed: walls round a silt fill siltBelow under their tops, one box over all
        float bl = bedZ1 - bedZ0, bc = (bedZ0 + bedZ1) * 0.5f, wh = bedY1 - bedY0;
        foreach (var sx in new[] { -1f, 1f }) S("BedSide", k, V(sx * (half - wallT * 0.5f), bedY0 + wh * 0.5f, bc), V(wallT, wh, bl), paint);
        S("Tailgate", k, V(0f, bedY0 + wh * 0.5f, bedZ0 + wallT * 0.5f), V(tkW, wh, wallT), paint); S("BedFront", k, V(0f, bedY0 + wh * 0.5f, bedZ1 - wallT * 0.5f), V(tkW, wh, wallT), paint);
        S("Silt", k, V(0f, (bedY0 + bedY1 - siltBelow) * 0.5f, bc), V(tkW - 2f * wallT, wh - siltBelow, bl - 2f * wallT), silt);
        var bedBox = kit.Blocker("BedBox", k, V(0f, (bedY0 + bedY1) * 0.5f, bc), V(tkW, wh, bl));
        S("BumperFront", k, V(0f, 0.55f, 3.78f), V(2.0f, 0.18f, 0.15f), steelM); S("BumperRear", k, V(0f, 0.5f, bedZ0 - 0.08f), V(2.0f, 0.15f, 0.12f), steelM);
        foreach (var sx in new[] { -1f, 1f }) S("Headlamp", k, V(sx * 0.65f, 0.95f, 3.7f + inset * 0.5f), V(0.25f, 0.18f, inset), lamp);
        foreach (var sx in new[] { -1f, 1f }) foreach (var wz in new[] { 2.8f, -1.2f }) Tyre(k, V(sx * (half - tkTyreW * 0.5f + 0.05f), tkTyreR, wz), tkTyreR, tkTyreW);
    }
}

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | tyres " + tyres + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
