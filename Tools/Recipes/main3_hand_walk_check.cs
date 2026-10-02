// Main3 hand-walk check (Play mode; 8.14a, Marlow's hand-walk list, Gate_8_14_Marlow.md section 13). Run in Main3 after the runner,
// by Tools/Recipes/main3_review_capture.sh after the sheets; never saves the scene. Every move is PlayerController.Step (the game's own
// rules: walk, sprint, jump, the slide off ground over the slope limit), dt 0.02, as in main3_8_14_climb_check.cs, which covers the
// lip, the end walls, the ring rims (sideways pushes from every climb point) and the band arms (IW2). This one adds:
// 1. TRAILS (13.7): every trail, both ways, steered at walk speed point to point with the day gates off (CairnGate, the cave's
//    DayOneBoard), from its first to its last point not inside a solid (a trail may end at a camp's pole); FAIL if the walker stalls.
// 2. W FOOT POCKET (13.3): from a 6 m grid over x 46 to 80, z 165 to 200, a 25 m walk west and 2.2 s sprint-jumps in 12 directions;
//    FAIL if one ends more than pocketGain metres over its start and steeper from it than the slope limit (up the W face, not up a
//    walkable slope).
// 3. PUMP TRENCH (13.4): from the last 30 m of Camp to pump, 20 m walks and sprint-jumps to both sides, and from the pump a walk and
//    sprint-jumps west at the sheer face. Reported, not judged: how many pushes climb out (end over trenchOut metres to the side and
//    more than trenchUp metres over the trail) and the most height gained west of the pump.
// 4. SPIKES (13.5): round each of the three despiked zones (8.1), walks and sprint-jumps in from 16 points just outside; FAIL if one ends
//    within spikeTopDrop metres of the zone's highest ground and within spikeTopReach metres of it (standing on a top). A zone whose
//    top stands less than spikeRise over the highest ground on the ring spikeOut outside it has no spike left and passes as it is.
// 5. FENCE (13.6): a walk north along the inside of the fence (x fenceWalkX, z 160 to 300), reported with where it stalls; and walks and
//    sprint-jumps east every 5 m; FAIL if one ends east of the fence (IW1 and the panels hold).
// 6. TRAPS (8.15, Wren: nobody trapped however they got there): the player is dropped on every trapStep grid point over the climb zone
//    (x -12 to 92, z 190 to 335; not inside rock), left trapSettle seconds to slide, and from each distinct place it settles (trapStep grid) tries 8 walks
//    and 8 sprint-jumps of trapTry metres; FAIL if none ends trapOut metres or more from where it settled. A drop needs no approach, so
//    any trap it lands in fails however it is reached.
// 7. BOULDER POCKETS (8.17 gate, Marlow 1): every boulder cluster, dropped on at 0.5 m and approached from a ring on 16 bearings at
//    walk, sprint and sprint-jump; FAIL if any place the player ends has no way out (see the section).
// Bounded loops only. Restores CairnGate and runInBackground (false) before it returns.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, pushWalk = 25f, sideWalk = 20f, hopTime = 2.2f, pocketGain = 3f, pocketStep = 6f, trenchBack = 30f, trenchStep = 5f, trenchOut = 4f, trenchUp = 1.5f;
const float spikeRise = 2f, spikeTopDrop = 1.5f, spikeTopReach = 3f, spikeOut = 4f, fenceX = 396f, fenceWalkX = 393f, fenceZ0 = 160f, fenceZ1 = 300f, fenceStep = 5f, fencePast = 0.5f;
const int hopDirs = 12, spikeStarts = 16, stallSteps = 600, walkSteps = 8000, trapWays = 8;
const float trapStep = 2.5f, trapSettle = 3f, trapTry = 10f, trapOut = 2f, closedZ = 273.5f, closedX0 = 20f, closedX1 = 64f; var trapX = new UnityEngine.Vector2(-12f, 92f); var trapZ = new UnityEngine.Vector2(190f, 335f);
var pocketX = new UnityEngine.Vector2(46f, 80f); var pocketZ = new UnityEngine.Vector2(165f, 200f);
var spikes = new (UnityEngine.Vector2 c, float r)[] { (new UnityEngine.Vector2(90f, 146f), 12f), (new UnityEngine.Vector2(62f, 48f), 14f), (new UnityEngine.Vector2(118f, 66f), 10f) };   // 8.1 despike zones
var pumpAt = new UnityEngine.Vector2(190f, 97f);
float walkSpeed = tuning.walkSpeed;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
void Walk(UnityEngine.Vector3 dir, float dist)
{
    float moved = 0f, lastBest = 0f; int since = 0;
    for (int s = 0; s < 20000 && moved < dist; s++)
    {
        var before = pc.transform.position; pc.Step(dir, false, false, dt); var after = pc.transform.position;
        moved += new UnityEngine.Vector2(after.x - before.x, after.z - before.z).magnitude;
        if (moved > lastBest + 0.05f) { lastBest = moved; since = 0; } else if (++since > stallSteps) break;
    }
}
void Hop(UnityEngine.Vector3 dir, float seconds)   // sprint with the jump held: a hop on every landing, then land
{
    for (float t = 0f; t < seconds; t += dt) pc.Step(dir, true, true, dt);
    for (int k = 0; k < 60; k++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
}
bool Steer(float x, float z)
{
    var t = new UnityEngine.Vector2(x, z); int since = 0; float best = float.MaxValue;
    for (int s = 0; s < walkSteps; s++)
    {
        var p = pc.transform.position; var d = t - new UnityEngine.Vector2(p.x, p.z); if (d.magnitude < 0.3f) return true;
        if (d.magnitude < best - 0.02f) { best = d.magnitude; since = 0; } else if (++since > stallSteps) return false;
        float frac = UnityEngine.Mathf.Min(1f, d.magnitude / (walkSpeed * dt));
        pc.Step(new UnityEngine.Vector3(d.x, 0f, d.y).normalized * frac, false, false, dt);
    }
    return false;
}
UnityEngine.Vector3 Dir(int k) => UnityEngine.Quaternion.Euler(0f, k * 360f / hopDirs, 0f) * UnityEngine.Vector3.forward;
UnityEngine.Vector3 Ground(float x, float z) => new UnityEngine.Vector3(x, H(x, z), z);
var gate = Root("Ward").transform.Find("CairnGate"); bool gateWas = gate.gameObject.activeSelf;
var board = Root("Cave").transform.Find("Mouth/DayOneBoard"); bool boardWas = board.gameObject.activeSelf;
bool Occupied(UnityEngine.Vector3 at) { foreach (var c in UnityEngine.Physics.OverlapCapsule(at + UnityEngine.Vector3.up * 0.5f, at + UnityEngine.Vector3.up * 1.5f, cc.radius, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) if (!(c is UnityEngine.TerrainCollider) && c != cc) return true; return false; }
var sb = new System.Text.StringBuilder(); bool allPass = true;
try
{
    // ---- 1. every trail, both ways
    gate.gameObject.SetActive(false); board.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
    int legsOk = 0, legsAll = 0; var stuck = new System.Collections.Generic.List<string>();
    foreach (UnityEngine.Transform leg in Root("Trails").transform)
    {
        var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position);
        if (pts.Count < 2) continue;
        foreach (var back in new[] { false, true })
        {
            var l = new System.Collections.Generic.List<UnityEngine.Vector3>(pts); if (back) l.Reverse();
            int first = 0, last = l.Count - 1; while (first < last && Occupied(l[first])) first++; while (last > first && Occupied(l[last])) last--;
            Put(l[first]); bool ok = true; for (int i = first + 1; i <= last && ok; i++) ok = Steer(l[i].x, l[i].z);
            legsAll++; if (ok) legsOk++; else stuck.Add(leg.name + (back ? " back" : " forward") + " stalls at " + pc.transform.position.ToString("F1"));
        }
    }
    if (stuck.Count > 0) allPass = false;
    sb.Append("TRAILS: " + legsOk + " of " + legsAll + " walked end to end (both ways): " + (stuck.Count == 0 ? "PASS" : "FAIL " + string.Join("; ", stuck)) + "\n");
    gate.gameObject.SetActive(gateWas); board.gameObject.SetActive(boardWas); UnityEngine.Physics.SyncTransforms();

    // ---- 2. the W foot pocket
    int pt = 0, pf = 0; float pBest = float.MinValue; string pFirst = "";
    for (float x = pocketX.x; x <= pocketX.y; x += pocketStep)
        for (float z = pocketZ.x; z <= pocketZ.y; z += pocketStep)
        {
            var o = Ground(x, z);
            var tries = new System.Collections.Generic.List<(string mode, UnityEngine.Vector3 dir)> { ("walk", UnityEngine.Vector3.left) };
            for (int k = 0; k < hopDirs; k++) tries.Add(("jump", Dir(k)));
            foreach (var t in tries)
            {
                Put(o); var start = pc.transform.position;
                if (t.mode == "walk") Walk(t.dir, pushWalk); else Hop(t.dir, hopTime);
                var e = pc.transform.position; pt++; float gain = e.y - start.y; pBest = UnityEngine.Mathf.Max(pBest, gain);
                float across = new UnityEngine.Vector2(e.x - start.x, e.z - start.z).magnitude;
                if (gain > pocketGain && gain > across * UnityEngine.Mathf.Tan(cc.slopeLimit * UnityEngine.Mathf.Deg2Rad)) { pf++; if (pFirst == "") pFirst = " first: " + t.mode + " from " + o.ToString("F1") + " toward " + t.dir.ToString("F2") + " ended " + e.ToString("F1"); }
            }
        }
    if (pf > 0) allPass = false;
    sb.Append("W FOOT POCKET: " + pt + " pushes, most height gained " + pBest.ToString("F1") + " m, " + pf + " over " + pocketGain + " m and steeper than the slope limit: " + (pf == 0 ? "PASS" : "FAIL") + pFirst + "\n");

    // ---- 3. the pump trench (reported)
    UnityEngine.Transform pumpLeg = Root("Trails").transform.Find("Camp to pump");
    if (pumpLeg == null) return "no trail Camp to pump";
    var pp = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in pumpLeg) pp.Add(p.position);
    float Len(System.Collections.Generic.List<UnityEngine.Vector3> l) { float s = 0f; for (int i = 1; i < l.Count; i++) s += UnityEngine.Vector3.Distance(l[i - 1], l[i]); return s; }
    UnityEngine.Vector3 At(System.Collections.Generic.List<UnityEngine.Vector3> l, float s)
    {
        if (s <= 0f) return l[0];
        for (int i = 1; i < l.Count; i++) { float d = UnityEngine.Vector3.Distance(l[i - 1], l[i]); if (s <= d) return UnityEngine.Vector3.Lerp(l[i - 1], l[i], d > 0f ? s / d : 0f); s -= d; }
        return l[l.Count - 1];
    }
    float pLen = Len(pp); int tt = 0, tOut = 0; string tFirst = "";
    for (float back = trenchBack; back >= 0f; back -= trenchStep)
    {
        var a = At(pp, pLen - back); var b = At(pp, UnityEngine.Mathf.Min(pLen, pLen - back + 1f)); if (back < 1f) { b = a; a = At(pp, pLen - 1f); }
        var along = new UnityEngine.Vector3(b.x - a.x, 0f, b.z - a.z).normalized; var side = new UnityEngine.Vector3(along.z, 0f, -along.x);
        var o = Ground(At(pp, pLen - back).x, At(pp, pLen - back).z);
        foreach (var d in new[] { side, -side })
            foreach (var mode in new[] { "walk", "jump" })
            {
                Put(o); if (mode == "walk") Walk(d, sideWalk); else Hop(d, hopTime);
                var e = pc.transform.position; tt++;
                float across = UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(new UnityEngine.Vector3(e.x - o.x, 0f, e.z - o.z), side));
                if (across > trenchOut && e.y > o.y + trenchUp) { tOut++; if (tFirst == "") tFirst = " e.g. " + mode + " " + back.ToString("F0") + " m from the pump ended " + e.ToString("F1"); }
            }
    }
    float westGain = float.MinValue; string westAt = "";
    {
        var o = Ground(pumpAt.x, pumpAt.y);
        var tries = new System.Collections.Generic.List<(string mode, UnityEngine.Vector3 dir)> { ("walk", UnityEngine.Vector3.left) };
        for (int k = 0; k < hopDirs; k++) { var d = Dir(k); if (d.x < 0f) tries.Add(("jump", d)); }
        foreach (var t in tries)
        {
            Put(o); if (t.mode == "walk") Walk(t.dir, pushWalk); else Hop(t.dir, hopTime);
            var e = pc.transform.position; if (e.y - o.y > westGain) { westGain = e.y - o.y; westAt = e.ToString("F1"); }
        }
    }
    sb.Append("PUMP TRENCH (report): " + tt + " side pushes from the last " + trenchBack + " m, " + tOut + " climb out" + tFirst + "; west of the pump the most height gained is " + westGain.ToString("F1") + " m (at " + westAt + ")\n");

    // ---- 4. the three spikes
    int st = 0, sf = 0; string sFirst = ""; var tops = new System.Collections.Generic.List<string>();
    foreach (var s in spikes)
    {
        float top = float.MinValue; var topAt = UnityEngine.Vector2.zero;
        for (float x = s.c.x - s.r; x <= s.c.x + s.r; x += 1f) for (float z = s.c.y - s.r; z <= s.c.y + s.r; z += 1f)
            if ((new UnityEngine.Vector2(x, z) - s.c).magnitude <= s.r) { float h = H(x, z); if (h > top) { top = h; topAt = new UnityEngine.Vector2(x, z); } }
        float ring = float.MinValue;
        for (int k = 0; k < spikeStarts; k++) { float ang = k * 360f / spikeStarts * UnityEngine.Mathf.Deg2Rad; var p = s.c + new UnityEngine.Vector2(UnityEngine.Mathf.Sin(ang), UnityEngine.Mathf.Cos(ang)) * (s.r + spikeOut); ring = UnityEngine.Mathf.Max(ring, H(p.x, p.y)); }
        bool spike = top - ring >= spikeRise;
        tops.Add("(" + s.c.x.ToString("F0") + ", " + s.c.y.ToString("F0") + ") top " + top.ToString("F1") + ", " + (top - ring).ToString("F1") + " m over its ring" + (spike ? "" : ", no spike"));
        if (!spike) continue;
        for (int k = 0; k < spikeStarts; k++)
        {
            float ang = k * 360f / spikeStarts * UnityEngine.Mathf.Deg2Rad; var p = s.c + new UnityEngine.Vector2(UnityEngine.Mathf.Sin(ang), UnityEngine.Mathf.Cos(ang)) * (s.r + spikeOut);
            var inward = new UnityEngine.Vector3(topAt.x - p.x, 0f, topAt.y - p.y).normalized;
            foreach (var mode in new[] { "walk", "jump" })
            {
                Put(Ground(p.x, p.y)); if (mode == "walk") Walk(inward, s.r + spikeOut); else Hop(inward, hopTime);
                var e = pc.transform.position; st++;
                if (e.y > top - spikeTopDrop && new UnityEngine.Vector2(e.x - topAt.x, e.z - topAt.y).magnitude <= spikeTopReach) { sf++; if (sFirst == "") sFirst = " first: " + mode + " from " + p.ToString("F1") + " ended " + e.ToString("F1"); }
            }
        }
    }
    if (sf > 0) allPass = false;
    sb.Append("SPIKES: " + string.Join("; ", tops) + "; " + st + " pushes in, " + sf + " on a top: " + (sf == 0 ? "PASS" : "FAIL") + sFirst + "\n");

    // ---- 6. traps
    {
        var settled = new System.Collections.Generic.HashSet<long>(); var spots = new System.Collections.Generic.List<UnityEngine.Vector3>();
        for (float x = trapX.x; x <= trapX.y; x += trapStep) for (float z = trapZ.x; z <= trapZ.y; z += trapStep)
        {
            var g0 = Ground(x, z); if (Occupied(g0)) continue;   // a drop inside rock is no place a player can be
            Put(g0); for (float t = 0f; t < trapSettle; t += dt) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
            var e = pc.transform.position; if (e.z > closedZ && e.x > closedX0 && e.x < closedX1) continue;   // north of the 8.20 giant: closed to the player (main3_8_20_closure_check.cs)
            long key = ((long)UnityEngine.Mathf.FloorToInt(e.x / trapStep) << 32) ^ (uint)UnityEngine.Mathf.FloorToInt(e.z / trapStep);
            if (settled.Add(key)) spots.Add(e);
        }
        int trapped = 0; var trapList = new System.Collections.Generic.List<string>();
        foreach (var sp in spots)
        {
            bool free = false;
            for (int k = 0; k < trapWays * 2 && !free; k++)
            {
                cc.enabled = false; pc.transform.position = sp; cc.enabled = true; UnityEngine.Physics.SyncTransforms();
                var d = UnityEngine.Quaternion.Euler(0f, (k % trapWays) * 360f / trapWays, 0f) * UnityEngine.Vector3.forward;
                if (k < trapWays) Walk(d, trapTry); else Hop(d, hopTime);
                if (UnityEngine.Vector3.Distance(pc.transform.position, sp) >= trapOut) free = true;
            }
            if (!free) { trapped++; if (trapList.Count < 10) trapList.Add(sp.ToString("F1")); }
        }
        if (trapped > 0) allPass = false;
        sb.Append("TRAPS: " + spots.Count + " places the player settles in the climb zone, " + trapped + " with no way out: " + (trapped == 0 ? "PASS" : "FAIL " + string.Join(", ", trapList)) + "\n");
    }

    // ---- 5. the fence line
    Put(Ground(fenceWalkX, fenceZ0)); var stalls = new System.Collections.Generic.List<string>();
    for (float z = fenceZ0 + fenceStep; z <= fenceZ1; z += fenceStep)
        if (!Steer(fenceWalkX, z)) { var at = pc.transform.position; stalls.Add("z " + at.z.ToString("F0") + " (" + at.x.ToString("F1") + ")"); Put(Ground(fenceWalkX, z)); }
    int ft = 0, ff = 0; string fFirst = "";
    var east = new[] { new UnityEngine.Vector3(1f, 0f, 0f), new UnityEngine.Vector3(0.7f, 0f, 0.7f), new UnityEngine.Vector3(0.7f, 0f, -0.7f) };
    for (float z = fenceZ0; z <= fenceZ1; z += fenceStep)
        foreach (var d in east)
            foreach (var mode in new[] { "walk", "jump" })
            {
                Put(Ground(fenceWalkX, z)); if (mode == "walk") Walk(d.normalized, sideWalk); else Hop(d.normalized, hopTime);
                var e = pc.transform.position; ft++;
                if (e.x > fenceX + fencePast) { ff++; if (fFirst == "") fFirst = " first: " + mode + " from z " + z.ToString("F0") + " toward " + d.ToString("F2") + " ended " + e.ToString("F1"); }
            }
    if (ff > 0) allPass = false;
    sb.Append("FENCE: walk north along x " + fenceWalkX + " from z " + fenceZ0 + " to " + fenceZ1 + ": " + (stalls.Count == 0 ? "no stall" : "stalls at " + string.Join(", ", stalls)) + " (report); " + ft + " pushes east, " + ff + " past the fence: " + (ff == 0 ? "PASS" : "FAIL") + fFirst + "\n");
    // 6. BOULDER POCKETS (8.17 gate, Marlow 1 and 2026-10-01: one line from (287, 107) missed the Camp 2 pocket a 1 m grid of approaches
    // found). Every cluster of boulders (colliders named Boulder*, BigBoulders* and the Camp 2 GraniteStack, joined when their bounds come
    // within pocketJoin m) gets (a) a drop on every pocketCell m point over its bounds (on the highest surface, left pocketSettle s), and
    // (b) approaches from a ring pocketRing1 and pocketRing2 m outside its bounds on pocketBearings bearings, aimed at its centre, at each
    // rock and at each settled drop point, at walk, sprint and sprint-jump, for the run there plus pocketOver s. From every distinct place
    // either ends (pocketCell grid), the way out: pocketBearings headings at walk and sprint-jump for pocketTry s each; FAIL when none gets
    // trapOut m away. Marlow's two lines, (287, 107) and (288, 110) toward (290.2, 113.5), run as approaches too.
    {
        const float pocketJoin = 1.5f, pocketCell = 0.5f, pocketSettle = 1f, pocketRing1 = 2f, pocketRing2 = 5f, pocketOver = 1f, pocketTry = 1.5f, pocketDrop = 30f; const int pocketBearings = 16;
        var rockCols = new System.Collections.Generic.List<UnityEngine.Collider>();
        foreach (var c in UnityEngine.Object.FindObjectsByType<UnityEngine.Collider>(UnityEngine.FindObjectsSortMode.None))
            if (c.enabled && !c.isTrigger && (c.name.StartsWith("Boulder") || c.name.StartsWith("BigBoulders") || c.name == "GraniteStack")) rockCols.Add(c);
        var clusters = new System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Collider>>(); var leftR = new System.Collections.Generic.List<UnityEngine.Collider>(rockCols);
        while (leftR.Count > 0)
        {
            var g = new System.Collections.Generic.List<UnityEngine.Collider> { leftR[0] }; leftR.RemoveAt(0);
            for (bool grew = true; grew;) { grew = false; for (int i = leftR.Count - 1; i >= 0; i--) foreach (var q in g) { var a = q.bounds; a.Expand(pocketJoin * 2f); if (a.Intersects(leftR[i].bounds)) { g.Add(leftR[i]); leftR.RemoveAt(i); grew = true; break; } } }
            if (g.Count >= 2) clusters.Add(g);
        }
        var known = new System.Collections.Generic.Dictionary<long, bool>();   // settled cell -> has a way out
        long Key(UnityEngine.Vector3 p) => ((long)UnityEngine.Mathf.FloorToInt(p.x / pocketCell) << 32) ^ (uint)UnityEngine.Mathf.FloorToInt(p.z / pocketCell);
        void PlaceAt(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); }
        bool WayOut(UnityEngine.Vector3 sp)
        {
            if (known.TryGetValue(Key(sp), out bool k0)) return k0;
            bool free = false;
            for (int k = 0; k < pocketBearings * 2 && !free; k++)
            {
                PlaceAt(sp); var d = UnityEngine.Quaternion.Euler(0f, (k % pocketBearings) * 360f / pocketBearings, 0f) * UnityEngine.Vector3.forward; bool hop = k >= pocketBearings;
                for (float t = 0f; t < pocketTry; t += dt) pc.Step(d, hop, hop, dt);
                if (new UnityEngine.Vector2(pc.transform.position.x - sp.x, pc.transform.position.z - sp.z).magnitude >= trapOut) free = true;
            }
            known[Key(sp)] = free; return free;
        }
        UnityEngine.Vector3 Settle(float seconds) { for (float t = 0f; t < seconds; t += dt) pc.Step(UnityEngine.Vector3.zero, false, false, dt); return pc.transform.position; }
        int drops = 0, runs = 0, badStarts = 0; var fails = new System.Collections.Generic.Dictionary<long, string>();   // the first way into each place with no way out
        void Approach(UnityEngine.Vector3 from, UnityEngine.Vector2 to, int mode)
        {
            // a start with no way out of its own is no place a player comes from
            if (Occupied(from)) return; Put(from); var start = Settle(pocketSettle); if (!WayOut(start)) { badStarts++; return; } Put(from);
            var d =new UnityEngine.Vector3(to.x - from.x, 0f, to.y - from.z); float dist = d.magnitude; d.Normalize();
            float speed = mode == 0 ? tuning.walkSpeed : tuning.sprintSpeed; for (float t = 0f; t < dist / speed + pocketOver; t += dt) pc.Step(d, mode == 2, mode > 0, dt);
            var e = Settle(pocketSettle); runs++;
            if (!WayOut(e) && !fails.ContainsKey(Key(e))) fails.Add(Key(e), (mode == 0 ? "walk" : mode == 1 ? "sprint" : "sprint-jump") + " from " + from.ToString("F1") + " toward (" + to.x.ToString("F1") + ", " + to.y.ToString("F1") + ") ends at " + e.ToString("F1"));
        }
        foreach (var cl in clusters)
        {
            var b = cl[0].bounds; foreach (var c in cl) b.Encapsulate(c.bounds);
            var aims = new System.Collections.Generic.List<UnityEngine.Vector2> { new UnityEngine.Vector2(b.center.x, b.center.z) }; foreach (var c in cl) aims.Add(new UnityEngine.Vector2(c.bounds.center.x, c.bounds.center.z));
            for (float x = b.min.x - pocketCell; x <= b.max.x + pocketCell; x += pocketCell) for (float z = b.min.z - pocketCell; z <= b.max.z + pocketCell; z += pocketCell)
            {
                if (!UnityEngine.Physics.Raycast(new UnityEngine.Vector3(x, b.max.y + pocketDrop, z), UnityEngine.Vector3.down, out var hit, b.size.y + pocketDrop * 2f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) continue;
                Put(hit.point); var e = Settle(pocketSettle); drops++;

                if (!WayOut(e)) { aims.Add(new UnityEngine.Vector2(e.x, e.z)); if (!fails.ContainsKey(Key(e))) fails.Add(Key(e), "dropped at (" + x.ToString("F1") + ", " + z.ToString("F1") + ") settles at " + e.ToString("F1")); }
            }
            float half = UnityEngine.Mathf.Max(b.extents.x, b.extents.z);
            foreach (var ring in new[] { half + pocketRing1, half + pocketRing2 })
                for (int k = 0; k < pocketBearings; k++)
                {
                    float a = k * 2f * UnityEngine.Mathf.PI / pocketBearings; float sx = b.center.x + UnityEngine.Mathf.Sin(a) * ring, sz = b.center.z + UnityEngine.Mathf.Cos(a) * ring;
                    foreach (var aim in aims) for (int mode = 0; mode < 3; mode++) Approach(Ground(sx, sz), aim, mode);
                }
        }
        foreach (var from in new[] { new UnityEngine.Vector2(287f, 107f), new UnityEngine.Vector2(288f, 110f) }) for (int mode = 0; mode < 3; mode++) Approach(Ground(from.x, from.y), new UnityEngine.Vector2(290.2f, 113.5f), mode);
        int stuckCells = fails.Count;   // places with no way out that a drop or an approach ends in (a bad ring start is not one)
        if (stuckCells > 0) allPass = false;
        sb.Append("BOULDER POCKETS: " + clusters.Count + " clusters, " + drops + " drops, " + runs + " approaches (" + badStarts + " ring starts left out: no way out of their own), " + known.Count + " places tested, " + stuckCells + " with no way out: " + (stuckCells == 0 ? "PASS" : "FAIL " + string.Join("; ", fails.Values)) + "\n");
    }
}
finally
{
    gate.gameObject.SetActive(gateWas); board.gameObject.SetActive(boardWas); UnityEngine.Physics.SyncTransforms();
    pc.enabled = true; UnityEngine.Application.runInBackground = false;
}
sb.Append("ALL " + (allPass ? "PASS" : "FAIL"));
return sb.ToString();
