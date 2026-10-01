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
// 8.16a: owned boulders over the pump trench and Camp 3 hollow faces (5 f); BK trees and hollow logs keep their trunk colliders.
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
// 8.16a (Marlow: players walked through trunks): a BK tree gets one capsule on its trunk, measured from its first LOD's bark: the
// median reach of the bark vertices between trunkBandLow and trunkBandHigh of the bark's height, about their centre, up trunkShare of
// that height (the pack's own colliders are removed: on the giants they are 0.7 m thick inside a 1.7 m trunk). Hollow logs keep the
// pack's log mesh collider. Foliage never collides.
const float trunkBandLow = 0.01f, trunkBandHigh = 0.06f, trunkBandMax = 0.4f, trunkShare = 0.3f; const int trunkMinVerts = 8;
var trunkCache = new System.Collections.Generic.Dictionary<UnityEngine.Mesh, (UnityEngine.Vector3 c, float r, float h)>();
bool IsPackTree(string path) => path.Contains("PureNature_Redwood/Prefabs/Trees/");
bool IsPackLog(string path) => path.Contains("PureNature_Redwood/Prefabs/HollowLogs/");
void TrunkCollider(UnityEngine.GameObject g)
{
    foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    var lod = g.GetComponent<UnityEngine.LODGroup>(); var r0 = lod != null ? lod.GetLODs()[0].renderers[0] : g.GetComponentInChildren<UnityEngine.MeshRenderer>(); if (r0 == null) return;
    var mf = r0.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) return; var m = mf.sharedMesh;
    if (!trunkCache.TryGetValue(m, out var tc))
    {
        var mats = r0.sharedMaterials; var vs = m.vertices; var bark = new System.Collections.Generic.HashSet<int>();
        for (int s = 0; s < m.subMeshCount && s < mats.Length; s++) { var n = mats[s] != null ? mats[s].name : ""; if (n.Contains("Leaves") || n.Contains("Branches")) continue; foreach (var ix in m.GetTriangles(s)) bark.Add(ix); }
        float y0 = float.MaxValue, y1 = float.MinValue; foreach (var ix in bark) { y0 = UnityEngine.Mathf.Min(y0, vs[ix].y); y1 = UnityEngine.Mathf.Max(y1, vs[ix].y); }
        float h = y1 - y0, lo = y0 + h * trunkBandLow, hi = y0 + h * trunkBandHigh, cx = 0f, cz = 0f; int n0 = 0;
        // a low-poly trunk has rings far apart: widen the band upward until it holds trunkMinVerts bark vertices (at most trunkBandMax)
        for (float band = trunkBandHigh; band <= trunkBandMax; band *= 2f) { int k = 0; hi = y0 + h * band; foreach (var ix in bark) if (vs[ix].y >= lo && vs[ix].y <= hi) k++; if (k >= trunkMinVerts) break; }
        foreach (var ix in bark) if (vs[ix].y >= lo && vs[ix].y <= hi) { cx += vs[ix].x; cz += vs[ix].z; n0++; }
        if (n0 == 0) return; cx /= n0; cz /= n0;
        var d = new System.Collections.Generic.List<float>(); foreach (var ix in bark) if (vs[ix].y >= lo && vs[ix].y <= hi) d.Add(UnityEngine.Mathf.Sqrt((vs[ix].x - cx) * (vs[ix].x - cx) + (vs[ix].z - cz) * (vs[ix].z - cz))); d.Sort();
        tc = (new UnityEngine.Vector3(cx, y0, cz), d[d.Count / 2], h); trunkCache[m] = tc;
    }
    // the round bottom end sits below the base, so the trunk is its full radius at the ground (a rounded end at the base let feet under it)
    var cap = mf.gameObject.AddComponent<UnityEngine.CapsuleCollider>(); cap.direction = 1; cap.radius = tc.r; cap.height = tc.h * trunkShare + tc.r * 2f;
    cap.center = new UnityEngine.Vector3(tc.c.x, tc.c.y - tc.r + cap.height * 0.5f, tc.c.z);
}
UnityEngine.GameObject Spawn(string path, UnityEngine.Transform parent)
{
    var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path + ".prefab"); if (pf == null) { if (!missing.Contains(path)) missing.Add(path); return null; }
    var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pf, parent);
    // dressing never catches the player; stops carry their own colliders. 8.16a (Marlow: players walked through trunks): BK trees and
    // hollow logs keep collision (TrunkCollider above; the pack log mesh), never on foliage
    if (IsPackTree(path)) TrunkCollider(g); else if (!IsPackLog(path)) foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
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
// the old burn: ValleyShapes.BurnOutline (ragged, RebuildSpecs 1.7)

// ---------- 1. ground layers ----------
UnityEngine.Texture2D Tex(string path) { var t = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(path); if (t == null) missing.Add(path); return t; }
const string surf = BK + "Textures/Surfaces/";
UnityEngine.TerrainLayer LayerNamed(string n) { foreach (var l in data.terrainLayers) if (l != null && l.name == n) return l; return null; }
void SetLayer(UnityEngine.TerrainLayer l, UnityEngine.Texture2D albedo, UnityEngine.Texture2D normal, float tile, UnityEngine.Color remap)
{
    l.diffuseTexture = albedo; l.normalMapTexture = normal; l.tileSize = new UnityEngine.Vector2(tile, tile);
    l.diffuseRemapMin = UnityEngine.Vector4.zero; l.diffuseRemapMax = new UnityEngine.Vector4(remap.r, remap.g, remap.b, 1f); UnityEditor.EditorUtility.SetDirty(l);
    UnityEditor.AssetDatabase.SaveAssetIfDirty(l);   // saved at once: a later CreateAsset in this recipe can reimport a new layer from disk and drop unsaved textures (8.16 gate: four layers drew as a grey checker)
}
const float floorTile = 4f, trailTile = 2f, rockTile = 10f, rockLuma = 0.55f, trailLift = 1.75f, burnDim = 0.65f;   // 1.75 (1.65 before 8.16; 1.9 put sunlit trail at 5 m over the 110 cap, Wren 8.16 gate)   // trailLift: the dirt brightened so the tread stands 20 grey over the floor 20 m ahead (Gate.md 4; 8.14a measured about 10)
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

// RebuildSpecs 2 (Vesper 2026-10-01: GrassPine's red and green blotches checkered the open lot and the ground across the road): over the
// lot side (x lotX0 and east, z lotZ0 to lotZ1, blended over lotBlend m) the floor is SoilPine at a lotTile m tile on its own layer,
// tinted so the texture's mean comes to lotTarget (the albedo mean; what the screen shows after light and the filter is graded by eye),
// with GrassPine left only in irregular patches (Perlin blobs about lotPatchScale m across) of at most lotGrassMax weight. Checked: URP's
// Terrain/Lit multiplies each layer's albedo by its diffuse remap (TerrainLitPasses.hlsl, _DiffuseRemapScale0 to 3), so the tint applies.
const string lotPath = "Assets/Terrain/Main3/Layer_LotSoil.terrainlayer", lotTarget = "#5E5440";
const float lotTile = 6f, lotX0 = 330f, lotZ0 = 120f, lotZ1 = 240f, lotBlend = 10f, lotPatchScale = 30f, lotPatchFrom = 0.55f, lotPatchSpan = 0.15f, lotGrassMax = 0.3f;
UnityEngine.Color MeanLinear(UnityEngine.Texture tex)   // alpha-weighted mean of a texture in linear colour (as 8.9f's retints)
{
    var rt = UnityEngine.RenderTexture.GetTemporary(64, 64, 0, UnityEngine.RenderTextureFormat.ARGBFloat, UnityEngine.RenderTextureReadWrite.Linear);
    UnityEngine.Graphics.Blit(tex, rt); var prev = UnityEngine.RenderTexture.active; UnityEngine.RenderTexture.active = rt;
    var t2 = new UnityEngine.Texture2D(64, 64, UnityEngine.TextureFormat.RGBAFloat, false, true); t2.ReadPixels(new UnityEngine.Rect(0, 0, 64, 64), 0, 0); t2.Apply();
    UnityEngine.RenderTexture.active = prev; UnityEngine.RenderTexture.ReleaseTemporary(rt);
    float w = 0f; var sum = UnityEngine.Color.black; foreach (var px in t2.GetPixels()) { sum += px * px.a; w += px.a; } UnityEngine.Object.DestroyImmediate(t2);
    return w > 0f ? sum / w : UnityEngine.Color.gray;
}
var soilTex = Tex(surf + "SoilPine_a.png"); UnityEngine.ColorUtility.TryParseHtmlString(lotTarget, out var lotC); var lotLin = lotC.linear; var soilMean = soilTex != null ? MeanLinear(soilTex) : UnityEngine.Color.gray;
var lLot = new UnityEngine.TerrainLayer { name = "Layer_LotSoil" }; UnityEditor.AssetDatabase.CreateAsset(lLot, lotPath);
SetLayer(lLot, soilTex, Tex(surf + "SoilPine_n.png"), lotTile, new UnityEngine.Color(lotLin.r / UnityEngine.Mathf.Max(0.01f, soilMean.r), lotLin.g / UnityEngine.Mathf.Max(0.01f, soilMean.g), lotLin.b / UnityEngine.Mathf.Max(0.01f, soilMean.b)));
layerList.Add(lLot); data.terrainLayers = layerList.ToArray(); int iLot = layerList.IndexOf(lLot);
{
    int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); float aX = size.x / ares, aZ = size.z / ares;
    for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
    {
        var p = P(tOrg.x + (xi + 0.5f) * aX, tOrg.z + (zi + 0.5f) * aZ); float g = alpha[zi, xi, iGround]; if (g <= 0f) continue;
        float outside = UnityEngine.Mathf.Max(lotX0 - p.x, UnityEngine.Mathf.Max(lotZ0 - p.y, p.y - lotZ1)); float w = 1f - SS(outside / lotBlend); if (w <= 0f) continue;
        float grass = lotGrassMax * UnityEngine.Mathf.Clamp01((UnityEngine.Mathf.PerlinNoise(p.x / lotPatchScale + 7.7f, p.y / lotPatchScale + 3.1f) - lotPatchFrom) / lotPatchSpan);
        float take = g * w * (1f - grass); alpha[zi, xi, iGround] = g - take; alpha[zi, xi, iLot] += take;
    }
    data.SetAlphamaps(0, 0, alpha);
}

// 8.15 gate (Pim W1: beside Jg to Camp 1 the pale clearing grass read as light as the dirt; 8.16: edge 1.3 and duff from 1.5 with the
// 1.8 m band): a strip of darker duff (SoilPine dimmed to
// duffDim) from duffIn to duffOut off every trail's centre points (2 m apart), taken duffShare from the floor layers
const string duffPath = "Assets/Terrain/Main3/Layer_Duff.terrainlayer"; const float duffDim = 0.45f, duffIn = 1.5f, duffOut = 3.5f, duffBlend = 0.8f, duffShare = 0.8f;
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
        foreach (int l in new[] { iGround, iBurn, iSoil, iLot }) { float take = alpha[zi, xi, l] * w; alpha[zi, xi, l] -= take; alpha[zi, xi, iDuff] += take; }
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
const float screeTile = 1.2f, screeLift = 0.75f, ashGrey = 0.4f, climbPaint = 12f, climbMouthX = 86f, climbSteep = 42f;   // scree 0.9 and ash 0.5 (were 1.25, 0.62; 8.15 gate: darker beside the climb's tread)
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
// 8.16a (Wren: standing snags stopped nobody): a capsule on a dead tree's trunk only, measured from its mesh: the median reach of the
// vertices in the lowest snagTrunkBand of its height about their centre, up snagTrunkShare of its height (limbs start above that)
const float snagTrunkBand = 0.15f, snagTrunkShare = 0.4f;
void SnagTrunk(UnityEngine.GameObject g)
{
    foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);   // the pack's convex hull takes in the limbs
    var mf = g.GetComponentInChildren<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) return;
    var m = mf.sharedMesh; float y0 = m.bounds.min.y, h = m.bounds.size.y, cx = 0f, cz = 0f; int n = 0; var vs = m.vertices;
    foreach (var v in vs) if (v.y < y0 + h * snagTrunkBand) { cx += v.x; cz += v.z; n++; } if (n == 0) return; cx /= n; cz /= n;
    var d = new System.Collections.Generic.List<float>(); foreach (var v in vs) if (v.y < y0 + h * snagTrunkBand) d.Add(UnityEngine.Mathf.Sqrt((v.x - cx) * (v.x - cx) + (v.z - cz) * (v.z - cz))); d.Sort();
    var cap = mf.gameObject.AddComponent<UnityEngine.CapsuleCollider>(); cap.direction = 1; cap.radius = d[d.Count / 2]; cap.height = h * snagTrunkShare; cap.center = new UnityEngine.Vector3(cx, y0 + h * snagTrunkShare * 0.5f, cz);
}
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
    if (climbDress.childCount > before) SnagTrunk(climbDress.GetChild(climbDress.childCount - 1).gameObject);
    Dress(CS + "Wood/CS_Log_Firewood_Burnt", s + 3f, (rng.NextDouble() < 0.5 ? 1f : -1f) * dressOff, 0.4f, 0f);
}
for (float s = sP3 + 3f; s < sP4 - firEndBeforeP4; s += firStep) Dress(BK + "Prefabs/Trees/RedFir" + (1 + rng.Next(4)), s, dressOff + R(0.3f, 1.2f), R(3f, 6f), 0f);
// 8.16a (ClimbFix.md 1 and 3: trees, not rock, fill the rise; no frame more than 30 percent rock): fir knots on the uphill apron of
// every leg from P1 to P4, knotIn to knotOut m off the tread, knotSize trees a knot every knotStep m, firs and pines (knotLow to
// knotHigh m). A spot steeper than knotSlope or within knotIn of any climb point is skipped. Trunk capsules as every BK tree (TrunkCollider).
const float knotStep = 7f, knotIn = 3.2f, knotOut = 6.5f, knotLow = 8f, knotHigh = 15f, knotSlope = 42f, knotUpProbe = 5f; const int knotSize = 3;
int knotN = 0; var knotRng = new System.Random(81616); float KR(float a, float b) => a + (float)knotRng.NextDouble() * (b - a);   // its own random stream, so the hedges and stops after it keep their places
for (float s = sP1 + 2f; s < sP4 - 2f; s += knotStep)   // not in the chute: saplings there hid the chute and P1 lanterns at night (Gate.md 4 N1)
{
    var c = ClimbAt(s, out var tan); var nrm = P(-tan.y, tan.x);
    float up = H(c.x + nrm.x * knotUpProbe, c.z + nrm.y * knotUpProbe) - H(c.x - nrm.x * knotUpProbe, c.z - nrm.y * knotUpProbe);
    foreach (var sideSign in new[] { up >= 0f ? 1f : -1f })
        for (int k = 0; k < knotSize; k++)
        {
            var q = P(c.x, c.z) + nrm * sideSign * KR(knotIn, knotOut) + tan * KR(-2f, 2f);
            if (data.GetSteepness((q.x - tOrg.x) / size.x, (q.y - tOrg.z) / size.z) > knotSlope) continue;
            bool near = false; foreach (var cp in climbPts) if ((P(cp.x, cp.z) - q).sqrMagnitude < knotIn * knotIn) { near = true; break; } if (near) continue;
            var g = Spawn(knotRng.NextDouble() < 0.6 ? BK + "Prefabs/Trees/RedFir" + (5 + knotRng.Next(4)) : BK + "Prefabs/Trees/RedPine" + (1 + knotRng.Next(5)), climbDress); if (g == null) continue;
            g.transform.rotation = UnityEngine.Quaternion.Euler(0f, KR(0f, 360f), 0f); float tall = KR(knotLow, knotHigh);
            float sc = tall / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(sc, sc, sc); SitOn(g, q.x, q.y, 0.2f); knotN++;
        }
}

// ClimbFix.md 4.1, 4.3 and 4.4 (draft 3, Sable; counts from Rook's temporary-fir test, 2026-10-01: the bowl frames went to rock 8 to 32
// with these): trees in front of the faces, each on ground no steeper than climbTreeSlope, climbTreeClear m or more off every climb point,
// on its own random stream. Chute head: RedFir1-4 every headStep m over x 38 to 52 round P1, 8 to 12 m, tops under the deck's line to
// the knob foot (78 at x 30 to 76 at x 45) so the knob stays the one bare break. Leg 4 screen: RedFir5-8 and RedPine1-5 in two staggered
// rows at x 31.5 and 35.5, z 265 to 300, every screenStep m, 12 to 18 m. Leg 3 to leg 4 treads: two RedFir1-4. N face knot: five
// RedFir1-4 at x 22 to 36, z 305 to 313, tops under the line from BACK 196 to the crest (66 at z 305 to 69 at z 313), and two more at x
// 36 to 44. The dip's foot: one RedFir1-4 at x 22, z 305. Leg 4's west treads: a hollow log and a RedFir1-4 every leg4TreadStep m.
const float climbTreeSlope = 38f, climbTreeClear = 2.5f, headStep = 3.5f, screenStep = 4f, leg4TreadStep = 4f, sinkTree = 0.3f; int climbTreeN = 0;
{
    var ctRng = new System.Random(81640); float CR(float a, float b) => a + (float)ctRng.NextDouble() * (b - a);
    var ct = new UnityEngine.GameObject("ClimbTrees").transform; ct.SetParent(climbDress, false);
    string Small() => BK + "Prefabs/Trees/RedFir" + (1 + ctRng.Next(4));
    string Big() => ctRng.NextDouble() < 0.5 ? BK + "Prefabs/Trees/RedFir" + (5 + ctRng.Next(4)) : BK + "Prefabs/Trees/RedPine" + (1 + ctRng.Next(5));
    bool Tree(string path, float x, float z, float tall, float clear)
    {
        if (tall < 3f || data.GetSteepness((x - tOrg.x) / size.x, (z - tOrg.z) / size.z) > climbTreeSlope) return false;
        foreach (var cp in climbPts) if ((P(cp.x, cp.z) - P(x, z)).sqrMagnitude < clear * clear) return false;
        var g = Spawn(path, ct); if (g == null) return false;
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, CR(0f, 360f), 0f); float sc = tall / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(sc, sc, sc); SitOn(g, x, z, sinkTree); climbTreeN++; return true;
    }
    float KnobLine(float x) => UnityEngine.Mathf.LerpUnclamped(78f, 76f, (x - 30f) / 15f);
    for (float x = 38f; x <= 52f; x += headStep) for (float z = 205f; z <= 228f; z += headStep) { float qx = x + CR(-0.5f, 0.5f), qz = z + CR(-0.5f, 0.5f); Tree(Small(), qx, qz, UnityEngine.Mathf.Min(CR(8f, 12f), KnobLine(qx) - H(qx, qz)), climbTreeClear); }
    foreach (var rx in new[] { 31.5f, 35.5f }) for (float z = 265f + (rx > 33f ? screenStep * 0.5f : 0f); z <= 300f; z += screenStep) Tree(Big(), rx + CR(-0.5f, 0.5f), z, CR(12f, 18f), climbTreeClear);
    Tree(Small(), 39f, 273f, CR(10f, 12f), 2f); Tree(Small(), 41f, 282f, CR(10f, 12f), 2f);
    for (int i = 0, n = 0; i < 60 && n < 5; i++) { float x = CR(22f, 36f), z = CR(305f, 313f); if (Tree(Small(), x, z, (66f + (z - 305f) / 8f * 3f) - H(x, z), 2f)) n++; }
    for (int i = 0, n = 0; i < 30 && n < 2; i++) { float x = CR(36f, 44f), z = CR(305f, 313f); if (Tree(Small(), x, z, CR(10f, 12f), 2f)) n++; }
    Tree(Small(), 22f, 305f, CR(10f, 12f), 2f);
    for (float z = 266f; z <= 300f; z += leg4TreadStep)
    {
        float x = CR(21f, 24f); if (Tree(Small(), x, z, CR(6f, 10f), 2f))
        { var lg = Spawn(BK + "Prefabs/HollowLogs/RedwoodHollowLog_" + ctRng.Next(3), ct); if (lg != null) { foreach (var c in lg.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c); lg.transform.rotation = UnityEngine.Quaternion.Euler(0f, CR(0f, 360f), 0f); SitOn(lg, x + 1.5f, z + 1.5f, 0.2f); } }   // the log on a tread under a riser: looks only
    }
}

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
const float trailPaintHalf = 0.9f, tuftClear = 0.1f;   // trailPaintHalf is 8.3's trailHalf
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
const float edgeLow = 3f, edgeHigh = 5f, edgeOff = 1.3f, stoneLow = 0.2f, stoneHigh = 0.55f, stoneTilt = 20f, logShare = 0.2f, rootShare = 0.2f;
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
const float hedgeStep = 1.4f, bushLow = 2.0f, bushHigh = 2.5f, hedgeColH = 1.8f, hedgeColT = 1.0f, hedgeJitter = 0.35f, hedgeWarpNear = 6.5f, hedgeWarpPush = 1f; const int logEveryBush = 9; const float logSlopeMax = 20f;   // 8.16b: hedge logs only on ground under this slope
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
            foreach (UnityEngine.Transform w in Root("DevWarps").transform) if ((P(w.position.x, w.position.z) - q).sqrMagnitude < hedgeWarpNear * hedgeWarpNear) { q += nl * hedgeWarpPush; break; }   // 8.16a (Pim: a bush 1.1 m from the Jg warp's view): brush near a warp stands further into the closed side
            // 8.16b (Marlow, Vesper: a log lying on the bank 15 m from the pump read as a red and green striped patch): no log on a slope
            // over logSlopeMax, a bush there instead
            bool log = n % logEveryBush == 0 && data.GetSteepness((q.x - tOrg.x) / size.x, (q.y - tOrg.z) / size.z) < logSlopeMax;
            var g = Spawn(log ? BK + "Prefabs/HollowLogs/RedwoodHollowLog_" + rng.Next(3) : CS + "Vegetation/" + bushes[rng.Next(bushes.Length)], h); if (g == null) continue;
            if (log) foreach (var lc in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(lc);   // 8.16a: the hedge collider holds; a solid log at its edge was a step over the brush band (IW3)
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
const float corridorHW = 3.5f, gridCell = 0.5f, gx0 = 145f, gx1 = 345f, gz0 = 90f, gz1 = 230f, poiPad = 2.5f;   // corridorHW 3.5 (was 2.5, 8.15 gate: hedges 1 m further from the band so it takes sun)
int GW = UnityEngine.Mathf.RoundToInt((gx1 - gx0) / gridCell), GH = UnityEngine.Mathf.RoundToInt((gz1 - gz0) / gridCell);
var fld = new float[GW + 1, GH + 1];   // > 0 walkable, < 0 closed
for (int i = 0; i <= GW; i++) for (int j = 0; j <= GH; j++)
{
    var p = P(gx0 + i * gridCell, gz0 + j * gridCell);
    bool closed = ValleyShapes.InBurn(p) || Inside(p, knollHull);
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
// 8.16a (Marlow 18: the south shore was walked; Pim W5: a bush 1.1 m from the Lake_Boathouse warp's south view): the east end starts at
// the boathouse pocket's south rail post (245, 48.7), so no gap is left beside the gangway foot, and turns south 5 m clear of the warp
Hedge("Hedge_LakeSouth_East", Line(P(245.2f, 48.4f), P(248.6f, 46.4f), P(248.6f, -4f)), -1f);
// 8.16a (Marlow 18 and 19, Valley.md 9.2: floor 50 to 90 m from any trail where neither tower nor trail shows): brush along the 46 m
// line from the trails (traced from the built trails, main3_reach_check_8_16a.cs), the SE one from the lake shore hedge to the SE
// corner's west hedge, the NE one from the N foot rock band to the brush band round the closed campground
Hedge("Hedge_SE_Floor", Line(P(248.6f, 7f), P(260f, 9f), P(272f, 12f), P(284f, 15f), P(296f, 20f), P(308f, 31f), P(320f, 43f), P(332f, 54f), P(345f, 70f)), -1f);
Hedge("Hedge_NE_Floor", Line(P(266f, 307f), P(278f, 299f), P(290f, 291f), P(302f, 279f), P(314f, 271f), P(326f, 251f), P(334f, 226f), P(340.5f, 220f)), 1f);
// (c) the mid pocket (215, 222): a ring of brush round its thicket core
{ var ring = new System.Collections.Generic.List<UnityEngine.Vector2>(); for (int i = 0; i <= 32; i++) { float a = i * UnityEngine.Mathf.PI * 2f / 32f; ring.Add(P(215f + UnityEngine.Mathf.Cos(a) * 12f, 222f + UnityEngine.Mathf.Sin(a) * 12f)); } Hedge("Hedge_MidPocket", ring, 1f); }
// 8.16a (main3_reach_check_8_16a.cs: the floor over 46 m from the built trails lies west of the ring, round (201, 212)): a second ring,
// an ellipse over that floor
{ const float mwX = 201f, mwZ = 212f, mwRX = 17f, mwRZ = 10f; var ring = new System.Collections.Generic.List<UnityEngine.Vector2>(); for (int i = 0; i <= 32; i++) { float a = i * UnityEngine.Mathf.PI * 2f / 32f; ring.Add(P(mwX + UnityEngine.Mathf.Cos(a) * mwRX, mwZ + UnityEngine.Mathf.Sin(a) * mwRZ)); } Hedge("Hedge_MidPocket_W", ring, 1f); }
// (d) 8.6's gray brush bands round the closed campground: owned brush over them, the gray boxes stop drawing (their colliders stay)
int bandsDressed = 0; const float bandWarpClear = 4f;
var fzBrush = Root("FrontZone").transform.Find("BrushBands");
if (fzBrush != null) foreach (UnityEngine.Transform b in fzBrush)
{
    var r = b.GetComponent<UnityEngine.Renderer>(); if (r != null) r.enabled = false;
    var bb = b.GetComponent<UnityEngine.Collider>().bounds; var band = new UnityEngine.GameObject("Dress_" + b.name).transform; band.SetParent(stops, false);
    for (float x = bb.min.x + 0.7f; x <= bb.max.x - 0.3f; x += hedgeStep) for (float z = bb.min.z + 0.7f; z <= bb.max.z - 0.3f; z += hedgeStep)
    {
        bool byWarp = false; foreach (UnityEngine.Transform w in Root("DevWarps").transform) if ((P(w.position.x, w.position.z) - P(x, z)).sqrMagnitude < bandWarpClear * bandWarpClear) byWarp = true; if (byWarp) continue;   // 8.16a (the Office warp's west view met a band bush at 1.5 m)
        var g = Spawn(CS + "Vegetation/" + bushes[rng.Next(bushes.Length)], band); if (g == null) continue;
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); float s = R(bushLow, bushHigh) / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(s, s, s);
        SitOn(g, x + R(-0.3f, 0.3f), z + R(-0.3f, 0.3f), 0.1f); bushN++;
    }
    bandsDressed++;
}
// hedge cover (8.15 gate, Pim W2: 41 of 334 hedge boxes under 80 percent under brush): each box's top face is sampled every
// coverSample m; where no bush or log of its hedge stands over a sample, a bush is added there, until the box is covered
const float coverSample = 0.5f, coverSlack = 0.5f; int coverAdded = 0;
foreach (UnityEngine.Transform hedge in stops)
{
    if (!hedge.name.StartsWith("Hedge")) continue;
    var brush = new System.Collections.Generic.List<UnityEngine.Renderer>(); foreach (var r in hedge.GetComponentsInChildren<UnityEngine.Renderer>()) brush.Add(r);
    foreach (var bc in hedge.GetComponentsInChildren<UnityEngine.BoxCollider>())
    {
        var tr = bc.transform;
        for (float lx = -bc.size.x * 0.5f; lx <= bc.size.x * 0.5f; lx += coverSample) for (float lz = -bc.size.z * 0.5f; lz <= bc.size.z * 0.5f; lz += coverSample)
        {
            var w = tr.TransformPoint(bc.center + V(lx, bc.size.y * 0.5f, lz)); bool covered = false;
            foreach (var r in brush) { var b = r.bounds; if (w.x >= b.min.x && w.x <= b.max.x && w.z >= b.min.z && w.z <= b.max.z && b.max.y >= w.y - coverSlack) { covered = true; break; } }
            if (covered) continue;
            var g = Spawn(CS + "Vegetation/" + bushes[rng.Next(bushes.Length)], hedge); if (g == null) continue;
            g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); float s = R(bushLow, bushHigh) / UnityEngine.Mathf.Max(0.2f, Top(g) - Bottom(g)); g.transform.localScale = V(s, s, s);
            SitOn(g, w.x, w.z, 0.1f); float short0 = w.y - Top(g); if (short0 > 0f) g.transform.position += V(0f, short0, 0f);   // on a slope the bush stands on the high side, its top at the box's
            foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) brush.Add(r); coverAdded++; bushN++;
        }
    }
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
// ClimbFix 4.2 b (Sable draft 3): along the cwm cut's east edge (x cutRimX, z cutRimZ0 to cutRimZ1), wherever the land east of it
// (at cutRimEastX) stands within cutRimNear m of the cut floor, a rim on the cut side, so nothing walks between the cut and the valley
// there (4.2 a and c are faces)
const float cutRimX = 65.4f, cutRimZ0 = 276f, cutRimZ1 = 297f, cutRimNear = 4f, cutRimEastX = 67.5f, cutRimStep = 0.5f; int cutRimRuns = 0;
{
    var pts = new System.Collections.Generic.List<UnityEngine.Vector2>();
    for (float z = cutRimZ0; z <= cutRimZ1 + 0.01f; z += cutRimStep)
    {
        bool near = UnityEngine.Mathf.Abs(H(cutRimX, z) - H(cutRimEastX, z)) < cutRimNear; if (near) pts.Add(P(cutRimX, z));
        if ((!near || z + cutRimStep > cutRimZ1 + 0.01f) && pts.Count > 1) { Rim(pts.ToArray(), 1f); cutRimRuns++; }
        if (!near) pts.Clear();
    }
}
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
// (f) 8.16a (Marlow, Gate_816_Marlow.md 3; Vesper, banks D): the pump trench and the Camp 3 hollow walls are terrain banks and faces
// 3 to 8 m high, up to 85 degrees, and the rock layer stretches up them in streaks, so they read as poured walls. A near-vertical
// face is thin in plan, so the banks are found by rays: from every faceEvery-th point of the trench stretch of Camp to pump and of the
// gorge end of W1 to Camp 3, level to both sides, and from the Camp 3 hollow's centre all round, at faceHeights over the ground, up to
// faceReach m. Where a ray meets terrain steeper than faceMin, an owned BK boulder (faceRockLow to faceRockHigh m) sits on the hit,
// faceShow of its size out of the bank along its normal, faceSpace m from the last. Solid only where the ground is walkable (under
// faceHold, the slope limit), or the player would walk into it; on a face it stays looks only (a collider would be a foothold out).
// 8.16b (Marlow 5, Vesper: the boulders covered the foot, the smooth streaked wall still filled the upper frame): rays to faceHeights up to
// 16.5 m and faceReach 14 m, bigger rocks, each squashed or stretched a little in plan (faceStretch) so no two read alike
const float faceMin = 35f, faceHold = 45f, faceSpace = 2.5f, faceReach = 14f, faceShow = 0.4f, faceRockLow = 3.5f, faceRockHigh = 7f, hollowRays = 48f, faceStretch = 0.25f, faceClimbNear = 25f;
var faceHeights = new[] { 0.6f, 2.5f, 4.5f, 6.5f, 8.5f, 10.5f, 12.5f, 14.5f, 16.5f }; const int faceEvery = 1;
// 8.17 gate (Marlow 3, 2026-10-01: the trench's smooth wall still filled Camp to pump FWD 40 and BACK 40 above the rocks): on Camp to pump
// the rays go on up to trenchFaceTop m every 2 m, and reach trenchFaceReach m
const float trenchFaceTop = 26.5f, trenchFaceReach = 20f; var trenchHeights = new System.Collections.Generic.List<float>(faceHeights); for (float h = faceHeights[faceHeights.Length - 1] + 2f; h <= trenchFaceTop; h += 2f) trenchHeights.Add(h);
// 8.16b (Marlow 6, Vesper: the cwm head wall, leg 4 coming down and the cleft read as a streaked curtain): the J to Ward stretch from the
// cwm to the cleft too; there the rocks are looks only (no foothold on the climb, whatever the slope)
var faceLegs = new (string leg, int from, int to)[] { ("Camp to pump", 10, 48), ("W1 to Camp 3", 30, 46), ("J to Ward", -1, 127) }; var hollowC = V(78f, 0f, 146f);   // ClimbFix 4.4 (Sable draft 3): on the climb only from P4 to the ledge (from -1: the point nearest P4); the legs take trees
string[] faceRocks = { BK + "Prefabs/Rocks/BigBoulders_0", BK + "Prefabs/Rocks/BigBoulders_1", BK + "Prefabs/Rocks/BigBoulders_2", BK + "Prefabs/Rocks/BigBoulders_3", BK + "Prefabs/Rocks/BigBoulders_4", BK + "Prefabs/Rocks/BigBoulders_5", BK + "Prefabs/Rocks/Boulder_0", BK + "Prefabs/Rocks/Boulder_1", BK + "Prefabs/Rocks/Boulder_2" };
var faceRoot = new UnityEngine.GameObject("FaceRock").transform; faceRoot.SetParent(stops, false); int faceN = 0;
{
    UnityEngine.Physics.SyncTransforms();   // the physics scene holds the terrain as this recipe left it
    var rays = new System.Collections.Generic.List<(UnityEngine.Vector3 o, UnityEngine.Vector3 d, float r)>();
    foreach (var fl in faceLegs)
    {
        var lg = legs.Find(q => q.name == fl.leg); if (lg.pts == null) return "no trail " + fl.leg;
        int from = fl.from; if (from < 0) { float bd = float.MaxValue; for (int j = 0; j < lg.pts.Count; j++) { float dj = (P(lg.pts[j].x, lg.pts[j].z) - P(26f, 262f)).sqrMagnitude; if (dj < bd) { bd = dj; from = j; } } }
        for (int i = UnityEngine.Mathf.Max(1, from); i < UnityEngine.Mathf.Min(fl.to, lg.pts.Count); i += faceEvery)
        {
            var a = lg.pts[i - 1]; var b = lg.pts[i]; var t = V(b.x - a.x, 0f, b.z - a.z).normalized; var side = V(t.z, 0f, -t.x);
            foreach (var h in fl.leg == "Camp to pump" ? trenchHeights.ToArray() : faceHeights) { { float rr = fl.leg == "Camp to pump" ? trenchFaceReach : faceReach; rays.Add((b + UnityEngine.Vector3.up * h, side, rr)); rays.Add((b + UnityEngine.Vector3.up * h, -side, rr)); } }
        }
    }
    var hc = V(hollowC.x, H(hollowC.x, hollowC.z), hollowC.z);
    for (int k = 0; k < hollowRays; k++) { var d = UnityEngine.Quaternion.Euler(0f, k * 360f / hollowRays, 0f) * UnityEngine.Vector3.forward; foreach (var h in faceHeights) rays.Add((hc + UnityEngine.Vector3.up * h, d, faceReach)); }
    var placedF = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (var ray in rays)
    {
        if (!UnityEngine.Physics.Raycast(ray.o, ray.d, out var hit, ray.r, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore) || !(hit.collider is UnityEngine.TerrainCollider)) continue;
        if (UnityEngine.Vector3.Angle(hit.normal, UnityEngine.Vector3.up) < faceMin) continue;
        bool skip = false; foreach (var q in placedF) if ((q - hit.point).sqrMagnitude < faceSpace * faceSpace) { skip = true; break; } if (skip) continue;
        var g = Spawn(faceRocks[rng.Next(faceRocks.Length)], faceRoot); if (g == null) break;
        g.transform.rotation = UnityEngine.Quaternion.Euler(R(-20f, 20f), R(0f, 360f), R(-20f, 20f)); g.transform.position = UnityEngine.Vector3.zero; g.transform.localScale = UnityEngine.Vector3.one;
        var b0 = new UnityEngine.Bounds(); bool first = true; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) { if (first) { b0 = r.bounds; first = false; } else b0.Encapsulate(r.bounds); }
        float sc = R(faceRockLow, faceRockHigh) / UnityEngine.Mathf.Max(0.2f, UnityEngine.Mathf.Max(b0.size.x, UnityEngine.Mathf.Max(b0.size.y, b0.size.z))); g.transform.localScale = V(sc * R(1f - faceStretch, 1f + faceStretch), sc * R(1f - faceStretch, 1f + faceStretch), sc);
        float half = UnityEngine.Mathf.Max(b0.size.x, b0.size.z) * sc * 0.5f;
        g.transform.position = hit.point - hit.normal * (half - 2f * half * faceShow) - (b0.center * sc);
        bool onClimb = false; var climbLeg = legs.Find(q => q.name == "J to Ward"); if (climbLeg.pts != null) foreach (var cp in climbLeg.pts) if ((cp - hit.point).sqrMagnitude < faceClimbNear * faceClimbNear) { onClimb = true; break; }
        if (!onClimb && UnityEngine.Vector3.Angle(hit.normal, UnityEngine.Vector3.up) < faceHold) { var mf0 = g.GetComponentInChildren<UnityEngine.MeshFilter>(); if (mf0 != null && mf0.sharedMesh != null) { var mc = mf0.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf0.sharedMesh; mc.convex = true; } }
        placedF.Add(hit.point); faceN++;
    }
}

// ClimbFix 4.6 (Sable draft 3; Vesper: the curtain, pillar and hanging boulders at BACK 256 and 246). Named by rays from those frames
// (Rook, 2026-10-01): the curtain is the steep terrain of the shoulder and N end faces (18 to 23 percent of each frame), the pillar the
// Rock/Ledge/Fin box (11 to 29 percent), the hanging boulders this recipe's face rocks over the ledge. Any face rock west of
// ledgeRockX1 between ledgeRockZ0 and ledgeRockZ1 whose bottom stands more than faceAirMax m over the ground at its centre goes. The
// curtain: from the BACK 256 and 246 eyes (the path end and curtainBack m back along the climb, looking back down it), rays over a
// curtainRaysX by curtainRaysY grid of the frame; on every steep (over curtainSteep) terrain hit within curtainReach m, CS_Rock pieces
// curtainLow to curtainHigh m, curtainSpace m apart, each sunk until its bottom is curtainSink m under the lowest ground under it (no air),
// looks only.
const float ledgeRockX1 = 30f, ledgeRockZ0 = 230f, ledgeRockZ1 = 300f, faceAirMax = 0.1f, curtainBack = 10f, curtainSteep = 50f, curtainReach = 40f, curtainLow = 3f, curtainHigh = 6f, curtainSpace = 2.5f, curtainSink = 0.3f, curtainPathKeep = 2f, curtainFovY = 60f, curtainAspect = 1920f / 988f;
const int curtainRaysX = 32, curtainRaysY = 16; int faceAirGone = 0, curtainN = 0;
{
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(faceRoot)))
    {
        var b = PlaceKit.MeshBounds(t.gameObject); if (b.center.x > ledgeRockX1 || b.center.z < ledgeRockZ0 || b.center.z > ledgeRockZ1) continue;
        if (b.min.y - H(b.center.x, b.center.z) > faceAirMax) { UnityEngine.Object.DestroyImmediate(t.gameObject); faceAirGone++; }
    }
    float len = climbS[climbS.Length - 1]; var curtainRoot = new UnityEngine.GameObject("Curtain").transform; curtainRoot.SetParent(faceRoot, false); var placedC = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (var back in new[] { 0f, curtainBack })
    {
        var e = ClimbAt(len - back, out var tanE); var eye = e + UnityEngine.Vector3.up * 1.6f; var ahead = ClimbAt(len - back - 3f, out _); var fwd = V(ahead.x - e.x, 0f, ahead.z - e.z).normalized; var right = V(fwd.z, 0f, -fwd.x);
        float tanY = UnityEngine.Mathf.Tan(curtainFovY * 0.5f * UnityEngine.Mathf.Deg2Rad), tanX = tanY * curtainAspect;
        for (int gy = 0; gy < curtainRaysY; gy++) for (int gx = 0; gx < curtainRaysX; gx++)
        {
            float u = ((gx + 0.5f) / curtainRaysX * 2f - 1f) * tanX, v = ((gy + 0.5f) / curtainRaysY * 2f - 1f) * tanY; var dir = (fwd + right * u + UnityEngine.Vector3.up * v).normalized;
            if (!UnityEngine.Physics.Raycast(eye, dir, out var hit, curtainReach, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore) || !(hit.collider is UnityEngine.TerrainCollider)) continue;
            if (UnityEngine.Vector3.Angle(hit.normal, UnityEngine.Vector3.up) < curtainSteep) continue;
            bool near = false; foreach (var q in placedC) if ((q - hit.point).sqrMagnitude < curtainSpace * curtainSpace) { near = true; break; } if (near) continue;
            var g = Spawn(CS + "Rocks and Stones/CS_Rock_" + (1 + rng.Next(8)), curtainRoot); if (g == null) break; PlaceKit.StripColliders(g);
            g.transform.rotation = UnityEngine.Quaternion.Euler(R(-10f, 10f), R(0f, 360f), R(-10f, 10f)); g.transform.position = UnityEngine.Vector3.zero; g.transform.localScale = UnityEngine.Vector3.one;
            var b0 = PlaceKit.MeshBounds(g); float sc = R(curtainLow, curtainHigh) / UnityEngine.Mathf.Max(0.2f, UnityEngine.Mathf.Max(b0.size.x, UnityEngine.Mathf.Max(b0.size.y, b0.size.z))); g.transform.localScale = V(sc, sc, sc);
            var b1 = PlaceKit.MeshBounds(g); g.transform.position += hit.point - b1.center;
            var b2 = PlaceKit.MeshBounds(g); float low = float.MaxValue; foreach (var cx in new[] { b2.min.x, b2.center.x, b2.max.x }) foreach (var cz in new[] { b2.min.z, b2.center.z, b2.max.z }) low = UnityEngine.Mathf.Min(low, H(cx, cz));
            g.transform.position += V(0f, low - curtainSink - b2.min.y, 0f);
            var b3 = PlaceKit.MeshBounds(g); bool onPath = false; foreach (var cp in climbPts) { float dx = UnityEngine.Mathf.Max(b3.min.x - cp.x, 0f, cp.x - b3.max.x), dz = UnityEngine.Mathf.Max(b3.min.z - cp.z, 0f, cp.z - b3.max.z); if (dx * dx + dz * dz < curtainPathKeep * curtainPathKeep) { onPath = true; break; } }
            if (onPath) { UnityEngine.Object.DestroyImmediate(g); continue; }   // sunk onto the tread or the ledge: none there
            placedC.Add(hit.point); curtainN++;
        }
    }
}

// (g) 8.16b (Vesper: the cleft's stacked box slabs, the box pillar): 8.1's ledge boxes (Rock/Ledge: lips, end walls, back walls, fin)
// take the owned BK rock texture tiled about once per ledgeTile m on their faces (one material per size step, in Assets/Materials/Places);
// the fin cap pillar stops drawing behind owned boulders stacked to its height (its collider stays: the fin hides the slot, F-1)
const float ledgeTile = 4f, capStack = 2.4f, finCell = 3f, finOverlap = 1f;   // finOverlap 1: each boulder fits its cell, so none passes the envelope
int ledgeRetex = 0, capRocks = 0, finRocks = 0;
{
    var kit = new PlaceKit(scene); UnityEngine.GameObject rockR = null; foreach (var r0 in scene.GetRootGameObjects()) if (r0.name == "Rock") rockR = r0;
    var ledge = rockR != null ? rockR.transform.Find("Ledge") : null;
    int StepT(float m) { foreach (var s in new[] { 1, 2, 3, 4, 6, 8, 12, 16 }) if (m / ledgeTile <= s * 1.3f) return s; return 16; }
    if (ledge != null) foreach (UnityEngine.Transform t in ledge)
    {
        var r = t.GetComponent<UnityEngine.MeshRenderer>(); if (r == null) continue;
        // ClimbFix 4.6 (Sable draft 3): the fin is the box pillar BACK 256 and 246 see; it stops drawing (its collider stays for F-1) and
        // owned boulders fill its envelope, one per finCell m cell, each held inside the box (footprint and top unchanged)
        if (t.name == "Fin")
        {
            r.enabled = false; var b = r.bounds;
            for (float y = b.min.y; y < b.max.y - 0.5f; y += finCell) for (float x = b.min.x; x < b.max.x - 0.2f; x += finCell) for (float z = b.min.z; z < b.max.z - 0.2f; z += finCell)
            {
                var cell = new UnityEngine.Bounds(); cell.SetMinMax(V(x, y, z), V(UnityEngine.Mathf.Min(x + finCell, b.max.x), UnityEngine.Mathf.Min(y + finCell, b.max.y), UnityEngine.Mathf.Min(z + finCell, b.max.z)));
                var g = Spawn(BK + "Prefabs/Rocks/" + (finRocks % 2 == 0 ? "Boulder_" + (finRocks % 6) : "BigBoulders_" + (finRocks % 6)), faceRoot); if (g == null) break; PlaceKit.StripColliders(g);
                g.transform.rotation = UnityEngine.Quaternion.Euler(R(-20f, 20f), R(0f, 360f), R(-20f, 20f)); g.transform.position = UnityEngine.Vector3.zero; g.transform.localScale = UnityEngine.Vector3.one;
                var gb = PlaceKit.MeshBounds(g); float sc = finOverlap * UnityEngine.Mathf.Min(cell.size.x / gb.size.x, UnityEngine.Mathf.Min(cell.size.y / gb.size.y, cell.size.z / gb.size.z)); g.transform.localScale = V(sc, sc, sc);
                g.transform.position += cell.center - PlaceKit.MeshBounds(g).center; finRocks++;
            }
            continue;
        }
        if (t.name == "FinCap")
        {
            r.enabled = false; var b = r.bounds;
            for (float y = b.min.y; y < b.max.y; y += capStack)
            {
                var g = Spawn(BK + "Prefabs/Rocks/Boulder_" + (capRocks % 6), faceRoot); if (g == null) break; PlaceKit.StripColliders(g);
                g.transform.rotation = UnityEngine.Quaternion.Euler(R(-15f, 15f), R(0f, 360f), R(-15f, 15f)); float sc = UnityEngine.Mathf.Max(b.size.x, b.size.z) * 1.3f / 4f; g.transform.localScale = V(sc, sc * 0.8f, sc);
                g.transform.position += V(b.center.x, y + capStack * 0.5f, b.center.z) - PlaceKit.MeshBounds(g).center; capRocks++;
            }
            continue;
        }
        var d = new[] { t.lossyScale.x, t.lossyScale.y, t.lossyScale.z }; System.Array.Sort(d); int u = StepT(d[1]), v = StepT(d[2]);
        r.sharedMaterial = kit.Tinted("Places_LedgeRock_" + u + "x" + v, "Assets/BK/PureNature_Redwood/Models/Rocks/Textures/Materials/Rocks.mat", new UnityEngine.Color(0.55f, 0.53f, 0.5f), new UnityEngine.Vector2(u, v)); ledgeRetex++;
    }
}

// RebuildSpecs 2 and Style 4.7 (no bare flat dirt in a dressed shot): grass tufts and stones on both verges of the highway beside the
// lot, every vergeStep m from z vergeZ0 to vergeZ1, vergeIn to vergeOut m off the road's centre line, vergePieces each time
const float vergeRoadX = 428f, vergeIn = 5.5f, vergeOut = 15f, vergeStep = 3f, vergeZ0 = 110f, vergeZ1 = 260f; const int vergePieces = 2; int vergeN = 0;
{
    var verge = new UnityEngine.GameObject("RoadVerge").transform; verge.SetParent(root, false);
    for (float z = vergeZ0; z <= vergeZ1; z += vergeStep) foreach (var side in new[] { -1f, 1f }) for (int i = 0; i < vergePieces; i++)
    {
        float x = vergeRoadX + side * R(vergeIn, vergeOut), zz = z + R(-vergeStep * 0.5f, vergeStep * 0.5f); if (x > tOrg.x + size.x - 1f) continue;
        bool stone = rng.NextDouble() < 0.35; var g = Spawn(stone ? CS + "Rocks and Stones/CS_Stone_" + (1 + rng.Next(8)) : BK + "Prefabs/Plants/Grass" + (1 + rng.Next(3)), verge); if (g == null) continue;
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); float s = stone ? R(0.4f, 0.9f) : R(0.8f, 1.4f); g.transform.localScale = V(s, s, s); SitOn(g, x, zz, stone ? 0.08f : 0.02f); vergeN++;
    }
}

// 8.15 gate (Pim: foliage at arm's length at some warps): dressing firs, pines, branches and bushes (not the hedges over their
// colliders, which would leave an invisible wall) whose bounds come within warpFoliage m of a warp are removed
const float warpFoliage = 2.5f; int warpCleared = 0;
{
    var doomed = new System.Collections.Generic.HashSet<UnityEngine.GameObject>();
    var roots = new System.Collections.Generic.List<UnityEngine.Transform> { root.transform }; var slice = Root("SliceLook"); if (slice != null) roots.Add(slice.transform);
    foreach (var rt in roots) foreach (var r in rt.GetComponentsInChildren<UnityEngine.Renderer>(true))
    {
        var n = r.name; if (!(n.StartsWith("CS_Bush") || n.StartsWith("RedFir") || n.StartsWith("RedPine") || n.StartsWith("Branchs"))) continue;
        var top = r.transform; while (top.parent != null && top.parent != rt && top.parent.parent != rt && !top.parent.name.StartsWith("Hedge")) top = top.parent;
        if (top.parent != null && top.parent.name.StartsWith("Hedge")) continue;   // hedge brush stays over its collider
        foreach (UnityEngine.Transform w in Root("DevWarps").transform)
        {
            var b = r.bounds; float dx = UnityEngine.Mathf.Max(b.min.x - w.position.x, 0f, w.position.x - b.max.x), dz = UnityEngine.Mathf.Max(b.min.z - w.position.z, 0f, w.position.z - b.max.z);
            if (dx * dx + dz * dz < warpFoliage * warpFoliage) { doomed.Add(top.gameObject); break; }
        }
    }
    foreach (var g in doomed) { UnityEngine.Object.DestroyImmediate(g); warpCleared++; }
}
// 8.16a (Pim W5 and later captures: hedge brush 1.0 to 1.4 m from a warp's view, its place shifting with every upstream change):
// brush over a hedge or brush band that comes within warpViewClear of a point 1 m ahead of a warp's eye (N, E, S, W; the capture's
// test point) shrinks about its base by warpShrink steps, down to warpShrinkMin of its size, then steps away from the point
const float warpViewClear = 1.8f, warpShrink = 0.85f, warpShrinkMin = 0.5f, warpStepAway = 0.25f; const int warpStepsMax = 8; int warpShrunk = 0;
{
    var views = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (UnityEngine.Transform w in Root("DevWarps").transform) { var e = V(w.position.x, H(w.position.x, w.position.z) + 1.6f, w.position.z); foreach (var yaw in new[] { 0f, 90f, 180f, 270f }) views.Add(e + UnityEngine.Quaternion.Euler(0f, yaw, 0f) * UnityEngine.Vector3.forward); }
    float Near(UnityEngine.GameObject g) { float best = float.MaxValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) foreach (var v in views) best = UnityEngine.Mathf.Min(best, UnityEngine.Mathf.Sqrt(r.bounds.SqrDistance(v))); return best; }
    foreach (UnityEngine.Transform grp in stops)
    {
        if (!(grp.name.StartsWith("Hedge") || grp.name.StartsWith("Dress_"))) continue;
        foreach (UnityEngine.Transform bush in grp)
        {
            if (!bush.name.StartsWith("CS_Bush") || Near(bush.gameObject) >= warpViewClear) continue;
            float s0 = bush.localScale.x; var at = bush.position; float b0 = Bottom(bush.gameObject);
            while (bush.localScale.x > s0 * warpShrinkMin && Near(bush.gameObject) < warpViewClear) { bush.localScale *= warpShrink; bush.position += V(0f, b0 - Bottom(bush.gameObject), 0f); }
            for (int k = 0; k < warpStepsMax && Near(bush.gameObject) < warpViewClear; k++)
            {
                var c = bush.position; UnityEngine.Vector3 nearest = views[0]; foreach (var v in views) if ((v - c).sqrMagnitude < (nearest - c).sqrMagnitude) nearest = v;
                var away = V(c.x - nearest.x, 0f, c.z - nearest.z).normalized; bush.position += away * warpStepAway; SitOn(bush.gameObject, bush.position.x, bush.position.z, 0.1f);
            }
            warpShrunk++;
        }
    }
}

foreach (var tl in data.terrainLayers) if (tl == null || tl.diffuseTexture == null) missing.Add("texture on terrain layer " + (tl != null ? tl.name : "(none)"));   // 8.16 gate: a layer without its texture draws a grey checker
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | ledge boxes textured " + ledgeRetex + ", fin cap rocks " + capRocks + " | layers: floor GrassPine, SoilPine added, shore and burn GrassMud, trail Ground054, rock Rocks_a | cover " + detailNames.Length + " detail kinds | trail edges " + edgeN
    + " | markers " + markers.childCount + " | climb fir knots " + knotN + ", ClimbFix 4 trees " + climbTreeN + " | brush eased off warp views " + warpShrunk + " | face rocks " + faceN + " (" + faceAirGone + " hanging over the ledge gone), curtain rocks " + curtainN + ", fin rocks " + finRocks + " | hedges: " + loopN + " traced round the burn and knoll, " + hedgeCols + " collider pieces, " + hedgeLen.ToString("F0") + " m, " + bushN + " brush and logs, " + bandsDressed + " front bands dressed, " + shoreBush + " shore bushes, " + oliveN + " bush renderers olive, rock on " + rockCells + " slope cells, " + warpCleared + " plants cleared at warps, " + coverAdded + " bushes added over bare hedge boxes | rims " + rimSegs + " pieces (cwm cut edge runs " + cutRimRuns + ") | lot soil SoilPine " + lotTile + " m, verge tufts and stones " + vergeN + " | missing: " + (missing.Count == 0 ? "none" : string.Join(", ", missing));
