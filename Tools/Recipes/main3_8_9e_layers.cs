// Main3 task 8.9e: distance layers (Main3.md revision 15, 2.11; Vesper's layer spec). Run after 8.8 in Main3, edit mode.
// Off-map, no colliders, root "Backdrop": near and far hill bands north, south and east (rolling bands, never cones) on
// DieAlone/Backdrop, each blended a fixed share toward the fog colour (near 55 percent, far 80, from LookTuning); the rolling
// forest east of the fence with a gap for the road; off-map ground and a skirt under the map edge; edge forest strips
// 60 to 100 m deep outside the north and south edges (Redwood pack firs and pines, colliders removed). The player camera's
// far clip reaches the far ranges (2.5 km). The night look's fog (LookTuning.asset) starts near: #05080D from 8 to 60 m;
// the day looks' fog is set with the other look values in 8.9d. Meshes are saved under Assets/Terrain/Main3/Backdrop.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Cave") == null) return "run 8.8 first";
if (Root("Backdrop") != null) return "Backdrop already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var look = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (look == null) return "no LookTuning";
const float mapX = 400f, mapZ = 300f;
const string meshDir = "Assets/Terrain/Main3/Backdrop";
if (!UnityEditor.AssetDatabase.IsValidFolder(meshDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Terrain/Main3", "Backdrop");
var root = new UnityEngine.GameObject("Backdrop").transform;

// ---------- materials ----------
var sh = UnityEngine.Shader.Find("DieAlone/Backdrop"); if (sh == null) return "DieAlone/Backdrop shader not found";
UnityEngine.Material Mat(string name, UnityEngine.Color c, float haze)
{
    string path = "Assets/Materials/Blockout/" + name + ".mat";
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(sh); UnityEditor.AssetDatabase.CreateAsset(m, path); }
    m.shader = sh; m.SetColor("_Color", c); m.SetFloat("_HazeBlend", haze); UnityEditor.EditorUtility.SetDirty(m); return m;
}
var nearForest = Mat("Backdrop_NearForest", look.backdropForestColor, look.backdropNearHaze);
var farRange = Mat("Backdrop_FarRange", look.backdropRangeColor, look.backdropFarHaze);
var groundMat = Mat("Backdrop_Ground", look.backdropGroundColor, look.backdropGroundHaze);

// ---------- mesh helpers ----------
int meshN = 0;
UnityEngine.GameObject MeshObject(string name, UnityEngine.Vector3[] verts, int cols, int rows, UnityEngine.Material mat)
{
    var tris = new System.Collections.Generic.List<int>();
    for (int r = 0; r < rows - 1; r++) for (int c = 0; c < cols - 1; c++)
    { int a = r * cols + c, b = a + 1, d = a + cols, e = d + 1; tris.AddRange(new[] { a, d, b, b, d, e }); }
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.vertices = verts; mesh.triangles = tris.ToArray(); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    // both windings face up or toward the map, whichever way a band runs
    var nrm = mesh.normals; bool flip = false; foreach (var n in nrm) { if (n.y < 0f) flip = true; break; }
    if (flip) { var t = mesh.triangles; for (int i = 0; i < t.Length; i += 3) { int s = t[i + 1]; t[i + 1] = t[i + 2]; t[i + 2] = s; } mesh.triangles = t; mesh.RecalculateNormals(); }
    UnityEditor.AssetDatabase.CreateAsset(mesh, meshDir + "/" + name + ".asset"); meshN++;
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(root, false);
    g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; var mr = g.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = mat;
    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false;
    return g;
}
float Noise(float s, float seed) => UnityEngine.Mathf.PerlinNoise(s, seed);
// a rolling hill band beside one map edge: along the edge (s), outward from it (o); the crest wanders between the given
// distances and heights, the front rises from the ground under the edge, the back falls away below it
UnityEngine.GameObject Band(string name, UnityEngine.Vector2 origin, UnityEngine.Vector2 alongDir, UnityEngine.Vector2 outDir, float s0, float s1, float step,
    float crestD0, float crestD1, float crestH0, float crestH1, float frontD, float frontH, float backD, float backH, float wave, float seed, UnityEngine.Material mat)
{
    int cols = UnityEngine.Mathf.CeilToInt((s1 - s0) / step) + 1; float[] across = { 0f, 0.35f, 0.7f, 1f, 1.3f, 1.8f }; int rows = across.Length;
    var verts = new UnityEngine.Vector3[cols * rows];
    for (int c = 0; c < cols; c++)
    {
        float s = s0 + c * step;
        float cd = UnityEngine.Mathf.Lerp(crestD0, crestD1, Noise(s / wave, seed));
        float ch = UnityEngine.Mathf.Lerp(crestH0, crestH1, Noise(s / (wave * 0.6f), seed + 7.3f));
        for (int r = 0; r < rows; r++)
        {
            float a = across[r], o, y;
            if (a <= 1f) { o = UnityEngine.Mathf.Lerp(frontD, cd, a); y = UnityEngine.Mathf.Lerp(frontH, ch, UnityEngine.Mathf.SmoothStep(0f, 1f, a)); }
            else { float b = (a - 1f) / 0.8f; o = UnityEngine.Mathf.Lerp(cd, backD, b); y = UnityEngine.Mathf.Lerp(ch, backH, b * b); }
            var p = origin + alongDir * s + outDir * o; verts[r * cols + c] = V(p.x, y, p.y);
        }
    }
    return MeshObject(name, verts, cols, rows, mat);
}

// ---------- the bands (Main3.md 2.11 table) ----------
var N = new UnityEngine.Vector2(0f, 1f); var S = new UnityEngine.Vector2(0f, -1f); var E = new UnityEngine.Vector2(1f, 0f);
var alongX = new UnityEngine.Vector2(1f, 0f); var alongZ = new UnityEngine.Vector2(0f, 1f);
float northEdge = float.MaxValue, southEdge = float.MaxValue, eastEdge = float.MaxValue;
for (float x = 140f; x <= mapX; x += 5f) northEdge = UnityEngine.Mathf.Min(northEdge, H(x, mapZ - 0.5f));   // east of the plateau
for (float x = 12f; x <= mapX; x += 5f) southEdge = UnityEngine.Mathf.Min(southEdge, H(x, 0.5f));
for (float z = 0f; z <= mapZ; z += 5f) eastEdge = UnityEngine.Mathf.Min(eastEdge, H(mapX - 0.5f, z));
const float groundSink = 0.5f;   // off-map ground sits just under the lowest edge ground, so no seam floats
float gN = northEdge - groundSink, gS = southEdge - groundSink, gE = eastEdge - groundSink;
// north: near ridge 120 to 250 m out, crest 45 to 60, forested; far range 1.2 to 1.8 km out, crest 150 to 220
Band("North_Near", new UnityEngine.Vector2(0f, mapZ), alongX, N, -500f, 900f, 20f, 120f, 250f, 45f, 60f, 100f, gN, 420f, gN - 20f, 180f, 1.3f, nearForest);
Band("North_Far", new UnityEngine.Vector2(0f, mapZ), alongX, N, -2500f, 2900f, 60f, 1200f, 1800f, 150f, 220f, 700f, gN, 2300f, gN - 40f, 700f, 2.1f, farRange);
// south, beyond the lake: low ridge 150 to 300 m out, crest 35 to 50; far range 1.5 to 2 km out, crest 180 to 250
Band("South_Near", new UnityEngine.Vector2(0f, 0f), alongX, S, -500f, 900f, 20f, 150f, 300f, 35f, 50f, 100f, gS, 480f, gS - 20f, 200f, 3.7f, nearForest);
Band("South_Far", new UnityEngine.Vector2(0f, 0f), alongX, S, -2500f, 2900f, 60f, 1500f, 2000f, 180f, 250f, 900f, gS, 2500f, gS - 40f, 800f, 4.9f, farRange);
// east, beyond the fence: far range 1.5 to 2.5 km out, crest 150 to 200
Band("East_Far", new UnityEngine.Vector2(mapX, 0f), alongZ, E, -2500f, 2800f, 60f, 1500f, 2500f, 150f, 200f, 800f, gE, 3000f, gE - 40f, 900f, 6.1f, farRange);
// east: rolling forest 20 to 35 m for 100 to 400 m beyond the fence, a gap where the road comes in (z 170)
const float forestX0 = 410f, forestX1 = 800f, forestZ0 = -400f, forestZ1 = 700f, canopyLow = 20f, canopyHigh = 35f, canopyStep = 10f, roadZ = 170f, roadGap = 9f;
{
    int cols = UnityEngine.Mathf.CeilToInt((forestX1 - forestX0) / canopyStep) + 1, rows = UnityEngine.Mathf.CeilToInt((forestZ1 - forestZ0) / canopyStep) + 1;
    var verts = new UnityEngine.Vector3[cols * rows];
    for (int r = 0; r < rows; r++) for (int c = 0; c < cols; c++)
    {
        float x = forestX0 + c * canopyStep, z = forestZ0 + r * canopyStep;
        float top = gE + UnityEngine.Mathf.Lerp(canopyLow, canopyHigh, Noise(x / 60f + 3.3f, z / 60f + 1.1f) * 0.7f + Noise(x / 17f, z / 17f + 9f) * 0.3f);
        float edge = UnityEngine.Mathf.Clamp01((x - forestX0) / canopyStep);          // the front of the forest rises from the ground
        float gap = UnityEngine.Mathf.Clamp01((UnityEngine.Mathf.Abs(z - roadZ) - roadGap) / canopyStep);   // the road's cut through the trees
        verts[r * cols + c] = V(x, UnityEngine.Mathf.Lerp(gE, top, edge * gap), z);
    }
    MeshObject("East_Forest", verts, cols, rows, nearForest);
}
// off-map ground: north, south and east of the map, and a skirt under each edge down to it
UnityEngine.GameObject Plane(string name, float x0, float x1, float z0, float z1, float y)
{
    var verts = new[] { V(x0, y, z0), V(x1, y, z0), V(x0, y, z1), V(x1, y, z1) };
    return MeshObject(name, verts, 2, 2, groundMat);
}
Plane("Ground_North", -500f, 900f, mapZ, 700f, gN); Plane("Ground_South", -500f, 900f, -700f, 0f, gS); Plane("Ground_East", mapX, 1000f, 0f, mapZ, gE);
UnityEngine.GameObject Skirt(string name, System.Func<float, UnityEngine.Vector2> at, float len, float step, float bottom)
{
    int cols = UnityEngine.Mathf.CeilToInt(len / step) + 1; var verts = new UnityEngine.Vector3[cols * 2];
    for (int c = 0; c < cols; c++) { var p = at(UnityEngine.Mathf.Min(c * step, len)); verts[c] = V(p.x, bottom, p.y); verts[cols + c] = V(p.x, H(p.x, p.y), p.y); }
    return MeshObject(name, verts, cols, 2, groundMat);
}
const float skirtStep = 2f;
Skirt("Skirt_North", s => new UnityEngine.Vector2(10f + s, mapZ - 0.01f), mapX - 10f, skirtStep, gN - 2f);
Skirt("Skirt_South", s => new UnityEngine.Vector2(10f + s, 0.01f), mapX - 10f, skirtStep, gS - 2f);
Skirt("Skirt_East", s => new UnityEngine.Vector2(mapX - 0.01f, s), mapZ, skirtStep, gE - 2f);

// ---------- edge forest strips (60 to 100 m deep outside the north and south edges) ----------
const string BK = "Assets/BK/PureNature_Redwood/Prefabs/Trees/";
const float stripDepth0 = 60f, stripDepth1 = 100f, stripSpacing = 11f, stripJitter = 4f, stripTreeH0 = 20f, stripTreeH1 = 35f;
var rng = new System.Random(8905); float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
var strips = new UnityEngine.GameObject("EdgeForest").transform; strips.SetParent(root, false); int treeN = 0; var missing = "";
foreach (var (edgeZ, dir, gy) in new[] { (mapZ, 1f, gN), (0f, -1f, gS) })
{
    for (float x = -20f; x <= mapX + 20f; x += stripSpacing)
    {
        float depth = UnityEngine.Mathf.Lerp(stripDepth0, stripDepth1, Noise(x / 50f, edgeZ + 2.2f));
        for (float o = 4f; o <= depth; o += stripSpacing)
        {
            string path = BK + (rng.NextDouble() < 0.6 ? "RedFir" + (1 + rng.Next(8)) : "RedPine" + (1 + rng.Next(5))) + ".prefab";
            var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path); if (src == null) { missing += path + " "; continue; }
            var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src, strips);
            foreach (var col in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(col);
            float px = x + R(-stripJitter, stripJitter), pz = edgeZ + dir * (o + R(-stripJitter, stripJitter));
            g.transform.position = V(px, gy, pz); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
            var b = g.GetComponentInChildren<UnityEngine.Renderer>().bounds; float hgt = 0f; foreach (var r2 in g.GetComponentsInChildren<UnityEngine.Renderer>()) hgt = UnityEngine.Mathf.Max(hgt, r2.bounds.max.y - gy);
            float s = R(stripTreeH0, stripTreeH1) / UnityEngine.Mathf.Max(0.5f, hgt); g.transform.localScale = V(s, s, s); treeN++;
        }
    }
}

// ---------- camera reach and the night look's fog ----------
const float farClip = 3500f;   // the east far range reaches 2.5 km past the fence
var cam = UnityEngine.Camera.main; if (cam == null) return "no main camera";
cam.farClipPlane = farClip;
look.fogColor = Hex("#05080D"); look.fogStart = 8f; look.fogEnd = 60f; UnityEditor.EditorUtility.SetDirty(look);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | backdrop meshes " + meshN + " | edge forest trees " + treeN + " | off-map ground N " + gN.ToString("F1") + " S " + gS.ToString("F1") + " E " + gE.ToString("F1") + " | far clip " + farClip + " | night fog 8 to 60 | missing: " + (missing == "" ? "none" : missing);
