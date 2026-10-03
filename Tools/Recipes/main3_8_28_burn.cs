// Main3 task 8.28, the old burn, the forage patches, Jg and the Jg and T trails, to BurnLayout.md draft 2 (Sable 2026-10-02; BurnLayout_UI.md,
// Pim; Marlow's 828 ground survey and paper check). Layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (moves are
// rules on where a piece stands now, so a piece already in place stays; the new pieces rebuilt under Places/Burn828). In the runner after
// main3_8_27_cave.cs and before main3_8_18a_solid.cs.
// NEEDS main3_8_28_keepouts.patch applied to Assets/Editor/KeepOuts.cs first (KeepOuts.Burn, WhichBurn) and main3_8_28_warplabels.patch
// applied to Assets/Scripts/Dev/DevWarpLabels.cs: both written while the 8.25a gate held the Editor, so they were kept out of Assets.
// B1  forage A (POI_Forage_patch_A, as built): each Bush keeps a solid collider and gets the stand-in usable `Forage` (the interact ray
//     skips triggers); the stand marker (241.6, 160.6) facing 301. R: SliceLook/ShotGround CS_Log_Firewood (240.21, 161.01). Keep clear:
//     nothing over clearOver m in a clearHalf m band from the Camp to Jg centreline to each of the two bushes nearest it (Forest and
//     SliceLook pieces removed). Deck screen: three RedFir5 at x 235.8, z 161.9, 163.15 and 164.4, screenTall m, trunk capsules.
// B2  the Hollow Giant: the gray Trunk's r 4.5 collider goes and the pack Sequoia gets PlaceKit.PackTrunkCapsule (8.16a numbers). The hollow:
//     a charred shell round a dark recess on the bark, hollowW wide, hollowH tall, hollowD deep, facing bearing 28, set where a ray along
//     that bearing meets the new capsule at hollowRayUp m over the ground; the inspect stand (203.9, 143.6) facing 208, no prompt.
// B3  Sign_Jg moves whole to (263.8, 170.2), its arms' bearings unchanged. Hedge_Burn_7: any collider box inside r jgOpen of the
//     junction is reported (BurnLayout: it stays only if its box keeps outside; not removed here).
// B4  the trailhead board at T: two arms on its north post, CAMP along 260.4 and CAMP 2 along 184.4, labelled both faces; the board gets a
//     collider and the stand-in usable `Examine` (BurnLayout_UI 1.5).
// B5  the Gate Tree stub: the gray Trunk's r 4.0 collider goes and the drawn Celestia Tree_Dead gets PlaceKit.DeadTrunkCapsule (8.16's snag
//     numbers; PackTrunkCapsule, the doc's word, is for BK pack trees and finds no LOD group on this one).
// B6  the firebreak, visual only: x 205 to 211 from the Camp to Jg corridor edge (trail + corridorHalf) north to z 200: every Forest and
//     SliceLook tree, snag or sapling standing in it is cut, and stumps every stumpStep m in three rows, stumpTall m high, no collider
//     (under the walk-into check's 0.5 m).
// B7  Ground815/TrailEdges pieces within scopeOff m of the four burn trails lose their colliders; CS_Stone_3 (261.40, 173.30) moves square
//     off the Jg to Camp 1 line to stoneOff m.
// B8  verge frame: every Forest/BurnDeadwood and SliceLook/BurnRegrowth piece within vergeClear m in plan of the deck-to-verge line, x 250
//     to 345, moves square off the line to vergeClear plus moveMargin. Office line: every such piece within officeClear m of the deck-to-
//     office-door line (lowest deck eye to (344, 4.3, 199)), x 230 to 340, whose top stands over the cap (the line less capDrop) moves square
//     off it to officeClear plus moveMargin. A move lands only off every trail by trailClear m, outside every keep-out zone and on the
//     terrain; it tries the near side, then the far side; a piece with no spot is listed (not removed).
// FB  forage B: two RedFir8 at (143.0, 166.0) and (143.0, 168.3), firBTall m, trunk capsules; its bushes get `Forage` as A's.
// FC  forage C (8.24's Places/ForageC): each shrub's capsule becomes a sphere forageCR round (gaps 0.3 or less on the r 1.0 ring).
// W   warps: Junction_Jg faces 40; Old_Burn faces 5 (its label and row: main3_8_28_warplabels.patch).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Vector2 XZ(UnityEngine.Vector3 v) => new UnityEngine.Vector2(v.x, v.z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
float G(float x, float z) => kit.H(x, z);
var notes = new System.Collections.Generic.List<string>(); var report = new System.Collections.Generic.List<string>();
UnityEngine.Transform RootT(string n) { var r = kit.Root(n); return r != null ? r.transform : null; }
var places = RootT("Places"); var poi = RootT("PointsOfInterest"); var slice = RootT("SliceLook"); var forest = RootT("Forest"); var ground815 = RootT("Ground815"); var giants = RootT("Giants"); var trails = RootT("Trails"); var warps = RootT("DevWarps");
if (places == null || poi == null || slice == null || forest == null || ground815 == null || giants == null || trails == null || warps == null) return "missing a root (Places, PointsOfInterest, SliceLook, Forest, Ground815, Giants, Trails or DevWarps): run the rebuild first";
var L = kit.Fresh("Burn828", places, UnityEngine.Vector3.zero, 0f);   // at the world origin, so local places are world places
int uses = 0; void Use(UnityEngine.GameObject g, string prompt, UnityEngine.Renderer target) { if (g == null) return; var u = g.GetComponent<ToggleColorInteractable>() ?? g.AddComponent<ToggleColorInteractable>(); var so = new UnityEditor.SerializedObject(u); so.FindProperty("prompt").stringValue = prompt; so.FindProperty("target").objectReferenceValue = target != null ? target : g.GetComponentInChildren<UnityEngine.Renderer>(); so.ApplyModifiedPropertiesWithoutUndo(); uses++; }
const float removeTol = 0.3f;
UnityEngine.Transform Near(UnityEngine.Transform parent, string name, float x, float z, float tol) { UnityEngine.Transform best = null; float bd = tol; if (parent != null) foreach (UnityEngine.Transform t in parent) { if (t.name != name) continue; float d = UnityEngine.Vector2.Distance(XZ(t.position), P(x, z)); if (d <= bd) { bd = d; best = t; } } return best; }
// the trails: their scene markers in order
System.Collections.Generic.List<UnityEngine.Vector2> Line(string leg) { var l = new System.Collections.Generic.List<UnityEngine.Vector2>(); var t = trails.Find(leg); if (t != null) foreach (UnityEngine.Transform p in t) l.Add(XZ(p.position)); return l; }
UnityEngine.Vector2 FootOn(System.Collections.Generic.List<UnityEngine.Vector2> l, UnityEngine.Vector2 q) { var best = l.Count > 0 ? l[0] : q; float bd = float.MaxValue; for (int i = 1; i < l.Count; i++) { var ab = l[i] - l[i - 1]; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - l[i - 1], ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); var f = l[i - 1] + ab * t; float d = UnityEngine.Vector2.Distance(q, f); if (d < bd) { bd = d; best = f; } } return best; }
float DistTo(System.Collections.Generic.List<UnityEngine.Vector2> l, UnityEngine.Vector2 q) => l.Count == 0 ? float.MaxValue : UnityEngine.Vector2.Distance(q, FootOn(l, q));
var burnLegs = new[] { "Camp to Jg", "Jg to T", "Jg to Camp 1", "Camp 2 to T" }; var allLegs = new System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>();
foreach (UnityEngine.Transform leg in trails) allLegs.Add(Line(leg.name));
foreach (var n in burnLegs) if (Line(n).Count < 2) notes.Add("no Trails/" + n);
var campToJg = Line("Camp to Jg"); var jgToT = Line("Jg to T"); var jgToC1 = Line("Jg to Camp 1");
float TrailD(UnityEngine.Vector2 q) { float d = float.MaxValue; foreach (var l in allLegs) d = UnityEngine.Mathf.Min(d, DistTo(l, q)); return d; }
// the drawable pieces under a root: a node with a LOD group or a renderer of its own is one piece; empty groups are walked into
System.Collections.Generic.List<UnityEngine.Transform> Pieces(UnityEngine.Transform root) { var o = new System.Collections.Generic.List<UnityEngine.Transform>(); void Walk(UnityEngine.Transform t) { foreach (UnityEngine.Transform c in t) { if (c.GetComponent<UnityEngine.LODGroup>() != null || c.GetComponent<UnityEngine.Renderer>() != null) o.Add(c); else Walk(c); } } if (root != null) Walk(root); return o; }
var treeName = new System.Text.RegularExpressions.Regex("RedFir|RedPine|Sequoia|Tree_Dead|Sapling");
float TopOver(UnityEngine.Transform t) { var b = PlaceKit.MeshBounds(t.gameObject); return b.max.y - G(t.position.x, t.position.z); }
// a piece moved in plan, its foot kept at the same height over the ground
void MoveTo(UnityEngine.Transform t, UnityEngine.Vector2 q) { float lift = t.position.y - G(t.position.x, t.position.z); t.position = V(q.x, G(q.x, q.y) + lift, q.y); }
const float trunkBandLow = 0.01f, trunkBandHigh = 0.06f, trunkBandMax = 0.4f, trunkShare = 0.3f; const int trunkMinVerts = 8;   // 8.16a's pack trunk numbers
const float snagTrunkBand = 0.15f, snagTrunkShare = 0.4f;   // 8.16's burn snag numbers
UnityEngine.GameObject PackTree(string prefab, UnityEngine.Transform parent, float x, float z, float yaw, float tall)
{
    var g = kit.Ground(PlaceKit.BK + "Trees/" + prefab, parent, x, z, yaw, 1f, false); if (g == null) return null;
    float h0 = PlaceKit.MeshBounds(g).size.y; g.transform.localScale = UnityEngine.Vector3.one * (tall / UnityEngine.Mathf.Max(0.5f, h0));
    var b = PlaceKit.MeshBounds(g); g.transform.position += V(0f, G(x, z) - b.min.y - 0.1f, 0f);
    PlaceKit.PackTrunkCapsule(g, trunkBandLow, trunkBandHigh, trunkBandMax, trunkMinVerts, trunkShare); return g;
}
UnityEngine.Transform PackAt(UnityEngine.Transform gray) { var gt = slice.Find("GiantTrees"); if (gt == null || gray == null) return null; foreach (UnityEngine.Transform t in gt) if ((XZ(t.position) - XZ(gray.position)).sqrMagnitude < 0.01f) return t; return null; }
var charred = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_RoofChar.mat"); if (charred == null) notes.Add("no Slice_RoofChar.mat");
var plank = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat"); if (plank == null) notes.Add("no Planks023A_1.0x1.0.mat");

// ================= B1: forage A =================
const float clearOver = 0.5f, clearHalf = 0.6f, screenTall = 9f, screenX = 235.8f;
{
    var a = poi.Find("POI_Forage_patch_A");
    if (a == null) notes.Add("no PointsOfInterest/POI_Forage_patch_A");
    else
    {
        var bushes = new System.Collections.Generic.List<UnityEngine.Transform>();
        foreach (UnityEngine.Transform b in a) { if (b.name != "Bush") continue; bushes.Add(b); var c = b.GetComponent<UnityEngine.Collider>(); if (c == null || c.isTrigger) { if (c != null) UnityEngine.Object.DestroyImmediate(c); b.gameObject.AddComponent<UnityEngine.SphereCollider>(); } Use(b.gameObject, "Forage", b.GetComponent<UnityEngine.Renderer>()); }
        if (bushes.Count != 5) notes.Add("forage A has " + bushes.Count + " bushes (5 built)");
        // keep clear: the two bushes nearest the centreline, a clearHalf band from each to its foot on the line
        bushes.Sort((p, q) => DistTo(campToJg, XZ(p.position)).CompareTo(DistTo(campToJg, XZ(q.position))));
        int cleared = 0;
        foreach (var root in new[] { forest, slice })
            foreach (var t in Pieces(root))
            {
                if (t == null || t.IsChildOf(L) || (slice.Find("GiantTrees") != null && t.IsChildOf(slice.Find("GiantTrees")))) continue;
                var q = XZ(PlaceKit.MeshBounds(t.gameObject).center); bool inBand = false;
                for (int i = 0; i < UnityEngine.Mathf.Min(2, bushes.Count); i++) { var bq = XZ(bushes[i].position); var f = FootOn(campToJg, bq); var ab = bq - f; float u = UnityEngine.Vector2.Dot(q - f, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude); if (u >= 0f && u <= 1f && UnityEngine.Vector2.Distance(q, f + ab * u) <= clearHalf) inBand = true; }
                if (inBand && TopOver(t) > clearOver) { report.Add("forage A keep clear: " + t.name + " at (" + F(t.position.x) + ", " + F(t.position.z) + ") removed"); PlaceKit.Remove(t); cleared++; }
            }
        report.Add("forage A keep clear: " + cleared + " removed");
    }
    var shot = slice.Find("ShotGround"); var log = Near(shot, "CS_Log_Firewood", 240.21f, 161.01f, removeTol); if (log != null) { PlaceKit.Remove(log); report.Add("forage A: R CS_Log_Firewood"); }
    kit.Marker("ForageA_Stand", L, V(241.6f, G(241.6f, 160.6f), 160.6f), 301f);
    var screen = kit.Group("ForageA_Screen", L, UnityEngine.Vector3.zero, 0f); int k = 0;
    foreach (var z in new[] { 161.9f, 163.15f, 164.4f }) { var f = PackTree("RedFir5", screen, screenX, z, k * 120f, screenTall); if (f == null) notes.Add("no RedFir5"); k++; }
}

// ================= B2: the Hollow Giant =================
const float hollowW = 1.2f, hollowH = 2.0f, hollowD = 0.6f, hollowFacing = 28f, hollowRayUp = 1.0f, hollowRayFrom = 12f, jambT = 0.18f, lintelT = 0.25f, recessOut = 0.1f, panelT = 0.04f;
{
    var hg = giants.Find("Heroes/Hollow_Giant"); var pack = PackAt(hg);
    if (hg == null || pack == null) notes.Add("no Giants/Heroes/Hollow_Giant or its pack tree under SliceLook/GiantTrees");
    else
    {
        int gone = 0; foreach (var c in hg.GetComponentsInChildren<UnityEngine.Collider>()) { UnityEngine.Object.DestroyImmediate(c); gone++; }
        PlaceKit.PackTrunkCapsule(pack.gameObject, trunkBandLow, trunkBandHigh, trunkBandMax, trunkMinVerts, trunkShare);
        var cap = pack.GetComponentInChildren<UnityEngine.CapsuleCollider>(); UnityEngine.Physics.SyncTransforms();
        if (cap == null) notes.Add("Hollow Giant: no trunk capsule fitted");
        else
        {
            var axis = XZ(cap.bounds.center); var dir = P(UnityEngine.Mathf.Sin(hollowFacing * UnityEngine.Mathf.Deg2Rad), UnityEngine.Mathf.Cos(hollowFacing * UnityEngine.Mathf.Deg2Rad));
            var from2 = axis + dir * hollowRayFrom;
            var from = V(from2.x, G(axis.x + dir.x * 2f, axis.y + dir.y * 2f) + hollowRayUp, from2.y);
            if (!cap.Raycast(new UnityEngine.Ray(from, V(-dir.x, 0f, -dir.y)), out var hit, hollowRayFrom * 2f)) notes.Add("Hollow Giant: the bearing-28 ray missed the capsule");
            else
            {
                var s = XZ(hit.point); float gy = G(s.x, s.y);
                var h = kit.Group("Hollow", L, V(s.x, gy, s.y), hollowFacing);   // local +z out of the bark
                var dark = kit.Tinted("Places_HollowDark", "Assets/Materials/Slice/Slice_RoofChar.mat", Hex("#0B0A09"), UnityEngine.Vector2.one);
                kit.Slab("Recess", h, V(0f, hollowH * 0.5f, recessOut), V(hollowW, hollowH, panelT), dark);
                foreach (var sx in new[] { -1f, 1f }) kit.Slab("Jamb", h, V(sx * (hollowW + jambT) * 0.5f, hollowH * 0.5f, recessOut + hollowD * 0.5f), V(jambT, hollowH, hollowD), charred, default, true);
                kit.Slab("Lintel", h, V(0f, hollowH + lintelT * 0.5f, recessOut + hollowD * 0.5f), V(hollowW + 2f * jambT, lintelT, hollowD), charred, default, true);
                kit.Marker("Hollow_Inspect", L, V(203.9f, G(203.9f, 143.6f), 143.6f), 208f);
                report.Add("hollow: gray colliders removed " + gone + ", capsule r " + F(cap.radius * cap.transform.lossyScale.x) + ", bark at (" + F(s.x) + ", " + F(s.y) + ") ground " + F(gy) + ", " + F(UnityEngine.Vector2.Distance(s, P(203.2f, 142.3f))) + " m from the doc's (203.2, 142.3)");
            }
        }
    }
}

// ================= B3: the Jg signpost =================
const float jgOpen = 4f; var jg = P(262f, 172f);
{
    var sign = ground815.Find("JunctionMarkers/Sign_Jg");
    if (sign == null) notes.Add("no Ground815/JunctionMarkers/Sign_Jg");
    else { var to = P(263.8f, 170.2f); var d = V(to.x - sign.position.x, G(to.x, to.y) - G(sign.position.x, sign.position.z), to.y - sign.position.z); sign.position += d; }
    var h7 = ground815.Find("Stops/Hedge_Burn_7");
    if (h7 == null) report.Add("no Ground815/Stops/Hedge_Burn_7");
    else foreach (var c in h7.GetComponentsInChildren<UnityEngine.BoxCollider>()) { var cp = c.ClosestPoint(V(jg.x, c.bounds.center.y, jg.y)); float d = UnityEngine.Vector2.Distance(XZ(cp), jg); if (d < jgOpen) notes.Add("Hedge_Burn_7 box at (" + F(c.bounds.center.x) + ", " + F(c.bounds.center.z) + ") reaches " + F(d) + " m from Jg (" + F(jgOpen) + " kept open): Sable's call"); }
}

// ================= B4: the trailhead board at T =================
const float armLen = 1.5f, armH = 0.3f, armT = 0.05f, armTopDrop = 0.2f, armGap = 0.4f;
{
    var tb = ground815.Find("JunctionMarkers/Trailhead_Board");
    if (tb == null) notes.Add("no Ground815/JunctionMarkers/Trailhead_Board");
    else
    {
        UnityEngine.Transform north = null; foreach (UnityEngine.Transform c in tb) if (c.name == "Post" && (north == null || c.position.z > north.position.z)) north = c;
        if (north == null) notes.Add("Trailhead_Board: no Post");
        else
        {
            var pb = north.GetComponent<UnityEngine.Renderer>().bounds; var arms = kit.Group("T_Arms", L, V(pb.center.x, pb.min.y, pb.center.z), 0f); int i = 0;
            foreach (var (text, bearing) in new[] { ("CAMP", 260.4f), ("CAMP 2", 184.4f) })
            {
                var d = P(UnityEngine.Mathf.Sin(bearing * UnityEngine.Mathf.Deg2Rad), UnityEngine.Mathf.Cos(bearing * UnityEngine.Mathf.Deg2Rad));
                var arm = kit.Slab("Arm_" + text, arms, V(d.x * armLen * 0.5f, pb.size.y - armTopDrop - i * armGap, d.y * armLen * 0.5f), V(armLen, armH, armT), plank, V(0f, bearing + 90f, 0f));
                kit.Label(arm.transform, text, new UnityEngine.Color(0.95f, 0.9f, 0.8f)); kit.Label(arm.transform, text, new UnityEngine.Color(0.95f, 0.9f, 0.8f), 24, true); i++;
            }
        }
        var board = tb.Find("Board");
        if (board == null) notes.Add("Trailhead_Board: no Board");
        else { if (board.GetComponent<UnityEngine.Collider>() == null) board.gameObject.AddComponent<UnityEngine.BoxCollider>(); Use(board.gameObject, "Examine", board.GetComponent<UnityEngine.Renderer>()); }
    }
}

// ================= B5: the Gate Tree stub =================
{
    var gt = giants.Find("Heroes/Gate_Tree"); var dead = PackAt(gt);
    if (gt == null || dead == null) notes.Add("no Giants/Heroes/Gate_Tree or its drawn Tree_Dead under SliceLook/GiantTrees");
    else
    {
        int gone = 0; foreach (var c in gt.GetComponentsInChildren<UnityEngine.Collider>()) { UnityEngine.Object.DestroyImmediate(c); gone++; }
        PlaceKit.DeadTrunkCapsule(dead.gameObject, snagTrunkBand, snagTrunkShare); var cap = dead.GetComponentInChildren<UnityEngine.CapsuleCollider>(); UnityEngine.Physics.SyncTransforms();
        if (cap == null) notes.Add("Gate Tree: no trunk capsule fitted");
        else report.Add("Gate Tree: gray colliders removed " + gone + ", capsule centre (" + F(cap.bounds.center.x) + ", " + F(cap.bounds.center.z) + "), half width " + F(cap.bounds.extents.x) + ", " + F(DistTo(jgToT, XZ(cap.bounds.center)) - cap.bounds.extents.x) + " m from the Jg to T centreline to its bark");
    }
}

// ================= B6: the firebreak =================
const float fbX0 = 205f, fbX1 = 211f, fbZ1 = 200f, corridorHalf = 3.5f, stumpStep = 3f, stumpTall = 0.45f, stumpJitter = 0.5f;
{
    float CorridorZ(float x) { float best = float.MinValue; for (float z = 120f; z < 170f; z += 0.25f) if (DistTo(campToJg, P(x, z)) <= corridorHalf) best = UnityEngine.Mathf.Max(best, z); return best; }
    var zEdge = new System.Collections.Generic.Dictionary<float, float>(); foreach (var x in new[] { fbX0, (fbX0 + fbX1) * 0.5f, fbX1 }) zEdge[x] = CorridorZ(x);
    float z0 = float.MinValue; foreach (var v in zEdge.Values) z0 = UnityEngine.Mathf.Max(z0, v);
    if (z0 == float.MinValue) notes.Add("firebreak: the Camp to Jg corridor is not under x 205 to 211");
    else
    {
        int cut = 0;
        foreach (var root in new[] { forest, slice })
            foreach (var t in Pieces(root))
            {
                if (t == null || !treeName.IsMatch(t.name) || t.IsChildOf(L) || (slice.Find("GiantTrees") != null && t.IsChildOf(slice.Find("GiantTrees")))) continue;
                var q = XZ(t.position); if (q.x < fbX0 || q.x > fbX1 || q.y < z0 || q.y > fbZ1) continue; PlaceKit.Remove(t); cut++;
            }
        var fb = kit.Group("Firebreak", L, UnityEngine.Vector3.zero, 0f); var rng = new System.Random(828); int stumps = 0;
        float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
        for (float x = fbX0 + 1f; x <= fbX1 - 1f + 1e-3f; x += (fbX1 - fbX0 - 2f) * 0.5f)
            for (float z = z0 + stumpStep * 0.5f; z < fbZ1; z += stumpStep)
            {
                var s = kit.Ground(PlaceKit.CI + "Vegetation/CITW_Tree_Stump", fb, x + R(-stumpJitter, stumpJitter), z + R(-stumpJitter, stumpJitter), R(0f, 360f), 1f, false); if (s == null) { notes.Add("no CITW_Tree_Stump"); break; }
                PlaceKit.StripColliders(s); float h0 = PlaceKit.MeshBounds(s).size.y; s.transform.localScale = UnityEngine.Vector3.one * (stumpTall / UnityEngine.Mathf.Max(0.05f, h0));
                var b = PlaceKit.MeshBounds(s); s.transform.position += V(0f, G(s.transform.position.x, s.transform.position.z) - b.min.y - 0.02f, 0f); stumps++;
            }
        report.Add("firebreak: z " + F(z0) + " to " + F(fbZ1) + ", trees cut " + cut + ", stumps " + stumps);
    }
}

// ================= B7: trail-edge stones =================
const float scopeOff = 8f, stoneOff = 1.4f;
{
    var edges = ground815.Find("TrailEdges"); int stripped = 0;
    if (edges == null) notes.Add("no Ground815/TrailEdges");
    else
    {
        foreach (UnityEngine.Transform t in edges) { var q = XZ(t.position); bool near = false; foreach (var n in burnLegs) if (DistTo(Line(n), q) <= scopeOff) near = true; if (!near || t.GetComponentInChildren<UnityEngine.Collider>() == null) continue; PlaceKit.StripColliders(t.gameObject); stripped++; }
        var s3 = Near(edges, "CS_Stone_3", 261.40f, 173.30f, removeTol) ?? Near(edges, "CS_Stone_3", 261.40f, 173.30f, stoneOff + removeTol);
        if (s3 == null) notes.Add("no TrailEdges/CS_Stone_3 near (261.40, 173.30)");
        else { var q = XZ(s3.position); var f = FootOn(jgToC1, q); var d = q - f; if (d.sqrMagnitude < 1e-4f) d = P(1f, 0f); MoveTo(s3, f + d.normalized * stoneOff); }
        report.Add("trail-edge stones: colliders removed on " + stripped);
    }
}

// ================= B8: the deck lines through the burn =================
const float vergeClear = 15f, officeClear = 5f, moveMargin = 0.5f, capDrop = 2f, trailClear = 3.5f, vergeX0 = 250f, vergeX1 = 345f, officeX0 = 230f, officeX1 = 340f;
{
    var set = Main3AreaSet.Load(); var tower = kit.Root("Camp").transform.Find("Tower"); var cab = tower.Find("Cab");
    var deck = XZ(tower.position); float eyeLow = cab.position.y + (set != null ? set.deckEye : 1.6f);
    var verge = P(419f, 139f); var door = V(344f, 4.3f, 199f);
    var terrain = UnityEngine.Terrain.activeTerrain; var tp = terrain.transform.position; var ts = terrain.terrainData.size;
    bool OnTerrain(UnityEngine.Vector2 q) => q.x > tp.x + 1f && q.x < tp.x + ts.x - 1f && q.y > tp.z + 1f && q.y < tp.z + ts.z - 1f;
    bool SpotOk(UnityEngine.Vector2 q) => OnTerrain(q) && TrailD(q) >= trailClear && KeepOuts.Which(q) == null;
    float LineD(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b, out UnityEngine.Vector2 side) { var ab = (b - a).normalized; var n = P(-ab.y, ab.x); float s = UnityEngine.Vector2.Dot(q - a, n); side = s >= 0f ? n : -n; return UnityEngine.Mathf.Abs(s); }
    // a move square off the line a to b to want m: the near side first, then the far side
    string Shift(UnityEngine.Transform t, UnityEngine.Vector2 a, UnityEngine.Vector2 b, float want)
    {
        var q = XZ(t.position); float d = LineD(q, a, b, out var side); var foot = q - side * d;
        foreach (var sgn in new[] { 1f, -1f }) { var to = foot + side * (sgn * want); if (!SpotOk(to)) continue; MoveTo(t, to); return "to (" + F(to.x) + ", " + F(to.y) + ")"; }
        return null;
    }
    var pieces = new System.Collections.Generic.List<UnityEngine.Transform>(); var bd = forest.Find("BurnDeadwood"); var br = slice.Find("BurnRegrowth");
    if (bd == null || br == null) notes.Add("no Forest/BurnDeadwood or SliceLook/BurnRegrowth");
    foreach (var root in new[] { bd, br }) if (root != null) foreach (UnityEngine.Transform t in root) pieces.Add(t);
    int vMoved = 0, oMoved = 0; var stuck = new System.Collections.Generic.List<string>();
    foreach (var t in pieces)
    {
        var q = XZ(t.position);
        if (q.x >= vergeX0 && q.x <= vergeX1 && LineD(q, deck, verge, out _) < vergeClear)
        { var m = Shift(t, deck, verge, vergeClear + moveMargin); if (m != null) { vMoved++; report.Add("verge: " + t.parent.name + "/" + t.name + " (" + F(q.x) + ", " + F(q.y) + ") " + m); } else stuck.Add(t.name + " (" + F(q.x) + ", " + F(q.y) + ") verge"); q = XZ(t.position); }
        if (q.x >= officeX0 && q.x <= officeX1 && LineD(q, deck, XZ(door), out _) < officeClear)
        {
            float u = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - deck, XZ(door) - deck) / (XZ(door) - deck).sqrMagnitude); float cap = UnityEngine.Mathf.Lerp(eyeLow, door.y, u) - capDrop;
            if (PlaceKit.MeshBounds(t.gameObject).max.y <= cap) continue;
            var m = Shift(t, deck, XZ(door), officeClear + moveMargin); if (m != null) { oMoved++; report.Add("office: " + t.parent.name + "/" + t.name + " (" + F(q.x) + ", " + F(q.y) + ") " + m); } else stuck.Add(t.name + " (" + F(q.x) + ", " + F(q.y) + ") office");
        }
    }
    report.Add("deck lines: verge moves " + vMoved + ", office moves " + oMoved);
    if (stuck.Count > 0) notes.Add("deck lines, no spot for: " + string.Join("; ", stuck));
}

// ================= FB, FC: forage B and C =================
const float firBTall = 10f, forageCR = 0.45f;
{
    var b = poi.Find("POI_Forage_patch_B");
    if (b == null) notes.Add("no PointsOfInterest/POI_Forage_patch_B");
    else foreach (UnityEngine.Transform s in b) { if (s.name != "Bush") continue; var c = s.GetComponent<UnityEngine.Collider>(); if (c == null || c.isTrigger) { if (c != null) UnityEngine.Object.DestroyImmediate(c); s.gameObject.AddComponent<UnityEngine.SphereCollider>(); } Use(s.gameObject, "Forage", s.GetComponent<UnityEngine.Renderer>()); }
    var firs = kit.Group("ForageB_Firs", L, UnityEngine.Vector3.zero, 0f);
    foreach (var (x, z, yaw) in new[] { (143.0f, 166.0f, 30f), (143.0f, 168.3f, 200f) }) if (PackTree("RedFir8", firs, x, z, yaw, firBTall) == null) notes.Add("no RedFir8");
    var fc = places.Find("ForageC");
    if (fc == null) notes.Add("no Places/ForageC");
    else foreach (UnityEngine.Transform s in fc)
    {
        if (!s.name.StartsWith("Bush_ForageC_")) continue;
        foreach (var c in s.GetComponents<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
        var mb = PlaceKit.MeshBounds(s.gameObject); var sc = s.gameObject.AddComponent<UnityEngine.SphereCollider>(); sc.center = s.InverseTransformPoint(mb.center); sc.radius = forageCR / UnityEngine.Mathf.Max(1e-3f, s.lossyScale.x);
    }
}

// ================= W: warps =================
foreach (var (n, yaw) in new[] { ("Junction_Jg", 40f), ("Old_Burn", 5f) }) { var w = warps.Find(n); if (w == null) notes.Add("no DevWarps/" + n); else w.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); }

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " | usables " + uses + " | " + string.Join(" | ", report) + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
