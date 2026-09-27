if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData;
string packRoot = "Assets/suffercord/PSX Autumn Forest Asset Pack/Models/";
UnityEngine.GameObject Pack(string n) { var p = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(packRoot + n + ".prefab"); if (p == null) throw new System.Exception("missing " + n); return p; }

// ---- Detail material with GPU instancing on (the pack material has it off, so terrain details never drew).
string detailMatPath = "Assets/Materials/Foliage_Detail.mat";
var detailMat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(detailMatPath);
if (detailMat == null)
{
    var src = Pack("Grass1").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;
    detailMat = new UnityEngine.Material(src); detailMat.enableInstancing = true;
    UnityEditor.AssetDatabase.CreateAsset(detailMat, detailMatPath);
}
UnityEngine.GameObject DetailVariant(string packName)
{
    string outPath = "Assets/Prefabs/Forest/Detail_" + packName + ".prefab";
    var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(outPath);
    if (existing != null) return existing;
    var inst = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Pack(packName));
    foreach (var r in inst.GetComponentsInChildren<UnityEngine.Renderer>()) r.sharedMaterial = detailMat;
    var v = UnityEditor.PrefabUtility.SaveAsPrefabAsset(inst, outPath); UnityEngine.Object.DestroyImmediate(inst); return v;
}
var detailNames = new[] { "Grass1", "Grass2", "Grass3", "Fern1", "Fern2", "Nettle1" };
var detailProtos = new UnityEngine.DetailPrototype[detailNames.Length];
for (int i = 0; i < detailNames.Length; i++)
{
    bool fern = i >= 3;
    detailProtos[i] = new UnityEngine.DetailPrototype { prototype = DetailVariant(detailNames[i]), usePrototypeMesh = true, renderMode = UnityEngine.DetailRenderMode.VertexLit, minHeight = fern ? 1.0f : 1.3f, maxHeight = fern ? 1.8f : 2.4f, minWidth = fern ? 1.0f : 1.3f, maxWidth = fern ? 1.8f : 2.4f, useInstancing = true, noiseSpread = 0.4f, alignToGround = 0.3f };
}
data.detailPrototypes = detailProtos;

// ---- Tree prototypes: the six collider variants plus four pack bushes (no collider, walk-through).
var treeProtos = new System.Collections.Generic.List<UnityEngine.TreePrototype>();
foreach (var n in new[] { "Tree_Pine1", "Tree_Pine2", "Tree_Pine3", "Tree_Pine5", "Tree_Aspen1", "Tree_Birch1" })
    treeProtos.Add(new UnityEngine.TreePrototype { prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>("Assets/Prefabs/Forest/" + n + ".prefab"), bendFactor = 0f });
foreach (var n in new[] { "Bush1", "Bush2", "Bush3", "Bush4" }) treeProtos.Add(new UnityEngine.TreePrototype { prefab = Pack(n), bendFactor = 0f });
data.treePrototypes = treeProtos.ToArray();

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
bool InWard(UnityEngine.Vector2 p) => UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(66f, 323f)) < 27f;

// ---- Trees: 3 m grid with jitter, bigger pines, hugging the trail at 2.2 m. Bushes on a 5 m grid between them.
var rng = new System.Random(11);
var trees = new System.Collections.Generic.List<UnityEngine.TreeInstance>();
float size = data.size.x;
int bigTrees = 0, bushes = 0;
for (float gz = 5f; gz < size - 5f; gz += 3.0f)
    for (float gx = 45f; gx < size - 5f; gx += 3.0f)
    {
        float x = gx + (float)(rng.NextDouble() - 0.5) * 2.4f, z = gz + (float)(rng.NextDouble() - 0.5) * 2.4f;
        var p = new UnityEngine.Vector2(x, z);
        if (x < 45f || InClearing(p)) continue;
        float td = TrailDist(p);
        if (td < 2.2f) continue;
        double roll = rng.NextDouble();
        int proto; float hs, ws;
        if (td < 12f && roll < 0.2) { proto = roll < 0.12 ? 4 : 5; hs = 1.3f + (float)rng.NextDouble() * 0.5f; ws = hs; }
        else { proto = rng.Next(0, 4); hs = 1.6f + (float)rng.NextDouble() * 0.8f; ws = 1.0f + (float)rng.NextDouble() * 0.35f; }
        trees.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / size, 0f, z / size), prototypeIndex = proto, heightScale = hs, widthScale = ws, rotation = (float)(rng.NextDouble() * System.Math.PI * 2.0), color = UnityEngine.Color.Lerp(new UnityEngine.Color(0.75f, 0.75f, 0.75f), UnityEngine.Color.white, (float)rng.NextDouble()), lightmapColor = UnityEngine.Color.white });
        bigTrees++;
    }
for (float gz = 6f; gz < size - 6f; gz += 5.0f)
    for (float gx = 46f; gx < size - 6f; gx += 5.0f)
    {
        float x = gx + (float)(rng.NextDouble() - 0.5) * 4f, z = gz + (float)(rng.NextDouble() - 0.5) * 4f;
        var p = new UnityEngine.Vector2(x, z);
        if (x < 45f || InWard(p)) continue;
        float td = TrailDist(p);
        if (td < 1.6f) continue;
        if (InClearing(p) && rng.NextDouble() < 0.7) continue;
        float s = 0.9f + (float)rng.NextDouble() * 0.7f;
        trees.Add(new UnityEngine.TreeInstance { position = new UnityEngine.Vector3(x / size, 0f, z / size), prototypeIndex = 6 + rng.Next(0, 4), heightScale = s, widthScale = s, rotation = (float)(rng.NextDouble() * System.Math.PI * 2.0), color = UnityEngine.Color.Lerp(new UnityEngine.Color(0.7f, 0.7f, 0.7f), UnityEngine.Color.white, (float)rng.NextDouble()), lightmapColor = UnityEngine.Color.white });
        bushes++;
    }
data.SetTreeInstances(trees.ToArray(), true);

// ---- Ground cover everywhere in the forest, thinner in clearings, none on the trail core or the ward.
int dres = 512;
data.SetDetailResolution(dres, 32);
var layers = new int[detailNames.Length][,];
for (int i = 0; i < layers.Length; i++) layers[i] = new int[dres, dres];
long placed = 0;
for (int zi = 0; zi < dres; zi++)
    for (int xi = 0; xi < dres; xi++)
    {
        float x = (xi + 0.5f) * size / dres, z = (zi + 0.5f) * size / dres;
        if (x < 44f) continue;
        var p = new UnityEngine.Vector2(x, z);
        if (InWard(p)) continue;
        float td = TrailDist(p);
        if (td < 1.0f) continue;
        bool clearing = InClearing(p);
        double r = rng.NextDouble();
        if (clearing && r > 0.35) continue;
        int count = td < 1.8f ? 1 : (clearing ? 1 : 1 + rng.Next(0, 2));
        int which; double k = rng.NextDouble();
        if (k < 0.3) which = 0; else if (k < 0.55) which = 1; else if (k < 0.75) which = 2; else if (k < 0.87) which = 3; else if (k < 0.95) which = 4; else which = 5;
        if (clearing && which >= 3) which = rng.Next(0, 3);
        layers[which][zi, xi] = count; placed += count;
    }
for (int i = 0; i < layers.Length; i++) data.SetDetailLayer(0, 0, i, layers[i]);
terrain.detailObjectDistance = 70f; terrain.detailObjectDensity = 1f; terrain.treeDistance = 400f; terrain.treeBillboardDistance = 400f;

UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " trees=" + bigTrees + " bushes=" + bushes + " grassInstances=" + placed + " detailMatInstancing=" + detailMat.enableInstancing;
