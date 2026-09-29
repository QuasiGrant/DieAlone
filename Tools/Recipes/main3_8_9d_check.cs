// Main3 8.9d check (Play mode): the dressed cabin still lets the player out. From the bunk spawn, open the cabin door
// (Door.Use; run the check twice, the door swings over frames) and walk the aisle, out the door, across the porch to the fire pit and back in; then lists the practical
// lights and whether each is on in the current look. Leaves runInBackground off and the controller on.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var cabin = Root("Camp").transform.Find("Cabin"); var door = cabin.Find("Door").GetComponent<Door>();
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
bool To(UnityEngine.Vector3 w)
{
    var t = new UnityEngine.Vector2(w.x, w.z);
    for (int s = 0; s < 8000; s++)
    {
        var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z); var d = t - flat; if (d.magnitude < 0.25f) return true;
        var m = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude); cc.Move(new UnityEngine.Vector3(m.x, -0.15f, m.y));
        var q = pc.transform.position; if (s > 400 && (new UnityEngine.Vector2(q.x, q.z) - flat).magnitude < 0.001f) return false;
    }
    return false;
}
var sb = new System.Text.StringBuilder(); bool ok = true;
// the door swings over frames and stops on anything in its sweep (the player too): first run opens it, second run walks
if (!door.IsOpen || door.CurrentAngle < 60f) { Put(cabin.TransformPoint(new UnityEngine.Vector3(-1.2f, 0.1f, 0.9f))); var user = pc.GetComponent<PlayerInteractor>(); if (door.IsOpen) door.Use(user); door.Use(user); pc.enabled = true; return "door opening (angle " + door.CurrentAngle.ToString("F0") + "); run again"; }
Put(cabin.TransformPoint(new UnityEngine.Vector3(-1.2f, 0.1f, 0.9f)));   // beside the bunk, where the player wakes
var route = new[] { new UnityEngine.Vector3(0f, 0f, 0.5f), new UnityEngine.Vector3(0f, 0f, -1.5f), new UnityEngine.Vector3(0f, 0f, -3.4f), new UnityEngine.Vector3(0f, 0f, -5.2f) };
foreach (var lp in route) { if (!To(cabin.TransformPoint(lp))) { ok = false; sb.Append("STUCK at " + pc.transform.position.ToString("F2") + " heading to cabin-local " + lp + "\n"); break; } }
// round the porch's west end to the open south-east side of the fire ring (the chair and a seat log sit either side)
var pit = Root("Camp").transform.Find("FirePit").position; var nearPit = pit + new UnityEngine.Vector3(1.9f, 0f, -2.5f);
if (ok && !To(cabin.TransformPoint(new UnityEngine.Vector3(-2.0f, 0f, -6.5f)))) { ok = false; sb.Append("STUCK past the porch at " + pc.transform.position.ToString("F2") + "\n"); }
if (ok) { ok = To(nearPit); sb.Append(ok ? "out of the cabin and to the fire pit: reached " + pc.transform.position.ToString("F1") + "\n" : "STUCK before the fire pit at " + pc.transform.position.ToString("F1") + "\n"); }
if (ok) { foreach (var lp in new[] { new UnityEngine.Vector3(-2.0f, 0f, -6.5f), new UnityEngine.Vector3(0f, 0f, -5.2f), new UnityEngine.Vector3(0f, 0f, -1.5f), new UnityEngine.Vector3(0f, 0f, 0.8f) }) if (ok && !To(cabin.TransformPoint(lp))) { ok = false; sb.Append("STUCK coming back at " + pc.transform.position.ToString("F2") + "\n"); } }
if (ok) sb.Append("back inside at " + pc.transform.position.ToString("F1") + "\n");
foreach (var pl in UnityEngine.Object.FindObjectsByType<PracticalLight>(UnityEngine.FindObjectsSortMode.None)) { var l = pl.GetComponent<UnityEngine.Light>(); sb.Append(pl.name + " " + (l.enabled ? "on" : "off") + " colour " + l.color.ToString("F2") + " intensity " + l.intensity.ToString("F2") + "; "); }
pc.enabled = true; UnityEngine.Application.runInBackground = false;
sb.Append("\nALL " + (ok ? "PASS" : "FAIL"));
return sb.ToString();
