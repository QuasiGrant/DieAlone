// Main3 8.9a check (Play mode): walks each spot the fix batch changed and reports pass or fail per finding.
// Drives the CharacterController in 0.06 m steps with gravity pushed each step, like Marlow's walk. Reset runInBackground after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var terrain = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
void Put(float x, float z, float y = float.NaN) { cc.enabled = false; pc.transform.position = new UnityEngine.Vector3(x, (float.IsNaN(y) ? H(x, z) : y) + 0.3f, z); cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
// walk toward (x, z); true if it gets within 0.3 m
bool To(float x, float z, bool hop = false)
{
    var t = new UnityEngine.Vector2(x, z);
    for (int s = 0; s < 6000; s++)
    {
        var p = pc.transform.position; var d = t - new UnityEngine.Vector2(p.x, p.z); if (d.magnitude < 0.3f) return true;
        var m = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude);
        float up = hop && s % 40 < 8 ? 0.08f : -0.15f;
        cc.Move(new UnityEngine.Vector3(m.x, up, m.y));
        if (s > 400 && (new UnityEngine.Vector2(pc.transform.position.x, pc.transform.position.z) - new UnityEngine.Vector2(p.x, p.z)).magnitude < 0.001f) return false;
    }
    return false;
}
string P3() => pc.transform.position.ToString("F1");
var sb = new System.Text.StringBuilder(); int fails = 0;
void Check(string what, bool ok, string detail) { if (!ok) fails++; sb.Append((ok ? "PASS " : "FAIL ") + what + ": " + detail + "\n"); }

// 1. Camp 2 stack top on foot by the east-face path
Put(296f, 100f);
bool up = To(298.9f, 106f) && To(298.9f, 118.7f) && To(300.4f, 118.7f) && To(300.4f, 107.3f) && To(298.9f, 107.3f) && To(298.9f, 118.7f) && To(300.4f, 118.7f) && To(300.4f, 107.3f) && To(294.5f, 107.3f);
Check("1 stack top reachable", up && pc.transform.position.y > 23.5f, "end " + P3());
// 2. lake holds: off the dock end, and across the old W6/W7 gap
Put(189f, 95f); To(189f, 87.2f); bool offDock = To(189f, 80f);
Check("2a cannot step off the dock end", !offDock && pc.transform.position.z > 86.3f, "stopped at " + P3());
Put(243f, 90f); bool inLake = To(190f, 60f);
Check("2b cannot walk into the lake from (243, 90)", !inLake && pc.transform.position.y > -5.6f, "stopped at " + P3());
Put(238f, 76f); bool gap = To(225f, 60f);
Check("2c W6/W7 ring holds", !gap && pc.transform.position.y > -5.6f, "stopped at " + P3());
// 3. off-trail walking blocked
Put(262f, 172f); bool cut1 = To(348f, 193f); Check("3a Jg straight to the office door blocked", !cut1, "stopped at " + P3());
Put(184f, 170f); bool cut2 = To(282f, 238f); Check("3b camp straight to Camp 1 blocked", !cut2, "stopped at " + P3());
Put(180f, 152f); bool cut3 = To(292f, 108f); Check("3c camp straight to Camp 2 blocked", !cut3, "stopped at " + P3());
Put(52f, 38.5f); bool cut4 = To(60f, 38.5f); Check("3d ravine floor off the cave trail blocked (no way onto the rim above the cave)", !cut4, "stopped at " + P3());
// 7. shift walls, switched on for the test: Marlow's bypass routes
var sw = Root("FrontZone").transform.Find("ShiftWalls").gameObject; sw.SetActive(true); UnityEngine.Physics.SyncTransforms();
Put(389f, 171f); bool b1 = To(381f, 178f) && To(381f, 186f); Check("7a spur mouth bypass on the grass blocked", !b1, "stopped at " + P3());
Put(385f, 171f); bool b2 = To(385f, 182f); Check("7b spur mouth straight on blocked", !b2, "stopped at " + P3());
Put(374.5f, 172f); bool b3 = To(374.5f, 160f) && To(384f, 160f); Check("7c turning circle via x 374.5 blocked", !b3, "stopped at " + P3());
Put(393.5f, 171f); bool b4 = To(393.5f, 160f) && To(384f, 160f); Check("7d turning circle via x 393.5 blocked", !b4, "stopped at " + P3());
Put(392f, 176f); bool b5 = To(389.5f, 176f) && To(360f, 171f); Check("7e booth to the lot stays open", b5, "end " + P3());
sw.SetActive(false); UnityEngine.Physics.SyncTransforms();
Check("7f shift walls off outside a shift", !sw.activeSelf, "ShiftWalls active " + sw.activeSelf);
// chain and cave board are solid
var legJ = Root("Trails").transform.Find("J to Ward"); var j0 = legJ.GetChild(0).position; var j6 = legJ.GetChild(6).position;
Put(j0.x, j0.z); bool overChain = To(j6.x, j6.z, true); Check("9a Ward chain closes the climb (walking and hopping)", !overChain, "stopped at " + P3());
Put(52f, 39.5f); bool inCave = To(52f, 30f, true); Check("9b cave board closes the mouth (walking and hopping)", !inCave, "stopped at " + P3());
// 8. boathouse trail end meets the gangway: from the end straight into the boathouse
var legB = Root("Trails").transform.Find("Pump to boathouse"); var bEnd = legB.GetChild(legB.childCount - 1).position;
Put(bEnd.x, bEnd.z); bool gw = To(241f, 52.4f); Check("8 boathouse trail end to the gangway and inside", gw, "trail end (" + bEnd.x.ToString("F1") + ", " + bEnd.z.ToString("F1") + "), end " + P3());
pc.enabled = true;
return (fails == 0 ? "ALL PASS" : fails + " FAIL") + "\n" + sb;
