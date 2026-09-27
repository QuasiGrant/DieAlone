var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\e2b909f8-0c00-4429-9329-45e87548c225\scratchpad\tower";
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
    ("01_tower_overview", new UnityEngine.Vector3(-16f, 9f, -6f),  new UnityEngine.Vector3(0f, 6f, 12f)),
    ("02_stairs_south",   new UnityEngine.Vector3(0f, 5f, -2f),    new UnityEngine.Vector3(0f, 5f, 8f)),
    ("03_stairs_closeup", new UnityEngine.Vector3(-5f, 2f, 3f),    new UnityEngine.Vector3(0f, 3f, 8f)),
    ("04_deck_arrival",   new UnityEngine.Vector3(-2.25f, 11.6f, 6.5f), new UnityEngine.Vector3(0f, 11f, 12f)),
    ("05_room_door",      new UnityEngine.Vector3(-1.1f, 11.6f, 8.6f), new UnityEngine.Vector3(-1.1f, 11.2f, 12f)),
    ("06_room_inside",    new UnityEngine.Vector3(-1.1f, 11.6f, 10.4f), new UnityEngine.Vector3(1.0f, 10.8f, 13.6f)),
    ("07_walkway",        new UnityEngine.Vector3(3.2f, 11.6f, 8.8f),  new UnityEngine.Vector3(3.2f, 11.0f, 15f)),
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
