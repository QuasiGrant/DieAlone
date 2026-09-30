// 8.16 baseline (Play mode, Main3): frame rate from fixed spots at the current Game view size (set 3840 x 1976 with
// ui_game_view_size_8_9h.cs first). For each spot: pose the camera, skip WarmFrames, then record Frames frame times
// (unscaled). Reports the average FPS and the 1% low (the FPS of the mean of the slowest 1% of frames). Editor Play mode,
// so Editor overhead is included; compare only runs made the same way. Runs from EditorApplication.update and removes
// itself; the result is Temp/perf_baseline.txt ("done" on the last line). The dev panel stays closed; the look is not changed.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
const int WarmFrames = 60, Frames = 600;
const float Eye = 1.6f, SpotLookAhead = 40f;
UnityEngine.Application.runInBackground = true;
string logPath = System.IO.Path.GetFullPath("Temp/perf_baseline.txt"); System.IO.File.WriteAllText(logPath, "");
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition;
var terrain = UnityEngine.Terrain.activeTerrain;
UnityEngine.Vector3 At(float x, float z) => new UnityEngine.Vector3(x, terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y + Eye, z);
UnityEngine.Vector3 Ahead(UnityEngine.Vector3 c, float yaw) => c + UnityEngine.Quaternion.Euler(0f, yaw, 0f) * UnityEngine.Vector3.forward * SpotLookAhead;
// Camp toward the tower and cabin; S1 across camp to the valley; the office lot east (LightingOptions.md spots)
var spots = new (string name, UnityEngine.Vector3 cam, UnityEngine.Vector3 look)[] {
    ("Camp", At(172f, 150f), Ahead(At(172f, 150f), 333f)),
    ("S1", At(156f, 148f), new UnityEngine.Vector3(178f, At(156f, 148f).y, 168f)),
    ("Office", At(340f, 196f), Ahead(At(340f, 196f), 68f)) };
void Pose(UnityEngine.Vector3 camPos, UnityEngine.Vector3 look)
{
    var dir = look - camPos; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z);
    cc.enabled = false; pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized); pc.transform.position = camPos - pc.transform.rotation * camLocal;
    cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
}
pc.enabled = false;
int spot = 0, frame = 0; var times = new System.Collections.Generic.List<float>();
var log = new System.Text.StringBuilder("screen " + UnityEngine.Screen.width + " x " + UnityEngine.Screen.height + ", " + Frames + " frames per spot after " + WarmFrames + " warm-up, vSync " + UnityEngine.QualitySettings.vSyncCount + ", target " + UnityEngine.Application.targetFrameRate + "\n");
Pose(spots[0].cam, spots[0].look);
UnityEditor.EditorApplication.CallbackFunction tick = null; int lastFrame = UnityEngine.Time.frameCount;
tick = () =>
{
    if (!UnityEngine.Application.isPlaying) { UnityEditor.EditorApplication.update -= tick; return; }
    if (UnityEngine.Time.frameCount == lastFrame) return;   // one sample per player frame
    lastFrame = UnityEngine.Time.frameCount;
    frame++;
    if (frame > WarmFrames) times.Add(UnityEngine.Time.unscaledDeltaTime);
    if (times.Count < Frames) return;
    times.Sort(); int worst = UnityEngine.Mathf.Max(1, times.Count / 100); float sum = 0f, worstSum = 0f;
    foreach (var t in times) sum += t; for (int i = times.Count - worst; i < times.Count; i++) worstSum += times[i];
    log.Append(spots[spot].name + ": average " + (times.Count / sum).ToString("F1") + " fps, 1% low " + (worst / worstSum).ToString("F1") + " fps\n");
    spot++; frame = 0; times.Clear();
    if (spot < spots.Length) { Pose(spots[spot].cam, spots[spot].look); return; }
    UnityEditor.EditorApplication.update -= tick;
    cc.enabled = true; pc.enabled = true;
    log.Append("done\n"); System.IO.File.WriteAllText(logPath, log.ToString());
};
UnityEditor.EditorApplication.update += tick;
return "started; log " + logPath;
