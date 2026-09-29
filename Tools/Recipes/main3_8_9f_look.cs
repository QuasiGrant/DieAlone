// Main3 task 8.9f: Vesper's fixes to the slice views before the captures (LookSlice.md; her review of the 8.9d working
// shots). Run after 8.9d in Main3, edit mode (the runner runs it before the sightlines).
// 1. Giants: the gray stand-ins stop drawing (trunk colliders and crown proxies stay for the sight checks) and each gets a
//    Redwood pack tree to its height: Sequoia, or a red pine on the knoll (LookSlice 2: no Sequoia on the knoll); the dead
//    Snag and the Gate Tree stub keep their trunks, retextured as weathered wood.
// 2. Pack retints in place (pack materials are git-ignored, so this recipe reapplies them on every rebuild): Redwood leaves
//    and impostor cards to dull olive #4F4A2C, the base colour dividing out each texture's own average.
// 3. The shared night look (LookTuning.asset): the tape filter at the day looks' strength; the scene's white sun off;
//    ambient and a faint warm fire fill from the west from LookTuning (LookEnvironment applies them).
// 4. Ground dressing within 10 m of the S1 and S2 shot cameras (dead leaves, moss, fern, clover, small rocks, fallen wood).
// 5. Day two: falling ash over the clearing (NM Prefab_Fire_Ashes_01, #8A8078), shown only in the day-two look; the
//    stand-in fire shows at night and on day two, never on day one by day (LookVisibility).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
var campRoot = Root("Camp"); if (campRoot == null || campRoot.transform.Find("Dressing") == null) return "run 8.9d first";
if (Root("SliceLook") != null) return "SliceLook already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var look = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");
var dayOne = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayOne.asset");
var dayTwo = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayTwo.asset");
if (look == null || dayOne == null || dayTwo == null) return "look assets missing";
var root = new UnityEngine.GameObject("SliceLook").transform;
var missing = new System.Collections.Generic.List<string>();
const string BK = "Assets/BK/PureNature_Redwood/Prefabs/", BKM = "Assets/BK/PureNature_Redwood/Models/Trees/", CS = "Assets/Revolving Pizza Games/Campsite/Prefabs/", SC = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
UnityEngine.GameObject Spawn(string path, UnityEngine.Transform parent)
{
    var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path + ".prefab");
    if (src == null) { if (!missing.Contains(path)) missing.Add(path); return null; }
    var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src, parent);
    foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);   // looks only: the gray proxies keep the collision
    return g;
}
var rng = new System.Random(8906); float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);

// ---------------- 1. giants ----------------
var campC = new UnityEngine.Vector2(170f, 160f); const float knollR = 63f, knollGround = 8f;   // 8.3's knoll rule
var packTrees = new UnityEngine.GameObject("GiantTrees").transform; packTrees.SetParent(root, false);
var weathered = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Wood_1x32.mat");
int seq = 0, pines = 0;
var giantList = new System.Collections.Generic.List<UnityEngine.Transform>();
foreach (UnityEngine.Transform g in Root("Giants").transform) { if (g.Find("Trunk") != null) giantList.Add(g); else foreach (UnityEngine.Transform h in g) if (h.Find("Trunk") != null) giantList.Add(h); }
foreach (var g in giantList)
{
    var trunk = g.Find("Trunk"); float tall = trunk.localPosition.y + trunk.localScale.y;
    foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) r.enabled = false;
    if (g.name == "Snag" || g.name == "Gate_Tree")   // dead snag and stub: no living crown; the trunk draws as weathered wood
    { var tr = trunk.GetComponent<UnityEngine.Renderer>(); tr.enabled = true; if (weathered != null) tr.sharedMaterial = weathered; continue; }
    var p = new UnityEngine.Vector2(g.position.x, g.position.z);
    bool onKnoll = UnityEngine.Vector2.Distance(p, campC) < knollR && H(p.x, p.y) > knollGround;
    var t = Spawn(BK + "Trees/" + (onKnoll ? "RedPine" + (4 + rng.Next(2)) : "Sequoia" + (1 + rng.Next(5))), packTrees); if (t == null) continue;
    t.transform.position = V(p.x, g.position.y, p.y); t.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
    var lod = t.GetComponent<UnityEngine.LODGroup>(); var rs = lod != null ? lod.GetLODs()[0].renderers : t.GetComponentsInChildren<UnityEngine.Renderer>();
    float top = float.MinValue; foreach (var r in rs) if (r != null) top = UnityEngine.Mathf.Max(top, r.bounds.max.y);
    float s = tall / UnityEngine.Mathf.Max(1f, top - g.position.y); t.transform.localScale = V(s, s, s);
    if (onKnoll) pines++; else seq++;
}

// ---------------- 2. pack retints: Redwood leaves and impostor cards to dull olive ----------------
var olive = Hex("#4F4A2C"); int retinted = 0;
UnityEngine.Color AvgRGB(UnityEngine.Texture tex)   // alpha-weighted, linear
{
    var rt = UnityEngine.RenderTexture.GetTemporary(64, 64, 0, UnityEngine.RenderTextureFormat.ARGBFloat, UnityEngine.RenderTextureReadWrite.Linear);
    UnityEngine.Graphics.Blit(tex, rt); var prev = UnityEngine.RenderTexture.active; UnityEngine.RenderTexture.active = rt;
    var t2 = new UnityEngine.Texture2D(64, 64, UnityEngine.TextureFormat.RGBAFloat, false, true); t2.ReadPixels(new UnityEngine.Rect(0, 0, 64, 64), 0, 0); t2.Apply();
    UnityEngine.RenderTexture.active = prev; UnityEngine.RenderTexture.ReleaseTemporary(rt);
    float w = 0f; var sum = UnityEngine.Color.black; foreach (var px in t2.GetPixels()) { sum += px * px.a; w += px.a; } UnityEngine.Object.DestroyImmediate(t2);
    return w > 0f ? sum / w : UnityEngine.Color.gray;
}
foreach (var guid in UnityEditor.AssetDatabase.FindAssets("t:Material", new[] { BKM }))
{
    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid); var name = System.IO.Path.GetFileNameWithoutExtension(path);
    if (!name.EndsWith("Leaves") && !name.EndsWith("_Imp")) continue;
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path); var tex = m.GetTexture("_BaseMap"); if (tex == null) continue;
    var a = AvgRGB(tex); var tl = olive.linear;
    var lin = new UnityEngine.Color(tl.r / UnityEngine.Mathf.Max(a.r, 0.02f), tl.g / UnityEngine.Mathf.Max(a.g, 0.02f), tl.b / UnityEngine.Mathf.Max(a.b, 0.02f), 1f);
    m.SetColor("_BaseColor", lin.gamma); UnityEditor.EditorUtility.SetDirty(m); retinted++;
}

// ---------------- 3. the shared night look ----------------
look.filterEnabled = true; look.lowResHeight = dayOne.lowResHeight; look.colorBleed = dayOne.colorBleed; look.washOut = dayOne.washOut; look.crushBlacks = dayOne.crushBlacks;
look.grainStrength = dayOne.grainStrength; look.grainSpeed = dayOne.grainSpeed; look.noiseBandStrength = dayOne.noiseBandStrength; look.noiseBandSpeed = dayOne.noiseBandSpeed;
look.noiseBandInterval = dayOne.noiseBandInterval; look.scanLines = dayOne.scanLines; look.blur = dayOne.blur; look.darkCorners = dayOne.darkCorners;
UnityEditor.EditorUtility.SetDirty(look);
var nightLighting = Root("NightLighting"); UnityEngine.Light fill = null;
if (nightLighting != null)
{
    var sun = nightLighting.transform.Find("Sun"); if (sun != null) sun.gameObject.SetActive(false);
    var fg = new UnityEngine.GameObject("FireFill"); fg.transform.SetParent(nightLighting.transform, false);
    const float fillElevation = 8f, fillFromBearing = 270f;   // low, from the burning west
    fg.transform.rotation = UnityEngine.Quaternion.Euler(fillElevation, fillFromBearing + 180f, 0f);
    fill = fg.AddComponent<UnityEngine.Light>(); fill.type = UnityEngine.LightType.Directional; fill.shadows = UnityEngine.LightShadows.None; fill.color = look.fireFillColor; fill.intensity = look.fireFillIntensity;
    UnityEngine.RenderSettings.sun = fill;
}
UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat; UnityEngine.RenderSettings.ambientLight = look.nightAmbient;
var env = UnityEngine.Object.FindFirstObjectByType<LookEnvironment>(); if (env != null && fill != null) { var eo = new UnityEditor.SerializedObject(env); eo.FindProperty("fireFill").objectReferenceValue = fill; eo.ApplyModifiedPropertiesWithoutUndo(); }

// ---------------- 4. ground dressing near the shot cameras ----------------
const float dressR = 12f, trailGap = 1.4f; const int dressPerShot = 260;   // Style.md 4.7: no bare ground within 10 m of a shot camera
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>();
foreach (UnityEngine.Transform leg in Root("Trails").transform) foreach (UnityEngine.Transform m in leg) trailPts.Add(new UnityEngine.Vector2(m.position.x, m.position.z));
bool OffTrail(UnityEngine.Vector2 p) { foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < trailGap) return false; return true; }
string[] litter = { BK + "Plants/DeadLeaves1", BK + "Plants/DeadLeaves2", BK + "Plants/GrassMoss", BK + "Plants/GrassMoss", BK + "Plants/DeadLeaves1", SC + "Grass3", BK + "Plants/Clovers2", BK + "Plants/ThinFern1", BK + "Plants/ThinFern2", BK + "Plants/Clovers1", SC + "Grass1", SC + "Grass2", CS + "Rocks and Stones/CS_Stone_1", CS + "Rocks and Stones/CS_Stone_3", CS + "Wood/CS_Log_Firewood" };
var dressing = new UnityEngine.GameObject("ShotGround").transform; dressing.SetParent(root, false); int dressed = 0;
foreach (var cam in new[] { new UnityEngine.Vector2(156f, 148f), new UnityEngine.Vector2(235f, 168f) })
    for (int i = 0, n = 0; i < dressPerShot * 3 && n < dressPerShot; i++)
    {
        float a = R(0f, UnityEngine.Mathf.PI * 2f), r = UnityEngine.Mathf.Sqrt(R(0.01f, 1f)) * dressR; var p = cam + new UnityEngine.Vector2(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * r;
        if (!OffTrail(p)) continue;
        var g = Spawn(litter[rng.Next(litter.Length)], dressing); if (g == null) continue;
        float s = R(1.2f, 2.6f); g.transform.localScale = V(s, s, s); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
        g.transform.position = V(p.x, 0f, p.y); float low = float.MaxValue; foreach (var rr in g.GetComponentsInChildren<UnityEngine.Renderer>()) low = UnityEngine.Mathf.Min(low, rr.bounds.min.y);
        g.transform.position = V(p.x, H(p.x, p.y) - low - 0.03f, p.y); dressed++; n++;
    }

// ---------------- 5. day two ash; the fire by look ----------------
var ash = new UnityEngine.GameObject("DayTwoAsh").transform; ash.SetParent(root, false); var ashCol = Hex("#8A8078");
const float ashHeight = 22f, ashSpread = 14f;   // over the 36 m clearing, falling through the frame
var ashObjs = new System.Collections.Generic.List<UnityEngine.GameObject>();
foreach (var off in new[] { V(0f, 0f, 0f), V(ashSpread, 0f, 0f), V(-ashSpread, 0f, 0f), V(0f, 0f, ashSpread), V(0f, 0f, -ashSpread) })
{
    var a = Spawn("Assets/NatureManufacture Assets/Fire and Smoke Particles/Prefabs/Other/Prefab_Fire_Ashes_01", ash); if (a == null) continue;
    a.transform.position = V(campC.x, H(campC.x, campC.y) + ashHeight, campC.y) + off;
    foreach (var ps in a.GetComponentsInChildren<UnityEngine.ParticleSystem>()) { var main = ps.main; main.startColor = ashCol; UnityEditor.EditorUtility.SetDirty(ps); }
    ashObjs.Add(a);
}
void Visibility(UnityEngine.GameObject host, LookVisibility.Show show, System.Collections.Generic.List<UnityEngine.GameObject> targets)
{
    var v = host.AddComponent<LookVisibility>(); var so = new UnityEditor.SerializedObject(v);
    so.FindProperty("show").enumValueIndex = (int)show; so.FindProperty("dayTwo").objectReferenceValue = dayTwo;
    var tp = so.FindProperty("targets"); tp.arraySize = targets.Count; for (int i = 0; i < targets.Count; i++) tp.GetArrayElementAtIndex(i).objectReferenceValue = targets[i];
    so.ApplyModifiedPropertiesWithoutUndo();
}
Visibility(root.gameObject, LookVisibility.Show.DayTwo, ashObjs);
var fire = Root("Ward").transform.Find("StandInFire"); if (fire == null) return "no StandInFire";
Visibility(Root("Ward"), LookVisibility.Show.NightAndDayTwo, new System.Collections.Generic.List<UnityEngine.GameObject> { fire.gameObject });
// day two by day is far glow and smoke only (Main3.md 5.9): the flames on the ridge and the burning valley show at night
var nightOnly = new System.Collections.Generic.List<UnityEngine.GameObject>(); foreach (var n in new[] { "BurningGiants", "ValleyFires" }) { var c = fire.Find(n); if (c != null) nightOnly.Add(c.gameObject); }
var fireNight = new UnityEngine.GameObject("FireNightOnly"); fireNight.transform.SetParent(root, false); Visibility(fireNight, LookVisibility.Show.Night, nightOnly);
// the cab's day look keeps its lit windows off; its LookVisibility (8.9d) needs the day-two asset only for day two, so none here

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | giants: " + seq + " Sequoia, " + pines + " red pines on the knoll, snag and stub as wood | retinted " + retinted + " pack materials | night filter on, sun off, fire fill | shot ground " + dressed + " | ash emitters " + ashObjs.Count + " | missing: " + (missing.Count == 0 ? "none" : string.Join(", ", missing));
