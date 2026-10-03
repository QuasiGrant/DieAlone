// Main3 8.25 Camp 2 check (Play mode, Main3; Camp2Layout.md draft 2). In main3_review_capture.sh --area camp2's Play checks; also runs
// alone. Never saves; restores the player and the temporary colliders. Every move is PlayerController.Step (dt 0.02).
// PROMPTS (gate round 2): from each stand the interactor's ray meets the usable with its word (hook "Lift the receiver", barrel "Take water",
//   R2 at the table and on top "Talk", ring box "Examine"); HOOK LOOKS: from the booth stand, at least half the looks near the phone meet it.
// TOP CHAIR: his chair on the top has box colliders only (no hull: a hulled seat is a perch over the rail; doc 4.2).
// T3 TOP RAIL: from points on the top, on the landing and in front of his chair (and on its seat if a jump reaches it: TOP CHAIR SEAT), walking, sprinting and sprint-jumping out on every railHeadings
//   heading for railTime s: none crosses the top rail (leaves the top for neither the top nor the stair); down the stair is no fall; runs that go
//   onto the stair and then over a stair rail are listed apart (T3 STAIR RAILS).
// S1 SIGHTLINE: seated at his chair, to the T signpost's top; HIGHWAY FROM THE TOP: one stand spot on the top floor sees the highway (Wren 2026-10-03); past the terrain, every drawn collider (not the invisible rail boxes),
//   and every drawn mesh (temporary exact colliders, as 8.24's stovepipe): reported clear or the first thing met.
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
var c2 = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_2") : null; var L = c2 != null ? c2.Find("Layout825") : null; var LT = c2 != null ? c2.Find("StackTop/Layout825") : null; var cd2 = c2 != null ? c2.Find("Dressing") : null;
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
    var chair = LT.Find("HisChair");
    // ---- PROMPTS (gate round 2, Wren 3 and 4): from each stand, eye 1.6, the interactor's own test (its mask, triggers ignored,
    // interactReach) aimed at the usable's collider centre meets an Interactable whose prompt is the doc's word
    {
        const float ringLook = 45f; var phone = cd2 != null ? cd2.Find("Payphone/Telephone_Booth/Handset") : null; var ring = LT.Find("RingBox"); var barrel = L.Find("Barrel"); var tableChair = L.Find("CardTable/HisChair");
        foreach (var (label, t, x, z, floorY, word) in new[] { ("hook", phone, 299.65f, 99.05f, float.NaN, "Lift the receiver"), ("barrel", barrel, 292.3f, 100.4f, float.NaN, "Take water"),
            ("R2 at the table", tableChair, 298.5f, 98.7f, float.NaN, "Talk"), ("R2 on top", chair, 293.8f, 109.5f, topY, "Talk"), ("ring box", ring, 290.3f, 108.0f, topY, "Examine") })
        {
            if (t == null) { Line(false, "PROMPT " + label + ": no object"); continue; }
            var eye = V(x, (float.IsNaN(floorY) ? H(x, z) : floorY) + eyeH, z); var col = t.GetComponentInChildren<UnityEngine.Collider>(); var aim = col != null ? col.bounds.center : t.position;
            string got = "nothing within " + F1(reach) + " m"; bool ok = false;
            if (UnityEngine.Physics.Raycast(eye, (aim - eye).normalized, out var hit, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore)) { var it = hit.collider.GetComponentInParent<Interactable>(); got = WalkIns.PathOf(hit.collider.transform) + " at " + F(hit.distance) + " m, prompt \"" + (it != null ? it.Prompt : "none") + "\""; ok = it != null && it.Prompt == word && hit.collider.transform.IsChildOf(t); }
            var dv = aim - eye; float down = UnityEngine.Mathf.Atan2(-dv.y, new UnityEngine.Vector2(dv.x, dv.z).magnitude) * UnityEngine.Mathf.Rad2Deg; if (label == "ring box" && down > ringLook) ok = false;   // ring box: a look of ringLook or less (gate round 2)
            Line(ok, "PROMPT " + label + " (\"" + word + "\"): from (" + F1(x) + ", " + F1(z) + "), " + F1(down) + " degrees down, the interactor's ray meets " + got);
        }
        // HOOK LOOKS (Marlow 825 gate 4: 0 to 6 of 1,224 looks met the old receiver): from the booth stand, looks within hookCone degrees of
        // the phone's centre (every hookStep in yaw and pitch): the share whose first hit within reach is the phone
        const float hookCone = 10f, hookStep = 2f, hookShare = 0.5f; int looks = 0, met = 0; var by = new System.Collections.Generic.SortedDictionary<string, int>();
        if (phone != null)
        {
            var eye = V(299.65f, H(299.65f, 99.05f) + eyeH, 99.05f); var c = phone.GetComponent<UnityEngine.Collider>().bounds.center; var d0 = (c - eye).normalized; float y0 = UnityEngine.Mathf.Atan2(d0.x, d0.z) * UnityEngine.Mathf.Rad2Deg, p0 = -UnityEngine.Mathf.Asin(d0.y) * UnityEngine.Mathf.Rad2Deg;
            for (float dy = -hookCone; dy <= hookCone + 1e-3f; dy += hookStep) for (float dp = -hookCone; dp <= hookCone + 1e-3f; dp += hookStep)
            {
                looks++; var dir = UnityEngine.Quaternion.Euler(p0 + dp, y0 + dy, 0f) * UnityEngine.Vector3.forward;
                if (!UnityEngine.Physics.Raycast(eye, dir, out var hit, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore)) { by["nothing"] = by.TryGetValue("nothing", out int a) ? a + 1 : 1; continue; }
                if (hit.collider.transform.IsChildOf(phone)) met++; else { var k = hit.collider.name; by[k] = by.TryGetValue(k, out int b) ? b + 1 : 1; }
            }
        }
        Line(looks > 0 && met >= hookShare * looks, "HOOK LOOKS: from the booth stand (299.65, 99.05), " + met + " of " + looks + " looks within " + F1(hookCone) + " degrees of the phone meet it first" + (by.Count > 0 ? "; the rest: " + string.Join(", ", System.Linq.Enumerable.Select(by, kv => kv.Key + " x" + kv.Value)) : ""));
    }
    // ---- TOP CHAIR
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
        // his chair: the body starts in front of it (the doc's "from the chair"); its seat box is a start only if a jump reaches it
        starts.Add(("in front of his chair", V(294.0f, topY, 109.6f)));
        if (chair != null && tuning != null)
        {
            var cb = new UnityEngine.Bounds(chair.position, UnityEngine.Vector3.zero); bool any = false; foreach (var c in chair.GetComponentsInChildren<UnityEngine.Collider>()) { if (!any) { cb = c.bounds; any = true; } else cb.Encapsulate(c.bounds); }
            float seat = cb.max.y - topY, reachUp = tuning.jumpHeight + cc.stepOffset; bool up = seat <= reachUp;
            Line(!up, "TOP CHAIR SEAT: its box " + F(seat) + " m over the top, a jump and a step reach " + F(reachUp) + " m: " + (up ? "the body gets on it (a perch over the rail)" : "the body cannot get on it"));
            if (up) starts.Add(("his chair's seat", V(cb.center.x, cb.max.y, cb.center.z)));
        }
        // a run leaves by the top rail when the body is first outside both the top (topR m from the stack centre) and the stair's footprint
        // (stairBox) while still near the top's height; by a stair rail when it was on the stair first; going down the stair is no fall
        const float topR = 4.6f; var stairBox = new UnityEngine.Rect(295.4f, 106.4f, 301.2f - 295.4f, 119.6f - 106.4f);
        bool OnTop(UnityEngine.Vector3 q) => new UnityEngine.Vector2(q.x - 292f, q.z - 108f).magnitude <= topR;
        bool OnStair(UnityEngine.Vector3 q) => stairBox.Contains(new UnityEngine.Vector2(q.x, q.z));
        var falls = new System.Collections.Generic.List<string>(); var stairFalls = new System.Collections.Generic.List<string>(); int runs = 0, downStair = 0; float least = float.MaxValue;
        foreach (var (name, p) in starts)
        {
            if (UnityEngine.Physics.OverlapCapsule(p + V(0f, cc.radius + 0.12f, 0f), p + V(0f, cc.height - cc.radius + 0.1f, 0f), cc.radius, ~0, UnityEngine.QueryTriggerInteraction.Ignore).Length > 0 && name != "his chair's seat") { sb.Append("note: T3 start " + name + " is not clear, skipped\n"); continue; }
            for (int h = 0; h < railHeadings; h++) foreach (var (jump, sprint, how) in new[] { (false, false, "walk"), (false, true, "sprint"), (true, true, "sprint-jump") })
            {
                Put(p); float yaw = h * 360f / railHeadings * UnityEngine.Mathf.Deg2Rad; var dir = V(UnityEngine.Mathf.Sin(yaw), 0f, UnityEngine.Mathf.Cos(yaw)); float low = float.MaxValue; runs++;
                bool wasStair = false; string left = null;
                for (float t = 0f; t < railTime; t += dt)
                {
                    pc.Step(dir, jump, sprint, dt); var q = pc.transform.position; low = UnityEngine.Mathf.Min(low, q.y);
                    if (left == null) { if (OnStair(q) && !OnTop(q)) wasStair = true; else if (!OnTop(q) && !OnStair(q)) left = wasStair ? "stair" : "top"; }
                }
                least = UnityEngine.Mathf.Min(least, low); var e = pc.transform.position;
                string s = name + " " + how + " heading " + F1(h * 360f / railHeadings) + " to (" + F1(e.x) + ", " + F1(e.y) + ", " + F1(e.z) + ")";
                if (left == "top") falls.Add(s); else if (left == "stair") stairFalls.Add(s); else if (low < topY - railDrop) downStair++;
            }
        }
        Line(falls.Count == 0, "T3 TOP RAIL: " + runs + " runs (walk, sprint, sprint-jump on " + railHeadings + " headings for " + F1(railTime) + " s from " + starts.Count + " starts), lowest " + F(least) + ", over the top rail " + falls.Count + (falls.Count > 0 ? ": " + string.Join("; ", falls.GetRange(0, UnityEngine.Mathf.Min(8, falls.Count))) : "") + "; down the stair (no fall) " + downStair);
        Line(stairFalls.Count == 0, "T3 STAIR RAILS (from the top): runs that went onto the stair and then off its footprint " + stairFalls.Count + (stairFalls.Count > 0 ? ": " + string.Join("; ", stairFalls.GetRange(0, UnityEngine.Mathf.Min(8, stairFalls.Count))) : ""));
    }
    // ---- STAIR VOID (the 8.25 trap under Ramp2, Wren 2026-10-02): from Ramp1's centre line every voidStep m, walking, sprinting and
    // sprint-jumping on headings east (voidFrom to voidTo), for voidTime s: none ends in the space under Ramp2 (east of the ramps' gap, inside
    // the stair's footprint, under voidUnder m over the ground)
    {
        const float voidStep = 1f, voidFrom = 30f, voidTo = 150f, voidHeadStep = 15f, voidTime = 2f, voidUnder = 2f;
        var r1 = c2.Find("StackPath/Ramp1") != null ? c2.Find("StackPath/Ramp1").GetComponent<UnityEngine.Collider>() : null; var r2 = c2.Find("StackPath/Ramp2") != null ? c2.Find("StackPath/Ramp2").GetComponent<UnityEngine.Collider>() : null;
        if (r1 == null || r2 == null) Line(false, "STAIR VOID: no Ramp1 or Ramp2");
        else
        {
            int runs = 0; var inVoid = new System.Collections.Generic.List<string>(); float cx = r1.bounds.center.x;
            for (float z = r1.bounds.min.z + 0.5f; z <= r1.bounds.max.z - 0.5f; z += voidStep)
            {
                if (!r1.Raycast(new UnityEngine.Ray(V(cx, r1.bounds.max.y + 1f, z), UnityEngine.Vector3.down), out var hit, 40f)) continue;
                for (float hd = voidFrom; hd <= voidTo + 1e-3f; hd += voidHeadStep) foreach (var (jump, sprint, how) in new[] { (false, false, "walk"), (false, true, "sprint"), (true, true, "sprint-jump") })
                {
                    Put(hit.point); var dir = UnityEngine.Quaternion.Euler(0f, hd, 0f) * UnityEngine.Vector3.forward; runs++;
                    for (float t = 0f; t < voidTime; t += dt) pc.Step(dir, jump, sprint, dt);
                    var e = pc.transform.position;
                    if (e.x > r2.bounds.min.x && e.x < r2.bounds.max.x && e.z > r2.bounds.min.z && e.z < r2.bounds.max.z && e.y < H(e.x, e.z) + voidUnder) inVoid.Add("from z " + F1(z) + " " + how + " heading " + F1(hd) + " to (" + F1(e.x) + ", " + F1(e.y) + ", " + F1(e.z) + ")");
                }
            }
            Line(inVoid.Count == 0, "STAIR VOID: " + runs + " runs off Ramp1 eastward, ending under Ramp2 " + inVoid.Count + (inVoid.Count > 0 ? ": " + string.Join("; ", inVoid.GetRange(0, UnityEngine.Mathf.Min(6, inVoid.Count))) : ""));
        }
    }
    // ---- S1 SIGHTLINE (Wren 2026-10-03): seated at his chair (its box centre, seatEye over the top) to the T signpost's top
    // (Ground815/JunctionMarkers/Trailhead_Board, less signDrop); HIGHWAY FROM THE TOP: at least one stand spot on the top floor (every
    // standStep m within standR of the centre, eye 1.6) sees a highway point (x highwayX, ground + 1, one of highwayZs). Lines pass the
    // terrain, every drawn collider (not invisible boxes) and every drawn mesh (temporary exact colliders, as 8.24's stovepipe).
    {
        const float seatEye = 1.2f, signDrop = 0.05f, standStep = 0.5f, standR = 3.8f, highwayX = 430f; float[] highwayZs = { 150f, 160f, 170f, 180f, 185f, 190f, 200f, 210f };
        var sign = Root("Ground815").transform.Find("JunctionMarkers/Trailhead_Board");
        if (sign == null || chair == null) Line(false, "S1 SIGHTLINE: no Trailhead_Board or his chair");
        else
        {
            var sbd = PlaceKit.MeshBounds(sign.gameObject); var tPt = V(sbd.center.x, sbd.max.y - signDrop, sbd.center.z);
            var cbd = chair.GetComponent<UnityEngine.Collider>().bounds; var seat = V(cbd.center.x, topY + seatEye, cbd.center.z);
            var stands = new System.Collections.Generic.List<UnityEngine.Vector3>();
            for (float x = 292f - standR; x <= 292f + standR + 1e-3f; x += standStep) for (float z = 108f - standR; z <= 108f + standR + 1e-3f; z += standStep) if (new UnityEngine.Vector2(x - 292f, z - 108f).magnitude <= standR) stands.Add(V(x, topY + eyeH, z));
            var hws = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (var hz in highwayZs) hws.Add(V(highwayX, H(highwayX, hz) + 1f, hz));
            var rays = new System.Collections.Generic.List<(UnityEngine.Vector3 a, UnityEngine.Vector3 b)> { (seat, tPt) }; foreach (var s in stands) foreach (var h in hws) rays.Add((s, h));
            var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
            foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
            {
                if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(pc.transform) || mr.transform.IsChildOf(chair)) continue;
                var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; bool on = false;
                foreach (var (a, b) in rays) { var d = b - a; if (mr.bounds.IntersectRay(new UnityEngine.Ray(a, d.normalized), out float dist) && dist <= d.magnitude) { on = true; break; } }
                if (!on) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
            }
            UnityEngine.Physics.SyncTransforms();
            string First(UnityEngine.Vector3 a, UnityEngine.Vector3 b)
            {
                var d = b - a; float best = float.MaxValue; string what = null;
                foreach (var hh in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude - signDrop, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
                { var ht = hh.collider.transform; if (ht.IsChildOf(pc.transform) || ht.IsChildOf(sign) || ht.IsChildOf(chair) || ht.gameObject.layer == 2 || (!(hh.collider is UnityEngine.TerrainCollider) && ht.GetComponent<UnityEngine.Renderer>() == null)) continue; if (hh.distance < best) { best = hh.distance; what = WalkIns.PathOf(ht) + " at " + F1(hh.distance) + " m"; } }
                return what;
            }
            var ft = First(seat, tPt);
            Line(ft == null, "S1 SIGHTLINE: seated at his chair (" + F(seat.x) + ", " + F(seat.y) + ", " + F(seat.z) + ") to the T signpost's top (" + F1(tPt.x) + ", " + F1(tPt.y) + ", " + F1(tPt.z) + "): " + (ft == null ? "clear" : "first " + ft));
            int seen = 0; string ex = "";
            foreach (var s in stands) foreach (var h in hws) if (First(s, h) == null) { seen++; if (ex == "") ex = " (first: (" + F1(s.x) + ", " + F1(s.z) + ") to z " + F1(h.z) + ")"; break; }
            Line(seen > 0, "HIGHWAY FROM THE TOP: " + seen + " of " + stands.Count + " stand spots on the top floor see a highway point" + ex);
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
