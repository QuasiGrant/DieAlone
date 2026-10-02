// Main3 8.20 closure check (Play mode; WardPath.md 3.2 and 3.3, Marlow 16 and 17). Never saves. PlayerController.Step, dt 0.02.
// 1. GIANT: from a 2 m grid south of the fallen giant (the bench x 33 to 59 and leg 4 x 22 to 30, z 262 to 270), walks and sprint-jumps
//    north in 5 headings (north, 30 and 60 degrees either side) for pushTime s; FAIL if one ends north of the giant (z over pastZ) less than
//    pastDrop m under its start (on the old shelf or leg 4; ending lower is a fall down the east drop to the sealed flats, not a way past).
// 2. CORNER: Marlow's slide-pocket pattern at the giant's corner on the face foot (x 33 to 38, z 268 to 271): the player dropped on a
//    0.5 m grid there, settled, then the 48 escapes (16 headings x walk, sprint and sprint-jump, escapeTime s); FAIL if none ends escapeOut
//    m or more away. Also the lookout's north and east edges and the flight edges: every 2 m along them, a step off outward, settled,
//    and the same escapes; FAIL if held (WardPath 3.3: a fall slides to the open bench, no soft lock).
// 3. PROW: walks and sprint-jumps outward from the prow's three railed sides and its corners; FAIL if one ends off the ledge (under 60.5).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
const float dt = 0.02f, pushTime = 3f, pastZ = 274f, pastDrop = 2f, escapeTime = 3f, escapeOut = 3f, settle = 2f; const int headings = 16;
var inv = System.Globalization.CultureInfo.InvariantCulture;
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
float Ground(float x, float y, float z) { if (UnityEngine.Physics.Raycast(V(x, y + 6f, z), UnityEngine.Vector3.down, out var h, 12f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) return h.point.y; return H(x, z); }
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (float s = 0f; s < settle; s += dt) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
UnityEngine.Vector3 Dir(float deg) => UnityEngine.Quaternion.Euler(0f, deg, 0f) * UnityEngine.Vector3.forward;
UnityEngine.Vector3 Move(UnityEngine.Vector3 from, UnityEngine.Vector3 dir, int mode, float time) { cc.enabled = false; pc.transform.position = from; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (float s = 0f; s < time; s += dt) pc.Step(dir, mode == 2, mode >= 1, dt); for (float s = 0f; s < 1f; s += dt) pc.Step(UnityEngine.Vector3.zero, false, false, dt); return pc.transform.position; }
int Escapes(UnityEngine.Vector3 at) { int n = 0; for (int h = 0; h < headings; h++) for (int m = 0; m < 3; m++) { var e = Move(at, Dir(h * 360f / headings), m, escapeTime); if (new UnityEngine.Vector2(e.x - at.x, e.z - at.z).magnitude >= escapeOut) n++; } return n; }
var sb = new System.Text.StringBuilder(); bool pass = true;
try
{
    // 1
    int pushes = 0, past = 0; string firstPast = "";
    foreach (var (x0, x1) in new[] { (33f, 59f), (22f, 30f) })
        for (float x = x0; x <= x1; x += 2f) for (float z = 262f; z <= 270f; z += 2f)
        {
            var start = V(x, Ground(x, H(x, z), z) + 0.3f, z); Put(start); start = pc.transform.position;
            if (start.z > 271f || start.x < x0 - 1f || start.x > x1 + 1f || start.y < (x0 > 32f ? 39f : 56f)) continue;   // slid off the bench or leg 4: not a start
            foreach (var hd in new[] { -60f, -30f, 0f, 30f, 60f }) for (int m = 0; m < 3; m += 2)
            { pushes++; var e = Move(start, Dir(hd), m, pushTime); if (e.z > pastZ && e.y > start.y - pastDrop) { past++; if (firstPast == "") firstPast = " first: " + (m == 0 ? "walk" : "sprint-jump") + " " + hd + " from " + start.ToString("F1") + " ended " + e.ToString("F1"); } }
        }
    if (past > 0) pass = false;
    sb.Append("GIANT: " + pushes + " pushes north from the bench and leg 4, " + past + " past z " + pastZ + (past > 0 ? ": FAIL" + firstPast : ": PASS") + "\n");
    // 2
    int held = 0, spots = 0; string firstHeld = "";
    var drops = new System.Collections.Generic.List<UnityEngine.Vector3>();
    for (float x = 33f; x <= 38f; x += 0.5f) for (float z = 268f; z <= 271f; z += 0.5f) drops.Add(V(x, 0f, z));   // the bench corner at the face foot
    for (float x = 22f; x <= 28f; x += 0.5f) for (float z = 268f; z <= 271f; z += 0.5f) drops.Add(V(x, 0f, z));   // and leg 4 against the giant and the crest face
    var wp = UnityEngine.GameObject.Find("Ward/WardPath/Lookout"); var look = wp != null ? wp.transform.position : V(29.2f, 60f, 261.2f);
    for (float z = 262f; z <= 264f; z += 2f) drops.Add(V(31.8f, 0f, z));                     // off the lookout's east edge
    for (float x = 27f; x <= 31f; x += 2f) drops.Add(V(x, 0f, 264.8f));                       // off its north edge
    foreach (var d in drops)
    {
        float gy = Ground(d.x, 70f, d.z); Put(V(d.x, gy + 0.3f, d.z)); var at = pc.transform.position; spots++;
        int e = Escapes(at); if (e == 0) { held++; if (firstHeld == "") firstHeld = " first: dropped at " + d.ToString("F1") + " settled " + at.ToString("F1"); }
    }
    if (held > 0) pass = false;
    sb.Append("CORNER AND EDGES: " + spots + " drops, " + held + " held (no escape of " + headings * 3 + ")" + (held > 0 ? ": FAIL" + firstHeld : ": PASS") + "\n");
    // 3
    var prow = UnityEngine.GameObject.Find("Ward/WardPath/Prow"); int prowPushes = 0, off = 0; string firstOff = "";
    if (prow == null) { pass = false; sb.Append("PROW: no Ward/WardPath/Prow: FAIL\n"); }
    else
    {
        foreach (var (sx, sz, hd) in new[] { (-11.5f, 248f, 0f), (-11.5f, 246f, 270f), (-11.5f, 244f, 180f), (-11.9f, 248.3f, 315f), (-11.9f, 243.7f, 225f), (-10.8f, 248.3f, 0f), (-10.8f, 243.7f, 180f) })
            foreach (var dh in new[] { -30f, 0f, 30f }) for (int m = 0; m < 3; m++)
            { prowPushes++; Put(V(sx, 62.3f, sz)); var start = pc.transform.position; var e = Move(start, Dir(hd + dh), m, pushTime); if (e.y < 60.5f) { off++; if (firstOff == "") firstOff = " first: from " + start.ToString("F1") + " heading " + (hd + dh) + " mode " + m + " ended " + e.ToString("F1"); } }
        if (off > 0) pass = false;
        sb.Append("PROW: " + prowPushes + " pushes outward, " + off + " off the ledge" + (off > 0 ? ": FAIL" + firstOff : ": PASS") + "\n");
    }
}
finally { pc.enabled = pcWas; UnityEngine.Application.runInBackground = false; }
return (pass ? "ALL PASS" : "FAILS") + ": the Ward path closure\n" + sb;
