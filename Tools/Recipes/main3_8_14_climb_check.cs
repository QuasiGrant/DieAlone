// Main3 8.14 check (Play mode; Valley.md rev 10 sections 4 and 8). Run in Main3 after the runner; never saves the scene.
// 1. Timed walks: camp to J along the Camp to J trail and J to the path end along the J to Ward trail (day gate off), at the walk
//    speed from the grounded horizontal distance the CharacterController covers (4.1: J to the path end 265 m, 106 s; camp to the
//    path end 345 m, 138 s; ceiling 150 s).
// 2. Push checks with the real CharacterController (WalkChecks.md mover: dt 0.02, gravity and jump from PlayerTuning, sprint-jumps
//    hop on every landing), "no way round":
//    a. the climb: from every J to Ward trail point past the chute mouth, a 25 m walk to each side and a 2.2 s sprint-jump in 12
//       directions. Fails if it ends nearer a trail point more than 40 m of walking further on (a skipped leg) or more than 2.5 m
//       above the nearest trail point (up a face), except on the ledge.
//    b. the ledge: pushes west at the lip, south at the S end wall, north at the N end wall and out of all four corners; each must
//       stay on the ledge (x -10 to 6, z 215 to 285, ground 61 or more).
//    c. IW2 by day (CairnGate on): from the valley side along the rock arms and the gap, walks west and sprint-jumps in 12
//       directions; fails if it ends past the band (x under 79.6, or x under 85.6 between z 199.5 and 226.5) or on a rock top.
//    d. IW3 in a shift (ShiftWalls on): from the lot and spur south of the brush band and from west of the W brush band, walks and
//       sprint-jumps; fails if it ends inside the closed campground (x 348.4 to 395.6, z 215.4 to 302) or on the brush.
// Bounded loops only. Restores CairnGate, ShiftWalls and runInBackground (false) before it returns.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, pushWalk = 25f, hopTime = 2.2f, skipWindow = 40f, climbGain = 2.5f, nearMouthX = 86f;
const int hopDirs = 12, stallSteps = 600, walkSteps = 8000;
float g = tuning.gravity, vJump = UnityEngine.Mathf.Sqrt(2f * g * tuning.jumpHeight), walkSpeed = tuning.walkSpeed, sprint = tuning.sprintSpeed;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
float maxFall = 0f, airTop = 0f; bool wasGrounded = true;
void Air(bool grounded) { float y = pc.transform.position.y; if (grounded) { if (!wasGrounded) maxFall = UnityEngine.Mathf.Max(maxFall, airTop - y); airTop = y; } else airTop = UnityEngine.Mathf.Max(airTop, y); wasGrounded = grounded; }
void Walk(UnityEngine.Vector3 dir, float dist)
{
    float vy = 0f, moved = 0f, lastBest = 0f; int since = 0;
    for (int s = 0; s < 20000 && moved < dist; s++)
    {
        var before = pc.transform.position; var f = cc.Move(new UnityEngine.Vector3(dir.x * walkSpeed, vy, dir.z * walkSpeed) * dt);
        vy = (f & UnityEngine.CollisionFlags.Below) != 0 ? -1f : vy - g * dt; Air((f & UnityEngine.CollisionFlags.Below) != 0);
        var after = pc.transform.position; moved += new UnityEngine.Vector2(after.x - before.x, after.z - before.z).magnitude;
        if (moved > lastBest + 0.05f) { lastBest = moved; since = 0; } else if (++since > stallSteps) break;
    }
}
void Hop(UnityEngine.Vector3 dir, float speed, float seconds)
{
    float vy = vJump;
    for (float t = 0f; t < seconds; t += dt) { var f = cc.Move(new UnityEngine.Vector3(dir.x * speed, vy, dir.z * speed) * dt); vy -= g * dt; Air((f & UnityEngine.CollisionFlags.Below) != 0); if ((f & UnityEngine.CollisionFlags.Below) != 0) vy = vJump; }
    for (int k = 0; k < 60; k++) { var f2 = cc.Move(UnityEngine.Vector3.down * 0.1f); Air((f2 & UnityEngine.CollisionFlags.Below) != 0); }   // land
}
// the timed walk moves like PlayerController.Move: walk speed toward the next trail point, vertical velocity -2 while grounded,
// gravity while airborne, every dt; the time is the steps taken (a fixed downward push, as the 8.9j walker, cannot climb the 40
// degree flights of cut steps: its push slid the capsule back down them)
float walkedTime = 0f;
bool Steer(float x, float z)
{
    var t = new UnityEngine.Vector2(x, z); float vy = 0f; int since = 0; float best = float.MaxValue;
    for (int s = 0; s < walkSteps; s++)
    {
        var p = pc.transform.position; var d = t - new UnityEngine.Vector2(p.x, p.z); if (d.magnitude < 0.3f) return true;
        if (d.magnitude < best - 0.02f) { best = d.magnitude; since = 0; } else if (++since > stallSteps) return false;
        var m = d.normalized * UnityEngine.Mathf.Min(walkSpeed, d.magnitude / dt);
        var f = cc.Move(new UnityEngine.Vector3(m.x, vy, m.y) * dt); walkedTime += dt;
        vy = (f & UnityEngine.CollisionFlags.Below) != 0 ? -2f : vy - g * dt;
    }
    return false;
}
UnityEngine.Transform Leg(string n) => Root("Trails").transform.Find(n);
System.Collections.Generic.List<UnityEngine.Vector3> Pts(UnityEngine.Transform leg) { var l = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) l.Add(p.position); return l; }
var gate = Root("Ward").transform.Find("CairnGate"); var shift = Root("FrontZone").transform.Find("ShiftWalls");
bool gateWas = gate.gameObject.activeSelf, shiftWas = shift.gameObject.activeSelf;
var sb = new System.Text.StringBuilder(); bool allPass = true;
try
{
    // ---- 1. timed walks
    gate.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
    var campJ = Pts(Leg("Camp to J")); var climb = Pts(Leg("J to Ward"));
    float PathLen(System.Collections.Generic.List<UnityEngine.Vector3> l) { float s = 0f; for (int i = 1; i < l.Count; i++) s += UnityEngine.Vector2.Distance(new UnityEngine.Vector2(l[i - 1].x, l[i - 1].z), new UnityEngine.Vector2(l[i].x, l[i].z)); return s; }
    Put(campJ[0]); walkedTime = 0f; bool okA = true; for (int i = 1; i < campJ.Count && okA; i++) okA = Steer(campJ[i].x, campJ[i].z); float tCampJ = walkedTime;
    Put(climb[0]); walkedTime = 0f; bool okB = true; for (int i = 1; i < climb.Count && okB; i++) okB = Steer(climb[i].x, climb[i].z); float tJ = walkedTime; var endAt = pc.transform.position;
    float tAll = tCampJ + tJ;
    bool timeOk = okA && okB && tAll <= 150f; if (!timeOk) allPass = false;
    sb.Append("TIMED (real mover, walk " + walkSpeed + " m/s): camp to J " + (okA ? "" : "STUCK ") + PathLen(campJ).ToString("F1") + " m " + tCampJ.ToString("F1") + " s; J to the path end " + (okB ? "reached" : "STUCK at " + endAt.ToString("F1")) + " " + PathLen(climb).ToString("F1") + " m " + tJ.ToString("F1") + " s (paper 265 m, 106 s); camp to the path end " + tAll.ToString("F1") + " s (paper 138 s; ceiling 150 s): " + (timeOk ? "PASS" : "FAIL") + "\n");

    // ---- 2a. climb pushes
    var cs = new float[climb.Count]; for (int i = 1; i < climb.Count; i++) cs[i] = cs[i - 1] + UnityEngine.Vector2.Distance(new UnityEngine.Vector2(climb[i - 1].x, climb[i - 1].z), new UnityEngine.Vector2(climb[i].x, climb[i].z));
    bool OnLedge(UnityEngine.Vector3 e) => e.x > -10f && e.x < 6f && e.z > 215f && e.z < 285f && e.y > 60.5f;
    int pushes = 0, fails = 0; float worstFall = 0f; var failList = new System.Collections.Generic.List<string>();
    for (int i = 0; i < climb.Count; i++)
    {
        var a = climb[i]; if (a.x > nearMouthX - 0.5f) continue;   // past the chute mouth only
        var b = climb[UnityEngine.Mathf.Min(i + 1, climb.Count - 1)]; if (i == climb.Count - 1) b = a + (a - climb[i - 1]);
        var along = new UnityEngine.Vector3(b.x - a.x, 0f, b.z - a.z).normalized; var side = new UnityEngine.Vector3(along.z, 0f, -along.x);
        var tries = new System.Collections.Generic.List<(string mode, UnityEngine.Vector3 dir)> { ("walk", side), ("walk", -side) };
        for (int k = 0; k < hopDirs; k++) tries.Add(("jump", UnityEngine.Quaternion.Euler(0f, k * 360f / hopDirs, 0f) * UnityEngine.Vector3.forward));
        foreach (var t in tries)
        {
            Put(a); maxFall = 0f; airTop = pc.transform.position.y; wasGrounded = true;
            if (t.mode == "walk") Walk(t.dir, pushWalk); else Hop(t.dir, sprint, hopTime);
            var e = pc.transform.position; pushes++; worstFall = UnityEngine.Mathf.Max(worstFall, maxFall);
            int ci = 0; float cd = float.MaxValue; for (int k = 0; k < climb.Count; k++) { float d3 = UnityEngine.Vector3.Distance(e, climb[k]); if (d3 < cd) { cd = d3; ci = k; } }
            bool skipped = cs[ci] > cs[i] + skipWindow, up = e.y > climb[ci].y + climbGain && !OnLedge(e);
            if (skipped || up)
            {
                fails++;
                if (failList.Count < 20) failList.Add(t.mode + " from " + a.ToString("F1") + " (" + cs[i].ToString("F0") + " m) toward " + t.dir.ToString("F2") + ": ended " + e.ToString("F1") + (skipped ? ", " + (cs[ci] - cs[i]).ToString("F0") + " m further on" : "") + (up ? ", " + (e.y - climb[ci].y).ToString("F1") + " m over the trail" : ""));
            }
        }
    }
    if (fails > 0) allPass = false;
    sb.Append("CLIMB: " + pushes + " pushes, " + fails + " fail (skip or up a face); largest fall " + worstFall.ToString("F1") + " m: " + (fails == 0 ? "PASS" : "FAIL") + "\n");
    foreach (var f in failList) sb.Append("  " + f + "\n");

    // ---- 2b. the ledge: lip, both end walls, corners
    int lt = 0, lf = 0, lSkipped = 0; string lFirst = "";
    bool Occupied(UnityEngine.Vector3 at) { foreach (var c in UnityEngine.Physics.OverlapCapsule(at + UnityEngine.Vector3.up * 0.5f, at + UnityEngine.Vector3.up * 1.5f, cc.radius, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) if (!(c is UnityEngine.TerrainCollider) && c != cc) return true; return false; }
    var lp = new System.Collections.Generic.List<(UnityEngine.Vector3 from, UnityEngine.Vector3 dir)>();
    for (float z = 218f; z <= 282f; z += 1.5f) lp.Add((new UnityEngine.Vector3(-7f, 0f, z), new UnityEngine.Vector3(-1f, 0f, 0f)));
    for (float x = -9.4f; x <= 5.5f; x += 1.5f) { lp.Add((new UnityEngine.Vector3(x, 0f, 220f), new UnityEngine.Vector3(0f, 0f, -1f))); lp.Add((new UnityEngine.Vector3(x, 0f, 280f), new UnityEngine.Vector3(0f, 0f, 1f))); }
    foreach (var c in new[] { (-8f, 218f, -1f, -1f), (4f, 218f, 1f, -1f), (-8f, 282f, -1f, 1f), (4f, 282f, 1f, 1f) }) lp.Add((new UnityEngine.Vector3(c.Item1, 0f, c.Item2), new UnityEngine.Vector3(c.Item3, 0f, c.Item4).normalized));
    foreach (var p in lp)
        foreach (var mode in new[] { "walk", "jump" })
        {
            var sp = new UnityEngine.Vector3(p.from.x, H(p.from.x, p.from.z), p.from.z); if (Occupied(sp)) { lSkipped++; continue; }   // a start inside a stone is no start
            Put(sp);
            if (mode == "walk") Walk(p.dir, 12f); else Hop(p.dir, sprint, hopTime);
            var e = pc.transform.position; lt++;
            if (!OnLedge(e)) { lf++; if (lFirst == "") lFirst = " first: " + mode + " from " + p.from.ToString("F1") + " ended " + e.ToString("F1"); }
        }
    if (lf > 0) allPass = false;
    sb.Append("LEDGE: " + lt + " pushes at the lip, both end walls and the corners (" + lSkipped + " starts inside a stone left out), " + lf + " off the ledge: " + (lf == 0 ? "PASS" : "FAIL") + lFirst + "\n");

    // ---- 2c. IW2 by day
    gate.gameObject.SetActive(true); UnityEngine.Physics.SyncTransforms();
    bool PastBand(UnityEngine.Vector3 e) => (e.z >= 195f && e.z <= 290f && e.x < 79.6f) || (e.x < 85.6f && e.z > 199.5f && e.z < 226.5f) || e.y > H(e.x, e.z) + 2f;   // behind the band, in the arms' bay, or up on rock
    int it = 0, ifl = 0; string iFirst = "";
    var ip = new System.Collections.Generic.List<UnityEngine.Vector3>();
    for (float z = 198f; z <= 228f; z += 1.5f) ip.Add(new UnityEngine.Vector3(87.2f, 0f, z));
    for (float x = 80.5f; x <= 86f; x += 1.5f) { ip.Add(new UnityEngine.Vector3(x, 0f, 198.8f)); ip.Add(new UnityEngine.Vector3(x, 0f, 227.2f)); }
    foreach (var o in ip)
    {
        var tries = new System.Collections.Generic.List<(string mode, UnityEngine.Vector3 dir)> { ("walk", new UnityEngine.Vector3(-1f, 0f, 0f)) };
        for (int k = 0; k < hopDirs; k++) tries.Add(("jump", UnityEngine.Quaternion.Euler(0f, k * 360f / hopDirs, 0f) * UnityEngine.Vector3.forward));
        foreach (var t in tries)
        {
            Put(new UnityEngine.Vector3(o.x, H(o.x, o.z), o.z));
            if (t.mode == "walk") Walk(t.dir, pushWalk); else Hop(t.dir, sprint, hopTime);
            var e = pc.transform.position; it++;
            if (PastBand(e)) { ifl++; if (iFirst == "") iFirst = " first: " + t.mode + " from " + o.ToString("F1") + " toward " + t.dir.ToString("F2") + " ended " + e.ToString("F1"); }
        }
    }
    if (ifl > 0) allPass = false;
    sb.Append("IW2: " + it + " pushes from the valley side of the chute gap and arms, " + ifl + " past the band: " + (ifl == 0 ? "PASS" : "FAIL") + iFirst + "\n");

    // ---- 2d. IW3 in a shift
    shift.gameObject.SetActive(true); UnityEngine.Physics.SyncTransforms();
    bool InCampground(UnityEngine.Vector3 e) => (e.x > 348.4f && e.x < 395.6f && e.z > 215.4f && e.z < 302f) || (e.x > 338f && e.x < 390f && e.z > 204f && e.z < 312f && e.y > H(e.x, e.z) + 1f);   // inside, or up on the brush
    int st = 0, sf = 0; string sFirst = "";
    var so = new System.Collections.Generic.List<(UnityEngine.Vector3 o, UnityEngine.Vector3[] dirs)>();
    var north = new[] { new UnityEngine.Vector3(0f, 0f, 1f), new UnityEngine.Vector3(0.7f, 0f, 0.7f), new UnityEngine.Vector3(-0.7f, 0f, 0.7f) };
    for (float x = 346f; x <= 395f; x += 2f) so.Add((new UnityEngine.Vector3(x, 0f, 205f), north));
    for (float z = 197f; z <= 209f; z += 3f) so.Add((new UnityEngine.Vector3(390f, 0f, z), north));
    for (float z = 208f; z <= 302f; z += 6f) so.Add((new UnityEngine.Vector3(337.5f, 0f, z), new[] { new UnityEngine.Vector3(1f, 0f, 0f), new UnityEngine.Vector3(0.7f, 0f, 0.7f), new UnityEngine.Vector3(0.7f, 0f, -0.7f) }));
    foreach (var s in so)
        foreach (var d in s.dirs)
            foreach (var mode in new[] { "walk", "jump" })
            {
                Put(new UnityEngine.Vector3(s.o.x, H(s.o.x, s.o.z), s.o.z));
                if (mode == "walk") Walk(d.normalized, 60f); else Hop(d.normalized, sprint, hopTime);
                var e = pc.transform.position; st++;
                if (InCampground(e)) { sf++; if (sFirst == "") sFirst = " first: " + mode + " from " + s.o.ToString("F1") + " toward " + d.ToString("F2") + " ended " + e.ToString("F1"); }
            }
    if (sf > 0) allPass = false;
    sb.Append("IW3: " + st + " pushes from the lot, the spur and west of the brush, " + sf + " into the closed campground: " + (sf == 0 ? "PASS" : "FAIL") + sFirst + "\n");
}
finally
{
    gate.gameObject.SetActive(gateWas); shift.gameObject.SetActive(shiftWas); UnityEngine.Physics.SyncTransforms();
    pc.enabled = true; UnityEngine.Application.runInBackground = false;
}
sb.Append("ALL " + (allPass ? "PASS" : "FAIL"));
return sb.ToString();
