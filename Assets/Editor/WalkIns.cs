using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// Finds the solid meshes a standing player's body can enter (8.18a; Marlow's breaks hunt 2026-10-01: 465 walk-in meshes, 363 of them
/// rocks). Shared by Tools/Recipes/main3_walk_into_check.cs (reports) and main3_8_18a_solid.cs (gives each a collider). Editor only.
/// A mesh counts when it is over MinTall m tall, is a LOD0 or plain renderer, and is not a look-only kind (foliage, trees, the fire,
/// smoke, water, glows, backdrops; names and shaders below). On a StepGrid m grid over its footprint, widened by the capsule radius,
/// every walkable surface under it (terrain or a collider not of the mesh's own object, no steeper than the slope limit) is a stand; a
/// stand where the player's capsule touches no collider but enters the exact mesh by EnterDepth m or more is a walk-in.
public static class WalkIns
{
    public const float MinTall = 0.5f, StepGrid = 0.5f, EnterDepth = 0.1f;
    const float ProbeOver = 2f, ProbeUnder = 3f, FootLift = 0.05f, SlopeDeg = 45f;
    public const int TempLayer = 31; const int MaxStandsPerMesh = 400;
    static readonly Regex SkipName = new Regex("Fern|Bush|Leaves|Branch|Grass|Moss|Reed|Plant|Flower|Fir|Pine|Sequoia|Tree|Sapling|Foliage|Crown|Spike|Flame|Glow|Smoke|Water|Bulb|Rope|Wire|Line|Cable|Label|Decal|Shadow|Window|Glass", RegexOptions.IgnoreCase);
    static readonly Regex SkipShader = new Regex("DieAlone/(FlameCard|FireStandIn|Smoke|Backdrop|Water|HorizonGlow|SkyGradient)|Particles|Nature/Tree|SpeedTree", RegexOptions.IgnoreCase);
    static readonly HashSet<string> SkipRoots = new HashSet<string> { "Forest", "Backdrop", "Giants" };

    public struct WalkIn { public MeshRenderer Renderer; public Vector3 At; public int Stands; public string Path; }

    /// Every walk-in mesh in the open scenes. capsuleRadius and standHeight are the player's (PlayerTuning).
    public static List<WalkIn> Find(float capsuleRadius, float standHeight)
    {
        Physics.SyncTransforms();
        var pc = Object.FindFirstObjectByType<PlayerController>(); var player = pc != null ? pc.transform : null;
        var fire = GameObject.Find("Ward/StandInFire"); var fireRoot = fire != null ? fire.transform : null;
        float slopeCos = Mathf.Cos(SlopeDeg * Mathf.Deg2Rad); int others = ~(1 << TempLayer);
        var found = new List<WalkIn>();
        var temp = new GameObject("WalkInProbe") { layer = TempLayer }; var exact = temp.AddComponent<MeshCollider>();
        try
        {
            foreach (var mr in Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None))
            {
                if (!mr.enabled || !mr.gameObject.activeInHierarchy) continue;
                var mf = mr.GetComponent<MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue;
                var b = mr.bounds; if (b.size.y < MinTall || Skip(mr, player, fireRoot)) continue;
                var lod = mr.GetComponentInParent<LODGroup>(); if (lod != null && lod.GetLODs().Length > 0 && System.Array.IndexOf(lod.GetLODs()[0].renderers, mr) < 0) continue;
                var ownerGo = PrefabUtility.GetOutermostPrefabInstanceRoot(mr.gameObject); var owner = ownerGo != null ? ownerGo.transform : lod != null ? lod.transform : mr.transform;
                temp.transform.SetPositionAndRotation(mr.transform.position, mr.transform.rotation); temp.transform.localScale = mr.transform.lossyScale;
                exact.sharedMesh = mf.sharedMesh; Physics.SyncTransforms();
                int stands = 0; Vector3 first = default;
                for (float x = b.min.x - capsuleRadius; x <= b.max.x + capsuleRadius && stands < MaxStandsPerMesh; x += StepGrid)
                    for (float z = b.min.z - capsuleRadius; z <= b.max.z + capsuleRadius && stands < MaxStandsPerMesh; z += StepGrid)
                        foreach (var h in Physics.RaycastAll(new Vector3(x, b.max.y + ProbeOver, z), Vector3.down, b.size.y + ProbeOver + ProbeUnder, others, QueryTriggerInteraction.Ignore))
                        {
                            if (h.normal.y < slopeCos || (player != null && h.collider.transform.IsChildOf(player)) || h.collider.transform.IsChildOf(owner)) continue;
                            var a = h.point + Vector3.up * (capsuleRadius + FootLift); var c = h.point + Vector3.up * (standHeight - capsuleRadius);
                            bool blocked = false; foreach (var o in Physics.OverlapCapsule(a, c, capsuleRadius, others, QueryTriggerInteraction.Ignore)) if (player == null || !o.transform.IsChildOf(player)) { blocked = true; break; }
                            if (blocked || !Physics.CheckCapsule(a, c, capsuleRadius - EnterDepth, 1 << TempLayer, QueryTriggerInteraction.Ignore)) continue;
                            if (stands == 0) first = h.point; stands++;
                        }
                if (stands > 0) found.Add(new WalkIn { Renderer = mr, At = first, Stands = stands, Path = PathOf(mr.transform) });
            }
        }
        finally { Object.DestroyImmediate(temp); Physics.SyncTransforms(); }
        found.Sort((p, q) => string.CompareOrdinal(p.Path, q.Path));
        return found;
    }

    // ---- trail treads (8.20 check, 2026-10-01: hulls of boulders by the chute gap and in the cleft closed the tread): a capsule TreadR
    // round from TreadLow to TreadHigh over every trail point and every TreadStep m between
    public const float TreadLow = 0.45f, TreadHigh = 1.5f, TreadR = 0.3f, TreadStep = 0.5f;
    public static List<(Vector3 a, Vector3 b)> Treads()
    {
        var list = new List<(Vector3, Vector3)>(); var trails = GameObject.Find("Trails"); if (trails == null) return list;
        foreach (Transform leg in trails.transform)
        {
            Vector3? prev = null;
            foreach (Transform pt in leg)
            {
                var p = pt.position; int n = prev.HasValue ? Mathf.Max(1, Mathf.CeilToInt(Vector3.Distance(prev.Value, p) / TreadStep)) : 1;
                for (int i = 1; i <= n; i++) { var q = prev.HasValue ? Vector3.Lerp(prev.Value, p, i / (float)n) : p; list.Add((q + Vector3.up * TreadLow, q + Vector3.up * TreadHigh)); }
                prev = p;
            }
        }
        return list;
    }
    /// Whether a collider enters any tread capsule (tested alone on TempLayer).
    public static bool InTread(Collider c, List<(Vector3 a, Vector3 b)> treads)
    {
        var b = c.bounds; b.Expand(TreadR * 2f); int layer = c.gameObject.layer; c.gameObject.layer = TempLayer; Physics.SyncTransforms(); bool hit = false;
        foreach (var t in treads) { if (!b.Contains(t.a) && !b.Contains(t.b)) continue; if (Physics.CheckCapsule(t.a, t.b, TreadR, 1 << TempLayer, QueryTriggerInteraction.Ignore)) { hit = true; break; } }
        c.gameObject.layer = layer; Physics.SyncTransforms(); return hit;
    }
    /// Whether a mesh, as drawn, enters any tread (a temporary exact collider).
    public static bool MeshInTread(MeshRenderer mr, List<(Vector3 a, Vector3 b)> treads)
    {
        var mf = mr.GetComponent<MeshFilter>(); if (mf == null || mf.sharedMesh == null) return false;
        var temp = new GameObject("TreadProbe"); temp.transform.SetPositionAndRotation(mr.transform.position, mr.transform.rotation); temp.transform.localScale = mr.transform.lossyScale;
        var mc = temp.AddComponent<MeshCollider>(); mc.sharedMesh = mf.sharedMesh; bool hit = InTread(mc, treads); Object.DestroyImmediate(temp); Physics.SyncTransforms(); return hit;
    }

    public static string PathOf(Transform t) { var s = t.name; for (var p = t.parent; p != null; p = p.parent) s = p.name + "/" + s; return s; }

    static bool Skip(MeshRenderer mr, Transform player, Transform fireRoot)
    {
        for (var t = mr.transform; t != null; t = t.parent) { if (SkipName.IsMatch(t.name)) return true; if (t.parent == null && SkipRoots.Contains(t.name)) return true; }
        foreach (var m in mr.sharedMaterials) if (m != null && (SkipShader.IsMatch(m.shader.name) || SkipName.IsMatch(m.name))) return true;
        if (fireRoot != null && mr.transform.IsChildOf(fireRoot)) return true;
        return player != null && mr.transform.IsChildOf(player);
    }
}
