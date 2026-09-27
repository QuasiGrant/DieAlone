var outDir = @"C:\Users\grant\UnityProjects\DieAlone\Docs\Look";
System.IO.Directory.CreateDirectory(outDir);
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
var shots = new (string name, UnityEngine.Vector3 pos, UnityEngine.Vector3 look)[] {
    ("A_tower_room",  new UnityEngine.Vector3(0.8f, 11.6f, 10.6f),  new UnityEngine.Vector3(-1.0f, 10.9f, 13.4f)),
    ("B_tower_stairs", new UnityEngine.Vector3(-2.6f, 6.6f, 7.7f),  new UnityEngine.Vector3(2.0f, 7.8f, 8.4f)),
    ("C_test_course", new UnityEngine.Vector3(-17f, 1.6f, 12f),     new UnityEngine.Vector3(-17f, 0.5f, -20f)),
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
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, s.name + ".png"), tex.EncodeToPNG());
    sb.Append(s.name + " ");
}
cam.targetTexture = null;
UnityEngine.Object.DestroyImmediate(rt);
UnityEngine.Object.DestroyImmediate(tex);
UnityEngine.Object.DestroyImmediate(camGo);
return "wrote to Docs/Look: " + sb;
