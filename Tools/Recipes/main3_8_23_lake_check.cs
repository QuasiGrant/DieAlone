// Main3 8.23 lake check (Play mode, Main3; LakeLayout.md draft 2). In main3_review_capture.sh --area lake's Play checks; also runs alone.
// Never saves; restores the player, the camera and the GPU Resident Drawer. Frames at Grant's size in outDir.
// LAMP: no lamp at the boathouse (no practical under Lake/Boathouse).
// SIGHTLINES (doc 5.4): S1 seated at the chair (238.8, -2.6, 56.4) to the tower cab, every drawn mesh (glass seen through; a hit on the
//   tower reaches it). S2 the reeds stand heading 290 and S3 the slip stand heading 270: frames, with the share of rays above the
//   horizon that meet no drawn collider within skyFar m (the terrain or one with a renderer; S2: sky in frame; S3: none by design).
// DOCK RAILS (8.23 round 2): Marlow's rail chain and a ring of jumps round the rail ends land on no rail and not in the closed lake.
// BOWL (8.23 round 2): how often a wader in the shallows, standing or jumping, reaches the bowl with the interactor, reported (Wren: the
//   step-floor rule is Milestone 10); fails only if the step floor itself cannot reach it.
// SLIP AND STEP: nothing in the slip or on the step gives a step over a rail: no collider top between stepLow and stepHigh m over the
//   floor within railNear m of a slip or step rail (the slip rest starts at 0.9).
// PLACES ON OBJECTS (doc 5.2). GAPS: the pieces 8.17 and 8.23 built (Boathouse/Dressing and Layout823) against anything, slots of 0.6
//   to 1.0 m with nothing else in them fail; slots under the house floor, inside the skirts, are out of reach.
// WALKS (doc 4): pump to the gangway foot (the trail), gangway foot to the step through the house, step to the slip stand, step off the
//   open edge into the shallows and out by the beach to the reeds stand, gangway foot round the step skirt to the under-stilts point and
//   back out, and (8.23 round 2) the cabin door to the pump stand, each arriving, with times (PlayerController.Step, dt 0.02, walk speed).
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_lake");
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv); string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation; float camFov = cam.fieldOfView;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
float stepHigh = (tuning != null ? tuning.jumpHeight + tuning.stepOffset : 0.7f);   // the highest top a standing jump reaches (Marlow: the crate's 0.80 is out of it)
const float dt = 0.02f, arrive = 0.5f, legTime = 90f, eyeH = 1.6f, skyFar = 400f, floorY = -3.8f, stepLow = 0.4f, railNear = 1.0f;
const float placeSlack = 0.3f, gapLow = 0.6f, gapHigh = 1.0f, gapTol = 0.005f, bodyLow = 0.3f, reachLow = 0.5f, gapNear = 1.0f;
const int shotW = 3840, shotH = 1976, skyRaysX = 24, skyRaysY = 12;
var lake = Root("Lake").transform; var B = lake.Find("Boathouse");
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
System.IO.Directory.CreateDirectory(outDir);
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode; urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
void Pose(UnityEngine.Vector3 e, UnityEngine.Vector3 at)
{
    var dir = at - e; var flat = V(dir.x, 0f, dir.z); cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = e - pc.transform.rotation * camLocal;
    cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
}
void Shoot(string file) { cam.targetTexture = rt; cam.Render(); cam.Render(); cam.targetTexture = null; UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null; System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), shot.EncodeToPNG()); }
void MeshBlockers(System.Collections.Generic.List<(UnityEngine.Vector3 a, UnityEngine.Vector3 b)> segs)
{
    var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var r in l[i].renderers) if (r != null) notLod0.Add(r); }
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.name.Contains("Glass") || mr.transform.IsChildOf(pc.transform)) continue;
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; bool crossed = false;
        foreach (var (a, b) in segs) { var d = b - a; if (mr.bounds.IntersectRay(new UnityEngine.Ray(a, d.normalized), out float dist) && dist <= d.magnitude) { crossed = true; break; } }
        if (!crossed) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
}
string FirstBlock(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform own)
{
    var d = b - a; float fo = float.MaxValue, fw = float.MaxValue; string what = null;
    foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
    { var ht = h.collider.transform; if (ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2 || h.collider.name.Contains("Glass")) continue; if (own != null && ht.IsChildOf(own)) fw = UnityEngine.Mathf.Min(fw, h.distance); else if (h.distance < fo) { fo = h.distance; what = WalkIns.PathOf(ht) + " at " + F1(h.distance) + " m"; } }
    return fo == float.MaxValue || fw < fo ? null : what;
}
// a collider the eye can see: the terrain, or one with a renderer on its object or under it (8.23 round 2, Sable 823 4.2: the Ward
// climb's renderer-less ring colliders counted as "not sky" and S2 read 2 percent against a frame full of sky)
var drawnCache = new System.Collections.Generic.Dictionary<UnityEngine.Collider, bool>();
bool Drawn(UnityEngine.Collider c) { if (drawnCache.TryGetValue(c, out var d)) return d; d = c is UnityEngine.TerrainCollider || c.GetComponentInChildren<UnityEngine.Renderer>() != null; drawnCache[c] = d; return d; }
float SkyShare(UnityEngine.Vector3 eye, float heading)   // rays over the frame's upper half above the horizon that meet no drawn collider within skyFar
{
    int up = 0, open = 0; float hfov = 2f * UnityEngine.Mathf.Atan(UnityEngine.Mathf.Tan(camFov * 0.5f * UnityEngine.Mathf.Deg2Rad) * shotW / shotH) * UnityEngine.Mathf.Rad2Deg;
    for (int i = 0; i < skyRaysX; i++) for (int j = 0; j < skyRaysY; j++)
    {
        float yaw = heading - hfov * 0.5f + hfov * (i + 0.5f) / skyRaysX, pitch = camFov * 0.5f * (j + 0.5f) / skyRaysY; if (pitch <= 0f) continue; up++;
        var d = UnityEngine.Quaternion.Euler(-pitch, yaw, 0f) * UnityEngine.Vector3.forward; bool hit = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(eye, d, skyFar, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { if (h.collider.transform.IsChildOf(pc.transform) || h.collider.gameObject.layer == 2 || !Drawn(h.collider)) continue; hit = true; break; }
        if (!hit) open++;
    }
    return up > 0 ? open / (float)up : 0f;
}
try
{
    // ---- LAMP
    int practicals = B.GetComponentsInChildren<PracticalLight>(true).Length; Line(practicals == 0, "LAMP: practicals under Lake/Boathouse " + practicals + " (Wren: no window lamp)");
    // ---- SIGHTLINES
    {
        var tower = Root("Camp").transform.Find("Tower"); var cab = tower.Find("Cab").position + V(0f, 1.6f, 0f);
        var chair = V(238.8f, -2.6f, 56.4f); var reedsEye = V(244.1f, H(244.1f, 62.0f) + eyeH, 62.0f); var slipEye = V(241.3f, floorY + eyeH, 52.4f);
        var segs = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3)> { (chair, cab) }; MeshBlockers(segs);
        string s1 = FirstBlock(chair, cab, tower); Line(s1 == null, "S1: seated at the chair (238.8, -2.6, 56.4) to the tower cab, " + F1(UnityEngine.Vector3.Distance(chair, cab)) + " m, every drawn mesh: " + (s1 ?? "clear"));
        foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        foreach (var (name, eye, heading, wantSky) in new[] { ("S2", reedsEye, 290f, true), ("S3", slipEye, 270f, false) })
        {
            var at = eye + UnityEngine.Quaternion.Euler(0f, heading, 0f) * UnityEngine.Vector3.forward * 10f; cam.fieldOfView = camFov; Pose(eye, at); Shoot(name + ".png");
            float sky = SkyShare(eye, heading); Line(wantSky ? sky > 0f : true, name + ": from (" + F1(eye.x) + ", " + F1(eye.y) + ", " + F1(eye.z) + ") heading " + F1(heading) + ", sky in the upper half of the frame " + (sky * 100f).ToString("F0", inv) + " percent (" + (wantSky ? "sky wanted" : "none by design") + ") | " + name + ".png");
        }
    }
    // ---- SLIP AND STEP: no step over a rail
    {
        var rails = new System.Collections.Generic.List<UnityEngine.Bounds>(); foreach (var c in B.GetComponentsInChildren<UnityEngine.Collider>()) if (c.name == "SlipRail" || c.name == "StepRail") rails.Add(c.bounds);
        var bad = new System.Collections.Generic.List<string>();
        foreach (var c in B.GetComponentsInChildren<UnityEngine.Collider>())
        {
            if (!c.enabled || c.name == "SlipRail" || c.name == "StepRail") continue; var b = c.bounds; float top = b.max.y - floorY; if (top < stepLow || top > stepHigh || b.min.y > floorY + stepHigh) continue;
            foreach (var r in rails) { var e = r; e.Expand(V(2f * railNear, 10f, 2f * railNear)); if (e.Intersects(b)) { bad.Add(WalkIns.PathOf(c.transform) + " top " + F(top)); break; } }
        }
        Line(bad.Count == 0 && rails.Count > 0, "SLIP AND STEP: " + rails.Count + " rails; colliders with a top " + F1(stepLow) + " to " + F1(stepHigh) + " m over the floor within " + F1(railNear) + " m of one: " + (bad.Count == 0 ? "none" : string.Join("; ", bad)));
    }
    // ---- BOWL FROM THE WATER (Wren 2026-10-02: the bowl answers only from the step floor; Pim's spacing 4): the bowl gets a temporary box
    // collider round its mesh (it has none yet); from the off-step point (240.0, 57.6) and every wader stand on a bowlGrid m grid of the
    // shallows within bowlNear m of it, standing and at the top of a jump, the interactor's ray (its mask, interactReach m) toward
    // bowlAims points on that box must meet the step or nothing before the bowl. Control: the same rays from the step floor reach it.
    {
        var bowlT = B.Find("Dressing/Step/CITW_Bowl_Small"); var pi = UnityEngine.Object.FindFirstObjectByType<PlayerInteractor>();
        var maskF = typeof(PlayerInteractor).GetField("mask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        if (bowlT == null || pi == null || maskF == null || tuning == null) Line(false, "BOWL: no Dressing/Step/CITW_Bowl_Small, PlayerInteractor, its mask or PlayerTuning");
        else
        {
            int mask = ((UnityEngine.LayerMask)maskF.GetValue(pi)).value; float reach = tuning.interactReach, jumpH = tuning.jumpHeight;
            var bb = PlaceKit.MeshBounds(bowlT.gameObject); var bc = bowlT.gameObject.AddComponent<UnityEngine.BoxCollider>(); bc.center = bowlT.InverseTransformPoint(bb.center); bc.size = bb.size; temps.Add(bc); UnityEngine.Physics.SyncTransforms();
            var aims = new System.Collections.Generic.List<UnityEngine.Vector3> { bb.center, V(bb.center.x, bb.max.y, bb.center.z) };
            for (int i = 0; i < 8; i++) aims.Add(V((i & 1) == 0 ? bb.min.x : bb.max.x, (i & 2) == 0 ? bb.min.y + 0.01f : bb.max.y, (i & 4) == 0 ? bb.min.z : bb.max.z));
            int Reaches(UnityEngine.Vector3 eye) { int n = 0; foreach (var a in aims) { var d = a - eye; if (UnityEngine.Physics.Raycast(eye, d.normalized, out var h, reach, mask, UnityEngine.QueryTriggerInteraction.Ignore) && h.collider == bc) n++; } return n; }
            UnityEngine.Vector3 Stand(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.3f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 40; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); return pc.transform.position; }
            float eyeOver = (pc.transform.rotation * camLocal).y;
            const float bowlGrid = 0.25f, bowlNear = 2.6f, waderMaxY = -5.6f, wadeBody = 0.35f;
            var off = Stand(V(240.0f, -6.0f, 57.6f)); int offStand = Reaches(off + V(0f, eyeOver, 0f)), offJump = Reaches(off + V(0f, eyeOver + jumpH, 0f));
            int stands = 0, standHits = 0, jumpHits = 0; string worst = "";
            for (float x = bb.center.x - bowlNear; x <= bb.center.x + bowlNear + 1e-3f; x += bowlGrid) for (float z = bb.center.z - bowlNear; z <= bb.center.z + bowlNear + 1e-3f; z += bowlGrid)
            {
                if (new UnityEngine.Vector2(x - bb.center.x, z - bb.center.z).magnitude > bowlNear || H(x, z) > waderMaxY + 0.3f || (x > 236.8f - wadeBody && x < 243.2f + wadeBody && z < 57.0f + wadeBody)) continue;   // not under the house or step, inside their skirts
                var s = Stand(V(x, H(x, z), z)); if (s.y > waderMaxY) continue; stands++;
                int a = Reaches(s + V(0f, eyeOver, 0f)), j = Reaches(s + V(0f, eyeOver + jumpH, 0f)); if (a > 0) standHits++; if (j > 0) jumpHits++; if ((a > 0 || j > 0) && worst == "") worst = " first at (" + F(s.x) + ", " + F(s.y) + ", " + F(s.z) + ")";
            }
            var ctl = Stand(V(240.8f, floorY, 56.0f)); int ctlHits = Reaches(ctl + V(0f, eyeOver, 0f));
            Line(ctlHits > 0, "BOWL (Wren 2026-10-02: a must-stand-on-the-step rule comes in Milestone 10, so only the step-floor control fails here): from the off-step point, standing at (" + F(off.x) + ", " + F(off.y) + ", " + F(off.z) + "), the interactor's ray (reach " + F1(reach) + " m) meets the bowl on " + offStand + " of " + aims.Count + " aims, at a jump's top " + offJump + "; " + stands + " wader stands within " + F1(bowlNear) + " m: standing " + standHits + ", jumping " + jumpHits + worst + "; control from the step floor: " + ctlHits + " of " + aims.Count);
        }
    }
    // ---- DOCK RAILS (8.23 round 2, Marlow 823 finding 1): his chain (sprint-jump 150 from the bank onto the east rail, 210 onto the end
    // rail, 180 off it into the lake), then a ring of starts on a ringGrid m grid within ringNear m of each rail's ends and the end rail,
    // ringHeadings headings, walk-jump and sprint-jump, each move followed to its landing: none may end on a rail, a rail stop or in the
    // closed lake (Main3AreaSet.closedFills)
    {
        var setA = Main3AreaSet.Load(); System.Func<UnityEngine.Vector3, bool> inLake = p => false;
        if (setA.closedFills != null) foreach (var f in setA.closedFills) if (f.label == "lake") inLake = Main3AreaSet.ClosedWater(f, H, c => c.transform.IsChildOf(pc.transform));
        var dock = lake.Find("Dock"); var railBounds = new System.Collections.Generic.List<UnityEngine.Bounds>();
        foreach (var c in dock.GetComponentsInChildren<UnityEngine.Collider>()) if (c.name == "Rail" || c.name == "RailEnd" || c.name == "RailStop") railBounds.Add(c.bounds);
        UnityEngine.Vector3 Jump(UnityEngine.Vector3 from, float heading, bool sprint)
        {
            cc.enabled = false; pc.transform.position = from + V(0f, 0.05f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 3; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
            var d = UnityEngine.Quaternion.Euler(0f, heading, 0f) * UnityEngine.Vector3.forward; for (float t = 0f; t < 1f; t += dt) pc.Step(d, true, sprint, dt);
            for (int i = 0; i < 500 && !cc.isGrounded; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); for (int i = 0; i < 10; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); return pc.transform.position;
        }
        bool OnRail(UnityEngine.Vector3 p) { foreach (var b in railBounds) { var e = b; e.Expand(V(0.7f, 0f, 0.7f)); if (e.Contains(V(p.x, b.center.y, p.z)) && p.y >= b.max.y - 0.3f) return true; } return false; }
        string Bad(UnityEngine.Vector3 p) => OnRail(p) ? "on a rail" : inLake(p) ? "in the closed lake" : null;
        var e1 = Jump(V(191.30f, -4.56f, 88.92f), 150f, true); var e2 = Jump(V(191.45f, -3.81f, 88.75f), 210f, true); var e3 = Jump(V(190.02f, -3.79f, 86.27f), 180f, true);
        string chain = "repro 1 ends (" + F(e1.x) + ", " + F(e1.y) + ", " + F(e1.z) + ") " + (Bad(e1) ?? "ok") + "; repro 2 from the east rail ends (" + F(e2.x) + ", " + F(e2.y) + ", " + F(e2.z) + ") " + (Bad(e2) ?? "ok") + "; repro 3 from the end rail ends (" + F(e3.x) + ", " + F(e3.y) + ", " + F(e3.z) + ") " + (Bad(e3) ?? "ok");
        bool chainOk = Bad(e1) == null;   // repros 2 and 3 start on rails a player can no longer reach; they are reported, not failed
        const float ringGrid = 0.25f, ringNear = 1.5f, waterY = -5.5f; const int ringHeadings = 16; int moves = 0, bad = 0; string firstBad = "";
        var centres = new[] { V(188.44f, 0f, 88.6f), V(191.56f, 0f, 88.6f), V(188.44f, 0f, 86.44f), V(191.56f, 0f, 86.44f), V(190f, 0f, 87.5f) };
        foreach (var c0 in centres) for (float x = c0.x - ringNear; x <= c0.x + ringNear + 1e-3f; x += ringGrid) for (float z = c0.z - ringNear; z <= c0.z + ringNear + 1e-3f; z += ringGrid)
        {
            var s = V(x, H(x, z), z); foreach (var rb in railBounds) if (rb.Contains(V(x, rb.center.y, z))) goto nextStart; if (s.y < waterY) continue;
            UnityEngine.Physics.SyncTransforms(); foreach (var h in UnityEngine.Physics.RaycastAll(V(x, -2f, z), UnityEngine.Vector3.down, 6f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if ((h.collider.name == "DeckFlat" || h.collider.name == "DeckRamp") && h.point.y > s.y) s.y = h.point.y;   // starts on the ground or the deck only, never on a stop
            if (s.y < waterY) continue;
            bool blocked = false; foreach (var c in UnityEngine.Physics.OverlapCapsule(s + V(0f, cc.radius + 0.05f, 0f), s + V(0f, cc.height - cc.radius, 0f), cc.radius, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!(c is UnityEngine.TerrainCollider) && !c.transform.IsChildOf(pc.transform)) blocked = true;
            if (blocked) continue;   // the body cannot stand there (inside a ring box, a post or a rail stop)
            for (int k = 0; k < ringHeadings; k++) foreach (var sprint in new[] { false, true })
            { var e = Jump(s, k * 360f / ringHeadings, sprint); moves++; var why = Bad(e); if (why == null) continue; bad++; if (bad <= 5) firstBad += "\n  from (" + F(s.x) + ", " + F(s.y) + ", " + F(s.z) + ") heading " + F1(k * 360f / ringHeadings) + (sprint ? " sprint" : " walk") + "-jump ends (" + F(e.x) + ", " + F(e.y) + ", " + F(e.z) + ") " + why; }
            nextStart:;
        }
        Line(chainOk && bad == 0, "DOCK RAILS: " + chain + "; ring of " + moves + " jumps round the rail ends: " + bad + " end on a rail or in the closed lake" + firstBad);
    }
    // ---- PLACES ON OBJECTS
    {
        var set = Main3AreaSet.Load(); var A = set.Find("lake"); int bad = 0; var pl = new System.Text.StringBuilder();
        foreach (var p in A.places)
        {
            if (string.IsNullOrEmpty(p.objectPath)) continue; var t = Main3AreaSet.At(scene, p.objectPath); if (t == null) { bad++; pl.Append(p.label + ": no " + p.objectPath + "; "); continue; }
            var b = PlaceKit.MeshBounds(t.gameObject); foreach (var c in t.GetComponentsInChildren<UnityEngine.Collider>()) b.Encapsulate(c.bounds); b.Expand(2f * placeSlack);
            if (!(p.point.x >= b.min.x && p.point.x <= b.max.x && p.point.z >= b.min.z && p.point.z <= b.max.z)) { bad++; pl.Append(p.label + " (" + F1(p.point.x) + ", " + F1(p.point.z) + ") off " + p.objectPath + "; "); }
        }
        Line(bad == 0, "PLACES ON OBJECTS: every lake place with an object lies on it" + (bad > 0 ? ": " + pl : ""));
    }
    // ---- GAPS (as main3_8_22_front_check.cs, over the Lake root)
    {
        UnityEngine.Physics.SyncTransforms();
        UnityEngine.Transform Grp(UnityEngine.Collider c) { var root = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(c.gameObject); return root != null ? root.transform : c.transform; }
        bool Body(UnityEngine.Bounds b) { float g = UnityEngine.Mathf.Max(H(b.center.x, b.center.z), b.center.y > floorY - 0.5f && b.center.x > 236f && b.center.x < 244f && b.center.z > 49f && b.center.z < 57.5f ? floorY : float.MinValue); return b.max.y > g + bodyLow && b.min.y < g + reachLow; }
        var groups = new System.Collections.Generic.Dictionary<UnityEngine.Transform, UnityEngine.Bounds>(); var ofGroup = new System.Collections.Generic.Dictionary<UnityEngine.Collider, UnityEngine.Transform>();
        void Add(UnityEngine.Collider c) { if (!c.enabled || c.isTrigger || c is UnityEngine.TerrainCollider || c.transform.IsChildOf(pc.transform)) return; var g = Grp(c); ofGroup[c] = g; var b = c.bounds; if (!Body(b)) return; if (groups.TryGetValue(g, out var gb)) { gb.Encapsulate(b); groups[g] = gb; } else groups[g] = b; }
        var built = new System.Collections.Generic.HashSet<UnityEngine.Transform>(); foreach (var c in lake.GetComponentsInChildren<UnityEngine.Collider>()) { var cp = WalkIns.PathOf(c.transform); if (!(cp.StartsWith("Lake/Boathouse/Layout823") || cp.StartsWith("Lake/Boathouse/Dressing"))) continue; Add(c); if (ofGroup.TryGetValue(c, out var g)) built.Add(g); }   // the pieces 8.17 and 8.23 built; the 8.4 dock, pockets and ring were walked by Marlow (823 paper 11)
        var bands = new System.Collections.Generic.List<string>(); var seenPair = new System.Collections.Generic.HashSet<string>();
        foreach (var g in System.Linq.Enumerable.ToArray(built))
        {
            if (!groups.TryGetValue(g, out var gb)) continue; var probe = gb; probe.Expand(V(2f * gapNear, 0f, 2f * gapNear));
            foreach (var c in UnityEngine.Physics.OverlapBox(probe.center, probe.extents, UnityEngine.Quaternion.identity, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!ofGroup.ContainsKey(c)) Add(c);
            foreach (var kv in System.Linq.Enumerable.ToArray(groups))
            {
                var o = kv.Key; if (o == g) continue; var ob = kv.Value; if (!ob.Intersects(probe)) continue;
                string key = g.GetInstanceID() < o.GetInstanceID() ? g.GetInstanceID() + "_" + o.GetInstanceID() : o.GetInstanceID() + "_" + g.GetInstanceID(); if (!seenPair.Add(key)) continue;
                float gx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(gb.min.x - ob.max.x, ob.min.x - gb.max.x)), gz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(gb.min.z - ob.max.z, ob.min.z - gb.max.z)); float gap = UnityEngine.Mathf.Sqrt(gx * gx + gz * gz);
                if (gap < gapLow || gap >= gapHigh - gapTol) continue;
                var p1 = V(UnityEngine.Mathf.Clamp(ob.center.x, gb.min.x, gb.max.x), 0f, UnityEngine.Mathf.Clamp(ob.center.z, gb.min.z, gb.max.z)); var p2 = V(UnityEngine.Mathf.Clamp(p1.x, ob.min.x, ob.max.x), 0f, UnityEngine.Mathf.Clamp(p1.z, ob.min.z, ob.max.z));
                var m = (p1 + p2) * 0.5f; m.y = UnityEngine.Mathf.Max(gb.min.y, ob.min.y) + 1f; bool other = false;
                foreach (var c in UnityEngine.Physics.OverlapSphere(m, gap * 0.5f - 0.01f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { if (c is UnityEngine.TerrainCollider || !c.enabled) continue; var cg = ofGroup.TryGetValue(c, out var x) ? x : Grp(c); if (cg != g && cg != o) { other = true; break; } }
                bool underHouse = m.x > 236.8f && m.x < 243.2f && m.z > 49.6f && m.z < 55.2f && m.y < floorY + 0.8f;   // inside the skirts, under the floor
                if (!other && !underHouse) bands.Add(F(gap) + " m between " + WalkIns.PathOf(g) + " and " + WalkIns.PathOf(o) + " at (" + F1(m.x) + ", " + F1(m.z) + ")");
            }
        }
        Line(bands.Count == 0, "GAPS: " + built.Count + " built lake pieces and their neighbours, slots of " + F1(gapLow) + " to " + F1(gapHigh) + " m: " + bands.Count + (bands.Count > 0 ? "\n  " + string.Join("\n  ", bands) : ""));
    }
    // ---- WALKS
    {
        void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
        bool Walk(UnityEngine.Vector3 to, ref float time, out float left)
        {
            for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = V(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, false, false, dt); time += dt; }
            var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
        }
        var trail = new System.Collections.Generic.List<UnityEngine.Vector3>(); var leg = Root("Trails").transform.Find("Pump to boathouse"); if (leg != null) foreach (UnityEngine.Transform p in leg) trail.Add(p.position);
        UnityEngine.Vector3 G(float x, float z) => V(x, H(x, z), z); UnityEngine.Vector3 FL(float x, float z) => V(x, floorY, z);
        var legs = new System.Collections.Generic.List<(string, UnityEngine.Vector3[])>();
        if (trail.Count > 1) legs.Add(("pump to the gangway foot, the trail", trail.ToArray())); else Line(false, "WALK: no Trails/Pump to boathouse");
        legs.Add(("gangway foot to the step, through the house", new[] { G(247.6f, 52.4f), FL(244.0f, 52.4f), FL(242.2f, 52.4f), FL(240.0f, 54.4f), FL(240.0f, 55.7f), FL(240.0f, 56.3f) }));
        legs.Add(("step to the slip stand", new[] { FL(240.0f, 56.3f), FL(240.0f, 54.4f), FL(241.3f, 54.2f), FL(241.3f, 52.4f) }));
        legs.Add(("off the step's open edge, out by the beach to the reeds stand", new[] { FL(240.0f, 56.3f), G(240.0f, 57.8f), G(242.5f, 58.6f), G(244.0f, 59.0f), G(245.8f, 59.6f), G(245.6f, 61.4f), G(244.1f, 62.0f) }));
        legs.Add(("gangway foot down the beach round the step skirt to the under-stilts point and back to the beach", new[] { G(247.6f, 52.4f), G(246.0f, 56.0f), G(243.5f, 58.3f), G(240.0f, 58.4f), G(238.4f, 58.0f), G(237.5f, 55.95f), G(238.4f, 58.0f), G(243.5f, 58.3f), G(246.0f, 57.0f) }));
        // door to pump (Sable 823 4.3): from doorOut m outside the cabin door, to the nearest point of the Camp to pump trail, along it to
        // the pump stand (190, 96)
        const float doorOut = 1.0f;
        var cabin = Root("Camp").transform.Find("Cabin"); var doorT = cabin != null ? cabin.Find("Door") : null; var c2p = Root("Trails").transform.Find("Camp to pump");
        if (doorT == null || c2p == null) Line(false, "WALK: no Camp/Cabin/Door or Trails/Camp to pump");
        else
        {
            var outward = V(doorT.position.x - cabin.position.x, 0f, doorT.position.z - cabin.position.z).normalized; var from = doorT.position + outward * doorOut;
            var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in c2p) pts.Add(p.position);
            int near = 0; for (int i = 1; i < pts.Count; i++) if (V(pts[i].x - from.x, 0f, pts[i].z - from.z).sqrMagnitude < V(pts[near].x - from.x, 0f, pts[near].z - from.z).sqrMagnitude) near = i;
            var route = new System.Collections.Generic.List<UnityEngine.Vector3> { G(from.x, from.z) }; for (int i = near; i < pts.Count; i++) route.Add(pts[i]); route.Add(G(190f, 96f));
            legs.Insert(0, ("cabin door to the pump stand (Camp to pump from its point " + near + " of " + pts.Count + ")", route.ToArray()));
        }
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
    cam.fieldOfView = camFov; cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas;
    UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.23 lake check (LakeLayout.md draft 2); frames in " + outDir + "\n" + sb;
