// Main3 8.9e re-walk check (Play mode), Marlow's re-walk 4 repros (Docs/Review/2026-09-29-Main3Walk-8.9c-checklist.md):
// (a) Wall_100: from (86.6, 51.3) toward (72.2, 30.8), jumping on every landing, at 2.5, 4 and 5.5 m/s for 8 s; fails if
// the player ends in the ravine or over the cave off the trails, or in the rim pocket at (82.2, 47). (b) Rim pocket: W1 to
// cave points 22 to 29, sprint-jumps in eight directions; fails on ending within 2.5 m of (82.2, 47) above 2 m. (c) Cairn
// gate by day: from (107.2, 203.3), (110, 208), (113.5, 205.6), (121.5, 198.8) toward climb point (99.5, 209.9), sprint-
// jumping 6 s; fails if the player ends on the climb past the gate. (d) Switchbacks: from every climb point up to the pass,
// sprint-jumps and walks 4 m straight out to both sides (points within 3 of a turn left out); fails if one lands on another part of the climb (over 8 m of trail
// away) or drops more than 2 m. (e) Boathouse: from Pump to boathouse point 38 walking toward the lake centre; fails if
// it ends in the bank pocket. Leaves runInBackground off and the controller on.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var ter = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => ter.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + ter.transform.position.y;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
void PutXZ(float x, float z) => Put(new UnityEngine.Vector3(x, H(x, z), z));
bool To(float x, float z)
{
    var t = new UnityEngine.Vector2(x, z);
    for (int s = 0; s < 8000; s++)
    {
        var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z); var d = t - flat; if (d.magnitude < 0.25f) return true;
        var m = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude); cc.Move(new UnityEngine.Vector3(m.x, -0.15f, m.y));
        var q = pc.transform.position; if (s > 400 && (new UnityEngine.Vector2(q.x, q.z) - flat).magnitude < 0.001f) return false;
    }
    return false;
}
float g = tuning.gravity, vJump = UnityEngine.Mathf.Sqrt(2f * g * tuning.jumpHeight); const float dt = 0.02f;
float Hop(UnityEngine.Vector3 dir, float speed, float seconds)   // returns the lowest height reached
{
    float vy = vJump, low = pc.transform.position.y;
    for (float t = 0f; t < seconds; t += dt) { var f = cc.Move(new UnityEngine.Vector3(dir.x * speed, vy, dir.z * speed) * dt); vy -= g * dt; if ((f & UnityEngine.CollisionFlags.Below) != 0) vy = vJump; low = UnityEngine.Mathf.Min(low, pc.transform.position.y); }
    return low;
}
UnityEngine.Vector3 Dir(float x0, float z0, float x1, float z1) => new UnityEngine.Vector3(x1 - x0, 0f, z1 - z0).normalized;
var sb = new System.Text.StringBuilder(); bool allOk = true;
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>();
foreach (UnityEngine.Transform leg in Root("Trails").transform) foreach (UnityEngine.Transform m in leg) trailPts.Add(new UnityEngine.Vector2(m.position.x, m.position.z));
float NearTrail(UnityEngine.Vector3 p) { float best = float.MaxValue; var q = new UnityEngine.Vector2(p.x, p.z); foreach (var t in trailPts) best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(q, t)); return best; }
var rimPocket = new UnityEngine.Vector2(82.2f, 47f);
bool InRimPocket(UnityEngine.Vector3 e) => UnityEngine.Vector2.Distance(new UnityEngine.Vector2(e.x, e.z), rimPocket) < 2.5f && e.y > 2f;
bool OverCave(UnityEngine.Vector3 e) => !(e.x > 49f && e.x < 55f && e.z > 29.5f && e.z < 41.5f) && NearTrail(e) > 4.5f && e.x > 44f && e.x < 92f && e.z > 3f && e.z < 48f && e.y < 3f;
// (a) Wall_100, Marlow's exact repro
{
    int bad = 0; string what = "";
    foreach (var spd in new[] { 2.5f, 4f, 5.5f }) { PutXZ(86.6f, 51.3f); Hop(Dir(86.6f, 51.3f, 72.2f, 30.8f), spd, 8f); var e = pc.transform.position; if (OverCave(e) || InRimPocket(e)) { bad++; what += " " + spd + " m/s ended " + e.ToString("F1"); } }
    allOk &= bad == 0; sb.Append("(a) Wall_100 repro: " + (bad == 0 ? "held at 2.5, 4 and 5.5 m/s" : bad + " got past:" + what) + "\n");
}
// (b) rim pocket
{
    var cave = Root("Trails").transform.Find("W1 to cave"); int tries = 0, bad = 0;
    for (int i = 22; i <= 29 && i < cave.childCount; i++)
        for (int a = 0; a < 360; a += 45) { Put(cave.GetChild(i).position); Hop(UnityEngine.Quaternion.Euler(0f, a, 0f) * UnityEngine.Vector3.forward, tuning.sprintSpeed, 3f); tries++; if (InRimPocket(pc.transform.position)) bad++; }
    allOk &= bad == 0; sb.Append("(b) rim pocket: " + bad + " of " + tries + " sprint-jumps end in it\n");
}
// (c) cairn gate by day
var climb = Root("Trails").transform.Find("J to Ward"); var cl = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in climb) cl.Add(m.position);
int NearClimb(UnityEngine.Vector3 p, out float dist) { int bi = 0; dist = float.MaxValue; for (int i = 0; i < cl.Count; i++) { float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(p.x, p.z), new UnityEngine.Vector2(cl[i].x, cl[i].z)); if (d < dist && UnityEngine.Mathf.Abs(p.y - cl[i].y) < 2.5f) { dist = d; bi = i; } } return bi; }
{
    int bad = 0; string what = "";
    foreach (var s in new[] { new UnityEngine.Vector2(107.2f, 203.3f), new UnityEngine.Vector2(110f, 208f), new UnityEngine.Vector2(113.5f, 205.6f), new UnityEngine.Vector2(121.5f, 198.8f) })
        foreach (var spd in new[] { 4f, 5.5f })
        {
            PutXZ(s.x, s.y); Hop(Dir(s.x, s.y, 99.5f, 209.9f), spd, 6f); var e = pc.transform.position;
            int ci = NearClimb(e, out float cd); if (cd < 2.5f && ci > 3) { bad++; if (what == "") what = " first from " + s + " at " + spd + " m/s: climb point " + ci + " " + e.ToString("F1"); }
        }
    allOk &= bad == 0; sb.Append("(c) gate by day: " + bad + " of 8 sprint-jumps reach the climb past the gate" + what + "\n");
}
// (d) switchbacks: jump or walk off a shelf
{
    int passIdx = 0; float bd = float.MaxValue; for (int i = 0; i < cl.Count; i++) { float d = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(cl[i].x, cl[i].z), new UnityEngine.Vector2(112f, 262f)); if (d < bd) { bd = d; passIdx = i; } }
    var gate = Root("Ward").transform.Find("CairnGate"); gate.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
    int tries = 0, skips = 0, drops = 0; float worstDrop = 0f; string first = "";
    bool NearTurn(int i) { for (int k = UnityEngine.Mathf.Max(1, i - 3); k <= UnityEngine.Mathf.Min(cl.Count - 2, i + 3); k++) { var u0 = Dir(cl[k - 1].x, cl[k - 1].z, cl[k].x, cl[k].z); var u1 = Dir(cl[k].x, cl[k].z, cl[k + 1].x, cl[k + 1].z); if (UnityEngine.Vector3.Dot(u0, u1) < 0.7f) return true; } return false; }
    int turnsSkipped = 0;
    for (int i = 3; i < passIdx; i++)
    {
        if (NearTurn(i)) { turnsSkipped++; continue; }   // at a turn "sideways" runs along the other leg, which is the trail
        var a = cl[i]; var b = cl[i + 1]; var along = Dir(a.x, a.z, b.x, b.z); var side = new UnityEngine.Vector3(along.z, 0f, -along.x);
        foreach (var sg in new[] { -1f, 1f })
            foreach (var jump in new[] { true, false })
            {
                Put(a); float y0 = pc.transform.position.y; float low;
                if (jump) low = Hop(side * sg, tuning.sprintSpeed, 3f);
                else { var w = a + side * sg * 4f; To(w.x, w.z); low = pc.transform.position.y; for (int k = 0; k < 30; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); low = UnityEngine.Mathf.Min(low, pc.transform.position.y); }
                tries++; var e = pc.transform.position; float drop = y0 - low; worstDrop = UnityEngine.Mathf.Max(worstDrop, drop);
                int ci = NearClimb(e, out float cd);
                if (cd < 2.5f && UnityEngine.Mathf.Abs(ci - i) > 4) { skips++; first += " [" + (jump ? "jump" : "walk") + " " + i + " to " + ci + ", " + (e.y - y0).ToString("F1") + " m]"; }
                else if (drop > 2f) { drops++; if (first == "") first = " first drop: point " + i + " " + drop.ToString("F1") + " m"; }
            }
    }
    gate.gameObject.SetActive(true); UnityEngine.Physics.SyncTransforms();
    allOk &= skips == 0 && drops == 0;
    sb.Append("(d) switchbacks: " + tries + " jumps and walk-offs from " + (passIdx - 3 - turnsSkipped) + " points (" + turnsSkipped + " at the turns left out), " + skips + " skip to another leg, " + drops + " drop over 2 m (worst " + worstDrop.ToString("F1") + ")" + first + "\n");
}
// (e) boathouse pocket from the north
{
    var pb = Root("Trails").transform.Find("Pump to boathouse"); var s = pb.GetChild(UnityEngine.Mathf.Min(38, pb.childCount - 1)).position;
    Put(s); To(190f, 60f); var e = pc.transform.position; bool inPocket = e.x > 243f && e.x < 245.2f && e.z > 53f && e.z < 56.5f && e.y < -4.2f;
    allOk &= !inPocket; sb.Append("(e) boathouse from Pump to boathouse point 38 toward the lake: ended " + e.ToString("F1") + (inPocket ? " IN THE POCKET" : ", not in the pocket") + "\n");
}
pc.enabled = true; UnityEngine.Application.runInBackground = false;
sb.Append("ALL " + (allOk ? "PASS" : "FAIL"));
return sb.ToString();
