// Main3 8.24 LOOP-LEG measure (Play mode, Main3; Sable 824 gate 3, BurnLayout_Sound.md 21): the Camp 1 to J trail centre from loop m
// mFrom to mTo, every mStep m, for the silent stretch between forage C and the ruin. Run in batches (main3_8_24_loopleg.sh sets mFrom,
// mTo and part): every full-scene render holds GPU memory until the frame ends, so each job renders only its own metres. Never saves;
// restores the player, the camera and the GPU Resident Drawer. Appends to Docs/Captures/Main3Review_north/LoopLeg.md (part "rest"
// starts the file).
// TOWER (part "tower"): from eyes 1.6 and 2.2 m over the ground at each metre, mesh rays to the centre of every Camp/Tower renderer past
//   the terrain, every collider and the drawn trees' first LODs (temporary exact colliders); and a pixel check aimed at the cab, the
//   scene rendered with and without the tower's renderers at pxW x pxH (0 px is the bar). Per metre: renderers with a clear ray, pixels.
// SURFACE (part "rest"): every collider within edgeBand m outside the tread (treadHalf m either side of the centre line), its gap; the
//   project has no SurfaceSound component (checked: no class of that name in Assets/Scripts); the terrain layer under the centre line.
// GRADE (part "rest"): the ground every 0.5 m, the largest rise and drop; a walk and a sprint along the stretch both ways with the real
//   mover (PlayerController.Step, dt 0.02): frames off the ground and the longest time in the air.
// VOLUMES (part "rest"): every AudioReverbZone, AudioSource (by its max distance) and trigger collider whose reach touches the stretch;
//   the project has no SoundZone component (checked).
float mFrom = 80f, mTo = 132f; string part = "rest";
const float mStep = 1f, treadHalf = 1.2f, edgeBand = 0.5f, gradeStep = 0.5f, dt = 0.02f, sprintCheck = 1f;
const int pxW = 960, pxH = 494;
string outFile = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_north/LoopLeg.md");
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
var leg = Root("Trails").transform.Find("Camp 1 to J"); if (leg == null) return "no Trails/Camp 1 to J";
var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position);
UnityEngine.Vector3 At(float m, out UnityEngine.Vector3 dir) { for (int i = 1; i < pts.Count; i++) { float s = UnityEngine.Vector3.Distance(pts[i - 1], pts[i]); if (m <= s || i == pts.Count - 1) { dir = (pts[i] - pts[i - 1]); dir.y = 0f; dir.Normalize(); var q = UnityEngine.Vector3.Lerp(pts[i - 1], pts[i], UnityEngine.Mathf.Clamp01(m / s)); q.y = H(q.x, q.z); return q; } m -= s; } dir = UnityEngine.Vector3.forward; return pts[pts.Count - 1]; }
var sb = new System.Text.StringBuilder(); var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
var tower = Root("Camp").transform.Find("Tower"); var cab = tower.Find("Cab");
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode;
var cam = UnityEngine.Camera.main; var camParent = cam.transform.parent; var camPos = cam.transform.localPosition; var camRot = cam.transform.localRotation;
var towerRends = tower.GetComponentsInChildren<UnityEngine.Renderer>(); var towerModes = new UnityEngine.Rendering.ShadowCastingMode[towerRends.Length]; for (int i = 0; i < towerRends.Length; i++) towerModes[i] = towerRends[i].shadowCastingMode;
// "off" keeps the tower's shadows (ShadowsOnly), so only the tower itself, not its shadow on the ground, counts as seen
void TowerOff() { for (int i = 0; i < towerRends.Length; i++) if (towerModes[i] == UnityEngine.Rendering.ShadowCastingMode.Off) towerRends[i].forceRenderingOff = true; else towerRends[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly; }
void TowerOn() { for (int i = 0; i < towerRends.Length; i++) if (towerRends[i] != null) { towerRends[i].forceRenderingOff = false; towerRends[i].shadowCastingMode = towerModes[i]; } }
try
{
    if (part == "tower")
    {
        // drawn trees and drawn meshes near the stretch block the rays (temporary exact colliders on first LODs and plain meshes)
        var span = new UnityEngine.Bounds(At(mFrom, out _), UnityEngine.Vector3.zero); for (float m = mFrom; m <= mTo; m += mStep) span.Encapsulate(At(m, out _)); span.Encapsulate(cab.position); span.Expand(V(20f, 80f, 20f));
        var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var r in l[i].renderers) if (r != null) notLod0.Add(r); }
        foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
        {
            if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(pc.transform) || mr.name.Contains("Glass") || !mr.bounds.Intersects(span)) continue;
            var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
        }
        UnityEngine.Physics.SyncTransforms();
        urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
        var rt = new UnityEngine.RenderTexture(pxW, pxH, 24); var shot = new UnityEngine.Texture2D(pxW, pxH, UnityEngine.TextureFormat.RGB24, false); var set = Main3AreaSet.Load();
        cam.transform.SetParent(null, true); cam.targetTexture = rt;
        UnityEngine.Color32[] Shoot(UnityEngine.Vector3 e) { cam.transform.position = e; cam.transform.rotation = UnityEngine.Quaternion.LookRotation(cab.position + V(0f, 1.5f, 0f) - e); cam.Render(); UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, pxW, pxH), 0, 0); shot.Apply(false); UnityEngine.RenderTexture.active = null; return shot.GetPixels32(); }
        try
        {
            for (float m = mFrom; m <= mTo + 1e-3f; m += mStep)
            {
                var g = At(m, out _); var line = new System.Text.StringBuilder("| " + F1(m) + " |");
                foreach (var eh in new[] { 1.6f, 2.2f })
                {
                    var eye = g + V(0f, eh, 0f); int clear = 0;
                    foreach (var r in towerRends)
                    {
                        if (r == null || !r.enabled) continue; var d = r.bounds.center - eye; bool blocked = false;
                        foreach (var h in UnityEngine.Physics.RaycastAll(eye, d.normalized, d.magnitude - 0.05f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) { if (h.collider.transform.IsChildOf(tower) || h.collider.transform.IsChildOf(pc.transform)) continue; blocked = true; break; }
                        if (!blocked) clear++;
                    }
                    Shoot(eye); var on = Shoot(eye); TowerOff(); var off = Shoot(eye); TowerOn();
                    int px = 0; for (int i = 0; i < on.Length; i++) if (System.Math.Abs(on[i].r - off[i].r) > set.pixelTolerance || System.Math.Abs(on[i].g - off[i].g) > set.pixelTolerance || System.Math.Abs(on[i].b - off[i].b) > set.pixelTolerance) px++;
                    line.Append(" " + clear + " | " + px + " |");
                }
                sb.Append(line + "\n");
            }
        }
        finally
        {
            TowerOn(); cam.targetTexture = null; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
        }
        System.IO.File.AppendAllText(outFile, sb.ToString());
        return "TOWER rows m " + F1(mFrom) + " to " + F1(mTo) + " appended to " + outFile + "\n" + sb;
    }
    // ---- part "rest": SURFACE, GRADE, VOLUMES, and the file's head
    var head = new System.Text.StringBuilder();
    head.Append("# LOOP-LEG, Camp 1 to J loop m " + F1(mFrom) + " to " + F1(mTo) + " (main3_8_24_loopleg.cs, " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ")\n\n");
    head.Append("Sable 824 gate 3 and BurnLayout_Sound.md 21. Eyes over the trail centre ground at 1.6 and 2.2 m. Every move is PlayerController.Step.\n\n");
    // SURFACE
    {
        var near = new System.Collections.Generic.Dictionary<UnityEngine.Collider, (float gap, float m)>();
        for (float m = mFrom; m <= mTo + 1e-3f; m += 0.5f)
        {
            var g = At(m, out var dir); var rot = UnityEngine.Quaternion.LookRotation(dir);
            foreach (var c in UnityEngine.Physics.OverlapBox(g + V(0f, 1.3f, 0f), V(treadHalf + edgeBand, 1.2f, 0.25f), rot, ~0, UnityEngine.QueryTriggerInteraction.Collide))
            {
                if (c is UnityEngine.TerrainCollider || c.transform.IsChildOf(pc.transform)) continue;
                var cp = c.ClosestPoint(g + V(0f, 1f, 0f)); var side = UnityEngine.Vector3.Cross(UnityEngine.Vector3.up, dir); float across = UnityEngine.Mathf.Abs(UnityEngine.Vector3.Dot(cp - g, side)); float gap = across - treadHalf;
                if (!near.TryGetValue(c, out var was) || gap < was.gap) near[c] = (gap, m);
            }
        }
        head.Append("## Surface\n\nColliders within " + F1(edgeBand) + " m outside the tread (" + F1(treadHalf) + " m either side of the centre line); a negative gap is on the tread. No SurfaceSound component exists in the project.\n\n");
        if (near.Count == 0) head.Append("None.\n\n"); else { head.Append("| Collider | Layer | Trigger | Gap m | at loop m |\n|---|---|---|---|---|\n"); foreach (var kv in near) head.Append("| " + WalkIns.PathOf(kv.Key.transform) + " | " + kv.Key.gameObject.layer + " | " + kv.Key.isTrigger + " | " + F(kv.Value.gap) + " | " + F1(kv.Value.m) + " |\n"); head.Append("\n"); }
        // the terrain layer under the centre line
        var td = ter.terrainData; var to = ter.transform.position; var counts = new System.Collections.Generic.Dictionary<string, int>();
        for (float m = mFrom; m <= mTo + 1e-3f; m += 0.5f)
        {
            var g = At(m, out _); int ax = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt((g.x - to.x) / td.size.x * td.alphamapWidth), 0, td.alphamapWidth - 1), az = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt((g.z - to.z) / td.size.z * td.alphamapHeight), 0, td.alphamapHeight - 1);
            var a = td.GetAlphamaps(ax, az, 1, 1); int bestL = 0; for (int l = 1; l < a.GetLength(2); l++) if (a[0, 0, l] > a[0, 0, bestL]) bestL = l;
            var name = td.terrainLayers[bestL] != null ? td.terrainLayers[bestL].name : "layer " + bestL; counts[name] = counts.TryGetValue(name, out var n) ? n + 1 : 1;
        }
        head.Append("Terrain layer under the centre line (strongest weight, every 0.5 m): "); foreach (var kv in counts) head.Append(kv.Key + " " + kv.Value + " samples; "); head.Append("\n\n");
    }
    // GRADE
    {
        float prevY = float.NaN, rise = 0f, drop = 0f, riseAt = 0f, dropAt = 0f;
        for (float m = mFrom; m <= mTo + 1e-3f; m += gradeStep) { var g = At(m, out _); if (!float.IsNaN(prevY)) { float dy = g.y - prevY; if (dy > rise) { rise = dy; riseAt = m; } if (-dy > drop) { drop = -dy; dropAt = m; } } prevY = g.y; }
        head.Append("## Grade\n\nGround every " + F1(gradeStep) + " m: largest rise " + F(rise) + " m (at m " + F1(riseAt) + "), largest drop " + F(drop) + " m (at m " + F1(dropAt) + ").\n\n| Walk | Frames off the ground | Longest in the air s | Arrived |\n|---|---|---|---|\n");
        foreach (var (sprint, back) in new[] { (false, false), (false, true), (true, false), (true, true) })
        {
            float m0 = back ? mTo : mFrom, m1 = back ? mFrom : mTo; var a0 = At(m0, out _);
            cc.enabled = false; pc.transform.position = a0 + V(0f, 0.1f, 0f); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
            int off = 0, run = 0, longest = 0; float m = m0; bool arrived = false;
            for (int k = 0; k < 20000; k++)
            {
                var target = At(m, out _); var p = pc.transform.position; var d = V(target.x - p.x, 0f, target.z - p.z);
                if (d.magnitude < 0.6f) { if ((back && m <= m1) || (!back && m >= m1)) { arrived = true; break; } m += back ? -sprintCheck : sprintCheck; m = back ? UnityEngine.Mathf.Max(m, m1) : UnityEngine.Mathf.Min(m, m1); continue; }
                pc.Step(d.normalized, false, sprint, dt); if (!cc.isGrounded) { off++; run++; longest = UnityEngine.Mathf.Max(longest, run); } else run = 0;
            }
            head.Append("| " + (sprint ? "sprint" : "walk") + (back ? ", J-ward to forage C" : ", forage C to J-ward") + " | " + off + " | " + F(longest * dt) + " | " + arrived + " |\n");
        }
        head.Append("\n");
    }
    // VOLUMES
    {
        var span = new UnityEngine.Bounds(At(mFrom, out _), UnityEngine.Vector3.zero); for (float m = mFrom; m <= mTo; m += mStep) span.Encapsulate(At(m, out _)); span.Expand(V(2f * treadHalf, 6f, 2f * treadHalf));
        head.Append("## Volumes\n\nNo SoundZone component exists in the project. Reverb zones, audio sources (by max distance) and trigger colliders reaching the stretch:\n\n"); int n = 0;
        foreach (var z in UnityEngine.Object.FindObjectsByType<UnityEngine.AudioReverbZone>(UnityEngine.FindObjectsSortMode.None)) if (span.SqrDistance(z.transform.position) <= z.maxDistance * z.maxDistance) { head.Append("- reverb zone " + WalkIns.PathOf(z.transform) + ", max " + F1(z.maxDistance) + " m\n"); n++; }
        foreach (var s in UnityEngine.Object.FindObjectsByType<UnityEngine.AudioSource>(UnityEngine.FindObjectsSortMode.None)) if (s.spatialBlend < 0.5f || span.SqrDistance(s.transform.position) <= s.maxDistance * s.maxDistance) { head.Append("- audio source " + WalkIns.PathOf(s.transform) + ", clip " + (s.clip != null ? s.clip.name : "none") + ", " + (s.spatialBlend < 0.5f ? "2D (heard everywhere)" : "max " + F1(s.maxDistance) + " m") + "\n"); n++; }
        foreach (var c in UnityEngine.Object.FindObjectsByType<UnityEngine.Collider>(UnityEngine.FindObjectsSortMode.None)) if (c.isTrigger && c.enabled && c.bounds.Intersects(span)) { head.Append("- trigger " + WalkIns.PathOf(c.transform) + "\n"); n++; }
        if (n == 0) head.Append("None.\n");
        head.Append("\n## Tower view\n\nPer metre, eye 1.6 m then 2.2 m: Camp/Tower renderers with a clear mesh ray (of " + towerRends.Length + "), and tower pixels aimed at the cab (" + pxW + " x " + pxH + ", 0 is the bar).\n\n| m | rays 1.6 | px 1.6 | rays 2.2 | px 2.2 |\n|---|---|---|---|---|\n");
    }
    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFile)); System.IO.File.WriteAllText(outFile, head.ToString());
    return "REST written to " + outFile + "\n" + head;
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    cam.targetTexture = null; cam.transform.SetParent(camParent, false); cam.transform.localPosition = camPos; cam.transform.localRotation = camRot; urp.gpuResidentDrawerMode = grdWas;
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
