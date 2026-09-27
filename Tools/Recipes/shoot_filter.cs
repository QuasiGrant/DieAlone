UnityEngine.Application.runInBackground = true;
var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\filter";
System.IO.Directory.CreateDirectory(outDir);
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");
bool wasOn = tuning.filterEnabled; int wasHeight = tuning.lowResHeight;
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
var spots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("A_tower_room",  new UnityEngine.Vector3(0.8f, 11.6f, 10.6f),  new UnityEngine.Vector3(-1.0f, 10.9f, 13.4f)),
    ("B_tower_stairs", new UnityEngine.Vector3(-2.6f, 6.6f, 7.7f),  new UnityEngine.Vector3(2.0f, 7.8f, 8.4f)),
    ("C_test_course", new UnityEngine.Vector3(-17f, 1.6f, 12f),     new UnityEngine.Vector3(-17f, 0.5f, -20f)),
};
var configs = new (string tag, bool on, int height)[] { ("off", false, 480), ("on480", true, 480), ("on240", true, 240) };
var sb = new System.Text.StringBuilder();
foreach (var c in configs)
{
    tuning.filterEnabled = c.on; tuning.lowResHeight = c.height;
    foreach (var s in spots)
    {
        camGo.transform.position = s.pos; camGo.transform.LookAt(s.look);
        cam.targetTexture = rt; cam.Render();
        UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + "_" + c.tag + ".png"), tex.EncodeToPNG());
        sb.Append(s.name + "_" + c.tag + " ");
    }
}
tuning.filterEnabled = wasOn; tuning.lowResHeight = wasHeight;
cam.targetTexture = null;
UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(camGo);
return "wrote: " + sb;
