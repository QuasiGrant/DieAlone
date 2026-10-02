// Main3 task 8.23, the lake to LakeLayout.md draft 2 (Sable 2026-10-02; LakeLayout_UI.md, LakeLayout_Story.md): layout and walkability
// only, dressing is 11.0. Edit mode, Main3; rerunnable (absolute places; the new pieces rebuilt under Lake/Boathouse/Layout823 and
// Lake/Dock/Layout823; terrain set by rule, never stacked). In the runner after main3_8_21a_stops.cs.
// L2  the intake pipe down the dock's east edge, its elbow into the water at the end post (no collider); the fix point and stand.
// L3  a dark band on the notch bank by ring box W24_W's end; the sample target and stand.
// L4  the oar (planks, no collider) along the rowboat's north-west side.
// L5  the boathouse lamp and its practical go (Wren: no window lamp); an empty hook on the north wall; the roof seated: the west wall
//     modules and corner pillars stop under the roof, plank bands close the slots under the roof on the N, S and E walls.
// L6  the slip: the floor cut x 236.95 to 240.45, z 51.47 to 53.33 (floor collider and planks split round it), rails 1.05 on N, S and E,
//     the west wall's middle module removed as the boat door (wall collider split round it); the slip rest clamped to the east rail,
//     its collider 0.9 to 1.3 m over the floor; the stand facing 270.
// L7  the barrel to the NE corner, flush to the walls; the rope to (238.2, 54.75).
// L8  the step's north rail (mesh and collider) goes.
// L10 skirts with colliders: the house on four sides (the west one open across the slip; the north one with a 0.5 x 0.4 gap at the
//     water line at x 237.25 to 237.75), the step on W, N and E, the gangway's north edge with a 1.05 rail (PocketDeck_N, its skirt,
//     rails and posts go).
// L11 the shallows, bed -6.0, x 237.0 to 241.9, z 55.2 to 60.4; the beach x 241.9 to 245.3, z 53.1 to 60.4, from the bank at x 245.4
//     down to -6.0 (1.5 m over 3.4 m, 24 degrees or less); the house footprint is left alone.
// L12 the stake line, the wade limit there: log stakes every stakeStep m (tops -4.7), a sagging rope, from ring W0 west along z 60.4
//     to x 236.8 and south to the house NW corner; a wade box on it, 0.4 thick, top -4.0, on Ignore Raycast; the bed outside it down to
//     -7.5 west of x 240 only; ring boxes W93 to W95 go.
// L13 the reeds rebuilt in the water only, x 240.5 to 242.6, z 60.8 to 63.5, the bed there -6.0 or shallower; the reeds rest on the
//     bank (243.6, 62.0), its collider 0.9 to 1.3 m over the ground, the stand (244.1, 62.0) facing 290.
// L14 the wading event points; L15 the soft ground patch. Warps: Lake_Pump (190, 99) facing south, Lake_Boathouse (248.5, 52.4) west.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var lakeG = kit.Root("Lake"); if (lakeG == null) return "no Lake";
var B = lakeG.transform.Find("Boathouse"); var D = lakeG.transform.Find("Dock"); var bd = B != null ? B.Find("Dressing") : null;
if (B == null || D == null || bd == null) return "run 8.4 and 8.17 first (Lake/Boathouse, Lake/Dock, Boathouse/Dressing)";
var notes = new System.Collections.Generic.List<string>();
const float bx0 = 236.8f, bx1 = 243.2f, bz0 = 49.6f, bz1 = 55.2f, floorY = -3.8f, wallH = 2.6f, wt = 0.15f, doorZ = 52.4f, water = -5.5f, modH = 3f;   // 8.4 and 8.17
const float plankW = 0.3f, plankT = 0.06f, railH = 1.05f, railT = 0.06f, skirtBottom = -8.5f, skirtT = 0.1f, floorUnder = -4.0f;
var deckWood = kit.Tinted("Places_DockPlank", "Assets/Materials/Planks023A_1.0x1.0.mat", Hex("#6E5A44"), new UnityEngine.Vector2(1f, 3f));
var steel = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Steel.mat");
var dark = kit.Tinted("Places_SoftGround", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#2E241B"), UnityEngine.Vector2.one);
var band = kit.Tinted("Places_LakeBand", "Assets/Materials/Concrete034_1.0x1.0.mat", Hex("#1F2420"), UnityEngine.Vector2.one);
var rope = kit.Tinted("Places_Rope", "Assets/Materials/Planks023A_1.0x1.0.mat", Hex("#8C7A58"), UnityEngine.Vector2.one);
if (steel == null) return "no Slice_Steel material";
var L = kit.Fresh("Layout823", B, B.position, 0f); var LD = kit.Fresh("Layout823", D, D.position, 0f);
UnityEngine.GameObject Box(string n, UnityEngine.Transform parent, UnityEngine.Vector3 c, UnityEngine.Vector3 s, UnityEngine.Material m, bool collide, float yaw = 0f)
{ var g = kit.Slab(n, parent, parent.InverseTransformPoint(c), s, m, V(0f, yaw, 0f), collide); return g; }
// a pack log from a to b (world), girth g, no collider (8.17's Log)
UnityEngine.GameObject Log(UnityEngine.Transform parent, UnityEngine.Vector3 a, UnityEngine.Vector3 b, float g)
{
    var lg = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", parent); if (lg == null) return null; PlaceKit.StripColliders(lg);
    var dir = b - a; lg.transform.rotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.right, dir.normalized);
    lg.transform.localScale = V(dir.magnitude / 2f, g / 0.37f, g / 0.37f); lg.transform.position = (a + b) * 0.5f; lg.transform.position += (a + b) * 0.5f - PlaceKit.MeshBounds(lg).center; return lg;
}
UnityEngine.Transform Marker(string n, float x, float y, float z, float yaw) { var t = new UnityEngine.GameObject(n).transform; t.SetParent(L, false); t.position = V(x, y, z); t.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); return t; }
UnityEngine.Physics.SyncTransforms();

// ================= L2, L3: dock points =================
const float pipeX = 191.35f, pipeZ0 = 94.4f, pipeZ1 = 86.7f, pipeD = 0.1f, deckTop = -4.8f;
{
    var pipe = kit.Fill(PlaceKit.CE + "Pipes/Pipe_Green_Straight_Medium", LD, LD.InverseTransformPoint(V(pipeX, 0f, (pipeZ0 + pipeZ1) * 0.5f)), V(pipeD, pipeZ0 - pipeZ1, pipeD));
    if (pipe != null) { var c = PlaceKit.MeshBounds(pipe).center; pipe.transform.RotateAround(c, UnityEngine.Vector3.right, 90f); var b = PlaceKit.MeshBounds(pipe); pipe.transform.position += V(pipeX - b.center.x, deckTop + pipeD * 0.5f + 0.01f - b.center.y, (pipeZ0 + pipeZ1) * 0.5f - b.center.z); pipe.name = "IntakePipe"; }
    var drop = kit.Fill(PlaceKit.CE + "Pipes/Pipe_Green_Straight_Medium", LD, LD.InverseTransformPoint(V(pipeX, water - 0.4f, pipeZ1)), V(pipeD, deckTop - (water - 0.4f), pipeD)); if (drop != null) drop.name = "IntakeDrop";
    var elbow = kit.Fill(PlaceKit.CE + "Pipes/Pipe_Green_Elbow_90_Short_Smooth", LD, LD.InverseTransformPoint(V(pipeX, deckTop - 0.1f, pipeZ1 + 0.05f)), V(pipeD * 1.4f, 0.2f, 0.2f)); if (elbow != null) elbow.name = "IntakeElbow";
    var fp = new UnityEngine.GameObject("IntakeFixPoint").transform; fp.SetParent(LD, false); fp.position = V(191.3f, -4.6f, 86.7f);
    var fs = new UnityEngine.GameObject("IntakeFixStand").transform; fs.SetParent(LD, false); fs.position = V(190.8f, deckTop, 87.2f);
    Box("SampleBand", LD, V(188.0f, kit.H(188.0f, 88.2f) + 0.01f, 88.2f), V(1.2f, 0.02f, 0.4f), band, false);
    var st = new UnityEngine.GameObject("SampleTarget").transform; st.SetParent(LD, false); st.position = V(188.0f, -5.25f, 88.2f);
    var ss = new UnityEngine.GameObject("SampleStand").transform; ss.SetParent(LD, false); ss.position = V(187.9f, kit.H(187.9f, 88.7f), 88.7f);
}

// ================= L4: the oar by the rowboat =================
const float oarLen = 2.6f, oarBlade = 0.45f;
{
    var boat = kit.Root("PointsOfInterest") != null ? kit.Root("PointsOfInterest").transform.Find("POI_Overturned_rowboat") : null; float yaw = boat != null ? boat.eulerAngles.y : 0f;
    var hullAxis = boat != null ? (PlaceKit.MeshBounds(boat.gameObject).size.x > PlaceKit.MeshBounds(boat.gameObject).size.z ? 90f : 0f) : 0f;
    var oar = kit.Group("Oar", L, V(225.65f, kit.H(225.65f, 87.76f) + 0.04f, 87.76f), hullAxis);
    kit.Slab("Shaft", oar, V(0f, 0f, -oarBlade * 0.5f), V(0.05f, 0.05f, oarLen - oarBlade), deckWood);
    kit.Slab("Blade", oar, V(0f, 0f, (oarLen - oarBlade) * 0.5f), V(0.16f, 0.025f, oarBlade), deckWood);
    if (boat == null) notes.Add("no PointsOfInterest/POI_Overturned_rowboat");
}

// ================= L5: lamp, hook, roof =================
foreach (var n in new[] { "Lamp", "CITW_Hanging_Oil_Lamp" }) { var t = bd.Find(n); if (t != null) UnityEngine.Object.DestroyImmediate(t.gameObject); }
Box("EmptyHook", L, V(238.9f, floorY + 2.0f, bz1 - wt - 0.04f), V(0.04f, 0.14f, 0.08f), steel, false);
var roofC = B.Find("Roof") != null ? B.Find("Roof").GetComponent<UnityEngine.Collider>() : null;
float RoofUnder(float x, float z) { if (roofC == null) return floorY + wallH; var r = new UnityEngine.Ray(V(x, floorY + 0.5f, z), UnityEngine.Vector3.up); return roofC.Raycast(r, out var h, 10f) ? h.point.y : floorY + wallH; }
float yW = RoofUnder(bx0 + 0.05f, (bz0 + bz1) * 0.5f), yE = RoofUnder(bx1 - 0.05f, (bz0 + bz1) * 0.5f);
// the west wall modules and its two corner pillars stop under the roof (8.17 scaled them to wallH)
foreach (UnityEngine.Transform t in bd)
{
    if (!(t.name.StartsWith("CITW_Plank_") || t.name.StartsWith("CITW_Wood_Pillar")) || t.position.x > bx0 + 0.4f) continue;
    var b = PlaceKit.MeshBounds(t.gameObject); float want = yW - 0.02f - floorY; if (b.size.y < 0.1f) continue;
    t.localScale = V(t.localScale.x, t.localScale.y * want / b.size.y, t.localScale.z); var nb = PlaceKit.MeshBounds(t.gameObject); t.position += V(0f, floorY - nb.min.y, 0f);
}
// the boat door: the west wall's middle module goes (the window module from 8.17)
foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(bd))) if (t.name.StartsWith("CITW_Plank_Window_Wall") && t.position.x < bx0 + 0.4f) UnityEngine.Object.DestroyImmediate(t.gameObject);
// plank bands closing the slots under the roof: N and S along the slope, E square
const float bandH = 0.8f; float slope = UnityEngine.Mathf.Atan2(yE - yW, bx1 - bx0) * UnityEngine.Mathf.Rad2Deg; float midY = (yW + yE) * 0.5f - bandH * 0.5f;
foreach (var z in new[] { bz0 + wt * 0.5f, bz1 - wt * 0.5f }) { var g = kit.Slab("RoofBand", L, L.InverseTransformPoint(V((bx0 + bx1) * 0.5f, midY, z)), V(bx1 - bx0, bandH, wt), deckWood, V(0f, 0f, slope)); }
kit.Slab("RoofBand", L, L.InverseTransformPoint(V(bx1 - wt * 0.5f, (floorY + wallH + yE) * 0.5f, (bz0 + bz1) * 0.5f)), V(wt, UnityEngine.Mathf.Max(0.02f, yE - (floorY + wallH)), bz1 - bz0), deckWood);

// ================= L6: the slip =================
const float sx0 = 236.95f, sx1 = 240.45f, sz0 = 51.47f, sz1 = 53.33f;
{
    var floorC = B.Find("Floor") != null ? B.Find("Floor").GetComponent<UnityEngine.BoxCollider>() : null; if (floorC != null) floorC.enabled = false; else notes.Add("no Boathouse/Floor collider");
    var fl = kit.Group("SlipFloor", L, L.position, 0f); float fy = floorY - 0.1f;
    foreach (var (x0, x1, z0, z1) in new[] { (bx0, bx1, bz0, sz0), (bx0, bx1, sz1, bz1), (bx0, sx0, sz0, sz1), (sx1, bx1, sz0, sz1) }) kit.Blocker("Floor", fl, fl.InverseTransformPoint(V((x0 + x1) * 0.5f, fy, (z0 + z1) * 0.5f)), V(x1 - x0, 0.2f, z1 - z0));
    // the floor planks across the slip: split round it
    var cut = new System.Collections.Generic.List<UnityEngine.Transform>(); foreach (UnityEngine.Transform t in bd) if (t.name.StartsWith("C_Plank_A_Thick")) { var b = PlaceKit.MeshBounds(t.gameObject); if (b.size.z > 4f && b.center.x > sx0 - plankW * 0.5f && b.center.x < sx1 + plankW * 0.5f) cut.Add(t); }
    foreach (var t in cut) UnityEngine.Object.DestroyImmediate(t.gameObject);
    for (float x = bx0 + plankW * 0.5f; x < bx1; x += plankW)
    {
        if (x < sx0 - plankW * 0.5f || x > sx1 + plankW * 0.5f) continue;
        foreach (var (z0, z1) in new[] { (bz0, sz0), (sz1, bz1) }) kit.Fill(PlaceKit.CC + "Props/C_Plank_A_Thick", L, L.InverseTransformPoint(V(x, floorY - plankT, (z0 + z1) * 0.5f)), V(plankW - 0.02f, plankT, z1 - z0));
    }
    // rails 1.05 on N, S, E, on the floor side of the cut
    var rails = kit.Group("SlipRails", L, L.position, 0f);
    foreach (var (c, s, yaw) in new[] { (V((sx0 + sx1) * 0.5f, floorY, sz1 + railT * 0.5f), V(sx1 - sx0, railH, railT), 0f), (V((sx0 + sx1) * 0.5f, floorY, sz0 - railT * 0.5f), V(sx1 - sx0, railH, railT), 0f), (V(sx1 + railT * 0.5f, floorY, (sz0 + sz1) * 0.5f), V(railT, railH, sz1 - sz0), 90f) })
    {
        kit.Blocker("SlipRail", rails, rails.InverseTransformPoint(c + V(0f, railH * 0.5f, 0f)), s);
        var rl = kit.On(PlaceKit.CI + "Building/CITW_Railing", rails, rails.InverseTransformPoint(c), yaw, 1f, false, null, true); if (rl != null) rl.transform.localScale = V((yaw == 0f ? s.x : s.z) / 2f, railH / 1.05f, 1f);
    }
    // the boat door: the 8.4 west wall collider split round the middle module, open to the wall top
    var wW = B.Find("Wall_W") != null ? B.Find("Wall_W").GetComponent<UnityEngine.BoxCollider>() : null; if (wW != null) wW.enabled = false; else notes.Add("no Boathouse/Wall_W collider");
    foreach (var (z0, z1) in new[] { (bz0, sz0), (sz1, bz1) }) kit.Blocker("WallW_Solid", L, L.InverseTransformPoint(V(bx0 + wt * 0.5f, floorY + wallH * 0.5f, (z0 + z1) * 0.5f)), V(wt, wallH, z1 - z0));
    // the slip rest on the east rail: a bracket and a rod over the slip; its collider 0.9 to 1.3 m over the floor, nothing lower
    var rest = kit.Group("SlipRest", L, V(240.85f, floorY, 52.4f), 0f);
    kit.Slab("Bracket", rest, V(-0.2f, railH, 0f), V(0.4f, 0.06f, 0.12f), deckWood);
    kit.Slab("Rod", rest, V(-1.0f, railH + 0.45f, 0f), V(1.9f, 0.025f, 0.025f), steel, V(0f, 0f, -18f));
    kit.Blocker("RestCollider", rest, V(0f, 1.1f, 0f), V(0.15f, 0.4f, 0.25f));
    Marker("SlipStand", 241.3f, floorY, 52.4f, 270f);
}

// ================= L7: barrel and rope =================
{
    var barrel = bd.Find("CITW_Barrel_2"); if (barrel != null) { var b = PlaceKit.MeshBounds(barrel.gameObject); barrel.position += V(bx1 - wt - b.max.x, 0f, bz1 - wt - b.max.z); } else notes.Add("no CITW_Barrel_2");
    var rp = bd.Find("Rope"); if (rp != null) { var b = PlaceKit.MeshBounds(rp.gameObject); rp.position += V(238.2f - b.center.x, 0f, 54.75f - b.center.z); } else notes.Add("no Rope");
}

// ================= L8: the step's north rail goes =================
var step = bd.Find("Step");
if (step != null) foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(step))) if ((t.name == "StepRail" || t.name.StartsWith("CITW_Railing")) && t.localPosition.z > 0.5f) UnityEngine.Object.DestroyImmediate(t.gameObject);
if (step == null) notes.Add("no Dressing/Step");
const float stepX0 = 238.2f, stepX1 = 241.8f, stepZ1 = 57.0f, stepUnder = -4.1f;

// ================= L10: skirts =================
var sk = kit.Group("Skirts", L, L.position, 0f); int skirtN = 0;
void Skirt(float x0, float z0, float x1, float z1, float top, float bottom)   // a plank wall with a collider, axis-aligned
{
    float len = UnityEngine.Mathf.Max(x1 - x0, z1 - z0); bool alongX = (x1 - x0) >= (z1 - z0);
    var c = V((x0 + x1) * 0.5f, (top + bottom) * 0.5f, (z0 + z1) * 0.5f); var s = alongX ? V(len, top - bottom, skirtT) : V(skirtT, top - bottom, len);
    kit.Slab("Skirt", sk, sk.InverseTransformPoint(c), s, deckWood, default, true); skirtN++;
}
const float gapX0 = 237.25f, gapX1 = 237.75f, gapLow = water - 0.2f, gapHigh = water + 0.2f;
Skirt(bx0, bz0, bx1, bz0, floorUnder, skirtBottom);                     // south
Skirt(bx1, bz0, bx1, bz1, floorUnder, skirtBottom);                     // east
Skirt(bx0, bz0, bx0, sz0, floorUnder, skirtBottom); Skirt(bx0, sz1, bx0, bz1, floorUnder, skirtBottom);   // west, open across the slip
Skirt(bx0, bz1, gapX0, bz1, floorUnder, skirtBottom); Skirt(gapX1, bz1, bx1, bz1, floorUnder, skirtBottom);   // north, with the gap
Skirt(gapX0, bz1, gapX1, bz1, floorUnder, gapHigh); Skirt(gapX0, bz1, gapX1, bz1, gapLow, skirtBottom);
Skirt(stepX0, bz1, stepX0, stepZ1, stepUnder, skirtBottom); Skirt(stepX1, bz1, stepX1, stepZ1, stepUnder, skirtBottom); Skirt(stepX0, stepZ1, stepX1, stepZ1, stepUnder, skirtBottom);   // the step
// the gangway's north edge: PocketDeck_N and its pieces go; a skirt from the gangway to below the bed and a 1.05 rail on it
foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(B)))
    if (t.name == "PocketDeck_N" || t.name == "PocketSkirt_N" || t.name == "PocketRail_N" || t.name == "PocketRail_WN" || (t.name == "PocketPost" && t.position.z > doorZ)) UnityEngine.Object.DestroyImmediate(t.gameObject);
var gw = B.Find("Gangway"); float gEdgeZ = doorZ + 0.7f; int gwPieces = 0;
if (gw != null)
{
    var gc = gw.GetComponent<UnityEngine.Collider>(); var gb = gc.bounds; UnityEngine.Physics.SyncTransforms();
    float GTop(float x) => gc.Raycast(new UnityEngine.Ray(V(x, gb.max.y + 2f, doorZ), UnityEngine.Vector3.down), out var h, 10f) ? h.point.y : float.NaN;
    const float piece = 0.5f;
    for (float x = gb.min.x; x < gb.max.x - 0.01f; x += piece)
    {
        float x1 = UnityEngine.Mathf.Min(gb.max.x, x + piece), top = GTop((x + x1) * 0.5f); if (float.IsNaN(top)) continue;
        Skirt(x, gEdgeZ, x1, gEdgeZ, top - 0.12f, skirtBottom);
        kit.Blocker("GangwayRail", sk, sk.InverseTransformPoint(V((x + x1) * 0.5f, top + railH * 0.5f, gEdgeZ)), V(x1 - x, railH, railT)); gwPieces++;
    }
    float t0 = GTop(gb.min.x + 0.1f), t1 = GTop(gb.max.x - 0.1f);
    if (!float.IsNaN(t0) && !float.IsNaN(t1)) Log(sk, V(gb.min.x, t0 + railH, gEdgeZ), V(gb.max.x, t1 + railH, gEdgeZ), 0.1f);
}
else notes.Add("no Boathouse/Gangway");

// ================= L11, L12, L13: shallows, beach, deep water, reed bed (terrain) =================
const float shX0 = 237.0f, shX1 = 241.9f, shZ0 = 55.2f, shZ1 = 60.4f, beachX1 = 245.3f, beachZ0 = 53.1f, shallow = -6.0f, deep = -7.5f, deepX1 = 240f, bankX = 245.4f;
const float reedX0 = 240.5f, reedX1 = 242.6f, reedZ0 = 60.8f, reedZ1 = 63.5f, deepZ1 = 64f, deepX0 = 233.5f, editPad = 1f;
int cellsSet = 0;
{
    var ter = kit.Terrain; var data = ter.terrainData; var org = ter.transform.position; int res = data.heightmapResolution; float cx = data.size.x / (res - 1), cz = data.size.z / (res - 1);
    float x0 = deepX0 - editPad, x1 = bankX + editPad, z0 = beachZ0 - editPad, z1 = deepZ1 + editPad;
    int i0 = UnityEngine.Mathf.FloorToInt((x0 - org.x) / cx), i1 = UnityEngine.Mathf.CeilToInt((x1 - org.x) / cx), j0 = UnityEngine.Mathf.FloorToInt((z0 - org.z) / cz), j1 = UnityEngine.Mathf.CeilToInt((z1 - org.z) / cz);
    int w = i1 - i0 + 1, h = j1 - j0 + 1; var hs = data.GetHeights(i0, j0, w, h);
    float Norm(float y) => (y - org.y) / data.size.y; float World(float n) => n * data.size.y + org.y;
    var bank = new System.Collections.Generic.Dictionary<int, float>();   // the bank at x bankX for each row, read before any change
    for (int j = 0; j < h; j++) { float z = org.z + (j0 + j) * cz; bank[j] = kit.H(bankX, z); }
    for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
    {
        float x = org.x + (i0 + i) * cx, z = org.z + (j0 + j) * cz, y = World(hs[j, i]), want = y;
        bool house = x >= bx0 && x <= bx1 && z >= bz0 && z <= bz1;
        if (house) continue;
        if (x >= shX0 && x <= shX1 && z >= shZ0 && z <= shZ1) want = shallow;
        else if (x > shX1 && x <= beachX1 && z >= beachZ0 && z <= shZ1) want = UnityEngine.Mathf.Max(shallow, bank[j] - (bank[j] - shallow) * (beachX1 - x) / (beachX1 - shX1));
        else if (x >= reedX0 && x <= reedX1 && z >= reedZ0 && z <= reedZ1) want = UnityEngine.Mathf.Max(y, shallow);
        else if (x < deepX1 && x >= deepX0 && ((z > shZ1 + 0.3f && z <= deepZ1) || (x < shX0 - 0.3f && z >= bz1 && z <= shZ1))) want = UnityEngine.Mathf.Min(y, deep);
        if (UnityEngine.Mathf.Abs(want - y) > 1e-4f) { hs[j, i] = Norm(want); cellsSet++; }
    }
    data.SetHeights(i0, j0, hs); UnityEditor.EditorUtility.SetDirty(data);
}
UnityEngine.Physics.SyncTransforms();
// L12: ring boxes W93 to W95 go; the stake line and its wade box
var wade = lakeG.transform.Find("WadeLimit"); int ringGone = 0;
if (wade != null) foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(wade))) if (t.name == "W93" || t.name == "W94" || t.name == "W95" || t.name.StartsWith("W93_") || t.name.StartsWith("W94_") || t.name.StartsWith("W95_")) { UnityEngine.Object.DestroyImmediate(t.gameObject); ringGone++; }
var w0 = wade != null ? wade.Find("W0") : null; float lineX1 = w0 != null ? w0.GetComponent<UnityEngine.Collider>().bounds.center.x : 243.1f;
const float lineZ = 60.4f, lineX0 = 236.8f, stakeStep = 2f, stakeTop = -4.7f, stakeG = 0.14f, boxT = 0.4f, boxTop = -4.0f, sag = 0.12f, ropeY = -4.85f, wadeOver = 2f, wadeReach = 2.5f, wadeSample = 0.5f;
var stakes = kit.Group("StakeLine", L, L.position, 0f); int stakeN = 0;
var line = new[] { V(lineX1, 0f, lineZ), V(lineX0, 0f, lineZ), V(lineX0, 0f, bz1) };
for (int s = 1; s < line.Length; s++)
{
    var a = line[s - 1]; var b = line[s]; float len = (b - a).magnitude; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(len / stakeStep)); var prev = UnityEngine.Vector3.zero;
    for (int k = 0; k <= n; k++)
    {
        var p = UnityEngine.Vector3.Lerp(a, b, k / (float)n); if (s > 1 && k == 0) { prev = V(p.x, ropeY, p.z); continue; }
        Log(stakes, V(p.x, kit.H(p.x, p.z) - 0.3f, p.z), V(p.x, stakeTop, p.z), stakeG); stakeN++;
        var top = V(p.x, ropeY, p.z); if (k > 0) { var mid = (prev + top) * 0.5f + V(0f, -sag, 0f); foreach (var (u, v) in new[] { (prev, mid), (mid, top) }) { var d = v - u; var r = kit.Slab("Rope", stakes, stakes.InverseTransformPoint((u + v) * 0.5f), V(0.03f, 0.03f, d.magnitude), rope); r.transform.rotation = UnityEngine.Quaternion.LookRotation(d.normalized); } }
        prev = top;
    }
    var dirAB = (b - a).normalized;
    // its top: at least boxTop, and wadeOver m over the highest walkable surface within wadeReach m (the brush band method, 8.22 round 2;
    // the lake flood stood on the west leg's -4.0 top from the house and step floor at -3.8)
    float segTop = boxTop; UnityEngine.Physics.SyncTransforms(); var side = V(dirAB.z, 0f, -dirAB.x);
    for (float u = -wadeReach; u <= len + wadeReach + 1e-3f; u += wadeSample) for (float v = -wadeReach; v <= wadeReach + 1e-3f; v += wadeSample)
    {
        var q = a + dirAB * u + side * v;
        foreach (var hh in UnityEngine.Physics.RaycastAll(V(q.x, floorY + 6f, q.z), UnityEngine.Vector3.down, 20f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        { if (hh.collider.gameObject.layer == 2 || hh.normal.y < 0.7071f || WalkIns.PathOf(hh.collider.transform).StartsWith("Forest") || hh.collider.name == "Roof" || hh.collider is UnityEngine.CapsuleCollider) continue; segTop = UnityEngine.Mathf.Max(segTop, hh.point.y + wadeOver); }
    }
    var box = new UnityEngine.GameObject("WadeBox"); box.transform.SetParent(stakes, false); box.layer = 2;
    box.transform.position = V((a.x + b.x) * 0.5f, (segTop + skirtBottom) * 0.5f, (a.z + b.z) * 0.5f); box.transform.rotation = UnityEngine.Quaternion.LookRotation(dirAB);
    box.AddComponent<UnityEngine.BoxCollider>().size = V(boxT, segTop - skirtBottom, len + (s == 1 ? 0.6f : boxT));   // the east end runs into W0, the south end into the house corner
}
// L13: the reeds in the water only (8.17's west reeds go), the reeds rest on the bank
var oldReeds = bd.Find("Reeds"); if (oldReeds != null) UnityEngine.Object.DestroyImmediate(oldReeds.gameObject);
{
    var reeds = kit.Group("Reeds", L, V((reedX0 + reedX1) * 0.5f, water, (reedZ0 + reedZ1) * 0.5f), 0f); var rng = new System.Random(8023);
    for (int i = 0; i < 26; i++) { float x = reedX0 + 0.3f + (float)rng.NextDouble() * (reedX1 - reedX0 - 0.6f), z = reedZ0 + 0.3f + (float)rng.NextDouble() * (reedZ1 - reedZ0 - 0.6f), hh = 1.4f + 0.6f * (float)rng.NextDouble(); var rd = kit.Fill(PlaceKit.CS + "Vegetation/CS_Grass_Long_" + (1 + i % 2), reeds, reeds.InverseTransformPoint(V(x, kit.H(x, z) - 0.05f, z)), V(0.9f, hh, 0.9f), (float)rng.NextDouble() * 360f); if (rd != null) PlaceKit.StripColliders(rd); }
    float g = kit.H(243.6f, 62.0f); var rr = kit.Group("ReedsRest", L, V(243.6f, g, 62.0f), 290f - 270f);
    Log(rr, V(243.6f, g - 0.2f, 62.0f), V(243.5f, g + 1.05f, 62.0f), 0.06f);
    kit.Slab("Rod", rr, V(-0.9f, 1.4f, 0f), V(1.9f, 0.025f, 0.025f), steel, V(0f, 0f, -18f));
    kit.Blocker("RestCollider", rr, V(0f, 1.1f, 0f), V(0.15f, 0.4f, 0.25f));
    Marker("ReedsStand", 244.1f, kit.H(244.1f, 62.0f), 62.0f, 290f);
}

// ================= L14, L15: event points, soft ground =================
Marker("Event_UnderStilts", 237.5f, shallow, 55.95f, 0f); Marker("Event_OffStep", 240.0f, shallow, 57.6f, 180f); Marker("Event_RopeDrift", 238.5f, shallow, 60.0f, 0f);
Box("SoftGround", L, V(250.0f, kit.H(250.0f, 58.35f) + 0.01f, 58.35f), V(1.2f, 0.02f, 0.7f), dark, false);
// warps (doc 5.1)
var warps = kit.Root("DevWarps").transform;
var wp = warps.Find("Lake_Pump"); if (wp != null) { wp.position = V(190f, kit.H(190f, 99f) + 0.2f, 99f); wp.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f); } else notes.Add("no warp Lake_Pump");
var wb = warps.Find("Lake_Boathouse"); if (wb != null) { wb.position = V(248.5f, kit.H(248.5f, 52.4f) + 0.2f, 52.4f); wb.rotation = UnityEngine.Quaternion.Euler(0f, 270f, 0f); } else notes.Add("no warp Lake_Boathouse");

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | roof under west " + F(yW) + ", east " + F(yE) + " | slip cut, rails, boat door | skirts " + skirtN + " (gangway " + gwPieces + " pieces) | terrain cells " + cellsSet + " | ring boxes removed " + ringGone + " | stakes " + stakeN + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
