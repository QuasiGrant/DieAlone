// Main3 8.9j check (Play mode): walks the Ward climb from J along the J to Ward trail's centre points (benches, platforms, leg 5,
// the cleft, the ramp) to the path end on the ledge, the day gate switched off for the walk, and times it at the walk speed from
// the grounded horizontal distance the CharacterController covers (Valley.md 5.7: 487 m, 195 s on paper). Then tries to walk off
// the ledge's west edge (x -15) and off a bench's downhill edge, both of which must stop the player. Reset runInBackground after.
if (!UnityEngine.Application.isPlaying) return "enter play mode first";
UnityEngine.Application.runInBackground = true;
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var pc = UnityEngine.Object.FindFirstObjectByType<PlayerController>(); var cc = pc.GetComponent<UnityEngine.CharacterController>(); pc.enabled = false;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
void Put(UnityEngine.Vector3 p) { cc.enabled = false; pc.transform.position = p + UnityEngine.Vector3.up * 0.3f; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); for (int k = 0; k < 20; k++) cc.Move(UnityEngine.Vector3.down * 0.1f); }
float walked = 0f;
bool To(float x, float z)   // bounded: at most 8000 steps of 6 cm, and gives up once stuck
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
float speed = tuning != null ? tuning.walkSpeed : 2.5f;
var gate = Root("Ward").transform.Find("CairnGate"); gate.gameObject.SetActive(false); UnityEngine.Physics.SyncTransforms();
var leg = Root("Trails").transform.Find("J to Ward"); var pts = new System.Collections.Generic.List<UnityEngine.Vector3>(); foreach (UnityEngine.Transform p in leg) pts.Add(p.position);
Put(pts[0]); float w0 = walked; bool ok = true; for (int i = 1; i < pts.Count && ok; i++) ok = To(pts[i].x, pts[i].z);
var end = pc.transform.position; float climb = walked - w0;
var sb = new System.Text.StringBuilder("J to the ledge: " + (ok ? "reached" : "STUCK at " + end.ToString("F1")) + ", " + climb.ToString("F1") + " m, " + (climb / speed).ToString("F1") + " s at " + speed + " m/s, ground at the end " + end.y.ToString("F1"));
bool edgeStops = ok && !To(-15f, end.z); sb.Append(" | ledge west edge stops the player: " + (edgeStops ? "yes, at x " + pc.transform.position.x.ToString("F1") : "NO"));
var terrain = UnityEngine.Terrain.activeTerrain; Put(new UnityEngine.Vector3(72f, terrain.SampleHeight(new UnityEngine.Vector3(72f, 0f, 250f)) + terrain.transform.position.y, 250f)); float benchY = pc.transform.position.y; bool benchStops = !To(80f, 250f);
sb.Append(" | leg 1 downhill edge stops the player: " + (benchStops ? "yes, at x " + pc.transform.position.x.ToString("F1") + " (bench ground " + benchY.ToString("F1") + ")" : "NO"));
gate.gameObject.SetActive(true); UnityEngine.Physics.SyncTransforms(); pc.enabled = true;
return sb.ToString();
