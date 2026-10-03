// Main3 8.25 search (edit mode, Main3; Wren's calls on the 8.25 fails, 2026-10-02). Never saves; removes its temporary colliders.
// S1 CHAIR: his top chair slid along the top rail (its centre chairInset m inside the rail's faces, the faces 4.18 m from the stack centre,
//   bearings every chairStep degrees, not on the landing mouth's face nor by the tent): from today's chair and each spot, seated (eye seatEye
//   over the top), the line to the T signpost's top past the terrain, every drawn collider and every drawn mesh (temporary exact colliders);
//   the nearest clear spot by arc. HIGHWAY FROM THE TOP: stand spots on the top floor that see a highway point.
// PS3: the talus within ps3Move m of PS3 (285.7, 109.9), every ps3Grid m, on the highest standable surface under it (no stack top, no
//   invisible box): the nearest spot found by Pim's rule (Main3AreaSet.Found) from a trail or a camp2 walk line, the paper's ps3H m tall.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
const float sx = 292f, sz = 108f, topY = 24f, face = 4.18f, chairInset = 0.58f, chairStep = 2f, seatEye = 1.2f, highwayX = 430f;
const float ps3X = 285.7f, ps3Z = 109.9f, ps3Move = 5f, ps3Grid = 0.5f, ps3H = 0.3f, stackTopMin = 20f;
var c2 = Root("Campsites").transform.Find("Camp_2"); var chair = c2.Find("StackTop/Layout825/HisChair"); var ps3 = c2.Find("Layout825/PaperSpots/PS3");
if (chair == null || ps3 == null) return "run main3_8_25_camp2.cs first";
var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var sb = new System.Text.StringBuilder();
bool Drawn(UnityEngine.Collider c) => c is UnityEngine.TerrainCollider || c.GetComponent<UnityEngine.Renderer>() != null;
try
{
    // ---- S1 CHAIR (Wren 2026-10-03): the T line is judged to the top of the T signpost (Ground815/JunctionMarkers/Trailhead_Board, its
    // drawn top less signDrop); the highway from at least one stand spot on the top floor (every standStep m within standR of the centre, eye
    // standEye), to highway points (highwayX, ground + 1, each of highwayZs)
    const float signDrop = 0.05f, standStep = 0.5f, standR = 3.8f, standEye = 1.6f; float[] highwayZs = { 150f, 160f, 170f, 180f, 185f, 190f, 200f, 210f };
    float BearingOf(UnityEngine.Vector3 p) => (UnityEngine.Mathf.Atan2(p.x - sx, p.z - sz) * UnityEngine.Mathf.Rad2Deg + 360f) % 360f;
    UnityEngine.Vector3 OnFace(float b)   // the point at bearing b, chairInset inside the octagon's face (faces centred on 22.5 + k*45)
    {
        float fc = 22.5f + UnityEngine.Mathf.Round((b - 22.5f) / 45f) * 45f; float r = (face - chairInset) / UnityEngine.Mathf.Cos((b - fc) * UnityEngine.Mathf.Deg2Rad);
        return V(sx + UnityEngine.Mathf.Sin(b * UnityEngine.Mathf.Deg2Rad) * r, topY, sz + UnityEngine.Mathf.Cos(b * UnityEngine.Mathf.Deg2Rad) * r);
    }
    var sign = Root("Ground815").transform.Find("JunctionMarkers/Trailhead_Board"); if (sign == null) return "no Ground815/JunctionMarkers/Trailhead_Board";
    var sbnd = PlaceKit.MeshBounds(sign.gameObject); var tPt = V(sbnd.center.x, sbnd.max.y - signDrop, sbnd.center.z);
    var cb0 = chair.GetComponent<UnityEngine.Collider>().bounds; var nowEye = V(cb0.center.x, topY + seatEye, cb0.center.z); float now = BearingOf(nowEye);
    var cands = new System.Collections.Generic.List<(float b, UnityEngine.Vector3 eye)>();
    for (float b = 0f; b < 360f; b += chairStep)
    {
        if ((b > 95f && b < 135f) || (b > 200f && b < 340f)) continue;   // the landing mouth's face; the tent, the lamp and the letters west
        cands.Add((b, OnFace(b) + UnityEngine.Vector3.up * seatEye));
    }
    var stands = new System.Collections.Generic.List<UnityEngine.Vector3>();
    for (float x = sx - standR; x <= sx + standR + 1e-3f; x += standStep) for (float z = sz - standR; z <= sz + standR + 1e-3f; z += standStep) if (new UnityEngine.Vector2(x - sx, z - sz).magnitude <= standR) stands.Add(V(x, topY + standEye, z));
    var hws = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (var hz in highwayZs) hws.Add(V(highwayX, H(highwayX, hz) + 1f, hz));
    var rays = new System.Collections.Generic.List<(UnityEngine.Vector3 a, UnityEngine.Vector3 b)> { (nowEye, tPt) }; foreach (var c in cands) rays.Add((c.eye, tPt)); foreach (var s in stands) foreach (var h in hws) rays.Add((s, h));
    var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(chair)) continue;
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; bool on = false;
        foreach (var (a, b) in rays) { var d = b - a; if (mr.bounds.IntersectRay(new UnityEngine.Ray(a, d.normalized), out float dist) && dist <= d.magnitude) { on = true; break; } }
        if (!on) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
    string First(UnityEngine.Vector3 a, UnityEngine.Vector3 b)
    {
        var d = b - a; float best = float.MaxValue; string what = null;
        foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, d.magnitude - signDrop, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        { var t = h.collider.transform; if (t.IsChildOf(chair) || t.IsChildOf(sign) || t.gameObject.layer == 2 || !Drawn(h.collider)) continue; if (h.distance < best) { best = h.distance; what = WalkIns.PathOf(t) + " at " + F(h.distance) + " m"; } }
        return what;
    }
    float Arc(float b) { float d = UnityEngine.Mathf.Abs(b - now) % 360f; return d > 180f ? 360f - d : d; }
    var tBy = new System.Collections.Generic.SortedDictionary<string, int>(); void Count(System.Collections.Generic.SortedDictionary<string, int> by, string w) { if (w == null) return; var k = w.Substring(0, w.LastIndexOf(" at ")); by[k] = by.TryGetValue(k, out int n) ? n + 1 : 1; }
    string nowT = First(nowEye, tPt); (float b, UnityEngine.Vector3 eye)? tOnly = null; int nT = 0;
    foreach (var c in cands) { var ft = First(c.eye, tPt); Count(tBy, ft); if (ft == null) { nT++; if (!tOnly.HasValue || Arc(c.b) < Arc(tOnly.Value.b)) tOnly = (c.b, c.eye); } }
    string Spot((float b, UnityEngine.Vector3 eye)? s) => s.HasValue ? "bearing " + F(s.Value.b) + " at (" + F(s.Value.eye.x) + ", " + F(s.Value.eye.z) + "), " + F(Arc(s.Value.b)) + " degrees round from today's" : "none";
    sb.Append("S1 T: to the signpost top (" + F(tPt.x) + ", " + F(tPt.y) + ", " + F(tPt.z) + "); from today's chair (bearing " + F(now) + "): " + (nowT ?? "clear") + "; slide spots clear " + nT + " of " + cands.Count + ", nearest " + Spot(tOnly) + "\n  first met: " + string.Join(", ", System.Linq.Enumerable.Select(tBy, kv => kv.Key + " x" + kv.Value)) + "\n");
    int hSpots = 0; string hEx = "none"; var hBy = new System.Collections.Generic.SortedDictionary<string, int>();
    foreach (var s in stands) { string clearTo = null; foreach (var h in hws) { var fh = First(s, h); if (fh == null) { clearTo = "(" + F(h.x) + ", " + F(h.z) + ")"; break; } Count(hBy, fh); } if (clearTo != null) { hSpots++; if (hEx == "none") hEx = "(" + F(s.x) + ", " + F(s.z) + ") to " + clearTo; } }
    sb.Append("HIGHWAY FROM THE TOP: " + hSpots + " of " + stands.Count + " stand spots see a highway point (x " + F(highwayX) + ", z " + string.Join("/", highwayZs) + "), e.g. " + hEx + "\n  first met when not: " + string.Join(", ", System.Linq.Enumerable.Select(hBy, kv => kv.Key + " x" + kv.Value)) + "\n");
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); temps.Clear(); UnityEngine.Physics.SyncTransforms();

    // ---- PS3
    var set = Main3AreaSet.Load(); var A = set.Find("camp2"); const float groundY = -900f;
    UnityEngine.Vector3 Pt(UnityEngine.Vector3 p) => p.y <= groundY ? V(p.x, H(p.x, p.z), p.z) : p;
    var legs = new System.Collections.Generic.List<(string leg, System.Collections.Generic.List<UnityEngine.Vector3> pts)>();
    foreach (UnityEngine.Transform leg in Root("Trails").transform)
    {
        var lp = new System.Collections.Generic.List<UnityEngine.Vector3>(); UnityEngine.Vector3? prev = null;
        foreach (UnityEngine.Transform pt in leg) { var qq = pt.position; if (prev.HasValue) { int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(prev.Value, qq) / 2f)); for (int i = 1; i <= n; i++) lp.Add(UnityEngine.Vector3.Lerp(prev.Value, qq, i / (float)n)); } else lp.Add(qq); prev = qq; }
        legs.Add((leg.name, lp));
    }
    foreach (var wl in A.walkLines)
    {
        var lp = new System.Collections.Generic.List<UnityEngine.Vector3>();
        for (int k = 0; k < wl.points.Length; k++) { var qq = Pt(wl.points[k]); if (k > 0) { var pv = Pt(wl.points[k - 1]); int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(pv, qq) / 2f)); for (int i = 1; i <= n; i++) lp.Add(UnityEngine.Vector3.Lerp(pv, qq, i / (float)n)); } else lp.Add(qq); }
        legs.Add(("walk: " + wl.label, lp));
    }
    foreach (var root in new[] { Root("Forest"), Root("SliceLook"), Root("Ground815") }) if (root != null)
        foreach (var lod in root.GetComponentsInChildren<UnityEngine.LODGroup>())
        { var lods = lod.GetLODs(); if (lods.Length == 0) continue; foreach (var r in lods[0].renderers) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null || r.GetComponent<UnityEngine.Collider>() != null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); } }
    UnityEngine.Physics.SyncTransforms();
    bool ClearTo(UnityEngine.Vector3 a, UnityEngine.Vector3 b)
    {
        var d = b - a; float len = d.magnitude - 0.05f; if (len <= 0f) return true;   // as the area check with an own object: 0.05 m short (rayEndSkip let a boulder at the paper pass)
        foreach (var h in UnityEngine.Physics.RaycastAll(a, d.normalized, len, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) if (!h.collider.transform.IsChildOf(ps3)) return false;
        return true;
    }
    float bestD = float.MaxValue; string best = "none"; int tried = 0, found = 0;
    for (float x = ps3X - ps3Move; x <= ps3X + ps3Move + 1e-3f; x += ps3Grid) for (float z = ps3Z - ps3Move; z <= ps3Z + ps3Move + 1e-3f; z += ps3Grid)
    {
        float dd = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(x, z), new UnityEngine.Vector2(ps3X, ps3Z)); if (dd > ps3Move) continue;
        float y = float.MinValue; UnityEngine.Vector3 nrm = UnityEngine.Vector3.up; string on = "";
        foreach (var h in UnityEngine.Physics.RaycastAll(V(x, topY + 5f, z), UnityEngine.Vector3.down, 40f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        { if (h.collider.isTrigger || h.collider.gameObject.layer == 2 || !Drawn(h.collider) || temps.Contains(h.collider) || h.collider.transform.IsChildOf(ps3)) continue; if (h.point.y > y) { y = h.point.y; nrm = h.normal; on = WalkIns.PathOf(h.collider.transform); } }
        if (y == float.MinValue || y > stackTopMin || nrm.y < 0.7f) continue; tried++;
        var pl = new Main3AreaSet.Place { label = "PS3", point = V(x, y, z), height = ps3H };
        var fr = set.Found(pl, legs, (px, pz) => H(px, pz), ClearTo); if (!fr.found) continue; found++;
        if (dd < bestD) { bestD = dd; best = "(" + F(x) + ", " + F(y) + ", " + F(z) + ") on " + on + ", " + F(dd) + " m from today's, found from " + fr.leg + " " + F(fr.dist) + " m out, " + F(fr.angle) + " degrees off"; }
    }
    sb.Append("PS3: " + tried + " standable spots within " + F(ps3Move) + " m, found from a trail or walk line at " + found + "; nearest " + best + "\n");
}
finally { foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); UnityEngine.Physics.SyncTransforms(); }
return sb.ToString();
