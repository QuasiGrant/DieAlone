var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\course";
System.IO.Directory.CreateDirectory(outDir);

var camGo = new UnityEngine.GameObject("__ShotCam");
camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>();
cam.fieldOfView = 60f;
cam.nearClipPlane = 0.05f;
cam.farClipPlane = 500f;
int w = 1280, h = 720;
var rt = new UnityEngine.RenderTexture(w, h, 24);
var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);

var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("01_overview",       new UnityEngine.Vector3(-30f, 28f, -34f), new UnityEngine.Vector3(0f, 0f, 2f)),
    ("02_sprint_lane",    new UnityEngine.Vector3(-22f, 3f, -27f),  new UnityEngine.Vector3(-22f, 0.5f, 10f)),
    ("03_tunnel_step_log",new UnityEngine.Vector3(-11f, 3.5f, -25f), new UnityEngine.Vector3(-17f, 0.5f, -12f)),
    ("04_fence_ramp",     new UnityEngine.Vector3(-10f, 3.5f, -10f), new UnityEngine.Vector3(-17f, 0.8f, 0f)),
    ("05_ramp_stairs",    new UnityEngine.Vector3(-11f, 4f, 10f),   new UnityEngine.Vector3(-17f, 0.8f, 4f)),
    ("06_room",           new UnityEngine.Vector3(18f, 3f, 6f),     new UnityEngine.Vector3(18f, 1f, 14f)),
    ("07_table_shelf",    new UnityEngine.Vector3(19.5f, 1.8f, 5.5f), new UnityEngine.Vector3(19.7f, 0.8f, 8f)),
};

var sb = new System.Text.StringBuilder();
foreach (var s in shots)
{
    camGo.transform.position = s.pos;
    camGo.transform.LookAt(s.look);
    cam.targetTexture = rt;
    cam.Render();
    UnityEngine.RenderTexture.active = rt;
    tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0);
    tex.Apply();
    UnityEngine.RenderTexture.active = null;
    var path = System.IO.Path.Combine(outDir, s.name + ".png");
    System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
    sb.Append(s.name + " ");
}
cam.targetTexture = null;
UnityEngine.Object.DestroyImmediate(rt);
UnityEngine.Object.DestroyImmediate(tex);
UnityEngine.Object.DestroyImmediate(camGo);
return "wrote: " + sb + " dirty=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty;
