// Main3 breaks recheck (8.18a; Marlow's Breaks_2026-10-01.md traps 1 to 5, 7c and 7d; 6, 7a and 7b are behind the 8.20 giant). Play mode; never saves. For each trap: Marlow's repro move
// (from his start, walk, sprint or sprint-jump on his heading for reproTime s), then from where that ends and from each of his trap spots
// (dropped 0.3 m over the ground there and settled), the 48 escapes (16 headings x walk, sprint and sprint-jump, escapeTime s each).
// A spot is a trap when no escape ends escapeOut m or more from it. PlayerController.Step, dt 0.02, as in the other checks.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); bool pcWas = pc.enabled; pc.enabled = false;
var ter = UnityEngine.Terrain.activeTerrain;
const float dt = 0.02f, settle = 2f, escapeTime = 3f, escapeOut = 3f, reproTime = 3f; const int headings = 16;
var inv = System.Globalization.CultureInfo.InvariantCulture;
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
float Ground(float x, float y, float z) { if (UnityEngine.Physics.Raycast(V(x, y + 6f, z), UnityEngine.Vector3.down, out var h, 10f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) return h.point.y; return ter.SampleHeight(V(x, 0f, z)) + ter.transform.position.y; }
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (float s = 0f; s < settle; s += dt) pc.Step(UnityEngine.Vector3.zero, false, false, dt); }
UnityEngine.Vector3 Dir(float deg) => UnityEngine.Quaternion.Euler(0f, deg, 0f) * UnityEngine.Vector3.forward;
UnityEngine.Vector3 Move(UnityEngine.Vector3 from, UnityEngine.Vector3 dir, int mode, float time) { cc.enabled = false; pc.transform.position = from; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (float s = 0f; s < time; s += dt) pc.Step(dir, mode == 2, mode >= 1, dt); for (float s = 0f; s < 1f; s += dt) pc.Step(UnityEngine.Vector3.zero, false, false, dt); return pc.transform.position; }
int Escapes(UnityEngine.Vector3 at) { int n = 0; for (int h = 0; h < headings; h++) for (int m = 0; m < 3; m++) { var e = Move(at, Dir(h * 360f / headings), m, escapeTime); if (new UnityEngine.Vector2(e.x - at.x, e.z - at.z).magnitude >= escapeOut) n++; } return n; }
// (name, repro start, repro heading degrees (0 north, 90 east), repro mode 0 walk 1 sprint 2 sprint-jump, trap spots)
var traps = new (string n, UnityEngine.Vector3 s, float hd, int mode, UnityEngine.Vector3[] spots)[]
{
    ("1 Old_Burn pocket", V(230f, 6.6f, 166f), 0f, 0, new[] { V(230f, 6.6f, 166f) }),
    ("2 cave mouth pit", V(58.5f, -0.8f, 33.8f), 315f, 0, new[] { V(54.4f, -5.7f, 35.5f), V(56f, -4f, 35.5f), V(57.9f, -2.5f, 35f), V(56.8f, -2.7f, 35f) }),
    ("3a pump trench", V(172.1f, 5.8f, 112.3f), 315f, 0, new[] { V(171.7f, 4f, 114f), V(171.7f, 4f, 116f), V(171.7f, 3.5f, 117.8f) }),
    ("3a pump trench (2)", V(172.2f, 4.6f, 114.6f), 225f, 0, new UnityEngine.Vector3[0]),
    ("3b pump trench", V(182.1f, 3.1f, 106.6f), 0f, 0, new[] { V(181.7f, 1f, 106.5f) }),
    ("4 south of camp", V(177f, 12.9f, 141.2f), 0f, 0, new[] { V(178f, 11f, 141f), V(175f, 11.5f, 142f) }),
    ("4 south of camp (2)", V(174.7f, 13.6f, 141.1f), 90f, 2, new UnityEngine.Vector3[0]),
    ("5 forage patch B", V(143.2f, 10.8f, 165.8f), 315f, 0, new[] { V(140.7f, 8f, 167.8f) }),
    // 6 (Ward_P3) and climb traps 7a and 7b lie north of the 8.20 fallen giant, closed to the player (main3_8_20_closure_check.cs)
    ("7c climb rim", V(55.1f, 33f, 232.9f), 180f, 2, new[] { V(55.1f, 32.2f, 229.4f) }),
    ("7d climb rim", V(47.1f, 30.3f, 212.6f), 45f, 0, new[] { V(47.6f, 29.2f, 213.4f) }),
    ("8 fire pit wedge (8.21 gate round 2, Marlow)", V(166.3f, 15.1f, 156f), 90f, 0, new[] { V(167.75f, 15.04f, 156f) }),
};
var sb = new System.Text.StringBuilder(); int held = 0;
try
{
    foreach (var t in traps)
    {
        Put(V(t.s.x, Ground(t.s.x, t.s.y, t.s.z) + 0.3f, t.s.z)); var start = pc.transform.position;
        var end = Move(start, Dir(t.hd), t.mode, reproTime); int e0 = Escapes(end);
        var line = new System.Text.StringBuilder(t.n + ": repro ends " + end.ToString("F1") + ", escapes " + e0 + " of " + headings * 3);
        bool later = false;
        if (e0 == 0 && !later) held++; if (later) line.Insert(0, "(8.20) ");
        foreach (var sp in t.spots)
        {
            var g = Ground(sp.x, sp.y, sp.z); Put(V(sp.x, g + 0.3f, sp.z)); var at = pc.transform.position; int e = Escapes(at); if (e == 0 && !later) held++;
            line.Append(" | spot " + sp.ToString("F1") + " settles " + at.ToString("F1") + ", escapes " + e);
        }
        sb.Append(line + "\n");
    }
}
finally { pc.enabled = pcWas; UnityEngine.Application.runInBackground = false; }
return (held == 0 ? "ALL PASS" : "HELD " + held) + ": Marlow's traps 1 to 5, 7c and 7d, " + headings * 3 + " escapes each\n" + sb;
