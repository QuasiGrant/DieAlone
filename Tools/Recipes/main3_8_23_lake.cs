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
// Round 2 (Wren's 8.23 fix list, 2026-10-02): DOCK rail stops, the ground under the deck and posts under the ramp, brush cleared; the
// slip's east plank strip; skirts as boards and posts over invisible colliders; her blanket as a bed; the reeds rest over the highest
// ground near it; the north-east water line.
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
const float plankW = 0.3f, plankT = 0.06f, railH = 1.05f, railT = 0.06f, skirtBottom = -8.5f, floorUnder = -4.0f;
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
// a stop's top: at least floorTop, and stopOver m over the highest walkable surface within stopReach m of the line a to b (the brush band
// method, 8.22 round 2); the wade ring (a stop, never stood on), the stop's own pieces and anything named in skip are left out
const float stopOver = 2f, stopReach = 2.5f, stopSample = 0.5f;
float StopTop(UnityEngine.Vector3 a, UnityEngine.Vector3 b, float floorTop, params string[] skip)
{
    float top = floorTop; UnityEngine.Physics.SyncTransforms(); var ab = V(b.x - a.x, 0f, b.z - a.z); float len = ab.magnitude; var dir = len > 1e-3f ? ab / len : UnityEngine.Vector3.forward; var side = V(dir.z, 0f, -dir.x);
    for (float u = -stopReach; u <= len + stopReach + 1e-3f; u += stopSample) for (float v = -stopReach; v <= stopReach + 1e-3f; v += stopSample)
    {
        var q = a + dir * u + side * v;
        foreach (var hh in UnityEngine.Physics.RaycastAll(V(q.x, floorY + 6f, q.z), UnityEngine.Vector3.down, 20f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            if (hh.collider.gameObject.layer == 2 || hh.normal.y < 0.7071f || hh.collider.name == "Roof" || hh.collider is UnityEngine.CapsuleCollider) continue;
            var hp = WalkIns.PathOf(hh.collider.transform); bool skipIt = hp.StartsWith("Forest") || hp.StartsWith("Lake/WadeLimit"); foreach (var s in skip) if (hp.StartsWith(s)) skipIt = true; if (skipIt) continue;
            top = UnityEngine.Mathf.Max(top, hh.point.y + stopOver);
        }
    }
    return top;
}

// ================= DOCK (8.23 round 2): the deck stands proud of the bank, posts under the ramp, brush cleared, rail stops =================
// Marlow 823 finding 1: the rail ends stood 0.69 m over the bank, so a jump stood the player on a rail and a jump off the end rail
// landed in the closed lake. Each rail and the end rail get an invisible RailStop (Ignore Raycast, as the tower's) round it, from the deck
// to stopOver m over the highest walkable surface within stopReach m. Vesper 823 note 1: the bank stood over the ramp and its brush hid
// the deck from the trail end; the ground under the dock goes dockClear m under the deck's underside (none at the ramp foot, where the
// trail meets it) and climbs back at bankSlope beyond the deck's sides; detail cleared brushClear m round the deck.
const float heightTol = 0.005f;   // the terrain stores heights in 16 bits (about 2 mm here), so a rerun reads its own heights back that close
const float dockX0 = 188.4f, dockX1 = 191.6f, dockZ0 = 86.4f, dockZ1 = 96.0f, deckT = 0.12f, dockClear = 0.5f, rampFootRun = 2f, bankSlope = 0.6f, bankMargin = 1.5f, brushClear = 1f, dockPostG = 0.24f, railPad = 0.02f;
int dockCells = 0, dockPosts = 0, brushCells = 0; var railStops = new System.Collections.Generic.List<string>();
{
    var deckCs = new System.Collections.Generic.List<UnityEngine.Collider>(); foreach (var n in new[] { "DeckFlat", "DeckRamp" }) { var t = D.Find(n); if (t != null && t.GetComponent<UnityEngine.Collider>() != null) deckCs.Add(t.GetComponent<UnityEngine.Collider>()); else notes.Add("no Lake/Dock/" + n + " collider"); }
    float DeckTop(float x, float z) { float best = float.NaN; foreach (var c in deckCs) if (c.Raycast(new UnityEngine.Ray(V(x, floorY + 6f, z), UnityEngine.Vector3.down), out var h, 20f)) best = float.IsNaN(best) ? h.point.y : UnityEngine.Mathf.Max(best, h.point.y); return best; }
    var ter = kit.Terrain; var data = ter.terrainData; var org = ter.transform.position; int res = data.heightmapResolution; float cx = data.size.x / (res - 1), cz = data.size.z / (res - 1);
    int i0 = UnityEngine.Mathf.FloorToInt((dockX0 - bankMargin - org.x) / cx), i1 = UnityEngine.Mathf.CeilToInt((dockX1 + bankMargin - org.x) / cx), j0 = UnityEngine.Mathf.FloorToInt((dockZ0 - org.z) / cz), j1 = UnityEngine.Mathf.CeilToInt((dockZ1 - org.z) / cz);
    int w = i1 - i0 + 1, h = j1 - j0 + 1; var hs = data.GetHeights(i0, j0, w, h);
    for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
    {
        float x = org.x + (i0 + i) * cx, z = org.z + (j0 + j) * cz; if (z < dockZ0 || z > dockZ1) continue;
        float top = DeckTop(UnityEngine.Mathf.Clamp(x, dockX0 + 0.05f, dockX1 - 0.05f), z); if (float.IsNaN(top)) continue;
        float clear = dockClear * UnityEngine.Mathf.Clamp01((dockZ1 - z) / rampFootRun), outside = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(dockX0 - x, x - dockX1));
        float want = top - deckT - clear + bankSlope * outside, y = hs[j, i] * data.size.y + org.y;
        if (want < y - heightTol) { hs[j, i] = (want - org.y) / data.size.y; dockCells++; }   // only ever lowers: rerunnable
    }
    data.SetHeights(i0, j0, hs); UnityEditor.EditorUtility.SetDirty(data); UnityEngine.Physics.SyncTransforms();
    // posts under the ramp, as 8.17's under the flat deck (their tops at the deck's underside, so nothing stands above the planks)
    foreach (var z in new[] { 92.0f, 94.0f }) foreach (var x in new[] { dockX0 + 0.15f, dockX1 - 0.15f })
    { float top = DeckTop(x, z); if (float.IsNaN(top)) continue; Log(LD, V(x, kit.H(x, z) - 0.3f, z), V(x, top - deckT, z), dockPostG); dockPosts++; }
    // brush: the terrain's detail cleared over the deck and brushClear m round it, pump to water
    for (float z = dockZ0; z <= dockZ1 + brushClear + 0.01f; z += 0.5f) for (float x = dockX0 - brushClear; x <= dockX1 + brushClear + 0.01f; x += 0.5f) brushCells += kit.ClearDetail(V(x, 0f, z), 0.5f);
    // rail stops
    var rs = kit.Group("RailStops", LD, LD.position, 0f);
    foreach (UnityEngine.Transform t in D)
    {
        if (t.name != "Rail" && t.name != "RailEnd") continue; var c = t.GetComponent<UnityEngine.Collider>(); if (c == null) continue; var b = c.bounds;
        bool alongZ = b.size.z >= b.size.x; var a = alongZ ? V(b.center.x, 0f, b.min.z) : V(b.min.x, 0f, b.center.z); var e = alongZ ? V(b.center.x, 0f, b.max.z) : V(b.max.x, 0f, b.center.z);
        float top = StopTop(a, e, b.max.y, "Lake/Dock/Rail", "Lake/Dock/Layout823/RailStops");
        var g = new UnityEngine.GameObject("RailStop"); g.transform.SetParent(rs, false); g.layer = 2; g.transform.position = V(b.center.x, (b.min.y + top) * 0.5f, b.center.z);
        g.AddComponent<UnityEngine.BoxCollider>().size = V(b.size.x + 2f * railPad, top - b.min.y, b.size.z + 2f * railPad); railStops.Add(t.name + " top " + F(top));
    }
}

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
        // a plank that straddles the slip's east edge keeps its part east of the cut across the slip's span (Marlow 823 finding 2: a
        // 0.2 m strip of water showed between the east rail and the next plank)
        float px0 = x - plankW * 0.5f + 0.01f, px1 = x + plankW * 0.5f - 0.01f;
        if (px0 < sx1 && px1 > sx1) kit.Fill(PlaceKit.CC + "Props/C_Plank_A_Thick", L, L.InverseTransformPoint(V((sx1 + px1) * 0.5f, floorY - plankT, (sz0 + sz1) * 0.5f)), V(px1 - sx1, plankT, sz1 - sz0));
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
// her blanket (8.23 round 2, Vesper 823 notes 7 and Pim's cat step: 8.17's flat CITW_Blanket read as a doormat and did not show from the
// deck): a bed, a pad with a rolled rim on the water side and the west end, open toward the doorway, warmer than the planks; no collider
// (so nothing the interactor or the body meets), as L8
const float bedX = 240.5f, bedZ = 56.62f, bedW = 0.84f, bedD = 0.68f, padH = 0.08f, rollG = 0.16f;   // L8 puts her blanket at (240.7, 56.65); 0.2 m west so the larger bed stands 0.19 m clear of the bowl
int bedPieces = 0;
if (step != null)
{
    foreach (var n in new[] { "CITW_Blanket", "Blanket" }) { var t = step.Find(n); if (t != null) UnityEngine.Object.DestroyImmediate(t.gameObject); }
    var warm = kit.Tinted("Places_CatBlanket", "Assets/Revolving Pizza Games/Cabin In The Woods/Materials/CITW_Blanket_Pillow_Armchair_1.mat", Hex("#E09A44"), UnityEngine.Vector2.one);
    var bed = kit.Group("Blanket", step, V(bedX, floorY, bedZ), 0f);
    UnityEngine.GameObject Piece(UnityEngine.Vector3 lp, UnityEngine.Vector3 size, float yaw)
    { var g = kit.Fill(PlaceKit.CI + "Furniture/CITW_Pillow", bed, lp, size, yaw); if (g == null) return null; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) r.sharedMaterial = warm; bedPieces++; return g; }
    Piece(V(0f, 0f, 0f), V(bedW, padH, bedD), 0f);
    Piece(V(0f, padH * 0.5f, bedD * 0.5f - rollG * 0.5f), V(bedW, rollG, rollG), 0f);                                    // the water side
    Piece(V(-(bedW * 0.5f - rollG * 0.5f), padH * 0.5f, -rollG * 0.5f), V(rollG, rollG, bedD - rollG), 0f);   // the west end; the east end stays open, as a rim there hid the bowl from the deck
}
const float stepX0 = 238.2f, stepX1 = 241.8f, stepZ1 = 57.0f, stepUnder = -4.1f;

// ================= L10: skirts =================
var sk = kit.Group("Skirts", L, L.position, 0f); int skirtN = 0, boardN = 0;
// 8.23 round 2 (Vesper 823 note 2, Marlow 823 finding 3: the cube skirts read as flat black slabs): a skirt is an invisible box
// collider (postG thick, so it holds the posts) and, over it, upright boards with a slit between them and a post every postStep m, from
// its top to boardWet m under the water (the water is opaque below that); the boards are 8.17's floor plank mesh stood on end
const float boardW = 0.22f, boardGap = 0.06f, boardT = 0.05f, boardWet = 0.3f, postStep = 1.6f, postG = 0.2f;
void Board(UnityEngine.Vector3 c, float h, bool alongX)
{
    var g = kit.Fill(PlaceKit.CC + "Props/C_Plank_A_Thick", sk, sk.InverseTransformPoint(c), alongX ? V(boardW, boardT, h) : V(h, boardT, boardW), alongX ? 0f : 90f); if (g == null) return;
    var mb = PlaceKit.MeshBounds(g); g.transform.RotateAround(mb.center, alongX ? UnityEngine.Vector3.right : UnityEngine.Vector3.forward, 90f);
    mb = PlaceKit.MeshBounds(g); g.transform.position += c - mb.center; boardN++;
}
void Skirt(float x0, float z0, float x1, float z1, float top, float bottom)   // axis-aligned
{
    float len = UnityEngine.Mathf.Max(x1 - x0, z1 - z0); bool alongX = (x1 - x0) >= (z1 - z0);
    var c = V((x0 + x1) * 0.5f, (top + bottom) * 0.5f, (z0 + z1) * 0.5f); var s = alongX ? V(len, top - bottom, postG) : V(postG, top - bottom, len);
    kit.Blocker("Skirt", sk, sk.InverseTransformPoint(c), s); skirtN++;
    float vBottom = UnityEngine.Mathf.Max(bottom, water - boardWet); if (top - vBottom < 0.05f) return;
    var a = V(x0, 0f, z0); var dir = alongX ? UnityEngine.Vector3.right : UnityEngine.Vector3.forward; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(len / (boardW + boardGap)));
    float pitch = len / n; for (int k = 0; k < n; k++) { var p = a + dir * (pitch * (k + 0.5f)); Board(V(p.x, (top + vBottom) * 0.5f, p.z), top - vBottom, alongX); }
    if (len < postStep * 0.5f) return; int posts = UnityEngine.Mathf.CeilToInt(len / postStep);
    for (int k = 0; k <= posts; k++) { var p = a + dir * (len * k / posts); Log(sk, V(p.x, vBottom, p.z), V(p.x, top, p.z), postG); }
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
// the north-east water line (8.23 round 2, Marlow 823 finding 6): the bed met the bank in a 0.5 m step that moved one cell per row, so the
// water drew a saw-tooth. Across a band either side of a smooth shore line the ground is set to the water plus shoreK per metre from it
// (a function of place only, so a rerun sets the same heights); the reed bed and the shallows keep their own rules
var shore = new[] { V(225.0f, 0f, 78.3f), V(230.0f, 0f, 76.5f), V(234.0f, 0f, 73.8f), V(238.0f, 0f, 70.6f), V(241.0f, 0f, 67.6f), V(242.4f, 0f, 65.2f), V(242.9f, 0f, 63.0f), V(243.0f, 0f, 60.8f) };
const float shoreK = 0.45f, shoreWet = 1.2f, shoreDry = 1.0f, shoreReedPad = 0.3f; int shoreCells = 0;   // counts only cells that move more than heightTol
{
    var ter = kit.Terrain; var data = ter.terrainData; var org = ter.transform.position; int res = data.heightmapResolution; float cx = data.size.x / (res - 1), cz = data.size.z / (res - 1);
    float x0 = 225.0f - shoreWet, x1 = 243.0f + shoreDry + 0.5f, z0 = 60.8f, z1 = 78.3f;
    int i0 = UnityEngine.Mathf.FloorToInt((x0 - org.x) / cx), i1 = UnityEngine.Mathf.CeilToInt((x1 - org.x) / cx), j0 = UnityEngine.Mathf.FloorToInt((z0 - org.z) / cz), j1 = UnityEngine.Mathf.CeilToInt((z1 - org.z) / cz);
    int w = i1 - i0 + 1, h = j1 - j0 + 1; var hs = data.GetHeights(i0, j0, w, h);
    for (int j = 0; j < h; j++) for (int i = 0; i < w; i++)
    {
        float x = org.x + (i0 + i) * cx, z = org.z + (j0 + j) * cz; if (z < z0 || z > z1) continue;
        if (x >= reedX0 - shoreReedPad && x <= reedX1 + shoreReedPad && z >= reedZ0 - shoreReedPad && z <= reedZ1 + shoreReedPad) continue;
        var p = V(x, 0f, z); float best = float.MaxValue, d = 0f;
        for (int s = 1; s < shore.Length; s++)
        {
            var a = shore[s - 1]; var ab = shore[s] - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector3.Dot(p - a, ab) / ab.sqrMagnitude); var q = a + ab * t; float dist = (p - q).magnitude;
            if (dist < best) { best = dist; var n = V(-ab.z, 0f, ab.x).normalized; d = UnityEngine.Vector3.Dot(p - q, n) >= 0f ? dist : -dist; }   // positive on the land side
        }
        if (d < -shoreWet || d > shoreDry) continue;
        float want = water + shoreK * d; if (UnityEngine.Mathf.Abs(want - (hs[j, i] * data.size.y + org.y)) < heightTol) continue;
        hs[j, i] = (want - org.y) / data.size.y; shoreCells++;
    }
    data.SetHeights(i0, j0, hs); UnityEditor.EditorUtility.SetDirty(data);
}
UnityEngine.Physics.SyncTransforms();
// L12: ring boxes W93 to W95 go; the stake line and its wade box
var wade = lakeG.transform.Find("WadeLimit"); int ringGone = 0;
if (wade != null) foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(wade))) if (t.name == "W93" || t.name == "W94" || t.name == "W95" || t.name.StartsWith("W93_") || t.name.StartsWith("W94_") || t.name.StartsWith("W95_")) { UnityEngine.Object.DestroyImmediate(t.gameObject); ringGone++; }
var w0 = wade != null ? wade.Find("W0") : null; float lineX1 = w0 != null ? w0.GetComponent<UnityEngine.Collider>().bounds.center.x : 243.1f;
const float lineZ = 60.4f, lineX0 = 236.8f, stakeStep = 2f, stakeTop = -4.7f, stakeG = 0.14f, boxT = 0.4f, boxTop = -4.0f, sag = 0.12f, ropeY = -4.85f;
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
    // its top: at least boxTop, and stopOver m over the highest walkable surface within stopReach m (StopTop;
    // the lake flood stood on the west leg's -4.0 top from the house and step floor at -3.8)
    float segTop = StopTop(a, b, boxTop);
    var box = new UnityEngine.GameObject("WadeBox"); box.transform.SetParent(stakes, false); box.layer = 2;
    box.transform.position = V((a.x + b.x) * 0.5f, (segTop + skirtBottom) * 0.5f, (a.z + b.z) * 0.5f); box.transform.rotation = UnityEngine.Quaternion.LookRotation(dirAB);
    box.AddComponent<UnityEngine.BoxCollider>().size = V(boxT, segTop - skirtBottom, len + (s == 1 ? 0.6f : boxT));   // the east end runs into W0, the south end into the house corner
}
// L13: the reeds in the water only (8.17's west reeds go), the reeds rest on the bank
var oldReeds = bd.Find("Reeds"); if (oldReeds != null) UnityEngine.Object.DestroyImmediate(oldReeds.gameObject);
{
    var reeds = kit.Group("Reeds", L, V((reedX0 + reedX1) * 0.5f, water, (reedZ0 + reedZ1) * 0.5f), 0f); var rng = new System.Random(8023);
    for (int i = 0; i < 26; i++) { float x = reedX0 + 0.3f + (float)rng.NextDouble() * (reedX1 - reedX0 - 0.6f), z = reedZ0 + 0.3f + (float)rng.NextDouble() * (reedZ1 - reedZ0 - 0.6f), hh = 1.4f + 0.6f * (float)rng.NextDouble(); var rd = kit.Fill(PlaceKit.CS + "Vegetation/CS_Grass_Long_" + (1 + i % 2), reeds, reeds.InverseTransformPoint(V(x, kit.H(x, z) - 0.05f, z)), V(0.9f, hh, 0.9f), (float)rng.NextDouble() * 360f); if (rd != null) PlaceKit.StripColliders(rd); }
    // its collider restLow to restHigh m over the highest walkable ground within stopReach m, not only the ground at its foot (8.23
    // round 2, Marlow 823 finding 4: the bank east of it stood 0.6 m higher, and a sprint-jump from there stood on it)
    const float restLow = 0.9f, restHigh = 1.3f;
    float g = kit.H(243.6f, 62.0f), gHigh = StopTop(V(243.6f, 0f, 62.0f), V(243.6f, 0f, 62.0f), float.MinValue) - stopOver, cLow = UnityEngine.Mathf.Max(g, gHigh) + restLow - g, cHigh = cLow + (restHigh - restLow);
    var rr = kit.Group("ReedsRest", L, V(243.6f, g, 62.0f), 290f - 270f);
    Log(rr, V(243.6f, g - 0.2f, 62.0f), V(243.5f, g + cHigh - 0.05f, 62.0f), 0.06f);
    kit.Slab("Rod", rr, V(-0.9f, cHigh + 0.1f, 0f), V(1.9f, 0.025f, 0.025f), steel, V(0f, 0f, -18f));
    kit.Blocker("RestCollider", rr, V(0f, (cLow + cHigh) * 0.5f, 0f), V(0.15f, cHigh - cLow, 0.25f));
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
return "saved=" + saved + " | dock: ground cells lowered " + dockCells + ", ramp posts " + dockPosts + ", brush cells cleared " + brushCells + ", rail stops " + string.Join(", ", railStops) + " | bed pieces " + bedPieces + " | skirt boards " + boardN + " | shore cells " + shoreCells + " | roof under west " + F(yW) + ", east " + F(yE) + " | slip cut, rails, boat door | skirts " + skirtN + " (gangway " + gwPieces + " pieces) | terrain cells " + cellsSet + " | ring boxes removed " + ringGone + " | stakes " + stakeN + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
