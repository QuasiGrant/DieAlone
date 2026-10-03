// Main3 8.26 Camp 3 check (Play mode, Main3; Camp3Layout.md draft 2). In main3_review_capture.sh --area camp3's Play checks; also runs
// alone. Never saves; restores the player. Every move is PlayerController.Step (dt 0.02).
// CREEK FALLS (C5; Marlow's running-minimum sample): along each water strip (Layout826/Creek/Upper and Lower) from its source, the water
//   never rises more than riseTol over the lowest water upstream; the ground under the line stays under the water.
// WATER EDGE (Wren 2026-10-03): the water's edge stands edgeToTread m or more from every tread edge (trail centre lines less treadHalf),
//   the crossings being the plank bridge (its POI and Camp to J's anchor (104.8, 203.2), where trail and creek meet) and the stones;
//   except within crossR m of the two designed crossings (POI_Plank_bridge, POI_Stepping_stones).
// TENT (block 1): its one box is its mesh's local bounds (within boxTol per axis), long side east-west, door north, centred at
//   (75.0, 140.7); the studio's pieces (easel, stool, paint box, table, faced canvases) stand studioClear m or more off its box.
// SEAT LOG (C2): yaw 90 (north-south), its gaps to the fire pit and BigBoulders_4.
// SNAG LINE (C3): every piece's bottom hangs pieceClear m or more over the first walkable surface under it; the stake stands stakeUp tall.
// FACEROCK (T1): every Ground815/Stops/FaceRock mesh within hollowR of the hollow's centre has a collider; from the steps at (90.0, 143.3)
//   a capsule walking north 1 m meets rock or no rock mesh at all.
// FOOTBRIDGE, LOG STEPS, RIM SPUR (C8, C4, block 2): POI_Footbridge is gone; one POI_Log_steps, its StairRamp over Camp to Camp 3 P80 to
//   P94; no trail point and no Camp 3 piece named like a spur in the ravine box.
// WALKS (doc 4): Camp to Camp 3, W1 to Camp 3 and Pump to W1 along their trails; the arrival to the fire stand, the easel stand and the dam
//   stand; the steps' top to the east rim spot. Each arrives within arrive m, with times.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv); string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
const float dt = 0.02f, arrive = 0.5f, legTime = 200f, riseTol = 0.05f, edgeToTread = 1.0f, treadHalf = 1.2f, crossR = 3f, waterHalf = 0.6f, boxTol = 0.05f, studioClear = 2.7f, pieceClear = 2.85f, stakeUp = 3.6f, hollowR = 14f;
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
var c3 = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_3") : null; var L = c3 != null ? c3.Find("Layout826") : null; var d = c3 != null ? c3.Find("Dressing") : null;
var poiRoot = Root("PointsOfInterest").transform; var trails = Root("Trails").transform;
float SegDist(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
try
{
    if (L == null || d == null) return "no Campsites/Camp_3/Layout826 or Dressing (run main3_8_26_camp3.cs)";
    // ---- CREEK FALLS and WATER EDGE
    {
        var plank = poiRoot.Find("POI_Plank_bridge"); var stones = poiRoot.Find("POI_Stepping_stones");
        var crossings = new System.Collections.Generic.List<UnityEngine.Vector2> { P(104.8f, 203.2f) }; if (plank != null) crossings.Add(P(plank.position.x, plank.position.z)); if (stones != null) crossings.Add(P(stones.position.x, stones.position.z));
        var lines = new System.Collections.Generic.List<(string name, UnityEngine.Vector2[] pts)>(); foreach (UnityEngine.Transform lg in trails) { var l = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform p in lg) l.Add(P(p.position.x, p.position.z)); if (l.Count > 1) lines.Add((lg.name, l.ToArray())); }
        foreach (var run in new[] { "Upper", "Lower" })
        {
            var s = L.Find("Creek/" + run); var mf = s != null ? s.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null) { Line(false, "CREEK " + run + ": no water strip"); continue; }
            var vs = mf.sharedMesh.vertices; float low = float.MaxValue, worstRise = 0f, worstDry = float.MinValue; string riseAt = "", dryAt = "", edgeAt = ""; float leastEdge = float.MaxValue; int pairs = vs.Length / 2;
            for (int i = 0; i < pairs; i++)
            {
                var a = s.TransformPoint(vs[i * 2]); var b = s.TransformPoint(vs[i * 2 + 1]); var c = (a + b) * 0.5f;
                if (c.y - low > worstRise) { worstRise = c.y - low; riseAt = "(" + F1(c.x) + ", " + F1(c.z) + ")"; } low = UnityEngine.Mathf.Min(low, c.y);
                float dry = H(c.x, c.z) - c.y; if (dry > worstDry) { worstDry = dry; dryAt = "(" + F1(c.x) + ", " + F1(c.z) + ")"; }
                foreach (var e in new[] { a, b })
                {
                    var q = P(e.x, e.z); bool cross = false; foreach (var x in crossings) if (UnityEngine.Vector2.Distance(q, x) < crossR) cross = true; if (cross) continue;
                    foreach (var (name, pts) in lines) for (int k = 1; k < pts.Length; k++) { float gap = SegDist(q, pts[k - 1], pts[k]) - treadHalf; if (gap < leastEdge) { leastEdge = gap; edgeAt = name + " at (" + F1(e.x) + ", " + F1(e.z) + ")"; } }
                }
            }
            Line(worstRise <= riseTol, "CREEK FALLS " + run + ": " + pairs + " samples from its source, the largest rise over the running minimum " + F(worstRise) + " m" + (worstRise > 0f ? " at " + riseAt : "") + "; ground over the water at most " + F(worstDry) + " m (" + dryAt + ")");
            Line(leastEdge >= edgeToTread - 0.02f, "WATER EDGE " + run + ": the least water edge to tread edge " + F(leastEdge) + " m (at least " + F1(edgeToTread) + ", crossings within " + F1(crossR) + " m exempt), " + edgeAt);
        }
    }
    // ---- TENT
    {
        UnityEngine.Transform tent = null; foreach (UnityEngine.Transform t in d) if (t.name.StartsWith("CS_Tent_Old_2")) tent = t;
        var bc = tent != null ? tent.GetComponent<UnityEngine.BoxCollider>() : null;
        if (tent == null || bc == null) Line(false, "TENT: no Dressing/CS_Tent_Old_2 or its box");
        else
        {
            int boxes = tent.GetComponentsInChildren<UnityEngine.Collider>().Length; UnityEngine.Bounds lb = default; bool any = false;
            foreach (var mf in tent.GetComponentsInChildren<UnityEngine.MeshFilter>()) { if (mf.sharedMesh == null || mf.name.Contains("Rope")) continue; var mb = mf.sharedMesh.bounds; for (int i = 0; i < 8; i++) { var l = tent.InverseTransformPoint(mf.transform.TransformPoint(V((i & 1) == 0 ? mb.min.x : mb.max.x, (i & 2) == 0 ? mb.min.y : mb.max.y, (i & 4) == 0 ? mb.min.z : mb.max.z))); if (!any) { lb = new UnityEngine.Bounds(l, UnityEngine.Vector3.zero); any = true; } else lb.Encapsulate(l); } }
            var diff = bc.size - lb.size; bool fit = UnityEngine.Mathf.Abs(diff.x) <= boxTol && UnityEngine.Mathf.Abs(diff.y) <= boxTol && UnityEngine.Mathf.Abs(diff.z) <= boxTol && boxes == 1;
            var wb = bc.bounds; bool ew = wb.size.x > wb.size.z; bool at = UnityEngine.Vector2.Distance(P(wb.center.x, wb.center.z), P(75.0f, 140.7f)) <= 0.1f;
            Line(fit && ew && at, "TENT: " + boxes + " collider, box " + F(bc.size.x) + " x " + F(bc.size.y) + " x " + F(bc.size.z) + " against its mesh " + F(lb.size.x) + " x " + F(lb.size.y) + " x " + F(lb.size.z) + "; world x " + F(wb.min.x) + " to " + F(wb.max.x) + ", z " + F(wb.min.z) + " to " + F(wb.max.z) + " (doc x 72.97 to 77.04, z 139.64 to 141.77), long side " + (ew ? "east-west" : "north-south"));
            var near = new System.Collections.Generic.List<string>();
            foreach (var path in new[] { "Dressing/Easel", "Layout826/PaintBox", "Layout826/PlankTable", "Layout826/FacedCanvases" })
            {
                var t = c3.Find(path); if (t == null) { near.Add(path + " missing"); continue; }
                foreach (var r in t.GetComponentsInChildren<UnityEngine.Renderer>()) { var rb = r.bounds; float gx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(wb.min.x - rb.max.x, rb.min.x - wb.max.x)), gz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(wb.min.z - rb.max.z, rb.min.z - wb.max.z)); float g = UnityEngine.Mathf.Sqrt(gx * gx + gz * gz); if (g < studioClear) { near.Add(path + " " + F(g) + " m"); break; } }
            }
            Line(near.Count == 0, "TENT STUDIO GAP: the studio's pieces " + F1(studioClear) + " m or more off the tent's box" + (near.Count > 0 ? "; not: " + string.Join(", ", near) : ""));
        }
    }
    // ---- SEAT LOG
    {
        UnityEngine.Transform log = null; foreach (UnityEngine.Transform t in d) if (t.name.StartsWith("CS_Log_Large_Long_Seat_1")) log = t;
        var fire = d.Find("Fire");
        if (log == null || fire == null) Line(false, "SEAT LOG: no seat log or fire");
        else
        {
            float yaw = log.eulerAngles.y; var lb = PlaceKit.MeshBounds(log.gameObject); var fb = PlaceKit.MeshBounds(fire.gameObject);
            float toPit = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(fb.min.x - lb.max.x, lb.min.x - fb.max.x));
            Line(UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(yaw, 90f)) <= 1f || UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(yaw, 270f)) <= 1f, "SEAT LOG: yaw " + F1(yaw) + ", z " + F(lb.min.z) + " to " + F(lb.max.z) + " at x " + F(lb.center.x) + " (doc x 73.8, z 149.2 to 151.2); " + F(toPit) + " m off the fire's pieces across x");
        }
    }
    // ---- SNAG LINE
    {
        var line = L.Find("SnagLine");
        if (line == null) Line(false, "SNAG LINE: no Layout826/SnagLine");
        else
        {
            var lows = new System.Collections.Generic.List<string>(); float least = float.MaxValue;
            for (int i = 1; i <= 4; i++)
            {
                var piece = line.Find("Piece" + i); if (piece == null) { lows.Add("no Piece" + i); continue; }
                var pb = PlaceKit.MeshBounds(piece.gameObject); var foot = V(pb.center.x, pb.min.y, pb.center.z); float under = H(foot.x, foot.z);
                foreach (var h in UnityEngine.Physics.RaycastAll(foot, UnityEngine.Vector3.down, 40f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (h.collider.gameObject.layer != 2 && !h.collider.transform.IsChildOf(pc.transform)) under = UnityEngine.Mathf.Max(under, h.point.y);
                float clear = foot.y - under; least = UnityEngine.Mathf.Min(least, clear); if (clear < pieceClear) lows.Add("Piece" + i + " " + F(clear) + " m");
            }
            var stake = line.Find("Stake"); float stakeH = stake != null ? stake.GetComponent<UnityEngine.Collider>().bounds.max.y - H(stake.position.x, stake.position.z) : -1f;
            Line(lows.Count == 0 && UnityEngine.Mathf.Abs(stakeH - stakeUp) <= 0.05f, "SNAG LINE: the least piece clearance " + F(least) + " m (at least " + F(pieceClear) + ")" + (lows.Count > 0 ? "; low: " + string.Join(", ", lows) : "") + "; stake " + F(stakeH) + " m (" + F1(stakeUp) + ")");
        }
    }
    // ---- FACEROCK (T1)
    {
        var fr = Root("Ground815") != null ? Root("Ground815").transform.Find("Stops/FaceRock") : null; int inR = 0, solid = 0; var bare = new System.Collections.Generic.List<string>();
        if (fr != null) foreach (var mr in fr.GetComponentsInChildren<UnityEngine.MeshRenderer>())
        {
            if (new UnityEngine.Vector2(mr.transform.position.x - 78f, mr.transform.position.z - 146f).magnitude > hollowR) continue; var lodG = mr.GetComponentInParent<UnityEngine.LODGroup>(); if (lodG != null && lodG.GetLODs().Length > 0 && System.Array.IndexOf(lodG.GetLODs()[0].renderers, mr) < 0) continue;
            inR++; if (mr.GetComponent<UnityEngine.Collider>() != null || mr.GetComponentInParent<UnityEngine.Collider>() != null) solid++; else bare.Add(WalkIns.PathOf(mr.transform) + " (" + F1(mr.transform.position.x) + ", " + F1(mr.transform.position.z) + ")");
        }
        Line(inR > 0 && bare.Count == 0, "FACEROCK: " + solid + " of " + inR + " rocks within " + F1(hollowR) + " m of the hollow's centre have a collider" + (bare.Count > 0 ? "; bare: " + string.Join(", ", bare) : ""));
        var from = V(90.0f, H(90.0f, 143.3f), 143.3f); float r = cc.radius; var p0 = from + V(0f, r + 0.15f, 0f); var p1 = from + V(0f, cc.height - r, 0f);
        bool hit = UnityEngine.Physics.CapsuleCast(p0, p1, r, UnityEngine.Vector3.forward, out var hh, 1f, ~0, UnityEngine.QueryTriggerInteraction.Ignore); string what = hit ? WalkIns.PathOf(hh.collider.transform) + " at " + F(hh.distance) + " m" : "nothing";
        bool meshInPath = false; if (!hit && fr != null) { var sweep = new UnityEngine.Bounds((p0 + p1) * 0.5f + V(0f, 0f, 0.5f), V(2f * r, cc.height, 1f + 2f * r)); foreach (var mr in fr.GetComponentsInChildren<UnityEngine.MeshRenderer>()) if (mr.bounds.Intersects(sweep)) meshInPath = true; }
        Line(hit || !meshInPath, "T1 STEPS: walking north 1 m from (90.0, 143.3) the body meets " + what + (meshInPath ? ", yet a FaceRock mesh lies in its path (walk-through rock)" : ""));
    }
    // ---- FOOTBRIDGE, LOG STEPS, RIM SPUR
    {
        int steps = 0; UnityEngine.Transform st = null; foreach (UnityEngine.Transform t in poiRoot) if (t.name == "POI_Log_steps") { steps++; st = t; }
        var ramp = st != null ? st.Find("StairRamp") : null; var leg = trails.Find("Camp to Camp 3"); var p80 = leg != null ? leg.Find("P80") : null; var p94 = leg != null ? leg.Find("P94") : null; string rampAt = "no StairRamp";
        bool rampOk = false; if (ramp != null && p80 != null && p94 != null) { var rc = ramp.GetComponent<UnityEngine.Collider>(); float t80 = rc.bounds.max.y, top80 = float.NaN, top94 = float.NaN; if (rc.Raycast(new UnityEngine.Ray(p80.position + V(0f, 2f, 0f) + (p94.position - p80.position).normalized * 0.3f, UnityEngine.Vector3.down), out var a, 5f)) top80 = a.point.y; if (rc.Raycast(new UnityEngine.Ray(p94.position + V(0f, 2f, 0f) - (p94.position - p80.position).normalized * 0.3f, UnityEngine.Vector3.down), out var b, 5f)) top94 = b.point.y; rampOk = !float.IsNaN(top80) && !float.IsNaN(top94); rampAt = "ramp top " + F(top80) + " near P80 (" + F(p80.position.y) + "), " + F(top94) + " near P94 (" + F(p94.position.y) + ")"; }
        Line(poiRoot.Find("POI_Footbridge") == null && steps == 1 && rampOk, "FOOTBRIDGE gone: " + (poiRoot.Find("POI_Footbridge") == null) + "; POI_Log_steps " + steps + " (one); " + rampAt);
        var inRavine = new System.Collections.Generic.List<string>(); var ravine = new UnityEngine.Rect(84f, 153f, 102f - 84f, 177f - 153f);
        foreach (UnityEngine.Transform lg in trails) foreach (UnityEngine.Transform p in lg) if (ravine.Contains(P(p.position.x, p.position.z))) inRavine.Add(lg.name + "/" + p.name);
        foreach (var t in c3.GetComponentsInChildren<UnityEngine.Transform>(true)) if (t.name.ToLowerInvariant().Contains("spur")) inRavine.Add(WalkIns.PathOf(t));
        Line(inRavine.Count == 0, "RIM SPUR: trail points or spur pieces in the ravine (x 84 to 102, z 153 to 177): " + (inRavine.Count == 0 ? "none" : string.Join(", ", inRavine)));
    }
    // ---- WALKS
    {
        void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
        bool Walk(UnityEngine.Vector3 to, ref float time, out float left)
        {
            for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var dd = V(to.x - p.x, 0f, to.z - p.z); if (dd.magnitude < arrive * 0.5f) break; pc.Step(dd.normalized, false, false, dt); time += dt; }
            var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
        }
        UnityEngine.Vector3 G(float x, float z) => V(x, H(x, z), z);
        var legs = new System.Collections.Generic.List<(string, UnityEngine.Vector3[])>();
        foreach (var n in new[] { "Camp to Camp 3", "W1 to Camp 3", "Pump to W1" }) { var lg = trails.Find(n); if (lg == null) { Line(false, "WALK: no Trails/" + n); continue; } var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in lg) pts.Add(p.position); legs.Add((n + ", the trail", pts.ToArray())); }
        var arrival = G(79.06f, 145.39f);
        legs.Add(("the arrival to the fire stand", new[] { arrival, G(74.6f, 151.2f) })); legs.Add(("the arrival round the table to the easel stand", new[] { arrival, G(79.7f, 146.6f), G(79.6f, 148.2f), G(79.0f, 149.9f) })); legs.Add(("the arrival to the dam stand", new[] { arrival, G(82.6f, 147.2f) }));
        legs.Add(("the steps' top to the east rim spot", new[] { G(96.5f, 142.8f), G(94.0f, 138.5f) }));
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
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.26 Camp 3 check (Camp3Layout.md draft 2)\n" + sb;
