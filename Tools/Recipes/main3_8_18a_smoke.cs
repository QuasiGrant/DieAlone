// Main3 task 8.18a, the fire's smoke as soft puffs (RebuildSpecs 4.3; Gate_batch_Vesper.md fix 4, Gate_batch_Marlow.md 8). Run after
// main3_8_18_look.cs in Main3, edit mode; rerunnable on the open scene (it removes and rebuilds Ward/SmokeSheet and
// Ward/StandInFire/SmokeColumns). Was part of main3_8_7_ward.cs: the opaque sheet's front face at x -110 hid every valley fire from the
// ledge at night, and the day-two columns were three stepped cylinders.
// Every piece is a soft-edged puff quad on DieAlone/Smoke (alpha blended, LookTuning smoke globals), facing east, where every eye is.
// Puff textures made here: alpha a noisy round blob; R the sun shade (lighter at the top); B where the fire lights it.
// 1. The day-one sheet (Ward/SmokeSheet/Body, LookVisibility DayOne): rows of puffs streaming west on the east wind, inside 8.7's old
//    envelope (top 60 over x -110 rising to 110 at x -500, level to x -900; z -60 to 400; down to the -40 floor at the front), so its
//    SheetTop_* points (F-1 targets) still bound it. Unlit by the fire (B 0).
// 2. The night sheet (Ward/SmokeSheet/Lid, LookVisibility Night): the same envelope's top, only lidThin m deep at the front thickening
//    to lidThick m by x -500, so the lines from the ledge eye to the valley fires (tops 45) pass under it and the far front's tops (105)
//    pass over it; its underside lit by the fire (B 1 at the bottom).
// 3. Day two (Ward/StandInFire/SmokeColumns, LookVisibility DayTwo): columnZ columns from the floor to columnTop, puffs widening upward,
//    lit from below up to columnLit, merging into a roof of flat puffs at roofY to roofY + roofRoll, its underside faintly lit.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
// the root Ward (DevWarps/Ward, the 8.1 warp, shares the name, so GameObject.Find("Ward") can return the warp)
UnityEngine.GameObject wardGo = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "Ward") wardGo = r;
if (wardGo == null) return "run 8.7 first";
var ward = wardGo.transform; var fire = ward.Find("StandInFire"); if (fire == null) return "no Ward/StandInFire";
var smokeSh = UnityEngine.Shader.Find("DieAlone/Smoke"); if (smokeSh == null) return "DieAlone/Smoke shader not found";
var dayTwoVis = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayTwo.asset"); if (dayTwoVis == null) return "no LookTuning_DayTwo";
const string fireDir = "Assets/Terrain/Main3/Fire", matDir = "Assets/Materials/Blockout";
if (!UnityEditor.AssetDatabase.IsValidFolder(fireDir)) return "no " + fireDir + "; run 8.7 first";
foreach (var old in new[] { ward.Find("SmokeSheet"), fire.Find("SmokeColumns") }) if (old != null) UnityEngine.Object.DestroyImmediate(old.gameObject);
foreach (var g in UnityEditor.AssetDatabase.FindAssets("", new[] { fireDir }))
{
    var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); var n = System.IO.Path.GetFileNameWithoutExtension(p);
    if (n.StartsWith("Column_") || n == "SmokeRoof" || n == "SmokeSheet" || n.StartsWith("Smoke_")) UnityEditor.AssetDatabase.DeleteAsset(p);
}
var rng = new System.Random(81801);
float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
void ShowIn(UnityEngine.GameObject host, LookVisibility.Show show, params UnityEngine.GameObject[] targets)
{
    var vis = host.AddComponent<LookVisibility>(); var so = new UnityEditor.SerializedObject(vis);
    so.FindProperty("show").enumValueIndex = (int)show; so.FindProperty("dayTwo").objectReferenceValue = dayTwoVis;
    var tp = so.FindProperty("targets"); tp.arraySize = targets.Length; for (int i = 0; i < targets.Length; i++) tp.GetArrayElementAtIndex(i).objectReferenceValue = targets[i]; so.ApplyModifiedPropertiesWithoutUndo();
}

// ---- puff textures and materials
const int texSize = 128; const float shadeLow = 0.55f, noiseScale = 3.2f, noiseAmount = 0.45f, roofLitUnder = 0.35f, sheetFog = 0.5f, lidFog = 0f, columnFog = 0.6f, puffAlpha = 0.6f;
UnityEngine.Texture2D PuffTex(string name, System.Func<float, float> fireAt)
{
    var t = new UnityEngine.Texture2D(texSize, texSize, UnityEngine.TextureFormat.RGBA32, true) { name = name, wrapMode = UnityEngine.TextureWrapMode.Clamp };
    for (int y = 0; y < texSize; y++) for (int x = 0; x < texSize; x++)
    {
        float u = (x + 0.5f) / texSize, v = (y + 0.5f) / texSize, du = (u - 0.5f) * 2f, dv = (v - 0.5f) * 2f;
        float n = UnityEngine.Mathf.PerlinNoise(u * noiseScale + 3.1f, v * noiseScale + 7.7f) * 0.65f + UnityEngine.Mathf.PerlinNoise(u * noiseScale * 2.3f + 11f, v * noiseScale * 2.3f + 2f) * 0.35f;
        float d = UnityEngine.Mathf.Sqrt(du * du + dv * dv) + (n - 0.5f) * noiseAmount;
        float a = UnityEngine.Mathf.SmoothStep(1f, 0f, UnityEngine.Mathf.Clamp01(d)); a *= a;
        t.SetPixel(x, y, new UnityEngine.Color(UnityEngine.Mathf.Lerp(shadeLow, 1f, v), 0f, fireAt(v), a));
    }
    t.Apply(true); string path = fireDir + "/" + name + ".asset"; UnityEditor.AssetDatabase.CreateAsset(t, path); return t;
}
var texDark = PuffTex("Smoke_PuffDark", v => 0f);
var texLit = PuffTex("Smoke_PuffLit", v => UnityEngine.Mathf.SmoothStep(1f, 0f, v / 0.7f));
var texRoof = PuffTex("Smoke_PuffRoof", v => roofLitUnder);
UnityEngine.Material Mat(string name, UnityEngine.Texture2D tex, float fog)
{
    string path = matDir + "/" + name + ".mat"; var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(smokeSh); UnityEditor.AssetDatabase.CreateAsset(m, path); }
    m.shader = smokeSh; m.SetTexture("_ShadeTex", tex); m.SetVector("_ShadeChannel", new UnityEngine.Vector4(1f, 0f, 0f, 0f)); m.SetTexture("_MaskTex", tex);
    m.SetFloat("_AlphaMultiplier", 1f); m.SetFloat("_FogAmount", fog); UnityEditor.EditorUtility.SetDirty(m); return m;
}
var matSheet = Mat("Blockout_SmokePuff_Sheet", texDark, sheetFog);
var matLid = Mat("Blockout_SmokePuff_Lid", texLit, lidFog);
var matColLit = Mat("Blockout_SmokePuff_ColumnLit", texLit, columnFog);
var matColDark = Mat("Blockout_SmokePuff_Column", texDark, columnFog);
var matRoof = Mat("Blockout_SmokePuff_Roof", texRoof, columnFog);

// ---- puff meshes: east-facing quads (x fixed, spanning y and z) or flat roof quads; drawn west to east so nearer puffs blend last
var puffs = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<(UnityEngine.Vector3 c, float w, float h, bool flat, float alpha, float shade)>>();
void Puff(string group, UnityEngine.Vector3 c, float w, float h, bool flat = false, float alpha = puffAlpha, float shade = 1f)
{
    if (!puffs.TryGetValue(group, out var l)) puffs[group] = l = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, bool, float, float)>();
    l.Add((c, w, h, flat, alpha, shade));
}
UnityEngine.GameObject Build(string group, UnityEngine.Transform parent, UnityEngine.Material mat)
{
    var l = puffs[group]; l.Sort((a, b) => a.c.x.CompareTo(b.c.x));
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var uvs = new System.Collections.Generic.List<UnityEngine.Vector2>(); var cols = new System.Collections.Generic.List<UnityEngine.Color>(); var tris = new System.Collections.Generic.List<int>(); var ns = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (var p in l)
    {
        int i0 = vs.Count; float hw = p.w * 0.5f, hh = p.h * 0.5f; bool flip = rng.NextDouble() < 0.5;
        if (p.flat) { vs.Add(p.c + V(hh, 0f, -hw)); vs.Add(p.c + V(hh, 0f, hw)); vs.Add(p.c + V(-hh, 0f, -hw)); vs.Add(p.c + V(-hh, 0f, hw)); for (int k = 0; k < 4; k++) ns.Add(UnityEngine.Vector3.down); }
        else { vs.Add(p.c + V(0f, -hh, -hw)); vs.Add(p.c + V(0f, -hh, hw)); vs.Add(p.c + V(0f, hh, -hw)); vs.Add(p.c + V(0f, hh, hw)); for (int k = 0; k < 4; k++) ns.Add(UnityEngine.Vector3.right); }
        float u0 = flip ? 1f : 0f, u1 = 1f - u0;
        uvs.Add(new UnityEngine.Vector2(u0, 0f)); uvs.Add(new UnityEngine.Vector2(u1, 0f)); uvs.Add(new UnityEngine.Vector2(u0, 1f)); uvs.Add(new UnityEngine.Vector2(u1, 1f));
        for (int k = 0; k < 4; k++) cols.Add(new UnityEngine.Color(p.shade, p.shade, p.shade, p.alpha));
        tris.AddRange(new[] { i0, i0 + 2, i0 + 1, i0 + 1, i0 + 2, i0 + 3 });
    }
    var mesh = new UnityEngine.Mesh { name = "Smoke_" + group, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.SetVertices(vs); mesh.SetNormals(ns); mesh.SetUVs(0, uvs); mesh.SetColors(cols); mesh.SetTriangles(tris, 0); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, fireDir + "/Smoke_" + group + ".asset");
    var go = new UnityEngine.GameObject(group); go.transform.SetParent(parent, false); go.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh;
    var mr = go.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = mat; mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false;
    return go;
}

// ---- 1 and 2: the sheet (8.7's envelope; every puff's quad top at or under SheetTop, inside z sheetZ0 to sheetZ1)
const float sheetX0 = -110f, sheetX1 = -500f, sheetXEnd = -900f, sheetZ0 = -60f, sheetZ1 = 400f, sheetTop0 = 60f, sheetTop1 = 110f, floorY = -40f;
const float rowStepNear = 35f, rowStepFar = 80f, puffW0 = 45f, puffW1 = 110f, puffOverlap = 0.55f, bodyDepthFar = 70f, lidThin = 5f, lidThick = 28f;
float L2(float a, float b, float t) => a + (b - a) * t;
float SheetTop(float x) => x >= sheetX1 ? L2(sheetTop0, sheetTop1, (sheetX0 - x) / (sheetX0 - sheetX1)) : sheetTop1;
float Far(float x) => UnityEngine.Mathf.Clamp01((sheetX0 - x) / (sheetX0 - sheetXEnd));
var sheet = new UnityEngine.GameObject("SmokeSheet"); sheet.transform.SetParent(ward, false);
for (float x = sheetX0; x >= sheetXEnd - 0.1f; x -= L2(rowStepNear, rowStepFar, Far(x)))
{
    float top = SheetTop(x), w = L2(puffW0, puffW1, Far(x)), step = w * puffOverlap;
    // day one: the front row reaches the floor, rows behind it fill bodyDepthFar m under the top
    float bodyLow = x == sheetX0 ? floorY : top - bodyDepthFar, lidLow = top - L2(lidThin, lidThick, UnityEngine.Mathf.Clamp01((sheetX0 - x) / (sheetX0 - sheetX1)));
    for (float z = sheetZ0 + w * 0.5f; z <= sheetZ1 - w * 0.5f + 0.1f; z += step * R(0.8f, 1.2f))
    {
        float zc = UnityEngine.Mathf.Clamp(z, sheetZ0 + w * 0.5f, sheetZ1 - w * 0.5f), xc = UnityEngine.Mathf.Min(x + R(-6f, 6f), sheetX0);
        float t = SheetTop(UnityEngine.Mathf.Max(xc, x));   // the quad top never above the envelope over its own x
        for (float hi = t; hi > bodyLow + 1f; )
        {
            float h = UnityEngine.Mathf.Min(w * R(0.55f, 0.8f), hi - bodyLow + w * 0.2f); h = UnityEngine.Mathf.Max(h, 8f);
            Puff("Body", V(xc, hi - h * 0.5f, zc), w * R(0.85f, 1.15f), h, false, puffAlpha * R(0.7f, 1f), R(0.85f, 1.05f));
            hi -= h * puffOverlap;
        }
        float lh = t - lidLow; Puff("Lid", V(xc, t - lh * 0.5f, zc), w * R(0.9f, 1.2f), lh, false, puffAlpha * R(0.6f, 0.9f), R(0.85f, 1.05f));
    }
}
var body = Build("Body", sheet.transform, matSheet); var lid = Build("Lid", sheet.transform, matLid);
foreach (var x in new[] { sheetX0, (sheetX0 + sheetX1) * 0.5f, sheetX1 })
    for (float z = sheetZ0; z <= sheetZ1 + 0.1f; z += 20f) { var mk = new UnityEngine.GameObject("SheetTop_" + x.ToString("F0") + "_" + z.ToString("F0")); mk.transform.SetParent(sheet.transform, false); mk.transform.position = V(x, SheetTop(x), z); }
ShowIn(sheet, LookVisibility.Show.DayOne, body);
ShowIn(sheet, LookVisibility.Show.Night, lid);

// ---- 3: day two, columns and roof
const float columnX = -300f, columnTop = 250f, columnLit = 130f, colW0 = 36f, colW1 = 85f, colRise = 0.45f, colJitter = 0.15f, roofY = 235f, roofRoll = 25f, roofX0 = -520f, roofX1 = -180f, roofZ0 = -60f, roofZ1 = 560f, roofStep = 55f, roofW = 110f;
float[] columnZ = { 40f, 170f, 300f, 430f };
var columns = new UnityEngine.GameObject("SmokeColumns"); columns.transform.SetParent(fire, false);
foreach (var cz in columnZ)
{
    float cx = columnX + R(-10f, 10f);
    for (float y = floorY; y < columnTop; )
    {
        float t = UnityEngine.Mathf.Clamp01((y - floorY) / (columnTop - floorY)), w = L2(colW0, colW1, t) * R(0.85f, 1.15f), h = w * R(0.9f, 1.2f);
        var c = V(cx + R(-8f, 8f), y + h * 0.5f, cz + R(-colJitter, colJitter) * w);
        Puff(y + h * 0.5f < columnLit ? "ColumnsLit" : "Columns", c, w, h, false, puffAlpha * R(0.8f, 1f), R(0.85f, 1.05f));
        y += h * colRise;
    }
}
for (float x = roofX1; x >= roofX0 - 0.1f; x -= roofStep)
    for (float z = roofZ0; z <= roofZ1 + 0.1f; z += roofStep)
        Puff("Roof", V(x + R(-15f, 15f), roofY + roofRoll * UnityEngine.Mathf.PerlinNoise(x / 90f + 1.7f, z / 90f + 5.2f), z + R(-15f, 15f)), roofW * R(0.85f, 1.2f), roofW * R(0.85f, 1.2f), true, puffAlpha * R(0.7f, 1f), R(0.85f, 1.05f));
var colLitGo = Build("ColumnsLit", columns.transform, matColLit); var colGo = Build("Columns", columns.transform, matColDark); var roofGo = Build("Roof", columns.transform, matRoof);
ShowIn(columns, LookVisibility.Show.DayTwo, colLitGo, colGo, roofGo);

UnityEditor.AssetDatabase.SaveAssets();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | sheet puffs: day one " + puffs["Body"].Count + ", night " + puffs["Lid"].Count + " (lid " + lidThin + " to " + lidThick + " m deep) | day two: columns " + (puffs["ColumnsLit"].Count + puffs["Columns"].Count) + " puffs (" + puffs["ColumnsLit"].Count + " lit), roof " + puffs["Roof"].Count + " | SheetTop points " + (sheet.transform.childCount - 2);
