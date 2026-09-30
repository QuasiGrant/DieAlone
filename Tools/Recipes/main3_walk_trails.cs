// Main3 trail walk (Play mode): for every leg under "Trails" (built by 8.3), teleport to its first centre-line point
// and drive the CharacterController through the rest, then back again, the day-one gates off. Reports stalls, stuck points and height range.
// Reset runInBackground to false after the check.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject trails = null;
foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == "Trails") trails = r;
if (trails == null) return "no Trails root";
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
var cc = pc.GetComponent<UnityEngine.CharacterController>();
pc.enabled = false;
// the day-one gates (the cairn chain on the Ward climb, the cave board) are switched off for the walk and back on after; they have
// their own check (WalkChecks.md 1 and 6; 8.9k)
var dayGates = new System.Collections.Generic.List<UnityEngine.GameObject>();
foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
{ if (r.name == "Ward") { var g = r.transform.Find("CairnGate"); if (g != null) dayGates.Add(g.gameObject); } if (r.name == "Cave") { var g = r.transform.Find("Mouth/DayOneBoard"); if (g != null) dayGates.Add(g.gameObject); } }
foreach (var g in dayGates) g.SetActive(false); UnityEngine.Physics.SyncTransforms();
var sb = new System.Text.StringBuilder(); int bad = 0;
foreach (UnityEngine.Transform leg in trails.transform)
{
    var pts = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (UnityEngine.Transform p in leg) pts.Add(p.position);
    foreach (var reverse in new[] { false, true })
    {
        var route = new System.Collections.Generic.List<UnityEngine.Vector3>(pts); if (reverse) route.Reverse();
        cc.enabled = false; pc.transform.position = route[0] + UnityEngine.Vector3.up * 1.2f; cc.enabled = true; UnityEngine.Physics.SyncTransforms();
        for (int k = 0; k < 30; k++) cc.Move(UnityEngine.Vector3.down * 0.1f);
        int stalls = 0; float minY = 999f, maxY = -999f; string stuck = "";
        for (int i = 1; i < route.Count && stuck == ""; i++)
        {
            var target = new UnityEngine.Vector2(route[i].x, route[i].z); int steps = 0;
            while (true)
            {
                var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z); var d = target - flat;
                if (d.magnitude < 0.3f) break;
                if (++steps > 6000) { stuck = p.ToString("F1") + " toward " + target; break; }
                var dir = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude);
                cc.Move(new UnityEngine.Vector3(dir.x, -0.15f, dir.y));
                var after = pc.transform.position;
                if ((new UnityEngine.Vector2(after.x, after.z) - flat).magnitude < 0.005f) stalls++;
                minY = UnityEngine.Mathf.Min(minY, after.y); maxY = UnityEngine.Mathf.Max(maxY, after.y);
            }
        }
        float endErr = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(pc.transform.position.x, pc.transform.position.z), new UnityEngine.Vector2(route[route.Count - 1].x, route[route.Count - 1].z));
        if (stuck != "" || stalls > 0) bad++;
        sb.Append(leg.name + (reverse ? " (back)" : "") + ": " + (stuck != "" ? "STUCK at " + stuck : "ok, end off by " + endErr.ToString("F1") + " m") + " stalls=" + stalls + " y[" + minY.ToString("F1") + "," + maxY.ToString("F1") + "]\n");
    }
}
foreach (var g in dayGates) g.SetActive(true); UnityEngine.Physics.SyncTransforms();
pc.enabled = true;
return "legs with trouble: " + bad + "\n" + sb;
