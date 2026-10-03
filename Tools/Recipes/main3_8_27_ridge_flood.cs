// Main3 8.27 west ridge flood (Play mode, Main3; the 8.27 gate round 2, Wren 2026-10-03 items A and B). Never saves; restores the player.
// The area check's flood (the real body, PlayerController.Step; floodHeadings headings x walk and sprint-jump moves of floodMoveTime s, a
// place per floodCell m cell and floodLevel m level) seeded from every trail point and every warp on the terrain within the region (x
// rx0 to rx1, z rz0 to rz1: the cave's rim plateau, the west ridge and the ground round them), not past it. Prints how many places it
// reaches on the west ridge (x under ridgeX1, ground ridgeLow or more: Marlow's 81 sprints to the mouth start there), and how many stand on a
// Hedge_CaveRim box (Marlow's 93 cells), with the first of each. A ridge or hedge top this flood never reaches cannot be reached on foot.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var inv = System.Globalization.CultureInfo.InvariantCulture; string P3(UnityEngine.Vector3 p) => "(" + p.x.ToString("F1", inv) + ", " + p.y.ToString("F1", inv) + ", " + p.z.ToString("F1", inv) + ")";
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, rx0 = 0f, rx1 = 160f, rz0 = 0f, rz1 = 220f, ridgeX1 = 38f, ridgeLow = 40f; const int landSteps = 10, settleSteps = 3, fallSteps = 500, maxPlaces = 60000;
var set = Main3AreaSet.Load(); var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
var hedge = Root("Ground815").transform.Find("Stops/Hedge_CaveRim");
long Key(UnityEngine.Vector3 p) { long i = UnityEngine.Mathf.FloorToInt(p.x / set.floodCell) + 10000, j = UnityEngine.Mathf.FloorToInt(p.z / set.floodCell) + 10000, k = UnityEngine.Mathf.FloorToInt(p.y / set.floodLevel) + 1000; return (i * 20000L + j) * 4000L + k; }
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int s = 0; s < settleSteps; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
UnityEngine.Vector3 Move(UnityEngine.Vector3 from, UnityEngine.Vector3 dir, bool jump, bool sprint, float time) { Put(from); for (float t = 0f; t < time; t += dt) pc.Step(dir, jump, sprint, dt); for (int s = 0; s < landSteps; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); for (int s = 0; s < fallSteps && !cc.isGrounded; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); return pc.transform.position; }
bool In(UnityEngine.Vector3 p) => p.x >= rx0 && p.x <= rx1 && p.z >= rz0 && p.z <= rz1;
bool OnHedge(UnityEngine.Vector3 p) { if (hedge == null) return false; if (UnityEngine.Physics.Raycast(p + UnityEngine.Vector3.up * 0.3f, UnityEngine.Vector3.down, out var h, 0.8f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) return h.collider.transform.IsChildOf(hedge); return false; }
var pos = new System.Collections.Generic.Dictionary<long, UnityEngine.Vector3>(); var q = new System.Collections.Generic.Queue<long>(); int moves = 0, seeds = 0;
int ridge = 0, onHedge = 0; string firstRidge = "none", firstHedge = "none";
try
{
    void Seed(UnityEngine.Vector3 p) { if (!In(p) || p.y < H(p.x, p.z) - 2f) return; Put(p + UnityEngine.Vector3.up * 0.5f); for (int s = 0; s < landSteps * 5; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); var e = pc.transform.position; long k = Key(e); if (pos.ContainsKey(k)) return; pos[k] = e; q.Enqueue(k); seeds++; }
    foreach (UnityEngine.Transform lg in Root("Trails").transform) foreach (UnityEngine.Transform p in lg) Seed(p.position);
    foreach (UnityEngine.Transform w in Root("DevWarps").transform) Seed(w.position);
    while (q.Count > 0 && pos.Count < maxPlaces)
    {
        var from = pos[q.Dequeue()];
        for (int hd = 0; hd < set.floodHeadings; hd++) foreach (var jump in new[] { false, true })
        {
            var e = Move(from, UnityEngine.Quaternion.Euler(0f, hd * 360f / set.floodHeadings, 0f) * UnityEngine.Vector3.forward, jump, jump, set.floodMoveTime); moves++;
            long ek = Key(e); if (pos.ContainsKey(ek)) continue; pos[ek] = e; if (!In(e)) continue;
            if (e.x < ridgeX1 && e.y >= ridgeLow) { ridge++; if (firstRidge == "none") firstRidge = P3(e) + " from " + P3(from); }
            if (OnHedge(e)) { onHedge++; if (firstHedge == "none") firstHedge = P3(e) + " from " + P3(from); }
            q.Enqueue(ek);
        }
    }
}
finally { cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false; }
return "flood over x " + rx0 + " to " + rx1 + ", z " + rz0 + " to " + rz1 + ": " + pos.Count + " places from " + seeds + " seeds, " + moves + " moves" + (pos.Count >= maxPlaces ? " (STOPPED at the cap)" : "") + "; on the west ridge " + ridge + " (first " + firstRidge + "); on a hedge box " + onHedge + " (first " + firstHedge + ")";
