// 8.15 prep (edit mode, throwaway): one sample of the owned ground layers for Pim's W1 test (the path reads brighter or
// darker than the ground). Opens an unsaved additive scene far from the map, builds a scratch terrain (layers made in
// memory, no assets written): forest floor (BK GrassPine) either side, a 3 m bare dirt trail (Ground054) down the middle,
// a rock slope (BK Rocks) rising on the right, with a BK boulder and a hollow log as a visible stop by the trail.
// Lit as day one (sun, ambient and fog from LookTuning_DayOne, the look filter through LookOverride), rendered at
// 1920 x 988 from 1.6 m on the trail looking along it, saved in grayscale to Docs/Captures/W1Sample (git-ignored).
// Closes the scene without saving and puts the active scene and the look back; the open scene is never touched.
if (UnityEngine.Application.isPlaying) return "edit mode only";
const int W = 1920, H = 988, Res = 129, AlphaRes = 128;
const float Size = 80f, MaxHeight = 20f, TrailHalf = 1.5f, RockFrom = 12f, RockRise = 0.6f, Eye = 1.6f, Far = 10000f;
string outDir = System.IO.Path.GetFullPath("Docs/Captures/W1Sample"); System.IO.Directory.CreateDirectory(outDir);
var d1 = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayOne.asset");
UnityEngine.Texture2D Tex(string p) => UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(p);
UnityEngine.GameObject Pf(string p) => UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p);
var floorTex = Tex("Assets/BK/PureNature_Redwood/Textures/Surfaces/GrassPine_a.png");
var trailTex = Tex("Assets/Textures/Ground054/Ground054_Color.jpg");
var rockTex = Tex("Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Rocks_a.png");
var boulder = Pf("Assets/BK/PureNature_Redwood/Prefabs/Rocks/Boulder_1.prefab");
var log = Pf("Assets/BK/PureNature_Redwood/Prefabs/HollowLogs/RedwoodHollowLog_1.prefab");
if (d1 == null || floorTex == null || trailTex == null || rockTex == null || boulder == null || log == null) return "missing an input asset";

var main = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Additive);
var made = new System.Collections.Generic.List<UnityEngine.Object>();
string result;
try
{
    UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);   // RenderSettings come from the active scene
    var origin = new UnityEngine.Vector3(Far, 0f, Far);
    var data = new UnityEngine.TerrainData(); made.Add(data);
    data.heightmapResolution = Res; data.alphamapResolution = AlphaRes; data.size = new UnityEngine.Vector3(Size, MaxHeight, Size);
    UnityEngine.TerrainLayer Layer(UnityEngine.Texture2D t, float tile) { var l = new UnityEngine.TerrainLayer { diffuseTexture = t, tileSize = new UnityEngine.Vector2(tile, tile) }; made.Add(l); return l; }
    data.terrainLayers = new[] { Layer(floorTex, 4f), Layer(trailTex, 2f), Layer(rockTex, 6f) };
    float centre = Size * 0.5f;   // the trail runs along z at x = centre
    var heights = new float[Res, Res];
    for (int z = 0; z < Res; z++) for (int x = 0; x < Res; x++) { float wx = x * Size / (Res - 1) - centre; heights[z, x] = wx > RockFrom ? (wx - RockFrom) * RockRise / MaxHeight : 0f; }
    data.SetHeights(0, 0, heights);
    var alpha = new float[AlphaRes, AlphaRes, 3];
    for (int z = 0; z < AlphaRes; z++) for (int x = 0; x < AlphaRes; x++)
    {
        float wx = (x + 0.5f) * Size / AlphaRes - centre; int k = UnityEngine.Mathf.Abs(wx) <= TrailHalf ? 1 : wx > RockFrom ? 2 : 0; alpha[z, x, k] = 1f;
    }
    data.SetAlphamaps(0, 0, alpha);
    var terrainGo = UnityEngine.Terrain.CreateTerrainGameObject(data); terrainGo.transform.position = origin;
    var sunGo = new UnityEngine.GameObject("Sun"); var sun = sunGo.AddComponent<UnityEngine.Light>(); sun.type = UnityEngine.LightType.Directional; sun.shadows = UnityEngine.LightShadows.Soft;
    sun.color = d1.sunColor; sun.intensity = d1.sunIntensity; sunGo.transform.rotation = UnityEngine.Quaternion.Euler(d1.sunElevation, d1.sunBearing + 180f, 0f);
    UnityEngine.RenderSettings.sun = sun;
    UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat; UnityEngine.RenderSettings.ambientLight = d1.sunsetAmbient;
    UnityEngine.RenderSettings.fog = true; UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Linear; UnityEngine.RenderSettings.fogColor = d1.sunsetFogColor;
    UnityEngine.RenderSettings.fogStartDistance = d1.sunsetFogStart; UnityEngine.RenderSettings.fogEndDistance = d1.sunsetFogEnd;
    // the stop: a boulder and a log just off the trail's left edge, 12 and 16 m ahead
    var b = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(boulder, scene); b.transform.position = origin + new UnityEngine.Vector3(centre - TrailHalf - 3.5f, 0f, 22f);
    var l2 = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(log, scene); l2.transform.position = origin + new UnityEngine.Vector3(centre - TrailHalf - 1.2f, 0f, 16f); l2.transform.rotation = UnityEngine.Quaternion.Euler(0f, 20f, 0f);
    var camGo = new UnityEngine.GameObject("Cam"); var cam = camGo.AddComponent<UnityEngine.Camera>();
    cam.fieldOfView = 60f; cam.farClipPlane = 1000f; cam.clearFlags = UnityEngine.CameraClearFlags.SolidColor; cam.backgroundColor = d1.skyHorizon;
    camGo.transform.position = origin + new UnityEngine.Vector3(centre, Eye, 4f); camGo.transform.rotation = UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(0f, -0.08f, 1f));
    LookOverride.Set(d1, true, null);
    var rt = new UnityEngine.RenderTexture(W, H, 24, UnityEngine.RenderTextureFormat.ARGB32); made.Add(rt);
    cam.targetTexture = rt; cam.Render(); cam.targetTexture = null;
    var tex = new UnityEngine.Texture2D(W, H, UnityEngine.TextureFormat.RGB24, false); made.Add(tex);
    UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, W, H), 0, 0); UnityEngine.RenderTexture.active = null;
    var px = tex.GetPixels32();
    // mean grey of trail, floor and rock in a strip 8 to 20 m ahead, from the screen positions of the band edges
    int Mean(float x0, float x1) { var a = cam.WorldToScreenPoint(origin + new UnityEngine.Vector3(centre + x0, 0f, 20f)); var c = cam.WorldToScreenPoint(origin + new UnityEngine.Vector3(centre + x1, 0f, 12f)); int s = 0, n = 0; for (int y = (int)UnityEngine.Mathf.Min(a.y, c.y); y < (int)UnityEngine.Mathf.Max(a.y, c.y); y++) for (int x = (int)UnityEngine.Mathf.Min(a.x, c.x); x < (int)UnityEngine.Mathf.Max(a.x, c.x); x++) { if (x < 0 || x >= W || y >= H) continue; var p = px[y * W + x]; s += (int)(0.2126f * p.r + 0.7152f * p.g + 0.0722f * p.b); n++; } return n > 0 ? s / n : -1; }
    string grey = "floor " + Mean(-8f, -3f) + ", trail " + Mean(-1f, 1f) + ", rock " + Mean(RockFrom + 2f, RockFrom + 6f) + " (0 to 255)";
    for (int i = 0; i < px.Length; i++) { byte g = (byte)(0.2126f * px[i].r + 0.7152f * px[i].g + 0.0722f * px[i].b); px[i] = new UnityEngine.Color32(g, g, g, 255); }
    tex.SetPixels32(px); tex.Apply();
    string file = System.IO.Path.Combine(outDir, "W1_sample_gray_1920x988.png"); System.IO.File.WriteAllBytes(file, tex.EncodeToPNG());
    result = file + " | " + grey;
}
finally
{
    LookOverride.Clear();
    UnityEngine.SceneManagement.SceneManager.SetActiveScene(main);
    UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
    foreach (var o in made) if (o != null) UnityEngine.Object.DestroyImmediate(o);
}
return result + " | open scene " + main.name + " dirty " + main.isDirty;
