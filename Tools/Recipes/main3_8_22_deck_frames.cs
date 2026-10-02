// Main3 8.22 deck frames (Play mode, Main3; FrontLayout.md 2.7, the 8.22 gate round 2: Vesper 9 and 11, Pim 1, Wren 5 and 6). Run by
// main3_review_capture.sh --area front once per look ("main3_8_22_deck_frames.cs?look=Day_one" and "?look=Night"); `look` selects that
// LookPreview row first and asks to be run again so the look applies. Never saves; restores the door, the camera and the GPU Resident
// Drawer. Frames at Grant's size (3840 x 1976) in outDir, per look:
// LECTERN: the office west door opening is seen from the reader's eye at the lectern (cab east side, Wren's call 2026-10-02) past every
//   drawn mesh (temporary exact colliders, glass seen through).
// DOOR PAIR: from that eye, naked eye and binoculars (binoFov degrees vertical, TowerCheck.md 7.2), the door open and shut, and a crop of
//   the opening and porch lamp magnified cropScale times from each binocular frame.
// LAMP: shut, the lamp's glow and light are off (DoorLamp); open, the brightest coreShare of the lamp's screen box in binoculars averages
//   nearWhite or brighter (luminance 0 to 255); open against shut, the lamp's pixels change by lampChange levels on average (the largest RGB
//   channel) over the render noise between two shut frames.
// VERGE TREE: one binocular frame from the same eye, centred on the verge tree.
string look = "";
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_front");
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
var pv = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
if (look != "" && pv != null && pv.CurrentLabel != look) { for (int i = 0; i < pv.Count; i++) if (pv.Label(i) == look) { pv.Select(i); return "selected " + look + "; run again"; } return "no look row " + look; }
string lookName = pv != null ? pv.CurrentLabel : "scene"; string tag = lookName.Replace(' ', '_');
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F1(float v) => v.ToString("F1", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
const float eyeH = 1.6f, binoFov = 15f, nearWhite = 200f, coreShare = 0.1f, lampChange = 32f, cropPad = 0.5f;   // coreShare: the lamp bar fills only part of its own screen box, so its face is the brightest share of that box
const int shotW = 3840, shotH = 1976, cropScale = 4;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation; float camFov = cam.fieldOfView;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var fz = Root("FrontZone").transform; var office = fz.Find("Office"); var doorT = office.Find("WestDoor"); var door = doorT != null ? doorT.GetComponent<Door>() : null;
var lamp = UnityEngine.Object.FindFirstObjectByType<DoorLamp>(UnityEngine.FindObjectsInactive.Include);
var glowT = office.Find("Porch/DoorLamp/Lamp/Glow"); var lightC = office.Find("Porch/DoorLamp/Lamp/DoorLampLight");
var set = Main3AreaSet.Load(); var campA = set.Find("camp"); UnityEngine.Vector3 stand = UnityEngine.Vector3.zero; foreach (var ia in campA.interactions) if (ia.label == "lectern") stand = ia.approach;
var eye = stand + UnityEngine.Vector3.up * eyeH; var opening = V(344.0f, 4.3f, 199.0f);
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
UnityEngine.RectInt Rect(params UnityEngine.Vector3[] pts)
{
    cam.targetTexture = rt; float x0 = float.MaxValue, y0 = float.MaxValue, x1 = float.MinValue, y1 = float.MinValue;
    foreach (var p in pts) { var s = cam.WorldToScreenPoint(p); x0 = UnityEngine.Mathf.Min(x0, s.x); y0 = UnityEngine.Mathf.Min(y0, s.y); x1 = UnityEngine.Mathf.Max(x1, s.x); y1 = UnityEngine.Mathf.Max(y1, s.y); }
    cam.targetTexture = null; int ix0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt(x0), 0, shotW - 1), iy0 = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.FloorToInt(y0), 0, shotH - 1);
    return new UnityEngine.RectInt(ix0, iy0, UnityEngine.Mathf.Clamp(UnityEngine.Mathf.CeilToInt(x1), 0, shotW - 1) - ix0 + 1, UnityEngine.Mathf.Clamp(UnityEngine.Mathf.CeilToInt(y1), 0, shotH - 1) - iy0 + 1);
}
void Crop(UnityEngine.Color32[] px, UnityEngine.RectInt r, string file)
{
    var t = new UnityEngine.Texture2D(r.width * cropScale, r.height * cropScale, UnityEngine.TextureFormat.RGB24, false); var o = new UnityEngine.Color32[t.width * t.height];
    for (int y = 0; y < t.height; y++) for (int x = 0; x < t.width; x++) o[y * t.width + x] = px[(r.y + y / cropScale) * shotW + r.x + x / cropScale];
    t.SetPixels32(o); t.Apply(); System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), t.EncodeToPNG()); UnityEngine.Object.DestroyImmediate(t);
}
float Lum(UnityEngine.Color32 c) => 0.299f * c.r + 0.587f * c.g + 0.114f * c.b;
float MeanChange(UnityEngine.Color32[] a, UnityEngine.Color32[] b, UnityEngine.RectInt r) { float s = 0f; int n = 0; for (int y = r.yMin; y < r.yMax; y++) for (int x = r.xMin; x < r.xMax; x++) { var p = a[y * shotW + x]; var q = b[y * shotW + x]; s += UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.r - q.r), UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(p.g - q.g), UnityEngine.Mathf.Abs(p.b - q.b))); n++; } return n > 0 ? s / n : 0f; }
void SetDoor(bool open) { door.HoldShut(!open, true); if (lamp != null) lamp.Sync(); }
try
{
    if (door == null || glowT == null) return "no FrontZone/Office/WestDoor or Porch/DoorLamp (run main3_8_22_front.cs)";
    // ---- LECTERN: every drawn mesh on the line, glass seen through
    {
        var seg = new UnityEngine.Ray(eye, (opening - eye).normalized); float len = UnityEngine.Vector3.Distance(eye, opening);
        var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var r in l[i].renderers) if (r != null) notLod0.Add(r); }
        foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
        {
            if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.name.Contains("Glass") || mr.transform.IsChildOf(pc.transform)) continue;
            var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; if (!mr.bounds.IntersectRay(seg, out float d) || d > len) continue;
            var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
        }
        UnityEngine.Physics.SyncTransforms(); string block = null; float first = float.MaxValue;
        foreach (var h in UnityEngine.Physics.RaycastAll(eye, seg.direction, len - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        { var ht = h.collider.transform; if (ht.IsChildOf(pc.transform) || ht.gameObject.layer == 2 || ht.IsChildOf(doorT) || h.collider.name.Contains("Glass")) continue; if (h.distance < first) { first = h.distance; block = WalkIns.PathOf(ht) + " at " + F1(h.distance) + " m"; } }
        foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        Line(block == null, "LECTERN: from the reader's eye at the lectern (" + F1(eye.x) + ", " + F1(eye.y) + ", " + F1(eye.z) + ") the office west door opening, " + F1(len) + " m, every drawn mesh: " + (block ?? "clear"));
    }
    // ---- DOOR PAIR and LAMP
    var lampB = glowT.GetComponent<UnityEngine.Renderer>().bounds;
    UnityEngine.Color32[] bOpen = null, bShut = null, bShut2 = null; UnityEngine.RectInt lampR = default, cropR = default;
    foreach (var open in new[] { true, false })
    {
        SetDoor(open); string st = open ? "Open" : "Shut";
        foreach (var bino in new[] { false, true })
        {
            cam.fieldOfView = bino ? binoFov : camFov; Pose(eye, opening);
            var px = Shoot("DeckDoor_" + tag + "_" + st + (bino ? "_Binoculars" : "_Eye") + ".png");
            if (!bino) continue;
            lampR = Rect(lampB.min, lampB.max, V(lampB.min.x, lampB.min.y, lampB.max.z), V(lampB.max.x, lampB.max.y, lampB.min.z));
            var cr = Rect(V(344f, 3.05f - cropPad, 198.4f - cropPad), V(344f, 5.6f + cropPad, 199.6f + cropPad)); cropR = cr;
            Crop(px, cr, "DeckDoor_" + tag + "_" + st + "_Crop" + cropScale + "x.png");
            if (open) bOpen = px; else { bShut = px; bShut2 = Shoot(null); }
        }
    }
    bool shutOff = !glowT.gameObject.activeInHierarchy && (lightC == null || !lightC.gameObject.activeInHierarchy);
    SetDoor(true); bool openOn = glowT.gameObject.activeInHierarchy && (lightC == null || lightC.gameObject.activeInHierarchy);
    var lums = new System.Collections.Generic.List<float>(); for (int y = lampR.yMin; y < lampR.yMax; y++) for (int x = lampR.xMin; x < lampR.xMax; x++) lums.Add(Lum(bOpen[y * shotW + x])); lums.Sort((p, q) => q.CompareTo(p)); int total = lums.Count, coreN = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(total * coreShare)); float core = 0f; for (int i = 0; i < coreN && i < total; i++) core += lums[i]; core /= coreN; int white = 0; foreach (var l in lums) if (l >= nearWhite) white++;
    float whiteShare = total > 0 ? white / (float)total : 0f; float change = MeanChange(bOpen, bShut, lampR), noise = MeanChange(bShut2, bShut, lampR);
    Line(shutOff && openOn && core >= nearWhite && change - noise >= lampChange, "LAMP (" + lookName + "): shut, glow and light off " + shutOff + "; open, on " + openOn + "; in binoculars the brightest " + (coreShare * 100f).ToString("F0", inv) + " percent of the lamp's " + total + " px average luminance " + F1(core) + " (bar " + F1(nearWhite) + "; " + (whiteShare * 100f).ToString("F0", inv) + " percent of them at the bar or more); open against shut " + F1(change) + " levels on average, noise " + F1(noise) + " (bar " + F1(lampChange) + " over the noise) | DeckDoor_" + tag + "_Open/Shut_Eye, _Binoculars, _Crop" + cropScale + "x.png");
    // ---- VERGE TREE
    var verge = fz.Find("VergeTree/DeadGiant"); var vb = PlaceKit.MeshBounds(verge.gameObject);
    cam.fieldOfView = binoFov; Pose(eye, vb.center); Shoot("Verge_" + tag + "_Binoculars.png"); cam.fieldOfView = camFov;
    sb.Append("NOTE VERGE TREE: binocular frame centred on it (" + F1(vb.center.x) + ", " + F1(vb.center.y) + ", " + F1(vb.center.z) + ") | Verge_" + tag + "_Binoculars.png\n");
}
finally
{
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    if (door != null) { door.HoldShut(false, true); if (lamp != null) lamp.Sync(); }
    cam.fieldOfView = camFov; cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas;
    UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.22 deck frames, " + lookName + " look; frames in " + outDir + "\n" + sb;
