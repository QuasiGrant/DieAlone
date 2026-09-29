// Main3 8.9c check (Play mode): climbs the tower's spiral stair from the south bay to the deck and back, and walks the Ward
// climb from J to the rock lip (the day gate switched off for the walk), timing each at the walk speed (2.5 m/s) from the
// grounded horizontal distance the CharacterController actually covers. Reset runInBackground after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var terrain = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
float walked = 0f;
bool To(float x, float z)
{
    var t = new UnityEngine.Vector2(x, z);
    for (int s = 0; s < 8000; s++)
    {
        var p = pc.transform.position; var flat = new UnityEngine.Vector2(p.x, p.z); var d = t - flat; if (d.magnitude < 0.25f) return true;
        var m = d.normalized * UnityEngine.Mathf.Min(0.06f, d.magnitude);
        cc.Move(new UnityEngine.Vector3(m.x, -0.15f, m.y));
        var q = pc.transform.position; float step = (new UnityEngine.Vector2(q.x, q.z) - flat).magnitude; walked += step;
        if (s > 400 && step < 0.001f) return false;
    }
    return false;
}
var sb = new System.Text.StringBuilder(); float speed = tuning != null ? tuning.walkSpeed : 2.5f;
// tower: corners of the spiral in tower-local space (the recipe's ring at 2.9 m), ten flights left-turning from the south-west
var tower = Root("Camp").transform.Find("Tower"); float c = 2.9f;
var cs = new[] { new UnityEngine.Vector2(-c, -c), new UnityEngine.Vector2(c, -c), new UnityEngine.Vector2(c, c), new UnityEngine.Vector2(-c, c) };
UnityEngine.Vector3 W(UnityEngine.Vector2 l, float y = 0f) => tower.TransformPoint(new UnityEngine.Vector3(l.x, y, l.y));
Put(W(new UnityEngine.Vector2(-2.9f, -7f)));
bool ok = To(W(cs[0]).x, W(cs[0]).z); float start = walked; float y0 = pc.transform.position.y;
for (int k = 1; k <= 10 && ok; k++) { var w = W(cs[k % 4]); ok = To(w.x, w.z); }
float upRun = walked - start; float yTop = pc.transform.position.y;
var walkway = W(new UnityEngine.Vector2(0f, 3.1f)); bool onDeck = ok && To(walkway.x, walkway.z);
sb.Append("tower up: " + (ok ? "reached" : "STUCK at " + pc.transform.position.ToString("F1")) + ", ground " + y0.ToString("F1") + " to " + yTop.ToString("F1") + ", " + upRun.ToString("F1") + " m walked, " + (upRun / speed).ToString("F1") + " s at " + speed + " m/s; onto the walkway " + onDeck + "\n");
float back0 = walked; var hatch = W(cs[2]); bool down = To(hatch.x, hatch.z);
for (int k = 9; k >= 0 && down; k--) { var w = W(cs[k % 4]); down = To(w.x, w.z); }
sb.Append("tower down: " + (down ? "reached the ground at " + pc.transform.position.y.ToString("F1") : "STUCK at " + pc.transform.position.ToString("F1")) + ", " + (walked - back0).ToString("F1") + " m\n");
// Ward climb from J to the lip along the trail's centre points, the day gate off for the walk
var gate = Root("Ward").transform.Find("CairnGate"); gate.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
var leg = Root("Trails").transform.Find("J to Ward"); var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position);
Put(pts[0]); float w0 = walked; bool wardOk = true; for (int i = 1; i < pts.Count && wardOk; i++) wardOk = To(pts[i].x, pts[i].z);
bool lipStops = wardOk && !To(9f, 252f);   // the lip and the cliff wall stop the player short of the edge
sb.Append("Ward climb J to the lip: " + (wardOk ? "reached" : "STUCK at " + pc.transform.position.ToString("F1")) + ", " + (walked - w0).ToString("F1") + " m, " + ((walked - w0) / speed).ToString("F1") + " s; lip stops the player at x " + pc.transform.position.x.ToString("F1") + " (" + lipStops + ")\n");
gate.gameObject.SetActive(true); UnityEngine.Physics.SyncTransforms();
pc.enabled = true;
return sb.ToString();
