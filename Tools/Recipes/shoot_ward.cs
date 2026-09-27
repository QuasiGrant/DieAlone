var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\spots";
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam"); camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.CopyFrom(main); cam.fieldOfView = 60f; cam.nearClipPlane = 0.1f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720; var rt = new UnityEngine.RenderTexture(w, h, 24); var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0, z));
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("W1_ward_from_warp", new UnityEngine.Vector3(62f, H(62f,322f) + 1.6f, 322f), new UnityEngine.Vector3(47f, H(47f,322f) + 4f, 322.5f)),
    ("W2_ward_close", new UnityEngine.Vector3(53f, H(53f,326f) + 1.6f, 326f), new UnityEngine.Vector3(47.5f, H(47.5f,322.5f) + 5f, 322.5f)),
    ("W3_ward_from_sign", new UnityEngine.Vector3(84f, H(84f,311f) + 1.6f, 311f), new UnityEngine.Vector3(47f, H(47f,322f) + 3f, 322f)),
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
