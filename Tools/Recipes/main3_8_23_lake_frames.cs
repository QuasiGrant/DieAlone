// Main3 8.23 lake frames (Play mode, Main3; Wren's 8.23 round 2 fix list, Vesper 823, Sable 823 4.1). Run by main3_review_capture.sh
// --area lake as "main3_8_23_lake_frames.cs?look=Day_one"; `look` selects that LookPreview row first and asks to be run again so the look
// applies. Never saves; restores the blanket, the camera and the GPU Resident Drawer. Frames at Grant's size (3840 x 1976) in outDir:
// DOCK: the pump and dock from the trail end facing south, and from 90 percent along Camp to pump facing along it (Vesper 823 note 1).
// STEP FROM THE DECK: of eyes round the deck edge at a person standing at the rail (set.deckEye up), the one where hiding her blanket changes the most
//   pixels; from it, binoculars (binoFov degrees vertical, as 8.22) with the blanket on and off, the naked eye with it on, and a
//   crop of the step magnified cropScale times from each binocular frame, with the blanket's pixels that change by pixelStep levels or
//   more (largest RGB channel) between on and off; whether it reads as a blanket is Vesper's call on the frames, so this is a NOTE.
// DOCK EVENTS (Sable 823 4.1): from IntakeFixStand toward IntakeFixPoint, and from SampleStand toward SampleTarget, eye height.
// FOUND (Vesper 823, Pim): the reeds rest, the beach, the water tank and the rowboat from their found eye (Main3AreaSet.Found over the trails, colliders and the
//   drawn trees block, as main3_area_check.cs), at full size; a place not found gets no frame and fails.
string look = "";
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_lake");
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
var pv = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
if (look != "" && pv != null && pv.CurrentLabel != look) { for (int i = 0; i < pv.Count; i++) if (pv.Label(i) == look) { pv.Select(i); return "selected " + look + "; run again"; } return "no look row " + look; }
string lookName = pv != null ? pv.CurrentLabel : "scene"; string tag = lookName.Replace(' ', '_');
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F1(float v) => v.ToString("F1", inv);
string P3(UnityEngine.Vector3 p) => "(" + F1(p.x) + ", " + F1(p.y) + ", " + F1(p.z) + ")";
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
const float eyeH = 1.6f, binoFov = 15f, pixelStep = 24f, aheadM = 10f, fwdShare = 0.9f, trailEndBack = 0.5f;
const int shotW = 3840, shotH = 1976, cropScale = 4;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation; float camFov = cam.fieldOfView;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
var set = Main3AreaSet.Load(); var A = set.Find("lake");
var lake = Root("Lake").transform; var blanket = lake.Find("Boathouse/Dressing/Step/Blanket"); var tower = Root("Camp").transform.Find("Tower");
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
UnityEngine.Color32[] Shoot(string file)
{
    cam.targetTexture = rt; cam.Render(); cam.Render(); cam.targetTexture = null;
    UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null;
    if (file != null) System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), shot.EncodeToPNG()); return shot.GetPixels32();
}
UnityEngine.RectInt ScreenBox(UnityEngine.Bounds b, float pad)
{
    cam.targetTexture = rt; float x0 = float.MaxValue, y0 = float.MaxValue, x1 = float.MinValue, y1 = float.MinValue;
    for (int i = 0; i < 8; i++) { var s = cam.WorldToScreenPoint(V((i & 1) == 0 ? b.min.x - pad : b.max.x + pad, (i & 2) == 0 ? b.min.y - pad : b.max.y + pad, (i & 4) == 0 ? b.min.z - pad : b.max.z + pad)); x0 = UnityEngine.Mathf.Min(x0, s.x); y0 = UnityEngine.Mathf.Min(y0, s.y); x1 = UnityEngine.Mathf.Max(x1, s.x); y1 = UnityEngine.Mathf.Max(y1, s.y); }
    cam.targetTexture = null; int ix0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt(x0), 0, shotW - 1), iy0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt(y0), 0, shotH - 1);
    return new UnityEngine.RectInt(ix0, iy0, UnityEngine.Mathf.Clamp(UnityEngine.Mathf.CeilToInt(x1), 0, shotW - 1) - ix0 + 1, UnityEngine.Mathf.Clamp(UnityEngine.Mathf.CeilToInt(y1), 0, shotH - 1) - iy0 + 1);
}
void Crop(UnityEngine.Color32[] px, UnityEngine.RectInt r, string file)
{
    var t = new UnityEngine.Texture2D(r.width * cropScale, r.height * cropScale, UnityEngine.TextureFormat.RGB24, false); var o = new UnityEngine.Color32[t.width * t.height];
    for (int y = 0; y < t.height; y++) for (int x = 0; x < t.width; x++) o[y * t.width + x] = px[(r.y + y / cropScale) * shotW + r.x + x / cropScale];
    t.SetPixels32(o); t.Apply(); System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), t.EncodeToPNG()); UnityEngine.Object.DestroyImmediate(t);
}
float MeanChange(UnityEngine.Color32[] a, UnityEngine.Color32[] b, UnityEngine.RectInt r) { float s = 0f; int n = 0; for (int y = r.yMin; y < r.yMax; y++) for (int x = r.xMin; x < r.xMax; x++) { var p = a[y * shotW + x]; var q = b[y * shotW + x]; s += UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.r - q.r), UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.g - q.g), UnityEngine.Mathf.Abs(p.b - q.b))); n++; } return n > 0 ? s / n : 0f; }
// every drawn mesh without a collider (LOD0 or plain) gets a temporary exact collider, so lines and found eyes are judged by what is drawn
void DrawnColliders()
{
    var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var r in l[i].renderers) if (r != null) notLod0.Add(r); }
    var area = new UnityEngine.Bounds(V(200f, 0f, 110f), V(160f, 140f, 160f));   // the lake, the Camp to pump trail and the tower
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.name.Contains("Glass") || mr.transform.IsChildOf(pc.transform) || !mr.bounds.Intersects(area)) continue;
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
}
bool ClearTo(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Transform own)   // a hit on own's colliders first reaches it
{
    var d = b - a; float firstOther = float.MaxValue, firstOwn = float.MaxValue;
    foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
    { var ht = h.collider.transform; if (ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2 || h.collider.name.Contains("Glass")) continue; if (own != null && ht.IsChildOf(own)) firstOwn = UnityEngine.Mathf.Min(firstOwn, h.distance); else firstOther = UnityEngine.Mathf.Min(firstOther, h.distance); }
    return firstOther == float.MaxValue || firstOwn < firstOther;
}
try
{
    if (A == null || blanket == null || tower == null) return "no lake area, Lake/Boathouse/Dressing/Step/Blanket or Camp/Tower (run main3_8_23_lake.cs and main3_areas_setup.cs)";
    // ---- DOCK
    {
        var endEye = V(190f, H(190f, 96f + trailEndBack) + eyeH, 96f + trailEndBack); Pose(endEye, V(190f, -4.6f, 88f)); Shoot("Dock_" + tag + "_TrailEnd.png");
        var leg = Root("Trails").transform.Find("Camp to pump"); var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position);
        float total = 0f; for (int i = 1; i < pts.Count; i++) total += UnityEngine.Vector3.Distance(pts[i - 1], pts[i]);
        UnityEngine.Vector3 At(float m) { for (int i = 1; i < pts.Count; i++) { float s = UnityEngine.Vector3.Distance(pts[i - 1], pts[i]); if (m <= s) return UnityEngine.Vector3.Lerp(pts[i - 1], pts[i], m / s); m -= s; } return pts[pts.Count - 1]; }
        var fwd = At(total * fwdShare); var ahead = At(UnityEngine.Mathf.Min(total, total * fwdShare + aheadM));
        Pose(fwd + V(0f, eyeH, 0f), ahead + V(0f, eyeH * 0.5f, 0f)); Shoot("Dock_" + tag + "_FWD90.png");
        sb.Append("NOTE DOCK: Dock_" + tag + "_TrailEnd.png from " + P3(endEye) + " facing south; Dock_" + tag + "_FWD90.png from " + P3(fwd + V(0f, eyeH, 0f)) + ", " + F1(total * fwdShare) + " of " + F1(total) + " m along Camp to pump\n");
    }
    // ---- DOCK EVENTS
    {
        var D = lake.Find("Dock/Layout823");
        foreach (var (stand, target, file) in new[] { ("IntakeFixStand", "IntakeFixPoint", "Intake"), ("SampleStand", "SampleTarget", "SampleBand") })
        {
            var s = D != null ? D.Find(stand) : null; var t = D != null ? D.Find(target) : null; if (s == null || t == null) { Line(false, "DOCK EVENTS: no Lake/Dock/Layout823/" + stand + " or " + target); continue; }
            var e = s.position + V(0f, eyeH, 0f); Pose(e, t.position); Shoot(file + "_" + tag + ".png");
            sb.Append("NOTE DOCK EVENTS: " + file + "_" + tag + ".png from " + P3(e) + " toward " + P3(t.position) + ", " + F1(UnityEngine.Vector3.Distance(e, t.position)) + " m\n");
        }
    }
    DrawnColliders();
    // ---- STEP FROM THE DECK
    {
        // the eye: of every deck eye, the one where hiding her blanket changes its screen box most, judged on pickScale-size renders
        // (rays could not choose: the drawn crowns' leaf cards are solid as colliders, and the deck floor and rails hide the step from most eyes)
        var bb = PlaceKit.MeshBounds(blanket.gameObject); float deckTop = tower.Find("Cab").position.y;
        UnityEngine.Vector3 best = default; float bestChange = float.MinValue; int tried = 0; const int pickScale = 4, edgeEyes = 8; const float railStand = 0.4f;   // eyes round the deck edge, railStand m in from its rails (the 8.21 eye grid stops 0.9 m short of them)
        int pw = shotW / pickScale, ph = shotH / pickScale; var prt = new UnityEngine.RenderTexture(pw, ph, 24, UnityEngine.RenderTextureFormat.ARGB32); var ptex = new UnityEngine.Texture2D(pw, ph, UnityEngine.TextureFormat.RGB24, false);
        UnityEngine.Color32[] Small() { cam.targetTexture = prt; cam.Render(); cam.Render(); cam.targetTexture = null; UnityEngine.RenderTexture.active = prt; ptex.ReadPixels(new UnityEngine.Rect(0, 0, pw, ph), 0, 0); ptex.Apply(); UnityEngine.RenderTexture.active = null; return ptex.GetPixels32(); }
        float SmallChange(UnityEngine.Color32[] a, UnityEngine.Color32[] b, UnityEngine.RectInt r) { float s = 0f; int n = 0; for (int y = r.yMin / pickScale; y <= r.yMax / pickScale && y < ph; y++) for (int x = r.xMin / pickScale; x <= r.xMax / pickScale && x < pw; x++) { var p = a[y * pw + x]; var q = b[y * pw + x]; s += UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.r - q.r), UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.g - q.g), UnityEngine.Mathf.Abs(p.b - q.b))); n++; } return n > 0 ? s / n : 0f; }
        try
        {
            var deckB = new UnityEngine.Bounds(); bool anyDeck = false; foreach (var c in tower.Find("Frame").GetComponentsInChildren<UnityEngine.Collider>()) if (c.name.StartsWith("Deck")) { if (!anyDeck) { deckB = c.bounds; anyDeck = true; } else deckB.Encapsulate(c.bounds); }
            var eyesAt = new System.Collections.Generic.List<UnityEngine.Vector3>(); float ex0 = deckB.min.x + railStand, ex1 = deckB.max.x - railStand, ez0 = deckB.min.z + railStand, ez1 = deckB.max.z - railStand;
            for (float u = 0f; u <= 1.001f; u += 1f / edgeEyes) { eyesAt.Add(V(UnityEngine.Mathf.Lerp(ex0, ex1, u), 0f, ez0)); eyesAt.Add(V(UnityEngine.Mathf.Lerp(ex0, ex1, u), 0f, ez1)); eyesAt.Add(V(ex0, 0f, UnityEngine.Mathf.Lerp(ez0, ez1, u))); eyesAt.Add(V(ex1, 0f, UnityEngine.Mathf.Lerp(ez0, ez1, u))); }
            foreach (var ep in eyesAt)
            {
                var e = V(ep.x, deckTop + set.deckEye, ep.z); cam.fieldOfView = binoFov; Pose(e, bb.center); var bx = ScreenBox(bb, 0f);
                var a = Small(); blanket.gameObject.SetActive(false); var b = Small(); blanket.gameObject.SetActive(true); tried++;
                float ch = SmallChange(a, b, bx); if (ch > bestChange) { bestChange = ch; best = e; }
            }
        }
        finally { cam.targetTexture = null; UnityEngine.Object.DestroyImmediate(prt); UnityEngine.Object.DestroyImmediate(ptex); }
        {
            float bestD = UnityEngine.Vector3.Distance(best, bb.center);
            cam.fieldOfView = binoFov; Pose(best, bb.center); var box = ScreenBox(bb, 0f); var crop = ScreenBox(new UnityEngine.Bounds(V(240f, -3.4f, 56.1f), V(4.6f, 1.6f, 2.6f)), 0f);
            var on = Shoot("Step_" + tag + "_Binoculars_BlanketOn.png"); var on2 = Shoot(null); Crop(on, crop, "Step_" + tag + "_Binoculars_BlanketOn_Crop" + cropScale + "x.png");
            blanket.gameObject.SetActive(false); var off = Shoot("Step_" + tag + "_Binoculars_BlanketOff.png"); Crop(off, crop, "Step_" + tag + "_Binoculars_BlanketOff_Crop" + cropScale + "x.png"); blanket.gameObject.SetActive(true);
            cam.fieldOfView = camFov; Pose(best, bb.center); Shoot("Step_" + tag + "_Eye_BlanketOn.png");
            float change = MeanChange(on, off, box), noise = MeanChange(on, on2, box);
            int changed = 0; for (int y = box.yMin; y < box.yMax; y++) for (int x = box.xMin; x < box.xMax; x++) { var p = on[y * shotW + x]; var q = off[y * shotW + x]; if (UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.r - q.r), UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.g - q.g), UnityEngine.Mathf.Abs(p.b - q.b))) >= pixelStep) changed++; }
            sb.Append("NOTE STEP FROM THE DECK (" + lookName + "): from deck eye " + P3(best) + " (the best of " + tried + " eyes round the deck edge), " + F1(bestD) + " m, binoculars " + F1(binoFov) + " degrees: in her blanket's " + box.width + " x " + box.height + " px box, " + changed + " px change by " + F1(pixelStep) + " levels or more blanket on against off (mean " + F1(change) + ", render noise " + F1(noise) + ") | Step_" + tag + "_*.png\n");
        }
    }
    // ---- FOUND
    {
        var legs = new System.Collections.Generic.List<(string leg, System.Collections.Generic.List<UnityEngine.Vector3> pts)>();
        foreach (UnityEngine.Transform leg in Root("Trails").transform)
        {
            var lp = new System.Collections.Generic.List<UnityEngine.Vector3>(); UnityEngine.Vector3? prev = null;
            foreach (UnityEngine.Transform pt in leg) { var q = pt.position; if (prev.HasValue) { int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(prev.Value, q) / 2f)); for (int i = 1; i <= n; i++) lp.Add(UnityEngine.Vector3.Lerp(prev.Value, q, i / (float)n)); } else lp.Add(q); prev = q; }
            legs.Add((leg.name, lp));
        }
        foreach (var label in new[] { "Reeds rest", "Beach", "Water tank", "Rowboat" })
        {
            Main3AreaSet.Place pl = default; bool have = false; foreach (var p in A.places) if (p.label == label) { pl = p; have = true; }
            if (!have) { Line(false, "FOUND: no lake place " + label); continue; }
            var obj = string.IsNullOrEmpty(pl.objectPath) ? null : Main3AreaSet.At(scene, pl.objectPath);
            var fr = set.Found(pl, legs, (x, z) => H(x, z), (a, b) => ClearTo(a, b, obj));
            if (!fr.found) { Line(false, "FOUND: " + label + " not found from any trail point (" + fr.tried + " tried)"); continue; }
            Pose(fr.eye, fr.aim); string file = "Found_" + label.Replace(' ', '_') + "_" + tag + ".png"; Shoot(file);
            Line(true, "FOUND: " + label + " from " + fr.leg + " at " + P3(fr.eye) + ", " + F1(fr.dist) + " m out, " + F1(fr.angle) + " degrees off the way, " + F1(fr.tall) + " degrees tall | " + file);
        }
    }
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    if (blanket != null) blanket.gameObject.SetActive(true);
    cam.fieldOfView = camFov; cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas;
    UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.23 lake frames, " + lookName + " look; frames in " + outDir + "\n" + sb;
