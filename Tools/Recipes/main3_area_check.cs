// Main3 area check (Play mode, Main3; PLAN 8.21 to 8.30, Wren 2026-10-02): the hard checks of one area, PASS or FAIL per line. Run by
// main3_review_capture.sh --area <id>, which sets `area` below; the warp landing check runs beside it with the area's warps. Data and
// numbers: Assets/Settings/Main3Areas.asset (main3_areas_setup.cs). Never saves; restores the player and runInBackground.
// FLOOD (Marlow's method, Breaks_2026-10-01.md): standing places on a floodCell grid keyed by a floodLevel height band, seeded from the
//   area's warps and trail points, each expanded by floodHeadings headings x (walk, sprint-jump) moves of floodMoveTime s with the real
//   mover (PlayerController.Step, dt 0.02), inside the bounds plus floodMargin m (a place past that is an exit and is not expanded); a
//   move ends where the body lands, never mid-fall (8.21a). Within stopNear m of a stop the cells are stopCell m and stopLevel m (8.21b).
//   FAIL on a place inside a closed zone (Valley.md 8 thicket) or closed water (closedFills, 8.23 round 2), fellUnder m under the terrain, or standing on a stop (stopRoots or the
//   Ignore Raycast layer: an escape over a stop, wherever it leads; 8.22 round 2).
// TRAPS: a reverse search from the seeds and exits finds the places with no way back; each group is retested from its first place with
//   escapeHeadings headings x (walk, sprint, sprint-jump) for escapeTime s; a trap is 0 escapes (an escape ends on a place with a way
//   back, or 4 m or more away on ground the flood never stood on).
// WALK-INTO: WalkIns.Find in the bounds; stop visuals, walk-decked pieces and meshes drawn into a tread are listed apart (as
//   main3_walk_into_check.cs).
// REACH AND FOUND (Pim; Wren 2026-10-02: 30 m line for now): each place has a flood place within reachRadius m (and reachRise m in
//   height), and a clear eye line from a trail point within foundReach m (colliders and the drawn trees block).
// DECK (Valley.md 7, CampLayout.md 5): eyes on a deckGrid m grid over the deck at eye and jump height. Must-see: at least mustSeeShare
//   of rays clear (trees block). Must-hide by rays: 0 clear (land and colliders only; trees do not count, as F-1). Must-hide where trees
//   or rock are the cover (treeCoverPath): listed here; the CampLayout pixel check itself is main3_deck_pixel_check.cs, which
//   main3_review_capture.sh --area runs in batches and adds to the summary. Hard must-see targets (8.22, FrontLayout.md 5.4) are tested
//   against every drawn mesh (temporary exact colliders on the meshes the deck rays cross), the tower's own when the area sets
//   hardSeesTower (the lake, 8.23 round 2); a target marked anyEye passes when one deck eye sees it (Wren 2026-10-02: the lake's
//   blanket and bowl, judged as the office door); loose ones are reported, never failed.
//   An area with no deck list prints "no deck list" and does
//   not pass.
// COLLIDER SIZE (8.33): every box, sphere or capsule collider over a drawn mesh in the bounds whose faces stand past the mesh by more
//   than colliderSlack m or colliderShare of it (ColliderFit) fails; invisible blockers, Ignore Raycast stops and colliderSkipRoots (the
//   8.18a pocket fills, by design) are not measured.
// INVENTORY: the area's items, expected and found; a zero fails.
string area = "camp";
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var set = Main3AreaSet.Load(); if (set == null) return "no Assets/Settings/Main3Areas.asset (run main3_areas_setup.cs)";
var A = set.Find(area); if (A == null) return "no area " + area;
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture;
string F1(float v) => v.ToString("F1", inv);
string P3(UnityEngine.Vector3 p) => "(" + F1(p.x) + ", " + F1(p.y) + ", " + F1(p.z) + ")";
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, groundY = -900f, raise = 20f, escapeAway = 4f, trailStep = 4f, eyeH = 1.6f;
const int landSteps = 10, settleSteps = 3, fallSteps = 500, shotW = 3840, shotH = 1976;
UnityEngine.Vector3 Pt(UnityEngine.Vector3 p) => p.y <= groundY ? new UnityEngine.Vector3(p.x, H(p.x, p.z) + 1f, p.z) : p;   // y -999: the ground plus 1 m
bool InRegion(UnityEngine.Vector3 p) { foreach (var r in A.bounds) { var e = new UnityEngine.Rect(r.x - set.floodMargin, r.y - set.floodMargin, r.width + 2f * set.floodMargin, r.height + 2f * set.floodMargin); if (e.Contains(new UnityEngine.Vector2(p.x, p.z))) return true; } return false; }
// closed water (8.23 round 2, Marlow 823 finding 1: the lake bed was not closed, so a landing in it passed): each fill the area's
// bounds touch, built once before the flood with the player's own colliders left out
UnityEngine.Physics.SyncTransforms();
var closedWater = new System.Collections.Generic.List<(string label, System.Func<UnityEngine.Vector3, bool> inside, int cells)>();
if (set.closedFills != null) foreach (var f in set.closedFills)
{
    bool touches = false; foreach (var r in A.bounds) if (r.Overlaps(f.within)) touches = true; if (!touches) continue;
    var fn = Main3AreaSet.ClosedWater(f, H, c => c.transform.IsChildOf(pc.transform)); closedWater.Add((f.label, fn, Main3AreaSet.ReachedCells(f, fn)));
}
bool Closed(UnityEngine.Vector3 p) { if (set.closedZones != null) foreach (var r in set.closedZones) if (r.Contains(new UnityEngine.Vector2(p.x, p.z))) return true; foreach (var w in closedWater) if (w.inside(p)) return true; return false; }
// finer keys near stops (8.21b): the coarse cells within stopNear m of a stop collider's bounds; a place there keys on stopCell cells and
// stopLevel levels (negative keys), so a short step from a raised fill onto a stop's top is its own place
var nearStop = new System.Collections.Generic.HashSet<long>();
long Coarse(float x, float z) => (UnityEngine.Mathf.FloorToInt(x / set.floodCell) + 10000L) * 20000L + (UnityEngine.Mathf.FloorToInt(z / set.floodCell) + 10000L);
foreach (var c in UnityEngine.Object.FindObjectsByType<UnityEngine.Collider>(UnityEngine.FindObjectsSortMode.None))
{
    if (!c.enabled || c.isTrigger || c is UnityEngine.TerrainCollider) continue; bool stop = c.gameObject.layer == 2;
    if (!stop && set.stopRoots != null) { var path = WalkIns.PathOf(c.transform); foreach (var sr in set.stopRoots) if (path.StartsWith(sr)) { stop = true; break; } }
    if (!stop) continue; var b = c.bounds; if (b.size.x > 200f || b.size.z > 200f) continue;
    for (float x = b.min.x - set.stopNear; x <= b.max.x + set.stopNear + set.floodCell; x += set.floodCell) for (float z = b.min.z - set.stopNear; z <= b.max.z + set.stopNear + set.floodCell; z += set.floodCell) nearStop.Add(Coarse(x, z));
}
long Key(UnityEngine.Vector3 p)
{
    if (nearStop.Contains(Coarse(p.x, p.z))) { long fi = UnityEngine.Mathf.FloorToInt(p.x / set.stopCell) + 100000, fj = UnityEngine.Mathf.FloorToInt(p.z / set.stopCell) + 100000, fk = UnityEngine.Mathf.FloorToInt(p.y / set.stopLevel) + 10000; return -((fi * 200000L + fj) * 20000L + fk) - 1; }
    long i = UnityEngine.Mathf.FloorToInt(p.x / set.floodCell) + 10000, j = UnityEngine.Mathf.FloorToInt(p.z / set.floodCell) + 10000, k = UnityEngine.Mathf.FloorToInt(p.y / set.floodLevel) + 1000; return (i * 20000L + j) * 4000L + k;
}
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int s = 0; s < settleSteps; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
UnityEngine.Vector3 Move(UnityEngine.Vector3 from, UnityEngine.Vector3 dir, bool jump, bool sprint, float time)
{
    Put(from); for (float t = 0f; t < time; t += dt) pc.Step(dir, jump, sprint, dt);
    for (int s = 0; s < landSteps; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
    for (int s = 0; s < fallSteps && !cc.isGrounded; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);   // a move ends where the body lands (8.21a: places recorded mid-fall off the tower deck were jumped from again, a second jump in the air)
    return pc.transform.position;
}
UnityEngine.Vector3 Dir(int k, int n) => UnityEngine.Quaternion.Euler(0f, k * 360f / n, 0f) * UnityEngine.Vector3.forward;
// trail points every trailStep m along every leg
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector3>();
foreach (UnityEngine.Transform leg in Root("Trails").transform)
{
    UnityEngine.Vector3? prev = null;
    foreach (UnityEngine.Transform pt in leg)
    {
        var p = pt.position;
        if (prev.HasValue) { int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(prev.Value, p) / trailStep)); for (int i = 1; i <= n; i++) trailPts.Add(UnityEngine.Vector3.Lerp(prev.Value, p, i / (float)n)); } else trailPts.Add(p);
        prev = p;
    }
}
var start = pc.transform.position; var startRot = pc.transform.rotation;
var sb = new System.Text.StringBuilder(); int fails = 0; bool incomplete = false;
var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
var clock = System.Diagnostics.Stopwatch.StartNew();
try
{
    // ---- FLOOD
    var pos = new System.Collections.Generic.Dictionary<long, UnityEngine.Vector3>();
    var back = new System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>>();   // reversed edges
    var good = new System.Collections.Generic.HashSet<long>();   // seeds and exits
    var q = new System.Collections.Generic.Queue<long>(); int moves = 0; var leaks = new System.Collections.Generic.List<string>(); var fell = new System.Collections.Generic.List<string>();
    var warpsRoot = Root("DevWarps").transform; int seeds = 0;
    void Seed(UnityEngine.Vector3 p) { Put(p); for (int s = 0; s < landSteps * 5; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); var e = pc.transform.position; long k = Key(e); if (pos.ContainsKey(k)) return; pos[k] = e; good.Add(k); q.Enqueue(k); seeds++; }
    foreach (var w in A.warps) { var t = warpsRoot.Find(w); if (t != null) Seed(t.position); else sb.Append("note: no warp " + w + "\n"); }
    foreach (var p in trailPts) if (A.Contains(p)) Seed(p + UnityEngine.Vector3.up * 0.5f);
    while (q.Count > 0)
    {
        long k = q.Dequeue(); var from = pos[k];
        for (int h = 0; h < set.floodHeadings; h++) foreach (var jump in new[] { false, true })
        {
            var e = Move(from, Dir(h, set.floodHeadings), jump, jump, set.floodMoveTime); moves++;
            long ek = Key(e); if (ek == k) continue;
            if (!back.TryGetValue(ek, out var lst)) { lst = new System.Collections.Generic.List<long>(); back[ek] = lst; } if (!lst.Contains(k)) lst.Add(k);
            if (pos.ContainsKey(ek)) continue;
            pos[ek] = e;
            if (e.y < H(e.x, e.z) - set.fellUnder) { fell.Add(P3(e) + " from " + P3(from)); continue; }
            if (Closed(e)) { leaks.Add(P3(e) + " from " + P3(from)); continue; }
            if (!InRegion(e)) { good.Add(ek); continue; }   // an exit: the rest of the map is the next area's
            q.Enqueue(ek);
        }
    }
    // a place standing on a stop is an escape over it (8.22 round 2, Marlow 1), wherever it leads
    var onStop = new System.Collections.Generic.List<string>(); var onStopPlaces = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (var kv in pos)
    {
        // only the surface the body stands on, the highest under it (8.23 round 2: ring boxes 0.2 m under the boathouse floor and the dock
        // deck read as stood on once Lake/WadeLimit was a stop)
        var p = kv.Value; UnityEngine.RaycastHit top = default; bool any = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(p + UnityEngine.Vector3.up * 0.3f, UnityEngine.Vector3.down, 0.6f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
            if (!h.collider.transform.IsChildOf(pc.transform) && (!any || h.distance < top.distance)) { top = h; any = true; }
        if (!any || top.collider is UnityEngine.TerrainCollider) continue;
        var ht = top.collider.transform; var path = WalkIns.PathOf(ht); bool stop = ht.gameObject.layer == 2;
        if (set.stopRoots != null) foreach (var sr in set.stopRoots) if (path.StartsWith(sr)) stop = true;
        if (stop) { onStop.Add(P3(p) + " on " + path); onStopPlaces.Add(p); }
    }
    bool floodOk = leaks.Count == 0 && fell.Count == 0 && onStop.Count == 0; if (!floodOk) fails++;
    sb.Append((floodOk ? "PASS" : "FAIL") + " FLOOD: " + pos.Count + " standing places from " + seeds + " seeds, " + moves + " moves; closed-zone leaks " + leaks.Count + ", fell through " + fell.Count + ", standing on a stop " + onStop.Count + (closedWater.Count > 0 ? "; closed water: " + string.Join(", ", System.Linq.Enumerable.Select(closedWater, w => w.label + " " + w.cells + " cells")) : "") + "\n");
    foreach (var l in onStop) sb.Append("  ON STOP " + l + "\n");
    foreach (var l in leaks) sb.Append("  LEAK " + l + "\n"); foreach (var l in fell) sb.Append("  FELL " + l + "\n");
    // ---- TRAPS
    var ok = new System.Collections.Generic.HashSet<long>(good); var rq = new System.Collections.Generic.Queue<long>(good);
    while (rq.Count > 0) { long k = rq.Dequeue(); if (!back.TryGetValue(k, out var lst)) continue; foreach (var f in lst) if (ok.Add(f)) rq.Enqueue(f); }
    var cand = new System.Collections.Generic.List<long>(); foreach (var kv in pos) if (!ok.Contains(kv.Key) && A.Contains(kv.Value) && !Closed(kv.Value) && kv.Value.y >= H(kv.Value.x, kv.Value.z) - set.fellUnder) cand.Add(kv.Key);
    var groups = new System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector3>>();
    foreach (var k in cand) { var p = pos[k]; System.Collections.Generic.List<UnityEngine.Vector3> into = null; foreach (var g in groups) foreach (var gp in g) if ((gp - p).magnitude < 3f * set.floodCell) { into = g; break; } if (into == null) { into = new System.Collections.Generic.List<UnityEngine.Vector3>(); groups.Add(into); } into.Add(p); }
    int traps = 0; var trapLines = new System.Text.StringBuilder(); var trapPlaces = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (var g in groups)
    {
        var p = g[0]; int esc = 0, tries = 0;
        for (int h = 0; h < set.escapeHeadings; h++) foreach (var (jump, sprint) in new[] { (false, false), (false, true), (true, true) })
        {
            tries++; var e = Move(p, Dir(h, set.escapeHeadings), jump, sprint, set.escapeTime); long ek = Key(e);
            if (ok.Contains(ek) || (!pos.ContainsKey(ek) && (e - p).magnitude >= escapeAway)) esc++;
        }
        if (esc == 0) { traps++; trapPlaces.AddRange(g); }
        trapLines.Append("  " + (esc == 0 ? "TRAP " : "no way back by flood, escapes ") + (esc == 0 ? "" : esc + " of " + tries + " ") + P3(p) + " (" + g.Count + " places)\n");
    }
    if (traps > 0) fails++;
    sb.Append((traps == 0 ? "PASS" : "FAIL") + " TRAPS: " + cand.Count + " places with no way back by flood in " + groups.Count + " groups; traps (0 of " + (set.escapeHeadings * 3) + " escapes) " + traps + "\n" + trapLines);
    // ---- WALK-INTO
    var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
    string[] laterRoots = { "Rock/ClimbRing/RimBoulders", "Ground815/Stops/", "Rock/KnobRock", "Rock/BandScree", "Campsites/Camp_2/Ladder", "PointsOfInterest/POI_Footbridge", "PointsOfInterest/POI_Stepping_stones" };   // as main3_walk_into_check.cs
    var treads = WalkIns.Treads(); int wiFail = 0, wiApart = 0; var wiLines = new System.Text.StringBuilder();
    foreach (var w in WalkIns.Find(tuning.capsuleRadius, tuning.standHeight))
    {
        if (!A.Contains(w.At)) continue; bool apart = false; foreach (var lr in laterRoots) if (w.Path.StartsWith(lr)) apart = true;
        if (apart || WalkIns.MeshInTread(w.Renderer, treads)) { wiApart++; continue; }
        wiFail++; wiLines.Append("  WALK-IN " + w.Path + " at " + P3(w.At) + ", " + w.Stands + " stands\n");
    }
    if (wiFail > 0) fails++;
    sb.Append((wiFail == 0 ? "PASS" : "FAIL") + " WALK-INTO: " + wiFail + " meshes the body enters with no collider (stop visuals, walk decks and tread meshes apart: " + wiApart + ")\n" + wiLines);
    // ---- COLLIDER SIZE (8.33, ColliderFit): every box, sphere and capsule collider in the area (not a trigger, not Ignore Raycast) whose
    // faces stand past its drawn mesh by more than colliderSlack m or colliderShare of the mesh, the turned-prop inflation
    {
        int measured = 0; var big = new System.Collections.Generic.List<string>();
        foreach (var c in UnityEngine.Object.FindObjectsByType<UnityEngine.Collider>(UnityEngine.FindObjectsSortMode.None))
        {
            if (!c.enabled || c.isTrigger || c.gameObject.layer == 2 || !c.gameObject.activeInHierarchy || c.transform.IsChildOf(pc.transform) || !A.Contains(c.bounds.center)) continue;
            if (set.colliderSkipRoots != null) { var cpath = WalkIns.PathOf(c.transform); bool skipIt = false; foreach (var sr in set.colliderSkipRoots) if (cpath.StartsWith(sr)) skipIt = true; if (skipIt) continue; }   // pocket fills, by design
            var r = ColliderFit.Measure(c, set.colliderSlack, set.colliderShare); if (!r.measured) continue; measured++;
            if (r.inflated) big.Add(WalkIns.PathOf(c.transform) + " (" + c.GetType().Name + " " + F1(r.colliderSize.x) + " x " + F1(r.colliderSize.y) + " x " + F1(r.colliderSize.z) + " over a mesh of " + F1(r.meshSize.x) + " x " + F1(r.meshSize.y) + " x " + F1(r.meshSize.z) + ", a face " + F1(r.excess) + " m out)");
        }
        if (big.Count > 0) fails++;
        sb.Append((big.Count == 0 ? "PASS" : "FAIL") + " COLLIDER SIZE: " + measured + " colliders over drawn meshes, " + big.Count + " standing more than " + F1(set.colliderSlack) + " m or " + (set.colliderShare * 100f).ToString("F0", inv) + " percent past their mesh\n");
        foreach (var l in big) sb.Append("  BIG " + l + "\n");
    }
    // ---- the drawn trees block sight from here on (temporary MeshColliders on each tree's first LOD; removed in finally)
    var tower = Root("Camp").transform.Find("Tower"); float deckTop = tower.Find("Cab").position.y;
    var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>();
    for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid)
        foreach (var hgt in new[] { set.deckEye, set.deckJump }) eyes.Add(new UnityEngine.Vector3(tower.position.x + gx, deckTop + hgt, tower.position.z + gz));
    bool Clear(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform ignore, UnityEngine.Transform own = null)   // own: the target's own object, never its own blocker (8.24 gate: the tent's own mesh hid it)
    {
        var d = b - a; float len = d.magnitude - set.rayEndSkip; if (len <= 0f) return true;
        foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, len, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore))
            if ((ignore == null || !h.collider.transform.IsChildOf(ignore)) && (own == null || !h.collider.transform.IsChildOf(own)) && !h.collider.transform.IsChildOf(pc.transform)) return false;
        return true;
    }
    string Blocker(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform ignore)   // the nearest blocking collider, for the report
    {
        var d = b - a; float len = d.magnitude - set.rayEndSkip; UnityEngine.RaycastHit best = default; bool any = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, len, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore))
            if ((ignore == null || !h.collider.transform.IsChildOf(ignore)) && !h.collider.transform.IsChildOf(pc.transform) && (!any || h.distance < best.distance)) { best = h; any = true; }
        return any ? WalkIns.PathOf(best.collider.transform) + " at " + P3(best.point) + (temps.Contains(best.collider) ? " (drawn tree)" : "") : "none";
    }
    // DECK must-hide by rays first: land and colliders only (trees do not count, as F-1)
    var deckLines = new System.Text.StringBuilder(); int deckFails = 0;
    if (!A.deckListWritten) { incomplete = true; sb.Append("---- DECK: no deck list (Sable writes it when the area starts)\n"); }
    else foreach (var t in A.deckHide)
    {
        if (!string.IsNullOrEmpty(t.treeCoverPath)) continue; var p = Pt(t.point); int clear = 0; foreach (var e in eyes) if (Clear(e, p, tower)) clear++;
        if (clear > 0) deckFails++; deckLines.Append("  " + (clear == 0 ? "ok   " : "SEEN ") + "hide " + t.label + " " + P3(p) + ": " + clear + " of " + eyes.Count + " rays clear\n");
    }
    foreach (var root in new[] { Root("Forest"), Root("SliceLook"), Root("Ground815") }) if (root != null)
        foreach (var lod in root.GetComponentsInChildren<UnityEngine.LODGroup>())
        {
            var lods = lod.GetLODs(); if (lods.Length == 0) continue;
            foreach (var r in lods[0].renderers) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null || r.GetComponent<UnityEngine.Collider>() != null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
        }
    UnityEngine.Physics.SyncTransforms();
    // ---- REACH AND FOUND (found: Pim's rule, 8.21 gate; Main3AreaSet.Found). The found points go to Temp/area_found_<id>.txt, from which
    // main3_review_capture.cs draws one labelled frame per place (Found_<id>.jpg)
    var legsOrdered = new System.Collections.Generic.List<(string leg, System.Collections.Generic.List<UnityEngine.Vector3> pts)>();
    foreach (UnityEngine.Transform leg in Root("Trails").transform)
    {
        var lp = new System.Collections.Generic.List<UnityEngine.Vector3>(); UnityEngine.Vector3? prev = null;
        foreach (UnityEngine.Transform pt in leg) { var qq = pt.position; if (prev.HasValue) { int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(prev.Value, qq) / 2f)); for (int i = 1; i <= n; i++) lp.Add(UnityEngine.Vector3.Lerp(prev.Value, qq, i / (float)n)); } else lp.Add(qq); prev = qq; }
        legsOrdered.Add((leg.name, lp));
    }
    // the area's walk lines where no trail runs (8.22: the front zone's lot, walk and spur, FrontLayout_UI.md found targets)
    if (A.walkLines != null) foreach (var wl in A.walkLines)
    {
        var lp = new System.Collections.Generic.List<UnityEngine.Vector3>();
        for (int k = 0; k < wl.points.Length; k++) { var qq = Pt(wl.points[k]); if (k > 0) { var pv = Pt(wl.points[k - 1]); int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(pv, qq) / 2f)); for (int i = 1; i <= n; i++) lp.Add(UnityEngine.Vector3.Lerp(pv, qq, i / (float)n)); } else lp.Add(qq); }
        legsOrdered.Add(("walk: " + wl.label, lp));
    }
    int rfFail = 0; var rfLines = new System.Text.StringBuilder(); var foundFile = new System.Text.StringBuilder();
    foreach (var pl in A.places)
    {
        var p = Pt(pl.point); float gy = pl.point.y <= groundY ? H(p.x, p.z) : pl.point.y; bool reached = false;
        foreach (var kv in pos) { var d = kv.Value - p; if (new UnityEngine.Vector2(d.x, d.z).magnitude <= set.reachRadius && UnityEngine.Mathf.Abs(kv.Value.y - gy) <= set.reachRise) { reached = true; break; } }
        var obj = string.IsNullOrEmpty(pl.objectPath) ? null : Main3AreaSet.At(scene, pl.objectPath);
        bool ClearTo(UnityEngine.Vector3 a, UnityEngine.Vector3 b)   // by meshes: colliders and the drawn trees; a hit on the place's own object reaches it
        {
            var d = b - a; float len = d.magnitude - (obj != null ? 0.05f : set.rayEndSkip); if (len <= 0f) return true; float firstOther = float.MaxValue, firstOwn = float.MaxValue;
            foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, len, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore))
            { if (h.collider.transform.IsChildOf(pc.transform)) continue; if (obj != null && h.collider.transform.IsChildOf(obj)) firstOwn = UnityEngine.Mathf.Min(firstOwn, h.distance); else firstOther = UnityEngine.Mathf.Min(firstOther, h.distance); }
            return firstOther == float.MaxValue || firstOwn < firstOther;
        }
        var fr = set.Found(pl, legsOrdered, (x, z) => H(x, z), ClearTo);
        if (!reached || !fr.found) rfFail++;
        rfLines.Append("  " + (reached && fr.found ? "ok   " : "FAIL ") + pl.label + ": " + (reached ? "reached" : "NOT REACHED") + ", " + (fr.found ? "found from " + fr.leg + " " + F1(fr.dist) + " m out, " + F1(fr.angle) + " degrees off the way, " + F1(fr.tall) + " degrees tall" : "NOT FOUND (" + fr.tried + " trail points within " + F1(set.foundReach) + " m, " + F1(set.foundMaxAngle) + " degrees of travel and " + F1(set.foundMinDeg) + " degrees tall; none with clear lines to its centre and top)") + "\n");
        foundFile.Append(pl.label + "|" + (fr.found ? "1" : "0") + "|" + fr.eye.x.ToString(inv) + "," + fr.eye.y.ToString(inv) + "," + fr.eye.z.ToString(inv) + "|" + fr.aim.x.ToString(inv) + "," + fr.aim.y.ToString(inv) + "," + fr.aim.z.ToString(inv) + "|" + fr.leg + "|" + F1(fr.dist) + "\n");
    }
    System.IO.Directory.CreateDirectory("Temp"); System.IO.File.WriteAllText("Temp/area_found_" + A.id + ".txt", foundFile.ToString());
    if (rfFail > 0) fails++;
    sb.Append((rfFail == 0 ? "PASS" : "FAIL") + " REACH AND FOUND: " + (A.places.Length - rfFail) + " of " + A.places.Length + " places reached and found (Pim's rule: within " + F1(set.foundReach) + " m of a trail, " + F1(set.foundMaxAngle) + " degrees of travel, " + F1(set.foundMinDeg) + " degree tall, clear by meshes)\n" + rfLines);
    // ---- SPACING (CampLayout_UI rule 1, Pim): the interaction points' collider bounds, centre and edge distances between points on one
    // floor within 3 m, and the first hit of a 2 m eye ray from each approach toward its point (it must be the point itself)
    if (A.interactions != null && A.interactions.Length > 0)
    {
        int spFail = 0; var spLines = new System.Text.StringBuilder(); var partsOf = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<UnityEngine.Transform>>(); var bodies = new System.Collections.Generic.List<(string label, UnityEngine.Bounds b, UnityEngine.Transform t, UnityEngine.Vector3 approach)>();
        foreach (var ia in A.interactions)
        {
            var paths = ia.path.Split(';'); var t = Main3AreaSet.At(scene, paths[0]); var parts = new System.Collections.Generic.List<UnityEngine.Transform>(); foreach (var pp in paths) { var pt = Main3AreaSet.At(scene, pp.Trim()); if (pt != null) parts.Add(pt); } partsOf[ia.label] = parts; bool any = false; var b = new UnityEngine.Bounds();
            if (t != null) foreach (var c in t.GetComponentsInChildren<UnityEngine.Collider>()) { if (!c.enabled) continue; if (!any) { b = c.bounds; any = true; } else b.Encapsulate(c.bounds); }
            if (!any) { spFail++; spLines.Append("  FAIL " + ia.label + ": no collider under " + ia.path + "\n"); continue; }
            var ap = ia.approach.y <= groundY ? new UnityEngine.Vector3(ia.approach.x, H(ia.approach.x, ia.approach.z), ia.approach.z) : ia.approach;
            bodies.Add((ia.label, b, t, ap));
            spLines.Append("  " + ia.label + ": collider x " + F1(b.min.x) + " to " + F1(b.max.x) + ", z " + F1(b.min.z) + " to " + F1(b.max.z) + ", top " + F1(b.max.y) + "\n");
        }
        for (int i = 0; i < bodies.Count; i++) for (int j = i + 1; j < bodies.Count; j++)
        {
            var a = bodies[i]; var c = bodies[j]; if (UnityEngine.Mathf.Abs(a.b.min.y - c.b.min.y) > 2f) continue;
            float centre = new UnityEngine.Vector2(a.b.center.x - c.b.center.x, a.b.center.z - c.b.center.z).magnitude; if (centre > 3f) continue;
            float gx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(a.b.min.x - c.b.max.x, c.b.min.x - a.b.max.x)), gz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(a.b.min.z - c.b.max.z, c.b.min.z - a.b.max.z)); float edgeGap = UnityEngine.Mathf.Sqrt(gx * gx + gz * gz);
            bool pairOk = centre >= set.spacingCentre - 0.01f && edgeGap >= set.spacingEdge - 0.02f; string why = "";
            // a pair that stands together by design (Area.spacingExceptions): one unit when its gap is at most maxEdge, out of the slot band,
            // with no trap or stop stand of the flood near either
            if (!pairOk && A.spacingExceptions != null) foreach (var ex in A.spacingExceptions)
            {
                if (!((ex.a == a.label && ex.b == c.label) || (ex.a == c.label && ex.b == a.label))) continue;
                int snags = 0; foreach (var bd in new[] { a.b, c.b }) { var cc0 = new UnityEngine.Vector2(bd.center.x, bd.center.z); foreach (var g in trapPlaces) if ((new UnityEngine.Vector2(g.x, g.z) - cc0).magnitude <= set.snagRadius) snags++; foreach (var sp in onStopPlaces) if ((new UnityEngine.Vector2(sp.x, sp.z) - cc0).magnitude <= set.snagRadius) snags++; }
                bool slot = edgeGap >= set.slotLow && edgeGap < set.slotHigh; pairOk = edgeGap <= ex.maxEdge + 0.02f && !slot && snags == 0;
                why = " (exception, " + ex.reason + ": edge at most " + F1(ex.maxEdge) + (slot ? ", IN THE SLOT BAND" : "") + ", trap and stop stands within " + F1(set.snagRadius) + " m " + snags + ")";
            }
            if (!pairOk) spFail++;
            spLines.Append("  " + (pairOk ? "ok   " : "FAIL ") + a.label + " to " + c.label + ": centres " + F1(centre) + " m (at least " + F1(set.spacingCentre) + "), edges " + edgeGap.ToString("F2", inv) + " m (at least " + F1(set.spacingEdge) + ")" + why + "\n");
        }
        foreach (var bd in bodies)
        {
            var eye = bd.approach + UnityEngine.Vector3.up * 1.6f; var dir = (bd.b.center - eye).normalized; string first = "none within " + F1(set.approachRay) + " m"; bool rayOk = true;
            if (UnityEngine.Physics.Raycast(eye, dir, out var hit, set.approachRay, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore) && !hit.collider.transform.IsChildOf(pc.transform))
            { bool own = false; foreach (var pt in partsOf[bd.label]) if (hit.collider.transform.IsChildOf(pt)) own = true; first = (own ? "itself" : WalkIns.PathOf(hit.collider.transform)) + " at " + F1(hit.distance) + " m"; rayOk = own; }
            if (!rayOk) spFail++;
            spLines.Append("  " + (rayOk ? "ok   " : "FAIL ") + bd.label + " eye ray from its approach " + P3(bd.approach) + ": first hit " + first + "\n");
        }
        if (spFail > 0) fails++;
        sb.Append((spFail == 0 ? "PASS" : "FAIL") + " SPACING: " + bodies.Count + " interaction points\n" + spLines);
    }
    // ---- DECK must-see (trees block) and the pixel check
    if (A.deckListWritten)
    {
        // hard must-see (FrontLayout.md 5.4; Marlow 8.22 paper 15: the verge tree has no collider, so a collider-only ray reads clear
        // whatever stands in the way): every drawn mesh blocks. Each enabled LOD0 or plain MeshRenderer with no collider whose bounds a
        // deck ray to a hard target crosses gets a temporary exact MeshCollider (removed in finally); a hit on the target's own object
        // (objectPath) reaches it
        var hardSegs = new System.Collections.Generic.List<UnityEngine.Ray>(); var hardLens = new System.Collections.Generic.List<float>();
        foreach (var t in A.deckSee) if (t.hard) { var p = Pt(t.point); foreach (var e in eyes) { var d = p - e; hardSegs.Add(new UnityEngine.Ray(e, d.normalized)); hardLens.Add(d.magnitude); } }
        int meshTemps = 0;
        if (hardSegs.Count > 0)
        {
            var span = new UnityEngine.Bounds(hardSegs[0].origin, UnityEngine.Vector3.zero); for (int i = 0; i < hardSegs.Count; i++) { span.Encapsulate(hardSegs[i].origin); span.Encapsulate(hardSegs[i].GetPoint(hardLens[i])); }
            var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>();
            foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var lods = lod.GetLODs(); for (int li = 1; li < lods.Length; li++) foreach (var r in lods[li].renderers) if (r != null) notLod0.Add(r); }
            foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
            {
                if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(pc.transform) || (!A.hardSeesTower && mr.transform.IsChildOf(tower)) || mr.name.Contains("Glass")) continue;   // glass is seen through
                var b = mr.bounds; if (!b.Intersects(span)) continue; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue;
                bool crossed = false; for (int i = 0; i < hardSegs.Count && !crossed; i++) if (b.IntersectRay(hardSegs[i], out float dist) && dist <= hardLens[i]) crossed = true;
                if (!crossed) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); meshTemps++;
            }
            UnityEngine.Physics.SyncTransforms();
        }
        bool ClearHard(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform own)
        {
            var d = b - a; float len = d.magnitude - 0.05f; float firstOther = float.MaxValue, firstOwn = float.MaxValue;
            foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, len, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
            {
                var ht = h.collider.transform; if ((!A.hardSeesTower && ht.IsChildOf(tower)) || ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2) continue;   // Ignore Raycast: invisible walls and hedge boxes
                if (own != null && ht.IsChildOf(own)) firstOwn = UnityEngine.Mathf.Min(firstOwn, h.distance); else firstOther = UnityEngine.Mathf.Min(firstOther, h.distance);
            }
            return firstOther == float.MaxValue || firstOwn < firstOther;
        }
        string HardBlocker(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform own)
        {
            var d = b - a; UnityEngine.RaycastHit best = default; bool any = false;
            foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
            { var ht = h.collider.transform; if ((!A.hardSeesTower && ht.IsChildOf(tower)) || ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2 || (own != null && ht.IsChildOf(own))) continue; if (!any || h.distance < best.distance) { best = h; any = true; } }
            return any ? WalkIns.PathOf(best.collider.transform) + " at " + P3(best.point) + (temps.Contains(best.collider) ? " (drawn mesh)" : "") : "none";
        }
        if (hardSegs.Count > 0) deckLines.Append("  (hard must-see: " + meshTemps + " drawn meshes on the deck lines got temporary colliders)\n");
        foreach (var t in A.deckSee)
        {
            var p = Pt(t.point); var own = string.IsNullOrEmpty(t.objectPath) ? null : Main3AreaSet.At(scene, t.objectPath);
            if (t.hard && !string.IsNullOrEmpty(t.objectPath) && own == null) { deckFails++; deckLines.Append("  HIDDEN see " + t.label + ": no object " + t.objectPath + "\n"); continue; }
            int clear = 0; foreach (var e in eyes) if (t.hard ? ClearHard(e, p, own) : Clear(e, p, tower, own)) clear++;
            float share = clear / (float)eyes.Count; bool pass = t.anyEye ? clear > 0 : share >= set.mustSeeShare; if (!pass && !t.loose) deckFails++;
            var centreEye = new UnityEngine.Vector3(tower.position.x, deckTop + set.deckEye, tower.position.z);
            deckLines.Append("  " + (pass ? "ok   " : t.loose ? "low  " : "HIDDEN ") + "see " + (t.loose ? "(loose) " : t.hard ? "(hard, every mesh) " : "") + t.label + " " + P3(p) + ": " + clear + " of " + eyes.Count + " rays clear (" + (share * 100f).ToString("F0", inv) + " percent, " + (t.anyEye ? "passes from any deck eye, Wren 2026-10-02" : "bar " + (set.mustSeeShare * 100f).ToString("F0", inv)) + ")" + (pass ? "" : "; from the deck centre the first block is " + (t.hard ? HardBlocker(centreEye, p, own) : Blocker(centreEye, p, tower))) + "\n");
        }
        // the pixel check runs in main3_deck_pixel_check.cs, a batch of eyes per job (one job of every render ran the GPU out of memory);
        // main3_review_capture.sh --area adds its PIXEL lines under this one
        int pixelTargets = 0; foreach (var t in A.deckHide) if (!string.IsNullOrEmpty(t.treeCoverPath)) { pixelTargets++; deckLines.Append("  (pixel) hide " + t.label + " (" + t.treeCoverPath + "): main3_deck_pixel_check.cs\n"); }
        if (deckFails > 0) fails++;
        sb.Append((deckFails == 0 ? "PASS" : "FAIL") + " DECK: " + (A.deckSee.Length + A.deckHide.Length) + " targets from " + eyes.Count + " eyes (" + (eyes.Count / 2) + " at eye and jump height)\n" + deckLines);
    }
    // ---- INVENTORY
    var items = set.ItemsFor(A); int zeros = 0; var invLines = new System.Text.StringBuilder();
    foreach (var it in items) { int found = Main3AreaSet.Count(scene, it, out bool miss); if (found == 0) zeros++; invLines.Append("  " + (found == 0 ? "ZERO " : it.expected >= 0 && found != it.expected ? "DIFF " : "ok   ") + it.label + ": expected " + (it.expected >= 0 ? it.expected.ToString() : "?") + ", found " + found + "\n"); }
    if (zeros > 0) fails++;
    sb.Append((zeros == 0 ? "PASS" : "FAIL") + " INVENTORY: " + items.Count + " items, " + zeros + " zero\n" + invLines);
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 && !incomplete ? "ALL PASS" : fails > 0 ? "FAILS " + fails : "INCOMPLETE") + " | area " + A.id + " (" + A.task + ", " + A.title + ") in " + clock.Elapsed.TotalSeconds.ToString("F0", inv) + " s\n" + sb;
