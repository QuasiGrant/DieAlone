// Main3 task 8.21a, camp stops nobody can stand on (Wren 2026-10-02; the 8.22 round 2 flood found 28 camp places on Hedge_Burn_0, 1 and 3,
// then 2 on Hedge_Burn_2, and one on the tower's Flight2 RailStop). Edit mode, Main3; rerunnable (every height is from the ground or the stairs as they are). In
// the runner after main3_8_22_front.cs.
// 1. Hedges, the brush band method (8.22 round 2): each HedgeCollider box of the listed hedges stands at least bandH m over the highest
//    walkable ground (under the controller's slope limit) within reachOut m of it, from 8.15's height (hedgeColH over the higher end of
//    its segment). Only the top moves; the brush over each hedge stays the visible stop.
// 2. Tower stairs: a flight's RailStop had a flat top part-way up the flight above it, and its 0.1 m top could be stood on. Each RailStop
//    now runs up to the bottom of the next RailStop in the same lane (flights stack every four), so the lane edges are one wall from the
//    first flight to the top flight and no stop has a top within reach. The handrails stay the visible stop.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
const float bandH = 2.0f, reachOut = 0.5f, sample = 0.25f, sameLane = 0.9f, hedgeColH = 1.8f, buried = 0.5f, segPad = 0.3f;   // hedgeColH, buried, segPad: 8.15's hedge box (main3_8_15_ground.cs)
var playerCC = Root("Player") != null ? Root("Player").GetComponentInChildren<UnityEngine.CharacterController>() : null; if (playerCC == null) return "no Player CharacterController"; float slopeLimit = playerCC.slopeLimit;
string[] hedges = { "Hedge_Burn_0", "Hedge_Burn_1", "Hedge_Burn_2", "Hedge_Burn_3" };   // Hedge_Burn_2: the first fix left two camp places on it
var notes = new System.Collections.Generic.List<string>();
// ---- 1. hedges
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
        // the highest walkable ground (under the player's slope limit) within reachOut m of the box
        float maxG = float.MinValue;
        for (float a = -bc.size.z * 0.5f - reachOut; a <= bc.size.z * 0.5f + reachOut + 1e-3f; a += sample)
            for (float b = -bc.size.x * 0.5f - reachOut; b <= bc.size.x * 0.5f + reachOut + 1e-3f; b += sample)
            {
                var p = t.TransformPoint(new UnityEngine.Vector3(b, 0f, a)); var d = ter.terrainData; var o = ter.transform.position;
                if (d.GetSteepness((p.x - o.x) / d.size.x, (p.z - o.z) / d.size.z) >= slopeLimit) continue; maxG = UnityEngine.Mathf.Max(maxG, H(p.x, p.z));
            }
        float want = UnityEngine.Mathf.Max(top815, maxG + bandH); if (want > top815 + 1e-3f) { raised++; most = UnityEngine.Mathf.Max(most, want - top815); }
        var local = t.InverseTransformPoint(new UnityEngine.Vector3(t.position.x, (bottom + want) * 0.5f, t.position.z));
        bc.center = new UnityEngine.Vector3(0f, local.y, 0f); bc.size = new UnityEngine.Vector3(bc.size.x, want - bottom, bc.size.z);
    }
}
// ---- 1b. brush over every raised top (visible): along each hedge box every brushStep m, where no bush of the hedge reaches within
// brushSlack m of the box top, a Campsite bush as tall as the box top plus brushOver m; rebuilt under each hedge's Brush821a each run
const float brushStep = 1.4f, brushSlack = 0.2f, brushOver = 0.3f;
string[] bushNames = { "CS_Bush_Large_1", "CS_Bush_Large_1_1", "CS_Bush_Large_1_2", "CS_Bush_Large_1_3", "CS_Bush_Large_1_4", "CS_Bush_Large_2", "CS_Bush_Large_2_1", "CS_Bush_Large_2_2", "CS_Bush_Large_2_3", "CS_Bush_Large_2_4" };
int added = 0; var brng = new System.Random(8211);
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
            g.transform.position += new UnityEngine.Vector3(p.x - mb.center.x, gy - 0.1f - mb.min.y, p.z - mb.center.z); bushes.Add(PlaceKit.MeshBounds(g)); added++;
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
return "saved=" + saved + " | hedge boxes " + boxes + ", raised " + raised + " (most " + F(most) + " m) to " + F(bandH) + " m over the highest ground within " + F(reachOut) + " m | bushes added over raised tops " + added + " | rail stops " + railStops + ", joined to the stop above " + joined + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes));
