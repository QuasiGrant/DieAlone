// Task 7.7: the day-one and day-two looks from Docs/Design/Style.md as two new LookTuning assets,
// and the dev-only LookPreview (F2) on the GameSystems prefab listing current, day one, day two.
// Each asset starts as a copy of Assets/Settings/LookTuning.asset (left untouched) and then takes
// the Style.md proposals: palette 2.0 / 2.1, VHS targets section 3, sun 6.0 / 6.1.
// Edit mode only. Safe to rerun: it overwrites the two preview assets and reuses the component.
if (UnityEditor.EditorApplication.isPlaying) return "edit mode only";
const string CurrentPath = "Assets/Settings/LookTuning.asset";
const string DayOnePath = "Assets/Settings/LookTuning_DayOne.asset";
const string DayTwoPath = "Assets/Settings/LookTuning_DayTwo.asset";
const string PrefabPath = "Assets/Prefabs/GameSystems.prefab";
const string SkyPath = "Assets/Materials/SkyGradient.mat";

UnityEngine.Color Hex(string hex)
{
    if (!UnityEngine.ColorUtility.TryParseHtmlString(hex, out var c)) throw new System.Exception("bad colour " + hex);
    return c;
}

void VhsTargets(LookTuning t)   // Style.md section 3, Target column
{
    t.lowResHeight = 360;
    t.colorBleed = 0.5f;
    t.washOut = 0.25f;
    t.crushBlacks = 0.3f;
    t.grainStrength = 0.2f;
    t.grainSpeed = 24f;
    t.noiseBandStrength = 0.35f;
    t.noiseBandInterval = 20f;
    t.scanLines = 0.3f;
    t.blur = 0.3f;
    t.darkCorners = 0.45f;
}

LookTuning Make(string path, System.Action<LookTuning> fill)
{
    var current = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(CurrentPath);
    var copy = UnityEngine.Object.Instantiate(current);
    copy.name = System.IO.Path.GetFileNameWithoutExtension(path);
    fill(copy);
    var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(path);
    if (existing != null)
    {
        UnityEditor.EditorUtility.CopySerialized(copy, existing);
        UnityEngine.Object.DestroyImmediate(copy);
        UnityEditor.EditorUtility.SetDirty(existing);
        return existing;
    }
    UnityEditor.AssetDatabase.CreateAsset(copy, path);
    return copy;
}

var dayOne = Make(DayOnePath, t =>   // Style.md 2.0 and 6.0: ordinary late afternoon, no fire
{
    VhsTargets(t);
    t.skyTop = Hex("#5E6878");
    t.skyHorizon = Hex("#E3A968");
    t.skyGround = Hex("#5A4A3C");
    t.sunGlowColor = Hex("#FFC98A");
    t.sunsetFogColor = Hex("#A8A08E");
    t.sunsetAmbient = Hex("#6E6658");
    t.sunColor = Hex("#FFC98A");
    t.sunElevation = 14f;
    t.sunBearing = 270f;        // west
    t.fireGlowIntensity = 0f;   // no fire glow, smoke or embers before nightfall (8.13)
    t.fireSmoke = 0f;
    t.fireEmbers = 0f;
});

var dayTwo = Make(DayTwoPath, t =>   // Style.md 2.1 and 6.1: the burning sunset
{
    VhsTargets(t);
    t.skyTop = Hex("#381C1A");
    t.skyHorizon = Hex("#D9662E");
    t.skyGround = Hex("#4D291A");
    t.sunGlowColor = Hex("#FF8C40");
    t.fireGlowColor = Hex("#FF6B1A");
    t.sunsetFogColor = Hex("#9E5C38");
    t.sunsetAmbient = Hex("#734D42");
    t.sunColor = Hex("#FF8C40");
    t.sunElevation = 6f;
    t.sunBearing = 270f;        // west, the fire side
    t.smokeBodyColor = Hex("#4A3A32");
    t.smokeFireColor = Hex("#6B2A12");
});
UnityEditor.AssetDatabase.SaveAssets();

var root = UnityEditor.PrefabUtility.LoadPrefabContents(PrefabPath);
try
{
    var preview = root.GetComponent<LookPreview>();
    if (preview == null) preview = root.AddComponent<LookPreview>();
    var so = new UnityEditor.SerializedObject(preview);
    var list = so.FindProperty("looks");
    list.arraySize = 3;
    void Entry(int i, string label, LookTuning tuning, bool daylight)
    {
        var e = list.GetArrayElementAtIndex(i);
        e.FindPropertyRelative("label").stringValue = label;
        e.FindPropertyRelative("tuning").objectReferenceValue = tuning;
        e.FindPropertyRelative("daylight").boolValue = daylight;
    }
    Entry(0, "Current", null, false);
    Entry(1, "Day one", dayOne, true);
    Entry(2, "Day two", dayTwo, true);
    so.FindProperty("skyMaterial").objectReferenceValue = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(SkyPath);
    so.ApplyModifiedPropertiesWithoutUndo();
    UnityEditor.PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
}
finally
{
    UnityEditor.PrefabUtility.UnloadPrefabContents(root);
}
return "day one " + DayOnePath + ", day two " + DayTwoPath + ", LookPreview on " + PrefabPath;
