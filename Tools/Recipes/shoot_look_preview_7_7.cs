// Task 7.7 shots: Graybox from the three look camera spots in the look LookPreview has active,
// then it selects the next look. Play mode in Graybox, runInBackground on. Run it once per look
// (three times from Current) with a few frames between runs, so LookEnvironment applies the new
// look's fog, sky and ambient before the next shots. Writes Docs/Look/Preview/<Look>_<spot>.png.
if (!UnityEditor.EditorApplication.isPlaying) return "play mode only";
if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Graybox") return "Graybox only";
var outDir = @"C:\Users\grant\UnityProjects\DieAlone\Docs\Look\Preview";
System.IO.Directory.CreateDirectory(outDir);
var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>();
if (preview == null) return "no LookPreview in the scene";
string look = string.Concat(preview.CurrentLabel.Split(' ').Where(w => w.Length > 0).Select(w => char.ToUpperInvariant(w[0]) + w.Substring(1)));   // "Day one" -> DayOne

var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam");
camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>();
cam.CopyFrom(main);
cam.fieldOfView = 60f; cam.nearClipPlane = 0.05f; cam.farClipPlane = 500f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720;
var rt = new UnityEngine.RenderTexture(w, h, 24);
var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 lookAt)[] {
    ("A_tower_room",   new UnityEngine.Vector3(0.8f, 11.6f, 10.6f), new UnityEngine.Vector3(-1.0f, 10.9f, 13.4f)),
    ("B_tower_stairs", new UnityEngine.Vector3(-2.6f, 6.6f, 7.7f),  new UnityEngine.Vector3(2.0f, 7.8f, 8.4f)),
    ("C_test_course",  new UnityEngine.Vector3(-17f, 1.6f, 12f),    new UnityEngine.Vector3(-17f, 0.5f, -20f)),
};
var sb = new System.Text.StringBuilder(look + ": ");
foreach (var s in shots)
{
    camGo.transform.position = s.pos;
    camGo.transform.LookAt(s.lookAt);
    cam.targetTexture = rt;
    cam.Render();
    UnityEngine.RenderTexture.active = rt;
    tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0);
    tex.Apply();
    UnityEngine.RenderTexture.active = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, look + "_" + s.name + ".png"), tex.EncodeToPNG());
    sb.Append(s.name + " ");
}
cam.targetTexture = null;
UnityEngine.Object.DestroyImmediate(rt);
UnityEngine.Object.DestroyImmediate(tex);
UnityEngine.Object.DestroyImmediate(camGo);
preview.Select((preview.Current + 1) % preview.Count);
return sb.ToString() + "| next look: " + preview.CurrentLabel;
