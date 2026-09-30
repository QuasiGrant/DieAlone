// Main3 walk check 12, day-one state (edit mode, read-only; WalkChecks.md 12 as retargeted in 8.9k): the saved scene holds
// the day-one state. The stand-in fire is ACTIVE (8.9j: always on, the land hides it), the cairn gate and the cave's day-one
// board are active, the shift walls are off, and the scene is not dirty.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
var sb = new System.Text.StringBuilder(); bool ok = true;
void Check(string what, UnityEngine.Transform t, bool wantActive)
{
    bool pass = t != null && t.gameObject.activeInHierarchy == wantActive;
    ok &= pass; sb.Append((pass ? "PASS " : "FAIL ") + what + ": " + (t == null ? "missing" : (t.gameObject.activeInHierarchy ? "active" : "inactive")) + "\n");
}
var ward = Root("Ward") != null ? Root("Ward").transform : null;
Check("StandInFire on", ward != null ? ward.Find("StandInFire") : null, true);
Check("CairnGate on", ward != null ? ward.Find("CairnGate") : null, true);
Check("Cave day-one board on", Root("Cave") != null ? Root("Cave").transform.Find("Mouth/DayOneBoard") : null, true);
Check("shift walls off", Root("FrontZone") != null ? Root("FrontZone").transform.Find("ShiftWalls") : null, false);
bool clean = !scene.isDirty; ok &= clean; sb.Append((clean ? "PASS" : "FAIL") + " scene not dirty\n");
return (ok ? "ALL PASS" : "FAIL") + "\n" + sb;
