var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\look_extreme";
var __t = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); float __b = __t.colorBleed, __w = __t.washOut, __c = __t.crushBlacks; __t.colorBleed = 1f; __t.washOut = 1f; __t.crushBlacks = 1f;
System.IO.Directory.CreateDirectory(outDir);
var main = UnityEngine.Camera.main;
var camGo = new UnityEngine.GameObject("__ShotCam");
camGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cam = camGo.AddComponent<UnityEngine.Camera>();
cam.CopyFrom(main);
cam.fieldOfView = 60f; cam.nearClipPlane = 0.05f; cam.farClipPlane = 500f;
var extra = camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
int w = 1280, h = 720;
var rt = new UnityEngine.RenderTexture(w, h, 24);
var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);
// The three fixed camera spots for Milestone 3. Recorded in PLAN.md Rules and Tips.
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
__t.colorBleed = __b; __t.washOut = __w; __t.crushBlacks = __c;
return "wrote: " + sb + " dirty=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty;
