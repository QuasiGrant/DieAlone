if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main2.unity") return "open Main2 first: " + scene.path;
if (UnityEngine.GameObject.Find("Beats") != null) return "Beats already exist";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
var wardMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/WardStone_1.mat");
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games", "Assets/Celestia_Studio" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
    throw new System.Exception("no prefab named " + name);
}
UnityEngine.GameObject Place(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Quaternion rot, bool collider = true, string rename = null)
{
    var go = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find(name), scene);
    go.transform.SetParent(parent, false); go.transform.position = pos; go.transform.rotation = rot;
    if (rename != null) go.name = rename;
    if (collider && go.GetComponentInChildren<UnityEngine.Collider>() == null)
    {
        var rends = go.GetComponentsInChildren<UnityEngine.Renderer>();
        if (rends.Length > 0) { var b = rends[0].bounds; foreach (var r in rends) b.Encapsulate(r.bounds); var box = go.AddComponent<UnityEngine.BoxCollider>(); box.center = go.transform.InverseTransformPoint(b.center); var s = go.transform.InverseTransformVector(b.size); box.size = new UnityEngine.Vector3(UnityEngine.Mathf.Abs(s.x), UnityEngine.Mathf.Abs(s.y), UnityEngine.Mathf.Abs(s.z)); }
    }
    return go;
}
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 sz, UnityEngine.Material mat, UnityEngine.Quaternion rot)
{ var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); go.name = name; go.transform.SetParent(parent, false); go.transform.position = pos; go.transform.rotation = rot; go.transform.localScale = sz; if (mat != null) go.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat; return go; }
UnityEngine.Quaternion Yaw(float y) => UnityEngine.Quaternion.Euler(0f, y, 0f);
var beats = new UnityEngine.GameObject("Beats"); var B = beats.transform;
var sb = new System.Text.StringBuilder();

// ================= PATH A =================
// 1. Fallen trunk across the trail at the second bend, propped on a rock so the player has to crouch under it.
{
    var g = new UnityEngine.GameObject("FallenTrunk"); g.transform.SetParent(B, false);
    var a = new UnityEngine.Vector2(130f, 270f); var b = new UnityEngine.Vector2(115f, 240f); var mid = UnityEngine.Vector2.Lerp(a, b, 0.55f); var dir = (b - a).normalized; var across = new UnityEngine.Vector2(-dir.y, dir.x);
    float yaw = UnityEngine.Mathf.Atan2(across.x, across.y) * UnityEngine.Mathf.Rad2Deg;
    var trunk = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); trunk.name = "Trunk"; trunk.transform.SetParent(g.transform, false);
    var lowEnd = mid - across * 3.5f; var highEnd = mid + across * 3.5f; float yLow = H(lowEnd.x, lowEnd.y) + 0.35f, yHigh = H(highEnd.x, highEnd.y) + 2.2f;
    var pLow = V(lowEnd.x, yLow, lowEnd.y); var pHigh = V(highEnd.x, yHigh, highEnd.y);
    trunk.transform.position = (pLow + pHigh) * 0.5f; trunk.transform.rotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.up, (pHigh - pLow).normalized); trunk.transform.localScale = V(0.7f, UnityEngine.Vector3.Distance(pLow, pHigh) * 0.5f, 0.7f);
    trunk.GetComponent<UnityEngine.Renderer>().sharedMaterial = Find("CS_Log_Large_Long_Seat_1").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;
    var rock = Place("CS_Rock_5", g.transform, V(highEnd.x, H(highEnd.x, highEnd.y) - 0.2f, highEnd.y), Yaw(yaw + 30f), rename: "PropRock"); rock.transform.localScale = V(1.6f, 1.5f, 1.6f);
    Place("CS_Log_Large_Short", g.transform, V(lowEnd.x + across.x * 0.5f, H(lowEnd.x, lowEnd.y), lowEnd.y + across.y * 0.5f), Yaw(yaw), rename: "Stump");
    float clearanceAtCentre = (yLow + yHigh) * 0.5f - 0.35f - H(mid.x, mid.y); sb.Append("trunk clearance " + clearanceAtCentre.ToString("F2") + " m; ");
}
// 2. Hunting stand in the trees off the third bend, ladder down to the ground.
{
    var g = new UnityEngine.GameObject("HuntingStand"); g.transform.SetParent(B, false);
    float sx = 148f, sz = 221f, gy = H(sx, sz);
    foreach (var (dx, dz) in new[] { (-0.8f, -0.8f), (0.8f, -0.8f), (-0.8f, 0.8f), (0.8f, 0.8f) }) Box("Leg", g.transform, V(sx + dx, gy + 1.8f, sz + dz), V(0.18f, 3.6f, 0.18f), planks, Yaw(0f));
    Box("Platform", g.transform, V(sx, gy + 3.6f, sz), V(2.2f, 0.12f, 2.2f), planks, Yaw(0f));
    foreach (var (dx, dz, w, d) in new[] { (0f, 1.05f, 2.2f, 0.06f), (0f, -1.05f, 2.2f, 0.06f), (1.05f, 0f, 0.06f, 2.2f) }) Box("Rail", g.transform, V(sx + dx, gy + 4.1f, sz + dz), V(w, 0.9f, d), planks, Yaw(0f));
    var ladder = Place("CITW_Ladder", g.transform, V(sx - 1.15f, gy, sz), UnityEngine.Quaternion.AngleAxis(-12f, UnityEngine.Vector3.forward) * Yaw(90f), rename: "Ladder");
    Place("CS_Backpack_Old_2", g.transform, V(sx + 0.3f, gy + 3.66f, sz - 0.4f), Yaw(40f), collider: false, rename: "LeftPack");
}
// 3. Cultist trace halfway: a small ring of stones, candle stubs, one rune-marked stone, three metres off the trail.
{
    var g = new UnityEngine.GameObject("TrailShrine"); g.transform.SetParent(B, false);
    float cx = 136.5f, cz = 219.5f;
    for (int i = 0; i < 7; i++) { float a = i / 7f * 6.283f; Place("CS_Stone_" + (1 + i % 8), g.transform, V(cx + UnityEngine.Mathf.Cos(a) * 0.9f, H(cx, cz), cz + UnityEngine.Mathf.Sin(a) * 0.9f), Yaw(i * 51f), collider: false, rename: "RingStone"); }
    var rune = Box("RuneStone", g.transform, V(cx, H(cx, cz) + 0.45f, cz), V(0.5f, 0.9f, 0.35f), wardMat, UnityEngine.Quaternion.Euler(8f, 25f, -5f));
    Place("CITW_Candle_1", g.transform, V(cx + 0.45f, H(cx, cz), cz - 0.3f), Yaw(0f), collider: false, rename: "Candle");
    Place("CITW_Candle_2", g.transform, V(cx - 0.4f, H(cx, cz), cz + 0.35f), Yaw(0f), collider: false, rename: "Candle");
    Place("CITW_Candle_1", g.transform, V(cx + 0.1f, H(cx, cz), cz + 0.5f), Yaw(0f), collider: false, rename: "Candle");
}
// 4. Old signpost with a missing board near the (200,200) bend.
{
    var g = new UnityEngine.GameObject("OldSignpost"); g.transform.SetParent(B, false);
    float px = 197.5f, pz = 203.5f;
    var post = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); post.name = "Post"; post.transform.SetParent(g.transform, false);
    post.transform.position = V(px, H(px, pz) + 0.95f, pz); post.transform.rotation = UnityEngine.Quaternion.Euler(6f, 0f, -4f); post.transform.localScale = V(0.13f, 0.95f, 0.13f); post.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
    Box("BrokenBoard", g.transform, V(px + 0.6f, H(px, pz) + 0.08f, pz + 0.3f), V(0.9f, 0.04f, 0.3f), planks, UnityEngine.Quaternion.Euler(0f, 35f, 0f));
    Box("Bracket", g.transform, V(px + 0.2f, H(px, pz) + 1.7f, pz), V(0.35f, 0.08f, 0.06f), planks, Yaw(20f));
}
// ================= ROAD =================
// 5. Car pulled off the road with a door open, a hundred metres before the office.
{
    var g = new UnityEngine.GameObject("RoadsideCar"); g.transform.SetParent(B, false);
    float cx = 318f, cz = 179.5f; var car = Place("Vehicle_Body", g.transform, V(cx, H(cx, cz), cz), Yaw(110f), rename: "Car");
    var tint = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Cars/Car_Tint_2.mat");
    foreach (var r in car.GetComponentsInChildren<UnityEngine.Renderer>()) { var mats = r.sharedMaterials; for (int m = 0; m < mats.Length; m++) if (mats[m] != null && mats[m].name == "M_Assets") mats[m] = tint; r.sharedMaterials = mats; }
    Place("Car_Door", g.transform, car.transform.position + car.transform.TransformDirection(V(1.25f, 1.64f, 0.6f)), Yaw(110f + 70f), collider: false, rename: "OpenDoor");
    Place("CS_Backpack_Modern_3", g.transform, car.transform.position + car.transform.TransformDirection(V(2.4f, 0f, 0.3f)), Yaw(160f), rename: "DroppedPack");
}
// 6. Notice board at the camp end of the road with a drawn map of the trails.
{
    var g = new UnityEngine.GameObject("NoticeBoard"); g.transform.SetParent(B, false);
    float bx = 268.5f, bz = 174.5f; var faceDir = V(251.5f, 0f, 182.5f) - V(bx, 0f, bz); faceDir.y = 0f; faceDir.Normalize();
    var rot = UnityEngine.Quaternion.LookRotation(faceDir, UnityEngine.Vector3.up);
    foreach (var side in new[] { -0.9f, 0.9f }) { var leg = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); leg.name = "Leg"; leg.transform.SetParent(g.transform, false); leg.transform.position = V(bx, H(bx, bz) + 1.1f, bz) + rot * V(side, 0f, 0f); leg.transform.localScale = V(0.12f, 1.1f, 0.12f); leg.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks; }
    var board = Box("Board", g.transform, V(bx, H(bx, bz) + 1.75f, bz), V(2.0f, 1.1f, 0.06f), planks, rot);
    Box("Roof", g.transform, V(bx, H(bx, bz) + 2.4f, bz) + rot * V(0f, 0f, -0.1f), V(2.3f, 0.06f, 0.5f), planks, rot * UnityEngine.Quaternion.Euler(-12f, 0f, 0f));
    // Map texture drawn from the trail polylines.
    const int S = 256; var tex = new UnityEngine.Texture2D(S, S, UnityEngine.TextureFormat.RGB24, false); var px = new UnityEngine.Color32[S * S];
    for (int i = 0; i < px.Length; i++) px[i] = new UnityEngine.Color32(214, 196, 160, 255);
    void Dot(int x, int y, UnityEngine.Color32 c, int r) { for (int ox = -r; ox <= r; ox++) for (int oy = -r; oy <= r; oy++) { int X = x + ox, Y = y + oy; if (X >= 0 && Y >= 0 && X < S && Y < S) px[Y * S + X] = c; } }
    void Line(UnityEngine.Vector2 a, UnityEngine.Vector2 b, UnityEngine.Color32 c, int r) { int n = (int)(UnityEngine.Vector2.Distance(a, b) * 0.64f) + 1; for (int i = 0; i <= n; i++) { var p = UnityEngine.Vector2.Lerp(a, b, (float)i / n); Dot((int)(p.x * S / 400f), (int)(p.y * S / 400f), c, r); } }
    var ink = new UnityEngine.Color32(70, 50, 35, 255); var water = new UnityEngine.Color32(90, 120, 150, 255); var red = new UnityEngine.Color32(150, 30, 30, 255);
    var trails = new[] { new UnityEngine.Vector2[] { new(266,178), new(300,176), new(340,174), new(388,174) }, new UnityEngine.Vector2[] { new(255,168), new(265,145), new(270,130) }, new UnityEngine.Vector2[] { new(270,130), new(250,138), new(228,146), new(216,148) }, new UnityEngine.Vector2[] { new(270,130), new(258,108), new(246,88) }, new UnityEngine.Vector2[] { new(270,130), new(290,100), new(312,75), new(322,60) }, new UnityEngine.Vector2[] { new(258,108), new(246,106), new(236,104) }, new UnityEngine.Vector2[] { new(236,104), new(235.5f,95), new(238,87), new(243,82.5f), new(249,80) }, new UnityEngine.Vector2[] { new(249,80), new(262,73), new(280,65), new(300,60), new(314,62) }, new UnityEngine.Vector2[] { new(330,63), new(342,86), new(352,112), new(356,140), new(356,172) } };
    foreach (var t in trails) for (int i = 0; i < t.Length - 1; i++) Line(t[i], t[i + 1], ink, 1);
    var lakePts = new UnityEngine.Vector2[] { new(232,104), new(222,134), new(200,136), new(168,128), new(150,132), new(118,122), new(122,104), new(112,78), new(124,66), new(160,60), new(194,48), new(220,74), new(228,94) };
    for (int i = 0; i < lakePts.Length; i++) Line(lakePts[i], lakePts[(i + 1) % lakePts.Length], water, 2);
    foreach (var (x, z) in new[] { (250f, 180f), (208f, 147f), (247f, 84.5f), (325f, 56.5f), (372f, 166f) }) Dot((int)(x * S / 400f), (int)(z * S / 400f), red, 3);
    Line(new UnityEngine.Vector2(238, 185), new UnityEngine.Vector2(200, 200), red, 1);   // the closed trail, shown only as a stub
    tex.SetPixels32(px); tex.Apply();
    if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Textures/UI")) UnityEditor.AssetDatabase.CreateFolder("Assets/Textures", "UI");
    string mapPath = "Assets/Textures/UI/CampMap.png"; System.IO.File.WriteAllBytes(mapPath, tex.EncodeToPNG()); UnityEngine.Object.DestroyImmediate(tex);
    UnityEditor.AssetDatabase.ImportAsset(mapPath); var imp = UnityEditor.AssetImporter.GetAtPath(mapPath) as UnityEditor.TextureImporter; imp.filterMode = UnityEngine.FilterMode.Point; imp.mipmapEnabled = false; imp.SaveAndReimport();
    var mapMat = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Lit")); mapMat.SetTexture("_BaseMap", UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(mapPath)); mapMat.SetFloat("_Smoothness", 0.05f);
    UnityEditor.AssetDatabase.CreateAsset(mapMat, "Assets/Materials/CampMap.mat");
    var sheet = Box("MapSheet", g.transform, board.transform.position + rot * V(0f, -0.05f, -0.04f), V(1.0f, 0.8f, 0.01f), mapMat, rot);
    var canvasGo = new UnityEngine.GameObject("Title", typeof(UnityEngine.RectTransform)); canvasGo.transform.SetParent(board.transform, false);
    var canvas = canvasGo.AddComponent<UnityEngine.Canvas>(); canvas.renderMode = UnityEngine.RenderMode.WorldSpace;
    var rt = canvasGo.GetComponent<UnityEngine.RectTransform>(); rt.sizeDelta = new UnityEngine.Vector2(200f, 20f); rt.localScale = new UnityEngine.Vector3(0.01f / board.transform.localScale.x, 0.01f / board.transform.localScale.y, 1f); rt.localPosition = new UnityEngine.Vector3(0f, 0.42f, -0.6f); rt.localRotation = UnityEngine.Quaternion.Euler(0f, 180f, 0f);
    var t2 = canvasGo.AddComponent<UnityEngine.UI.Text>(); t2.font = font; t2.fontSize = 14; t2.fontStyle = UnityEngine.FontStyle.Bold; t2.alignment = UnityEngine.TextAnchor.MiddleCenter; t2.color = new UnityEngine.Color(0.95f, 0.9f, 0.8f); t2.text = "CAMPGROUND MAP";
    Place("CITW_Book_4", g.transform, V(bx, H(bx, bz), bz) + rot * V(0.6f, 0f, 0.4f), Yaw(30f), collider: false, rename: "DroppedLeaflet");
}
// ================= ARRIVAL =================
// 7. The player's own car: the one with the open door gets a pack, a jacket-sized blanket on the seat and a key on the ground.
{
    var lot = UnityEngine.GameObject.Find("Entrance/Parking").transform;
    var car = lot.Find("Car_1");
    if (car != null)
    {
        Place("CS_Backpack_Modern_1", B, car.position + car.TransformDirection(V(-1.8f, 0f, 1.6f)), Yaw(200f), rename: "PlayerPack");
        Place("CITW_Blanket", B, car.position + car.TransformDirection(V(0.4f, 0.75f, 0.3f)), Yaw(15f), collider: false, rename: "SeatBlanket");
        Place("Car_Key", B, car.position + car.TransformDirection(V(-1.6f, 0.02f, -0.4f)), Yaw(70f), collider: false, rename: "DroppedKey");
    }
}
// ================= CAVE SPUR BREADCRUMBS =================
foreach (var (x, z) in new[] { (309f, 195f), (313.5f, 226f), (304.5f, 250f) }) Place("CITW_Candle_2", B, V(x, H(x, z), z), Yaw(0f), collider: false, rename: "SpurCandle");
// ================= LANTERN POSTS at the fork and the trailheads (unlit for now) =================
foreach (var (x, z, name) in new[] { (268.5f, 133f, "Fork"), (256.5f, 173.5f, "TrailB"), (238f, 187.5f, "TrailA"), (265f, 180.5f, "Road") })
{
    var g = new UnityEngine.GameObject("LanternPost_" + name); g.transform.SetParent(B, false);
    var post = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); post.name = "Post"; post.transform.SetParent(g.transform, false); post.transform.position = V(x, H(x, z) + 0.9f, z); post.transform.localScale = V(0.1f, 0.9f, 0.1f); post.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
    Box("Arm", g.transform, V(x, H(x, z) + 1.85f, z + 0.15f), V(0.06f, 0.06f, 0.4f), planks, Yaw(0f));
    Place("CS_Lantern_Old_Rusted", g.transform, V(x, H(x, z) + 1.4f, z + 0.32f), Yaw(0f), collider: false, rename: "Lantern");
}
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " beats=" + B.childCount + " " + sb;
