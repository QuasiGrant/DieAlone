// Main3 8.18a, solid props (Marlow's breaks hunt 2026-10-01, Breaks_2026-10-01.md 9 to 11: the player walked into 465 meshes with no
// collider; 363 of them rocks: the Camp 3 and trench stop boulders, the scree bands, the stop logs, camp props). Edit mode, Main3;
// rerunnable; in the runner after the last recipe that places props (main3_8_18a_smoke.cs) and before the sightlines.
// One rule for every recipe's props, so a recipe that adds a rock later is covered on the next run: every mesh WalkIns.Find reports
// (Assets/Editor/WalkIns.cs: a solid mesh over 0.5 m the body can enter without touching a collider) gets its convex hull as a collider,
// repeated until none is left (at most maxPasses). Walkable pieces under 0.5 m that Marlow walked through get the same: the footbridge
// deck and rails and the stepping stones. The climb and Ward pieces (Ward/Climb, Rock/ClimbRing) are left to PLAN 8.20. No hull may enter a
// trail tread (InTread below): one that does takes the exact mesh, so the collider is the rock as drawn.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset"); if (tuning == null) return "no PlayerTuning";
// rimRoots: the rim rocks stand on the climb's rim and the ledge lip, which are the stops; a hull on one is a step over the stop (8.20 check,
// 2026-10-01: a jump went over the lip at z 236 off CS_Rock_6's hull), so they never get one, and one already there goes
string[] laterRoots = { }; string[] rimRoots = { "Rock/ClimbRing/RimBoulders" }; string[] lowWalkables = { "PointsOfInterest/POI_Footbridge", "PointsOfInterest/POI_Stepping_stones" };
const int maxPasses = 3;
int added = 0, exactN = 0; var groups = new System.Collections.Generic.SortedDictionary<string, int>();
// no collider may close a trail tread (8.20 check, 2026-10-01: hulls of the boulders by the chute gap and in the cleft did): a hull that
// enters one takes the exact mesh; a mesh that enters one as drawn gets no collider and is listed for its owning recipe to move
// (WalkIns.MeshInTread; the walk-into check lists it apart)
var treads = WalkIns.Treads(); var inTread = new System.Collections.Generic.List<string>(); var skip = new System.Collections.Generic.HashSet<UnityEngine.MeshRenderer>();
string[] passRoots = { "Rock/BandScree", "Rock/KnobRock", "Ground815", "Campsites", "PointsOfInterest", "FrontZone", "Places", "Camp/", "Cave/ChamberDressing", "Cave/SideRoom" };   // where this pass adds hulls
bool IsLowWalkable(UnityEngine.Transform t) { var p = WalkIns.PathOf(t); foreach (var r in lowWalkables) if (p.StartsWith(r)) return true; return false; }
bool RimRoot(UnityEngine.Transform t) { var p = WalkIns.PathOf(t); foreach (var r in rimRoots) if (p.StartsWith(r)) return true; return false; }
bool PassRoot(UnityEngine.Transform t) { var p = WalkIns.PathOf(t); foreach (var r in passRoots) if (p.StartsWith(r)) return true; return false; }
bool Solid(UnityEngine.MeshRenderer mr, bool flagged = true)
{
    if (skip.Contains(mr) || RimRoot(mr.transform)) return false;
    var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) return false;
    var had = mr.GetComponent<UnityEngine.MeshCollider>();   // a hull PhysX had to simplify (over 255 faces) can sit inside the mesh: the exact mesh then
    if (flagged && had != null && had.convex && had.sharedMesh == mf.sharedMesh) { had.convex = false; exactN++; return true; }
    if (mr.GetComponent<UnityEngine.Collider>() != null) return false;
    if (!IsLowWalkable(mr.transform) && WalkIns.MeshInTread(mr, treads)) { skip.Add(mr); inTread.Add(WalkIns.PathOf(mr.transform)); return false; }
    var mc = mr.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; added++;
    if (!IsLowWalkable(mr.transform) && WalkIns.InTread(mc, treads)) { mc.convex = false; exactN++; }
    var p = WalkIns.PathOf(mr.transform).Split('/'); string g = p.Length > 2 ? p[0] + "/" + p[1] + "/" + p[2] : string.Join("/", p); groups[g] = groups.TryGetValue(g, out var k) ? k + 1 : 1; return true;
}
// hulls already standing (an earlier run) that enter a tread: the exact mesh, or none where the mesh itself is in the tread
int treadFixed = 0, treadRemoved = 0, rimRemoved = 0;
foreach (var mc in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshCollider>(UnityEngine.FindObjectsSortMode.None)) { var mf = mc.GetComponent<UnityEngine.MeshFilter>(); if (RimRoot(mc.transform) && mf != null && mf.sharedMesh == mc.sharedMesh) { UnityEngine.Object.DestroyImmediate(mc); rimRemoved++; } }
foreach (var mc in UnityEngine.Object.FindObjectsByType<UnityEngine.MeshCollider>(UnityEngine.FindObjectsSortMode.None))
{
    if (!mc.enabled || IsLowWalkable(mc.transform) || !PassRoot(mc.transform)) continue; var mf = mc.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh != mc.sharedMesh) continue;
    if (!WalkIns.InTread(mc, treads)) continue;
    if (mc.convex) { mc.convex = false; treadFixed++; if (!WalkIns.InTread(mc, treads)) continue; }
    var mr = mc.GetComponent<UnityEngine.MeshRenderer>(); UnityEngine.Object.DestroyImmediate(mc); treadRemoved++; if (mr != null) { skip.Add(mr); inTread.Add(WalkIns.PathOf(mr.transform)); }
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
return "saved=" + saved + " | rim rock hulls removed " + rimRemoved + " | standing hulls in a tread made exact " + treadFixed + ", removed " + treadRemoved + " | meshes in a tread, no collider (move them): " + inTread.Count + (inTread.Count > 0 ? " (" + string.Join(", ", inTread) + ")" : "") + " | convex colliders added " + added + " (" + exactN + " made exact) in " + (pass + 1) + " passes; walk-ins left with no fix " + left + " | " + string.Join(", ", parts);
