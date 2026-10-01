// Main3 8.18a, solid props (Marlow's breaks hunt 2026-10-01, Breaks_2026-10-01.md 9 to 11: the player walked into 465 meshes with no
// collider; 363 of them rocks: the Camp 3 and trench stop boulders, the scree bands, the stop logs, camp props). Edit mode, Main3;
// rerunnable; in the runner after the last recipe that places props (main3_8_18a_smoke.cs) and before the sightlines.
// One rule for every recipe's props, so a recipe that adds a rock later is covered on the next run: every mesh WalkIns.Find reports
// (Assets/Editor/WalkIns.cs: a solid mesh over 0.5 m the body can enter without touching a collider) gets its convex hull as a collider,
// repeated until none is left (at most maxPasses). Walkable pieces under 0.5 m that Marlow walked through get the same: the footbridge
// deck and rails and the stepping stones. The climb and Ward pieces (Ward/Climb, Rock/ClimbRing) are left to PLAN 8.20.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset"); if (tuning == null) return "no PlayerTuning";
string[] laterRoots = { "Ward/Climb/", "Rock/ClimbRing/" }; string[] lowWalkables = { "PointsOfInterest/POI_Footbridge", "PointsOfInterest/POI_Stepping_stones" };
const int maxPasses = 3;
int added = 0, exactN = 0; var groups = new System.Collections.Generic.SortedDictionary<string, int>();
bool Solid(UnityEngine.MeshRenderer mr, bool flagged = true)
{
    var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) return false;
    var had = mr.GetComponent<UnityEngine.MeshCollider>();   // a hull PhysX had to simplify (over 255 faces) can sit inside the mesh: the exact mesh then
    if (flagged && had != null && had.convex && had.sharedMesh == mf.sharedMesh) { had.convex = false; exactN++; return true; }
    if (mr.GetComponent<UnityEngine.Collider>() != null) return false;
    var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; added++;
    var p = WalkIns.PathOf(mr.transform).Split('/'); string g = p.Length > 2 ? p[0] + "/" + p[1] + "/" + p[2] : string.Join("/", p); groups[g] = groups.TryGetValue(g, out var k) ? k + 1 : 1; return true;
}
foreach (var root in lowWalkables)
{
    var t = UnityEngine.GameObject.Find(root); if (t == null) return "no " + root;
    foreach (var mr in t.GetComponentsInChildren<UnityEngine.MeshRenderer>()) Solid(mr, false);
}
int pass = 0, left = 0;
for (; pass < maxPasses; pass++)
{
    left = 0; int now = 0;
    foreach (var w in WalkIns.Find(tuning.capsuleRadius, tuning.standHeight))
    {
        bool later = false; foreach (var lr in laterRoots) if (w.Path.StartsWith(lr)) later = true; if (later) continue;
        left++; if (Solid(w.Renderer)) now++;
    }
    if (now == 0) break;
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var parts = new System.Collections.Generic.List<string>(); foreach (var kv in groups) parts.Add(kv.Key + " " + kv.Value);
return "saved=" + saved + " | convex colliders added " + added + " (" + exactN + " made exact) in " + (pass + 1) + " passes; walk-ins left with no fix " + left + " | " + string.Join(", ", parts);
