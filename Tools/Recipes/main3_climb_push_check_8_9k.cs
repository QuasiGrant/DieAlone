// Main3 8.9k climb side-push check (Play mode; WalkChecks.md 4 and 8 for the Ward climb only, Wren 2026-09-29). From every J to
// Ward trail point past the cairn gate (the benched legs, P1 to P4, leg 5, the cleft's parts A and B, the exit and the ramp):
// - a 25 m walk straight out to each side (perpendicular to the trail), with gravity;
// - a sprint-jump (5.5 m/s, 0.6 m hop on every landing, 2.2 s, about 12 m) in 12 directions.
// A push passes when it ends within 4.5 m (3D) of a trail point on the same stretch (at most 20 points, 40 m of trail, from the start, with no fall over 2.5 m,
// so it neither dropped to a lower bench nor climbed onto a higher one) and the player walks back along the trail to the start.
// The ledge lip: from x -6 every 1.5 m over z 240 to 274, a 12 m walk and a sprint-jump west must stop at x -9.3 or east of it
// and on the ledge (98). The day gate is switched off for the check and back on. Bounded loops only. Reset runInBackground after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, pushWalk = 25f, hopTime = 2.2f, lipX = -9.3f, ledgeFloor = 97.5f;
const int hopDirs = 12, gateSkip = 3, walkBackSteps = 8000, stallSteps = 600, shelfWindow = 20;
const float nearTrail = 4.5f, maxDropAllowed = 2.5f, ledgeX0 = -9f, ledgeX1 = 4f, ledgeZ0 = 232f, ledgeZ1 = 275f;
float g = tuning.gravity, vJump = UnityEngine.Mathf.Sqrt(2f * g * tuning.jumpHeight), walkSpeed = tuning.walkSpeed, sprint = tuning.sprintSpeed;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
// walk in a straight line with gravity for up to dist metres; stops when progress stalls; returns the lowest height reached
float maxFall = 0f, airTop = 0f; bool wasGrounded = true;   // WalkChecks: a drop is air-top minus landing height, on every landing
void Air(bool grounded) { float y = pc.transform.position.y; if (grounded) { if (!wasGrounded) maxFall = UnityEngine.Mathf.Max(maxFall, airTop - y); airTop = y; } else airTop = UnityEngine.Mathf.Max(airTop, y); wasGrounded = grounded; }
float Walk(UnityEngine.Vector3 dir, float dist)
{
    float vy = 0f, low = pc.transform.position.y, moved = 0f, lastBest = 0f; int since = 0;
    for (int s = 0; s < 20000 && moved < dist; s++)
    {
        var before = pc.transform.position; var f = cc.Move(new UnityEngine.Vector3(dir.x * walkSpeed, vy, dir.z * walkSpeed) * dt);
        vy = (f & UnityEngine.CollisionFlags.Below) != 0 ? -1f : vy - g * dt; Air((f & UnityEngine.CollisionFlags.Below) != 0);
        var after = pc.transform.position; moved += new UnityEngine.Vector2(after.x - before.x, after.z - before.z).magnitude; low = UnityEngine.Mathf.Min(low, after.y);
        if (moved > lastBest + 0.05f) { lastBest = moved; since = 0; } else if (++since > stallSteps) break;
    }
    return low;
}
float Hop(UnityEngine.Vector3 dir, float speed, float seconds)
{
    float vy = vJump, low = pc.transform.position.y;
    for (float t = 0f; t < seconds; t += dt) { var f = cc.Move(new UnityEngine.Vector3(dir.x * speed, vy, dir.z * speed) * dt); vy -= g * dt; Air((f & UnityEngine.CollisionFlags.Below) != 0); if ((f & UnityEngine.CollisionFlags.Below) != 0) vy = vJump; low = UnityEngine.Mathf.Min(low, pc.transform.position.y); }
    for (int k = 0; k < 60; k++) { var f2 = cc.Move(UnityEngine.Vector3.down * 0.1f); Air((f2 & UnityEngine.CollisionFlags.Below) != 0); }   // land
    return UnityEngine.Mathf.Min(low, pc.transform.position.y);
}
bool WalkBack(UnityEngine.Vector3 target)   // the fixed-step walker of the trail walks, bounded
{
    var t = new UnityEngine.Vector2(target.x, target.z);
    for (int s = 0; s < walkBackSteps; s++)
    {
        var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z); var d = t - flat;
        if (d.magnitude < 0.4f) { for (int k = 0; k < 40; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); return UnityEngine.Mathf.Abs(pc.transform.position.y - target.y) < 1.5f; }   // settle onto the ground first
        var m = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude); cc.Move(new UnityEngine.Vector3(m.x, -0.15f, m.y));
        var q = pc.transform.position; if (s > 400 && (new UnityEngine.Vector2(q.x, q.z) - flat).magnitude < 0.001f) return false;
    }
    return false;
}
var gate = Root("Ward").transform.Find("CairnGate"); gate.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
var cl = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in Root("Trails").transform.Find("J to Ward")) cl.Add(m.position);
var others = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform lg in Root("Trails").transform) if (lg.name != "J to Ward") foreach (UnityEngine.Transform m in lg) others.Add(m.position);
bool WalkBackAlong(int from, int to) { int step = to >= from ? 1 : -1; for (int k = from, n = 0; n <= shelfWindow + 1; k += step, n++) { if (!WalkBack(cl[k])) return false; if (k == to) return true; } return false; }
int pushes = 0, fails = 0; float worstDrop = 0f, worstGain = 0f, worstFall = 0f; var failList = new System.Collections.Generic.List<string>();
string Where(UnityEngine.Vector3 p) => p.x < 5.5f && p.z > 225f ? (p.z > 237.2f ? "exit/ramp" : "cleft") : p.x > 70f ? "leg 1" : p.x > 57f ? "leg 2 or P1" : p.x > 44f ? "leg 3 or P2" : p.x > 30f ? "leg 4, P3 or P4" : "leg 5";
for (int i = gateSkip; i < cl.Count; i++)
{
    var a = cl[i]; var b = cl[UnityEngine.Mathf.Min(i + 1, cl.Count - 1)]; if (i == cl.Count - 1) b = a + (a - cl[i - 1]);
    var along = new UnityEngine.Vector3(b.x - a.x, 0f, b.z - a.z).normalized; var side = new UnityEngine.Vector3(along.z, 0f, -along.x);
    var tries = new System.Collections.Generic.List<(string mode, UnityEngine.Vector3 dir)> { ("walk", side), ("walk", -side) };
    for (int k = 0; k < hopDirs; k++) tries.Add(("jump", UnityEngine.Quaternion.Euler(0f, k * 360f / hopDirs, 0f) * UnityEngine.Vector3.forward));
    foreach (var t in tries)
    {
        Put(a); float y0 = pc.transform.position.y; maxFall = 0f; airTop = y0; wasGrounded = true;
        float low = t.mode == "walk" ? Walk(t.dir, pushWalk) : Hop(t.dir, sprint, hopTime);
        var e = pc.transform.position; float drop = y0 - low, gain = e.y - y0; pushes++;
        worstDrop = UnityEngine.Mathf.Max(worstDrop, drop); worstGain = UnityEngine.Mathf.Max(worstGain, gain);
        // where it ended: the nearest trail point in 3D. A push passes when it ends within reach of the trail on the same stretch
        // (at most shelfWindow points, 2 m apart, from the start: a jump along a 23.5 percent leg rises or falls with the leg, so
        // height is judged against the trail where it lands, not against the start) and the player walks back along the trail
        int ci = 0; float cd = float.MaxValue; for (int k = 0; k < cl.Count; k++) { float d3 = UnityEngine.Vector3.Distance(e, cl[k]); if (d3 < cd) { cd = d3; ci = k; } }
        bool sameStretch = UnityEngine.Mathf.Abs(ci - i) <= shelfWindow, near = cd <= nearTrail;
        // two walkable places beside the climb also count: the ledge top (the Ward, x -9 to 4, z 232 to 275, ground 98), from which
        // the player walks to the path end; and ground within 4.5 m of another trail (J and the Camp to J trail at the climb's foot)
        bool onLedge = e.x >= ledgeX0 && e.x <= ledgeX1 && e.z >= ledgeZ0 && e.z <= ledgeZ1 && e.y >= ledgeFloor;
        float od = float.MaxValue; foreach (var q in others) od = UnityEngine.Mathf.Min(od, UnityEngine.Vector3.Distance(e, q));
        bool nearOther = !near && od <= nearTrail;
        bool back = onLedge ? WalkBack(cl[cl.Count - 1]) : nearOther ? true : near && sameStretch && WalkBackAlong(ci, i); float fall = maxFall; worstFall = UnityEngine.Mathf.Max(worstFall, fall);
        if (!back || fall > maxDropAllowed)
        {
            fails++;
            string why = fall > maxDropAllowed && back ? "a fall of " + fall.ToString("F1") + " m" : !near ? "off the trail (" + cd.ToString("F1") + " m from it)" : !sameStretch ? "on another stretch of the climb (point " + ci + ", started at " + i + ")" : "no way back along the trail";
            if (failList.Count < 25) failList.Add(t.mode + " from " + cl[i].ToString("F1") + " (" + Where(a) + ") toward " + t.dir.ToString("F2") + ": ended " + e.ToString("F1") + ", " + why + ", largest fall " + fall.ToString("F1") + " m");
        }
    }
}
// the ledge lip
int lipTries = 0, lipFails = 0; float lipWest = float.MaxValue; string lipFirst = "";
for (float z = 240f; z <= 274f; z += 1.5f)
    foreach (var mode in new[] { "walk", "jump" })
    {
        Put(new UnityEngine.Vector3(-6f, H(-6f, z), z)); var west = new UnityEngine.Vector3(-1f, 0f, 0f);
        if (mode == "walk") Walk(west, 12f); else Hop(west, sprint, hopTime);
        var e = pc.transform.position; lipTries++; lipWest = UnityEngine.Mathf.Min(lipWest, e.x);
        if (e.x < lipX || e.y < ledgeFloor) { lipFails++; if (lipFirst == "") lipFirst = " first: " + mode + " at z " + z + " ended " + e.ToString("F1"); }
    }
gate.gameObject.SetActive(true); UnityEngine.Physics.SyncTransforms(); pc.enabled = true; UnityEngine.Application.runInBackground = false;
var sb = new System.Text.StringBuilder();
sb.Append("climb pushes: " + pushes + " from " + (cl.Count - gateSkip) + " trail points (2 walks, " + hopDirs + " sprint-jumps each), " + fails + " fail; largest fall (air-top to landing) " + worstFall.ToString("F1") + " m\n");
foreach (var f in failList) sb.Append("  " + f + "\n");
sb.Append("ledge lip: " + lipTries + " pushes west, " + lipFails + " past x " + lipX + " or off the ledge; westmost x " + lipWest.ToString("F1") + lipFirst + "\n");
sb.Append("ALL " + (fails == 0 && lipFails == 0 ? "PASS" : "FAIL"));
return sb.ToString();
