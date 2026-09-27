var outDir = System.IO.Path.Combine(@"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad", "camp");
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam"); camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.CopyFrom(main); cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720; var rt = new UnityEngine.RenderTexture(w, h, 24); var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("K1_stairs", V(238f, 25.6f, 192f), V(236f, 30f, 196f)),
    ("K2_fire_from_cabin_door", V(245f, 25.6f, 187f), V(251.5f, 24.8f, 182.5f)),
    ("K3_cabin_front", V(252f, 25.6f, 184f), V(246f, 25.5f, 189f)),
    ("K4_fire_close", V(254.5f, 25.4f, 185.5f), V(251.5f, 24.6f, 182.5f)),
};
foreach (var s in shots)
{
    camGo.transform.position = s.pos; camGo.transform.LookAt(s.look);
    cam.targetTexture = rt; cam.Render(); UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + ".png"), tex.EncodeToPNG());
}
cam.targetTexture = null; UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(camGo);
return "wrote camp shots";
