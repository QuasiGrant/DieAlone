UnityEngine.Application.runInBackground = true;
var p = UnityEngine.GameObject.Find("Player");
var cc = p.GetComponent<UnityEngine.CharacterController>();
var it = p.GetComponent<PlayerInteractor>();
var carry = p.GetComponent<PlayerCarry>();
var cube = UnityEngine.GameObject.Find("TestCourse/Table/Loose_Cube");
var cam = p.transform.Find("Main Camera");
// Stand south of the table and look at the cube on it.
cc.enabled = false; p.transform.position = new UnityEngine.Vector3(cube.transform.position.x, 0f, cube.transform.position.z - 1.2f); cc.enabled = true;
p.transform.rotation = UnityEngine.Quaternion.identity;
var dir = cube.transform.position - cam.position;
float pitch = -UnityEngine.Mathf.Atan2(dir.y, new UnityEngine.Vector2(dir.x, dir.z).magnitude) * UnityEngine.Mathf.Rad2Deg;
cam.localRotation = UnityEngine.Quaternion.Euler(pitch, 0f, 0f);
typeof(PlayerController).GetField("pitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(p.GetComponent<PlayerController>(), pitch);
UnityEngine.Physics.SyncTransforms();
// Run the interactor's own target search this frame by calling Update via reflection.
typeof(PlayerInteractor).GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(it, null);
string seen = "target=" + (it.Current != null ? it.Current.name + "(" + it.Current.GetType().Name + ")" : "null") + " prompt=" + (it.Current != null ? it.Current.Prompt : "");
if (it.Current is Carryable) it.Current.Use(it);
return seen + " | carrying=" + carry.IsCarrying + " held=" + (carry.Held != null ? carry.Held.name : "none") + " colliderEnabled=" + cube.GetComponent<UnityEngine.Collider>().enabled + " frame=" + UnityEngine.Time.frameCount;
