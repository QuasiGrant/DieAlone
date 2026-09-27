var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\camp2";
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
    ("D1_cabin_corner",   new UnityEngine.Vector3(251f, H(251f,184f) + 1.6f, 184f), new UnityEngine.Vector3(246f, 26f, 190f)),
    ("D2_cabin_roof_high", new UnityEngine.Vector3(238f, 34f, 178f), new UnityEngine.Vector3(245f, 26f, 190f)),
    ("D3_cabin_back",     new UnityEngine.Vector3(240f, H(240f,197f) + 1.6f, 197f), new UnityEngine.Vector3(245f, 26f, 191f)),
    ("D4_tower_stairs",   new UnityEngine.Vector3(226f, H(226f,199f) + 1.6f, 199f), new UnityEngine.Vector3(229f, 28f, 204f)),
    ("D5_from_cabin_door", new UnityEngine.Vector3(245f, 25.7f, 187.5f), new UnityEngine.Vector3(260f, 24.5f, 175f)),
    ("D6_camp_wide",      new UnityEngine.Vector3(268f, 30f, 160f), new UnityEngine.Vector3(240f, 25f, 195f)),
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
