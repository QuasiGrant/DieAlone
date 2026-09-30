// Main3 task 8.7: Ward pieces, gray. Run after 8.6 in Main3, edit mode.
// Cairn gate 4 m up the J to Ward trail: pale cairn on its west side, clear of the Camp to J trail end (Marlow finding 9), and a
// solid chain at 0.9 m across it to a post: the climb is closed by day (DECISIONS 2026-09-25); a later night rule opens it,
// and the dev warps reach the ledge meanwhile. The climb itself is 8.3's J to Ward trail; the ridge, cleft and ledge are 8.1's terrain.
// Ward on the ledge (Valley.md rev 5, 5.6): stones at (-4, 268), (1, 271), (-7, 272), 3.6 x 4 m, tops 110, to the right of the
// path end (-2, 258), all behind the knob from the tower. The rock lip is gone (the ledge edge is the stop).
// The stand-in fire west of the W ridge (Valley.md 3.1, Edges.md 9.3), always on (DECISIONS 2026-09-29: the land hides it):
// a gray floor at -40 from the terrain's west edge out past the far ridge; the far ridge (crest 30, x -300 to -500) with burning
// giants and flame tops at 130; valley fires on the floor at x -50 to -150, z 0 to 500, flame tops 60. Floor and ridge run 500 m
// past the last flame, no colliders. No smoke (Valley.md 3.2 is being redrawn, Wren 2026-09-29).
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("FrontZone") == null) return "run 8.6 first";
if (Root("Ward") != null) return "Ward already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P2(float x, float z) => new UnityEngine.Vector2(x, z);
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.GameObject Prim(UnityEngine.PrimitiveType t, string name, UnityEngine.Transform parent, UnityEngine.Vector3 wp, UnityEngine.Vector3 sc, float yaw = 0f, bool collider = true)
{
    var g = UnityEngine.GameObject.CreatePrimitive(t); g.name = name; g.transform.SetParent(parent, false); g.transform.position = wp; g.transform.localScale = sc;
    g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    if (!collider) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());
    return g;
}
var Cube = UnityEngine.PrimitiveType.Cube; var Sph = UnityEngine.PrimitiveType.Sphere;
var ward = new UnityEngine.GameObject("Ward").transform;

// the J to Ward trail start, for placing the cairn gate across it
var legT = Root("Trails").transform.Find("J to Ward"); if (legT == null) return "no J to Ward trail";
var p0 = legT.GetChild(0).position; var p1 = legT.GetChild(2).position;
var along = V(p1.x - p0.x, 0f, p1.z - p0.z).normalized; var side = V(along.z, 0f, -along.x);
var gateAt = p0 + along * 4.0f;
var cairnGate = new UnityEngine.GameObject("CairnGate").transform; cairnGate.SetParent(ward, false);
var cairnPos = gateAt - side * 2.2f; cairnPos.y = H(cairnPos.x, cairnPos.z);
float cy = cairnPos.y;
// pale matte stone (8.12 fix): the default Lit primitive material is smooth and mirrors the blue default reflection, so the cairn read see-through
var cairnStone = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Concrete034_1.0x1.0.mat"); if (cairnStone == null) return "no Concrete034_1.0x1.0.mat";
foreach (var s in new[] { (1.4f, 0.5f), (1.1f, 0.45f), (0.85f, 0.4f), (0.6f, 0.35f), (0.4f, 0.3f) })
{ Prim(Sph, "Stone", cairnGate, V(cairnPos.x, cy + s.Item2 * 0.45f, cairnPos.z), V(s.Item1, s.Item2, s.Item1)).GetComponent<UnityEngine.MeshRenderer>().sharedMaterial = cairnStone; cy += s.Item2 * 0.8f; }
var postPos = gateAt + side * 2.2f; postPos.y = H(postPos.x, postPos.z);
Prim(Cube, "ChainPost", cairnGate, postPos + V(0f, 0.55f, 0f), V(0.15f, 1.1f, 0.15f));
var chainA = cairnPos + V(0f, 0.9f, 0f); var chainB = postPos + V(0f, 0.9f, 0f);
var chain = Prim(Cube, "Chain", cairnGate, (chainA + chainB) * 0.5f, V(0.06f, 0.06f, UnityEngine.Vector3.Distance(chainA, chainB)), 0f, true);   // solid: the climb is closed by day
chain.transform.rotation = UnityEngine.Quaternion.LookRotation(chainB - chainA, UnityEngine.Vector3.up);
// the gate proper (8.9b): an invisible block 3 m tall across the whole trail corridor and into the side walls, on the
// Ignore Raycast layer, so the climb cannot be crouched under the chain, jumped over it or passed round the cairn end
var gateBlock = new UnityEngine.GameObject("GateBlocker"); gateBlock.transform.SetParent(cairnGate, false); gateBlock.layer = 2;
gateBlock.transform.position = V(gateAt.x, H(gateAt.x, gateAt.z) + 1.2f, gateAt.z); gateBlock.transform.rotation = UnityEngine.Quaternion.LookRotation(along, UnityEngine.Vector3.up);
gateBlock.AddComponent<UnityEngine.BoxCollider>().size = V(6.0f, 3.4f, 0.5f);


// Ward stones (Valley.md rev 5, 5.6): on the ledge, to the right of the path end, facing the fire
const float stoneTop = 110f;
var stones = new UnityEngine.GameObject("Stones").transform; stones.SetParent(ward, false);
var stonePos = new[] { P2(-4f, 268f), P2(1f, 271f), P2(-7f, 272f) };
for (int n = 0; n < stonePos.Length; n++)
{
    float g = H(stonePos[n].x, stonePos[n].y);
    Prim(Cube, "Stone_" + (n + 1), stones, V(stonePos[n].x, g + (stoneTop - g) * 0.5f - 0.5f, stonePos[n].y), V(3.6f, stoneTop - g + 1f, 4f));
}

// ---- the stand-in fire west of the ridge (always on, no colliders, no shadows)
var fire = new UnityEngine.GameObject("StandInFire"); fire.transform.SetParent(ward, false);
var lookT = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (lookT == null) return "no LookTuning";
var backSh = UnityEngine.Shader.Find("DieAlone/Backdrop"); if (backSh == null) return "DieAlone/Backdrop shader not found";
var grayMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_WestGround.mat");
if (grayMat == null) { grayMat = new UnityEngine.Material(backSh); UnityEditor.AssetDatabase.CreateAsset(grayMat, "Assets/Materials/Blockout/Blockout_WestGround.mat"); }
grayMat.shader = backSh; grayMat.SetColor("_Color", lookT.backdropGroundColor); grayMat.SetFloat("_HazeBlend", lookT.backdropGroundHaze); UnityEditor.EditorUtility.SetDirty(grayMat);
const string fireDir = "Assets/Terrain/Main3/Fire";
if (!UnityEditor.AssetDatabase.IsValidFolder(fireDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Terrain/Main3", "Fire");
var rng = new System.Random(8013);
float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
// the far side: floor at -40 (a 2 m roll, zero at the terrain's edge, so it is land and not a flat plane) and the far ridge
const float floorY = -40f, floorRoll = 2f, floorX0 = -1100f, floorX1 = -40f, floorZ0 = -1000f, floorZ1 = 1250f, floorStep = 50f;
const float ridgeX0 = -500f, ridgeX1 = -300f, ridgeZ0 = -750f, ridgeZ1 = 1250f, ridgeStep = 20f, ridgeCrestLo = 25f, ridgeCrest = 30f;
float FloorY(float x, float z) => floorY + floorRoll * UnityEngine.Mathf.PerlinNoise(x / 170f + 2.3f, z / 170f + 8.1f) * UnityEngine.Mathf.Clamp01((floorX1 - x) / floorStep);
float RidgeY(float x, float z)
{
    if (x <= ridgeX0 || x >= ridgeX1) return FloorY(x, z);
    float u = (x - (ridgeX0 + ridgeX1) * 0.5f) / ((ridgeX1 - ridgeX0) * 0.5f), crest = UnityEngine.Mathf.Lerp(ridgeCrestLo, ridgeCrest, UnityEngine.Mathf.PerlinNoise(z / 140f + 4.4f, 0.37f));
    return UnityEngine.Mathf.Max(FloorY(x, z), floorY + (crest - floorY) * UnityEngine.Mathf.Pow(1f - u * u, 0.8f));
}
UnityEngine.GameObject Grid(string name, float x0, float x1, float z0, float z1, float step, System.Func<float, float, float> y)
{
    int cols = UnityEngine.Mathf.CeilToInt((x1 - x0) / step) + 1, rows = UnityEngine.Mathf.CeilToInt((z1 - z0) / step) + 1;
    var vs = new UnityEngine.Vector3[cols * rows]; var tris = new System.Collections.Generic.List<int>();
    for (int r = 0; r < rows; r++) for (int c = 0; c < cols; c++) { float x = UnityEngine.Mathf.Min(x0 + c * step, x1), z = UnityEngine.Mathf.Min(z0 + r * step, z1); vs[r * cols + c] = V(x, y(x, z), z); }
    for (int r = 0; r < rows - 1; r++) for (int c = 0; c < cols - 1; c++) { int a = r * cols + c, b = a + 1, d = a + cols, e = d + 1; tris.AddRange(new[] { a, d, b, b, d, e }); }
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.vertices = vs; mesh.triangles = tris.ToArray(); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, fireDir + "/" + name + ".asset");
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(fire.transform, false);
    g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; var mr = g.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = grayMat;
    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false; return g;
}
Grid("ValleyFloor", floorX0, floorX1, floorZ0, floorZ1, floorStep, FloorY);
Grid("Ridge", ridgeX0, ridgeX1, ridgeZ0, ridgeZ1, ridgeStep, RidgeY);
// ---- flames as textured cards (8.9f, Vesper): vertical quads from an 8 x 8 flipbook of the Fire & Smoke pack on DieAlone/FlameCard
// (additive, no fog), combined per group into mesh assets. Measured in 8.9k (frames 16 to 47 of the flipbook, lit rows): the flame reaches
// up to 0.93 of its card (frame 47), so a card is only as tall as its flame top: it rises from its base b to the stated top T.
const string nmDir = "Assets/NatureManufacture Assets/Fire and Smoke Particles/";
var flameTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(nmDir + "Textures/T_fire_flipbook_big_01.png");
var cardSh = UnityEngine.Shader.Find("DieAlone/FlameCard");
if (flameTex == null || cardSh == null) return "fire card assets missing";
var flameMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_FlameCard.mat");
if (flameMat == null) { flameMat = new UnityEngine.Material(cardSh); UnityEditor.AssetDatabase.CreateAsset(flameMat, "Assets/Materials/Blockout/Blockout_FlameCard.mat"); }
flameMat.shader = cardSh; flameMat.SetTexture("_BaseMap", flameTex); flameMat.SetColor("_Color", UnityEngine.Color.white); flameMat.SetFloat("_Intensity", lookT.fireGlowIntensity); UnityEditor.EditorUtility.SetDirty(flameMat);
const int sheetTiles = 8;   // the pack flipbooks are 8 x 8
const float flameShare = 1f;   // the card top is the flame top (was 2/3, an unmeasured guess; 8.9k)
var faceTo = new UnityEngine.Vector2(-2f, 258f);   // cards turn toward the path end on the ledge
UnityEngine.GameObject Cards(string name, UnityEngine.Transform parent, System.Collections.Generic.List<(UnityEngine.Vector3 b, float w, float h, int frame, float lean, float alpha)> cs, UnityEngine.Material mat, int tiles)
{
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var uvs = new System.Collections.Generic.List<UnityEngine.Vector2>();
    var cols = new System.Collections.Generic.List<UnityEngine.Color>(); var tris = new System.Collections.Generic.List<int>();
    foreach (var c in cs)
    {
        var n = new UnityEngine.Vector2(faceTo.x - c.b.x, faceTo.y - c.b.z).normalized; var right = V(n.y, 0f, -n.x) * (c.w * 0.5f);
        var up = V(c.lean, c.h, 0f); int i0 = vs.Count;
        vs.Add(c.b - right); vs.Add(c.b + right); vs.Add(c.b - right + up); vs.Add(c.b + right + up);
        float s = 1f / tiles, u0 = (c.frame % tiles) * s, v0 = (tiles - 1 - c.frame / tiles) * s;
        uvs.Add(new UnityEngine.Vector2(u0, v0)); uvs.Add(new UnityEngine.Vector2(u0 + s, v0)); uvs.Add(new UnityEngine.Vector2(u0, v0 + s)); uvs.Add(new UnityEngine.Vector2(u0 + s, v0 + s));
        for (int k = 0; k < 4; k++) cols.Add(new UnityEngine.Color(1f, 1f, 1f, c.alpha));
        tris.AddRange(new[] { i0, i0 + 2, i0 + 1, i0 + 1, i0 + 2, i0 + 3 });
    }
    var mesh = new UnityEngine.Mesh { name = name, indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.SetVertices(vs); mesh.SetUVs(0, uvs); mesh.SetColors(cols); mesh.SetTriangles(tris, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, fireDir + "/" + name + ".asset");
    var g = new UnityEngine.GameObject(name); g.transform.SetParent(parent, false);
    g.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; var mr = g.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = mat;
    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false; return g;
}
int FlameFrame() => 16 + rng.Next(32);   // the middle rows: full flames, not the first flicker or the dying tail
float CardH(float baseY, float top) => (top - baseY) / flameShare;
// the far front: burning giants on the far ridge (the pack's dead tree stretched to a giant), flame tops 130
const string deadTreePath = "Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead.prefab";
var deadTree = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(deadTreePath); if (deadTree == null) return "dead tree prefab missing";
const float deadTreeGirth = 0.6f, giantGirth = 6f;   // the pack tree's trunk is about 0.6 m across; a giant's about 6 m
const float frontZ0 = -250f, frontZ1 = 750f, frontX0 = -470f, frontX1 = -330f, frontTop = 130f, frontStep = 30f, flameSink = 8f;
float deadTreeTall = 0f; foreach (var rr in deadTree.GetComponentsInChildren<UnityEngine.Renderer>()) deadTreeTall = UnityEngine.Mathf.Max(deadTreeTall, rr.bounds.max.y);
var giants = new UnityEngine.GameObject("BurningGiants").transform; giants.SetParent(fire.transform, false);
var flameCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
for (float z = frontZ0; z <= frontZ1; z += frontStep)
{
    float x = R(frontX0, frontX1), baseY = RidgeY(x, z), tall = R(40f, 50f);
    var t = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(deadTree, giants); t.name = "Trunk";
    t.transform.position = V(x, baseY, z); t.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
    float sxz = giantGirth / deadTreeGirth, sy = tall / UnityEngine.Mathf.Max(0.5f, deadTreeTall); t.transform.localScale = V(sxz, sy, sxz);
    foreach (var c in t.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    foreach (var rr in t.GetComponentsInChildren<UnityEngine.Renderer>()) rr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    for (int k = 0; k < 3; k++) { float fb = baseY - flameSink; flameCards.Add((V(x + R(-6f, 6f), fb, z + R(-10f, 10f)), R(40f, 56f), CardH(fb, frontTop) * R(0.85f, 1f), FlameFrame(), R(-4f, 4f), 1f)); }
}
Cards("RidgeFlames", giants, flameCards, flameMat, sheetTiles);
// the valley fires on the -40 floor, x -50 to -150, z 0 to 500, flame tops 60
const float valleyX0 = -150f, valleyX1 = -50f, valleyZ0 = 0f, valleyZ1 = 500f, valleyTop = 60f, valleyStepX = 20f, valleyStepZ = 26f, valleyJitter = 8f;
var valley = new UnityEngine.GameObject("ValleyFires").transform; valley.SetParent(fire.transform, false);
var valleyCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
for (float z = valleyZ0; z <= valleyZ1; z += valleyStepZ)
    for (float x = valleyX1; x >= valleyX0; x -= valleyStepX)
    {
        float fx = UnityEngine.Mathf.Clamp(x + R(-valleyJitter, valleyJitter), valleyX0, valleyX1), fz = UnityEngine.Mathf.Clamp(z + R(-valleyJitter, valleyJitter), valleyZ0, valleyZ1), fb = FloorY(fx, fz) - 2f;
        valleyCards.Add((V(fx, fb, fz), R(40f, 56f), CardH(fb, valleyTop) * R(0.85f, 1f), FlameFrame(), 0f, 1f));
    }
Cards("ValleyFlames", valley, valleyCards, flameMat, sheetTiles);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cairn at " + cairnPos.ToString("F1") + " solid chain 4 m up the trail | stones ground " + H(-4f, 268f).ToString("F1") + " tops " + stoneTop + " | stand-in fire built, on: floor " + floorY + ", far ridge crest " + ridgeCrest + ", " + flameCards.Count + " front cards to " + frontTop + ", " + valleyCards.Count + " valley cards to " + valleyTop;
