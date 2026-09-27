var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Graybox.unity") return "wrong scene";
if (UnityEngine.GameObject.Find("Tower/Stairs/Flight1/StairRamp") != null) return "ramps already exist";

var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
float theta = UnityEngine.Mathf.Atan2(0.25f, 0.3f) * UnityEngine.Mathf.Rad2Deg;   // stair pitch, 39.8 deg
float sinT = UnityEngine.Mathf.Sin(theta * UnityEngine.Mathf.Deg2Rad), cosT = UnityEngine.Mathf.Cos(theta * UnityEngine.Mathf.Deg2Rad);

// Collider-only sloped box whose TOP face runs along the stair nosings.
UnityEngine.GameObject Ramp(string name, UnityEngine.Transform parent, UnityEngine.Vector3 lineCenter, UnityEngine.Vector3 upNormal, UnityEngine.Vector3 size, UnityEngine.Vector3 euler)
{
    var go = new UnityEngine.GameObject(name);
    go.transform.SetParent(parent, false);
    go.transform.localPosition = lineCenter - upNormal * (size.y * 0.5f);
    go.transform.localRotation = UnityEngine.Quaternion.Euler(euler);
    go.transform.localScale = size;
    go.AddComponent<UnityEngine.BoxCollider>();
    return go;
}

// ---- Player: small step offset, stairs are now handled by ramps.
var cc = UnityEngine.GameObject.Find("Player").GetComponent<UnityEngine.CharacterController>();
cc.stepOffset = 0.1f;

// ---- Tower flights: rise 2.5 over 3.0 m of run, 1.2 m wide. Ramp spans from one run before
// the first nosing (at h0) to the top nosing (at h0 + 2.5).
float rampLen = UnityEngine.Mathf.Sqrt(3f * 3f + 2.5f * 2.5f);
var stairs = UnityEngine.GameObject.Find("Tower/Stairs").transform;
var laneN = 8.4f; var laneS = 7.0f;
void TowerRamp(int n, float z, float h0, bool eastward)
{
    var flight = stairs.Find("Flight" + n);
    float x0 = eastward ? -1.5f : 1.5f;
    float cx = eastward ? x0 + 1.2f : x0 - 1.2f;
    var center = V(cx, h0 + 1.25f, z);
    var up = eastward ? V(-sinT, cosT, 0f) : V(sinT, cosT, 0f);
    Ramp("StairRamp", flight, center, up, V(rampLen, 0.2f, 1.2f), V(0f, 0f, eastward ? theta : -theta));
}
TowerRamp(1, laneN, 0.0f, true);
TowerRamp(2, laneS, 2.5f, false);
TowerRamp(3, laneN, 5.0f, true);
TowerRamp(4, laneS, 7.5f, false);

// ---- Course stairs: 5 steps down from the 1.5 m platform toward +z. Nosing line from
// (z 6.0, y 1.5) at the platform edge to (z 7.8, y 0) on the ground. Width 2.
var climb = UnityEngine.GameObject.Find("TestCourse/RampAndStairs").transform;
float courseLen = UnityEngine.Mathf.Sqrt(1.8f * 1.8f + 1.5f * 1.5f);
Ramp("StairRamp", climb, V(-17f, 0.75f, 6.9f), V(0f, cosT, sinT), V(2f, 0.2f, courseLen), V(theta, 0f, 0f));

// ---- Course ramp: seat its top face exactly on the line from (z -3, y 0) to (z 3, y 1.5).
var ramp = climb.Find("Ramp");
float a = UnityEngine.Mathf.Atan2(1.5f, 6f);
var rampUp = V(0f, UnityEngine.Mathf.Cos(a), -UnityEngine.Mathf.Sin(a));
ramp.localPosition = V(-17f, 0.75f, 0f) - rampUp * 0.1f;

// ---- Step and log back to 0.4 m.
var step = UnityEngine.GameObject.Find("TestCourse/LowStep");
step.transform.localScale = V(3f, 0.4f, 1f);
step.transform.localPosition = V(-17f, 0.2f, -14f);
var log = UnityEngine.GameObject.Find("TestCourse/Log");
log.transform.localScale = V(0.4f, 1.5f, 0.4f);
log.transform.localPosition = V(-17f, 0.2f, -10f);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " stepOffset=" + cc.stepOffset + " skin=" + cc.skinWidth + " pitch=" + theta.ToString("F1") + " rampPos=" + ramp.localPosition.ToString("F3");
