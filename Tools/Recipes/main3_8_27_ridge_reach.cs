// Main3 8.27 west ridge reach (edit mode, Main3; the 8.27 gate round 2, Wren 2026-10-03 item A: Marlow's 81 sprints from the far west ridge,
// x 21 to 37, ground 45 to 74, reach the mouth floor; can a player get onto that ridge?). Never saves. A terrain walk over cell m cells from
// every trail point and every warp on the terrain: a step to a neighbour is allowed when it rises no more than the slope limit allows over
// the cell plus the step offset (steeper, the body slides, jumping or not), any fall allowed. Colliders are not counted, so a ridge cell
// this walk never reaches cannot be reached on foot. Prints how many ridge cells (x ridgeX0 to ridgeX1, ground ridgeLow or more) it reaches,
// and the first, with the trail or warp it came from.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var ter = UnityEngine.Terrain.activeTerrain; var td = ter.terrainData; var org = ter.transform.position;
const float cell = 1f, ridgeX0 = 18f, ridgeX1 = 40f, ridgeLow = 40f, ridgeZ0 = 30f, ridgeZ1 = 110f;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var ctl = pc.GetComponent<UnityEngine.CharacterController>();
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
float climb = cell * UnityEngine.Mathf.Tan(ctl.slopeLimit * UnityEngine.Mathf.Deg2Rad) + ctl.stepOffset;   // steeper than the slope limit the body slides, jumping or not
int nx = (int)(td.size.x / cell), nz = (int)(td.size.z / cell);
var h = new float[nx, nz]; for (int i = 0; i < nx; i++) for (int k = 0; k < nz; k++) h[i, k] = ter.SampleHeight(new UnityEngine.Vector3(org.x + (i + 0.5f) * cell, 0f, org.z + (k + 0.5f) * cell)) + org.y;
var seen = new bool[nx, nz]; var from = new string[nx, nz]; var q = new System.Collections.Generic.Queue<(int, int)>();
void Seed(UnityEngine.Vector3 p, string why) { if (p.y < ter.SampleHeight(p) + org.y - 2f) return; int i = (int)((p.x - org.x) / cell), k = (int)((p.z - org.z) / cell);   // underground warps (the cave) are not on the terrain
    if (i < 0 || k < 0 || i >= nx || k >= nz || seen[i, k]) return; seen[i, k] = true; from[i, k] = why; q.Enqueue((i, k)); }
foreach (UnityEngine.Transform lg in Root("Trails").transform) foreach (UnityEngine.Transform p in lg) Seed(p.position, "trail " + lg.name);
foreach (UnityEngine.Transform w in Root("DevWarps").transform) Seed(w.position, "warp " + w.name);
while (q.Count > 0)
{
    var (i, k) = q.Dequeue();
    for (int di = -1; di <= 1; di++) for (int dk = -1; dk <= 1; dk++)
    {
        if (di == 0 && dk == 0) continue; int a = i + di, b = k + dk; if (a < 0 || b < 0 || a >= nx || b >= nz || seen[a, b]) continue;
        float run = (di != 0 && dk != 0) ? 1.414f : 1f; if (h[a, b] - h[i, k] > climb * run) continue;
        seen[a, b] = true; from[a, b] = from[i, k]; q.Enqueue((a, b));
    }
}
int ridge = 0, reached = 0; string first = "none";
for (int i = 0; i < nx; i++) for (int k = 0; k < nz; k++)
{
    float x = org.x + (i + 0.5f) * cell, z = org.z + (k + 0.5f) * cell; if (x < ridgeX0 || x > ridgeX1 || z < ridgeZ0 || z > ridgeZ1 || h[i, k] < ridgeLow) continue;
    ridge++; if (!seen[i, k]) continue; reached++; if (first == "none") first = "(" + x.ToString("F0") + ", " + h[i, k].ToString("F0") + ", " + z.ToString("F0") + ") from " + from[i, k];
}
return "climb per cell " + climb.ToString("F2") + " m (slope limit " + ctl.slopeLimit.ToString("F0") + " degrees plus the step); ridge cells " + ridge + ", reached on foot " + reached + "; first " + first;
