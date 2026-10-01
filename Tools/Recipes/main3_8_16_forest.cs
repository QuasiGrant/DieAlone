// Main3 task 8.16: the forest (Valley.md rev 10 section 10; Docs/Design/ForestPlan.md sections 2 to 5 and 7; owned BK trees only in the
// forest). Run after 8.15 in Main3, edit mode (the runner runs it before the day-one start and the sightlines). Everything goes under a
// "Forest" root with no colliders: trees are looks only, the land is the only cover (F-1). Every tree keeps treeTrailGap m off every trail
// centre point, out of the named clearings, the lake, the front zone and the fence line, off slopes over floorSlope (crests crestSlope),
// and clear of any collider (buildings, stops, rock).
// 1. Groves (section 10 table): giants 8 to 15 m apart, tops at most giantCap absolute (Style 5.8), firs and pines 8 to 20 m under
//    them, and at each grove's foot 8 bushes, 12 ferns, 1 or 2 hollow logs, 2 leaf patches and branches.
// 2. The north fir wall, x 90 to 300, z 285 to 310: 150 firs 8 to 24 m, ragged, front edge wandering, 5 giants breaking the top line,
//    saplings and bushes in front, deadfall at the foot.
// 3. Crests (looks only): the W belt and hooks (x 5 to 30, ground 70 or more) in knots, 250 firs 18 to 24 m and 12 giants in 4 knots;
//    the knob, the ledge, the cleft and the climb stay bare; the N line (z 320 to 350) in 5 runs with a gap over x 120 to 170; the S line
//    (z -30 to -50), 40 firs, bare over the S saddle.
// 4. The open east: 2 knots of 3 giants and 10 fir clumps of about 20, clear of the deck's lines to the verge tree, the office door and
//    the T, and of the stack-to-T line. 5. Beyond the highway: about 200 firs and pines 20 to 35 m, 100 to 400 m out.
// 6. The old burn: 20 more snags and 10 fallen trunks.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Ground815") == null) return "run 8.15 first";
if (Root("Forest") != null) return "Forest already exists; rebuild Main3 from 8.1";
// 8.16a: in a runner job the physics scene does not hold the colliders of the scene as loaded until it is synced; without this every
// collider test below (Free's clearance, NearLowStop) saw only what this recipe had made itself
UnityEngine.Physics.SyncTransforms();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>(); var data = terrain.terrainData; var tOrg = terrain.transform.position; var size = data.size;
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + tOrg.y;
bool OnTerrain(float x, float z) => x > tOrg.x + 1f && x < tOrg.x + size.x - 1f && z > tOrg.z + 1f && z < tOrg.z + size.z - 1f;
float Slope(float x, float z) => data.GetSteepness((x - tOrg.x) / size.x, (z - tOrg.z) / size.z);
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-4f)); return UnityEngine.Vector2.Distance(p, a + ab * t); }
bool Inside(UnityEngine.Vector2 p, UnityEngine.Vector2[] poly)
{
    bool c = false;
    for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
        if (((poly[i].y > p.y) != (poly[j].y > p.y)) && (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) c = !c;
    return c;
}
var rng = new System.Random(8161);
float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
UnityEngine.Vector2 InDisk(UnityEngine.Vector2 c, float r) { float a = R(0f, 2f * UnityEngine.Mathf.PI), d = r * UnityEngine.Mathf.Sqrt((float)rng.NextDouble()); return c + P(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * d; }
var missing = new System.Collections.Generic.List<string>();
const string BK = "Assets/BK/PureNature_Redwood/Prefabs/", SUF = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
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
    // looks only, except (8.16a, Marlow: players walked through trunks) BK trees and hollow logs: collision from
    // the trunk capsule of TrunkCollider above and the pack log mesh, never on foliage
    if (IsPackTree(path)) TrunkCollider(g); else if (!IsPackLog(path)) foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    return g;
}
float Bottom(UnityEngine.GameObject g) { float low = float.MaxValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) low = UnityEngine.Mathf.Min(low, r.bounds.min.y); return low; }
float Top(UnityEngine.GameObject g) { float hi = float.MinValue; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) hi = UnityEngine.Mathf.Max(hi, r.bounds.max.y); return hi; }

// ---- where a tree may stand
const float treeTrailGap = 3.5f, floorSlope = 38f, crestSlope = 50f, colliderClear = 1.2f, lakeKeep = 1.15f, giantCap = 50f;
const float lakeX = 190f, lakeZ = 60f, lakeA = 54.8f, lakeB = 27.6f, fenceX = 396f;
// the planting numbers (ForestPlan sections 2 to 5; Valley.md 10)
const float treeSink = 0.3f, giantSpace = 8f, firSpace = 3f, firUnderLow = 3f, firUnderHigh = 9f;
const int footTries = 20, footBush = 8, footFern = 12, footLeaves = 2, footBranch = 2; const float footTrailGap = 1.6f;
const int groveTries = 400, groveFirCap = 10; const float groveCountPad = 6f;   // groveFirCap 10: ForestPlan 7 thinning step 5 (Camp 1% low 58 to 68 with the full counts)
const float c1RingIn = 29f, c1RingOut = 40f, knollIn = 23f, knollOut = 30f, knollTall = 35f;
const float wallX0 = 90f, wallX1 = 300f, wallZ0 = 285f, wallZ1 = 310f, wallWander = 5f, wallFootStep = 12f; const int wallFirs = 110, wallGiants = 5, wallSaplings = 30;   // wallFirs 110 (was 150): ForestPlan 7 thinning step 3 (the back half thinner, by chance of placement)
// 8.16b (Vesper: the W crest read as one single-file row of even pines): every belt fir stands in a knot (clumps of 3 to 7, beltKnotR, mixed
// heights beltLow to beltHigh), and the knots reach down the slope to beltGround, so they stand in depth, not along the crest line
const float beltX0 = 5f, beltX1 = 30f, beltZ0 = -40f, beltZ1 = 350f, beltGround = 64f, beltGapZ0 = 190f, beltGapZ1 = 300f, beltKnotR = 5f, beltKnotShare = 1f, beltLow = 12f, beltHigh = 26f;
const int beltKnots = 40, beltFirs = 180, beltGiants = 12, beltGiantKnots = 4;   // beltFirs 180 (was 250): thinning step 4
// 8.16b (Vesper: the north bench held singles): each north run is nClumps clumps of about nFirsPerRun / nClumps firs within nClumpR m
var nRuns = new[] { P(40f, 110f), P(175f, 230f), P(240f, 290f), P(300f, 350f), P(355f, 392f) }; const float nZ0 = 320f, nZ1 = 350f, nClumpR = 6f; const int nFirsPerRun = 20, nClumps = 4;   // 20 (was 30): ForestPlan 7 thinning step 2
const float sX0 = 40f, sX1 = 390f, sZ0 = -30f, sZ1 = -50f, sSaddleX = 170f, sSaddleHalf = 20f, sSpace = 6f; const int sFirs = 40;
const float eastX0 = 230f, eastX1 = 390f, eastZ0 = 60f, eastZ1 = 300f, clumpR = 9f, clumpLineKeep = 10f; const int eastClumps = 10, clumpFirs = 12, clumpTries = 400;   // clumpFirs 12 (was 20): ForestPlan 7 thinning step 1, S1 1% low 57.8 with 20
int eastClumpsN = 0;
// 8.16b (Vesper: one even band of one height past the road): two bands, a near one beyondNear to beyondMid m out at nearLow to nearHigh m,
// and a back one beyondBack to beyondFar m out, taller (backLow to backHigh), so it rises behind and pales with the distance
const float roadX = 428f, beyondNear = 100f, beyondMid = 190f, beyondBack = 260f, beyondFar = 420f, nearLow = 18f, nearHigh = 28f, backLow = 30f, backHigh = 45f, beyondZ0 = -150f, beyondZ1 = 450f, beyondRayTop = 300f, beyondSpace = 7f; const int beyondTrees = 200, beyondNearShare = 55;
const float snagSpace = 6f, snagGirth = 1.5f; const int burnSnags = 20, burnFallen = 10;
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform leg in Root("Trails").transform) foreach (UnityEngine.Transform p in leg) trailPts.Add(P(p.position.x, p.position.z));
var clearings = new (UnityEngine.Vector2 c, float r)[] { (P(170f, 160f), 22f), (P(282f, 238f), 28f), (P(292f, 108f), 18f), (P(78f, 146f), 14f) };   // camp, Camp 1, Camp 2, Camp 3
var frontZone = new UnityEngine.Rect(334f, 145f, fenceX - 334f, 70f);   // the lot, office, store and booth
// 8.16a (the runner's sightlines, now that trunks have colliders): no tree on the next-destination lines 8.9 checks (Pump to the Snag)
var sightKeepLines = new (UnityEngine.Vector2 a, UnityEngine.Vector2 b)[] { (P(190f, 97f), P(98.7f, 145.2f)) }; const float sightKeep = 3f;
var warpPts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform w in Root("DevWarps").transform) warpPts.Add(P(w.position.x, w.position.z)); const float treeWarpGap = 8f;   // 8 (was 5; 8.16a: a pine's crown reached 0.6 m from the Closed_Campground warp's view)
var trees = new System.Collections.Generic.List<(UnityEngine.Vector2 p, float r)>();   // trunks placed (and the giants 8.3 placed), with their spacing
var existingGiants = new System.Collections.Generic.List<UnityEngine.Vector2>(); var existingT = new System.Collections.Generic.List<UnityEngine.Transform>();
foreach (var g in new[] { Root("Giants") }) if (g != null) foreach (UnityEngine.Transform t in g.transform) foreach (var tt in t.name == "Heroes" ? System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(t)) : new[] { t }) { trees.Add((P(tt.position.x, tt.position.z), 4f)); existingGiants.Add(P(tt.position.x, tt.position.z)); existingT.Add(tt); }   // the three heroes sit in a Heroes group
int rejTrail = 0, rejPlace = 0, rejSlope = 0, rejCollider = 0, rejSpace = 0;
bool Free(UnityEngine.Vector2 p, float spacing, float slopeMax, bool beyondFence = false)
{
    if (!beyondFence && !OnTerrain(p.x, p.y)) { rejPlace++; return false; }
    if (!beyondFence && p.x > fenceX - 2f) { rejPlace++; return false; }
    foreach (var t in trailPts) if ((t - p).sqrMagnitude < treeTrailGap * treeTrailGap) { rejTrail++; return false; }
    foreach (var c in clearings) if (UnityEngine.Vector2.Distance(p, c.c) < c.r) { rejPlace++; return false; }
    if (frontZone.Contains(p)) { rejPlace++; return false; }
    foreach (var sl in sightKeepLines) if (SegD(p, sl.a, sl.b) < sightKeep) { rejPlace++; return false; }   // 8.16a
    foreach (var w in warpPts) if ((w - p).sqrMagnitude < treeWarpGap * treeWarpGap) { rejPlace++; return false; }   // no warp lands in a tree (Pim, 8.15 gate)
    float ex = (p.x - lakeX) / (lakeA * lakeKeep), ez = (p.y - lakeZ) / (lakeB * lakeKeep); if (ex * ex + ez * ez < 1f) { rejPlace++; return false; }
    if (!beyondFence && Slope(p.x, p.y) > slopeMax) { rejSlope++; return false; }
    float gy = beyondFence ? 0f : H(p.x, p.y);
    if (!beyondFence && UnityEngine.Physics.CheckCapsule(V(p.x, gy + 0.6f, p.y), V(p.x, gy + 3f, p.y), colliderClear, UnityEngine.Physics.AllLayers, UnityEngine.QueryTriggerInteraction.Ignore))
    { foreach (var c in UnityEngine.Physics.OverlapCapsule(V(p.x, gy + 0.6f, p.y), V(p.x, gy + 3f, p.y), colliderClear, UnityEngine.Physics.AllLayers, UnityEngine.QueryTriggerInteraction.Ignore)) if (!(c is UnityEngine.TerrainCollider)) { rejCollider++; return false; } }
    foreach (var t in trees) { float need = UnityEngine.Mathf.Max(spacing, t.r); if ((t.p - p).sqrMagnitude < need * need) { rejSpace++; return false; } }
    return true;
}

// ---- planting
var forest = new UnityEngine.GameObject("Forest").transform;
int giantsN = 0, firsN = 0, footN = 0, snagsN = 0;
string[] firPaths = { BK + "Trees/RedFir5", BK + "Trees/RedFir6", BK + "Trees/RedFir7", BK + "Trees/RedFir8", BK + "Trees/RedPine1", BK + "Trees/RedPine2", BK + "Trees/RedPine3", BK + "Trees/RedPine4", BK + "Trees/RedPine5" };
string[] saplingPaths = { BK + "Trees/RedFir1", BK + "Trees/RedFir2", BK + "Trees/RedFir3", BK + "Trees/RedFir4" };
UnityEngine.GameObject Tree(string path, UnityEngine.Transform parent, UnityEngine.Vector2 p, float tall, float groundY, float spacing)
{
    var g = Spawn(path, parent); if (g == null) return null;
    g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); g.transform.position = V(p.x, 0f, p.y); g.transform.localScale = UnityEngine.Vector3.one;
    float h0 = Top(g) - Bottom(g); float s = tall / UnityEngine.Mathf.Max(0.5f, h0); g.transform.localScale = V(s, s, s);
    float b = Bottom(g); g.transform.position = V(p.x, groundY - b - treeSink, p.y);
    trees.Add((p, spacing)); return g;
}
UnityEngine.GameObject Giant(UnityEngine.Transform parent, UnityEngine.Vector2 p, float tallLow, float tallHigh)
{
    float gy = H(p.x, p.y); float tall = UnityEngine.Mathf.Min(R(tallLow, tallHigh), giantCap - gy); if (tall < tallLow * 0.7f) return null;   // tops 50 absolute or lower
    var g = Tree(BK + "Trees/Sequoia" + (1 + rng.Next(5)), parent, p, tall, gy, giantSpace); if (g != null) giantsN++; return g;
}
UnityEngine.GameObject Fir(UnityEngine.Transform parent, UnityEngine.Vector2 p, float tallLow, float tallHigh, bool sapling = false)
{
    var g = Tree(sapling ? saplingPaths[rng.Next(saplingPaths.Length)] : firPaths[rng.Next(firPaths.Length)], parent, p, R(tallLow, tallHigh), H(p.x, p.y), firSpace); if (g != null) firsN++; return g;
}
// the grove foot: suffercord bushes on an olive copy of their material (ForestPlan: retinted off autumn orange), BK ferns, logs, leaves
UnityEngine.ColorUtility.TryParseHtmlString("#8C8A50", out var sufOlive);
var sufCache = new System.Collections.Generic.Dictionary<UnityEngine.Material, UnityEngine.Material>();
UnityEngine.Material SufOlive(UnityEngine.Material src)
{
    if (sufCache.TryGetValue(src, out var m)) return m;
    string path = "Assets/Materials/Slice/Slice_Olive_" + src.name.Replace(' ', '_') + ".mat"; m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(src); UnityEditor.AssetDatabase.CreateAsset(m, path); } else m.CopyPropertiesFromMaterial(src);
    m.shader = src.shader; if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", sufOlive); if (m.HasProperty("_Color")) m.SetColor("_Color", sufOlive); UnityEditor.EditorUtility.SetDirty(m); sufCache[src] = m; return m;
}
// a stop lower than lowStopTop m over the ground (brush bands, hedges, rims) within logStopGap m of p; trees and logs of the Forest do not count
const float logStopGap = 4f, lowStopTop = 3f; int logSkips = 0;
bool NearLowStop(UnityEngine.Vector2 p)
{
    float gy = H(p.x, p.y); var fr = forest;
    foreach (var c in UnityEngine.Physics.OverlapSphere(V(p.x, gy + 1f, p.y), logStopGap, UnityEngine.Physics.AllLayers, UnityEngine.QueryTriggerInteraction.Ignore))
        if (!(c is UnityEngine.TerrainCollider) && !c.transform.IsChildOf(fr) && c.bounds.max.y - gy < lowStopTop) return true;
    return false;
}
void Foot(UnityEngine.Transform parent, string path, UnityEngine.Vector2 c, float r, float scaleLow, float scaleHigh, bool olive, bool lying = false)
{
    for (int k = 0; k < footTries; k++)
    {
        var p = InDisk(c, r); if (!OnTerrain(p.x, p.y) || Slope(p.x, p.y) > floorSlope) continue;
        if (IsPackLog(path) && NearLowStop(p)) { logSkips++; continue; }   // 8.16a: a solid log beside a low stop is a step over it (the IW3 check jumped log, brush band, campground)
        bool nearTrail = false; foreach (var t in trailPts) if ((t - p).sqrMagnitude < footTrailGap * footTrailGap) { nearTrail = true; break; } if (nearTrail) continue;
        var g = Spawn(path, parent); if (g == null) return;
        float s = R(scaleLow, scaleHigh); g.transform.localScale = V(s, s, s); g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f);
        if (olive) foreach (var rr in g.GetComponentsInChildren<UnityEngine.Renderer>()) { var ms = rr.sharedMaterials; for (int i = 0; i < ms.Length; i++) if (ms[i] != null) ms[i] = SufOlive(ms[i]); rr.sharedMaterials = ms; }
        g.transform.position = V(p.x, 0f, p.y); float b = Bottom(g); g.transform.position = V(p.x, H(p.x, p.y) - b - (lying ? 0.2f : 0.05f), p.y); footN++; return;
    }
}
void GroveFoot(UnityEngine.Transform g, UnityEngine.Vector2 c, float r)
{
    for (int i = 0; i < footBush; i++) Foot(g, SUF + "Bush" + (1 + rng.Next(4)), c, r, 1.2f, 2f, true);
    for (int i = 0; i < footFern; i++) Foot(g, BK + "Plants/ThinFern" + (1 + rng.Next(5)), c, r, 1.5f, 2.5f, false);
    int logs = 1 + rng.Next(2); for (int i = 0; i < logs; i++) Foot(g, BK + "HollowLogs/RedwoodHollowLog_" + rng.Next(3), c, r, 0.9f, 1.3f, false, true);
    for (int i = 0; i < footLeaves; i++) Foot(g, BK + "Plants/DeadLeaves" + (1 + rng.Next(2)), c, r, 1.5f, 2.5f, false);
    for (int i = 0; i < footBranch; i++) Foot(g, BK + "Plants/Branchs", c, r, 1f, 1.5f, false);
}

// 1. groves: (name, centre, radius, giants, tall low, tall high, firs)
var groves = new (string n, UnityEngine.Vector2 c, float r, int giants, float lo, float hi, int firs)[] {
    ("N1", P(175f, 282f), 20f, 8, 34f, 40f, 15), ("N2", P(120f, 280f), 16f, 6, 32f, 38f, 12), ("NE", P(335f, 285f), 16f, 5, 38f, 42f, 12),
    ("Mid", P(215f, 222f), 14f, 5, 36f, 42f, 15), ("J", P(132f, 212f), 14f, 6, 36f, 40f, 12), ("NWFoot", P(92f, 250f), 14f, 6, 34f, 38f, 12),
    ("Rim", P(115f, 128f), 14f, 6, 40f, 46f, 12), ("SW", P(92f, 75f), 14f, 6, 42f, 48f, 12), ("Ravine", P(65f, 20f), 14f, 5, 32f, 36f, 10),
    ("LakeS1", P(165f, 18f), 14f, 7, 42f, 48f, 13), ("LakeS2", P(220f, 18f), 14f, 6, 42f, 48f, 13), ("BoathouseE", P(270f, 40f), 14f, 5, 41f, 47f, 12),
    ("Camp2E", P(325f, 100f), 14f, 5, 40f, 46f, 12), ("SE", P(340f, 40f), 16f, 6, 40f, 46f, 13), ("HollowGiant", P(208f, 130f), 10f, 4, 38f, 44f, 10),
    ("BurnEdge", P(285f, 130f), 12f, 5, 40f, 46f, 10), ("EastKnotN", P(315f, 225f), 8f, 3, 40f, 45f, 0), ("EastKnotS", P(305f, 50f), 8f, 3, 40f, 45f, 0) };
var groveMembers = new System.Collections.Generic.List<(string n, System.Collections.Generic.List<UnityEngine.Transform> m)>();   // for the emergent read (section 7)
var inGrove = new System.Collections.Generic.HashSet<UnityEngine.Transform>();
foreach (var gv in groves)
{
    var g = new UnityEngine.GameObject("Grove_" + gv.n).transform; g.SetParent(forest, false);
    var placed = new System.Collections.Generic.List<UnityEngine.Vector2>(); var members = new System.Collections.Generic.List<UnityEngine.Transform>();
    // 8.3's giants inside the grove count toward its number (Valley.md 10: about 115 giants in all, 41 of them already standing)
    int standing = 0;
    for (int i = 0; i < existingGiants.Count; i++) if (UnityEngine.Vector2.Distance(existingGiants[i], gv.c) <= gv.r + groveCountPad) { standing++; placed.Add(existingGiants[i]); if (inGrove.Add(existingT[i])) members.Add(existingT[i]); }
    for (int t = 0, n = standing; t < groveTries && n < gv.giants; t++) { var p = InDisk(gv.c, gv.r); if (!Free(p, giantSpace, floorSlope)) continue; var gg = Giant(g, p, gv.lo, gv.hi); if (gg != null) { placed.Add(p); members.Add(gg.transform); n++; } }
    groveMembers.Add((gv.n, members));
    for (int t = 0, n = 0; t < groveTries && n < UnityEngine.Mathf.Min(gv.firs, groveFirCap); t++)
    {
        var near = placed.Count > 0 ? placed[rng.Next(placed.Count)] : gv.c; var p = near + (InDisk(P(0f, 0f), 1f).normalized * R(firUnderLow, firUnderHigh));   // under the giants, not beside them
        if (!Free(p, firSpace, floorSlope)) continue; if (Fir(g, p, 8f, 20f) != null) n++;
    }
    GroveFoot(g, gv.c, gv.r);
}
// 1b. 8.17 gate (Marlow 4; Valley.md E11: the ruin is not seen from the deck): a screen of firs across the deck-to-ruin line, ruinScreen
// m south of the ruin in three rows (between the ruin and the north loop, which runs 10 m south), 2 m apart (ruinScreenSpace), south of
// the side path's view from the loop to the south-west doorway, so the ruin stays seen from the loop
const float ruinScreenSpace = 2f;
{
    var g = new UnityEngine.GameObject("RuinScreen").transform; g.SetParent(forest, false);
    var deckP = P(164f, 166f); var ruinP = P(172f, 281f); var along = (deckP - ruinP).normalized; var side = P(along.y, -along.x);
    foreach (var s in new[] { 5f, 7.5f, 10f }) foreach (var off in new[] { -6f, -4f, -2f, 0f, 2f, 4f, 6f })
    {
        var p = ruinP + along * s + side * (off + R(-0.4f, 0.4f)); if (Free(p, ruinScreenSpace, floorSlope)) Fir(g, p, 14f, 22f);
    }
}
// C1 ring round Camp 1 (r 35) and the knoll (4 giants at 35 m round the camp clearing's edge)
var knollM = new System.Collections.Generic.List<UnityEngine.Transform>();
{
    var g = new UnityEngine.GameObject("Grove_C1Ring").transform; g.SetParent(forest, false); var ringM = new System.Collections.Generic.List<UnityEngine.Transform>();
    for (int t = 0, n = 0; t < groveTries && n < 7; t++) { float a = R(0f, 2f * UnityEngine.Mathf.PI); var p = P(282f, 238f) + P(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * R(c1RingIn, c1RingOut); if (!Free(p, giantSpace, floorSlope)) continue; var gg = Giant(g, p, 40f, 45f); if (gg != null) { ringM.Add(gg.transform); n++; } }
    groveMembers.Add(("C1Ring", ringM));
    for (int t = 0, n = 0; t < groveTries && n < 15; t++) { float a = R(0f, 2f * UnityEngine.Mathf.PI); var p = P(282f, 238f) + P(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * R(c1RingIn, c1RingOut); if (Free(p, firSpace, floorSlope) && Fir(g, p, 8f, 20f) != null) n++; }
    GroveFoot(g, P(282f, 238f) + P(0f, c1RingOut * 0.9f), c1RingOut - c1RingIn);
    var k = new UnityEngine.GameObject("Grove_Knoll").transform; k.SetParent(forest, false);
    for (int t = 0, n = 0; t < groveTries && n < 4; t++) { float a = R(0f, 2f * UnityEngine.Mathf.PI); var p = P(170f, 160f) + P(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * R(knollIn, knollOut); if (!Free(p, giantSpace, floorSlope)) continue; var gg = Giant(k, p, knollTall, knollTall); if (gg != null) { knollM.Add(gg.transform); n++; } }
}

// 2. the north fir wall
{
    var w = new UnityEngine.GameObject("NorthFirWall").transform; w.SetParent(forest, false);
    float Front(float x) => wallZ0 + wallWander * (UnityEngine.Mathf.PerlinNoise(x / 25f + 3.3f, 0.7f) * 2f - 1f);
    for (int t = 0, n = 0; t < wallFirs * 6 && n < wallFirs; t++) { float x = R(wallX0, wallX1); var p = P(x, R(Front(x), wallZ1)); if (Free(p, firSpace, floorSlope) && Fir(w, p, 8f, 24f) != null) n++; }
    for (int t = 0, n = 0; t < groveTries && n < wallGiants; t++) { float x = R(wallX0, wallX1); var p = P(x, R(Front(x) + 5f, wallZ1)); if (Free(p, giantSpace, floorSlope) && Giant(w, p, 34f, 38f) != null) n++; }
    for (int t = 0, n = 0; t < wallSaplings * 6 && n < wallSaplings; t++) { float x = R(wallX0, wallX1); var p = P(x, Front(x) - R(1f, 5f)); if (Free(p, firSpace, floorSlope) && Fir(w, p, 2f, 5f, true) != null) n++; }
    for (float x = wallX0; x < wallX1; x += wallFootStep) { Foot(w, SUF + "Bush" + (1 + rng.Next(4)), P(x, Front(x) - 1f), 3f, 1.2f, 2f, true); Foot(w, BK + (rng.NextDouble() < 0.5 ? "HollowLogs/RedwoodHollowLog_" + rng.Next(3) : "Plants/RedFirBranches"), P(x, Front(x) + 4f), 4f, 0.9f, 1.3f, false, true); }
}

// 3. crests
int crestN = 0;
{
    var belt = new UnityEngine.GameObject("CrestBelt_W").transform; belt.SetParent(forest, false);
    bool BeltOk(UnityEngine.Vector2 p) => H(p.x, p.y) >= beltGround && !(p.y > beltGapZ0 && p.y < beltGapZ1);   // the knob, ledge, cleft and climb stay bare
    var knots = new System.Collections.Generic.List<UnityEngine.Vector2>(); for (int i = 0; i < beltKnots; i++) knots.Add(P(R(beltX0, beltX1), R(beltZ0, beltZ1)));
    for (int t = 0, n = 0; t < beltFirs * 8 && n < beltFirs; t++)
    {
        var p = rng.NextDouble() < beltKnotShare ? InDisk(knots[rng.Next(knots.Count)], beltKnotR) : P(R(beltX0, beltX1), R(beltZ0, beltZ1));   // knots and scatter, never a line
        if (!BeltOk(p) || !Free(p, firSpace, crestSlope)) continue; if (Fir(belt, p, beltLow, beltHigh) != null) { n++; crestN++; }
    }
    for (int k = 0; k < beltGiantKnots; k++)
    {
        var c = knots[rng.Next(knots.Count)];
        for (int t = 0, n = 0; t < groveTries && n < beltGiants / beltGiantKnots; t++) { var p = InDisk(c, beltKnotR); if (!BeltOk(p) || !Free(p, giantSpace, crestSlope)) continue; float gy = H(p.x, p.y); if (Tree(BK + "Trees/Sequoia" + (1 + rng.Next(5)), belt, p, R(30f, 38f), gy, giantSpace) != null) { n++; giantsN++; crestN++; } }
    }
    var nl = new UnityEngine.GameObject("CrestLine_N").transform; nl.SetParent(forest, false);
    foreach (var run in nRuns) for (int k = 0; k < nClumps; k++) { var cc = P(R(run.x, run.y), R(nZ0, nZ1)); for (int t = 0, n = 0; t < nFirsPerRun * 8 && n < nFirsPerRun / nClumps; t++) { var p = InDisk(cc, nClumpR); if (!Free(p, firSpace, crestSlope)) continue; if (Fir(nl, p, 12f, 24f) != null) { n++; crestN++; } } }
    var sl = new UnityEngine.GameObject("CrestLine_S").transform; sl.SetParent(forest, false);
    for (int t = 0, n = 0; t < sFirs * 10 && n < sFirs; t++) { var p = P(R(sX0, sX1), R(sZ1, sZ0)); if (UnityEngine.Mathf.Abs(p.x - sSaddleX) < sSaddleHalf || !Free(p, sSpace, crestSlope)) continue; if (Fir(sl, p, 15f, 22f) != null) { n++; crestN++; } }
}

// 4. the open east: 10 clumps of about 20 firs, clear of the tower deck's lines and the stack-to-T line
{
    var ce = new UnityEngine.GameObject("OpenEastClumps").transform; ce.SetParent(forest, false);
    var deck = P(164f, 166f); var lines = new[] { (deck, P(418f, 136f)), (deck, P(344f, 200f)), (deck, P(428f, 170f)), (P(290f, 105f), P(338f, 170f)) };
    bool LineClear(UnityEngine.Vector2 p, float keep) { foreach (var l in lines) if (SegD(p, l.Item1, l.Item2) < keep) return false; return true; }
    int clumps = 0;
    for (int t = 0; t < clumpTries && clumps < eastClumps; t++)
    {
        var c = P(R(eastX0, eastX1), R(eastZ0, eastZ1)); if (!LineClear(c, clumpLineKeep + clumpR)) continue;
        int n = 0; for (int k = 0; k < groveTries && n < clumpFirs; k++) { var p = InDisk(c, clumpR); if (!LineClear(p, clumpLineKeep) || !Free(p, firSpace, floorSlope)) continue; if (Fir(ce, p, 8f, 20f) != null) n++; }
        if (n > 0) clumps++;
    }
    eastClumpsN = clumps;
}

// 4b. 8.16b (Vesper: round open blobs in the top-down, north loop interior, east of the lake, south of W8): a sweep of the valley floor
// every gapStep m; wherever no tree stands within gapOpen m (so no open ground wider than about 2 x gapOpen outside the named clearings,
// the lake and the front zone, which Free keeps out), a clump of gapClumpMin to gapClumpMax firs 8 to 20 m and a bush, at most gapMax
int gapClumps = 0;
{
    var gf = new UnityEngine.GameObject("GapClumps").transform; gf.SetParent(forest, false);
    const float gapStep = 8f, gapOpen = 15f, gapClumpR = 5f, gapX0 = 40f, gapX1 = 392f, gapZ0 = -20f, gapZ1 = 300f, gapJitter = 3f; const int gapMax = 45, gapClumpMin = 3, gapClumpMax = 6;
    var burnP = new[] { P(185f, 181f), P(340f, 213f), P(340f, 143f), P(185f, 151f) };   // 8.15's old burn: its regrowth fills it
    for (float gx = gapX0; gx < gapX1 && gapClumps < gapMax; gx += gapStep) for (float gz = gapZ0; gz < gapZ1 && gapClumps < gapMax; gz += gapStep)
    {
        var p = P(gx + R(-gapJitter, gapJitter), gz + R(-gapJitter, gapJitter)); if (Inside(p, burnP)) continue;
        bool open = true; foreach (var t in trees) if ((t.p - p).sqrMagnitude < gapOpen * gapOpen) { open = false; break; } if (!open) continue;
        if (!Free(p, firSpace, floorSlope)) continue;
        int want = gapClumpMin + rng.Next(gapClumpMax - gapClumpMin + 1), n = 0;
        for (int t = 0; t < groveTries && n < want; t++) { var q = InDisk(p, gapClumpR); if (Free(q, firSpace, floorSlope) && Fir(gf, q, 8f, 20f) != null) n++; }
        if (n > 0) { Foot(gf, SUF + "Bush" + (1 + rng.Next(4)), p, gapClumpR, 1.2f, 2f, true); gapClumps++; }
    }
}
// 5. beyond the highway: masses of firs and pines 20 to 35 m on the outer ground, 100 to 400 m past the road
int beyondN = 0;
{
    var bf = new UnityEngine.GameObject("BeyondRoad").transform; bf.SetParent(forest, false);
    for (int t = 0; t < beyondTrees * 6 && beyondN < beyondTrees; t++)
    {
        bool nearBand = beyondN < beyondTrees * beyondNearShare / 100; var p = P(nearBand ? R(roadX + beyondNear, roadX + beyondMid) : R(roadX + beyondBack, roadX + beyondFar), R(beyondZ0, beyondZ1));
        float gy = float.MinValue; foreach (var h in UnityEngine.Physics.RaycastAll(V(p.x, beyondRayTop, p.y), UnityEngine.Vector3.down, beyondRayTop * 2f, UnityEngine.Physics.AllLayers, UnityEngine.QueryTriggerInteraction.Ignore)) gy = UnityEngine.Mathf.Max(gy, h.point.y);
        if (gy == float.MinValue) continue;
        bool crowd = false; foreach (var tr in trees) if ((tr.p - p).sqrMagnitude < beyondSpace * beyondSpace) { crowd = true; break; } if (crowd) continue;
        if (Tree(firPaths[rng.Next(firPaths.Length)], bf, p, nearBand ? R(nearLow, nearHigh) : R(backLow, backHigh), gy, beyondSpace) != null) beyondN++;
    }
}

// 8.16a (Wren: standing snags stopped nobody): a capsule on a dead tree's trunk only, measured from its mesh: the median reach of the
// vertices in the lowest snagTrunkBand of its height about their centre, up snagTrunkShare of its height (limbs start above that)
const float snagTrunkBand = 0.15f, snagTrunkShare = 0.4f, fallenTrunkShare = 0.7f;   // 8.16b (Marlow: the fallen trunks had no collider): a lying trunk keeps fallenTrunkShare of its length
void SnagTrunk(UnityEngine.GameObject g) => PlaceKit.DeadTrunkCapsule(g, snagTrunkBand, snagTrunkShare);
// 6. the old burn: 20 snags and 10 fallen trunks (Celestia's dead tree on the charred wood material)
{
    var burn = new UnityEngine.GameObject("BurnDeadwood").transform; burn.SetParent(forest, false);
    var burnPoly = new[] { P(185f, 181f), P(340f, 213f), P(340f, 143f), P(185f, 151f) };   // 8.15's old burn
    var charred = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_RoofChar.mat");
    const string dead = "Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead";
    int snags = 0, fallen = 0;
    // 8.16b (Vesper: the burn reads as a hard-edged green rectangle from the top): young firs thinning out past its edge, every
    // featherStep m along each side, at featherOut m or less outside it, more often nearer the edge
    const float featherStep = 3f, featherOut = 14f; int feathered = 0;
    for (int e = 0; e < burnPoly.Length; e++)
    {
        var a = burnPoly[e]; var b = burnPoly[(e + 1) % burnPoly.Length]; var t = (b - a).normalized; var o = P(t.y, -t.x);
        if (Inside((a + b) * 0.5f + o, burnPoly)) o = -o;   // outward
        for (float s = 0f; s < (b - a).magnitude; s += featherStep)
        {
            float d = R(0f, featherOut); if (R(0f, 1f) > 1f - d / featherOut) continue;
            var p = a + t * s + o * d; if (Free(p, firSpace, floorSlope) && Fir(burn, p, 3f, 8f) != null) feathered++;
        }
    }
    for (int t = 0; t < 600 && (snags < burnSnags || fallen < burnFallen); t++)
    {
        var p = P(R(185f, 340f), R(143f, 213f)); if (!Inside(p, burnPoly) || !Free(p, snagSpace, floorSlope)) continue;
        bool lying = snags >= burnSnags; var g = Spawn(dead, burn); if (g == null) break;
        // 8.16b (trunk check: a player got under the raised end of a trunk lying at 80 to 88 degrees): fallen trunks lie flat
        g.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), lying ? 90f : R(-4f, 4f)); g.transform.position = V(p.x, 0f, p.y); g.transform.localScale = UnityEngine.Vector3.one;
        float h0 = lying ? 4.3f : Top(g) - Bottom(g); float s = R(lying ? 6f : 8f, lying ? 12f : 16f) / UnityEngine.Mathf.Max(0.5f, h0); g.transform.localScale = V(s * snagGirth, s, s * snagGirth);
        float b = Bottom(g); g.transform.position = V(p.x, H(p.x, p.y) - b - (lying ? 0.3f : 0.2f), p.y);
        if (charred != null) foreach (var rr in g.GetComponentsInChildren<UnityEngine.Renderer>()) rr.sharedMaterial = charred;
        if (lying) { PlaceKit.DeadTrunkCapsule(g, snagTrunkBand, fallenTrunkShare); fallen++; } else { SnagTrunk(g); snags++; } snagsN++;
    }
}

// 7. Emergent read inside the cap (Style.md 5.8, 2026-09-30): in each grove (the section 1 table with the 8.3 giants it holds, the C1
// ring, and 8.3's other groves, clustered at clusterLink m) one giant tops out within emergentBelowMax m of giantCap with a broken
// crown (a dead spike, spikeTall m of weathered wood, standing spikeShow m over the live crown); the rest top out restLow to restHigh m
// below it. The knoll (8.3's knoll rule plus the knoll ring) gets one giant knollTall m tall and the rest knollRestLow to knollRestHigh
// m. No fir or pine within firKeep m of an emergent's trunk: the emergent is the giant with the fewest firs that this recipe did not
// plant (and no RuinScreen fir) in that ring, then the tallest; this recipe's firs in the ring are removed. Heroes are never resized
// (the Hollow Giant, top 50, is its grove's emergent); the Snag and the Gate Tree stub are not giants here. 8.3 giants resize with
// their 8.9f pack tree, about their base, trunk collider included.
const float emergentBelowMin = 0.3f, emergentBelowMax = 1.7f, restLow = 12f, restHigh = 18f, knollRestLow = 17f, knollRestHigh = 23f, firKeep = 10f, spikeTall = 8f, spikeShow = 4f, clusterLink = 22f, knollR = 63f, knollGround = 8f;
var emergentReport = new System.Text.StringBuilder(); int emergentN = 0, firsCleared = 0, firsKept = 0; float restTallMin = float.MaxValue;
{
    var giantsRoot = Root("Giants").transform; var slice = Root("SliceLook"); var packRoot = slice != null ? slice.transform.Find("GiantTrees") : null;
    var weathered = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Wood_1x32.mat"); if (weathered == null) missing.Add("Slice_Wood_1x32.mat");
    UnityEngine.Transform Pack(UnityEngine.Transform gray) { if (packRoot == null) return null; foreach (UnityEngine.Transform t in packRoot) if (P(t.position.x - gray.position.x, t.position.z - gray.position.z).sqrMagnitude < 0.01f) return t; return null; }
    bool IsGray(UnityEngine.Transform t) => t.IsChildOf(giantsRoot);
    bool IsHero(UnityEngine.Transform t) => t.parent != null && t.parent.name == "Heroes";
    UnityEngine.Transform Look(UnityEngine.Transform t) => IsGray(t) ? (IsHero(t) ? t : Pack(t)) : t;
    float TopOf(UnityEngine.Transform t) { var l = Look(t); if (l == null) return float.MinValue; if (IsHero(t)) { var tr = t.Find("Trunk"); return t.position.y + tr.localPosition.y + tr.localScale.y; } return Top(l.gameObject); }
    UnityEngine.Vector2 XZ(UnityEngine.Transform t) => P(t.position.x, t.position.z);
    void Resize(UnityEngine.Transform t, float top)   // uniform scale about the trunk's foot so the crown tops out at top
    {
        var p = XZ(t); float gy = H(p.x, p.y); float k = (top - gy) / UnityEngine.Mathf.Max(0.5f, TopOf(t) - gy);
        foreach (var x in IsGray(t) ? new[] { t, Look(t) } : new[] { t }) { if (x == null) continue; x.localScale *= k; x.position = V(x.position.x, gy + (x.position.y - gy) * k, x.position.z); }
    }
    // every fir and pine in the scene (pack instance roots), and which of them this recipe planted and may remove
    var firs = new System.Collections.Generic.List<(UnityEngine.GameObject g, bool mine)>(); var ruinScreen = forest.Find("RuinScreen");
    foreach (var r in scene.GetRootGameObjects()) foreach (var lg in r.GetComponentsInChildren<UnityEngine.LODGroup>(true))
    {
        var path = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(lg.gameObject); if (path == null || !(path.Contains("/Trees/RedFir") || path.Contains("/Trees/RedPine"))) continue;
        if (packRoot != null && lg.transform.IsChildOf(packRoot)) continue;   // the knoll's giants drawn as red pines
        firs.Add((lg.gameObject, lg.transform.IsChildOf(forest) && (ruinScreen == null || !lg.transform.IsChildOf(ruinScreen))));
    }
    int Near(UnityEngine.Transform t, bool mine) { int n = 0; var p = XZ(t); foreach (var f in firs) if (f.g != null && f.mine == mine && (P(f.g.transform.position.x, f.g.transform.position.z) - p).sqrMagnitude < firKeep * firKeep) n++; return n; }
    // the groups: knoll first (it takes any 8.3 knoll giant not already in a table grove), then 8.3's other groves by clustering
    var knoll = new System.Collections.Generic.List<UnityEngine.Transform>(knollM); var loose = new System.Collections.Generic.List<UnityEngine.Transform>();
    foreach (var t in existingT)
    {
        if (inGrove.Contains(t) || IsHero(t)) continue; var p = XZ(t);
        if (UnityEngine.Vector2.Distance(p, P(170f, 160f)) < knollR && H(p.x, p.y) > knollGround) knoll.Add(t); else loose.Add(t);
    }
    var groups = new System.Collections.Generic.List<(string n, System.Collections.Generic.List<UnityEngine.Transform> m, bool knoll)>();
    foreach (var gm in groveMembers) groups.Add((gm.n, gm.m, false));
    groups.Add(("Knoll", knoll, true));
    for (int c = 0; loose.Count > 0; c++)
    {
        var cl = new System.Collections.Generic.List<UnityEngine.Transform> { loose[0] }; loose.RemoveAt(0);
        for (bool grew = true; grew;) { grew = false; for (int i = loose.Count - 1; i >= 0; i--) foreach (var q in cl) if (UnityEngine.Vector2.Distance(XZ(q), XZ(loose[i])) <= clusterLink) { cl.Add(loose[i]); loose.RemoveAt(i); grew = true; break; } }
        groups.Add(("Giants83_" + c, cl, false));
    }
    foreach (var gr in groups)
    {
        var m = gr.m.FindAll(t => t != null && Look(t) != null && t.name != "Snag" && t.name != "Gate_Tree"); if (m.Count == 0) continue;
        var em = m.Find(IsHero);
        if (em == null) { em = m[0]; foreach (var t in m) { int a = Near(t, false), b = Near(em, false); if (a < b || (a == b && (Near(t, true) < Near(em, true) || (Near(t, true) == Near(em, true) && TopOf(t) > TopOf(em))))) em = t; } }
        var ep = XZ(em); float egy = H(ep.x, ep.y);
        float emTop = IsHero(em) ? TopOf(em) : gr.knoll ? egy + knollTall : giantCap - R(emergentBelowMin, emergentBelowMax);
        if (!IsHero(em))
        {
            Resize(em, emTop - spikeShow);
            var sp = Spawn("Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead", Look(em));
            if (sp != null)
            {
                sp.name = "BrokenTop"; sp.transform.rotation = UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f); sp.transform.position = V(ep.x, 0f, ep.y);
                float s = spikeTall / UnityEngine.Mathf.Max(0.5f, Top(sp) - Bottom(sp)); sp.transform.localScale = sp.transform.localScale * s; sp.transform.position = V(ep.x, emTop - Top(sp), ep.y);
                if (weathered != null) foreach (var rr in sp.GetComponentsInChildren<UnityEngine.Renderer>()) rr.sharedMaterial = weathered;
            }
        }
        float lo = float.MaxValue, hi = float.MinValue;
        foreach (var t in m)
        {
            if (t == em || IsHero(t)) continue; var p = XZ(t); float gy = H(p.x, p.y);
            float top = gr.knoll ? gy + R(knollRestLow, knollRestHigh) : emTop - R(restLow, restHigh); Resize(t, top);
            float got = TopOf(t); lo = UnityEngine.Mathf.Min(lo, got); hi = UnityEngine.Mathf.Max(hi, got); restTallMin = UnityEngine.Mathf.Min(restTallMin, got - gy);
        }
        for (int i = 0; i < firs.Count; i++) { var f = firs[i]; if (f.g == null || (P(f.g.transform.position.x, f.g.transform.position.z) - ep).sqrMagnitude >= firKeep * firKeep) continue; if (f.mine) { UnityEngine.Object.DestroyImmediate(f.g); firsCleared++; } else firsKept++; }
        emergentN++;
        emergentReport.Append(gr.n + " " + m.Count + ": top " + TopOf(em).ToString("F1") + (IsHero(em) ? " (hero)" : "") + (m.Count > 1 ? ", rest " + lo.ToString("F1") + " to " + hi.ToString("F1") : "") + "; ");
    }
}

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | emergent groves " + emergentN + ", firs cleared " + firsCleared + ", other firs within " + firKeep + " m " + firsKept + ", shortest rest giant " + restTallMin.ToString("F1") + " m tall (" + emergentReport + ") | giants " + giantsN + ", firs and pines " + firsN + " (crests " + crestN + ", open-east clumps " + eastClumpsN + ", gap clumps " + gapClumps + "), beyond the road " + beyondN + ", burn deadwood " + snagsN + ", foot pieces " + footN + " (logs kept off low stops " + logSkips + ")"
    + " | rejected: trail " + rejTrail + ", place " + rejPlace + ", slope " + rejSlope + ", collider " + rejCollider + ", spacing " + rejSpace + " | missing: " + (missing.Count == 0 ? "none" : string.Join(", ", missing));
