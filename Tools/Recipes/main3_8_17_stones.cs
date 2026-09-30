// Main3 task 8.17, the Ward stones and the trail boulders (Check3_Quality 4: the stones are two slabs, gray domes on the trails). Run
// after 8.16 in Main3, edit mode; rerunnable. The Ward stones (8.7, Ward/Stones/Stone_1 to 3) stop drawing and keep their boxes: the
// boxes are their collision and 8.9's W-1 reads their bounds (a disabled renderer keeps its bounds, checked 8.17); an owned carved
// menhir (Effigy DarkRock, runes) fills each box, sunk menhirSink into the ground. The four gray POI_Boulder spheres of 8.3 (sight-breaks
// the trails bow round) become owned BK big boulders of the same width in the same places, with the pack's own colliders, each still
// under a POI_Boulder group.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
if (kit.Root("Forest") == null) return "run 8.16 first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
const float menhirSink = 0.4f, bigBoulderWidth = 6.1f, boulderSink = 0.6f;
// ---- Ward stones
var stones = kit.Root("Ward").transform.Find("Stones"); if (stones == null) return "no Ward/Stones";
var ward = kit.Fresh("StonesDressing", kit.Root("Ward").transform, stones.position, 0f);
int menhirs = 0; var picks = new[] { 5, 2, 7 };
foreach (UnityEngine.Transform s in stones)
{
    var r = s.GetComponent<UnityEngine.Renderer>(); if (r == null) continue; r.enabled = false; var b = r.bounds;
    var m = kit.Fill(PlaceKit.MH + "Single/DarkRock/DarkRock_CarvedRunes/StoneMenhir_" + picks[menhirs % picks.Length] + "_DarkRock_CarvedRunes", ward,
        ward.InverseTransformPoint(V(b.center.x, b.min.y - menhirSink, b.center.z)), V(b.size.x, b.size.y + menhirSink, b.size.z), menhirs * 70f);
    menhirs++;
}
// ---- trail boulders
var poi = kit.Root("PointsOfInterest").transform; int swapped = 0;
foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(poi)))
{
    if (t.name != "POI_Boulder" || t.GetComponent<UnityEngine.MeshRenderer>() == null) continue;   // an already swapped one is a bare group
    var p = t.position; float w = t.localScale.x;
    PlaceKit.Remove(t);
    var g = kit.Group("POI_Boulder", poi, p, 0f);
    kit.Ground(PlaceKit.BK + "Rocks/BigBoulders_" + (swapped % 6), g, p.x, p.z, swapped * 83f, w / bigBoulderWidth * 1.1f, true, boulderSink);
    swapped++;
}
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
return "saved=" + saved + " stones: menhirs " + menhirs + ", trail boulders swapped " + swapped + " | " + kit.Report();
