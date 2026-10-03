// Main3 8.24a deck frames (Play mode, Main3; PLAN 8.24a, Grant's walk 2026-10-03: the big-tent camp could not be seen from the tower). Run by
// main3_review_capture.sh --area north as "main3_8_24a_deck_frames.cs?look=Day_one"; `look` selects that LookPreview row first and asks to be
// run again so the look applies. Never saves; restores the player, the camera and the GPU Resident Drawer. Frames at Grant's size (shotW x
// shotH) in outDir: from the first standing deck eye (the grid and the rail eyes, as the area check's DECK) whose line to Camp 1's tent passes
// every collider and drawn mesh (temporary exact colliders; the tent's own meshes do not count), the naked eye and binoculars (binoFov).
// EYE: some standing deck eye sees the tent.
string look = "";
string outDir = System.IO.Path.GetFullPath("Docs/Captures/Main3Review_north");
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
var line = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_1/Dressing/CS_Tent_Large_Modern_Preset_1") : null; var cab = Root("Camp") != null ? Root("Camp").transform.Find("Tower/Cab") : null;
if (line == null || cab == null) { pc.enabled = pcWas; return "no Camp_1 tent or the tower cab"; }
System.IO.Directory.CreateDirectory(outDir);
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode; urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
try
{
    var pb = PlaceKit.MeshBounds(line.gameObject); var at = pb.center; var aims = new System.Collections.Generic.List<UnityEngine.Vector3> { at };   // the tent's centre
    var set = Main3AreaSet.Load(); var toLine = V(at.x - cab.position.x, 0f, at.z - cab.position.z).normalized; UnityEngine.Vector3 eye = V(cab.position.x, cab.position.y + eyeH, cab.position.z); bool clearEye = false;
    var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid) eyes.Add(V(cab.position.x + gx, cab.position.y + set.deckEye, cab.position.z + gz));   // standing eyes only (Wren 2026-10-03, B)
    { var rails = cab.parent.Find("DeckRails"); UnityEngine.Bounds RB(string n) { var r = rails != null ? rails.Find(n) : null; var c = r != null ? r.GetComponent<UnityEngine.Collider>() : null; return c != null ? c.bounds : new UnityEngine.Bounds(); } var rw = RB("RailW"); var re = RB("RailE"); var rs = RB("RailS"); var rn = RB("RailN"); float x0 = rw.max.x + set.railEyeInset, x1 = re.min.x - set.railEyeInset, z0 = rs.max.z + set.railEyeInset, z1 = rn.min.z - set.railEyeInset, ry = cab.position.y + set.deckEye; if (rw.size != UnityEngine.Vector3.zero) { for (float x = x0; x <= x1 + 1e-3f; x += set.railEyeStep) { eyes.Add(V(x, ry, z0)); eyes.Add(V(x, ry, z1)); } for (float z = z0 + set.railEyeStep; z < z1 - 1e-3f; z += set.railEyeStep) { eyes.Add(V(x0, ry, z)); eyes.Add(V(x1, ry, z)); } } }   // the rail eyes, as the area check (Wren 2026-10-03)
    var cabAt = cab.position; eyes = System.Linq.Enumerable.ToList(System.Linq.Enumerable.OrderByDescending(eyes, q => (q.x - cabAt.x) * toLine.x + (q.z - cabAt.z) * toLine.z));
    var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
    var mrs = UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None);
    foreach (var e in eyes) foreach (var a in aims)
    {
        if (clearEye) break; var d = a - e; var ray = new UnityEngine.Ray(e, d.normalized);
        foreach (var mr in mrs) { if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.transform.IsChildOf(line) || mr.transform.IsChildOf(pc.transform)) continue; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null || !mr.bounds.IntersectRay(ray, out float dist) || dist > d.magnitude) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
        UnityEngine.Physics.SyncTransforms(); bool blocked = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(ray, d.magnitude - 0.1f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) if (!h.collider.transform.IsChildOf(line) && !h.collider.transform.IsChildOf(pc.transform) && h.collider.gameObject.layer != 2) { blocked = true; break; }
        foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();
        if (!blocked) { eye = e; at = a; clearEye = true; break; }
    }
    Line(clearEye, "EYE: " + (clearEye ? "the deck eye (" + eye.x.ToString("F1") + ", " + eye.y.ToString("F1") + ", " + eye.z.ToString("F1") + ") sees the tent at (" + at.x.ToString("F1") + ", " + at.y.ToString("F1") + ", " + at.z.ToString("F1") + ")" : "no standing deck eye sees the tent past every mesh"));
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
    foreach (var bino in new[] { false, true }) { cam.fieldOfView = bino ? binoFov : camFov; Pose(eye, at); Shoot("DeckTent_" + tag + (bino ? "_Binoculars" : "_Eye") + ".jpg"); }
    sb.Append("frames DeckTent_" + tag + "_Eye.jpg and _Binoculars.jpg\n");
}
finally
{
    cam.fieldOfView = camFov; cam.transform.localRotation = camRot; cam.targetTexture = null; urp.gpuResidentDrawerMode = grdWas; rt.Release(); UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(shot);
    cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false;
}
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.24a deck frames, " + lookName + " look; frames in " + outDir + "\n" + sb;
