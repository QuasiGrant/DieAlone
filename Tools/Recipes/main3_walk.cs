// Main3 walk check (Play mode). Drives the Player's CharacterController along named waypoint routes with the
// controller's own collision, gravity pushed each step, and reports stalls, end points and height range.
// Edit ROUTES below per check. Needs Play mode and runInBackground (set here; reset it after the check).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
var cc = pc.GetComponent<UnityEngine.CharacterController>();
pc.enabled = false;
var V2 = new System.Func<float, float, UnityEngine.Vector2>((x, z) => new UnityEngine.Vector2(x, z));
var routes = new System.Collections.Generic.List<(string name, bool teleport, UnityEngine.Vector3 start, UnityEngine.Vector2[] pts)> { ROUTES };
var sb = new System.Text.StringBuilder();
foreach (var r in routes)
{
    if (r.teleport) { cc.enabled = false; pc.transform.position = r.start; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
    int stalls = 0; float maxY = -999f, minY = 999f, walked = 0f; string firstStall = ""; bool stuck = false;
    foreach (var target in r.pts)
    {
        int steps = 0;
        while (steps < 6000)
        {
            var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z);
            var d = target - flat; if (d.magnitude < 0.3f) break;
            var dir = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude);
            cc.Move(new UnityEngine.Vector3(dir.x, -0.15f, dir.y));
            var after = new UnityEngine.Vector2(pc.transform.position.x, pc.transform.position.z);
            float moved = (after - flat).magnitude; walked += moved;
            if (moved < 0.005f) { stalls++; if (firstStall == "") firstStall = p.ToString("F1") + " toward " + target; }
            maxY = UnityEngine.Mathf.Max(maxY, pc.transform.position.y); minY = UnityEngine.Mathf.Min(minY, pc.transform.position.y);
            steps++;
        }
        if (steps >= 6000) { stuck = true; sb.Append(r.name + ": STUCK at " + pc.transform.position.ToString("F2") + " toward " + target + "\n"); break; }
    }
    if (!stuck) sb.Append(r.name + ": end=" + pc.transform.position.ToString("F2") + " walked=" + walked.ToString("F0") + "m stalls=" + stalls + (firstStall != "" ? " first=" + firstStall : "") + " y[" + minY.ToString("F2") + "," + maxY.ToString("F2") + "]\n");
}
pc.enabled = true;
return sb.ToString();
