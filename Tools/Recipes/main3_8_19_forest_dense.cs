// Main3 task 8.19, the dense forest (Docs/Design/ForestPlan.md section 8, Vesper 2026-10-01; Grant: "Forest is far too sparse"). Run after
// main3_8_20_ward_path.cs in Main3, edit mode (the runner runs it before the 8.18 look); rerunnable on the open scene: it rebuilds
// Forest/Dense and its own terrain detail layers each run, and leaves 8.16's forest and the places as they are.
// Engine facts checked in 6000.3 (2026-10-01): the GPU Resident Drawer takes scene MeshRenderers and LODGroups only (URP core package
// source, GPUResidentDrawer.cs EnableTypeTracking), so terrain trees would not go through it: the canopy and saplings stay pack prefabs
// (Wren 2026-10-01). Instanced terrain detail meshes take the pack's ThinFern, DeadLeaves and Bush prefabs (DetailPrototype.Validate
// true) but not any LODGroup prefab (the firs): the understory is terrain detail, the trees are GameObjects.
// 1. Canopy (8.2): on the forest floor (as main3_8_19_forest_check.cs: inside the fence, off the lake, the named clearings, the camps,
//    the lot, the burn core, the Ward path region and slopes over floorSlope, firstTrunk m or more off every trail, so crowns stay off the
//    tread at eye height; saplings firstSapling m), a scan on a jittered grid at each area's
//    mean spacing; each spot draws its own spacing (the mean, plus or minus spacingVary) and stands if no trunk is nearer; 60 percent of
//    spots are clumps of clumpLow to clumpHigh (clumpR round, clumpGap apart), the rest single. Firs and pines firLow to firHigh m tall,
//    lower within emergentKeep m of a giant over emergentTop (Style 5.8: no crown near an emergent's top). The open east takes clumps only,
//    clear of the deck's lines (8.16 section 4). No tree stands on a line the sightline checks use (the deck to every place target, the
//    junctions to the cab, the pump to the Snag) where its crown would rise into the line.
// 2. Trail edges (8.4): every edgeStep m along every trail on the forest floor, each side: if no trunk stands edgeNear0 to edgeNear1 m from
//    the centre line, a fir at edgeIn0 to edgeIn1 m.
// 3. Saplings: RedFir1-4 in clumps of 3 to 5, saplingLow to saplingHigh m, saplingsPer100 per 100 square metres of grove floor (within
//    groveR of a canopy trunk).
// 4. Understory (8.3) as instanced terrain detail on grove floor (within groveR of a trunk), half at the edge band: ThinFern1-3,
//    DeadLeaves1-2, Bush1-4 on the olive copy of their material (made here as prefabs in Assets/Prefabs/Forest/Dense); clear of the trail
//    cover edge, the clearings and slopes over 35 degrees.
// 5. Floor pieces (8.3): stumps (one per stumpArea m2) and branches (branchesPer100 per 100 m2) as prefabs, hollow logs (one per logArea m2)
//    lying at angles never parallel to a trail, logTrailClear m off every trail, not beside a low stop (8.16a's rule); brush masses (one
//    per brushArea m2 in cores).
// 6. Colliders (8.5): a trunk capsule (PlaceKit.PackTrunkCapsule, 8.16a's measure) on every new tree within colliderReach m of a trail; none
//    farther out. Saplings, ferns and brush never collide; logs keep the pack log collider; stumps get their convex hull.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
var forestRoot = kit.Root("Forest"); if (forestRoot == null) return "run 8.16 first";
UnityEngine.Physics.SyncTransforms();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var terrain = kit.Terrain; var data = terrain.terrainData; var tOrg = terrain.transform.position; var size = data.size;
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + tOrg.y;
float Slope(float x, float z) => data.GetSteepness((x - tOrg.x) / size.x, (z - tOrg.z) / size.z);
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b, out float t) { var ab = b - a; t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-4f)); return UnityEngine.Vector2.Distance(p, a + ab * t); }
var rng = new System.Random(8191); float R(float a, float b) => a + (float)rng.NextDouble() * (b - a);
const string BK = PlaceKit.BK, SUF = "suffercord/PSX Autumn Forest Asset Pack/Models/";

// ---- numbers (ForestPlan 8, all P)
const float floorSlope = 38f, firstTrunk = 3.5f, firstSapling = 2.5f, spacingVary = 0.5f, clumpShare = 0.6f, clumpR = 3.5f, clumpGap = 1.8f, firLow = 9f, firHigh = 22f;
const int clumpLow = 3, clumpHigh = 7;
const float emergentTop = 45f, emergentKeep = 15f, emergentDrop = 20f, warpKeep = 8f, colliderClear = 1.2f, crownR = 3f, lineSlack = 0.5f, eastLineKeep = 10f;
const float edgeStep = 10f, edgeNear0 = 2.5f, edgeNear1 = 6f, edgeIn0 = 3f, edgeIn1 = 5f, gapOpen = 7f, gapStep = 2f, gapSearch = 3f, gapMinGap = 3f, lakeSide = 25f; const int gapPasses = 2;
const float saplingLow = 2f, saplingHigh = 6f, saplingsPer100 = 1.5f, groveR = 6f, edgeBand = 10f;
const float fernPer100 = 10f, leavesPer100 = 1f, bushPer100 = 2.5f, coverEdge = 1.8f, coverSlope = 35f;
const float stumpArea = 200f, branchesPer100 = 2f, logArea = 300f, brushArea = 400f, logTrailClear = 1.5f, logStopGap = 4f, lowStopTop = 3f;
const float colliderReach = 40f, treeSink = 0.3f, footR = 2.5f, footSlope = 42f, pieceTop = 2.5f;
const float wardX = 62f, wardZ = 200f;   // the Ward path region left to 8.20 (Proof Beat3_StairFoot, 2026-10-01: dense firs filled the stair foot)
const float lakeX = 190f, lakeZ = 60f, lakeA = 54.8f * 1.15f, lakeB = 27.6f * 1.15f, burnCore = 10f, fenceX = 392f;
// area mean spacing (8.2): north groves 4 to 5, the north fir wall 4, west 5, lake south and SE 5 to 6, centre 6, the open east clumps
float Spacing(UnityEngine.Vector2 p)
{
    if (p.x >= 90f && p.x <= 300f && p.y >= 285f) return 4f;
    if (p.y >= 235f) return 4.5f;
    if (p.x < 120f) return 5f;
    if (p.y < 50f || (p.y < 110f && p.x > 240f)) return 5.5f;
    return 6f;
}
bool OpenEast(UnityEngine.Vector2 p) => p.x > 240f && p.y > 110f && p.y < 235f;
var clearings = new (UnityEngine.Vector2 c, float r)[] { (P(170f, 160f), 25f), (P(282f, 238f), 15f), (P(292f, 108f), 15f), (P(78f, 146f), 15f), (P(172f, 281f), 8f), (P(52f, 37.5f), 6f) };   // camp, camps 1 to 3, the ruin's gap, the cave mouth
var front = new UnityEngine.Rect(330f, 140f, 66f, 80f);
bool Floor(UnityEngine.Vector2 p)
{
    if (p.x < 40f || p.x > fenceX || p.y < -25f || p.y > 330f) return false;
    if (p.x < wardX && p.y > wardZ) return false;   // the Ward path (8.20) dresses its own beats: no forest wall on the bench, the stair or the shelf
    float ex = (p.x - lakeX) / lakeA, ez = (p.y - lakeZ) / lakeB; if (ex * ex + ez * ez < 1f) return false;
    foreach (var c in clearings) if (UnityEngine.Vector2.Distance(p, c.c) < c.r) return false;
    if (front.Contains(p) || ValleyShapes.BurnDepth(p) > burnCore || Slope(p.x, p.y) > floorSlope) return false;
    return true;
}
// trails
var trails = kit.Root("Trails"); var trailSegs = new System.Collections.Generic.List<(string leg, UnityEngine.Vector2 a, UnityEngine.Vector2 b)>();
foreach (UnityEngine.Transform leg in trails.transform) { UnityEngine.Vector2? prev = null; foreach (UnityEngine.Transform pt in leg) { var q = P(pt.position.x, pt.position.z); if (prev.HasValue) trailSegs.Add((leg.name, prev.Value, q)); prev = q; } }
const float tCell = 10f; var tGrid = new System.Collections.Generic.Dictionary<(int, int), System.Collections.Generic.List<int>>();
for (int i = 0; i < trailSegs.Count; i++) { var s = trailSegs[i]; var mn = UnityEngine.Vector2.Min(s.a, s.b); var mx = UnityEngine.Vector2.Max(s.a, s.b); for (int x = (int)UnityEngine.Mathf.Floor((mn.x - 45f) / tCell); x <= (int)UnityEngine.Mathf.Floor((mx.x + 45f) / tCell); x++) for (int z = (int)UnityEngine.Mathf.Floor((mn.y - 45f) / tCell); z <= (int)UnityEngine.Mathf.Floor((mx.y + 45f) / tCell); z++) { if (!tGrid.TryGetValue((x, z), out var l)) tGrid[(x, z)] = l = new System.Collections.Generic.List<int>(); l.Add(i); } }
float TrailD(UnityEngine.Vector2 p) { float best = 1e9f; if (tGrid.TryGetValue(((int)UnityEngine.Mathf.Floor(p.x / tCell), (int)UnityEngine.Mathf.Floor(p.y / tCell)), out var l)) foreach (var i in l) best = UnityEngine.Mathf.Min(best, SegD(p, trailSegs[i].a, trailSegs[i].b, out _)); return best; }
// sight lines the checks use: (a, b) in 3D; a tree whose crown (crownR round) reaches within lineSlack of a line under it is refused
var lines = new System.Collections.Generic.List<(UnityEngine.Vector3 a, UnityEngine.Vector3 b)>();
var deckEye = V(164f, 57.6f, 166f); float Hg(float x, float z) => H(x, z);
foreach (var t in new[] { V(190f, -5.4f, 34f), V(190f, -5.4f, 60f), V(190f, -5.4f, 86f), V(240f, -1f, 52.4f), V(284f, 29f, 240f), V(284f, 20f, 240f), V(271f, Hg(271f, 234.5f) + 2f, 234.5f), V(292f, 24.2f, 108f), V(287.4f, 23.6f, 108f), V(274f, Hg(274f, 104f) + 1f, 104f), V(96f, 54f, 146.5f), V(98.9f, 40f, 147.2f), V(343.5f, 3.1f, 170f), V(358f, 3.1f, 170f), V(370f, 4.2f, 179.4f), V(350f, 6.4f, 200f), V(366f, 6.2f, 200f), V(357f, 33.3f, 205f) }) lines.Add((deckEye, t));
var cab = V(164f, 58f, 166f);
foreach (var j in new[] { P(104f, 206f), P(262f, 172f), P(128f, 70f), P(340f, 170f), P(190f, 97f), P(276f, 232f), P(286f, 99f), P(78f, 146f), P(79f, 145f) }) lines.Add((V(j.x, H(j.x, j.y) + 1.6f, j.y), cab));
lines.Add((V(190f, H(190f, 97f) + 1.6f, 97f), V(96f, 54f, 146.5f)));   // pump to the Snag (8.16a)
foreach (var t in new[] { V(240f, -1f, 52.4f), V(242.8f, -1.2f, 52.4f), V(240f, -1f, 55f) }) lines.Add((V(286.5f, H(286.5f, 102.2f) + 1.6f, 102.2f), t));   // Camp 2 to the boathouse roof (8.9 next destination)
var eastLines = new[] { (P(164f, 166f), P(418f, 136f)), (P(164f, 166f), P(344f, 200f)), (P(164f, 166f), P(428f, 170f)), (P(290f, 105f), P(338f, 170f)) };
bool LineOk(UnityEngine.Vector2 p, float top)
{
    foreach (var l in lines)
    {
        float d = SegD(p, P(l.a.x, l.a.z), P(l.b.x, l.b.z), out float t); if (d > crownR) continue;
        if (top > UnityEngine.Mathf.Lerp(l.a.y, l.b.y, t) - lineSlack) return false;
    }
    if (OpenEast(p)) foreach (var l in eastLines) if (SegD(p, l.Item1, l.Item2, out _) < eastLineKeep) return false;
    return true;
}
var warpPts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform w in kit.Root("DevWarps").transform) warpPts.Add(P(w.position.x, w.position.z));

// ---- existing trunks (every tree prefab in the scene but this recipe's) and the emergents
var dense = kit.Fresh("Dense", forestRoot.transform, V(200f, 0f, 150f), 0f);
const float hCell = 6f; var trunkGrid = new System.Collections.Generic.Dictionary<(int, int), System.Collections.Generic.List<UnityEngine.Vector2>>();
void AddTrunk(UnityEngine.Vector2 p) { var k = ((int)UnityEngine.Mathf.Floor(p.x / hCell), (int)UnityEngine.Mathf.Floor(p.y / hCell)); if (!trunkGrid.TryGetValue(k, out var l)) trunkGrid[k] = l = new System.Collections.Generic.List<UnityEngine.Vector2>(); l.Add(p); }
float NearTrunk(UnityEngine.Vector2 p, float max) { float best = max; int cx = (int)UnityEngine.Mathf.Floor(p.x / hCell), cz = (int)UnityEngine.Mathf.Floor(p.y / hCell), reach = UnityEngine.Mathf.CeilToInt(max / hCell); for (int i = -reach; i <= reach; i++) for (int j = -reach; j <= reach; j++) if (trunkGrid.TryGetValue((cx + i, cz + j), out var l)) foreach (var q in l) best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, q)); return best; }
var emergents = new System.Collections.Generic.List<(UnityEngine.Vector2 p, float top)>(); int existing = 0;
foreach (var r in scene.GetRootGameObjects())
    foreach (var t in r.GetComponentsInChildren<UnityEngine.Transform>())
    {
        if (t.IsChildOf(dense) || !UnityEditor.PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject)) continue;
        var src = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject); string n = src != null ? src.name : t.name;
        if (!(n.StartsWith("Sequoia") || n.StartsWith("RedFir") || n.StartsWith("RedPine") || n.StartsWith("Tree_Dead"))) continue;
        AddTrunk(P(t.position.x, t.position.z)); existing++;
        if (n.StartsWith("Sequoia")) { float top = float.MinValue; foreach (var rr in t.GetComponentsInChildren<UnityEngine.Renderer>()) top = UnityEngine.Mathf.Max(top, rr.bounds.max.y); if (top >= emergentTop) emergents.Add((P(t.position.x, t.position.z), top)); }
    }
float TopCap(UnityEngine.Vector2 p) { float cap = float.MaxValue; foreach (var e in emergents) if (UnityEngine.Vector2.Distance(p, e.p) < emergentKeep) cap = UnityEngine.Mathf.Min(cap, e.top - emergentDrop); return cap; }
bool ColliderFree(UnityEngine.Vector2 p, float gy)
{
    foreach (var c in UnityEngine.Physics.OverlapCapsule(V(p.x, gy + 0.6f, p.y), V(p.x, gy + 3f, p.y), colliderClear, UnityEngine.Physics.AllLayers, UnityEngine.QueryTriggerInteraction.Ignore))
        if (!(c is UnityEngine.TerrainCollider)) return false;
    return true;
}
int rejected = 0;
// a trunk at the foot of ground steeper than the slope limit is a slide pocket (hand walk TRAPS 2026-10-01: a pine at (53.3, 201.4) at the
// foot of the draw's fill bank held the player): no tree where the ground within footR m is steeper than footSlope
bool SteepNear(UnityEngine.Vector2 p) { for (int k = 0; k < 8; k++) { float a = k * UnityEngine.Mathf.PI / 4f; if (Slope(p.x + UnityEngine.Mathf.Cos(a) * footR, p.y + UnityEngine.Mathf.Sin(a) * footR) > footSlope) return true; } return false; }
bool Spot(UnityEngine.Vector2 p, float minGap, float top)
{
    if (!Floor(p) || TrailD(p) < firstTrunk || SteepNear(p)) { rejected++; return false; }
    foreach (var w in warpPts) if ((w - p).sqrMagnitude < warpKeep * warpKeep) { rejected++; return false; }
    if (NearTrunk(p, minGap) < minGap || !LineOk(p, top) || !ColliderFree(p, H(p.x, p.y))) { rejected++; return false; }
    return true;
}
// ---- planting
string[] firPaths = { BK + "Trees/RedFir5", BK + "Trees/RedFir6", BK + "Trees/RedFir7", BK + "Trees/RedFir8", BK + "Trees/RedPine1", BK + "Trees/RedPine2", BK + "Trees/RedPine3", BK + "Trees/RedPine4", BK + "Trees/RedPine5" };
string[] saplingPaths = { BK + "Trees/RedFir1", BK + "Trees/RedFir2", BK + "Trees/RedFir3", BK + "Trees/RedFir4" };
var heights = new System.Collections.Generic.Dictionary<string, float>();   // each prefab's height at scale 1
float PrefabTall(string path) { if (heights.TryGetValue(path, out var h)) return h; var g = kit.Spawn(path, dense); var b = PlaceKit.MeshBounds(g); h = b.size.y; UnityEngine.Object.DestroyImmediate(g); heights[path] = h; return h; }
int canopyN = 0, saplingN = 0, collidersN = 0;
var canopyGroup = kit.Group("Canopy", dense, dense.position, 0f); var saplingGroup = kit.Group("Saplings", dense, dense.position, 0f); var floorGroup = kit.Group("Floor", dense, dense.position, 0f);
var canopyPts = new System.Collections.Generic.List<UnityEngine.Vector2>();
UnityEngine.GameObject Tree(string path, UnityEngine.Transform parent, UnityEngine.Vector2 p, float tall, bool sapling)
{
    var g = kit.Spawn(path, parent); if (g == null) return null; PlaceKit.StripColliders(g);
    float s = tall / UnityEngine.Mathf.Max(0.5f, PrefabTall(path)); g.transform.SetPositionAndRotation(V(p.x, H(p.x, p.y) - treeSink * s, p.y), UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f)); g.transform.localScale = V(s, s, s);
    if (!sapling) { AddTrunk(p); canopyPts.Add(p); if (TrailD(p) <= colliderReach) { PlaceKit.PackTrunkCapsule(g, 0.01f, 0.06f, 0.4f, 8, 0.3f); collidersN++; } }
    return g;
}
float FirTall(UnityEngine.Vector2 p) => UnityEngine.Mathf.Min(R(firLow, firHigh), TopCap(p) - H(p.x, p.y));
// 1. canopy
for (float gx = 40f; gx <= fenceX; gx += 1f)
    for (float gz = -25f; gz <= 330f; gz += 1f)
    {
        var c = P(gx, gz); float sp = Spacing(c); if (UnityEngine.Mathf.Repeat(gx, sp) >= 1f || UnityEngine.Mathf.Repeat(gz, sp) >= 1f) continue;   // one try per sp x sp cell
        var p = c + P(R(0f, sp), R(0f, sp)); float local = sp * (1f + R(-spacingVary, spacingVary)); bool clump = OpenEast(p) || rng.NextDouble() < clumpShare;
        float tall = FirTall(p); if (tall < firLow * 0.7f) continue;
        if (!Spot(p, local, H(p.x, p.y) + tall)) continue;
        if (Tree(firPaths[rng.Next(firPaths.Length)], canopyGroup, p, tall, false) != null) canopyN++;
        if (!clump) continue;
        int want = rng.Next(clumpLow, clumpHigh + 1) - 1;
        for (int k = 0, tries = 0; k < want && tries < want * 6; tries++)
        {
            float a = R(0f, 2f * UnityEngine.Mathf.PI), d = R(clumpGap, clumpR); var q = p + P(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * d; float tq = FirTall(q) * R(0.7f, 1.3f);
            if (tq < firLow * 0.7f || !Spot(q, clumpGap, H(q.x, q.y) + tq)) continue;
            if (Tree(firPaths[rng.Next(firPaths.Length)], canopyGroup, q, tq, false) != null) { canopyN++; k++; }
        }
    }
// 1b. gaps (8.1: no open floor over 15 m across): every floor cell (gapStep grid) farther than gapOpen m from any trunk gets a fir if a
// spot within gapSearch m of it takes one (gapMinGap m from other trunks), so the scan's misses and the clump spacing leave no hole
int gapN = 0;
for (int pass = 0; pass < gapPasses; pass++)
    for (float gx = 40.5f; gx <= fenceX; gx += gapStep) for (float gz = -24.5f; gz <= 330f; gz += gapStep)
    {
        var c = P(gx, gz); if (!Floor(c) || TrailD(c) < firstTrunk || NearTrunk(c, gapOpen) < gapOpen) continue;
        for (int tries = 0; tries < 8; tries++) { var p = c + P(R(-gapSearch, gapSearch), R(-gapSearch, gapSearch)); float tall = FirTall(p); if (tall < firLow * 0.7f || !Spot(p, gapMinGap, H(p.x, p.y) + tall)) continue; if (Tree(firPaths[rng.Next(firPaths.Length)], canopyGroup, p, tall, false) != null) { gapN++; canopyN++; break; } }
    }
// 2. trail edges
int edgeN = 0;
foreach (UnityEngine.Transform leg in trails.transform)
{
    var pts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform pt in leg) pts.Add(P(pt.position.x, pt.position.z));
    float s0 = 0f;
    for (int i = 1; i < pts.Count; i++)
    {
        float segL = UnityEngine.Vector2.Distance(pts[i - 1], pts[i]); var dir = segL > 0f ? (pts[i] - pts[i - 1]) / segL : P(0f, 1f); var side = P(dir.y, -dir.x);
        for (; s0 <= segL; s0 += edgeStep)
        {
            var q = UnityEngine.Vector2.Lerp(pts[i - 1], pts[i], segL > 0f ? s0 / segL : 0f); if (!Floor(q)) continue;
            foreach (var sgn in new[] { -1f, 1f })
            {
                var look = q + side * sgn * 6f; float lx = (look.x - lakeX) / (lakeA + lakeSide), lz = (look.y - lakeZ) / (lakeB + lakeSide); if (lx * lx + lz * lz < 1f && UnityEngine.Vector2.Distance(look, P(lakeX, lakeZ)) < UnityEngine.Vector2.Distance(q, P(lakeX, lakeZ))) continue;   // 8.4: the water side of a lake trail stays open
                bool have = false; for (float d = edgeNear0; d <= edgeNear1 && !have; d += 0.5f) for (float u = -4f; u <= 4f && !have; u += 1f) if (NearTrunk(q + side * sgn * d + dir * u, 0.6f) < 0.6f) have = true;
                if (have) continue;
                for (int tries = 0; tries < 6; tries++) { var p = q + side * sgn * R(edgeIn0, edgeIn1) + dir * R(-3f, 3f); float tall = FirTall(p); if (tall < firLow * 0.7f || !Spot(p, 2f, H(p.x, p.y) + tall)) continue; if (Tree(firPaths[rng.Next(firPaths.Length)], canopyGroup, p, tall, false) != null) { edgeN++; canopyN++; break; } }
            }
        }
        s0 -= segL;
    }
}
// grove floor: within groveR of a canopy trunk (new or old); the edge band beyond it to edgeBand
float floorArea = 0f; var grovePts = new System.Collections.Generic.List<UnityEngine.Vector2>();
for (float gx = 40.5f; gx <= fenceX; gx += 2f) for (float gz = -24.5f; gz <= 330f; gz += 2f) { var p = P(gx, gz); if (!Floor(p) || TrailD(p) < firstTrunk) continue; if (NearTrunk(p, groveR) < groveR) { grovePts.Add(p); floorArea += 4f; } }
// 3. saplings
{
    int want = UnityEngine.Mathf.RoundToInt(floorArea / 100f * saplingsPer100);
    for (int t = 0; t < want * 4 && saplingN < want; t++)
    {
        var c = grovePts[rng.Next(grovePts.Count)] + P(R(-1f, 1f), R(-1f, 1f)); int n = rng.Next(3, 6);
        for (int k = 0; k < n && saplingN < want; k++)
        {
            var p = c + P(R(-2f, 2f), R(-2f, 2f)); if (!Floor(p) || TrailD(p) < firstSapling || NearTrunk(p, 1.2f) < 1.2f) continue;
            float tall = R(saplingLow, saplingHigh); if (!LineOk(p, H(p.x, p.y) + tall)) continue;
            if (Tree(saplingPaths[rng.Next(saplingPaths.Length)], saplingGroup, p, tall, true) != null) saplingN++;
        }
    }
}
// 5. floor pieces
int stumps = 0, branches = 0, logs = 0, brush = 0, logSkips = 0;
bool NearLowStop(UnityEngine.Vector2 p) { float gy = H(p.x, p.y); foreach (var c in UnityEngine.Physics.OverlapSphere(V(p.x, gy + 1f, p.y), logStopGap, UnityEngine.Physics.AllLayers, UnityEngine.QueryTriggerInteraction.Ignore)) if (!(c is UnityEngine.TerrainCollider) && !c.transform.IsChildOf(forestRoot.transform) && c.bounds.max.y - gy < lowStopTop) return true; return false; }
UnityEngine.GameObject Piece(string path, UnityEngine.Vector2 p, float scale, bool keepCollider)
{
    var g = kit.Spawn(path, floorGroup); if (g == null) return null; if (!keepCollider) PlaceKit.StripColliders(g);
    g.transform.SetPositionAndRotation(V(p.x, 0f, p.y), UnityEngine.Quaternion.Euler(0f, R(0f, 360f), 0f)); g.transform.localScale = V(scale, scale, scale);
    var b = PlaceKit.MeshBounds(g); g.transform.position += V(0f, H(p.x, p.y) - b.min.y - 0.05f, 0f); return g;
}
foreach (var (path, perArea, count) in new[] { (PlaceKit.CI + "Vegetation/CITW_Tree_Stump", stumpArea, 0), (BK + "Plants/Branchs", 100f / branchesPer100, 1), (BK + "HollowLogs/RedwoodHollowLog_", logArea, 2), ("Revolving Pizza Games/Campsite/Prefabs/Vegetation/CS_Bush_Large_1", brushArea, 3) })
{
    int want = UnityEngine.Mathf.RoundToInt(floorArea / perArea);
    for (int t = 0, n = 0; t < want * 3 && n < want; t++)
    {
        var p = grovePts[rng.Next(grovePts.Count)] + P(R(-1f, 1f), R(-1f, 1f)); if (!Floor(p) || TrailD(p) < (count == 2 ? 3f : 2f) || !LineOk(p, H(p.x, p.y) + pieceTop)) continue;   // a log blocked Camp 2's view of the boathouse (8.9, 2026-10-01)
        if (count == 3 && NearTrunk(p, groveR * 0.5f) > groveR * 0.5f) continue;   // brush masses in the cores
        if ((count == 2 || count == 0) && (NearLowStop(p) || SteepNear(p))) { logSkips++; continue; }   // logs and stumps: never beside a low stop or at a steep foot (a stump by the band made a pocket)
        var g = Piece(count == 2 ? path + rng.Next(3) : path, p, count == 2 ? R(0.9f, 1.3f) : count == 3 ? R(1f, 1.4f) : R(0.8f, 1.2f), count == 2);   // logs keep the pack collider
        if (g != null && count == 0) foreach (var mf in g.GetComponentsInChildren<UnityEngine.MeshFilter>()) { var mc = mf.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; }   // a stump stands solid (the pack ships it without a collider)
        if (g == null) continue;
        if (count == 2)   // a log: never parallel to a trail, its whole drawn length logTrailClear m off every trail
        {
            var lb = PlaceKit.MeshBounds(g); bool close = false;
            foreach (var s in trailSegs) { if (SegD(P(lb.center.x, lb.center.z), s.a, s.b, out _) > 15f) continue; for (float u = 0f; u <= 1f && !close; u += 0.1f) { var q = UnityEngine.Vector2.Lerp(s.a, s.b, u); float dx = UnityEngine.Mathf.Max(lb.min.x - q.x, 0f, q.x - lb.max.x), dz = UnityEngine.Mathf.Max(lb.min.z - q.y, 0f, q.y - lb.max.z); if (dx * dx + dz * dz < logTrailClear * logTrailClear) close = true; } if (close) break; }
            if (close) { UnityEngine.Object.DestroyImmediate(g); continue; }
        }
        n++; if (count == 0) stumps++; else if (count == 1) branches++; else if (count == 2) logs++; else brush++;
    }
}
// 4. understory as instanced detail: prefabs made here (olive bushes), prototypes appended after 8.15's, layers set each run
const string denseDir = "Assets/Prefabs/Forest/Dense";
if (!UnityEditor.AssetDatabase.IsValidFolder(denseDir)) UnityEditor.AssetDatabase.CreateFolder("Assets/Prefabs/Forest", "Dense");
var olive = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Olive_Foliage512.mat"); if (olive == null) return "no Slice_Olive_Foliage512.mat (8.16)";
UnityEngine.GameObject DetailPrefab(string name, string src, UnityEngine.Material mat)
{
    string path = denseDir + "/" + name + ".prefab"; var pf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/" + src + ".prefab"); if (pf == null) { kit.Missing.Add(src); return null; }
    var inst = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pf); UnityEditor.PrefabUtility.UnpackPrefabInstance(inst, UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);
    PlaceKit.StripColliders(inst); if (mat != null) foreach (var r in inst.GetComponentsInChildren<UnityEngine.Renderer>()) { var ms = r.sharedMaterials; for (int i = 0; i < ms.Length; i++) ms[i] = mat; r.sharedMaterials = ms; }
    inst.transform.position = UnityEngine.Vector3.zero; var saved = UnityEditor.PrefabUtility.SaveAsPrefabAsset(inst, path); UnityEngine.Object.DestroyImmediate(inst); return saved;
}
var understory = new System.Collections.Generic.List<(UnityEngine.GameObject pf, float per100, float minS, float maxS)>();
for (int i = 1; i <= 3; i++) understory.Add((DetailPrefab("Dense_ThinFern" + i, BK + "Plants/ThinFern" + i, null), fernPer100 / 3f, 1.4f, 2.4f));
for (int i = 1; i <= 2; i++) understory.Add((DetailPrefab("Dense_DeadLeaves" + i, BK + "Plants/DeadLeaves" + i, null), leavesPer100 / 2f, 1.5f, 2.5f));
for (int i = 1; i <= 4; i++) understory.Add((DetailPrefab("Dense_Bush" + i, SUF + "Bush" + i, olive), bushPer100 / 4f, 1.2f, 2f));
var protos = new System.Collections.Generic.List<UnityEngine.DetailPrototype>();
foreach (var dp in data.detailPrototypes) if (dp.prototype == null || !UnityEditor.AssetDatabase.GetAssetPath(dp.prototype).StartsWith(denseDir)) protos.Add(dp);
int firstOurs = protos.Count;
foreach (var u in understory) { if (u.pf == null) continue; var dp = new UnityEngine.DetailPrototype { prototype = u.pf, usePrototypeMesh = true, renderMode = UnityEngine.DetailRenderMode.VertexLit, useInstancing = true, minWidth = u.minS, maxWidth = u.maxS, minHeight = u.minS, maxHeight = u.maxS, alignToGround = 0.3f }; if (!dp.Validate(out string why)) return "detail " + u.pf.name + " invalid: " + why; protos.Add(dp); }
data.detailPrototypes = protos.ToArray();
{
    int dres = data.detailResolution; float dX = size.x / dres, dZ = size.z / dres, cellArea = dX * dZ; long placed = 0;
    var maps = new int[understory.Count][,]; for (int k = 0; k < maps.Length; k++) maps[k] = new int[dres, dres];
    for (int z = 0; z < dres; z++) for (int x = 0; x < dres; x++)
    {
        var p = P(tOrg.x + (x + 0.5f) * dX, tOrg.z + (z + 0.5f) * dZ); if (!Floor(p) || data.GetSteepness((x + 0.5f) / dres, (z + 0.5f) / dres) > coverSlope) continue;
        float td = TrailD(p); if (td < coverEdge) continue;
        float nt = NearTrunk(p, edgeBand); if (nt >= edgeBand) continue; float share = nt <= groveR ? 1f : 0.5f;
        for (int k = 0; k < understory.Count; k++) { double expect = understory[k].per100 / 100f * cellArea * share; if (rng.NextDouble() < expect) { maps[k][z, x] = 1; placed++; } }
    }
    for (int k = 0; k < understory.Count; k++) data.SetDetailLayer(0, 0, firstOurs + k, maps[k]);
    UnityEditor.EditorUtility.SetDirty(data);
    floorArea = UnityEngine.Mathf.Round(floorArea);
    UnityEditor.AssetDatabase.SaveAssets();
    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
    bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
    return "saved=" + saved + " | existing trees " + existing + " (emergents " + emergents.Count + ") | canopy added " + canopyN + " (trail edge " + edgeN + "), saplings " + saplingN + ", trunk colliders " + collidersN + " | grove floor " + floorArea + " m2 | stumps " + stumps + ", branches " + branches + ", logs " + logs + " (skipped by a low stop " + logSkips + "), brush " + brush + " | understory instances " + placed + " | spots refused " + rejected + " | " + kit.Report();
}
