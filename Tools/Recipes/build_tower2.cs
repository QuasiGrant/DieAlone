if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first: " + scene.path;
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
UnityEngine.GameObject Prefab(string path) { var p = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path); if (p == null) throw new System.Exception("missing " + path); return p; }
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return Prefab(p); }
    throw new System.Exception("no prefab named " + name);
}
UnityEngine.GameObject Place(string name, UnityEngine.Transform parent, UnityEngine.Vector3 localPos, float yaw, bool collider = true, string rename = null)
{
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(name), scene);
    go.transform.SetParent(parent, false); go.transform.localPosition = localPos; go.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    if (rename != null) go.name = rename;
    if (collider && go.GetComponentInChildren<UnityEngine.Collider>() == null)
    {
        var rends = go.GetComponentsInChildren<UnityEngine.Renderer>();
        if (rends.Length > 0)
        {
            var b = new UnityEngine.Bounds(); bool first = true;
            foreach (var r in rends) { if (first) { b = r.bounds; first = false; } else b.Encapsulate(r.bounds); }
            var box = go.AddComponent<UnityEngine.BoxCollider>();
            box.center = go.transform.InverseTransformPoint(b.center);
            var inv = UnityEngine.Quaternion.Inverse(go.transform.rotation) * b.size;
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

var camp = UnityEngine.GameObject.Find("Camp").transform;
var oldTower = camp.Find("FirewatchTower");
float tx = 236f, tz = 200f;
if (oldTower != null) { tx = oldTower.position.x; tz = oldTower.position.z; UnityEngine.Object.DestroyImmediate(oldTower.gameObject); }
float ty = H(tx, tz);
var wallMat = Find("CITW_Log_Wall").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;
var plankMat = Find("CITW_Plank_Wall").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;

// ================= FIREWATCH TOWER, deck at 15 m (six flights) so the room clears the bigger canopy =================
var tower = new UnityEngine.GameObject("FirewatchTower"); tower.transform.SetParent(camp, false); tower.transform.position = V(tx, ty, tz);
var T = tower.transform;
float deckTop = 15f, deckThick = 0.5f, deckHalf = 4f;
var structure = new UnityEngine.GameObject("Structure"); structure.transform.SetParent(T, false);
foreach (var sx in new[] { -3.25f, 3.25f }) foreach (var sz in new[] { -3.25f, 3.25f }) Box("Leg", structure.transform, V(sx, deckTop * 0.5f, sz), V(0.5f, deckTop, 0.5f), wallMat);
// Cross braces every 5 m so the taller legs read as a built frame.
foreach (var lvl in new[] { 4.5f, 9.5f })
{
    Box("BraceN", structure.transform, V(0f, lvl, 3.25f), V(6.5f, 0.25f, 0.25f), wallMat); Box("BraceS", structure.transform, V(0f, lvl, -3.25f), V(6.5f, 0.25f, 0.25f), wallMat);
    Box("BraceE", structure.transform, V(3.25f, lvl, 0f), V(0.25f, 0.25f, 6.5f), wallMat); Box("BraceW", structure.transform, V(-3.25f, lvl, 0f), V(0.25f, 0.25f, 6.5f), wallMat);
}
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
Flight(3, laneN, 5f, true); Landing(3, 2.25f, 7.5f, true, true, true); Flight(4, laneS, 7.5f, false); Landing(4, -2.25f, 10f, true, true, true);
Flight(5, laneN, 10f, true); Landing(5, 2.25f, 12.5f, true, true, true); Flight(6, laneS, 12.5f, false); Landing(6, -2.25f, 15f, false, true, true);
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

// Clear the new forest off the tower footprint and the stair approach.
var data = terrain.terrainData; float size = data.size.x;
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>();
var clear = new (UnityEngine.Vector2 c, float r)[] { (new UnityEngine.Vector2(tx, tz), 10.5f), (new UnityEngine.Vector2(tx, tz - 5f), 6f) };
int removed = 0;
foreach (var t in data.treeInstances)
{
    var p = new UnityEngine.Vector2(t.position.x * size, t.position.z * size); bool drop = false;
    foreach (var c in clear) if (UnityEngine.Vector2.Distance(p, c.c) < c.r) drop = true;
    if (drop) removed++; else keep.Add(t);
}
data.SetTreeInstances(keep.ToArray(), true);
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " tower=(" + tx + "," + ty.ToString("F1") + "," + tz + ") deckTop=" + (ty + deckTop).ToString("F1") + " roomRoof=" + (ty + deckTop + wh + 0.2f).ToString("F1") + " treesRemoved=" + removed;
