// Main3 8.25 frames (Play mode, Main3; the 8.25 gate round 2, Wren 6: Vesper's night deck frame with the Camp 2 lamp lit). Run by
// main3_review_capture.sh --area camp2 once per look ("main3_8_25_camp2_frames.cs?look=Day_one" and "?look=Night"); `look` selects that
// LookPreview row first and asks to be run again so the look applies. Never saves; restores the player, the camera and the GPU Resident
// Drawer. Frames at Grant's size (shotW x shotH) in outDir, per look: from the deck eye on the cab side nearest Camp 2 (deck floor plus
// eyeH, deckIn m in from the cab's centre toward the lamp), aimed at the lamp's core, the naked eye and binoculars (binoFov degrees).
// LAMP: at night the lamp's practical light is on. CORE (gate round 2, Wren 3): at night the core's brightest pixel in the naked-eye frame
// stands coreAbove grey or more over the frame's mean.
string look = "";
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_camp2");
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
var pv = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
if (look != "" && pv != null && pv.CurrentLabel != look) { for (int i = 0; i < pv.Count; i++) if (pv.Label(i) == look) { pv.Select(i); return "selected " + look + "; run again"; } return "no look row " + look; }
string lookName = pv != null ? pv.CurrentLabel : "scene"; string tag = lookName.Replace(' ', '_');
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
const float eyeH = 1.6f, binoFov = 15f, deckIn = 3f, coreAbove = 40f; const int shotW = 3840, shotH = 1976, corePad = 2;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation; float camFov = cam.fieldOfView;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var lamp = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_2/Layout825a/Top/Lamp") : null; var cab = Root("Camp") != null ? Root("Camp").transform.Find("Tower/Cab") : null;
if (lamp == null || cab == null) { pc.enabled = pcWas; return "no Camp_2 Lamp or the tower cab (run main3_8_25a_knob.cs)"; }
System.IO.Directory.CreateDirectory(outDir);
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode; urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
try
{
    var core = lamp.Find("LampCore"); var at = core != null ? core.position : lamp.position + V(0f, 1.3f, 0f);
    // the eye: of the deck's eyes (Main3AreaSet deckHalf, deckGrid, deckEye over the cab floor), the one nearest the lamp's side whose line
    // to the core passes every collider and drawn mesh (temporary exact colliders); the deck test passes on a share of eyes, so a frame
    // from a blocked eye showed a tree's dark crown (gate round 2 rerun)
    var set = Main3AreaSet.Load(); var toLamp = V(at.x - cab.position.x, 0f, at.z - cab.position.z).normalized; UnityEngine.Vector3 eye = V(cab.position.x, cab.position.y + eyeH, cab.position.z) + toLamp * deckIn; bool clearEye = false;
    var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid) eyes.Add(V(cab.position.x + gx, cab.position.y + set.deckEye, cab.position.z + gz));
    var cabAt = cab.position; var lampDir = toLamp; eyes = System.Linq.Enumerable.ToList(System.Linq.Enumerable.OrderByDescending(eyes, q => (q.x - cabAt.x) * lampDir.x + (q.z - cabAt.z) * lampDir.z));
    var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
    var mrs = UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None);
    foreach (var e in eyes)
    {
        var d = at - e; var ray = new UnityEngine.Ray(e, d.normalized);
        foreach (var mr in mrs) { if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(lamp) || mr.transform.IsChildOf(pc.transform)) continue; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null || !mr.bounds.IntersectRay(ray, out float dist) || dist > d.magnitude) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
        UnityEngine.Physics.SyncTransforms(); bool blocked = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(ray, d.magnitude - 0.1f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!h.collider.transform.IsChildOf(lamp) && !h.collider.transform.IsChildOf(pc.transform) && h.collider.gameObject.layer != 2) { blocked = true; break; }
        foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        if (!blocked) { eye = e; clearEye = true; break; }
    }
    if (!clearEye) Line(false, "EYE: no deck eye sees the lamp's core past every mesh");
    void Pose(UnityEngine.Vector3 e, UnityEngine.Vector3 aim)
    {
        var dir = aim - e; var flat = V(dir.x, 0f, dir.z); cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = e - pc.transform.rotation * camLocal;
        cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
    }
    void Shoot(string file)
    {
        cam.targetTexture = rt; cam.Render(); cam.Render(); cam.targetTexture = null;
        UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(); UnityEngine.RenderTexture.active = null;
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, file), shot.EncodeToJPG(92));
    }
    float coreOver = float.NaN, frameMean = 0f;
    foreach (var bino in new[] { false, true })
    {
        cam.fieldOfView = bino ? binoFov : camFov; Pose(eye, at); Shoot("DeckLamp_" + tag + (bino ? "_Binoculars" : "_Eye") + ".jpg");
        if (bino || core == null) continue;
        // the core's pixels against the frame: the brightest grey (0 to 255) in the core's screen box (corePad px round it) over the frame's mean grey
        var px = shot.GetPixels32(); double sum = 0; foreach (var c in px) sum += (c.r + c.g + c.b) / 3.0; frameMean = (float)(sum / px.Length);
        cam.targetTexture = rt; var cb = core.GetComponent<UnityEngine.Renderer>().bounds; float x0 = float.MaxValue, y0 = float.MaxValue, x1 = float.MinValue, y1 = float.MinValue;
        for (int k = 0; k < 8; k++) { var w = V((k & 1) == 0 ? cb.min.x : cb.max.x, (k & 2) == 0 ? cb.min.y : cb.max.y, (k & 4) == 0 ? cb.min.z : cb.max.z); var sp = cam.WorldToScreenPoint(w); x0 = UnityEngine.Mathf.Min(x0, sp.x); y0 = UnityEngine.Mathf.Min(y0, sp.y); x1 = UnityEngine.Mathf.Max(x1, sp.x); y1 = UnityEngine.Mathf.Max(y1, sp.y); }
        cam.targetTexture = null; float best = 0f;
        for (int y = UnityEngine.Mathf.Max(0, (int)y0 - corePad); y <= UnityEngine.Mathf.Min(shotH - 1, (int)y1 + corePad); y++) for (int x = UnityEngine.Mathf.Max(0, (int)x0 - corePad); x <= UnityEngine.Mathf.Min(shotW - 1, (int)x1 + corePad); x++) { var c = px[y * shotW + x]; best = UnityEngine.Mathf.Max(best, (c.r + c.g + c.b) / 3f); }
        coreOver = best - frameMean;
    }
    if (lookName.ToLowerInvariant().Contains("night")) Line(!float.IsNaN(coreOver) && coreOver >= coreAbove, "CORE (" + lookName + "): the lamp core's brightest pixel in DeckLamp_" + tag + "_Eye is " + (float.IsNaN(coreOver) ? "not measured" : coreOver.ToString("F0") + " grey over the frame mean " + frameMean.ToString("F0")) + " (at least " + coreAbove.ToString("F0") + ")");
    var light = lamp.GetComponentInChildren<UnityEngine.Light>(true); bool night = lookName.ToLowerInvariant().Contains("night");
    if (night) Line(light != null && light.enabled && light.gameObject.activeInHierarchy && light.intensity > 0f, "LAMP (" + lookName + "): the Camp 2 lamp's light is " + (light == null ? "missing" : (light.enabled && light.gameObject.activeInHierarchy && light.intensity > 0f ? "on, intensity " + light.intensity.ToString("F2") : "off")));
    sb.Append("frames DeckLamp_" + tag + "_Eye.jpg and _Binoculars.jpg from (" + eye.x.ToString("F1") + ", " + eye.y.ToString("F1") + ", " + eye.z.ToString("F1") + ")\n");
}
finally
{
    cam.fieldOfView = camFov; cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.25 Camp 2 frames, " + lookName + " look; frames in " + outDir + "\n" + sb;
