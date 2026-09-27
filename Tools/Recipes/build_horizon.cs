if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("Horizon") != null) return "Horizon already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var sb = new System.Text.StringBuilder();

// ---- Shader checks
foreach (var name in new[] { "DieAlone/SkyGradient", "DieAlone/HorizonGlow" })
{
    var sh = UnityEngine.Shader.Find(name); if (sh == null) return "shader missing: " + name;
    if (UnityEditor.ShaderUtil.ShaderHasError(sh)) { var m = UnityEditor.ShaderUtil.GetShaderMessages(sh); return "shader error in " + name + ": " + (m.Length > 0 ? m[0].message + " line " + m[0].line : "?"); }
}
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");

// ---- Materials
var skyMat = new UnityEngine.Material(UnityEngine.Shader.Find("DieAlone/SkyGradient"));
UnityEditor.AssetDatabase.CreateAsset(skyMat, "Assets/Materials/SkyGradient.mat");
var glowMat = new UnityEngine.Material(UnityEngine.Shader.Find("DieAlone/HorizonGlow"));
UnityEditor.AssetDatabase.CreateAsset(glowMat, "Assets/Materials/HorizonGlow.mat");
var ridgeMat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Lit"));
ridgeMat.SetColor("_BaseColor", new UnityEngine.Color(0.08f, 0.05f, 0.04f)); ridgeMat.SetFloat("_Smoothness", 0f);
UnityEditor.AssetDatabase.CreateAsset(ridgeMat, "Assets/Materials/Ridge.mat");

// ---- Ridge: a jagged line of dark blocks across the valley, well beyond the terrain edge.
var horizon = new UnityEngine.GameObject("Horizon");
var ridge = new UnityEngine.GameObject("Ridge"); ridge.transform.SetParent(horizon.transform, false);
var rng = new System.Random(5);
float ridgeX = -150f;
for (float z = -160f; z <= 560f; z += 28f)
{
    float h = 26f + (float)rng.NextDouble() * 34f, w = 36f + (float)rng.NextDouble() * 30f, d = 40f + (float)rng.NextDouble() * 40f;
    float x = ridgeX + (float)(rng.NextDouble() - 0.5) * 50f;
    var b = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    b.name = "Peak"; b.transform.SetParent(ridge.transform, false);
    b.transform.position = V(x, h * 0.5f - 8f, z + (float)(rng.NextDouble() - 0.5) * 10f);
    b.transform.rotation = UnityEngine.Quaternion.Euler((float)(rng.NextDouble() - 0.5) * 12f, (float)rng.NextDouble() * 360f, (float)(rng.NextDouble() - 0.5) * 16f);
    b.transform.localScale = V(w, h, d);
    b.GetComponent<UnityEngine.Renderer>().sharedMaterial = ridgeMat;
    UnityEngine.Object.DestroyImmediate(b.GetComponent<UnityEngine.Collider>());
}
// Valley floor filler so the gap between the terrain edge and the ridge is not empty.
var floor = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
floor.name = "ValleyFloor"; floor.transform.SetParent(ridge.transform, false);
floor.transform.position = V(-80f, -6f, 200f); floor.transform.localScale = V(180f, 12f, 800f);
floor.GetComponent<UnityEngine.Renderer>().sharedMaterial = ridgeMat; UnityEngine.Object.DestroyImmediate(floor.GetComponent<UnityEngine.Collider>());

// ---- Glow strips: additive quads standing just behind the crest line, facing the cliff.
var glows = new System.Collections.Generic.List<UnityEngine.Renderer>();
var glowRoot = new UnityEngine.GameObject("Glow"); glowRoot.transform.SetParent(horizon.transform, false);
for (float z = -120f; z <= 520f; z += 32f)
{
    var q = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Quad);
    q.name = "GlowStrip"; q.transform.SetParent(glowRoot.transform, false);
    float top = 22f + (float)rng.NextDouble() * 30f;
    q.transform.position = V(ridgeX - 18f, top, z);
    q.transform.rotation = UnityEngine.Quaternion.Euler(0f, -90f, 0f);            // faces +x, toward the player
    q.transform.localScale = V(70f, 40f + (float)rng.NextDouble() * 30f, 1f);
    var r = q.GetComponent<UnityEngine.Renderer>(); r.sharedMaterial = glowMat; r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    UnityEngine.Object.DestroyImmediate(q.GetComponent<UnityEngine.Collider>());
    glows.Add(r);
}
// Low fire patches on the near slopes: smaller, brighter quads.
for (int i = 0; i < 14; i++)
{
    var q = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Quad);
    q.name = "FirePatch"; q.transform.SetParent(glowRoot.transform, false);
    float z = -100f + (float)rng.NextDouble() * 600f;
    q.transform.position = V(ridgeX + 30f + (float)rng.NextDouble() * 20f, 6f + (float)rng.NextDouble() * 14f, z);
    q.transform.rotation = UnityEngine.Quaternion.Euler(0f, -90f, 0f);
    q.transform.localScale = V(14f + (float)rng.NextDouble() * 18f, 8f + (float)rng.NextDouble() * 10f, 1f);
    var r = q.GetComponent<UnityEngine.Renderer>(); r.sharedMaterial = glowMat; r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    UnityEngine.Object.DestroyImmediate(q.GetComponent<UnityEngine.Collider>());
    glows.Add(r);
}

// ---- Smoke and embers from the Campsite pack, scaled up over the ridge.
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games/Campsite" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
    throw new System.Exception("no prefab named " + name);
}
var smoke = new System.Collections.Generic.List<UnityEngine.ParticleSystem>();
var embers = new System.Collections.Generic.List<UnityEngine.ParticleSystem>();
var fxRoot = new UnityEngine.GameObject("FX"); fxRoot.transform.SetParent(horizon.transform, false);
for (int i = 0; i < 6; i++)
{
    float z = -60f + i * 110f;
    var s = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find("FX_Smoke_Thick_Tall"), scene);
    s.name = "Smoke_" + i; s.transform.SetParent(fxRoot.transform, false);
    s.transform.position = V(ridgeX + 10f, 30f + (float)rng.NextDouble() * 10f, z); s.transform.localScale = V(22f, 22f, 22f);
    foreach (var ps in s.GetComponentsInChildren<UnityEngine.ParticleSystem>(true))
    {
        var main = ps.main; main.scalingMode = UnityEngine.ParticleSystemScalingMode.Hierarchy; main.simulationSpeed = 0.35f; main.maxParticles = 400;
        var em = ps.emission; em.rateOverTime = 3f;
        smoke.Add(ps);
    }
    var e = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find("FX_Embers"), scene);
    e.name = "Embers_" + i; e.transform.SetParent(fxRoot.transform, false);
    e.transform.position = V(ridgeX + 25f, 24f, z + 40f); e.transform.localScale = V(30f, 30f, 30f);
    foreach (var ps in e.GetComponentsInChildren<UnityEngine.ParticleSystem>(true))
    {
        var main = ps.main; main.scalingMode = UnityEngine.ParticleSystemScalingMode.Hierarchy; main.simulationSpeed = 0.5f; main.maxParticles = 600;
        var em = ps.emission; em.rateOverTime = 12f;
        embers.Add(ps);
    }
}

// ---- Controller
var hf = horizon.AddComponent<HorizonFire>();
var so = new UnityEditor.SerializedObject(hf);
so.FindProperty("tuning").objectReferenceValue = tuning;
var pg = so.FindProperty("glowStrips"); pg.arraySize = glows.Count; for (int i = 0; i < glows.Count; i++) pg.GetArrayElementAtIndex(i).objectReferenceValue = glows[i];
var psm = so.FindProperty("smoke"); psm.arraySize = smoke.Count; for (int i = 0; i < smoke.Count; i++) psm.GetArrayElementAtIndex(i).objectReferenceValue = smoke[i];
var pe = so.FindProperty("embers"); pe.arraySize = embers.Count; for (int i = 0; i < embers.Count; i++) pe.GetArrayElementAtIndex(i).objectReferenceValue = embers[i];
so.ApplyModifiedPropertiesWithoutUndo();

// ---- Main's environment: sunset fog set, gradient sky.
var env = UnityEngine.Object.FindFirstObjectByType<LookEnvironment>();
var soE = new UnityEditor.SerializedObject(env);
soE.FindProperty("fogSet").enumValueIndex = 1;
soE.FindProperty("skyMaterial").objectReferenceValue = skyMat;
soE.ApplyModifiedPropertiesWithoutUndo();
typeof(LookEnvironment).GetMethod("Apply", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(env, null);
typeof(HorizonFire).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(hf, null);
typeof(HorizonFire).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(hf, null);

// Sun a little lower and warmer for the fire to read.
var sun = UnityEngine.GameObject.Find("NightLighting").transform.Find("Sun");
sun.rotation = UnityEngine.Quaternion.Euler(10f, 255f, 0f);
sun.GetComponent<UnityEngine.Light>().color = new UnityEngine.Color(1f, 0.55f, 0.3f);
UnityEngine.RenderSettings.ambientLight = new UnityEngine.Color(0.50f, 0.34f, 0.28f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " peaks=" + ridge.transform.childCount + " glows=" + glows.Count + " smoke=" + smoke.Count + " embers=" + embers.Count + " fog=" + UnityEngine.RenderSettings.fogStartDistance + ".." + UnityEngine.RenderSettings.fogEndDistance + " clear=" + UnityEngine.Camera.main.clearFlags;
