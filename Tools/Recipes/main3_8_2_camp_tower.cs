// Main3 task 8.2: keeper's camp and tower, gray. Run after main3_8_1_scene_ground.cs in Main3, edit mode.
// Tower base (164, 166) on the 8 m knoll, deck top 56 m absolute (48 m up), eye 57.6 (Main3.md 5.1).
// Timber-style frame (square posts, beams, X braces; Vesper) in default gray: the office mast stays the only lattice.
// Stairs: 12 flights of 16 steps (0.25 rise, 0.3 run, 4 m per flight) switching back on two lanes south of the legs,
// each with a collider-only StairRamp along the nosings (STAIRS RULE). Cab 4.4 x 4.4 on an 8 x 8 deck (1.8 m walkway).
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
UnityEngine.GameObject Root(string name) { foreach (var r in scene.GetRootGameObjects()) if (r.name == name) return r; return null; }
if (Root("Camp") != null) return "Camp already exists; rebuild Main3 from 8.1";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = Root("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0f, z)) + terrain.transform.position.y;
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
if (tuning == null) return "no PlayerTuning";

UnityEngine.GameObject Group(string name, UnityEngine.Transform parent, UnityEngine.Vector3 local, float yaw = 0f)
{
    var g = new UnityEngine.GameObject(name); if (parent != null) g.transform.SetParent(parent, false);
    g.transform.localPosition = local; g.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); return g;
}
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name; go.transform.SetParent(parent, false); go.transform.localPosition = pos; go.transform.localScale = size;
    if (euler.HasValue) go.transform.localRotation = UnityEngine.Quaternion.Euler(euler.Value);
    return go;
}
// Door system (Rules and Tips: DOORS): hinge at the west jamb, gray panel child with the collider, kinematic body on the hinge.
UnityEngine.GameObject MakeDoor(UnityEngine.Transform parent, UnityEngine.Vector3 hingeLocal, float yaw)
{
    var hinge = Group("Door", parent, hingeLocal, yaw);
    var rb = hinge.AddComponent<UnityEngine.Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
    var panel = Box("Panel", hinge.transform, V(0.48f, 1.04f, 0f), V(0.96f, 2.08f, 0.08f));
    var door = hinge.AddComponent<Door>();
    var so = new UnityEditor.SerializedObject(door);
    so.FindProperty("prompt").stringValue = "Open"; so.FindProperty("tuning").objectReferenceValue = tuning;
    so.FindProperty("panel").objectReferenceValue = panel.GetComponent<UnityEngine.BoxCollider>();
    so.ApplyModifiedPropertiesWithoutUndo();
    return hinge;
}

var camp = Group("Camp", null, V(0f, 0f, 0f));

// ================= TOWER =================
float tx = 164f, tz = 166f, ty = H(tx, tz);
const float deckTop = 48f, deckThick = 0.4f, deckHalf = 4f, legAt = 3.25f;
var tower = Group("Tower", camp.transform, V(tx, ty, tz)); var T = tower.transform;
var frame = Group("Frame", T, V(0f, 0f, 0f)).transform;
foreach (var sx in new[] { -legAt, legAt }) foreach (var sz in new[] { -legAt, legAt })
    Box("Leg", frame, V(sx, (deckTop - deckThick) * 0.5f, sz), V(0.5f, deckTop - deckThick, 0.5f));
float span = legAt * 2f;
for (float lvl = 8f; lvl < deckTop; lvl += 8f)
{
    Box("BeamN", frame, V(0f, lvl, legAt), V(span, 0.3f, 0.3f)); Box("BeamS", frame, V(0f, lvl, -legAt), V(span, 0.3f, 0.3f));
    Box("BeamE", frame, V(legAt, lvl, 0f), V(0.3f, 0.3f, span)); Box("BeamW", frame, V(-legAt, lvl, 0f), V(0.3f, 0.3f, span));
}
// X braces between beam levels on every face
float bayH = 8f, diag = UnityEngine.Mathf.Sqrt(span * span + bayH * bayH), ang = UnityEngine.Mathf.Atan2(bayH, span) * UnityEngine.Mathf.Rad2Deg;
for (float b0 = 0f; b0 < deckTop - 1f; b0 += bayH)
{
    float cy = b0 + bayH * 0.5f;
    foreach (var s in new[] { 1f, -1f })
    {
        Box("BraceN", frame, V(0f, cy, legAt), V(diag, 0.2f, 0.2f), V(0f, 0f, s * ang));
        Box("BraceS", frame, V(0f, cy, -legAt), V(diag, 0.2f, 0.2f), V(0f, 0f, s * ang));
        Box("BraceE", frame, V(legAt, cy, 0f), V(0.2f, 0.2f, diag), V(s * ang, 0f, 0f));
        Box("BraceW", frame, V(-legAt, cy, 0f), V(0.2f, 0.2f, diag), V(s * ang, 0f, 0f));
    }
}
Box("Deck", frame, V(0f, deckTop - deckThick * 0.5f, 0f), V(deckHalf * 2f, deckThick, deckHalf * 2f));

// ---- stairs: lanes south of the legs; flight k climbs 4k to 4k + 4, even flights east on the north lane, odd west on the south lane
var stairs = Group("Stairs", T, V(0f, 0f, 0f)).transform;
const float laneN = -4.7f, laneS = -6.1f, laneW = 1.2f, run = 0.3f, rise = 0.25f, stepThick = 0.5f, railH = 1.0f;
const int steps = 16; float flightRun = steps * run, flightRise = steps * rise, halfRun = flightRun * 0.5f;
float theta = UnityEngine.Mathf.Atan2(rise, run) * UnityEngine.Mathf.Rad2Deg;
float rampLen = UnityEngine.Mathf.Sqrt(flightRun * flightRun + flightRise * flightRise);
const float landW = 1.5f, landZ0 = -4.1f, landZ1 = -6.7f;
float landDepth = landZ0 - landZ1, landCz = (landZ0 + landZ1) * 0.5f;
int flights = (int)(deckTop / flightRise);
for (int k = 0; k < flights; k++)
{
    bool east = k % 2 == 0; float z = east ? laneN : laneS; float h0 = k * flightRise; float dir = east ? 1f : -1f; float x0 = -dir * halfRun;
    var g = Group("Flight" + (k + 1), stairs, V(0f, 0f, 0f)).transform;
    for (int i = 0; i < steps; i++)
    {
        float top = h0 + rise * (i + 1);
        Box("Step" + (i + 1), g, V(x0 + dir * (run * i + run * 0.5f), top - stepThick * 0.5f, z), V(run, stepThick, laneW));
    }
    // StairRamp: top face along the nosings, from one run below the first nosing to the top nosing
    float nx0 = x0 - dir * run, nx1 = x0 + dir * (run * (steps - 1));
    float cx = (nx0 + nx1) * 0.5f, cy = h0 + flightRise * 0.5f;
    float rl = UnityEngine.Mathf.Sqrt((nx1 - nx0) * (nx1 - nx0) + flightRise * flightRise);
    var up = V(-dir * UnityEngine.Mathf.Sin(theta * UnityEngine.Mathf.Deg2Rad), UnityEngine.Mathf.Cos(theta * UnityEngine.Mathf.Deg2Rad), 0f);
    var ramp = Group("StairRamp", g, V(cx, cy, z) - up * 0.1f); ramp.transform.localRotation = UnityEngine.Quaternion.Euler(0f, 0f, dir * theta);
    ramp.transform.localScale = V(rl, 0.2f, laneW); ramp.AddComponent<UnityEngine.BoxCollider>();
    // side rails
    float rmid = h0 + flightRise * 0.5f + railH * 0.5f + 0.2f;
    Box("RailA", g, V(0f, rmid, z + laneW * 0.5f), V(rampLen, railH, 0.05f), V(0f, 0f, dir * theta));
    Box("RailB", g, V(0f, rmid, z - laneW * 0.5f), V(rampLen, railH, 0.05f), V(0f, 0f, dir * theta));
    // landing at the top end of this flight
    float top1 = h0 + flightRise; float lx = dir * (halfRun + landW * 0.5f); bool last = k == flights - 1;
    var L = Group("Landing" + (k + 1), stairs, V(0f, 0f, 0f)).transform;
    float z0 = last ? -deckHalf : landZ0;   // the top landing reaches the deck edge
    Box("Slab", L, V(lx, top1 - 0.15f, (z0 + landZ1) * 0.5f), V(landW, 0.3f, z0 - landZ1));
    float ry = top1 + railH * 0.5f; float outer = dir * (halfRun + landW);
    Box("RailOuter", L, V(outer, ry, (z0 + landZ1) * 0.5f), V(0.05f, railH, z0 - landZ1));
    Box("RailSouth", L, V(lx, ry, landZ1), V(landW, railH, 0.05f));
    if (!last) Box("RailNorth", L, V(lx, ry, landZ0), V(landW, railH, 0.05f));
    else { float za = -deckHalf, zb = laneN - laneW * 0.5f - 0.1f; Box("RailOverNorthLane", L, V(-halfRun, ry, (za + zb) * 0.5f), V(0.05f, railH, za - zb)); }   // the void over the north lane beside the top landing
}
foreach (var sx in new[] { -(halfRun + landW), halfRun + landW }) Box("StairPost", stairs, V(sx, deckTop * 0.5f, landZ1), V(0.25f, deckTop, 0.25f));

// ---- deck rails, with the opening onto the top landing (south-west corner)
var rails = Group("DeckRails", T, V(0f, 0f, 0f)).transform;
float dry = deckTop + 0.55f; float topLandX0 = -(halfRun + landW), topLandX1 = -halfRun;
Box("RailN", rails, V(0f, dry, deckHalf), V(deckHalf * 2f, 1.1f, 0.08f));
Box("RailE", rails, V(deckHalf, dry, 0f), V(0.08f, 1.1f, deckHalf * 2f));
Box("RailW", rails, V(-deckHalf, dry, 0f), V(0.08f, 1.1f, deckHalf * 2f));
Box("RailS", rails, V((topLandX1 + deckHalf) * 0.5f, dry, -deckHalf), V(deckHalf - topLandX1, 1.1f, 0.08f));

// ---- cab: 4.4 x 4.4, 2.5 m walls with a window band from 1.0 to 2.1 m, door in the south wall at x -1.1
var cab = Group("Cab", T, V(0f, deckTop, 0f)).transform;
const float wh = 2.5f, wt = 0.2f, sill = 1.0f, winTop = 2.1f; float cw = 4.4f, ch = cw * 0.5f - wt * 0.5f;
foreach (var side in new[] { "N", "E", "W" })
{
    bool ns = side == "N"; float sgn = side == "W" ? -1f : 1f;
    var c = ns ? V(0f, 0f, ch) : V(sgn * ch, 0f, 0f); var len = ns ? V(cw, 1f, wt) : V(wt, 1f, cw);
    Box(side + "_Sill", cab, c + V(0f, sill * 0.5f, 0f), V(len.x, sill, len.z));
    Box(side + "_Head", cab, c + V(0f, (wh + winTop) * 0.5f, 0f), V(len.x, wh - winTop, len.z));
    foreach (var e in new[] { -1f, 1f }) Box(side + "_Corner", cab, c + (ns ? V(e * (ch - 0.15f), (sill + winTop) * 0.5f, 0f) : V(0f, (sill + winTop) * 0.5f, e * (ch - 0.15f))), ns ? V(0.3f, winTop - sill, wt) : V(wt, winTop - sill, 0.3f));
}
// south wall: pier, doorway 1.0 m at x -1.1, then a window
Box("S_PierW", cab, V(-1.9f, wh * 0.5f, -ch), V(0.6f, wh, wt));
Box("S_Lintel", cab, V(-1.1f, (2.1f + wh) * 0.5f, -ch), V(1.0f, wh - 2.1f, wt));
Box("S_Sill", cab, V(0.8f, sill * 0.5f, -ch), V(2.8f, sill, wt));
Box("S_Head", cab, V(0.8f, (wh + winTop) * 0.5f, -ch), V(2.8f, wh - winTop, wt));
Box("S_Corner", cab, V(2.05f, (sill + winTop) * 0.5f, -ch), V(0.3f, winTop - sill, wt));
Box("Roof", cab, V(0f, wh + 0.1f, 0f), V(cw + 0.6f, 0.2f, cw + 0.6f));
MakeDoor(cab, V(-1.6f, 0f, -ch), 0f);
var lectern = Box("Lectern", cab, V(0f, 0.55f, 1.3f), V(0.5f, 1.1f, 0.4f));
Box("LecternTop", lectern.transform.parent, V(0f, 1.12f, 1.2f), V(0.6f, 0.05f, 0.45f), V(-15f, 0f, 0f));

// ================= CABIN: centre (178, 168), door facing the camp centre (south-west) =================
float kx = 178f, kz = 168f, ky = H(kx, kz);
var cabin = Group("Cabin", camp.transform, V(kx, ky, kz), 45f); var C = cabin.transform;
const float cwid = 6f, cdep = 4f, cwh = 2.6f, cwt = 0.2f, floorTop = 0.03f;
Box("Floor", C, V(0f, floorTop - 0.1f, 0f), V(cwid, 0.2f, cdep));
float fz = -cdep * 0.5f + cwt * 0.5f, bz = cdep * 0.5f - cwt * 0.5f, sx2 = cwid * 0.5f - cwt * 0.5f;
Box("Front_W", C, V((-cwid * 0.5f + -0.5f) * 0.5f, cwh * 0.5f, fz), V(cwid * 0.5f - 0.5f, cwh, cwt));
Box("Front_E", C, V((cwid * 0.5f + 0.5f) * 0.5f, cwh * 0.5f, fz), V(cwid * 0.5f - 0.5f, cwh, cwt));
Box("Front_Lintel", C, V(0f, (2.1f + cwh) * 0.5f, fz), V(1.0f, cwh - 2.1f, cwt));
Box("Back_Sill", C, V(0f, 0.5f, bz), V(cwid, 1.0f, cwt));
Box("Back_Head", C, V(0f, (2.0f + cwh) * 0.5f, bz), V(cwid, cwh - 2.0f, cwt));
Box("Back_PierW", C, V(-1.75f, 1.5f, bz), V(2.5f, 1.0f, cwt));
Box("Back_PierE", C, V(1.75f, 1.5f, bz), V(2.5f, 1.0f, cwt));
Box("Side_W", C, V(-sx2, cwh * 0.5f, 0f), V(cwt, cwh, cdep));
Box("Side_E", C, V(sx2, cwh * 0.5f, 0f), V(cwt, cwh, cdep));
Box("Roof", C, V(0f, cwh + 0.1f, 0f), V(cwid + 0.6f, 0.2f, cdep + 0.6f));
MakeDoor(C, V(-0.5f, floorTop, -cdep * 0.5f + cwt * 0.5f), 0f);
Box("Bunk", C, V(-2.4f, 0.3f, 0.7f), V(0.9f, 0.6f, 2.0f));
var desk = Box("Desk", C, V(1.9f, 0.375f, 1.45f), V(1.4f, 0.75f, 0.7f));
Box("ReportBox", C, V(2.3f, 0.9f, 1.45f), V(0.4f, 0.3f, 0.3f));
Box("Stove", C, V(-2.35f, 0.45f, -1.2f), V(0.7f, 0.9f, 0.7f));
var pipe = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); pipe.name = "StovePipe";
pipe.transform.SetParent(C, false); pipe.transform.localPosition = V(-2.35f, 1.9f, -1.2f); pipe.transform.localScale = V(0.15f, 1.0f, 0.15f);
UnityEngine.Object.DestroyImmediate(pipe.GetComponent<UnityEngine.Collider>());

// ================= FIRE PIT and GENERATOR =================
var pit = Group("FirePit", camp.transform, V(172f, H(172f, 163f), 163f)).transform;
for (int i = 0; i < 8; i++) { float a = i * UnityEngine.Mathf.PI / 4f; Box("Stone", pit, V(UnityEngine.Mathf.Cos(a) * 0.8f, 0.15f, UnityEngine.Mathf.Sin(a) * 0.8f), V(0.35f, 0.3f, 0.35f), V(0f, i * 45f, 0f)); }
Box("Logs", pit, V(0f, 0.12f, 0f), V(0.8f, 0.24f, 0.8f), V(0f, 30f, 0f));
Box("Generator", camp.transform, V(181.5f, H(181.5f, 171.5f) + 0.45f, 171.5f), V(1.2f, 0.9f, 0.7f), V(0f, 45f, 0f));

// ================= spawn and warps =================
UnityEngine.GameObject player = Root("Player");
player.transform.position = C.TransformPoint(V(0f, floorTop + 0.1f, 0.2f));
player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 225f, 0f);
var warps = Root("DevWarps").transform;
void Warp(string name, UnityEngine.Vector3 pos, float yaw) { var w = Group(name, warps, pos, yaw); w.transform.SetSiblingIndex(1); }
Warp("Tower_Deck", T.TransformPoint(V(-3f, deckTop + 0.2f, 3f)), 300f);
Warp("Cabin", C.TransformPoint(V(0f, floorTop + 0.2f, 0.2f)), 225f);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " towerBase=" + ty.ToString("F2") + " deckTop=" + (ty + deckTop).ToString("F2") + " eye=" + (ty + deckTop + 1.6f).ToString("F2") + " cabRoof=" + (ty + deckTop + wh + 0.2f).ToString("F2")
    + " flights=" + flights + " cabinFloor=" + (ky + floorTop).ToString("F2") + " spawn=" + player.transform.position.ToString("F1");
