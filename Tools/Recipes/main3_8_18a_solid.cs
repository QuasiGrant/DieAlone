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
// rimRoots (stop visuals): the rim rocks stand on the climb's rim and the ledge lip, and 8.15's stop boulders and logs on its stop boxes
// and hedges; a hull on one is a step over the stop or a new pocket beside it (8.20 checks, 2026-10-01: a jump went over the lip at z 236
// off CS_Rock_6's hull; the hand walk found 32 new BOULDER POCKETS by the trench and south of the camp), so they never get one, and one
// already there goes. Marlow 9 for these is 8.15's: its stops should be the rocks' own hulls, not boxes inside and behind them.
// lowWalkables: the footbridge deck and the stepping stones are walked on: they get a walk deck (WalkDeck below), not hulls, whose
// steps stalled the walker (hand walk 2026-10-01)
// BandScree: the scree on the rock bands (8.1), which are stops; the Camp 2 ladder against the stack (its two rail hulls made a pocket at
// its foot, BOULDER POCKETS 2026-10-01); KnobRock: the bare knob on the crest, which no player reaches (a hull there only makes pockets the BOULDER POCKETS drops find)
string[] laterRoots = { }; string[] rimRoots = { "Rock/ClimbRing/RimBoulders", "Ground815/Stops/", "Rock/KnobRock", "Rock/BandScree", "Campsites/Camp_2/Ladder" }; string[] lowWalkables = { "PointsOfInterest/POI_Footbridge", "PointsOfInterest/POI_Stepping_stones" };
const int maxPasses = 3;
int added = 0, exactN = 0; var groups = new System.Collections.Generic.SortedDictionary<string, int>();
// no collider may close a trail tread (8.20 check, 2026-10-01: hulls of the boulders by the chute gap and in the cleft did): a hull that
// enters one takes the exact mesh; a mesh that enters one as drawn gets no collider and is listed for its owning recipe to move
// (WalkIns.MeshInTread; the walk-into check lists it apart)
var treads = WalkIns.Treads(); var inTread = new System.Collections.Generic.List<string>(); var skip = new System.Collections.Generic.HashSet<UnityEngine.MeshRenderer>();
string[] passRoots = { "Rock/BandScree", "Rock/KnobRock", "Ground815", "Campsites", "PointsOfInterest", "FrontZone", "Places", "Camp/", "Cave/ChamberDressing", "Cave/SideRoom" };   // where this pass adds hulls
bool IsLowWalkable(UnityEngine.Transform t) { var p = WalkIns.PathOf(t); foreach (var r in lowWalkables) if (p.StartsWith(r)) return true; return false; }
// 8.26 (Camp3Layout draft 2, T1): the FaceRock rocks within hollowR of the Camp 3 hollow's centre stand on the floor's edge, not on a stop
// edge, so they are no rim roots: they get hulls, and the walk-into and area checks count them
const float hollowX = 78f, hollowZ = 146f, hollowR = 14f;
bool HollowRock(UnityEngine.Transform t) => WalkIns.PathOf(t).StartsWith("Ground815/Stops/FaceRock") && new UnityEngine.Vector2(t.position.x - hollowX, t.position.z - hollowZ).magnitude <= hollowR;
bool RimRoot(UnityEngine.Transform t) { if (HollowRock(t)) return false; var p = WalkIns.PathOf(t); foreach (var r in rimRoots) if (p.StartsWith(r)) return true; return false; }
bool PassRoot(UnityEngine.Transform t) { var p = WalkIns.PathOf(t); foreach (var r in passRoots) if (p.StartsWith(r)) return true; return false; }
bool Solid(UnityEngine.MeshRenderer mr, bool flagged = true)
{
    if (skip.Contains(mr) || RimRoot(mr.transform) || IsLowWalkable(mr.transform)) return false;
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
// the walk decks: along the trail through each walkable POI, every deckStep m, the walking height is the top of the POI's meshes under the
// centre line (or the ground), joined by collider-only ramps deckWidth wide (the STAIRS RULE); the POI meshes themselves keep no collider
const float deckStep = 0.5f, deckWidth = 1.6f, deckThick = 0.2f, deckReach = 1.5f, deckRise = 0.2f;
int deckRamps = 0;
foreach (var root in lowWalkables)
{
    var t = UnityEngine.GameObject.Find(root); if (t == null) return "no " + root;
    PlaceKit.Remove(t.transform.Find("WalkDeck")); var deck = new UnityEngine.GameObject("WalkDeck").transform; deck.SetParent(t.transform, false);
    var temps = new System.Collections.Generic.List<UnityEngine.GameObject>(); UnityEngine.Bounds bb = default; bool any = false;
    foreach (var mr in t.GetComponentsInChildren<UnityEngine.MeshRenderer>())
    {
        foreach (var mc in mr.GetComponents<UnityEngine.MeshCollider>()) UnityEngine.Object.DestroyImmediate(mc);   // an earlier run's hull
        var mf = mr.GetComponent<UnityEngine.MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue;
        var g = new UnityEngine.GameObject("DeckProbe") { layer = WalkIns.TempLayer }; g.transform.SetPositionAndRotation(mr.transform.position, mr.transform.rotation); g.transform.localScale = mr.transform.lossyScale; g.AddComponent<UnityEngine.MeshCollider>().sharedMesh = mf.sharedMesh; temps.Add(g);
        if (!any) { bb = mr.bounds; any = true; } else bb.Encapsulate(mr.bounds);
    }
    UnityEngine.Physics.SyncTransforms(); bb.Expand(deckReach * 2f);
    var samples = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (UnityEngine.Transform leg in UnityEngine.GameObject.Find("Trails").transform)
    {
        UnityEngine.Vector3? prev = null;
        foreach (UnityEngine.Transform pt in leg)
        {
            var p = pt.position;
            if (prev.HasValue) { int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(UnityEngine.Vector3.Distance(prev.Value, p) / deckStep)); for (int i = 1; i <= n; i++) { var q = UnityEngine.Vector3.Lerp(prev.Value, p, i / (float)n); if (bb.Contains(new UnityEngine.Vector3(q.x, bb.center.y, q.z))) samples.Add(q); } }
            prev = p;
        }
    }
    var ter = UnityEngine.Terrain.activeTerrain; var walk = new System.Collections.Generic.List<UnityEngine.Vector3>();
    foreach (var q in samples)
    {
        float y = ter.SampleHeight(q) + ter.transform.position.y;
        if (UnityEngine.Physics.Raycast(new UnityEngine.Vector3(q.x, q.y + 5f, q.z), UnityEngine.Vector3.down, out var h, 10f, 1 << WalkIns.TempLayer, UnityEngine.QueryTriggerInteraction.Ignore)) y = UnityEngine.Mathf.Max(y, h.point.y);
        walk.Add(new UnityEngine.Vector3(q.x, y, q.z));
    }
    foreach (var g in temps) UnityEngine.Object.DestroyImmediate(g);
    // no rise steeper than deckRise per deckStep (22 degrees): the approach ramps climb to the deck over the ground (hand walk 2026-10-01: the
    // footbridge ends rose 0.4 m in 0.5 m and stalled the walker)
    for (int i = 1; i < walk.Count; i++) walk[i] = new UnityEngine.Vector3(walk[i].x, UnityEngine.Mathf.Max(walk[i].y, walk[i - 1].y - deckRise), walk[i].z);
    for (int i = walk.Count - 2; i >= 0; i--) walk[i] = new UnityEngine.Vector3(walk[i].x, UnityEngine.Mathf.Max(walk[i].y, walk[i + 1].y - deckRise), walk[i].z);
    for (int i = 1; i < walk.Count; i++)
    {
        var a = walk[i - 1]; var b = walk[i]; var along = b - a; if (along.sqrMagnitude < 1e-4f || UnityEngine.Vector3.Distance(a, b) > deckStep * 3f) continue;
        var side = new UnityEngine.Vector3(along.z, 0f, -along.x).normalized; var up = UnityEngine.Vector3.Cross(along, side).normalized; if (up.y < 0f) up = -up;
        var r = new UnityEngine.GameObject("Ramp"); r.transform.SetParent(deck, false); r.transform.SetPositionAndRotation((a + b) * 0.5f - up * deckThick * 0.5f, UnityEngine.Quaternion.LookRotation(along.normalized, up));
        r.AddComponent<UnityEngine.BoxCollider>().size = new UnityEngine.Vector3(deckWidth, deckThick, along.magnitude + 0.05f); deckRamps++;
    }
}
UnityEngine.Physics.SyncTransforms();
int pass = 0, left = 0;
for (; pass < maxPasses; pass++)
{
    left = 0; int now = 0;
    foreach (var w in WalkIns.Find(tuning.capsuleRadius, tuning.standHeight))
    {
        bool later = false; foreach (var lr in laterRoots) if (w.Path.StartsWith(lr)) later = true; if (later) continue;
        if (RimRoot(w.Renderer.transform) || IsLowWalkable(w.Renderer.transform) || skip.Contains(w.Renderer)) continue;   // stop visuals, walk-decked and in-tread meshes get no hull by rule (the walk-into check lists them apart), so they are not "left"
        left++; if (Solid(w.Renderer)) now++; else if (skip.Contains(w.Renderer)) left--;   // drawn into a tread: listed above, not "left"
    }
    if (now == 0) break;
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var parts = new System.Collections.Generic.List<string>(); foreach (var kv in groups) parts.Add(kv.Key + " " + kv.Value);
return "saved=" + saved + " | walk deck ramps " + deckRamps + " | stop visual hulls removed " + rimRemoved + " | standing hulls in a tread made exact " + treadFixed + ", removed " + treadRemoved + " | meshes in a tread, no collider (move them): " + inTread.Count + (inTread.Count > 0 ? " (" + string.Join(", ", inTread) + ")" : "") + " | convex colliders added " + added + " (" + exactN + " made exact) in " + (pass + 1) + " passes; walk-ins left with no fix " + left + " | " + string.Join(", ", parts);
