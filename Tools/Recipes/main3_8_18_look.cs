// Main3 task 8.18, the fire and night look (RebuildSpecs.md 4, Vesper 2026-10-01; Edges.md 6, Style.md 6.2 and 6.3), the look values. Run
// after the 8.17 place recipes in Main3, edit mode (the runner runs it before the day-one start); rerunnable. The fire itself is built by
// main3_8_7_ward.cs. Sets, in the look assets:
// 4.6 night horizon: the night look draws the gradient sky: nightHorizon from the horizon to nightBandDeg, nightTop above; the fog is
//     left as it is (never lightened).
// 4.4 sky glow: westGlowHex on the western horizon over the front's width, gone by westGlowTopDeg up (night), none by day.
// 4.3 smoke: the low sheet and the day-two columns lit from below in smokeFireHex at backdropFire (night), backdropFireDayTwo (day two),
//     0 on day one.
// 4.2 day two: the flame cards at dayTwoFlameDim less than night.
// 4.9 the sun halo: day one dayOneHaloHex, size dayOneHaloSize; day two stays dayTwoHaloHex, dayTwoHaloSize.
// 4.7 the lamp (PlayerTuning): lampIntensity, lampRange.
// 4.8 no blowouts: every pale rock, plank, stone and boulder material (project materials named for them, and the BK rock material) has its
//     mean albedo (texture mean times base colour, linear) held to albedoCapHex's luminance or under.
// 4.10 the chain-link fence: the pack's Metal mid grey (chainHex), low metallic and smoothness, so it is no bright panel under the lamp.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string nightHorizon = "#0C1016", nightTop = "#05080D", westGlowHex = "#5A2412", smokeFireHex = "#6B2A12", dayOneHaloHex = "#E0A848", dayTwoHaloHex = "#FF8C40", albedoCapHex = "#A89C88", chainHex = "#7A7A7A";
const float nightBandDeg = 8f, westGlowSpreadDeg = 77f, westGlowTopDeg = 30f, backdropFire = 1.3f, backdropFireDayTwo = 1f, dayTwoFlameDim = 0.5f, dayOneHaloSize = 40f, dayTwoHaloSize = 24f;
const float lampIntensity = 6f, lampRange = 9f, chainMetallic = 0.2f, chainSmooth = 0.15f;
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var night = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");
var dayOne = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayOne.asset");
var dayTwo = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayTwo.asset");
var player = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var sky = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/SkyGradient.mat");
if (night == null || dayOne == null || dayTwo == null || player == null || sky == null) return "missing: a look asset, PlayerTuning or SkyGradient.mat";
// night
night.nightGradientSky = true; night.gradientSky = sky; night.skyHorizon = Hex(nightHorizon); night.skyTop = Hex(nightTop); night.skyGround = Hex(nightTop); night.skyBandHeight = nightBandDeg;
night.sunGlowColor = UnityEngine.Color.black; night.westGlowColor = Hex(westGlowHex); night.westGlowSpread = westGlowSpreadDeg; night.westGlowTop = westGlowTopDeg;
night.smokeFireColor = Hex(smokeFireHex); night.backdropFireStrength = backdropFire; night.fireCardDim = 0f;
// day one and day two
dayOne.westGlowColor = UnityEngine.Color.black; dayOne.backdropFireStrength = 0f; dayOne.sunGlowColor = Hex(dayOneHaloHex); dayOne.sunGlowSize = dayOneHaloSize; dayOne.nightGradientSky = false;
dayTwo.westGlowColor = UnityEngine.Color.black; dayTwo.smokeFireColor = Hex(smokeFireHex); dayTwo.backdropFireStrength = backdropFireDayTwo; dayTwo.fireCardDim = dayTwoFlameDim; dayTwo.sunGlowColor = Hex(dayTwoHaloHex); dayTwo.sunGlowSize = dayTwoHaloSize; dayTwo.nightGradientSky = false;
foreach (var t in new[] { night, dayOne, dayTwo }) UnityEditor.EditorUtility.SetDirty(t);
// the lamp
player.lampIntensity = lampIntensity; player.lampRange = lampRange; UnityEditor.EditorUtility.SetDirty(player);
// no blowouts: albedo caps
UnityEngine.Color MeanLinear(UnityEngine.Texture tex)
{
    var rt = UnityEngine.RenderTexture.GetTemporary(64, 64, 0, UnityEngine.RenderTextureFormat.ARGBFloat, UnityEngine.RenderTextureReadWrite.Linear);
    UnityEngine.Graphics.Blit(tex, rt); var prev = UnityEngine.RenderTexture.active; UnityEngine.RenderTexture.active = rt;
    var t2 = new UnityEngine.Texture2D(64, 64, UnityEngine.TextureFormat.RGBAFloat, false, true); t2.ReadPixels(new UnityEngine.Rect(0, 0, 64, 64), 0, 0); t2.Apply();
    UnityEngine.RenderTexture.active = prev; UnityEngine.RenderTexture.ReleaseTemporary(rt);
    float w = 0f; var sum = UnityEngine.Color.black; foreach (var px in t2.GetPixels()) { sum += px * px.a; w += px.a; } UnityEngine.Object.DestroyImmediate(t2);
    return w > 0f ? sum / w : UnityEngine.Color.white;
}
float Lum(UnityEngine.Color c) => 0.2126f * c.r + 0.7152f * c.g + 0.0722f * c.b;
float capLum = Lum(Hex(albedoCapHex).linear); int capped = 0, checkedN = 0; var cappedNames = new System.Collections.Generic.List<string>();
var pale = new System.Text.RegularExpressions.Regex("Rock|Granite|Plank|Wood|Board|Stone|Ledge|Tread|Stack|Boulder|Slab|Roof|Canvas|Step");
var paths = new System.Collections.Generic.List<string>();
foreach (var g in UnityEditor.AssetDatabase.FindAssets("t:Material", new[] { "Assets/Materials" })) { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (pale.IsMatch(System.IO.Path.GetFileNameWithoutExtension(p))) paths.Add(p); }
paths.Add("Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Materials/Rocks.mat");
foreach (var p in paths)
{
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(p); if (m == null) continue;
    string prop = m.HasProperty("_BaseColor") ? "_BaseColor" : m.HasProperty("_Color") ? "_Color" : null; if (prop == null) continue;
    var tex = m.HasProperty("_BaseMap") ? m.GetTexture("_BaseMap") : m.HasProperty("_MainTex") ? m.GetTexture("_MainTex") : null;
    var tint = m.GetColor(prop); var mean = tex != null ? MeanLinear(tex) : UnityEngine.Color.white; checkedN++;
    float lum = Lum(new UnityEngine.Color(mean.r * tint.r, mean.g * tint.g, mean.b * tint.b));
    if (lum <= capLum) continue;
    float k = capLum / lum; var c2 = new UnityEngine.Color(tint.r * k, tint.g * k, tint.b * k, tint.a);
    m.SetColor(prop, c2); if (prop == "_BaseColor" && m.HasProperty("_Color")) m.SetColor("_Color", c2); UnityEditor.EditorUtility.SetDirty(m); capped++; cappedNames.Add(m.name);
}
// the chain-link fence
var chain = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Modular Chain Link Fence/Materials/Metal.mat"); bool chainSet = false;
if (chain != null) { foreach (var prop in new[] { "_BaseColor", "_Color" }) if (chain.HasProperty(prop)) chain.SetColor(prop, Hex(chainHex)); if (chain.HasProperty("_Metallic")) chain.SetFloat("_Metallic", chainMetallic); if (chain.HasProperty("_Smoothness")) chain.SetFloat("_Smoothness", chainSmooth); UnityEditor.EditorUtility.SetDirty(chain); chainSet = true; }
UnityEditor.AssetDatabase.SaveAssets();
return "night sky " + nightHorizon + " to " + nightBandDeg + " degrees, " + nightTop + " above, west glow " + westGlowHex + " | smoke underside " + smokeFireHex + " night " + backdropFire + ", day two " + backdropFireDayTwo + " | day two flames dim " + dayTwoFlameDim + " | halo day one " + dayOneHaloHex + " " + dayOneHaloSize + ", day two " + dayTwoHaloHex + " " + dayTwoHaloSize
    + " | lamp " + lampIntensity + " range " + lampRange + " | albedo cap " + albedoCapHex + ": " + capped + " of " + checkedN + " pale materials lowered (" + string.Join(", ", cappedNames) + ") | chain-link " + (chainSet ? chainHex : "MISSING");
