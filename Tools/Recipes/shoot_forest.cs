var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\forest";
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam");
camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>();
cam.CopyFrom(main);
cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720;
var rt = new UnityEngine.RenderTexture(w, h, 24);
var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0, z));
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("F1_path_a_eye",   new UnityEngine.Vector3(130f, H(130f,270f) + 1.6f, 270f), new UnityEngine.Vector3(115f, H(115f,240f) + 1.2f, 240f)),
    ("F2_camp_to_ward", new UnityEngine.Vector3(240f, H(240f,188f) + 1.6f, 188f), new UnityEngine.Vector3(66f, 30f, 323f)),
    ("F3_camp_clearing_edge", new UnityEngine.Vector3(250f, H(250f,180f) + 1.6f, 180f), new UnityEngine.Vector3(312f, 25f, 124f)),
    ("F4_ward_looking_back", new UnityEngine.Vector3(80f, H(80f,312f) + 1.6f, 312f), new UnityEngine.Vector3(110f, 26f, 300f)),
    ("F5_overview",     new UnityEngine.Vector3(200f, 140f, 60f), new UnityEngine.Vector3(200f, 24f, 220f)),
};
var sb = new System.Text.StringBuilder();
foreach (var s in shots)
{
    camGo.transform.position = s.pos; camGo.transform.LookAt(s.look);
    cam.targetTexture = rt; cam.Render();
    UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + ".png"), tex.EncodeToPNG());
    sb.Append(s.name + " ");
}
cam.targetTexture = null;
UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(camGo);
return "wrote: " + sb;
