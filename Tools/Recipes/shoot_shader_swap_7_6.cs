// Task 7.6 check shots: one image per swapped material group, look filter on (game camera).
// Builds the samples in a temporary additive scene that is made active for its lighting and then
// closed without saving, so Graybox and every other scene stay untouched. Edit mode only.
// Writes PNGs to the scratchpad folder below. Smoke colours come from LookTuning.asset.
if (UnityEditor.EditorApplication.isPlaying) return "edit mode only";
var outDir = @"C:\Users\grant\AppData\Local\Temp\claude\C--Users-grant-UnityProjects-DieAlone\4493a8cb-7fee-455f-ab76-5be059ca73b3\scratchpad\swap_7_6";
System.IO.Directory.CreateDirectory(outDir);
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");
LookEnvironment.ApplySmokeGlobals(tuning);

var previousActive = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Additive);
UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
UnityEngine.RenderSettings.ambientLight = tuning.sunsetAmbient;
UnityEngine.RenderSettings.fog = false;
UnityEngine.RenderSettings.skybox = null;

var sunGo = new UnityEngine.GameObject("Sun");
UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(sunGo, scene);
var sun = sunGo.AddComponent<UnityEngine.Light>();
sun.type = UnityEngine.LightType.Directional;
sun.color = new UnityEngine.Color(1f, 0.79f, 0.54f);   // Style.md 6.0 day one sun #FFC98A
sun.intensity = 1.2f;
sun.shadows = UnityEngine.LightShadows.Soft;
sunGo.transform.rotation = UnityEngine.Quaternion.Euler(20f, 60f, 0f);

var camGo = new UnityEngine.GameObject("ShotCam");
UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(camGo, scene);
var cam = camGo.AddComponent<UnityEngine.Camera>();
cam.clearFlags = UnityEngine.CameraClearFlags.SolidColor;
cam.backgroundColor = tuning.skyHorizon * 0.6f;
cam.fieldOfView = 50f; cam.nearClipPlane = 0.05f; cam.farClipPlane = 2000f;
camGo.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();

int w = 960, h = 540;
var rt = new UnityEngine.RenderTexture(w, h, 24);
var tex = new UnityEngine.Texture2D(w, h, UnityEngine.TextureFormat.RGB24, false);

UnityEngine.GameObject Place(string path, UnityEngine.Vector3 pos, UnityEngine.Transform parent)
{
    var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path);
    if (prefab == null) throw new System.Exception("missing prefab " + path);
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, scene);
    go.transform.SetParent(parent, false);
    go.transform.localPosition = pos;
    return go;
}

void Shoot(string name, UnityEngine.GameObject root, UnityEngine.Vector3 viewDir, float distanceScale, float simulate)
{
    foreach (var ps in root.GetComponentsInChildren<UnityEngine.ParticleSystem>()) ps.Simulate(simulate, true, true);
    var rs = root.GetComponentsInChildren<UnityEngine.Renderer>();
    var b = rs[0].bounds;
    foreach (var r in rs) b.Encapsulate(r.bounds);
    float d = b.extents.magnitude / UnityEngine.Mathf.Tan(cam.fieldOfView * 0.5f * UnityEngine.Mathf.Deg2Rad) * distanceScale;
    camGo.transform.position = b.center - viewDir.normalized * d;
    camGo.transform.LookAt(b.center);
    cam.targetTexture = rt;
    cam.Render();
    UnityEngine.RenderTexture.active = rt;
    tex.ReadPixels(new UnityEngine.Rect(0, 0, w, h), 0, 0);
    tex.Apply();
    UnityEngine.RenderTexture.active = null;
    cam.targetTexture = null;
    System.IO.File.WriteAllBytes(System.IO.Path.Combine(outDir, name + ".png"), tex.EncodeToPNG());
    root.SetActive(false);
}

UnityEngine.GameObject Group(string name)
{
    var g = new UnityEngine.GameObject(name);
    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(g, scene);
    g.transform.position = new UnityEngine.Vector3(4000f, 0f, 4000f);   // far from whatever scene is open
    return g;
}

var side = new UnityEngine.Vector3(-1f, -0.35f, 1f);
var sb = new System.Text.StringBuilder();
try
{
    var rocks = Group("Rocks");
    Place("Assets/BK/PureNature_Redwood/Prefabs/Rocks/Boulder_0.prefab", new UnityEngine.Vector3(0, 0, 0), rocks.transform);
    Place("Assets/BK/PureNature_Redwood/Prefabs/HollowLogs/RedwoodHollowLog_0.prefab", new UnityEngine.Vector3(5, 0, 2), rocks.transform);
    Shoot("01_boulder_log", rocks, side, 0.9f, 0f);

    var near = Group("GiantNear");
    Place("Assets/BK/PureNature_Redwood/Prefabs/Trees/Sequoia1.prefab", UnityEngine.Vector3.zero, near.transform);
    Shoot("02_giant_near", near, new UnityEngine.Vector3(-1f, 0.1f, 1f), 0.8f, 0f);

    var far = Group("GiantFar");
    var farTree = Place("Assets/BK/PureNature_Redwood/Prefabs/Trees/Sequoia1.prefab", UnityEngine.Vector3.zero, far.transform);
    var lod = farTree.GetComponentInChildren<UnityEngine.LODGroup>();
    if (lod != null) lod.ForceLOD(lod.lodCount - 1);
    Shoot("03_giant_impostor", far, new UnityEngine.Vector3(-1f, 0.1f, 1f), 0.8f, 0f);
    sb.Append("lodCount=" + (lod != null ? lod.lodCount : 0) + " ");

    var grass = Group("Grass");
    Place("Assets/BK/PureNature_Redwood/Prefabs/Plants/Grass1.prefab", new UnityEngine.Vector3(0, 0, 0), grass.transform);
    Place("Assets/BK/PureNature_Redwood/Prefabs/Plants/ThinFern1.prefab", new UnityEngine.Vector3(1.2f, 0, 0.5f), grass.transform);
    Place("Assets/BK/PureNature_Redwood/Prefabs/Plants/Clovers1.prefab", new UnityEngine.Vector3(-1f, 0, 0.8f), grass.transform);
    Place("Assets/BK/PureNature_Redwood/Prefabs/Plants/DeadLeaves1.prefab", new UnityEngine.Vector3(0.3f, 0, -1f), grass.transform);
    Shoot("04_grass_patch", grass, new UnityEngine.Vector3(-1f, -0.5f, 1f), 0.9f, 0f);

    cam.backgroundColor = tuning.fogColor;   // flames, smoke and candles read against the night
    var fire = Group("Fire");
    Place("Assets/NatureManufacture Assets/Fire and Smoke Particles/Prefabs/Other/Prefab_Fire_Pit.prefab", UnityEngine.Vector3.zero, fire.transform);
    Shoot("05_fire_flames", fire, new UnityEngine.Vector3(0f, -0.1f, 1f), 0.5f, 3f);

    var smoke = Group("Smoke");
    Place("Assets/NatureManufacture Assets/Fire and Smoke Particles/Prefabs/Other/Prefab_Fire_Smoke_01.prefab", UnityEngine.Vector3.zero, smoke.transform);
    Shoot("06_smoke", smoke, new UnityEngine.Vector3(0f, -0.1f, 1f), 0.6f, 6f);

    var cata = Group("Catacombs");
    Place("Assets/Revolving Pizza Games/Catacombs/Prefabs/Decoration Presets/C_Wall_Torch_1.prefab", UnityEngine.Vector3.zero, cata.transform);
    Place("Assets/Revolving Pizza Games/Catacombs/Prefabs/FX/C_Candle_Fire.prefab", new UnityEngine.Vector3(0.6f, 0.2f, -0.3f), cata.transform);
    Shoot("07_catacomb_flames", cata, new UnityEngine.Vector3(0f, -0.2f, 1f), 1.0f, 2f);
    sb.Append("wrote 7 shots");
}
finally
{
    UnityEngine.Object.DestroyImmediate(rt);
    UnityEngine.Object.DestroyImmediate(tex);
    UnityEngine.SceneManagement.SceneManager.SetActiveScene(previousActive);
    UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
}
return sb.ToString();
