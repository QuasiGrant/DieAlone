// Main3 8.9k, WalkChecks 11 for the Ward climb (edit mode, read-only): the longest straight view along the J to Ward trail, per
// part of the climb. From each trail point, both walking directions, the view reaches the farthest later point for which every
// trail point up to it is visible (eye 1.6, target 0.3 m over the trail, default layers, so the thicket walls do not count).
// Parts: J to leg 1, the four legs (Sable's ruling covers them), leg 5, the cleft (parts A and B), the exit and ramp. A view is
// counted against the part its eye stands on. Pass: 60 m or less except J to leg 1, the four legs and leg 5 (views up and across
// the W ridge face, ruled covered: Sable for the legs, Wren 2026-09-29 for J to leg 1 and leg 5).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
const float eye = 1.6f, target = 0.3f, limit = 60f;
var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in Root("Trails").transform.Find("J to Ward")) pts.Add(m.position);
string Part(UnityEngine.Vector3 p)
{
    if (p.x > 74f) return "J to leg 1";
    if (p.z > 225f && p.x < 22f) return p.z > 237.2f || p.x < 3f ? "exit and ramp" : "cleft";
    if (p.x < 31f || (p.z < 204f && p.x < 33f)) return "leg 5";
    return "legs 1 to 4";
}
var best = new System.Collections.Generic.Dictionary<string, (float d, UnityEngine.Vector3 a, UnityEngine.Vector3 b)>();
foreach (var rev in new[] { false, true })
{
    var s = new System.Collections.Generic.List<UnityEngine.Vector3>(pts); if (rev) s.Reverse();
    for (int i = 0; i < s.Count; i++)
    {
        var e = s[i] + UnityEngine.Vector3.up * eye; int j = i;
        for (int k = i + 1; k < s.Count; k++) { if (UnityEngine.Physics.Linecast(e, s[k] + UnityEngine.Vector3.up * target, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) break; j = k; }
        float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(s[i].x, s[i].z), new UnityEngine.Vector2(s[j].x, s[j].z));
        string part = Part(s[i]); if (!best.ContainsKey(part) || d > best[part].d) best[part] = (d, s[i], s[j]);
    }
}
var sb = new System.Text.StringBuilder(); bool ok = true;
foreach (var kv in best)
{
    // Wren 2026-09-29: views up and across the W ridge face count as covered (J to leg 1, leg 5; Sable for the four legs)
    bool ruled = kv.Key == "legs 1 to 4" || kv.Key == "J to leg 1" || kv.Key == "leg 5"; bool pass = ruled || kv.Value.d <= limit; ok &= pass;
    sb.Append(kv.Key + ": " + kv.Value.d.ToString("F0") + " m, " + kv.Value.a.ToString("F0") + " to " + kv.Value.b.ToString("F0") + (ruled ? " (ruled covered)" : pass ? "" : " OVER 60") + "\n");
}
return (ok ? "PASS" : "FAIL") + "\n" + sb;
