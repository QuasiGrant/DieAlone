// Task 7.6 one-off: the first run of swap_pack_shaders_7_6.cs kept pack keywords and BK's
// _DetailNormalMap (same name as URP Lit's detail slot), so URP Lit kept _METALLICGLOSSMAP and
// _DETAIL_MULX2 on the swapped BK materials, and the flame materials kept a white _EmissionColor. The recipe now clears them; this cleans materials
// swapped by that first run. Only needed in a clone where the first run happened. Safe to rerun.
// It also touches the six BK materials 7.4 had already put on URP Lit (Sequoia1-5 bark, Checker):
// their metallic map is cleared, so metallic is the slider value 0 (ShaderSwap.md 2.2).
var urpEditor = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Unity.RenderPipelines.Universal.Editor");
UnityEditor.ShaderGUI Gui(string name) => (UnityEditor.ShaderGUI)System.Activator.CreateInstance(urpEditor.GetType("UnityEditor.Rendering.Universal.ShaderGUI." + name), true);
var litGui = Gui("LitShader");
var unlitGui = Gui("UnlitShader");
var particlesUnlitGui = Gui("ParticlesUnlitShader");
var swappedParticles = new[] { "Assets/NatureManufacture Assets/Fire and Smoke Particles/Materials/Others/M_fire_", "Assets/Revolving Pizza Games/Catacombs/Materials/C_Candle_Fire", "Assets/Revolving Pizza Games/Catacombs/Materials/C_Torch_Fire" };
const float FlameBrightness = 6f;   // same value and FlameTint as swap_pack_shaders_7_6.cs
// The pack value is no longer a shader property after the swap, but Unity keeps it in the saved properties.
UnityEngine.Color SavedColor(UnityEngine.Material m, string name)
{
    var colors = new UnityEditor.SerializedObject(m).FindProperty("m_SavedProperties.m_Colors");
    for (int i = 0; i < colors.arraySize; i++)
    {
        var e = colors.GetArrayElementAtIndex(i);
        if (e.FindPropertyRelative("first").stringValue == name) return e.FindPropertyRelative("second").colorValue;
    }
    return UnityEngine.Color.clear;   // missing (Catacombs): FlameTint then gives plain white
}
UnityEngine.Color FlameTint(UnityEngine.Color emission)
{
    float peak = UnityEngine.Mathf.Max(emission.r, UnityEngine.Mathf.Max(emission.g, emission.b));
    if (peak <= 0f) return UnityEngine.Color.white;
    float G(float c) => UnityEngine.Mathf.LinearToGammaSpace(c / peak * FlameBrightness);
    return new UnityEngine.Color(G(emission.r), G(emission.g), G(emission.b), 1f);
}
int lit = 0, unlit = 0, particles = 0;
foreach (var g in UnityEditor.AssetDatabase.FindAssets("t:Material", new[] { "Assets/BK", "Assets/NatureManufacture Assets", "Assets/Revolving Pizza Games/Catacombs" }))
{
    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) continue;
    var s = m.shader.name;
    if (path.StartsWith("Assets/BK/") && s == "Universal Render Pipeline/Lit")
    {
        m.shaderKeywords = new string[0];
        m.SetTexture("_MetallicGlossMap", null);
        m.SetTexture("_DetailMask", null);
        m.SetTexture("_DetailAlbedoMap", null);
        m.SetTexture("_DetailNormalMap", null);
        litGui.ValidateMaterial(m);
        lit++;
    }
    else if (path.StartsWith("Assets/BK/") && s == "Universal Render Pipeline/Unlit")
    {
        m.shaderKeywords = new string[0];
        unlitGui.ValidateMaterial(m);
        unlit++;
    }
    else if (s == "Universal Render Pipeline/Particles/Unlit" && swappedParticles.Any(p => path.StartsWith(p)))
    {
        m.shaderKeywords = new string[0];
        m.SetTexture("_EmissionMap", null);
        m.SetColor("_EmissionColor", UnityEngine.Color.black);
        m.SetColor("_BaseColor", FlameTint(SavedColor(m, "_Emission_Color")));
        m.SetFloat("_Blend", 2f);                   // Additive for all eight (recipe now uses it for Catacombs too)
        m.SetFloat("_Cull", 0f);
        particlesUnlitGui.ValidateMaterial(m);
        particles++;
    }
    else continue;
    UnityEditor.EditorUtility.SetDirty(m);
}
UnityEditor.AssetDatabase.SaveAssets();
return "lit=" + lit + " unlit=" + unlit + " particles=" + particles;
