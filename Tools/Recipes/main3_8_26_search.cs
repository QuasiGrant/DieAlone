// Main3 8.26 viewpoint search (edit mode, Main3; the 8.26 gate, Wren 2026-10-03, item 6). Never saves; removes its temporary colliders.
// For each viewpoint: stand spots every grid m within reach m of its doc spot, on ground at least groundMin, treadClear m or more off every
// trail centre line; eye 1.6; a target counts as seen when the line to it passes the terrain, every collider and every drawn mesh
// (temporary exact colliders on every drawn mesh; only those and the terrain block: a trunk's capsule is thinner than its bark). Prints the nearest spot that sees at least need of its targets, and how many it sees.
// RIM SPOT (C6): the camp floor (the open floor, fire, tent, table, easel; 4 of 5), from the doc's spot (94, 138.5).
// SNAG LINE VIEW: Snag line pieces 1 to 4, from the steps' top (96.5, 142.8).
// F1 EYE: the fire, the pool and the faced canvases, from the steps' top; this eye may stand anywhere, the trail and the steps too.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
const float grid = 0.5f, eyeH = 1.6f, groundMin = 3.0f, treadClear = 1.8f, insideReach = 6f; const int insideDirs = 8;
var c3 = Root("Campsites").transform.Find("Camp_3"); var line = c3.Find("Layout826/SnagLine");
(UnityEngine.Transform t, UnityEngine.Vector3 c) Centre(string path, UnityEngine.Vector3 fallback) { var t = c3.Find(path); return (t, t != null ? PlaceKit.MeshBounds(t.gameObject).center : fallback); }   // a target's own meshes never hide it
var fire = Centre("Dressing/Fire", V(76f, -3.6f, 150.5f)); var tent = Centre("Dressing/CS_Tent_Old_2", V(75f, -3.2f, 140.7f)); var table = Centre("Layout826/PlankTable", V(78.5f, -3.4f, 147.3f));
var easel = Centre("Dressing/Easel", V(80.3f, -3f, 148.8f)); var pool = Centre("Layout826/Creek/Pool", V(83.6f, -4.2f, 149.7f)); var canv = Centre("Layout826/FacedCanvases", V(70.3f, -3.4f, 145.95f));
var floor = ((UnityEngine.Transform)null, V(77.5f, H(77.5f, 146.5f) + 0.1f, 146.5f));   // the open floor between the table and the fire
var pieces = new System.Collections.Generic.List<(UnityEngine.Transform t, UnityEngine.Vector3 c)>(); for (int i = 1; i <= 4; i++) pieces.Add(Centre("Layout826/SnagLine/Piece" + i, V(0f, 0f, 0f)));
var lines = new System.Collections.Generic.List<UnityEngine.Vector2[]>(); foreach (UnityEngine.Transform lg in Root("Trails").transform) { var l = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform p in lg) l.Add(P(p.position.x, p.position.z)); if (l.Count > 1) lines.Add(l.ToArray()); }
float SegDist(UnityEngine.Vector2 q, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(q - a, ab) / UnityEngine.Mathf.Max(1e-4f, ab.sqrMagnitude)); return UnityEngine.Vector2.Distance(q, a + ab * t); }
float TrailDist(UnityEngine.Vector2 q) { float best = float.MaxValue; foreach (var l in lines) for (int i = 1; i < l.Length; i++) best = UnityEngine.Mathf.Min(best, SegDist(q, l[i - 1], l[i])); return best; }
var views = new (string name, UnityEngine.Vector2 at, float reach, (UnityEngine.Transform t, UnityEngine.Vector3 c)[] targets, int need, bool offTrail)[] {
    ("RIM SPOT", P(94f, 138.5f), 12f, new[] { floor, fire, tent, table, easel }, 4, true),
    ("SNAG LINE VIEW", P(96.5f, 142.8f), 6f, pieces.ToArray(), 3, true),
    ("F1 EYE", P(96.5f, 142.8f), 14f, new[] { fire, pool, canv }, 3, false) };   // a frame eye may stand on the trail, down the steps
var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var sb = new System.Text.StringBuilder();
try
{
    // every drawn mesh near the lines gets a temporary exact collider (once, for all views)
    var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
    var area = new UnityEngine.Bounds(V(84f, 0f, 146f), V(44f, 60f, 44f));
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    { if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || !mr.bounds.Intersects(area)) continue; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
    UnityEngine.Physics.SyncTransforms();
    bool Seen(UnityEngine.Vector3 eye, (UnityEngine.Transform t, UnityEngine.Vector3 c) tg) { var d = tg.c - eye; foreach (var h in UnityEngine.Physics.RaycastAll(eye, d.normalized, d.magnitude - 0.3f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { if (!(h.collider is UnityEngine.TerrainCollider) && !temps.Contains(h.collider)) continue; if (tg.t != null && h.collider.transform.IsChildOf(tg.t)) continue; return false; } return true; }
    // an eye inside a drawn mesh (the dead Snag's bark: its Trunk collider is not the drawn tree) sees out through back faces: rejected when
    // a level ray in any of insideDirs directions meets a back face first (8.26 round 2: the first viewpoint stood in the Snag)
    bool Inside(UnityEngine.Vector3 eye) { bool was = UnityEngine.Physics.queriesHitBackfaces; UnityEngine.Physics.queriesHitBackfaces = true; try { for (int k = 0; k < insideDirs; k++) { var dir = UnityEngine.Quaternion.Euler(0f, k * 360f / insideDirs, 0f) * UnityEngine.Vector3.forward; float best = float.MaxValue; bool back = false; foreach (var h in UnityEngine.Physics.RaycastAll(eye, dir, insideReach, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { if (!temps.Contains(h.collider) || h.distance >= best) continue; best = h.distance; back = UnityEngine.Vector3.Dot(h.normal, dir) > 0f; } if (back) return true; } return false; } finally { UnityEngine.Physics.queriesHitBackfaces = was; } }
    foreach (var (name, at, reach, targets, need, offTrail) in views)
    {
        float bestD = float.MaxValue; string best = "none"; int tried = 0;
        for (float x = at.x - reach; x <= at.x + reach + 1e-3f; x += grid) for (float z = at.y - reach; z <= at.y + reach + 1e-3f; z += grid)
        {
            float dd = UnityEngine.Vector2.Distance(P(x, z), at); if (dd > reach) continue; float g = H(x, z); if (offTrail && (g < groundMin || TrailDist(P(x, z)) < treadClear)) continue; tried++;
            var eye = V(x, g + eyeH, z); if (Inside(eye)) continue; int seen = 0; string which = ""; for (int k = 0; k < targets.Length; k++) if (Seen(eye, targets[k])) { seen++; which += k; }
            if (dd < 0.01f) sb.Append(name + " at the doc's spot sees targets " + which + "\n");
            if (seen >= need && dd < bestD) { bestD = dd; best = "(" + F(x) + ", " + F(g) + ", " + F(z) + "), " + F(dd) + " m from the doc's spot, sees " + seen + " of " + targets.Length + " (" + which + ")"; }
        }
        sb.Append(name + ": " + tried + " spots tried; nearest seeing " + need + " or more: " + best + "\n");
    }
}
finally { foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); UnityEngine.Physics.SyncTransforms(); }
return sb.ToString();
