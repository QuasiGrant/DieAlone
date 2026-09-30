// Main3 task 8.9f: Vesper's fixes to the slice views before the captures (LookSlice.md; her review of the 8.9d working
// shots). Run after 8.9d in Main3, edit mode (the runner runs it before the sightlines).
// 1. Giants: the gray stand-ins stop drawing (trunk colliders and crown proxies stay for the sight checks) and each gets a
//    Redwood pack tree to its height: Sequoia, or a red pine on the knoll (LookSlice 2: no Sequoia on the knoll); the dead
//    Snag and the Gate Tree stub become a dead pack tree (6).
// 2. Pack retints in place (pack materials are git-ignored, so this recipe reapplies them on every rebuild): Redwood leaves
//    and impostor cards to dull olive #4F4A2C, the base colour dividing out each texture's own average.
// 3. The shared night look (LookTuning.asset): the tape filter at the day looks' strength; the scene's white sun off;
//    ambient and a faint warm fire fill from the west from LookTuning (LookEnvironment applies them).
// 4. Ground dressing within 12 m of the S1 and S2 shot cameras (dead leaves, moss, fern, clover, small rocks, fallen wood).
// 5. Day two: falling ash over the clearing (NM Prefab_Fire_Ashes_01, #8A8078), shown only in the day-two look; the
//    stand-in fire shows at night and on day two, never on day one by day (LookVisibility); 8.9j: always on.
// 6. (second review) front zone and Camp 2 textured, the office lit at night (five windows and a porch bulb), every terrain
//    layer textured and palette-tinted, thicket markers hidden, young regrowth, dead snags and branches in the old burn,
//    low cover and small firs over the open ground east of the tower, a tree edge east of the fence, the Snag and Gate Tree
//    stub as a dead pack tree.
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
// crushed blacks and dark corners are the night's own (8.9g brightened day one only; night keeps the 8.9f values)
const float nightCrushBlacks = 0.3f, nightDarkCorners = 0.45f;
look.interiorFillIntensity = 0f;   // the cabin daylight fill (8.9g) is off at night
look.filterEnabled = true; look.lowResHeight = dayOne.lowResHeight; look.colorBleed = dayOne.colorBleed; look.washOut = dayOne.washOut; look.crushBlacks = nightCrushBlacks;
look.grainStrength = dayOne.grainStrength; look.grainSpeed = dayOne.grainSpeed; look.noiseBandStrength = dayOne.noiseBandStrength; look.noiseBandSpeed = dayOne.noiseBandSpeed;
look.noiseBandInterval = dayOne.noiseBandInterval; look.scanLines = dayOne.scanLines; look.blur = dayOne.blur; look.darkCorners = nightDarkCorners;
// smoke at night: no sun on it, so its body sinks toward char and the fire lights its underside (Style.md 2.3: #6B2A12 lit
// underside, near-black sky); by day the day looks keep their own smoke colours
const float nightSmokeBody = 0.3f, nightSmokeFire = 1.6f;
look.smokeBodyColor = UnityEngine.Color.Lerp(Hex("#1E1916"), Hex("#4A3A32"), nightSmokeBody); look.smokeFireStrength = nightSmokeFire;
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
// 8.9j: the fire is always on (DECISIONS 2026-09-29, the land hides it; Valley.md 3), so it has no LookVisibility any more
// the cab's day look keeps its lit windows off; its LookVisibility (8.9d) needs the day-two asset only for day two, so none here


// ---------------- 6. Vesper's second review (8.9f): no bare planes, diagram lines or untextured blue slabs in S4 ----------------
// Palette mixes (Style.md 2): asphalt is char lifted toward granite, gravel is granite, the trail earth sits between the forest
// floor and weathered wood.
var granite = Hex("#6E6660"); var charC = Hex("#1E1916"); var floorC = Hex("#58583A"); var woodC = Hex("#5C4632");
const float asphaltLift = 0.35f;
var asphalt = UnityEngine.Color.Lerp(charC, granite, asphaltLift); var gravel = granite; var trailEarth = UnityEngine.Color.Lerp(floorC, woodC, 0.5f);
var concrete = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Concrete034_1.0x1.0.mat");
var planksM = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
var groundM = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Ground054_25.0x25.0.mat");
if (concrete == null || planksM == null || groundM == null) return "project materials missing";
UnityEngine.Material Tinted(string name, UnityEngine.Material from, UnityEngine.Color target, UnityEngine.Vector2 tiling)
{
    string path = "Assets/Materials/Slice/" + name + ".mat"; var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(from); UnityEditor.AssetDatabase.CreateAsset(m, path); } else m.CopyPropertiesFromMaterial(from);
    var a = AvgRGB(from.GetTexture("_BaseMap")); var tl = target.linear;
    m.SetColor("_BaseColor", new UnityEngine.Color(tl.r / UnityEngine.Mathf.Max(a.r, 0.02f), tl.g / UnityEngine.Mathf.Max(a.g, 0.02f), tl.b / UnityEngine.Mathf.Max(a.b, 0.02f), 1f).gamma);
    m.SetTextureScale("_BaseMap", tiling); m.SetFloat("_Smoothness", 0.1f); UnityEditor.EditorUtility.SetDirty(m); return m;
}
var matAsphalt = Tinted("Slice_Asphalt", concrete, asphalt, new UnityEngine.Vector2(8f, 8f));
var matGravel = Tinted("Slice_Gravel", groundM, gravel, new UnityEngine.Vector2(2f, 8f));
var matSiding = Tinted("Slice_Siding", planksM, woodC, new UnityEngine.Vector2(4f, 1f));
var matRoof = Tinted("Slice_RoofChar", concrete, charC, new UnityEngine.Vector2(4f, 4f));
var matSteel = Tinted("Slice_Steel", concrete, granite, new UnityEngine.Vector2(1f, 4f));
// front zone: surfaces to asphalt and gravel, buildings to siding and roofs, everything else gray to steel (the fire-marker
// lamps on DieAlone/FireStandIn keep theirs)
int fzN = 0;
foreach (var r in Root("FrontZone").GetComponentsInChildren<UnityEngine.MeshRenderer>(true))
{
    if (r.sharedMaterial != null && r.sharedMaterial.shader.name.StartsWith("DieAlone/")) continue;
    string n = r.name; UnityEngine.Material m;
    if (n == "ParkingLot" || n == "Drive" || n == "TurningCircle") m = matAsphalt;
    else if (n.StartsWith("Spur") || n.StartsWith("Loop") || n == "Pitch") m = matGravel;
    else if (n == "Roof") m = matRoof;
    else if (n.StartsWith("Wall") || n == "Floor") m = matSiding;
    else m = matSteel;
    r.sharedMaterial = m; fzN++;
}
// Camp 2 in the S4 view: its gray ramps read as diagram strips; granite stack and rocks to granite, ramps, landings,
// rails and ladder to weathered planks, roofs to char, the barrel to steel (the lamp and the resident spot keep theirs)
var matGranite = Tinted("Slice_Granite", concrete, granite, new UnityEngine.Vector2(2f, 2f)); int c2N = 0;
var camp2 = Root("Campsites") != null ? Root("Campsites").transform.Find("Camp_2") : null; if (camp2 == null) return "Camp_2 missing";   // a DevWarps point shares the name
foreach (var r in camp2.GetComponentsInChildren<UnityEngine.MeshRenderer>(true))
{
    string n = r.name; UnityEngine.Material m;
    if (n == "GraniteStack" || n == "Rock") m = matGranite;
    else if (n.StartsWith("Ramp") || n.StartsWith("Landing") || n == "Post" || n == "Rail" || n == "Rung") m = matSiding;
    else if (n.StartsWith("Roof")) m = matRoof;
    else if (n == "Barrel") m = matSteel;
    else continue;
    r.sharedMaterial = m; c2N++;
}
// the office lit at night: window glow on its west and south faces (toward the tower), and a lamp inside
var office = Root("FrontZone").transform.Find("Office"); var glowWin = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_CabWindowGlow.mat");
var officeGlow = new System.Collections.Generic.List<UnityEngine.GameObject>();
if (office != null && glowWin != null)
{
    const float officeX0 = 344f, officeZ0 = 196f, winY = 1.3f, winH = 1.3f, winW = 2.4f, glowOff = 0.03f, porchBulb = 0.45f, porchY = 2.7f, porchOut = 0.6f;   // map footprint x 344 to 356, z 196 to 204; floor at 3
    float gy = office.position.y;
    // two windows on the west face and three on the south face, the faces the tower sees; unfogged so they read past the night fog end
    var wins = new System.Collections.Generic.List<(UnityEngine.Vector3, UnityEngine.Vector3, float)>();
    foreach (var wz in new[] { 198f, 202f }) wins.Add((V(officeX0 - glowOff, gy + winY + winH * 0.5f, wz), V(winW, winH, 1f), 90f));
    foreach (var wx in new[] { 346.5f, 350f, 353.5f }) wins.Add((V(wx, gy + winY + winH * 0.5f, officeZ0 - glowOff), V(winW, winH, 1f), 0f));
    foreach (var (pos, size, yaw) in wins)
    {
        var q = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Quad); q.name = "OfficeWindowGlow"; q.transform.SetParent(root, false);
        q.transform.position = pos; q.transform.localScale = size; q.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
        UnityEngine.Object.DestroyImmediate(q.GetComponent<UnityEngine.Collider>()); var qr = q.GetComponent<UnityEngine.Renderer>(); qr.sharedMaterial = glowWin; qr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        officeGlow.Add(q);
    }
    // a porch bulb over the south door, unfogged like the cab lamp bulb
    var bulbMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_CabLampBulb.mat");
    if (bulbMat != null)
    {
        var pb = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere); pb.name = "OfficePorchBulb"; pb.transform.SetParent(root, false);
        pb.transform.position = V(350f, gy + porchY, officeZ0 - porchOut); pb.transform.localScale = V(porchBulb, porchBulb, porchBulb);
        UnityEngine.Object.DestroyImmediate(pb.GetComponent<UnityEngine.Collider>()); var pr = pb.GetComponent<UnityEngine.Renderer>(); pr.sharedMaterial = bulbMat; pr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        officeGlow.Add(pb);
    }
    var lampGo = new UnityEngine.GameObject("OfficeLamp"); lampGo.transform.SetParent(office, false); lampGo.transform.position = V(350f, gy + 2.5f, 200f);
    var ol = lampGo.AddComponent<UnityEngine.Light>(); ol.type = UnityEngine.LightType.Point; ol.range = 8f; ol.intensity = look.firePitIntensity; ol.shadows = UnityEngine.LightShadows.None;
    var olp = lampGo.AddComponent<PracticalLight>(); var olo = new UnityEditor.SerializedObject(olp); olo.FindProperty("tuning").objectReferenceValue = look; olo.FindProperty("kind").enumValueIndex = (int)PracticalLight.Kind.Lamp; olo.FindProperty("byDay").enumValueIndex = (int)PracticalLight.ByDay.Off; olo.ApplyModifiedPropertiesWithoutUndo();
    var og = new UnityEngine.GameObject("OfficeNightGlow"); og.transform.SetParent(root, false); Visibility(og, LookVisibility.Show.Night, officeGlow);
}
// the ground reads as ground, not a flat gray plane or bright diagram strips: every gray terrain layer takes the project
// Ground054 texture tinted to the palette (forest floor, granite, burnt earth, worn trail earth, dark lake bed); the gray
// thicket edge markers stop drawing (their invisible walls stay; the look pass puts vegetation on those edges later)
var burnEarth = UnityEngine.Color.Lerp(charC, Hex("#8A8078"), 0.5f); var lakeBed = UnityEngine.Color.Lerp(charC, floorC, 0.5f);
var layerTint = new System.Collections.Generic.Dictionary<string, UnityEngine.Color> { { "Layer_Ground", floorC }, { "Layer_Rock", granite }, { "Layer_Burn", burnEarth }, { "Layer_Trail", trailEarth }, { "Layer_LakeBed", lakeBed } };
var dataT = terrain.terrainData; int trailLayers = 0;
var gtex = groundM.GetTexture("_BaseMap") as UnityEngine.Texture2D; var ga = AvgRGB(gtex);
foreach (var tlayer in dataT.terrainLayers)
{
    if (tlayer == null || !layerTint.TryGetValue(tlayer.name, out var tc)) continue;
    var te = tc.linear; tlayer.diffuseTexture = gtex; tlayer.tileSize = new UnityEngine.Vector2(3f, 3f);
    tlayer.diffuseRemapMax = new UnityEngine.Vector4(UnityEngine.Mathf.LinearToGammaSpace(te.r / UnityEngine.Mathf.Max(ga.r, 0.02f)), UnityEngine.Mathf.LinearToGammaSpace(te.g / UnityEngine.Mathf.Max(ga.g, 0.02f)), UnityEngine.Mathf.LinearToGammaSpace(te.b / UnityEngine.Mathf.Max(ga.b, 0.02f)), 1f);
    UnityEditor.EditorUtility.SetDirty(tlayer); trailLayers++;
}
int markerN = 0; foreach (UnityEngine.Transform t in Root("Thicket").transform) if (t.name.StartsWith("Marker_")) { t.GetComponent<UnityEngine.Renderer>().enabled = false; markerN++; }
// the old burn: dense young regrowth 4 to 6 m (4 m in the last 40 m before the front zone), off the trails (Main3.md 2.10)
var burnPoly = new[] { new UnityEngine.Vector2(185f, 181f), new UnityEngine.Vector2(340f, 213f), new UnityEngine.Vector2(340f, 143f), new UnityEngine.Vector2(185f, 151f) };
bool InBurn(UnityEngine.Vector2 p) { bool c = false; for (int i = 0, j = burnPoly.Length - 1; i < burnPoly.Length; j = i++) if (((burnPoly[i].y > p.y) != (burnPoly[j].y > p.y)) && (p.x < (burnPoly[j].x - burnPoly[i].x) * (p.y - burnPoly[i].y) / (burnPoly[j].y - burnPoly[i].y) + burnPoly[i].x)) c = !c; return c; }
const float regrowthStep = 5f, regrowthJitter = 2.5f, regrowthTrailGap = 3.5f, regrowthLowX = 300f;
// a view lane from the S2 camera on Camp to Jg to the tower foot stays open, so the tower reads base to cab (LookSlice 6)
const float s2LaneHalf = 6f; var s2Cam = new UnityEngine.Vector2(235f, 168f); var s2Tower = new UnityEngine.Vector2(164f, 166f);
bool InS2Lane(UnityEngine.Vector2 p, float extra = 0f) { var ab = s2Tower - s2Cam; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - s2Cam, ab) / ab.sqrMagnitude); return UnityEngine.Vector2.Distance(p, s2Cam + ab * t) < s2LaneHalf + extra; }
var regrowth = new UnityEngine.GameObject("BurnRegrowth").transform; regrowth.SetParent(root, false); int regrowthN = 0;
for (float x = 186f; x < 340f; x += regrowthStep) for (float z = 143f; z < 213f; z += regrowthStep)
{
    var p = new UnityEngine.Vector2(x + R(-regrowthJitter, regrowthJitter), z + R(-regrowthJitter, regrowthJitter));
    if (!InBurn(p) || InS2Lane(p)) continue; bool clear = true; foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < regrowthTrailGap) { clear = false; break; } if (!clear) continue;
    var tree = Spawn(BK + "Trees/" + (rng.NextDouble() < 0.6 ? "RedFir" + (1 + rng.Next(8)) : "RedPine" + (1 + rng.Next(5))), regrowth); if (tree == null) continue;
    tree.transform.position = V(p.x, 0f, p.y); tree.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
    float top = float.MinValue; foreach (var rr in tree.GetComponentsInChildren<UnityEngine.Renderer>()) top = UnityEngine.Mathf.Max(top, rr.bounds.max.y);
    float want = p.x > regrowthLowX ? 4f : R(4f, 6f), sc = want / UnityEngine.Mathf.Max(0.5f, top);
    tree.transform.localScale = V(sc, sc, sc); tree.transform.position = V(p.x, H(p.x, p.y) - 0.1f, p.y); regrowthN++;
}
// the open ground the tower sees east (S4): no bare flat plane. Off the trails and the burn, low growth across the view:
// grass, ferns and dead leaves everywhere, and small firs 2 to 4 m outside the front zone (behind the thicket walls, so
// never in reach). In the front zone only ground cover, kept off its paving and buildings.
const float openX0 = 230f, openX1 = 398f, openZ0 = 100f, openZ1 = 298f, openStep = 4f, openJitter = 1.8f, openTrailGap = 2.5f, fzX = 338f, fzClear = 1f, firShare = 0.3f;
var fzFoot = new System.Collections.Generic.List<UnityEngine.Rect>();
foreach (var r in Root("FrontZone").GetComponentsInChildren<UnityEngine.Renderer>(true)) { var b = r.bounds; fzFoot.Add(new UnityEngine.Rect(b.min.x - fzClear, b.min.z - fzClear, b.size.x + fzClear * 2f, b.size.z + fzClear * 2f)); }
string[] cover = { BK + "Plants/Grass1", BK + "Plants/Grass2", BK + "Plants/Grass3", BK + "Plants/GrassMoss", BK + "Plants/DeadLeaves1", BK + "Plants/DeadLeaves2", BK + "Plants/ThinFern3", BK + "Plants/ThinFern4", SC + "Grass3" };
var open = new UnityEngine.GameObject("OpenGround").transform; open.SetParent(root, false); int openCover = 0, openFirs = 0;
for (float x = openX0; x < openX1; x += openStep) for (float z = openZ0; z < openZ1; z += openStep)
{
    var p = new UnityEngine.Vector2(x + R(-openJitter, openJitter), z + R(-openJitter, openJitter));
    if (InBurn(p)) continue; bool clear = true; foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < openTrailGap) { clear = false; break; } if (!clear) continue;
    foreach (var fr in fzFoot) if (fr.Contains(p)) { clear = false; break; } if (!clear) continue;
    bool fir = p.x < fzX && rng.NextDouble() < firShare;
    var g = Spawn(fir ? BK + "Trees/RedFir" + (1 + rng.Next(8)) : cover[rng.Next(cover.Length)], open); if (g == null) continue;
    g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); g.transform.position = V(p.x, 0f, p.y);
    if (fir) { float top = float.MinValue; foreach (var rr in g.GetComponentsInChildren<UnityEngine.Renderer>()) top = UnityEngine.Mathf.Max(top, rr.bounds.max.y); float sc = R(2f, 4f) / UnityEngine.Mathf.Max(0.5f, top); g.transform.localScale = V(sc, sc, sc); openFirs++; }
    else { float s = R(1.6f, 3f); g.transform.localScale = V(s, s, s); openCover++; }
    float low = float.MaxValue; foreach (var rr in g.GetComponentsInChildren<UnityEngine.Renderer>()) low = UnityEngine.Mathf.Min(low, rr.bounds.min.y);
    g.transform.position = V(p.x, H(p.x, p.y) - low - 0.03f, p.y);
}
// standing dead snags 8 to 16 m and fallen branches through the burn, so it reads as burnt forest from the tower, not a plane
const float snagStep = 14f, snagJitter = 5f, snagLow = 8f, snagHigh = 16f, snagSpread = 6f, branchStep = 6f;   // snagSpread: their limbs reach about this far, kept out of the S2 lane
var deadPrefab = "Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead"; int snagN = 0, branchN = 0;
for (float x = 186f; x < 340f; x += snagStep) for (float z = 143f; z < 213f; z += snagStep)
{
    var p = new UnityEngine.Vector2(x + R(-snagJitter, snagJitter), z + R(-snagJitter, snagJitter));
    if (!InBurn(p) || InS2Lane(p, snagSpread)) continue; bool clear = true; foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < regrowthTrailGap) { clear = false; break; } if (!clear) continue;
    var d = Spawn(deadPrefab, regrowth); if (d == null) continue;
    d.transform.position = V(p.x, 0f, p.y); d.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
    float top = 0f; foreach (var rr in d.GetComponentsInChildren<UnityEngine.Renderer>()) top = UnityEngine.Mathf.Max(top, rr.bounds.max.y);
    float sc = R(snagLow, snagHigh) / UnityEngine.Mathf.Max(0.5f, top); d.transform.localScale = V(sc, sc, sc); d.transform.position = V(p.x, H(p.x, p.y) - 0.2f, p.y); snagN++;
}
for (float x = 186f; x < 340f; x += branchStep) for (float z = 143f; z < 213f; z += branchStep)
{
    var p = new UnityEngine.Vector2(x + R(-2.5f, 2.5f), z + R(-2.5f, 2.5f));
    if (!InBurn(p)) continue; bool clear = true; foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < regrowthTrailGap) { clear = false; break; } if (!clear) continue;
    var b = Spawn(BK + "Plants/Branchs", regrowth); if (b == null) continue; float s = R(1.5f, 3f);
    b.transform.localScale = V(s, s, s); b.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); b.transform.position = V(p.x, H(p.x, p.y), p.y); branchN++;
}
// east of the fence (S4's far ground): real trees 20 to 35 m from the map edge back into the rolling east forest mesh, so its
// front reads as forest, not a flat wall over bare ground; the road's gap at z 170 stays open; no colliders (off-map)
// 8.9j: they stand on the E ridge's front slope (terrain now), below its crest at x 445 (Edges.md 9.5: no trees on crests)
const float eastX0 = 404f, eastX1 = 430f, eastStep = 11f, eastJitter = 4f, eastTreeLow = 20f, eastTreeHigh = 35f, eastRoadZ = 170f, eastRoadGap = 12f;


var eastEdge = new UnityEngine.GameObject("EastEdgeForest").transform; eastEdge.SetParent(root, false); int eastN = 0;
for (float x = eastX0; x <= eastX1; x += eastStep) for (float z = -20f; z <= 320f; z += eastStep)
{
    float px = x + R(-eastJitter, eastJitter), pz = z + R(-eastJitter, eastJitter); if (UnityEngine.Mathf.Abs(pz - eastRoadZ) < eastRoadGap) continue;
    var g = Spawn(BK + "Trees/" + (rng.NextDouble() < 0.6 ? "RedFir" + (1 + rng.Next(8)) : "RedPine" + (1 + rng.Next(5))), eastEdge); if (g == null) continue;
    foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    float eastY = H(px, pz); g.transform.position = V(px, eastY, pz); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
    float hgt = 0f; foreach (var rr in g.GetComponentsInChildren<UnityEngine.Renderer>()) hgt = UnityEngine.Mathf.Max(hgt, rr.bounds.max.y - eastY);
    float s = R(eastTreeLow, eastTreeHigh) / UnityEngine.Mathf.Max(0.5f, hgt); g.transform.localScale = V(s, s, s); eastN++;
}
// the dead Snag and the Gate Tree stub: a dead pack tree stretched to their heights instead of plain wood columns
const float deadTreeGirth = 0.6f;   // the pack dead tree's trunk, about 0.6 m across
int deadN = 0;
foreach (var g in giantList) if (g.name == "Snag" || g.name == "Gate_Tree")
{
    var trunk = g.Find("Trunk"); float tall = trunk.localPosition.y + trunk.localScale.y; float girth = trunk.localScale.x;
    trunk.GetComponent<UnityEngine.Renderer>().enabled = false;
    var d = Spawn("Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead", packTrees); if (d == null) continue;
    d.transform.position = g.position; float h0 = 0f; foreach (var rr in d.GetComponentsInChildren<UnityEngine.Renderer>()) h0 = UnityEngine.Mathf.Max(h0, rr.bounds.max.y - g.position.y);
    float sy = tall / UnityEngine.Mathf.Max(0.5f, h0), sxz = girth / deadTreeGirth; d.transform.localScale = V(sxz, sy, sxz); deadN++;
}
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | giants: " + seq + " Sequoia, " + pines + " red pines on the knoll, snag and stub as wood | retinted " + retinted + " pack materials | night filter on, sun off, fire fill | shot ground " + dressed + " | ash emitters " + ashObjs.Count + " | front zone " + fzN + " retextured, office glow " + officeGlow.Count + ", terrain layers " + trailLayers + ", markers hidden " + markerN + ", camp 2 " + c2N + ", burn regrowth " + regrowthN + ", snags " + snagN + ", branches " + branchN + ", dead trees " + deadN + ", open ground " + openCover + " cover, " + openFirs + " firs, east edge trees " + eastN + " | missing: " + (missing.Count == 0 ? "none" : string.Join(", ", missing));
