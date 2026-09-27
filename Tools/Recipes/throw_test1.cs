UnityEngine.Application.runInBackground = true;
var p = UnityEngine.GameObject.Find("Player");
var cc = p.GetComponent<UnityEngine.CharacterController>();
var carry = p.GetComponent<PlayerCarry>();
var cam = p.transform.Find("Main Camera");
var cube = UnityEngine.GameObject.Find("Loose_Cube");
var rb = cube.GetComponent<UnityEngine.Rigidbody>();
// Player on open ground looking level south, cube brought beside the player, then picked up.
cc.enabled = false; p.transform.position = new UnityEngine.Vector3(-4f, 0f, -12f); cc.enabled = true;
p.transform.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);
cam.localRotation = UnityEngine.Quaternion.identity;
typeof(PlayerController).GetField("pitch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(p.GetComponent<PlayerController>(), 0f);
rb.linearVelocity = UnityEngine.Vector3.zero; rb.angularVelocity = UnityEngine.Vector3.zero;
cube.transform.SetPositionAndRotation(new UnityEngine.Vector3(-4f, 0.5f, -13f), UnityEngine.Quaternion.identity);
UnityEngine.Physics.SyncTransforms();
carry.PickUp(cube.GetComponent<Carryable>());
return "carrying=" + carry.IsCarrying + " frame=" + UnityEngine.Time.frameCount;
