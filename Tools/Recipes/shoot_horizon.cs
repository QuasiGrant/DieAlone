var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\horizon";
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam"); camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.CopyFrom(main); cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720; var rt = new UnityEngine.RenderTexture(w, h, 24); var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0, z));
var tower = UnityEngine.GameObject.Find("Camp/FirewatchTower").transform;
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("H1_ledge_west",   new UnityEngine.Vector3(60f, H(60f,323f) + 1.6f, 323f), new UnityEngine.Vector3(-150f, 40f, 300f)),
    ("H2_ledge_wide",   new UnityEngine.Vector3(70f, H(70f,318f) + 1.6f, 318f), new UnityEngine.Vector3(-120f, 30f, 200f)),
    ("H3_tower_top",    tower.position + new UnityEngine.Vector3(-3.5f, 11.6f, 0f), new UnityEngine.Vector3(-150f, 45f, 250f)),
    ("H4_sky_up",       new UnityEngine.Vector3(60f, H(60f,323f) + 1.6f, 323f), new UnityEngine.Vector3(-100f, 120f, 300f)),
};
var sb = new System.Text.StringBuilder();
foreach (var s in shots)
{
    camGo.transform.position = s.pos; camGo.transform.LookAt(s.look);
    cam.targetTexture = rt; cam.Render(); UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + ".png"), tex.EncodeToPNG()); sb.Append(s.name + " ");
}
cam.targetTexture = null; UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(camGo);
return "wrote: " + sb;
