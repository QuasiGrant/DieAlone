// Main3 task 8.22, the front zone to FrontLayout.md draft 2 (Sable 2026-10-02): layout and walkability only, dressing is 11.0.
// Edit mode, Main3; rerunnable (every move is to an absolute place; built groups are rebuilt). In the runner after main3_8_21_camp.cs
// and before main3_8_18a_solid.cs (which gives new meshes with no collider their hulls; every piece here that the body can meet has its
// own). Wren's calls 2026-10-02: the booth on the driver's side with a lift barrier, no turning circle (refused cars back onto the apron),
// a shift starts on entering the booth, IW3 on while an admitted car is on the spur, the office door is the built west door.
// 1. Gate (doc 2.1, 2.2, 4.1): the old booth at (392, 176), the turning circle and the old arm at x 396 go; the new booth x 390.6 to
//    393.2, z 164.2 to 166.6 (window north, 1.0 m doorway south, counter 1.0 high with a drawer, the stool pushed under the counter so it
//    needs no collider, a shutter, a roof light); the barrier post at (389.3, 166.3), the arm over the lane to z 172.5 with a collider
//    (down); IW1 flush with the gate posts (z 167.35 to 172.65) on Ignore Raycast; the spur mouth flared x 381 to 389; the apron x 400 to
//    412, z 162 to 178; the car stop points (FrontZone/CarStops: the gate stop, the refused car backed onto the apron, the spur, the
//    chain, the ring join and P1 to P8, nose-in, P1 first counter-clockwise from the entry). Car behaviour is Milestone 10.
//    The highway reflector posts in the drive mouth at the T go (a refused car leaving hit one); the mast stands against the brush band
//    with one base collider round its legs (the gap scan found 0.64 to 0.95 m slots there).
// 2. IW3 (doc 2.5, 4.2): the shift wall at the brush gap is active, on Ignore Raycast, with SpurWall (off until an admitted car is on
//    the spur). The chain hangs chainH m with its collider (doc 2.4).
// 3. Office (doc 2.6, 2.7): the west door propped open inward with no prompt (Door startOpen, fixedSwing +1, noPrompt; reopened at every
//    wake, shut only by HoldShut for an Office CHECK); a porch lamp over it lit only while it is open (DoorLamp; doc 2.7, the open and
//    shut deck frames did not differ clearly without it); the porch sign onto the wall south of the door; the counter, stool, radio, chair,
//    stove, cabinets, map board and blind in the front room; bed, trunk, desk, sink, hot plate and locker in the back room (O1 to O6).
// 4. Store (doc 2.8): the door widened to 1.0 m (x 364.5 to 365.5): the door module scaled up and its two window neighbours down, so
//    the run stays 12 m; the built interior stays (the store level's depth 0); the checkout counter flush to the front wall; the canopy
//    x 360.5 to 371.9 with posts at (360.7, 193.8) and (371.9, 193.8); the ice chest (the built freezer) against the wall at x 367.9 to
//    369.7; the bench and bin off the 0.6 to 1.0 m band.
// 5. Lot (doc 2.9): stall lines (4 by the office, one 3.6 m, 4 by the store, 6 trailhead stalls z 153 to 169.2), the walk z 190 to
//    193.5 to the porch and the west door; the verge tree's snapped top along the verge, off the road; the T stop sign on the exit's
//    right side (423.1, 166.0).
// 6. Vault toilet (doc 2.10): x 340.4 to 342.0, door east, the CITW outhouse with box walls and a bench, as the 8.21 privy; its face is
//    the outhouse's own 2.0 m, so z 185.0 to 187.0 (doc 185.1 to 186.9).
// 7. Campground (doc 2.3): every forest piece (logs, trunks, stumps, saplings, bushes) on the spur, the ring road or a pitch (cars
//    included), plus clearM m, moves to the nearest free spot off them inside the campground; one with no spot within searchMax m is
//    removed and listed.
// 8. Warps (doc 5.2): Gate_Booth to (392, 162.5) facing north. The PlayerInteractor's mask in Player.prefab leaves out Ignore Raycast
//    (doc 4.1), so IW1, IW3 and the hedge boxes never take the eye ray.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 V2(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var fzG = kit.Root("FrontZone"); if (fzG == null) return "no FrontZone (run 8.6 first)"; var fz = fzG.transform;
var office = fz.Find("Office"); var store = fz.Find("Store"); var gate = fz.Find("Gate"); var surf = fz.Find("Surfaces");
if (office == null || store == null || gate == null || surf == null) return "run 8.6 and 8.17 first (FrontZone/Office, Store, Gate, Surfaces)";
var notes = new System.Collections.Generic.List<string>();
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
const float G = 3f, slab = 0.02f, paintY = 0.03f;
UnityEngine.Transform Need(UnityEngine.Transform p, string n) { var t = p != null ? p.Find(n) : null; if (t == null) notes.Add("no " + (p != null ? p.name : "?") + "/" + n); return t; }
UnityEngine.Transform ByName(UnityEngine.Transform p, string n, int nth = 0) { int k = 0; foreach (UnityEngine.Transform c in p) if (c.name == n && k++ == nth) return c; notes.Add("no " + p.name + "/" + n + (nth > 0 ? " #" + nth : "")); return null; }
// moves t (and so its children) so its mesh bounds' min corner (x, z) lands on (x, z); y untouched
void MinTo(UnityEngine.Transform t, float x, float z) { if (t == null) return; var b = PlaceKit.MeshBounds(t.gameObject); t.position += V(x - b.min.x, 0f, z - b.min.z); }
void CentreTo(UnityEngine.Transform t, float x, float z) { if (t == null) return; var b = PlaceKit.MeshBounds(t.gameObject); t.position += V(x - b.center.x, 0f, z - b.center.z); }

// ---- materials
var planks = "Assets/Materials/Planks023A_1.0x1.0.mat"; var concrete = "Assets/Materials/Concrete034_1.0x1.0.mat";
var boothWall = kit.Tinted("Places_BoothWall", planks, Hex("#6A6656"), new UnityEngine.Vector2(2f, 2f));
var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
var roofChar = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_RoofChar.mat");
var paint = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_RoadPaint.mat");
var refl = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_Reflector.mat");
var gravel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Gravel.mat");
var asphalt = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Asphalt.mat");
var campMap = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/CampMap.mat");
if (steel == null || roofChar == null || paint == null || refl == null || gravel == null || asphalt == null || campMap == null) return "materials missing (Slice_Steel, Slice_RoofChar, Blockout_RoadPaint, Blockout_Reflector, Slice_Gravel, Slice_Asphalt, CampMap)";
var apronMat = kit.Tinted("Places_Apron", concrete, Hex("#77726A"), new UnityEngine.Vector2(12f, 2f));
var blindMat = kit.Tinted("Places_Blind", planks, Hex("#B8AE96"), new UnityEngine.Vector2(1f, 1f));
var signGlow = kit.Glow("Places_SignGlow", kit.Look.practicalColor, kit.Look.cabWindowGlowIntensity);
var boardWood = kit.Tinted("Places_SignBoard", planks, Hex("#3E4A3A"), new UnityEngine.Vector2(2f, 1f));

// ================= 1. GATE, BOOTH, BARRIER =================
PlaceKit.Remove(fz.Find("GateBooth")); PlaceKit.Remove(surf.Find("TurningCircle")); PlaceKit.Remove(gate.Find("BarrierArm"));
const float bX0 = 390.6f, bX1 = 393.2f, bZ0 = 164.2f, bZ1 = 166.6f, bT = 0.1f, bH = 2.5f, bWinX0 = 391.0f, bWinX1 = 392.8f, bSill = 1.0f, bHead = 2.1f;
const float bDoorX0 = 391.4f, bDoorX1 = 392.4f, bCounterD = 0.4f, bRoofOver = 0.2f, bRoofT = 0.15f, stoolR = 0.16f, stoolH = 0.7f;
var booth = kit.Fresh("GateBooth", fz, V((bX0 + bX1) * 0.5f, G, (bZ0 + bZ1) * 0.5f), 0f);
UnityEngine.GameObject BW(string n, float x0, float x1, float y0, float y1, float z0, float z1, UnityEngine.Material m, bool col = true) =>
    kit.Slab(n, booth, booth.InverseTransformPoint(V((x0 + x1) * 0.5f, G + (y0 + y1) * 0.5f, (z0 + z1) * 0.5f)), V(x1 - x0, y1 - y0, z1 - z0), m, default, col);
BW("Floor", bX0, bX1, 0f, 0.03f, bZ0, bZ1, apronMat, false);
// north wall with the window on the lane (x bWinX0 to bWinX1, sill bSill, head bHead)
BW("Wall_N_Sill", bX0, bX1, 0f, bSill, bZ1 - bT, bZ1, boothWall); BW("Wall_N_Head", bX0, bX1, bHead, bH, bZ1 - bT, bZ1, boothWall);
BW("Wall_N_W", bX0, bWinX0, bSill, bHead, bZ1 - bT, bZ1, boothWall); BW("Wall_N_E", bWinX1, bX1, bSill, bHead, bZ1 - bT, bZ1, boothWall);
// south wall with the 1.0 m doorway
BW("Wall_S_W", bX0, bDoorX0, 0f, bH, bZ0, bZ0 + bT, boothWall); BW("Wall_S_E", bDoorX1, bX1, 0f, bH, bZ0, bZ0 + bT, boothWall); BW("Wall_S_Head", bDoorX0, bDoorX1, bHead, bH, bZ0, bZ0 + bT, boothWall);
BW("Wall_W", bX0, bX0 + bT, 0f, bH, bZ0 + bT, bZ1 - bT, boothWall); BW("Wall_E", bX1 - bT, bX1, 0f, bH, bZ0 + bT, bZ1 - bT, boothWall);
BW("Roof", bX0 - bRoofOver, bX1 + bRoofOver, bH, bH + bRoofT, bZ0 - bRoofOver, bZ1 + bRoofOver, roofChar);
// the counter along the window, 1.0 high, solid to the floor; its drawer front faces the stool; the shutter rolled up under the head
BW("Counter", bX0 + bT, bX1 - bT, 0f, bSill, bZ1 - bT - bCounterD, bZ1 - bT, boothWall);
BW("Drawer", bWinX0 + 0.5f, bWinX0 + 1.0f, bSill - 0.25f, bSill - 0.08f, bZ1 - bT - bCounterD - 0.02f, bZ1 - bT - bCounterD, steel, false);
BW("Shutter", bWinX0, bWinX1, bHead - 0.12f, bHead, bZ1 - bT * 0.5f - 0.06f, bZ1 - bT * 0.5f + 0.06f, steel, false);
// the stool pushed in under the counter (doc 2.1: no collider; inside the counter's collider the body never meets it)
var stool = kit.On(PlaceKit.CI + "Furniture/CITW_Stool_1", booth, booth.InverseTransformPoint(V((bWinX0 + bWinX1) * 0.5f, G + 0.03f, bZ1 - bT - bCounterD * 0.5f)), 0f, 1f, false, null, true);
if (stool != null) { PlaceKit.StripColliders(stool); var sb0 = PlaceKit.MeshBounds(stool); float s = UnityEngine.Mathf.Min(2f * stoolR / UnityEngine.Mathf.Max(sb0.size.x, sb0.size.z), stoolH / sb0.size.y); stool.transform.localScale *= s; CentreTo(stool.transform, (bWinX0 + bWinX1) * 0.5f, bZ1 - bT - bCounterD * 0.5f); var sb1 = PlaceKit.MeshBounds(stool); stool.transform.position += V(0f, G + 0.03f - sb1.min.y, 0f); stool.name = "Stool"; }
// the roof light: a lit box on the roof (seen from the deck, doc 5.4) and the lamp under the roof
var roofLamp = BW("RoofLight", (bX0 + bX1) * 0.5f - 0.2f, (bX0 + bX1) * 0.5f + 0.2f, bH + bRoofT, bH + bRoofT + 0.25f, (bZ0 + bZ1) * 0.5f - 0.2f, (bZ0 + bZ1) * 0.5f + 0.2f, signGlow, false);
kit.On(PlaceKit.CE + "Decoration_Lamps/Cage_Light", booth, booth.InverseTransformPoint(V((bX0 + bX1) * 0.5f, G + bH - 0.35f, (bZ0 + bZ1) * 0.5f)), 0f, 1f, false, null, true);
kit.Practical("BoothLight", booth, booth.InverseTransformPoint(V((bX0 + bX1) * 0.5f, G + bH - 0.45f, (bZ0 + bZ1) * 0.5f)), 6f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
// the booth floor covers the open-ground cover and the grass (Marlow 8.22 paper 13)
int coverGone = 0; { var slice = kit.Root("SliceLook"); var foot = new UnityEngine.Bounds(V((bX0 + bX1) * 0.5f, G + 1f, (bZ0 + bZ1) * 0.5f), V(bX1 - bX0 + 0.6f, 4f, bZ1 - bZ0 + 0.6f));
    if (slice != null) foreach (var grp in new[] { "OpenGround", "ShotGround" }) { var g = slice.transform.Find(grp); if (g == null) continue; foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(g))) { var r = t.GetComponentInChildren<UnityEngine.Renderer>(); if (r != null && r.bounds.Intersects(foot)) { UnityEngine.Object.DestroyImmediate(t.gameObject); coverGone++; } } } }
int detailGone = kit.ClearDetail(V((bX0 + bX1) * 0.5f, G, (bZ0 + bZ1) * 0.5f), 2.2f);
// the barrier: post 0.2 x 0.2 at (389.3, 166.3), the arm armH over the lane to z 172.5, striped, with its collider (down)
const float postX = 389.3f, postZ = 166.3f, postS = 0.2f, postH = 1.15f, armH = 1.0f, armT = 0.1f, armZ1 = 172.5f, stripe = 0.5f;
var barrier = kit.Fresh("Barrier", gate, V(postX, G, postZ), 0f);
kit.Slab("Post", barrier, V(0f, postH * 0.5f, 0f), V(postS, postH, postS), steel, default, true);
var arm = kit.Slab("BarrierArm", barrier, V(0f, armH, (armZ1 - (postZ + postS * 0.5f)) * 0.5f + postS * 0.5f), V(armT, armT, armZ1 - (postZ + postS * 0.5f)), paint, default, true);
for (float z = postZ + postS * 0.5f + stripe; z < armZ1 - stripe * 0.5f; z += stripe * 2f) kit.Slab("Stripe", barrier, V(0f, armH, z - postZ + stripe * 0.5f), V(armT + 0.01f, armT + 0.01f, stripe), refl);
// IW1 flush with the gate posts (doc 4.1; Marlow 8.22 paper 17), on Ignore Raycast
var gPosts = new System.Collections.Generic.List<UnityEngine.Collider>(); foreach (UnityEngine.Transform c in gate) if (c.name == "Post") gPosts.Add(c.GetComponent<UnityEngine.Collider>());
var iw1 = Need(gate, "PlayerBlocker");
if (iw1 != null && gPosts.Count == 2)
{
    float zA = UnityEngine.Mathf.Min(gPosts[0].bounds.max.z, gPosts[1].bounds.max.z), zB = UnityEngine.Mathf.Max(gPosts[0].bounds.min.z, gPosts[1].bounds.min.z);
    var bc = iw1.GetComponent<UnityEngine.BoxCollider>(); iw1.position = V(iw1.position.x, iw1.position.y, (zA + zB) * 0.5f); bc.center = UnityEngine.Vector3.zero; bc.size = V(bc.size.x, bc.size.y, zB - zA); iw1.gameObject.layer = 2;
}
else notes.Add("IW1: no Gate/PlayerBlocker or not two gate posts");
// the spur mouth flared x 381 to 389 (doc 2.2) and the apron (doc 2.2, Quill 6); colliderless surfaces like 8.6's
const float mouthX0 = 381f, mouthX1 = 389f, mouthZ0 = 172.4f, mouthZ1 = 176f, apX0 = 400f, apX1 = 412f, apZ0 = 162f, apZ1 = 178f;
PlaceKit.Remove(surf.Find("SpurMouth")); kit.Slab("SpurMouth", surf, surf.InverseTransformPoint(V((mouthX0 + mouthX1) * 0.5f, G + slab * 0.5f, (mouthZ0 + mouthZ1) * 0.5f)), V(mouthX1 - mouthX0, slab, mouthZ1 - mouthZ0), gravel);
PlaceKit.Remove(surf.Find("Apron")); kit.Slab("Apron", surf, surf.InverseTransformPoint(V((apX0 + apX1) * 0.5f, G + slab * 0.5f, (apZ0 + apZ1) * 0.5f)), V(apX1 - apX0, slab, apZ1 - apZ0), asphalt);
// car stop points (layout only; Milestone 10 drives them): a car is carL x carW; it stops with its driver's door carDoorOff m off the
// window and its front at the arm; refused, it backs refuseBack m; admitted, it parks nose-in on the next empty pitch from the entry
const float carL = 4.5f, carW = 1.8f, carDoorOff = 1.2f, carFront = 389.5f, refuseBack = 14f, ringCX = 372f, ringCZ = 262f, ringR = 17.5f, ringHalf = 2.5f, spurHalf = 2f, pitchIn = 9.5f, pitchOut = 14f, pitchHalfW = 1.5f;
var stops = kit.Fresh("CarStops", fz, V(0f, G, 0f), 0f);
float laneZ = bZ1 + carDoorOff + carW * 0.5f;
kit.Marker("Gate_Stop", stops, V(carFront + carL * 0.5f, 0f, laneZ), 270f);
kit.Marker("Refused_Backed", stops, V(carFront + refuseBack + carL * 0.5f, 0f, laneZ), 270f);
kit.Marker("Refused_Turned", stops, V((apX0 + apX1) * 0.5f, 0f, 170f + 1.25f), 90f);
kit.Marker("Spur_Mouth", stops, V(385f, 0f, 176f), 0f);
kit.Marker("Chain", stops, V(390f, 0f, 236f), 0f);
kit.Marker("Ring_Join", stops, V(387.5f, 0f, 252f), 345f);
var pitchList = new System.Collections.Generic.List<(float ang, UnityEngine.Transform t)>();
var pitchesRoot = Need(fz, "EmptyPitches");
if (pitchesRoot != null) foreach (UnityEngine.Transform p in pitchesRoot) { float a = UnityEngine.Mathf.Atan2(p.position.z - ringCZ, p.position.x - ringCX) * UnityEngine.Mathf.Rad2Deg; pitchList.Add((a, p)); }
float entryAng = UnityEngine.Mathf.Atan2(252f - ringCZ, 387.5f - ringCX) * UnityEngine.Mathf.Rad2Deg;
pitchList.Sort((x, y) => UnityEngine.Mathf.Repeat(x.ang - entryAng, 360f).CompareTo(UnityEngine.Mathf.Repeat(y.ang - entryAng, 360f)));   // counter-clockwise from the entry
for (int i = 0; i < pitchList.Count; i++)
{
    float a = pitchList[i].ang * UnityEngine.Mathf.Deg2Rad, rc = (pitchIn + pitchOut) * 0.5f; var dir = V(UnityEngine.Mathf.Cos(a), 0f, UnityEngine.Mathf.Sin(a));
    kit.Marker("P" + (i + 1), stops, V(ringCX, 0f, ringCZ) + dir * rc, UnityEngine.Quaternion.LookRotation(-dir).eulerAngles.y);
    pitchList[i].t.name = "Pitch_P" + (i + 1);
}
// the gate T stop sign on the exit's right side (doc 5.4); the verge tree's snapped top along the verge, off the road (doc 2.9)
var gT = Need(fz, "GateT");
if (gT != null) foreach (UnityEngine.Transform c in gT) if (c.name == "StopSign" || c.name == "StopSignPost") c.position = V(423.05f, c.position.y, 166f);
const float roadEdgeX = 424.25f, topX = 421.6f, topZ = 143.5f;
var snapped = fz.Find("VergeTree/SnappedTop"); float snappedMaxX = 0f;
if (snapped != null)
{
    // lying as 8.6 laid it (tilt 80 about its own z); turned so its length runs along the road
    float best = float.MaxValue, bestYaw = 0f;
    for (float yaw = 0f; yaw < 180f; yaw += 5f) { snapped.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 80f); var b = PlaceKit.MeshBounds(snapped.gameObject); if (b.size.x < best) { best = b.size.x; bestYaw = yaw; } }
    snapped.rotation = UnityEngine.Quaternion.Euler(0f, bestYaw, 80f); CentreTo(snapped, topX, topZ); var b2 = PlaceKit.MeshBounds(snapped.gameObject);
    snapped.position += V(0f, kit.H(topX, topZ) - 0.3f - b2.min.y, 0f); snappedMaxX = PlaceKit.MeshBounds(snapped.gameObject).max.x;
    if (snappedMaxX > roadEdgeX) notes.Add("snapped top reaches the road (max x " + F(snappedMaxX) + ")");
}
else notes.Add("no VergeTree/SnappedTop");

// the mast's north legs against the brush band (gap scan: 0.95 m between its east leg and the band, in the 0.6 to 1.0 band); the legs
// stand mastToBrush m off the band
const float mastToBrush = 0.05f;
var mast = Need(fz, "Mast"); var brushBand = fz.Find("BrushBands/Brush");
if (mast != null && brushBand != null)
{
    float legMaxZ = float.MinValue; foreach (UnityEngine.Transform c in mast) if (c.name == "Leg") legMaxZ = UnityEngine.Mathf.Max(legMaxZ, c.GetComponent<UnityEngine.Collider>().bounds.max.z);
    float dz = brushBand.GetComponent<UnityEngine.Collider>().bounds.min.z - mastToBrush - legMaxZ; if (UnityEngine.Mathf.Abs(dz) > 1e-3f) foreach (UnityEngine.Transform c in mast) c.position += V(0f, 0f, dz);
}

// one base collider round the mast's three legs (gap scan: 0.64 and 0.68 m slots between the legs into a triangle too small to stand in)
const float mastBaseH = 2.5f;
if (mast != null)
{
    PlaceKit.Remove(mast.Find("MastBase")); bool anyLeg = false; var lb = new UnityEngine.Bounds();
    foreach (UnityEngine.Transform c in mast) if (c.name == "Leg") { var b = c.GetComponent<UnityEngine.Collider>().bounds; if (!anyLeg) { lb = b; anyLeg = true; } else lb.Encapsulate(b); }
    if (anyLeg) { var mb = new UnityEngine.GameObject("MastBase"); mb.transform.SetParent(mast, true); mb.transform.position = V(lb.center.x, G + mastBaseH * 0.5f, lb.center.z); mb.AddComponent<UnityEngine.BoxCollider>().size = V(lb.size.x, mastBaseH, lb.size.z); }
}
// the highway's reflector posts in the drive's mouth at the T (swept path: a refused car leaving hit the one at z 170) go
const float driveMouthHalf = 3.5f; int reflGone = 0;
{ var hw = fz.Find("Highway"); if (hw != null) foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(hw))) if ((t.name == "ReflectorPost" || t.name == "Reflector") && t.position.x < 428f && UnityEngine.Mathf.Abs(t.position.z - 170f) < driveMouthHalf) { UnityEngine.Object.DestroyImmediate(t.gameObject); reflGone++; } }

// ================= 2. IW3 AND THE CHAIN =================
var shift = Need(fz, "ShiftWalls"); var iw3 = shift != null ? shift.Find("IW3_SpurGap") : null;
if (iw3 != null)
{
    shift.gameObject.SetActive(true); iw3.gameObject.layer = 2;
    var sw = iw3.GetComponent<SpurWall>(); if (sw == null) sw = iw3.gameObject.AddComponent<SpurWall>();
    iw3.GetComponent<UnityEngine.BoxCollider>().enabled = false;   // as SpurWall starts it: no admitted car on the spur
}
else notes.Add("no ShiftWalls/IW3_SpurGap");
const float chainH = 0.8f;
var chainG = Need(fz, "Chain"); if (chainG != null) { var ch = chainG.Find("Chain"); if (ch != null) ch.position = V(ch.position.x, G + chainH, ch.position.z); }

// ================= 3. OFFICE =================
// doc local metres: origin the inside south-west corner (344.25, 196.25), X east, Z north
const float oX = 344.25f, oZ = 196.25f, oFloor = 3.05f;
UnityEngine.Vector3 W(float X, float Z, float y = 0f) => V(oX + X, oFloor + y, oZ + Z);
// O1 the west door: open inward, no prompt (the hinge's back is east, into the room: side +1)
UnityEngine.Transform westDoor = null, backDoor = null;
foreach (UnityEngine.Transform c in office) if (c.GetComponent<Door>() != null) { if (c.position.x < 346f) westDoor = c; else backDoor = c; }
if (westDoor != null) westDoor.name = "WestDoor"; if (backDoor != null) backDoor.name = "BackDoor";   // named apart, so the area data can point at each
if (westDoor != null) { var so = new UnityEditor.SerializedObject(westDoor.GetComponent<Door>()); so.FindProperty("startOpen").boolValue = true; so.FindProperty("fixedSwing").boolValue = true; so.FindProperty("fixedSide").floatValue = 1f; so.FindProperty("noPrompt").boolValue = true; so.ApplyModifiedPropertiesWithoutUndo(); }
else notes.Add("no office west door");
// the porch sign off the opening (doc 2.7, Marlow 3): on the wall south of the door, facing west
var porch = Need(office, "Porch");
if (porch != null)
{
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(porch))) if (t.name == "Label") UnityEngine.Object.DestroyImmediate(t.gameObject);
    var sign = porch.Find("SignBoard");
    if (sign != null) { const float signLen = 1.9f, signY = 2.1f; sign.position = V(344f - 0.04f, oFloor + signY, oZ + 0.2f + signLen * 0.5f); sign.localScale = V(signLen, sign.localScale.y, sign.localScale.z); kit.Label(sign, "RANGER STATION", Hex("#EDE3CF"), 40); }
}
// the porch lamp over the west door, lit only while the door stands open (doc 2.7: from the deck the open and shut frames did not
// differ clearly, 16 percent of the opening's pixels in binoculars, 2026-10-02); DoorLamp shows its glow and light with the door
const float lampW = 1.2f, lampH = 0.25f, lampD = 0.12f, lampY = 2.33f, lampRange = 6f;   // a lit bar the door's width over its head (0.45 m read as a few pixels through the filter)
if (porch != null && westDoor != null)
{
    PlaceKit.Remove(porch.Find("DoorLamp")); var host = new UnityEngine.GameObject("DoorLamp"); host.transform.SetParent(porch, false); host.transform.position = V(344f - lampD * 0.5f - 0.01f, oFloor + lampY, oZ + 2.75f);
    var lampG = new UnityEngine.GameObject("Lamp"); lampG.transform.SetParent(host.transform, false);
    kit.Slab("Glow", lampG.transform, UnityEngine.Vector3.zero, V(lampD, lampH, lampW), signGlow);
    kit.Practical("DoorLampLight", lampG.transform, V(-0.3f, -0.1f, 0f), lampRange, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
    var dl = host.AddComponent<DoorLamp>(); var so = new UnityEditor.SerializedObject(dl); so.FindProperty("door").objectReferenceValue = westDoor.GetComponent<Door>(); so.FindProperty("lamp").objectReferenceValue = lampG; so.ApplyModifiedPropertiesWithoutUndo();
}
var fr = Need(office, "FrontRoom"); var br = Need(office, "BackRoom");
var lay = kit.Fresh("Layout822", office, office.position, 0f);
if (fr != null && br != null)
{
    // O3 the counter X 2.2 to 2.8, Z 0 to 5.0, 1.0 high: five base cabinets, drawers to the ranger's side, one box collider
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(fr))) if (t.name == "Drawer_Cabinet") UnityEngine.Object.DestroyImmediate(t.gameObject);
    const float cX0 = 2.2f, cX1 = 2.8f, cZ1 = 5.0f, cH = 1.0f;
    var counter = kit.Group("Counter", lay, W(0f, 0f), 0f);
    for (int i = 0; i < 5; i++) kit.Fill(PlaceKit.CE + "Decoration_Kitchen/Kitchen_Countertop_Base_Cabinet", counter, counter.InverseTransformPoint(W((cX0 + cX1) * 0.5f, i + 0.5f)), V(cX1 - cX0, cH, 1f), 90f, false);
    kit.Blocker("CounterCollider", counter, counter.InverseTransformPoint(W((cX0 + cX1) * 0.5f, cZ1 * 0.5f, cH * 0.5f)), V(cX1 - cX0, cH, cZ1));
    // the stool (3.3, 2.75), R5's spot on it; the radio on the counter's north end (2.5, 4.5); stamps, pad, mug and the desk lamp on the counter
    var rStool = kit.On(PlaceKit.CI + "Furniture/CITW_Stool_1", lay, lay.InverseTransformPoint(W(3.3f, 2.75f)), 0f, 1f, true, null, true); if (rStool != null) rStool.name = "R5_Stool";
    var spot = fr.Find("Resident_Office_Spot"); if (spot != null) { spot.position = W(3.3f, 2.75f); spot.rotation = UnityEngine.Quaternion.Euler(0f, 270f, 0f); }
    var radio = fr.Find("Radio"); if (radio != null) { CentreTo(radio, oX + 2.5f, oZ + 4.5f); radio.position = V(radio.position.x, oFloor + cH + (radio.position.y - PlaceKit.MeshBounds(radio.gameObject).min.y), radio.position.z); }
    void OnCounter(UnityEngine.Transform t, float X, float Z) { if (t == null) return; CentreTo(t, oX + X, oZ + Z); var b = PlaceKit.MeshBounds(t.gameObject); t.position += V(0f, oFloor + cH - b.min.y, 0f); }
    OnCounter(fr.Find("Mug"), 2.6f, 2.2f); OnCounter(fr.Find("Ballpoint_Pen"), 2.4f, 3.1f); OnCounter(fr.Find("Table_Lamp"), 2.5f, 3.9f);
    { int k = 0; foreach (UnityEngine.Transform t in fr) if (t.name == "Paper") { CentreTo(t, oX + 2.5f, oZ + 2.75f); t.position = V(t.position.x, oFloor + cH + k * 0.004f, t.position.z); k++; } }
    var stamps = kit.On(PlaceKit.CE + "Decoration_Home/Paper", lay, lay.InverseTransformPoint(W(2.5f, 3.3f, cH)), 0f, 0.4f, false, null, true); if (stamps != null) stamps.name = "StampPad";
    var frLamp = fr.Find("FrontRoomLamp"); if (frLamp != null) frLamp.position = W(2.5f, 3.9f, cH + 0.75f);
    // O2 the chair in the north-west corner X 0.2 to 0.7, Z 6.9 to 7.5; the map board on the south wall (no collider); a blind over the
    // top of the west window (Z 4.15 to 5.35)
    var chair = fr.Find("Chair"); if (chair != null) { chair.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); MinTo(chair, oX + 0.2f, oZ + 6.9f); }
    var map = kit.Slab("MapBoard", lay, lay.InverseTransformPoint(W(4.0f, 0.03f, 1.6f)), V(1.4f, 0.9f, 0.03f), campMap); map.transform.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f);
    kit.Slab("Blind", lay, lay.InverseTransformPoint(W(0.03f, 4.75f, 1.95f)), V(0.02f, 0.4f, 1.2f), blindMat);
    // O4 the stove 0.55 m west, X 5.6 to 6.4, Z 0.35 to 1.15, its pipe, kettle and glow with it; the filing cabinets and one drawer
    // cabinet on the north wall X 5.4 to 7.4, Z 6.9 to 7.5
    var stove = fr.Find("Potbelly_Stove");
    if (stove != null) { var sb = PlaceKit.MeshBounds(stove.gameObject); var d = V(oX + 6.0f - sb.center.x, 0f, oZ + 0.75f - sb.center.z); foreach (var n in new[] { "Potbelly_Stove", "Potbelly_Stove_Pipe_Long", "CITW_Kettle", "StoveGlow" }) { var t = fr.Find(n); if (t != null) t.position += d; } }
    var fc0 = ByName(fr, "Filing_Cabinet", 0); var fc1 = ByName(fr, "Filing_Cabinet", 1);
    if (fc0 != null) MinTo(fc0, oX + 5.4f, oZ + 6.9f); if (fc1 != null) MinTo(fc1, oX + 5.4f + PlaceKit.MeshBounds(fc0 != null ? fc0.gameObject : fc1.gameObject).size.x + 0.02f, oZ + 6.9f);
    float drawerX0 = fc1 != null ? PlaceKit.MeshBounds(fc1.gameObject).max.x + 0.02f : oX + 6.8f;
    kit.Fill(PlaceKit.CE + "Furniture/Drawer_Cabinet", lay, lay.InverseTransformPoint(V((drawerX0 + oX + 7.4f) * 0.5f, oFloor, oZ + 7.2f)), V(oX + 7.4f - drawerX0, 1.13f, 0.6f), 180f, true);
    // O6 the back room: the bed X 10.6 to 11.5, Z 5.3 to 7.3 (the cot, boots on); the trunk at its foot; the desk X 10.8 to 11.5,
    // Z 2.4 to 3.6, its chair; the sink and hot plate X 8.9 to 10.7 and the locker X 10.7 to 11.5 on the south wall, Z 0 to 0.6
    var bed = br.Find("Single_Bed_Metal");
    if (bed != null) { var bb = PlaceKit.MeshBounds(bed.gameObject); if (UnityEngine.Mathf.Abs(bb.size.x - 0.9f) > 0.02f || UnityEngine.Mathf.Abs(bb.size.z - 2.0f) > 0.02f) { float h = bb.size.y; PlaceKit.Remove(bed); var nb = kit.Fill(PlaceKit.CE + "Furniture/Single_Bed_Metal", br, br.InverseTransformPoint(W(11.05f, 6.3f)), V(0.9f, h, 2.0f), 90f, true); if (nb != null) nb.name = "Single_Bed_Metal"; } else MinTo(bed, oX + 10.6f, oZ + 5.3f); }
    var blanket = br.Find("CITW_Blanket"); if (blanket != null) { blanket.rotation = UnityEngine.Quaternion.Euler(blanket.eulerAngles.x, 90f, blanket.eulerAngles.z); CentreTo(blanket, oX + 11.05f, oZ + 6.1f); }
    var trunk = br.Find("CITW_Trunk_1"); if (trunk != null) { trunk.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f); var tb = PlaceKit.MeshBounds(trunk.gameObject); if (tb.size.z > tb.size.x) { trunk.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); tb = PlaceKit.MeshBounds(trunk.gameObject); } trunk.position += V(oX + 11.5f - 0.02f - tb.max.x, 0f, oZ + 5.3f - 0.05f - tb.max.z); }
    var desk = br.Find("Table");
    if (desk != null) { var db = PlaceKit.MeshBounds(desk.gameObject); if (UnityEngine.Mathf.Abs(db.size.x - 0.7f) > 0.02f || UnityEngine.Mathf.Abs(db.size.z - 1.2f) > 0.02f) { float h = db.size.y; PlaceKit.Remove(desk); var nd = kit.Fill(PlaceKit.CE + "Furniture/Table", br, br.InverseTransformPoint(W(11.15f, 3.0f)), V(0.7f, h, 1.2f), 0f, true); if (nd != null) nd.name = "Table"; } }
    var deskNow = br.Find("Table"); float deskTop = deskNow != null ? PlaceKit.MeshBounds(deskNow.gameObject).max.y : oFloor + 0.67f;
    void OnDesk(UnityEngine.Transform t, float X, float Z) { if (t == null) return; CentreTo(t, oX + X, oZ + Z); var b = PlaceKit.MeshBounds(t.gameObject); t.position += V(0f, deskTop - b.min.y, 0f); }
    OnDesk(br.Find("Table_Lamp"), 11.2f, 3.4f); OnDesk(br.Find("Mug"), 11.2f, 2.65f);
    var brLamp = br.Find("BackRoomLamp"); if (brLamp != null) brLamp.position = V(oX + 11.2f, deskTop + 0.65f, oZ + 3.4f);
    var bChair = br.Find("Chair"); if (bChair != null) { bChair.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); var cb = PlaceKit.MeshBounds(bChair.gameObject); bChair.position += V(oX + 10.75f - cb.max.x, 0f, oZ + 3.0f - cb.center.z); }
    var sink = kit.Group("Sink", lay, W(9.8f, 0.3f), 0f);
    for (int i = 0; i < 2; i++) kit.Fill(PlaceKit.CE + "Decoration_Kitchen/Kitchen_Countertop_Base_Cabinet", sink, sink.InverseTransformPoint(W(9.35f + i * 0.9f, 0.3f)), V(0.9f, 0.9f, 0.6f), 0f, false);
    kit.Blocker("SinkCollider", sink, sink.InverseTransformPoint(W(9.8f, 0.3f, 0.45f)), V(1.8f, 0.9f, 0.6f));
    kit.On(PlaceKit.CE + "Decoration_Bathroom/Bathroom_Sink", sink, sink.InverseTransformPoint(W(9.35f, 0.3f, 0.9f)), 0f, 1f, false, null, true);
    kit.On(PlaceKit.CE + "Decoration_Kitchen/Pot", sink, sink.InverseTransformPoint(W(10.3f, 0.3f, 0.9f)), 0f, 1f, false, null, true);   // the hot plate's pot
    var locker = kit.Fill(PlaceKit.CE + "Furniture/Wardrobe", lay, lay.InverseTransformPoint(W(11.1f, 0.3f)), V(0.8f, 1.9f, 0.6f), 0f, true); if (locker != null) locker.name = "Locker";
}
UnityEngine.Physics.SyncTransforms();

// ================= 4. STORE =================
var shell = Need(store, "Shell"); var sFront = Need(store, "Front"); var sInside = Need(store, "Inside"); var canopy = Need(store, "Canopy");
const float doorX0 = 364.5f, doorX1 = 365.5f, sFaceZ = 195.5f;
if (shell != null)
{
    // the south run: [362, 364) window, [364, 366) door, [366, 368) window, pivots at their east ends (yaw 180). The door module scales
    // so its 0.8 m opening (module x 0.61 to 1.41 of 2) is 1.0 m from x 364.5; its neighbours shrink to meet it
    const float modW = 2f, openA = 0.61f, openB = 1.41f; float sc = (doorX1 - doorX0) / (openB - openA), dW = modW * sc, dPivot = doorX1 + openA * sc;   // yaw 180: the module runs back (west) from its pivot, so its opening is pivot - openB to pivot - openA
    float westEnd = dPivot - dW, eastEnd = dPivot; float wScale = (westEnd - 362f) / modW, eScale = (368f - eastEnd) / modW;
    foreach (UnityEngine.Transform c in shell)
    {
        if (UnityEngine.Mathf.Abs(c.position.z - sFaceZ) > 0.3f) continue; var b = PlaceKit.MeshBounds(c.gameObject); float cx = b.center.x;
        if (c.name.StartsWith("Wall_Small_Door_Small")) { c.position = V(dPivot, c.position.y, c.position.z); c.localScale = V(sc, 1f, 1f); }
        else if (c.name.StartsWith("Wall_Small_Window_Large") && cx > 362f && cx < 364.5f) { c.position = V(westEnd, c.position.y, c.position.z); c.localScale = V(wScale, 1f, 1f); }
        else if (c.name.StartsWith("Wall_Small_Window_Large") && cx > 365.5f && cx < 368f) { c.position = V(368f, c.position.y, c.position.z); c.localScale = V(eScale, 1f, 1f); }
    }
    // window glass and the night glow panes in those two windows: centred on their openings, as wide as them
    float winA = 0.4f, winB = 1.6f;   // window opening, module x (8.17); each window module runs back (west) from its pivot
    float wC = westEnd - (winA + winB) * 0.5f * wScale, eC = 368f - (winA + winB) * 0.5f * eScale;
    float wWid = (winB - winA) * wScale, eWid = (winB - winA) * eScale;
    void Refit(UnityEngine.Transform t, float cx, float wid) { var b = PlaceKit.MeshBounds(t.gameObject); if (b.size.x < 0.01f) return; var s = t.localScale; t.localScale = V(s.x * wid / b.size.x, s.y, s.z); b = PlaceKit.MeshBounds(t.gameObject); t.position += V(cx - b.center.x, 0f, 0f); }
    foreach (var grp in new[] { shell, sInside }) if (grp != null) foreach (UnityEngine.Transform c in grp)
    {
        if (!(c.name == "Window_Glass" || c.name == "WindowGlow")) continue; var b = PlaceKit.MeshBounds(c.gameObject); if (b.min.z > sFaceZ + 0.7f) continue;
        if (b.center.x > 362f && b.center.x < 364f) Refit(c, wC, wWid); else if (b.center.x > 366f && b.center.x < 368f) Refit(c, eC, eWid);
    }
    // the glass door, shut (the store level loads past it later), filling the 1.0 m opening
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(shell))) if (t.name == "Door_GlassPanel") UnityEngine.Object.DestroyImmediate(t.gameObject);
    kit.Fill(PlaceKit.CE + "Building_Parts/Door_GlassPanel", shell, shell.InverseTransformPoint(V((doorX0 + doorX1) * 0.5f, G, sFaceZ + 0.1f)), V(doorX1 - doorX0, 2.1f, 0.06f), 90f, true);
}
UnityEngine.Physics.SyncTransforms();
// the checkout counter flush to the front wall's inside face (Marlow 8.22 paper 8: it left 0.65 m), its register with it
if (sInside != null)
{
    float wallIn = sFaceZ + 0.23f; var cc = sInside.Find("Checkout_Counter"); var reg = sInside.Find("Cash_Register");
    if (cc != null) { float dz = wallIn + 0.01f - PlaceKit.MeshBounds(cc.gameObject).min.z; cc.position += V(0f, 0f, dz); if (reg != null) reg.position += V(0f, 0f, dz); }
}
// the canopy x 360.5 to 371.9 over the porch z 193.5 to 195.5, posts at (360.7, 193.8) and (371.9, 193.8)
const float cnX0 = 360.5f, cnX1 = 371.9f, cnZ0 = 193.5f, canopyH = 2.55f, postW = 0.12f;
if (canopy != null)
{
    var deck = canopy.Find("Deck"); var edge = canopy.Find("Edge");
    if (deck != null) { deck.position = V((cnX0 + cnX1) * 0.5f, deck.position.y, (cnZ0 + sFaceZ) * 0.5f); deck.localScale = V(cnX1 - cnX0, deck.localScale.y, sFaceZ - cnZ0); }
    if (edge != null) { edge.position = V((cnX0 + cnX1) * 0.5f, edge.position.y, cnZ0); edge.localScale = V(cnX1 - cnX0, edge.localScale.y, edge.localScale.z); }
    var posts = new System.Collections.Generic.List<UnityEngine.Transform>(); foreach (UnityEngine.Transform c in canopy) if (c.name == "Post") posts.Add(c); posts.Sort((a, b) => a.position.x.CompareTo(b.position.x));
    if (posts.Count == 2) { posts[0].position = V(360.7f, posts[0].position.y, 193.8f); posts[1].position = V(371.9f, posts[1].position.y, 193.8f); } else notes.Add("canopy: " + posts.Count + " posts, not 2");
}
// the ice chest: the built freezer against the wall, x 367.9 to 369.7 (doc 2.8), its ICE sign over it; the bench and the bin
if (sFront != null)
{
    var chest = sFront.Find("Ice_Cream_Freezer");
    if (chest != null) { var b = PlaceKit.MeshBounds(chest.gameObject); float dx = 367.9f - b.min.x, dz = sFaceZ - 0.02f - b.max.z; chest.position += V(dx, 0f, dz);
        float signDx = 0f; var iceSign = sFront.Find("IceSign"); if (iceSign != null) { signDx = 368.8f - iceSign.position.x; iceSign.position += V(signDx, 0f, 0f); }
        foreach (UnityEngine.Transform c in sFront) if (c.name == "Label" && UnityEngine.Mathf.Abs(c.position.y - (iceSign != null ? iceSign.position.y : -1f)) < 0.05f) c.position += V(signDx, 0f, 0f); }
    // the closed boxes by the dumpster against it (gap scan: 0.77 m between the near box and the dumpster)
    var dump = sFront.Find("Dumpster");
    if (dump != null) { float dz0 = PlaceKit.MeshBounds(dump.gameObject).min.z; foreach (UnityEngine.Transform c in sFront) if (c.name == "Cardboard_Box_Closed") { var cb = PlaceKit.MeshBounds(c.gameObject); if (cb.max.z < dz0 && dz0 - cb.max.z > 0.1f) c.position += V(0f, 0f, dz0 - 0.05f - cb.max.z); } }
    // the bench 0.54 m off the west canopy post (Marlow 8.22 paper 7: the band), its box with it; the bin against its east end
    var bench = sFront.Find("Bench"); var box = sFront.Find("Cardboard_Box_Open"); const float benchZ0 = 194.4f;
    if (bench != null) { var bb = PlaceKit.MeshBounds(bench.gameObject); float dz = benchZ0 - bb.min.z; bench.position += V(0f, 0f, dz); if (box != null) box.position += V(0f, 0f, dz);
        var bin = sFront.Find("Trash_Can"); if (bin != null) { bb = PlaceKit.MeshBounds(bench.gameObject); var nb = PlaceKit.MeshBounds(bin.gameObject); bin.position += V(bb.max.x + 0.02f - nb.min.x, 0f, bb.center.z - nb.center.z); } }
}

// ================= 5. LOT =================
// stall lines (paint, no colliders): 4 by the office (x 344.3 to 356.0, the west one 3.6 accessible), 4 by the store, nose-in z 184.5 to
// 190; 6 trailhead stalls on the west edge x 343 to 348.5, z 153 to 169.2; R6's bay x 367.5 to 373, z 178.0 to 180.8
const float lineW = 0.1f, stall = 2.7f, accessible = 3.6f, nZ0 = 184.5f, nZ1 = 190f, tX0 = 343f, tX1 = 348.5f, tZ0 = 153f;
var marks = kit.Fresh("LotMarks", fz, V(0f, G, 0f), 0f);
void Line(float x0, float z0, float x1, float z1) => kit.Slab("Line", marks, V((x0 + x1) * 0.5f, paintY, (z0 + z1) * 0.5f), V(UnityEngine.Mathf.Max(lineW, x1 - x0), 0.005f, UnityEngine.Mathf.Max(lineW, z1 - z0)), paint);
{ float x = 344.3f; Line(x, nZ0, x, nZ1); x += accessible; Line(x, nZ0, x, nZ1); for (int i = 0; i < 3; i++) { x += stall; Line(x, nZ0, x, nZ1); } }
{ float x = 360.0f; Line(x, nZ0, x, nZ1); for (int i = 0; i < 4; i++) { x += stall; Line(x, nZ0, x, nZ1); } }
{ float z = tZ0; Line(tX0, z, tX1, z); for (int i = 0; i < 6; i++) { z += stall; Line(tX0, z, tX1, z); } }
Line(367.5f, 178.0f, 373f, 178.0f); Line(367.5f, 180.8f, 373f, 180.8f);
// the walk to the porch and the west door (doc 2.9): z 190 to 193.5 from the office porch's west end to the store, and up to the porch step
var walk = kit.Fresh("LotWalk", fz, V(0f, G, 0f), 0f);
kit.Slab("Walk", walk, V(356.5f, slab * 0.5f, 191.75f), V(31f, slab, 3.5f), apronMat);
kit.Slab("WalkToPorch", walk, V(342.1f, slab * 0.5f, 195.25f), V(2.6f, slab, 3.5f), apronMat);

// ================= 6. VAULT TOILET =================
const float tlX0 = 340.4f, tlX1 = 342.0f, tlZc = 186.0f, tlYaw = 90f, tlWall = 0.1f, tlWallH = 2.4f, tlBenchH = 0.45f, tlBenchD = 0.4f;   // bench 0.4 deep: the 1.6 m shed leaves 1.0 m to the front wall (0.45 left 0.95, in the band)
float tlD = tlX1 - tlX0, tlW = 2f; float tlXc = (tlX0 + tlX1) * 0.5f;
var toilet = kit.Fresh("VaultToilet", fz, V(tlXc, kit.H(tlXc, tlZc), tlZc), tlYaw);
var outhouse = kit.Spawn(PlaceKit.CI + "Building/CITW_Outhouse", toilet);
if (outhouse != null)
{
    PlaceKit.StripColliders(outhouse);
    outhouse.transform.localPosition = UnityEngine.Vector3.zero; outhouse.transform.localRotation = UnityEngine.Quaternion.identity; outhouse.transform.localScale = V(1f, 1f, tlD / 2f);
    var b = PlaceKit.LocalBounds(outhouse, toilet);
    float doorGround = kit.H(toilet.TransformPoint(V(0f, 0f, tlD * 0.5f)).x, toilet.TransformPoint(V(0f, 0f, tlD * 0.5f)).z) - toilet.position.y;
    outhouse.transform.localPosition = V(-b.center.x, doorGround - b.min.y, -b.center.z);
    var leaf = outhouse.transform.Find("CITW_Outhouse Door"); float leafYaw = float.NaN;
    if (leaf != null) for (float a = 0f; a < 360f && float.IsNaN(leafYaw); a += 10f) { leaf.localRotation = UnityEngine.Quaternion.Euler(0f, a, 0f); var lb = PlaceKit.LocalBounds(leaf.gameObject, toilet); if (lb.min.z >= tlD * 0.5f - 0.05f && lb.size.z <= 0.4f && (lb.max.x <= -0.5f || lb.min.x >= 0.5f)) leafYaw = a; }
    if (leaf != null && float.IsNaN(leafYaw)) { leaf.gameObject.SetActive(false); notes.Add("toilet leaf: no turn lies flat outside the front wall, so it is off (an open doorway)"); }
    var shedCol = outhouse.AddComponent<UnityEngine.MeshCollider>(); shedCol.sharedMesh = outhouse.GetComponent<UnityEngine.MeshFilter>().sharedMesh; shedCol.convex = false;
    if (leaf != null && leaf.gameObject.activeSelf) PlaceKit.FitCollider(leaf.gameObject);
    UnityEngine.Physics.SyncTransforms();
    var down = new UnityEngine.Ray(toilet.TransformPoint(V(0f, doorGround + 2f, 0.2f)), UnityEngine.Vector3.down);
    if (shedCol.Raycast(down, out var fh, 6f)) outhouse.transform.position += V(0f, toilet.position.y + doorGround - fh.point.y, 0f); else notes.Add("toilet: no shed floor under its centre");
    UnityEngine.Physics.SyncTransforms();
    var cols = new UnityEngine.GameObject("Colliders").transform; cols.SetParent(toilet, false);
    void Wall(string n, UnityEngine.Vector3 c, UnityEngine.Vector3 s) { var g = new UnityEngine.GameObject(n); g.transform.SetParent(cols, false); g.transform.localPosition = c; g.AddComponent<UnityEngine.BoxCollider>().size = s; }
    float fy = doorGround;
    Wall("Floor", V(0f, fy - 0.05f, 0f), V(tlW, 0.1f, tlD));
    Wall("Back", V(0f, fy + tlWallH * 0.5f, -tlD * 0.5f + tlWall * 0.5f), V(tlW, tlWallH, tlWall));
    Wall("SideL", V(-tlW * 0.5f + tlWall * 0.5f, fy + tlWallH * 0.5f, 0f), V(tlWall, tlWallH, tlD));
    Wall("SideR", V(tlW * 0.5f - tlWall * 0.5f, fy + tlWallH * 0.5f, 0f), V(tlWall, tlWallH, tlD));
    foreach (var sx in new[] { -1f, 1f }) Wall("Front" + (sx < 0f ? "L" : "R"), V(sx * (tlW * 0.5f - 0.25f), fy + tlWallH * 0.5f, tlD * 0.5f - tlWall * 0.5f), V(0.5f, tlWallH, tlWall));
    Wall("Bench", V(0f, fy + tlBenchH * 0.5f, -tlD * 0.5f + tlWall + tlBenchD * 0.5f), V(tlW - 2f * tlWall, tlBenchH, tlBenchD));
}

// ================= 7. CAMPGROUND: the spur, the ring road and the pitches clear =================
// keep-out (doc 2.3): the spur strips (half width spurHalf), the ring (|r - ringR| <= ringHalf), each pitch with its car nose-in (radial
// pitchIn to pitchOut, half width pitchHalfW), plus clearM m. A forest piece's footprint is its colliders' bounds, or the middle of its
// mesh bounds (coreShare) when it has none.
const float clearM = 1f, coreShare = 0.5f, searchStep = 0.5f, searchMax = 16f, overlapPad = 0.3f, sampleStep = 0.25f, warpClear = 1.5f;
const float cgX0 = 349f, cgX1 = 394.5f, cgZ0 = 216.5f, cgZ1 = 289f, stripX0 = 383f, stripX1 = 395.5f, stripZ0 = 176f;   // where moved pieces may go: the campground, and the strip between the brush band and the fence along the spur
bool InPlace(UnityEngine.Rect r) => (r.xMin >= cgX0 && r.xMax <= cgX1 && r.yMin >= cgZ0 && r.yMax <= cgZ1) || (r.xMin >= stripX0 && r.xMax <= stripX1 && r.yMin >= stripZ0 && r.yMax <= cgZ0);
var spurPts = new[] { V2(385f, 172.5f), V2(385f, 186f), V2(390f, 196f), V2(390f, 242f), V2(387.5f, 252f) };
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = ab.sqrMagnitude > 0f ? UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude) : 0f; return (p - (a + ab * t)).magnitude; }
var pitchAngs = new System.Collections.Generic.List<float>(); foreach (var pl in pitchList) pitchAngs.Add(pl.ang * UnityEngine.Mathf.Deg2Rad);
bool InKeep(UnityEngine.Vector2 p, float m)
{
    for (int i = 1; i < spurPts.Length; i++) if (SegD(p, spurPts[i - 1], spurPts[i]) <= spurHalf + m) return true;
    var c = V2(ringCX, ringCZ); float r = (p - c).magnitude; if (UnityEngine.Mathf.Abs(r - ringR) <= ringHalf + m) return true;
    foreach (var a in pitchAngs) { var d = V2(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)); var q = p - c; float u = UnityEngine.Vector2.Dot(q, d), v = d.x * q.y - d.y * q.x; if (u >= pitchIn - m && u <= pitchOut + m && UnityEngine.Mathf.Abs(v) <= pitchHalfW + m) return true; }
    return false;
}
bool RectInKeep(UnityEngine.Rect r, float m) { for (float x = r.xMin; x <= r.xMax + 1e-3f; x += sampleStep) for (float z = r.yMin; z <= r.yMax + 1e-3f; z += sampleStep) if (InKeep(V2(UnityEngine.Mathf.Min(x, r.xMax), UnityEngine.Mathf.Min(z, r.yMax)), m)) return true; return false; }
var forest = kit.Root("Forest"); int moved = 0, removedCg = 0; var cgList = new System.Collections.Generic.List<string>();
var warpsRoot = kit.Root("DevWarps").transform;
if (forest != null)
{
    UnityEngine.Physics.SyncTransforms();
    var region = new UnityEngine.Bounds(V((cgX0 + cgX1) * 0.5f, 0f, (spurPts[0].y + cgZ1) * 0.5f), V(cgX1 - cgX0 + 4f, 400f, cgZ1 - spurPts[0].y + 4f));
    var cands = new System.Collections.Generic.List<UnityEngine.GameObject>(); var seen = new System.Collections.Generic.HashSet<UnityEngine.GameObject>();
    foreach (var r in forest.GetComponentsInChildren<UnityEngine.Renderer>())
    {
        if (!r.bounds.Intersects(region)) continue; var root = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(r.gameObject); var go = root != null ? root : r.gameObject;
        if (seen.Add(go)) cands.Add(go);
    }
    cands.Sort((a, b) => string.CompareOrdinal(WalkIns.PathOf(a.transform) + a.transform.position.ToString("F2"), WalkIns.PathOf(b.transform) + b.transform.position.ToString("F2")));
    UnityEngine.Rect Foot(UnityEngine.GameObject g, out bool hasCol)
    {
        bool any = false; var b = new UnityEngine.Bounds(); foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) { if (!c.enabled || c.isTrigger) continue; if (!any) { b = c.bounds; any = true; } else b.Encapsulate(c.bounds); }
        hasCol = any; if (!any) { b = PlaceKit.MeshBounds(g); b.extents = V(b.extents.x * coreShare, b.extents.y, b.extents.z * coreShare); }
        return new UnityEngine.Rect(b.min.x, b.min.z, b.size.x, b.size.z);
    }
    foreach (var g in cands)
    {
        if (g == null) continue; var foot = Foot(g, out bool hasCol); if (foot.width > 15f || foot.height > 15f) continue;   // a group, not a piece
        if (!RectInKeep(foot, hasCol ? clearM : 0f)) continue;
        var ownCols = new System.Collections.Generic.HashSet<UnityEngine.Collider>(g.GetComponentsInChildren<UnityEngine.Collider>());
        UnityEngine.Vector2? found = null;
        for (float d = searchStep; d <= searchMax && found == null; d += searchStep)
            for (int k = 0; k < 32 && found == null; k++)
            {
                float a = k * UnityEngine.Mathf.PI * 2f / 32f; var off = V2(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * d; var f2 = new UnityEngine.Rect(foot.x + off.x, foot.y + off.y, foot.width, foot.height);
                if (!InPlace(f2)) continue;
                if (RectInKeep(f2, clearM)) continue;
                bool nearWarp = false; foreach (UnityEngine.Transform w in warpsRoot) if (new UnityEngine.Rect(f2.x - warpClear, f2.y - warpClear, f2.width + 2f * warpClear, f2.height + 2f * warpClear).Contains(V2(w.position.x, w.position.z))) nearWarp = true;
                if (nearWarp) continue;
                float gy = kit.H(f2.center.x, f2.center.y); bool blocked = false;
                foreach (var col in UnityEngine.Physics.OverlapBox(V(f2.center.x, gy + 1.1f, f2.center.y), V(f2.width * 0.5f + overlapPad, 0.9f, f2.height * 0.5f + overlapPad), UnityEngine.Quaternion.identity, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
                    if (!(col is UnityEngine.TerrainCollider) && !ownCols.Contains(col)) { blocked = true; break; }
                if (!blocked) found = off;
            }
        var was = g.transform.position; string label = WalkIns.PathOf(g.transform) + " (" + F(was.x) + ", " + F(was.z) + ")";
        if (found == null) { cgList.Add("removed " + label + ": no free spot within " + F(searchMax) + " m"); UnityEngine.Object.DestroyImmediate(g); removedCg++; continue; }
        var to = V(was.x + found.Value.x, 0f, was.z + found.Value.y); g.transform.position = V(to.x, was.y + kit.H(to.x, to.z) - kit.H(was.x, was.z), to.z);
        UnityEngine.Physics.SyncTransforms(); moved++; cgList.Add(label + " moved " + F(found.Value.magnitude) + " m");
    }
}

// ================= 8. WARPS AND THE INTERACTOR MASK =================
var wb = warpsRoot.Find("Gate_Booth"); if (wb != null) { wb.position = V(392f, G + 0.2f, 162.5f); wb.rotation = UnityEngine.Quaternion.Euler(0f, 0f, 0f); } else notes.Add("no DevWarps/Gate_Booth");
int maskWas = 0, maskNow = 0;
{
    var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Prefabs/Player.prefab"); var pi = pf != null ? pf.GetComponentInChildren<PlayerInteractor>(true) : null;
    if (pi != null) { var so = new UnityEditor.SerializedObject(pi); var mp = so.FindProperty("mask"); maskWas = mp.intValue; mp.intValue = ~(1 << 2); so.ApplyModifiedPropertiesWithoutUndo(); maskNow = mp.intValue; if (maskWas != maskNow) { UnityEditor.EditorUtility.SetDirty(pf); UnityEditor.AssetDatabase.SaveAssets(); } }
    else notes.Add("no PlayerInteractor in Assets/Prefabs/Player.prefab");
}

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | booth x " + F(bX0) + " to " + F(bX1) + ", z " + F(bZ0) + " to " + F(bZ1) + ", cover pieces cleared " + coverGone + ", detail cells " + detailGone + " | barrier at (" + F(postX) + ", " + F(postZ) + ")" +
    " | IW1 z " + (iw1 != null ? F(iw1.GetComponent<UnityEngine.BoxCollider>().bounds.min.z) + " to " + F(iw1.GetComponent<UnityEngine.BoxCollider>().bounds.max.z) : "-") + " | IW3 " + (iw3 != null ? "SpurWall, off" : "MISSING") +
    " | stops " + stops.childCount + " | office west door " + (westDoor != null ? "open in, no prompt" : "MISSING") + " | snapped top max x " + F(snappedMaxX) + " | reflector pieces off the drive mouth " + reflGone +
    " | campground pieces moved " + moved + ", removed " + removedCg + (cgList.Count > 0 ? " (" + string.Join("; ", cgList) + ")" : "") +
    " | interactor mask " + maskWas + " -> " + maskNow + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
