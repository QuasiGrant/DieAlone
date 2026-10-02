// Main3 8.21 camp check (Play mode, Main3; CampLayout.md draft 2): the cabin and privy as the doc walks them, with the real mover
// (PlayerController.Step, dt 0.02, walk speed). In main3_review_capture.sh --area camp's Play checks. Never saves; restores the player.
// DOOR: the cabin door stands open into the room at load (Door startOpen, fixedSwing).
// WAKE: Play starts with the player at the wake spot, doc (4.9, 3.2), within wakeSlack m, facing 245 within 5 degrees.
// ROOM: from just inside the door, a walk to the bunk front, the stove front, the desk chair and the shelf front each arrives within arrive m.
// PRIVY: the path from the porch's west end up the cabin's west side (x 173.5) to the privy door, then 1.5 m in through the door,
//   each leg arriving; inside, the player stands on the privy floor.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string n) { foreach (var r in scene.GetRootGameObjects()) if (r.name == n) return r; return null; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
const float dt = 0.02f, arrive = 0.5f, wakeSlack = 0.3f, legTime = 20f, inPrivy = 1.5f;
var C = Root("Camp").transform.Find("Cabin");
UnityEngine.Vector3 Doc(float x, float z) => C.TransformPoint(new UnityEngine.Vector3(x - 3f, 0.1f, z - 2.25f));   // doc cabin-local (X, Z) to world
var sb = new System.Text.StringBuilder(); int fails = 0;
void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
// WAKE (the player has not been moved yet)
var wake = Doc(4.9f, 3.2f); float wd = new UnityEngine.Vector2(start.x - wake.x, start.z - wake.z).magnitude, yawOff = UnityEngine.Mathf.Abs(UnityEngine.Mathf.DeltaAngle(startRot.eulerAngles.y, 245f));
Line(wd <= wakeSlack && yawOff <= 5f, "WAKE: Play starts " + F(wd) + " m from the wake spot, facing " + F(startRot.eulerAngles.y));
// DOOR
var door = C.Find("Door") != null ? C.Find("Door").GetComponent<Door>() : null;
Line(door != null && door.IsOpen && door.CurrentAngle < 0f, "DOOR: " + (door == null ? "no Camp/Cabin/Door" : "open " + door.IsOpen + ", angle " + F(door.CurrentAngle) + " (negative is into the room)"));
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 10; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
bool Walk(UnityEngine.Vector3 to, out float left)
{
    for (float t = 0f; t < legTime; t += dt) { var p = pc.transform.position; var d = new UnityEngine.Vector3(to.x - p.x, 0f, to.z - p.z); if (d.magnitude < arrive * 0.5f) break; pc.Step(d.normalized, false, false, dt); }
    var e = pc.transform.position; left = new UnityEngine.Vector2(e.x - to.x, e.z - to.z).magnitude; return left <= arrive;
}
try
{
    // ROOM
    var inDoor = Doc(3f, 0.6f);
    foreach (var (n, x, z) in new[] { ("bunk front", 4.9f, 3.1f), ("stove front", 0.8f, 2.9f), ("desk chair", 1.4f, 1.6f), ("shelf front", 3.0f, 3.4f) })
    { Put(inDoor); bool ok = Walk(Doc(x, z), out float left); Line(ok, "ROOM: door to the " + n + (ok ? " arrives" : ", stops " + F(left) + " m short at (" + F(pc.transform.position.x) + ", " + F(pc.transform.position.z) + ")")); }
    // PRIVY: porch west end, up the west side at x 173.5, to the door (west face), then in
    var privy = Root("Camp").transform.Find("Privy"); if (privy == null) { Line(false, "PRIVY: no Camp/Privy"); }
    else
    {
        var doorOut = privy.position + privy.forward * 1.6f; var inside = privy.position + privy.forward * (1.6f - inPrivy);
        var legs = new[] { ("porch west end", new UnityEngine.Vector3(174.6f, 0f, 164.6f)), ("west side", new UnityEngine.Vector3(173.5f, 0f, 170f)), ("west side north", new UnityEngine.Vector3(173.5f, 0f, doorOut.z)), ("privy door", doorOut), ("inside the privy", inside) };
        Put(Doc(3f, -1.0f) + new UnityEngine.Vector3(0f, 0.2f, 0f));
        bool all = true; var sbp = new System.Text.StringBuilder();
        foreach (var (n, p) in legs) { bool ok = Walk(p, out float left); sbp.Append(n + (ok ? " ok" : " stops " + F(left) + " m short at (" + F(pc.transform.position.x) + ", " + F(pc.transform.position.z) + ")") + "; "); if (!ok) { all = false; break; } }
        bool onFloor = UnityEngine.Physics.Raycast(pc.transform.position + UnityEngine.Vector3.up * 0.5f, UnityEngine.Vector3.down, out var hit, 1.5f) && hit.collider.transform.IsChildOf(privy);
        Line(all && onFloor, "PRIVY: " + sbp + (all ? (onFloor ? "standing on the privy floor" : "not on the privy floor") : ""));
    }
}
finally { cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false; }
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.21 camp walk (CampLayout.md draft 2)\n" + sb;
