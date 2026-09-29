// Main3 task 8.7: Ward climb pieces, gray. Run after 8.6 in Main3, edit mode.
// Cairn gate 4 m up the J to Ward trail: pale cairn on its west side, clear of the Camp to J trail end (Marlow finding 9), and a
// solid chain at 0.9 m across it to a post: the climb is closed by day (DECISIONS 2026-09-25); a later night rule opens it,
// and the dev warps reach the ledge meanwhile. The climb itself is 8.3's J to Ward trail.
// The Tor (5.4): granite dome at (76, 223), radius 18, base 34, top 58 in the doc, raised by torRaise so the W-1 check keeps
// 3 m with eyes and targets raised 3 m (8.9a): an ellipsoid mesh (horizontal radius 18, vertical 28, centre y 30 + torRaise), so its skirt runs below the ground on the lower south-east side instead of floating.
// Ward stones: three stones 3.6 x 4 m on the cliff edge (DECISIONS 2026-09-25) at x 12.5, z 252 / 258 / 264 (map z), 12 m
// tall on the 36 m ledge (tops 48). The cliff wall stands at x 10.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("FrontZone") == null) return "run 8.6 first";
if (Root("Ward") != null) return "Ward already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
UnityEngine.GameObject Prim(UnityEngine.PrimitiveType t, string name, UnityEngine.Transform parent, UnityEngine.Vector3 wp, UnityEngine.Vector3 sc, float yaw = 0f, bool collider = true)
{
    var g = UnityEngine.GameObject.CreatePrimitive(t); g.name = name; g.transform.SetParent(parent, false); g.transform.position = wp; g.transform.localScale = sc;
    g.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    if (!collider) UnityEngine.Object.DestroyImmediate(g.GetComponent<UnityEngine.Collider>());
    return g;
}
var Cube = UnityEngine.PrimitiveType.Cube; var Sph = UnityEngine.PrimitiveType.Sphere;
var ward = new UnityEngine.GameObject("Ward").transform;

// the J to Ward trail start, for placing the cairn gate across it
var legT = Root("Trails").transform.Find("J to Ward"); if (legT == null) return "no J to Ward trail";
var p0 = legT.GetChild(0).position; var p1 = legT.GetChild(2).position;
var along = V(p1.x - p0.x, 0f, p1.z - p0.z).normalized; var side = V(along.z, 0f, -along.x);
var gateAt = p0 + along * 4.0f;
var cairnGate = new UnityEngine.GameObject("CairnGate").transform; cairnGate.SetParent(ward, false);
var cairnPos = gateAt - side * 2.2f; cairnPos.y = H(cairnPos.x, cairnPos.z);
float cy = cairnPos.y;
foreach (var s in new[] { (1.4f, 0.5f), (1.1f, 0.45f), (0.85f, 0.4f), (0.6f, 0.35f), (0.4f, 0.3f) })
{ Prim(Sph, "Stone", cairnGate, V(cairnPos.x, cy + s.Item2 * 0.45f, cairnPos.z), V(s.Item1, s.Item2, s.Item1)); cy += s.Item2 * 0.8f; }
var postPos = gateAt + side * 2.2f; postPos.y = H(postPos.x, postPos.z);
Prim(Cube, "ChainPost", cairnGate, postPos + V(0f, 0.55f, 0f), V(0.15f, 1.1f, 0.15f));
var chainA = cairnPos + V(0f, 0.9f, 0f); var chainB = postPos + V(0f, 0.9f, 0f);
var chain = Prim(Cube, "Chain", cairnGate, (chainA + chainB) * 0.5f, V(0.06f, 0.06f, UnityEngine.Vector3.Distance(chainA, chainB)), 0f, true);   // solid: the climb is closed by day
chain.transform.rotation = UnityEngine.Quaternion.LookRotation(chainB - chainA, UnityEngine.Vector3.up);

// the Tor
const float torRaise = 5.5f;
var tor = Prim(Sph, "Tor", ward, V(76f, 30f + torRaise, 223f), V(36f, 56f, 36f), 0f, false);
tor.AddComponent<UnityEngine.MeshCollider>().sharedMesh = tor.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
float torTop = tor.GetComponent<UnityEngine.Renderer>().bounds.max.y;
// clearance from the climb trail to the Tor's widest ring
float minD = float.MaxValue; foreach (UnityEngine.Transform m in legT) minD = UnityEngine.Mathf.Min(minD, UnityEngine.Vector2.Distance(new UnityEngine.Vector2(m.position.x, m.position.z), new UnityEngine.Vector2(76f, 223f)));

// Ward stones on the ledge
var stones = new UnityEngine.GameObject("Stones").transform; stones.SetParent(ward, false);
int n = 0; foreach (var z in new[] { 264f, 258f, 252f })
{
    n++; float g = H(12.5f, z);
    Prim(Cube, "Stone_" + n, stones, V(12.5f, g + (48f - g) * 0.5f - 0.5f, z), V(3.6f, 48f - g + 1f, 4f));
}
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cairn at " + cairnPos.ToString("F1") + " solid chain 4 m up the trail | Tor top " + torTop.ToString("F1") + " ground at Tor centre " + H(76f, 223f).ToString("F1") + " closest climb point " + minD.ToString("F1") + " m from the Tor centre | stones ground " + H(12.5f, 258f).ToString("F1") + " tops 48";
