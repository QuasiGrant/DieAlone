if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData; float size = data.size.x;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));

// ---- Sign: the trailhead board moves to the mouth of path A, 2.4 m off the centre line, facing the fire pit.
var signs = UnityEngine.GameObject.Find("Signs").transform;
var post = signs.Find("Sign_TrailA");
UnityEngine.Transform board = null; foreach (UnityEngine.Transform c in signs) if (c.name == "Board" && UnityEngine.Vector3.Distance(c.position, post.position) < 1.5f) board = c;
float sx = 239.6f, sz = 186.8f;
var readFrom = V(251.5f, 0f, 182.5f);
post.position = V(sx, H(sx, sz) + 1.05f, sz);
var faceDir = readFrom - V(sx, 0f, sz); faceDir.y = 0f; faceDir.Normalize();
board.rotation = UnityEngine.Quaternion.LookRotation(UnityEngine.Vector3.Cross(faceDir, UnityEngine.Vector3.up), UnityEngine.Vector3.up);
board.position = V(sx, H(sx, sz) + 1.75f, sz) + faceDir * 0.09f;

// ---- Moss layer: green ground everywhere that is not trail, clearing or rock.
var mossLayer = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainLayer>("Assets/Terrain/Layer_Moss.terrainlayer");
if (mossLayer == null)
{
    var tex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>("Assets/Textures/Ground054/Ground054_Color.jpg");
    mossLayer = new UnityEngine.TerrainLayer { diffuseTexture = tex, tileSize = new UnityEngine.Vector2(4f, 4f), diffuseRemapMax = new UnityEngine.Vector4(0.42f, 0.58f, 0.3f, 1f), diffuseRemapMin = new UnityEngine.Vector4(0.02f, 0.04f, 0.01f, 0f) };
    UnityEditor.AssetDatabase.CreateAsset(mossLayer, "Assets/Terrain/Layer_Moss.terrainlayer");
}
var layers = new System.Collections.Generic.List<UnityEngine.TerrainLayer>(data.terrainLayers);
if (!layers.Contains(mossLayer)) { layers.Add(mossLayer); data.terrainLayers = layers.ToArray(); }
int floorIdx = 0, trailIdx = 1, rockIdx = -1, mossIdx = layers.IndexOf(mossLayer);
for (int i = 0; i < layers.Count; i++) if (layers[i].name == "Layer_Rock") rockIdx = i;

var pathA = new UnityEngine.Vector2[] { new(58,323), new(75,315), new(110,300), new(130,270), new(115,240), new(140,215), new(175,225), new(200,200), new(225,195), new(238,185) };
var pathB = new UnityEngine.Vector2[] { new(255,168), new(265,145), new(270,130) };
var b1 = new UnityEngine.Vector2[] { new(270,130), new(295,128), new(312,124) };
var b2 = new UnityEngine.Vector2[] { new(270,130), new(258,108), new(246,88) };
var b3 = new UnityEngine.Vector2[] { new(270,130), new(290,100), new(312,75), new(322,60) };
var trails = new[] { pathA, pathB, b1, b2, b3 };
var clearings = new (UnityEngine.Vector2 c, float r)[] { (new(250,180), 17f), (new(312,124), 8f), (new(246,88), 8f), (new(322,60), 8f), (new(54,322), 15f), (new(45,336), 11f) };
float DistToPolyline(UnityEngine.Vector2 p, UnityEngine.Vector2[] pts)
{
    float best = float.MaxValue;
    for (int i = 0; i < pts.Length - 1; i++) { var a = pts[i]; var b = pts[i + 1]; var ab = b - a; float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / ab.sqrMagnitude); best = UnityEngine.Mathf.Min(best, UnityEngine.Vector2.Distance(p, a + ab * t)); }
    return best;
}
float TrailDist(UnityEngine.Vector2 p) { float d = float.MaxValue; foreach (var t in trails) d = UnityEngine.Mathf.Min(d, DistToPolyline(p, t)); return d; }
float ClearingWeight(UnityEngine.Vector2 p) { float w = 0f; foreach (var c in clearings) w = UnityEngine.Mathf.Max(w, 1f - UnityEngine.Mathf.Clamp01((UnityEngine.Vector2.Distance(p, c.c) - c.r) / 4f)); return w; }
int ares = data.alphamapResolution;
var alpha = data.GetAlphamaps(0, 0, ares, ares);
var outAlpha = new float[ares, ares, layers.Count];
long mossy = 0;
for (int zi = 0; zi < ares; zi++) for (int xi = 0; xi < ares; xi++)
{
    float x = (xi + 0.5f) * size / ares, z = (zi + 0.5f) * size / ares; var p = new UnityEngine.Vector2(x, z);
    float trail = alpha[zi, xi, trailIdx];
    float rock = rockIdx >= 0 ? alpha[zi, xi, rockIdx] : 0f;
    // Moss fades in over 3 m from the trail edge and the clearing edge; none west of the cliff line.
    float edge = UnityEngine.Mathf.Clamp01((TrailDist(p) - 1.3f) / 3f);
    float moss = x < 40f ? 0f : edge * (1f - ClearingWeight(p)) * (1f - trail - rock);
    moss = UnityEngine.Mathf.Clamp01(moss * (0.85f + 0.15f * UnityEngine.Mathf.PerlinNoise(x / 9f, z / 9f)));
    float floor = UnityEngine.Mathf.Max(0f, 1f - trail - rock - moss);
    for (int l = 0; l < layers.Count; l++) outAlpha[zi, xi, l] = 0f;
    outAlpha[zi, xi, floorIdx] = floor; outAlpha[zi, xi, trailIdx] = trail; if (rockIdx >= 0) outAlpha[zi, xi, rockIdx] = rock; outAlpha[zi, xi, mossIdx] = moss;
    if (moss > 0.5f) mossy++;
}
data.SetAlphamaps(0, 0, outAlpha);
// Ground cover tints lean green to sit on moss.
var protos = data.detailPrototypes;
for (int i = 0; i < protos.Length; i++) { if (protos[i].prototype.name.Contains("Mushroom")) continue; protos[i].healthyColor = new UnityEngine.Color(0.6f, 0.78f, 0.45f); protos[i].dryColor = new UnityEngine.Color(0.72f, 0.7f, 0.42f); }
data.detailPrototypes = protos;
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " layers=" + layers.Count + " mossCells=" + mossy + "/" + (ares * ares) + " sign=" + post.position.ToString("F1") + " boardFaces=" + faceDir.ToString("F2");
