if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first: " + scene.path;
if (UnityEngine.GameObject.Find("Camp") != null) return "Camp already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
string cabinRoot = "Assets/Revolving Pizza Games/Cabin In The Woods/Prefabs/";
string campRoot = "Assets/Revolving Pizza Games/Campsite/Prefabs/";

UnityEngine.GameObject Prefab(string path) { var p = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path); if (p == null) throw new System.Exception("missing " + path); return p; }
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return Prefab(p); }
    throw new System.Exception("no prefab named " + name);
}
// Place a pack prefab and fit a box collider to it unless told not to.
UnityEngine.GameObject Place(string name, UnityEngine.Transform parent, UnityEngine.Vector3 localPos, float yaw, bool collider = true, string rename = null)
{
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(name), scene);
    go.transform.SetParent(parent, false);
    go.transform.localPosition = localPos;
    go.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    if (rename != null) go.name = rename;
    if (collider && go.GetComponentInChildren<UnityEngine.Collider>() == null)
    {
        var rends = go.GetComponentsInChildren<UnityEngine.Renderer>();
        if (rends.Length > 0)
        {
            // Bounds in the prefab's own space: rotate back by yaw.
            var b = new UnityEngine.Bounds(); bool first = true;
            foreach (var r in rends) { if (first) { b = r.bounds; first = false; } else b.Encapsulate(r.bounds); }
            var box = go.AddComponent<UnityEngine.BoxCollider>();
            box.center = go.transform.InverseTransformPoint(b.center);
            var inv = UnityEngine.Quaternion.Inverse(go.transform.rotation) * b.size; // approximate for yaw multiples of 90
            box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(inv.x), UnityEngine.Mathf.Abs(inv.y), UnityEngine.Mathf.Abs(inv.z));
        }
    }
    return go;
}
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Material mat, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name; go.transform.SetParent(parent, false); go.transform.localPosition = pos; go.transform.localScale = size;
    if (euler.HasValue) go.transform.localRotation = UnityEngine.Quaternion.Euler(euler.Value);
    if (mat != null) go.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat;
    return go;
}
UnityEngine.GameObject MakeDoor(UnityEngine.Transform parent, UnityEngine.Vector3 hingeLocal, float yaw, string prompt)
{
    var hinge = new UnityEngine.GameObject("Door"); hinge.transform.SetParent(parent, false);
    hinge.transform.localPosition = hingeLocal; hinge.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    var rb = hinge.AddComponent<UnityEngine.Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
    var panel = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find("CITW_Door_1"), scene);
    panel.name = "Panel"; panel.transform.SetParent(hinge.transform, false); panel.transform.localPosition = UnityEngine.Vector3.zero;
    var col = panel.AddComponent<UnityEngine.BoxCollider>(); col.center = V(0.5f, 1.05f, 0f); col.size = V(1.0f, 2.1f, 0.23f);
    var door = hinge.AddComponent<Door>();
    var so = new UnityEditor.SerializedObject(door);
    so.FindProperty("prompt").stringValue = prompt; so.FindProperty("tuning").objectReferenceValue = tuning; so.FindProperty("panel").objectReferenceValue = col;
    so.ApplyModifiedPropertiesWithoutUndo();
    return hinge;
}

var camp = new UnityEngine.GameObject("Camp");
var era = new UnityEngine.GameObject("Era_Modern");
var wallMat = Find("CITW_Log_Wall").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;
var plankMat = Find("CITW_Plank_Wall").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;
var metalMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/PaintedMetal006_1.0x1.0.mat");

// ================= CABIN: 6 x 4 m, three modules by two, door in the south wall =================
float cx = 245f, cz = 190f, cy = H(cx, cz) + 0.1f;
var cabin = new UnityEngine.GameObject("Cabin"); cabin.transform.SetParent(camp.transform, false); cabin.transform.position = V(cx, cy, cz);
var ct = cabin.transform;
for (int i = -1; i <= 1; i++) for (int j = 0; j < 2; j++) Place("CITW_Floor", ct, V(i * 2f, 0f, j == 0 ? -1f : 1f), 0f);
// South wall, z -2: wall, doorway, wall
Place("CITW_Log_Wall", ct, V(-2f, 0f, -2f), 0f);
Place("CITW_Log_Doorway", ct, V(0f, 0f, -2f), 0f, collider: false);
Place("CITW_Log_Wall", ct, V(2f, 0f, -2f), 0f);
// Doorway side colliders so the opening is exactly the door width
Box("DoorJambW", ct, V(-0.75f, 1.5f, -2f), V(0.5f, 3f, 0.3f), null).GetComponent<UnityEngine.Renderer>().enabled = false;
Box("DoorJambE", ct, V(0.75f, 1.5f, -2f), V(0.5f, 3f, 0.3f), null).GetComponent<UnityEngine.Renderer>().enabled = false;
Box("DoorLintel", ct, V(0f, 2.55f, -2f), V(1f, 0.9f, 0.3f), null).GetComponent<UnityEngine.Renderer>().enabled = false;
Place("CITW_Door_Frame", ct, V(0f, 0f, -2f), 0f, collider: false);
MakeDoor(ct, V(-0.5f, 0f, -2f), 0f, "Open");
// North wall, z +2 (rotated 180)
Place("CITW_Log_Window_Wall", ct, V(-2f, 0f, 2f), 180f);
Place("CITW_Log_Wall", ct, V(0f, 0f, 2f), 180f);
Place("CITW_Log_Window_Wall", ct, V(2f, 0f, 2f), 180f);
// East and west walls (2 modules each, rotated)
Place("CITW_Log_Wall", ct, V(3f, 0f, -1f), 90f);
Place("CITW_Log_Window_Wall", ct, V(3f, 0f, 1f), 90f);
Place("CITW_Log_Wall", ct, V(-3f, 0f, -1f), -90f);
Place("CITW_Log_Wall", ct, V(-3f, 0f, 1f), -90f);
// Gable roof from two sloped slabs and two triangle ends, pack wall material
float pitch = UnityEngine.Mathf.Atan2(1.4f, 2.3f) * UnityEngine.Mathf.Rad2Deg;
float slabLen = UnityEngine.Mathf.Sqrt(2.3f * 2.3f + 1.4f * 1.4f);
Box("Roof_S", ct, V(0f, 3.7f, -1.15f), V(6.8f, 0.15f, slabLen), plankMat, V(-pitch, 0f, 0f));
Box("Roof_N", ct, V(0f, 3.7f, 1.15f), V(6.8f, 0.15f, slabLen), plankMat, V(pitch, 0f, 0f));
Box("Gable_E", ct, V(3f, 3.5f, 0f), V(0.3f, 1.0f, 4.0f), wallMat);
Box("Gable_W", ct, V(-3f, 3.5f, 0f), V(0.3f, 1.0f, 4.0f), wallMat);
// Inside: bunk and table, a few props
Place("CITW_Bed", ct, V(-2.0f, 0f, 0.6f), 0f, rename: "Bunk");
Place("CITW_Table", ct, V(1.6f, 0f, 0.9f), 0f, rename: "Table");
Place("CITW_Stool_1", ct, V(1.6f, 0f, -0.2f), 0f);
Place("CITW_Crate", ct, V(2.4f, 0f, -1.3f), 15f);
Place("CITW_Oil_Lamp_1", ct, V(1.2f, 0.9f, 1.1f), 0f, collider: false);
var lampLight = new UnityEngine.GameObject("CabinLight"); lampLight.transform.SetParent(ct, false); lampLight.transform.localPosition = V(0f, 2.4f, 0f);
var ll = lampLight.AddComponent<UnityEngine.Light>(); ll.type = UnityEngine.LightType.Point; ll.color = new UnityEngine.Color(1f, 0.75f, 0.45f); ll.intensity = 2.5f; ll.range = 7f; ll.shadows = UnityEngine.LightShadows.Soft;

// ================= FIRE PIT with seats =================
float fx = 260f, fz = 175f, fy = H(fx, fz);
var fire = Place("CS_Campfire_1", camp.transform, V(fx, fy, fz), 0f, rename: "FirePit");
var flames = Place("FX_Flames_Short", fire.transform, V(0f, 0.1f, 0f), 0f, collider: false);
var embers = Place("FX_Embers", fire.transform, V(0f, 0.3f, 0f), 0f, collider: false);
var smoke = Place("FX_Smoke_Thin", fire.transform, V(0f, 0.6f, 0f), 0f, collider: false);
var fireLightGo = new UnityEngine.GameObject("FireLight"); fireLightGo.transform.SetParent(fire.transform, false); fireLightGo.transform.localPosition = V(0f, 0.9f, 0f);
var fl = fireLightGo.AddComponent<UnityEngine.Light>(); fl.type = UnityEngine.LightType.Point; fl.color = new UnityEngine.Color(1f, 0.55f, 0.2f); fl.intensity = 4f; fl.range = 12f; fl.shadows = UnityEngine.LightShadows.Soft;
var pit = fire.AddComponent<FirePit>();
var soF = new UnityEditor.SerializedObject(pit);
soF.FindProperty("prompt").stringValue = "Fire";
var arr = soF.FindProperty("burningObjects"); arr.arraySize = 4;
arr.GetArrayElementAtIndex(0).objectReferenceValue = flames; arr.GetArrayElementAtIndex(1).objectReferenceValue = embers; arr.GetArrayElementAtIndex(2).objectReferenceValue = smoke; arr.GetArrayElementAtIndex(3).objectReferenceValue = fireLightGo;
soF.ApplyModifiedPropertiesWithoutUndo();
for (int i = 0; i < 4; i++)
{
    float ang = 45f + i * 90f; float r = 2.3f;
    var pos = V(fx + UnityEngine.Mathf.Sin(ang * UnityEngine.Mathf.Deg2Rad) * r, 0f, fz + UnityEngine.Mathf.Cos(ang * UnityEngine.Mathf.Deg2Rad) * r);
    pos.y = H(pos.x, pos.z);
    Place("CS_Log_Large_Seat_" + (i + 1), camp.transform, pos, ang + 90f, rename: "Seat_" + (i + 1));
}

// ================= GENERATOR (blockout) and modern props under Era_Modern =================
var genGo = Box("Generator", era.transform, V(232f, H(232f, 198f) + 0.45f, 198f), V(1.2f, 0.9f, 0.7f), metalMat, V(0f, 20f, 0f));
var gen = genGo.AddComponent<ToggleColorInteractable>();
var soG = new UnityEditor.SerializedObject(gen);
soG.FindProperty("prompt").stringValue = "Start the generator"; soG.FindProperty("target").objectReferenceValue = genGo.GetComponent<UnityEngine.Renderer>();
soG.FindProperty("onColor").colorValue = new UnityEngine.Color(0.9f, 0.6f, 0.2f); soG.FindProperty("popScale").floatValue = 1.05f;
soG.ApplyModifiedPropertiesWithoutUndo();
Place("CS_Lantern_Modern", era.transform, V(fx + 1.4f, H(fx + 1.4f, fz + 2.6f), fz + 2.6f), 0f, collider: false);
Place("CS_Table_Small_Modern_1", era.transform, V(fx + 4f, H(fx + 4f, fz + 1f), fz + 1f), 30f);
// Campground props (era neutral)
Place("CITW_Firewood_1", camp.transform, V(cx + 4.2f, H(cx + 4.2f, cz - 1f), cz - 1f), 0f);
Place("CITW_Axe", camp.transform, V(cx + 4.4f, H(cx + 4.4f, cz - 2.2f), cz - 2.2f), 40f, collider: false);
Place("CITW_Barrel_1", camp.transform, V(cx - 4.0f, H(cx - 4.0f, cz - 2.4f), cz - 2.4f), 0f);
Place("CITW_Crate", camp.transform, V(cx - 4.6f, H(cx - 4.6f, cz - 1.2f), cz - 1.2f), -20f);
Place("CITW_Bucket", camp.transform, V(cx + 3.6f, H(cx + 3.6f, cz + 1.6f), cz + 1.6f), 0f, collider: false);

// ================= FIREWATCH TOWER, Graybox pattern with pack materials =================
float tx = 228f, tz = 208f, ty = H(tx, tz);
var tower = new UnityEngine.GameObject("FirewatchTower"); tower.transform.SetParent(camp.transform, false); tower.transform.position = V(tx, ty, tz);
var T = tower.transform;
float deckTop = 10f, deckThick = 0.5f, deckHalf = 4f;
var structure = new UnityEngine.GameObject("Structure"); structure.transform.SetParent(T, false);
foreach (var sx in new[] { -3.25f, 3.25f }) foreach (var sz in new[] { -3.25f, 3.25f }) Box("Leg", structure.transform, V(sx, deckTop * 0.5f, sz), V(0.5f, deckTop, 0.5f), wallMat);
Box("Deck", structure.transform, V(0f, deckTop - deckThick * 0.5f, 0f), V(8f, deckThick, 8f), plankMat);
var rails = new UnityEngine.GameObject("DeckRails"); rails.transform.SetParent(T, false);
float rh = 1.1f, ry = deckTop + rh * 0.5f;
Box("RailN", rails.transform, V(0f, ry, deckHalf), V(8f, rh, 0.1f), plankMat); Box("RailE", rails.transform, V(deckHalf, ry, 0f), V(0.1f, rh, 8f), plankMat); Box("RailW", rails.transform, V(-deckHalf, ry, 0f), V(0.1f, rh, 8f), plankMat);
Box("RailS_East", rails.transform, V(1.25f, ry, -deckHalf), V(5.5f, rh, 0.1f), plankMat); Box("RailS_West", rails.transform, V(-3.5f, ry, -deckHalf), V(1.0f, rh, 0.1f), plankMat);
var stairs = new UnityEngine.GameObject("Stairs"); stairs.transform.SetParent(T, false);
float laneN = -3.6f, laneS = -5.0f, laneW = 1.2f, stepThick = 0.5f, landThick = 0.5f, stairRail = 1.0f, landDepthZ = 2.6f, landCenterZ = -4.3f;
float theta = UnityEngine.Mathf.Atan2(0.25f, 0.3f) * UnityEngine.Mathf.Rad2Deg, sinT = UnityEngine.Mathf.Sin(theta * UnityEngine.Mathf.Deg2Rad), cosT = UnityEngine.Mathf.Cos(theta * UnityEngine.Mathf.Deg2Rad);
float rampLen = UnityEngine.Mathf.Sqrt(3f * 3f + 2.5f * 2.5f);
void Flight(int n, float z, float h0, bool eastward)
{
    var g = new UnityEngine.GameObject("Flight" + n); g.transform.SetParent(stairs.transform, false);
    for (int i = 0; i < 10; i++) { float top = h0 + 0.25f * (i + 1); float x = eastward ? (-1.5f + 0.15f + 0.3f * i) : (1.5f - 0.15f - 0.3f * i); Box("Step" + (i + 1), g.transform, V(x, top - stepThick * 0.5f, z), V(0.3f, stepThick, laneW), plankMat); }
    float midY = (h0 + 0.25f + h0 + 2.5f) * 0.5f + stairRail * 0.5f; float len = UnityEngine.Mathf.Sqrt(3f * 3f + 2.25f * 2.25f); float ang = UnityEngine.Mathf.Atan2(2.25f, 3f) * UnityEngine.Mathf.Rad2Deg * (eastward ? 1f : -1f);
    Box("RailN", g.transform, V(0f, midY, z + laneW * 0.5f), V(len, stairRail, 0.05f), plankMat, V(0f, 0f, ang)); Box("RailS", g.transform, V(0f, midY, z - laneW * 0.5f), V(len, stairRail, 0.05f), plankMat, V(0f, 0f, ang));
    // STAIRS RULE: collider-only ramp along the nosings
    float x0 = eastward ? -1.5f : 1.5f; float cxr = eastward ? x0 + 1.2f : x0 - 1.2f;
    var up = eastward ? V(-sinT, cosT, 0f) : V(sinT, cosT, 0f);
    var ramp = new UnityEngine.GameObject("StairRamp"); ramp.transform.SetParent(g.transform, false);
    ramp.transform.localPosition = V(cxr, h0 + 1.25f, z) - up * 0.1f; ramp.transform.localRotation = UnityEngine.Quaternion.Euler(0f, 0f, eastward ? theta : -theta); ramp.transform.localScale = V(rampLen, 0.2f, laneW);
    ramp.AddComponent<UnityEngine.BoxCollider>();
}
void Landing(int n, float x, float top, bool railNorth, bool railSouth, bool railOuter)
{
    var g = new UnityEngine.GameObject("Landing" + n); g.transform.SetParent(stairs.transform, false);
    Box("Slab", g.transform, V(x, top - landThick * 0.5f, landCenterZ), V(1.5f, landThick, landDepthZ), plankMat);
    float y = top + stairRail * 0.5f;
    if (railNorth) Box("RailN", g.transform, V(x, y, landCenterZ + landDepthZ * 0.5f), V(1.5f, stairRail, 0.05f), plankMat);
    if (railSouth) Box("RailS", g.transform, V(x, y, landCenterZ - landDepthZ * 0.5f), V(1.5f, stairRail, 0.05f), plankMat);
    if (railOuter) Box("RailOuter", g.transform, V(x + (x > 0 ? 0.75f : -0.75f), y, landCenterZ), V(0.05f, stairRail, landDepthZ), plankMat);
}
Flight(1, laneN, 0f, true); Landing(1, 2.25f, 2.5f, true, true, true); Flight(2, laneS, 2.5f, false); Landing(2, -2.25f, 5f, true, true, true);
Flight(3, laneN, 5f, true); Landing(3, 2.25f, 7.5f, true, true, true); Flight(4, laneS, 7.5f, false); Landing(4, -2.25f, 10f, false, true, true);
// Top room 4 x 4, plank walls, door on the south wall at x -1.1
var room = new UnityEngine.GameObject("TopRoom"); room.transform.SetParent(T, false); room.transform.localPosition = V(0f, deckTop, 0f);
float wh = 2.5f, wt = 0.2f, sill = 1.2f, winH = 0.8f, topBand = wh - sill - winH;
Box("N_Bottom", room.transform, V(0f, sill * 0.5f, 2.1f), V(4.4f, sill, wt), plankMat); Box("N_Top", room.transform, V(0f, wh - topBand * 0.5f, 2.1f), V(4.4f, topBand, wt), plankMat);
Box("N_PierW", room.transform, V(-1.35f, sill + winH * 0.5f, 2.1f), V(1.7f, winH, wt), plankMat); Box("N_PierE", room.transform, V(1.35f, sill + winH * 0.5f, 2.1f), V(1.7f, winH, wt), plankMat);
foreach (var side in new[] { -2.1f, 2.1f }) { string p = side > 0 ? "E_" : "W_"; Box(p + "Bottom", room.transform, V(side, sill * 0.5f, 0f), V(wt, sill, 4f), plankMat); Box(p + "Top", room.transform, V(side, wh - topBand * 0.5f, 0f), V(wt, topBand, 4f), plankMat); Box(p + "PierS", room.transform, V(side, sill + winH * 0.5f, -1.25f), V(wt, winH, 1.5f), plankMat); Box(p + "PierN", room.transform, V(side, sill + winH * 0.5f, 1.25f), V(wt, winH, 1.5f), plankMat); }
Box("S_PierW", room.transform, V(-1.9f, wh * 0.5f, -2.1f), V(0.6f, wh, wt), plankMat); Box("S_DoorLintel", room.transform, V(-1.1f, 2.3f, -2.1f), V(1.0f, 0.4f, wt), plankMat); Box("S_PierMid", room.transform, V(0f, wh * 0.5f, -2.1f), V(1.2f, wh, wt), plankMat);
Box("S_WinBottom", room.transform, V(1.1f, sill * 0.5f, -2.1f), V(1.0f, sill, wt), plankMat); Box("S_WinTop", room.transform, V(1.1f, wh - topBand * 0.5f, -2.1f), V(1.0f, topBand, wt), plankMat); Box("S_PierE", room.transform, V(1.9f, wh * 0.5f, -2.1f), V(0.6f, wh, wt), plankMat);
Box("Roof", room.transform, V(0f, wh + 0.1f, 0f), V(4.4f, 0.2f, 4.4f), plankMat);
Place("CITW_Table", room.transform, V(1.2f, 0f, 1.5f), 0f, rename: "Desk");
MakeDoor(room.transform, V(-1.6f, 0f, -2.1f), 0f, "Open");

// ================= Clear trees where the camp buildings stand =================
var data = terrain.terrainData; float size = data.size.x;
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>();
var clear = new (UnityEngine.Vector2 c, float r)[] { (new UnityEngine.Vector2(tx, tz), 10.5f), (new UnityEngine.Vector2(tx, tz - 5f), 6f), (new UnityEngine.Vector2(232f, 198f), 3f), (new UnityEngine.Vector2(cx, cz), 6.5f) };
int removed = 0;
foreach (var t in data.treeInstances)
{
    var p = new UnityEngine.Vector2(t.position.x * size, t.position.z * size); bool drop = false;
    foreach (var c in clear) if (UnityEngine.Vector2.Distance(p, c.c) < c.r) drop = true;
    if (drop) removed++; else keep.Add(t);
}
data.SetTreeInstances(keep.ToArray(), true);
UnityEditor.EditorUtility.SetDirty(data);

// ================= Player wakes inside the cabin, facing the door =================
var player = UnityEngine.GameObject.Find("Player");
player.transform.position = V(cx + 0.2f, cy + 0.05f, cz + 0.6f);
player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
int campObjects = camp.GetComponentsInChildren<UnityEngine.Transform>().Length, eraObjects = era.GetComponentsInChildren<UnityEngine.Transform>().Length;
return "saved=" + saved + " campObjects=" + campObjects + " eraObjects=" + eraObjects + " treesRemoved=" + removed + " cabinFloorY=" + cy.ToString("F2") + " towerY=" + ty.ToString("F2");
