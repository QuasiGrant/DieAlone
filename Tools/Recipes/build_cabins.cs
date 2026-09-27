if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
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
        if (rends.Length > 0) { var b = rends[0].bounds; foreach (var r in rends) b.Encapsulate(r.bounds); var box = go.AddComponent<UnityEngine.BoxCollider>(); box.center = go.transform.InverseTransformPoint(b.center); var s = go.transform.InverseTransformVector(b.size); box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(s.x), UnityEngine.Mathf.Abs(s.y), UnityEngine.Mathf.Abs(s.z)); }
    }
    return go;
}
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 sz, UnityEngine.Material mat, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name; go.transform.SetParent(parent, false); go.transform.localPosition = pos; go.transform.localScale = sz;
    if (euler.HasValue) go.transform.localRotation = UnityEngine.Quaternion.Euler(euler.Value);
    if (mat != null) go.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat; else go.GetComponent<UnityEngine.Renderer>().enabled = false;
    return go;
}
void MakeDoor(UnityEngine.Transform parent, UnityEngine.Vector3 hingeLocal, float yaw, string panelPrefab)
{
    var hinge = new UnityEngine.GameObject("Door"); hinge.transform.SetParent(parent, false);
    hinge.transform.localPosition = hingeLocal; hinge.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    var rb = hinge.AddComponent<UnityEngine.Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
    var panel = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(panelPrefab), scene);
    panel.name = "Panel"; panel.transform.SetParent(hinge.transform, false); panel.transform.localPosition = UnityEngine.Vector3.zero;
    var col = panel.AddComponent<UnityEngine.BoxCollider>(); col.center = V(0.5f, 1.05f, 0f); col.size = V(1.0f, 2.1f, 0.23f);
    var door = hinge.AddComponent<Door>(); var so = new UnityEditor.SerializedObject(door);
    so.FindProperty("prompt").stringValue = "Open"; so.FindProperty("tuning").objectReferenceValue = tuning; so.FindProperty("panel").objectReferenceValue = col; so.ApplyModifiedPropertiesWithoutUndo();
}
void Usable(UnityEngine.GameObject go, string prompt)
{
    var t = go.AddComponent<ToggleColorInteractable>(); var so = new UnityEditor.SerializedObject(t);
    so.FindProperty("prompt").stringValue = prompt; so.FindProperty("target").objectReferenceValue = go.GetComponentInChildren<UnityEngine.Renderer>();
    so.FindProperty("onColor").colorValue = new UnityEngine.Color(0.75f, 0.85f, 0.6f); so.FindProperty("popScale").floatValue = 1.04f; so.ApplyModifiedPropertiesWithoutUndo();
}
// ---- Cabin builder: w x d modules of 2 m, door in the north wall at module index doorIx, optional porch on the north side, optional partition.
UnityEngine.Transform Cabin(string name, UnityEngine.Transform parent, float cx, float cz, float yaw, int w, int d, int doorIx, string style, bool porch, int partitionAtModule)
{
    string wall = "CITW_" + style + "_Wall", window = "CITW_" + style + "_Window_Wall", doorway = "CITW_" + style + "_Doorway";
    var cabin = new UnityEngine.GameObject(name); cabin.transform.SetParent(parent, false); cabin.transform.position = V(cx, H(cx, cz) + 0.1f, cz); cabin.transform.rotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    var C = cabin.transform;
    float hw = w, hd = d;   // half extents in metres (module 2 m)
    float X(int i) => (i - (w - 1) * 0.5f) * 2f; float Z(int j) => (j - (d - 1) * 0.5f) * 2f;
    for (int i = 0; i < w; i++) for (int j = 0; j < d; j++) Place("CITW_Floor", C, V(X(i), 0f, Z(j)), 0f);
    for (int i = 0; i < w; i++)
    {
        // South wall (yaw 0), windows at the ends.
        Place(i == 0 || i == w - 1 ? window : wall, C, V(X(i), 0f, -hd), 0f);
        // North wall (yaw 180) with the doorway.
        if (i == doorIx) { Place(doorway, C, V(X(i), 0f, hd), 180f, collider: false); Place("CITW_Door_Frame", C, V(X(i), 0f, hd), 180f, collider: false); Box("DoorJambA", C, V(X(i) - 0.75f, 1.5f, hd), V(0.5f, 3f, 0.3f), null); Box("DoorJambB", C, V(X(i) + 0.75f, 1.5f, hd), V(0.5f, 3f, 0.3f), null); Box("DoorLintel", C, V(X(i), 2.55f, hd), V(1f, 0.9f, 0.3f), null); MakeDoor(C, V(X(i) + 0.5f, 0f, hd), 180f, style == "Log" ? "CITW_Door_1" : "CITW_Door_3"); }
        else Place(i == 0 && w > 2 ? window : wall, C, V(X(i), 0f, hd), 180f);
    }
    for (int j = 0; j < d; j++) { Place(j == d - 1 ? window : wall, C, V(hw, 0f, Z(j)), 90f); Place(wall, C, V(-hw, 0f, Z(j)), -90f); }
    if (partitionAtModule > 0) { float px = X(partitionAtModule) - 1f; for (int j = 0; j < d; j++) Place(j == 0 ? doorway : wall, C, V(px, 0f, Z(j)), 90f, collider: j != 0); }
    // Roof: two slabs pitched over the depth, ridge along x, plank gables.
    float run = hd + 0.35f, drop = run * 0.6f, slabLen = UnityEngine.Mathf.Sqrt(run * run + drop * drop), pitch = UnityEngine.Mathf.Atan2(drop, run) * UnityEngine.Mathf.Rad2Deg;
    Box("Roof_S", C, V(0f, 3f + drop * 0.5f + 0.1f, -run * 0.5f), V(hw * 2f + 0.8f, 0.15f, slabLen), planks, V(-pitch, 0f, 0f));
    Box("Roof_N", C, V(0f, 3f + drop * 0.5f + 0.1f, run * 0.5f), V(hw * 2f + 0.8f, 0.15f, slabLen), planks, V(pitch, 0f, 0f));
    Box("RidgeBeam", C, V(0f, 3f + drop + 0.1f, 0f), V(hw * 2f + 1f, 0.2f, 0.2f), planks);
    foreach (var gx in new[] { -hw, hw })
    {
        var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
        var g = pb.gameObject; g.name = "Gable"; g.transform.SetParent(C, false);
        var pos = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
        for (int i = 0; i < pos.Count; i++) { var p = pos[i]; pos[i] = new UnityEngine.Vector3(p.x * 0.3f, p.y > 0f ? drop : 0f, p.y > 0f ? 0f : p.z * (hd * 2f + 0.3f)); }
        pb.positions = pos; pb.ToMesh(); pb.Refresh();
        g.transform.localPosition = V(gx, 3.0f, 0f); g.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
    }
    if (porch)
    {
        for (int i = 0; i < w; i++) Place("CITW_Floor", C, V(X(i), 0f, hd + 1f), 0f);
        foreach (var px in new[] { -hw + 0.3f, hw - 0.3f }) Place("CITW_Wood_Pillar", C, V(px, 0f, hd + 1.8f), 0f);
        Box("PorchRoof", C, V(0f, 2.9f, hd + 1.1f), V(hw * 2f + 0.8f, 0.12f, 2.4f), planks, V(8f, 0f, 0f));
        for (int i = 0; i < w; i++) if (i != doorIx) Place("CITW_Railing", C, V(X(i), 0.1f, hd + 1.9f), 0f, collider: false);
        Place("CITW_Railing", C, V(-hw, 0.1f, hd + 1f), 90f, collider: false); Place("CITW_Railing", C, V(hw, 0.1f, hd + 1f), 90f, collider: false);
    }
    var lampGo = new UnityEngine.GameObject("CabinLight"); lampGo.transform.SetParent(C, false); lampGo.transform.localPosition = V(0f, 2.4f, 0f);
    var ll = lampGo.AddComponent<UnityEngine.Light>(); ll.type = UnityEngine.LightType.Point; ll.color = new UnityEngine.Color(1f, 0.75f, 0.45f); ll.intensity = 2.0f; ll.range = 7f;
    return C;
}
void ClearTrees(float cx, float cz, float r)
{
    var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>();
    foreach (var ti in data.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(cx, cz)) < r) continue; keep.Add(ti); }
    data.SetTreeInstances(keep.ToArray(), true);
    int dres = data.detailResolution;
    for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
    {
        var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
        for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++) { if (map[zi, xi] == 0) continue; var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres); if (UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(cx, cz)) < r * 0.6f) { map[zi, xi] = 0; changed = true; } }
        if (changed) data.SetDetailLayer(0, 0, layer, map);
    }
}

// ================= Campsite 2: small log cabin with a porch, door facing the trail (north) =================
var s2 = UnityEngine.GameObject.Find("Campsites/Campsite_2_LeanTo").transform; s2.name = "Campsite_2_Cabin";
foreach (var n in new[] { "LeanTo", "CS_Bedroll_Old_2", "CS_Backpack_Old_3" }) { var c = s2.Find(n); if (c != null) UnityEngine.Object.DestroyImmediate(c.gameObject); }
ClearTrees(247f, 84.5f, 10f);
var c2 = Cabin("Cabin", s2, 247f, 84.5f, 0f, 2, 2, 1, "Log", true, 0);
Place("CITW_Bed", c2, V(-1.0f, 0f, -0.8f), 0f, rename: "Bunk");
Usable(c2.Find("Bunk").gameObject, "Check the bunk");
Place("CITW_Nightstand", c2, V(1.2f, 0f, -1.3f), 0f);
Place("CITW_Oil_Lamp_1", c2, V(1.2f, 0.7f, -1.3f), 0f, collider: false);
Place("CITW_Chair", c2, V(1.3f, 0f, 0.8f), -120f);
Place("CS_Backpack_Old_3", c2, V(-1.4f, 0f, 1.2f), 40f);
Place("CITW_Bucket", s2, V(250.2f, H(250.2f, 87.5f), 87.5f), 0f);

// ================= Campsite 3: long plank cabin, two rooms, door north; fire ring moved off the doorstep =================
var s3 = UnityEngine.GameObject.Find("Campsites/Campsite_3_FireRing").transform; s3.name = "Campsite_3_Cabin";
foreach (UnityEngine.Transform c in s3) { var p = c.position; c.position = V(p.x - 6f, H(p.x - 6f, p.z + 4f) + (p.y - H(p.x, p.z)), p.z + 4f); }
ClearTrees(325f, 56.5f, 11f);
var c3 = Cabin("Cabin", s3, 325f, 56.5f, 0f, 4, 2, 1, "Plank", false, 2);
Place("CITW_Bed", c3, V(2.6f, 0f, -0.9f), 0f, rename: "Bunk_1");
Place("CITW_Bed", c3, V(2.6f, 0f, 1.0f), 0f, rename: "Bunk_2");
Place("CITW_Table", c3, V(-2.2f, 0f, -0.6f), 0f, rename: "Table");
Place("CITW_Stool_2", c3, V(-2.2f, 0f, 0.6f), 0f);
Place("CITW_Wood_Stove", c3, V(-3.2f, 0f, 1.2f), 90f);
Place("CITW_Shelf", c3, V(-0.2f, 0f, -1.7f), 0f);
Place("CITW_Canned_Food_2", c3, V(-0.2f, 1.0f, -1.7f), 0f, collider: false);
Place("CITW_Oil_Lamp_2", c3, V(-2.2f, 0.8f, -0.6f), 0f, collider: false);
Usable(c3.Find("Table").gameObject, "Search the table");
Place("CITW_Barrel_4", s3, V(330.5f, H(330.5f, 59.5f), 59.5f), 20f);
Place("CS_Firewood_Logs", s3, V(329.6f, H(329.6f, 54.2f), 54.2f), 0f);

UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);
var warps = UnityEngine.GameObject.Find("DevWarps").transform;
var w2 = new UnityEngine.GameObject("Campsite 2 cabin"); w2.transform.SetParent(warps, false); w2.transform.position = V(252f, H(252f, 96f) + 0.2f, 96f); w2.transform.rotation = UnityEngine.Quaternion.LookRotation(V(247f, 0f, 86.5f) - V(252f, 0f, 96f), UnityEngine.Vector3.up);
var w3 = new UnityEngine.GameObject("Campsite 3 cabin"); w3.transform.SetParent(warps, false); w3.transform.position = V(314f, H(314f, 72f) + 0.2f, 72f); w3.transform.rotation = UnityEngine.Quaternion.LookRotation(V(323f, 0f, 58.5f) - V(314f, 0f, 72f), UnityEngine.Vector3.up);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " cabin2=" + c2.position.ToString("F1") + " parts=" + c2.childCount + " cabin3=" + c3.position.ToString("F1") + " parts=" + c3.childCount + " ring=" + s3.Find("FireRing").position.ToString("F1");
