if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
    throw new System.Exception("no prefab named " + name);
}
var camp = UnityEngine.GameObject.Find("Camp").transform;
UnityEngine.GameObject Place(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Quaternion rot, bool collider = true, string rename = null)
{
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(name), scene);
    go.transform.SetParent(parent, false); go.transform.position = pos; go.transform.rotation = rot;
    if (rename != null) go.name = rename;
    if (collider && go.GetComponentInChildren<UnityEngine.Collider>() == null)
    {
        var rends = go.GetComponentsInChildren<UnityEngine.Renderer>();
        if (rends.Length > 0)
        {
            var b = rends[0].bounds; foreach (var r in rends) b.Encapsulate(r.bounds);
            var box = go.AddComponent<UnityEngine.BoxCollider>();
            box.center = go.transform.InverseTransformPoint(b.center);
            var s = go.transform.InverseTransformVector(b.size); box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(s.x), UnityEngine.Mathf.Abs(s.y), UnityEngine.Mathf.Abs(s.z));
        }
    }
    return go;
}
UnityEngine.Quaternion Yaw(float y) => UnityEngine.Quaternion.Euler(0f, y, 0f);
float fx = 251.5f, fz = 182.5f, fy = H(fx, fz);

// ---- Seats: logs tangent to the fire ring, 2.3 m out, with a gap toward the cabin (north-west).
foreach (var n in new[] { "Seat_1", "Seat_2", "Seat_3", "Seat_4" }) { var s = camp.Find(n); if (s != null) UnityEngine.Object.DestroyImmediate(s.gameObject); }
int seatIdx = 0;
foreach (var a in new[] { 70f, 165f, 250f })
{
    seatIdx++;
    float r = 2.3f; var p = V(fx + UnityEngine.Mathf.Sin(a * UnityEngine.Mathf.Deg2Rad) * r, 0f, fz + UnityEngine.Mathf.Cos(a * UnityEngine.Mathf.Deg2Rad) * r); p.y = H(p.x, p.z);
    Place("CS_Log_Large_Seat_" + seatIdx, camp, p, Yaw(a), rename: "Seat_" + seatIdx);   // log runs along x, so yaw = angle makes it tangent
}
{ float a = 320f, r = 2.4f; var p = V(fx + UnityEngine.Mathf.Sin(a * UnityEngine.Mathf.Deg2Rad) * r, 0f, fz + UnityEngine.Mathf.Cos(a * UnityEngine.Mathf.Deg2Rad) * r); p.y = H(p.x, p.z); Place("CS_Chair_1", camp, p, Yaw(a + 180f), rename: "CampChair"); }
{ float a = 20f, r = 2.0f; var p = V(fx + UnityEngine.Mathf.Sin(a * UnityEngine.Mathf.Deg2Rad) * r, 0f, fz + UnityEngine.Mathf.Cos(a * UnityEngine.Mathf.Deg2Rad) * r); p.y = H(p.x, p.z); Place("CS_Stool_1", camp, p, Yaw(a + 180f), rename: "Stool"); }

// ---- Fire: bigger flames, a tall flame layer, a tripod and pot over it.
var fire = camp.Find("FirePit");
var shortFlames = fire.Find("FX_Flames_Short"); if (shortFlames != null) shortFlames.localScale = V(0.8f, 0.8f, 0.8f);
var tall = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find("FX_Flames_Tall"), scene);
tall.name = "FX_Flames_Tall"; tall.transform.SetParent(fire, false); tall.transform.localPosition = V(0f, 0.05f, 0f); tall.transform.localScale = V(0.55f, 0.55f, 0.55f);
var pit = fire.GetComponent<FirePit>();
var so = new UnityEditor.SerializedObject(pit); var arr = so.FindProperty("burningObjects"); arr.arraySize++; arr.GetArrayElementAtIndex(arr.arraySize - 1).objectReferenceValue = tall; so.ApplyModifiedPropertiesWithoutUndo();
Place("CS_Campfire_Tripod_Wood", camp, V(fx, fy, fz), Yaw(25f), collider: false, rename: "Tripod");
Place("CS_Cookware_Pot_1", camp, V(fx, fy + 0.8f, fz), Yaw(0f), collider: false, rename: "Pot");

// ---- Wood: chopping block with the axe lying on it, a stacked pile against the cabin wall, split logs about.
foreach (var n in new[] { "CITW_Firewood_1", "CITW_Axe" }) { var s = camp.Find(n); if (s != null) UnityEngine.Object.DestroyImmediate(s.gameObject); }
float bx = 250.2f, bz = 188.4f;
var block = Place("CS_Log_Large_Short", camp, V(bx, H(bx, bz), bz), Yaw(20f), rename: "ChoppingBlock");
Place("CS_Tool_Axe_Wood", camp, V(bx - 0.05f, H(bx, bz) + 0.39f, bz + 0.1f), UnityEngine.Quaternion.Euler(90f, 35f, 0f), collider: false, rename: "Axe");
Place("CS_Firewood_Logs", camp, V(248.75f, H(248.75f, 190.9f), 190.9f), Yaw(90f), rename: "WoodPile");
Place("CS_Firewood_Short_1", camp, V(251.2f, H(251.2f, 189.0f), 189.0f), Yaw(40f), collider: false, rename: "SplitLog_1");
Place("CS_Firewood_Short_2", camp, V(250.9f, H(250.9f, 187.4f), 187.4f), Yaw(110f), collider: false, rename: "SplitLog_2");

// ---- Decoration: table with kettle and mug, lantern on a stool by the door, ladder against the wall, second barrel, pack by the door.
var table = Place("CS_Table_Small_Primitive_1", camp, V(254.6f, H(254.6f, 180.0f), 180.0f), Yaw(15f), rename: "CampTable");
Place("CS_Cookware_Kettle_1", camp, V(254.5f, H(254.6f, 180.0f) + 0.61f, 180.1f), Yaw(60f), collider: false, rename: "Kettle");
Place("CITW_Mug", camp, V(254.8f, H(254.6f, 180.0f) + 0.61f, 179.8f), Yaw(0f), collider: false, rename: "Mug");
Place("CS_Stool_2", camp, V(246.6f, H(246.6f, 187.0f), 187.0f), Yaw(0f), rename: "DoorStool");
Place("CS_Lantern_Old", camp, V(246.6f, H(246.6f, 187.0f) + 0.53f, 187.0f), Yaw(30f), collider: false, rename: "DoorLantern");
Place("CITW_Ladder", camp, V(248.85f, H(248.85f, 191.6f), 191.6f), UnityEngine.Quaternion.AngleAxis(15f, UnityEngine.Vector3.forward) * Yaw(90f), rename: "Ladder");
Place("CITW_Barrel_2", camp, V(241.0f, H(241f, 186.5f), 186.5f), Yaw(30f), rename: "Barrel_2");
Place("CS_Backpack_Modern_1", camp, V(243.7f, H(243.7f, 187.3f), 187.3f), Yaw(200f), rename: "DoorPack");

// ---- Ground cover: none right around the fire and inside the cabin, thin across the clearing.
var data = terrain.terrainData; int dres = data.detailResolution; float size = data.size.x; var rng = new System.Random(3); int cleared = 0;
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
    {
        if (map[zi, xi] == 0) continue;
        float x = (xi + 0.5f) * size / dres, z = (zi + 0.5f) * size / dres;
        float df = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(x, z), new UnityEngine.Vector2(fx, fz));
        bool cabin = x > 241f && x < 249.5f && z > 186.5f && z < 193f;
        bool clearing = UnityEngine.Vector2.Distance(new UnityEngine.Vector2(x, z), new UnityEngine.Vector2(250f, 180f)) < 17f;
        if (df < 5.5f || cabin || (clearing && rng.NextDouble() < 0.6)) { map[zi, xi] = 0; changed = true; cleared++; }
    }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " campChildren=" + camp.childCount + " burning=" + arr.arraySize + " grassCleared=" + cleared;
