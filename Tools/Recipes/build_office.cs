if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("Entrance") != null) return "Entrance already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x, sizeY = data.size.y;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/Settings/PlayerTuning.asset");
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games", "Assets/Celestia_Studio" }))
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
        if (rends.Length > 0)
        {
            var b = rends[0].bounds; foreach (var r in rends) b.Encapsulate(r.bounds);
            var box = go.AddComponent<UnityEngine.BoxCollider>(); box.center = go.transform.InverseTransformPoint(b.center);
            var s = go.transform.InverseTransformVector(b.size); box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(s.x), UnityEngine.Mathf.Abs(s.y), UnityEngine.Mathf.Abs(s.z));
        }
    }
    return go;
}
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 sz, UnityEngine.Material mat, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name; go.transform.SetParent(parent, false); go.transform.localPosition = pos; go.transform.localScale = sz;
    if (euler.HasValue) go.transform.localRotation = UnityEngine.Quaternion.Euler(euler.Value);
    if (mat != null) go.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat;
    return go;
}
UnityEngine.GameObject MakeDoor(UnityEngine.Transform parent, UnityEngine.Vector3 hingeLocal, float yaw, string prompt)
{
    var hinge = new UnityEngine.GameObject("Door"); hinge.transform.SetParent(parent, false);
    hinge.transform.localPosition = hingeLocal; hinge.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
    var rb = hinge.AddComponent<UnityEngine.Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
    var panel = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find("CITW_Door_2"), scene);
    panel.name = "Panel"; panel.transform.SetParent(hinge.transform, false); panel.transform.localPosition = UnityEngine.Vector3.zero;
    var col = panel.AddComponent<UnityEngine.BoxCollider>(); col.center = V(0.5f, 1.05f, 0f); col.size = V(1.0f, 2.1f, 0.23f);
    var door = hinge.AddComponent<Door>();
    var so = new UnityEditor.SerializedObject(door);
    so.FindProperty("prompt").stringValue = prompt; so.FindProperty("tuning").objectReferenceValue = tuning; so.FindProperty("panel").objectReferenceValue = col;
    so.ApplyModifiedPropertiesWithoutUndo();
    return hinge;
}

// ================= TERRAIN: road east, clearing for office and parking =================
var road = new UnityEngine.Vector2[] { new(266,178), new(300,176), new(340,174), new(388,174) };
float SegDist(UnityEngine.Vector2 p, UnityEngine.Vector2 a, UnityEngine.Vector2 b) { var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); return UnityEngine.Vector2.Distance(p, a + ab * t); }
float RoadDist(UnityEngine.Vector2 p) { float best = float.MaxValue; for (int i = 0; i < road.Length - 1; i++) best = UnityEngine.Mathf.Min(best, SegDist(p, road[i], road[i + 1])); return best; }
var lotC = new UnityEngine.Vector2(373f, 181f); float lotR = 24f;
bool InLot(UnityEngine.Vector2 p) => p.x > 358f && p.x < 388f && p.y > 156f && p.y < 202f;
int res = data.heightmapResolution; var heights = data.GetHeights(0, 0, res, res);
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * size / (res - 1), z = zi * size / (res - 1); var p = new UnityEngine.Vector2(x, z);
    float w = UnityEngine.Mathf.Max(1f - UnityEngine.Mathf.Clamp01((RoadDist(p) - 2.5f) / 3f), 1f - UnityEngine.Mathf.Clamp01((UnityEngine.Vector2.Distance(p, lotC) - lotR) / 6f));
    if (w <= 0f) continue;
    float h = heights[zi, xi] * sizeY; heights[zi, xi] = UnityEngine.Mathf.Lerp(h, 24f, w) / sizeY;
}
data.SetHeights(0, 0, heights);
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    float trail = UnityEngine.Mathf.Max(1f - UnityEngine.Mathf.Clamp01((RoadDist(p) - 2.0f) / 0.7f), InLot(p) ? 1f : 0f);
    float clear = 1f - UnityEngine.Mathf.Clamp01((UnityEngine.Vector2.Distance(p, lotC) - lotR) / 4f);
    if (trail <= 0f && clear <= 0f) continue;
    float t = UnityEngine.Mathf.Max(alpha[zi, xi, 1], trail);
    float moss = L > 3 ? alpha[zi, xi, 3] * (1f - UnityEngine.Mathf.Max(trail, clear)) : 0f;
    alpha[zi, xi, 1] = t; if (L > 3) alpha[zi, xi, 3] = moss; alpha[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - t - moss - (L > 2 ? alpha[zi, xi, 2] : 0f));
}
data.SetAlphamaps(0, 0, alpha);
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cut = 0;
foreach (var ti in data.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (RoadDist(p) < 4f || UnityEngine.Vector2.Distance(p, lotC) < lotR) { cut++; continue; } keep.Add(ti); }
data.SetTreeInstances(keep.ToArray(), true);
int dres = data.detailResolution; var rng = new System.Random(61);
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
    {
        if (map[zi, xi] == 0) continue;
        var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres);
        if (RoadDist(p) < 2.6f || InLot(p) || (UnityEngine.Vector2.Distance(p, lotC) < lotR && rng.NextDouble() < 0.7)) { map[zi, xi] = 0; changed = true; }
    }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);

// ================= ENTRANCE: fence, gate, office, parking, lights =================
var ent = new UnityEngine.GameObject("Entrance"); var E = ent.transform;
// Fence along x 388 from z 120 to 240, gap for the gate at z 170..178. Panels are 2 m, pivot at one end, running along +x, so yaw 90 runs them along +z.
var fence = new UnityEngine.GameObject("Fence"); fence.transform.SetParent(E, false);
int panels = 0;
for (float z = 120f; z < 240f; z += 2f)
{
    if (z >= 170f && z < 178f) continue;
    float y = H(388f, z + 1f);
    Place("Chain_Link_Fence", fence.transform, V(388f, y, z), 90f, rename: "Panel"); panels++;
    Place("Chain_Link_Fence_Post", fence.transform, V(388f, y, z), 90f, collider: false, rename: "Post");
}
Place("Chain_Link_Fence_Post", fence.transform, V(388f, H(388f, 240f), 240f), 90f, collider: false, rename: "Post");
// Gate: two 4 m leaves on hinges at z 170 and z 178, chain-link stretched to fit, opened with the Door system.
var gate = new UnityEngine.GameObject("Gate"); gate.transform.SetParent(E, false); gate.transform.position = V(388f, H(388f, 174f), 174f);
foreach (var side in new[] { -1f, 1f })
{
    var hinge = new UnityEngine.GameObject(side < 0 ? "GateLeaf_S" : "GateLeaf_N"); hinge.transform.SetParent(gate.transform, false);
    hinge.transform.localPosition = V(0f, 0f, side * 4f); hinge.transform.localRotation = UnityEngine.Quaternion.Euler(0f, side < 0 ? 90f : -90f, 0f);
    var rb = hinge.AddComponent<UnityEngine.Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
    var leaf = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find("Chain_Link_Fence"), scene);
    leaf.name = "Panel"; leaf.transform.SetParent(hinge.transform, false); leaf.transform.localPosition = UnityEngine.Vector3.zero; leaf.transform.localScale = V(2f, 1.25f, 1f);
    foreach (var c in leaf.GetComponentsInChildren<UnityEngine.Collider>()) UnityEngine.Object.DestroyImmediate(c);
    var col = leaf.AddComponent<UnityEngine.BoxCollider>(); col.center = V(1f, 0.82f, 0f); col.size = V(2f, 1.64f, 0.12f);
    Box("Rail", hinge.transform, V(2f, 2.05f, 0f), V(4f, 0.1f, 0.1f), null).GetComponent<UnityEngine.Renderer>().sharedMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/PaintedMetal006_1.0x1.0.mat");
    var door = hinge.AddComponent<Door>(); var so = new UnityEditor.SerializedObject(door);
    so.FindProperty("prompt").stringValue = "Open the gate"; so.FindProperty("tuning").objectReferenceValue = tuning; so.FindProperty("panel").objectReferenceValue = col; so.ApplyModifiedPropertiesWithoutUndo();
}
Place("Chain_Link_Fence_Post", gate.transform, V(0f, 0f, -4f), 90f, collider: false, rename: "GatePost_S");
Place("Chain_Link_Fence_Post", gate.transform, V(0f, 0f, 4f), 90f, collider: false, rename: "GatePost_N");

// Office: 8 x 6 m plank building, door on the west wall facing the road, counter inside.
float ox = 372f, oz = 166f, oy = H(ox, oz) + 0.1f;
var office = new UnityEngine.GameObject("Office"); office.transform.SetParent(E, false); office.transform.position = V(ox, oy, oz);
var O = office.transform;
for (int i = -1; i <= 2; i++) for (int j = -1; j <= 1; j++) Place("CITW_Floor", O, V(i * 2f - 1f, 0f, j * 2f), 0f);
// South wall z -3 (four modules at x -3,-1,1,3), north wall z +3
Place("CITW_Plank_Wall", O, V(-3f, 0f, -3f), 0f); Place("CITW_Plank_Window_Wall", O, V(-1f, 0f, -3f), 0f); Place("CITW_Plank_Wall", O, V(1f, 0f, -3f), 0f); Place("CITW_Plank_Window_Wall", O, V(3f, 0f, -3f), 0f);
Place("CITW_Plank_Window_Wall", O, V(-3f, 0f, 3f), 180f); Place("CITW_Plank_Wall", O, V(-1f, 0f, 3f), 180f); Place("CITW_Plank_Wall", O, V(1f, 0f, 3f), 180f); Place("CITW_Plank_Wall", O, V(3f, 0f, 3f), 180f);
// East wall x +4 (three modules), west wall x -4 with the doorway in the middle facing the road
Place("CITW_Plank_Wall", O, V(4f, 0f, -2f), 90f); Place("CITW_Plank_Window_Wall", O, V(4f, 0f, 0f), 90f); Place("CITW_Plank_Wall", O, V(4f, 0f, 2f), 90f);
Place("CITW_Plank_Window_Wall", O, V(-4f, 0f, -2f), -90f); Place("CITW_Plank_Doorway", O, V(-4f, 0f, 0f), -90f, collider: false); Place("CITW_Plank_Wall", O, V(-4f, 0f, 2f), -90f);
Box("DoorJambS", O, V(-4f, 1.5f, -0.75f), V(0.3f, 3f, 0.5f), null).GetComponent<UnityEngine.Renderer>().enabled = false;
Box("DoorJambN", O, V(-4f, 1.5f, 0.75f), V(0.3f, 3f, 0.5f), null).GetComponent<UnityEngine.Renderer>().enabled = false;
Box("DoorLintel", O, V(-4f, 2.55f, 0f), V(0.3f, 0.9f, 1f), null).GetComponent<UnityEngine.Renderer>().enabled = false;
Place("CITW_Door_Frame", O, V(-4f, 0f, 0f), -90f, collider: false);
MakeDoor(O, V(-4f, 0f, 0.5f), -90f, "Open");
// Roof: two slabs with a ridge along x, plank gables, overhang.
float pitch = UnityEngine.Mathf.Atan2(1.6f, 3.4f) * UnityEngine.Mathf.Rad2Deg; float slabLen = UnityEngine.Mathf.Sqrt(3.4f * 3.4f + 1.6f * 1.6f);
Box("Roof_S", O, V(0f, 3.8f, -1.7f), V(9.0f, 0.15f, slabLen), planks, V(-pitch, 0f, 0f));
Box("Roof_N", O, V(0f, 3.8f, 1.7f), V(9.0f, 0.15f, slabLen), planks, V(pitch, 0f, 0f));
Box("RidgeBeam", O, V(0f, 4.62f, 0f), V(9.2f, 0.2f, 0.2f), planks);
foreach (var gx in new[] { -4f, 4f })
{
    var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var g = pb.gameObject; g.name = "Gable"; g.transform.SetParent(O, false);
    var pos = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    for (int i = 0; i < pos.Count; i++) { var p = pos[i]; pos[i] = new UnityEngine.Vector3(p.x * 0.3f, p.y > 0f ? 1.6f : 0f, p.y > 0f ? 0f : p.z * 6.4f); }
    pb.positions = pos; pb.ToMesh(); pb.Refresh();
    g.transform.localPosition = V(gx, 3.0f, 0f); g.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
}
// Inside: counter facing the door, chair, shelf, lamp, papers.
Place("Checkout_Counter", O, V(0.2f, 0f, -0.6f), 90f, rename: "Counter");
Place("CITW_Chair", O, V(2.0f, 0f, -0.3f), -90f);
Place("CITW_Shelf", O, V(3.6f, 0f, 2.0f), -90f);
Place("CITW_Book_2", O, V(3.6f, 1.0f, 2.0f), 20f, collider: false);
Place("CITW_Oil_Lamp_2", O, V(1.4f, 0.95f, 0.3f), 0f, collider: false);
Place("CITW_Trunk_1", O, V(2.6f, 0f, -2.3f), 0f);
var lampGo = new UnityEngine.GameObject("OfficeLight"); lampGo.transform.SetParent(O, false); lampGo.transform.localPosition = V(0f, 2.5f, 0f);
var ol = lampGo.AddComponent<UnityEngine.Light>(); ol.type = UnityEngine.LightType.Point; ol.color = new UnityEngine.Color(1f, 0.8f, 0.55f); ol.intensity = 2.2f; ol.range = 8f;
// Office sign over the door.
var signBoard = Box("OfficeSign", O, V(-4.2f, 3.2f, 0f), V(0.06f, 0.5f, 2.6f), planks);
{
    var canvasGo = new UnityEngine.GameObject("Face", typeof(UnityEngine.RectTransform)); canvasGo.transform.SetParent(signBoard.transform, false);
    var canvas = canvasGo.AddComponent<UnityEngine.Canvas>(); canvas.renderMode = UnityEngine.RenderMode.WorldSpace;
    var rt = canvasGo.GetComponent<UnityEngine.RectTransform>(); rt.sizeDelta = new UnityEngine.Vector2(260f, 50f);
    rt.localScale = new UnityEngine.Vector3(0.01f / signBoard.transform.localScale.z, 0.01f / signBoard.transform.localScale.y, 1f);
    rt.localPosition = new UnityEngine.Vector3(-0.55f, 0f, 0f); rt.localRotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f);
    var t = canvasGo.AddComponent<UnityEngine.UI.Text>(); t.font = font; t.fontSize = 30; t.fontStyle = UnityEngine.FontStyle.Bold; t.alignment = UnityEngine.TextAnchor.MiddleCenter; t.color = new UnityEngine.Color(0.95f, 0.9f, 0.8f); t.text = "RANGER OFFICE";
}

// Parking: four cars in a row facing the office, one door open. Tints are project material copies.
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Materials/Cars")) UnityEditor.AssetDatabase.CreateFolder("Assets/Materials", "Cars");
var tints = new[] { new UnityEngine.Color(0.55f, 0.6f, 0.65f), new UnityEngine.Color(0.5f, 0.28f, 0.22f), new UnityEngine.Color(0.35f, 0.42f, 0.32f), new UnityEngine.Color(0.7f, 0.68f, 0.6f) };
var lot = new UnityEngine.GameObject("Parking"); lot.transform.SetParent(E, false);
for (int i = 0; i < 4; i++)
{
    float cx = 364f + i * 4.2f, cz = 190f;
    var car = Place("Vehicle_Body", lot.transform, V(cx, H(cx, cz), cz), 180f + (i == 2 ? 6f : 0f), rename: "Car_" + (i + 1));
    string mpath = "Assets/Materials/Cars/Car_Tint_" + (i + 1) + ".mat";
    var tint = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(mpath);
    foreach (var r in car.GetComponentsInChildren<UnityEngine.Renderer>())
    {
        var mats = r.sharedMaterials;
        for (int m = 0; m < mats.Length; m++)
        {
            if (mats[m] == null || mats[m].name != "M_Assets") continue;
            if (tint == null) { tint = new UnityEngine.Material(mats[m]); tint.SetColor("_BaseColor", tints[i]); UnityEditor.AssetDatabase.CreateAsset(tint, mpath); }
            mats[m] = tint;
        }
        r.sharedMaterials = mats;
    }
}
Place("Car_Door", lot.transform, V(365.2f, H(365f, 190f) + 1.64f, 187.6f), 150f, collider: false, rename: "OpenDoor");
// Street lights, bench, phone booth.
foreach (var (lx, lz, yaw) in new[] { (360f, 184f, 0f), (384f, 196f, 180f), (384f, 168f, 180f) })
{
    var sl = Place("Street_Light", E, V(lx, H(lx, lz), lz), yaw, rename: "StreetLight");
    var lg = new UnityEngine.GameObject("Bulb"); lg.transform.SetParent(sl.transform, false); lg.transform.localPosition = V(0f, 4.6f, 0.9f);
    var l = lg.AddComponent<UnityEngine.Light>(); l.type = UnityEngine.LightType.Point; l.color = new UnityEngine.Color(1f, 0.85f, 0.6f); l.intensity = 1.6f; l.range = 12f;
}
Place("Bench", E, V(366f, H(366f, 172f), 172f), 0f, rename: "Bench");
Place("Telephone_Booth", E, V(378f, H(378f, 172f), 172f), 180f, rename: "PhoneBooth");
Place("CITW_Barrel_3", E, V(381f, H(381f, 162f), 162f), 0f);

// Sign at the camp end of the road, and a dev warp.
var signs = UnityEngine.GameObject.Find("Signs").transform;
{
    float sx = 267f, sz = 181.5f; var readFrom = V(251.5f, 0f, 182.5f);
    var post = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); post.name = "Sign_Road"; post.transform.SetParent(signs, false);
    post.transform.position = V(sx, H(sx, sz) + 1.05f, sz); post.transform.localScale = V(0.14f, 1.05f, 0.14f); post.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
    var faceDir = readFrom - post.transform.position; faceDir.y = 0f; faceDir.Normalize();
    var board = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); board.name = "Board"; board.transform.SetParent(signs, false);
    board.transform.rotation = UnityEngine.Quaternion.LookRotation(UnityEngine.Vector3.Cross(faceDir, UnityEngine.Vector3.up), UnityEngine.Vector3.up);
    board.transform.position = V(sx, H(sx, sz) + 1.75f, sz) + faceDir * 0.09f; board.transform.localScale = V(0.04f, 0.34f, 1.15f);
    board.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks; UnityEngine.Object.DestroyImmediate(board.GetComponent<UnityEngine.Collider>());
    var canvasGo = new UnityEngine.GameObject("Face", typeof(UnityEngine.RectTransform)); canvasGo.transform.SetParent(board.transform, false);
    var canvas = canvasGo.AddComponent<UnityEngine.Canvas>(); canvas.renderMode = UnityEngine.RenderMode.WorldSpace;
    var rt = canvasGo.GetComponent<UnityEngine.RectTransform>(); rt.sizeDelta = new UnityEngine.Vector2(112f, 30f);
    rt.localScale = new UnityEngine.Vector3(0.01f / board.transform.localScale.z, 0.01f / board.transform.localScale.y, 1f);
    rt.localPosition = new UnityEngine.Vector3(0.55f, 0f, 0f); rt.localRotation = UnityEngine.Quaternion.Euler(0f, -90f, 0f);
    var t = canvasGo.AddComponent<UnityEngine.UI.Text>(); t.font = font; t.fontSize = 13; t.fontStyle = UnityEngine.FontStyle.Bold; t.alignment = UnityEngine.TextAnchor.MiddleCenter; t.color = new UnityEngine.Color(0.95f, 0.9f, 0.8f); t.text = "OFFICE";
}
var warps = UnityEngine.GameObject.Find("DevWarps").transform;
var warp = new UnityEngine.GameObject("Office"); warp.transform.SetParent(warps, false); warp.transform.position = V(350f, H(350f, 174f) + 0.2f, 174f); warp.transform.rotation = UnityEngine.Quaternion.Euler(0f, 90f, 0f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " fencePanels=" + panels + " treesCut=" + cut + " entranceObjects=" + E.GetComponentsInChildren<UnityEngine.Transform>().Length + " officeY=" + oy.ToString("F1");
