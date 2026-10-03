// Main3 8.24a Camp 1 from the deck (edit mode, Main3; PLAN 8.24a, Grant's walk 2026-10-03; NorthLayout 5.4: "those two move 6 m outward round
// the ring and their spots become K"). The tent, the kid's table and the tripod are hard deck targets; this moves the trees on their lines
// until each is seen from a standing deck eye (the grid at deckEye and the rail eyes, as the area check's DECK). Rerunnable: a tree already
// off every line stays where it is. In the runner after main3_8_24_north.cs.
// Lines: from each standing eye to each target's mesh centre, against every collider (Ignore Raycast, the player and the target apart) and
// the drawn LOD0 meshes of the trees within treeNear m of the lines (temporary exact colliders, also on a tree whose trunk has a capsule).
// While a target is seen from no eye: of its lines blocked by trees alone, the one with the fewest trees; each of those trees moves to the
// first spot, in order of the smallest move, where it is on none of the lines to any target, its foot is not in a keep-out zone, and it
// stands treeSpace m or more from every other tree's foot: a Grove_C1Ring tree round the ring (ringStep degrees at a time, out to ringMax,
// both ways), any other tree square off the line (sideStep m at a time, out to sideMax, both sides). Up to maxRounds such rounds.
// The vacated spots of the ring trees are KeepOuts.Camp1 (C1 deck spots), so the forest recipes do not fill them again.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F1", inv);
var ter = UnityEngine.Terrain.activeTerrain; float G(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
const float treeNear = 15f, ringStep = 5f, ringMax = 60f, sideStep = 2f, sideMax = 12f, treeSpace = 3f; const int maxRounds = 8;
var ringCentre = P(282f, 238f);
var targetPaths = new[] { ("tent", "Campsites/Camp_1/Dressing/CS_Tent_Large_Modern_Preset_1"), ("kid's table", "Campsites/Camp_1/Dressing/KidTable"), ("tripod", "Campsites/Camp_1/Dressing/Cookfire") };
var set = Main3AreaSet.Load(); var tower = Root("Camp").transform.Find("Tower"); float deckTop = tower.Find("Cab").position.y; var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
var forest = Root("Forest") != null ? Root("Forest").transform : null; if (forest == null) return "no Forest";
// the standing eyes, as main3_area_check.cs
var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>(); float ey = deckTop + set.deckEye;
for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid) eyes.Add(V(tower.position.x + gx, ey, tower.position.z + gz));
{
    var rails = tower.Find("DeckRails"); UnityEngine.Bounds RB(string n) { var r = rails != null ? rails.Find(n) : null; var c = r != null ? r.GetComponent<UnityEngine.Collider>() : null; return c != null ? c.bounds : new UnityEngine.Bounds(); }
    var rw = RB("RailW"); var re = RB("RailE"); var rs = RB("RailS"); var rn = RB("RailN");
    if (rw.size != UnityEngine.Vector3.zero && re.size != UnityEngine.Vector3.zero && rs.size != UnityEngine.Vector3.zero && rn.size != UnityEngine.Vector3.zero)
    {
        float x0 = rw.max.x + set.railEyeInset, x1 = re.min.x - set.railEyeInset, z0 = rs.max.z + set.railEyeInset, z1 = rn.min.z - set.railEyeInset;
        for (float x = x0; x <= x1 + 1e-3f; x += set.railEyeStep) { eyes.Add(V(x, ey, z0)); eyes.Add(V(x, ey, z1)); }
        for (float z = z0 + set.railEyeStep; z < z1 - 1e-3f; z += set.railEyeStep) { eyes.Add(V(x0, ey, z)); eyes.Add(V(x1, ey, z)); }
    }
}
var targets = new System.Collections.Generic.List<(string label, UnityEngine.Transform own, UnityEngine.Vector3 at)>();
foreach (var (l, p) in targetPaths) { var t = Main3AreaSet.At(scene, p); if (t == null) return "no " + p; targets.Add((l, t, PlaceKit.MeshBounds(t.gameObject).center)); }
// a tree: the Forest transform holding a LOD group (one per tree)
UnityEngine.Transform TreeOf(UnityEngine.Transform t) { if (!t.IsChildOf(forest)) return null; for (var c = t; c != null && c != forest; c = c.parent) if (c.GetComponent<UnityEngine.LODGroup>() != null) return c; return null; }
float SegDist(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var moved = new System.Collections.Generic.List<string>(); var sb = new System.Text.StringBuilder();
var allTrees = new System.Collections.Generic.List<UnityEngine.Transform>(); foreach (var lod in forest.GetComponentsInChildren<UnityEngine.LODGroup>()) allTrees.Add(lod.transform);
try
{
    // temporary exact colliders on the LOD0 meshes of every tree whose foot is within treeNear m of some line (the lines' ground track)
    var c2 = P(tower.position.x, tower.position.z);
    foreach (var tr in allTrees)
    {
        var f2 = P(tr.position.x, tr.position.z); bool near = false; foreach (var tg in targets) if (SegDist(f2, c2, P(tg.at.x, tg.at.z)) <= treeNear) near = true; if (!near) continue;
        var lods = tr.GetComponent<UnityEngine.LODGroup>().GetLODs(); if (lods.Length == 0) continue;
        foreach (var r in lods[0].renderers) { if (r == null) continue; var mf = r.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
    }
    UnityEngine.Physics.SyncTransforms();
    // the blockers on one line: trees, and whether anything else blocks it too
    (System.Collections.Generic.HashSet<UnityEngine.Transform> trees, bool other) Blockers(UnityEngine.Vector3 e, (string label, UnityEngine.Transform own, UnityEngine.Vector3 at) tg)
    {
        var d = tg.at - e; var trees = new System.Collections.Generic.HashSet<UnityEngine.Transform>(); bool other = false;
        foreach (var h in UnityEngine.Physics.RaycastAll(e, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        { var ht = h.collider.transform; if (ht.gameObject.layer == 2 || ht.IsChildOf(pc.transform) || ht.IsChildOf(tg.own)) continue; var tree = TreeOf(ht); if (tree != null) trees.Add(tree); else other = true; }
        return (trees, other);
    }
    int Seen((string label, UnityEngine.Transform own, UnityEngine.Vector3 at) tg) { int n = 0; foreach (var e in eyes) { var b = Blockers(e, tg); if (b.trees.Count == 0 && !b.other) n++; } return n; }
    bool OnAnyLine(UnityEngine.Transform tree) { foreach (var tg in targets) foreach (var e in eyes) if (Blockers(e, tg).trees.Contains(tree)) return true; return false; }
    bool SpotOk(UnityEngine.Transform tree, UnityEngine.Vector2 q) { if (KeepOuts.Contains(q)) return false; foreach (var o in allTrees) if (o != tree && UnityEngine.Vector2.Distance(P(o.position.x, o.position.z), q) < treeSpace) return false; return true; }
    void Put(UnityEngine.Transform tree, UnityEngine.Vector2 q, float lift) { tree.position = V(q.x, G(q.x, q.y) + lift, q.y); UnityEngine.Physics.SyncTransforms(); }
    for (int round = 0; round < maxRounds; round++)
    {
        (string label, UnityEngine.Transform own, UnityEngine.Vector3 at) want = default; bool any = false; foreach (var tg in targets) if (Seen(tg) == 0) { want = tg; any = true; break; }
        if (!any) break;
        System.Collections.Generic.HashSet<UnityEngine.Transform> best = null; UnityEngine.Vector3 bestEye = default;
        foreach (var e in eyes) { var b = Blockers(e, want); if (b.other || b.trees.Count == 0) continue; if (best == null || b.trees.Count < best.Count) { best = b.trees; bestEye = e; } }
        if (best == null) { sb.Append(want.label + ": no line is blocked by trees alone; stopped\n"); break; }
        foreach (var tree in best)
        {
            var from = P(tree.position.x, tree.position.z); float lift = tree.position.y - G(from.x, from.y); bool done = false; bool ring = tree.parent != null && tree.parent.name == "Grove_C1Ring";
            var steps = new System.Collections.Generic.List<UnityEngine.Vector2>();
            if (ring) { var rel = from - ringCentre; float r0 = rel.magnitude, a0 = UnityEngine.Mathf.Atan2(rel.y, rel.x); for (float da = ringStep; da <= ringMax + 1e-3f; da += ringStep) foreach (var sgn in new[] { 1f, -1f }) { float a = a0 + sgn * da * UnityEngine.Mathf.Deg2Rad; steps.Add(ringCentre + P(UnityEngine.Mathf.Cos(a), UnityEngine.Mathf.Sin(a)) * r0); } }
            else { var dir = P(want.at.x - bestEye.x, want.at.z - bestEye.z).normalized; var side = P(-dir.y, dir.x); for (float s = sideStep; s <= sideMax + 1e-3f; s += sideStep) foreach (var sgn in new[] { 1f, -1f }) steps.Add(from + side * s * sgn); }
            foreach (var q in steps) { if (!SpotOk(tree, q)) continue; Put(tree, q, lift); if (!OnAnyLine(tree)) { done = true; moved.Add(WalkIns.PathOf(tree) + " from (" + F(from.x) + ", " + F(from.y) + ") to (" + F(q.x) + ", " + F(q.y) + "), " + F(UnityEngine.Vector2.Distance(from, q)) + " m"); break; } }
            if (!done) { Put(tree, from, lift); sb.Append(WalkIns.PathOf(tree) + ": no spot off every line within " + (ring ? F(ringMax) + " degrees round the ring" : F(sideMax) + " m") + "\n"); }
        }
    }
    foreach (var tg in targets) sb.Append(tg.label + ": seen from " + Seen(tg) + " of " + eyes.Count + " standing eyes\n");
}
finally { foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); UnityEngine.Physics.SyncTransforms(); }
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene); bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | moved " + moved.Count + (moved.Count > 0 ? ": " + string.Join("; ", moved) : "") + "\n" + sb;
