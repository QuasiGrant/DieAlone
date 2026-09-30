// Main3 task 8.6: front zone and gate, gray. Run after 8.5 in Main3, edit mode. Main3.md 3.1 and 3.2, map positions.
// Ground surfaces (lot, drive, turning circle, spur, loop, pitches) are colliderless slabs 2 cm over the flat 3 m ground.
// The shift wall (IW3) is built inactive: nothing starts a shift yet (the gate minigame is not designed); a later task turns it on.
// 8.14 (Valley.md rev 10): the highway, the T at the gate, the verge tree, and the brush bands that close the campground.
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
// IW3 (Valley.md rev 10 section 8): the only shift wall, across the spur's gap in the brush band at z 210 from the band's east end
// (x 386) to the fence, so an admitted car cannot be followed to the closed campground. Inactive until a shift system exists. The
// rev 7 walls (spur mouth and the 24 turning-circle pieces) are in git history; IW1 stops a player following a refused car.
var shift = Group("ShiftWalls", fz);
const float iw3Z = 210f, iw3X0 = 386f, iw3X1 = 396f, iw3H = 10f, iw3Thick = 0.4f;
{ var w = new UnityEngine.GameObject("IW3_SpurGap"); w.transform.SetParent(shift, false); w.transform.position = V((iw3X0 + iw3X1) * 0.5f, G + iw3H * 0.5f, iw3Z); w.AddComponent<UnityEngine.BoxCollider>().size = V(iw3X1 - iw3X0 + iw3Thick, iw3H, iw3Thick); }
shift.gameObject.SetActive(false);
// brush bands round the closed campground (Valley.md 8, rev 10): gray stand-ins 1.4 m tall, solid, x 345 to 386 at z 206 to 215
// (behind the office and store) and x 340 to 348 from z 206 to 310 (into the N foot rock band). 8.15 dresses them with owned brush.
const float brushH = 1.4f;
var brush = Group("BrushBands", fz);
foreach (var bb in new[] { (345f, 386f, 206f, 215f), (340f, 348f, 206f, 310f) })
    Prim(Cube, "Brush", brush, V((bb.Item1 + bb.Item2) * 0.5f, H((bb.Item1 + bb.Item2) * 0.5f, (bb.Item3 + bb.Item4) * 0.5f) + brushH * 0.5f - 0.2f, (bb.Item3 + bb.Item4) * 0.5f), V(bb.Item2 - bb.Item1, brushH + 0.4f, bb.Item4 - bb.Item3));
// the highway and the gate junction (Valley.md 3): the drive runs on 32 m from the gate to a T on the highway (x 428), two lanes
// 7.5 m on 8.1's road bed, a painted centre line, reflector posts every 12.5 m (25 before 8.14a), power poles on the far side, a stop sign, the
// park's entrance sign facing the road and a mailbox post at the T; the dead broken-top verge giant at (418, 136). Gray, no colliders
// (the player never reaches them).
const float roadX = 428f, roadW = 7.5f, roadN = 430f, roadS = -130f, roadR = 120f, lineW = 0.15f, driveW = 5f, postStep = 12.5f, poleStep = 50f, poleH = 9f, poleOff = 9f, postOff = 4.6f;
var road = Group("Highway", fz);
UnityEngine.GameObject Flat(string name, UnityEngine.Transform parent, UnityEngine.Vector2 a, UnityEngine.Vector2 b, float w, float y)
{
    var d = b - a; return Prim(Cube, name, parent, V((a.x + b.x) * 0.5f, y, (a.y + b.y) * 0.5f), V(w, slab, d.magnitude + 0.05f), UnityEngine.Mathf.Atan2(d.x, d.y) * UnityEngine.Mathf.Rad2Deg, false);
}
const int roadArcSteps = 5;   // the arcs run to the terrain edge (z about 486 and -186), 28 degrees round; the arms hide the rest
var roadLine = new System.Collections.Generic.List<UnityEngine.Vector2>();
for (int i = roadArcSteps; i >= 1; i--) { float a = -i * UnityEngine.Mathf.PI / 32f; roadLine.Add(new UnityEngine.Vector2(roadX - roadR + UnityEngine.Mathf.Cos(a) * roadR, roadS + UnityEngine.Mathf.Sin(a) * roadR)); }   // south arc
for (float z = roadS; z <= roadN + 0.1f; z += 20f) roadLine.Add(new UnityEngine.Vector2(roadX, z));
for (int i = 1; i <= roadArcSteps; i++) { float a = i * UnityEngine.Mathf.PI / 32f; roadLine.Add(new UnityEngine.Vector2(roadX - roadR + UnityEngine.Mathf.Cos(a) * roadR, roadN + UnityEngine.Mathf.Sin(a) * roadR)); }
var lineMat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Unlit"));
const string lineMatPath = "Assets/Materials/Blockout/Blockout_RoadLine.mat";
{ var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(lineMatPath); if (m == null) UnityEditor.AssetDatabase.CreateAsset(lineMat, lineMatPath); else lineMat = m; UnityEngine.ColorUtility.TryParseHtmlString("#C8A848", out var lc); lineMat.SetColor("_BaseColor", lc); UnityEditor.EditorUtility.SetDirty(lineMat); }
for (int i = 0; i < roadLine.Count - 1; i++)
{
    var a = roadLine[i]; var b = roadLine[i + 1]; float y = G + slab * 0.5f;
    Flat("Road", road, a, b, roadW, y);
    Flat("CentreLine", road, a, b, lineW, y + slab).GetComponent<UnityEngine.Renderer>().sharedMaterial = lineMat;
}
Flat("Drive_To_T", road, new UnityEngine.Vector2(396.5f, 170f), new UnityEngine.Vector2(roadX - roadW * 0.5f, 170f), driveW, G + slab * 0.5f);
for (float z = roadS; z <= roadN; z += poleStep) { Prim(Cyl, "PowerPole", road, V(roadX + poleOff, G + poleH * 0.5f, z), V(0.3f, poleH * 0.5f, 0.3f), 0f, false); Prim(Cube, "Crossarm", road, V(roadX + poleOff, G + poleH - 0.4f, z), V(0.12f, 0.12f, 2f), 0f, false); }
var tJ = Group("GateT", fz);
Prim(Cube, "StopSignPost", tJ, V(roadX - roadW * 0.5f - 1.2f, G + 1.1f, 173f), V(0.08f, 2.2f, 0.08f), 0f, false);
Prim(Cube, "StopSign", tJ, V(roadX - roadW * 0.5f - 1.2f, G + 2.3f, 173f), V(0.75f, 0.75f, 0.05f), 45f, false);
foreach (var ez in new[] { 164f, 166.5f }) Prim(Cube, "EntranceSignPost", tJ, V(roadX - roadW * 0.5f - 3f, G + 1f, ez), V(0.15f, 2f, 0.15f), 0f, false);
Prim(Cube, "EntranceSign", tJ, V(roadX - roadW * 0.5f - 3f, G + 1.7f, 165.25f), V(0.1f, 1.2f, 3.2f), 0f, false);   // faces the road (east)
Prim(Cube, "MailboxPost", tJ, V(roadX - roadW * 0.5f - 1.2f, G + 0.6f, 176f), V(0.1f, 1.2f, 0.1f), 0f, false);
Prim(Cube, "Mailbox", tJ, V(roadX - roadW * 0.5f - 1.2f, G + 1.3f, 176f), V(0.5f, 0.3f, 0.25f), 0f, false);
// 8.14a (Marlow, Vesper: from the lot and the office the road was 1 to 5 px): white edge lines, white reflector posts every
// postStep with an amber reflector head, and a street light at the T (8 m, its sodium head on the lot lights' marker material) so
// the road and the T read from the lot by day and at night; the paint keeps its colour through 8.9f
const float edgeIn = 0.25f, reflPostH = 1.2f, reflPostW = 0.14f, reflHead = 0.18f, tLightH = 8f, tLightArm = 3f, tLightBack = 2.5f, tLightZ = 177f;
var paintMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_RoadPaint.mat");
if (paintMat == null) { paintMat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Unlit")); UnityEditor.AssetDatabase.CreateAsset(paintMat, "Assets/Materials/Blockout/Blockout_RoadPaint.mat"); }
{ UnityEngine.ColorUtility.TryParseHtmlString("#CFCBBE", out var pc); paintMat.SetColor("_BaseColor", pc); UnityEditor.EditorUtility.SetDirty(paintMat); }
var reflMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Blockout/Blockout_Reflector.mat");
if (reflMat == null) { reflMat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Unlit")); UnityEditor.AssetDatabase.CreateAsset(reflMat, "Assets/Materials/Blockout/Blockout_Reflector.mat"); }
{ UnityEngine.ColorUtility.TryParseHtmlString("#C87A2A", out var rc); reflMat.SetColor("_BaseColor", rc); UnityEditor.EditorUtility.SetDirty(reflMat); }
for (int i = 0; i < roadLine.Count - 1; i++)
{
    var a = roadLine[i]; var b = roadLine[i + 1]; var d = (b - a).normalized; var side = new UnityEngine.Vector2(d.y, -d.x) * (roadW * 0.5f - edgeIn);
    foreach (var sg in new[] { -1f, 1f }) Flat("EdgeLine", road, a + side * sg, b + side * sg, lineW, G + slab * 1.5f).GetComponent<UnityEngine.Renderer>().sharedMaterial = paintMat;
}
for (float z = roadS; z <= roadN; z += postStep) foreach (var sx in new[] { -1f, 1f })
{
    Prim(Cube, "ReflectorPost", road, V(roadX + sx * postOff, G + reflPostH * 0.5f, z), V(reflPostW, reflPostH, reflPostW), 0f, false).GetComponent<UnityEngine.Renderer>().sharedMaterial = paintMat;
    Prim(Cube, "Reflector", road, V(roadX + sx * postOff, G + reflPostH - reflHead, z), V(reflPostW + 0.02f, reflHead, reflPostW + 0.02f), 0f, false).GetComponent<UnityEngine.Renderer>().sharedMaterial = reflMat;
}
{
    float px = roadX - roadW * 0.5f - tLightBack;
    Prim(Cyl, "TLightPole", tJ, V(px, G + tLightH * 0.5f, tLightZ), V(0.22f, tLightH * 0.5f, 0.22f), 0f, false);
    Prim(Cube, "TLightArm", tJ, V(px + tLightArm * 0.5f, G + tLightH - 0.1f, tLightZ), V(tLightArm, 0.12f, 0.12f), 0f, false);
    Prim(Cube, "TLightHead", tJ, V(px + tLightArm, G + tLightH - 0.3f, tLightZ), V(lotHeadW, 0.3f, lotHeadD), 0f, false).GetComponent<UnityEngine.Renderer>().sharedMaterial = lotLightMat;
}
// the verge tree: a dead giant with a broken top, 6 m off the road edge, read from the deck at 250 m (3.4). 8.14a: the owned dead tree
// (Celestia Tree_Dead) stretched to a giant, with its snapped top lying on the verge beside it, no colliders (the player
// never reaches it); the gray cylinders are in git history
const float vergeTall = 30f, vergeGirth = 3f, deadTreeGirth = 0.6f, topShare = 0.25f;
var verge = Group("VergeTree", fz); var vp = new UnityEngine.Vector2(418f, 136f);
{
    var deadPf = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Celestia_Studio/PSX_Modular_Complete_Pack/Prefabs/Decoration_Out/Tree_Dead.prefab"); if (deadPf == null) return "missing Tree_Dead";
    var dt = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(deadPf, verge); dt.name = "DeadGiant";
    foreach (var c in dt.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    float h0 = 0f; foreach (var r in dt.GetComponentsInChildren<UnityEngine.Renderer>()) h0 = UnityEngine.Mathf.Max(h0, r.bounds.max.y);
    float full = vergeTall, sxz = vergeGirth / deadTreeGirth; dt.transform.localScale = V(sxz, full / UnityEngine.Mathf.Max(0.5f, h0), sxz);
    dt.transform.SetPositionAndRotation(V(vp.x, H(vp.x, vp.y) - 0.5f, vp.y), UnityEngine.Quaternion.Euler(0f, 35f, 3f));
    var stub = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(deadPf, verge); stub.name = "SnappedTop";
    foreach (var c in stub.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    stub.transform.localScale = V(sxz * 0.5f, (full * topShare) / UnityEngine.Mathf.Max(0.5f, h0), sxz * 0.5f);
    stub.transform.SetPositionAndRotation(V(vp.x + 4f, H(vp.x + 4f, vp.y + 3f), vp.y + 3f), UnityEngine.Quaternion.Euler(0f, 110f, 80f));   // lying on the verge
}

// warps: booth doorway, closed campground behind the chain
var warps = Root("DevWarps").transform;
var wb = warps.Find("Gate_Booth"); if (wb != null) { wb.position = V(389.5f, G + 0.2f, 176.05f); wb.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); }
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " ground at lot " + H(358f, 170f).ToString("F2") + ", gate " + H(395f, 170f).ToString("F2") + ", loop " + H(372f, 244.5f).ToString("F2") + " | IW3 inactive, gate blocker (IW1) on | highway " + roadLine.Count + " points, brush bands " + brush.childCount;
