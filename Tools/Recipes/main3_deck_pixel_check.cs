// Main3 deck pixel check (Play mode, Main3; CampLayout.md 5): one must-hide target of an area's deck list whose cover is trees or rock
// (treeCoverPath), from a batch of deck eyes. Run by main3_review_capture.sh --area, which sets the four values below and loops the
// batches in separate jobs: every 3840 x 1976 readback keeps a GPU staging buffer until the frame ends, and one job of all 128 eyes for
// three targets ran the Editor out of GPU memory (d3d12 device lost, 2026-10-02). Never saves; restores everything it touches.
// The target is painted flat magenta (URP Unlit) and rendered through the game camera (look filter on; all renders fall in one frame, so
// the filter's noise is the same in each) at its field of view, aimed at the target point, then rendered again with the target off;
// changed pixels (any channel more than pixelTolerance) are target pixels. raised = 1 lifts the target 20 m (or its controlRaise) first (the control: it
// must show from at least one eye or the check is VOID). The first eye also renders the off frame twice: that noise must be 0.
// The target's LOD groups are held at LOD 0 for every render (8.24: SS1's LODs culled it at deck range and voided its control).
// Returns "PIXEL <target> raised <0|1> eyes <from>-<to>: seen <eyes> worst <px> noise <px>".
string area = "camp"; int target = 0; int eyeFrom = 0; int raised = 0;
const int eyeBatch = 16;   // eyes per job (3 renders each plus one: 49 readbacks, about 1.5 GB of staging buffers at 3840 x 1976)
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var set = Main3AreaSet.Load(); var A = set != null ? set.Find(area) : null; if (A == null) return "no area " + area;
var covered = new System.Collections.Generic.List<Main3AreaSet.DeckTarget>(); foreach (var t in A.deckHide) if (!string.IsNullOrEmpty(t.treeCoverPath)) covered.Add(t);
if (target < 0 || target >= covered.Count) return "no pixel target " + target + " (" + covered.Count + " in " + area + ")";
var T = covered[target];
var obj = Main3AreaSet.At(scene, T.treeCoverPath); if (obj == null) return "PIXEL " + T.label + ": no " + T.treeCoverPath;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
const int shotW = 3840, shotH = 1976; const float defaultRaise = 20f;
float raise = T.controlRaise > 0f ? T.controlRaise : defaultRaise;   // 8.24: a target under fir and giant crowns needs a higher control
var tower = Root("Camp").transform.Find("Tower"); float deckTop = tower.Find("Cab").position.y;
var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>();   // the same order as main3_area_check.cs
for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid)
    foreach (var hgt in new[] { set.deckEye, set.deckJump }) eyes.Add(new UnityEngine.Vector3(tower.position.x + gx, deckTop + hgt, tower.position.z + gz));
int eyeTo = UnityEngine.Mathf.Min(eyes.Count, eyeFrom + eyeBatch);
var ter = UnityEngine.Terrain.activeTerrain;
var aim = T.point.y <= -900f ? new UnityEngine.Vector3(T.point.x, ter.SampleHeight(T.point) + ter.transform.position.y + 1f, T.point.z) : T.point;
if (raised == 1) aim += UnityEngine.Vector3.up * raise;
// every renderer under the target, drawn or not (the Ward stones are built with their renderers off): painted and drawn for the test
var rends = new System.Collections.Generic.List<UnityEngine.Renderer>(); var wasOn = new System.Collections.Generic.List<bool>(); foreach (var r in obj.GetComponentsInChildren<UnityEngine.Renderer>()) { rends.Add(r); wasOn.Add(r.enabled); }
var mats = new System.Collections.Generic.List<UnityEngine.Material[]>(); var lods = obj.GetComponentsInChildren<UnityEngine.LODGroup>();
var cam = UnityEngine.Camera.main; var camParent = cam.transform.parent; var camPos = cam.transform.localPosition; var camRot = cam.transform.localRotation;
var home = obj.position; UnityEngine.RenderTexture rt = null; UnityEngine.Texture2D shot = null; UnityEngine.Material flat = null;
int seen = 0, worst = 0, noise = 0;
try
{
    flat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Unlit")); flat.SetColor("_BaseColor", UnityEngine.Color.magenta);
    foreach (var r in rends) r.enabled = true;
    foreach (var lg in lods) lg.ForceLOD(0);   // 8.24: a small target's LOD group culls it at deck range, so the control could never show (SS1); the test is of cover, at LOD 0
    foreach (var r in rends) { mats.Add(r.sharedMaterials); var m = new UnityEngine.Material[r.sharedMaterials.Length]; for (int i = 0; i < m.Length; i++) m[i] = flat; r.sharedMaterials = m; }
    if (raised == 1) obj.position = home + UnityEngine.Vector3.up * raise;
    rt = new UnityEngine.RenderTexture(shotW, shotH, 24); shot = new UnityEngine.Texture2D(shotW, shotH, UnityEngine.TextureFormat.RGB24, false);
    cam.transform.SetParent(null, true); cam.targetTexture = rt;
    UnityEngine.Color32[] Shoot(UnityEngine.Vector3 eye)
    {
        cam.transform.position = eye; cam.transform.rotation = UnityEngine.Quaternion.LookRotation(aim - eye); cam.Render();
        var was = UnityEngine.RenderTexture.active; UnityEngine.RenderTexture.active = rt; shot.ReadPixels(new UnityEngine.Rect(0, 0, shotW, shotH), 0, 0); shot.Apply(false); UnityEngine.RenderTexture.active = was;
        return shot.GetPixels32();
    }
    int Diff(UnityEngine.Color32[] a, UnityEngine.Color32[] b) { int n = 0; for (int i = 0; i < a.Length; i++) if (System.Math.Abs(a[i].r - b[i].r) > set.pixelTolerance || System.Math.Abs(a[i].g - b[i].g) > set.pixelTolerance || System.Math.Abs(a[i].b - b[i].b) > set.pixelTolerance) n++; return n; }
    for (int e = eyeFrom; e < eyeTo; e++)
    {
        Shoot(eyes[e]);   // the first render after the camera moves carries the last view's history (837,204 px on a target with nothing drawn); discard it
        foreach (var r in rends) r.forceRenderingOff = false; var on = Shoot(eyes[e]);
        foreach (var r in rends) r.forceRenderingOff = true; var off = Shoot(eyes[e]);
        if (e == eyeFrom && eyeFrom == 0) noise = Diff(off, Shoot(eyes[e]));
        int px = Diff(on, off); if (px > 0) seen++; if (px > worst) worst = px;
    }
}
finally
{
    for (int i = 0; i < rends.Count; i++) { rends[i].forceRenderingOff = false; rends[i].enabled = wasOn[i]; if (i < mats.Count) rends[i].sharedMaterials = mats[i]; }
    foreach (var lg in lods) if (lg != null) lg.ForceLOD(-1);
    obj.position = home;
    cam.targetTexture = null; cam.transform.SetParent(camParent, false); cam.transform.localPosition = camPos; cam.transform.localRotation = camRot;
    if (rt != null) { rt.Release(); UnityEngine.Object.DestroyImmediate(rt); } if (shot != null) UnityEngine.Object.DestroyImmediate(shot); if (flat != null) UnityEngine.Object.DestroyImmediate(flat);
}
return "PIXEL " + T.label + " raised " + raised + " eyes " + eyeFrom + "-" + (eyeTo - 1) + " of " + eyes.Count + ": seen " + seen + " worst " + worst + " noise " + noise + " renderers " + rends.Count;
