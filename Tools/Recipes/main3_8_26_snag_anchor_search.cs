// Main3 8.26 Snag anchor search (edit mode, Main3; the 8.26 gate round 2, Wren 2026-10-03 item C). Never saves; removes its temporary
// colliders. For each Snag-end height over its ground from fromUp to toUp in stepUp m: the rope from the Snag end (as main3_8_26_camp3.cs
// builds it: on the trunk collider's face toward the stake) to the stake top, each piece hanging pieceDrop + pieceSide / 2 under its peg;
// a piece counts as seen when one standing deck eye (Main3AreaSet deckGrid over the deck, deckEye high) has a clear line to its centre past
// every collider (Ignore Raycast, the Snag line itself and the player apart) and every drawn mesh, the tower's own included (temporary exact
// colliders on the drawn meshes those lines cross, as the area check's DECK). Prints, per height, how many standing eyes see pieces 1 to 3
// and each piece's clearance over its ground, and the least height where each of pieces 1 to 3 is seen and each clears pieceClear.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var ter = UnityEngine.Terrain.activeTerrain; float G(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
const float fromUp = 3.5f, toUp = 14f, stepUp = 0.5f, pieceClear = 2.85f, pieceSide = 0.5f, pieceDrop = 0.2f, stakeUp = 3.6f; const int seenPieces = 3;
var pegs = new[] { P(92.8f, 149.8f), P(89.6f, 153.1f), P(86.4f, 156.4f), P(83.2f, 159.7f) }; var stakeAt = P(80f, 163f);
var trunk = Root("Giants").transform.Find("Heroes/Snag/Trunk"); var line = Root("Campsites").transform.Find("Camp_3/Layout826/SnagLine"); var tower = Root("Camp").transform.Find("Tower");
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var set = Main3AreaSet.Load(); float deckTop = tower.Find("Cab").position.y;
var tc = trunk.GetComponent<UnityEngine.Collider>().bounds; var sc = P(tc.center.x, tc.center.z); var a2 = sc + (stakeAt - sc).normalized * tc.extents.x; var b = V(stakeAt.x, G(stakeAt.x, stakeAt.y) + stakeUp, stakeAt.y);
var eyes = new System.Collections.Generic.List<UnityEngine.Vector3>(); for (float gx = -set.deckHalf; gx <= set.deckHalf + 0.01f; gx += set.deckGrid) for (float gz = -set.deckHalf; gz <= set.deckHalf + 0.01f; gz += set.deckGrid) eyes.Add(V(tower.position.x + gx, deckTop + set.deckEye, tower.position.z + gz));
UnityEngine.Vector3[] Pieces(float up) { var a = V(a2.x, G(a2.x, a2.y) + up, a2.y); var res = new UnityEngine.Vector3[pegs.Length]; for (int i = 0; i < pegs.Length; i++) { var ab = P(b.x - a.x, b.z - a.z); float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(pegs[i] - P(a.x, a.z), ab) / ab.sqrMagnitude); res[i] = UnityEngine.Vector3.Lerp(a, b, t) - V(0f, pieceDrop + pieceSide * 0.5f, 0f); } return res; }
var heights = new System.Collections.Generic.List<float>(); for (float u = fromUp; u <= toUp + 1e-3f; u += stepUp) heights.Add(u);
// the temporary colliders: every drawn LOD0 mesh with no collider that some standing eye's line to some piece at some height crosses
var rays = new System.Collections.Generic.List<(UnityEngine.Ray r, float len)>(); foreach (var u in heights) { var ps = Pieces(u); for (int i = 0; i < seenPieces; i++) foreach (var e in eyes) { var d = ps[i] - e; rays.Add((new UnityEngine.Ray(e, d.normalized), d.magnitude)); } }
var span = new UnityEngine.Bounds(rays[0].r.origin, UnityEngine.Vector3.zero); foreach (var (r, l) in rays) { span.Encapsulate(r.origin); span.Encapsulate(r.GetPoint(l)); }
var notLod0 = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(); foreach (var lod in UnityEngine.Object.FindObjectsByType<UnityEngine.LODGroup>(UnityEngine.FindObjectsSortMode.None)) { var l = lod.GetLODs(); for (int i = 1; i < l.Length; i++) foreach (var rr in l[i].renderers) if (rr != null) notLod0.Add(rr); }
var temps = new System.Collections.Generic.List<UnityEngine.Collider>(); var sb = new System.Text.StringBuilder();
try
{
    foreach (var mr in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshRenderer>(UnityEngine.FindObjectsSortMode.None))
    {
        if (!mr.enabled || !mr.gameObject.activeInHierarchy || notLod0.Contains(mr) || mr.GetComponent<UnityEngine.Collider>() != null || mr.transform.IsChildOf(line) || mr.transform.IsChildOf(pc.transform) || mr.name.Contains("Glass")) continue;
        var bb = mr.bounds; if (!bb.Intersects(span)) continue; var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue;
        bool crossed = false; foreach (var (r, l) in rays) if (bb.IntersectRay(r, out float dist) && dist <= l) { crossed = true; break; }
        if (!crossed) continue; var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc);
    }
    UnityEngine.Physics.SyncTransforms();
    bool Clear(UnityEngine.Vector3 e, UnityEngine.Vector3 p) { var d = p - e; foreach (var h in UnityEngine.Physics.RaycastAll(e, d.normalized, d.magnitude - 0.05f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { var ht = h.collider.transform; if (ht.gameObject.layer == 2 || ht.IsChildOf(line) || ht.IsChildOf(pc.transform)) continue; return false; } return true; }
    float best = float.NaN;
    foreach (var u in heights)
    {
        var ps = Pieces(u); bool all = true; var row = new System.Text.StringBuilder("up " + F(u) + ":");
        for (int i = 0; i < pegs.Length; i++)
        {
            float clear = ps[i].y - pieceSide * 0.5f - G(ps[i].x, ps[i].z); int seen = 0; if (i < seenPieces) foreach (var e in eyes) if (Clear(e, ps[i])) seen++;
            row.Append(" piece " + (i + 1) + (i < seenPieces ? " seen " + seen : "") + " clear " + F(clear) + ";"); if (clear < pieceClear || (i < seenPieces && seen == 0)) all = false;
        }
        sb.Append(row + "\n"); if (all && float.IsNaN(best)) best = u;
    }
    sb.Insert(0, "temporary mesh colliders " + temps.Count + "; standing eyes " + eyes.Count + "; least Snag-end height with pieces 1 to 3 each seen and every piece clear: " + (float.IsNaN(best) ? "none up to " + F(toUp) : F(best) + " m") + "\n");
}
finally { foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t); UnityEngine.Physics.SyncTransforms(); }
return sb.ToString();
