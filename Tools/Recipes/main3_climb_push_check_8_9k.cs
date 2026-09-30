// Main3 8.9k climb side-push check (Play mode; WalkChecks.md 4 and 8 for the Ward climb only, Wren 2026-09-29). From every J to
// Ward trail point past the cairn gate (the benched legs, P1 to P4, leg 5, the cleft's parts A and B, the exit and the ramp):
// - a 25 m walk straight out to each side (perpendicular to the trail), with gravity;
// - a sprint-jump (5.5 m/s, 0.6 m hop on every landing, 2.2 s, about 12 m) in 12 directions.
// A push passes when the player can walk back to its start point afterwards, never fell more than 2.5 m below it, and did not end
// more than 2.5 m above it (so it neither dropped to a lower bench nor climbed onto a higher one, and is not stuck or off the map).
// The ledge lip: from x -6 every 1.5 m over z 240 to 274, a 12 m walk and a sprint-jump west must stop at x -9.3 or east of it
// and on the ledge (98). The day gate is switched off for the check and back on. Bounded loops only. Reset runInBackground after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, pushWalk = 25f, hopTime = 2.2f, maxDrop = 2.5f, maxGain = 2.5f, lipX = -9.3f, ledgeFloor = 97.5f;
const int hopDirs = 12, gateSkip = 3, walkBackSteps = 8000, stallSteps = 600;
float g = tuning.gravity, vJump = UnityEngine.Mathf.Sqrt(2f * g * tuning.jumpHeight), walkSpeed = tuning.walkSpeed, sprint = tuning.sprintSpeed;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
// walk in a straight line with gravity for up to dist metres; stops when progress stalls; returns the lowest height reached
float Walk(UnityEngine.Vector3 dir, float dist)
{
    float vy = 0f, low = pc.transform.position.y, moved = 0f, lastBest = 0f; int since = 0;
    for (int s = 0; s < 20000 && moved < dist; s++)
    {
        var before = pc.transform.position; var f = cc.Move(new UnityEngine.Vector3(dir.x * walkSpeed, vy, dir.z * walkSpeed) * dt);
        vy = (f & UnityEngine.CollisionFlags.Below) != 0 ? -1f : vy - g * dt;
        var after = pc.transform.position; moved += new UnityEngine.Vector2(after.x - before.x, after.z - before.z).magnitude; low = UnityEngine.Mathf.Min(low, after.y);
        if (moved > lastBest + 0.05f) { lastBest = moved; since = 0; } else if (++since > stallSteps) break;
    }
    return low;
}
float Hop(UnityEngine.Vector3 dir, float speed, float seconds)
{
    float vy = vJump, low = pc.transform.position.y;
    for (float t = 0f; t < seconds; t += dt) { var f = cc.Move(new UnityEngine.Vector3(dir.x * speed, vy, dir.z * speed) * dt); vy -= g * dt; if ((f & UnityEngine.CollisionFlags.Below) != 0) vy = vJump; low = UnityEngine.Mathf.Min(low, pc.transform.position.y); }
    for (int k = 0; k < 60; k++) cc.Move(UnityEngine.Vector3.down * 0.1f);   // land
    return UnityEngine.Mathf.Min(low, pc.transform.position.y);
}
bool WalkBack(UnityEngine.Vector3 target)   // the fixed-step walker of the trail walks, bounded
{
    var t = new UnityEngine.Vector2(target.x, target.z);
    for (int s = 0; s < walkBackSteps; s++)
    {
        var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z); var d = t - flat; if (d.magnitude < 0.4f) return UnityEngine.Mathf.Abs(p.y - target.y) < 1.5f;
        var m = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude); cc.Move(new UnityEngine.Vector3(m.x, -0.15f, m.y));
        var q = pc.transform.position; if (s > 400 && (new UnityEngine.Vector2(q.x, q.z) - flat).magnitude < 0.001f) return false;
    }
    return false;
}
var gate = Root("Ward").transform.Find("CairnGate"); gate.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
var cl = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in Root("Trails").transform.Find("J to Ward")) cl.Add(m.position);
int pushes = 0, fails = 0; float worstDrop = 0f, worstGain = 0f; var failList = new System.Collections.Generic.List<string>();
string Where(UnityEngine.Vector3 p) => p.x < 5.5f && p.z > 225f ? (p.z > 237.2f ? "exit/ramp" : "cleft") : p.x > 70f ? "leg 1" : p.x > 57f ? "leg 2 or P1" : p.x > 44f ? "leg 3 or P2" : p.x > 30f ? "leg 4, P3 or P4" : "leg 5";
for (int i = gateSkip; i < cl.Count; i++)
{
    var a = cl[i]; var b = cl[UnityEngine.Mathf.Min(i + 1, cl.Count - 1)]; if (i == cl.Count - 1) b = a + (a - cl[i - 1]);
    var along = new UnityEngine.Vector3(b.x - a.x, 0f, b.z - a.z).normalized; var side = new UnityEngine.Vector3(along.z, 0f, -along.x);
    var tries = new System.Collections.Generic.List<(string mode, UnityEngine.Vector3 dir)> { ("walk", side), ("walk", -side) };
    for (int k = 0; k < hopDirs; k++) tries.Add(("jump", UnityEngine.Quaternion.Euler(0f, k * 360f / hopDirs, 0f) * UnityEngine.Vector3.forward));
    foreach (var t in tries)
    {
        Put(a); float y0 = pc.transform.position.y;
        float low = t.mode == "walk" ? Walk(t.dir, pushWalk) : Hop(t.dir, sprint, hopTime);
        var e = pc.transform.position; float drop = y0 - low, gain = e.y - y0; pushes++;
        worstDrop = UnityEngine.Mathf.Max(worstDrop, drop); worstGain = UnityEngine.Mathf.Max(worstGain, gain);
        bool back = WalkBack(a);
        if (drop > maxDrop || gain > maxGain || !back)
        {
            fails++;
            if (failList.Count < 25) failList.Add(t.mode + " from " + cl[i].ToString("F1") + " (" + Where(a) + ") toward " + t.dir.ToString("F2") + ": ended " + e.ToString("F1") + ", drop " + drop.ToString("F1") + ", gain " + gain.ToString("F1") + (back ? "" : ", NO WAY BACK"));
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
sb.Append("climb pushes: " + pushes + " from " + (cl.Count - gateSkip) + " trail points (2 walks, " + hopDirs + " sprint-jumps each), " + fails + " fail; worst drop " + worstDrop.ToString("F1") + " m, worst gain " + worstGain.ToString("F1") + " m\n");
foreach (var f in failList) sb.Append("  " + f + "\n");
sb.Append("ledge lip: " + lipTries + " pushes west, " + lipFails + " past x " + lipX + " or off the ledge; westmost x " + lipWest.ToString("F1") + lipFirst + "\n");
sb.Append("ALL " + (fails == 0 && lipFails == 0 ? "PASS" : "FAIL"));
return sb.ToString();
