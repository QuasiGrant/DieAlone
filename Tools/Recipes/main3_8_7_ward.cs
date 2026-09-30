// Main3 task 8.7: Ward pieces, gray. Run after 8.6 in Main3, edit mode.
// 8.14 (Valley.md rev 10): the cairn 2 m up the approach from J; the chain and IW2 across the 3 m gap in the W foot rock band at the
// chute mouth (the climb is closed by day, DECISIONS 2026-09-25 and 2026-09-30; a later night rule opens it, the dev warps reach the
// ledge meanwhile). The climb itself is 8.3's J to Ward trail on 8.1's carved ground and rock; here are its gray pieces: the cut steps
// of the chute, a landmark per leg, a hiding place per platform. Stones on the ledge at (-4, 226), (1, 223), (-7, 221), 3.6 x 4 m,
// tops 70, left of the path end (-8.5, 246), behind the knob from the tower. The rev 7 gate, stones and fire are in git history.
// The stand-in fire west of the W ridge (Valley.md 5), always on (DECISIONS 2026-09-29: the land hides it): a gray floor at -40
// from the terrain's west edge out past the far ridge; the far ridge rising from the floor at x -200 to its crest (30) at x -240;
// the far front on it, x -240 to -400, z -60 to 650 (rev 10 trims the south end; Wren 2026-09-30 keeps the north end at 650), burning
// giants and flame tops at 105; valley fires on the floor at x -110 to -220, z 0 to 500, flame tops 20; the day-one smoke sheet
// streaming west, top 60 at x -110 rising to 110 at x -500, z -60 to 400, shown in the day-one look only. No colliders.
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
var Cube = UnityEngine.PrimitiveType.Cube; var Sph = UnityEngine.PrimitiveType.Sphere; var Cyl = UnityEngine.PrimitiveType.Cylinder;
var ward = new UnityEngine.GameObject("Ward").transform;

// the cairn at J and the day gate at the chute mouth (Valley.md rev 10, 1.6 and 8): a pale cairn 2 m up the approach from J; the
// chain hangs across the 3 m gap in the W foot rock band between the two rock arms (x 86, z 211.5 to 214.5), and IW2 fills the same
// gap exactly: invisible, on the Ignore Raycast layer, taller than the arms, so the climb is closed by day (a later night rule turns
// CairnGate off). The rev 7 gate across the old trail start is in git history.
var legT = Root("Trails").transform.Find("J to Ward"); if (legT == null) return "no J to Ward trail";
var jPt = legT.GetChild(0).position; var jNext = legT.GetChild(1).position;
var along = V(jNext.x - jPt.x, 0f, jNext.z - jPt.z).normalized; var side = V(along.z, 0f, -along.x);
var cairnGate = new UnityEngine.GameObject("CairnGate").transform; cairnGate.SetParent(ward, false);
const float cairnAlong = 2f, cairnSide = 2.2f;
var cairnPos = jPt + along * cairnAlong + side * cairnSide; cairnPos.y = H(cairnPos.x, cairnPos.z);
float cy = cairnPos.y;
// pale matte stone (8.12 fix): the default Lit primitive material is smooth and mirrors the blue default reflection, so the cairn read see-through
var cairnStone = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Concrete034_1.0x1.0.mat"); if (cairnStone == null) return "no Concrete034_1.0x1.0.mat";
foreach (var s in new[] { (1.4f, 0.5f), (1.1f, 0.45f), (0.85f, 0.4f), (0.6f, 0.35f), (0.4f, 0.3f) })
{ Prim(Sph, "Stone", cairnGate, V(cairnPos.x, cy + s.Item2 * 0.45f, cairnPos.z), V(s.Item1, s.Item2, s.Item1)).GetComponent<UnityEngine.MeshRenderer>().sharedMaterial = cairnStone; cy += s.Item2 * 0.8f; }
var lookCairn = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (lookCairn == null) return "no LookTuning";
// the cairn lamp (Valley.md 1.6: "the cairn lamp at J is lit" at night; 8.14a): an owned lantern on the cairn's top stone with a practical
// light, on at night and off by day (PracticalLight, LookTuning's lantern brightness), so the one night walk has its lead-on
{
    const float lampRange = 8f;
    var lanternPf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Revolving Pizza Games/Campsite/Prefabs/CS_Lantern_Old.prefab"); if (lanternPf == null) return "missing CS_Lantern_Old";
    var lantern = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(lanternPf, cairnGate.parent); lantern.name = "CairnLamp";
    foreach (var c in lantern.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    lantern.transform.position = V(cairnPos.x, cy, cairnPos.z);
    var lg = new UnityEngine.GameObject("CairnLampLight"); lg.transform.SetParent(lantern.transform, false); lg.transform.localPosition = V(0f, 0.3f, 0f);
    var l = lg.AddComponent<UnityEngine.Light>(); l.type = UnityEngine.LightType.Point; l.range = lampRange; l.intensity = lookCairn.firePitIntensity; l.shadows = UnityEngine.LightShadows.None; l.color = lookCairn.practicalColor;
    var pl = lg.AddComponent<PracticalLight>(); var so = new UnityEditor.SerializedObject(pl);
    so.FindProperty("tuning").objectReferenceValue = lookCairn; so.FindProperty("kind").enumValueIndex = (int)PracticalLight.Kind.Lantern; so.FindProperty("byDay").enumValueIndex = (int)PracticalLight.ByDay.Off; so.ApplyModifiedPropertiesWithoutUndo();
}
const float gapX = 86f, gapZ0 = 211.5f, gapZ1 = 214.5f, chainH = 0.9f, iw2Over = 3f, iw2Thick = 0.5f;
float gapG = H(gapX + 0.5f, (gapZ0 + gapZ1) * 0.5f);
var chainA = V(gapX, gapG + chainH, gapZ0); var chainB = V(gapX, gapG + chainH, gapZ1);
var chain = Prim(Cube, "Chain", cairnGate, (chainA + chainB) * 0.5f, V(0.06f, 0.06f, UnityEngine.Vector3.Distance(chainA, chainB)), 0f, false);
chain.transform.rotation = UnityEngine.Quaternion.LookRotation(chainB - chainA, UnityEngine.Vector3.up);
// IW2: from under the ground to iw2Over above the highest rock-arm top beside the gap
float armTop = gapG; foreach (var r in new[] { gapZ0 - 1f, gapZ1 + 1f }) if (UnityEngine.Physics.Raycast(V(gapX + 0.5f, 200f, r), UnityEngine.Vector3.down, out var ah, 300f)) armTop = UnityEngine.Mathf.Max(armTop, ah.point.y);
var gateBlock = new UnityEngine.GameObject("GateBlocker"); gateBlock.transform.SetParent(cairnGate, false); gateBlock.layer = 2;
float iwBot = gapG - 1f, iwTop = armTop + iw2Over;
gateBlock.transform.position = V(gapX, (iwBot + iwTop) * 0.5f, (gapZ0 + gapZ1) * 0.5f);
gateBlock.AddComponent<UnityEngine.BoxCollider>().size = V(iw2Thick, iwTop - iwBot, gapZ1 - gapZ0 + 0.4f);

// the climb's pieces (Valley.md rev 10 section 4), gray stand-ins with no colliders (they must not catch the player): leg 1's three
// flights of 18 cut steps on the chute floor (the ground under them is their nosing line, 8.1), and one landmark per leg and a hiding
// place per platform at their chainage along the trail from J: the seep and tin cup (54), the rune post (88, on the outer edge),
// the split snag (144, the trail passes through it), the bent fir (191, knee height beside the trail); P1's boulder overhang, P2's
// rock roof, P3's root plate
var climbPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in legT) climbPts.Add(m.position);
var climbS = new float[climbPts.Count]; for (int i = 1; i < climbPts.Count; i++) climbS[i] = climbS[i - 1] + UnityEngine.Vector2.Distance(new UnityEngine.Vector2(climbPts[i - 1].x, climbPts[i - 1].z), new UnityEngine.Vector2(climbPts[i].x, climbPts[i].z));
(UnityEngine.Vector3 p, UnityEngine.Vector3 dir) AtS(float s)
{
    int i = 1; while (i < climbPts.Count - 1 && climbS[i] < s) i++;
    float t = UnityEngine.Mathf.InverseLerp(climbS[i - 1], climbS[i], s); var d = climbPts[i] - climbPts[i - 1]; d.y = 0f;
    return (UnityEngine.Vector3.Lerp(climbPts[i - 1], climbPts[i], t), d.normalized);
}
float L2(float a, float b, float t) => a + (b - a) * t;
var climbRoot = new UnityEngine.GameObject("Climb").transform; climbRoot.SetParent(ward, false);
var steps = new UnityEngine.GameObject("ChuteSteps").transform; steps.SetParent(climbRoot, false);
{
    // 8.1's leg 1: from the chute mouth, ramp and flight in turn; the chute runs straight, so the flights start at fixed distances
    var mouth = new UnityEngine.Vector2(86f, 213f); var p1In = new UnityEngine.Vector2(52f, 216f) + (mouth - new UnityEngine.Vector2(52f, 216f)).normalized * 2f;
    const int stepsPerFlight = 18; const float stepRise = 0.25f, stepRun = 0.3f, stepWidth = 3f, stairRampThick = 0.2f;
    float leg1 = UnityEngine.Vector2.Distance(mouth, p1In), flightRun = stepsPerFlight * stepRun, rampRun = (leg1 - 3f * flightRun) / 4f;
    var dir2 = (p1In - mouth).normalized; float yaw = UnityEngine.Mathf.Atan2(dir2.x, dir2.y) * UnityEngine.Mathf.Rad2Deg;
    for (int f = 0; f < 3; f++)
    {
        float s0 = rampRun * (f + 1) + flightRun * f;
        for (int k = 0; k < stepsPerFlight; k++)
        {
            var q = mouth + dir2 * (s0 + (k + 0.5f) * stepRun); float top = H(mouth.x + dir2.x * (s0 + (k + 1) * stepRun), mouth.y + dir2.y * (s0 + (k + 1) * stepRun));
            Prim(Cube, "Step", steps, V(q.x, top - stepRise * 0.5f, q.y), V(stepWidth, stepRise, stepRun), yaw, false);
        }
        // STAIRS RULE: a collider-only StairRamp whose top face runs along the flight's nosing line, from one run below the first
        // nosing to the top nosing (8.14: on the bare terrain the flight's triangles reach past the 45 degree slope limit and stop the player)
        var a2 = mouth + dir2 * (s0 - stepRun); var b2 = mouth + dir2 * (s0 + flightRun);
        var a3 = V(a2.x, H(a2.x, a2.y), a2.y); var b3 = V(b2.x, H(b2.x, b2.y), b2.y); var along3 = b3 - a3;
        var nUp = UnityEngine.Vector3.Cross(along3, V(dir2.y, 0f, -dir2.x)).normalized; if (nUp.y < 0f) nUp = -nUp;
        var ramp = new UnityEngine.GameObject("StairRamp"); ramp.transform.SetParent(steps, false);
        ramp.transform.SetPositionAndRotation((a3 + b3) * 0.5f - nUp * stairRampThick * 0.5f, UnityEngine.Quaternion.LookRotation(along3.normalized, nUp));
        ramp.AddComponent<UnityEngine.BoxCollider>().size = V(stepWidth, stairRampThick, along3.magnitude);
    }
}
void Mark(string name, float s, float sideOff, UnityEngine.PrimitiveType t, UnityEngine.Vector3 size, float up, float tilt = 0f)
{
    var a = AtS(s); var sd = V(a.dir.z, 0f, -a.dir.x); var p = a.p + sd * sideOff; p.y = H(p.x, p.z) + up;
    var g = Prim(t, name, climbRoot, p, size, UnityEngine.Mathf.Atan2(a.dir.x, a.dir.z) * UnityEngine.Mathf.Rad2Deg, false);
    if (tilt != 0f) g.transform.rotation = g.transform.rotation * UnityEngine.Quaternion.Euler(0f, 0f, tilt);
}
// chainage of the trail point nearest a map point; the landmarks sit by 8.1's climb points (the doc's chainages are from the paper map)
float SAt(float x, float z) { int bi = 0; float bd = float.MaxValue; for (int i = 0; i < climbPts.Count; i++) { float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(climbPts[i].x, climbPts[i].z), new UnityEngine.Vector2(x, z)); if (d < bd) { bd = d; bi = i; } } return climbS[bi]; }
const float landmarkShare = 0.6f;
float sP1 = SAt(52f, 216f), sP2 = SAt(57f, 276f), sP3 = SAt(26f, 304f), sP4 = SAt(26f, 262f), sSeep = sP1 - 3.5f, sRune = (sP1 + sP2) * 0.5f;
float sSnag = L2(sP2, sP3, landmarkShare), sFir = L2(sP3, sP4, landmarkShare);
Mark("Seep_Rock", sSeep, -2.1f, Cube, V(0.4f, 2.5f, 2f), 1.25f); Mark("Seep_Cup", sSeep, -1.8f, Cyl, V(0.1f, 0.06f, 0.1f), 1.3f);
Mark("RunePost", sRune, 2.6f, Cube, V(0.3f, 2f, 0.3f), 1f);
Mark("SplitSnag_L", sSnag, -1.2f, Cyl, V(1.2f, 7f, 1.2f), 7f); Mark("SplitSnag_R", sSnag, 1.2f, Cyl, V(1.2f, 6f, 1.2f), 6f);
Mark("BentFir", sFir, 2.2f, Cyl, V(0.35f, 2f, 0.35f), 0.5f, 90f);
Mark("P1_Overhang", sP1, -2.2f, Cube, V(1.5f, 0.5f, 3f), 2.4f); Mark("P2_RockRoof", sP2, -2f, Cube, V(2f, 0.5f, 2.5f), 2.5f); Mark("P3_RootPlate", sP3, 2.3f, Cyl, V(4f, 0.3f, 4f), 2f, 90f);

// Ward stones (Valley.md rev 10, 4): on the ledge south of the path end and off to one side of the approach, 3.6 x 4 m, tops 70
const float stoneTop = 70f;
var stones = new UnityEngine.GameObject("Stones").transform; stones.SetParent(ward, false);
var stonePos = new[] { P2(-4f, 226f), P2(1f, 223f), P2(-7f, 221f) };
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
float FloorY(float x, float z) => floorY + floorRoll * UnityEngine.Mathf.PerlinNoise(x / 170f + 2.3f, z / 170f + 8.1f) * UnityEngine.Mathf.Clamp01((floorX1 - x) / floorStep);
// the far ridge (Valley.md 5.1): rises from the floor at x -200 to its crest at x -240, holds it under the far front to x -400,
// and falls back to the floor by x -500
const float ridgeFoot = -200f, ridgeFront = -240f, ridgeBack = -400f, ridgeBackFoot = -500f, ridgeX0 = -520f, ridgeX1 = -190f, ridgeZ0 = -750f, ridgeZ1 = 1250f, ridgeStep = 10f, ridgeCrestLo = 25f, ridgeCrest = 30f;
float RidgeY(float x, float z)
{
    float crest = UnityEngine.Mathf.Lerp(ridgeCrestLo, ridgeCrest, UnityEngine.Mathf.PerlinNoise(z / 140f + 4.4f, 0.37f));
    float w = x > ridgeFront ? UnityEngine.Mathf.SmoothStep(0f, 1f, (ridgeFoot - x) / (ridgeFoot - ridgeFront)) : x > ridgeBack ? 1f : UnityEngine.Mathf.SmoothStep(0f, 1f, (x - ridgeBackFoot) / (ridgeBack - ridgeBackFoot));
    return UnityEngine.Mathf.Max(FloorY(x, z), floorY + (crest - floorY) * UnityEngine.Mathf.Clamp01(w));
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
// 8.14a (Vesper, Marlow: by day the far ridge was a black slab): the fire's land takes a stronger blend toward the fog colour, so by day
// it reads as far, hazed ground and at night (a near-black fog) it stays dark
const float fireLandHaze = 0.6f;
var fireLand = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_FireLand.mat");
if (fireLand == null) { fireLand = new UnityEngine.Material(backSh); UnityEditor.AssetDatabase.CreateAsset(fireLand, "Assets/Materials/Blockout/Blockout_FireLand.mat"); }
fireLand.shader = backSh; fireLand.SetColor("_Color", lookT.backdropGroundColor); fireLand.SetFloat("_HazeBlend", fireLandHaze); UnityEditor.EditorUtility.SetDirty(fireLand);
foreach (var landName in new[] { "ValleyFloor", "Ridge" }) fire.transform.Find(landName).GetComponent<UnityEngine.MeshRenderer>().sharedMaterial = fireLand;
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
var faceTo = new UnityEngine.Vector2(-8.5f, 246f);   // cards turn toward the path end on the ledge
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
// 8.14a: flame heights from flameLow to the full stated top, so the front is a ragged burn, not a picket row (tops never pass the stated top)
const float flameLow = 0.5f;
int FlameFrame() => 16 + rng.Next(32);   // the middle rows: full flames, not the first flicker or the dying tail
float CardH(float baseY, float top) => (top - baseY) / flameShare;
// the far front: burning giants on the far ridge (the pack's dead tree stretched to a giant), a near and a far row, flame tops 105
const string deadTreePath = "Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead.prefab";
var deadTree = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(deadTreePath); if (deadTree == null) return "dead tree prefab missing";
const float deadTreeGirth = 0.6f, giantGirth = 6f;   // the pack tree's trunk is about 0.6 m across; a giant's about 6 m
const float frontZ0 = -60f, frontZ1 = 650f, frontTop = 105f, frontStep = 30f, flameSink = 8f, frontJitterZ = 8f, giantLow = 40f, giantHigh = 50f;
var frontRows = new[] { (-240f, -300f), (-330f, -400f) };
float deadTreeTall = 0f; foreach (var rr in deadTree.GetComponentsInChildren<UnityEngine.Renderer>()) deadTreeTall = UnityEngine.Mathf.Max(deadTreeTall, rr.bounds.max.y);
var giants = new UnityEngine.GameObject("BurningGiants").transform; giants.SetParent(fire.transform, false);
var flameCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
foreach (var row in frontRows)
    for (float z = frontZ0; z <= frontZ1; z += frontStep)
    {
        float x = R(row.Item2, row.Item1), baseY = RidgeY(x, z), tall = R(giantLow, giantHigh);
        var t = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(deadTree, giants); t.name = "Trunk";
        t.transform.position = V(x, baseY, z); t.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
        float sxz = giantGirth / deadTreeGirth, sy = tall / UnityEngine.Mathf.Max(0.5f, deadTreeTall); t.transform.localScale = V(sxz, sy, sxz);
        foreach (var c in t.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
        foreach (var rr in t.GetComponentsInChildren<UnityEngine.Renderer>()) rr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        for (int k = 0; k < 3; k++)
        {
            float fb = baseY - flameSink, cz = UnityEngine.Mathf.Clamp(z + R(-frontJitterZ, frontJitterZ), frontZ0, frontZ1);
            flameCards.Add((V(UnityEngine.Mathf.Clamp(x + R(-6f, 6f), row.Item2, row.Item1), fb, cz), R(40f, 56f), CardH(fb, frontTop) * R(flameLow, 1f), FlameFrame(), R(-4f, 4f), 1f));
        }
    }
Cards("RidgeFlames", giants, flameCards, flameMat, sheetTiles);
// the valley fires on the -40 floor, x -110 to -220, z 0 to 500, flame tops 20
const float valleyX0 = -220f, valleyX1 = -110f, valleyZ0 = 0f, valleyZ1 = 500f, valleyTop = 20f, valleyStepX = 20f, valleyStepZ = 26f, valleyJitter = 8f;
var valley = new UnityEngine.GameObject("ValleyFires").transform; valley.SetParent(fire.transform, false);
var valleyCards = new System.Collections.Generic.List<(UnityEngine.Vector3, float, float, int, float, float)>();
for (float z = valleyZ0; z <= valleyZ1; z += valleyStepZ)
    for (float x = valleyX1; x >= valleyX0; x -= valleyStepX)
    {
        float fx = UnityEngine.Mathf.Clamp(x + R(-valleyJitter, valleyJitter), valleyX0, valleyX1), fz = UnityEngine.Mathf.Clamp(z + R(-valleyJitter, valleyJitter), valleyZ0, valleyZ1), fb = RidgeY(fx, fz) - 2f;
        valleyCards.Add((V(fx, fb, fz), R(40f, 56f), CardH(fb, valleyTop) * R(flameLow, 1f), FlameFrame(), 0f, 1f));
    }
Cards("ValleyFlames", valley, valleyCards, flameMat, sheetTiles);
// night smoke columns (Valley.md 5.4, 8.14a stand-ins): from nightfall of night 1, columnCount columns stand over the far front to
// columnTop, lit from below up to columnLit; stacked cylinders widening upward on DieAlone/Backdrop in the smoke colour, the lit part a
// warm glow on DieAlone/FireStandIn. Shown at night and on day two (LookVisibility); no colliders, no shadows
const float columnX = -300f, columnTop = 250f, columnLit = 130f, columnGlow = 0.35f; float[] columnZ = { 40f, 170f, 300f, 430f };
var fireStand = UnityEngine.Shader.Find("DieAlone/FireStandIn"); if (fireStand == null) return "DieAlone/FireStandIn shader not found";
UnityEngine.Material ColumnMat(string path, UnityEngine.Shader sh, System.Action<UnityEngine.Material> set) { var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path); if (m == null) { m = new UnityEngine.Material(sh); UnityEditor.AssetDatabase.CreateAsset(m, path); } m.shader = sh; set(m); UnityEditor.EditorUtility.SetDirty(m); return m; }
var smokeMat = ColumnMat("Assets/Materials/Blockout/Blockout_SmokeColumn.mat", backSh, m => { m.SetColor("_Color", lookT.smokeBodyColor); m.SetFloat("_HazeBlend", 0.3f); });
UnityEngine.ColorUtility.TryParseHtmlString("#5A2412", out var litUnder);   // Style.md 2.3 lit underside
var glowMat = ColumnMat("Assets/Materials/Blockout/Blockout_SmokeColumnLit.mat", fireStand, m => { m.SetColor("_Color", litUnder); m.SetFloat("_Intensity", columnGlow); });
var columns = new UnityEngine.GameObject("SmokeColumns"); columns.transform.SetParent(fire.transform, false); var columnParts = new System.Collections.Generic.List<UnityEngine.GameObject>();
var colSteps = new[] { (0f, 0.4f, 18f), (0.4f, 0.72f, 28f), (0.72f, 1f, 40f) };   // share of the height from, to, radius
foreach (var cz in columnZ)
{
    float baseY = RidgeY(columnX, cz), hgt = columnTop - baseY;
    foreach (var st in colSteps)
    {
        float y0 = baseY + hgt * st.Item1, y1 = baseY + hgt * st.Item2;
        var c = Prim(Cyl, "Column", columns.transform, V(columnX + R(-10f, 10f), (y0 + y1) * 0.5f, cz), V(st.Item3 * 2f, (y1 - y0) * 0.5f, st.Item3 * 2f), R(0f, 360f), false);
        var cr = c.GetComponent<UnityEngine.MeshRenderer>(); cr.sharedMaterial = smokeMat; cr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; columnParts.Add(c);
    }
    var lit = Prim(Cyl, "ColumnLit", columns.transform, V(columnX, (baseY + columnLit) * 0.5f, cz), V(colSteps[0].Item3 * 2.1f, (columnLit - baseY) * 0.5f, colSteps[0].Item3 * 2.1f), 0f, false);
    var lr = lit.GetComponent<UnityEngine.MeshRenderer>(); lr.sharedMaterial = glowMat; lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; columnParts.Add(lit);
}
{
    var dayTwoL = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayTwo.asset"); if (dayTwoL == null) return "no LookTuning_DayTwo";
    var vis = columns.AddComponent<LookVisibility>(); var so = new UnityEditor.SerializedObject(vis);
    so.FindProperty("show").enumValueIndex = (int)LookVisibility.Show.NightAndDayTwo; so.FindProperty("dayTwo").objectReferenceValue = dayTwoL;
    var tp = so.FindProperty("targets"); tp.arraySize = columnParts.Count; for (int i = 0; i < columnParts.Count; i++) tp.GetArrayElementAtIndex(i).objectReferenceValue = columnParts[i]; so.ApplyModifiedPropertiesWithoutUndo();
}
// the day-one smoke sheet (5.3): one low sheet streaming west on the east wind: its top 60 over x -110 rising to 110 at x -500, level
// past it to x -900; z -60 to 400; a front face down to the floor at x -110 and side faces at both ends. Drawn on DieAlone/Backdrop in
// the day-one smoke colour, hazed toward the fog; shown in the day-one look only (LookVisibility). Its top edge points are marked
// (SheetTop_*) for F-1.
const float sheetX0 = -110f, sheetX1 = -500f, sheetXEnd = -900f, sheetZ0 = -60f, sheetZ1 = 400f, sheetTop0 = 60f, sheetTop1 = 110f, sheetStepX = 40f, sheetStepZ = 20f, sheetHaze = 0.6f;
var dayOneLook = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayOne.asset"); if (dayOneLook == null) return "no LookTuning_DayOne";
var dayTwoLook = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning_DayTwo.asset"); if (dayTwoLook == null) return "no LookTuning_DayTwo";
var sheetMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_SmokeSheet.mat");
if (sheetMat == null) { sheetMat = new UnityEngine.Material(backSh); UnityEditor.AssetDatabase.CreateAsset(sheetMat, "Assets/Materials/Blockout/Blockout_SmokeSheet.mat"); }
sheetMat.shader = backSh; sheetMat.SetColor("_Color", dayOneLook.smokeBodyColor); sheetMat.SetFloat("_HazeBlend", sheetHaze); UnityEditor.EditorUtility.SetDirty(sheetMat);
float SheetTop(float x) => x >= sheetX1 ? L2(sheetTop0, sheetTop1, (sheetX0 - x) / (sheetX0 - sheetX1)) : sheetTop1;
var sheet = new UnityEngine.GameObject("SmokeSheet"); sheet.transform.SetParent(ward, false);
{
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var tris = new System.Collections.Generic.List<int>();
    var xs = new System.Collections.Generic.List<float>(); for (float x = sheetX0; x >= sheetXEnd - 0.1f; x -= sheetStepX) xs.Add(x);
    var zs = new System.Collections.Generic.List<float>(); for (float z = sheetZ0; z <= sheetZ1 + 0.1f; z += sheetStepZ) zs.Add(z);
    int cols = xs.Count, rows = zs.Count;
    // one winding here; the back faces are a second copy of the vertices below, so each side keeps its own normals (a shared
    // vertex under both windings sums its normals to zero and the Backdrop shader draws it black)
    for (int r = 0; r < rows; r++) for (int c = 0; c < cols; c++) vs.Add(V(xs[c], SheetTop(xs[c]), zs[r]));   // the top
    for (int r = 0; r < rows - 1; r++) for (int c = 0; c < cols - 1; c++) { int a = r * cols + c, b = a + 1, d = a + cols, e = d + 1; tris.AddRange(new[] { a, d, b, b, d, e }); }
    int f0 = vs.Count; foreach (var z in zs) { vs.Add(V(sheetX0, SheetTop(sheetX0), z)); vs.Add(V(sheetX0, floorY, z)); }   // the front face, down to the floor
    for (int r = 0; r < rows - 1; r++) { int a = f0 + r * 2, b = a + 1, d = a + 2, e = a + 3; tris.AddRange(new[] { a, b, d, d, b, e }); }
    foreach (var ez in new[] { sheetZ0, sheetZ1 })
    {
        int s0 = vs.Count; foreach (var x in xs) { vs.Add(V(x, SheetTop(x), ez)); vs.Add(V(x, floorY, ez)); }
        for (int c = 0; c < cols - 1; c++) { int a = s0 + c * 2, b = a + 1, d = a + 2, e = a + 3; tris.AddRange(new[] { a, b, d, d, b, e }); }
    }
    { int nv = vs.Count, nt = tris.Count; vs.AddRange(vs.GetRange(0, nv)); for (int i = 0; i < nt; i += 3) tris.AddRange(new[] { tris[i] + nv, tris[i + 2] + nv, tris[i + 1] + nv }); }   // the back faces
    var mesh = new UnityEngine.Mesh { name = "SmokeSheet", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.SetVertices(vs); mesh.SetTriangles(tris, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, fireDir + "/SmokeSheet.asset");
    var body = new UnityEngine.GameObject("Body"); body.transform.SetParent(sheet.transform, false);
    body.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; var mr = body.AddComponent<UnityEngine.MeshRenderer>(); mr.sharedMaterial = sheetMat;
    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; mr.receiveShadows = false;
    foreach (var x in new[] { sheetX0, (sheetX0 + sheetX1) * 0.5f, sheetX1 })
        foreach (var z in zs) { var mk = new UnityEngine.GameObject("SheetTop_" + x.ToString("F0") + "_" + z.ToString("F0")); mk.transform.SetParent(sheet.transform, false); mk.transform.position = V(x, SheetTop(x), z); }
    var vis = sheet.AddComponent<LookVisibility>(); var so = new UnityEditor.SerializedObject(vis);
    so.FindProperty("show").enumValueIndex = (int)LookVisibility.Show.DayOne; so.FindProperty("dayTwo").objectReferenceValue = dayTwoLook;
    var tp = so.FindProperty("targets"); tp.arraySize = 1; tp.GetArrayElementAtIndex(0).objectReferenceValue = body; so.ApplyModifiedPropertiesWithoutUndo();
}
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cairn at " + cairnPos.ToString("F1") + " | chain and IW2 across the chute gap, IW2 " + iwBot.ToString("F1") + " to " + iwTop.ToString("F1") + " | steps " + steps.childCount + ", climb pieces " + climbRoot.childCount
    + " | stones ground " + H(-4f, 226f).ToString("F1") + " tops " + stoneTop + " | stand-in fire: floor " + floorY + ", far ridge crest " + ridgeCrest + ", " + flameCards.Count + " front cards to " + frontTop + " (z " + frontZ0 + " to " + frontZ1 + "), " + valleyCards.Count + " valley cards to " + valleyTop + " | day-one sheet " + sheet.transform.childCount + " parts";
