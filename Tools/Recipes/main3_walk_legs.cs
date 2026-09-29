// Main3 reach check (Play mode): walks a chain of trail legs (by name under "Trails", optionally reversed) and extra
// waypoints, starting at the first point, and reports where the player ended and any stuck point.
// Edit PLAN below: each step is ("leg:<name>" | "leg-back:<name>" | "to:x,z"); add "|stop=<m>" to end a leg that far short of its
// last point and "|start=<m>" to begin it that far from its first (for legs that meet inside a structure, like the Camp 2 stack).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
var plan = new[] { PLAN };
UnityEngine.GameObject trails = null;
foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == "Trails") trails = r;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var pts = new System.Collections.Generic.List<UnityEngine.Vector3>();
foreach (var step in plan)
{
    if (step.StartsWith("to:")) { var xz = step.Substring(3).Split(','); pts.Add(new UnityEngine.Vector3(float.Parse(xz[0], System.Globalization.CultureInfo.InvariantCulture), float.NaN, float.Parse(xz[1], System.Globalization.CultureInfo.InvariantCulture))); continue; }
    bool back = step.StartsWith("leg-back:"); string name = step.Substring(back ? 9 : 4); float stop = 0f, skip = 0f;
    if (name.Contains("|start=")) { var k = name.IndexOf("|start="); var rest = name.Substring(k + 7); var end = rest.IndexOf("|"); skip = float.Parse(end < 0 ? rest : rest.Substring(0, end), System.Globalization.CultureInfo.InvariantCulture); name = name.Substring(0, k) + (end < 0 ? "" : rest.Substring(end)); }
    if (name.Contains("|stop=")) { stop = float.Parse(name.Substring(name.IndexOf("|stop=") + 6), System.Globalization.CultureInfo.InvariantCulture); name = name.Substring(0, name.IndexOf("|stop=")); }
    var leg = trails.transform.Find(name); if (leg == null) return "no leg " + name;
    var l = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) l.Add(p.position); if (back) l.Reverse();
    if (skip > 0f) { var first = l[0]; l.RemoveAll(q => UnityEngine.Vector2.Distance(new UnityEngine.Vector2(q.x, q.z), new UnityEngine.Vector2(first.x, first.z)) < skip); }
    if (stop > 0f) { var last = l[l.Count - 1]; l.RemoveAll(q => UnityEngine.Vector2.Distance(new UnityEngine.Vector2(q.x, q.z), new UnityEngine.Vector2(last.x, last.z)) < stop); }
    pts.AddRange(l);
}
var start = pts[0]; if (float.IsNaN(start.y)) start.y = UnityEngine.Terrain.activeTerrain.SampleHeight(start) - 45f;
cc.enabled = false; pc.transform.position = start + UnityEngine.Vector3.up * 1.2f; cc.enabled = true; UnityEngine.Physics.SyncTransforms();
for (int k = 0; k < 30; k++) cc.Move(UnityEngine.Vector3.down * 0.1f);
var sb = new System.Text.StringBuilder(); float walked = 0f;
for (int i = 1; i < pts.Count; i++)
{
    var target = new UnityEngine.Vector2(pts[i].x, pts[i].z); int steps = 0;
    while (true)
    {
        var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z); var d = target - flat;
        if (d.magnitude < 0.3f) break;
        if (++steps > 6000) { pc.enabled = true; return sb + "STUCK at " + p.ToString("F2") + " toward " + target + " after " + walked.ToString("F0") + " m"; }
        var dir = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude);
        cc.Move(new UnityEngine.Vector3(dir.x, -0.15f, dir.y));
        walked += (new UnityEngine.Vector2(pc.transform.position.x, pc.transform.position.z) - flat).magnitude;
    }
    if (float.IsNaN(pts[i].y)) sb.Append("reached (" + pts[i].x + ", " + pts[i].z + ") at y " + pc.transform.position.y.ToString("F2") + "; ");
}
pc.enabled = true;
return sb + "done, walked " + walked.ToString("F0") + " m, end " + pc.transform.position.ToString("F2");
