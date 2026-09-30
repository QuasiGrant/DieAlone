// Main3 trunk check (Play mode; 8.16a, Marlow's walk-through test, Gate_816_Marlow.md 16). Never saves the scene. For every standing
// tree (BK Sequoia, RedFir, RedPine and the standing dead snags) and hollow log under Forest, Ground815, SliceLook and Camp:
// - COLLIDER: it has its own trunk capsule (a log: its log mesh collider), or, for SliceLook's pack giants, the gray 8.3 giant under it
//   holds its trunk (a collider under the Giants root within its trunk), or, for a hedge log, the hedge collider under it (8.15).
//   One with none of these is a FAIL.
// - WALK: the player starts startOut m from its centre on each of four sides in turn and walks straight at it with the real mover
//   (PlayerController.Step, dt 0.02) for walkSeconds. A tree FAILs when the player's axis (the camera) ever comes inside its trunk
//   capsule's radius; a log FAILs when a ray down through the player's axis ever meets the log (the player inside or on it). Starts
//   not on open ground (inside another solid) are skipped and counted.
// Also Marlow's repro: the Grove_Knoll giant nearest (146, 174), walked at from 3.6 m east.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, startOut = 4f, walkSeconds = 3f, reproOut = 3.6f, rayUp = 3f, logInside = 0.3f, proxyReach = 1.5f; const int sides = 4;
var reproAt = new UnityEngine.Vector2(146f, 174f); var giantsRoot = Root("Giants");
var inv = System.Globalization.CultureInfo.InvariantCulture;
bool Kind(string n, out bool log)
{
    log = n.StartsWith("RedwoodHollowLog");
    return log || n.StartsWith("Sequoia") || (n.StartsWith("RedFir") && !n.StartsWith("RedFirBranches")) || n.StartsWith("RedPine") || n.StartsWith("Tree_Dead");
}
bool Open(UnityEngine.Vector3 at) { foreach (var c in UnityEngine.Physics.OverlapCapsule(at + UnityEngine.Vector3.up * (cc.radius + 0.1f), at + UnityEngine.Vector3.up * (cc.height - cc.radius), cc.radius, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) if (!(c is UnityEngine.TerrainCollider) && !c.transform.IsChildOf(pc.transform)) return false; return true; }
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.05f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 5; k++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
// walks at centre; returns the least distance from the player's axis to the trunk axis (tree) or whether a down ray met the log
float WalkTree(UnityEngine.Vector3 centre, float capLow, float capTop)   // only while the player's body overlaps the capsule's height (not on a bench above it)
    // counted only while the straight parts of both capsules share a height
{
    var from = pc.transform.position; var dir = new UnityEngine.Vector3(centre.x - from.x, 0f, centre.z - from.z).normalized; float least = float.MaxValue;
    for (float t = 0f; t < walkSeconds; t += dt) { pc.Step(dir, false, false, dt); var p = pc.transform.position; if (p.y + cc.radius > capTop || p.y + cc.height - cc.radius < capLow) continue; least = UnityEngine.Mathf.Min(least, new UnityEngine.Vector2(p.x - centre.x, p.z - centre.z).magnitude); }
    return least;
}
bool WalkLog(UnityEngine.Vector3 centre, UnityEngine.Collider[] cols)
{
    var from = pc.transform.position; var dir = new UnityEngine.Vector3(centre.x - from.x, 0f, centre.z - from.z).normalized;
    for (float t = 0f; t < walkSeconds; t += dt)
    {
        pc.Step(dir, false, false, dt); var p = pc.transform.position;
        foreach (var h in UnityEngine.Physics.RaycastAll(p + UnityEngine.Vector3.up * rayUp, UnityEngine.Vector3.down, rayUp + 1f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore))
            foreach (var c in cols) if (h.collider == c && h.point.y > p.y + logInside) return true;   // standing on a log is allowed; below its top is inside
    }
    return false;
}
var noColGroups = new System.Collections.Generic.SortedDictionary<string, int>();
int trees = 0, walks = 0, through = 0, skipped = 0, noCol = 0, proxyHeld = 0, hedgeHeld = 0; var fails = new System.Collections.Generic.List<string>(); var kinds = new System.Collections.Generic.SortedDictionary<string, int>();
string Name(string rn, UnityEngine.Transform t) => rn + "/" + (t.parent != null ? t.parent.name + "/" : "") + t.name + " at (" + t.position.x.ToString("F0", inv) + ", " + t.position.z.ToString("F0", inv) + ")";
string repro = "";
try
{
    foreach (var rn in new[] { "Forest", "Ground815", "SliceLook", "Camp" })
    {
        var root = Root(rn); if (root == null) continue; var beyond = root.transform.Find("BeyondRoad");   // past the fence and the road: out of reach
        foreach (UnityEngine.Transform t in root.GetComponentsInChildren<UnityEngine.Transform>())
        {
            if (!Kind(t.name, out bool log) || (t.parent != null && Kind(t.parent.name, out _))) continue;   // the instance root only
            if (beyond != null && t.IsChildOf(beyond)) continue;
            var rends = t.GetComponentsInChildren<UnityEngine.Renderer>(); if (rends.Length == 0) continue;
            if (!log && UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(0f, t.eulerAngles.z)) > 45f) continue;   // fallen snags lie down: not standing
            trees++; string kn = log ? "log" : t.name.StartsWith("Sequoia") ? "giant" : t.name.StartsWith("Tree_Dead") ? "snag" : "fir or pine"; kinds.TryGetValue(kn, out var kc); kinds[kn] = kc + 1;
            var cols = t.GetComponentsInChildren<UnityEngine.Collider>();
            var cap = t.GetComponentInChildren<UnityEngine.CapsuleCollider>();
            if (cols.Length == 0)
            {
                if (log && t.parent != null && t.parent.name.StartsWith("Hedge")) { hedgeHeld++; continue; }   // a hedge log lies over its hedge's collider (8.15)
                bool held = false;
                // a gray giant's trunk is a hollow mesh collider, so a sphere inside it touches nothing: test its bounds instead
                if (giantsRoot != null) foreach (var c in giantsRoot.GetComponentsInChildren<UnityEngine.Collider>()) { var gb = c.bounds; if (t.position.x > gb.min.x && t.position.x < gb.max.x && t.position.z > gb.min.z && t.position.z < gb.max.z && t.position.y + proxyReach > gb.min.y && t.position.y < gb.max.y) held = true; }
                if (held) { proxyHeld++; continue; }
                noCol++; var grp = rn + "/" + (t.parent != null ? t.parent.name : "") + " " + t.name.Split(new[] { ' ', '(' })[0]; noColGroups.TryGetValue(grp, out var ng); noColGroups[grp] = ng + 1; continue;
            }
            var b = rends[0].bounds; foreach (var r in rends) b.Encapsulate(r.bounds);
            var centre = log ? b.center : (cap != null ? cap.transform.TransformPoint(cap.center) : t.position);
            float capR = cap != null ? cap.radius * UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(cap.transform.lossyScale.x), UnityEngine.Mathf.Abs(cap.transform.lossyScale.z)) : 0f;
            float capHalf = cap != null ? cap.height * 0.5f * UnityEngine.Mathf.Abs(cap.transform.lossyScale.y) - capR : 0f, capLow = centre.y - capHalf, capTop = centre.y + capHalf;   // the straight part
            float reach = log ? UnityEngine.Mathf.Max(b.extents.x, b.extents.z) : capR;
            for (int k = 0; k < sides; k++)
            {
                var dir = UnityEngine.Quaternion.Euler(0f, 45f + k * 360f / sides, 0f) * UnityEngine.Vector3.forward; var st = centre + dir * (startOut + reach);
                st.y = H(st.x, st.z); if (!Open(st)) { skipped++; continue; }
                Put(st); walks++;
                bool inside; string how;
                if (log) { inside = WalkLog(centre, cols); how = "stood inside or on the log"; }
                else { float least = WalkTree(centre, capLow, capTop); inside = least < capR; how = "axis " + least.ToString("F2", inv) + " m from the trunk axis, trunk radius " + capR.ToString("F2", inv); }
                if (inside) { through++; if (fails.Count < 20) fails.Add(Name(rn, t) + " " + how); }
            }
        }
    }
    UnityEngine.Transform rg = null; float rd = float.MaxValue; var knoll = Root("Forest").transform.Find("Grove_Knoll");
    if (knoll != null) foreach (UnityEngine.Transform g in knoll) { float d = new UnityEngine.Vector2(g.position.x - reproAt.x, g.position.z - reproAt.y).magnitude; if (g.name.StartsWith("Sequoia") && d < rd) { rd = d; rg = g; } }
    if (rg != null)
    {
        var cap = rg.GetComponentInChildren<UnityEngine.CapsuleCollider>(); var c0 = cap != null ? cap.transform.TransformPoint(cap.center) : rg.position; float capR = cap != null ? cap.radius * cap.transform.lossyScale.x : 0f;
        var st = c0 + UnityEngine.Vector3.right * reproOut; st.y = H(st.x, st.z); Put(st); float least = WalkTree(c0, float.MinValue, float.MaxValue);
        repro = "Marlow's repro, Grove_Knoll/" + rg.name + " at (" + rg.position.x.ToString("F0", inv) + ", " + rg.position.z.ToString("F0", inv) + ") from " + reproOut.ToString("F1", inv) + " m east: the player stopped " + least.ToString("F2", inv) + " m from its axis (trunk radius " + capR.ToString("F2", inv) + ")\n";
    }
}
finally { pc.enabled = true; }
var ks = new System.Text.StringBuilder(); foreach (var kv in kinds) ks.Append(kv.Key + " " + kv.Value + ", ");
bool pass = through == 0 && noCol == 0; var ncg = new System.Text.StringBuilder(); foreach (var kv in noColGroups) ncg.Append(kv.Key + " x" + kv.Value + "; ");
return "TRUNKS: " + trees + " trees and logs (" + ks.ToString().TrimEnd(' ', ',') + "), " + noCol + " without a collider (" + ncg + "), " + proxyHeld + " held by the 8.3 giant under them, " + hedgeHeld + " hedge logs over their hedge collider; " + walks + " walks from " + startOut.ToString("F0", inv) + " m, " + skipped + " starts not on open ground, " + through + " into a trunk or log: " + (pass ? "PASS" : "FAIL " + string.Join("; ", fails)) + "\n" + repro + (pass ? "ALL PASS" : "SOME FAIL");
