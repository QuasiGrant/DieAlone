// Main3 task 8.17, front zone places: the store and the office (Valley.md rev 11 section 6, M1 and M3; Style.md 5 and 10; Check1_Story
// and Check1_Mechanics rows 15 and 16; Check3_Quality 4). Run after 8.16 in Main3, edit mode (the runner runs it before the day-one
// start and the sightlines). Replaces 8.6's gray Store and Office shells and 8.9f's glow quads for the old office; rerunnable.
// STORE (M1): (366, 200), 12 x 9 m outside (x 360 to 372, z 195.5 to 204.5), owned brick walls (Celestia modular kit, their own
//   colliders), glass door on the lot side that stays shut (the store's game loads its own level later; DECISIONS 2026-09-30), display
//   windows onto the lot with shelves, counter and coolers behind them, flat roof with a fascia, canopy over the door, a lit sign, a lit
//   ICE sign over the ice chest, a propane cage, a porch light, bench, bin and dumpster.
// OFFICE (M3): (350, 200), 12 x 8 m (x 344 to 356, z 196 to 204), owned plank walls (Cabin In The Woods) with fitted colliders, a gable
//   roof with an aerial, windows on every face, the west door (Door system) onto a porch with the RANGER STATION sign, opening into the
//   front room (counter, radio, filing, stove, R5's spot); one door (Door system) into the back room at the east end (cot, desk).
// Night: warm window glow (LookVisibility, Night) and interior practicals; by day the lamps dim (interiors darker than outside).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
var fz = kit.Root("FrontZone"); if (fz == null) return "no FrontZone";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
const float G = 3f;   // the front zone's flat ground (8.1)
// 8.6's gray shells and 8.9f's glow for the old office go; the resident capsule at the car becomes a bare marker
PlaceKit.Remove(fz.transform.Find("Store")); PlaceKit.Remove(fz.transform.Find("Office"));
var slice = kit.Root("SliceLook"); if (slice != null) foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(slice.transform)))
    if (t.name == "OfficeWindowGlow" || t.name == "OfficePorchBulb" || t.name == "OfficeNightGlow") PlaceKit.Remove(t);
PlaceKit.MarkerOnly(fz.transform.Find("Resident_Car/Resident_Office_Spot"));

// ---------------- materials ----------------
var glass = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Glass.mat");
var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
var roofChar = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_RoofChar.mat");
var winGlow = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_CabWindowGlow.mat");
if (glass == null || steel == null || roofChar == null || winGlow == null) return "Slice materials missing (run 8.9d and 8.9f)";
const string concrete = "Assets/Materials/Concrete034_1.0x1.0.mat", planks = "Assets/Materials/Planks023A_1.0x1.0.mat";
UnityEngine.Material Fascia(float len) => kit.Tinted("Places_Fascia_" + UnityEngine.Mathf.CeilToInt(len), concrete, Hex("#4E4843"), new UnityEngine.Vector2(UnityEngine.Mathf.Ceil(len), 1f));
var apron = kit.Tinted("Places_Apron", concrete, Hex("#77726A"), new UnityEngine.Vector2(12f, 2f));
var boardWood = kit.Tinted("Places_SignBoard", planks, Hex("#3E4A3A"), new UnityEngine.Vector2(2f, 1f));
var signGlow = kit.Glow("Places_SignGlow", kit.Look.practicalColor, kit.Look.cabWindowGlowIntensity);
var darkText = Hex("#2A1E14"); var paleText = Hex("#EDE3CF");
void Glaze(UnityEngine.GameObject g) { if (g != null) foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) r.sharedMaterial = glass; }
var nightGlow = new System.Collections.Generic.List<UnityEngine.GameObject>();
UnityEngine.GameObject GlowPane(string name, UnityEngine.Transform parent, UnityEngine.Vector3 lp, UnityEngine.Vector2 size, float yaw)
{
    var q = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Quad); q.name = name; q.transform.SetParent(parent, false);
    q.transform.localPosition = lp; q.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); q.transform.localScale = V(size.x, size.y, 1f);
    UnityEngine.Object.DestroyImmediate(q.GetComponent<UnityEngine.Collider>()); var r = q.GetComponent<UnityEngine.Renderer>(); r.sharedMaterial = winGlow; r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    nightGlow.Add(q); return q;
}

// ================= STORE =================
const float sW = 12f, sD = 9f, sWallH = 2.7f, sPiece = 2f, fasciaH = 0.55f, canopyD = 2.4f, canopyH = 2.55f;
const string WP = PlaceKit.CE + "Wall_Parts/";
const string wStraight = WP + "Wall_Small_Straight_BrickIntExt_BorderDarkPlaster", wWindow = WP + "Wall_Small_Window_Large_BrickIntExt_BorderDarkPlaster";
const string wDoor = WP + "Wall_Small_Door_Small_BrickIntExt_BorderDarkPlaster", wSmallWin = WP + "Wall_Small_Window_Small_BrickIntExt_BorderDarkPlaster";
// Celestia wall pieces: pivot at one end, 2 m along local +x, 0.2 m body on local -z, 2.7 m tall. Window opening x 0.4 to 1.6, sill 0.95,
// head 2.25; door opening x 0.6 to 1.4, head 2.1 (measured by rays, 8.17).
const float winX0 = 0.4f, winX1 = 1.6f, winSill = 0.95f, winHead = 2.25f, doorW = 0.8f, doorH = 2.1f;
var store = kit.Group("Store", fz.transform, V(360f, G, 195.5f), 0f);
var shell = kit.Group("Shell", store, store.position, 0f);
// each run: (piece names from the pivot end, side). South faces the lot (door at piece 2, local x 4 to 6).
string[] south = { wWindow, wWindow, wDoor, wWindow, wWindow, wStraight }, north = { wStraight, wStraight, wStraight, wStraight, wStraight, wStraight };
string[] west = { wStraight, wSmallWin, wSmallWin, wStraight }, east = { wStraight, wStraight, wStraight, wStraight };
float sideScale = sD / (west.Length * sPiece);
var windowCentres = new System.Collections.Generic.List<(UnityEngine.Vector3 p, float yaw, float w)>();
for (int i = 0; i < south.Length; i++) { kit.On(south[i], shell, V((i + 1) * sPiece, 0f, 0f), 180f); if (south[i] == wWindow) windowCentres.Add((V(i * sPiece + 1f, 0f, 0.1f), 0f, winX1 - winX0)); }
for (int i = 0; i < north.Length; i++) kit.On(north[i], shell, V(i * sPiece, 0f, sD), 0f);
for (int i = 0; i < west.Length; i++) { var g = kit.On(west[i], shell, V(0f, 0f, i * sPiece * sideScale), -90f); if (g != null) g.transform.localScale = V(sideScale, 1f, 1f); }
for (int i = 0; i < east.Length; i++) { var g = kit.On(east[i], shell, V(sW, 0f, (i + 1) * sPiece * sideScale), 90f); if (g != null) g.transform.localScale = V(sideScale, 1f, 1f); }
// glass in the display windows; the glass door, shut
foreach (var w in windowCentres) Glaze(kit.Fill(PlaceKit.CE + "Building_Parts/Window_Glass", shell, V(w.p.x, winSill, w.p.z), V(w.w, winHead - winSill, 0.01f)));
var sDoorX = 2 * sPiece + 1f;
kit.Fill(PlaceKit.CE + "Building_Parts/Door_GlassPanel", shell, V(sDoorX, 0f, 0.1f), V(doorW, doorH, 0.06f), 90f, true);
// roof: flat tiles, a fascia round the top, a concrete apron along the front (base band)
var roof = kit.Group("Roof", store, store.position + V(0f, sWallH, 0f), 0f);
for (float x = 0f; x < sW - 0.01f; x += 2f) for (float z = 0f; z < sD - 0.01f; z += sD / 5f)
    kit.Fill(PlaceKit.CE + "Roof/Roof_Tile_Flat", roof, V(x + 1f, 0f, z + sD / 10f), V(2f, 0.13f, sD / 5f));
foreach (var f in new[] { (V(sW * 0.5f, 0f, -0.1f), V(sW + 0.4f, fasciaH, 0.2f)), (V(sW * 0.5f, 0f, sD + 0.1f), V(sW + 0.4f, fasciaH, 0.2f)), (V(-0.1f, 0f, sD * 0.5f), V(0.2f, fasciaH, sD)), (V(sW + 0.1f, 0f, sD * 0.5f), V(0.2f, fasciaH, sD)) })
    kit.Slab("Fascia", roof, f.Item1 + V(0f, fasciaH * 0.5f - 0.05f, 0f), f.Item2, Fascia(UnityEngine.Mathf.Max(f.Item2.x, f.Item2.z)));
kit.Slab("Apron", store, V(sW * 0.5f, 0.04f, -0.9f), V(sW, 0.08f, 1.8f), apron);
// canopy over the door and the two windows beside it, on two steel posts
var canopy = kit.Group("Canopy", store, store.TransformPoint(V(sDoorX, 0f, 0f)), 0f);
kit.Slab("Deck", canopy, V(0f, canopyH, -canopyD * 0.5f), V(6.4f, 0.14f, canopyD), steel);
kit.Slab("Edge", canopy, V(0f, canopyH - 0.12f, -canopyD), V(6.4f, 0.3f, 0.06f), Fascia(6.4f));
foreach (var px in new[] { -2.9f, 2.9f }) kit.Slab("Post", canopy, V(px, canopyH * 0.5f, -canopyD + 0.15f), V(0.12f, canopyH, 0.12f), steel, default, true);
// the lit sign on the fascia over the canopy, and the porch light under the canopy
var sign = kit.Slab("SignBox", store, V(sDoorX, sWallH + fasciaH * 0.5f - 0.05f, -0.28f), V(4.4f, 0.5f, 0.12f), signGlow);
kit.Label(sign.transform, "GENERAL STORE", darkText, 40);
kit.On(PlaceKit.CE + "Decoration_Lamps/Cage_Light", canopy, V(0f, canopyH - 0.35f, -canopyD * 0.5f), 0f);
kit.Practical("PorchLight", canopy, V(0f, canopyH - 0.45f, -canopyD * 0.5f), 7f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Off);
// out front: the ice chest under a lit ICE sign, the propane cage at the east end, a bench, a bin; the dumpster round the east side
var front = kit.Group("Front", store, store.position, 0f);
kit.On(PlaceKit.CE + "Marketplace_Assets/Ice_Cream_Freezer", front, V(8.4f, 0.08f, -0.75f), 180f, 1f, false, null, true);
var ice = kit.Slab("IceSign", front, V(8.4f, 2.0f, -0.08f), V(1.2f, 0.4f, 0.08f), signGlow);
kit.Label(ice.transform, "ICE", darkText, 40);
var cage = kit.Group("PropaneCage", front, store.TransformPoint(V(10.9f, 0.08f, -0.7f)), 0f);
foreach (var side in new[] { (V(0f, 0f, -0.5f), V(1.6f, 1.5f, 0.04f)), (V(-0.8f, 0f, 0f), V(0.04f, 1.5f, 1.0f)), (V(0.8f, 0f, 0f), V(0.04f, 1.5f, 1.0f)) })
    kit.Fill(PlaceKit.CE + "Building_Parts/Chain_Link_Fence", cage, side.Item1, side.Item2);
kit.Slab("Lid", cage, V(0f, 1.52f, 0f), V(1.64f, 0.05f, 1.04f), steel);
for (int i = 0; i < 3; i++) kit.On(PlaceKit.SP + "GasTank", cage, V(-0.5f + i * 0.5f, 0f, 0.05f), i * 40f, 1f, false, null, true);
kit.Blocker("CageBlock", cage, V(0f, 0.75f, 0f), V(1.64f, 1.5f, 1.04f));
kit.On(PlaceKit.CE + "Decoration_Out/Bench", front, V(1.6f, 0.08f, -0.7f), 180f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Out/Trash_Can", front, V(3.5f, 0.08f, -0.45f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Out/Cardboard_Box_Open", front, V(1.3f, 0.52f, -0.7f), 20f, 1f, false, null, true);   // left on the bench mid-restock
kit.On(PlaceKit.CE + "Decoration_Out/Dumpster", front, V(sW + 0.9f, 0f, 6.8f), 90f, 1.2f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Out/Cardboard_Box_Closed", front, V(sW + 0.6f, 0f, 4.9f), 30f, 1.2f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Out/Cardboard_Box_Closed", front, V(sW + 0.7f, 0f, 5.4f), 75f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Out/Drum_Grey", front, V(sW + 0.6f, 0f, 2.2f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Building_Parts/Air_Vent", front, V(sW + 0.02f, 2.2f, 7.5f), 90f);
// inside, seen through the windows: tile floor, three shelf rows, coolers on the back wall, the counter and register by the door
var inside = kit.Group("Inside", store, store.position, 0f);
for (float x = 0f; x < sW - 0.01f; x += 2f) for (float z = 0f; z < sD - 0.01f; z += 2.25f)
    kit.Fill(PlaceKit.CE + "Building_Parts/FloorTile_LightConcrete", inside, V(x + 1f, 0.02f, z + 1.125f), V(2f, 0.005f, 2.25f));
foreach (var rx in new[] { 1.6f, 3.6f, 9.4f })
    for (int k = 0; k < 4; k++) kit.On(PlaceKit.CE + "Marketplace_Assets/Retail_Shelf_Long", inside, V(rx, 0.02f, 3.0f + k * 1.0f), 90f, 1f, false, null, true);
for (int k = 0; k < 8; k++) kit.On(PlaceKit.CE + "Marketplace_Assets/Retail_Shelf_Refrigerated_Module", inside, V(2.5f + k * 1.0f, 0.02f, sD - 0.6f), 180f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Marketplace_Assets/Checkout_Counter", inside, V(7.2f, 0.02f, 1.6f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Marketplace_Assets/Cash_Register", inside, V(7.6f, 0.96f, 1.7f), 180f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Marketplace_Assets/Frozen_Food_Freezer", inside, V(6.5f, 0.02f, 5.2f), 90f, 1f, false, null, true);
foreach (var lx in new[] { 3f, 6f, 9f }) kit.On(PlaceKit.CE + "Decoration_Lamps/Fluorescent_Light", inside, V(lx, sWallH - 0.15f, 4.5f), 90f);
kit.Practical("StoreLight", inside, V(6f, sWallH - 0.4f, 4f), 9f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
foreach (var w in windowCentres) GlowPane("WindowGlow", inside, V(w.p.x, (winSill + winHead) * 0.5f, 0.45f), new UnityEngine.Vector2(w.w, winHead - winSill), 0f);

// ================= OFFICE =================
const float oW = 12f, oD = 8f, oWallH = 3f, oMod = 2f, oT = 0.26f, oBack = 8f, roofRise = 1.6f, eave = 0.4f, floorTop = 0.05f;
// CITW plank modules: 2 x 3 m, pivot bottom centre, 0.26 thick; doorway and window openings x -0.6 to 0.6, door head 2.15, window 0.95 to 2.15
const float openHalf = 0.6f, openHead = 2.15f;
var office = kit.Group("Office", fz.transform, V(344f, G, 196f), 0f);
var oShell = kit.Group("Shell", office, office.position, 0f);
string CW(string n) => PlaceKit.CI + "Building/CITW_Plank_" + n;
// one wall module: its mesh and a fitted collider (a doorway keeps its opening clear)
void Module(string kind, float lx, float lz, float yaw)
{
    var g = kit.On(CW(kind), oShell, V(lx, floorTop, lz), yaw, 1f, false, null, true); if (g == null) return;
    var holder = kit.Group("Collider_" + kind, oShell, oShell.TransformPoint(V(lx, 0f, lz)), yaw);
    if (kind == "Doorway")
    {
        foreach (var sx in new[] { -1f, 1f }) kit.Blocker("Jamb", holder, V(sx * (oMod * 0.5f + openHalf) * 0.5f, oWallH * 0.5f, 0f), V(oMod * 0.5f - openHalf, oWallH, oT));
        kit.Blocker("Lintel", holder, V(0f, (openHead + oWallH) * 0.5f + floorTop, 0f), V(openHalf * 2f, oWallH - openHead, oT));
    }
    else kit.Blocker("Wall", holder, V(0f, oWallH * 0.5f, 0f), V(oMod, oWallH, oT));
}
string[] oS = { "Window_Wall", "Wall", "Window_Wall", "Wall", "Wall", "Window_Wall" }, oN = { "Wall", "Window_Wall", "Wall", "Window_Wall", "Wall", "Window_Wall" };
string[] oWst = { "Wall", "Doorway", "Window_Wall", "Wall" }, oE = { "Wall", "Window_Wall", "Window_Wall", "Wall" }, oPart = { "Wall", "Wall", "Doorway", "Wall" };
var oWins = new System.Collections.Generic.List<(UnityEngine.Vector3 p, float yaw)>();
for (int i = 0; i < 6; i++) { float x = oMod * 0.5f + i * oMod; Module(oS[i], x, oT * 0.5f, 0f); Module(oN[i], x, oD - oT * 0.5f, 180f); if (oS[i] == "Window_Wall") oWins.Add((V(x, 0f, oT + 0.02f), 0f)); if (oN[i] == "Window_Wall") oWins.Add((V(x, 0f, oD - oT - 0.02f), 180f)); }
for (int i = 0; i < 4; i++) { float z = oMod * 0.5f + i * oMod; Module(oWst[i], oT * 0.5f, z, 90f); Module(oE[i], oW - oT * 0.5f, z, -90f); Module(oPart[i], oBack, z, 90f);
    if (oWst[i] == "Window_Wall") oWins.Add((V(oT + 0.02f, 0f, z), 90f)); if (oE[i] == "Window_Wall") oWins.Add((V(oW - oT - 0.02f, 0f, z), -90f)); }
foreach (var cx in new[] { 0f, oW }) foreach (var cz in new[] { 0f, oD }) kit.On(PlaceKit.CI + "Building/CITW_Wood_Pillar", oShell, V(cx, floorTop, cz), 0f, 1f, false, null, true);
foreach (var w in oWins) { bool alongX = w.yaw == 0f || w.yaw == 180f; var gl = kit.Fill(PlaceKit.CI + "Building/CITW_Window_Glass", oShell, w.p + V(0f, 0.95f + floorTop, 0f), alongX ? V(openHalf * 2f, openHead - 0.95f, 0.01f) : V(0.01f, openHead - 0.95f, openHalf * 2f), alongX ? 0f : 90f); Glaze(gl); }
for (float x = 0f; x < oW - 0.01f; x += oMod) for (float z = 0f; z < oD - 0.01f; z += oMod) kit.Fill(PlaceKit.CI + "Building/CITW_Floor", oShell, V(x + 1f, floorTop - 0.1f, z + 1f), V(oMod, 0.1f, oMod));
// the two doors: west, onto the porch, into the front room; and between the front and back rooms
kit.Door(office, V(oT * 0.5f, floorTop, oMod * 1.5f - openHalf), -90f, V(openHalf * 2f, openHead, 0.06f), PlaceKit.CI + "Building/CITW_Door_1");
kit.Door(office, V(oBack, floorTop, oMod * 2.5f - openHalf), -90f, V(openHalf * 2f, openHead, 0.06f), PlaceKit.CI + "Building/CITW_Door_2");
// gable roof, ridge east to west at z 4, eaves eave m out; the gable ends are the pack's plank triangles
var oRoof = kit.Group("Roof", office, office.TransformPoint(V(0f, floorTop + oWallH, 0f)), 0f);
float run = oD * 0.5f + eave, slopeLen = UnityEngine.Mathf.Sqrt(run * run + roofRise * roofRise), pitch = UnityEngine.Mathf.Atan2(roofRise, run) * UnityEngine.Mathf.Rad2Deg;
foreach (var side in new[] { -1f, 1f })
    kit.Slab("RoofSlope", oRoof, V(oW * 0.5f, roofRise * 0.5f - 0.1f, oD * 0.5f + side * run * 0.5f), V(oW + eave * 2f, 0.14f, slopeLen), roofChar, V(side * pitch, 0f, 0f));
foreach (var gx in new[] { oT * 0.5f, oW - oT * 0.5f }) { var tri = kit.On(CW("Wall_Triangle"), oRoof, V(gx, 0f, oD * 0.5f), 90f, 1f, false, null, true); if (tri != null) tri.transform.localScale = V(oD / oMod, roofRise / 0.75f, 1f); }
kit.On(PlaceKit.CE + "Decoration_Home/Antenna", oRoof, V(oW - 2f, roofRise - 0.1f, oD * 0.5f), 0f, 2.5f);
// the porch on the west: a plank deck flush with the floor, a front step, two posts under a lean-to roof, the sign, a bench
var porch = kit.Group("Porch", office, office.position, 0f);
const float porchD = 2.2f, porchZ0 = 1f, porchZ1 = 5f, porchRoofH = 2.7f;
for (float z = porchZ0; z < porchZ1 - 0.01f; z += 2f) kit.Fill(PlaceKit.CI + "Building/CITW_Floor", porch, V(-porchD * 0.5f, floorTop - 0.1f, z + 1f), V(porchD, 0.1f, 2f));
kit.Fill(PlaceKit.CI + "Building/CITW_Floor", porch, V(-porchD - 0.3f, 0f, (porchZ0 + porchZ1) * 0.5f), V(0.6f, 0.03f, 2.4f));   // the step
foreach (var pz in new[] { porchZ0 + 0.2f, porchZ1 - 0.2f }) kit.On(PlaceKit.CI + "Building/CITW_Wood_Pillar", porch, V(-porchD + 0.2f, floorTop, pz), 0f, porchRoofH / 3f, true, null, true);
kit.Slab("PorchRoof", porch, V(-porchD * 0.5f, porchRoofH + 0.15f, (porchZ0 + porchZ1) * 0.5f), V(porchD + 0.5f, 0.12f, porchZ1 - porchZ0 + 0.4f), roofChar, V(0f, 0f, -12f));
var oSign = kit.Slab("SignBoard", porch, V(-porchD - 0.05f, porchRoofH - 0.3f, (porchZ0 + porchZ1) * 0.5f), V(2.8f, 0.5f, 0.06f), boardWood, V(0f, 90f, 0f));
kit.Label(oSign.transform, "RANGER STATION", paleText, 40);
kit.On(PlaceKit.CE + "Decoration_Out/Bench", porch, V(-0.7f, floorTop, porchZ1 - 0.9f), 90f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Lamps/Wall_Cage_Light", porch, V(-0.02f, 2.35f, oMod * 1.5f + 0.9f), -90f);
kit.Practical("PorchLight", porch, V(-0.5f, 2.3f, oMod * 1.5f + 0.9f), 6f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Off);
kit.On(PlaceKit.CE + "Decoration_Out/Trash_Can", porch, V(-porchD - 0.6f, 0f, porchZ0 - 0.4f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Barrel_2", porch, V(oW + 0.6f, 0f, 7.2f), 0f, 1f, true, null, true);   // rain barrel at the back corner
kit.On(PlaceKit.CI + "Props/CITW_Firewood_1", porch, V(3f, 0f, oD + 0.5f), 90f, 1f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Firewood_2", porch, V(3.6f, 0f, oD + 0.5f), 80f, 1f, false, null, true);
// front room: counter facing the door with the radio, filing, a stove with a kettle, a wall map, R5's spot behind the counter
var fr = kit.Group("FrontRoom", office, office.position, 0f);
kit.On(PlaceKit.CE + "Furniture/Drawer_Cabinet", fr, V(3.6f, floorTop, 5.4f), 90f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Furniture/Drawer_Cabinet", fr, V(3.6f, floorTop, 6.7f), 90f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Radio", fr, V(3.6f, floorTop + 1.13f, 5.3f), -90f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Kitchen/Mug", fr, V(3.5f, floorTop + 1.13f, 6.2f), 30f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Notepad", fr, V(3.7f, floorTop + 1.13f, 6.8f), 10f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Lamps/Table_Lamp", fr, V(3.6f, floorTop + 1.13f, 7.2f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Furniture/Chair", fr, V(5.0f, floorTop, 6.0f), -90f, 1f, true, null, true);
foreach (var fx in new[] { 5.6f, 6.3f }) kit.On(PlaceKit.CE + "Furniture/Filing_Cabinet", fr, V(fx, floorTop, oD - 0.6f), 180f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Potbelly_Stove", fr, V(6.8f, floorTop, 1.0f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Potbelly_Stove_Pipe_Long", fr, V(6.8f, floorTop + 1.1f, 1.0f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Kettle", fr, V(6.8f, floorTop + 1.1f, 1.0f), 200f, 1f, false, null, true);   // left on the stove
kit.On(PlaceKit.CE + "Decoration_Home/Tear-Off_Calendar", fr, V(2.4f, 1.6f, oD - oT - 0.02f), 180f);
kit.On(PlaceKit.CE + "Furniture/Wall_Shelf", fr, V(7.7f, 1.6f, 3.0f), -90f, 1f, false, null, true);
kit.Marker("Resident_Office_Spot", fr, V(4.8f, floorTop, 5.2f), -90f);   // R5 behind the counter (Check1_Mechanics row 6)
kit.Practical("FrontRoomLamp", fr, V(3.6f, 1.9f, 7.0f), 6f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
kit.Practical("StoveGlow", fr, V(6.8f, 0.6f, 1.5f), 3f, PracticalLight.Kind.Stove, PracticalLight.ByDay.Dimmed);
// back room: cot along the east wall, a desk under the north window with a lamp, a trunk
var br = kit.Group("BackRoom", office, office.position, 0f);
kit.On(PlaceKit.CE + "Furniture/Single_Bed_Metal", br, V(oW - 0.8f, floorTop, 2.0f), 90f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Furniture/Table", br, V(10.0f, floorTop, oD - 0.8f), 0f, 0.7f, true, null, true);
kit.On(PlaceKit.CE + "Furniture/Chair", br, V(10.0f, floorTop, oD - 1.6f), 0f, 1f, true, null, true);
kit.On(PlaceKit.CE + "Decoration_Lamps/Table_Lamp", br, V(10.6f, floorTop + 0.68f, oD - 0.7f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CE + "Decoration_Home/Ashtray", br, V(9.6f, floorTop + 0.68f, oD - 0.8f), 0f, 1f, false, null, true);
kit.On(PlaceKit.CI + "Props/CITW_Trunk_1", br, V(8.9f, floorTop, 1.0f), 90f, 1f, true, null, true);
kit.Practical("BackRoomLamp", br, V(10.6f, 1.4f, oD - 0.8f), 5f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
foreach (var w in oWins) GlowPane("WindowGlow", office, w.p + V(0f, (0.95f + openHead) * 0.5f + floorTop, 0f) + UnityEngine.Quaternion.Euler(0f, w.yaw, 0f) * V(0f, 0f, 0.2f), new UnityEngine.Vector2(openHalf * 2f, openHead - 0.95f), w.yaw);
PlaceKit.Remove(fz.transform.Find("PlacesNightGlow")); kit.ShowIn(fz.transform, "PlacesNightGlow", LookVisibility.Show.Night, nightGlow);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " front places: store " + store.GetComponentsInChildren<UnityEngine.Renderer>().Length + " renderers, office " + office.GetComponentsInChildren<UnityEngine.Renderer>().Length + " renderers, window glows " + nightGlow.Count + " | " + kit.Report();
