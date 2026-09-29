// Main3 8.9f (Play mode): the 13 slice captures (LookSlice.md 6). Poses the player camera on a fixed shot in a look, for
// capture_game_view with source=camera (the look filter draws, the dev label does not) saved to
// Docs/Look/Slice/<Look>_<Shot>.png. Edit shot (1 to 4), lookIndex (0 night, the scene's own look; 1 day one; 2 day two) and
// filterOff (true only for the one filter-off shot; run again with false afterwards to switch the filter back on: the
// look is an asset). The eye sits 1.6 m over the ground or at the given height in the cab; a shot whose eye would sit in
// a collider moves up to 3 m and says so. The controller stays off; leave Play mode after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
int shot = 1, lookIndex = 0; bool filterOff = false;
UnityEngine.Application.runInBackground = true;
var terrain = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
const float eye = 1.6f;
// (position, look-at); S2's y is the ground plus the eye; S3 looks level west
var shots = new (UnityEngine.Vector3 at, UnityEngine.Vector3 look)[] {
    (new UnityEngine.Vector3(156f, 16.6f, 148f), new UnityEngine.Vector3(178f, 16f, 168f)),
    (new UnityEngine.Vector3(235f, float.NaN, 168f), new UnityEngine.Vector3(164f, 38f, 166f)),
    (new UnityEngine.Vector3(164f, 57.6f, 166f), new UnityEngine.Vector3(64f, 57.6f, 166f)),
    // S4 moved 1.8 m east onto the walkway (x 167.8): from the cab's east wall (x 166) the 1.1 m deck rail hides the lot and the
    // burn lane, which sit about 11 degrees down, right at the rail's top (LookSlice 6 allows a move of up to 3 m, recorded here)
    (new UnityEngine.Vector3(167.8f, 57.6f, 166f), new UnityEngine.Vector3(360f, 20f, 185f)) };
var s = shots[shot - 1]; var at = s.at; if (float.IsNaN(at.y)) at.y = H(at.x, at.z) + eye;
string moved = "";
for (float up = 0f; up <= 3f; up += 0.25f) { if (!UnityEngine.Physics.CheckSphere(at + UnityEngine.Vector3.up * up, 0.2f, UnityEngine.Physics.DefaultRaycastLayers, UnityEngine.QueryTriggerInteraction.Ignore)) { if (up > 0f) moved = " (moved up " + up.ToString("F2") + " m to clear a collider)"; at += UnityEngine.Vector3.up * up; break; } }
var preview = UnityEngine.Object.FindFirstObjectByType<LookPreview>(); if (preview != null) preview.Select(lookIndex);
var active = LookOverride.Tuning != null ? LookOverride.Tuning : UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset");
active.filterEnabled = !filterOff;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false; cc.enabled = false;
var cam = UnityEngine.Camera.main; var camLocal = cam.transform.localPosition;
var dir = s.look - at; var flat = new UnityEngine.Vector3(dir.x, 0f, dir.z);
pc.transform.rotation = UnityEngine.Quaternion.LookRotation(flat.normalized);
pc.transform.position = at - pc.transform.rotation * camLocal;   // the camera, not the feet, lands on the shot position
cam.transform.localRotation = UnityEngine.Quaternion.Euler(-UnityEngine.Mathf.Atan2(dir.y, flat.magnitude) * UnityEngine.Mathf.Rad2Deg, 0f, 0f);
return (filterOff ? "FILTER OFF " : "") + "S" + shot + " look " + (preview != null ? preview.CurrentLabel : "scene") + " camera " + cam.transform.position.ToString("F1") + " toward " + s.look + moved;
