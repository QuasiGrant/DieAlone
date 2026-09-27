if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
UnityEngine.Transform cave = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "Cave") cave = r.transform;
float floorY = 24f, headroom = 3.3f; var cc = new UnityEngine.Vector2(300f, 281f);
// Corridor volumes the rocks must stay out of: the tunnel strip (with its wobble) and the chamber disc, up to head height.
bool Intrudes(UnityEngine.Bounds b)
{
    // Tunnel and approach: x within 1.6 of the centre line, z 250..277, y below the ceiling.
    for (float z = UnityEngine.Mathf.Max(b.min.z, 250f); z <= UnityEngine.Mathf.Min(b.max.z, 277f); z += 0.5f)
    {
        float cx = 300f + UnityEngine.Mathf.Sin(z * 0.35f) * 0.5f;
        if (b.max.x > cx - 1.6f && b.min.x < cx + 1.6f && b.min.y < floorY + headroom) return true;
    }
    // Chamber: disc of radius 6.6.
    var closest = new UnityEngine.Vector2(UnityEngine.Mathf.Clamp(cc.x, b.min.x, b.max.x), UnityEngine.Mathf.Clamp(cc.y, b.min.z, b.max.z));
    if (UnityEngine.Vector2.Distance(closest, cc) < 6.6f && b.min.y < floorY + headroom + 0.4f) return true;
    return false;
}
int moved = 0, steps = 0, removed = 0;
foreach (UnityEngine.Transform rock in cave)
{
    var rend = rock.GetComponentInChildren<UnityEngine.Renderer>(); if (rend == null) continue;
    if (!(rock.name.EndsWith("Rock") || rock.name == "ChamberCeiling")) continue;
    bool ceiling = rock.name == "CeilingRock" || rock.name == "ChamberCeiling" || (rock.name == "MouthRock" && rock.position.y > floorY + 3f);
    int n = 0;
    while (Intrudes(rend.bounds) && n < 40)
    {
        if (ceiling) rock.position += UnityEngine.Vector3.up * 0.25f;
        else
        {
            var p = rock.position; UnityEngine.Vector3 dir;
            if (p.z < 277.5f && UnityEngine.Vector2.Distance(new UnityEngine.Vector2(p.x, p.z), cc) > 6.6f) dir = new UnityEngine.Vector3(p.x < 300f + UnityEngine.Mathf.Sin(p.z * 0.35f) * 0.5f ? -1f : 1f, 0f, 0f);
            else { var d = new UnityEngine.Vector2(p.x, p.z) - cc; dir = d.sqrMagnitude < 0.01f ? UnityEngine.Vector3.right : new UnityEngine.Vector3(d.x, 0f, d.y).normalized; }
            rock.position += dir * 0.25f;
        }
        n++; steps++;
    }
    if (n > 0) moved++;
    if (n >= 40) { UnityEngine.Object.DestroyImmediate(rock.gameObject); removed++; }
}
UnityEngine.Physics.SyncTransforms();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " rocksMoved=" + moved + " steps=" + steps + " removed=" + removed;
