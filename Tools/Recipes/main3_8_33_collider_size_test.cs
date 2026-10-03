// Main3 8.33 collider size test (edit mode, any scene; never saves). Proves the area check's COLLIDER SIZE line (ColliderFit) flags the
// known case on a test copy: Camp 3's tent (CS_Tent_Old_2, as main3_8_17_camp3.cs places it, turned 215 degrees) with the box PlaceKit
// fitted to its world bounds, the turned-prop inflation; and passes the same tent with a box fitted to its meshes in its own axes.
// The copy stands far off the map and is destroyed before returning.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var kit = new PlaceKit(scene);
var set = Main3AreaSet.Load(); if (set == null) return "no Assets/Settings/Main3Areas.asset (run main3_areas_setup.cs)";
const float testYaw = 215f, testScale = 1.25f;   // Camp 3's turn and scale (main3_8_17_camp3.cs)
var holder = new UnityEngine.GameObject("ColliderSizeTest"); holder.transform.position = new UnityEngine.Vector3(-5000f, 0f, -5000f);
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
string Size(UnityEngine.Vector3 v) => v.x.ToString("F2") + " x " + v.y.ToString("F2") + " x " + v.z.ToString("F2");
try
{
    foreach (var exact in new[] { false, true })
    {
        var tent = kit.Spawn(PlaceKit.CS + "CS_Tent_Old_2", holder.transform); if (tent == null) return "no CS_Tent_Old_2 prefab";
        PlaceKit.StripColliders(tent); tent.transform.localPosition = UnityEngine.Vector3.zero; tent.transform.rotation = UnityEngine.Quaternion.Euler(0f, testYaw, 0f); tent.transform.localScale = UnityEngine.Vector3.one * testScale;
        UnityEngine.BoxCollider bc;
        if (!exact) { PlaceKit.FitCollider(tent); bc = tent.GetComponent<UnityEngine.BoxCollider>(); }   // the 8.17 way: world bounds into the turned object's axes
        else
        {
            bool any = false; var b = new UnityEngine.Bounds();
            foreach (var mf in tent.GetComponentsInChildren<UnityEngine.MeshFilter>())
            {
                var mr = mf.GetComponent<UnityEngine.MeshRenderer>(); if (mf.sharedMesh == null || mr == null || WalkIns.LooksOnly(mr)) continue; var mb = mf.sharedMesh.bounds;
                for (int i = 0; i < 8; i++) { var l = tent.transform.InverseTransformPoint(mf.transform.TransformPoint(new UnityEngine.Vector3((i & 1) == 0 ? mb.min.x : mb.max.x, (i & 2) == 0 ? mb.min.y : mb.max.y, (i & 4) == 0 ? mb.min.z : mb.max.z))); if (!any) { b = new UnityEngine.Bounds(l, UnityEngine.Vector3.zero); any = true; } else b.Encapsulate(l); }
            }
            bc = tent.AddComponent<UnityEngine.BoxCollider>(); bc.center = b.center; bc.size = b.size;
        }
        var r = ColliderFit.Measure(bc, set.colliderSlack, set.colliderShare);
        string what = (exact ? "fitted to its meshes in its own axes" : "fitted by PlaceKit.FitCollider (world bounds)") + ": box " + Size(r.colliderSize) + " over a mesh of " + Size(r.meshSize) + ", a face " + r.excess.ToString("F2") + " m out";
        Line(r.measured && r.inflated == !exact, "TENT " + (exact ? "CONTROL" : "CASE") + " (" + (exact ? "must pass" : "must be flagged") + "), " + what + (r.inflated ? ": flagged" : ": not flagged"));
        UnityEngine.Object.DestroyImmediate(tent);
    }
}
finally { UnityEngine.Object.DestroyImmediate(holder); }
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.33 collider size test (slack " + set.colliderSlack.ToString("F2") + " m, share " + set.colliderShare.ToString("F2") + ")\n" + sb;
