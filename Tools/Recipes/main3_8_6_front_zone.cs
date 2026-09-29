// Main3 task 8.6: front zone and gate, gray. Run after 8.5 in Main3, edit mode. Main3.md 3.1 and 3.2, map positions.
// Ground surfaces (lot, drive, turning circle, spur, loop, pitches) are colliderless slabs 2 cm over the flat 3 m ground.
// Shift walls are built inactive: nothing starts a shift yet (the gate minigame is not designed); a later task turns them on.
// The gate's PlayerBlocker is always on (8.1 builds no gate wall; the fence gap is closed here).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Campsites") == null) return "run 8.5 first";
if (Root("FrontZone") != null) return "FrontZone already exists; rebuild Main3 from 8.1";
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
UnityEngine.Transform Group(string name, UnityEngine.Transform parent) { var g = new UnityEngine.GameObject(name); if (parent != null) g.transform.SetParent(parent, false); return g.transform; }
var Cube = UnityEngine.PrimitiveType.Cube; var Cyl = UnityEngine.PrimitiveType.Cylinder; var Cap = UnityEngine.PrimitiveType.Capsule; var Sph = UnityEngine.PrimitiveType.Sphere;
const float G = 3f, slab = 0.02f;
var fz = Group("FrontZone", null);
var ground = Group("Surfaces", fz);
void Slab(string name, float x0, float x1, float z0, float z1) => Prim(Cube, name, ground, V((x0 + x1) * 0.5f, G + slab * 0.5f, (z0 + z1) * 0.5f), V(x1 - x0, slab, z1 - z0), 0f, false);
void Strip(string name, UnityEngine.Vector2 a, UnityEngine.Vector2 b, float w)
{
    var d = b - a; var g = Prim(Cube, name, ground, V((a.x + b.x) * 0.5f, G + slab * 0.5f, (a.y + b.y) * 0.5f), V(w, slab, d.magnitude + w * 0.5f), UnityEngine.Mathf.Atan2(d.x, d.y) * UnityEngine.Mathf.Rad2Deg, false);
}
Slab("ParkingLot", 343f, 373f, 150f, 190f);                          // 3.1.5
Slab("Drive", 373f, 396f, 167.5f, 172.5f);                            // 3.1.3, one lane 5 m
Prim(Cyl, "TurningCircle", ground, V(384f, G + slab * 0.5f, 160f), V(16f, slab * 0.5f, 16f), 0f, false);   // 3.2.3
var spur = new[] { new UnityEngine.Vector2(385f, 172.5f), new UnityEngine.Vector2(385f, 186f), new UnityEngine.Vector2(390f, 196f), new UnityEngine.Vector2(390f, 242f), new UnityEngine.Vector2(387.5f, 252f) };   // the last piece joins the loop ring (8.9b)
for (int i = 0; i < spur.Length - 1; i++) Strip("Spur" + i, spur[i], spur[i + 1], 4f);   // 3.2.2, 4 m, on to the loop
for (int i = 0; i < 40; i++)   // 3.2.5 loop road 5 m wide, 40 m across outside, centred (372, 262)
{
    float a0 = i * UnityEngine.Mathf.PI * 2f / 40f, a1 = (i + 1) * UnityEngine.Mathf.PI * 2f / 40f; const float rc = 17.5f;
    Strip("Loop" + i, new UnityEngine.Vector2(372f + UnityEngine.Mathf.Cos(a0) * rc, 262f + UnityEngine.Mathf.Sin(a0) * rc), new UnityEngine.Vector2(372f + UnityEngine.Mathf.Cos(a1) * rc, 262f + UnityEngine.Mathf.Sin(a1) * rc), 5f);
}
var pitches = Group("EmptyPitches", fz);
for (int i = 0; i < 8; i++) { float a = (i + 0.5f) * UnityEngine.Mathf.PI * 2f / 8f; Prim(Cube, "Pitch", pitches, V(372f + UnityEngine.Mathf.Cos(a) * 11f, G + 0.05f, 262f + UnityEngine.Mathf.Sin(a) * 11f), V(3f, 0.1f, 3f), -a * UnityEngine.Mathf.Rad2Deg, false); }

// buildings: map footprints, doorways (1.0 x 2.1) toward the lot
void Building(string name, float cx, float cz, float w, float d, float h, float doorX)
{
    var b = Group(name, fz); b.position = V(cx, G, cz); const float t = 0.2f;
    float hw = w * 0.5f, hd = d * 0.5f;
    Prim(Cube, "Floor", b, V(cx, G + 0.01f, cz), V(w, 0.02f, d), 0f, false);
    Prim(Cube, "Wall_N", b, V(cx, G + h * 0.5f, cz + hd - t * 0.5f), V(w, h, t));
    Prim(Cube, "Wall_W", b, V(cx - hw + t * 0.5f, G + h * 0.5f, cz), V(t, h, d));
    Prim(Cube, "Wall_E", b, V(cx + hw - t * 0.5f, G + h * 0.5f, cz), V(t, h, d));
    float zs = cz - hd + t * 0.5f, dl = doorX - 0.5f, dr = doorX + 0.5f;
    Prim(Cube, "Wall_S_W", b, V((cx - hw + dl) * 0.5f, G + h * 0.5f, zs), V(dl - (cx - hw), h, t));
    Prim(Cube, "Wall_S_E", b, V((cx + hw + dr) * 0.5f, G + h * 0.5f, zs), V(cx + hw - dr, h, t));
    Prim(Cube, "Wall_S_Lintel", b, V(doorX, G + (2.1f + h) * 0.5f, zs), V(1.0f, h - 2.1f, t));
    Prim(Cube, "Roof", b, V(cx, G + h + 0.1f, cz), V(w + 0.4f, 0.2f, d + 0.4f));
}
Building("Office", 350f, 200f, 12f, 8f, 3.2f, 348f);    // 3.1.7, map x 344 to 356, z 196 to 204
Building("Store", 366f, 200f, 8f, 5.6f, 3.0f, 366f);    // map x 362 to 370, z 197.2 to 202.8
// mast at the office north-east corner (map), 30 m, steel lattice stand-in: three legs and rings, red lamp on top
var mast = Group("Mast", fz); mast.position = V(357f, G, 205f);
for (int i = 0; i < 3; i++) { float a = i * 120f * UnityEngine.Mathf.Deg2Rad; Prim(Cube, "Leg", mast, V(357f + UnityEngine.Mathf.Cos(a) * 0.45f, G + 15f, 205f + UnityEngine.Mathf.Sin(a) * 0.45f), V(0.1f, 30f, 0.1f)); }
for (int k = 1; k < 10; k++) Prim(Cyl, "Ring", mast, V(357f, G + k * 3f, 205f), V(1.0f, 0.03f, 1.0f), 0f, false);
// far markers (rev 15, 2.11): the mast lamp and two sodium lot lights on DieAlone/FireStandIn (unlit, no fog), so they still
// mark the office at night past the night fog end (Style.md 2.4 colours from LookTuning)
var look = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>("Assets/Settings/LookTuning.asset"); if (look == null) return "no LookTuning";
var markerSh = UnityEngine.Shader.Find("DieAlone/FireStandIn"); if (markerSh == null) return "DieAlone/FireStandIn shader not found";
UnityEngine.Material MarkerMat(string path, UnityEngine.Color c)
{
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null) { m = new UnityEngine.Material(markerSh); UnityEditor.AssetDatabase.CreateAsset(m, path); }
    m.shader = markerSh; m.SetColor("_Color", c); m.SetFloat("_Intensity", look.farMarkerIntensity); UnityEditor.EditorUtility.SetDirty(m); return m;
}
// sized to stay a dot through the look filter's low resolution from the tower deck (about 200 m; 8.9f)
const float markerLamp = 1.0f, lotHeadW = 1.2f, lotHeadD = 0.7f;
var redLamp = Prim(Sph, "RedLamp", mast, V(357f, G + 30.3f, 205f), V(markerLamp, markerLamp, markerLamp), 0f, false);
redLamp.GetComponent<UnityEngine.Renderer>().sharedMaterial = MarkerMat("Assets/Materials/Blockout/Blockout_MastLamp.mat", look.mastLampColor);
// lot lights: two 6 m posts at opposite corners of the lot, a sodium head on each (Main3.md 5.4, "red lamp and sodium")
const float lotPostH = 6f;
var lotLightMat = MarkerMat("Assets/Materials/Blockout/Blockout_LotLight.mat", look.lotLightColor);
var lotLights = Group("LotLights", fz);
foreach (var lp in new[] { new UnityEngine.Vector2(344.5f, 151.5f), new UnityEngine.Vector2(371.5f, 188.5f) })
{
    Prim(Cyl, "Post", lotLights, V(lp.x, G + lotPostH * 0.5f, lp.y), V(0.15f, lotPostH * 0.5f, 0.15f));
    Prim(Cube, "Head", lotLights, V(lp.x, G + lotPostH + 0.1f, lp.y), V(lotHeadW, 0.3f, lotHeadD), 0f, false).GetComponent<UnityEngine.Renderer>().sharedMaterial = lotLightMat;
}
// the lot resident's car (map (370, 179.4)), gray stand-in
var car = Group("Resident_Car", fz); car.position = V(370f, G, 179.4f);
Prim(Cube, "Body", car, V(370f, G + 0.55f, 179.4f), V(4.5f, 0.8f, 1.8f));
Prim(Cube, "Cabin", car, V(369.7f, G + 1.25f, 179.4f), V(2.4f, 0.6f, 1.6f), 0f, false);
Prim(Cap, "Resident_Office_Spot", car, V(370f, G + 0.9f, 181.4f), V(0.6f, 0.9f, 0.6f), 0f, false);
// booth (392, 176), 2 x 2, window on the lane side, doorway on the west
var booth = Group("GateBooth", fz); booth.position = V(392f, G, 176f);
{
    const float t = 0.12f, h = 2.5f;
    Prim(Cube, "Wall_N", booth, V(392f, G + h * 0.5f, 177f - t * 0.5f), V(2f, h, t));
    Prim(Cube, "Wall_E", booth, V(393f - t * 0.5f, G + h * 0.5f, 176f), V(t, h, 2f));
    Prim(Cube, "Wall_S_Sill", booth, V(392f, G + 0.5f, 175f + t * 0.5f), V(2f, 1f, t));
    Prim(Cube, "Wall_S_Head", booth, V(392f, G + 2.3f, 175f + t * 0.5f), V(2f, 0.4f, t));
    Prim(Cube, "Wall_W_N", booth, V(391f + t * 0.5f, G + h * 0.5f, 176.75f), V(t, h, 0.5f));
    Prim(Cube, "Wall_W_S", booth, V(391f + t * 0.5f, G + h * 0.5f, 175.3f), V(t, h, 0.6f));
    Prim(Cube, "Wall_W_Lintel", booth, V(391f + t * 0.5f, G + 2.3f, 176.05f), V(t, 0.4f, 0.9f));
    Prim(Cube, "Roof", booth, V(392f, G + h + 0.08f, 176f), V(2.4f, 0.15f, 2.4f));
    Prim(Cube, "Counter", booth, V(392f, G + 0.9f, 175.4f), V(1.6f, 0.08f, 0.4f), 0f, false);
}
// chain across the spur at (390, 238), 0.5 m high between two posts; the player walks around it outside shifts
var chain = Group("Chain", fz);
foreach (var cx in new[] { 387.7f, 392.3f }) Prim(Cube, "Post", chain, V(cx, G + 0.5f, 238f), V(0.15f, 1f, 0.15f));
Prim(Cube, "Chain", chain, V(390f, G + 0.5f, 238f), V(4.6f, 0.05f, 0.05f));
// gate at (396, 170): posts, lift barrier (down), and the player-only blocker that is always on
var gate = Group("Gate", fz);
foreach (var gzz in new[] { 167.2f, 172.8f }) Prim(Cube, "Post", gate, V(396f, G + 0.6f, gzz), V(0.3f, 1.2f, 0.3f));
Prim(Cube, "BarrierArm", gate, V(396f, G + 1.0f, 170f), V(0.12f, 0.12f, 5.4f), 0f, false);
var blocker = new UnityEngine.GameObject("PlayerBlocker"); blocker.transform.SetParent(gate, false); blocker.transform.position = V(396f, G + 20f, 170f);
blocker.AddComponent<UnityEngine.BoxCollider>().size = V(0.6f, 60f, 5.2f);
// shift walls (3.2.7), inactive until a shift system exists
var shift = Group("ShiftWalls", fz);
// Walls that cannot be walked around (8.9a, Marlow finding 7): the thicket (8.3) closes everything off the surfaces, so a U
// across the spur mouth (z 178.8 from x 380.5 to the fence, and down to the drive at x 380.5) and a closed ring round the
// turning circle (r 9.3) seal both. The drive stays open between them.
void ShiftWall(string name, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var w = new UnityEngine.GameObject(name); w.transform.SetParent(shift, false); var d = b - a; w.transform.position = V((a.x + b.x) * 0.5f, G + 5f, (a.y + b.y) * 0.5f); w.transform.rotation = UnityEngine.Quaternion.LookRotation(V(d.x, 0f, d.y).normalized, UnityEngine.Vector3.up); w.AddComponent<UnityEngine.BoxCollider>().size = V(0.4f, 10f, d.magnitude + 0.4f); }
ShiftWall("SpurMouth", new UnityEngine.Vector2(380.5f, 178.8f), new UnityEngine.Vector2(395.9f, 178.8f));
ShiftWall("SpurMouthWest", new UnityEngine.Vector2(380.5f, 173.6f), new UnityEngine.Vector2(380.5f, 178.8f));
for (int i = 0; i < 24; i++) { float a0 = i * UnityEngine.Mathf.PI / 12f, a1 = (i + 1) * UnityEngine.Mathf.PI / 12f; ShiftWall("TurningCircle" + i, new UnityEngine.Vector2(384f + UnityEngine.Mathf.Cos(a0) * 9.3f, 160f + UnityEngine.Mathf.Sin(a0) * 9.3f), new UnityEngine.Vector2(384f + UnityEngine.Mathf.Cos(a1) * 9.3f, 160f + UnityEngine.Mathf.Sin(a1) * 9.3f)); }
shift.gameObject.SetActive(false);

// warps: booth doorway, closed campground behind the chain
var warps = Root("DevWarps").transform;
var wb = warps.Find("Gate_Booth"); if (wb != null) { wb.position = V(389.5f, G + 0.2f, 176.05f); wb.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); }
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " ground at lot " + H(358f, 170f).ToString("F2") + ", gate " + H(395f, 170f).ToString("F2") + ", loop " + H(372f, 244.5f).ToString("F2") + " | shift walls inactive, gate blocker on";
