if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first: " + scene.path;
var terrain = UnityEngine.Terrain.activeTerrain;
var data = terrain.terrainData;
if (data.treePrototypes.Length > 0) return "forest already placed";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
string packRoot = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";

// ---- Tree prefabs with colliders: variants of the pack prefabs under Assets/Prefabs/Forest.
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Prefabs/Forest")) UnityEditor.AssetDatabase.CreateFolder("Assets/Prefabs", "Forest");
UnityEngine.GameObject TreeVariant(string packName, float radius, float height)
{
    string outPath = "Assets/Prefabs/Forest/Tree_" + packName + ".prefab";
    var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(outPath);
    if (existing != null) return existing;
    var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + packName + ".prefab");
    if (src == null) throw new System.Exception("missing pack prefab " + packName);
    var inst = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src);
    var cap = inst.AddComponent<UnityEngine.CapsuleCollider>();
    cap.radius = radius; cap.height = height; cap.center = V(0f, height * 0.5f, 0f);
    var variant = UnityEditor.PrefabUtility.SaveAsPrefabAsset(inst, outPath);
    UnityEngine.Object.DestroyImmediate(inst);
    return variant;
}
var pine1 = TreeVariant("Pine1", 0.35f, 8f); var pine2 = TreeVariant("Pine2", 0.35f, 8f);
var pine3 = TreeVariant("Pine3", 0.35f, 8f); var pine5 = TreeVariant("Pine5", 0.35f, 8f);
var aspen = TreeVariant("Aspen1", 0.3f, 6f); var birch = TreeVariant("Birch1", 0.3f, 6f);
var protos = new[] { pine1, pine2, pine3, pine5, aspen, birch };
var treeProtos = new UnityEngine.TreePrototype[protos.Length];
for (int i = 0; i < protos.Length; i++) treeProtos[i] = new UnityEngine.TreePrototype { prefab = protos[i], bendFactor = 0f };
data.treePrototypes = treeProtos;

// ---- Keep-clear geometry from the layout (metres).
var pathA = new UnityEngine.Vector2[] { new(75,315), new(110,300), new(130,270), new(115,240), new(140,215), new(175,225), new(200,200), new(225,195), new(238,185) };
var pathB = new UnityEngine.Vector2[] { new(255,168), new(265,145), new(270,130) };
var b1 = new UnityEngine.Vector2[] { new(270,130), new(295,128), new(312,124) };
var b2 = new UnityEngine.Vector2[] { new(270,130), new(258,108), new(246,88) };
var b3 = new UnityEngine.Vector2[] { new(270,130), new(290,100), new(312,75), new(322,60) };
var trails = new[] { pathA, pathB, b1, b2, b3 };
var clearings = new (UnityEngine.Vector2 c, float r)[] { (new(250,180), 17f), (new(312,124), 8f), (new(246,88), 8f), (new(322,60), 8f), (new(66,323), 27f) };
float DistToPolyline(UnityEngine.Vector2 p, UnityEngine.Vector2[] pts)
{
    float best = float.MaxValue;
    for (int i = 0; i < pts.Length - 1; i++)
    {
        var a = pts[i]; var b = pts[i + 1]; var ab = b - a;
        float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 0.0001f));
        best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t));
    }
    return best;
}
float TrailDist(UnityEngine.Vector2 p) { float d = float.MaxValue; foreach (var t in trails) d = UnityEngine.Mathf.Min(d, DistToPolyline(p, t)); return d; }
bool InClearing(UnityEngine.Vector2 p) { foreach (var c in clearings) if (UnityEngine.Vector2.Distance(p, c.c) < c.r) return true; return false; }

// ---- Trees: jittered grid, mostly pines, some aspen and birch near the trails.
var rng = new System.Random(11);
var trees = new System.Collections.Generic.List<UnityEngine.TreeInstance>();
float size = data.size.x, step = 4.2f;
for (float gz = 6f; gz < size - 6f; gz += step)
    for (float gx = 46f; gx < size - 6f; gx += step)
    {
        float x = gx + (float)(rng.NextDouble() - 0.5) * 3.2f, z = gz + (float)(rng.NextDouble() - 0.5) * 3.2f;
        var p = new UnityEngine.Vector2(x, z);
        if (x < 45f || InClearing(p)) continue;
        float td = TrailDist(p);
        if (td < 3.3f) continue;
        double roll = rng.NextDouble();
        int proto; float hs, ws;
        if (td < 14f && roll < 0.22) { proto = roll < 0.13 ? 4 : 5; hs = 1.0f + (float)rng.NextDouble() * 0.35f; ws = hs; }
        else { proto = rng.Next(0, 4); hs = 1.15f + (float)rng.NextDouble() * 0.6f; ws = 0.95f + (float)rng.NextDouble() * 0.3f; }
        trees.Add(new UnityEngine.TreeInstance
        {
            position = new UnityEngine.Vector3(x / size, 0f, z / size),   // y is taken from the terrain
            prototypeIndex = proto, heightScale = hs, widthScale = ws,
            rotation = (float)(rng.NextDouble() * System.Math.PI * 2.0),
            color = UnityEngine.Color.Lerp(new UnityEngine.Color(0.8f, 0.8f, 0.8f), UnityEngine.Color.white, (float)rng.NextDouble()),
            lightmapColor = UnityEngine.Color.white
        });
    }
data.SetTreeInstances(trees.ToArray(), true);

// ---- Ground cover as terrain details: grass and fern along the trails and in the clearings.
var grass1 = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + "Grass1.prefab");
var grass2 = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + "Grass2.prefab");
var fern1 = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + "Fern1.prefab");
UnityEngine.DetailPrototype Detail(UnityEngine.GameObject prefab, float minH, float maxH)
    => new UnityEngine.DetailPrototype { prototype = prefab, usePrototypeMesh = true, renderMode = UnityEngine.DetailRenderMode.VertexLit, minHeight = minH, maxHeight = maxH, minWidth = minH, maxWidth = maxH, useInstancing = true, noiseSpread = 0.3f };
data.detailPrototypes = new[] { Detail(grass1, 1.4f, 2.2f), Detail(grass2, 1.4f, 2.2f), Detail(fern1, 1.2f, 1.8f) };
int dres = 512;
data.SetDetailResolution(dres, 32);
var g1 = new int[dres, dres]; var g2 = new int[dres, dres]; var f1 = new int[dres, dres];
for (int zi = 0; zi < dres; zi++)
    for (int xi = 0; xi < dres; xi++)
    {
        float x = (xi + 0.5f) * size / dres, z = (zi + 0.5f) * size / dres;
        if (x < 45f) continue;
        var p = new UnityEngine.Vector2(x, z);
        float td = TrailDist(p);
        bool band = td > 1.6f && td < 11f;
        bool clearing = InClearing(p) && !(UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(66f, 323f)) < 27f);
        if (!band && !clearing) continue;
        double r = rng.NextDouble();
        if (r < 0.45) g1[zi, xi] = 2; else if (r < 0.8) g2[zi, xi] = 2; else if (band) f1[zi, xi] = 1;
    }
data.SetDetailLayer(0, 0, 0, g1); data.SetDetailLayer(0, 0, 1, g2); data.SetDetailLayer(0, 0, 2, f1);
terrain.detailObjectDistance = 60f; terrain.detailObjectDensity = 1f; terrain.treeDistance = 400f; terrain.treeBillboardDistance = 400f;
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
UnityEngine.Physics.SyncTransforms();

// ---- Line of sight checks at eye height through the tree colliders.
bool Blocked(UnityEngine.Vector2 a, UnityEngine.Vector2 b)
{
    var pa = V(a.x, terrain.SampleHeight(V(a.x, 0, a.y)) + 1.6f, a.y); var pb = V(b.x, terrain.SampleHeight(V(b.x, 0, b.y)) + 1.6f, b.y);
    return UnityEngine.Physics.Linecast(pa, pb, ~0, UnityEngine.QueryTriggerInteraction.Ignore);
}
var camp = new UnityEngine.Vector2(250f, 180f);
var sb = new System.Text.StringBuilder("saved=" + saved + " trees=" + trees.Count);
sb.Append(" | sight blocked: camp-ward=" + Blocked(camp, new(66f, 323f)) + " camp-c1=" + Blocked(camp, new(312f, 124f)) + " camp-c2=" + Blocked(camp, new(246f, 88f)) + " camp-c3=" + Blocked(camp, new(322f, 60f)) + " c1-c2=" + Blocked(new(312f, 124f), new(246f, 88f)) + " c1-c3=" + Blocked(new(312f, 124f), new(322f, 60f)) + " ward-camp=" + Blocked(new(66f, 323f), camp));
return sb.ToString();
