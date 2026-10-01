// Main3 warp seat (8.18a; Grant 2026-10-01: warps that fall through the map). Edit mode, Main3; rerunnable; in the runner after the
// last recipe that changes ground or places (before the sightlines, which start rays at the warps). 8.1 sets every warp 0.2 m over its
// terrain, and later recipes raise the ground under some (8.14 carving, 8.15 ground, 8.17 places): a warp left under the surface
// drops the player through the terrain collider (Junction_J 1.3 m and Cave_Mouth 1.4 m under, 2026-10-01).
// Each warp is set seatLift m over the first solid collider found by a ray down from probeUp m over it (under any roof or deck above
// it, so warps inside the cabin, store and booth stay inside), and is left where it is if no collider is found within probeDown m.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject warps = null; foreach (var r in scene.GetRootGameObjects()) if (r.name == "DevWarps") warps = r; if (warps == null) return "no DevWarps root";
const float seatLift = 0.2f, probeUp = 2f, probeDown = 12f, moveNote = 0.05f;
UnityEngine.Physics.SyncTransforms();
var moved = new System.Collections.Generic.List<string>(); var none = new System.Collections.Generic.List<string>(); int n = 0;
foreach (UnityEngine.Transform w in warps.transform)
{
    n++; var from = w.position + UnityEngine.Vector3.up * probeUp;
    if (!UnityEngine.Physics.Raycast(from, UnityEngine.Vector3.down, out var hit, probeUp + probeDown, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { none.Add(w.name); continue; }
    float y = hit.point.y + seatLift; if (UnityEngine.Mathf.Abs(y - w.position.y) < moveNote) continue;
    moved.Add(w.name + " " + w.position.y.ToString("F2") + " to " + y.ToString("F2") + " on " + hit.collider.name);
    w.position = new UnityEngine.Vector3(w.position.x, y, w.position.z);
}
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | " + n + " warps; seated " + moved.Count + (moved.Count > 0 ? " (" + string.Join(", ", moved) + ")" : "") + "; no ground under: " + (none.Count == 0 ? "none" : string.Join(", ", none));
