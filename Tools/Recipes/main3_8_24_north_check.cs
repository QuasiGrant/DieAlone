// Main3 8.24 north check (Play mode, Main3; NorthLayout.md draft 2). In main3_review_capture.sh --area north's Play checks; also runs
// alone. Never saves; restores the player and the temporary colliders. Every move is PlayerController.Step (dt 0.02).
// RUIN ROOM (N13, Marlow 824 block 1): standing cells on a roomGrid m grid of the room floor (ruin local x -2.85 to 2.85, z -1.85 to
//   1.85) where the capsule fits clear of every collider; the doorway step (the ground outside against the floor slab's top).
// REPORT BOX: the interactor's ray (its mask, interactReach) from the stand (170.16, 277.32) toward the box meets the box or its post.
// FORAGE: the interactor's ray from the forage stand toward the south shrub meets a usable with the prompt "Forage".
// N14 STOVEPIPE: from the eye (192.7, G+1.6, 276.6) to the pipe's top, past every drawn mesh (temporary exact colliders, as 8.22): the
//   first thing met is the pipe.
// WALKS (doc 4): Jg to Camp 1 and Camp 1 to J along their trails; the side path from its mouth in at the doorway to the bunk, the
//   cache trunk and the table fronts, out again; the report box stand. Each arrives within arrive m, with times.
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
const float dt = 0.02f, arrive = 0.5f, legTime = 200f, eyeH = 1.6f, roomGrid = 0.2f, roomX = 2.85f, roomZ = 1.85f, stepMax = 0.1f;
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
var ruin = Root("Places") != null ? Root("Places").transform.Find("NorthRuin") : null; var L = ruin != null ? ruin.Find("Layout824") : null;
try
{
    if (ruin == null || L == null) return "no Places/NorthRuin/Layout824 (run main3_8_24_north.cs)";
    var slab = L.Find("FloorSlab").GetComponent<UnityEngine.BoxCollider>(); float floorTop = slab.bounds.max.y;
    // ---- RUIN ROOM
    {
        float r = cc.radius + cc.skinWidth, h = cc.height; int cells = 0, tried = 0;
        for (float x = -roomX + roomGrid * 0.5f; x < roomX; x += roomGrid) for (float z = -roomZ + roomGrid * 0.5f; z < roomZ; z += roomGrid)
        {
            var p = ruin.TransformPoint(V(x, 0f, z)); p.y = floorTop; tried++; bool clear = true;
            foreach (var c in UnityEngine.Physics.OverlapCapsule(p + V(0f, r + 0.02f, 0f), p + V(0f, h - r, 0f), r, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!c.transform.IsChildOf(pc.transform) && !(c is UnityEngine.TerrainCollider)) { clear = false; break; }
            if (clear) cells++;
        }
        var outside = ruin.TransformPoint(V(0f, 0f, 2.5f)); float ground = H(outside.x, outside.z);
        foreach (var hh in UnityEngine.Physics.RaycastAll(outside + V(0f, 3f, 0f), UnityEngine.Vector3.down, 6f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!hh.collider.transform.IsChildOf(pc.transform) && hh.collider.gameObject.layer != 2) ground = UnityEngine.Mathf.Max(ground, hh.point.y);
        Line(cells > 0, "RUIN ROOM: " + cells + " of " + tried + " floor cells (" + F1(roomGrid) + " m) where the capsule stands clear");
        Line(UnityEngine.Mathf.Abs(floorTop - ground) <= stepMax, "DOORWAY STEP: floor top " + F(floorTop) + ", ground outside the doorway " + F(ground) + ", step " + F(floorTop - ground) + " m (at most " + F1(stepMax) + ")");
    }
    // ---- REPORT BOX
    {
        var post = L.Find("ReportPost"); var box = post != null ? post.Find("ReportBox") : null; var pi = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>();
        var maskF = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        if (box == null || pi == null || maskF == null || tuning == null) Line(false, "REPORT BOX: no ReportPost/ReportBox, PlayerInteractor, its mask or PlayerTuning");
        else
        {
            int mask = ((UnityEngine.LayerMask)maskF.GetValue(pi)).value; var eye = V(170.16f, H(170.16f, 277.32f) + eyeH, 277.32f); var aim = PlaceKit.MeshBounds(box.gameObject).center;
            string first = "nothing within " + F1(tuning.interactReach) + " m"; bool ok = false;
            if (UnityEngine.Physics.Raycast(eye, (aim - eye).normalized, out var hit, tuning.interactReach, mask, UnityEngine.QueryTriggerInteraction.Ignore)) { ok = hit.collider.transform.IsChildOf(post); first = WalkIns.PathOf(hit.collider.transform) + " at " + F(hit.distance) + " m"; }
            Line(ok, "REPORT BOX: from the stand (170.16, 277.32) the interactor's ray meets " + first);
        }
    }
    // ---- FORAGE (Pim, Wren 2026-10-02): from the stand at the trail edge (229.2, 270.6), the interactor's own test (camera ray, its mask,
    // triggers ignored, interactReach) toward the south shrub (229.3, 272.1) meets an Interactable whose prompt is "Forage"
    {
        var pi = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>(); var maskF = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        var fc = Root("Places") != null ? Root("Places").transform.Find("ForageC") : null; UnityEngine.Transform south = null; float bestD = float.MaxValue;
        if (fc != null) foreach (UnityEngine.Transform s in fc) { if (!s.name.StartsWith("Bush_ForageC_")) continue; float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(s.position.x, s.position.z), new UnityEngine.Vector2(229.3f, 272.1f)); if (d < bestD) { bestD = d; south = s; } }
        if (pi == null || maskF == null || tuning == null || south == null) Line(false, "FORAGE: no PlayerInteractor, its mask, PlayerTuning or a Places/ForageC shrub");
        else
        {
            int mask = ((UnityEngine.LayerMask)maskF.GetValue(pi)).value; var eye = V(229.2f, H(229.2f, 270.6f) + eyeH, 270.6f); var aim = PlaceKit.MeshBounds(south.gameObject).center;
            string got = "nothing within " + F1(tuning.interactReach) + " m"; bool ok = false;
            if (UnityEngine.Physics.Raycast(eye, (aim - eye).normalized, out var hit, tuning.interactReach, mask, UnityEngine.QueryTriggerInteraction.Ignore))
            { var it = hit.collider.GetComponentInParent<Interactable>(); ok = it != null && it.Prompt == "Forage"; got = WalkIns.PathOf(hit.collider.transform) + " at " + F(hit.distance) + " m, prompt " + (it != null ? "\"" + it.Prompt + "\"" : "none"); }
            Line(ok, "FORAGE: from the stand (229.2, 270.6) toward " + south.name + " the interactor's ray meets " + got);
            // the gaps between the shrubs' colliders: none in the 0.6 to 1.0 m band (a slot the body cannot pass but a view says it can)
            var caps = new System.Collections.Generic.List<UnityEngine.Bounds>(); foreach (UnityEngine.Transform s in fc) { var c = s.GetComponent<UnityEngine.Collider>(); if (c != null) caps.Add(c.bounds); }
            float least = float.MaxValue; int band = 0;
            for (int i = 0; i < caps.Count; i++) for (int j = i + 1; j < caps.Count; j++) { float gap = new UnityEngine.Vector2(caps[i].center.x - caps[j].center.x, caps[i].center.z - caps[j].center.z).magnitude - (caps[i].extents.x + caps[j].extents.x); least = UnityEngine.Mathf.Min(least, gap); if (gap >= 0.6f && gap < 1.0f) band++; }
            Line(caps.Count == 5 && band == 0, "FORAGE GAPS: " + caps.Count + " shrub colliders, least gap " + F(least) + " m, gaps in the 0.6 to 1.0 m band " + band);
        }
    }
    // ---- N14 STOVEPIPE
    {
        var pipe = ruin.Find("Potbelly_Stove_Pipe_Long");
        if (pipe == null) Line(false, "N14: no ruin stovepipe");
        else
        {
            var pb = PlaceKit.MeshBounds(pipe.gameObject); var top = V(pb.center.x, pb.max.y - 0.1f, pb.center.z); var eye = V(192.7f, H(192.7f, 276.6f) + eyeH, 276.6f); var d = top - eye; var ray = new UnityEngine.Ray(eye, d.normalized);
            var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
            foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
            {
                if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(pc.transform)) continue;
                var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null || !mr.bounds.IntersectRay(ray, out float dist) || dist > d.magnitude) continue;
                var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
            }
            UnityEngine.Physics.SyncTransforms(); float fo = float.MaxValue, fp = float.MaxValue; string what = "nothing";
            foreach (var hh in UnityEngine.Physics.RaycastAll(eye, d.normalized, d.magnitude + 0.2f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
            { var ht = hh.collider.transform; if (ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2) continue; if (ht.IsChildOf(pipe)) fp = UnityEngine.Mathf.Min(fp, hh.distance); else if (hh.distance < fo) { fo = hh.distance; what = WalkIns.PathOf(ht) + " at " + F1(hh.distance) + " m"; } }
            Line(fo == float.MaxValue || fp < fo, "N14 STOVEPIPE: from (192.7, " + F1(eye.y) + ", 276.6) to the pipe top (" + F1(top.x) + ", " + F1(top.y) + ", " + F1(top.z) + "), " + F1(d.magnitude) + " m, every drawn mesh: " + (fo == float.MaxValue || fp < fo ? "clear" : "first " + what));
            foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        }
    }
    // ---- WALKS
    {
        void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
        bool Walk(UnityEngine.Vector3 to, ref float time, out float left)
        {
            for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = V(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, false, false, dt); time += dt; }
            var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
        }
        UnityEngine.Vector3 G(float x, float z) => V(x, H(x, z), z); UnityEngine.Vector3 RL(float x, float z) { var p = ruin.TransformPoint(V(x, 0f, z)); p.y = slab.bounds.max.y; return p; }
        var legs = new System.Collections.Generic.List<(string, UnityEngine.Vector3[])>();
        foreach (var n in new[] { "Jg to Camp 1", "Camp 1 to J" }) { var leg = Root("Trails").transform.Find(n); if (leg == null) { Line(false, "WALK: no Trails/" + n); continue; } var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position); legs.Add((n + ", the trail", pts.ToArray())); }
        legs.Add(("side path in at the doorway to the bunk, the cache trunk and the table, and out", new[] { G(168.1f, 267.1f), RL(0f, 3.2f), RL(0f, 1.0f), RL(-1.35f, -0.6f), RL(-2.0f, -0.45f), RL(-0.5f, -0.4f), RL(-0.5f, 1.3f), RL(-1.8f, 1.3f), RL(0f, 1.0f), RL(0f, 3.2f), G(168.1f, 267.1f) }));
        legs.Add(("side path to the report box stand", new[] { G(168.1f, 267.1f), G(170.16f, 277.32f) }));
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
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.24 north check (NorthLayout.md draft 2)\n" + sb;
