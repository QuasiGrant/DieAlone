// Main3 8.25 Camp 2 check (Play mode, Main3; Camp2Layout.md draft 2). In main3_review_capture.sh --area camp2's Play checks; also runs
// alone. Never saves; restores the player and the temporary colliders. Every move is PlayerController.Step (dt 0.02).
// HOOK (C2): the interactor's ray (its mask, interactReach) from the booth mouth (299.6, G+1.6, 99.2) facing 180, pitch 17 down, meets the handset.
// BARREL (C5): the interactor's ray from the stand (292.3, 100.4) toward the barrel's centre meets the barrel.
// TOP CHAIR: his chair on the top has box colliders only (no hull: a hulled seat is a perch over the rail; doc 4.2).
// T3 TOP RAIL: from points on the top, on the landing and on his chair's seat, walking, sprinting and sprint-jumping out on every railHeadings
//   heading for railTime s, the body never drops under the top (24) less railDrop.
// S1 SIGHTLINE: seated at his chair (294.76, 25.2, 110.31), to the T (337, 170) and the highway on heading 66 (x 430), past the terrain,
//   every collider and every drawn mesh (temporary exact colliders, as 8.24's stovepipe): reported clear or the first thing met.
// WALKS (doc 4): boathouse to Camp 2 and Camp 2 to T along their trails; the ramp foot up the stair to his top chair; your seat to the
//   booth door; the table to the ramp foot. Each arrives within arrive m, with times.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv); string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
const float dt = 0.02f, arrive = 0.5f, legTime = 200f, eyeH = 1.6f, topY = 24f, railDrop = 0.5f, railTime = 3f, hookPitch = 17f; const int railHeadings = 24;
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
var c2 = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_2") : null; var L = c2 != null ? c2.Find("Layout825") : null; var LT = c2 != null ? c2.Find("StackTop/Layout825") : null;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
try
{
    if (L == null || LT == null) return "no Campsites/Camp_2/Layout825 or StackTop/Layout825 (run main3_8_25_camp2.cs)";
    var pi = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>(); var maskF = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
    int mask = pi != null && maskF != null ? ((UnityEngine.LayerMask)maskF.GetValue(pi)).value : ~0; float reach = tuning != null ? tuning.interactReach : 2f;
    if (pi == null || maskF == null || tuning == null) Line(false, "no PlayerInteractor, its mask or PlayerTuning (the rays below use every layer and 2 m)");
    string Meet(UnityEngine.Vector3 eye, UnityEngine.Vector3 dir, UnityEngine.Transform want, out bool ok)
    {
        ok = false; if (!UnityEngine.Physics.Raycast(eye, dir.normalized, out var hit, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore)) return "nothing within " + F1(reach) + " m";
        ok = hit.collider.transform.IsChildOf(want); return WalkIns.PathOf(hit.collider.transform) + " at " + F(hit.distance) + " m";
    }
    // ---- HOOK
    {
        var hs = L.Find("Handset"); var eye = V(299.6f, H(299.6f, 99.2f) + eyeH, 99.2f);
        if (hs == null) Line(false, "HOOK: no Layout825/Handset");
        else { var s = Meet(eye, UnityEngine.Quaternion.Euler(hookPitch, 180f, 0f) * UnityEngine.Vector3.forward, hs, out bool ok); Line(ok, "HOOK: from the booth mouth (299.6, 99.2) facing 180, " + F1(hookPitch) + " down, the interactor's ray meets " + s); }
    }
    // ---- BARREL
    {
        var b = L.Find("Barrel"); var eye = V(292.3f, H(292.3f, 100.4f) + eyeH, 100.4f);
        if (b == null) Line(false, "BARREL: no Layout825/Barrel");
        else { var aim = PlaceKit.MeshBounds(b.gameObject).center; var s = Meet(eye, aim - eye, b, out bool ok); var d = aim - eye; Line(ok, "BARREL: from the stand (292.3, 100.4), heading " + F1((UnityEngine.Mathf.Atan2(d.x, d.z) * UnityEngine.Mathf.Rad2Deg + 360f) % 360f) + " (doc 318), the interactor's ray meets " + s); }
    }
    // ---- TOP CHAIR
    var chair = LT.Find("HisChair");
    {
        if (chair == null) Line(false, "TOP CHAIR: no StackTop/Layout825/HisChair");
        else
        {
            int boxes = 0; var other = new System.Collections.Generic.List<string>();
            foreach (var c in chair.GetComponentsInChildren<UnityEngine.Collider>()) if (c is UnityEngine.BoxCollider) boxes++; else other.Add(c.GetType().Name + " on " + c.name);
            Line(boxes > 0 && other.Count == 0, "TOP CHAIR: " + boxes + " box colliders" + (other.Count > 0 ? ", and " + string.Join(", ", other) : ", nothing else"));
        }
    }
    // ---- T3 TOP RAIL
    {
        var starts = new System.Collections.Generic.List<(string, UnityEngine.Vector3)> { ("top centre", V(292f, topY, 108f)) };
        for (int k = 0; k < 8; k++) { float b = (22.5f + k * 45f) * UnityEngine.Mathf.Deg2Rad; starts.Add(("top, bearing " + F1(22.5f + k * 45f) + " at 3.6 m", V(292f + UnityEngine.Mathf.Sin(b) * 3.6f, topY, 108f + UnityEngine.Mathf.Cos(b) * 3.6f))); }
        starts.Add(("landing west", V(296.9f, topY, 107.25f))); starts.Add(("landing east", V(300.4f, topY, 107.25f)));
        if (chair != null) { var cb = new UnityEngine.Bounds(chair.position, UnityEngine.Vector3.zero); bool any = false; foreach (var c in chair.GetComponentsInChildren<UnityEngine.Collider>()) { if (!any) { cb = c.bounds; any = true; } else cb.Encapsulate(c.bounds); } starts.Add(("his chair's seat", V(cb.center.x, cb.max.y, cb.center.z))); }
        var falls = new System.Collections.Generic.List<string>(); int runs = 0; float least = float.MaxValue;
        foreach (var (name, p) in starts)
        {
            if (UnityEngine.Physics.OverlapCapsule(p + V(0f, cc.radius + 0.12f, 0f), p + V(0f, cc.height - cc.radius + 0.1f, 0f), cc.radius, ~0, UnityEngine.QueryTriggerInteraction.Ignore).Length > 0 && name != "his chair's seat") { sb.Append("note: T3 start " + name + " is not clear, skipped\n"); continue; }
            for (int h = 0; h < railHeadings; h++) foreach (var (jump, sprint, how) in new[] { (false, false, "walk"), (false, true, "sprint"), (true, true, "sprint-jump") })
            {
                Put(p); float yaw = h * 360f / railHeadings * UnityEngine.Mathf.Deg2Rad; var dir = V(UnityEngine.Mathf.Sin(yaw), 0f, UnityEngine.Mathf.Cos(yaw)); float low = float.MaxValue; runs++;
                for (float t = 0f; t < railTime; t += dt) { pc.Step(dir, jump, sprint, dt); low = UnityEngine.Mathf.Min(low, pc.transform.position.y); }
                least = UnityEngine.Mathf.Min(least, low);
                if (low < topY - railDrop) falls.Add(name + " " + how + " heading " + F1(h * 360f / railHeadings) + " to (" + F1(pc.transform.position.x) + ", " + F1(pc.transform.position.y) + ", " + F1(pc.transform.position.z) + ")");
            }
        }
        Line(falls.Count == 0, "T3 TOP RAIL: " + runs + " runs (walk, sprint, sprint-jump on " + railHeadings + " headings for " + F1(railTime) + " s from " + starts.Count + " starts), lowest " + F(least) + ", off the top " + falls.Count + (falls.Count > 0 ? ": " + string.Join("; ", falls.GetRange(0, UnityEngine.Mathf.Min(6, falls.Count))) : ""));
    }
    // ---- S1 SIGHTLINE
    {
        var eye = V(294.76f, 25.2f, 110.31f); const float s1 = 66f; float r = s1 * UnityEngine.Mathf.Deg2Rad; var hdir = V(UnityEngine.Mathf.Sin(r), 0f, UnityEngine.Mathf.Cos(r));
        float tHw = (430f - eye.x) / hdir.x; var hw = eye + hdir * tHw; hw.y = H(hw.x, hw.z) + 1f; var tT = V(337f, H(337f, 170f) + 1f, 170f);
        var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
        foreach (var (label, to) in new[] { ("the T (337, 170)", tT), ("the highway on heading 66 (" + F1(hw.x) + ", " + F1(hw.z) + ")", hw) })
        {
            var d = to - eye; var ray = new UnityEngine.Ray(eye, d.normalized);
            foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
            {
                if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(pc.transform)) continue;
                var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null || !mr.bounds.IntersectRay(ray, out float dist) || dist > d.magnitude) continue;
                var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
            }
            UnityEngine.Physics.SyncTransforms(); float fo = float.MaxValue; string what = "";
            foreach (var hh in UnityEngine.Physics.RaycastAll(eye, d.normalized, d.magnitude, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
            { var ht = hh.collider.transform; if (ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2 || (chair != null && ht.IsChildOf(chair))) continue; if (hh.distance < fo) { fo = hh.distance; what = WalkIns.PathOf(ht) + " at " + F1(hh.distance) + " m"; } }
            Line(fo == float.MaxValue, "S1 SIGHTLINE: seated at his chair to " + label + ", " + F1(d.magnitude) + " m: " + (fo == float.MaxValue ? "clear" : "first " + what));
            foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        }
    }
    // ---- WALKS
    {
        bool Walk(UnityEngine.Vector3 to, ref float time, out float left)
        {
            for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = V(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, false, false, dt); time += dt; }
            var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
        }
        UnityEngine.Vector3 G(float x, float z) => V(x, H(x, z), z); UnityEngine.Vector3 T(float x, float y, float z) => V(x, y, z);
        var legs = new System.Collections.Generic.List<(string, UnityEngine.Vector3[])>();
        foreach (var n in new[] { "Boathouse to Camp 2", "Camp 2 to T" }) { var leg = Root("Trails").transform.Find(n); if (leg == null) { Line(false, "WALK: no Trails/" + n); continue; } var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position); legs.Add((n + ", the trail", pts.ToArray())); }
        // the stair: Ramp1 north (x 298.9), LandingN1, Ramp2 south (x 300.4), LandingS1, Ramp3 north, LandingN2, Ramp4 south, the top landing, west to the front of his chair
        legs.Add(("the ramp foot up the stair to his top chair", new[] { G(298.9f, 107.8f), T(298.9f, 9f, 118.75f), T(300.4f, 9f, 118.75f), T(300.4f, 14f, 107.25f), T(298.9f, 14f, 107.25f), T(298.9f, 19f, 118.75f), T(300.4f, 19f, 118.75f), T(300.4f, topY, 107.25f), T(296.0f, topY, 107.25f), T(294.0f, topY, 109.6f) }));
        legs.Add(("your seat round the table's north side to the booth door", new[] { G(297.5f, 98.6f), G(299.0f, 99.0f), G(299.6f, 99.2f) }));
        legs.Add(("the table to the ramp foot", new[] { G(298.5f, 98.7f), G(298.9f, 107.8f) }));
        float speed = tuning != null ? tuning.walkSpeed : 2.5f;
        foreach (var (name, pts) in legs)
        {
            Put(pts[0]); float time = 0f, len = 0f; bool ok = true; string where = ""; for (int i = 1; i < pts.Length; i++) len += V(pts[i].x - pts[i - 1].x, 0f, pts[i].z - pts[i - 1].z).magnitude;
            for (int i = 1; i < pts.Length && ok; i++) if (!Walk(pts[i], ref time, out float left)) { ok = false; where = ", stops " + F(left) + " m short of (" + F1(pts[i].x) + ", " + F1(pts[i].z) + ") at (" + F1(pc.transform.position.x) + ", " + F1(pc.transform.position.y) + ", " + F1(pc.transform.position.z) + ")"; }
            Line(ok, "WALK: " + name + ", " + F1(len) + " m, " + F1(time) + " s at " + F1(speed) + " m/s" + where);
        }
    }
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.25 Camp 2 check (Camp2Layout.md draft 2)\n" + sb;
