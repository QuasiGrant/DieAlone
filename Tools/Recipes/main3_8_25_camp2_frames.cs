// Main3 8.25 frames (Play mode, Main3; the 8.25 gate round 2, Wren 6: Vesper's night deck frame with the Camp 2 lamp lit). Run by
// main3_review_capture.sh --area camp2 once per look ("main3_8_25_camp2_frames.cs?look=Day_one" and "?look=Night"); `look` selects that
// LookPreview row first and asks to be run again so the look applies. Never saves; restores the player, the camera and the GPU Resident
// Drawer. Frames at Grant's size (shotW x shotH) in outDir, per look: from the deck eye on the cab side nearest Camp 2 (deck floor plus
// eyeH, deckIn m in from the cab's centre toward the lamp), aimed at the lamp's core, the naked eye and binoculars (binoFov degrees).
// LAMP: at night the lamp's practical light is on.
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
const float eyeH = 1.6f, binoFov = 15f, deckIn = 3f; const int shotW = 3840, shotH = 1976;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition; var camRot = cam.transform.localRotation; float camFov = cam.fieldOfView;
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var lamp = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_2/StackTop/Layout825/Lamp") : null; var cab = Root("Camp") != null ? Root("Camp").transform.Find("Tower/Cab") : null;
if (lamp == null || cab == null) { pc.enabled = pcWas; return "no Camp_2 Lamp or the tower cab (run main3_8_25_camp2.cs)"; }
System.IO.Directory.CreateDirectory(outDir);
var urp = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline; var grdWas = urp.gpuResidentDrawerMode; urp.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.Disabled;
var rt = new UnityEngine.RenderTexture(shotW, shotH, 24, UnityEngine.RenderTextureFormat.ARGB32); var shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
try
{
    var core = lamp.Find("LampCore"); var at = core != null ? core.position : lamp.position + V(0f, 1.3f, 0f);
    var toLamp = V(at.x - cab.position.x, 0f, at.z - cab.position.z).normalized; var eye = V(cab.position.x, cab.position.y + eyeH, cab.position.z) + toLamp * deckIn;
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
    foreach (var bino in new[] { false, true }) { cam.fieldOfView = bino ? binoFov : camFov; Pose(eye, at); Shoot("DeckLamp_" + tag + (bino ? "_Binoculars" : "_Eye") + ".jpg"); }
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
