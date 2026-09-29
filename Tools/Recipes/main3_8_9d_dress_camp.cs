// Main3 task 8.9d: dressed slice of the keeper's camp and the tower (Docs/Design/LookSlice.md, Vesper). Run after 8.8 in
// Main3, edit mode; the runner then reruns the sightlines on the dressed camp. Everything goes under Camp/Dressing, plus
// in-place changes to the 8.2 gray pieces: tower renderers get project wood and rust, the cab roof overhang grows to
// 0.8 m, the gray cabin walls, floor, roof, furniture and fire pit stones stop drawing (their colliders stay: they are the
// cabin's collision and the player's spawn), and the camp gets a terrain layer of its own.
// Pack prefabs on their own (swapped, URP) materials; project materials only on primitives (PACK MATERIALS ON PLAIN
// GEOMETRY). New project materials live in Assets/Materials/Slice. Practical lights carry PracticalLight (LookTuning).
// The day one and day two preview looks (LookTuning_DayOne, _DayTwo) take LookSlice 4 and 5 sun, ambient, sky and fog.
// Substitutions and gaps are returned in the report and logged in Docs/Design/Main3_BuildNotes.md.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Cave") == null) return "run 8.8 first";
var campRoot = Root("Camp"); if (campRoot == null) return "no Camp";
if (campRoot.transform.Find("Dressing") != null) return "Camp is already dressed; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var look = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (look == null) return "no LookTuning";
var report = new System.Text.StringBuilder(); var missing = new System.Collections.Generic.List<string>();

var T = campRoot.transform.Find("Tower"); var C = campRoot.transform.Find("Cabin"); var pitT = campRoot.transform.Find("FirePit");
var genT = campRoot.transform.Find("Generator"); if (T == null || C == null || pitT == null || genT == null) return "8.2 camp pieces missing";
var dress = new UnityEngine.GameObject("Dressing").transform; dress.SetParent(campRoot.transform, false);
UnityEngine.Transform Group(string name, UnityEngine.Transform parent, UnityEngine.Vector3 localPos) { var g = new UnityEngine.GameObject(name).transform; g.SetParent(parent, false); g.localPosition = localPos; return g; }

// ---------------- materials (project textures, LookSlice 3.2 retints) ----------------
const string SliceDir = "Assets/Materials/Slice";
if (!UnityEditor.AssetDatabase.IsValidFolder(SliceDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Materials", "Slice");
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
// rust and char go on the plain Concrete034 texture: PaintedMetal006 is green paint, which a rust retint turns into red and black stripes
var metal = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Concrete034_1.0x1.0.mat");
if (planks == null || metal == null) return "project Planks023A or Concrete034 material missing";
var wood = Hex("#5C4632"); var rust = Hex("#8B4A2B"); var charC = Hex("#1E1916");
const float MatSmoothness = 0.15f;   // ShaderSwap 2.2: at most 0.2, nothing wet or glossy
// a retint is the colour the surface should average to: the base colour divides out the texture's own average colour
// (linear), so the planks and metal read #5C4632 and #8B4A2B, not the texture times the tint
var avgCache = new System.Collections.Generic.Dictionary<UnityEngine.Texture, UnityEngine.Color>();
UnityEngine.Color TexAverage(UnityEngine.Texture tex)
{
    if (avgCache.TryGetValue(tex, out var have)) return have;
    var rt = UnityEngine.RenderTexture.GetTemporary(64, 64, 0, UnityEngine.RenderTextureFormat.ARGBFloat, UnityEngine.RenderTextureReadWrite.Linear);
    UnityEngine.Graphics.Blit(tex, rt); var prev = UnityEngine.RenderTexture.active; UnityEngine.RenderTexture.active = rt;
    var t2 = new UnityEngine.Texture2D(64, 64, UnityEngine.TextureFormat.RGBAFloat, false, true); t2.ReadPixels(new UnityEngine.Rect(0, 0, 64, 64), 0, 0); t2.Apply();
    UnityEngine.RenderTexture.active = prev; UnityEngine.RenderTexture.ReleaseTemporary(rt);
    var sum = UnityEngine.Color.black; foreach (var px in t2.GetPixels()) sum += px; UnityEngine.Object.DestroyImmediate(t2);
    var avg = sum / 4096f; avgCache[tex] = avg; return avg;
}
UnityEngine.Color Compensate(UnityEngine.Material from, UnityEngine.Color target)
{
    var tex = from.GetTexture("_BaseMap"); if (tex == null) return target;
    var a = TexAverage(tex); var tl = target.linear;
    var lin = new UnityEngine.Color(tl.r / UnityEngine.Mathf.Max(a.r, 0.02f), tl.g / UnityEngine.Mathf.Max(a.g, 0.02f), tl.b / UnityEngine.Mathf.Max(a.b, 0.02f), 1f);
    return lin.gamma;
}
var matCache = new System.Collections.Generic.Dictionary<string, UnityEngine.Material>();
UnityEngine.Material SliceMat(string name, UnityEngine.Material from, UnityEngine.Color tint, UnityEngine.Vector2 tiling)
{
    if (matCache.TryGetValue(name, out var cached)) return cached;
    string path = SliceDir + "/" + name + ".mat";
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(from); UnityEditor.AssetDatabase.CreateAsset(m, path); } else m.CopyPropertiesFromMaterial(from);
    m.SetColor("_BaseColor", Compensate(from, tint)); m.SetTextureScale("_BaseMap", tiling); m.SetFloat("_Smoothness", MatSmoothness); UnityEditor.EditorUtility.SetDirty(m);
    matCache[name] = m; return m;
}
// planks tile about once a metre on each face of a box: the two longest sides of the box set the tiling, rounded to steps
int Step(float m) { foreach (var s in new[] { 1, 2, 4, 8, 16, 32, 48 }) if (m <= s * 1.4f) return s; return 48; }
UnityEngine.Material WoodFor(UnityEngine.Vector3 size)
{
    var d = new[] { size.x, size.y, size.z }; System.Array.Sort(d);
    int u = Step(d[1]), v = Step(d[2]); return SliceMat("Slice_Wood_" + u + "x" + v, planks, wood, new UnityEngine.Vector2(u, v));
}
// the tower's rust reads as clean red paint in the flat blockout light (Vesper, 8.9f): weathered a third of the way to char
const float rustWeathering = 0.35f;
var rustMat = SliceMat("Slice_Rust", metal, UnityEngine.Color.Lerp(rust, charC, rustWeathering), UnityEngine.Vector2.one);
var charMat = SliceMat("Slice_Char", metal, charC, UnityEngine.Vector2.one);
// cab glazing (LookSlice 2): URP Lit transparent, #5E6878 at 12 percent, smoothness 0.3; URP's own Lit GUI sets the keywords
var urpEditor = System.Linq.Enumerable.First(System.AppDomain.CurrentDomain.GetAssemblies(), a => a.GetName().Name == "Unity.RenderPipelines.Universal.Editor");
var litGui = (UnityEditor.ShaderGUI)System.Activator.CreateInstance(urpEditor.GetType("UnityEditor.Rendering.Universal.ShaderGUI.LitShader"), true);
UnityEngine.Material glass = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(SliceDir + "/Slice_Glass.mat");
if (glass == null) { glass = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Lit")); UnityEditor.AssetDatabase.CreateAsset(glass, SliceDir + "/Slice_Glass.mat"); }
var gc = Hex("#5E6878"); gc.a = 0.12f;
glass.SetFloat("_Surface", 1f); glass.SetFloat("_Blend", 0f); glass.SetColor("_BaseColor", gc); glass.SetFloat("_Smoothness", 0.3f); glass.SetFloat("_Metallic", 0f);
glass.shaderKeywords = new string[0]; litGui.ValidateMaterial(glass); UnityEditor.EditorUtility.SetDirty(glass);
// cab lamp bulb: DieAlone/FireStandIn (unlit, no fog) so the lamp still marks the tower past the night fog end
var standIn = UnityEngine.Shader.Find("DieAlone/FireStandIn"); if (standIn == null) return "DieAlone/FireStandIn shader not found";
var bulb = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(SliceDir + "/Slice_CabLampBulb.mat");
if (bulb == null) { bulb = new UnityEngine.Material(standIn); UnityEditor.AssetDatabase.CreateAsset(bulb, SliceDir + "/Slice_CabLampBulb.mat"); }
bulb.shader = standIn; bulb.SetColor("_Color", look.practicalColor); bulb.SetFloat("_Intensity", look.cabLampBulbIntensity); UnityEditor.EditorUtility.SetDirty(bulb);
var flames = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Flames_Additive.mat");

// ---------------- helpers ----------------
const string CI = "Assets/Revolving Pizza Games/Cabin In The Woods/Prefabs/", CS = "Assets/Revolving Pizza Games/Campsite/Prefabs/";
const string FT = "Assets/PSX Farm Tools Pack/Prefabs/", SP = "Assets/PSX Supplies Pack/Prefabs/", BK = "Assets/BK/PureNature_Redwood/Prefabs/";
const string SC = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
int props = 0;
UnityEngine.Bounds LocalBounds(UnityEngine.GameObject g)   // mesh renderers only, in g's local space
{
    bool any = false; var b = new UnityEngine.Bounds();
    foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>())
    {
        if (!(r is UnityEngine.MeshRenderer) && !(r is UnityEngine.SkinnedMeshRenderer)) continue;
        var wb = r.bounds;
        for (int i = 0; i < 8; i++)
        {
            var c = g.transform.InverseTransformPoint(V((i & 1) == 0 ? wb.min.x : wb.max.x, (i & 2) == 0 ? wb.min.y : wb.max.y, (i & 4) == 0 ? wb.min.z : wb.max.z));
            if (!any) { b = new UnityEngine.Bounds(c, UnityEngine.Vector3.zero); any = true; } else b.Encapsulate(c);
        }
    }
    return b;
}
void Fit(UnityEngine.GameObject g)   // a fitted box where the player can touch it; pack prefabs ship without colliders
{
    if (g.GetComponentInChildren<UnityEngine.Collider>() != null) return;
    var b = LocalBounds(g); var bc = g.AddComponent<UnityEngine.BoxCollider>(); bc.center = b.center; bc.size = b.size;
}
UnityEngine.GameObject Spawn(string path, UnityEngine.Transform parent)
{
    var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path + ".prefab");
    if (src == null) { if (!missing.Contains(path)) missing.Add(path); return null; }
    props++; return (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src, parent);
}
// inside a parent: its lowest mesh point sits on the surface at local height y
UnityEngine.GameObject On(string path, UnityEngine.Transform parent, float x, float y, float z, float yaw, UnityEngine.Vector3 scale, bool collide, UnityEngine.Vector3? tilt = null)
{
    var g = Spawn(path, parent); if (g == null) return null;
    g.transform.localPosition = V(x, y, z); g.transform.localRotation = UnityEngine.Quaternion.Euler(tilt.HasValue ? tilt.Value.x : 0f, yaw, tilt.HasValue ? tilt.Value.z : 0f); g.transform.localScale = scale;
    float low = float.MaxValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) if (r is UnityEngine.MeshRenderer) low = UnityEngine.Mathf.Min(low, r.bounds.min.y);
    float want = parent.TransformPoint(V(x, y, z)).y; if (low < float.MaxValue) g.transform.position += V(0f, want - low, 0f);
    if (collide) Fit(g); return g;
}
// on the terrain at world (x, z): the lowest mesh point on the lowest ground under the footprint, so nothing floats
UnityEngine.GameObject Ground(string path, UnityEngine.Transform parent, float x, float z, float yaw, UnityEngine.Vector3 scale, bool collide, float sink = 0.02f)
{
    var g = Spawn(path, parent); if (g == null) return null;
    g.transform.position = V(x, 0f, z); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); g.transform.localScale = scale;
    float low = float.MaxValue, minX = x, maxX = x, minZ = z, maxZ = z;
    foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) if (r is UnityEngine.MeshRenderer) { low = UnityEngine.Mathf.Min(low, r.bounds.min.y); minX = UnityEngine.Mathf.Min(minX, r.bounds.min.x); maxX = UnityEngine.Mathf.Max(maxX, r.bounds.max.x); minZ = UnityEngine.Mathf.Min(minZ, r.bounds.min.z); maxZ = UnityEngine.Mathf.Max(maxZ, r.bounds.max.z); }
    float gy = UnityEngine.Mathf.Min(UnityEngine.Mathf.Min(H(minX, minZ), H(maxX, maxZ)), UnityEngine.Mathf.Min(UnityEngine.Mathf.Min(H(minX, maxZ), H(maxX, minZ)), H(x, z)));
    if (low < float.MaxValue) g.transform.position = V(x, gy - low - sink, z);
    if (collide) Fit(g); return g;
}
// project-material boxes (roofs, glazing, vane), colliderless unless asked
UnityEngine.GameObject Slab(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Material mat, UnityEngine.Vector3 euler, bool collide = false)
{
    var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); g.name = name; g.transform.SetParent(parent, false);
    g.transform.localPosition = pos; g.transform.localScale = size; g.transform.localRotation = UnityEngine.Quaternion.Euler(euler);
    if (!collide) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());
    g.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat != null ? mat : WoodFor(size); return g;
}
// practical light (LookSlice 4): #FFA860 through PracticalLight, no shadows, brightness relative to the fire pit
UnityEngine.Light Practical(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, float range, PracticalLight.Kind kind, PracticalLight.ByDay byDay)
{
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(parent, false); g.transform.localPosition = pos;
    var l = g.AddComponent<UnityEngine.Light>(); l.type = UnityEngine.LightType.Point; l.range = range; l.intensity = look.firePitIntensity; l.shadows = UnityEngine.LightShadows.None; l.color = look.practicalColor;
    var pl = g.AddComponent<PracticalLight>(); var so = new UnityEditor.SerializedObject(pl);
    so.FindProperty("tuning").objectReferenceValue = look; so.FindProperty("kind").enumValueIndex = (int)kind; so.FindProperty("byDay").enumValueIndex = (int)byDay; so.ApplyModifiedPropertiesWithoutUndo();
    return l;
}
void Hide(UnityEngine.Transform t) { if (t == null) return; foreach (var r in t.GetComponentsInChildren<UnityEngine.Renderer>()) r.enabled = false; }

// ================= TOWER (LookSlice 2: timber, rust only on rails) =================
int woodN = 0, rustN = 0;
foreach (var r in T.GetComponentsInChildren<UnityEngine.MeshRenderer>(true))
{
    bool isRail = r.name.Contains("Rail"); r.sharedMaterial = isRail ? rustMat : WoodFor(r.transform.lossyScale); if (isRail) rustN++; else woodN++;
}
// deck rails draw as open rails (8.9f): the 8.2 panels keep their collision but stop drawing; a top rail, a mid rail and posts
// every railPostGap in rust stand on their lines, so the lit cab windows show between the bars from below at night
const float railPostGap = 1.5f, railBar = 0.07f;
var deckRails = T.Find("DeckRails"); int railBars = 0;
if (deckRails != null)
    foreach (var panel in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(deckRails)))   // a snapshot: the bars are added under the same parent
    {
        var pr = panel.GetComponent<UnityEngine.Renderer>(); if (pr == null) continue; pr.enabled = false;
        var s = panel.localScale; bool alongX = s.x >= s.z; float len = alongX ? s.x : s.z, top = s.y * 0.5f;
        UnityEngine.Vector3 Axis(float t) => alongX ? V(t, 0f, 0f) : V(0f, 0f, t);
        UnityEngine.Vector3 Bar(float l) => alongX ? V(l, railBar, railBar) : V(railBar, railBar, l);
        Slab(panel.name + "_Top", deckRails, panel.localPosition + V(0f, top - railBar * 0.5f, 0f), Bar(len), rustMat, UnityEngine.Vector3.zero);
        Slab(panel.name + "_Mid", deckRails, panel.localPosition, Bar(len), rustMat, UnityEngine.Vector3.zero);
        int posts = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(len / railPostGap));
        for (int k = 0; k <= posts; k++) Slab(panel.name + "_Post", deckRails, panel.localPosition + Axis(-len * 0.5f + len * k / posts), V(railBar, s.y, railBar), rustMat, UnityEngine.Vector3.zero);
        railBars += 2 + posts + 1;
    }
var cab = T.Find("Cab"); if (cab == null) return "no Cab";
const float cabW = 4.4f, cabWallT = 0.2f, sill = 1.0f, winTop = 2.1f, cabWallH = 2.5f, roofOver = 0.8f;   // 8.2 cab, LookSlice roof overhang
float ch = cabW * 0.5f - cabWallT * 0.5f;
var roof = cab.Find("Roof"); roof.localScale = V(cabW + roofOver * 2f, roof.localScale.y, cabW + roofOver * 2f); roof.GetComponent<UnityEngine.Renderer>().sharedMaterial = WoodFor(roof.localScale);
var cabDress = Group("CabDressing", cab, V(0f, 0f, 0f));
// glazing in every window band, mullions at thirds; no colliders (the sightline rays and the deck grid look through)
float paneH = winTop - sill, paneY = (sill + winTop) * 0.5f;
foreach (var side in new[] { "N", "E", "W" })
{
    bool ns = side == "N"; float sgn = side == "W" ? -1f : 1f; float len = cabW - 0.6f;
    var c = ns ? V(0f, paneY, ch) : V(sgn * ch, paneY, 0f);
    Slab("Glass_" + side, cabDress, c, ns ? V(len, paneH, 0.02f) : V(0.02f, paneH, len), glass, UnityEngine.Vector3.zero);
    foreach (var f in new[] { -1f / 6f, 1f / 6f }) Slab("Mullion_" + side, cabDress, c + (ns ? V(f * len * 2f, 0f, 0f) : V(0f, 0f, f * len * 2f)), ns ? V(0.06f, paneH, 0.08f) : V(0.08f, paneH, 0.06f), null, UnityEngine.Vector3.zero);
}
Slab("Glass_S", cabDress, V(0.65f, paneY, -ch), V(2.5f, paneH, 0.02f), glass, UnityEngine.Vector3.zero);   // south window between the door and the corner
Slab("Mullion_S", cabDress, V(0.65f, paneY, -ch), V(0.06f, paneH, 0.08f), null, UnityEngine.Vector3.zero);
// lit windows at night (8.9e re-walk, Wren): one-sided quads just outside the glass on DieAlone/FireStandIn, facing out, so
// from the Ward pass the tower reads as a small warm light past the night fog; from inside the cab they are back faces and
// do not draw; LookVisibility (on the cab) switches them off in the daylight looks
var glowMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(SliceDir + "/Slice_CabWindowGlow.mat");
if (glowMat == null) { glowMat = new UnityEngine.Material(standIn); UnityEditor.AssetDatabase.CreateAsset(glowMat, SliceDir + "/Slice_CabWindowGlow.mat"); }
glowMat.shader = standIn; glowMat.SetColor("_Color", look.practicalColor); glowMat.SetFloat("_Intensity", look.cabWindowGlowIntensity); UnityEditor.EditorUtility.SetDirty(glowMat);
const float glowOut = 0.06f;
var glowRs = new System.Collections.Generic.List<UnityEngine.Renderer>();
foreach (var (gPos, gSize, gYaw) in new[] { (V(0f, paneY, ch + glowOut), V(cabW - 0.6f, paneH, 1f), 180f), (V(ch + glowOut, paneY, 0f), V(cabW - 0.6f, paneH, 1f), -90f), (V(-ch - glowOut, paneY, 0f), V(cabW - 0.6f, paneH, 1f), 90f), (V(0.65f, paneY, -ch - glowOut), V(2.5f, paneH, 1f), 0f) })
{
    var gq = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Quad); gq.name = "WindowGlow"; gq.transform.SetParent(cabDress, false);
    gq.transform.localPosition = gPos; gq.transform.localScale = gSize; gq.transform.localRotation = UnityEngine.Quaternion.Euler(0f, gYaw, 0f);
    UnityEngine.Object.DestroyImmediate(gq.GetComponent<UnityEngine.Collider>());
    var qr = gq.GetComponent<UnityEngine.Renderer>(); qr.sharedMaterial = glowMat; qr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; glowRs.Add(qr);
}
// eave lamp (8.9f): a lantern hung under the east roof overhang, its bulb unfogged at night, because from the old burn below
// (S2) the deck and its rails hide the cab windows; the lamp hangs out past them, so the tower still marks itself at night
const float eaveOut = 0.55f, eaveDrop = 0.45f, eaveBulb = 0.3f;
var eaveAt = V(ch + eaveOut, cabWallH - eaveDrop, 0f);
On(CS + "CS_Lantern_Old_Rusted", cabDress, eaveAt.x, eaveAt.y, eaveAt.z, 90f, UnityEngine.Vector3.one, false);
var eaveBulbGo = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere); eaveBulbGo.name = "EaveLampBulb"; eaveBulbGo.transform.SetParent(cabDress, false);
eaveBulbGo.transform.localPosition = eaveAt + V(0f, 0.2f, 0f); eaveBulbGo.transform.localScale = V(eaveBulb, eaveBulb, eaveBulb); UnityEngine.Object.DestroyImmediate(eaveBulbGo.GetComponent<UnityEngine.Collider>());
var ebr = eaveBulbGo.GetComponent<UnityEngine.Renderer>(); ebr.sharedMaterial = bulb; ebr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; glowRs.Add(ebr);
Practical("EaveLamp", cabDress, eaveAt + V(0f, 0.2f, 0f), 4f, PracticalLight.Kind.Lantern, PracticalLight.ByDay.Off);
var lv = cabDress.gameObject.AddComponent<LookVisibility>(); var lvo = new UnityEditor.SerializedObject(lv);
lvo.FindProperty("show").enumValueIndex = (int)LookVisibility.Show.Night; var lvt = lvo.FindProperty("targets");
lvt.arraySize = glowRs.Count; for (int i = 0; i < glowRs.Count; i++) lvt.GetArrayElementAtIndex(i).objectReferenceValue = glowRs[i].gameObject; lvo.ApplyModifiedPropertiesWithoutUndo();
// wind vane on the roof: rod and arrow, rust
float roofTop = cabWallH + 0.2f;
var vaneRod = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); vaneRod.name = "WindVaneRod"; vaneRod.transform.SetParent(cabDress, false);
vaneRod.transform.localPosition = V(0f, roofTop + 0.6f, 0f); vaneRod.transform.localScale = V(0.05f, 0.6f, 0.05f); UnityEngine.Object.DestroyImmediate(vaneRod.GetComponent<UnityEngine.Collider>()); vaneRod.GetComponent<UnityEngine.Renderer>().sharedMaterial = rustMat;
Slab("WindVaneArrow", cabDress, V(0f, roofTop + 1.1f, 0f), V(0.04f, 0.12f, 0.9f), rustMat, V(0f, 35f, 0f));
Slab("WindVaneTail", cabDress, V(0.24f, roofTop + 1.1f, -0.34f), V(0.02f, 0.3f, 0.3f), rustMat, V(0f, 35f, 0f));
// cab interior: logbook on the lectern, hanging lamp (the night marker), stool, flashlight and water on the west sill
On(CI + "Props/CITW_Book_5", cabDress, 0f, 1.15f, 1.2f, 0f, UnityEngine.Vector3.one, false, V(-15f, 0f, 90f));
var lamp = On(CI + "Props/CITW_Hanging_Oil_Lamp", cabDress, 1.3f, cabWallH - 1.01f, 1.3f, 180f, V(0.5f, 0.5f, 0.5f), false);
Practical("CabLamp", cabDress, V(1.3f, cabWallH - 0.85f, 1.2f), 5f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
var bulbGo = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere); bulbGo.name = "CabLampBulb"; bulbGo.transform.SetParent(cabDress, false);
bulbGo.transform.localPosition = V(1.3f, cabWallH - 0.85f, 1.2f); bulbGo.transform.localScale = V(0.22f, 0.26f, 0.22f);   // lamp-glass sized, so it still reads as a dot from the burn at 70 m UnityEngine.Object.DestroyImmediate(bulbGo.GetComponent<UnityEngine.Collider>());
var bulbR = bulbGo.GetComponent<UnityEngine.Renderer>(); bulbR.sharedMaterial = bulb; bulbR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
On(CI + "Furniture/CITW_Stool_1", cabDress, -0.7f, 0f, 0.5f, 20f, UnityEngine.Vector3.one, true);
On(FT + "Default/Flashlight", cabDress, -ch, sill, -0.6f, 90f, UnityEngine.Vector3.one, false);
On(SP + "Water", cabDress, -ch, sill, 0.3f, 0f, UnityEngine.Vector3.one, false);
// tower foot cluster, outside the north-east leg, clear of the stair entry in the south bay
var foot = Group("FootCluster", T, V(0f, 0f, 0f));
Ground(CI + "Props/CITW_Barrel_1", foot, T.position.x + 5.0f, T.position.z + 3.0f, 15f, UnityEngine.Vector3.one, true);
Ground(FT + "Worn/Bucket_Worn", foot, T.position.x + 4.6f, T.position.z + 4.8f, 40f, UnityEngine.Vector3.one, false);
Ground(FT + "Worn/WateringCan_Worn", foot, T.position.x + 3.1f, T.position.z + 5.1f, 110f, UnityEngine.Vector3.one, false);
Ground(FT + "Default/Rope", foot, T.position.x + 5.2f, T.position.z + 1.6f, 0f, UnityEngine.Vector3.one, false);
var shovel = Spawn(FT + "Worn/ShovelSquare_Worn", foot);   // leaning on the north-east leg
if (shovel != null) { shovel.transform.position = V(T.position.x + 4.15f, H(T.position.x + 4.3f, T.position.z + 3.75f) + 0.7f, T.position.z + 3.75f); shovel.transform.rotation = UnityEngine.Quaternion.Euler(0f, 90f, -14f); }
var footLantern = Spawn(CS + "CS_Lantern_Old_Rusted", foot);   // hung on the leg at 2 m
if (footLantern != null) { footLantern.transform.position = V(T.position.x + 4.12f, H(T.position.x + 4f, T.position.z + 3.5f) + 1.6f, T.position.z + 3.4f); footLantern.transform.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); }
Practical("FootLantern", foot, V(4.3f, 1.8f, 3.4f), 3f, PracticalLight.Kind.Lantern, PracticalLight.ByDay.Off);

// ================= CABIN (LookSlice 2: CITW log shell over the 8.2 collision) =================
foreach (var n in new[] { "Floor", "S_West", "S_East", "S_Lintel", "N_Wall", "E_Wall", "W_Sill", "W_Head", "W_PierS", "W_PierN", "Roof", "Bunk", "Desk", "ReportBox", "Stove", "StovePipe" }) Hide(C.Find(n));
var cabinDoor = C.Find("Door"); if (cabinDoor != null) Hide(cabinDoor.Find("Panel"));
var shell = Group("Shell", C, V(0f, 0f, 0f));
const float inW = 6f, inD = 4.5f, logT = 0.3f, wallH = 2.7f, modW = 2f, modH = 3f, floorTop = 0.03f;   // 8.2 cabin inside and ceiling; CITW modules 2 x 3 m
float ySc = wallH / modH, zW = inD * 0.5f + logT * 0.5f, xW = inW * 0.5f + logT * 0.5f;
var hz = V(1f, ySc, 1f); var endSc = V(inD / 3f / modW, ySc, 1f);
foreach (var x in new[] { -modW, 0f, modW }) On(CI + (x == 0f ? "Building/CITW_Log_Doorway" : "Building/CITW_Log_Wall"), shell, x, floorTop, -zW, 0f, hz, false);
foreach (var x in new[] { -modW, 0f, modW }) On(CI + "Building/CITW_Log_Wall", shell, x, floorTop, zW, 180f, hz, false);
foreach (var z in new[] { -inD / 3f, 0f, inD / 3f }) On(CI + "Building/CITW_Log_Wall", shell, xW, floorTop, z, 90f, endSc, false);
foreach (var z in new[] { -inD / 3f, 0f, inD / 3f }) On(CI + (z == 0f ? "Building/CITW_Log_Window_Wall" : "Building/CITW_Log_Wall"), shell, -xW, floorTop, z, -90f, endSc, false);
On(CI + "Building/CITW_Window_Frame", shell, -xW, floorTop + 1.0f * ySc, 0f, -90f, endSc, false);
var winGlass = On(CI + "Building/CITW_Window_Glass", shell, -xW, floorTop + 1.1f * ySc, 0f, -90f, endSc, false);   // the pack pane draws opaque white; it takes the slice glass
if (winGlass != null) foreach (var gr in winGlass.GetComponentsInChildren<UnityEngine.Renderer>()) gr.sharedMaterial = glass;
foreach (var sx in new[] { -1f, 1f }) foreach (var sz in new[] { -1f, 1f }) On(CI + "Building/CITW_Wood_Pillar", shell, sx * xW, floorTop, sz * zW, 0f, V(1f, ySc, 1f), false);
foreach (var x in new[] { -modW, 0f, modW }) foreach (var z in new[] { -inD / 3f, 0f, inD / 3f }) { var f = On(CI + "Building/CITW_Floor", shell, x, floorTop - 0.1f, z, 0f, V(1f, 1f, inD / 3f / modW), false); if (f != null) f.transform.localPosition += V(0f, 0.1f, 0f); }
if (cabinDoor != null) On(CI + "Building/CITW_Door_1", cabinDoor, 0f, 0f, 0f, 0f, UnityEngine.Vector3.one, false);
// gables on the east and west ends (CITW log triangles) and a two-slab plank roof, ridge east-west, 0.5 m overhang (the
// CITW roof modules are cut for 2 m spans; the slabs are project planks, as LookSlice 2 allows for ProBuilder pieces)
const float pitch = 0.75f, eaveOver = 0.5f, roofT = 0.12f;
float halfSpan = zW + logT * 0.5f, rise = halfSpan * pitch, ridgeY = floorTop + wallH + rise;
foreach (var sx in new[] { -1f, 1f }) On(CI + "Building/CITW_Log_Wall_Triangle", shell, sx * xW, floorTop + wallH, 0f, 90f, V(halfSpan * 2f / modW, rise / 0.75f, 1f), false);
float runZ = halfSpan + eaveOver, slopeLen = runZ * UnityEngine.Mathf.Sqrt(1f + pitch * pitch), ang = UnityEngine.Mathf.Atan(pitch) * UnityEngine.Mathf.Rad2Deg, roofLenX = (xW + logT * 0.5f + eaveOver) * 2f;
foreach (var sz in new[] { -1f, 1f })
    Slab("RoofSlab", shell, V(0f, ridgeY - pitch * runZ * 0.5f + roofT * 0.5f, sz * runZ * 0.5f), V(roofLenX, roofT, slopeLen), null, V(sz * ang, 0f, 0f));
Slab("RidgeBeam", shell, V(0f, ridgeY + roofT, 0f), V(roofLenX, 0.16f, 0.2f), null, UnityEngine.Vector3.zero);
// stovepipe through the roof over the north-east corner, rust (the CITW flue is a masonry-sized chimney)
var pipe = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); pipe.name = "StovePipe"; pipe.transform.SetParent(shell, false);
const float pipeX = 2.7f, pipeZ = 1.85f, pipeAboveRoof = 0.9f;   // from the ceiling (the CITW stove's own pipe) to 0.9 m over the roof
float pipeBottom = floorTop + wallH - 0.1f, pipeTop = ridgeY - pitch * pipeZ + pipeAboveRoof;
pipe.transform.localPosition = V(pipeX, (pipeBottom + pipeTop) * 0.5f, pipeZ); pipe.transform.localScale = V(0.18f, (pipeTop - pipeBottom) * 0.5f, 0.18f); UnityEngine.Object.DestroyImmediate(pipe.GetComponent<UnityEngine.Collider>()); pipe.GetComponent<UnityEngine.Renderer>().sharedMaterial = rustMat;
// porch: south face, full width, 1.8 m deep, on the ground; posts, a light plank roof under the eave, end rails
const float porchD = 1.8f, porchRoofBack = 2.28f, porchRoofFront = 2.05f;
float porchZ = -(zW + logT * 0.5f + porchD * 0.5f), porchW = (xW + logT * 0.5f) * 2f;
var porch = Group("Porch", C, V(0f, 0f, 0f));
foreach (var x in new[] { -modW * 1.1f, 0f, modW * 1.1f }) { var f = On(CI + "Building/CITW_Floor", porch, x, floorTop - 0.1f, porchZ, 0f, V(1.1f, 1f, porchD / modW), false); if (f != null) f.transform.localPosition += V(0f, 0.1f, 0f); }
float porchFront = porchZ - porchD * 0.5f + 0.15f;
foreach (var sx in new[] { -1f, 1f }) On(CI + "Building/CITW_Wood_Pillar", porch, sx * (porchW * 0.5f - 0.15f), floorTop, porchFront, 0f, V(1f, porchRoofFront / modH, 1f), true);
float prLen = UnityEngine.Mathf.Sqrt(porchD * porchD + (porchRoofBack - porchRoofFront) * (porchRoofBack - porchRoofFront)) + 0.3f;
Slab("PorchRoof", porch, V(0f, floorTop + (porchRoofBack + porchRoofFront) * 0.5f + roofT * 0.5f, porchZ - 0.15f), V(porchW + 0.4f, roofT, prLen), null, V(-UnityEngine.Mathf.Atan((porchRoofBack - porchRoofFront) / porchD) * UnityEngine.Mathf.Rad2Deg, 0f, 0f));
foreach (var sx in new[] { -1f, 1f }) On(CI + "Building/CITW_Railing", porch, sx * (porchW * 0.5f - 0.15f), floorTop, porchZ, 90f, V(porchD * 0.9f / modW, 1f, 1f), true);
// skirt under the porch edge where the ground falls away, so the floor never floats
float porchGround = float.MaxValue; foreach (var px in new[] { -porchW * 0.5f, 0f, porchW * 0.5f }) { var wp = C.TransformPoint(V(px, 0f, porchFront - 0.15f)); porchGround = UnityEngine.Mathf.Min(porchGround, H(wp.x, wp.z)); }
float skirtH = C.position.y + floorTop - porchGround; if (skirtH > 0.05f) Slab("PorchSkirt", porch, V(0f, floorTop - skirtH * 0.5f, porchFront - 0.15f), V(porchW, skirtH + 0.1f, 0.12f), null, UnityEngine.Vector3.zero);
// lantern on a stool by the door (night only)
var stool = On(CS + "CS_Log_Stool_1", porch, 1.4f, floorTop, -3.0f, 0f, UnityEngine.Vector3.one, true);
On(CS + "CS_Lantern_Old_Rusted", porch, 1.4f, floorTop + 0.5f, -3.0f, 30f, UnityEngine.Vector3.one, false);
Practical("PorchLantern", porch, V(1.4f, floorTop + 0.75f, -3.0f), 4f, PracticalLight.Kind.Lantern, PracticalLight.ByDay.Off);
// interior (Main3.md 3.4): bed along the north wall, west half; stove in the north-east corner; desk under the west window
var inside = Group("Interior", C, V(0f, 0f, 0f));
On(CI + "Furniture/CITW_Bed", inside, -1.74f, floorTop, inD * 0.5f - 0.76f, 90f, UnityEngine.Vector3.one, false);
var stove = On(CI + "Furniture/CITW_Wood_Stove", inside, inW * 0.5f - 0.31f, floorTop, 1.85f, 90f, UnityEngine.Vector3.one, true);
On(CI + "Props/CITW_Kettle", inside, 2.4f, 0.86f, 1.85f, 200f, UnityEngine.Vector3.one, false);
Practical("StoveLight", inside, V(2.1f, 0.5f, 1.6f), 4f, PracticalLight.Kind.Stove, PracticalLight.ByDay.Full);
On(CI + "Furniture/CITW_Table", inside, -2.6f, floorTop, 0f, 90f, V(0.7f, 0.83f, 0.7f), false);
float deskTop = floorTop + 0.9f * 0.83f;
On(CI + "Props/CITW_Crate", inside, -2.6f, deskTop, 0.35f, 0f, V(0.3f, 0.3f, 0.4f), false);   // report box
On(CI + "Props/CITW_Oil_Lamp_1", inside, -2.65f, deskTop, -0.4f, 0f, UnityEngine.Vector3.one, false);
Practical("DeskLamp", inside, V(-2.65f, deskTop + 0.35f, -0.4f), 3f, PracticalLight.Kind.Lamp, PracticalLight.ByDay.Dimmed);
On(CI + "Props/CITW_Mug", inside, -2.4f, deskTop, -0.05f, 60f, UnityEngine.Vector3.one, false);
On(CI + "Props/CITW_Book_1", inside, -2.85f, deskTop, -0.62f, 0f, UnityEngine.Vector3.one, false);
On(CI + "Props/CITW_Book_3", inside, -2.85f, deskTop, -0.55f, 0f, UnityEngine.Vector3.one, false);
On(SP + "MatchesV1", inside, -2.4f, deskTop, -0.35f, 25f, UnityEngine.Vector3.one, false);
On(CI + "Furniture/CITW_Chair", inside, -1.95f, floorTop, 0f, -90f, UnityEngine.Vector3.one, true);
On(CI + "Furniture/CITW_Shelf", inside, inW * 0.5f, floorTop + 1.5f, -1.0f, -90f, UnityEngine.Vector3.one, false);
On(CI + "Props/CITW_Canned_Food_1", inside, 2.75f, floorTop + 1.53f, -1.3f, 0f, UnityEngine.Vector3.one, false);
On(CI + "Props/CITW_Canned_Food_2", inside, 2.75f, floorTop + 1.53f, -1.1f, 40f, UnityEngine.Vector3.one, false);
On(CI + "Furniture/CITW_Rug_1", inside, 0.2f, floorTop + 0.005f, 0.2f, 0f, V(0.8f, 1f, 0.8f), false);
On(SP + "Water", inside, -0.6f, floorTop, 1.4f, 0f, UnityEngine.Vector3.one, false);
// woodpile and chopping block against the east wall, beside the porch; the axe in the block is the mid-task object
var wp2 = Group("Woodpile", C, V(0f, 0f, 0f)); float ex = xW + logT * 0.5f;
UnityEngine.Vector3 W(float lx, float lz) => C.TransformPoint(V(lx, 0f, lz));
var wpA = W(ex + 0.6f, -0.4f); Ground(CS + "Wood/CS_Firewood_Logs", wp2, wpA.x, wpA.z, 0f, V(1.2f, 1.2f, 1.2f), true);
var wpB = W(ex + 0.5f, 0.9f); Ground(CS + "Wood/CS_Log_Firewood", wp2, wpB.x, wpB.z, 90f, V(1.4f, 1.4f, 1.4f), true);
var wpC = W(ex + 0.9f, 1.0f); Ground(CI + "Props/CITW_Firewood_1", wp2, wpC.x, wpC.z, 20f, UnityEngine.Vector3.one, false);
var wpD = W(ex + 1.2f, -1.2f); Ground(CI + "Props/CITW_Firewood_2", wp2, wpD.x, wpD.z, 70f, UnityEngine.Vector3.one, false);
var blockAt = W(ex + 1.5f, -2.6f); var block = Spawn(CI + "Vegetation/CITW_Tree_Stump", wp2);
if (block != null) { block.transform.position = V(blockAt.x, H(blockAt.x, blockAt.z), blockAt.z); block.transform.localScale = V(0.35f, 0.9f, 0.35f); Fit(block); }
float blockTop = H(blockAt.x, blockAt.z) + 0.52f * 0.9f;
var axe = Spawn(CS + "Tools/CS_Tool_Axe_Metal_Rusted", wp2);
if (axe != null) { axe.transform.position = V(blockAt.x, blockTop + 0.1f, blockAt.z); axe.transform.rotation = UnityEngine.Quaternion.Euler(0f, 30f, 30f); }

// ================= FIRE PIT CLUSTER (172, 163) =================
foreach (UnityEngine.Transform ch2 in pitT) Hide(ch2);
foreach (var col in pitT.GetComponentsInChildren<UnityEngine.Collider>()) col.enabled = false;
var pitC = pitT.position; var fire = Group("PitDressing", pitT, V(0f, 0f, 0f));
Ground(CS + "CS_Campfire_1", fire, pitC.x, pitC.z, 0f, UnityEngine.Vector3.one, false);
var flameGo = Spawn(CS + "FX/FX_Flames_Short", fire);
if (flameGo != null) { flameGo.transform.position = pitC + V(0f, 0.1f, 0f); flameGo.transform.localScale = V(0.8f, 0.8f, 0.8f); foreach (var pr in flameGo.GetComponentsInChildren<UnityEngine.ParticleSystemRenderer>()) if (flames != null) pr.sharedMaterial = flames; }
var pitLight = Practical("FirePitLight", fire, V(0f, 0.6f, 0f), 8f, PracticalLight.Kind.FirePit, PracticalLight.ByDay.Dimmed);
var fp = pitT.gameObject.AddComponent<FirePit>(); var fpo = new UnityEditor.SerializedObject(fp);
fpo.FindProperty("prompt").stringValue = "Fire"; fpo.FindProperty("fireLight").objectReferenceValue = pitLight;
var bo = fpo.FindProperty("burningObjects"); bo.arraySize = flameGo != null ? 2 : 1; bo.GetArrayElementAtIndex(0).objectReferenceValue = pitLight.gameObject; if (flameGo != null) bo.GetArrayElementAtIndex(1).objectReferenceValue = flameGo;
fpo.ApplyModifiedPropertiesWithoutUndo();
var pitBox = pitT.gameObject.AddComponent<UnityEngine.BoxCollider>(); pitBox.center = V(0f, 0.45f, 0f); pitBox.size = V(1.7f, 0.9f, 1.7f);   // the interact ray reaches it (Rules and Tips, CAMP)
Ground(CS + "Cookware/CS_Campfire_Tripod_Wood", fire, pitC.x, pitC.z, 0f, UnityEngine.Vector3.one, false);
var kettle = Spawn(CS + "Cookware/CS_Cookware_Kettle_3", fire); if (kettle != null) kettle.transform.position = V(pitC.x, H(pitC.x, pitC.z) + 0.9f, pitC.z);
const float seatR = 2.3f;   // logs tangent at 2.3 m (LookSlice 2)
UnityEngine.GameObject firstSeat = null; int si = 0;
foreach (var a in new[] { 125f, 200f, 275f })
{
    si++; float rad = a * UnityEngine.Mathf.Deg2Rad; var s = Ground(CS + "CS_Log_Large_Seat_" + si, fire, pitC.x + UnityEngine.Mathf.Cos(rad) * seatR, pitC.z + UnityEngine.Mathf.Sin(rad) * seatR, -a - 90f, V(1.6f, 1.4f, 1.4f), true);
    if (firstSeat == null) firstSeat = s;
}
foreach (var (a, path) in new[] { (50f, CS + "CS_Chair_1"), (345f, CS + "CS_Log_Stool_1") })
{
    float rad = a * UnityEngine.Mathf.Deg2Rad; float x = pitC.x + UnityEngine.Mathf.Cos(rad) * seatR, z = pitC.z + UnityEngine.Mathf.Sin(rad) * seatR;
    var g = Ground(path, fire, x, z, 0f, UnityEngine.Vector3.one, true);
    if (g != null) { var to = pitC - g.transform.position; to.y = 0f; g.transform.rotation = UnityEngine.Quaternion.LookRotation(to.normalized); }
}
if (firstSeat != null) { var st = firstSeat.transform.position; var mug = Spawn(CS + "Tableware/CS_Tableware_Mug_Metal_1", fire); if (mug != null) mug.transform.position = st + V(0f, 0.37f * 1.4f, 0f); }

// ================= GENERATOR (gap: no pack generator) =================
genT.GetComponent<UnityEngine.Renderer>().sharedMaterial = rustMat;
var gp = genT.position; var genD = Group("GeneratorDressing", campRoot.transform, V(0f, 0f, 0f));
Slab("GeneratorBase", genD, V(gp.x, gp.y - 0.45f + 0.03f, gp.z), V(1.3f, 0.06f, 0.8f), charMat, UnityEngine.Vector3.zero);
Ground(FT + "Default/OilCanister", genD, gp.x + 0.95f, gp.z - 0.15f, 20f, UnityEngine.Vector3.one, false);
Ground(FT + "Default/Canister", genD, gp.x + 0.95f, gp.z + 0.3f, 80f, UnityEngine.Vector3.one, false);

// ================= GROUND: camp floor layer, cover, rocks, edge trees =================
var campC = new UnityEngine.Vector2(170f, 160f); const float clearR = 18f, flankR = 38f;   // 36 m clearing, flank to 20 m outside (LookSlice 1)
// trail points and the placed structures to keep clear of
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>();
foreach (UnityEngine.Transform leg in Root("Trails").transform) foreach (UnityEngine.Transform m in leg) if (UnityEngine.Vector2.Distance(new UnityEngine.Vector2(m.position.x, m.position.z), campC) < flankR + 6f) trailPts.Add(new UnityEngine.Vector2(m.position.x, m.position.z));
var keepOut = new System.Collections.Generic.List<(UnityEngine.Vector2 c, float r)> { (new UnityEngine.Vector2(C.position.x, C.position.z - 0.8f), 6.2f), (new UnityEngine.Vector2(T.position.x, T.position.z), 6.5f), (new UnityEngine.Vector2(pitC.x, pitC.z), 3.6f), (new UnityEngine.Vector2(gp.x, gp.z), 2f) };
bool Clear(UnityEngine.Vector2 p, float trailGap) { foreach (var t in trailPts) if (UnityEngine.Vector2.Distance(p, t) < trailGap) return false; foreach (var k in keepOut) if (UnityEngine.Vector2.Distance(p, k.c) < k.r) return false; return true; }
// terrain layer: the camp floor, #58583A (LookSlice 2), on the project Ground054 texture; trails keep their own layer
var data = terrain.terrainData; string layerDir = "Assets/Terrain/Main3";
var groundMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Ground054_25.0x25.0.mat");
var floorTex = groundMat != null ? groundMat.GetTexture("_BaseMap") as UnityEngine.Texture2D : null; if (floorTex == null) return "Ground054 texture missing";
var floorCol = Hex("#58583A");
var campLayer = new UnityEngine.TerrainLayer { diffuseTexture = floorTex, tileSize = new UnityEngine.Vector2(4f, 4f), diffuseRemapMin = UnityEngine.Vector4.zero, diffuseRemapMax = new UnityEngine.Vector4(floorCol.r * 2f, floorCol.g * 2f, floorCol.b * 2f, 1f) };
UnityEditor.AssetDatabase.CreateAsset(campLayer, layerDir + "/Layer_CampFloor.terrainlayer");
var oldLayers = data.terrainLayers; int trailLayer = 3, nl = oldLayers.Length + 1;
int ar = data.alphamapResolution; var oldA = data.GetAlphamaps(0, 0, ar, ar); var newA = new float[ar, ar, nl];
var newLayers = new UnityEngine.TerrainLayer[nl]; for (int i = 0; i < oldLayers.Length; i++) newLayers[i] = oldLayers[i]; newLayers[nl - 1] = campLayer;
int painted = 0;
for (int zi = 0; zi < ar; zi++) for (int xi = 0; xi < ar; xi++)
{
    for (int k = 0; k < nl - 1; k++) newA[zi, xi, k] = oldA[zi, xi, k];
    float x = (xi + 0.5f) * data.size.x / ar, z = (zi + 0.5f) * data.size.z / ar; float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(x, z), campC);
    if (d > flankR || oldA[zi, xi, trailLayer] > 0.3f || oldA[zi, xi, 1] > 0.5f) continue;
    float w = 1f - UnityEngine.Mathf.Clamp01((d - (flankR - 4f)) / 4f); if (w <= 0f) continue;
    for (int k = 0; k < nl - 1; k++) newA[zi, xi, k] *= 1f - w; newA[zi, xi, nl - 1] = w; painted++;
}
data.terrainLayers = newLayers; data.SetAlphamaps(0, 0, newA); UnityEditor.EditorUtility.SetDirty(data);
// cover: dead leaves, clover and moss inside the clearing; ferns and olive grass thicker on the edge and the flank
var rng = new System.Random(8904); float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
var cover = Group("GroundCover", dress, V(0f, 0f, 0f)); int coverN = 0;
string[] inner = { BK + "Plants/DeadLeaves1", BK + "Plants/DeadLeaves2", BK + "Plants/Clovers1", BK + "Plants/Clovers2", BK + "Plants/GrassMoss", SC + "Grass1", SC + "Grass2" };
string[] outer = { BK + "Plants/ThinFern1", BK + "Plants/ThinFern2", BK + "Plants/ThinFern3", BK + "Plants/GrassMoss", BK + "Plants/DeadLeaves1", SC + "Grass1", SC + "Grass2", SC + "Grass3", BK + "Plants/Clovers1" };
for (int i = 0; i < 700 && coverN < 420; i++)
{
    float a = R(0f, UnityEngine.Mathf.PI * 2f), r = UnityEngine.Mathf.Sqrt(R(0.02f, 1f)) * flankR; var p = campC + new UnityEngine.Vector2(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * r;
    if (!Clear(p, 1.6f)) continue;
    bool edge = r > clearR - 3f; if (!edge && rng.NextDouble() < 0.45) continue;   // thinner inside the clearing, where people walk
    var pick = edge ? outer[rng.Next(outer.Length)] : inner[rng.Next(inner.Length)]; float s = R(0.8f, 1.4f) * (pick.StartsWith(SC) ? 1.6f : 1f);
    if (Ground(pick, cover, p.x, p.y, R(0f, 360f), V(s, s, s), false, 0.03f) != null) coverN++;
}
// rocks at the clearing edge, and edge trees clustered on the flank (at most 35 m on the knoll; kept low beside the tower lines)
var rocks = Group("Rocks", dress, V(0f, 0f, 0f)); int rockN = 0;
for (int i = 0; i < 80 && rockN < 10; i++)
{
    float a = R(0f, UnityEngine.Mathf.PI * 2f), r = R(clearR - 1f, clearR + 2f); var p = campC + new UnityEngine.Vector2(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * r;
    if (!Clear(p, 3.5f)) continue; float s = R(0.45f, 0.9f);
    if (Ground(CS + "Rocks and Stones/CS_Rock_" + (1 + rng.Next(4)), rocks, p.x, p.y, R(0f, 360f), V(s, s * R(0.6f, 1f), s), true, 0.15f) != null) rockN++;
}
var trees = Group("EdgeTrees", dress, V(0f, 0f, 0f)); int treeN = 0; float tallest = 0f;
const float treeMinH = 11f, treeMaxH = 18f;   // well under the 35 m cap: the deck's lines to the places pass low over the flank
for (int cl = 0; cl < 40 && treeN < 18; cl++)
{
    float a = R(0f, UnityEngine.Mathf.PI * 2f), r = R(clearR + 6f, flankR - 4f); var cc = campC + new UnityEngine.Vector2(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * r;
    if (!Clear(cc, 6f)) continue;
    for (int j = 0; j < 3 && treeN < 18; j++)
    {
        var p = cc + new UnityEngine.Vector2(R(-4f, 4f), R(-4f, 4f)); if (!Clear(p, 5f)) continue;
        string path = rng.NextDouble() < 0.6 ? BK + "Trees/RedFir" + (1 + rng.Next(8)) : BK + "Trees/RedPine" + (1 + rng.Next(5));
        var g = Spawn(path, trees); if (g == null) continue;
        g.transform.position = V(p.x, 0f, p.y); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
        var b = LocalBounds(g); float want = R(treeMinH, treeMaxH), s = want / UnityEngine.Mathf.Max(0.5f, b.size.y);
        g.transform.localScale = V(s, s, s); g.transform.position = V(p.x, H(p.x, p.y) - 0.2f, p.y); tallest = UnityEngine.Mathf.Max(tallest, want); treeN++;
    }
}

// ================= LOOKS: day one and day two presets (LookSlice 4 and 5) =================
void SetLook(string path, string sun, float elev, float inten, string amb, string skyT, string skyH, string fogC, float fogS, float fogE)
{
    var t = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(path); if (t == null) { missing.Add(path); return; }
    t.sunColor = Hex(sun); t.sunElevation = elev; t.sunBearing = 270f; t.sunIntensity = inten; t.sunsetAmbient = Hex(amb);
    t.skyTop = Hex(skyT); t.skyHorizon = Hex(skyH); t.sunsetFogColor = Hex(fogC); t.sunsetFogStart = fogS; t.sunsetFogEnd = fogE; UnityEditor.EditorUtility.SetDirty(t);
}
SetLook("Assets/Settings/LookTuning_DayOne.asset", "#FFC98A", 14f, 1.1f, "#6E6658", "#5E6878", "#E3A968", "#A8A08E", 40f, 600f);
SetLook("Assets/Settings/LookTuning_DayTwo.asset", "#FF8C40", 6f, 1.2f, "#734D42", "#381C1A", "#D9662E", "#9E5C38", 25f, 420f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
report.Append("saved=" + saved + " | pack props " + props + " | tower renderers wood " + woodN + ", rust " + rustN + " | materials " + (matCache.Count + 3) + " in " + SliceDir);
report.Append(" | cover " + coverN + ", rocks " + rockN + ", edge trees " + treeN + " (tallest " + tallest.ToString("F0") + " m) | camp floor cells " + painted);
report.Append(" | missing: " + (missing.Count == 0 ? "none" : string.Join(", ", missing)));
return report.ToString();
