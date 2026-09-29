// Main3 8.9c re-walk 2 check (Play mode). (1b) walks the camp to pump leg down and back up, timed. (1) Camp re-entry: from each camp leg's trail point about 30 m out, walk the
// markers back to the camp end and on to the clearing edge 12 m from the centre, on the 15 m knoll top. (2) Tower stair: sprint-jumps (5.5 m/s, gravity 20,
// 0.6 m hop, jumping again on every landing, dt 0.02, 3 s) from three points on every flight in six directions (both sides,
// and 45 degrees up and down the flight to both sides) and from every landing in three outward directions. PASS when the
// player never leaves the stair ring (a lane or landing, 2.3 to 3.5 m out from the tower axis) anywhere over 3 m up (below that the open foot of the first flight lets a player step off onto the ground and low braces; a stop there would block the way in), until the ground is reached, up to the
// deck; walking down and off the foot of the first flight is allowed. Leaves runInBackground off and the controller on.
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
        var m = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude);
        cc.Move(new UnityEngine.Vector3(m.x, -0.15f, m.y));
        var q = pc.transform.position; if (s > 400 && (new UnityEngine.Vector2(q.x, q.z) - flat).magnitude < 0.001f) return false;
    }
    return false;
}
var sb = new System.Text.StringBuilder(); bool allOk = true;
// (1) camp re-entry
var camp = new UnityEngine.Vector2(170f, 160f);
foreach (UnityEngine.Transform leg in Root("Trails").transform)
{
    if (leg.childCount < 2) continue;
    var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in leg) pts.Add(m.position);
    UnityEngine.Vector2 F(UnityEngine.Vector3 v) => new UnityEngine.Vector2(v.x, v.z);
    bool endAtCamp = UnityEngine.Vector2.Distance(F(pts[pts.Count - 1]), camp) < 20f, startAtCamp = UnityEngine.Vector2.Distance(F(pts[0]), camp) < 20f;
    if (!endAtCamp && !startAtCamp) continue;
    if (startAtCamp) pts.Reverse();   // now walking toward camp
    int from = 0; for (int i = 0; i < pts.Count; i++) if (UnityEngine.Vector2.Distance(F(pts[i]), camp) <= 30f) { from = i; break; }
    Put(pts[from]); float y0 = pc.transform.position.y; bool ok = true;
    for (int i = from + 1; i < pts.Count && ok; i++) ok = To(pts[i].x, pts[i].z);
    var inCamp = camp + (F(pts[pts.Count - 1]) - camp).normalized * 12f;   // the clearing edge; the tower and cabin stand nearer the centre
    ok = ok && (To(inCamp.x, inCamp.y) || UnityEngine.Vector2.Distance(new UnityEngine.Vector2(pc.transform.position.x, pc.transform.position.z), camp) < 14f) && pc.transform.position.y > 14.4f;   // stopped by the tower foot inside the clearing counts as in allOk &= ok;
    sb.Append("camp re-entry " + leg.name + ": from " + F(pts[from]).ToString("F1") + " ground " + y0.ToString("F1") + " -> " + (ok ? "in camp at " + pc.transform.position.ToString("F1") : "STUCK at " + pc.transform.position.ToString("F1")) + "\n");
}
// (1b) camp to pump both ways (Wren's switchback leg): the clearing edge to the pump end and back, timed at the walk speed
{
    var leg = Root("Trails").transform.Find("Camp to pump"); var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform m in leg) pts.Add(m.position);
    var pumpEnd = new UnityEngine.Vector2(190f, 96f); var edge = camp + (new UnityEngine.Vector2(pts[0].x, pts[0].z) - camp).normalized * 12f;
    float Walk(System.Collections.Generic.IEnumerable<UnityEngine.Vector2> route, out bool ok)
    {
        float dist = 0f; ok = true;
        foreach (var q in route) { var a = pc.transform.position; ok = To(q.x, q.y); var b = pc.transform.position; dist += new UnityEngine.Vector2(b.x - a.x, b.z - a.z).magnitude; if (!ok) break; }
        return dist;
    }
    var down = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (var p in pts) down.Add(new UnityEngine.Vector2(p.x, p.z)); down.Add(pumpEnd);
    var up = new System.Collections.Generic.List<UnityEngine.Vector2>(down); up.Reverse(); up.RemoveAt(0); up.Add(edge);
    Put(new UnityEngine.Vector3(edge.x, 15f, edge.y)); float dDown = Walk(down, out bool okDown); var atPump = pc.transform.position;
    float dUp = okDown ? Walk(up, out bool okUp) : 0f; bool upOk = okDown && pc.transform.position.y > 14.4f && UnityEngine.Vector2.Distance(new UnityEngine.Vector2(pc.transform.position.x, pc.transform.position.z), edge) < 0.5f;
    allOk &= okDown && upOk; float spd = tuning.walkSpeed;
    sb.Append("camp to pump, clearing edge to the pump: " + (okDown ? "reached " + atPump.ToString("F1") : "STUCK at " + atPump.ToString("F1")) + ", " + dDown.ToString("F1") + " m, " + (dDown / spd).ToString("F1") + " s at " + spd + " m/s; back up: " + (upOk ? "reached the clearing at " + pc.transform.position.ToString("F1") : "STUCK at " + pc.transform.position.ToString("F1")) + ", " + dUp.ToString("F1") + " m, " + (dUp / spd).ToString("F1") + " s\n");
}
// (2) tower stair sprint-jumps
var tower = Root("Camp").transform.Find("Tower"); const float c = 2.9f, run = 0.3f; const int flights = 10, steps = 16;
float deckTop = 41f, rise = deckTop / (flights * steps), flightRise = rise * steps;
var cs = new[] { new UnityEngine.Vector3(-c, 0f, -c), new UnityEngine.Vector3(c, 0f, -c), new UnityEngine.Vector3(c, 0f, c), new UnityEngine.Vector3(-c, 0f, c) };
float g = tuning.gravity, vJump = UnityEngine.Mathf.Sqrt(2f * g * tuning.jumpHeight), vRun = tuning.sprintSpeed; const float dt = 0.02f, tMax = 3f;
int tries = 0, fails = 0; float worstOff = 0f; string firstFail = "";
void Jump(UnityEngine.Vector3 localStart, UnityEngine.Vector3 localDir, float floorLocalY, string label)
{
    Put(tower.TransformPoint(localStart + UnityEngine.Vector3.up * 0.2f)); var wdir = tower.TransformDirection(localDir).normalized;
    float vy = vJump, worstRing = 0f; var worstAt = UnityEngine.Vector3.zero;
    for (float t = 0f; t < tMax; t += dt)
    {
        var f = cc.Move(new UnityEngine.Vector3(wdir.x * vRun, vy, wdir.z * vRun) * dt);
        vy -= g * dt; if ((f & UnityEngine.CollisionFlags.Below) != 0) vy = vJump;   // jump again on every landing
        if ((f & UnityEngine.CollisionFlags.Below) != 0 && UnityEngine.Physics.Raycast(pc.transform.position + UnityEngine.Vector3.up * 0.3f, UnityEngine.Vector3.down, out var gh, 0.6f) && gh.collider is UnityEngine.TerrainCollider) break;   // off the foot of the stair onto the ground: done
        var q = tower.InverseTransformPoint(pc.transform.position); float rq = UnityEngine.Mathf.Max(UnityEngine.Mathf.Abs(q.x), UnityEngine.Mathf.Abs(q.z)); float off = UnityEngine.Mathf.Max(2.3f - rq, rq - 3.5f);
        if (q.y > 3f && q.y < deckTop - 0.5f && off > worstRing) { worstRing = off; worstAt = q; }
    }
    bool ok = worstRing <= 0f; tries++; worstOff = UnityEngine.Mathf.Max(worstOff, worstRing);
    if (!ok) { fails++; if (fails <= 15) firstFail += " | " + label + " left the ring by " + worstRing.ToString("F2") + " m at local " + worstAt.ToString("F2"); }
}
for (int k = 0; k < flights; k++)
{
    var from = cs[k % 4]; var to = cs[(k + 1) % 4]; var dir = (to - from).normalized; var side = new UnityEngine.Vector3(dir.z, 0f, -dir.x); float h0 = k * flightRise;
    foreach (int i in new[] { 3, 8, 13 })
    {
        var p = from + dir * (0.5f + run * i + run * 0.5f) + UnityEngine.Vector3.up * (h0 + rise * (i + 1));
        foreach (var sgn in new[] { -1f, 1f })
        {
            Jump(p, side * sgn, h0 + rise * (i + 1), "flight " + (k + 1) + " step " + (i + 1) + " side " + sgn);
            Jump(p, side * sgn + dir, h0 + rise * (i + 1), "flight " + (k + 1) + " step " + (i + 1) + " up-side " + sgn);
            Jump(p, side * sgn - dir, h0 + rise * (i + 1), "flight " + (k + 1) + " step " + (i + 1) + " down-side " + sgn);
        }
    }
    if (k < flights - 1)
    {
        var L = to + UnityEngine.Vector3.up * (h0 + flightRise); var ox = new UnityEngine.Vector3(UnityEngine.Mathf.Sign(to.x), 0f, 0f); var oz = new UnityEngine.Vector3(0f, 0f, UnityEngine.Mathf.Sign(to.z));
        foreach (var d in new[] { ox, oz, ox + oz }) Jump(L, d, h0 + flightRise, "landing " + (k + 1) + " toward " + d);
    }
}
allOk &= fails == 0;
sb.Append("tower sprint-jumps: " + tries + " tries, " + fails + " left the stair; worst " + worstOff.ToString("F2") + " m off the ring" + (firstFail != "" ? "; first: " + firstFail : "") + "\n");
pc.enabled = true; UnityEngine.Application.runInBackground = false;
sb.Append("ALL " + (allOk ? "PASS" : "FAIL"));
return sb.ToString();
