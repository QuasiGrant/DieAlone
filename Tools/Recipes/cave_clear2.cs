if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Transform cave = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "Cave") cave = r.transform;
float floorY = 24f; var cc = new UnityEngine.Vector2(300f, 281f);
// Corridor probes: vertical capsules along the tunnel and across the chamber, head height plus a little.
var probes = new System.Collections.Generic.List<(UnityEngine.Vector3 pos, float radius, float height)>();
for (float z = 250f; z <= 277.5f; z += 0.6f) probes.Add((V(300f + UnityEngine.Mathf.Sin(z * 0.35f) * 0.5f, floorY + 1.7f, z), 1.5f, 3.4f));
for (float dx = -6.2f; dx <= 6.2f; dx += 1.0f) for (float dz = -6.2f; dz <= 6.2f; dz += 1.0f) if (dx * dx + dz * dz <= 6.2f * 6.2f) probes.Add((V(cc.x + dx, floorY + 1.9f, cc.y + dz), 1.2f, 3.8f));
var probeGo = new UnityEngine.GameObject("__Probe"); probeGo.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
var cap = probeGo.AddComponent<UnityEngine.CapsuleCollider>();
int moved = 0, pushes = 0, removed = 0;
UnityEngine.Physics.SyncTransforms();
foreach (UnityEngine.Transform rock in cave)
{
    if (!(rock.name.EndsWith("Rock") || rock.name == "ChamberCeiling")) continue;
    var cols = rock.GetComponentsInChildren<UnityEngine.Collider>(); if (cols.Length == 0) continue;
    bool ceiling = rock.name == "CeilingRock" || rock.name == "ChamberCeiling" || (rock.name == "MouthRock" && rock.position.y > floorY + 3f);
    bool any = false; int guard = 0;
    for (int pass = 0; pass < 12; pass++)
    {
        bool hit = false;
        foreach (var pr in probes)
        {
            cap.radius = pr.radius; cap.height = pr.height; probeGo.transform.position = pr.pos;
            foreach (var col in cols)
            {
                UnityEngine.Vector3 dir; float dist;
                if (UnityEngine.Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, cap, pr.pos, UnityEngine.Quaternion.identity, out dir, out dist) && dist > 0.001f)
                {
                    // Push the whole rock out along the penetration direction, keeping wall rocks level and ceiling rocks rising.
                    var push = dir * (dist + 0.05f);
                    if (ceiling) push = UnityEngine.Vector3.up * UnityEngine.Mathf.Max(0.1f, UnityEngine.Vector3.Dot(push, UnityEngine.Vector3.up) > 0f ? push.y : dist * 0.5f);
                    else push.y = 0f;
                    if (push.sqrMagnitude < 0.0001f) push = ceiling ? UnityEngine.Vector3.up * 0.1f : new UnityEngine.Vector3(dir.x, 0f, dir.z).normalized * dist;
                    rock.position += push; UnityEngine.Physics.SyncTransforms(); hit = true; any = true; pushes++; guard++;
                }
            }
        }
        if (!hit) break;
    }
    if (any) moved++;
    if (guard > 60) { UnityEngine.Object.DestroyImmediate(rock.gameObject); removed++; }
}
UnityEngine.Object.DestroyImmediate(probeGo);
UnityEngine.Physics.SyncTransforms();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " rocksMoved=" + moved + " pushes=" + pushes + " removed=" + removed + " probes=" + probes.Count;
