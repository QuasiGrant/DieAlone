// Main3 task 8.24, Camp 1 and the north loop to NorthLayout.md draft 2 (Sable 2026-10-02; NorthLayout_UI.md, NorthLayout_Story.md,
// Marlow's 824 paper check): layout and walkability only, dressing is 11.0. Edit mode, Main3; rerunnable (absolute places; the new pieces
// rebuilt under Campsites/Camp_1/Dressing/Layout824, Places/NorthRuin/Layout824 and Places/NorthLoop; nothing stacks). In the runner
// after main3_8_23_lake.cs, before the late passes (8.18 look, 8.18a pockets and solid).
// REMOVALS (R): each named piece by its name within removeTol m of the position Sable gives (Marlow's read), wherever it stands outside
//   the places; then, as the forest recipes do on a rerun (KeepOuts.North, 8.16 grove feet and 8.19), every fir, sapling, bush, fern,
//   log, stump or plant left inside a keep-out zone under Forest or SliceLook. Giants (Sequoia) and snags stay.
// N1  the kid's table: the customer chair (CS_Chair_3 at 0.8) flush to the table's west end at table local (-1.4, 0), facing his stool,
//     the bear in it (the bear's old stool goes); the lid stool and its lid flush to the south edge at (-0.3, -1.06); pots in a back
//     row, eight ingredients in a front row.
// N4  the tent's box shrinks to its fabric (guy ropes left out, they carry "Rope"); a row of four taped boxes 0.5 m, 0.2 m apart, at
//     tent local x 3.4 to 3.9.
// N5  a bucket and a 20 L can at the cookfire, 0.55 m apart.
// N8  forage C: five shrubs on r 1.0 round (229.5, 273.5) (BurnLayout draft tightens NorthLayout's 1.4) under Places/ForageC (named Bush_ForageC_1 to 5), each with a solid capsule and
//     the "Forage" stand-in usable (the interact ray skips triggers); the
//     stand marker facing 0.
// N9, N10, N12  the search spots SS1 to SS3 under Places/NorthLoop: SS1 a low boulder at the fir foot, SS2 an owned stump at scale 0.3
//     with its box fitted, SS3 a leaf bed behind the giant.
// N11 the fallen giant under Places/NorthLoop/FallenGiant: a Sequoia laid along (145.8, 271.1) to (131.5, 257.3) as in 8.20, its
//     capsule giantColR round and sunk giantSink, tilted to the ground; the root plate at the NE end, a 4.0 m disc 0.8 thick facing NE,
//     sunk 0.5, with its box.
// N13 the ruin (Places/NorthRuin, ruin local metres): every wall, half wall, jamb and lintel box 0.30 thick on its log line (8.17 took
//     the module's depth, 2.3 m); a floor slab over x -2.85 to 2.85, z -1.85 to 1.85, its top at the ruin's foot (local 0); the stump goes and
//     the report box stands on a 1.0 m post at (-1.3, 2.4); the roof slab 4.6 x 0.12 x 1.8 with a box, from the back half-wall tops
//     (z -1.85, y 1.5) to the floor at z -0.9 (58 degrees, slides), the ridge beam lying on it; the bunk frame (x -2.3 to -0.4, z -1.85
//     to -1.05, top 0.45), the cache trunk (x -2.85 to -2.3, z -1.85 to -0.85, box fitted to its mesh), the table (x -2.85 to -2.25,
//     z 0.5 to 1.3, top 0.78) under the window, the rocking chair on its side at (-1.5, 0.3), a hook inside the door; the stovepipe
//     runs to 4.5 m over the floor (N14's approach target).
// N15 the sky gap: no fir or pine under skyTop m tall within skyR m of the ruin (giants stay); the K zone keeps it.
// WARPS (doc 5.1): Camp_1 (268, 226) facing 49, North_Loop_Ruin (165.8, 267.7) facing 25.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
var kit = new PlaceKit(scene);
UnityEngine.Vector3 V(float x, float y, float z) => new UnityEngine.Vector3(x, y, z);
UnityEngine.Vector2 P(float x, float z) => new UnityEngine.Vector2(x, z);
UnityEngine.Color Hex(string h) { UnityEngine.ColorUtility.TryParseHtmlString(h, out var c); return c; }
var inv = System.Globalization.CultureInfo.InvariantCulture; string F(float v) => v.ToString("F2", inv);
var notes = new System.Collections.Generic.List<string>();
var c1 = kit.Root("Campsites") != null ? kit.Root("Campsites").transform.Find("Camp_1") : null; var cd = c1 != null ? c1.Find("Dressing") : null;
var places = kit.Root("Places"); var ruin = places != null ? places.transform.Find("NorthRuin") : null; var forest = kit.Root("Forest");
if (cd == null || ruin == null || forest == null) return "run 8.16, 8.17 camp1 and 8.17 ruin first (Campsites/Camp_1/Dressing, Places/NorthRuin, Forest)";
UnityEngine.Physics.SyncTransforms();
// a pack log from a to b (world), girth g, no collider (as 8.23)
UnityEngine.GameObject Log(UnityEngine.Transform parent, UnityEngine.Vector3 a, UnityEngine.Vector3 b, float g)
{
    var lg = kit.Spawn(PlaceKit.CS + "Wood/CS_Log_Large_Long", parent); if (lg == null) return null; PlaceKit.StripColliders(lg);
    var dir = b - a; lg.transform.rotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.right, dir.normalized);
    lg.transform.localScale = V(dir.magnitude / 2f, g / 0.37f, g / 0.37f); lg.transform.position = (a + b) * 0.5f; lg.transform.position += (a + b) * 0.5f - PlaceKit.MeshBounds(lg).center; return lg;
}
// a box collider on g fitted to its meshes in g's own axes, from each mesh's own bounds (PlaceKit.FitCollider measures world boxes and
// grows them when g is turned: the tent's 7.32 m box, the trunk's 1.58 m one); renderers whose name holds skipName are left out
UnityEngine.BoxCollider ExactBox(UnityEngine.GameObject g, string skipName)
{
    foreach (var c in g.GetComponents<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    bool any = false; var b = new UnityEngine.Bounds();
    foreach (var mf in g.GetComponentsInChildren<UnityEngine.MeshFilter>())
    {
        if (mf.sharedMesh == null || (skipName != null && mf.name.Contains(skipName))) continue; var mb = mf.sharedMesh.bounds;
        for (int i = 0; i < 8; i++)
        {
            var w = mf.transform.TransformPoint(V((i & 1) == 0 ? mb.min.x : mb.max.x, (i & 2) == 0 ? mb.min.y : mb.max.y, (i & 4) == 0 ? mb.min.z : mb.max.z)); var l = g.transform.InverseTransformPoint(w);
            if (!any) { b = new UnityEngine.Bounds(l, UnityEngine.Vector3.zero); any = true; } else b.Encapsulate(l);
        }
    }
    if (!any) return null; var bc = g.AddComponent<UnityEngine.BoxCollider>(); bc.center = b.center; bc.size = b.size; return bc;
}
// the ground under a point (terrain)
float G(float x, float z) => kit.H(x, z);

// ================= REMOVALS =================
const float removeTol = 0.3f;
var named = new (string n, float x, float z, string why)[] {
    ("CITW_Tree_Stump", 266.49f, 235.33f, "N4"),
    ("RedPine2", 296.22f, 248.87f, "N7"), ("RedPine3", 299.02f, 250.30f, "N7"), ("RedFir8", 298.85f, 248.12f, "N7"), ("RedPine1", 296.26f, 246.93f, "N7"), ("RedPine4", 293.08f, 249.85f, "N7"),
    ("ThinFern3", 228.6f, 273.0f, "N8"), ("Grass1", 228.2f, 274.7f, "N8"),
    ("RedFir5", 145.15f, 270.24f, "N11"), ("RedFir7", 143.67f, 268.65f, "N11"), ("RedPine2", 141.72f, 266.98f, "N11"), ("RedPine3", 141.76f, 268.77f, "N11"), ("RedFir7", 136.95f, 264.59f, "N11"),
    ("RedPine4", 134.50f, 262.39f, "N11"), ("RedFir5", 131.70f, 257.67f, "N11"), ("RedwoodHollowLog_2", 139.65f, 268.35f, "N11"), ("RedPine3", 137.64f, 266.77f, "N11"),
    ("Bush3", 173.6f, 282.3f, "N13"), ("Bush3", 171.2f, 273.7f, "N13"),
    ("RedPine2", 182.98f, 278.55f, "N14"), ("RedPine3", 180.96f, 278.75f, "N14"), ("RedFir4", 185.40f, 278.08f, "N14") };
string[] keepOutKinds = { "RedFir", "RedPine", "Bush", "CS_Bush", "ThinFern", "Fern", "DeadLeaves", "Branchs", "RedwoodHollowLog", "CITW_Tree_Stump", "Grass", "Nettle", "Mushroom" };
string[] removeRoots = { "Forest", "SliceLook", "Ground815" };
bool Kind(string n, string[] kinds) { foreach (var k in kinds) if (n.StartsWith(k)) return true; return false; }
// the piece a removal takes: the outermost prefab instance (or the named object itself) under a remove root, never under Ground815/Stops
var pieces = new System.Collections.Generic.List<UnityEngine.Transform>();
foreach (var rn in removeRoots) { var r = kit.Root(rn); if (r == null) continue; foreach (var t in r.GetComponentsInChildren<UnityEngine.Transform>(true)) { if (t == r.transform || WalkIns.PathOf(t).StartsWith("Ground815/Stops")) continue; if (UnityEditor.PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject) || !UnityEditor.PrefabUtility.IsPartOfPrefabInstance(t.gameObject)) pieces.Add(t); } }
int namedGone = 0; var namedMissing = new System.Collections.Generic.List<string>(); var gone = new System.Collections.Generic.HashSet<UnityEngine.Transform>();
foreach (var (n, x, z, why) in named)
{
    UnityEngine.Transform hit = null; float best = removeTol;
    foreach (var t in pieces) { if (t == null || gone.Contains(t) || !(t.name == n || t.name.StartsWith(n + " ("))) continue; float d = UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(x, z)); if (d <= best) { best = d; hit = t; } }
    if (hit == null) { namedMissing.Add(why + " " + n + " (" + F(x) + ", " + F(z) + ")"); continue; }
    gone.Add(hit); UnityEngine.Object.DestroyImmediate(hit.gameObject); namedGone++;
}
// N11: the saplings and the bush inside the giant's capsule (Marlow 4); then every listed kind left in a keep-out zone
const float giantColR = 1.5f, giantR = 1.3f, giantSink = 0.3f; var giantA = P(145.8f, 271.1f); var giantB = P(131.5f, 257.3f);
float AxisD(UnityEngine.Vector2 p) { var ab = giantB - giantA; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - giantA, ab) / ab.sqrMagnitude); return UnityEngine.Vector2.Distance(p, giantA + ab * t); }
int zoneGone = 0; var zoneBy = new System.Collections.Generic.Dictionary<string, int>();
foreach (var t in pieces)
{
    if (t == null || gone.Contains(t) || !Kind(t.name, keepOutKinds) || t.name.StartsWith("Sequoia")) continue; var p = P(t.position.x, t.position.z);
    string zone = KeepOuts.Which(p); if (zone == null && AxisD(p) <= giantColR) zone = "N11 giant capsule"; if (zone == null) continue;
    gone.Add(t); UnityEngine.Object.DestroyImmediate(t.gameObject); zoneGone++; zoneBy[zone] = zoneBy.TryGetValue(zone, out var k0) ? k0 + 1 : 1;
}
// N15: no fir or pine under skyTop m tall within skyR m of the ruin, anywhere in the forest (giants stay)
const float skyR = 8f, skyTop = 25f; int skyGone = 0;
foreach (var t in pieces)
{
    if (t == null || gone.Contains(t) || !(t.name.StartsWith("RedFir") || t.name.StartsWith("RedPine"))) continue;
    if (UnityEngine.Vector2.Distance(P(t.position.x, t.position.z), P(ruin.position.x, ruin.position.z)) > skyR) continue;
    float top = PlaceKit.MeshBounds(t.gameObject).max.y - G(t.position.x, t.position.z); if (top >= skyTop) continue;
    gone.Add(t); UnityEngine.Object.DestroyImmediate(t.gameObject); skyGone++;
}
UnityEngine.Physics.SyncTransforms();

// ================= N1: the kid's table =================
var kid = cd.Find("KidTable"); var L1 = kid != null ? kit.Fresh("Layout824", kid, kid.position, kid.eulerAngles.y) : null;
const float chairX = -1.4f, chairScale = 0.8f, lidX = -0.3f, lidZ = -1.06f, potRowZ = 0.2f, foodRowZ = -0.05f, rowPitch = 0.12f;
if (kid == null) notes.Add("no Camp_1/Dressing/KidTable");
else
{
    // the bear's stool (west) goes; the customer chair stands flush to the west end, facing his stool (local +x)
    foreach (var t in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Cast<UnityEngine.Transform>(kid))) if (t.name.StartsWith("CS_Log_Stool_1") && t.localPosition.x < 0f) UnityEngine.Object.DestroyImmediate(t.gameObject);
    var chair = kit.On(PlaceKit.CS + "CS_Chair_3", L1, V(chairX, 0f, 0f), 90f, chairScale, false, null, true); if (chair != null) ExactBox(chair, null);
    float seat = chair != null ? PlaceKit.LocalBounds(chair, L1).min.y + 0.45f * chairScale : 0.36f;
    var bear = kid.Find("Toy_Teddy_Bear"); if (bear != null) { var b = PlaceKit.LocalBounds(bear.gameObject, kid); bear.localPosition += V(chairX - b.center.x, seat - b.min.y, 0f - b.center.z); bear.localRotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); } else notes.Add("no KidTable/Toy_Teddy_Bear");
    // the lid stool and its lid flush to the south edge
    var lidStool = kid.Find("CS_Log_Stool_2"); var lid = kid.Find("Drum_Grey_Lid");
    if (lidStool != null) { var b = PlaceKit.LocalBounds(lidStool.gameObject, kid); var shift = V(lidX - b.center.x, 0f, lidZ - b.center.z); lidStool.localPosition += shift; if (lid != null) { var lb = PlaceKit.LocalBounds(lid.gameObject, kid); lid.localPosition += V(lidX - lb.center.x, 0f, lidZ - lb.center.z); } }
    else notes.Add("no KidTable/CS_Log_Stool_2");
    // eight ingredients in the front row: 8.17's five, three more
    var table = kid.Find("CS_Table_Small_Modern_1"); float tTop = table != null ? PlaceKit.LocalBounds(table.gameObject, kid).max.y : 0.78f;
    string[] more = { "Food/CS_Food_Bread_2_Slice", "Food/CS_Food_Can_1", "Food/CS_Drink_Coke" };
    for (int i = 0; i < more.Length; i++) kit.On(PlaceKit.CS + more[i], L1, V(-0.45f + i * rowPitch, tTop, foodRowZ - 0.12f), 0f, 1f, false, null, true);
    kit.On(PlaceKit.CE + "Decoration_Kitchen/Apple_Bitten", L1, V(-0.45f + more.Length * rowPitch, tTop, foodRowZ - 0.12f), 0f, 1f, false, null, true);
}

// ================= N4: the tent's box, the box row; N5: water at the fire =================
const float tentYaw = 200f, boxSize = 0.5f, boxGap = 0.2f, rowX = 3.65f; const int boxN = 4;
var tent = cd.Find("CS_Tent_Large_Modern_Preset_1"); string tentBox = "none";
if (tent == null) notes.Add("no Camp_1/Dressing/CS_Tent_Large_Modern_Preset_1");
else
{
    var bc = ExactBox(tent.gameObject, "Rope"); if (bc != null) tentBox = F(bc.size.x) + " x " + F(bc.size.y) + " x " + F(bc.size.z);
    var row = kit.Fresh("Layout824_Boxes", cd, tent.position, tentYaw);
    for (int i = 0; i < boxN; i++) { float z = (i - (boxN - 1) * 0.5f) * (boxSize + boxGap); var bx = kit.Fill(PlaceKit.CE + "Decoration_Out/Cardboard_Box_Closed", row, V(rowX, G(row.TransformPoint(V(rowX, 0f, z)).x, row.TransformPoint(V(rowX, 0f, z)).z) - row.position.y, z), V(boxSize, boxSize, boxSize), i * 7f); if (bx != null) { bx.name = "TapedBox_" + (i + 1); ExactBox(bx, null); } }
}
var water = kit.Fresh("Layout824_Water", cd, V(277.3f, G(277.3f, 230.8f), 230.8f), 0f);
kit.Ground(PlaceKit.CI + "Props/CITW_Bucket", water, 276.9f, 231.0f, 20f);
kit.Ground(PlaceKit.FT + "Canister", water, 277.7f, 230.6f, 110f, 1f, false);

// ================= N8, N9, N10, N12, N11: forage C, the search spots, the fallen giant =================
var olive = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Slice/Slice_Olive_Foliage512.mat"); if (olive == null) notes.Add("no Slice_Olive_Foliage512.mat");
const string SUF = "suffercord/PSX Autumn Forest Asset Pack/Models/";
const float forageX = 229.5f, forageZ = 273.5f, forageR = 1.0f, shrubLow = 0.6f, shrubHigh = 1.0f, shrubR = 0.3f; const int shrubs = 5;
{
    var fc = kit.Fresh("ForageC", places.transform, V(forageX, G(forageX, forageZ), forageZ), 0f); var rng = new System.Random(8240);
    for (int i = 0; i < shrubs; i++)
    {
        float a = (180f + i * 360f / shrubs) * UnityEngine.Mathf.Deg2Rad, x = forageX + UnityEngine.Mathf.Sin(a) * forageR, z = forageZ + UnityEngine.Mathf.Cos(a) * forageR, h = shrubLow + (float)rng.NextDouble() * (shrubHigh - shrubLow);
        var s = kit.Spawn(SUF + "Bush" + (1 + i % 4), fc); if (s == null) continue; PlaceKit.StripColliders(s); s.name = "Bush_ForageC_" + (i + 1);
        s.transform.rotation = UnityEngine.Quaternion.Euler(0f, (float)rng.NextDouble() * 360f, 0f); var b = PlaceKit.MeshBounds(s); s.transform.localScale = UnityEngine.Vector3.one * (h / UnityEngine.Mathf.Max(0.1f, b.size.y));
        b = PlaceKit.MeshBounds(s); s.transform.position += V(x - b.center.x, G(x, z) - b.min.y - 0.03f, z - b.center.z);
        if (olive != null) foreach (var r in s.GetComponentsInChildren<UnityEngine.Renderer>()) { var ms = r.sharedMaterials; for (int k = 0; k < ms.Length; k++) ms[k] = olive; r.sharedMaterials = ms; }
        // 8.24 (Pim, Wren 2026-10-02): the interactor's ray skips triggers (PlayerInteractor: QueryTriggerInteraction.Ignore) and meets
        // colliders only, so each shrub gets a solid capsule shrubR round, its own height, and the stand-in usable (8.5's
        // ToggleColorInteractable) with the prompt "Forage" until the forage system exists; on r 1.0 (BurnLayout draft, Sable: the ring
        // tightened from 1.4) the capsules stand 1.18 m apart, 0.58 m gaps, under the 0.6 to 1.0 band
        b = PlaceKit.MeshBounds(s); float sk = s.transform.lossyScale.x; var col = s.AddComponent<UnityEngine.CapsuleCollider>(); col.radius = shrubR / sk; col.height = UnityEngine.Mathf.Max(b.size.y, 2f * shrubR) / sk; col.center = s.transform.InverseTransformPoint(b.center);
        var use = s.AddComponent<ToggleColorInteractable>(); var so = new UnityEditor.SerializedObject(use); so.FindProperty("prompt").stringValue = "Forage"; so.FindProperty("target").objectReferenceValue = s.GetComponentInChildren<UnityEngine.Renderer>(); so.ApplyModifiedPropertiesWithoutUndo();
    }
    kit.Marker("ForageC_Stand", fc, fc.InverseTransformPoint(V(229.2f, G(229.2f, 270.6f), 270.6f)), 0f);
    kit.ClearDetail(V(forageX, 0f, forageZ), forageR + 0.8f);
}
const float ssTall = 0.5f;
var loop = kit.Fresh("NorthLoop", places.transform, V(150f, 0f, 270f), 0f);
{
    // SS1 at the fir foot: a pile of branches; SS2 an owned stump at scale 0.3, its box fitted; SS3 a leaf bed behind the giant
    // SS1 a low boulder ssTall m tall a kid could sit on (a branch pile 0.1 m tall subtended under 1 degree from the trail, Pim's found bar)
    var s1 = kit.Group("SS1", loop, V(251.6f, G(251.6f, 272.0f), 272.0f), 0f); var rk = kit.Fill(PlaceKit.BK + "Rocks/Boulder_0", s1, V(0f, -0.05f, 0f), V(0.7f, ssTall, 0.6f), 30f); if (rk != null) ExactBox(rk, null);
    var s2 = kit.Group("SS2", loop, V(150.5f, G(150.5f, 276.5f), 276.5f), 0f); var st = kit.Ground(PlaceKit.CI + "Vegetation/CITW_Tree_Stump", s2, 150.5f, 276.5f, 40f, 0.3f); if (st != null) ExactBox(st, null);
    var s3 = kit.Group("SS3", loop, V(132.3f, G(132.3f, 262.3f), 262.3f), 0f); kit.Ground(PlaceKit.BK + "Plants/DeadLeaves1", s3, 132.3f, 262.3f, 0f, 1.2f);
}
const float plateD = 4.0f, plateT = 0.8f, plateSink = 0.5f, trunkBand = 0.03f;
{
    var giant = kit.Group("FallenGiant", loop, V((giantA.x + giantB.x) * 0.5f, 0f, (giantA.y + giantB.y) * 0.5f), 0f);
    var ea = V(giantA.x, G(giantA.x, giantA.y) + giantColR - giantSink, giantA.y); var eb = V(giantB.x, G(giantB.x, giantB.y) + giantColR - giantSink, giantB.y);   // the NE (butt) and SW (top) ends
    var axis = (eb - ea).normalized; float len = UnityEngine.Vector3.Distance(ea, eb);
    var colGo = new UnityEngine.GameObject("Collider"); colGo.transform.SetParent(giant, false); colGo.transform.SetPositionAndRotation((ea + eb) * 0.5f, UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.right, axis));
    var cap = colGo.AddComponent<UnityEngine.CapsuleCollider>(); cap.direction = 0; cap.radius = giantColR; cap.height = len + 2f * giantColR;
    var tree = kit.Spawn(PlaceKit.BK + "Trees/Sequoia2", giant);
    if (tree != null)
    {
        PlaceKit.StripColliders(tree); tree.transform.rotation = UnityEngine.Quaternion.identity; tree.transform.localScale = UnityEngine.Vector3.one; var b = PlaceKit.MeshBounds(tree);
        float baseW = 0f; { var lod = tree.GetComponentInChildren<UnityEngine.LODGroup>(); var r0 = lod != null ? lod.GetLODs()[0].renderers[0] : tree.GetComponentInChildren<UnityEngine.Renderer>(); var mf0 = r0 != null ? r0.GetComponent<UnityEngine.MeshFilter>() : null;
            if (mf0 != null) { float x0 = float.MaxValue, x1 = float.MinValue; foreach (var v in mf0.sharedMesh.vertices) { var w = mf0.transform.TransformPoint(v); if (w.y > b.min.y + b.size.y * trunkBand) continue; x0 = UnityEngine.Mathf.Min(x0, w.x); x1 = UnityEngine.Mathf.Max(x1, w.x); } baseW = x1 - x0; } }
        float girth = 2f * giantR / UnityEngine.Mathf.Max(0.3f, baseW);
        tree.transform.localScale = V(girth, len / b.size.y, girth);
        tree.transform.rotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.up, axis);   // its up axis toward the SW end: the butt at the NE end
        tree.transform.position = ea;   // the pack trees stand on their pivot at the trunk's base centre
    }
    // the root plate: a stump turned to face NE (its height along the axis outward), 4.0 across, 0.8 thick, sunk plateSink
    var outward = -axis; outward.y = 0f; outward.Normalize();
    var pc = V(giantA.x, 0f, giantA.y) + outward * plateT * 0.5f; pc.y = G(pc.x, pc.z) + plateD * 0.5f - plateSink;
    var plate = kit.Group("RootPlate", giant, pc, 0f); plate.rotation = UnityEngine.Quaternion.LookRotation(outward, UnityEngine.Vector3.up);
    var stump = kit.Spawn(PlaceKit.CI + "Vegetation/CITW_Tree_Stump", plate);
    if (stump != null)
    {
        PlaceKit.StripColliders(stump); stump.transform.localRotation = UnityEngine.Quaternion.Euler(90f, 0f, 0f); stump.transform.localScale = UnityEngine.Vector3.one;
        var lb = PlaceKit.LocalBounds(stump, plate); stump.transform.localScale = V(plateD / UnityEngine.Mathf.Max(0.1f, lb.size.x), plateT / UnityEngine.Mathf.Max(0.1f, lb.size.z), plateD / UnityEngine.Mathf.Max(0.1f, lb.size.y));
        lb = PlaceKit.LocalBounds(stump, plate); stump.transform.localPosition -= lb.center;
    }
    kit.Blocker("PlateBox", plate, UnityEngine.Vector3.zero, V(plateD, plateD, plateT));
}

// ================= N13: the ruin =================
const float wallT = 0.30f, floorX = 2.85f, floorZ = 1.85f, floorT = 0.10f, postX = -1.3f, postZ = 2.4f, postH = 1.0f, postW = 0.16f;
const float slabW = 4.6f, slabT = 0.12f, slabX = -0.85f, slabHighZ = -1.85f, slabHighY = 1.5f, slabLowZ = -0.9f, beamZ = -1.7f, beamLen = 4.4f;
const float bunkX0 = -2.3f, bunkX1 = -0.4f, bunkZ0 = -1.85f, bunkZ1 = -1.05f, bunkTop = 0.45f, trunkX0 = -2.85f, trunkX1 = -2.3f, trunkZ0 = -1.85f, trunkZ1 = -0.85f;
const float tableX0 = -2.85f, tableX1 = -2.25f, tableZ0 = 0.5f, tableZ1 = 1.3f, tableTop = 0.78f, chairLX = -1.5f, chairLZ = 0.3f, pipeTop = 4.5f;
int wallBoxes = 0; string slabDeg = "";
{
    // every wall, half wall, jamb and lintel box on its log line, wallT thick (8.17 took the module depth, 2.3 m)
    foreach (UnityEngine.Transform h in ruin) if (h.name.StartsWith("Collider_")) foreach (var bc in h.GetComponentsInChildren<UnityEngine.BoxCollider>()) { var s = bc.size; s.z = wallT; bc.size = s; bc.center = V(bc.center.x, bc.center.y, 0f); wallBoxes++; }
    // 8.17's pieces the doc moves or replaces
    foreach (var n in new[] { "CITW_Tree_Stump", "CITW_Crate", "RoofSlab", "CITW_Trunk_2" }) PlaceKit.Remove(ruin.Find(n));
    var R = kit.Fresh("Layout824", ruin, ruin.position, ruin.eulerAngles.y);
    // the floor: a slab over the room; 8.17's floor boards lie 0.05 over it (looks only)
    float boards = 0f; foreach (UnityEngine.Transform t in ruin) if (t.name.StartsWith("CITW_Floor")) boards = UnityEngine.Mathf.Max(boards, PlaceKit.LocalBounds(t.gameObject, ruin).max.y);
    kit.Blocker("FloorSlab", R, V(0f, -floorT * 0.5f, 0f), V(2f * floorX, floorT, 2f * floorZ));   // its top at the ruin's foot (doc: local 0), so the doorway step is the ground's fall only (0.11 with its top on the boards)
    // the report box on its post
    Log(R, R.TransformPoint(V(postX, boards - 0.05f, postZ)), R.TransformPoint(V(postX, boards + postH, postZ)), postW);
    var post = kit.Blocker("ReportPost", R, V(postX, boards + postH * 0.5f, postZ), V(postW, postH, postW));
    var crate = kit.Fill(PlaceKit.CI + "Props/CITW_Crate", post.transform, V(0f, postH * 0.5f, 0f), V(0.3f, 0.3f, 0.4f)); if (crate != null) { crate.name = "ReportBox"; ExactBox(crate, null); }
    // the roof slab, high edge on the back half-wall tops, low edge on the floor; the ridge beam lying on it
    var low = V(slabX, boards, slabLowZ); var high = V(slabX, slabHighY, slabHighZ); var along = high - low; slabDeg = F(UnityEngine.Mathf.Atan2(along.y, -along.z) * UnityEngine.Mathf.Rad2Deg);
    var pale = kit.Tinted("Places_RuinRoof", "Assets/Materials/Planks023A_1.0x1.0.mat", Hex("#B4AC9C"), new UnityEngine.Vector2(3f, 2f));
    var slab = kit.Slab("RoofSlab", R, (low + high) * 0.5f, V(slabW, slabT, along.magnitude), pale, UnityEngine.Quaternion.LookRotation(along.normalized, UnityEngine.Vector3.up).eulerAngles, true);
    var beam = ruin.Find("CS_Log_Large_Long");
    if (beam != null)
    {
        float t = (beamZ - slabLowZ) / (slabHighZ - slabLowZ); float y = boards + (slabHighY - boards) * t + slabT * 0.5f + 0.16f;
        // it stays 8.17's piece under the ruin (Layout824 is rebuilt each run); the ruin's axes are Layout824's
        beam.localRotation = UnityEngine.Quaternion.identity; beam.localScale = V(beamLen / 2f, 0.8f, 0.8f); beam.localPosition = UnityEngine.Vector3.zero;
        var bb = PlaceKit.LocalBounds(beam.gameObject, ruin); beam.localPosition = V(slabX - bb.center.x, y - bb.center.y, beamZ - bb.center.z);
    }
    else notes.Add("no ruin ridge beam");
    // the bunk frame, the cache trunk, the table under the window, a hook inside the door
    var bunk = kit.Fill(PlaceKit.CE + "Furniture/Bed_Frame", R, V((bunkX0 + bunkX1) * 0.5f, boards, (bunkZ0 + bunkZ1) * 0.5f), V(bunkX1 - bunkX0, bunkTop, bunkZ1 - bunkZ0)); if (bunk != null) bunk.name = "BunkFrame";
    kit.Blocker("BunkBox", R, V((bunkX0 + bunkX1) * 0.5f, boards + bunkTop * 0.5f, (bunkZ0 + bunkZ1) * 0.5f), V(bunkX1 - bunkX0, bunkTop, bunkZ1 - bunkZ0));
    var trunk = kit.On(PlaceKit.CI + "Props/CITW_Trunk_2", R, V((trunkX0 + trunkX1) * 0.5f, boards, (trunkZ0 + trunkZ1) * 0.5f), 0f, 1f, false, null, true);
    if (trunk != null) { var tb = PlaceKit.LocalBounds(trunk, R); if (tb.size.x > tb.size.z) { trunk.transform.localRotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f); tb = PlaceKit.LocalBounds(trunk, R); trunk.transform.localPosition += V((trunkX0 + trunkX1) * 0.5f - tb.center.x, 0f, (trunkZ0 + trunkZ1) * 0.5f - tb.center.z); } trunk.name = "CacheTrunk"; ExactBox(trunk, null); }
    var table = kit.Fill(PlaceKit.CI + "Furniture/CITW_Table", R, V((tableX0 + tableX1) * 0.5f, boards, (tableZ0 + tableZ1) * 0.5f), V(tableX1 - tableX0, tableTop, tableZ1 - tableZ0)); if (table != null) table.name = "Table";
    kit.Blocker("TableBox", R, V((tableX0 + tableX1) * 0.5f, boards + tableTop * 0.5f, (tableZ0 + tableZ1) * 0.5f), V(tableX1 - tableX0, tableTop, tableZ1 - tableZ0));
    kit.On(PlaceKit.FT + "Hook1", R, V(-0.85f, boards + 1.7f, 2f - wallT * 0.5f - 0.03f), 180f, 1f, false, null, true);
    // the rocking chair on its side, moved off the table's ground (it keeps 8.17's tilt; 8.18a hulls it)
    var rock = ruin.Find("CITW_Rocking_Chair"); if (rock != null) { var rb = PlaceKit.LocalBounds(rock.gameObject, ruin); rock.localPosition += V(chairLX - rb.center.x, 0f, chairLZ - rb.center.z); } else notes.Add("no ruin CITW_Rocking_Chair");
    // the stovepipe up to pipeTop over the floor (N14's approach target); its foot stays on the stove
    var pipe = ruin.Find("Potbelly_Stove_Pipe_Long");
    if (pipe != null) for (int i = 0; i < 3; i++) { var pb = PlaceKit.LocalBounds(pipe.gameObject, ruin); if (pb.size.y < 0.1f) break; float k = (boards + pipeTop - pb.min.y) / pb.size.y; pipe.localScale = V(pipe.localScale.x, pipe.localScale.y * k, pipe.localScale.z); var nb = PlaceKit.LocalBounds(pipe.gameObject, ruin); pipe.localPosition += V(0f, pb.min.y - nb.min.y, 0f); }
    else notes.Add("no ruin stovepipe");
    kit.ClearDetail(ruin.position, floorX + 1f);
}

// ================= WARPS (doc 5.1) =================
var warps = kit.Root("DevWarps").transform;
foreach (var (n, x, z, yaw) in new[] { ("Camp_1", 268f, 226f, 49f), ("North_Loop_Ruin", 165.8f, 267.7f, 25f) })
{ var w = warps.Find(n); if (w == null) { notes.Add("no warp " + n); continue; } w.position = V(x, G(x, z) + 0.2f, z); w.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f); }

UnityEngine.Physics.SyncTransforms();
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene); UnityEditor.AssetDatabase.SaveAssets();
var zones = new System.Collections.Generic.List<string>(); foreach (var kv in zoneBy) zones.Add(kv.Key + " " + kv.Value);
return "saved=" + saved + " | removed by name " + namedGone + " of " + named.Length + (namedMissing.Count > 0 ? " (not found, likely gone already: " + string.Join(", ", namedMissing) + ")" : "") + ", in keep-out zones " + zoneGone + (zones.Count > 0 ? " (" + string.Join(", ", zones) + ")" : "") + ", sky gap " + skyGone
    + " | tent box " + tentBox + " | ruin wall boxes " + wallBoxes + ", roof slab " + slabDeg + " degrees | notes: " + (notes.Count == 0 ? "none" : string.Join("; ", notes)) + " | " + kit.Report();
