// Main3 task 8.9e: distance layers (Main3.md revision 15, 2.11; Vesper's layer spec). Run after 8.8 in Main3, edit mode.
// 8.9j (Valley.md rev 5): only the far ranges and the outer ground are left here; the near bands, planes, skirts, east forest and
// edge forest strips described below were removed when the ridges became terrain (8.1). West_Far is new.
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

// ---------- the far ranges (Main3.md 2.11 table), as built in rev 16; 8.9j tucks their lower edges under the outer ground ----------
// 8.9j (Valley.md rev 5, 2.7): the near bands, the rev 16 Ground_North, Ground_South and Ground_East planes, the skirts, the east
// forest and the edge forest strips are gone: the N, S and E ridges and their back slopes are terrain now (8.1), and one outer
// ground mesh covers everything outside the terrain out to the far ranges. The far ranges keep their places (fronts at z 1000,
// z -900, x 1200); their front edge sits at farFrontY, under the outer ground (0 to 10) and the west floor (-40).
var N = new UnityEngine.Vector2(0f, 1f); var S = new UnityEngine.Vector2(0f, -1f); var E = new UnityEngine.Vector2(1f, 0f); var W = new UnityEngine.Vector2(-1f, 0f);
var alongX = new UnityEngine.Vector2(1f, 0f); var alongZ = new UnityEngine.Vector2(0f, 1f);
const float farFrontY = -45f, farBackY = -45f, westExtraHaze = 0.1f;
// north: far range 1.2 to 1.8 km out, crest 150 to 220
Band("North_Far", new UnityEngine.Vector2(0f, mapZ), alongX, N, -2500f, 2900f, 60f, 1200f, 1800f, 150f, 220f, 700f, farFrontY, 2300f, farBackY, 700f, 2.1f, farRange);
// south: far range 1.5 to 2 km out, crest 180 to 250
Band("South_Far", new UnityEngine.Vector2(0f, 0f), alongX, S, -2500f, 2900f, 60f, 1500f, 2000f, 180f, 250f, 900f, farFrontY, 2500f, farBackY, 800f, 4.9f, farRange);
// east, beyond the fence: far range 1.5 to 2.5 km out, crest 150 to 200
Band("East_Far", new UnityEngine.Vector2(mapX, 0f), alongZ, E, -2500f, 2800f, 60f, 1500f, 2500f, 150f, 200f, 800f, farFrontY, 3000f, farBackY, 900f, 6.1f, farRange);
// west, beyond the fire's far ridge (8.9j, Rook): the same far range as the east, so a look west from the ledge below level ends on
// land, not void (E-1); hidden from every other place by the W ridge. Proposed, not in Valley.md or Edges.md: Vesper and Sable confirm.
// Vesper 2026-09-29: keep it low and hazed, a flat silhouette whose top stays below the day-two fire glow (flame tops 130 at
// 300 to 500 m from the ledge stand about 4 degrees up; this crest, 105 to 120 at 1.5 to 2.5 km, stays under 1 degree)
var westFar = Mat("Backdrop_WestFar", look.backdropRangeColor, UnityEngine.Mathf.Min(1f, look.backdropFarHaze + westExtraHaze));
Band("West_Far", new UnityEngine.Vector2(-40f, 0f), alongZ, W, -2500f, 2800f, 60f, 1500f, 2500f, 105f, 120f, 800f, farFrontY, 3000f, farBackY, 900f, 7.7f, westFar);

// ---------- the outer ground (Valley.md rev 5, 2.7): one mesh, x -40 to 1200, z -900 to 1000, outside the terrain ----------
// Floor(), EastHills() and Outer() are copies of 8.1's (change both together): gently rolling 0 to 10; west of x 0 a 45 degree step down to the
// -40 floor at x -40, the same slope as the terrain's, so no ray can pass under it. Vertices inside the terrain sit 2 m under the
// ground there, so the mesh runs under the terrain's edge with no crack; cells well inside the terrain are left out.
float Floor(float x, float z) => UnityEngine.Mathf.Clamp(5f + 4.5f * (UnityEngine.Mathf.PerlinNoise(x / 280f + 11.3f, z / 280f + 4.7f) * 2f - 1f), 0f, 10f);
// 8.14 (Valley.md rev 10, 3.5): east of the highway the land rolls up to 20 to 35 m (EastHills), as in 8.1
const float hillX0 = 520f, hillRamp = 100f, hillLow = 20f, hillSpan = 15f, hillScale = 160f;
float EastHills(float x, float z) => UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01((x - hillX0) / hillRamp)) * (hillLow + hillSpan * UnityEngine.Mathf.PerlinNoise(x / hillScale + 2.2f, z / hillScale + 6.1f));
float Outer(float x, float z) => x >= 0f ? Floor(x, z) + EastHills(x, z) : UnityEngine.Mathf.Lerp(-40f, Floor(0f, z), (x + 40f) / 40f);
const float outerX0 = -40f, outerX1 = 1200f, outerZ0 = -900f, outerZ1 = 1000f, outerStep = 20f, underTerrain = 2f, keepUnder = 40f;
var tp = terrain.transform.position; var ts = terrain.terrainData.size;
float tX0 = tp.x, tX1 = tp.x + ts.x, tZ0 = tp.z, tZ1 = tp.z + ts.z;
var xsL = new System.Collections.Generic.SortedSet<float>(); var zsL = new System.Collections.Generic.SortedSet<float>();
for (float x = outerX0; x <= outerX1 + 0.01f; x += outerStep) xsL.Add(x); for (float z = outerZ0; z <= outerZ1 + 0.01f; z += outerStep) zsL.Add(z);
xsL.Add(tX1); zsL.Add(tZ0); zsL.Add(tZ1);   // grid lines on the terrain's edges
var xs = new System.Collections.Generic.List<float>(xsL); var zs = new System.Collections.Generic.List<float>(zsL);
bool InTerrain(float x, float z) => x > tX0 + 0.01f && x < tX1 - 0.01f && z > tZ0 + 0.01f && z < tZ1 - 0.01f;
{
    int cols = xs.Count, rows = zs.Count; var verts = new UnityEngine.Vector3[cols * rows]; var tris = new System.Collections.Generic.List<int>();
    for (int r = 0; r < rows; r++) for (int c = 0; c < cols; c++) { float x = xs[c], z = zs[r]; verts[r * cols + c] = V(x, Outer(x, z) - (InTerrain(x, z) ? underTerrain : 0f), z); }
    for (int r = 0; r < rows - 1; r++) for (int c = 0; c < cols - 1; c++)
    {
        float cx = (xs[c] + xs[c + 1]) * 0.5f, cz = (zs[r] + zs[r + 1]) * 0.5f;
        if (cx > tX0 + keepUnder && cx < tX1 - keepUnder && cz > tZ0 + keepUnder && cz < tZ1 - keepUnder) continue;   // well inside the terrain
        int a = r * cols + c, b = a + 1, d = a + cols, e = d + 1; tris.AddRange(new[] { a, d, b, b, d, e });
    }
    var mesh = new UnityEngine.Mesh { name = "OuterGround", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.vertices = verts; mesh.triangles = tris.ToArray(); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, meshDir + "/OuterGround.asset"); meshN++;
    var g = new UnityEngine.GameObject("OuterGround"); g.transform.SetParent(root, false);
    g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; var mr = g.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = groundMat;
    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false;
}
string outerBox = "x " + outerX0 + " to " + outerX1 + ", z " + outerZ0 + " to " + outerZ1 + " (hole inside the terrain " + tX0 + " to " + tX1 + ", " + tZ0 + " to " + tZ1 + ")";
string gone = ""; foreach (var n in new[] { "Ground_North", "Ground_South", "Ground_East", "North_Near", "South_Near", "East_Forest", "EdgeForest" }) if (root.Find(n) != null) gone += n + " ";
int treeN = 0; var missing = "";

// ---------- camera reach and the night look's fog ----------
const float farClip = 3500f;   // the east far range reaches 2.5 km past the fence
var cam = UnityEngine.Camera.main; if (cam == null) return "no main camera";
cam.farClipPlane = farClip;
look.fogColor = Hex("#05080D"); look.fogStart = 8f; look.fogEnd = 60f; UnityEditor.EditorUtility.SetDirty(look);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | backdrop meshes " + meshN + " | outer ground " + outerBox + " | rev 16 planes and near bands left: " + (gone == "" ? "none" : gone) + " | far range fronts at " + farFrontY + " | far clip " + farClip + " | night fog 8 to 60 | missing: " + (missing == "" ? "none" : missing);
