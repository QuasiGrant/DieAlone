// Main3 warp landing check (Play mode; Grant 2026-10-01: "Several warps have you fall through the map"). Runs in the capture
// (main3_review_capture.sh, into Checks.md) and at the end of the runner (main3_rebuild.sh, through main3_warp_landing_check.sh).
// For every child of DevWarps, the player is put exactly as DevMenu.Warp puts it (controller off, position = the warp point, yaw = the
// warp's, controller on), then left settleTime seconds under PlayerController.Step (the game's own rules, dt 0.02). FAIL if:
//   FELL: it ends more than maxFall metres under the warp point;
//   NO GROUND: no collider within groundReach metres under the capsule's foot;
//   STUCK: a walk of walkTry metres (walkTime seconds allowed) fails in all 8 directions (ahead, back, left, right of the warp and the
//   diagonals); directions that fail are listed as notes (walls inside a building, the ledge lip).
// Each line gives the warp, the ground under it (terrain height and the first collider down from 50 m up) and the result.
// Never saves the scene; restores the player's place and runInBackground (false) before it returns.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var warps = Root("DevWarps"); if (warps == null) return "no DevWarps root";
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); bool pcWas = pc.enabled; pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, settleTime = 3f, maxFall = 2f, groundReach = 0.5f, walkTry = 3f, walkTime = 4f, probeUp = 50f;
var inv = System.Globalization.CultureInfo.InvariantCulture;
var start = pc.transform.position; var startRot = pc.transform.rotation;
void Warp(UnityEngine.Transform t) { cc.enabled = false; pc.transform.position = t.position; pc.transform.rotation = UnityEngine.Quaternion.Euler(0f, t.eulerAngles.y, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); }
void Settle() { for (float s = 0f; s < settleTime; s += dt) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
UnityEngine.Vector3 Foot() => pc.transform.TransformPoint(cc.center) + UnityEngine.Vector3.down * (cc.height * 0.5f);
string Under(UnityEngine.Vector3 p)
{
    var top = new UnityEngine.Vector3(p.x, p.y + probeUp, p.z); string first = "none";
    foreach (var h in UnityEngine.Physics.RaycastAll(top, UnityEngine.Vector3.down, probeUp * 2f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        if (!h.collider.transform.IsChildOf(pc.transform) && (first == "none" || h.point.y > float.Parse(first.Split('@')[1], inv)))
            first = h.collider.name + "@" + h.point.y.ToString("F2", inv);
    return "terrain " + H(p.x, p.z).ToString("F2", inv) + ", top collider " + first.Replace("@", " at ");
}
var sb = new System.Text.StringBuilder(); int fails = 0, n = 0;
try
{
    foreach (UnityEngine.Transform w in warps.transform)
    {
        n++; Warp(w); Settle();
        var settled = pc.transform.position; var problems = new System.Collections.Generic.List<string>();
        float fall = w.position.y - settled.y; if (fall > maxFall) problems.Add("FELL " + fall.ToString("F1", inv) + " m");
        var foot = Foot(); bool ground = UnityEngine.Physics.SphereCast(foot + UnityEngine.Vector3.up * (cc.radius + 0.05f), cc.radius * 0.9f, UnityEngine.Vector3.down, out var gh, groundReach + 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore) && !gh.collider.transform.IsChildOf(pc.transform);
        if (!ground) problems.Add("NO GROUND within " + groundReach + " m");
        var stuck = new System.Collections.Generic.List<string>();
        var fwd = UnityEngine.Quaternion.Euler(0f, w.eulerAngles.y, 0f) * UnityEngine.Vector3.forward;
        var names = new[] { "ahead", "ahead right", "right", "back right", "back", "back left", "left", "ahead left" };
        for (int k = 0; k < names.Length; k++)
        {
            string name = names[k]; var dir = UnityEngine.Quaternion.Euler(0f, k * 45f, 0f) * fwd; cc.enabled = false; pc.transform.position = settled; cc.enabled = true; UnityEngine.Physics.SyncTransforms();
            float got = 0f;
            for (float s = 0f; s < walkTime && got < walkTry; s += dt) { pc.Step(dir, false, false, dt); var d = pc.transform.position - settled; got = new UnityEngine.Vector2(d.x, d.z).magnitude; }
            if (got < walkTry) stuck.Add(name + " " + got.ToString("F1", inv));
        }
        if (stuck.Count == names.Length) problems.Add("STUCK (" + string.Join(", ", stuck) + " m)");
        string notes = stuck.Count > 0 && stuck.Count < names.Length ? " (blocked: " + string.Join(", ", stuck) + " m)" : "";
        if (problems.Count > 0) fails++;
        sb.Append((problems.Count == 0 ? "ok   " : "FAIL ") + w.name + " at " + w.position.ToString("F1") + ": " + (problems.Count == 0 ? "lands, fell " + UnityEngine.Mathf.Max(0f, fall).ToString("F2", inv) + " m" + notes : string.Join("; ", problems) + ", ends at " + settled.ToString("F1")) + " | " + Under(w.position) + "\n");
    }
}
finally { cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false; }
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": " + n + " warps, each settled " + settleTime + " s, fall over " + maxFall + " m, ground within " + groundReach + " m, a " + walkTry + " m walk in at least 1 of 8 directions\n" + sb;
