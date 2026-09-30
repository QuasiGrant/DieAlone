// 8.12 (edit mode): builds Vesper's lighting options (Docs/Design/LightingOptions.md sections 1, 2 and 5) as named
// looks the dev panel's TIME row steps through. Each is a copy of LookTuning_DayOne.asset ("D1 now") under
// Assets/Settings/LookOptions with its listed changes, added to LookPreview.looks in Assets/Prefabs/GameSystems.prefab
// with daylight on, after Night, Day one and Day two. Rerunnable: an existing option asset is overwritten from D1 in place
// (CopySerialized keeps its GUID, so the prefab links hold), and its prefab row is reused. Rerun after anything changes
// D1 (the runner's 8.9d recipe owns the D1 numbers), or the options stop being "D1 plus one change".
// "A Tri" is A with Trilight ambient (LookTuning sunsetTrilight, added in 8.12). Returns every row and each option's changes.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const string Folder = "Assets/Settings/LookOptions", DayOnePath = "Assets/Settings/LookTuning_DayOne.asset", PrefabPath = "Assets/Prefabs/GameSystems.prefab";
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
void Sun(LookTuning t, float elev, float bearing, float intensity, string colour) { t.sunElevation = elev; t.sunBearing = bearing; t.sunIntensity = intensity; t.sunColor = Hex(colour); }
void Fog(LookTuning t, string colour, float start, float end) { t.sunsetFogColor = Hex(colour); t.sunsetFogStart = start; t.sunsetFogEnd = end; }
void Tape(LookTuning t, float crush, float wash, float corners) { t.crushBlacks = crush; t.washOut = wash; t.darkCorners = corners; }
void Sky(LookTuning t, string horizon, float glowSize, string glow) { t.skyHorizon = Hex(horizon); t.sunGlowSize = glowSize; t.sunGlowColor = Hex(glow); }
void CandidateA(LookTuning t) { Sun(t, 24f, 205f, 1.25f, "#FFC98A"); t.sunsetAmbient = Hex("#6E6658"); Fog(t, "#9A9A94", 110f, 850f); Tape(t, 0.28f, 0.18f, 0.4f); Sky(t, "#E3A968", 80f, "#C8A070"); }
var options = new (string key, string label, System.Action<LookTuning> change, string what)[] {
    ("V1", "V1 Ambient", t => t.sunsetAmbient = Hex("#6E6658"), "sunsetAmbient #6E6658"),
    ("V2", "V2 Fog colour", t => t.sunsetFogColor = Hex("#9A9A94"), "sunsetFogColor #9A9A94"),
    ("V3", "V3 Fog range", t => { t.sunsetFogStart = 110f; t.sunsetFogEnd = 850f; }, "sunsetFog 110 to 850"),
    ("V4", "V4 Crush", t => t.crushBlacks = 0.28f, "crushBlacks 0.28"),
    ("V5", "V5 Wash", t => t.washOut = 0.18f, "washOut 0.18"),
    ("V6", "V6 Sun height", t => t.sunElevation = 24f, "sunElevation 24"),
    ("V7", "V7 Sun bearing", t => t.sunBearing = 205f, "sunBearing 205"),
    ("V8", "V8 Sun strength", t => t.sunIntensity = 1.25f, "sunIntensity 1.25"),
    ("V9", "V9 Corners", t => t.darkCorners = 0.4f, "darkCorners 0.4"),
    ("V10", "V10 Glow", t => { t.sunGlowSize = 80f; t.sunGlowColor = Hex("#C8A070"); }, "sunGlow 80 #C8A070"),
    ("A", "A Vesper", CandidateA, "candidate A"),
    ("ATri", "A Tri", t => { CandidateA(t); t.sunsetTrilight = true; t.sunsetAmbientSky = Hex("#6A7080"); t.sunsetAmbientGround = Hex("#3A3228"); }, "A with Trilight #6A7080 / #6E6658 / #3A3228"),
    ("B", "B Late gold", t => { Sun(t, 16f, 212f, 1.35f, "#FFB878"); t.sunsetAmbient = Hex("#66584A"); Fog(t, "#A89A86", 90f, 750f); Tape(t, 0.30f, 0.16f, 0.4f); Sky(t, "#E39A58", 60f, "#D09060"); }, "candidate B"),
    ("C", "C Overcast", t => { Sun(t, 30f, 205f, 0.7f, "#E8DCC8"); t.sunsetAmbient = Hex("#6A6C6C"); Fog(t, "#8E9296", 70f, 600f); Tape(t, 0.32f, 0.22f, 0.4f); Sky(t, "#A8A8A4", 200f, "#A8A49C"); }, "candidate C") };

var dayOne = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(DayOnePath);
if (dayOne == null) return "no " + DayOnePath;
if (!UnityEditor.AssetDatabase.IsValidFolder(Folder)) UnityEditor.AssetDatabase.CreateFolder("Assets/Settings", "LookOptions");
var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(PrefabPath);
var preview = prefab != null ? prefab.GetComponentInChildren<LookPreview>(true) : null;
if (preview == null) return "no LookPreview in " + PrefabPath;
var so = new UnityEditor.SerializedObject(preview);
var looks = so.FindProperty("looks");
var sb = new System.Text.StringBuilder();
foreach (var o in options)
{
    string path = Folder + "/LookTuning_Opt_" + o.key + ".asset";
    var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(path);
    if (asset == null) { UnityEditor.AssetDatabase.CopyAsset(DayOnePath, path); asset = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(path); }
    else { UnityEditor.EditorUtility.CopySerialized(dayOne, asset); asset.name = System.IO.Path.GetFileNameWithoutExtension(path); }
    o.change(asset);
    UnityEditor.EditorUtility.SetDirty(asset);
    int row = -1;
    for (int i = 0; i < looks.arraySize; i++) if (looks.GetArrayElementAtIndex(i).FindPropertyRelative("label").stringValue == o.label) row = i;
    if (row < 0) { row = looks.arraySize; looks.InsertArrayElementAtIndex(row); }
    var e = looks.GetArrayElementAtIndex(row);
    e.FindPropertyRelative("label").stringValue = o.label;
    e.FindPropertyRelative("tuning").objectReferenceValue = asset;
    e.FindPropertyRelative("daylight").boolValue = true;
    sb.Append(o.label + ": " + o.what + "\n");
}
if (so.ApplyModifiedPropertiesWithoutUndo()) UnityEditor.PrefabUtility.SavePrefabAsset(prefab);
UnityEditor.AssetDatabase.SaveAssets();
var labels = new System.Collections.Generic.List<string>();
for (int i = 0; i < looks.arraySize; i++) labels.Add(looks.GetArrayElementAtIndex(i).FindPropertyRelative("label").stringValue);
return "rows: " + string.Join(", ", labels) + " | start " + so.FindProperty("startLook").intValue + "\n" + sb;
