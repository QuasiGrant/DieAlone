// Main3 task 8.2: keeper's camp and tower, gray. Run after main3_8_1_scene_ground.cs in Main3, edit mode.
// Tower base (164, 166) on the 15 m knoll (rev 13), deck top 56 m absolute (41 m up), eye 57.6 (Main3.md 5.1).
// Timber-style frame (square posts, beams, X braces; Vesper) in default gray: the office mast stays the only lattice.
// Stairs: a square spiral of ten 16-step flights inside the legs with corner landings, arriving through a deck hatch;
// each flight has a collider-only StairRamp along the nosings (STAIRS RULE). Cab 4.4 x 4.4 on an 8 x 8 deck (1.8 m walkway).
// Cabin 6 x 4.5 m inside with the rev 13 layout; the player wakes in the bunk.
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
float deckTop = 56f - ty;   // the deck stays at 56 m absolute on the raised knoll (rev 13: 41 m of legs on ground 15)
const float deckThick = 0.4f, deckHalf = 4f, legAt = 3.75f;
var tower = Group("Tower", camp.transform, V(tx, ty, tz)); var T = tower.transform;
var frame = Group("Frame", T, V(0f, 0f, 0f)).transform;
foreach (var sx in new[] { -legAt, legAt }) foreach (var sz in new[] { -legAt, legAt })
    Box("Leg", frame, V(sx, (deckTop - deckThick) * 0.5f, sz), V(0.5f, deckTop - deckThick, 0.5f));
float span = legAt * 2f;
for (float lvl = 8f; lvl < deckTop - 1f; lvl += 8f)
{
    Box("BeamN", frame, V(0f, lvl, legAt), V(span, 0.3f, 0.3f)); Box("BeamS", frame, V(0f, lvl, -legAt), V(span, 0.3f, 0.3f));
    Box("BeamE", frame, V(legAt, lvl, 0f), V(0.3f, 0.3f, span)); Box("BeamW", frame, V(-legAt, lvl, 0f), V(0.3f, 0.3f, span));
}
// X braces between beam levels on every face; the south face's bottom bay stays open as the way in to the stair
float bayH = 8f, diag = UnityEngine.Mathf.Sqrt(span * span + bayH * bayH), ang = UnityEngine.Mathf.Atan2(bayH, span) * UnityEngine.Mathf.Rad2Deg;
for (float b0 = 0f; b0 < deckTop - 1f; b0 += bayH)
{
    float cy = b0 + bayH * 0.5f;
    if (cy + bayH * 0.5f > deckTop - deckThick) break;   // only whole bays under the deck
    foreach (var s in new[] { 1f, -1f })
    {
        Box("BraceN", frame, V(0f, cy, legAt), V(diag, 0.2f, 0.2f), V(0f, 0f, s * ang));
        if (b0 > 0f) Box("BraceS", frame, V(0f, cy, -legAt), V(diag, 0.2f, 0.2f), V(0f, 0f, s * ang));
        Box("BraceE", frame, V(legAt, cy, 0f), V(0.2f, 0.2f, diag), V(s * ang, 0f, 0f));
        Box("BraceW", frame, V(-legAt, cy, 0f), V(0.2f, 0.2f, diag), V(s * ang, 0f, 0f));
    }
}

// ---- square spiral stair inside the legs (rev 13, 3.4.4): ten flights of 16 steps (0.3 run), turning left at 1 x 1 m
// corner landings, 2.5 turns from the south-west corner to the north-east corner at deck level, arriving through a hatch.
// Each flight has a collider-only StairRamp along its nosings (STAIRS RULE) and rails on both sides.
var stairs = Group("Stairs", T, V(0f, 0f, 0f)).transform;
const int flights = 10, steps = 16; const float run = 0.3f, laneW = 1.0f, stepThick = 0.5f, railH = 1.0f;
// invisible stops: 2.8 m over the highest floor they guard clears a 0.6 m jump with a 1.8 m capsule and margin; 0.1 m thick
const float stopH = 2.8f, stopDepth = 0.3f, stopThick = 0.1f;
UnityEngine.GameObject Stop(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size)
{
    var go = new UnityEngine.GameObject(name); go.layer = 2;   // Ignore Raycast: blocks the player, not sight checks
    go.transform.SetParent(parent, false); go.transform.localPosition = pos;
    go.AddComponent<UnityEngine.BoxCollider>().size = size; return go;
}
float rise = deckTop / (flights * steps), flightRise = rise * steps, flightRun = run * steps;
float ringC = (flightRun + laneW) * 0.5f;   // corner landing centres at (+-ringC, +-ringC), 2.9 m
var corners = new[] { V(-ringC, 0f, -ringC), V(ringC, 0f, -ringC), V(ringC, 0f, ringC), V(-ringC, 0f, ringC) };
float theta = UnityEngine.Mathf.Atan2(rise, run) * UnityEngine.Mathf.Rad2Deg;
float rampLen = UnityEngine.Mathf.Sqrt(flightRun * flightRun + flightRise * flightRise);
for (int k = 0; k < flights; k++)
{
    var from = corners[k % 4]; var to = corners[(k + 1) % 4]; var dir = (to - from).normalized; float h0 = k * flightRise;
    var g = Group("Flight" + (k + 1), stairs, from + dir * (laneW * 0.5f) + V(0f, h0, 0f), UnityEngine.Mathf.Atan2(dir.x, dir.z) * UnityEngine.Mathf.Rad2Deg).transform;
    // steps are looks only: the ramp carries the player (a step collider caught the capsule where the ramp meets the landing)
    for (int i = 0; i < steps; i++) UnityEngine.Object.DestroyImmediate(Box("Step" + (i + 1), g, V(0f, rise * (i + 1) - stepThick * 0.5f, run * i + run * 0.5f), V(laneW, stepThick, run)).GetComponent<UnityEngine.Collider>());
    // StairRamp: top face on the nosing line, from one run below the first nosing (z -run, y 0) to the top nosing (z run*(steps-1), y rise*steps)
    var ramp = Group("StairRamp", g, V(0f, 0f, 0f)); ramp.transform.localRotation = UnityEngine.Quaternion.Euler(-theta, 0f, 0f);
    var up = ramp.transform.localRotation * UnityEngine.Vector3.up;
    ramp.transform.localPosition = V(0f, rise * steps * 0.5f, run * (steps - 2) * 0.5f) - up * 0.1f;
    ramp.transform.localScale = V(laneW, 0.2f, rampLen); ramp.AddComponent<UnityEngine.BoxCollider>();
    foreach (var sx in new[] { -laneW * 0.5f, laneW * 0.5f })
        Box("Rail", g, V(sx, flightRise * 0.5f + railH * 0.5f + 0.1f, flightRun * 0.5f), V(0.05f, railH, rampLen * (flightRun - 0.8f) / flightRun), V(-theta, 0f, 0f));   // short of both ends, so the tilted rail never pokes into a landing
    // RailStops (8.9c re-walk 2): invisible walls just outside both lane edges, exactly the flight's length so none reaches
    // into a landing's open edge, from the foot to stopH over the top landing (a player leaping down from the top landing
    // flies well above the lower steps), on Ignore Raycast
    foreach (var sx in new[] { -1f, 1f })
        Stop("RailStop", g, V(sx * (laneW + stopThick) * 0.5f, (flightRise + stopH - stopDepth) * 0.5f, flightRun * 0.5f), V(stopThick, flightRise + stopH + stopDepth, flightRun));
    if (k < flights - 1)
    {
        // corner landing at the top of this flight, rails on its two outer edges
        var L = Group("Landing" + (k + 1), stairs, to + V(0f, h0 + flightRise, 0f)).transform;
        Box("Slab", L, V(0f, -0.15f, 0f), V(laneW, 0.3f, laneW));
        Box("RailX", L, V(UnityEngine.Mathf.Sign(to.x) * laneW * 0.5f, railH * 0.5f, 0f), V(0.05f, railH, laneW));
        Box("RailZ", L, V(0f, railH * 0.5f, UnityEngine.Mathf.Sign(to.z) * laneW * 0.5f), V(laneW, railH, 0.05f));
        float sOut = (laneW + stopThick) * 0.5f, sLen = laneW + stopThick * 2f, sTop = UnityEngine.Mathf.Min(flightRise + stopH, deckTop - deckThick - (h0 + flightRise)),   // as tall as the flight stops (players leap down onto landings too), never up through the deck walkway
              sy = (sTop - stopDepth) * 0.5f, sh = sTop + stopDepth;
        Stop("RailStopX", L, V(UnityEngine.Mathf.Sign(to.x) * sOut, sy, 0f), V(stopThick, sh, sLen));
        Stop("RailStopZ", L, V(0f, sy, UnityEngine.Mathf.Sign(to.z) * sOut), V(sLen, sh, stopThick));
    }
}
// deck with the hatch over the last flight (east side, x 2.4 to 3.4, z -2.4 to 2.4); rails round the hatch and the deck edge
float hx0 = ringC - laneW * 0.5f, hx1 = ringC + laneW * 0.5f, hz = ringC - laneW * 0.5f, dy = deckTop - deckThick * 0.5f;
Box("Deck_W", frame, V((-deckHalf + hx0) * 0.5f, dy, 0f), V(hx0 + deckHalf, deckThick, deckHalf * 2f));
Box("Deck_E", frame, V((hx1 + deckHalf) * 0.5f, dy, 0f), V(deckHalf - hx1, deckThick, deckHalf * 2f));
Box("Deck_HatchN", frame, V(ringC, dy, (hz + deckHalf) * 0.5f), V(laneW, deckThick, deckHalf - hz));
Box("Deck_HatchS", frame, V(ringC, dy, (-hz - deckHalf) * 0.5f), V(laneW, deckThick, deckHalf - hz));
var rails = Group("DeckRails", T, V(0f, 0f, 0f)).transform;
float dry = deckTop + 0.55f;
Box("RailN", rails, V(0f, dry, deckHalf), V(deckHalf * 2f, 1.1f, 0.08f)); Box("RailS", rails, V(0f, dry, -deckHalf), V(deckHalf * 2f, 1.1f, 0.08f));
Box("RailE", rails, V(deckHalf, dry, 0f), V(0.08f, 1.1f, deckHalf * 2f)); Box("RailW", rails, V(-deckHalf, dry, 0f), V(0.08f, 1.1f, deckHalf * 2f));
Box("HatchRailW", rails, V(hx0, dry, 0f), V(0.06f, 1.1f, hz * 2f)); Box("HatchRailE", rails, V(hx1, dry, 0f), V(0.06f, 1.1f, hz * 2f));
Box("HatchRailS", rails, V(ringC, dry, -hz), V(laneW, 1.1f, 0.06f));

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

// ================= CABIN (rev 13, 3.4.1): centre (178, 168), inside 6 m east-west by 4.5 m north-south, ceiling 2.7 m =================
// Door centred in the south wall; bunk along the north wall, west half (the player wakes in it); stove in the north-east
// corner with 1 m clear on its open sides; desk under the west window with the report box; a 1.5 m aisle from the door north.
float kx = 178f, kz = 168f, ky = H(kx, kz);
var cabin = Group("Cabin", camp.transform, V(kx, ky, kz), 0f); var C = cabin.transform;
const float cwid = 6f, cdep = 4.5f, cwh = 2.7f, cwt = 0.2f, floorTop = 0.03f;
float ox = cwid * 0.5f + cwt * 0.5f, oz = cdep * 0.5f + cwt * 0.5f, owid = cwid + cwt * 2f;
Box("Floor", C, V(0f, floorTop - 0.1f, 0f), V(owid, 0.2f, cdep + cwt * 2f));
Box("S_West", C, V((-owid * 0.5f - 0.5f) * 0.5f, cwh * 0.5f, -oz), V(owid * 0.5f - 0.5f, cwh, cwt));
Box("S_East", C, V((owid * 0.5f + 0.5f) * 0.5f, cwh * 0.5f, -oz), V(owid * 0.5f - 0.5f, cwh, cwt));
Box("S_Lintel", C, V(0f, (2.1f + cwh) * 0.5f, -oz), V(1.0f, cwh - 2.1f, cwt));
Box("N_Wall", C, V(0f, cwh * 0.5f, oz), V(owid, cwh, cwt));
Box("E_Wall", C, V(ox, cwh * 0.5f, 0f), V(cwt, cwh, cdep));
// west wall with a 1 m window (sill 1.0, head 2.0) over the desk
Box("W_Sill", C, V(-ox, 0.5f, 0f), V(cwt, 1.0f, cdep)); Box("W_Head", C, V(-ox, (2.0f + cwh) * 0.5f, 0f), V(cwt, cwh - 2.0f, cdep));
Box("W_PierS", C, V(-ox, 1.5f, (-cdep * 0.5f - 0.5f) * 0.5f), V(cwt, 1.0f, cdep * 0.5f - 0.5f));
Box("W_PierN", C, V(-ox, 1.5f, (cdep * 0.5f + 0.5f) * 0.5f), V(cwt, 1.0f, cdep * 0.5f - 0.5f));
Box("Roof", C, V(0f, cwh + 0.1f, 0f), V(owid + 0.6f, 0.2f, cdep + cwt * 2f + 0.6f));
MakeDoor(C, V(-0.5f, floorTop, -oz), 0f);
Box("Bunk", C, V(-1.9f, 0.3f, 1.8f), V(2.0f, 0.6f, 0.9f));
Box("Desk", C, V(-2.6f, 0.375f, 0f), V(0.7f, 0.75f, 1.4f));
Box("ReportBox", C, V(-2.6f, 0.9f, 0.35f), V(0.3f, 0.3f, 0.4f));
Box("Stove", C, V(2.6f, 0.45f, 1.85f), V(0.7f, 0.9f, 0.7f));
var pipe = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); pipe.name = "StovePipe";
pipe.transform.SetParent(C, false); pipe.transform.localPosition = V(2.6f, 1.85f, 1.85f); pipe.transform.localScale = V(0.15f, 1.0f, 0.15f);
UnityEngine.Object.DestroyImmediate(pipe.GetComponent<UnityEngine.Collider>());

// ================= FIRE PIT and GENERATOR =================
var pit = Group("FirePit", camp.transform, V(172f, H(172f, 163f), 163f)).transform;
for (int i = 0; i < 8; i++) { float a = i * UnityEngine.Mathf.PI / 4f; Box("Stone", pit, V(UnityEngine.Mathf.Cos(a) * 0.8f, 0.15f, UnityEngine.Mathf.Sin(a) * 0.8f), V(0.35f, 0.3f, 0.35f), V(0f, i * 45f, 0f)); }
Box("Logs", pit, V(0f, 0.12f, 0f), V(0.8f, 0.24f, 0.8f), V(0f, 30f, 0f));
Box("Generator", camp.transform, V(183f, H(183f, 172.5f) + 0.45f, 172.5f), V(1.2f, 0.9f, 0.7f));   // behind the cabin

// ================= spawn and warps =================
UnityEngine.GameObject player = Root("Player");
player.transform.position = C.TransformPoint(V(-1.9f, 0.7f, 1.8f));   // wakes in the bunk (rev 13)
player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);
var warps = Root("DevWarps").transform;
void Warp(string name, UnityEngine.Vector3 pos, float yaw) { var w = Group(name, warps, pos, yaw); w.transform.SetSiblingIndex(1); }
Warp("Tower_Deck", T.TransformPoint(V(-3f, deckTop + 0.2f, 3f)), 300f);
Warp("Cabin", C.TransformPoint(V(0f, floorTop + 0.2f, 0f)), 180f);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " towerBase=" + ty.ToString("F2") + " deckTop=" + (ty + deckTop).ToString("F2") + " eye=" + (ty + deckTop + 1.6f).ToString("F2") + " cabRoof=" + (ty + deckTop + wh + 0.2f).ToString("F2")
    + " flights=" + flights + " cabinFloor=" + (ky + floorTop).ToString("F2") + " spawn=" + player.transform.position.ToString("F1");
