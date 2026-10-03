// Main3 8.27 cave check (Play mode, Main3; CaveLayout.md draft 2). In main3_review_capture.sh --area cave's Play checks; also runs alone.
// Never saves; restores the player, DeeperClosed and the temporary colliders. Every move is PlayerController.Step (dt 0.02).
// RAIL (V1): every post railOff m (within railTol) south of the W1 to cave centre line, its foot within footTol of the tread's height.
// MOUTH STRIP (doc 2.2): nothing drawn stands more than stripTop over the ground in x 52 to 54.5, z 38 to 46.5 (the overhang above
//   the opening apart).
// NICHE (V3): the body stands inside it; the ground over its ceiling is coverMin m or more above it.
// CABLE (V3): no collider and no light under Layout827/Cable.
// TOILET (V4): the lid's top lidMax over the ground, the shovel's top shovelMax; the lid unseen from every W1 to cave point P40 to P84
//   (eye 1.6, terrain and every collider).
// KEEP CLEAR (V5, V6): nothing drawn over clearTop in the chamber strip x 71 to 89.25, z 10.8 to 13.2, or the side-room strip x 89.25 to
//   92.4, z 11 to 13.
// TALK (V5): R7's spot (its head headUp over the shelf) within reach of the talk stand's eye, and within talkCone degrees of its 98 heading.
// SIDE ROOM (V6): his chair at world yaw 270; the crate flush to the west and north walls; BigBoulders_0 east of the chamber wall; the guest
//   chair the first hit of the interactor's ray from (91.0, 12.0) facing 90, pitch 15 down.
// DEEPER (V9): shut by DeeperClosed; with it lifted for the test, the body walks from the standing point to the inspect point (101.0, 14.0).
//   PROMPT lines (CaveLayout_Story 23): no prompt at the shut rock; open, none at the opening and `Examine` at the dead end facing 0.
// NARROW (V10): the row's rock colliders all narrowMin m or more from the tread's centre line, and no gap between neighbours wider than
//   gapMax (continuous).
// RIM (V11): from eyes on the band's north side (every rimStep m in x, eye 1.6), no clear line to the mouth's board, void edges or floor
//   past the terrain and the band's brush (temporary exact colliders); a walk south from each eye stays north of the band.
// WALKS (doc 4): W1 to the mouth along the trail; the mouth to the chamber down the legs; the passage end to the doorway, the guest chair
//   stand and the seat stand. Each arrives within arrive m, with times.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv); string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
const float dt = 0.02f, arrive = 0.5f, legTime = 200f, eyeH = 1.6f, floorY = -18f;
const float railOff = 1.2f, railTol = 0.25f, footTol = 0.2f, stripTop = 0.3f, coverMin = 3.5f, lidMax = 0.1f, shovelMax = 0.3f, clearTop = 0.3f, headUp = 1.5f, talkCone = 10f, narrowMin = 1.4f, gapMax = 0.6f, rimStep = 2f;
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
var cave = Root("Cave") != null ? Root("Cave").transform : null; var L = cave != null ? cave.Find("Layout827") : null; var poiRoot = Root("PointsOfInterest").transform; var trails = Root("Trails").transform;
UnityEngine.Transform closed = null; bool closedWas = true;
float SegDist(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
bool Walk(UnityEngine.Vector3 to, ref float time, out float left)
{
    for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = V(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, false, false, dt); time += dt; }
    var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
}
// the drawn renderers with no collider of their own that a set of rays crosses get temporary exact colliders
void TempColliders(System.Collections.Generic.IEnumerable<(UnityEngine.Vector3 a, UnityEngine.Vector3 b)> rays)
{
    var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
    var list = System.Linq.Enumerable.ToList(rays);
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(pc.transform)) continue;
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; bool on = false;
        foreach (var (a, b) in list) { var d = b - a; if (mr.bounds.IntersectRay(new UnityEngine.Ray(a, d.normalized), out float dist) && dist <= d.magnitude) { on = true; break; } }
        if (!on) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
}
void DropTemps() { foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms(); }
bool ClearLine(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform own) { var d = b - a; foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { var t = h.collider.transform; if (t.IsChildOf(pc.transform) || (own != null && t.IsChildOf(own))) continue; return false; } return true; }
// the drawn things standing in a strip more than top m over the ground (or the floor at floorAt) there
System.Collections.Generic.List<string> Tall(UnityEngine.Rect strip, float top, float floorAt, System.Func<UnityEngine.Renderer, bool> skip)
{
    var o = new System.Collections.Generic.List<string>();
    foreach (var r in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!r.enabled || !r.gameObject.activeInHierarchy || r.transform.IsChildOf(pc.transform) || (skip != null && skip(r))) continue; var b = r.bounds;
        if (b.max.x < strip.xMin || b.min.x > strip.xMax || b.max.z < strip.yMin || b.min.z > strip.yMax) continue;
        float g = float.IsNaN(floorAt) ? H(b.center.x, b.center.z) : floorAt; if (b.max.y - g <= top || b.min.y - g > 2.0f) continue;   // over 2 m up is overhead
        o.Add(WalkIns.PathOf(r.transform) + " " + F(b.max.y - g) + " m");
    }
    return o;
}
var tread = new System.Collections.Generic.List<(string name, UnityEngine.Vector3 p)>(); var leg = trails.Find("W1 to cave"); if (leg != null) foreach (UnityEngine.Transform p in leg) tread.Add((p.name, p.position));
float TreadDist(UnityEngine.Vector2 q, out float y) { float best = float.MaxValue; y = 0f; for (int i = 1; i < tread.Count; i++) { float d = SegDist(q, P(tread[i - 1].p.x, tread[i - 1].p.z), P(tread[i].p.x, tread[i].p.z)); if (d < best) { best = d; y = (tread[i - 1].p.y + tread[i].p.y) * 0.5f; } } return best; }
try
{
    if (L == null || leg == null) return "no Cave/Layout827 or Trails/W1 to cave (run main3_8_27_cave.cs)";
    // ---- RAIL
    {
        var rail = poiRoot.Find("RopeRail827"); var bad = new System.Collections.Generic.List<string>(); int n = 0;
        if (rail != null) foreach (UnityEngine.Transform t in rail) { if (t.name != "Post") continue; n++; var b = t.GetComponent<UnityEngine.Collider>().bounds; float d = TreadDist(P(b.center.x, b.center.z), out float ty); if (UnityEngine.Mathf.Abs(d - railOff) > railTol || UnityEngine.Mathf.Abs(b.min.y - ty) > footTol + 0.5f) bad.Add("(" + F1(b.center.x) + ", " + F1(b.center.z) + ") " + F(d) + " m off, foot " + F(b.min.y - ty)); }
        Line(n > 0 && bad.Count == 0, "RAIL: " + n + " posts " + F1(railOff) + " m (within " + F(railTol) + ") south of the centre line" + (bad.Count > 0 ? "; off: " + string.Join("; ", bad) : ""));
    }
    // ---- MOUTH STRIP
    { var o = Tall(new UnityEngine.Rect(52f, 38f, 2.5f, 8.5f), stripTop, float.NaN, r => r.bounds.min.y > -3.9f); Line(o.Count == 0, "MOUTH STRIP: drawn things over " + F1(stripTop) + " m in x 52 to 54.5, z 38 to 46.5: " + (o.Count == 0 ? "none" : string.Join(", ", o))); }
    // ---- NICHE
    {
        var niche = L.Find("Niche"); if (niche == null) Line(false, "NICHE: no Layout827/Niche");
        else
        {
            var at = V(54.0f, -6f, 32.5f); float r = cc.radius; bool clear = true; string hit = "";
            foreach (var c in UnityEngine.Physics.OverlapCapsule(at + V(0f, r + 0.05f, 0f), at + V(0f, UnityEngine.Mathf.Min(cc.height, 1.95f) - r, 0f), r * 0.9f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { if (c.transform.IsChildOf(pc.transform) || c.name == "Generator" || c is UnityEngine.TerrainCollider) continue; clear = false; hit = WalkIns.PathOf(c.transform); }
            float cover = float.MaxValue; for (float x = 53.5f; x <= 55.5f; x += 0.5f) for (float z = 31.6f; z <= 33.4f; z += 0.45f) cover = UnityEngine.Mathf.Min(cover, H(x, z) - (-4f + 0.5f));
            Line(clear && cover >= coverMin - 1e-3f, "NICHE: the body " + (clear ? "stands in it" : "is blocked by " + hit) + "; ground over its ceiling rock at least " + F(cover) + " m (" + F1(coverMin) + ")");
        }
    }
    // ---- CABLE
    { var cable = L.Find("Cable"); int cols = cable != null ? cable.GetComponentsInChildren<UnityEngine.Collider>().Length : -1, lights = cable != null ? cable.GetComponentsInChildren<UnityEngine.Light>().Length : -1; Line(cable != null && cols == 0 && lights == 0, "CABLE: " + (cable == null ? "missing" : cable.childCount + " runs, colliders " + cols + ", lights " + lights)); }
    // ---- TOILET
    {
        var toilet = L.Find("Toilet"); var lid = toilet != null ? toilet.Find("PitLid") : null; UnityEngine.Transform shovel = null; if (toilet != null) foreach (UnityEngine.Transform t in toilet) if (t.name.StartsWith("Shovel")) shovel = t;
        if (lid == null || shovel == null) Line(false, "TOILET: no lid or shovel");
        else
        {
            var lb = PlaceKit.MeshBounds(lid.gameObject); var sbd = PlaceKit.MeshBounds(shovel.gameObject); float lidTop = lb.max.y - H(lb.center.x, lb.center.z), shTop = sbd.max.y - H(sbd.center.x, sbd.center.z);
            var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (var (n, p) in tread) { int k; if (n.StartsWith("P") && int.TryParse(n.Substring(1), out k) && k >= 40 && k <= 84) eyes.Add(p + V(0f, eyeH, 0f)); }
            var aim = V(lb.center.x, lb.max.y, lb.center.z); var rays = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3)>(); foreach (var e in eyes) rays.Add((e, aim)); TempColliders(rays);
            int seen = 0; foreach (var e in eyes) if (ClearLine(e, aim, toilet)) seen++; DropTemps();
            Line(lidTop <= lidMax + 0.02f && shTop <= shovelMax && seen == 0, "TOILET: lid " + F(lidTop) + " m (" + F(lidMax) + "), shovel " + F(shTop) + " m (" + F(shovelMax) + "); the lid seen from " + seen + " of " + eyes.Count + " tread points P40 to P84");
        }
    }
    // ---- KEEP CLEAR
    {
        var a = Tall(new UnityEngine.Rect(71f, 10.8f, 18.25f, 2.4f), clearTop, floorY, r => r.bounds.min.y > floorY + 2.0f); var b = Tall(new UnityEngine.Rect(89.25f, 11f, 3.15f, 2f), clearTop, floorY, r => r.bounds.min.y > floorY + 2.0f);
        Line(a.Count == 0 && b.Count == 0, "KEEP CLEAR: chamber strip " + (a.Count == 0 ? "clear" : string.Join(", ", a)) + "; side-room strip " + (b.Count == 0 ? "clear" : string.Join(", ", b)));
    }
    // ---- TALK
    {
        var spot = cave.Find("Resident_Cave_Spot"); var reach = tuning != null ? tuning.interactReach : 2f;
        if (spot == null) Line(false, "TALK: no Resident_Cave_Spot");
        else { var eye = V(86.9f, floorY + eyeH, 6.2f); var head = spot.position + V(0f, headUp - 0.6f, 0f); var d = head - eye; float dist = d.magnitude, brg = (UnityEngine.Mathf.Atan2(d.x, d.z) * UnityEngine.Mathf.Rad2Deg + 360f) % 360f; Line(dist <= reach && UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(brg, 98f)) <= talkCone, "TALK: R7's head " + F(dist) + " m from the talk stand's eye (reach " + F1(reach) + "), bearing " + F1(brg) + " (98, within " + F1(talkCone) + ")"); }
    }
    // ---- SIDE ROOM
    {
        var room = cave.Find("SideRoom"); var tbl = room.Find("RouletteTable"); UnityEngine.Transform his = null, guest = null; if (tbl != null) foreach (UnityEngine.Transform t in tbl) if (t.name.StartsWith("Chair")) { if (t.localPosition.z > 0f) his = t; else guest = t; }
        UnityEngine.Transform crate = null; foreach (var t in room.GetComponentsInChildren<UnityEngine.Transform>()) if (t.name.StartsWith("C_Crate_Small_1")) crate = t;
        UnityEngine.Transform bb0 = null; foreach (UnityEngine.Transform t in room) if (t.name.StartsWith("BigBoulders_0")) bb0 = t;
        float hisYaw = his != null ? his.eulerAngles.y : float.NaN; var cb = crate != null ? crate.GetComponent<UnityEngine.Collider>().bounds : default; float gapW = crate != null ? cb.min.x - 89.5f : float.NaN, gapN = crate != null ? 15.5f - cb.max.z : float.NaN;
        float bbWest = bb0 != null ? PlaceKit.MeshBounds(bb0.gameObject).min.x : float.NaN;
        var mask = ~0; var pi = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>(); var mf = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance); if (pi != null && mf != null) mask = ((UnityEngine.LayerMask)mf.GetValue(pi)).value;
        string first = "nothing"; bool guestFirst = false; var eye = V(91.0f, floorY + eyeH, 12.0f); var dir = UnityEngine.Quaternion.Euler(15f, 90f, 0f) * UnityEngine.Vector3.forward;
        if (UnityEngine.Physics.Raycast(eye, dir, out var hit, tuning != null ? tuning.interactReach : 2f, mask, UnityEngine.QueryTriggerInteraction.Ignore)) { first = WalkIns.PathOf(hit.collider.transform) + " at " + F(hit.distance) + " m"; guestFirst = guest != null && hit.collider.transform.IsChildOf(guest); }
        Line(UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(hisYaw, 270f)) <= 1f && UnityEngine.Mathf.Abs(gapW) <= 0.02f && UnityEngine.Mathf.Abs(gapN) <= 0.02f && bbWest >= 89.5f && guestFirst,
            "SIDE ROOM: his chair yaw " + F1(hisYaw) + " (270); the crate " + F(gapW) + " m off the west wall and " + F(gapN) + " m off the north; BigBoulders_0 west edge " + F(bbWest) + " (89.5 or more); from (91.0, 12.0) facing 90, 15 down, the ray meets " + first);
    }
    // ---- DEEPER
    {
        // the interactor's own test (its mask, triggers ignored, interactReach) from an eye at a heading and pitch: the prompt it would show
        var piC = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>(); var maskFC = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        int iMask = piC != null && maskFC != null ? ((UnityEngine.LayerMask)maskFC.GetValue(piC)).value : ~0; float iReach = tuning != null ? tuning.interactReach : 2f;
        string PromptFrom(UnityEngine.Vector3 stand, float yaw, float pitch)
        {
            var eye = stand + V(0f, eyeH, 0f); var dir = UnityEngine.Quaternion.Euler(pitch, yaw, 0f) * UnityEngine.Vector3.forward;
            if (!UnityEngine.Physics.Raycast(eye, dir, out var h, iReach, iMask, UnityEngine.QueryTriggerInteraction.Ignore)) return "none (nothing within " + F1(iReach) + " m)";
            var it = h.collider.GetComponentInParent<Interactable>(); return (it != null ? "\"" + it.Prompt + "\"" : "none") + " on " + WalkIns.PathOf(h.collider.transform) + " at " + F(h.distance) + " m";
        }
        closed = L.Find("Deeper/DeeperClosed");
        if (closed == null) Line(false, "DEEPER: no Layout827/Deeper/DeeperClosed");
        else
        {
            Put(V(96.5f, floorY, 13.8f)); float t0 = 0f; bool shut = !Walk(V(101.0f, floorY, 14.0f), ref t0, out float leftShut);
            string atShut = PromptFrom(V(96.5f, floorY, 13.8f), 90f, 0f);
            closedWas = closed.gameObject.activeSelf; closed.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
            Put(V(96.5f, floorY, 13.8f)); float t1 = 0f; bool open = Walk(V(101.0f, floorY, 13.8f), ref t1, out float leftOpen) && Walk(V(101.0f, floorY, 15.0f), ref t1, out leftOpen);
            string atOpening = PromptFrom(V(96.5f, floorY, 13.8f), 90f, 0f), atEnd = PromptFrom(V(101.0f, floorY, 14.0f), 0f, 0f);
            closed.gameObject.SetActive(closedWas); UnityEngine.Physics.SyncTransforms();
            Line(shut && open, "DEEPER: shut by DeeperClosed " + shut + " (stops " + F(leftShut) + " m short); open, the body walks to the inspect point and on to the dead end " + open + " (" + F1(t1) + " s)");
            // CaveLayout_Story 23 (Quill, 2026-10-03): shut days no prompt at the rock; open days none at the opening, `Examine` at the dead end
            Line(atShut.StartsWith("none"), "PROMPT event 16 shut: from (96.5, 13.8) facing 90 at the shut rock, no prompt: " + atShut);
            Line(atOpening.StartsWith("none"), "PROMPT event 16 open, the opening: from (96.5, 13.8) facing 90, no prompt: " + atOpening);
            Line(atEnd.StartsWith("\"Examine\""), "PROMPT event 16 open, the dead end: from (101.0, 14.0) facing 0, \"Examine\": " + atEnd);
        }
    }
    // ---- NARROW
    {
        var row = L.Find("NarrowRow"); var cols = new System.Collections.Generic.List<UnityEngine.Bounds>(); float least = float.MaxValue;
        if (row != null) foreach (var c in row.GetComponentsInChildren<UnityEngine.Collider>()) cols.Add(c.bounds);
        foreach (var c in row != null ? row.GetComponentsInChildren<UnityEngine.Collider>() : new UnityEngine.Collider[0]) for (int i = 1; i < tread.Count; i++) { var a = tread[i - 1].p; var b = tread[i].p; if (a.x < 59f && b.x < 59f) continue; if (a.x > 74f && b.x > 74f) continue; for (float u = 0f; u <= 1f; u += 0.1f) { var q = UnityEngine.Vector3.Lerp(a, b, u) + V(0f, 0.5f, 0f); var cp = c.ClosestPoint(q); least = UnityEngine.Mathf.Min(least, new UnityEngine.Vector2(cp.x - q.x, cp.z - q.z).magnitude); } }
        cols.Sort((a, b) => a.center.x.CompareTo(b.center.x)); float widest = 0f; for (int i = 1; i < cols.Count; i++) widest = UnityEngine.Mathf.Max(widest, cols[i].min.x - cols[i - 1].max.x);
        Line(cols.Count > 0 && least >= narrowMin && widest <= gapMax, "NARROW: " + cols.Count + " rock colliders, the nearest " + F(least) + " m from the tread centre (" + F1(narrowMin) + " or more), the widest gap between neighbours " + F(widest) + " m (" + F1(gapMax) + " or less)");
    }
    // ---- RIM
    {
        var hedge = Root("Ground815").transform.Find("Stops/Hedge_CaveRim");
        if (hedge == null) Line(false, "RIM: no Ground815/Stops/Hedge_CaveRim");
        else
        {
            var targets = new[] { V(52f, -4.0f, 37.6f), V(50.6f, -5.0f, 37.9f), V(53.4f, -5.0f, 37.9f), V(52f, -5.9f, 38.4f) };   // the board, the void's edges, the floor
            var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>(); var lineZ = new System.Collections.Generic.Dictionary<int, float>();
            foreach (UnityEngine.Transform t in hedge) if (t.name == "HedgeCollider") { var b = t.GetComponent<UnityEngine.Collider>().bounds; lineZ[UnityEngine.Mathf.RoundToInt(b.center.x)] = b.max.z; }
            for (float x = 58f; x <= 78f + 1e-3f; x += rimStep) { int k = UnityEngine.Mathf.RoundToInt(x); if (!lineZ.ContainsKey(k)) continue; float z = lineZ[k] + 0.6f; eyes.Add(V(x, H(x, z) + eyeH, z)); }
            var rays = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3)>(); foreach (var e in eyes) foreach (var t in targets) rays.Add((e, t)); TempColliders(rays);
            int seen = 0; foreach (var e in eyes) foreach (var t in targets) if (ClearLine(e, t, null)) seen++; DropTemps();
            int crossed = 0; foreach (var e in eyes) { var from = e - V(0f, eyeH, 0f); Put(from); for (float t = 0f; t < 3f; t += dt) pc.Step(V(0f, 0f, -1f), true, true, dt); if (pc.transform.position.z < from.z - 1.2f) crossed++; }
            Line(eyes.Count > 0 && seen == 0 && crossed == 0, "RIM: " + eyes.Count + " eyes on the band's north side, clear lines to the mouth " + seen + " of " + (eyes.Count * targets.Length) + "; sprint-jumps south past the band " + crossed);
        }
    }
    // ---- WALKS
    {
        var legs = new System.Collections.Generic.List<(string, UnityEngine.Vector3[])>(); var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (var (n, p) in tread) pts.Add(p); legs.Add(("W1 to the mouth, the trail", pts.ToArray()));
        legs.Add(("the mouth to the chamber, down the legs", new[] { V(52f, -6f, 37.0f), V(52f, -6f, 22f), V(68f, -10f, 22f), V(68f, -10f, 17f), V(52f, -14f, 17f), V(52f, -14f, 12f), V(68f, -18f, 12f), V(71f, floorY, 12f) }));
        legs.Add(("the passage end to the doorway and the guest chair stand", new[] { V(71f, floorY, 12f), V(89.25f, floorY, 12f), V(91.0f, floorY, 12f) }));
        legs.Add(("the passage end to the seat stand", new[] { V(71f, floorY, 12f), V(80f, floorY, 9.5f), V(86.9f, floorY, 6.2f) }));
        float speed = tuning != null ? tuning.walkSpeed : 2.5f;
        foreach (var (name, wp) in legs)
        {
            Put(wp[0]); float time = 0f, len = 0f; bool ok = true; string where = ""; for (int i = 1; i < wp.Length; i++) len += V(wp[i].x - wp[i - 1].x, 0f, wp[i].z - wp[i - 1].z).magnitude;
            for (int i = 1; i < wp.Length && ok; i++) if (!Walk(wp[i], ref time, out float left)) { ok = false; where = ", stops " + F(left) + " m short of (" + F1(wp[i].x) + ", " + F1(wp[i].z) + ") at (" + F1(pc.transform.position.x) + ", " + F1(pc.transform.position.y) + ", " + F1(pc.transform.position.z) + ")"; }
            Line(ok, "WALK: " + name + ", " + F1(len) + " m, " + F1(time) + " s at " + F1(speed) + " m/s" + where);
        }
    }
}
finally
{
    DropTemps(); if (closed != null) closed.gameObject.SetActive(closedWas);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.27 cave check (CaveLayout.md draft 2)\n" + sb;
