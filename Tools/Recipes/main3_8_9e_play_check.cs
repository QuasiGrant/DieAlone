// Main3 8.9e check (Play mode), the revision 15 leftovers: (1) Wall_100: sprint-jumps (5.5 m/s, 0.6 m hop on every landing,
// 3 s) from the W1 to cave trail points within 12 m of (86.6, 51.3), aimed at (72.2, 30.8) and 20 and 40 degrees either
// side; a jump fails if it ends in the ravine east of the mouth or on the ground over the cave (x 44 to 92, z 3 to 48, below 3 m,
// outside the cave mouth approach), as Marlow found; ends more than 4.5 m from any trail point elsewhere are listed as pockets. (2) Camp 2:
// both legs walked to their ends at the ramp foot (298.9, 107.8). (3) Boathouse pocket: walking and hopping west from
// (247.2, 54.9) must not drop into the pocket (the fill holds the player level, lowest over -4.2). (4) Tower deck: the walkway walked all round at 3.9 m from the
// centre, east side past the hatch, without dropping. Leaves runInBackground off and the controller on.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
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
void Hop(UnityEngine.Vector3 dir, float speed, float seconds)
{
    float vy = vJump; for (float t = 0f; t < seconds; t += dt) { var f = cc.Move(new UnityEngine.Vector3(dir.x * speed, vy, dir.z * speed) * dt); vy -= g * dt; if ((f & UnityEngine.CollisionFlags.Below) != 0) vy = vJump; }
}
var sb = new System.Text.StringBuilder(); bool allOk = true;
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>();
foreach (UnityEngine.Transform leg in Root("Trails").transform) foreach (UnityEngine.Transform m in leg) trailPts.Add(new UnityEngine.Vector2(m.position.x, m.position.z));
float NearTrail(UnityEngine.Vector3 p) { float best = float.MaxValue; var q = new UnityEngine.Vector2(p.x, p.z); foreach (var t in trailPts) best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(q, t)); return best; }
// (1) Wall_100
{
    var cave = Root("Trails").transform.Find("W1 to cave"); int tries = 0, fails = 0, pockets = 0; string first = "", pocketAt = "";
    var aim = new UnityEngine.Vector2(72.2f, 30.8f);
    foreach (UnityEngine.Transform m in cave)
    {
        var mp = new UnityEngine.Vector2(m.position.x, m.position.z); if (UnityEngine.Vector2.Distance(mp, new UnityEngine.Vector2(86.6f, 51.3f)) > 12f) continue;
        foreach (var turn in new[] { -40f, -20f, 0f, 20f, 40f })
        {
            Put(m.position); var d0 = (aim - mp).normalized; var dir = UnityEngine.Quaternion.Euler(0f, turn, 0f) * new UnityEngine.Vector3(d0.x, 0f, d0.y);
            Hop(dir, tuning.sprintSpeed, 3f); tries++;
            var e = pc.transform.position; bool approach = e.x > 49f && e.x < 55f && e.z > 29.5f && e.z < 41.5f;
            bool ravine = !approach && NearTrail(e) > 4.5f && e.x > 44f && e.x < 92f && e.z > 3f && e.z < 48f && e.y < 3f;
            if (ravine) { fails++; if (first == "") first = " first: from " + mp.ToString("F1") + " turn " + turn + " ended " + e.ToString("F1"); }
            else if (NearTrail(e) > 4.5f && !approach) { pockets++; if (pocketAt == "") pocketAt = " (first pocket " + e.ToString("F1") + ")"; }
        }
    }
    allOk &= fails == 0; sb.Append("Wall_100 sprint-jumps: " + tries + " tries, " + fails + " into the ravine or over the cave" + first + "; " + pockets + " end off-trail elsewhere" + pocketAt + "\n");
}
// (2) Camp 2 legs end at the ramp foot
foreach (var name in new[] { "Boathouse to Camp 2", "Camp 2 to T" })
{
    var leg = Root("Trails").transform.Find(name); var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in leg) pts.Add(m.position);
    if (name == "Camp 2 to T") pts.Reverse();   // walk toward Camp 2 on both
    Put(pts[0]); bool ok = true; for (int i = 1; i < pts.Count && ok; i++) ok = To(pts[i].x, pts[i].z);
    ok = ok && To(298.9f, 107.8f); allOk &= ok;
    sb.Append(name + " to the Camp 2 ramp foot: " + (ok ? "reached " + pc.transform.position.ToString("F1") : "STUCK at " + pc.transform.position.ToString("F1")) + "\n");
}
// (3) boathouse pocket
{
    float minX = float.MaxValue, lowY = float.MaxValue;
    foreach (var mode in new[] { 0, 1 })
    {
        var ter = UnityEngine.Terrain.activeTerrain; Put(new UnityEngine.Vector3(247.2f, ter.SampleHeight(new UnityEngine.Vector3(247.2f, 0f, 54.9f)) + ter.transform.position.y, 54.9f));
        if (mode == 0) To(242f, 55f); else Hop(new UnityEngine.Vector3(-1f, 0f, 0f), tuning.sprintSpeed, 3f);
        minX = UnityEngine.Mathf.Min(minX, pc.transform.position.x); lowY = UnityEngine.Mathf.Min(lowY, pc.transform.position.y);
    }
    bool ok = lowY > -4.2f; allOk &= ok; sb.Append("boathouse pocket: westmost x " + minX.ToString("F2") + ", lowest " + lowY.ToString("F2") + " (" + (ok ? "held level over the pocket" : "DROPPED IN") + ")\n");
}
// (4) tower deck walkway, all round
{
    var tower = Root("Camp").transform.Find("Tower"); const float deckTop = 41f, ring = 3.9f;
    var corners = new[] { new UnityEngine.Vector3(-ring, deckTop, -ring), new UnityEngine.Vector3(ring, deckTop, -ring), new UnityEngine.Vector3(ring, deckTop, ring), new UnityEngine.Vector3(-ring, deckTop, ring) };
    Put(tower.TransformPoint(corners[0] + UnityEngine.Vector3.up * 0.2f)); bool ok = true; float lowest = float.MaxValue;
    for (int k = 1; k <= 4 && ok; k++) { var w = tower.TransformPoint(corners[k % 4]); ok = To(w.x, w.z); lowest = UnityEngine.Mathf.Min(lowest, tower.InverseTransformPoint(pc.transform.position).y); }
    ok = ok && lowest > deckTop - 0.5f; allOk &= ok;
    sb.Append("tower walkway all round at " + ring + " m: " + (ok ? "walked, lowest " + lowest.ToString("F2") : "FAILED at local " + tower.InverseTransformPoint(pc.transform.position).ToString("F2")) + "\n");
}
pc.enabled = true; UnityEngine.Application.runInBackground = false;
sb.Append("ALL " + (allOk ? "PASS" : "FAIL"));
return sb.ToString();
