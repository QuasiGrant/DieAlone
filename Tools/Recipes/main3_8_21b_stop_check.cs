// Main3 8.21b stop check (Play mode, Main3; Marlow 822_Marlow.md round 2, items 1 and 2). In main3_review_capture.sh --area camp's Play
// checks; also runs alone. Never saves; restores the player.
// REPROS: Marlow's two, as he ran them: a walk south from (181.67, 13.84, 139.74) off CampStep toward Hedge_Burn_1; sprint-jumps from
//   (171.74, 4.88, 117.84) heading 0 and from (171.53, 5.10, 114.42) heading 180 off TrenchWest toward Hedge_Burn_0.
// RING (Lessons 2026-10-01: a moved trap passes the exact old repro): from every point of a ringStep m grid within ringR m of each
//   repro start that stands on ground or a collider, ringHeadings headings x (walk, sprint, sprint-jump) for moveTime s, each move
//   followed to its landing. FAIL on any end standing on a stop (a collider on Ignore Raycast or under Main3AreaSet.stopRoots).
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.Application.runInBackground = true;
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var set = Main3AreaSet.Load();
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>();
var start = pc.transform.position; var startRot = pc.transform.rotation; bool pcWas = pc.enabled; pc.enabled = false;
const float dt = 0.02f, moveTime = 1.2f, ringR = 3f, ringStep = 0.5f; const int ringHeadings = 16, landSteps = 10, fallSteps = 500;
var sb = new System.Text.StringBuilder(); int fails = 0; void Line(bool ok, string s) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + s + "\n"); }
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int i = 0; i < 25; i++) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
UnityEngine.Vector3 Move(UnityEngine.Vector3 from, UnityEngine.Vector3 dir, bool jump, bool sprint, float time)
{
    Put(from); for (float t = 0f; t < time; t += dt) pc.Step(dir, jump, sprint, dt);
    for (int s = 0; s < landSteps; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
    for (int s = 0; s < fallSteps && !cc.isGrounded; s++) pc.Step(UnityEngine.Vector3.zero, false, false, dt);
    return pc.transform.position;
}
string OnStop(UnityEngine.Vector3 p)
{
    foreach (var h in UnityEngine.Physics.RaycastAll(p + UnityEngine.Vector3.up * 0.3f, UnityEngine.Vector3.down, 0.6f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
    {
        var ht = h.collider.transform; if (ht.IsChildOf(pc.transform) || h.collider is UnityEngine.TerrainCollider) continue; var path = WalkIns.PathOf(ht);
        bool stop = ht.gameObject.layer == 2; if (set.stopRoots != null) foreach (var sr in set.stopRoots) if (path.StartsWith(sr)) stop = true;
        if (stop) return path;
    }
    return null;
}
UnityEngine.Vector3 Dir(float deg) => UnityEngine.Quaternion.Euler(0f, deg, 0f) * UnityEngine.Vector3.forward;
try
{
    // ---- REPROS
    var r1 = Move(V(181.67f, 13.84f + 0.1f, 139.74f), Dir(180f), false, false, 0.7f); var s1 = OnStop(r1);
    Line(s1 == null, "REPRO 1: walk south 0.7 s from (181.67, 13.84, 139.74) ends at (" + F(r1.x) + ", " + F(r1.y) + ", " + F(r1.z) + ")" + (s1 != null ? " ON " + s1 : ", not on a stop"));
    foreach (var (p, hd) in new[] { (V(171.74f, 4.88f + 0.1f, 117.84f), 0f), (V(171.53f, 5.10f + 0.1f, 114.42f), 180f) })
    {
        var e = Move(p, Dir(hd), true, true, moveTime); var s = OnStop(e);
        Line(s == null, "REPRO 2: sprint-jump heading " + F(hd) + " from (" + F(p.x) + ", " + F(p.y - 0.1f) + ", " + F(p.z) + ") ends at (" + F(e.x) + ", " + F(e.y) + ", " + F(e.z) + ")" + (s != null ? " ON " + s : ", not on a stop"));
    }
    // ---- RING round each repro start
    foreach (var c in new[] { V(181.67f, 13.84f, 139.74f), V(171.74f, 4.88f, 117.84f), V(171.53f, 5.10f, 114.42f) })
    {
        int starts = 0, moves = 0; var bad = new System.Collections.Generic.List<string>();
        for (float dx = -ringR; dx <= ringR + 1e-3f; dx += ringStep) for (float dz = -ringR; dz <= ringR + 1e-3f; dz += ringStep)
        {
            if (dx * dx + dz * dz > ringR * ringR) continue;
            // stand on the highest walkable surface there under c.y + 1.5 that is not a stop
            UnityEngine.Vector3? stand = null;
            foreach (var h in UnityEngine.Physics.RaycastAll(V(c.x + dx, c.y + 1.5f, c.z + dz), UnityEngine.Vector3.down, 8f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
            { if (h.normal.y < 0.7f || h.collider.transform.IsChildOf(pc.transform) || OnStop(h.point) != null) continue; if (stand == null || h.point.y > stand.Value.y) stand = h.point; }
            if (stand == null) continue; starts++;
            for (int k = 0; k < ringHeadings; k++) foreach (var (jump, sprint) in new[] { (false, false), (false, true), (true, true) })
            {
                var e = Move(stand.Value + V(0f, 0.1f, 0f), Dir(k * 360f / ringHeadings), jump, sprint, moveTime); moves++; var s = OnStop(e);
                if (s != null && bad.Count < 6) bad.Add("(" + F(stand.Value.x) + ", " + F(stand.Value.z) + ") heading " + F(k * 360f / ringHeadings) + (jump ? " sprint-jump" : sprint ? " sprint" : " walk") + " -> " + s);
                else if (s != null) bad.Add("");
            }
        }
        Line(bad.Count == 0, "RING round (" + F(c.x) + ", " + F(c.z) + "): " + starts + " starts within " + F(ringR) + " m, " + moves + " moves, " + bad.Count + " ending on a stop" + (bad.Count > 0 ? ": " + string.Join("; ", bad.FindAll(x => x != "")) : ""));
    }
}
finally { cc.enabled = false; pc.transform.position = start; pc.transform.rotation = startRot; cc.enabled = true; pc.enabled = pcWas; UnityEngine.Physics.SyncTransforms(); UnityEngine.Application.runInBackground = false; }
return (fails == 0 ? "ALL PASS" : "FAILS " + fails) + ": the 8.21b stop repros (822_Marlow.md round 2, items 1 and 2) and a ring round each\n" + sb;
