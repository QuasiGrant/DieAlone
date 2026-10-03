// Main3 8.25a Camp 2 check (Play mode, Main3; Camp2Layout.md draft 4). In main3_review_capture.sh --area camp2's Play checks; also runs
// alone. Never saves; restores the player, the camera and every temporary object. Every move is PlayerController.Step (dt 0.02).
// PROMPTS: from each stand the interactor's ray meets the usable with its word (hook "Lift the receiver", barrel "Take water", R2 at the
//   table and on top "Talk", ring box "Examine" from the tent door, ringLook degrees down or less); HOOK LOOKS as 8.25.
// TOP CHAIR: box colliders only; its seat out of a jump's reach (nothing new to climb). CHAIR EDGES (doc 4 gaps): its box chairEdge m or more
//   inside every edge of the flat top. HEAD CLEAR (K2): nothing over the top within headR m of the scramble head.
// T3 EDGES: from points edgeIn m inside every edge of the flat top (not the gully), walk, sprint and sprint-jump out at 0 and +-edgeSpread
//   degrees for edgeTime s: each lands on the floor or the scramble, or stays on the top; a landing on the floor must escape (below); one
//   that ends perched over the floor (climbTop m or more over the ground, off the knob and the scramble) fails.
// T9 POCKETS: Marlow's grid, every podStep m from podNear to podFar m out from the knob's footprint, a walk straight at the knob's centre for
//   podTime s, then the escape test; none trapped.
// T11 WEDGES: from both inner corners of each rockfall boulder against the east face, the 48-heading escape test; at least one escapes.
//   ESCAPE: a walk on each heading for escTime s; escaped when one ends escDist m or more from the start on the floor.
// T12 GULLY: the foot to the head by the landing and back, walking and sprint-jumping; each arrives within arrive m (no stall at the turn).
// WALKS (doc 4): boathouse to Camp 2 and Camp 2 to T along their trails; the trail end across the floor and up the scramble to the talk
//   stand; your seat to the booth door; the table to the trail end. Each arrives within arrive m, with times.
// K-TOP (doc 2.2): no tree trunk within kTopR m of (293, 121); no tree's LOD0 vertex over the flat top (no crown over it).
// S1 SIGHTLINE (doc 2.1, 6.5): seated at his chair (its box centre, eye seatEye), a 2 m flat magenta cube at each target (the highway, the T
//   stop sign, the barrier arm) shows more than s1Px pixels through the game camera at Grant's size (rendered with and without it; changed
//   pixels past the area set's pixelTolerance). If one fails: the doc's fallback cells (fallbackR m round (292, 122) on a fallbackStep grid)
//   with clear rays to all three, nearest first, and the first fallbackPix of them pixel-tested; the chair is not moved here.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv); string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z); UnityEngine.Vector2 P2(float x, float z) => new UnityEngine.Vector2(x, z);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset"); var set = Main3AreaSet.Load();
const float dt = 0.02f, arrive = 0.5f, legTime = 200f, eyeH = 1.6f, topY = 9f, climbTop = 0.7f, hookPitch = 17f;
// the knob (Camp2Layout K1, K2): footprint, flat top, the scramble's points
const float kx0 = 288.8f, kx1 = 299f, kz0 = 116f, kz1 = 127f, tx0 = 289f, tx1 = 297.5f, tz0 = 117f, tz1 = 125.5f, rampHalf = 0.8f;
var foot = V(288.0f, 4.0f, 114.5f); var landS = V(288.0f, 7.0f, 120.7f); var land = V(288.0f, 7.0f, 121.5f); var head = V(291.0f, topY, 120.6f); var talk = V(291.3f, topY, 123.2f);
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
var temps = new System.Collections.Generic.List<UnityEngine.Object>();
var c2 = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_2") : null; var L = c2 != null ? c2.Find("Layout825") : null; var K = c2 != null ? c2.Find("Layout825a") : null; var KT = K != null ? K.Find("Top") : null; var cd2 = c2 != null ? c2.Find("Dressing") : null;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
float Off(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
bool OnScramble(UnityEngine.Vector3 q) { var p = P2(q.x, q.z); return Off(p, P2(foot.x, foot.z), P2(land.x, land.z + rampHalf)) <= rampHalf + cc.radius || Off(p, P2(land.x, land.z), P2(head.x, head.z)) <= rampHalf + cc.radius; }
bool OnKnob(UnityEngine.Vector3 q) => q.x >= kx0 - 0.1f && q.x <= kx1 + 0.1f && q.z >= kz0 - 0.1f && q.z <= kz1 + 0.1f && q.y >= topY - 0.3f;
bool OnFloor(UnityEngine.Vector3 q) => q.y < H(q.x, q.z) + climbTop;
bool Clear(UnityEngine.Vector3 p) => UnityEngine.Physics.OverlapCapsule(p + V(0f, cc.radius + 0.12f, 0f), p + V(0f, cc.height - cc.radius + 0.1f, 0f), cc.radius, ~0, UnityEngine.QueryTriggerInteraction.Ignore).Length == 0;
// ESCAPE: from p, a walk on each of n headings for escTime s; how many end escDist m or more away on the floor
const float escTime = 2f, escDist = 2f;
int Escapes(UnityEngine.Vector3 p, int n) { int k = 0; for (int h = 0; h < n; h++) { Put(p); var s = pc.transform.position; float yaw = h * 360f / n * UnityEngine.Mathf.Deg2Rad; var dir = V(UnityEngine.Mathf.Sin(yaw), 0f, UnityEngine.Mathf.Cos(yaw)); for (float t = 0f; t < escTime; t += dt) pc.Step(dir, false, false, dt); var e = pc.transform.position; if (P2(e.x - s.x, e.z - s.z).magnitude >= escDist && OnFloor(e)) k++; } return k; }
try
{
    if (L == null || K == null || KT == null) return "no Campsites/Camp_2/Layout825 or Layout825a/Top (run main3_8_25_camp2.cs and main3_8_25a_knob.cs)";
    var pi = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>(); var maskF = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
    int mask = pi != null && maskF != null ? ((UnityEngine.LayerMask)maskF.GetValue(pi)).value : ~0; float reach = tuning != null ? tuning.interactReach : 2f;
    if (pi == null || maskF == null || tuning == null) Line(false, "no PlayerInteractor, its mask or PlayerTuning (the rays below use every layer and 2 m)");
    var chair = KT.Find("HisChair");
    // ---- PROMPTS
    {
        const float ringLook = 45f; var phone = cd2 != null ? cd2.Find("Payphone/Telephone_Booth/Handset") : null; var ring = KT.Find("RingBox"); var barrel = L.Find("Barrel"); var tableChair = L.Find("CardTable/HisChair");
        foreach (var (label, t, x, z, floorY, word) in new[] { ("hook", phone, 299.65f, 99.05f, float.NaN, "Lift the receiver"), ("barrel", barrel, 292.3f, 100.4f, float.NaN, "Take water"),
            ("R2 at the table", tableChair, 298.5f, 98.7f, float.NaN, "Talk"), ("R2 on top", chair, talk.x, talk.z, topY, "Talk"), ("ring box", ring, 295.6f, 120.3f, topY, "Examine") })
        {
            if (t == null) { Line(false, "PROMPT " + label + ": no object"); continue; }
            var eye = V(x, (float.IsNaN(floorY) ? H(x, z) : floorY) + eyeH, z); var col = t.GetComponentInChildren<UnityEngine.Collider>(); var aim = col != null ? col.bounds.center : t.position;
            string got = "nothing within " + F1(reach) + " m"; bool ok = false;
            if (UnityEngine.Physics.Raycast(eye, (aim - eye).normalized, out var hit, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore)) { var it = hit.collider.GetComponentInParent<Interactable>(); got = WalkIns.PathOf(hit.collider.transform) + " at " + F(hit.distance) + " m, prompt \"" + (it != null ? it.Prompt : "none") + "\""; ok = it != null && it.Prompt == word && hit.collider.transform.IsChildOf(t); }
            var dv = aim - eye; float down = UnityEngine.Mathf.Atan2(-dv.y, new UnityEngine.Vector2(dv.x, dv.z).magnitude) * UnityEngine.Mathf.Rad2Deg; if (label == "ring box" && down > ringLook) ok = false;
            Line(ok, "PROMPT " + label + " (\"" + word + "\"): from (" + F1(x) + ", " + F1(z) + "), " + F1(down) + " degrees down, the interactor's ray meets " + got);
        }
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
    // ---- TOP CHAIR, CHAIR EDGES, HEAD CLEAR
    {
        const float chairEdge = 1.0f, headR = 1.0f;
        if (chair == null) Line(false, "TOP CHAIR: no Layout825a/Top/HisChair");
        else
        {
            int boxes = 0; var other = new System.Collections.Generic.List<string>(); bool any = false; var cb = new UnityEngine.Bounds();
            foreach (var c in chair.GetComponentsInChildren<UnityEngine.Collider>()) { if (c is UnityEngine.BoxCollider) boxes++; else other.Add(c.GetType().Name + " on " + c.name); if (!any) { cb = c.bounds; any = true; } else cb.Encapsulate(c.bounds); }
            Line(boxes > 0 && other.Count == 0, "TOP CHAIR: " + boxes + " box colliders" + (other.Count > 0 ? ", and " + string.Join(", ", other) : ", nothing else"));
            if (tuning != null) { float seat = cb.max.y - topY, reachUp = tuning.jumpHeight + cc.stepOffset; Line(seat > reachUp, "TOP CHAIR SEAT: its box " + F(seat) + " m over the top, a jump and a step reach " + F(reachUp) + " m"); }
            float edge = UnityEngine.Mathf.Min(UnityEngine.Mathf.Min(cb.min.x - tx0, tx1 - cb.max.x), UnityEngine.Mathf.Min(cb.min.z - tz0, tz1 - cb.max.z));
            Line(edge >= chairEdge, "CHAIR EDGES: his chair's box (x " + F(cb.min.x) + " to " + F(cb.max.x) + ", z " + F(cb.min.z) + " to " + F(cb.max.z) + ") is " + F(edge) + " m inside the flat top's nearest edge (" + F1(chairEdge) + " wanted)");
        }
        var near = new System.Collections.Generic.List<string>();
        foreach (var c in UnityEngine.Physics.OverlapCapsule(head + V(0f, 0.1f + headR, 0f), head + V(0f, cc.height, 0f), headR, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        { if (c.bounds.max.y <= topY + 0.1f || c.transform.IsChildOf(pc.transform)) continue; var cp = c.ClosestPoint(head + V(0f, 0.5f, 0f)); float d = P2(cp.x - head.x, cp.z - head.z).magnitude; if (d < headR) near.Add(WalkIns.PathOf(c.transform) + " at " + F(d) + " m"); }
        Line(near.Count == 0, "HEAD CLEAR: within " + F1(headR) + " m of the scramble head (291.0, 120.6) over the top: " + (near.Count == 0 ? "nothing" : string.Join("; ", near)));
    }
    // ---- T3 EDGES
    {
        const float edgeIn = 1.0f, edgeStep = 1.5f, edgeSpread = 35f, edgeTime = 3f; const int escN = 16;
        var starts = new System.Collections.Generic.List<(string, UnityEngine.Vector3, float)>();
        for (float x = tx0 + edgeIn; x <= tx1 - edgeIn + 1e-3f; x += edgeStep) { starts.Add(("south edge x " + F1(x), V(x, topY, tz0 + edgeIn), 180f)); starts.Add(("north edge x " + F1(x), V(x, topY, tz1 - edgeIn), 0f)); }
        for (float z = tz0 + edgeIn; z <= tz1 - edgeIn + 1e-3f; z += edgeStep) { starts.Add(("east edge z " + F1(z), V(tx1 - edgeIn, topY, z), 90f)); if (Off(P2(tx0 + edgeIn, z), P2(land.x, land.z), P2(head.x, head.z)) > rampHalf + edgeIn) starts.Add(("west edge z " + F1(z), V(tx0 + edgeIn, topY, z), 270f)); }
        int runs = 0, stayed = 0, floor = 0, scramble = 0; var bad = new System.Collections.Generic.List<string>(); var escaped = new System.Collections.Generic.Dictionary<string, int>();
        foreach (var (name, p, outward) in starts)
        {
            if (!Clear(p)) { sb.Append("note: T3 start " + name + " is not clear, skipped\n"); continue; }
            foreach (var spread in new[] { -edgeSpread, 0f, edgeSpread }) foreach (var (jump, sprint, how) in new[] { (false, false, "walk"), (false, true, "sprint"), (true, true, "sprint-jump") })
            {
                Put(p); float yaw = (outward + spread) * UnityEngine.Mathf.Deg2Rad; var dir = V(UnityEngine.Mathf.Sin(yaw), 0f, UnityEngine.Mathf.Cos(yaw)); runs++;
                for (float t = 0f; t < edgeTime; t += dt) pc.Step(dir, jump, sprint, dt);
                var e = pc.transform.position; string s = name + " " + how + " heading " + F1((outward + spread + 360f) % 360f) + " to (" + F1(e.x) + ", " + F1(e.y) + ", " + F1(e.z) + ")";
                if (OnKnob(e)) { stayed++; continue; }
                if (OnScramble(e)) { scramble++; continue; }
                if (!OnFloor(e)) { bad.Add(s + ", perched " + F(e.y - H(e.x, e.z)) + " m over the ground"); continue; }
                floor++; var key = F1(e.x) + "," + F1(e.z); if (!escaped.ContainsKey(key)) escaped[key] = Escapes(e, escN); if (escaped[key] == 0) bad.Add(s + ", trapped (0 of " + escN + " escapes)");
            }
        }
        Line(bad.Count == 0, "T3 EDGES: " + runs + " runs (walk, sprint, sprint-jump at 0 and +-" + F1(edgeSpread) + " degrees out of " + starts.Count + " edge points): stayed on top " + stayed + ", onto the scramble " + scramble + ", landed on the floor and escaped " + floor + ", failed " + bad.Count + (bad.Count > 0 ? ": " + string.Join("; ", bad.GetRange(0, UnityEngine.Mathf.Min(8, bad.Count))) : ""));
    }
    // ---- T9 POCKETS (Marlow's grid round the whole foot)
    {
        const float podNear = 2f, podFar = 9f, podStep = 1f, podTime = 4f; const int escN = 8;
        var kc = V((kx0 + kx1) * 0.5f, 0f, (kz0 + kz1) * 0.5f); int runs = 0; var trapped = new System.Collections.Generic.List<string>();
        for (float x = kx0 - podFar; x <= kx1 + podFar + 1e-3f; x += podStep) for (float z = kz0 - podFar; z <= kz1 + podFar + 1e-3f; z += podStep)
        {
            float ox = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(kx0 - x, x - kx1)), oz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(kz0 - z, z - kz1)); float o = UnityEngine.Mathf.Sqrt(ox * ox + oz * oz);
            if (o < podNear || o > podFar) continue; var p = V(x, H(x, z), z); if (!Clear(p) || OnScramble(p)) continue;
            Put(p); var dir = V(kc.x - x, 0f, kc.z - z).normalized; runs++; for (float t = 0f; t < podTime; t += dt) pc.Step(dir, false, false, dt);
            var e = pc.transform.position; if (OnKnob(e) || OnScramble(e)) continue;
            if (!OnFloor(e)) { trapped.Add("from (" + F1(x) + ", " + F1(z) + ") perched at (" + F1(e.x) + ", " + F1(e.y) + ", " + F1(e.z) + ")"); continue; }
            if (Escapes(e, escN) == 0) trapped.Add("from (" + F1(x) + ", " + F1(z) + ") trapped at (" + F1(e.x) + ", " + F1(e.y) + ", " + F1(e.z) + ")");
        }
        Line(trapped.Count == 0, "T9 POCKETS: " + runs + " walks at the knob's centre from " + F1(podNear) + " to " + F1(podFar) + " m out every " + F1(podStep) + " m, perched or trapped " + trapped.Count + (trapped.Count > 0 ? ": " + string.Join("; ", trapped.GetRange(0, UnityEngine.Mathf.Min(8, trapped.Count))) : ""));
    }
    // ---- T11 WEDGES
    {
        const int escN = 48; var bf = c2.Find("BoulderField"); int n = 0;
        if (bf != null) foreach (UnityEngine.Transform b in bf)
        {
            if (!b.name.StartsWith("Boulder_2") || b.position.x < kx1) continue; var bb = PlaceKit.MeshBounds(b.gameObject); n++;
            foreach (var (side, z) in new[] { ("south", bb.min.z - cc.radius - 0.05f), ("north", bb.max.z + cc.radius + 0.05f) })
            {
                var p = V(kx1 + cc.radius + 0.05f, H(kx1 + cc.radius + 0.05f, z), z); int k = Escapes(p, escN);
                Line(k > 0, "T11 WEDGE: " + b.name + " at (" + F1(bb.center.x) + ", " + F1(bb.center.z) + "), " + side + " inner corner (" + F1(p.x) + ", " + F1(p.z) + "): " + k + " of " + escN + " headings escape");
            }
        }
        if (n != 2) Line(false, "T11 WEDGES: " + n + " rockfall boulders east of the face (2 wanted)");
    }
    // ---- T12 GULLY, WALKS
    {
        bool Walk(UnityEngine.Vector3 to, bool jump, ref float time, out float left)
        {
            for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = V(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, jump, jump, dt); time += dt; }
            var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
        }
        UnityEngine.Vector3 G(float x, float z) => V(x, H(x, z), z);
        var legs = new System.Collections.Generic.List<(string, UnityEngine.Vector3[], bool)>
        {
            ("T12 GULLY up, walking", new[] { foot, land, head }, false), ("T12 GULLY down, walking", new[] { head, land, foot }, false),
            ("T12 GULLY up, sprint-jumping", new[] { foot, land, head }, true), ("T12 GULLY down, sprint-jumping", new[] { head, land, foot }, true)
        };
        foreach (var n in new[] { "Boathouse to Camp 2", "Camp 2 to T" }) { var leg = Root("Trails").transform.Find(n); if (leg == null) { Line(false, "WALK: no Trails/" + n); continue; } var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position); legs.Add(("WALK: " + n + ", the trail", pts.ToArray(), false)); }
        legs.Add(("WALK: the trail end across the floor and up the scramble to the talk stand", new[] { G(298.9f, 107.8f), foot, land, head, talk }, false));
        legs.Add(("WALK: your seat round the table's north side to the booth door", new[] { G(297.5f, 98.6f), G(299.0f, 99.0f), G(299.6f, 99.2f) }, false));
        legs.Add(("WALK: the table to the trail end", new[] { G(298.5f, 98.7f), G(298.9f, 107.8f) }, false));
        float speed = tuning != null ? tuning.walkSpeed : 2.5f;
        foreach (var (name, pts, jump) in legs)
        {
            Put(pts[0]); float time = 0f, len = 0f; bool ok = true; string where = ""; for (int i = 1; i < pts.Length; i++) len += V(pts[i].x - pts[i - 1].x, 0f, pts[i].z - pts[i - 1].z).magnitude;
            for (int i = 1; i < pts.Length && ok; i++) if (!Walk(pts[i], jump, ref time, out float left)) { ok = false; where = ", stops " + F(left) + " m short of (" + F1(pts[i].x) + ", " + F1(pts[i].z) + ") at (" + F1(pc.transform.position.x) + ", " + F1(pc.transform.position.y) + ", " + F1(pc.transform.position.z) + ")"; }
            var e = pc.transform.position; if (ok && pts[pts.Length - 1].y >= topY - 0.1f && e.y < topY - 0.3f) { ok = false; where = ", arrives below the top at y " + F(e.y); }
            Line(ok, name + ", " + F1(len) + " m, " + F1(time) + " s" + (jump ? "" : " at " + F1(speed) + " m/s") + where);
        }
    }
    // ---- K-TOP: trunks and crowns
    {
        const float kTopR = 8f, scanR = 30f; var kc = P2(293f, 121f); var trunks = new System.Collections.Generic.List<string>(); var crowns = new System.Collections.Generic.List<string>();
        string[] notTree = { "Bush", "Fern", "Grass", "Rock", "Boulder", "Log", "Stump", "Flower", "Plant", "Moss", "Mushroom", "Rubble" };
        foreach (var lg in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None))
        {
            var t = lg.transform; var q = P2(t.position.x, t.position.z); if (UnityEngine.Vector2.Distance(q, kc) > scanR || !lg.gameObject.activeInHierarchy || t.IsChildOf(c2)) continue;
            bool tree = true; foreach (var w in notTree) if (t.name.Contains(w)) tree = false; if (!tree) continue;
            float d = UnityEngine.Vector2.Distance(q, kc); if (d < kTopR) trunks.Add(WalkIns.PathOf(t) + " at " + F(d) + " m");
            var lods = lg.GetLODs(); if (lods.Length == 0) continue; bool over = false;
            foreach (var r in lods[0].renderers) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null || !r.bounds.Intersects(new UnityEngine.Bounds(V((tx0 + tx1) * 0.5f, topY + 50f, (tz0 + tz1) * 0.5f), V(tx1 - tx0, 100f, tz1 - tz0)))) continue; foreach (var v in mf.sharedMesh.vertices) { var w = r.transform.TransformPoint(v); if (w.x > tx0 && w.x < tx1 && w.z > tz0 && w.z < tz1 && w.y > topY) { over = true; break; } } if (over) break; }
            if (over) crowns.Add(WalkIns.PathOf(t));
        }
        foreach (var ti in ter.terrainData.treeInstances) { var w = UnityEngine.Vector3.Scale(ti.position, ter.terrainData.size) + ter.transform.position; float d = UnityEngine.Vector2.Distance(P2(w.x, w.z), kc); if (d < kTopR) trunks.Add("terrain tree at (" + F1(w.x) + ", " + F1(w.z) + "), " + F(d) + " m"); }
        Line(trunks.Count == 0, "K-TOP TRUNKS: within " + F1(kTopR) + " m of (293, 121): " + (trunks.Count == 0 ? "none" : string.Join("; ", trunks)));
        Line(crowns.Count == 0, "K-TOP CROWNS: trees with an LOD0 vertex over the flat top: " + (crowns.Count == 0 ? "none" : string.Join("; ", crowns)));
    }
    // ---- S1 SIGHTLINE
    {
        const float seatEye = 1.2f, cube = 2f, fallbackR = 1.5f, fallbackStep = 0.25f; const int s1Px = 24, fallbackPix = 3, shotW = 3840, shotH = 1976;
        var targets = new[] { ("the highway", V(430f, 4f, 185f)), ("the gate T stop sign", V(423.05f, 5.3f, 166f)), ("the barrier arm", V(389.3f, 4.5f, 169.5f)) };
        if (chair == null) Line(false, "S1 SIGHTLINE: no chair");
        else
        {
            var cbd = chair.GetComponent<UnityEngine.Collider>().bounds; var seat = V(cbd.center.x, topY + seatEye, cbd.center.z);
            var cam = UnityEngine.Camera.main; var camParent = cam.transform.parent; var camPos = cam.transform.localPosition; var camRot = cam.transform.localRotation;
            var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode;
            var flat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Unlit")); flat.SetColor("_BaseColor", UnityEngine.Color.magenta); temps.Add(flat);
            var box = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); UnityEngine.Object.DestroyImmediate(box.GetComponent<UnityEngine.Collider>()); box.transform.localScale = V(cube, cube, cube); var boxR = box.GetComponent<UnityEngine.Renderer>(); boxR.sharedMaterial = flat; boxR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; temps.Add(box);
            var rt = new UnityEngine.RenderTexture(shotW, shotH, 24); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false); temps.Add(shot);
            int tol = set != null ? set.pixelTolerance : 8;
            try
            {
                urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled; cam.transform.SetParent(null, true); cam.targetTexture = rt;
                UnityEngine.Color32[] Shoot(UnityEngine.Vector3 eye, UnityEngine.Vector3 aim) { cam.transform.position = eye; cam.transform.rotation = UnityEngine.Quaternion.LookRotation(aim - eye); cam.Render(); var was = UnityEngine.RenderTexture.active; UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(false); UnityEngine.RenderTexture.active = was; return shot.GetPixels32(); }
                int Diff(UnityEngine.Color32[] a, UnityEngine.Color32[] b) { int n = 0; for (int i = 0; i < a.Length; i++) if (System.Math.Abs(a[i].r - b[i].r) > tol || System.Math.Abs(a[i].g - b[i].g) > tol || System.Math.Abs(a[i].b - b[i].b) > tol) n++; return n; }
                int Px(UnityEngine.Vector3 eye, UnityEngine.Vector3 at) { box.transform.position = at; Shoot(eye, at); boxR.forceRenderingOff = false; var on = Shoot(eye, at); boxR.forceRenderingOff = true; var off = Shoot(eye, at); return Diff(on, off); }
                boxR.forceRenderingOff = true; var n0 = Shoot(seat, targets[0].Item2); int noise = Diff(n0, Shoot(seat, targets[0].Item2));
                int failed = 0; var parts = new System.Collections.Generic.List<string>();
                foreach (var (label, at) in targets) { int px = Px(seat, at); if (px <= s1Px) failed++; parts.Add(label + " " + px + " px"); }
                Line(failed == 0 && noise == 0, "S1 SIGHTLINE: seated at his chair (" + F(seat.x) + ", " + F(seat.y) + ", " + F(seat.z) + "), a " + F1(cube) + " m cube at each target, more than " + s1Px + " px wanted: " + string.Join(", ", parts) + "; render noise " + noise + " px");
                if (failed > 0)
                {
                    // the doc's fallback: cells with clear rays (terrain and colliders) to all three, nearest first, the first fallbackPix pixel-tested
                    var cells = new System.Collections.Generic.List<UnityEngine.Vector3>();
                    for (float dx = -fallbackR; dx <= fallbackR + 1e-3f; dx += fallbackStep) for (float dz = -fallbackR; dz <= fallbackR + 1e-3f; dz += fallbackStep) if (P2(dx, dz).magnitude <= fallbackR) cells.Add(V(292f + dx, topY + seatEye, 122f + dz));
                    cells.Sort((a, b) => P2(a.x - 292f, a.z - 122f).sqrMagnitude.CompareTo(P2(b.x - 292f, b.z - 122f).sqrMagnitude));
                    int tried = 0; string found = null;
                    foreach (var c in cells)
                    {
                        bool clear = true; foreach (var (_, at) in targets) if (UnityEngine.Physics.Linecast(c, at, out var h, ~0, UnityEngine.QueryTriggerInteraction.Ignore) && !h.collider.transform.IsChildOf(chair) && h.collider.gameObject.layer != 2 && UnityEngine.Vector3.Distance(h.point, at) > cube) { clear = false; break; }
                        if (!clear) continue; if (tried++ >= fallbackPix) break;
                        var ps = new System.Collections.Generic.List<string>(); bool all = true; foreach (var (label, at) in targets) { int px = Px(c, at); if (px <= s1Px) all = false; ps.Add(px + " px"); }
                        sb.Append("note: S1 fallback cell (" + F(c.x) + ", " + F(c.z) + "): " + string.Join(", ", ps) + "\n"); if (all) { found = "(" + F(c.x) + ", " + F(c.z) + ")"; break; }
                    }
                    sb.Append("note: S1 fallback: " + (found != null ? "the nearest passing cell is " + found : "no cell of the first " + fallbackPix + " ray-clear ones passes") + "\n");
                }
            }
            finally
            {
                cam.targetTexture = null; cam.transform.SetParent(camParent, false); cam.transform.localPosition = camPos; cam.transform.localRotation = camRot; urp.gpuResidentDrawerMode = grdWas; rt.Release(); UnityEngine.Object.DestroyImmediate(rt);
            }
        }
    }
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.25a Camp 2 check (Camp2Layout.md draft 4)\n" + sb;
