// Main3 reach check (Play mode; 8.16a, Marlow's 50 m rule, Gate_816_Marlow.md 18 and 19; Valley.md 9.2). Run in Main3 after the runner,
// by Tools/Recipes/main3_review_capture.sh with the other Play checks; never saves the scene. Day gates stay as they are (by day the
// Ward path is closed). Method as Marlow's: a flood fill over a cell m grid from every trail point, each move a sprint with the real
// mover (PlayerController.Step, dt 0.02) from where the player settled in one cell toward the centre of a neighbour; a neighbour is
// reached when the player ends within arrive m of its centre. Then, for every reached cell:
// - FAR: more than farLimit m from every trail centre point or front-zone road point (Valley.md 9.2; the roads are followed like trails).
// - LOST: FAR, and from the eye (1.6 m) no line to the tower cab and none to any trail or road point within seeTrail m. Lines are blocked by
//   every collider and by the drawn trees (temporary MeshColliders on each tree's first LOD, removed before it returns; alpha-cut
//   foliage counts as solid, so LOST is overstated, never understated).
// PASS: no LOST cell (Wren's fix list 2026-09-30: fix the far cells where a player loses sight of both the tower and the trails).
// FAR cells are listed by area with the worst one, for the reviewers.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, cell = 4f, arrive = 1.5f, farLimit = 50f, seeTrail = 60f, eye = 1.6f, x0 = -20f, x1 = 400f, z0 = -60f, z1 = 360f;
const int moveSteps = 120, stallSteps = 25;
var inv = System.Globalization.CultureInfo.InvariantCulture;
int NX = UnityEngine.Mathf.CeilToInt((x1 - x0) / cell), NZ = UnityEngine.Mathf.CeilToInt((z1 - z0) / cell);
float CX(int i) => x0 + (i + 0.5f) * cell; float CZ(int j) => z0 + (j + 0.5f) * cell;
var at = new UnityEngine.Vector3?[NX, NZ];   // where the player settled in each reached cell
var from = new (int, int)[NX, NZ];   // the cell each one was first reached from
// ground Valley.md 8 closes with thicket: no walker may reach it (the entering move is printed, so a leak shows where it is)
var closedZones = new (string n, UnityEngine.Rect r)[] { ("SE corner", new UnityEngine.Rect(346f, -10f, 49f, 109f)), ("lake south shore", new UnityEngine.Rect(141f, -10f, 107f, 35f)) };
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform leg in Root("Trails").transform) foreach (UnityEngine.Transform p in leg) trailPts.Add(p.position);
var cab = Root("Camp").transform.Find("Tower/Cab"); if (cab == null) return "no Camp/Tower/Cab";
// the front zone's roads count as paths (Wren's fix list: the lot, drive, spur and campground loop are followed like trails): points
// every roadStep m over each road piece's own mesh bounds
const float roadStep = 4f; var pathPts = new System.Collections.Generic.List<UnityEngine.Vector3>(trailPts); int roadPts = 0;
foreach (var mf in Root("FrontZone").GetComponentsInChildren<UnityEngine.MeshFilter>(true))
{
    var n = mf.name; if (!(n == "ParkingLot" || n == "Drive" || n == "Road" || n == "Drive_To_T" || n == "TurningCircle" || n.StartsWith("Spur") || n.StartsWith("Loop")) || mf.sharedMesh == null) continue;
    var lb = mf.sharedMesh.bounds; var sc = mf.transform.lossyScale; float sx = UnityEngine.Mathf.Max(1f, lb.size.x * UnityEngine.Mathf.Abs(sc.x) / roadStep), sz = UnityEngine.Mathf.Max(1f, lb.size.z * UnityEngine.Mathf.Abs(sc.z) / roadStep);
    for (float a = 0f; a <= 1f; a += 1f / sx) for (float b = 0f; b <= 1f; b += 1f / sz) { pathPts.Add(mf.transform.TransformPoint(new UnityEngine.Vector3(lb.min.x + lb.size.x * a, lb.max.y, lb.min.z + lb.size.z * b))); roadPts++; }
}
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.05f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
bool Sprint(float x, float z)
{
    var t = new UnityEngine.Vector2(x, z); float best = float.MaxValue; int since = 0;
    for (int s = 0; s < moveSteps; s++)
    {
        var p = pc.transform.position; var d = t - new UnityEngine.Vector2(p.x, p.z); if (d.magnitude < arrive * 0.5f) break;
        if (d.magnitude < best - 0.02f) { best = d.magnitude; since = 0; } else if (++since > stallSteps) break;
        pc.Step(new UnityEngine.Vector3(d.x, 0f, d.y).normalized, false, true, dt);
    }
    for (int k = 0; k < 10; k++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);   // land
    var e = pc.transform.position; return new UnityEngine.Vector2(e.x - x, e.z - z).magnitude < arrive;
}
var temp = new System.Collections.Generic.List<UnityEngine.Collider>();
var sb = new System.Text.StringBuilder(); bool pass = true;
try
{
    var q = new System.Collections.Generic.Queue<(int, int)>();
    foreach (var p in pathPts)
    {
        int i = UnityEngine.Mathf.FloorToInt((p.x - x0) / cell), j = UnityEngine.Mathf.FloorToInt((p.z - z0) / cell);
        if (i < 0 || j < 0 || i >= NX || j >= NZ || at[i, j].HasValue) continue; at[i, j] = p; q.Enqueue((i, j));
    }
    int moves = 0; var n4 = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
    while (q.Count > 0)
    {
        var (i, j) = q.Dequeue();
        foreach (var (di, dj) in n4)
        {
            int a = i + di, b = j + dj; if (a < 0 || b < 0 || a >= NX || b >= NZ || at[a, b].HasValue) continue;
            Put(at[i, j].Value); moves++;
            if (Sprint(CX(a), CZ(b))) { at[a, b] = pc.transform.position; from[a, b] = (i, j); q.Enqueue((a, b)); }
        }
    }
    // the drawn trees block sight lines for the LOST test
    foreach (var root in new[] { Root("Forest"), Root("SliceLook"), Root("Ground815") }) if (root != null)
        foreach (var lod in root.GetComponentsInChildren<UnityEngine.LODGroup>())
        {
            var lods = lod.GetLODs(); if (lods.Length == 0) continue;
            foreach (var r in lods[0].renderers) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null || r.GetComponent<UnityEngine.Collider>() != null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temp.Add(mc); }
        }
    UnityEngine.Physics.SyncTransforms();
    var cabAt = cab.position + UnityEngine.Vector3.up * 1.5f;
    bool Clear(UnityEngine.Vector3 a, UnityEngine.Vector3 b) { var d = b - a; return !UnityEngine.Physics.Raycast(a, d.normalized, d.magnitude - 1f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore); }
    int reached = 0, far = 0, lost = 0; float worstD = 0f; string worst = "";
    var areas = new System.Collections.Generic.SortedDictionary<string, (int n, int lost, float worst, UnityEngine.Vector2 at)>();
    var lostList = new System.Collections.Generic.List<string>();
    string Area(float x, float z) => x >= 340f ? "front and east of x 340" : z < 50f ? (x < 140f ? "SW, z under 50" : x < 250f ? "lake south shore, z under 50" : "SE, z under 50") : z > 250f ? (x < 180f ? "NW, z over 250" : "NE, z over 250") : "mid";
    for (int i = 0; i < NX; i++) for (int j = 0; j < NZ; j++)
    {
        if (!at[i, j].HasValue) continue; reached++;
        var p = at[i, j].Value; float d = float.MaxValue; foreach (var t in pathPts) d = UnityEngine.Mathf.Min(d, new UnityEngine.Vector2(t.x - p.x, t.z - p.z).magnitude);
        if (d <= farLimit) continue; far++;
        var e = p + UnityEngine.Vector3.up * eye; bool sees = Clear(e, cabAt);
        if (!sees) foreach (var t in pathPts) { if (new UnityEngine.Vector2(t.x - p.x, t.z - p.z).magnitude > seeTrail) continue; if (Clear(e, t + UnityEngine.Vector3.up * 0.2f)) { sees = true; break; } }
        string ar = Area(p.x, p.z); areas.TryGetValue(ar, out var v);
        if (!sees) { lost++; lostList.Add("(" + p.x.ToString("F0", inv) + ", " + p.z.ToString("F0", inv) + ") " + d.ToString("F0", inv) + " m"); }
        areas[ar] = (v.n + 1, v.lost + (sees ? 0 : 1), UnityEngine.Mathf.Max(v.worst, d), d > v.worst ? new UnityEngine.Vector2(p.x, p.z) : v.at);
        if (d > worstD) { worstD = d; worst = "(" + p.x.ToString("F0", inv) + ", " + p.z.ToString("F0", inv) + ")"; }
    }
    int closedIn = 0;
    foreach (var zc in closedZones)
    {
        int n = 0; string first = "";
        for (int i = 0; i < NX; i++) for (int j = 0; j < NZ; j++) if (at[i, j].HasValue && zc.r.Contains(new UnityEngine.Vector2(CX(i), CZ(j)))) { n++; if (first == "") { var (fi, fj) = from[i, j]; first = " first (" + CX(i).ToString("F0", inv) + ", " + CZ(j).ToString("F0", inv) + ") from (" + CX(fi).ToString("F0", inv) + ", " + CZ(fj).ToString("F0", inv) + ")"; } }
        closedIn += n; sb.Append("CLOSED " + zc.n + ": " + n + " cells reached" + first + ": " + (n == 0 ? "PASS" : "FAIL") + "\n");
    }
    pass = lost == 0 && closedIn == 0;
    sb.Append("REACH: " + moves + " sprint moves on a " + cell.ToString("F0", inv) + " m grid, " + reached + " cells reached, " + far + " over " + farLimit.ToString("F0", inv) + " m from a trail or road (" + roadPts + " road points; worst " + worstD.ToString("F0", inv) + " m at " + worst + "), " + lost + " of them LOST (no tower, no trail within " + seeTrail.ToString("F0", inv) + " m in sight): " + (pass ? "PASS" : "FAIL") + "\n");
    foreach (var kv in areas) sb.Append("  " + kv.Key + ": " + kv.Value.n + " far, " + kv.Value.lost + " lost, worst " + kv.Value.worst.ToString("F0", inv) + " m at (" + kv.Value.at.x.ToString("F0", inv) + ", " + kv.Value.at.y.ToString("F0", inv) + ")\n");
    if (lostList.Count > 0) sb.Append("  LOST cells: " + string.Join("; ", lostList) + "\n");
}
finally
{
    foreach (var c in temp) if (c != null) UnityEngine.Object.Destroy(c);
    pc.enabled = true;
}
return sb.ToString() + (pass ? "ALL PASS" : "SOME FAIL");
