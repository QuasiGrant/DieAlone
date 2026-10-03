// Main3 8.26 frames (Play mode, Main3; the 8.26 gate round 2, Wren 2026-10-03 item 10: a deck binocular frame of the Snag line). Run by
// main3_review_capture.sh --area camp3 as "main3_8_26_camp3_frames.cs?look=Day_one"; `look` selects that LookPreview row first and asks to
// be run again so the look applies. Never saves; restores the player, the camera and the GPU Resident Drawer. Frames at Grant's size
// (shotW x shotH) in outDir: from the deck eye on the cab side nearest the Snag line whose line to a piece (2, 1, 3, 4 in turn) passes every collider
// and drawn mesh (temporary exact colliders; the line's own meshes do not count), the naked eye and binoculars (binoFov degrees).
// EYE: some deck eye sees a Snag line piece.
string look = "";
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_camp3");
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
var pv = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
if (look != "" && pv != null && pv.CurrentLabel != look) { for (int i = 0; i < pv.Count; i++) if (pv.Label(i) == look) { pv.Select(i); return "selected " + look + "; run again"; } return "no look row " + look; }
string lookName = pv != null ? pv.CurrentLabel : "scene"; string tag = lookName.Replace(' ', '_');
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
const float eyeH = 1.6f, binoFov = 15f; const int shotW = 3840, shotH = 1976;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation; float camFov = cam.fieldOfView;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var line = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_3/Layout826/SnagLine") : null; var cab = Root("Camp") != null ? Root("Camp").transform.Find("Tower/Cab") : null;
if (line == null || cab == null) { pc.enabled = pcWas; return "no Camp_3 SnagLine or the tower cab (run main3_8_26_camp3.cs)"; }
System.IO.Directory.CreateDirectory(outDir);
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode; urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
try
{
    // the pieces' centre orders the eyes; the aim is the first piece a deck eye sees
    var pb = new UnityEngine.Bounds(); bool any = false; for (int i = 1; i <= 4; i++) { var p = line.Find("Piece" + i); if (p == null) continue; var b = PlaceKit.MeshBounds(p.gameObject); if (!any) { pb = b; any = true; } else pb.Encapsulate(b); }
    if (!any) { Line(false, "EYE: no Snag line pieces"); return "FAILS 1\n" + sb; }
    var at = pb.center; var aims = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (var i in new[] { 2, 1, 3, 4 }) { var p = line.Find("Piece" + i); if (p != null) aims.Add(PlaceKit.MeshBounds(p.gameObject).center); }   // aimed at the first piece an eye sees, the deck's hard pieces first
    var set = Main3AreaSet.Load(); var toLine = V(at.x - cab.position.x, 0f, at.z - cab.position.z).normalized; UnityEngine.Vector3 eye = V(cab.position.x, cab.position.y + eyeH, cab.position.z); bool clearEye = false;
    var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid) foreach (var hgt in new[] { set.deckEye, set.deckJump }) eyes.Add(V(cab.position.x + gx, cab.position.y + hgt, cab.position.z + gz));   // eye and jump height, as the area check
    var cabAt = cab.position; eyes = System.Linq.Enumerable.ToList(System.Linq.Enumerable.OrderByDescending(eyes, q => (q.x - cabAt.x) * toLine.x + (q.z - cabAt.z) * toLine.z));
    var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
    var mrs = UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None);
    foreach (var e in eyes) foreach (var a in aims)
    {
        if (clearEye) break; var d = a - e; var ray = new UnityEngine.Ray(e, d.normalized);
        foreach (var mr in mrs) { if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(line) || mr.transform.IsChildOf(pc.transform)) continue; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null || !mr.bounds.IntersectRay(ray, out float dist) || dist > d.magnitude) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
        UnityEngine.Physics.SyncTransforms(); bool blocked = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(ray, d.magnitude - 0.1f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!h.collider.transform.IsChildOf(line) && !h.collider.transform.IsChildOf(pc.transform) && h.collider.gameObject.layer != 2) { blocked = true; break; }
        foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        if (!blocked) { eye = e; at = a; clearEye = true; break; }
    }
    Line(clearEye, "EYE: " + (clearEye ? "the deck eye (" + eye.x.ToString("F1") + ", " + eye.y.ToString("F1") + ", " + eye.z.ToString("F1") + ") sees a Snag line piece at (" + at.x.ToString("F1") + ", " + at.y.ToString("F1") + ", " + at.z.ToString("F1") + ")" : "no deck eye sees any Snag line piece past every mesh"));
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
    foreach (var bino in new[] { false, true }) { cam.fieldOfView = bino ? binoFov : camFov; Pose(eye, at); Shoot("DeckSnagLine_" + tag + (bino ? "_Binoculars" : "_Eye") + ".jpg"); }
    sb.Append("frames DeckSnagLine_" + tag + "_Eye.jpg and _Binoculars.jpg\n");
}
finally
{
    cam.fieldOfView = camFov; cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.26 Camp 3 frames, " + lookName + " look; frames in " + outDir + "\n" + sb;
