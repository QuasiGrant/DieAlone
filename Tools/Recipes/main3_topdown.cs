// Main3 top-down image, rendered in 100 m tiles (5 x 5, 512 px each, x -50 to 450, z -75 to 425: the valley and its ring of ridges,
// 8.9j) and stitched to Docs/Layout/Main3/Main3_top.png.
// Never one oversized render. A temporary Preview-type camera skips the look filter; fog and the scene lights are
// switched off for the shot, a temporary overhead light is used, and everything is restored afterward.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
const int tilesX = 5, tilesZ = 5, px = 512; const float tileM = 100f, originX = -50f, originZ = -75f;
string outDir = System.IO.Path.GetFullPath("Docs/Layout/Main3");
System.IO.Directory.CreateDirectory(outDir);

bool fog = UnityEngine.RenderSettings.fog; UnityEngine.RenderSettings.fog = false;
var lights = UnityEngine.Object.FindObjectsByType<UnityEngine.Light>(UnityEngine.FindObjectsSortMode.None);
var wasOn = new bool[lights.Length];
for (int i = 0; i < lights.Length; i++) { wasOn[i] = lights[i].enabled; lights[i].enabled = false; }
var lightGo = new UnityEngine.GameObject("TopDownLight");
var l = lightGo.AddComponent<UnityEngine.Light>(); l.type = UnityEngine.LightType.Directional; l.intensity = 1.1f; l.shadows = UnityEngine.LightShadows.None;
lightGo.transform.rotation = UnityEngine.Quaternion.Euler(55f, 315f, 0f);   // light from the north-west, the usual map hillshade
var camGo = new UnityEngine.GameObject("TopDownCam");
var cam = camGo.AddComponent<UnityEngine.Camera>();
cam.cameraType = UnityEngine.CameraType.Preview;
cam.orthographic = true; cam.orthographicSize = tileM / 2f; cam.aspect = 1f;
cam.nearClipPlane = 1f; cam.farClipPlane = 500f;
cam.clearFlags = UnityEngine.CameraClearFlags.SolidColor; cam.backgroundColor = UnityEngine.Color.black;
cam.enabled = false;
var rt = new UnityEngine.RenderTexture(px, px, 24, UnityEngine.RenderTextureFormat.ARGB32);
var full = new UnityEngine.Texture2D(px * tilesX, px * tilesZ, UnityEngine.TextureFormat.RGB24, false);
var tile = new UnityEngine.Texture2D(px, px, UnityEngine.TextureFormat.RGB24, false);
try
{
    cam.targetTexture = rt;
    for (int tz = 0; tz < tilesZ; tz++)
        for (int tx = 0; tx < tilesX; tx++)
        {
            camGo.transform.position = new UnityEngine.Vector3(originX + (tx + 0.5f) * tileM, 300f, originZ + (tz + 0.5f) * tileM);
            camGo.transform.rotation = UnityEngine.Quaternion.Euler(90f, 0f, 0f);   // up on the image is north
            cam.Render();
            UnityEngine.RenderTexture.active = rt;
            tile.ReadPixels(new UnityEngine.Rect(0, 0, px, px), 0, 0); tile.Apply();
            UnityEngine.RenderTexture.active = null;
            full.SetPixels(tx * px, tz * px, px, px, tile.GetPixels());
        }
    full.Apply();
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, "Main3_top.png"), full.EncodeToPNG());
}
finally
{
    cam.targetTexture = null; rt.Release();
    UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(full); UnityEngine.Object.DestroyImmediate(tile);
    UnityEngine.Object.DestroyImmediate(camGo); UnityEngine.Object.DestroyImmediate(lightGo);
    for (int i = 0; i < lights.Length; i++) lights[i].enabled = wasOn[i];
    UnityEngine.RenderSettings.fog = fog;
}
UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "wrote " + System.IO.Path.Combine(outDir, "Main3_top.png") + " (" + (px * tilesX) + " x " + (px * tilesZ) + ", 5.12 px per m, north up, x " + originX + " to " + (originX + tilesX * tileM) + ", z " + originZ + " to " + (originZ + tilesZ * tileM) + ")";
