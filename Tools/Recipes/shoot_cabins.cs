var outDir = System.IO.Path.Combine(@"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad", "cabins");
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam"); camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.CopyFrom(main); cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720; var rt = new UnityEngine.RenderTexture(w, h, 24); var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("B1_cabin2_from_trail", V(253f, 25.6f, 97f), V(247f, 25.5f, 86.5f)),
    ("B2_cabin2_side", V(238f, 25.6f, 90f), V(247f, 25.5f, 85f)),
    ("B3_cabin3_from_trail", V(314f, 25.6f, 72f), V(323f, 25.5f, 58.5f)),
    ("B4_cabin3_inside", V(323f, 25.6f, 57.8f), V(328f, 25f, 57f)),
};
foreach (var s in shots)
{
    camGo.transform.position = s.pos; camGo.transform.LookAt(s.look);
    cam.targetTexture = rt; cam.Render(); UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + ".png"), tex.EncodeToPNG());
}
cam.targetTexture = null; UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(camGo);
return "wrote cabin shots";
