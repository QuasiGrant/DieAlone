if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("Lake") != null) return "Lake already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x, sizeY = data.size.y;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games", "Assets/Celestia_Studio" }))
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

// ---- Shoreline: the map's bezier outline sampled into a polygon, world metres.
var ctrl = new UnityEngine.Vector2[] {
    new(232,104), new(236,120), new(222,134), new(200,136),
    new(178,138), new(168,128), new(150,132),
    new(130,136), new(118,122), new(122,104),
    new(124,90), new(112,78), new(124,66),
    new(138,52), new(160,60), new(176,54),
    new(194,48), new(214,58), new(220,74),
    new(224,86), new(228,94), new(232,104) };
var poly = new System.Collections.Generic.List<UnityEngine.Vector2>();
for (int seg = 0; seg < 7; seg++)
{
    var p0 = ctrl[seg * 3]; var p1 = ctrl[seg * 3 + 1]; var p2 = ctrl[seg * 3 + 2]; var p3 = ctrl[seg * 3 + 3];
    for (int k = 0; k < 12; k++) { float t = k / 12f, u = 1f - t; poly.Add(u * u * u * p0 + 3f * u * u * t * p1 + 3f * u * t * t * p2 + t * t * t * p3); }
}
bool Inside(UnityEngine.Vector2 p) { bool inside = false; for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++) { if ((poly[i].y > p.y) != (poly[j].y > p.y) && p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x) inside = !inside; } return inside; }
float EdgeDist(UnityEngine.Vector2 p) { float best = float.MaxValue; for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++) { var a = poly[j]; var b = poly[i]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }
float Signed(UnityEngine.Vector2 p) => Inside(p) ? EdgeDist(p) : -EdgeDist(p);
float waterY = 23.25f;
// Spur trail from the campsite 2 branch to the shore.
var spur = new UnityEngine.Vector2[] { new(258,108), new(246,106), new(236,104) };
float SpurDist(UnityEngine.Vector2 p) { float best = float.MaxValue; for (int i = 0; i < spur.Length - 1; i++) { var a = spur[i]; var b = spur[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); } return best; }

// ---- Heights: bed slopes from the bank (24 m) down to 1.4 m below the water 12 m out, with a little noise.
int res = data.heightmapResolution; var heights = data.GetHeights(0, 0, res, res); int carved = 0;
for (int zi = 0; zi < res; zi++) for (int xi = 0; xi < res; xi++)
{
    float x = xi * size / (res - 1), z = zi * size / (res - 1); var p = new UnityEngine.Vector2(x, z);
    if (x < 100f || x > 250f || z < 36f || z > 150f) continue;
    float sd = Signed(p);
    float h = heights[zi, xi] * sizeY;
    if (sd > -8f)
    {
        float depth = UnityEngine.Mathf.SmoothStep(0f, 1f, UnityEngine.Mathf.Clamp01((sd + 1.5f) / 13.5f)) * 2.2f + (UnityEngine.Mathf.PerlinNoise(x / 7f, z / 7f) - 0.5f) * 0.25f;
        float bank = UnityEngine.Mathf.Clamp01((sd + 8f) / 8f);           // 0 far outside, 1 at the shoreline
        float target = 24f - depth;
        h = UnityEngine.Mathf.Lerp(h, target, bank); carved++;
    }
    float sw = 1f - UnityEngine.Mathf.Clamp01((SpurDist(p) - 1.2f) / 2.5f);
    if (sw > 0f && sd < -1f) h = UnityEngine.Mathf.Lerp(h, 24f, sw);
    heights[zi, xi] = UnityEngine.Mathf.Clamp01(h / sizeY);
}
data.SetHeights(0, 0, heights);
// ---- Paint: dirt on the bank strip and the spur, sand-ish dirt under water, no moss in the lake.
int ares = data.alphamapResolution; var alpha = data.GetAlphamaps(0, 0, ares, ares); int L = alpha.GetLength(2);
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    if (x < 100f || x > 262f || z < 36f || z > 150f) continue;
    float sd = Signed(p);
    float bankW = 1f - UnityEngine.Mathf.Clamp01((-sd - 2.5f) / 2f);      // 1 inside and up to 2.5 m out, fading by 4.5 m
    float trail = UnityEngine.Mathf.Max(alpha[zi, xi, 1], 1f - UnityEngine.Mathf.Clamp01((SpurDist(p) - 0.7f) / 0.6f), bankW);
    if (trail <= alpha[zi, xi, 1]) continue;
    float rock = L > 2 ? alpha[zi, xi, 2] : 0f;
    alpha[zi, xi, 1] = UnityEngine.Mathf.Min(1f - rock, trail);
    if (L > 3) alpha[zi, xi, 3] = UnityEngine.Mathf.Min(alpha[zi, xi, 3], 1f - trail - rock);
    alpha[zi, xi, 0] = UnityEngine.Mathf.Max(0f, 1f - alpha[zi, xi, 1] - rock - (L > 3 ? alpha[zi, xi, 3] : 0f));
}
data.SetAlphamaps(0, 0, alpha);
// ---- Trees and cover out of the water, the bank and the spur.
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int cut = 0;
foreach (var ti in data.treeInstances) { var p = new UnityEngine.Vector2(ti.position.x * size, ti.position.z * size); if (Signed(p) > -3.5f || SpurDist(p) < 2.4f || UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(234f, 104f)) < 7f) { cut++; continue; } keep.Add(ti); }
data.SetTreeInstances(keep.ToArray(), true);
int dres = data.detailResolution; var rng = new System.Random(71);
for (int layer = 0; layer < data.detailPrototypes.Length; layer++)
{
    var map = data.GetDetailLayer(0, 0, dres, dres, layer); bool changed = false;
    for (int zi = 0; zi < dres; zi++) for (int xi = 0; xi < dres; xi++)
    {
        var p = new UnityEngine.Vector2((xi + 0.5f) * size / dres, (zi + 0.5f) * size / dres);
        if (p.x < 100f || p.x > 262f || p.y < 36f || p.y > 150f) continue;
        float sd = Signed(p);
        if (sd > -1.0f || SpurDist(p) < 1.0f) { if (map[zi, xi] != 0) { map[zi, xi] = 0; changed = true; } }
        else if (layer == 1 && sd > -3.5f && rng.NextDouble() < 0.5) { map[zi, xi] = 2; changed = true; }   // tall grass as reeds on the bank
    }
    if (changed) data.SetDetailLayer(0, 0, layer, map);
}
UnityEngine.Physics.SyncTransforms();
UnityEditor.EditorUtility.SetDirty(data);

// ---- Lake object: water plane, wade limit colliders, dock, bench, rocks, warp, sign.
var lake = new UnityEngine.GameObject("Lake"); var Lk = lake.transform;
var shader = UnityEngine.Shader.Find("DieAlone/Water"); if (shader == null) throw new System.Exception("DieAlone/Water shader not found");
var waterMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Water.mat");
if (waterMat == null) { waterMat = new UnityEngine.Material(shader); UnityEditor.AssetDatabase.CreateAsset(waterMat, "Assets/Materials/Water.mat"); }
var water = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Plane); water.name = "Water"; water.transform.SetParent(Lk, false);
water.transform.position = V(174f, waterY, 93f); water.transform.localScale = V(13f, 1f, 9.4f);   // plane is 10 m, so 130 x 94 m
UnityEngine.Object.DestroyImmediate(water.GetComponent<UnityEngine.Collider>());
var wr = water.GetComponent<UnityEngine.Renderer>(); wr.sharedMaterial = waterMat; wr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; wr.receiveShadows = false;
// Wade limit: invisible wall following the shoreline 4 m out, so the player can get wet feet and no further.
var limit = new UnityEngine.GameObject("WadeLimit"); limit.transform.SetParent(Lk, false); int walls = 0;
var inner = new System.Collections.Generic.List<UnityEngine.Vector2>();
var centre = new UnityEngine.Vector2(175f, 95f);
foreach (var p in poly) { var d = (p - centre).normalized; inner.Add(p - d * 4f); }
for (int i = 0; i < inner.Count; i++)
{
    var a = inner[i]; var b = inner[(i + 1) % inner.Count]; var mid = (a + b) * 0.5f; float len = UnityEngine.Vector2.Distance(a, b);
    var w = new UnityEngine.GameObject("Wall"); w.transform.SetParent(limit.transform, false); w.transform.position = V(mid.x, waterY, mid.y);
    w.transform.rotation = UnityEngine.Quaternion.LookRotation(V(b.x - a.x, 0f, b.y - a.y), UnityEngine.Vector3.up);
    var c = w.AddComponent<UnityEngine.BoxCollider>(); c.size = V(0.3f, 4f, len + 0.4f); walls++;
}
// Dock: planks from the shore point west over the water, posts, bench at the end.
var dock = new UnityEngine.GameObject("Dock"); dock.transform.SetParent(Lk, false); dock.transform.position = V(232.5f, 23.85f, 104f); dock.transform.rotation = UnityEngine.Quaternion.Euler(0f, -90f, 0f);
var D = dock.transform;
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 sz, UnityEngine.Material mat)
{ var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); go.name = name; go.transform.SetParent(parent, false); go.transform.localPosition = pos; go.transform.localScale = sz; go.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat; return go; }
for (int i = 0; i < 14; i++) Box("Plank" + i, D, V(0f, 0f, 0.35f + i * 0.72f), V(2.2f, 0.08f, 0.62f), planks).GetComponent<UnityEngine.Collider>().enabled = false;
var deckCol = Box("DeckCollider", D, V(0f, -0.02f, 5.2f), V(2.2f, 0.12f, 10.4f), null); deckCol.GetComponent<UnityEngine.Renderer>().enabled = false;
Box("Stringer_L", D, V(-0.95f, -0.12f, 5.2f), V(0.15f, 0.18f, 10.4f), planks).GetComponent<UnityEngine.Collider>().enabled = false;
Box("Stringer_R", D, V(0.95f, -0.12f, 5.2f), V(0.15f, 0.18f, 10.4f), planks).GetComponent<UnityEngine.Collider>().enabled = false;
for (int i = 0; i < 4; i++) foreach (var sx in new[] { -0.95f, 0.95f })
{
    var post = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); post.name = "Post"; post.transform.SetParent(D, false);
    post.transform.localPosition = V(sx, -1.1f, 0.6f + i * 3.2f); post.transform.localScale = V(0.18f, 1.3f, 0.18f); post.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks; post.GetComponent<UnityEngine.Collider>().enabled = false;
}
Box("Rail_L", D, V(-1.0f, 0.5f, 5.2f), V(0.08f, 0.08f, 10.0f), planks).GetComponent<UnityEngine.Collider>().enabled = false;
for (int i = 0; i < 5; i++) Box("RailPost" + i, D, V(-1.0f, 0.25f, 0.6f + i * 2.4f), V(0.08f, 0.5f, 0.08f), planks).GetComponent<UnityEngine.Collider>().enabled = false;
var bench = Place("Bench", D, D.TransformPoint(V(0.3f, 0.04f, 9.3f)), 0f, rename: "DockBench"); bench.transform.rotation = D.rotation * UnityEngine.Quaternion.Euler(0f, 90f, 0f);
// Shore rocks and a couple of props.
int rocks = 0;
foreach (var (rx, rz) in new[] { (226f, 112f), (218f, 124f), (236f, 96f), (228f, 88f), (150f, 134f), (121f, 100f), (128f, 66f), (180f, 52f), (212f, 58f), (236f, 118f) })
{ var r = Place("CS_Rock_" + (1 + rocks % 8), Lk, V(rx, H(rx, rz) - 0.1f, rz), (float)(rocks * 47 % 360), rename: "ShoreRock"); r.transform.localScale = V(1.4f, 1.2f, 1.4f); rocks++; }
Place("CS_Backpack_Old_1", Lk, V(233.6f, H(233.6f, 101.5f), 101.5f), 120f, collider: false, rename: "ShorePack");
Place("CS_Lantern_Old_Rusted", D, D.TransformPoint(V(-0.7f, 0.04f, 8.6f)), 0f, collider: false, rename: "DockLantern");
// Warp and sign.
var warps = UnityEngine.GameObject.Find("DevWarps").transform;
var warp = new UnityEngine.GameObject("Lake"); warp.transform.SetParent(warps, false); warp.transform.position = V(240f, H(240f, 105f) + 0.2f, 105f); warp.transform.rotation = UnityEngine.Quaternion.Euler(0f, -90f, 0f);
var signs = UnityEngine.GameObject.Find("Signs").transform;
{
    float sx = 257.5f, sz = 110.5f; var readFrom = V(268f, 0f, 128f);
    var post = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder); post.name = "Sign_Lake"; post.transform.SetParent(signs, false);
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
    var t = canvasGo.AddComponent<UnityEngine.UI.Text>(); t.font = font; t.fontSize = 13; t.fontStyle = UnityEngine.FontStyle.Bold; t.alignment = UnityEngine.TextAnchor.MiddleCenter; t.color = new UnityEngine.Color(0.95f, 0.9f, 0.8f); t.text = "LAKE";
}
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " polyPts=" + poly.Count + " carvedCells=" + carved + " treesCut=" + cut + " wadeWalls=" + walls + " rocks=" + rocks + " bed@centre=" + H(175f, 95f).ToString("F1") + " shore@(230,104)=" + H(230f, 104f).ToString("F1") + " water=" + waterY;
