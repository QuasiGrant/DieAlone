// Main3 tower stairs check (8.18a; Marlow's Breaks_2026-10-01.md 12: two waypoint walks stalled at the stair foot on the bottom-tier
// X braces). Play mode; never saves. From the ground west of the first flight, a walk (no sprint, no jump; PlayerController.Step,
// dt 0.02) steered along the stair centre line: each flight's low end to its high end, round each landing, to the top of flight 10, then
// onto the deck. Every flight and landing is read from Camp/Tower/Stairs (their colliders' bounds), so a rebuilt tower is walked as built.
// The walk starts approach m west of the first flight's low end and comes in through the west face's open bottom bay (8.18a).
// FAIL if the walker stalls (moves under stallMove m in stallTime s) or ends under deckMin m.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); bool pcWas = pc.enabled; pc.enabled = false;
var stairs = UnityEngine.GameObject.Find("Camp/Tower/Stairs"); if (stairs == null) return "no Camp/Tower/Stairs";
const float dt = 0.02f, reach = 0.35f, stallTime = 2f, stallMove = 0.2f, maxTime = 240f, approach = 2.5f, deckMin = 55f;
UnityEngine.Bounds B(UnityEngine.Transform t) { var cs = t.GetComponentsInChildren<UnityEngine.Collider>(); var b = cs[0].bounds; foreach (var c in cs) b.Encapsulate(c.bounds); return b; }
// waypoints: the middle of each piece's short side, in order (a flight's long axis runs between its landings)
var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); UnityEngine.Bounds? prev = null;
foreach (UnityEngine.Transform piece in stairs.transform)
{
    var b = B(piece); var c = b.center;
    if (prev.HasValue) pts.Add(new UnityEngine.Vector3(c.x, 0f, c.z));
    else { var lowEnd = new UnityEngine.Vector3(b.min.x, 0f, c.z); pts.Add(lowEnd + UnityEngine.Vector3.left * approach); pts.Add(lowEnd); }   // in through the open west bottom bay (8.2, 8.18a), up the first flight
    prev = b;
}
var last = B(stairs.transform.GetChild(stairs.transform.childCount - 1)); pts.Add(new UnityEngine.Vector3(last.center.x, 0f, last.max.z + 1.5f));   // off the top onto the deck
var ter = UnityEngine.Terrain.activeTerrain; var start = pts[0]; start.y = ter.SampleHeight(start) + ter.transform.position.y + 0.2f;
cc.enabled = false; pc.transform.position = start; cc.enabled = true; UnityEngine.Physics.SyncTransforms();
int wp = 1; float t = 0f, since = 0f; var mark = pc.transform.position; string result = null;
try
{
    while (wp < pts.Count && t < maxTime)
    {
        var p = pc.transform.position; var to = new UnityEngine.Vector3(pts[wp].x - p.x, 0f, pts[wp].z - p.z);
        if (to.magnitude < reach) { wp++; continue; }
        pc.Step(to.normalized, false, false, dt); t += dt; since += dt;
        if (since >= stallTime) { if ((pc.transform.position - mark).magnitude < stallMove) { result = "FAIL stalled at " + pc.transform.position.ToString("F1") + " heading for waypoint " + wp + " of " + (pts.Count - 1) + " " + pts[wp].ToString("F1"); break; } mark = pc.transform.position; since = 0f; }
    }
    if (result == null) result = pc.transform.position.y >= deckMin ? "ALL PASS: walked the stairs to the deck, ends at " + pc.transform.position.ToString("F1") + " in " + t.ToString("F0") + " s" : "FAIL ended at " + pc.transform.position.ToString("F1") + " under the deck (" + deckMin + ")";
}
finally { pc.enabled = pcWas; UnityEngine.Application.runInBackground = false; }
return result;
