// Task 7.6: move every bought-pack material off pack shaders (BK/*, NatureManufacture/*, Legacy Shaders/*)
// onto built-in URP shaders or project shaders, per Docs/Design/ShaderSwap.md. Swaps in place so pack
// prefabs keep working. Pack folders are git-ignored, so rerun this after any pack re-import.
// Only materials still on a pack shader are touched, so a second run changes nothing.
// Pack property values are read before the shader changes. URP keywords and render queue are then
// rebuilt with URP's own ShaderGUI.ValidateMaterial (LitShader, UnlitShader, ParticlesUnlitShader),
// created by reflection because those classes are internal.
// Deviation from ShaderSwap.md 2.2: the BK mask goes in _OcclusionMap only. Putting it in
// _MetallicGlossMap too would let its R channel set metallic, and the rule is metallic 0.
// Palette retint to Style.md is left to Vesper's review; pack tints are copied as they are,
// except ground cover, which is dulled toward grey so it is never grass green.
const float Smoothness = 0.15f;          // ShaderSwap 2.2: at most 0.2, nothing wet or glossy
const float GroundCoverDulling = 0.5f;   // 0 keeps the pack tint, 1 is fully grey
const float SmokeFogAmount = 1f;         // near smoke fogs normally; far columns get their own value later
const float FlameBrightness = 6f;       // linear multiplier on the pack flame hue; pack HDR values (up to 500) are far past the look filter
var PackPrefixes = new[] { "BK/", "NatureManufacture/", "Legacy Shaders/" };

var urpEditor = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Unity.RenderPipelines.Universal.Editor");
UnityEditor.ShaderGUI Gui(string name) => (UnityEditor.ShaderGUI)System.Activator.CreateInstance(urpEditor.GetType("UnityEditor.Rendering.Universal.ShaderGUI." + name), true);
var litGui = Gui("LitShader");
var unlitGui = Gui("UnlitShader");
var particlesUnlitGui = Gui("ParticlesUnlitShader");

UnityEngine.Shader Need(string name)
{
    var s = UnityEngine.Shader.Find(name);
    if (s == null) throw new System.Exception("Shader not found: " + name);
    return s;
}
var lit = Need("Universal Render Pipeline/Lit");
var unlit = Need("Universal Render Pipeline/Unlit");
var particlesUnlit = Need("Universal Render Pipeline/Particles/Unlit");
var smoke = Need("DieAlone/Smoke");
var sky = Need("DieAlone/SkyGradient");
var water = Need("DieAlone/Water");

UnityEngine.Texture Tex(UnityEngine.Material m, string n) => m.HasProperty(n) ? m.GetTexture(n) : null;
UnityEngine.Color Col(UnityEngine.Material m, string n) => m.HasProperty(n) ? m.GetColor(n) : UnityEngine.Color.white;
float Num(UnityEngine.Material m, string n, float fallback) => m.HasProperty(n) ? m.GetFloat(n) : fallback;

void ToLit(UnityEngine.Material m, UnityEngine.Texture baseMap, UnityEngine.Color tint, UnityEngine.Texture normal, float normalScale, UnityEngine.Texture mask, float occlusion, bool cutout, float cutoff)
{
    m.shader = lit;
    m.shaderKeywords = new string[0];                // drop pack keywords; ValidateMaterial rebuilds URP's
    tint.a = 1f;
    m.SetFloat("_WorkflowMode", 1f);                 // metallic workflow
    m.SetFloat("_Surface", 0f);
    m.SetTexture("_BaseMap", baseMap);
    m.SetColor("_BaseColor", tint);
    m.SetTexture("_BumpMap", normal);
    m.SetFloat("_BumpScale", normalScale);
    m.SetTexture("_MetallicGlossMap", null);
    m.SetFloat("_Metallic", 0f);
    m.SetFloat("_Smoothness", Smoothness);
    m.SetFloat("_SmoothnessTextureChannel", 0f);     // metallic alpha; no map, so the slider rules
    m.SetTexture("_OcclusionMap", mask);
    m.SetFloat("_OcclusionStrength", mask != null ? occlusion : 1f);
    m.SetTexture("_EmissionMap", null);
    m.SetColor("_EmissionColor", UnityEngine.Color.black);
    m.SetTexture("_DetailMask", null);               // BK _DetailNormalMap shares URP's name; no moss layer
    m.SetTexture("_DetailAlbedoMap", null);
    m.SetTexture("_DetailNormalMap", null);
    m.SetFloat("_AlphaClip", cutout ? 1f : 0f);
    m.SetFloat("_Cutoff", cutoff);
    m.SetFloat("_Cull", cutout ? 0f : 2f);           // cut-out foliage renders both faces
    m.SetFloat("_QueueControl", 0f);                 // saved-only value URP reads; 0 lets it pick the queue from the surface
    litGui.ValidateMaterial(m);
}

// Pack flame hue scaled to FlameBrightness in linear space. SetColor treats the value as sRGB and
// converts it, so the linear target is stored through LinearToGammaSpace.
UnityEngine.Color FlameTint(UnityEngine.Color emission)
{
    float peak = UnityEngine.Mathf.Max(emission.r, UnityEngine.Mathf.Max(emission.g, emission.b));
    if (peak <= 0f) return UnityEngine.Color.white;
    float G(float c) => UnityEngine.Mathf.LinearToGammaSpace(c / peak * FlameBrightness);
    return new UnityEngine.Color(G(emission.r), G(emission.g), G(emission.b), 1f);
}

void ToParticlesUnlit(UnityEngine.Material m, UnityEngine.Texture baseMap, float blend, UnityEngine.Color tint)
{
    m.shader = particlesUnlit;
    m.shaderKeywords = new string[0];
    m.SetTexture("_BaseMap", baseMap);
    m.SetColor("_BaseColor", tint);
    m.SetTexture("_EmissionMap", null);             // a leftover white _EmissionColor paints the whole quad
    m.SetColor("_EmissionColor", UnityEngine.Color.black);
    m.SetFloat("_Surface", 1f);
    m.SetFloat("_Blend", blend);
    m.SetFloat("_AlphaClip", 0f);
    m.SetFloat("_Cull", 0f);                         // pack flame meshes face either way; draw both sides
    m.SetFloat("_QueueControl", 0f);
    particlesUnlitGui.ValidateMaterial(m);
}

void ToSmoke(UnityEngine.Material m, UnityEngine.Texture shadeTex, UnityEngine.Vector4 channel, UnityEngine.Vector2 shadeScale, UnityEngine.Vector2 shadeOffset, UnityEngine.Texture maskTex, UnityEngine.Vector2 maskScale, UnityEngine.Vector2 maskOffset, float alphaMultiplier)
{
    m.shader = smoke;
    m.shaderKeywords = new string[0];
    m.renderQueue = -1;                              // use the shader's Transparent queue
    m.SetTexture("_ShadeTex", shadeTex);
    m.SetTextureScale("_ShadeTex", shadeScale);
    m.SetTextureOffset("_ShadeTex", shadeOffset);
    m.SetVector("_ShadeChannel", channel);
    m.SetTexture("_MaskTex", maskTex);
    m.SetTextureScale("_MaskTex", maskScale);
    m.SetTextureOffset("_MaskTex", maskOffset);
    m.SetFloat("_AlphaMultiplier", alphaMultiplier);
    m.SetFloat("_FogAmount", SmokeFogAmount);
}

var swapped = new System.Collections.Generic.SortedDictionary<string, int>();
var left = new System.Collections.Generic.List<string>();
foreach (var g in UnityEditor.AssetDatabase.FindAssets("t:Material", new[] { "Assets" }))
{
    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
    if (!path.EndsWith(".mat")) continue;
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null || m.shader == null) continue;
    var from = m.shader.name;
    if (!PackPrefixes.Any(p => from.StartsWith(p))) continue;

    switch (from)
    {
        case "BK/Standard Layered":
            ToLit(m, Tex(m, "_MainTex"), Col(m, "_Color"), Tex(m, "_BumpMap"), Num(m, "_NormalPower", 1f), Tex(m, "_MetallicGlossMap"), Num(m, "_OcclusionPower", 1f), false, 0.5f);
            break;
        case "BK/Vegetation Trunk":
            ToLit(m, Tex(m, "_MainTex"), Col(m, "_Color"), Tex(m, "_BumpMap"), Num(m, "_NormalPower", 1f), Tex(m, "_MetallicROcclusionGSmoothnessA"), Num(m, "_OcclusionPower", 1f), false, 0.5f);
            break;
        case "BK/Vegetation Leaves":
            ToLit(m, Tex(m, "_Diffuse"), Col(m, "_MainColor"), Tex(m, "_Normal"), Num(m, "_NormalPower", 1f), null, 1f, true, Num(m, "_AlphaClippingTreshold", 0.5f));
            break;
        case "BK/Impostor":
            ToLit(m, Tex(m, "_Diffuse"), Col(m, "_MainColor"), Tex(m, "_Normal"), Num(m, "_NormalPower", 1f), Tex(m, "_Mask"), 1f, true, Num(m, "_Cutoff", 0.5f));
            break;
        case "BK/Grass":
        {
            var c = UnityEngine.Color.Lerp(Col(m, "_Color01"), Col(m, "_Color02"), 0.5f);
            float grey = c.grayscale;
            var dulled = UnityEngine.Color.Lerp(c, new UnityEngine.Color(grey, grey, grey), GroundCoverDulling);
            ToLit(m, Tex(m, "_MainTex"), dulled, null, 1f, null, 1f, true, Num(m, "_Cutoff", 0.5f));
            break;
        }
        case "BK/Sky":
            m.shader = sky;
            m.shaderKeywords = new string[0];
            break;
        case "BK/Water":
            m.shader = water;
            m.shaderKeywords = new string[0];
            m.renderQueue = -1;
            break;
        case "BK/Clouds":
        {
            var cloud = Col(m, "_CloudsColor");
            m.shader = unlit;
            m.shaderKeywords = new string[0];
            m.SetTexture("_BaseMap", null);
            m.SetColor("_BaseColor", cloud);
            m.SetFloat("_Surface", 1f);
            m.SetFloat("_Blend", 0f);
            m.SetFloat("_QueueControl", 0f);
            unlitGui.ValidateMaterial(m);
            break;
        }
        case "NatureManufacture/URP/Particles/Fire Unlit":
            ToParticlesUnlit(m, Tex(m, "_Emission_Flipbook"), 2f, FlameTint(Col(m, "_Emission_Color")));   // Additive
            break;
        case "Legacy Shaders/Particles/~Additive-Multiply":
            ToParticlesUnlit(m, Tex(m, "_MainTex"), 2f, UnityEngine.Color.white);            // Additive: the flame textures have no alpha, so legacy Additive-Multiply drew them purely additive and Premultiply would draw an opaque square
            break;
        case "NatureManufacture/URP/Particles/Smoke Lightmap Lit":
        case "NatureManufacture/URP/Particles/Smoke Lightmap Unlit":
        {
            const string shadeName = "_Lightmap_Right_R_Left_G_Top_B_Bottom_A";
            const string maskName = "_Lightmap_Front_R_Back_G_Emission_B_Transparency_A";
            ToSmoke(m, Tex(m, shadeName), new UnityEngine.Vector4(0, 0, 1, 0), m.GetTextureScale(shadeName), m.GetTextureOffset(shadeName),
                Tex(m, maskName), m.GetTextureScale(maskName), m.GetTextureOffset(maskName), Num(m, "_Alpha_Multiplier", 1f));
            break;
        }
        case "NatureManufacture/URP/Particles/Smoke Normalmap Lit":
        case "NatureManufacture/URP/Particles/Smoke Normalmap Unlit":
        {
            const string maskName = "_Color_Mask_R_Emission_B_Transparency_A";
            ToSmoke(m, Tex(m, maskName), new UnityEngine.Vector4(1, 0, 0, 0), m.GetTextureScale(maskName), m.GetTextureOffset(maskName),
                Tex(m, maskName), m.GetTextureScale(maskName), m.GetTextureOffset(maskName), Num(m, "_Alpha_Multiplier", 1f));
            break;
        }
        default:
            left.Add(from + " | " + path);
            continue;
    }
    UnityEditor.EditorUtility.SetDirty(m);
    swapped[from] = swapped.ContainsKey(from) ? swapped[from] + 1 : 1;
}
UnityEditor.AssetDatabase.SaveAssets();

var sb = new System.Text.StringBuilder("swapped: ");
foreach (var kv in swapped) sb.Append(kv.Key + "=" + kv.Value + "; ");
sb.Append("\nno rule for: " + (left.Count == 0 ? "none" : string.Join("\n", left)));
return sb.ToString();
