UnityEngine.Application.runInBackground = true;
var p = UnityEngine.GameObject.Find("Player");
var cc = p.GetComponent<UnityEngine.CharacterController>();
var it = p.GetComponent<PlayerInteractor>();
var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
var upd = typeof(Door).GetMethod("Update", flags);
var sb = new System.Text.StringBuilder();

void Teleport(UnityEngine.Vector3 pos) { cc.enabled = false; p.transform.position = pos; cc.enabled = true; UnityEngine.Physics.SyncTransforms(); }
void Step(Door d, int n) { for (int i = 0; i < n; i++) { upd.Invoke(d, null); UnityEngine.Physics.SyncTransforms(); } }

var d1 = UnityEngine.GameObject.Find("TestCourse/TestRoom/Door").GetComponent<Door>();
var d2 = UnityEngine.GameObject.Find("Tower/TopRoom/Door").GetComponent<Door>();

// A. Test room door, player OUTSIDE (south). Open: should swing north (+z), angle -90.
Teleport(new UnityEngine.Vector3(18f, 0f, 10.4f));
d1.Use(it); Step(d1, 400);
sb.Append("A open from south: angle=" + d1.CurrentAngle.ToString("F0") + " open=" + d1.IsOpen + " prompt=" + d1.Prompt + " | ");

// B. Player stands IN the doorway, close: door must hold, not push.
Teleport(new UnityEngine.Vector3(18f, 0f, 11.9f));
d1.Use(it); Step(d1, 400);
sb.Append("B close with player in doorway: angle=" + d1.CurrentAngle.ToString("F0") + " (held if not 0) | ");

// C. Player steps back out, door finishes closing.
Teleport(new UnityEngine.Vector3(18f, 0f, 10.4f));
Step(d1, 400);
sb.Append("C after player clears: angle=" + d1.CurrentAngle.ToString("F0") + " open=" + d1.IsOpen + " | ");

// D. Test room door from INSIDE (north): should swing south (-z), angle +90. Then close.
Teleport(new UnityEngine.Vector3(18f, 0f, 13.4f));
d1.Use(it); Step(d1, 400);
float dAngle = d1.CurrentAngle;
d1.Use(it); Step(d1, 400);
sb.Append("D open from north: angle=" + dAngle.ToString("F0") + " then closed=" + d1.CurrentAngle.ToString("F0") + " | ");

// E. Tower door from the deck (south) and from inside (north).
Teleport(new UnityEngine.Vector3(-1.1f, 10f, 8.4f));
d2.Use(it); Step(d2, 400); float e1 = d2.CurrentAngle; d2.Use(it); Step(d2, 400); float e2 = d2.CurrentAngle;
Teleport(new UnityEngine.Vector3(-1.1f, 10f, 11.4f));
d2.Use(it); Step(d2, 400); float e3 = d2.CurrentAngle; d2.Use(it); Step(d2, 400); float e4 = d2.CurrentAngle;
sb.Append("E tower: fromSouth open=" + e1.ToString("F0") + " close=" + e2.ToString("F0") + " fromNorth open=" + e3.ToString("F0") + " close=" + e4.ToString("F0"));

Teleport(new UnityEngine.Vector3(-10f, 0f, -10f));
return sb.ToString();
