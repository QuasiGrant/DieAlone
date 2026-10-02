// Main3 task 8.21a, camp stops nobody can stand on (Wren 2026-10-02; the 8.22 round 2 flood found 28 camp places on Hedge_Burn_0, 1 and 3,
// then 2 on Hedge_Burn_2, and one on the tower's Flight2 RailStop). Edit mode, Main3; rerunnable (every height is from the ground or the stairs as they are). In
// the runner after main3_8_22_front.cs.
// 1. Hedges, the brush band method (8.22 round 2): each HedgeCollider box of the listed hedges stands at least bandH m over the highest
//    walkable surface within reachOut m of it, any collider face under the controller's slope limit (trees, the cabin and the tower left
//    out: nobody stands there) as well as the terrain (8.21b: the
//    Ground815/Pockets fills beside Hedge_Burn_0 and 1 were missed), from 8.15's height (hedgeColH over the higher end of its segment).
//    Only the top moves; the brush over each hedge stays the visible stop.
// 2. Tower stairs: a flight's RailStop had a flat top part-way up the flight above it, and its 0.1 m top could be stood on. Each RailStop
//    now runs up to the bottom of the next RailStop in the same lane (flights stack every four), so the lane edges are one wall from the
//    first flight to the top flight and no stop has a top within reach. The handrails stay the visible stop.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
const float bandH = 2.0f, reachOut = 2.5f, probeUp = 8f, sample = 0.25f, sameLane = 0.9f, hedgeColH = 1.8f, buried = 0.5f, segPad = 0.3f;   // hedgeColH, buried, segPad: 8.15's hedge box (main3_8_15_ground.cs)
var playerCC = Root("Player") != null ? Root("Player").GetComponentInChildren<UnityEngine.CharacterController>() : null; if (playerCC == null) return "no Player CharacterController"; float slopeLimit = playerCC.slopeLimit; float minUp = UnityEngine.Mathf.Cos(slopeLimit * UnityEngine.Mathf.Deg2Rad);   // reachOut: a sprint-jump's reach; probeUp: over the tallest pocket fill
// the hedges: every Ground815/Stops hedge with a box within hedgeMargin m of the camp area's bounds (8.21b: the finer flood stood on
// Hedge_Burn_4 once 0 to 3 were fixed; a fixed list missed it)
const float hedgeMargin = 15f; var hedgeList = new System.Collections.Generic.List<string>();
{ var campA = Main3AreaSet.Load() != null ? Main3AreaSet.Load().Find("camp") : null; var st0 = Root("Ground815") != null ? Root("Ground815").transform.Find("Stops") : null;
  if (campA != null && st0 != null) foreach (UnityEngine.Transform hh in st0) { if (!hh.name.StartsWith("Hedge")) continue; bool near = false; foreach (var bc in hh.GetComponentsInChildren<UnityEngine.BoxCollider>()) foreach (var r in campA.bounds) { var e = new UnityEngine.Rect(r.x - hedgeMargin, r.y - hedgeMargin, r.width + 2f * hedgeMargin, r.height + 2f * hedgeMargin); if (e.Contains(new UnityEngine.Vector2(bc.bounds.center.x, bc.bounds.center.z))) near = true; } if (near) hedgeList.Add(hh.name); } }
var hedges = hedgeList.ToArray();
var notes = new System.Collections.Generic.List<string>();
var TreeName = new System.Text.RegularExpressions.Regex("RedFir|RedPine|Sequoia|Tree_Dead|Sapling");   // tree meshes and trunks anywhere (Camp/Dressing/EdgeTrees, SliceLook/BurnRegrowth)
// ---- 1. hedges
UnityEngine.Physics.SyncTransforms();   // an eval job's physics scene holds the colliders only after this (PHYSICS IN RUNNER JOBS)
var stops = Root("Ground815") != null ? Root("Ground815").transform.Find("Stops") : null; int raised = 0, boxes = 0; float most = 0f;
if (stops == null) notes.Add("no Ground815/Stops");
else foreach (var hn in hedges)
{
    var h = stops.Find(hn); if (h == null) { notes.Add("no " + hn); continue; }
    foreach (var bc in h.GetComponentsInChildren<UnityEngine.BoxCollider>())
    {
        if (bc.name != "HedgeCollider") continue; boxes++; var t = bc.transform;
        // 8.15's box as built: from 0.5 m under the lower end of its segment to hedgeColH over the higher end (recomputed, so a rerun sets
        // the same height and never stacks)
        float segLen = bc.size.z - segPad; var ea = t.position - t.forward * segLen * 0.5f; var eb = t.position + t.forward * segLen * 0.5f;
        float gTop = UnityEngine.Mathf.Max(H(ea.x, ea.z), H(eb.x, eb.z)), gLow = UnityEngine.Mathf.Min(H(ea.x, ea.z), H(eb.x, eb.z)), bottom = gLow - buried, top815 = gTop + hedgeColH;
        // the highest walkable surface (8.21b, Marlow round 2: terrain alone missed Ground815/Pockets/CampStep beside Hedge_Burn_1 and
        // TrenchWest beside Hedge_Burn_0): every collider face under the slope limit within reachOut m, terrain included; hedge and wall
        // boxes (Ignore Raycast), tree trunks (Forest) and the rock bands, which nobody stands on, left out
        float maxG = float.MinValue;
        for (float a = -bc.size.z * 0.5f - reachOut; a <= bc.size.z * 0.5f + reachOut + 1e-3f; a += sample)
            for (float b = -bc.size.x * 0.5f - reachOut; b <= bc.size.x * 0.5f + reachOut + 1e-3f; b += sample)
            {
                var p = t.TransformPoint(new UnityEngine.Vector3(b, 0f, a));
                foreach (var hit in UnityEngine.Physics.RaycastAll(new UnityEngine.Vector3(p.x, H(p.x, p.z) + probeUp, p.z), UnityEngine.Vector3.down, probeUp + 1f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    var ht = hit.collider.transform; if (hit.collider.gameObject.layer == 2 || hit.normal.y < minUp) continue; var path = WalkIns.PathOf(ht);
                    if (path.StartsWith("Forest") || path.StartsWith("Rock") || path.StartsWith("Player") || path.StartsWith("Camp/Cabin") || path.StartsWith("Camp/Tower") || hit.collider is UnityEngine.CapsuleCollider || TreeName.IsMatch(path)) continue;   // trees and the cabin and tower roofs: nobody stands there
                    if (hit.point.y > maxG) { maxG = hit.point.y; }
                }
            }
        float want = UnityEngine.Mathf.Max(top815, maxG + bandH); if (want > top815 + 1e-3f) { raised++; most = UnityEngine.Mathf.Max(most, want - top815); }
        var local = t.InverseTransformPoint(new UnityEngine.Vector3(t.position.x, (bottom + want) * 0.5f, t.position.z));
        bc.center = new UnityEngine.Vector3(0f, local.y, 0f); bc.size = new UnityEngine.Vector3(bc.size.x, want - bottom, bc.size.z);
    }
}
// ---- 1b. brush over every raised top (visible): along each hedge box every brushStep m, where no bush of the hedge reaches within
// brushSlack m of the box top, a Campsite bush as tall as the box top plus brushOver m; rebuilt under each hedge's Brush821a each run
const float brushStep = 1.4f, brushSlack = 0.2f, brushOver = 0.3f, bushWide = 2.5f, warpClear = 2.5f, lowProp = 1.5f;   // bushWide: 8.15's widest hedge bush
string[] bushNames = { "CS_Bush_Large_1", "CS_Bush_Large_1_1", "CS_Bush_Large_1_2", "CS_Bush_Large_1_3", "CS_Bush_Large_1_4", "CS_Bush_Large_2", "CS_Bush_Large_2_1", "CS_Bush_Large_2_2", "CS_Bush_Large_2_3", "CS_Bush_Large_2_4" };
int added = 0, skipped = 0; var brng = new System.Random(8211); var warps = Root("DevWarps").transform;
if (stops != null) foreach (var hn in hedges)
{
    var h = stops.Find(hn); if (h == null) continue; var old = h.Find("Brush821a"); if (old != null) UnityEngine.Object.DestroyImmediate(old.gameObject);
    var bushes = new System.Collections.Generic.List<UnityEngine.Bounds>(); foreach (var r in h.GetComponentsInChildren<UnityEngine.Renderer>()) if (r.name.StartsWith("CS_Bush")) bushes.Add(r.bounds);
    var dress = new UnityEngine.GameObject("Brush821a").transform; dress.SetParent(h, false);
    foreach (var bc in h.GetComponentsInChildren<UnityEngine.BoxCollider>())
    {
        if (bc.name != "HedgeCollider") continue; var t = bc.transform; float boxTop = bc.bounds.max.y; int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.RoundToInt(bc.size.z / brushStep));
        for (int i = 0; i <= n; i++)
        {
            var p = t.TransformPoint(new UnityEngine.Vector3(0f, 0f, -bc.size.z * 0.5f + bc.size.z * i / n)); float cover = float.MinValue;
            foreach (var b in bushes) if (p.x >= b.min.x && p.x <= b.max.x && p.z >= b.min.z && p.z <= b.max.z) cover = UnityEngine.Mathf.Max(cover, b.max.y);
            if (cover >= boxTop - brushSlack) continue;
            var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/" + PlaceKit.CS + "Vegetation/" + bushNames[brng.Next(bushNames.Length)] + ".prefab"); if (src == null) { notes.Add("no Campsite bush prefab"); break; }
            var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src, dress); foreach (var c in g.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
            g.transform.rotation = UnityEngine.Quaternion.Euler(0f, (float)brng.NextDouble() * 360f, 0f); float gy = H(p.x, p.z);
            var mb = PlaceKit.MeshBounds(g); g.transform.localScale *= (boxTop + brushOver - gy) / UnityEngine.Mathf.Max(0.2f, mb.size.y); mb = PlaceKit.MeshBounds(g);
            // only as wide as a hedge bush (8.21b: an 8 m bush scaled evenly reached into the cabin and over warps)
            float wide = UnityEngine.Mathf.Max(mb.size.x, mb.size.z); if (wide > bushWide) { var ls = g.transform.localScale; g.transform.localScale = new UnityEngine.Vector3(ls.x * bushWide / wide, ls.y, ls.z * bushWide / wide); mb = PlaceKit.MeshBounds(g); }
            g.transform.position += new UnityEngine.Vector3(p.x - mb.center.x, gy - 0.1f - mb.min.y, p.z - mb.center.z); mb = PlaceKit.MeshBounds(g);
            // none over a warp's view or into a wall or building (lowProp m or taller); low props and rocks may stand in the brush
            bool clash = false; foreach (UnityEngine.Transform w in warps) if (new UnityEngine.Vector2(w.position.x - p.x, w.position.z - p.z).magnitude < warpClear + mb.extents.x) clash = true;
            if (!clash) foreach (var c in UnityEngine.Physics.OverlapBox(mb.center, mb.extents, UnityEngine.Quaternion.identity, ~(1 << 2), UnityEngine.QueryTriggerInteraction.Ignore)) { if (c is UnityEngine.TerrainCollider) continue; var cp = WalkIns.PathOf(c.transform); if (cp.StartsWith("Forest") || cp.StartsWith("Ground815/Stops") || c.bounds.max.y - gy < lowProp) continue; clash = true; break; }   // low props and rocks may stand in the brush
            if (clash) { UnityEngine.Object.DestroyImmediate(g); skipped++; continue; }
            bushes.Add(mb); added++;
        }
    }
}
// ---- 2. tower stair rail stops
var stairs = Root("Camp") != null ? Root("Camp").transform.Find("Tower/Stairs") : null; int joined = 0, railStops = 0;
if (stairs == null) notes.Add("no Camp/Tower/Stairs");
else
{
    var all = new System.Collections.Generic.List<UnityEngine.BoxCollider>(); foreach (var bc in stairs.GetComponentsInChildren<UnityEngine.BoxCollider>()) if (bc.name == "RailStop") all.Add(bc);
    railStops = all.Count; UnityEngine.Physics.SyncTransforms();
    var orig = new System.Collections.Generic.Dictionary<UnityEngine.BoxCollider, UnityEngine.Bounds>(); foreach (var bc in all) orig[bc] = bc.bounds;
    float Overlap(UnityEngine.Bounds a, UnityEngine.Bounds b) { float ox = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Min(a.max.x, b.max.x) - UnityEngine.Mathf.Max(a.min.x, b.min.x)), oz = UnityEngine.Mathf.Max(0f, UnityEngine.Mathf.Min(a.max.z, b.max.z) - UnityEngine.Mathf.Max(a.min.z, b.min.z)); float area = UnityEngine.Mathf.Max(1e-4f, a.size.x * a.size.z); return ox * oz / area; }
    foreach (var bc in all)
    {
        var a = orig[bc]; UnityEngine.BoxCollider above = null; float aboveY = float.MaxValue;
        foreach (var o in all) { if (o == bc) continue; var b = orig[o]; if (b.min.y <= a.min.y + 0.1f || Overlap(a, b) < sameLane) continue; if (b.min.y < aboveY) { aboveY = b.min.y; above = o; } }
        if (above == null || aboveY <= a.max.y + 1e-3f) continue;
        var t = bc.transform; var c = bc.center; var local = t.InverseTransformPoint(new UnityEngine.Vector3(a.center.x, (a.min.y + aboveY) * 0.5f, a.center.z));
        float scaleY = UnityEngine.Mathf.Abs(t.lossyScale.y) > 1e-4f ? t.lossyScale.y : 1f;
        bc.center = new UnityEngine.Vector3(c.x, local.y, c.z); bc.size = new UnityEngine.Vector3(bc.size.x, (aboveY - a.min.y) / scaleY, bc.size.z); joined++;
    }
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | hedges " + string.Join(", ", hedges) + " | hedge boxes " + boxes + ", raised " + raised + " (most " + F(most) + " m) to " + F(bandH) + " m over the highest ground within " + F(reachOut) + " m | bushes added over raised tops " + added + " (" + skipped + " left out by a warp or a solid) | rail stops " + railStops + ", joined to the stop above " + joined + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes));
