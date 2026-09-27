var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\signs2";
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam"); camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.CopyFrom(main); cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720; var rt = new UnityEngine.RenderTexture(w, h, 24); var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0, z));
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("J1_from_cabin_door", new UnityEngine.Vector3(245f, H(245f,187.3f) + 1.6f, 187.3f), new UnityEngine.Vector3(243f, 25.6f, 181f)),
    ("J2_trailA_sign",     new UnityEngine.Vector3(245.5f, H(245.5f,183.5f) + 1.6f, 183.5f), new UnityEngine.Vector3(240.5f, 25.8f, 183f)),
    ("J3_toward_trailB",   new UnityEngine.Vector3(249f, H(249f,181f) + 1.6f, 181f), new UnityEngine.Vector3(253f, 25.8f, 171.5f)),
    ("J4_trailB_sign",     new UnityEngine.Vector3(251.5f, H(251.5f,176f) + 1.6f, 176f), new UnityEngine.Vector3(253f, 25.8f, 171.5f)),
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
