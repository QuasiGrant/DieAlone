// Main3 8.9c re-walk 2 check, edit mode. (1) Camp trail ends: trail ground at 12, 18 and 25 m from the camp centre against
// the 15 m knoll, and the steepest 10 m window of each camp leg. (2) Ward stones: eyes 1.6 m over the ground every 0.5 m
// along the J to Ward leg; a stone counts as seen when a ray on the default layers reaches it first (12 points per stone).
// Reports the first seen point; 8.9j: the stones must first show past the fin's north end (the reveal: x under 5.5, z over 237.2).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var terrain = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
var sb = new System.Text.StringBuilder();
var camp = new UnityEngine.Vector2(170f, 160f);
// (1) camp legs
foreach (UnityEngine.Transform leg in Root("Trails").transform)
{
    if (leg.childCount < 2) continue;
    var a = leg.GetChild(0).position; var b = leg.GetChild(leg.childCount - 1).position;
    bool atStart = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(a.x, a.z), camp) < 20f, atEnd = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(b.x, b.z), camp) < 20f;
    if (!atStart && !atEnd) continue;
    var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in leg) pts.Add(m.position); if (atEnd) pts.Reverse();
    // resample at 0.5 m from the camp end
    var s = new System.Collections.Generic.List<UnityEngine.Vector3>(); s.Add(pts[0]);
    for (int i = 1; i < pts.Count; i++) { var d = pts[i] - pts[i - 1]; float l = new UnityEngine.Vector2(d.x, d.z).magnitude; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(l / 0.5f)); for (int k = 1; k <= n; k++) { var q = pts[i - 1] + d * (k / (float)n); q.y = H(q.x, q.z); s.Add(q); } }
    string At(float r) { foreach (var q in s) if (UnityEngine.Vector2.Distance(new UnityEngine.Vector2(q.x, q.z), camp) >= r) return q.y.ToString("F1"); return "-"; }
    float worst = 0f; float along = 0f, worstAt = 0f; var acc = new System.Collections.Generic.List<float> { 0f };
    for (int i = 1; i < s.Count; i++) acc.Add(acc[i - 1] + new UnityEngine.Vector2(s[i].x - s[i - 1].x, s[i].z - s[i - 1].z).magnitude);
    for (int i = 0, j = 0; i < s.Count; i++) { while (j < s.Count - 1 && acc[j] - acc[i] < 10f) j++; if (acc[j] - acc[i] >= 9.9f) { float g = UnityEngine.Mathf.Abs(s[j].y - s[i].y) / (acc[j] - acc[i]); if (g > worst) { worst = g; worstAt = acc[i]; } } }
    // trench check: trail ground against the ground 3 m to each side, within 30 m of camp
    float trench = 0f;
    for (int i = 1; i < s.Count; i++)
    {
        if (UnityEngine.Vector2.Distance(new UnityEngine.Vector2(s[i].x, s[i].z), camp) > 30f) break;
        var dir = new UnityEngine.Vector2(s[i].x - s[i - 1].x, s[i].z - s[i - 1].z).normalized; var side = new UnityEngine.Vector2(dir.y, -dir.x);
        foreach (var sg in new[] { -3f, 3f }) trench = UnityEngine.Mathf.Max(trench, H(s[i].x + side.x * sg, s[i].z + side.y * sg) - s[i].y);
    }
    sb.Append(leg.name + ": ground at r12 " + At(12f) + ", r18 " + At(18f) + ", r25 " + At(25f) + " (knoll 15); steepest 10 m " + (worst * 100f).ToString("F1") + "% at " + worstAt.ToString("F0") + " m; deepest bank within 30 m " + trench.ToString("F1") + " m\n");
}
// (2) stones
var stones = Root("Ward").transform.Find("Stones");
var targets = new System.Collections.Generic.List<(UnityEngine.Collider c, UnityEngine.Vector3 p)>();
foreach (UnityEngine.Transform st in stones)
{
    var bnd = st.GetComponent<UnityEngine.Collider>().bounds; var col = st.GetComponent<UnityEngine.Collider>();
    foreach (var fx in new[] { 0.05f, 0.5f, 0.95f }) foreach (var fz in new[] { 0.05f, 0.95f }) foreach (var fy in new[] { 0.7f, 0.98f })
        targets.Add((col, new UnityEngine.Vector3(UnityEngine.Mathf.Lerp(bnd.min.x, bnd.max.x, fx), UnityEngine.Mathf.Lerp(bnd.min.y, bnd.max.y, fy), UnityEngine.Mathf.Lerp(bnd.min.z, bnd.max.z, fz))));
}
var legT = Root("Trails").transform.Find("J to Ward"); var wp = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in legT) wp.Add(m.position);
float total = 0f, firstSeenAlong = -1f; UnityEngine.Vector3 firstSeen = default; int seenBeforeCrest = 0, samples = 0, seenAfterCrest = 0, afterCrest = 0;
for (int i = 1; i < wp.Count; i++)
{
    var d = wp[i] - wp[i - 1]; float l = new UnityEngine.Vector2(d.x, d.z).magnitude; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(l / 0.5f));
    for (int k = 1; k <= n; k++)
    {
        var q = wp[i - 1] + d * (k / (float)n); var eye = new UnityEngine.Vector3(q.x, H(q.x, q.z) + 1.6f, q.z); total += l / n;
        bool seen = false;
        foreach (var t in targets)
        {
            var v = t.p - eye; if (UnityEngine.Physics.Raycast(eye, v.normalized, out var hit, v.magnitude + 0.5f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore) && hit.collider == t.c) { seen = true; break; }
        }
        bool before = !(q.x < 5.5f && q.z > 237.2f); if (before) samples++; else afterCrest++;   // 8.9j: before the reveal
        if (seen && firstSeenAlong < 0f) { firstSeenAlong = total; firstSeen = q; }
        if (seen && before) { seenBeforeCrest++; if (seenBeforeCrest <= 40) sb.Append(q.x.ToString("F1") + "," + q.z.ToString("F1") + " "); } if (seen && !before) seenAfterCrest++;
    }
}
sb.Append("Stones: leg " + total.ToString("F1") + " m; first seen at " + (firstSeenAlong < 0 ? "never" : firstSeenAlong.ToString("F1") + " m, " + firstSeen.ToString("F1")) + "; seen from " + seenBeforeCrest + " of " + samples + " points before the reveal (past the fin's north end), " + seenAfterCrest + " of " + afterCrest + " from there to the path end; " + (seenBeforeCrest == 0 ? "PASS" : "FAIL"));
return sb.ToString();
