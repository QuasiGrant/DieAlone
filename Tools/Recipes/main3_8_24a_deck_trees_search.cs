// Main3 8.24a deck tree search (edit mode, Main3; PLAN 8.24a, Grant's walk 2026-10-03: the big-tent camp cannot be seen from the tower).
// Never saves; removes its temporary colliders. For each Camp 1 target (the tent, the kid's table, the tripod: their meshes' centres) and
// each standing deck eye (Main3AreaSet deckGrid over the deck at deckEye, plus the rail eyes railEyeInset inside each rail every
// railEyeStep m, as the area check's DECK): the line from eye to target against every collider (Ignore Raycast, the player and the target's
// own object apart) and every drawn LOD0 mesh it crosses (temporary exact colliders, also on meshes with a collider of their own: a tree's
// trunk capsule is not its crown), the tower's own included. Prints, per target: the
// standing eyes with a clear line; every blocker on the lines, with the lines it is on and the lines it alone blocks (lines whose only
// blockers are trees name the trees); and the fewest trees (greedy) whose moving opens one line, with their positions.
// A blocker is a tree when it sits under Forest/ or a tree root named in treeRoots.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F1", inv); string P3(UnityEngine.Vector3 p) => "(" + F(p.x) + ", " + F(p.y) + ", " + F(p.z) + ")";
var treeRoots = new[] { "Forest", "Giants", "SliceLook" };
var targetPaths = new[] { ("tent", "Campsites/Camp_1/Dressing/CS_Tent_Large_Modern_Preset_1"), ("kid's table", "Campsites/Camp_1/Dressing/KidTable"), ("tripod", "Campsites/Camp_1/Dressing/Cookfire") };
var set = Main3AreaSet.Load(); var tower = Root("Camp").transform.Find("Tower"); float deckTop = tower.Find("Cab").position.y; var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
// the standing eyes: the grid, then the rail eyes (as main3_area_check.cs)
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
string TreeOf(UnityEngine.Transform t) { var top = t; while (top.parent != null) top = top.parent; if (System.Array.IndexOf(treeRoots, top.name) < 0) return null; for (var c = t; c != null; c = c.parent) if (c.GetComponent<UnityEngine.LODGroup>() != null) return WalkIns.PathOf(c); return WalkIns.PathOf(t); }   // a tree: its LOD group's path, one name per tree
var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var sb = new System.Text.StringBuilder();
try
{
    // temporary exact colliders on the drawn LOD0 meshes (no collider of their own) that any line crosses
    var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
    var rays = new System.Collections.Generic.List<(UnityEngine.Ray r, float len)>(); foreach (var tg in targets) foreach (var e in eyes) { var d = tg.at - e; rays.Add((new UnityEngine.Ray(e, d.normalized), d.magnitude)); }
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.transform.IsChildOf(pc.transform) || mr.name.Contains("Glass")) continue;   // every drawn mesh, a collider of its own or not: a tree's trunk capsule is not its crown (8.24a: the first run named no tree)
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var bb = mr.bounds; bool crossed = false;
        foreach (var (r, l) in rays) if (bb.IntersectRay(r, out float dist) && dist <= l) { crossed = true; break; }
        if (!crossed) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
    foreach (var tg in targets)
    {
        int clear = 0; var onLines = new System.Collections.Generic.Dictionary<string, int>(); var only = new System.Collections.Generic.Dictionary<string, int>(); var where = new System.Collections.Generic.Dictionary<string, UnityEngine.Vector3>();
        var treeOnlyLines = new System.Collections.Generic.List<System.Collections.Generic.HashSet<string>>(); int otherBlocked = 0; var isTree = new System.Collections.Generic.HashSet<string>();
        foreach (var e in eyes)
        {
            var d = tg.at - e; var names = new System.Collections.Generic.HashSet<string>(); bool nonTree = false;
            foreach (var h in UnityEngine.Physics.RaycastAll(e, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
            {
                var ht = h.collider.transform; if (ht.gameObject.layer == 2 || ht.IsChildOf(pc.transform) || ht.IsChildOf(tg.own)) continue;
                var tree = TreeOf(ht); var n = tree ?? WalkIns.PathOf(ht); names.Add(n); if (!where.ContainsKey(n)) where[n] = tree != null ? h.collider.bounds.center : h.point; if (tree == null) nonTree = true; else isTree.Add(n);
            }
            if (names.Count == 0) { clear++; continue; }
            foreach (var n in names) onLines[n] = onLines.TryGetValue(n, out var c) ? c + 1 : 1;
            if (names.Count == 1) foreach (var n in names) only[n] = only.TryGetValue(n, out var c2) ? c2 + 1 : 1;
            if (nonTree) otherBlocked++; else treeOnlyLines.Add(names);
        }
        sb.Append(tg.label.ToUpperInvariant() + " at " + P3(tg.at) + ": " + clear + " of " + eyes.Count + " standing eyes clear; " + treeOnlyLines.Count + " lines blocked by trees alone, " + otherBlocked + " by something else too\n");
        foreach (var kv in System.Linq.Enumerable.OrderByDescending(onLines, k => k.Value)) sb.Append("  " + kv.Key + " at " + P3(where[kv.Key]) + ": on " + kv.Value + " lines, the only blocker on " + (only.TryGetValue(kv.Key, out var o) ? o : 0) + (isTree.Contains(kv.Key) ? " (tree)" : "") + "\n");
        // the fewest trees whose moving opens one line: the tree-only line with the fewest trees
        System.Collections.Generic.HashSet<string> best = null; foreach (var s in treeOnlyLines) if (best == null || s.Count < best.Count) best = s;
        sb.Append("  fewest trees to open one line: " + (best == null ? (clear > 0 ? "none needed" : "no line is blocked by trees alone") : best.Count + " (" + string.Join("; ", System.Linq.Enumerable.Select(best, n => n + " " + P3(where[n]))) + ")") + "\n");
    }
}
finally { foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); UnityEngine.Physics.SyncTransforms(); }
return "standing eyes " + eyes.Count + " (grid " + ((int)(set.deckHalf * 2f / set.deckGrid) + 1) * ((int)(set.deckHalf * 2f / set.deckGrid) + 1) + " plus rail); temporary mesh colliders " + temps.Count + "\n" + sb;
