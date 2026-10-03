// Main3 task 8.21, the keeper's camp to CampLayout.md draft 2 (Sable 2026-10-02): layout and walkability only, dressing is 11.0.
// Edit mode, Main3; rerunnable (every move is to an absolute place). In the runner after main3_8_19_forest_dense.cs (it clears forest
// pieces off the new spots) and before main3_8_18a_solid.cs (which gives the new pieces their hulls where they have none).
// Cabin local metres in the doc: origin the inside south-west corner (175, 165.75), X east, Z north; the cabin group's own origin is the
// room centre (178, 168), so a doc (X, Z) is cabin-local (X - 3, Z - 2.25).
// 1. Interior (doc 2): bunk on the north wall, east half, pillow east, collider 2.0 x 0.9 (C1); stove in the north-west corner, collider
//    0.7 x 0.7 (C2); wood box (C4); counter and shelf on the north wall (C5); desk and chair on the west wall, Z 1.0 to 2.2 (C6; the window
//    stays where 8.9d built it, centred Z 2.25, not 1.6: moving it means rebuilding the west wall modules, listed for Sable); washstand
//    on the south wall (C7). The 8.2 boxes that carry the bunk, stove and desk collision move with them; the pack meshes keep none.
// 2. The player wakes at (4.9, 3.2) facing 245 (doc 1, Pim's heading).
// 3. Outside (doc 3): the fire pit is cold (no flame, light or prompt); both porch end railings go; the woodpile stacks against the east wall, x 181.2 to 182.2; the chopping block stays
//    the stump at (182.8, 165.4) (already there); the generator moves to (181, 172.8); the fire pit to (169, 156) (doc 2.5).
// 4. Privy (doc 4) at (176.5, 178): the CITW outhouse, door 1.0 m facing west, box walls, a 0.45 m bench, a floor at the door.
// 5. Overlaps (doc 4.4): forest pieces (trees, saplings, logs) whose bounds touch the stump, generator, privy or fire pit footprint
//    plus clearance are removed; any other collider there (hedge boxes, stops) is listed, not moved.
// 6. The rune post (doc 5): to the shelf's uphill (west) side, and an owned BK BigBoulders rock with a collider on the outer rim on the
//    line to the deck, its top runeScreenOver m over the post top. The deck pixel check (main3_deck_pixel_check.cs) is the proof.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
var camp = kit.Root("Camp"); if (camp == null) return "no Camp";
var C = camp.transform.Find("Cabin"); var inside = C != null ? C.Find("Interior") : null; var shell = C != null ? C.Find("Shell") : null; var porch = C != null ? C.Find("Porch") : null; var wood = C != null ? C.Find("Woodpile") : null;
if (C == null || inside == null || shell == null || porch == null || wood == null) return "run 8.2 and 8.9d first (Cabin, Interior, Shell, Porch, Woodpile)";
const float floorTop = 0.03f, wallIn = 2.25f, wallGap = 0.02f;   // floor top; the inside face of the north and south walls (cabin-local z)
var notes = new System.Collections.Generic.List<string>();
string F(float v) => v.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
UnityEngine.Transform Child(UnityEngine.Transform p, string n) { var t = p.Find(n); if (t == null) notes.Add("no " + p.name + "/" + n); return t; }
// a piece turned to yaw, its mesh bounds centred on (lx, lz) in cabin space; northFace: its north side on the north wall instead
void Seat(UnityEngine.Transform t, float yaw, float lx, float lz, bool northFace = false)
{
    if (t == null) return; t.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    var b = PlaceKit.LocalBounds(t.gameObject, C); var p = t.localPosition;
    t.localPosition = V(p.x + lx - b.center.x, p.y, northFace ? p.z + (wallIn - wallGap) - b.max.z : p.z + lz - b.center.z);
}
void Box(string n, float lx, float ly, float lz, float sx, float sy, float sz) { var t = Child(C, n); if (t == null) return; t.localPosition = V(lx, ly, lz); t.localScale = V(sx, sy, sz); }
UnityEngine.Transform ByName(UnityEngine.Transform p, string n) { foreach (UnityEngine.Transform c in p) if (c.name == n) return c; notes.Add("no " + p.name + "/" + n); return null; }

// ---- 1. interior
// C1 bunk: X 3.9 to 5.9, Z 3.6 to 4.5, pillow east. The doc's collider sizes win over the mesh (2.2): the pack bed (2.48 x 1.48) is
// fitted to the 2.0 x 0.9 box at its own height, so no mattress lies outside the box (8.21 gate, Marlow 1: the solid pass had given
// the overhang a hull and the wake spawned on the mattress)
const float bunkX = 1.9f, bunkZ = 1.8f, bunkW = 2.0f, bunkD = 0.9f, bunkH = 0.6f;
{
    var oldBed = ByName(inside, "CITW_Bed"); float bedH = oldBed != null ? PlaceKit.LocalBounds(oldBed.gameObject, inside).size.y : 1f;
    if (oldBed != null && UnityEngine.Mathf.Abs(PlaceKit.LocalBounds(oldBed.gameObject, inside).size.x - bunkW) > 0.02f) { PlaceKit.Remove(oldBed); var nb = kit.Fill(PlaceKit.CI + "Furniture/CITW_Bed", inside, V(bunkX, floorTop, bunkZ), V(bunkW, bedH, bunkD), 270f, false); if (nb != null) nb.name = "CITW_Bed"; }
    var bedNow = inside.Find("CITW_Bed"); if (bedNow != null) PlaceKit.StripColliders(bedNow.gameObject);
}
Box("Bunk", bunkX, bunkH * 0.5f, bunkZ, bunkW, bunkH, bunkD);
// C2 stove: X 0.45 to 1.15, Z 3.35 to 4.05, back to the north wall. Its collider stands stoveColH m tall, so its top cannot be jumped
// onto (8.21 gate, Marlow 3: a sprint-jump landed on the stove top under the roof box and could not walk off)
const float stoveX = -2.2f, stoveZ = 1.45f, stoveS = 0.7f, stoveColH = 2.0f;
var stove = ByName(inside, "CITW_Wood_Stove"); Seat(stove, 0f, stoveX, stoveZ); if (stove != null) PlaceKit.StripColliders(stove.gameObject);
Box("Stove", stoveX, stoveColH * 0.5f, stoveZ, stoveS, stoveColH, stoveS);
var kettle = ByName(inside, "CITW_Kettle"); if (kettle != null) kettle.localPosition = V(stoveX, kettle.localPosition.y, stoveZ);
var stoveLight = ByName(inside, "StoveLight"); if (stoveLight != null) stoveLight.localPosition = V(stoveX + 0.3f, stoveLight.localPosition.y, stoveZ - 0.3f);
var pipe = ByName(shell, "StovePipe"); if (pipe != null) pipe.localPosition = V(stoveX, pipe.localPosition.y, stoveZ);
// C4 wood box X 2.15 to 2.6; C5 counter and shelf X 2.6 to 3.4, Z 3.9 to 4.5
PlaceKit.Remove(inside.Find("WoodBox")); var woodBox = kit.Fill(PlaceKit.CI + "Props/CITW_Crate", inside, V(-0.625f, floorTop, wallIn - 0.25f), V(0.45f, 0.5f, 0.45f), 0f, true); if (woodBox != null) woodBox.name = "WoodBox";
PlaceKit.Remove(inside.Find("Counter")); var counter = kit.Fill(PlaceKit.CI + "Furniture/CITW_Dresser", inside, V(0f, floorTop, wallIn - 0.3f), V(0.8f, 0.9f, 0.6f), 180f, true); if (counter != null) counter.name = "Counter";
var shelf = ByName(inside, "CITW_Shelf"); Seat(shelf, 180f, 0f, 0f, true);
float shelfY = shelf != null ? shelf.localPosition.y : 1.6f;
var can1 = ByName(inside, "CITW_Canned_Food_1"); if (can1 != null) can1.localPosition = V(-0.15f, can1.localPosition.y, wallIn - 0.2f);
var can2 = ByName(inside, "CITW_Canned_Food_2"); if (can2 != null) can2.localPosition = V(0.15f, can2.localPosition.y, wallIn - 0.2f);
// C6 desk under the window (centre Z 2.25), its things and the chair with it
const float chairBack = -0.15f;   // 8.21 round 2 (Marlow): the chair 0.88 m from the stove, in the 0.6 to 1.0 band; 0.15 m south makes it 1.03
const float deskShift = 0f;   // 8.21 gate (Sable 3.1): the desk centred on the window (Z 2.25), its 1.4 m from Z 1.55 to 2.95
foreach (var n in new[] { "CITW_Table", "CITW_Crate", "CITW_Oil_Lamp_1", "DeskLamp", "CITW_Mug", "CITW_Book_1", "CITW_Book_3", "MatchesV1", "CITW_Chair" })
{ var t = ByName(inside, n); if (t != null) t.localPosition = V(t.localPosition.x, t.localPosition.y, (n == "CITW_Book_1" ? -0.62f : n == "CITW_Book_3" ? -0.55f : n == "CITW_Oil_Lamp_1" || n == "DeskLamp" ? -0.4f : n == "MatchesV1" ? -0.35f : n == "CITW_Mug" ? -0.05f : n == "CITW_Crate" ? 0.35f : n == "CITW_Chair" ? chairBack : 0f) + deskShift); }
foreach (var n in new[] { "Desk", "ReportBox" }) { var t = Child(C, n); if (t != null) t.localPosition = V(t.localPosition.x, t.localPosition.y, (n == "ReportBox" ? 0.35f : 0f) + deskShift); }
var water = ByName(inside, "Water"); if (water != null) water.localPosition = V(-2.6f, water.localPosition.y, -0.3f + deskShift);   // under the desk, off the walking floor
// the desk 1.2 m long, Z 1.65 to 2.85 (8.21 gate, Sable 3.1; Pim: 0.5 m edge to the stove at Z 3.35): its 8.2 box and the pack table fitted to it
const float deskLen = 1.2f, deskW = 0.7f; var deskBox = C.Find("Desk"); if (deskBox != null) deskBox.localScale = V(deskW, deskBox.localScale.y, deskLen);
{ var tbl = ByName(inside, "CITW_Table"); if (tbl != null) { var tb = PlaceKit.LocalBounds(tbl.gameObject, inside); if (UnityEngine.Mathf.Abs(tb.size.z - deskLen) > 0.02f) { float th = tb.size.y; var tp = tbl.localPosition; PlaceKit.Remove(tbl); var nt = kit.Fill(PlaceKit.CI + "Furniture/CITW_Table", inside, V(tp.x, floorTop, deskShift), V(deskW, th, deskLen), 90f, false); if (nt != null) nt.name = "CITW_Table"; } } }
// C7 washstand X 4.0 to 4.6, Z 0 to 0.5
PlaceKit.Remove(inside.Find("Washstand")); var wash = kit.Fill(PlaceKit.CI + "Furniture/CITW_Nightstand", inside, V(1.3f, floorTop, -wallIn + 0.25f), V(0.6f, 0.8f, 0.5f), 0f, true); if (wash != null) wash.name = "Washstand";

// C8 the door (doc 4.5): open at load and at every wake, always into the room (the hinge forward is north, into the room: side -1)
var cabinDoor = Child(C, "Door") != null ? C.Find("Door").GetComponent<Door>() : null;
if (cabinDoor != null) { var dso = new UnityEditor.SerializedObject(cabinDoor); dso.FindProperty("startOpen").boolValue = true; dso.FindProperty("fixedSwing").boolValue = true; dso.FindProperty("fixedSide").floatValue = -1f; dso.ApplyModifiedPropertiesWithoutUndo(); } else notes.Add("no Door on Camp/Cabin/Door");

// ---- 2. wake spot (doc X 4.9, Z 3.2), facing 245
var wakeAt = C.TransformPoint(V(1.9f, floorTop, 0.95f));
var player = kit.Root("Player"); if (player != null) { player.transform.position = wakeAt + V(0f, 0.1f, 0f); player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 245f, 0f); } else notes.Add("no Player");
// the Cabin dev warp lands on the wake spot (8.21 gate, Marlow 9); warps stand 0.2 m over the floor as 8.1's
var cabinWarp = kit.Root("DevWarps") != null ? kit.Root("DevWarps").transform.Find("Cabin") : null;
if (cabinWarp != null) { cabinWarp.position = wakeAt + V(0f, 0.2f, 0f); cabinWarp.rotation = UnityEngine.Quaternion.Euler(0f, 245f, 0f); } else notes.Add("no DevWarps/Cabin");
// the open door clears 1.0 m (8.21 gate, Marlow 6: the open leaf stood 0.04 m inside the opening): the hinge sits half the leaf's
// thickness into the west jamb
const float hingeX = -0.54f; var hinge = C.Find("Door"); if (hinge != null) hinge.localPosition = V(hingeX, hinge.localPosition.y, hinge.localPosition.z);
// the porch roof takes a collider (8.21 gate, Marlow 5: a jump on the porch put the eye through it)
var porchRoof = porch.Find("PorchRoof"); if (porchRoof != null && porchRoof.GetComponent<UnityEngine.Collider>() == null) porchRoof.gameObject.AddComponent<UnityEngine.BoxCollider>(); else if (porchRoof == null) notes.Add("no Porch/PorchRoof");

// ---- 3. outside
int rails = 0; var railList = new System.Collections.Generic.List<UnityEngine.GameObject>(); foreach (UnityEngine.Transform c in porch) if (c.name == "CITW_Railing") railList.Add(c.gameObject); foreach (var g in railList) { UnityEngine.Object.DestroyImmediate(g); rails++; }
// woodpile: one column at x 181.7 (cabin x 3.7), z 166.5 to 170 (the stump and axe stay at (182.8, 165.4))
var woodZ = new System.Collections.Generic.Dictionary<string, float> { { "CS_Firewood_Logs", -1.4f }, { "CS_Log_Firewood", -0.2f }, { "CITW_Firewood_1", 1.0f }, { "CITW_Firewood_2", 1.9f } };
const float woodX = 3.7f;
foreach (var kv in woodZ)
{
    var t = ByName(wood, kv.Key); if (t == null) continue; var before = t.position; var b = PlaceKit.LocalBounds(t.gameObject, C);
    t.localPosition += V(woodX - b.center.x, 0f, kv.Value - b.center.z); var after = t.position;
    t.position += V(0f, kit.H(after.x, after.z) - kit.H(before.x, before.z), 0f);
}
void MoveOnGround(UnityEngine.Transform t, UnityEngine.Vector3 delta)   // t and every child keep their height over the ground
{
    if (t == null) return; var kids = new System.Collections.Generic.List<(UnityEngine.Transform, UnityEngine.Vector3)>(); foreach (UnityEngine.Transform c in t) kids.Add((c, c.position));
    var p0 = t.position; t.position = V(p0.x + delta.x, p0.y + kit.H(p0.x + delta.x, p0.z + delta.z) - kit.H(p0.x, p0.z), p0.z + delta.z);
    foreach (var (c, was) in kids) c.position = V(was.x + delta.x, was.y + kit.H(was.x + delta.x, was.z + delta.z) - kit.H(was.x, was.z), was.z + delta.z);
}
// generator to (181, 172.8): its 8.2 box and its 8.9d dressing
var gen = Child(camp.transform, "Generator"); var genD = Child(camp.transform, "GeneratorDressing");
if (gen != null) { var d = V(181f - gen.position.x, 0f, 172.8f - gen.position.z); if (d.sqrMagnitude > 1e-4f) { var g0 = gen.position; gen.position = V(181f, g0.y + kit.H(181f, 172.8f) - kit.H(g0.x, g0.z), 172.8f); if (genD != null) foreach (UnityEngine.Transform c in genD) { var w = c.position; c.position = V(w.x + d.x, w.y + kit.H(w.x + d.x, w.z + d.z) - kit.H(w.x, w.z), w.z + d.z); } } }
// fire pit to (169, 156), cold (doc 2.5; open question 2 for Grant on FirePit.cs)
var pit = Child(camp.transform, "FirePit"); if (pit != null) { var d = V(169f - pit.position.x, 0f, 156f - pit.position.z); if (d.sqrMagnitude > 1e-4f) { var dress = pit.Find("PitDressing"); MoveOnGround(pit, d); if (dress != null) foreach (UnityEngine.Transform c in dress) { var w = c.position; c.position = V(w.x, kit.H(w.x, w.z) + (w.y - kit.H(w.x - d.x, w.z - d.z)), w.z); } } }
// the fire pit stays cold (Status.md 2026-10-02, CampLayout open question 2 settled): no flame, no light, no prompt; its stones, logs and tripod stay as dressing
int coldOff = 0; if (pit != null) { var fpc = pit.GetComponent<FirePit>(); if (fpc != null) { UnityEngine.Object.DestroyImmediate(fpc); coldOff++; } var pitDress = pit.Find("PitDressing"); if (pitDress != null) foreach (UnityEngine.Transform c in pitDress) if (c.name.StartsWith("FX_Flames") || c.name == "FirePitLight") { if (c.gameObject.activeSelf) coldOff++; c.gameObject.SetActive(false); } }
// the pit's seats 2.9 m out (8.21 round 2, Marlow: a wedge at (167.75, 15.04, 156) between the pit box and two log seats held the walker;
// 8.9d set them at 2.3 m): each seat, chair and stool moved along its own bearing from the pit centre, back on the ground
const float seatOut = 2.9f; int seatsMoved = 0;
if (pit != null) { var pdr = pit.Find("PitDressing"); if (pdr != null) foreach (UnityEngine.Transform c in pdr) { if (!(c.name.StartsWith("CS_Log_Large_Seat") || c.name.StartsWith("CS_Chair") || c.name.StartsWith("CS_Log_Stool") || c.name.StartsWith("CS_Tableware_Mug"))) continue; var cp = c.position; var off = new UnityEngine.Vector2(cp.x - pit.position.x, cp.z - pit.position.z); if (off.magnitude < 0.5f || UnityEngine.Mathf.Abs(off.magnitude - seatOut) < 0.02f) continue; var np = new UnityEngine.Vector2(pit.position.x, pit.position.z) + off.normalized * seatOut; c.position = V(np.x, cp.y + kit.H(np.x, np.y) - kit.H(cp.x, cp.z), np.y); seatsMoved++; } }

// ---- 4. privy at (176.5, 178), door west; outside 2.0 x 1.8 (the door 1.0 m clear), box walls, bench, floor
const float privyX = 176.5f, privyZ = 178f, privyYaw = 270f, privyWall = 0.1f, privyWallH = 2.4f, benchH = 0.45f, benchD = 0.45f;
var privy = kit.Fresh("Privy", camp.transform, V(privyX, kit.H(privyX, privyZ), privyZ), privyYaw);
var outhouse = kit.Spawn(PlaceKit.CI + "Building/CITW_Outhouse", privy);
float pw = 2f, pd = 1.8f;
if (outhouse != null)
{
    outhouse.transform.localPosition = UnityEngine.Vector3.zero; outhouse.transform.localRotation = UnityEngine.Quaternion.identity; outhouse.transform.localScale = V(1f, 1f, pd / 2f);
    var b = PlaceKit.LocalBounds(outhouse, privy);
    // the floor sits on the ground at the door (local +z), so the threshold is flush
    float doorGround = kit.H(privy.TransformPoint(V(0f, 0f, pd * 0.5f)).x, privy.TransformPoint(V(0f, 0f, pd * 0.5f)).z) - privy.position.y;
    outhouse.transform.localPosition = V(-b.center.x, doorGround - b.min.y, -b.center.z);
    // the leaf stands open flat against the outside of the front wall, off the opening: the first turn (10 degree steps) whose bounds lie
    // outside the front face and clear of the opening's width
    var leaf = outhouse.transform.Find("CITW_Outhouse Door"); float leafYaw = float.NaN;
    if (leaf != null)
        for (float a = 0f; a < 360f && float.IsNaN(leafYaw); a += 10f)
        {
            leaf.localRotation = UnityEngine.Quaternion.Euler(0f, a, 0f); var lb = PlaceKit.LocalBounds(leaf.gameObject, privy);
            if (lb.min.z >= pd * 0.5f - 0.05f && lb.size.z <= 0.4f && (lb.max.x <= -0.5f || lb.min.x >= 0.5f)) leafYaw = a;
        }
    if (leaf != null && float.IsNaN(leafYaw)) { leaf.gameObject.SetActive(false); notes.Add("privy leaf: no turn lies flat outside the front wall, so it is off (an open doorway; the leaf is dressing, 11.0)"); }
    // the shed takes its exact mesh as its collider (a convex hull from the solid pass would fill it); the open leaf a box
    var shedCol = outhouse.AddComponent<UnityEngine.MeshCollider>(); shedCol.sharedMesh = outhouse.GetComponent<UnityEngine.MeshFilter>().sharedMesh; shedCol.convex = false;
    if (leaf != null && leaf.gameObject.activeSelf) PlaceKit.FitCollider(leaf.gameObject);
    UnityEngine.Physics.SyncTransforms();
    // the shed's own floor on the ground at the door (its floor is not the mesh's lowest point)
    var down = new UnityEngine.Ray(privy.TransformPoint(V(0f, doorGround + 2f, 0.2f)), UnityEngine.Vector3.down);
    if (shedCol.Raycast(down, out var fh, 6f)) outhouse.transform.position += V(0f, privy.position.y + doorGround - fh.point.y, 0f); else notes.Add("privy: no shed floor under its centre");
    UnityEngine.Physics.SyncTransforms();
    var cols = new UnityEngine.GameObject("Colliders").transform; cols.SetParent(privy, false);
    void Wall(string n, UnityEngine.Vector3 c, UnityEngine.Vector3 s) { var g = new UnityEngine.GameObject(n); g.transform.SetParent(cols, false); g.transform.localPosition = c; var bc = g.AddComponent<UnityEngine.BoxCollider>(); bc.size = s; }
    float fy = doorGround;
    Wall("Floor", V(0f, fy - 0.05f, 0f), V(pw, 0.1f, pd));
    Wall("Back", V(0f, fy + privyWallH * 0.5f, -pd * 0.5f + privyWall * 0.5f), V(pw, privyWallH, privyWall));
    Wall("SideL", V(-pw * 0.5f + privyWall * 0.5f, fy + privyWallH * 0.5f, 0f), V(privyWall, privyWallH, pd));
    Wall("SideR", V(pw * 0.5f - privyWall * 0.5f, fy + privyWallH * 0.5f, 0f), V(privyWall, privyWallH, pd));
    foreach (var sx in new[] { -1f, 1f }) Wall("Front" + (sx < 0f ? "L" : "R"), V(sx * (pw * 0.5f - 0.25f), fy + privyWallH * 0.5f, pd * 0.5f - privyWall * 0.5f), V(0.5f, privyWallH, privyWall));
    Wall("Bench", V(0f, fy + benchH * 0.5f, -pd * 0.5f + privyWall + benchD * 0.5f), V(pw - 2f * privyWall, benchH, benchD));
}

// ---- 5. overlaps at the stump, generator, privy and fire pit
var spots = new (string n, UnityEngine.Vector3 c, UnityEngine.Vector3 half)[] {
    ("stump", V(182.8f, 0f, 165.4f), V(0.5f, 0f, 0.5f)), ("generator", V(181f, 0f, 172.8f), V(0.6f, 0f, 0.35f)),
    ("privy", V(privyX, 0f, privyZ), V(pd * 0.5f, 0f, pw * 0.5f)), ("privy door", V(privyX - pd * 0.5f - 0.6f, 0f, privyZ), V(0.6f, 0f, 0.6f)),
    ("fire pit", V(169f, 0f, 156f), V(1.6f, 0f, 1.6f)) };
const float clear = 0.3f; int removed = 0; var listed = new System.Collections.Generic.List<string>();
var forest = kit.Root("Forest");
foreach (var s in spots)
{
    var box = new UnityEngine.Bounds(V(s.c.x, kit.H(s.c.x, s.c.z) + 1.5f, s.c.z), V(2f * (s.half.x + clear), 3f, 2f * (s.half.z + clear)));
    if (forest != null)
        foreach (var r in forest.GetComponentsInChildren<UnityEngine.Renderer>())
        {
            if (r == null || !r.bounds.Intersects(box)) continue;
            var root = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(r.gameObject); var t = root != null ? root.transform : r.transform;
            var rb = PlaceKit.MeshBounds(t.gameObject); if (UnityEngine.Mathf.Max(rb.extents.x, rb.extents.z) > 15f) { listed.Add(s.n + ": left " + WalkIns.PathOf(t) + " (a group " + F(rb.size.x) + " m across)"); continue; }
            if (UnityEngine.Mathf.Max(rb.extents.x, rb.extents.z) > 6f && !(new UnityEngine.Bounds(V(rb.center.x, box.center.y, rb.center.z), V(1.5f, 3f, 1.5f)).Intersects(box))) continue;   // a big crown overhead whose trunk stands clear
            listed.Add(s.n + ": removed " + WalkIns.PathOf(t)); UnityEngine.Object.DestroyImmediate(t.gameObject); removed++;
        }
    UnityEngine.Physics.SyncTransforms();
    foreach (var col in UnityEngine.Physics.OverlapBox(box.center, box.extents, UnityEngine.Quaternion.identity, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
    {
        if (col is UnityEngine.TerrainCollider || col.transform.IsChildOf(privy) || col.transform.IsChildOf(wood) || (gen != null && col.transform.IsChildOf(gen)) || (pit != null && col.transform.IsChildOf(pit)) || (genD != null && col.transform.IsChildOf(genD))) continue;
        listed.Add(s.n + ": collider " + WalkIns.PathOf(col.transform) + " (left; the owning recipe moves it)");
    }
}

// ---- 5b. the clearing's edge (8.21 gate, Vesper 1: E and S of the Keepers_Camp warp were bare level dirt to a bush wall): a ragged
// line of firs and saplings about edgeOut m past the fire pit on the south (z) and east (x), off the trails, the warps and the pit's
// seats; every third spot far enough from a trail a fir with its trunk capsule (as 8.19), the rest saplings without colliders. Seeded,
// rebuilt each run under Camp/ClearingEdge.
const float edgeOut = 6f, edgeStep = 2.2f, edgeJitter = 0.7f, edgeSaplingTrail = 2.5f, edgeFirTrail = 3.5f, edgeWarpClear = 2.5f, edgePitClear = 3.5f, edgeX0 = 158f, edgeZ0 = 145f, edgeZ1 = 158f;
var trailPts = new System.Collections.Generic.List<UnityEngine.Vector2>();
foreach (UnityEngine.Transform leg in kit.Root("Trails").transform) { UnityEngine.Vector3? prev = null; foreach (UnityEngine.Transform pt in leg) { var q = pt.position; if (prev.HasValue) for (int i = 1; i <= 8; i++) { var m = UnityEngine.Vector3.Lerp(prev.Value, q, i / 8f); trailPts.Add(new UnityEngine.Vector2(m.x, m.z)); } else trailPts.Add(new UnityEngine.Vector2(q.x, q.z)); prev = q; } }
float TrailD(UnityEngine.Vector2 p) { float d = float.MaxValue; foreach (var t in trailPts) d = UnityEngine.Mathf.Min(d, (t - p).magnitude); return d; }
var warpPts = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform w in kit.Root("DevWarps").transform) warpPts.Add(new UnityEngine.Vector2(w.position.x, w.position.z));
var edge = kit.Fresh("ClearingEdge", camp.transform, camp.transform.position, 0f); var erng = new System.Random(8211); float ER(float a, float b) => a + (float)erng.NextDouble() * (b - a);
string[] edgeFirs = { PlaceKit.BK + "Trees/RedFir5", PlaceKit.BK + "Trees/RedFir6", PlaceKit.BK + "Trees/RedFir7" }, edgeSaplings = { PlaceKit.BK + "Trees/RedFir1", PlaceKit.BK + "Trees/RedFir2", PlaceKit.BK + "Trees/RedFir3", PlaceKit.BK + "Trees/RedFir4" };
var pitC = new UnityEngine.Vector2(169f, 156f); float southZ = pitC.y - edgeOut, eastX = pitC.x + edgeOut; int edgeFirN = 0, edgeSapN = 0, edgeSkip = 0, ek = 0;
var edgeSpots = new System.Collections.Generic.List<UnityEngine.Vector2>();
for (float x = edgeX0; x <= eastX; x += edgeStep) edgeSpots.Add(new UnityEngine.Vector2(x, southZ));
for (float z = edgeZ0; z <= edgeZ1; z += edgeStep) edgeSpots.Add(new UnityEngine.Vector2(eastX, z));
foreach (var s0 in edgeSpots)
{
    var s = s0 + new UnityEngine.Vector2(ER(-edgeJitter, edgeJitter), ER(-edgeJitter, edgeJitter)); float td = TrailD(s); bool nearWarp = false; foreach (var w in warpPts) if ((w - s).magnitude < edgeWarpClear) nearWarp = true;
    if (td < edgeSaplingTrail || nearWarp || (s - pitC).magnitude < edgePitClear || UnityEngine.Physics.CheckSphere(new UnityEngine.Vector3(s.x, kit.H(s.x, s.y) + 1f, s.y), 0.8f, ~0, UnityEngine.QueryTriggerInteraction.Ignore)) { edgeSkip++; continue; }
    bool fir = (ek++ % 3 == 0) && td >= edgeFirTrail;
    var g = kit.Ground(fir ? edgeFirs[erng.Next(edgeFirs.Length)] : edgeSaplings[erng.Next(edgeSaplings.Length)], edge, s.x, s.y, ER(0f, 360f), fir ? ER(0.8f, 1.0f) : ER(0.6f, 0.9f), false, 0.1f);
    if (g == null) continue; if (fir) { PlaceKit.PackTrunkCapsule(g, 0.01f, 0.06f, 0.4f, 8, 0.3f); edgeFirN++; } else edgeSapN++;
}
UnityEngine.Physics.SyncTransforms();
// ---- 5c. worn paths (8.21 gate, Vesper 2; CampLayout 3 steps 2, 6 and 9): the trail layer, narrow and part strength, from the porch to
// the stair foot by the tower's south side and the SW gap, from the porch's east end to the stump, and from the SW gap to the Camp to J
// mouth. Raises the trail share to wornShare at most, never lowers it, so a rerun paints the same.
const float wornHalf = 0.45f, wornBlend = 0.5f, wornShare = 0.75f;
var stairFoot = camp.transform.Find("Tower/Stairs/Flight1/Step1"); var sfp = stairFoot != null ? stairFoot.position : V(164f, 0f, 160f);
var wornRoutes = new[] {
    new[] { new UnityEngine.Vector2(178f, 162.6f), new UnityEngine.Vector2(171f, 160.6f), new UnityEngine.Vector2(160.6f, 160.6f), new UnityEngine.Vector2(sfp.x, sfp.z) },
    new[] { new UnityEngine.Vector2(181f, 162.8f), new UnityEngine.Vector2(182.6f, 164.4f) },
    new[] { new UnityEngine.Vector2(160.6f, 160.6f), new UnityEngine.Vector2(158.5f, 164f), new UnityEngine.Vector2(159f, 170.5f) } };
int wornCells = 0;
{
    var ter = kit.Terrain; var data = ter.terrainData; var tOrg = ter.transform.position; int trailLayer = -1; for (int k = 0; k < data.terrainLayers.Length; k++) if (data.terrainLayers[k].name == "Layer_Trail") trailLayer = k;
    if (trailLayer < 0) notes.Add("no Layer_Trail");
    else
    {
        float minX = float.MaxValue, minZ = float.MaxValue, maxX = float.MinValue, maxZ = float.MinValue;
        foreach (var r in wornRoutes) foreach (var p in r) { minX = UnityEngine.Mathf.Min(minX, p.x); minZ = UnityEngine.Mathf.Min(minZ, p.y); maxX = UnityEngine.Mathf.Max(maxX, p.x); maxZ = UnityEngine.Mathf.Max(maxZ, p.y); }
        float pad = wornHalf + wornBlend + 1f; float aX = data.size.x / data.alphamapWidth, aZ = data.size.z / data.alphamapHeight;
        int ax0 = UnityEngine.Mathf.Max(0, UnityEngine.Mathf.FloorToInt((minX - pad - tOrg.x) / aX)), az0 = UnityEngine.Mathf.Max(0, UnityEngine.Mathf.FloorToInt((minZ - pad - tOrg.z) / aZ));
        int ax1 = UnityEngine.Mathf.Min(data.alphamapWidth - 1, UnityEngine.Mathf.CeilToInt((maxX + pad - tOrg.x) / aX)), az1 = UnityEngine.Mathf.Min(data.alphamapHeight - 1, UnityEngine.Mathf.CeilToInt((maxZ + pad - tOrg.z) / aZ));
        int aw = ax1 - ax0 + 1, ah = az1 - az0 + 1, layersN = data.alphamapLayers; var alpha = data.GetAlphamaps(ax0, az0, aw, ah);
        float SegD(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = ab.sqrMagnitude > 0f ? UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude) : 0f; return (p - (a + ab * t)).magnitude; }
        for (int z = 0; z < ah; z++) for (int x = 0; x < aw; x++)
        {
            var p = new UnityEngine.Vector2(tOrg.x + (ax0 + x + 0.5f) * aX, tOrg.z + (az0 + z + 0.5f) * aZ); float d = float.MaxValue;
            foreach (var r in wornRoutes) for (int i = 1; i < r.Length; i++) d = UnityEngine.Mathf.Min(d, SegD(p, r[i - 1], r[i]));
            float w = wornShare * (1f - UnityEngine.Mathf.Clamp01((d - wornHalf) / wornBlend)); float t0 = alpha[z, x, trailLayer]; if (w <= t0) continue;
            float rest = 1f - t0, keep = rest > 1e-4f ? (1f - w) / rest : 0f;
            for (int k = 0; k < layersN; k++) alpha[z, x, k] = k == trailLayer ? w : alpha[z, x, k] * keep; wornCells++;
        }
        data.SetAlphamaps(ax0, az0, alpha); UnityEditor.EditorUtility.SetDirty(data);
    }
}

// ---- 6. the rune post to the shelf's uphill side, a rim rock between it and the deck
const float runeUphill = 2f, runeScreenAt = 2.2f, runeScreenOver = 1.5f, runeScreenWide = 3.2f;   // the pack rock tapers: 1.8 m wide and 1 m over left the post showing at its shoulders from 44 of 128 deck eyes
var ward = kit.Root("Ward"); var post = ward != null ? ward.transform.Find("Climb/RunePost") : null; float postTop = 0f; UnityEngine.GameObject screen = null;
if (post == null) notes.Add("no Ward/Climb/RunePost");
else
{
    var climb = post.parent; PlaceKit.Remove(climb.Find("RuneScreen"));
    var pb = PlaceKit.MeshBounds(post.gameObject); float sink = post.position.y - pb.min.y;
    // the uphill side: the higher of the two shelf directions across the drop (the drop runs down to the east here)
    var anchor = climb.Find("RunePostAnchor"); if (anchor == null) { anchor = new UnityEngine.GameObject("RunePostAnchor").transform; anchor.SetParent(climb, false); anchor.position = post.position; }   // first run's spot, so a rerun never walks the post further
    var a = anchor.position; float hW = kit.H(a.x - runeUphill, a.z), hE = kit.H(a.x + runeUphill, a.z); float dir = hW >= hE ? -1f : 1f;
    float nx = a.x + dir * runeUphill, nz = a.z; float ng = kit.H(nx, nz);
    post.position = V(nx, ng + (a.y - kit.H(a.x, a.z)), nz);
    pb = PlaceKit.MeshBounds(post.gameObject); post.position += V(0f, ng - pb.min.y - 0.05f, 0f); pb = PlaceKit.MeshBounds(post.gameObject); postTop = pb.max.y;
    var tower = camp.transform.Find("Tower"); var deck = tower.position + V(0f, tower.Find("Cab").position.y - tower.position.y + 1.6f, 0f);
    var toDeck = V(deck.x - nx, 0f, deck.z - nz).normalized; var sp = V(nx, 0f, nz) + toDeck * runeScreenAt; float sg = UnityEngine.Mathf.Min(kit.H(sp.x - 0.5f, sp.z), kit.H(sp.x + 0.5f, sp.z), kit.H(sp.x, sp.z));
    screen = kit.Spawn(PlaceKit.BK + "Rocks/BigBoulders_2", climb);
    if (screen != null)
    {
        screen.name = "RuneScreen"; PlaceKit.StripColliders(screen);
        // LOD 0 only: the pack rock's LODGroup culls it at the deck's 125 m, and the post showed past it (pixel check, 2026-10-02)
        var lodG = screen.GetComponentInChildren<UnityEngine.LODGroup>(); if (lodG != null) { var lods = lodG.GetLODs(); var keep = new System.Collections.Generic.HashSet<UnityEngine.Renderer>(lods.Length > 0 ? lods[0].renderers : new UnityEngine.Renderer[0]); foreach (var r in screen.GetComponentsInChildren<UnityEngine.Renderer>()) if (!keep.Contains(r)) r.enabled = false; UnityEngine.Object.DestroyImmediate(lodG); } screen.transform.rotation = UnityEngine.Quaternion.LookRotation(toDeck); screen.transform.localScale = UnityEngine.Vector3.one;
        var b = PlaceKit.MeshBounds(screen); float wantH = postTop + runeScreenOver - sg + 0.3f;   // 0.3 m buried
        float wide = UnityEngine.Mathf.Max(b.size.x, b.size.z);
        screen.transform.localScale = V(runeScreenWide / wide, wantH / b.size.y, runeScreenWide / wide);
        b = PlaceKit.MeshBounds(screen); screen.transform.position += V(sp.x - b.center.x, sg - 0.3f - b.min.y, sp.z - b.center.z);
        foreach (var mf in screen.GetComponentsInChildren<UnityEngine.MeshFilter>()) { if (mf.sharedMesh == null) continue; var mr = mf.GetComponent<UnityEngine.MeshRenderer>(); if (mr == null || !mr.enabled) continue; var mc = mf.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; mc.convex = true; }
        UnityEngine.Physics.SyncTransforms();
        var treads = WalkIns.Treads(); foreach (var mr in screen.GetComponentsInChildren<UnityEngine.MeshRenderer>()) if (WalkIns.MeshInTread(mr, treads)) notes.Add("RuneScreen enters a trail tread (" + WalkIns.PathOf(mr.transform) + ")");
    }
}

// ---- 7. the deck's hard sight lines (Valley.md 7.6: the verge tree, the office west door, the cat step, the Snag line; trees move if they
// block): forest pieces standing on the lines from the deck's centre and four corner eyes to each are removed (drawn trees count, by
// temporary colliders on their first LOD, removed after)
var hardTargets = new (string n, UnityEngine.Vector3 p)[] { ("verge tree", V(419f, 25f, 139f)), ("office west door", V(341f, 4.2f, 199f)), ("cat step", V(240f, -3.3f, 56f)), ("Snag line", V(96f, 54f, 146.5f)) };
int deckCleared = 0; var deckList = new System.Collections.Generic.List<string>();
{
    var tw = camp.transform.Find("Tower"); float eyeY = tw.Find("Cab").position.y + 1.6f; const float corner = 3.5f;
    var deckEyes = new[] { V(tw.position.x, eyeY, tw.position.z), V(tw.position.x - corner, eyeY, tw.position.z - corner), V(tw.position.x + corner, eyeY, tw.position.z - corner), V(tw.position.x - corner, eyeY, tw.position.z + corner), V(tw.position.x + corner, eyeY, tw.position.z + corner) };
    var temps = new System.Collections.Generic.List<UnityEngine.Collider>();
    if (forest != null)
        foreach (var lod in forest.GetComponentsInChildren<UnityEngine.LODGroup>())
        {
            var lods = lod.GetLODs(); if (lods.Length == 0) continue;
            foreach (var r in lods[0].renderers) { var mf = r != null ? r.GetComponent<UnityEngine.MeshFilter>() : null; if (mf == null || mf.sharedMesh == null || r.GetComponent<UnityEngine.Collider>() != null) continue; var mc = r.gameObject.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = mf.sharedMesh; temps.Add(mc); }
        }
    UnityEngine.Physics.SyncTransforms();
    var gone = new System.Collections.Generic.HashSet<UnityEngine.GameObject>();
    foreach (var (n, p) in hardTargets) foreach (var e in deckEyes)
    {
        var d = p - e; foreach (var h in UnityEngine.Physics.RaycastAll(e, d.normalized, d.magnitude - 1.5f, ~0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            if (forest == null || !h.collider.transform.IsChildOf(forest.transform)) continue;
            var root = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(h.collider.gameObject); var go = root != null ? root : h.collider.gameObject;
            if (gone.Add(go)) deckList.Add(n + ": " + WalkIns.PathOf(go.transform));
        }
    }
    foreach (var t in temps) if (t != null) UnityEngine.Object.DestroyImmediate(t);
    foreach (var go in gone) if (go != null) { UnityEngine.Object.DestroyImmediate(go); deckCleared++; }
}

// 8.33 (Wren 2026-10-02): the clearing-edge rocks 8.9d placed (Camp/Dressing/Rocks) had boxes fitted to world bounds, which grow on a
// turned rock (0.4 to 0.8 m out); each is refitted in its own axes from its meshes (PlaceKit.FitExact)
int rocksRefit = 0;
{ var rocksT = kit.Root("Camp") != null ? kit.Root("Camp").transform.Find("Dressing/Rocks") : null; if (rocksT != null) foreach (UnityEngine.Transform r in rocksT) if (PlaceKit.FitExact(r.gameObject) != null) rocksRefit++; }
UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);

return "saved=" + saved + " | rocks refit " + rocksRefit + " | interior: bunk, stove, wood box, counter, shelf, desk, washstand placed | porch railings removed " + rails + " | privy " + (outhouse != null ? "built" : "MISSING") +
    " | rune post at (" + (post != null ? F(post.position.x) + ", " + F(post.position.z) : "-") + "), top " + F(postTop) + ", screen " + (screen != null ? "top " + F(PlaceKit.MeshBounds(screen).max.y) : "MISSING") +
    " | edge firs " + edgeFirN + ", saplings " + edgeSapN + " (" + edgeSkip + " spots skipped) | worn path cells " + wornCells + " | fire pit cold (" + coldOff + " switched off), seats moved out " + seatsMoved + " | forest pieces removed " + removed + " | deck lines cleared of " + deckCleared + (deckList.Count > 0 ? " (" + string.Join(", ", deckList) + ")" : "") + " | " + (listed.Count == 0 ? "no other colliders on the spots" : string.Join("; ", listed)) + " | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | missing: " + (kit.Missing.Count == 0 ? "none" : string.Join(", ", kit.Missing));
