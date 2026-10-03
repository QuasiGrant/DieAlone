using System.Collections.Generic;
using UnityEngine;

/// How far a box, sphere or capsule collider reaches past the visible meshes it stands for (PLAN 8.33: the turned-prop inflation that
/// broke the Camp 3 tent, the cave props and the Hollow Giant, where a box fitted to world bounds grew when its object was turned).
/// Measured in the collider's own axes: every drawn mesh under the collider's object (look-only ones left out by name, WalkIns.LooksOnly;
/// pieces with colliders of their own included, as the 8.24 gate workbenches showed: their tables carry their own) are boxed by their exact mesh bounds, and each of the six faces of the collider is compared with
/// that box. A face that stands out by more than slack m, or by more than share of the mesh's size on that axis, whichever is larger,
/// makes the collider inflated. Invisible blockers (no drawn mesh under them) are not measured. Editor only.
public static class ColliderFit
{
    public struct Result { public bool measured, inflated; public float excess; public Vector3 colliderSize, meshSize; }

    public static Result Measure(Collider c, float slack, float share)
    {
        var r = new Result(); var t = c.transform; Vector3 cMin, cMax;
        switch (c)
        {
            case BoxCollider b: cMin = b.center - b.size * 0.5f; cMax = b.center + b.size * 0.5f; break;
            case SphereCollider s: cMin = s.center - Vector3.one * s.radius; cMax = s.center + Vector3.one * s.radius; break;
            case CapsuleCollider k:
            {
                var e = Vector3.one * k.radius; e[k.direction] = Mathf.Max(k.radius, k.height * 0.5f); cMin = k.center - e; cMax = k.center + e; break;
            }
            default: return r;   // mesh colliders are the mesh, terrain is the ground
        }
        bool any = false; var mb = new Bounds();
        foreach (var mf in c.GetComponentsInChildren<MeshFilter>())
        {
            var mr = mf.GetComponent<MeshRenderer>(); if (mf.sharedMesh == null || mr == null || !mr.enabled || !mr.gameObject.activeInHierarchy || WalkIns.LooksOnly(mr)) continue;
            var lb = mf.sharedMesh.bounds;
            for (int i = 0; i < 8; i++)
            {
                var w = mf.transform.TransformPoint(new Vector3((i & 1) == 0 ? lb.min.x : lb.max.x, (i & 2) == 0 ? lb.min.y : lb.max.y, (i & 4) == 0 ? lb.min.z : lb.max.z));
                var l = t.InverseTransformPoint(w); if (!any) { mb = new Bounds(l, Vector3.zero); any = true; } else mb.Encapsulate(l);
            }
        }
        if (!any) return r;
        r.measured = true; var scale = t.lossyScale; r.colliderSize = Vector3.Scale(cMax - cMin, Abs(scale)); r.meshSize = Vector3.Scale(mb.size, Abs(scale));
        for (int a = 0; a < 3; a++)
        {
            float s = Mathf.Abs(scale[a]), over = Mathf.Max(cMax[a] - mb.max[a], mb.min[a] - cMin[a]) * s, allow = Mathf.Max(slack, share * mb.size[a] * s);
            r.excess = Mathf.Max(r.excess, over); if (over > allow) r.inflated = true;
        }
        return r;
    }

    static Vector3 Abs(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
}
