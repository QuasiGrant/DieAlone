// Main3 8.23 lake check (Play mode, Main3; LakeLayout.md draft 2). In main3_review_capture.sh --area lake's Play checks; also runs alone.
// Never saves; restores the player, the camera and the GPU Resident Drawer. Frames at Grant's size in outDir.
// LAMP: no lamp at the boathouse (no practical under Lake/Boathouse).
// SIGHTLINES (doc 5.4): S1 seated at the chair (238.8, -2.6, 56.4) to the tower cab, every drawn mesh (glass seen through; a hit on the
//   tower reaches it). S2 the reeds stand heading 290 and S3 the slip stand heading 270: frames, with the share of rays above the
//   horizon that meet nothing within skyFar m (S2: sky in frame; S3: none by design).
// SLIP AND STEP: nothing in the slip or on the step gives a step over a rail: no collider top between stepLow and stepHigh m over the
//   floor within railNear m of a slip or step rail (the slip rest starts at 0.9).
// PLACES ON OBJECTS (doc 5.2). GAPS: the pieces 8.17 and 8.23 built (Boathouse/Dressing and Layout823) against anything, slots of 0.6
//   to 1.0 m with nothing else in them fail; slots under the house floor, inside the skirts, are out of reach.
// WALKS (doc 4): pump to the gangway foot (the trail), gangway foot to the step through the house, step to the slip stand, step off the
//   open edge into the shallows and out by the beach to the reeds stand, gangway foot round the step skirt to the under-stilts point and
//   back out, each arriving, with times (PlayerController.Step, dt 0.02, walk speed).
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
float SkyShare(UnityEngine.Vector3 eye, float heading)   // rays over the frame's upper half above the horizon that meet nothing within skyFar
{
    int up = 0, open = 0; float hfov = 2f * UnityEngine.Mathf.Atan(UnityEngine.Mathf.Tan(camFov * 0.5f * UnityEngine.Mathf.Deg2Rad) * shotW / shotH) * UnityEngine.Mathf.Rad2Deg;
    for (int i = 0; i < skyRaysX; i++) for (int j = 0; j < skyRaysY; j++)
    {
        float yaw = heading - hfov * 0.5f + hfov * (i + 0.5f) / skyRaysX, pitch = camFov * 0.5f * (j + 0.5f) / skyRaysY; if (pitch <= 0f) continue; up++;
        var d = UnityEngine.Quaternion.Euler(-pitch, yaw, 0f) * UnityEngine.Vector3.forward; bool hit = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(eye, d, skyFar, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { if (h.collider.transform.IsChildOf(pc.transform) || h.collider.gameObject.layer == 2) continue; hit = true; break; }
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
