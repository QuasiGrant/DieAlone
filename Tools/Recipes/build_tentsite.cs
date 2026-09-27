if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
    throw new System.Exception("no prefab named " + name);
}
UnityEngine.GameObject Place(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, float yaw, bool collider = true, string rename = null)
{
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(name), scene);
    go.transform.SetParent(parent, false); go.transform.position = pos; go.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    if (rename != null) go.name = rename;
    if (collider && go.GetComponentInChildren<UnityEngine.Collider>() == null)
    {
        var rends = go.GetComponentsInChildren<UnityEngine.Renderer>();
        if (rends.Length > 0) { var b = rends[0].bounds; foreach (var r in rends) b.Encapsulate(r.bounds); var box = go.AddComponent<UnityEngine.BoxCollider>(); box.center = go.transform.InverseTransformPoint(b.center); var s = go.transform.InverseTransformVector(b.size); box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(s.x), UnityEngine.Mathf.Abs(s.y), UnityEngine.Mathf.Abs(s.z)); }
    }
    return go;
}
void Usable(UnityEngine.GameObject go, string prompt)
{
    var t = go.AddComponent<ToggleColorInteractable>();
    var so = new UnityEditor.SerializedObject(t);
    so.FindProperty("prompt").stringValue = prompt; so.FindProperty("target").objectReferenceValue = go.GetComponentInChildren<UnityEngine.Renderer>();
    so.FindProperty("onColor").colorValue = new UnityEngine.Color(0.75f, 0.85f, 0.6f); so.FindProperty("popScale").floatValue = 1.04f;
    so.ApplyModifiedPropertiesWithoutUndo();
}
var site = UnityEngine.GameObject.Find("Campsites/Campsite_1_Tent").transform;
site.name = "Campsite_1_Tents";
for (int i = site.childCount - 1; i >= 0; i--) UnityEngine.Object.DestroyImmediate(site.GetChild(i).gameObject);
float fx = 314f, fz = 123f, fy = H(fx, fz);
UnityEngine.Vector3 Ring(float angle, float r) { var p = V(fx + UnityEngine.Mathf.Sin(angle * UnityEngine.Mathf.Deg2Rad) * r, 0f, fz + UnityEngine.Mathf.Cos(angle * UnityEngine.Mathf.Deg2Rad) * r); p.y = H(p.x, p.z); return p; }

// ---- Lit fire: a copy of the camp fire pit (flames, embers, smoke, flickering light, FirePit script).
var fire = UnityEngine.Object.Instantiate(UnityEngine.GameObject.Find("Camp/FirePit")); fire.name = "FirePit"; fire.transform.SetParent(site, false); fire.transform.position = V(fx, fy, fz);
// ---- Tents around the fire, opening side toward it; the west stays open for the trail.
var tents = new (string prefab, float angle, float r, string name)[] { ("CS_Tent_Old_1", 20f, 6.5f, "Tent_1"), ("CS_Tent_Old_3", 95f, 6.0f, "Tent_2"), ("CS_Tent_Modern_1", 165f, 5.5f, "Tent_3"), ("CS_Tent_Modern_2", 215f, 6.0f, "Tent_4") };
foreach (var t in tents) Place(t.prefab, site, Ring(t.angle, t.r), t.angle + 180f, rename: t.name);
var big = Place("CS_Tent_Large_Old_Preset_1", site, Ring(325f, 9.5f), 325f + 180f, rename: "Tent_Main");
Usable(big, "Look inside the tent");
// ---- Seats and camp furniture.
int s = 0; foreach (var a in new[] { 50f, 140f, 250f }) { s++; Place("CS_Log_Large_Seat_" + s, site, Ring(a, 2.3f), a, rename: "Seat_" + s); }
Place("CS_Chair_2", site, Ring(300f, 2.6f), 300f + 180f, rename: "CampChair");
Place("CS_Log_Stool_1", site, Ring(190f, 2.2f), 10f, rename: "Stool");
Place("CS_Campfire_Tripod_Metal", site, V(fx, fy, fz), 40f, collider: false, rename: "Tripod");
Place("CS_Cookware_Kettle_2", site, V(fx, fy + 0.8f, fz), 0f, collider: false, rename: "Kettle");
var table = Place("CS_Table_Big_Modern_1", site, Ring(125f, 5.0f), 30f, rename: "Table");
Place("CS_Cookware_Pan_1", site, table.transform.position + V(0.1f, 0.9f, 0.1f), 20f, collider: false, rename: "Pan");
Place("CS_Lantern_Modern", site, table.transform.position + V(-0.3f, 0.9f, -0.2f), 0f, collider: false, rename: "TableLantern");
Place("CS_Firewood_Logs", site, Ring(75f, 4.6f), 60f, rename: "WoodPile");
Place("CS_Backpack_Modern_2", site, Ring(160f, 4.2f), 200f, rename: "Pack_1");
Place("CS_Backpack_Old_4", site, Ring(30f, 4.6f), 120f, rename: "Pack_2");
Place("CS_Bedroll_Modern_Rolled_1", site, Ring(222f, 4.3f), 80f, collider: false, rename: "Bedroll");
Place("CS_Lantern_Old", site, Ring(300f, 4.0f), 0f, collider: false, rename: "GroundLantern");
// ---- Clearing: wider, no trees within 14 m, cover thinned, none under tents or at the fire.
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cut = 0;
foreach (var ti in data.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(fx, fz)) < 14f) { cut++; continue; } keep.Add(ti); }
data.SetTreeInstances(keep.ToArray(), true);
var tentPos = new System.Collections.Generic.List<UnityEngine.Vector2>(); foreach (UnityEngine.Transform c in site) if (c.name.StartsWith("Tent")) tentPos.Add(new UnityEngine.Vector2(c.position.x, c.position.z));
int dres = data.detailResolution; var rng = new System.Random(83);
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
    {
        if (map[zi, xi] == 0) continue;
        var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres);
        float df = UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(fx, fz)); if (df > 14f) continue;
        bool underTent = false; foreach (var tp in tentPos) if (UnityEngine.Vector2.Distance(p, tp) < 3.2f) underTent = true;
        if (df < 5f || underTent || rng.NextDouble() < 0.6) { map[zi, xi] = 0; changed = true; }
    }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);
var warps = UnityEngine.GameObject.Find("DevWarps").transform;
var warp = new UnityEngine.GameObject("Campsite 1 tents"); warp.transform.SetParent(warps, false); warp.transform.position = V(303f, H(303f, 126f) + 0.2f, 126f); warp.transform.rotation = UnityEngine.Quaternion.LookRotation(V(fx, 0f, fz) - V(303f, 0f, 126f), UnityEngine.Vector3.up);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var pit = fire.GetComponent<FirePit>(); var so2 = new UnityEditor.SerializedObject(pit); int burning = so2.FindProperty("burningObjects").arraySize; int own = 0; for (int i = 0; i < burning; i++) { var o = so2.FindProperty("burningObjects").GetArrayElementAtIndex(i).objectReferenceValue as UnityEngine.GameObject; if (o != null && o.transform.IsChildOf(fire.transform)) own++; }
return "saved=" + saved + " children=" + site.childCount + " treesCut=" + cut + " fireBurningRefs=" + burning + " pointingAtOwnChildren=" + own;
