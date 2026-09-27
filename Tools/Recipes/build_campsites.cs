if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("Campsites") != null) return "Campsites already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");

UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games/Campsite" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
    throw new System.Exception("no prefab named " + name);
}
UnityEngine.GameObject Place(string name, UnityEngine.Transform parent, float x, float z, float yaw, bool collider = true, float lift = 0f, string rename = null)
{
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(name), scene);
    go.transform.SetParent(parent, true);
    go.transform.position = V(x, H(x, z) + lift, z);
    go.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
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
            var s = UnityEngine.Quaternion.Inverse(go.transform.rotation) * b.size;
            box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(s.x), UnityEngine.Mathf.Abs(s.y), UnityEngine.Mathf.Abs(s.z));
        }
    }
    return go;
}
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Material mat, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name; go.transform.SetParent(parent, true); go.transform.position = pos; go.transform.localScale = size;
    if (euler.HasValue) go.transform.rotation = UnityEngine.Quaternion.Euler(euler.Value);
    go.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat; return go;
}
void Usable(UnityEngine.GameObject go, string prompt)
{
    var t = go.AddComponent<ToggleColorInteractable>();
    var so = new UnityEditor.SerializedObject(t);
    so.FindProperty("prompt").stringValue = prompt;
    so.FindProperty("target").objectReferenceValue = go.GetComponentInChildren<UnityEngine.Renderer>();
    so.FindProperty("onColor").colorValue = new UnityEngine.Color(0.75f, 0.85f, 0.6f);
    so.FindProperty("popScale").floatValue = 1.04f;
    so.ApplyModifiedPropertiesWithoutUndo();
}

var root = new UnityEngine.GameObject("Campsites");

// ================= Campsite 1 at (312,124): a small old tent, cold fire, a pack left behind =================
var c1 = new UnityEngine.GameObject("Campsite_1_Tent"); c1.transform.SetParent(root.transform, false);
var tent = Place("CS_Tent_Old_1", c1.transform, 315f, 126.5f, 210f, rename: "Tent");
var fire1 = Place("CS_Campfire_2", c1.transform, 311f, 123f, 0f, rename: "ColdFire");
Place("CS_Firewood_Logs_Burnt", fire1.transform, 311f, 123f, 20f, collider: false);
Place("CS_Log_Stool_1", c1.transform, 309.2f, 121.6f, 0f);
Place("CS_Log_Stool_2", c1.transform, 312.9f, 121.2f, 0f);
Place("CS_Backpack_Old_1", c1.transform, 313.6f, 124.4f, -40f);
Place("CS_Lantern_Old_Rusted", c1.transform, 310.2f, 124.6f, 0f, collider: false);
Place("CS_Cookware_Pot_1", c1.transform, 312.2f, 123.9f, 0f, collider: false);
Usable(tent, "Look inside the tent");

// ================= Campsite 2 at (246,88): a lean-to of poles and a tarp, bedroll, pot on a tripod =================
var c2 = new UnityEngine.GameObject("Campsite_2_LeanTo"); c2.transform.SetParent(root.transform, false);
var tarpMat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Lit"));
tarpMat.SetColor("_BaseColor", new UnityEngine.Color(0.22f, 0.24f, 0.16f)); tarpMat.SetFloat("_Smoothness", 0.1f);
UnityEditor.AssetDatabase.CreateAsset(tarpMat, "Assets/Materials/Tarp.mat");
float lx = 243.5f, lz = 90f, ly = H(lx, lz);
var leanTo = new UnityEngine.GameObject("LeanTo"); leanTo.transform.SetParent(c2.transform, false); leanTo.transform.position = V(lx, ly, lz); leanTo.transform.rotation = UnityEngine.Quaternion.Euler(0f, 35f, 0f);
var L = leanTo.transform;
// Two upright poles, a ridge pole, then the tarp sloping down to the ground behind.
foreach (var sx in new[] { -1.6f, 1.6f })
{
    var pole = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder);
    pole.name = "Pole"; pole.transform.SetParent(L, false); pole.transform.localPosition = V(sx, 0.9f, 0f); pole.transform.localScale = V(0.12f, 0.9f, 0.12f);
    pole.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
}
var ridge = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder);
ridge.name = "RidgePole"; ridge.transform.SetParent(L, false); ridge.transform.localPosition = V(0f, 1.8f, 0f); ridge.transform.localRotation = UnityEngine.Quaternion.Euler(0f, 0f, 90f); ridge.transform.localScale = V(0.1f, 1.9f, 0.1f);
ridge.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
float tarpDrop = 1.75f, tarpRun = 2.4f; float tarpLen = UnityEngine.Mathf.Sqrt(tarpDrop * tarpDrop + tarpRun * tarpRun); float tarpAng = UnityEngine.Mathf.Atan2(tarpDrop, tarpRun) * UnityEngine.Mathf.Rad2Deg;
var tarp = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
tarp.name = "Tarp"; tarp.transform.SetParent(L, false); tarp.transform.localPosition = V(0f, 1.8f - tarpDrop * 0.5f, tarpRun * 0.5f); tarp.transform.localRotation = UnityEngine.Quaternion.Euler(-tarpAng, 0f, 0f); tarp.transform.localScale = V(3.6f, 0.04f, tarpLen);
tarp.GetComponent<UnityEngine.Renderer>().sharedMaterial = tarpMat;
Place("CS_Bedroll_Old_2", c2.transform, lx + 0.3f, lz + 0.6f, 35f, collider: false, lift: 0.02f);
var fire2 = Place("CS_Campfire_2", c2.transform, lx - 1.2f, lz - 3.2f, 0f, rename: "ColdFire");
Place("CS_Firewood_Logs_Burnt", fire2.transform, lx - 1.2f, lz - 3.2f, -30f, collider: false);
Place("CS_Campfire_Tripod_Wood", c2.transform, lx - 1.2f, lz - 3.2f, 15f);
Place("CS_Cookware_Pot_2", c2.transform, lx - 1.2f, lz - 3.2f, 0f, collider: false, lift: 0.75f);
Place("CS_Log_Large_Seat_3", c2.transform, lx + 1.4f, lz - 3.8f, 100f);
Place("CS_Backpack_Old_3", c2.transform, lx - 2.3f, lz + 0.2f, 60f);
Usable(leanTo, "Check the shelter");
var leanCol = leanTo.AddComponent<UnityEngine.BoxCollider>(); leanCol.center = V(0f, 0.9f, 1.2f); leanCol.size = V(3.6f, 1.8f, 2.4f);

// ================= Campsite 3 at (322,60): a bare stone fire ring, one log, a bottle =================
var c3 = new UnityEngine.GameObject("Campsite_3_FireRing"); c3.transform.SetParent(root.transform, false);
float rx = 322f, rz = 60f;
var ring = new UnityEngine.GameObject("FireRing"); ring.transform.SetParent(c3.transform, false); ring.transform.position = V(rx, H(rx, rz), rz);
for (int i = 0; i < 10; i++)
{
    float ang = i * 36f + (i % 2) * 7f; float r = 0.85f;
    var s = Place("CS_Stone_" + (i % 8 + 1), ring.transform, rx + UnityEngine.Mathf.Sin(ang * UnityEngine.Mathf.Deg2Rad) * r, rz + UnityEngine.Mathf.Cos(ang * UnityEngine.Mathf.Deg2Rad) * r, ang * 3f, collider: false);
}
Place("CS_Firewood_Logs_Burnt", ring.transform, rx, rz, 50f, collider: false);
var ringCol = ring.AddComponent<UnityEngine.BoxCollider>(); ringCol.center = V(0f, 0.3f, 0f); ringCol.size = V(2.0f, 0.6f, 2.0f);
Place("CS_Log_Large_Long_Seat_2", c3.transform, rx + 2.4f, rz - 0.6f, 75f);
Place("CS_Drink_Whiskey", c3.transform, rx + 1.9f, rz + 0.6f, 0f, collider: false);
Place("CS_Rock_3", c3.transform, rx - 3.6f, rz + 2.8f, 120f);
Place("CS_Tool_Shovel_Wood_Rusted", c3.transform, rx - 1.1f, rz + 2.2f, 200f, collider: false);
Usable(ring, "Search the fire ring");

// ================= Clear any trees inside the three sites =================
var data = terrain.terrainData; float size = data.size.x;
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int removed = 0;
var sites = new[] { new UnityEngine.Vector2(312f, 124f), new UnityEngine.Vector2(246f, 88f), new UnityEngine.Vector2(322f, 60f) };
foreach (var t in data.treeInstances)
{
    var p = new UnityEngine.Vector2(t.position.x * size, t.position.z * size); bool drop = false;
    foreach (var s in sites) if (UnityEngine.Vector2.Distance(p, s) < 8.5f) drop = true;
    if (drop) removed++; else keep.Add(t);
}
if (removed > 0) { data.SetTreeInstances(keep.ToArray(), true); UnityEditor.EditorUtility.SetDirty(data); }
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " objects=" + root.GetComponentsInChildren<UnityEngine.Transform>().Length + " treesRemoved=" + removed + " tentPos=" + tent.transform.position.ToString("F1") + " leanToPos=" + leanTo.transform.position.ToString("F1") + " ringPos=" + ring.transform.position.ToString("F1");
