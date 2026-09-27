var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
if (UnityEngine.GameObject.Find("Night") != null) return "already built";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));

UnityEngine.Texture2D Tex(string id) => UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>("Assets/Textures/" + id + "/" + id + "_Color.jpg");
var dirt = Tex("Ground054"); var metal = Tex("PaintedMetal016"); var concrete = Tex("Concrete034"); var planks = Tex("Planks023A");
if (dirt == null || metal == null || concrete == null || planks == null) return "texture missing";
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Materials")) UnityEditor.AssetDatabase.CreateFolder("Assets", "Materials");
var lit = UnityEngine.Shader.Find("Universal Render Pipeline/Lit");
var cache = new System.Collections.Generic.Dictionary<string, UnityEngine.Material>();
int created = 0, applied = 0;

UnityEngine.Material Mat(UnityEngine.Texture2D tex, float tx, float ty, float smooth)
{
    string key = tex.name + "_" + tx.ToString("0.0") + "x" + ty.ToString("0.0");
    if (cache.TryGetValue(key, out var m)) return m;
    string path = "Assets/Materials/" + key.Replace("_Color", "") + ".mat";
    m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null)
    {
        m = new UnityEngine.Material(lit);
        m.SetTexture("_BaseMap", tex);
        m.SetTextureScale("_BaseMap", new UnityEngine.Vector2(tx, ty));
        m.SetFloat("_Smoothness", smooth);
        UnityEditor.AssetDatabase.CreateAsset(m, path);
        created++;
    }
    cache[key] = m;
    return m;
}
float Snap(float v) => UnityEngine.Mathf.Max(0.5f, UnityEngine.Mathf.Round(v * 2f) * 0.5f);
void Apply(UnityEngine.Transform t, UnityEngine.Texture2D tex, float unit, float smooth, bool recurse)
{
    var rends = recurse ? t.GetComponentsInChildren<UnityEngine.Renderer>(true) : new[] { t.GetComponent<UnityEngine.Renderer>() };
    foreach (var r in rends)
    {
        if (r == null) continue;
        var s = r.transform.lossyScale;
        float sx = UnityEngine.Mathf.Abs(s.x), sy = UnityEngine.Mathf.Abs(s.y), sz = UnityEngine.Mathf.Abs(s.z);
        bool flat = sy < UnityEngine.Mathf.Min(sx, sz);
        float tx = flat ? sx : UnityEngine.Mathf.Max(sx, sz);
        float ty = flat ? sz : sy;
        r.sharedMaterial = Mat(tex, Snap(tx / unit), Snap(ty / unit), smooth);
        applied++;
    }
}
UnityEngine.Transform F(string path) { var go = UnityEngine.GameObject.Find(path); return go != null ? go.transform : null; }

// ---- Ground and scale boxes
Apply(F("Ground"), dirt, 2f, 0.05f, false);
foreach (var n in new[] { "Box_Small_A", "Box_Small_B", "Box_Medium_A", "Box_Medium_B", "Box_Large_A", "Box_Tall_A" }) Apply(F(n), concrete, 2f, 0.1f, false);

// ---- Tower: metal structure, concrete room, plank furniture and door
Apply(F("Tower/Structure"), metal, 1f, 0.35f, true);
Apply(F("Tower/DeckRails"), metal, 1f, 0.35f, true);
Apply(F("Tower/Stairs"), metal, 1f, 0.35f, true);
Apply(F("Tower/TopRoom"), concrete, 2f, 0.1f, true);
Apply(F("Tower/TopRoom/Desk"), planks, 1f, 0.2f, false);
Apply(F("Tower/TopRoom/Bunk"), planks, 1f, 0.2f, false);
Apply(F("Tower/TopRoom/Door"), planks, 1f, 0.2f, true);

// ---- Test course
Apply(F("TestCourse/SprintLane"), concrete, 2f, 0.1f, true);
Apply(F("TestCourse/LowTunnel"), concrete, 2f, 0.1f, true);
Apply(F("TestCourse/LowStep"), concrete, 2f, 0.1f, false);
Apply(F("TestCourse/Log"), planks, 1f, 0.2f, false);
Apply(F("TestCourse/Fence"), planks, 1f, 0.2f, false);
Apply(F("TestCourse/RampAndStairs"), concrete, 2f, 0.1f, true);
Apply(F("TestCourse/TestRoom"), concrete, 2f, 0.1f, true);
Apply(F("TestCourse/TestRoom/Door"), planks, 1f, 0.2f, true);
Apply(F("TestCourse/Table"), planks, 1f, 0.2f, true);
Apply(F("TestCourse/Shelf"), planks, 1f, 0.2f, true);
Apply(F("TestInteractBox"), planks, 1f, 0.2f, false);

// ---- Warm lamp in the tower room
var lamp = new UnityEngine.GameObject("Lamp");
lamp.transform.position = V(0f, 12.3f, 12f);
var pl = lamp.AddComponent<UnityEngine.Light>();
pl.type = UnityEngine.LightType.Point; pl.color = new UnityEngine.Color(1f, 0.78f, 0.5f); pl.intensity = 3f; pl.range = 9f; pl.shadows = UnityEngine.LightShadows.Soft;
var bulb = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
bulb.name = "Bulb"; bulb.transform.SetParent(lamp.transform, false); bulb.transform.localPosition = V(0f, 0.1f, 0f); bulb.transform.localScale = V(0.15f, 0.15f, 0.15f);
UnityEngine.Object.DestroyImmediate(bulb.GetComponent<UnityEngine.Collider>());
var bulbMat = new UnityEngine.Material(lit);
bulbMat.SetColor("_BaseColor", new UnityEngine.Color(1f, 0.9f, 0.7f));
bulbMat.EnableKeyword("_EMISSION"); bulbMat.globalIlluminationFlags = UnityEngine.MaterialGlobalIlluminationFlags.RealtimeEmissive;
bulbMat.SetColor("_EmissionColor", new UnityEngine.Color(1f, 0.78f, 0.5f) * 4f);
UnityEditor.AssetDatabase.CreateAsset(bulbMat, "Assets/Materials/Bulb.mat");
bulb.GetComponent<UnityEngine.Renderer>().sharedMaterial = bulbMat;
lamp.transform.SetParent(F("Tower/TopRoom"), true);

// ---- Night lighting as the default. Daytime light kept, disabled.
var day = UnityEngine.GameObject.Find("Directional Light");
day.SetActive(false);
var night = new UnityEngine.GameObject("Night");
var moonGo = new UnityEngine.GameObject("Moon"); moonGo.transform.SetParent(night.transform, false);
var moon = moonGo.AddComponent<UnityEngine.Light>();
moon.type = UnityEngine.LightType.Directional; moon.color = new UnityEngine.Color(0.55f, 0.65f, 0.9f); moon.intensity = 0.12f; moon.shadows = UnityEngine.LightShadows.Soft;
moonGo.transform.rotation = UnityEngine.Quaternion.Euler(40f, -60f, 0f);
UnityEngine.RenderSettings.sun = moon;
UnityEngine.RenderSettings.skybox = null;
UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
UnityEngine.RenderSettings.ambientLight = new UnityEngine.Color(0.035f, 0.045f, 0.075f);
var cam = UnityEngine.Camera.main;
cam.clearFlags = UnityEngine.CameraClearFlags.SolidColor;
cam.backgroundColor = new UnityEngine.Color(0.01f, 0.015f, 0.03f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " materialsCreated=" + created + " renderersTextured=" + applied + " dayActive=" + day.activeSelf;
