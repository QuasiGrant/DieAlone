// Main3 8.19 forest check (ForestPlan.md section 8, Vesper 2026-10-01). Edit mode or Play; never saves. Counts and the two floor bars:
// 1. COUNTS: canopy (BK firs and pines over saplingTop m, Sequoia giants, snags counted apart) and saplings, scene-wide, and per area of
//    section 8.2 (areas below, by their map boxes; unverified against the plan's hectares, which it says are read off the map).
// 2. OPEN FLOOR (8.1): on a 1 m grid over the forest floor (the valley floor inside the fence, x 40 to 392, z -25 to 330, ground slope
//    under floorSlope; not the lake, the named clearings, the camps, the lot and front zone, the burn core, or within trailKeep m of a
//    trail), the distance to the nearest trunk; a cell farther than openR (half of 15 m) is open floor. FAIL if any open patch is found;
//    lists the worst (largest) patch centres.
// 3. TRAIL TRUNKS (8.1 and 8.4): along every trail, every edgeStep m, each side: a trunk within edgeNear m of the centre line. Reported per
//    trail as the share of points with trunks on both sides; the bar is 4 of 5 frames (80 percent), measured off the lake, camps, lot and
//    climb (which 8.4 dresses otherwise).
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var ter = UnityEngine.Terrain.activeTerrain; var data = ter.terrainData; var tOrg = ter.transform.position; var size = data.size;
float Slope(float x, float z) => data.GetSteepness((x - tOrg.x) / size.x, (z - tOrg.z) / size.z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
const float saplingTop = 8f, floorSlope = 38f, openR = 7.5f, trailKeep = 3f, edgeStep = 10f, edgeNear = 10f, lakeX = 190f, lakeZ = 60f, lakeA = 54.8f * 1.15f, lakeB = 27.6f * 1.15f, burnCore = 10f;
var clearings = new (UnityEngine.Vector2 c, float r)[] { (P(170f, 160f), 25f), (P(282f, 238f), 15f), (P(292f, 108f), 15f), (P(78f, 146f), 15f), (P(172f, 281f), 8f) };   // camp, camps 1 to 3, the ruin's light gap
var front = new UnityEngine.Rect(330f, 140f, 66f, 80f);   // the lot, office, store and booth
var areas = new (string n, UnityEngine.Rect r)[] { ("North groves", new UnityEngine.Rect(80f, 235f, 312f, 50f)), ("North fir wall", new UnityEngine.Rect(90f, 285f, 210f, 30f)), ("West", new UnityEngine.Rect(40f, -20f, 80f, 255f)), ("Lake south and SE", new UnityEngine.Rect(120f, -25f, 272f, 75f)), ("Centre", new UnityEngine.Rect(120f, 110f, 120f, 125f)), ("Open east", new UnityEngine.Rect(240f, 50f, 152f, 185f)) };
// trunks
var trunks = new System.Collections.Generic.List<(UnityEngine.Vector2 p, string kind)>();
int giants = 0, canopy = 0, saplings = 0, snags = 0;
foreach (var r in scene.GetRootGameObjects())
    foreach (var t in r.GetComponentsInChildren<UnityEngine.Transform>())
    {
        if (!UnityEditor.PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject)) continue;
        var src = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject); string n = src != null ? src.name : t.name;
        string kind = n.StartsWith("Sequoia") ? "giant" : n.StartsWith("RedFir") || n.StartsWith("RedPine") ? "fir" : n.StartsWith("Tree_Dead") || n.Contains("Snag") ? "snag" : null; if (kind == null) continue;
        float top = float.MinValue, bot = float.MaxValue; foreach (var rr in t.GetComponentsInChildren<UnityEngine.Renderer>()) { top = UnityEngine.Mathf.Max(top, rr.bounds.max.y); bot = UnityEngine.Mathf.Min(bot, rr.bounds.min.y); }
        if (kind == "fir" && top - bot < saplingTop) { saplings++; continue; }
        if (kind == "giant") giants++; else if (kind == "fir") canopy++; else snags++;
        trunks.Add((P(t.position.x, t.position.z), kind));
    }
bool allOk = true; var sb = new System.Text.StringBuilder("COUNTS: giants " + giants + ", canopy firs and pines " + canopy + ", saplings " + saplings + ", snags " + snags + " (plan: about 150 giants, 3,040 canopy, 1,650 saplings)\n");
foreach (var a in areas) { int n = 0; foreach (var tr in trunks) if (tr.kind != "snag" && a.r.Contains(tr.p)) n++; sb.Append("  " + a.n + ": " + n + " canopy and giants\n"); }
// spatial hash of trunks
const float cell = 8f; var grid = new System.Collections.Generic.Dictionary<(int, int), System.Collections.Generic.List<UnityEngine.Vector2>>();
foreach (var tr in trunks) { var k = ((int)UnityEngine.Mathf.Floor(tr.p.x / cell), (int)UnityEngine.Mathf.Floor(tr.p.y / cell)); if (!grid.TryGetValue(k, out var l)) grid[k] = l = new System.Collections.Generic.List<UnityEngine.Vector2>(); l.Add(tr.p); }
float Nearest(UnityEngine.Vector2 p, float max) { float best = max; int cx = (int)UnityEngine.Mathf.Floor(p.x / cell), cz = (int)UnityEngine.Mathf.Floor(p.y / cell), reach = UnityEngine.Mathf.CeilToInt(max / cell); for (int i = -reach; i <= reach; i++) for (int j = -reach; j <= reach; j++) if (grid.TryGetValue((cx + i, cz + j), out var l)) foreach (var q in l) best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, q)); return best; }
// trails
var trailSegs = new System.Collections.Generic.List<(string leg, UnityEngine.Vector2 a, UnityEngine.Vector2 b)>();
foreach (UnityEngine.Transform leg in UnityEngine.GameObject.Find("Trails").transform) { UnityEngine.Vector2? prev = null; foreach (UnityEngine.Transform pt in leg) { var q = P(pt.position.x, pt.position.z); if (prev.HasValue) trailSegs.Add((leg.name, prev.Value, q)); prev = q; } }
float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 1e-4f)); return UnityEngine.Vector2.Distance(p, a + ab * t); }
bool Floor(UnityEngine.Vector2 p)
{
    if (p.x < 40f || p.x > 392f || p.y < -25f || p.y > 330f) return false;
    if (p.x < 62f && p.y > 200f) return false;   // the Ward path region (8.20 dresses it)
    float ex = (p.x - lakeX) / lakeA, ez = (p.y - lakeZ) / lakeB; if (ex * ex + ez * ez < 1f) return false;
    foreach (var c in clearings) if (UnityEngine.Vector2.Distance(p, c.c) < c.r) return false;
    if (front.Contains(p) || ValleyShapes.BurnDepth(p) > burnCore || Slope(p.x, p.y) > floorSlope) return false;
    return true;
}
var trailNear = new System.Collections.Generic.HashSet<(int, int)>();
foreach (var s in trailSegs) { var mn = UnityEngine.Vector2.Min(s.a, s.b) - UnityEngine.Vector2.one * trailKeep; var mx = UnityEngine.Vector2.Max(s.a, s.b) + UnityEngine.Vector2.one * trailKeep; for (int x = (int)mn.x; x <= (int)mx.x; x++) for (int z = (int)mn.y; z <= (int)mx.y; z++) if (SegD(P(x + 0.5f, z + 0.5f), s.a, s.b) < trailKeep) trailNear.Add((x, z)); }
int floorCells = 0, openCells = 0; var open = new System.Collections.Generic.List<(UnityEngine.Vector2 p, float d)>();
for (int x = 40; x < 392; x++) for (int z = -25; z < 330; z++)
{
    var p = P(x + 0.5f, z + 0.5f); if (trailNear.Contains((x, z)) || !Floor(p)) continue; floorCells++;
    float d = Nearest(p, openR * 3f); if (d > openR) { openCells++; open.Add((p, d)); }
}
open.Sort((a, b) => b.d.CompareTo(a.d)); var worst = new System.Collections.Generic.List<(UnityEngine.Vector2 p, float d)>();
foreach (var o in open) { bool near = false; foreach (var w in worst) if (UnityEngine.Vector2.Distance(w.p, o.p) < 15f) near = true; if (!near) worst.Add(o); if (worst.Count >= 25) break; }
if (openCells > 0) allOk = false;
sb.Append("OPEN FLOOR: " + floorCells + " floor cells (1 m), " + openCells + " more than " + openR + " m from any trunk (" + (100f * openCells / UnityEngine.Mathf.Max(1, floorCells)).ToString("F1") + " percent)" + (openCells == 0 ? ": PASS" : ": FAIL") + "\n");
foreach (var w in worst) sb.Append("  open at " + w.p.ToString("F0") + ", nearest trunk " + w.d.ToString("F1") + " m\n");
// trail trunks
foreach (UnityEngine.Transform leg in UnityEngine.GameObject.Find("Trails").transform)   // the climb is dressed by 8.20 and 8.4's climb rule, not this bar
{
    if (leg.name == "J to Ward") continue;
    var pts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform pt in leg) pts.Add(P(pt.position.x, pt.position.z));
    int both = 0, n = 0; float s0 = 0f;
    for (int i = 1; i < pts.Count; i++)
    {
        float segL = UnityEngine.Vector2.Distance(pts[i - 1], pts[i]);
        for (; s0 <= segL; s0 += edgeStep)
        {
            var q = UnityEngine.Vector2.Lerp(pts[i - 1], pts[i], segL > 0f ? s0 / segL : 0f); if (!Floor(q)) continue;
            var dir = (pts[i] - pts[i - 1]).normalized; var side = P(dir.y, -dir.x); bool l = false, r = false;
            int cx = (int)UnityEngine.Mathf.Floor(q.x / cell), cz = (int)UnityEngine.Mathf.Floor(q.y / cell);
            for (int a = -2; a <= 2; a++) for (int b = -2; b <= 2; b++) if (grid.TryGetValue((cx + a, cz + b), out var lst)) foreach (var t in lst) { var d = t - q; if (d.magnitude > edgeNear) continue; if (UnityEngine.Vector2.Dot(d, side) > 0f) r = true; else l = true; }
            foreach (var sgn in new[] { -1f, 1f }) { var look = q + side * sgn * 6f; float lx = (look.x - lakeX) / (lakeA + 25f), lz = (look.y - lakeZ) / (lakeB + 25f); if (lx * lx + lz * lz < 1f && UnityEngine.Vector2.Distance(look, P(lakeX, lakeZ)) < UnityEngine.Vector2.Distance(q, P(lakeX, lakeZ))) { if (sgn > 0f) r = true; else l = true; } }   // 8.4: a lake trail's water side stays open
            n++; if (l && r) both++;
        }
        s0 -= segL;
    }
    if (n > 0 && both * 5 < n * 4) allOk = false;
    if (n > 0) sb.Append("TRAIL " + leg.name + ": trunks within " + edgeNear + " m both sides at " + both + " of " + n + " points (" + (100f * both / n).ToString("F0") + " percent; bar 80)\n");
}
return (allOk ? "ALL PASS" : "FAILS") + ": the forest bars (ForestPlan 8.1)\n" + sb;
