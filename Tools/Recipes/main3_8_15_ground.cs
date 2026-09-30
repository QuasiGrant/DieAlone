// Main3 task 8.15: paths, ground and stops (Valley.md rev 10 sections 8, 9, 11 and 12; Docs/Design/ForestPlan.md section 6; owned
// assets per Docs/Process/AssetCatalogue.md). Run after 8.9f in Main3, edit mode (the runner runs it before the day-one start and the
// sightlines). No trees (8.16).
// 1. Ground layers: the floor GrassPine, SoilPine under the north groves and the fir wall, GrassMud on the lake shore and in the old
//    burn, bare Ground054 dirt on the trails (8.3 paints them 1.4 m wide with a 0.4 m blend), Rocks_a on every slope over 35 degrees
//    (8.1 paints it), Mud_darker on the lake bed. Raw greys (AssetCatalogue): trail 140, floors 79 to 89.
// 2. Ground cover to the trail's edge: grass and fern details whose tufts reach the edge (cells from half the widest tuft past it), thick for 8 m, thinner beyond.
// 3. Trail edges: a pale stone or a log on one edge every 4 m, sides alternating (Valley.md 12.1; the night cue), looks only.
// 4. Junction markers (section 11): signposts at camp, the pump, Jg and J; the trailhead board at T; a blaze on a post on W1's Camp 3
//    branch; a blaze on a stump where the north loop leaves Camp 1. The gate T is 8.6's.
// 5. Stops (section 8), every one a visible thing with its collider inside it: brush hedges (Campsite CS_Bush_Large and BK hollow
//    logs, the AssetCatalogue fallback for brush over 2 m) round the old burn and between the knoll switchbacks (traced round the
//    trail corridors, so junctions stay open), across the south-east corner, the W foot pocket, the mid pocket and both ends of the
//    lake's south shore, and dressing 8.6's gray brush bands; rock rims (1.3 m, as 8.1's climb ring) round the ravine above the cave
//    spur and the Camp 3 hollow, open where the trails go in. The lake's edge is 8.4's wade limit, at the water line.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("SliceLook") == null) return "run 8.9f first";
if (Root("Ground815") != null) return "Ground815 already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>(); var data = terrain.terrainData; var tOrg = terrain.transform.position; var size = data.size;
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + tOrg.y;
float SS(float a) => UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01(a));
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-6f)); return UnityEngine.Vector2.Distance(p, a + ab * t); }
bool Inside(UnityEngine.Vector2 p, UnityEngine.Vector2[] poly)
{
    bool c = false;
    for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
        if (((poly[i].y > p.y) != (poly[j].y > p.y)) && (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) c = !c;
    return c;
}
var rng = new System.Random(8151);
float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
var missing = new System.Collections.Generic.List<string>();
UnityEngine.GameObject Spawn(string path, UnityEngine.Transform parent)
{
    var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path + ".prefab"); if (pf == null) { if (!missing.Contains(path)) missing.Add(path); return null; }
    var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pf, parent);
    foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);   // dressing never catches the player; stops carry their own colliders
    return g;
}
float Bottom(UnityEngine.GameObject g) { float low = float.MaxValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) low = UnityEngine.Mathf.Min(low, r.bounds.min.y); return low; }
float Top(UnityEngine.GameObject g) { float hi = float.MinValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) hi = UnityEngine.Mathf.Max(hi, r.bounds.max.y); return hi; }
void SitOn(UnityEngine.GameObject g, float x, float z, float sink) { g.transform.position = V(x, 0f, z); float b = Bottom(g); g.transform.position = V(x, H(x, z) - b - sink, z); }
const string BK = "Assets/BK/PureNature_Redwood/", CS = "Assets/Revolving Pizza Games/Campsite/Prefabs/";
var root = new UnityEngine.GameObject("Ground815").transform;

// trails: every leg's centre points (8.3, 2 m apart)
var legs = new System.Collections.Generic.List<(string name, System.Collections.Generic.List<UnityEngine.Vector3> pts)>();
foreach (UnityEngine.Transform leg in Root("Trails").transform) { var l = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) l.Add(p.position); legs.Add((leg.name, l)); }
var allPts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (var lg in legs) foreach (var p in lg.pts) allPts.Add(P(p.x, p.z));
var pois = new System.Collections.Generic.List<UnityEngine.Vector2>(); if (Root("PointsOfInterest") != null) foreach (UnityEngine.Transform t in Root("PointsOfInterest").transform) pois.Add(P(t.position.x, t.position.z));
var lakeC = P(190f, 60f); const float lakeA = 54.8f, lakeB = 27.6f;
float LakeRe(UnityEngine.Vector2 p) { var q = p - lakeC; return UnityEngine.Mathf.Sqrt((q.x / lakeA) * (q.x / lakeA) + (q.y / lakeB) * (q.y / lakeB)); }
var campC = P(170f, 160f); const float campR = 18f;
var burnPoly = new[] { P(185f, 181f), P(340f, 213f), P(340f, 143f), P(185f, 151f) };

// ---------- 1. ground layers ----------
UnityEngine.Texture2D Tex(string path) { var t = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(path); if (t == null) missing.Add(path); return t; }
const string surf = BK + "Textures/Surfaces/";
UnityEngine.TerrainLayer LayerNamed(string n) { foreach (var l in data.terrainLayers) if (l != null && l.name == n) return l; return null; }
void SetLayer(UnityEngine.TerrainLayer l, UnityEngine.Texture2D albedo, UnityEngine.Texture2D normal, float tile, UnityEngine.Color remap)
{
    l.diffuseTexture = albedo; l.normalMapTexture = normal; l.tileSize = new UnityEngine.Vector2(tile, tile);
    l.diffuseRemapMin = UnityEngine.Vector4.zero; l.diffuseRemapMax = new UnityEngine.Vector4(remap.r, remap.g, remap.b, 1f); UnityEditor.EditorUtility.SetDirty(l);
}
const float floorTile = 4f, trailTile = 2f, rockTile = 10f, rockLuma = 0.55f, trailLift = 1.65f, burnDim = 0.65f;   // trailLift: the dirt brightened so the tread stands 20 grey over the floor 20 m ahead (Gate.md 4; 8.14a measured about 10)
// rockTile: rock at 10 m so its forms read at a distance through the look filter (8.14a)
UnityEngine.ColorUtility.TryParseHtmlString("#6E6660", out var granite);   // Style.md granite; Rocks_a's mean luma is 0.55 (AssetCatalogue)
var white = UnityEngine.Color.white;
var lGround = LayerNamed("Layer_Ground"); var lRock = LayerNamed("Layer_Rock"); var lBurn = LayerNamed("Layer_Burn"); var lTrail = LayerNamed("Layer_Trail"); var lBed = LayerNamed("Layer_LakeBed");
if (lGround == null || lRock == null || lBurn == null || lTrail == null || lBed == null) return "8.1's terrain layers missing";
SetLayer(lGround, Tex(surf + "GrassPine_a.png"), Tex(surf + "GrassPine_n.png"), floorTile, white);
SetLayer(lBurn, Tex(surf + "GrassMud_a.png"), Tex(surf + "GrassMud_n.png"), floorTile, new UnityEngine.Color(burnDim, burnDim, burnDim));   // 8.15: the burn floor dimmed so the trails through it stand 20 grey over it
SetLayer(lTrail, Tex("Assets/Textures/Ground054/Ground054_Color.jpg"), null, trailTile, new UnityEngine.Color(trailLift, trailLift, trailLift));
SetLayer(lRock, Tex(BK + "Models/Rocks/Textures/Rocks_a.png"), Tex(BK + "Models/Rocks/Textures/Rocks_n.png"), rockTile, new UnityEngine.Color(granite.r / rockLuma, granite.g / rockLuma, granite.b / rockLuma));
SetLayer(lBed, Tex(surf + "Mud_darker_a.png"), Tex(surf + "Mud_darker_n.png"), floorTile, white);
const string soilPath = "Assets/Terrain/Main3/Layer_SoilPine.terrainlayer";
var lSoil = new UnityEngine.TerrainLayer { name = "Layer_SoilPine" }; UnityEditor.AssetDatabase.CreateAsset(lSoil, soilPath);
SetLayer(lSoil, Tex(surf + "SoilPine_a.png"), Tex(surf + "SoilPine_n.png"), floorTile, white);
if (missing.Count > 0) return "missing: " + string.Join(", ", missing);
var layerList = new System.Collections.Generic.List<UnityEngine.TerrainLayer>(data.terrainLayers); layerList.Add(lSoil); data.terrainLayers = layerList.ToArray();
int iGround = layerList.IndexOf(lGround), iBurn = layerList.IndexOf(lBurn), iSoil = layerList.IndexOf(lSoil);
// SoilPine (needles) under the north and west groves and the north fir wall (ForestPlan 6), GrassMud on the shore band (lake radii
// 1.03 to 1.25), both taken from the floor layer only, so trail, rock and bed paint stay
var soilCircles = new (UnityEngine.Vector2 c, float r)[] { (P(175f, 282f), 20f), (P(120f, 280f), 16f), (P(92f, 250f), 14f), (P(132f, 212f), 14f), (P(115f, 128f), 14f), (P(92f, 75f), 14f), (P(65f, 20f), 14f) };
const float soilBlend = 4f, wallX0 = 90f, wallX1 = 300f, wallZ0 = 285f, wallZ1 = 310f, shoreIn = 1.03f, shoreOut = 1.25f, shoreBlend = 0.08f;
{
    int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); float aX = size.x / ares, aZ = size.z / ares;
    for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
    {
        var p = P(tOrg.x + (xi + 0.5f) * aX, tOrg.z + (zi + 0.5f) * aZ); float g = alpha[zi, xi, iGround]; if (g <= 0f) continue;
        float soil = 0f;
        foreach (var c in soilCircles) soil = UnityEngine.Mathf.Max(soil, 1f - SS((UnityEngine.Vector2.Distance(p, c.c) - c.r) / soilBlend));
        float dWall = UnityEngine.Mathf.Max(UnityEngine.Mathf.Max(wallX0 - p.x, p.x - wallX1), UnityEngine.Mathf.Max(wallZ0 - p.y, p.y - wallZ1));
        soil = UnityEngine.Mathf.Max(soil, 1f - SS(dWall / soilBlend));
        float re = LakeRe(p), shore = re < shoreIn || re > shoreOut + shoreBlend ? 0f : 1f - SS((re - shoreOut) / shoreBlend);
        float toSoil = g * soil, toShore = (g - toSoil) * shore;
        alpha[zi, xi, iGround] = g - toSoil - toShore; alpha[zi, xi, iSoil] += toSoil; alpha[zi, xi, iBurn] += toShore;
    }
    data.SetAlphamaps(0, 0, alpha);
}

// 8.15 gate (Pim W1: beside Jg to Camp 1 the pale clearing grass read as light as the dirt): a strip of darker duff (SoilPine dimmed to
// duffDim) from duffIn to duffOut off every trail's centre points (2 m apart), taken duffShare from the floor layers
const string duffPath = "Assets/Terrain/Main3/Layer_Duff.terrainlayer"; const float duffDim = 0.65f, duffIn = 1.3f, duffOut = 3.5f, duffBlend = 0.8f, duffShare = 0.8f;
var lDuff = new UnityEngine.TerrainLayer { name = "Layer_Duff" }; UnityEditor.AssetDatabase.CreateAsset(lDuff, duffPath);
SetLayer(lDuff, Tex(surf + "SoilPine_a.png"), Tex(surf + "SoilPine_n.png"), floorTile, new UnityEngine.Color(duffDim, duffDim, duffDim));
layerList.Add(lDuff); data.terrainLayers = layerList.ToArray(); int iDuff = layerList.IndexOf(lDuff);
{
    int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); float aX = size.x / ares, aZ = size.z / ares;
    var near = new float[ares, ares]; for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++) near[zi, xi] = float.MaxValue;
    foreach (var p in allPts)
    {
        int x0 = UnityEngine.Mathf.Max(0, (int)((p.x - tOrg.x - duffOut) / aX)), x1 = UnityEngine.Mathf.Min(ares - 1, (int)((p.x - tOrg.x + duffOut) / aX) + 1);
        int z0 = UnityEngine.Mathf.Max(0, (int)((p.y - tOrg.z - duffOut) / aZ)), z1 = UnityEngine.Mathf.Min(ares - 1, (int)((p.y - tOrg.z + duffOut) / aZ) + 1);
        for (int zi = z0; zi <= z1; zi++) for (int xi = x0; xi <= x1; xi++) { float d = UnityEngine.Vector2.Distance(p, P(tOrg.x + (xi + 0.5f) * aX, tOrg.z + (zi + 0.5f) * aZ)); if (d < near[zi, xi]) near[zi, xi] = d; }
    }
    for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
    {
        float d = near[zi, xi]; if (d < duffIn || d > duffOut + duffBlend) continue;
        float w = duffShare * (1f - SS((d - duffOut) / duffBlend));
        foreach (int l in new[] { iGround, iBurn, iSoil }) { float take = alpha[zi, xi, l] * w; alpha[zi, xi, l] -= take; alpha[zi, xi, iDuff] += take; }
    }
    data.SetAlphamaps(0, 0, alpha);
}

// 8.15 gate (Marlow, Vesper: banks over 35 degrees carry burn and grass stripes): the terrain is final here (8.3 re-cut the banks after
// 8.1 painted rock), so every slope over rockFrom takes the rock layer, fully at rockFull, from every layer but the trail's dirt
const float rockFrom = 32f, rockFull = 40f;
int iRockL = layerList.IndexOf(lRock), iTrailL = layerList.IndexOf(lTrail); int rockCells = 0;
{
    int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int layersN = alpha.GetLength(2);
    for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
    {
        float st = data.GetSteepness((xi + 0.5f) / ares, (zi + 0.5f) / ares); if (st <= rockFrom) continue;
        float w = UnityEngine.Mathf.Clamp01((st - rockFrom) / (rockFull - rockFrom)); float moved = 0f;
        for (int l = 0; l < layersN; l++) { if (l == iRockL || l == iTrailL) continue; float take = alpha[zi, xi, l] * w; alpha[zi, xi, l] -= take; moved += take; }
        alpha[zi, xi, iRockL] += moved; if (moved > 0.5f) rockCells++;
    }
    data.SetAlphamaps(0, 0, alpha);
}

// ---------- 1b. the climb's four grounds (Valley.md 1.6 and 4; ForestPlan 6; 8.14a): each leg its own ground and edge ----------
// Leg 1 the chute: bare stone (the rock layer) between rock walls, the cut steps; leg 2 the shelf: scree (Rocks_a at a small tile,
// lighter) and sparse rubble; leg 3 the burned cwm: ash (Mud_darker greyed), charred snags and burnt wood; leg 4 under the wall:
// needles (SoilPine) with a screen of young firs on the valley side that ends before P4, so P4 opens east; the cleft and the ledge:
// bare stone. Painted over every gentle cell within climbPaint of the climb trail west of the chute mouth (the trail's dirt band
// there is replaced).
var climbPts = legs.Find(x => x.name == "J to Ward").pts; if (climbPts == null) return "no J to Ward trail";
var climbS = new float[climbPts.Count]; for (int i = 1; i < climbPts.Count; i++) climbS[i] = climbS[i - 1] + UnityEngine.Vector2.Distance(P(climbPts[i - 1].x, climbPts[i - 1].z), P(climbPts[i].x, climbPts[i].z));
float SAtPt(float x, float z) { int bi = 0; float bd = float.MaxValue; for (int i = 0; i < climbPts.Count; i++) { float d = UnityEngine.Vector2.Distance(P(climbPts[i].x, climbPts[i].z), P(x, z)); if (d < bd) { bd = d; bi = i; } } return climbS[bi]; }
float sP1 = SAtPt(52f, 216f), sP2 = SAtPt(57f, 276f), sP3 = SAtPt(26f, 304f), sP4 = SAtPt(26f, 262f), sSlot = SAtPt(24.5f, 262f);
const string screePath = "Assets/Terrain/Main3/Layer_Scree.terrainlayer", ashPath = "Assets/Terrain/Main3/Layer_Ash.terrainlayer";
const float screeTile = 1.2f, screeLift = 0.9f, ashGrey = 0.5f, climbPaint = 12f, climbMouthX = 86f, climbSteep = 42f;   // scree 0.9 and ash 0.5 (were 1.25, 0.62; 8.15 gate: darker beside the climb's tread)
var lScree = new UnityEngine.TerrainLayer { name = "Layer_Scree" }; UnityEditor.AssetDatabase.CreateAsset(lScree, screePath);
SetLayer(lScree, Tex(BK + "Models/Rocks/Textures/Rocks_a.png"), Tex(BK + "Models/Rocks/Textures/Rocks_n.png"), screeTile, new UnityEngine.Color(granite.r / rockLuma * screeLift, granite.g / rockLuma * screeLift, granite.b / rockLuma * screeLift));
var lAsh = new UnityEngine.TerrainLayer { name = "Layer_Ash" }; UnityEditor.AssetDatabase.CreateAsset(lAsh, ashPath);
SetLayer(lAsh, Tex(surf + "Mud_darker_a.png"), Tex(surf + "Mud_darker_n.png"), floorTile, new UnityEngine.Color(ashGrey, ashGrey, ashGrey));
layerList.Add(lScree); layerList.Add(lAsh); data.terrainLayers = layerList.ToArray();
int iRock = layerList.IndexOf(lRock), iScree = layerList.IndexOf(lScree), iAsh = layerList.IndexOf(lAsh), iTrail = layerList.IndexOf(lTrail);
// 8.15 (Pim W1: the climb read -2 grey against its ground): a worn tread climbTread m either side of the line keeps the trail's dirt,
// blended over climbTreadBlend into the leg's own ground
const float climbTread = 0.8f, climbTreadBlend = 0.4f;   // 0.8 (was 0.6; 8.15 gate, Marlow: the chute and cleft tread could not be told from the floor)
{
    int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); float aX = size.x / ares, aZ = size.z / ares; int layersN = alpha.GetLength(2);
    int x0 = 0, x1 = UnityEngine.Mathf.Min(ares - 1, (int)((climbMouthX - tOrg.x) / aX)), z0 = (int)((190f - tOrg.z) / aZ), z1 = UnityEngine.Mathf.Min(ares - 1, (int)((340f - tOrg.z) / aZ));
    for (int zi = z0; zi <= z1; zi++) for (int xi = x0; xi <= x1; xi++)
    {
        float x = tOrg.x + (xi + 0.5f) * aX, z = tOrg.z + (zi + 0.5f) * aZ;
        if (data.GetSteepness((xi + 0.5f) / ares, (zi + 0.5f) / ares) > climbSteep) continue;
        int bi = -1; float bd = float.MaxValue; for (int i = 0; i < climbPts.Count; i++) { float d = UnityEngine.Vector2.Distance(P(climbPts[i].x, climbPts[i].z), P(x, z)); if (d < bd) { bd = d; bi = i; } }
        if (bd > climbPaint || UnityEngine.Mathf.Abs(H(x, z) - climbPts[bi].y) > 2f) continue;   // the bench the trail is on, not the one below
        float s = climbS[bi]; int k = s < sP1 ? iRock : s < sP2 ? iScree : s < sP3 ? iAsh : s < sSlot ? iSoil : iRock;
        float line = bd;   // distance to the trail line, not its points (2 m apart), so the tread has no beads
        for (int j = UnityEngine.Mathf.Max(1, bi); j <= UnityEngine.Mathf.Min(climbPts.Count - 1, bi + 1); j++) { var a = P(climbPts[j - 1].x, climbPts[j - 1].z); var ab = P(climbPts[j].x, climbPts[j].z) - a; float tt = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(P(x, z) - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); line = UnityEngine.Mathf.Min(line, UnityEngine.Vector2.Distance(P(x, z), a + ab * tt)); }
        float tread = 1f - UnityEngine.Mathf.Clamp01((line - climbTread) / climbTreadBlend);
        for (int l = 0; l < layersN; l++) alpha[zi, xi, l] = 0f; alpha[zi, xi, k] = 1f - tread; alpha[zi, xi, iTrail] += tread;
    }
    data.SetAlphamaps(0, 0, alpha);
}
var climbDress = new UnityEngine.GameObject("ClimbGrounds").transform; climbDress.SetParent(root, false);
UnityEngine.Vector3 ClimbAt(float s, out UnityEngine.Vector2 tan)
{
    int i = 1; while (i < climbPts.Count - 1 && climbS[i] < s) i++;
    float t = UnityEngine.Mathf.InverseLerp(climbS[i - 1], climbS[i], s); tan = (P(climbPts[i].x, climbPts[i].z) - P(climbPts[i - 1].x, climbPts[i - 1].z)).normalized;
    return UnityEngine.Vector3.Lerp(climbPts[i - 1], climbPts[i], t);
}
const float rubbleStep = 9f, snagStep = 7f, firStep = 5f, dressOff = 2.4f, firEndBeforeP4 = 6f;
int climbDressN = 0;
void Dress(string path, float s, float side, float height, float tiltMax)
{
    var c = ClimbAt(s, out var tan); var n = P(-tan.y, tan.x) * side; var q = P(c.x, c.z) + n;
    var g = Spawn(path, climbDress); if (g == null) return;
    g.transform.rotation = UnityEngine.Quaternion.Euler(R(-tiltMax, tiltMax), R(0f, 360f), R(-tiltMax, tiltMax));
    float sc = height / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(sc, sc, sc); SitOn(g, q.x, q.y, 0.15f); climbDressN++;
}
// leg 2: rubble patches on both edges; leg 3: charred snags and burnt wood; leg 4: young firs on the valley (east, left going south) side
for (float s = sP1 + 4f; s < sP2 - 2f; s += rubbleStep) Dress(BK + "Prefabs/Rocks/RubbleSparse_" + (1 + rng.Next(3)), s, (rng.NextDouble() < 0.5 ? 1f : -1f) * dressOff, 0.6f, 0f);
var charred = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_RoofChar.mat");
for (float s = sP2 + 4f; s < sP3 - 2f; s += snagStep)
{
    int before = climbDress.childCount; Dress("Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead", s, (rng.NextDouble() < 0.5 ? 1f : -1f) * (dressOff + R(0.5f, 3f)), R(5f, 9f), 6f);
    if (charred != null && climbDress.childCount > before) foreach (var r in climbDress.GetChild(climbDress.childCount - 1).GetComponentsInChildren<UnityEngine.Renderer>()) r.sharedMaterial = charred;
    Dress(CS + "Wood/CS_Log_Firewood_Burnt", s + 3f, (rng.NextDouble() < 0.5 ? 1f : -1f) * dressOff, 0.4f, 0f);
}
for (float s = sP3 + 3f; s < sP4 - firEndBeforeP4; s += firStep) Dress(BK + "Prefabs/Trees/RedFir" + (1 + rng.Next(4)), s, dressOff + R(0.3f, 1.2f), R(3f, 6f), 0f);

// ---------- 2. ground cover: grass and fern details, the tufts reaching the trail edge ----------
string[] detailNames = { "Detail_Grass1", "Detail_Grass2", "Detail_Grass3", "Detail_Fern1", "Detail_Fern2" };
var protos = new UnityEngine.DetailPrototype[detailNames.Length];
for (int i = 0; i < detailNames.Length; i++)
{
    var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Prefabs/Forest/" + detailNames[i] + ".prefab"); if (pf == null) return "missing detail prefab " + detailNames[i];
    bool fern = detailNames[i].Contains("Fern");
    protos[i] = new UnityEngine.DetailPrototype { prototype = pf, usePrototypeMesh = true, renderMode = UnityEngine.DetailRenderMode.VertexLit, useInstancing = true, minHeight = fern ? 1.0f : 1.2f, maxHeight = fern ? 1.6f : 2.0f, minWidth = fern ? 1.0f : 1.2f, maxWidth = fern ? 1.6f : 2.0f, noiseSpread = 0.4f, alignToGround = 0.3f };
}
// 8.14a grey check: a tuft is drawn centred on its cell up to maxWidth wide, so cells start half the widest tuft past the trail's
// painted half-width; at 1.1 m the tufts leaned over the tread and hid it from 20 m
const float trailPaintHalf = 0.7f, tuftClear = 0.1f;   // trailPaintHalf is 8.3's trailHalf
float widest = 0f; foreach (var pr in protos) widest = UnityEngine.Mathf.Max(widest, pr.maxWidth);
float coverEdge = trailPaintHalf + widest * 0.5f + tuftClear;
const int detailRes = 1024, detailPatch = 32; const float coverBand = 8f, coverFar = 0.35f, coverDistance = 60f, coverFrontX = 338f, clearingPad = 2f;
data.SetDetailResolution(detailRes, detailPatch); data.detailPrototypes = protos; data.SetDetailScatterMode(UnityEngine.DetailScatterMode.InstanceCountMode);
terrain.detailObjectDistance = coverDistance;
var clearings = new (UnityEngine.Vector2 c, float r)[] { (campC, campR), (P(282f, 238f), 30f), (P(292f, 108f), 20f), (P(78f, 146f), 8f) };
{
    float dX = size.x / detailRes, dZ = size.z / detailRes; var dist = new float[detailRes, detailRes];
    for (int z = 0; z < detailRes; z++) for (int x = 0; x < detailRes; x++) dist[z, x] = float.MaxValue;
    const float reach = 12f;
    foreach (var p in allPts)
    {
        int x0 = UnityEngine.Mathf.Max(0, (int)((p.x - tOrg.x - reach) / dX)), x1 = UnityEngine.Mathf.Min(detailRes - 1, (int)((p.x - tOrg.x + reach) / dX) + 1);
        int z0 = UnityEngine.Mathf.Max(0, (int)((p.y - tOrg.z - reach) / dZ)), z1 = UnityEngine.Mathf.Min(detailRes - 1, (int)((p.y - tOrg.z + reach) / dZ) + 1);
        for (int z = z0; z <= z1; z++) for (int x = x0; x <= x1; x++) { float d = UnityEngine.Vector2.Distance(p, P(tOrg.x + (x + 0.5f) * dX, tOrg.z + (z + 0.5f) * dZ)); if (d < dist[z, x]) dist[z, x] = d; }
    }
    var layers = new int[protos.Length][,]; for (int k = 0; k < protos.Length; k++) layers[k] = new int[detailRes, detailRes];
    for (int z = 0; z < detailRes; z++) for (int x = 0; x < detailRes; x++)
    {
        var p = P(tOrg.x + (x + 0.5f) * dX, tOrg.z + (z + 0.5f) * dZ); float d = dist[z, x];
        if (d < coverEdge || p.x > coverFrontX || LakeRe(p) < shoreIn) continue;
        if (data.GetSteepness((x + 0.5f) / detailRes, (z + 0.5f) / detailRes) > 35f) continue;
        bool inClearing = false; foreach (var c in clearings) if (UnityEngine.Vector2.Distance(p, c.c) < c.r - clearingPad) { inClearing = true; break; } if (inClearing) continue;
        if (d > coverBand && rng.NextDouble() > coverFar) continue;
        layers[rng.Next(protos.Length)][z, x] = 1;
    }
    for (int k = 0; k < protos.Length; k++) data.SetDetailLayer(0, 0, k, layers[k]);
}
UnityEditor.EditorUtility.SetDirty(data);

// ---------- 3. trail edges: a stone, a root or a log every edgeLow to edgeHigh m, on one edge or the other ----------
// 8.15 gate (Vesper: one pale faceted stone every 10 m, the same each time, brighter than the trail): mixed pieces at 3 to 5 m, the side
// picked at random, stones in varied sizes and tilts on 8.1's granite rock (#6E6660 lit, tops under #B8B0A4), roots from the pack's
// branch pieces, logs from the firewood and CITW logs
var edges = new UnityEngine.GameObject("TrailEdges").transform; edges.SetParent(root, false);
var paleStone = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Concrete034_1.0x1.0.mat"); if (paleStone == null) return "no Concrete034_1.0x1.0.mat";   // the painted blazes (below)
var edgeRock = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_BandRock.mat"); if (edgeRock == null) return "no Blockout_BandRock.mat (8.1)";
const float edgeLow = 3f, edgeHigh = 5f, edgeOff = 1.05f, stoneLow = 0.2f, stoneHigh = 0.55f, stoneTilt = 20f, logShare = 0.2f, rootShare = 0.2f;
string[] edgeLogs = { CS + "Wood/CS_Log_Firewood_Short", CS + "Wood/CS_Firewood_Short_Thick_1", CS + "Wood/CS_Firewood_Short_Thick_2" };
const string edgeRoot = BK + "Prefabs/Plants/Branchs";
int edgeN = 0;
foreach (var lg in legs)
{
    var ps = lg.pts; float acc = 0f, next = R(edgeLow, edgeHigh);
    for (int i = 1; i < ps.Count; i++)
    {
        var a = P(ps[i - 1].x, ps[i - 1].z); var b = P(ps[i].x, ps[i].z); acc += UnityEngine.Vector2.Distance(a, b); if (acc < next) continue; next += R(edgeLow, edgeHigh);
        if (UnityEngine.Vector2.Distance(b, campC) < campR || b.x > coverFrontX) continue;
        var t = (b - a).normalized; var n = P(-t.y, t.x) * (rng.NextDouble() < 0.5 ? 1f : -1f); var q = b + n * (edgeOff + R(0f, 0.3f));
        double pick = rng.NextDouble(); bool isLog = pick < logShare, isRoot = !isLog && pick < logShare + rootShare;
        var g = Spawn(isLog ? edgeLogs[rng.Next(edgeLogs.Length)] : isRoot ? edgeRoot : CS + "Rocks and Stones/CS_Stone_" + (1 + rng.Next(8)), edges); if (g == null) continue;
        float along = UnityEngine.Mathf.Atan2(t.x, t.y) * UnityEngine.Mathf.Rad2Deg;
        if (isLog || isRoot) g.transform.rotation = UnityEngine.Quaternion.Euler(0f, along + R(-25f, 25f), 0f);
        else
        {
            g.transform.rotation = UnityEngine.Quaternion.Euler(R(-stoneTilt, stoneTilt), R(0f, 360f), R(-stoneTilt, stoneTilt));
            float s = R(stoneLow, stoneHigh) / UnityEngine.Mathf.Max(0.1f, Top(g) - Bottom(g)); g.transform.localScale = V(s * R(0.8f, 1.4f), s, s * R(0.8f, 1.4f));
            foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) r.sharedMaterial = edgeRock;
        }
        SitOn(g, q.x, q.y, 0.08f); edgeN++;
    }
}

// ---------- 4. junction markers (Valley.md 11) ----------
var markers = new UnityEngine.GameObject("JunctionMarkers").transform; markers.SetParent(root, false);
var plank = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat"); if (plank == null) return "no Planks023A_1.0x1.0.mat";
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 sc, UnityEngine.Quaternion rot, UnityEngine.Material m, bool collider)
{
    var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); g.name = name; g.transform.SetParent(parent, false); g.transform.SetPositionAndRotation(pos, rot); g.transform.localScale = sc;
    g.GetComponent<UnityEngine.Renderer>().sharedMaterial = m; if (!collider) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>()); return g;
}
void Label(UnityEngine.Transform board, string text)   // world-space text on both faces of a board (the project's sign pattern, build_signs.cs), unscaled by the board
{
    const float textScale = 0.01f, faceOff = 0.035f;
    foreach (var face in new[] { 1f, -1f })
    {
        var cg = new UnityEngine.GameObject("Label", typeof(UnityEngine.RectTransform)); cg.transform.SetParent(board.parent, false);
        var canvas = cg.AddComponent<UnityEngine.Canvas>(); canvas.renderMode = UnityEngine.RenderMode.WorldSpace;
        cg.GetComponent<UnityEngine.RectTransform>().sizeDelta = new UnityEngine.Vector2(board.localScale.x / textScale, board.localScale.y / textScale);
        cg.transform.SetPositionAndRotation(board.position + board.forward * faceOff * face, board.rotation * UnityEngine.Quaternion.Euler(0f, face > 0f ? 180f : 0f, 0f));
        cg.transform.localScale = V(textScale, textScale, textScale);
        var tg = new UnityEngine.GameObject("Text", typeof(UnityEngine.RectTransform)); tg.transform.SetParent(cg.transform, false);
        var trt = tg.GetComponent<UnityEngine.RectTransform>(); trt.anchorMin = UnityEngine.Vector2.zero; trt.anchorMax = UnityEngine.Vector2.one; trt.offsetMin = trt.offsetMax = UnityEngine.Vector2.zero;
        var tx = tg.AddComponent<UnityEngine.UI.Text>(); tx.font = font; tx.fontSize = 14; tx.fontStyle = UnityEngine.FontStyle.Bold; tx.alignment = UnityEngine.TextAnchor.MiddleCenter;
        tx.color = new UnityEngine.Color(0.95f, 0.9f, 0.8f); tx.text = text; tx.horizontalOverflow = UnityEngine.HorizontalWrapMode.Wrap; tx.resizeTextForBestFit = true; tx.resizeTextMinSize = 6; tx.resizeTextMaxSize = 24;
    }
}
UnityEngine.Vector2 LegToward(string leg, UnityEngine.Vector2 from, float along)   // a point 'along' metres down the named leg from its end nearest 'from'
{
    var l = legs.Find(x => x.name == leg).pts; if (l == null) return from; bool rev = UnityEngine.Vector2.Distance(P(l[0].x, l[0].z), from) > UnityEngine.Vector2.Distance(P(l[l.Count - 1].x, l[l.Count - 1].z), from);
    int idx = UnityEngine.Mathf.Clamp(UnityEngine.Mathf.RoundToInt(along / 2f), 0, l.Count - 1); var q = rev ? l[l.Count - 1 - idx] : l[idx]; return P(q.x, q.z);
}
// a point beside the named leg: 'along' metres from its end nearest 'from', then 'side' metres square to it (right when positive), so a
// marker stands off the tread (8.14a: the W1 blaze offset in x stood on the trail and stopped a walker)
const float blazeSide = 1.8f;
UnityEngine.Vector2 LegBeside(string leg, UnityEngine.Vector2 from, float along, float side)
{
    var a = LegToward(leg, from, along); var b = LegToward(leg, from, along + 2f); var t = (b - a).normalized; return a + new UnityEngine.Vector2(t.y, -t.x) * side;
}
// 8.14a gate (Pim, Marlow: "CMP1" for Camp 1, and one arm's label read into the next): arms 1.5 m by 0.3 m, 0.4 m apart, the text
// fitted inside its arm
const float postH = 2.6f, armLen = 1.5f, armH = 0.3f, armT = 0.05f, armGap = 0.4f, signAlong = 8f;
void Signpost(string name, UnityEngine.Vector2 at, (string text, UnityEngine.Vector2 toward)[] arms)
{
    var s = new UnityEngine.GameObject(name).transform; s.SetParent(markers, false); float gy = H(at.x, at.y); s.position = V(at.x, gy, at.y);
    Box("Post", s, V(at.x, gy + postH * 0.5f - 0.3f, at.y), V(0.14f, postH + 0.6f, 0.14f), UnityEngine.Quaternion.identity, plank, true);
    for (int i = 0; i < arms.Length; i++)
    {
        var d = (arms[i].toward - at).normalized; var rot = UnityEngine.Quaternion.LookRotation(V(d.x, 0f, d.y)) * UnityEngine.Quaternion.Euler(0f, 90f, 0f);
        var b = Box("Arm_" + arms[i].text, s, V(at.x + d.x * armLen * 0.5f, gy + postH - 0.2f - i * armGap, at.y + d.y * armLen * 0.5f), V(armLen, armH, armT), rot, plank, false);
        Label(b.transform, arms[i].text);
    }
}
void Blaze(string name, UnityEngine.Vector2 at, bool stump)
{
    var s = new UnityEngine.GameObject(name).transform; s.SetParent(markers, false); float gy = H(at.x, at.y); s.position = V(at.x, gy, at.y);
    float top = gy + 1.4f;
    if (stump) { var st = Spawn("Assets/Revolving Pizza Games/Cabin In The Woods/Prefabs/Vegetation/CITW_Tree_Stump", s); if (st != null) { SitOn(st, at.x, at.y, 0.05f); top = Top(st); } }
    else Box("Post", s, V(at.x, gy + 0.7f, at.y), V(0.14f, 1.8f, 0.14f), UnityEngine.Quaternion.identity, plank, true);
    Box("Blaze", s, V(at.x, top - 0.35f, at.y), V(0.2f, 0.3f, 0.2f), UnityEngine.Quaternion.identity, paleStone, false);   // a pale painted band
}
Signpost("Sign_Camp", P(165f, 158f), new[] { ("LAKE", LegToward("Camp to pump", campC, signAlong + campR)), ("CAMP 3", LegToward("Camp to Camp 3", campC, signAlong + campR)), ("SPRING", LegToward("Camp to J", campC, signAlong + campR)), ("LOT", LegToward("Camp to Jg", campC, signAlong + campR)) });
Signpost("Sign_Pump", P(193f, 99f), new[] { ("CAMP", LegToward("Camp to pump", P(190f, 96f), signAlong)), ("BOATHOUSE", LegToward("Pump to boathouse", P(190f, 96f), signAlong)), ("WEST SHORE", LegToward("Pump to W1", P(190f, 96f), signAlong)) });
Signpost("Sign_Jg", P(265f, 169f), new[] { ("CAMP", LegToward("Camp to Jg", P(262f, 172f), signAlong)), ("LOT", LegToward("Jg to T", P(262f, 172f), signAlong)), ("CAMP 1", LegToward("Jg to Camp 1", P(262f, 172f), signAlong)) });
Signpost("Sign_J", P(107f, 203f), new[] { ("CAMP", LegToward("Camp to J", P(104f, 206f), signAlong)), ("NORTH LOOP", LegToward("Camp 1 to J", P(104f, 206f), signAlong)) });
{
    // the trailhead board with the trail map at T (338, 170), facing the lot
    var tb = new UnityEngine.GameObject("Trailhead_Board").transform; tb.SetParent(markers, false); var at = P(338f, 172.5f); float gy = H(at.x, at.y); tb.position = V(at.x, gy, at.y);   // the marker's own position is where it stands (capture frames aim at it)
    foreach (var dz in new[] { -0.9f, 0.9f }) Box("Post", tb, V(at.x, gy + 1.0f, at.y + dz), V(0.14f, 2.0f, 0.14f), UnityEngine.Quaternion.identity, plank, true);
    var board = Box("Board", tb, V(at.x, gy + 1.5f, at.y), V(1.9f, 1.1f, 0.06f), UnityEngine.Quaternion.Euler(0f, 90f, 0f), plank, false);
    Label(board.transform, "VALLEY TRAILS");
}
Blaze("Blaze_W1_Camp3", LegBeside("W1 to Camp 3", P(128f, 70f), 6f, blazeSide), false);
// 8.15 gate (Pim: a workbench stood between the trail and the stump): the stump blaze on the north loop blazeLoopAlong m out of Camp 1
const float blazeLoopAlong = 14f; Blaze("Blaze_Camp1_Stump", LegBeside("Camp 1 to J", P(282f, 238f), blazeLoopAlong, blazeSide), true);

// ---------- 5. stops ----------
var stops = new UnityEngine.GameObject("Stops").transform; stops.SetParent(root, false);
string[] bushes = { "CS_Bush_Large_1", "CS_Bush_Large_1_1", "CS_Bush_Large_1_2", "CS_Bush_Large_1_3", "CS_Bush_Large_1_4", "CS_Bush_Large_2", "CS_Bush_Large_2_1", "CS_Bush_Large_2_2", "CS_Bush_Large_2_3", "CS_Bush_Large_2_4" };
// a hedge: brush every hedgeStep along the line (pushed toward the closed side), a hollow log every few metres, and inside it a
// collider hedgeColH over the highest ground under it, on the Ignore Raycast layer (sight checks see the brush, not the box)
const float hedgeStep = 1.4f, bushLow = 2.0f, bushHigh = 2.5f, hedgeColH = 1.8f, hedgeColT = 1.0f, hedgeJitter = 0.35f; const int logEveryBush = 9;
int bushN = 0, hedgeCols = 0; float hedgeLen = 0f;
void Hedge(string name, System.Collections.Generic.List<UnityEngine.Vector2> line, float closedSide)   // closedSide +1: the closed ground is on the line's left
{
    if (line.Count < 2) return;
    var h = new UnityEngine.GameObject(name).transform; h.SetParent(stops, false);
    float acc = 0f, next = 0f; int n = 0;
    for (int i = 0; i < line.Count - 1; i++)
    {
        var a = line[i]; var b = line[i + 1]; float len = UnityEngine.Vector2.Distance(a, b); if (len < 0.01f) continue; var t = (b - a) / len; var nl = P(-t.y, t.x) * closedSide;
        // the collider
        float gTop = UnityEngine.Mathf.Max(H(a.x, a.y), H(b.x, b.y)), gLow = UnityEngine.Mathf.Min(H(a.x, a.y), H(b.x, b.y)); var mid = (a + b) * 0.5f;
        var col = new UnityEngine.GameObject("HedgeCollider"); col.transform.SetParent(h, false); col.layer = 2;
        col.transform.SetPositionAndRotation(V(mid.x, (gLow - 0.5f + gTop + hedgeColH) * 0.5f, mid.y), UnityEngine.Quaternion.LookRotation(V(t.x, 0f, t.y)));
        col.AddComponent<UnityEngine.BoxCollider>().size = V(hedgeColT, gTop + hedgeColH - (gLow - 0.5f), len + 0.3f); hedgeCols++; hedgeLen += len;
        // the brush over it
        while (next <= acc + len)
        {
            var q = a + t * (next - acc) + nl * R(0.1f, hedgeJitter) + P(-t.y, t.x) * R(-0.1f, 0.1f); next += hedgeStep; n++;
            bool log = n % logEveryBush == 0;
            var g = Spawn(log ? BK + "Prefabs/HollowLogs/RedwoodHollowLog_" + rng.Next(3) : CS + "Vegetation/" + bushes[rng.Next(bushes.Length)], h); if (g == null) continue;
            g.transform.rotation = UnityEngine.Quaternion.Euler(0f, log ? UnityEngine.Mathf.Atan2(t.x, t.y) * UnityEngine.Mathf.Rad2Deg + R(-15f, 15f) : R(0f, 360f), 0f);
            float want = log ? 1.2f : R(bushLow, bushHigh), s = want / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(s, s, s);
            SitOn(g, q.x, q.y, 0.1f); bushN++;
        }
        acc += len;
    }
}
System.Collections.Generic.List<UnityEngine.Vector2> Line(params UnityEngine.Vector2[] pts) => new System.Collections.Generic.List<UnityEngine.Vector2>(pts);
// (a) the old burn and the knoll switchbacks: closed ground traced round the trail corridors (marching squares on a 0.5 m grid, as
// the rev 7 thicket traced its walls), so the hedge follows each trail and junctions and side pieces stay open
var knollHull = new[] { P(152f, 145f), P(170f, 160f), P(188f, 133f), P(190f, 96f), P(176f, 105f) };
const float corridorHW = 3.5f, gridCell = 0.5f, gx0 = 145f, gx1 = 345f, gz0 = 90f, gz1 = 220f, poiPad = 2.5f;   // corridorHW 3.5 (was 2.5, 8.15 gate: hedges 1 m further from the band so it takes sun)
int GW = UnityEngine.Mathf.RoundToInt((gx1 - gx0) / gridCell), GH = UnityEngine.Mathf.RoundToInt((gz1 - gz0) / gridCell);
var fld = new float[GW + 1, GH + 1];   // > 0 walkable, < 0 closed
for (int i = 0; i <= GW; i++) for (int j = 0; j <= GH; j++)
{
    var p = P(gx0 + i * gridCell, gz0 + j * gridCell);
    bool closed = Inside(p, burnPoly) || Inside(p, knollHull);
    fld[i, j] = closed ? -1f : 1f;
}
void Stamp(UnityEngine.Vector2 c, float r)
{
    int i0 = UnityEngine.Mathf.Max(0, (int)((c.x - r - gx0) / gridCell) - 1), i1 = UnityEngine.Mathf.Min(GW, (int)((c.x + r - gx0) / gridCell) + 1);
    int j0 = UnityEngine.Mathf.Max(0, (int)((c.y - r - gz0) / gridCell) - 1), j1 = UnityEngine.Mathf.Min(GH, (int)((c.y + r - gz0) / gridCell) + 1);
    for (int i = i0; i <= i1; i++) for (int j = j0; j <= j1; j++) { float v = r - UnityEngine.Vector2.Distance(c, P(gx0 + i * gridCell, gz0 + j * gridCell)); if (v > fld[i, j]) fld[i, j] = v; }
}
foreach (var p in allPts) Stamp(p, corridorHW);
Stamp(campC, campR); Stamp(P(262f, 172f), 4f); Stamp(P(340f, 170f), 4f);
foreach (var q in pois) Stamp(q, poiPad + 1.5f);
// 8.14a (Marlow: the Lake pump, Office and Jg warps landed inside brush): every warp keeps warpClear m open round it
const float warpClear = 5.5f; foreach (UnityEngine.Transform w in Root("DevWarps").transform) Stamp(P(w.position.x, w.position.z), warpClear);
// marching squares: segments between edge crossings, chained into loops
var ptOf = new System.Collections.Generic.Dictionary<long, UnityEngine.Vector2>(); var adj = new System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>>();
long EKey(int i, int j, int dir) => ((long)j * (GW + 1) + i) * 2 + dir;
UnityEngine.Vector2 EPoint(int i, int j, int dir) { float va = fld[i, j], vb = dir == 0 ? fld[i + 1, j] : fld[i, j + 1]; float t = va / (va - vb); return dir == 0 ? P(gx0 + (i + t) * gridCell, gz0 + j * gridCell) : P(gx0 + i * gridCell, gz0 + (j + t) * gridCell); }
void Link(long a, long b, UnityEngine.Vector2 pa, UnityEngine.Vector2 pb)
{
    ptOf[a] = pa; ptOf[b] = pb;
    if (!adj.TryGetValue(a, out var la)) adj[a] = la = new System.Collections.Generic.List<long>(); la.Add(b);
    if (!adj.TryGetValue(b, out var lb)) adj[b] = lb = new System.Collections.Generic.List<long>(); lb.Add(a);
}
for (int i = 0; i < GW; i++) for (int j = 0; j < GH; j++)
{
    int cs = (fld[i, j] > 0 ? 1 : 0) | (fld[i + 1, j] > 0 ? 2 : 0) | (fld[i + 1, j + 1] > 0 ? 4 : 0) | (fld[i, j + 1] > 0 ? 8 : 0);
    if (cs == 0 || cs == 15) continue;
    long B = EKey(i, j, 0), Rr = EKey(i + 1, j, 1), T = EKey(i, j + 1, 0), Lf = EKey(i, j, 1);
    UnityEngine.Vector2 pB() => EPoint(i, j, 0); UnityEngine.Vector2 pR() => EPoint(i + 1, j, 1); UnityEngine.Vector2 pT() => EPoint(i, j + 1, 0); UnityEngine.Vector2 pL() => EPoint(i, j, 1);
    bool centre = (fld[i, j] + fld[i + 1, j] + fld[i + 1, j + 1] + fld[i, j + 1]) > 0f;
    switch (cs)
    {
        case 1: case 14: Link(Lf, B, pL(), pB()); break;
        case 2: case 13: Link(B, Rr, pB(), pR()); break;
        case 3: case 12: Link(Lf, Rr, pL(), pR()); break;
        case 4: case 11: Link(Rr, T, pR(), pT()); break;
        case 6: case 9: Link(B, T, pB(), pT()); break;
        case 7: case 8: Link(Lf, T, pL(), pT()); break;
        case 5: if (centre) { Link(B, Rr, pB(), pR()); Link(T, Lf, pT(), pL()); } else { Link(Lf, B, pL(), pB()); Link(Rr, T, pR(), pT()); } break;
        case 10: if (centre) { Link(Lf, B, pL(), pB()); Link(Rr, T, pR(), pT()); } else { Link(B, Rr, pB(), pR()); Link(T, Lf, pT(), pL()); } break;
    }
}
var seen = new System.Collections.Generic.HashSet<long>(); int loopN = 0;
System.Collections.Generic.List<UnityEngine.Vector2> Simplify(System.Collections.Generic.List<UnityEngine.Vector2> pts, float tol)
{
    var keep = new bool[pts.Count]; keep[0] = keep[pts.Count - 1] = true; var st = new System.Collections.Generic.Stack<(int, int)>(); st.Push((0, pts.Count - 1));
    while (st.Count > 0) { var (a, b) = st.Pop(); float best = 0f; int bi = -1; for (int i = a + 1; i < b; i++) { float d = SegD(pts[i], pts[a], pts[b]); if (d > best) { best = d; bi = i; } } if (bi >= 0 && best > tol) { keep[bi] = true; st.Push((a, bi)); st.Push((bi, b)); } }
    var o = new System.Collections.Generic.List<UnityEngine.Vector2>(); for (int i = 0; i < pts.Count; i++) if (keep[i]) o.Add(pts[i]); return o;
}
bool Walkable(UnityEngine.Vector2 q) { int i = UnityEngine.Mathf.RoundToInt((q.x - gx0) / gridCell), j = UnityEngine.Mathf.RoundToInt((q.y - gz0) / gridCell); return i < 0 || j < 0 || i > GW || j > GH || fld[i, j] > 0f; }
foreach (var k0 in adj.Keys)
{
    if (seen.Contains(k0)) continue;
    var loop = new System.Collections.Generic.List<UnityEngine.Vector2>(); long prev = -1, cur = k0;
    while (true) { seen.Add(cur); loop.Add(ptOf[cur]); long nx = -1; foreach (var m in adj[cur]) if (m != prev && !seen.Contains(m)) { nx = m; break; } if (nx < 0) break; prev = cur; cur = nx; }
    if (loop.Count < 3) continue; loop.Add(loop[0]);
    var line = Simplify(loop, 0.3f);
    // which side is closed: test a point a metre to the left of the first segment
    var t0 = (line[1] - line[0]).normalized; var probe = (line[0] + line[1]) * 0.5f + P(-t0.y, t0.x);
    Hedge("Hedge_Burn_" + (loopN++), line, Walkable(probe) ? -1f : 1f);
}
// (b) straight closures (Valley.md 8 and 9.2): the south-east corner, the W foot pocket, both ends of the lake's south shore
Hedge("Hedge_SE_North", Line(P(345f, 100f), P(395.8f, 100f)), -1f);
Hedge("Hedge_SE_West", Line(P(345f, -3f), P(345f, 100f)), -1f);
Hedge("Hedge_WFootPocket", Line(P(46.5f, 166f), P(84f, 166f), P(84f, 199.6f)), 1f);
Hedge("Hedge_LakeSouth_West", Line(P(140f, 50f), P(140f, 1f)), 1f);
Hedge("Hedge_LakeSouth_East", Line(P(248.6f, 49.3f), P(248.6f, -4f)), -1f);
// (c) the mid pocket (215, 222): a ring of brush round its thicket core
{ var ring = new System.Collections.Generic.List<UnityEngine.Vector2>(); for (int i = 0; i <= 32; i++) { float a = i * UnityEngine.Mathf.PI * 2f / 32f; ring.Add(P(215f + UnityEngine.Mathf.Cos(a) * 12f, 222f + UnityEngine.Mathf.Sin(a) * 12f)); } Hedge("Hedge_MidPocket", ring, 1f); }
// (d) 8.6's gray brush bands round the closed campground: owned brush over them, the gray boxes stop drawing (their colliders stay)
int bandsDressed = 0;
var fzBrush = Root("FrontZone").transform.Find("BrushBands");
if (fzBrush != null) foreach (UnityEngine.Transform b in fzBrush)
{
    var r = b.GetComponent<UnityEngine.Renderer>(); if (r != null) r.enabled = false;
    var bb = b.GetComponent<UnityEngine.Collider>().bounds; var band = new UnityEngine.GameObject("Dress_" + b.name).transform; band.SetParent(stops, false);
    for (float x = bb.min.x + 0.7f; x <= bb.max.x - 0.3f; x += hedgeStep) for (float z = bb.min.z + 0.7f; z <= bb.max.z - 0.3f; z += hedgeStep)
    {
        var g = Spawn(CS + "Vegetation/" + bushes[rng.Next(bushes.Length)], band); if (g == null) continue;
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); float s = R(bushLow, bushHigh) / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(s, s, s);
        SitOn(g, x + R(-0.3f, 0.3f), z + R(-0.3f, 0.3f), 0.1f); bushN++;
    }
    bandsDressed++;
}
// (e) rock rims: 1.3 m over the ground outside, 1.2 m thick, open where a trail goes in (gap within rimGap of a trail point)
const float rimH = 1.3f, rimThick = 1.2f, rimGap = 2.6f, rimStep = 0.5f;
var rockMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_BandRock.mat"); if (rockMat == null) return "no Blockout_BandRock.mat (8.1)";
// 8.14a gate (Marlow: 0.5 m blocks each at its own height made every top edge a staircase): each run between trail gaps is one
// continuous wall, its top the ground's high side plus rimH, smoothed over rimSmooth samples either way
const int rimSmooth = 4;
var runs = new System.Collections.Generic.List<System.Collections.Generic.List<(UnityEngine.Vector2 c, UnityEngine.Vector2 o, float hi, float lo)>>(); int rimSegs = 0;
void Rim(UnityEngine.Vector2[] poly, float outSide)   // outSide +1: the high (outer) ground is on the line's left; the rock sits there
{
    System.Collections.Generic.List<(UnityEngine.Vector2 c, UnityEngine.Vector2 o, float hi, float lo)> run = null;
    for (int i = 0; i < poly.Length - 1; i++)
    {
        var a = poly[i]; var b = poly[i + 1]; float len = UnityEngine.Vector2.Distance(a, b); var t = (b - a) / len; var o = P(-t.y, t.x) * outSide;
        for (float s = 0f; s < len; s += rimStep)
        {
            var c = a + t * s; bool gap = false; foreach (var q in allPts) if (UnityEngine.Vector2.Distance(q, c + o * (rimThick * 0.5f)) < rimGap) { gap = true; break; }
            if (gap) { run = null; continue; }
            if (run == null) { run = new System.Collections.Generic.List<(UnityEngine.Vector2, UnityEngine.Vector2, float, float)>(); runs.Add(run); }
            float gHi = UnityEngine.Mathf.Max(H(c.x, c.y), H(c.x + o.x * rimThick, c.y + o.y * rimThick)), gLo = UnityEngine.Mathf.Min(H(c.x, c.y), H(c.x - o.x * rimThick, c.y - o.y * rimThick));
            run.Add((c, o, gHi, gLo)); rimSegs++;
        }
    }
}
// the ravine round the cave mouth (8.1's ravine polygon), east of the W band (x 38); the Camp 3 hollow's edge at r 12.5
Rim(new[] { P(38f, 21.6f), P(80f, 25.2f), P(94.8f, 50f), P(74f, 60f), P(70f, 62f), P(38f, 56.6f) }, -1f);
{ var hol = new UnityEngine.Vector2[33]; for (int i = 0; i <= 32; i++) { float a = -i * UnityEngine.Mathf.PI * 2f / 32f; hol[i] = P(78f + UnityEngine.Mathf.Cos(a) * 12.5f, 146f + UnityEngine.Mathf.Sin(a) * 12.5f); } Rim(hol, 1f); }
{
    var vs = new System.Collections.Generic.List<UnityEngine.Vector3>(); var uv = new System.Collections.Generic.List<UnityEngine.Vector2>(); var tris = new System.Collections.Generic.List<int>();
    void Quad(UnityEngine.Vector3 p0, UnityEngine.Vector3 p1, UnityEngine.Vector3 p2, UnityEngine.Vector3 p3, UnityEngine.Vector3 outward)
    {
        if (UnityEngine.Vector3.Dot(UnityEngine.Vector3.Cross(p1 - p0, p2 - p0), outward) < 0f) { var sw = p1; p1 = p3; p3 = sw; }
        bool top = outward.y > 0.5f; int i0 = vs.Count;
        foreach (var v in new[] { p0, p1, p2, p3 }) { vs.Add(v); uv.Add(top ? P(v.x / 4f, v.z / 4f) : P((v.x + v.z) / 4f, v.y / 4f)); }
        tris.AddRange(new[] { i0, i0 + 1, i0 + 2, i0, i0 + 2, i0 + 3 });
    }
    foreach (var run in runs)
    {
        if (run.Count < 2) continue; int n = run.Count; var top = new float[n];
        for (int i = 0; i < n; i++) { float sum = 0f; int k = 0; for (int j = UnityEngine.Mathf.Max(0, i - rimSmooth); j <= UnityEngine.Mathf.Min(n - 1, i + rimSmooth); j++) { sum += run[j].hi; k++; } top[i] = UnityEngine.Mathf.Max(run[i].hi, sum / k) + rimH; }
        for (int i = 0; i < n - 1; i++)
        {
            var r0 = run[i]; var r1 = run[i + 1]; var o3 = V(r0.o.x, 0f, r0.o.y); var t3 = V(r1.c.x - r0.c.x, 0f, r1.c.y - r0.c.y);
            UnityEngine.Vector3 F(UnityEngine.Vector2 c, float y) => V(c.x, y, c.y);
            var f0b = F(r0.c, r0.lo - 1f); var f1b = F(r1.c, r1.lo - 1f); var f0t = F(r0.c, top[i]); var f1t = F(r1.c, top[i + 1]);
            var b0b = F(r0.c + r0.o * rimThick, r0.lo - 1f); var b1b = F(r1.c + r1.o * rimThick, r1.lo - 1f); var b0t = F(r0.c + r0.o * rimThick, top[i]); var b1t = F(r1.c + r1.o * rimThick, top[i + 1]);
            Quad(f0b, f1b, f1t, f0t, -o3); Quad(b0b, b1b, b1t, b0t, o3); Quad(f0t, f1t, b1t, b0t, UnityEngine.Vector3.up);
            if (i == 0) Quad(f0b, f0t, b0t, b0b, -t3);
            if (i == n - 2) Quad(f1b, f1t, b1t, b1b, t3);
        }
    }
    var mesh = new UnityEngine.Mesh { name = "Rims815", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
    mesh.SetVertices(vs); mesh.SetUVs(0, uv); mesh.SetTriangles(tris, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
    UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/Terrain/Main3/Rims815.asset");
    var go = new UnityEngine.GameObject("Rims"); go.transform.SetParent(stops, false);
    go.AddComponent<UnityEngine.MeshFilter>().sharedMesh = mesh; go.AddComponent<UnityEngine.MeshRenderer>().sharedMaterial = rockMat; go.AddComponent<UnityEngine.MeshCollider>().sharedMesh = mesh;
}
// the lake's south belt: brush down to the water between the two south hedges, looks only (the wade limit holds the water's edge)
int shoreBush = 0;
for (float x = 142f; x <= 246f; x += 2.2f)
{
    float dz = 1f - ((x - lakeC.x) / lakeA) * ((x - lakeC.x) / lakeA); if (dz <= 0f) continue; float shoreZ = lakeC.y - lakeB * UnityEngine.Mathf.Sqrt(dz);
    var g = Spawn(CS + "Vegetation/" + bushes[rng.Next(bushes.Length)], stops); if (g == null) continue;
    g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); float s = R(bushLow, bushHigh) / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(s, s, s);
    SitOn(g, x + R(-0.6f, 0.6f), shoreZ - R(1.5f, 4f), 0.1f); shoreBush++;
}

// 8.15 gate (Vesper: the hedges are grass green, Style.md 2.4.4): every Campsite bush this recipe placed takes an olive copy of the pack's
// vegetation material (its colour multiplied by bushOlive); the pack material itself is untouched
UnityEngine.ColorUtility.TryParseHtmlString("#E0CC80", out var bushOlive);
var oliveCache = new System.Collections.Generic.Dictionary<UnityEngine.Material, UnityEngine.Material>(); int oliveN = 0;
foreach (var r in root.GetComponentsInChildren<UnityEngine.Renderer>(true))
{
    var src = r.sharedMaterial; if (src == null || src.name != "CS_Vegetation") continue;
    if (!oliveCache.TryGetValue(src, out var ol))
    {
        string path = "Assets/Materials/Slice/Slice_BushOlive.mat"; ol = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
        if (ol == null) { ol = new UnityEngine.Material(src); UnityEditor.AssetDatabase.CreateAsset(ol, path); } else ol.CopyPropertiesFromMaterial(src);
        ol.shader = src.shader; ol.SetColor("_BaseColor", bushOlive); UnityEditor.EditorUtility.SetDirty(ol); oliveCache[src] = ol;
    }
    r.sharedMaterial = ol; oliveN++;
}

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | layers: floor GrassPine, SoilPine added, shore and burn GrassMud, trail Ground054, rock Rocks_a | cover " + detailNames.Length + " detail kinds | trail edges " + edgeN
    + " | markers " + markers.childCount + " | hedges: " + loopN + " traced round the burn and knoll, " + hedgeCols + " collider pieces, " + hedgeLen.ToString("F0") + " m, " + bushN + " brush and logs, " + bandsDressed + " front bands dressed, " + shoreBush + " shore bushes, " + oliveN + " bush renderers olive, rock on " + rockCells + " slope cells | rims " + rimSegs + " pieces | missing: " + (missing.Count == 0 ? "none" : string.Join(", ", missing));
