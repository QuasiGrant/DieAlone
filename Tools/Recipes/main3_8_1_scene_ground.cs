// Main3 task 8.1: scene, terrain, fence, bounds, spawn, dev warps.
// Source: Docs/Design/Main3.md rev 11 table 2.1 and Main3_map.svg; resolutions in Docs/Design/Main3_BuildNotes.md.
// Run from Graybox (or any saved scene) in edit mode. Refuses if Main3.unity exists: delete it and its terrain folder to rebuild,
// then rerun every later Main3 recipe in task order.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) return "active scene dirty, save first";
const string scenePath = "Assets/Scenes/Main3.unity";
const string dir = "Assets/Terrain/Main3";
if (System.IO.File.Exists(scenePath)) return "Main3.unity already exists";
if (!NewSceneMenu.CreateScene(scenePath)) return "recipe failed";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != scenePath) return "wrong scene after recipe: " + scene.path;
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
foreach (var r in scene.GetRootGameObjects()) if (r.name == "Ground") UnityEngine.Object.DestroyImmediate(r);

// ---------- map data (metres, x east, z north) ----------
const float sizeX = 400f, sizeZ = 300f, baseY = -45f, sizeY = 90f;
const int res = 513, ares = 512;
var spurEdge = new[] { P(10,190), P(60,200), P(100,225.2f), P(124.8f,260), P(134.8f,300) };        // SE boundary of the spur
var spurPoly = new[] { P(10,190), P(60,200), P(100,225.2f), P(124.8f,260), P(134.8f,300), P(10,300) };
var crest = new[] { P(72,212), P(108,284) };
var ravPoly = new[] { P(20,20), P(80,25.2f), P(94.8f,50), P(74,60), P(70,62), P(30,55.2f) };
var ravEdges = new[] { P(30,55.2f), P(20,20), P(80,25.2f), P(94.8f,50) };                           // west, south and east sides
var rim = new[] { P(30,55.2f), P(70,62), P(74,60), P(94.8f,50) };
var burnPoly = new[] { P(185,181), P(340,213), P(340,143), P(185,151) };
var creek = new[] { P(104,215.2f), P(104.8f,203.2f), P(100,175.2f), P(84,150), P(78,146), P(84,128), P(100,110), P(128,78), P(136.5f,66) };   // through the hollow centre, 11 m clear of the Snag
var creekBed = new[] { 9.5f, 8f, 3.5f, -4.1f, -4.1f, -4.3f, -4.8f, -5.3f, -5.8f };
var lakeC = P(190, 60); const float lakeA = 54.8f, lakeB = 27.6f;
// named ground points: centre, flat radius, blend width, height (table 2.1)
var named = new (UnityEngine.Vector2 c, float r, float blend, float h)[] {
    (P(170,160), 18f, 27f, 8f),      // keeper's camp knoll top
    (P(282,238), 30f, 15f, 5f),      // Camp 1
    (P(292,108), 20f, 12f, 4f),      // Camp 2 boulder field
    (P(262,172), 4f, 12f, 5f),       // Jg
    (P(290,176), 3f, 10f, 4f),       // Gate Tree
    (P(202,140), 5f, 10f, 6f),       // Hollow Giant
    (P(240,162.8f), 4f, 10f, 5f),    // forage A
    (P(142.4f,163.6f), 4f, 10f, 6f), // forage B
    (P(104,206), 5f, 10f, 10f),      // J
    (P(128,70), 4f, 8f, -4.5f),      // W1
    (P(32,258), 12f, 8f, 36f),       // Ward ledge
};
// old burn: 8 at the knoll foot (x 185) falling to 3 at the front zone (x 340)
float BurnH(float x) => 8f - 5f * UnityEngine.Mathf.Clamp01((x - 185f) / 155f);

float SegDist(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b, out float t)
{
    var ab = b - a; t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-4f));
    return UnityEngine.Vector2.Distance(p, a + ab * t);
}
float LineDist(UnityEngine.Vector2 p, UnityEngine.Vector2[] pts, out float s)
{
    float best = float.MaxValue, acc = 0f; s = 0f;
    for (int i = 0; i < pts.Length - 1; i++)
    {
        float d = SegDist(p, pts[i], pts[i + 1], out float t); float L = UnityEngine.Vector2.Distance(pts[i], pts[i + 1]);
        if (d < best) { best = d; s = acc + t * L; }
        acc += L;
    }
    return best;
}
float LineLen(UnityEngine.Vector2[] pts) { float L = 0; for (int i = 0; i < pts.Length - 1; i++) L += UnityEngine.Vector2.Distance(pts[i], pts[i + 1]); return L; }
bool Inside(UnityEngine.Vector2 p, UnityEngine.Vector2[] poly)
{
    bool c = false;
    for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
        if (((poly[i].y > p.y) != (poly[j].y > p.y)) && (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) c = !c;
    return c;
}
float SS(float a) => UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01(a));
float L(float a, float b, float t) => UnityEngine.Mathf.Lerp(a, b, t);

// base: 0 in the south-west rolling up to 5 in the north-east
float Base(float x, float z) => 2.5f * (x / sizeX + z / sizeZ);
var ctrl = new System.Collections.Generic.List<(UnityEngine.Vector2 c, float h)>();
foreach (var n in named) ctrl.Add((n.c, n.h));
for (float bx = 200f; bx <= 330f; bx += 32.5f) ctrl.Add((P(bx, 166f + (bx - 185f) * 0.08f), BurnH(bx)));
float rimLen = LineLen(rim);
float rimS18 = 45.1f;   // arc length of the vertex (74, 60) along the rim line

float Height(float x, float z)
{
    var p = P(x, z);
    // 1. base plus smooth offsets toward the named heights, small roll
    float b = Base(x, z), num = 0f, den = 0.15f;
    foreach (var c in ctrl) { float w = UnityEngine.Mathf.Exp(-(UnityEngine.Vector2.SqrMagnitude(p - c.c)) / (40f * 40f)); num += w * (c.h - Base(c.c.x, c.c.y)); den += w; }
    float h = b + num / den + (UnityEngine.Mathf.PerlinNoise(x / 45f + 3.1f, z / 45f + 7.7f) - 0.5f) * 1.6f;
    float field = h;
    // 2. spur and plateau: from its SE edge up to the crest at 34, then to 36 toward the Ward ledge and the cliff
    if (Inside(p, spurPoly))
    {
        float db = LineDist(p, spurEdge, out _);
        float dc = LineDist(p, crest, out _);
        var cd = crest[1] - crest[0];
        float side = cd.x * (z - crest[0].y) - cd.y * (x - crest[0].x);   // > 0 north-west of the crest line
        float target = side > 0f ? 34f + 2f * SS(dc / 40f) : L(34f, h, SS(dc / (dc + db)));
        h = side > 0f ? L(h, target, SS(db / 12f)) : target;
    }
    // 3. named flats
    foreach (var n in named) { float d = UnityEngine.Vector2.Distance(p, n.c); if (d < n.r + n.blend) h = L(n.h, h, SS((d - n.r) / n.blend)); }
    // 4. Camp 3 hollow
    {
        float d = UnityEngine.Vector2.Distance(p, P(78, 146));
        if (d < 30f)
        {
            float hh = d < 8f ? -4f : d < 12f ? L(-4f, 4f, SS((d - 8f) / 4f)) : d < 20f ? 4f : L(4f, h, SS((d - 20f) / 10f));
            h = hh;
        }
    }
    // 5. lake: bed -8 at the centre shelving to -5.7 at the water line, bank -4.5 at 1.5 m, back to the ground within 15 m
    {
        var q = p - lakeC; float re = UnityEngine.Mathf.Sqrt((q.x / lakeA) * (q.x / lakeA) + (q.y / lakeB) * (q.y / lakeB));
        float Lr = re > 1e-4f ? q.magnitude / re : lakeB; float dOut = (re - 1f) * Lr;
        if (dOut <= 0f) h = -5.7f - 2.3f * (1f - re * re);
        else if (dOut < 1.5f) h = L(-5.7f, -4.5f, dOut / 1.5f);
        else if (dOut < 15f) h = L(-4.5f, h, (dOut - 1.5f) / 13.5f);   // linear bank, under 30 degrees
        if (dOut > 0f && z > 80f) h = UnityEngine.Mathf.Min(h, -4.5f + 0.7f * UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Abs(x - 190f) - 2.5f) + 0.5f * UnityEngine.Mathf.Max(0f, z - 96f));   // dock root notch at -4.5 to z 96, soft sides
    }
    // 6. sill between lake and ravine
    {
        float sx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(100f - x, x - 120f)), sz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(45f - z, z - 65f));
        h = UnityEngine.Mathf.Max(h, -3f - 0.5f * UnityEngine.Mathf.Sqrt(sx * sx + sz * sz));   // at least -3 inside, soft edges
    }
    // 7. ravine: rim ridge on the north side (14, 18 near (74, 60)), floor -6 north of the mouth face
    {
        float dr = LineDist(p, rim, out float s);
        float rimTop = (s >= rimS18 - 20f && s <= rimS18 + 15f) ? 18f : 14f;
        if (s < rimS18 - 20f) rimTop = L(14f, 18f, SS((s - (rimS18 - 25f)) / 5f));
        float taper = SS(UnityEngine.Mathf.Min(s, rimLen - s) / 8f);
        bool inside = Inside(p, ravPoly);
        if (inside)
        {
            float de = LineDist(p, ravEdges, out _);
            float floorW = SS((z - 33f) / 4f) * SS(de / 10f);
            float fl = L(h, -6f, floorW);
            float rimSide = L(fl, L(field, rimTop, taper), 1f - SS((dr - 1.5f) / 12f));
            h = UnityEngine.Mathf.Max(fl, rimSide);
        }
        else if (dr < 22f)
        {
            float ridge = L(L(h, rimTop, taper), h, SS((dr - 1.5f) / 20f));
            h = UnityEngine.Mathf.Max(h, ridge);
        }
    }
    // 8. creek: channel above the hollow, a low walkable valley from the hollow to the lake
    {
        float dcr = float.MaxValue, bed = 0f, acc = 0f; int seg = 0;
        for (int i = 0; i < creek.Length - 1; i++)
        {
            float d = SegDist(p, creek[i], creek[i + 1], out float t);
            if (d < dcr) { dcr = d; bed = L(creekBed[i], creekBed[i + 1], t); seg = i; }
        }
        // cascade channel above the hollow; steep notch at the hollow, blended out by 26 m; low valley from there to the lake
        float hollowT = SS((UnityEngine.Vector2.Distance(p, P(78, 146)) - 18f) / 8f);   // 0 at the hollow, 1 from 26 m out
        float hw = L(2.5f, seg <= 2 ? 1.2f : 4f, hollowT);
        float slope = L(2.0f, seg <= 2 ? 1.0f : 0.45f, hollowT);
        float floorH = seg >= 5 && dcr >= 1.2f ? bed + 0.3f : bed;
        float carve = floorH + UnityEngine.Mathf.Max(0f, dcr - hw) * slope;
        float reach = 1f - SS((dcr - hw - 8f) / 4f);   // banks end 12 m out; the creek never cuts distant ground
        h = UnityEngine.Mathf.Min(h, L(h, carve, reach));
        // W1 stays at its table height beside the inlet
        float dW = UnityEngine.Vector2.Distance(p, P(128, 70));
        if (dW < 6f) h = L(-4.5f, h, SS((dW - 3f) / 3f));
    }
    // 9. ground over the cave: 0 over the chamber and ramp (x 44 to 89, z 3 to 22) and over the entrance passage (x 50 to 54, z 22 to 34)
    {
        float dx = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(42f - x, x - 91f)), dz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Max(1f - z, z - 24f));
        float dRect = UnityEngine.Mathf.Sqrt(dx * dx + dz * dz);
        if (dRect < 4f) h = L(0f, h, SS(dRect / 4f));
        if (x >= 48f && x <= 56f && z >= 22f && z <= 34f) h = 0f;
    }
    // 10. front zone flat at 3
    if (x >= 330f) h = L(h, 3f, SS((x - 330f) / 10f));
    // 11. cliff at x 10, valley floor -40
    if (x < 10f) h = L(-40f, h, SS((x - 4f) / 6f));
    return h;
}

if (!UnityEditor.AssetDatabase.IsValidFolder(dir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Terrain", "Main3");
// Layers and their textures are imported before the terrain data exists: an import refresh can reload an unsaved TerrainData empty.
// ---------- gray layers: solid tone textures made here, no pack art ----------
UnityEngine.TerrainLayer Layer(string name, float g, float warm, float tile)
{
    string texPath = dir + "/Gray_" + name + ".png";
    var tex = new UnityEngine.Texture2D(8, 8, UnityEngine.TextureFormat.RGB24, false);
    var col = new UnityEngine.Color(g + warm, g, g - warm);
    var px = new UnityEngine.Color[64];
    for (int i = 0; i < 64; i++) { float n = ((i * 7 + i / 8 * 3) % 5) * 0.006f; px[i] = col + new UnityEngine.Color(n, n, n); }
    tex.SetPixels(px); tex.Apply();
    System.IO.File.WriteAllBytes(texPath, tex.EncodeToPNG());
    UnityEngine.Object.DestroyImmediate(tex);
    UnityEditor.AssetDatabase.ImportAsset(texPath);
    var layer = new UnityEngine.TerrainLayer { diffuseTexture = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(texPath), tileSize = new UnityEngine.Vector2(tile, tile) };
    UnityEditor.AssetDatabase.CreateAsset(layer, dir + "/Layer_" + name + ".terrainlayer");
    return layer;
}
var layers = new[] { Layer("Ground", 0.50f, 0f, 4f), Layer("Rock", 0.33f, 0f, 4f), Layer("Burn", 0.42f, 0.025f, 4f), Layer("Trail", 0.64f, 0.01f, 2f), Layer("LakeBed", 0.36f, -0.03f, 4f) };
// ---------- terrain data ----------
var data = new UnityEngine.TerrainData();
data.heightmapResolution = res;
data.size = V(sizeX, sizeY, sizeZ);
data.alphamapResolution = ares;
data.baseMapResolution = 512;
UnityEditor.AssetDatabase.CreateAsset(data, dir + "/Main3_TerrainData.asset");
var hm = new float[res, res];
for (int zi = 0; zi < res; zi++)
    for (int xi = 0; xi < res; xi++)
        hm[zi, xi] = UnityEngine.Mathf.Clamp01((Height(xi * sizeX / (res - 1), zi * sizeZ / (res - 1)) - baseY) / sizeY);
data.SetHeights(0, 0, hm);
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();

data.terrainLayers = layers;
var alpha = new float[ares, ares, layers.Length];
for (int zi = 0; zi < ares; zi++)
    for (int xi = 0; xi < ares; xi++)
    {
        float x = (xi + 0.5f) * sizeX / ares, z = (zi + 0.5f) * sizeZ / ares;
        float steep = data.GetSteepness((xi + 0.5f) / ares, (zi + 0.5f) / ares);
        var q = P(x, z) - lakeC; float re = UnityEngine.Mathf.Sqrt((q.x / lakeA) * (q.x / lakeA) + (q.y / lakeB) * (q.y / lakeB));
        int k = steep > 35f ? 1 : re < 1.03f ? 4 : Inside(P(x, z), burnPoly) ? 2 : 0;
        alpha[zi, xi, k] = 1f;
    }
data.SetAlphamaps(0, 0, alpha);
UnityEditor.EditorUtility.SetDirty(data);

var terrainGo = UnityEngine.Terrain.CreateTerrainGameObject(data);
terrainGo.name = "Terrain";
terrainGo.transform.position = V(0f, baseY, 0f);
var terrain = terrainGo.GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + baseY;

// ---------- bounds: invisible walls on the north, south and west edges and at the cliff ----------
var bounds = new UnityEngine.GameObject("Bounds");
void Wall(string name, UnityEngine.Vector3 c, UnityEngine.Vector3 s)
{
    var w = new UnityEngine.GameObject(name); w.transform.SetParent(bounds.transform, false);
    w.transform.position = c; w.AddComponent<UnityEngine.BoxCollider>().size = s;
}
Wall("Wall_Cliff", V(10f, 25f, 150f), V(1f, 150f, 300f));
Wall("Wall_West", V(0f, 25f, 150f), V(1f, 150f, 300f));
Wall("Wall_North", V(200f, 25f, 300f), V(400f, 150f, 1f));
Wall("Wall_South", V(200f, 25f, 0f), V(400f, 150f, 1f));
Wall("Wall_GateOpening", V(396f, 25f, 170f), V(1f, 150f, 5f));   // task 8.6 replaces this with the gate barrier

// ---------- fence along x 396, gray, 2.1 m, gap for the gate lane z 167.5 to 172.5 ----------
var fence = new UnityEngine.GameObject("Fence");
void Cube(string name, UnityEngine.Transform parent, UnityEngine.Vector3 c, UnityEngine.Vector3 s)
{
    var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); g.name = name;
    g.transform.SetParent(parent, false); g.transform.position = c; g.transform.localScale = s;
}
void Run(float z0, float z1)
{
    int n = UnityEngine.Mathf.CeilToInt((z1 - z0) / 2.5f); float step = (z1 - z0) / n;
    for (int i = 0; i <= n; i++) { float z = z0 + i * step; Cube("Post", fence.transform, V(396f, H(396f, z) + 1.05f, z), V(0.12f, 2.1f, 0.12f)); }
    for (int i = 0; i < n; i++) { float z = z0 + (i + 0.5f) * step; Cube("Panel", fence.transform, V(396f, H(396f, z) + 1.05f, z), V(0.04f, 2.0f, step)); }
}
Run(0f, 167.5f); Run(172.5f, 300f);

// ---------- lighting: day sun on (Main3.md: day is the sunset from waking), moon off ----------
foreach (var r in scene.GetRootGameObjects()) if (r.name == "NightLighting")
{
    var moon = r.transform.Find("Moon"); var sun = r.transform.Find("Sun");
    if (moon != null) moon.gameObject.SetActive(false);
    if (sun != null) { sun.gameObject.SetActive(true); sun.rotation = UnityEngine.Quaternion.Euler(9f, 250f, 0f); UnityEngine.RenderSettings.sun = sun.GetComponent<UnityEngine.Light>(); }
}

// ---------- spawn: inside the cabin position (178, 168), facing the tower and fire ----------
UnityEngine.GameObject player = null;
foreach (var r in scene.GetRootGameObjects()) if (r.name == "Player") player = r;
if (player == null) return "no Player in the new scene";
player.transform.position = V(178f, H(178f, 168f) + 0.1f, 168f);
player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 250f, 0f);

// ---------- dev warps: every place in Main3.md plus the junctions ----------
var warps = new UnityEngine.GameObject("DevWarps");
void Warp(string name, float x, float z, float lx, float lz)
{
    var w = new UnityEngine.GameObject(name); w.transform.SetParent(warps.transform, false);
    w.transform.position = V(x, H(x, z) + 0.2f, z);
    w.transform.rotation = UnityEngine.Quaternion.LookRotation(V(lx - x, 0f, lz - z).normalized, UnityEngine.Vector3.up);
}
Warp("Keepers_Camp", 172f, 150f, 164f, 166f);
Warp("Lake_Pump", 190f, 99f, 190f, 60f);
Warp("Lake_Boathouse", 232f, 64f, 240f, 52f);
Warp("Camp_1", 268f, 226f, 282f, 238f);
Warp("Camp_2", 268f, 110f, 292f, 108f);
Warp("Camp_3", 74f, 142f, 78f, 146f);
Warp("Camp_3_Rim", 100f, 148f, 78f, 146f);
Warp("Office", 340f, 196f, 350f, 200f);
Warp("Store", 366f, 193f, 366f, 200f);
Warp("Gate_Booth", 388f, 176f, 396f, 170f);
Warp("Closed_Campground", 385f, 232f, 372f, 262f);
Warp("Trailhead_T", 337f, 170f, 358f, 170f);
Warp("Junction_Jg", 262f, 168f, 340f, 170f);
Warp("Junction_J", 106f, 203f, 76f, 223f);
Warp("Junction_W1", 130f, 72f, 190f, 60f);
Warp("Cave_Mouth", 58f, 44f, 52f, 34f);
Warp("Tor", 98f, 240f, 76f, 223f);
Warp("Ward", 40f, 258f, 21.8f, 258f);
Warp("Old_Burn", 230f, 166f, 340f, 178f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var sb = new System.Text.StringBuilder("saved=" + saved + " warps=" + warps.transform.childCount + " fence pieces=" + fence.transform.childCount);
sb.Append(" | ground: camp=" + H(170, 160).ToString("F1") + " tower=" + H(164, 166).ToString("F1") + " camp1=" + H(282, 238).ToString("F1") + " camp2=" + H(292, 108).ToString("F1")
    + " hollow=" + H(78, 146).ToString("F1") + " snag=" + H(90, 146).ToString("F1") + " J=" + H(104, 206).ToString("F1") + " bridge=" + H(104.8f, 203.2f).ToString("F1")
    + " spring=" + H(104, 215.2f).ToString("F1") + " crest=" + H(90, 248).ToString("F1") + " torBase=" + H(76, 223).ToString("F1") + " ward=" + H(32, 258).ToString("F1")
    + " lakeC=" + H(190, 60).ToString("F1") + " pump=" + H(190, 96).ToString("F1") + " W1=" + H(128, 70).ToString("F1") + " sill=" + H(110, 55).ToString("F1")
    + " mouthFloor=" + H(52, 38).ToString("F1") + " overPassage=" + H(52, 28).ToString("F1") + " overChamber=" + H(80, 12).ToString("F1") + " rim74=" + H(74, 60).ToString("F1")
    + " rim40=" + H(40, 57).ToString("F1") + " jg=" + H(262, 172).ToString("F1") + " gateTree=" + H(290, 176).ToString("F1") + " hollowGiant=" + H(202, 140).ToString("F1")
    + " office=" + H(350, 200).ToString("F1") + " valley=" + H(3, 150).ToString("F1") + " cliffTop=" + H(11, 150).ToString("F1"));
sb.Append(" | build list: "); foreach (var s in UnityEditor.EditorBuildSettings.scenes) sb.Append(System.IO.Path.GetFileNameWithoutExtension(s.path) + " ");
return sb.ToString();
