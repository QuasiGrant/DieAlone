// Main3 area check (Play mode, Main3; PLAN 8.21 to 8.30, Wren 2026-10-02): the hard checks of one area, PASS or FAIL per line. Run by
// main3_review_capture.sh --area <id>, which sets `area` below; the warp landing check runs beside it with the area's warps. Data and
// numbers: Assets/Settings/Main3Areas.asset (main3_areas_setup.cs). Never saves; restores the player and runInBackground.
// FLOOD (Marlow's method, Breaks_2026-10-01.md): standing places on a floodCell grid keyed by a floodLevel height band, seeded from the
//   area's warps and trail points, each expanded by floodHeadings headings x (walk, sprint-jump) moves of floodMoveTime s with the real
//   mover (PlayerController.Step, dt 0.02), inside the bounds plus floodMargin m (a place past that is an exit and is not expanded).
//   FAIL on a place inside a closed zone (Valley.md 8 thicket) or fellUnder m under the terrain.
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
//   main3_review_capture.sh --area runs in batches and adds to the summary. An area with no deck list prints "no deck list" and does
//   not pass.
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
const int landSteps = 10, settleSteps = 3, shotW = 3840, shotH = 1976;
UnityEngine.Vector3 Pt(UnityEngine.Vector3 p) => p.y <= groundY ? new UnityEngine.Vector3(p.x, H(p.x, p.z) + 1f, p.z) : p;   // y -999: the ground plus 1 m
bool InRegion(UnityEngine.Vector3 p) { foreach (var r in A.bounds) { var e = new UnityEngine.Rect(r.x - set.floodMargin, r.y - set.floodMargin, r.width + 2f * set.floodMargin, r.height + 2f * set.floodMargin); if (e.Contains(new UnityEngine.Vector2(p.x, p.z))) return true; } return false; }
bool Closed(UnityEngine.Vector3 p) { if (set.closedZones != null) foreach (var r in set.closedZones) if (r.Contains(new UnityEngine.Vector2(p.x, p.z))) return true; return false; }
long Key(UnityEngine.Vector3 p) { long i = UnityEngine.Mathf.FloorToInt(p.x / set.floodCell) + 10000, j = UnityEngine.Mathf.FloorToInt(p.z / set.floodCell) + 10000, k = UnityEngine.Mathf.FloorToInt(p.y / set.floodLevel) + 1000; return (i * 20000L + j) * 4000L + k; }
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int s = 0; s < settleSteps; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
UnityEngine.Vector3 Move(UnityEngine.Vector3 from, UnityEngine.Vector3 dir, bool jump, bool sprint, float time)
{
    Put(from); for (float t = 0f; t < time; t += dt) pc.Step(dir, jump, sprint, dt);
    for (int s = 0; s < landSteps; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
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
    bool floodOk = leaks.Count == 0 && fell.Count == 0; if (!floodOk) fails++;
    sb.Append((floodOk ? "PASS" : "FAIL") + " FLOOD: " + pos.Count + " standing places from " + seeds + " seeds, " + moves + " moves; closed-zone leaks " + leaks.Count + ", fell through " + fell.Count + "\n");
    foreach (var l in leaks) sb.Append("  LEAK " + l + "\n"); foreach (var l in fell) sb.Append("  FELL " + l + "\n");
    // ---- TRAPS
    var ok = new System.Collections.Generic.HashSet<long>(good); var rq = new System.Collections.Generic.Queue<long>(good);
    while (rq.Count > 0) { long k = rq.Dequeue(); if (!back.TryGetValue(k, out var lst)) continue; foreach (var f in lst) if (ok.Add(f)) rq.Enqueue(f); }
    var cand = new System.Collections.Generic.List<long>(); foreach (var kv in pos) if (!ok.Contains(kv.Key) && A.Contains(kv.Value) && !Closed(kv.Value) && kv.Value.y >= H(kv.Value.x, kv.Value.z) - set.fellUnder) cand.Add(kv.Key);
    var groups = new System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector3>>();
    foreach (var k in cand) { var p = pos[k]; System.Collections.Generic.List<UnityEngine.Vector3> into = null; foreach (var g in groups) foreach (var gp in g) if ((gp - p).magnitude < 3f * set.floodCell) { into = g; break; } if (into == null) { into = new System.Collections.Generic.List<UnityEngine.Vector3>(); groups.Add(into); } into.Add(p); }
    int traps = 0; var trapLines = new System.Text.StringBuilder();
    foreach (var g in groups)
    {
        var p = g[0]; int esc = 0, tries = 0;
        for (int h = 0; h < set.escapeHeadings; h++) foreach (var (jump, sprint) in new[] { (false, false), (false, true), (true, true) })
        {
            tries++; var e = Move(p, Dir(h, set.escapeHeadings), jump, sprint, set.escapeTime); long ek = Key(e);
            if (ok.Contains(ek) || (!pos.ContainsKey(ek) && (e - p).magnitude >= escapeAway)) esc++;
        }
        if (esc == 0) traps++;
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
    // ---- the drawn trees block sight from here on (temporary MeshColliders on each tree's first LOD; removed in finally)
    var tower = Root("Camp").transform.Find("Tower"); float deckTop = tower.Find("Cab").position.y;
    var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>();
    for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid)
        foreach (var hgt in new[] { set.deckEye, set.deckJump }) eyes.Add(new UnityEngine.Vector3(tower.position.x + gx, deckTop + hgt, tower.position.z + gz));
    bool Clear(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform ignore)
    {
        var d = b - a; float len = d.magnitude - set.rayEndSkip; if (len <= 0f) return true;
        foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, len, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore))
            if ((ignore == null || !h.collider.transform.IsChildOf(ignore)) && !h.collider.transform.IsChildOf(pc.transform)) return false;
        return true;
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
    // ---- REACH AND FOUND
    int rfFail = 0; var rfLines = new System.Text.StringBuilder();
    foreach (var pl in A.places)
    {
        var p = Pt(pl.point); float gy = pl.point.y <= groundY ? H(p.x, p.z) : pl.point.y; bool reached = false;
        foreach (var kv in pos) { var d = kv.Value - p; if (new UnityEngine.Vector2(d.x, d.z).magnitude <= set.reachRadius && UnityEngine.Mathf.Abs(kv.Value.y - gy) <= set.reachRise) { reached = true; break; } }
        var aim = new UnityEngine.Vector3(p.x, gy + set.foundAimUp, p.z); float best = -1f;
        foreach (var tp in trailPts) { var d = tp - p; float xz = new UnityEngine.Vector2(d.x, d.z).magnitude; if (xz > set.foundReach) continue; var eye = new UnityEngine.Vector3(tp.x, H(tp.x, tp.z) + eyeH, tp.z); if (Clear(eye, aim, null) && (best < 0f || xz < best)) best = xz; }
        bool found = best >= 0f; if (!reached || !found) rfFail++;
        rfLines.Append("  " + (reached && found ? "ok   " : "FAIL ") + pl.label + " " + P3(aim) + ": " + (reached ? "reached" : "NOT REACHED") + ", " + (found ? "seen from a trail " + F1(best) + " m away" : "NOT SEEN from any trail within " + F1(set.foundReach) + " m") + "\n");
    }
    if (rfFail > 0) fails++;
    sb.Append((rfFail == 0 ? "PASS" : "FAIL") + " REACH AND FOUND: " + (A.places.Length - rfFail) + " of " + A.places.Length + " places reached and seen from a trail\n" + rfLines);
    // ---- DECK must-see (trees block) and the pixel check
    if (A.deckListWritten)
    {
        foreach (var t in A.deckSee)
        {
            var p = Pt(t.point); int clear = 0; foreach (var e in eyes) if (Clear(e, p, tower)) clear++;
            float share = clear / (float)eyes.Count; bool pass = share >= set.mustSeeShare; if (!pass) deckFails++;
            deckLines.Append("  " + (pass ? "ok   " : "HIDDEN ") + "see " + t.label + " " + P3(p) + ": " + clear + " of " + eyes.Count + " rays clear (" + (share * 100f).ToString("F0", inv) + " percent, bar " + (set.mustSeeShare * 100f).ToString("F0", inv) + ")\n");
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
