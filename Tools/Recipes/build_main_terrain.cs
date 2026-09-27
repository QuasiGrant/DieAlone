if (UnityEngine.Application.isPlaying) return "stop play mode first";
if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty) return "active scene dirty, save first";
const string scenePath = "Assets/Scenes/Main.unity";
if (System.IO.File.Exists(scenePath)) return "Main.unity already exists";
if (!NewSceneMenu.CreateScene(scenePath)) return "recipe failed";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != scenePath) return "wrong scene after recipe: " + scene.path;
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));

// ---- Remove the recipe's starter ground; the terrain replaces it.
var starter = UnityEngine.GameObject.Find("Ground");
if (starter != null) UnityEngine.Object.DestroyImmediate(starter);

// ---- Terrain data asset
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Terrain")) UnityEditor.AssetDatabase.CreateFolder("Assets", "Terrain");
const int res = 513;
float sizeXZ = 400f, sizeY = 60f;
var data = new UnityEngine.TerrainData();
data.heightmapResolution = res;
data.size = V(sizeXZ, sizeY, sizeXZ);
data.alphamapResolution = 512;
data.baseMapResolution = 512;
UnityEditor.AssetDatabase.CreateAsset(data, "Assets/Terrain/Main_TerrainData.asset");

// ---- Map features in metres (x east, z north), from Docs/Layout/Main_layout.svg
float baseH = 24f;
float cliffX = 40f;
var pathA = new UnityEngine.Vector2[] { new(75,315), new(110,300), new(130,270), new(115,240), new(140,215), new(175,225), new(200,200), new(225,195), new(238,185) };
var pathB = new UnityEngine.Vector2[] { new(255,168), new(265,145), new(270,130) };
var b1 = new UnityEngine.Vector2[] { new(270,130), new(295,128), new(312,124) };
var b2 = new UnityEngine.Vector2[] { new(270,130), new(258,108), new(246,88) };
var b3 = new UnityEngine.Vector2[] { new(270,130), new(290,100), new(312,75), new(322,60) };
var trails = new[] { pathA, pathB, b1, b2, b3 };
var clearings = new (UnityEngine.Vector2 c, float r)[] { (new(250,180), 18f), (new(312,124), 9f), (new(246,88), 9f), (new(322,60), 9f), (new(65,320), 26f) };

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
float ClearingWeight(UnityEngine.Vector2 p) { float w = 0f; foreach (var c in clearings) w = UnityEngine.Mathf.Max(w, 1f - UnityEngine.Mathf.Clamp01((UnityEngine.Vector2.Distance(p, c.c) - c.r) / 6f)); return w; }

// ---- Heights: gentle rolling forest floor, flat clearings and trails, cliff drop west of x 40.
var heights = new float[res, res];
for (int zi = 0; zi < res; zi++)
{
    for (int xi = 0; xi < res; xi++)
    {
        float x = xi * sizeXZ / (res - 1), z = zi * sizeXZ / (res - 1);
        var p = new UnityEngine.Vector2(x, z);
        float roll = (UnityEngine.Mathf.PerlinNoise(x / 55f + 3.1f, z / 55f + 7.7f) - 0.5f) * 3.0f
                   + (UnityEngine.Mathf.PerlinNoise(x / 14f + 1.3f, z / 14f + 2.9f) - 0.5f) * 0.6f;
        float h = baseH + roll;
        float flat = UnityEngine.Mathf.Max(ClearingWeight(p), 1f - UnityEngine.Mathf.Clamp01((TrailDist(p) - 2f) / 4f));
        h = UnityEngine.Mathf.Lerp(h, baseH, flat);
        if (x < cliffX)
        {
            float t = UnityEngine.Mathf.Clamp01((cliffX - x) / 14f);          // 14 m of cliff face
            float valley = 1.5f + (UnityEngine.Mathf.PerlinNoise(x / 30f, z / 30f) - 0.5f) * 2f;
            h = UnityEngine.Mathf.Lerp(h, valley, UnityEngine.Mathf.SmoothStep(0f, 1f, t));
        }
        heights[zi, xi] = UnityEngine.Mathf.Clamp01(h / sizeY);
    }
}
data.SetHeights(0, 0, heights);

// ---- Layers: forest floor dirt, and the same dirt darkened for trails.
var dirtTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>("Assets/Textures/Ground054/Ground054_Color.jpg");
var floorLayer = new UnityEngine.TerrainLayer { diffuseTexture = dirtTex, tileSize = new UnityEngine.Vector2(4f, 4f) };
var trailLayer = new UnityEngine.TerrainLayer { diffuseTexture = dirtTex, tileSize = new UnityEngine.Vector2(2f, 2f), diffuseRemapMax = new UnityEngine.Vector4(0.55f, 0.5f, 0.45f, 1f) };
UnityEditor.AssetDatabase.CreateAsset(floorLayer, "Assets/Terrain/Layer_ForestFloor.terrainlayer");
UnityEditor.AssetDatabase.CreateAsset(trailLayer, "Assets/Terrain/Layer_Trail.terrainlayer");
data.terrainLayers = new[] { floorLayer, trailLayer };
int ares = data.alphamapResolution;
var alpha = new float[ares, ares, 2];
for (int zi = 0; zi < ares; zi++)
    for (int xi = 0; xi < ares; xi++)
    {
        float x = (xi + 0.5f) * sizeXZ / ares, z = (zi + 0.5f) * sizeXZ / ares;
        float trail = 1f - UnityEngine.Mathf.Clamp01((TrailDist(new UnityEngine.Vector2(x, z)) - 1.2f) / 0.8f);   // 3 m wide, soft edge
        alpha[zi, xi, 0] = 1f - trail; alpha[zi, xi, 1] = trail;
    }
data.SetAlphamaps(0, 0, alpha);

var terrainGo = UnityEngine.Terrain.CreateTerrainGameObject(data);
terrainGo.name = "Terrain";
terrainGo.transform.position = UnityEngine.Vector3.zero;

// ---- Bounds: invisible walls. Cliff wall along x 40 keeps the player off the drop.
var bounds = new UnityEngine.GameObject("Bounds");
UnityEngine.GameObject Wall(string name, UnityEngine.Vector3 center, UnityEngine.Vector3 size)
{
    var w = new UnityEngine.GameObject(name); w.transform.SetParent(bounds.transform, false);
    w.transform.position = center; var c = w.AddComponent<UnityEngine.BoxCollider>(); c.size = size; return w;
}
Wall("Wall_Cliff", V(cliffX, 30f, 200f), V(1f, 40f, 400f));
Wall("Wall_East", V(400f, 30f, 200f), V(1f, 40f, 400f));
Wall("Wall_North", V(220f, 30f, 400f), V(360f, 40f, 1f));
Wall("Wall_South", V(220f, 30f, 0f), V(360f, 40f, 1f));

// ---- Spawn at the cabin spot, facing the fire pit.
var player = UnityEngine.GameObject.Find("Player");
var terrain = terrainGo.GetComponent<UnityEngine.Terrain>();
float spawnY = terrain.SampleHeight(V(245f, 0f, 190f)) + 0.1f;
player.transform.position = V(245f, spawnY, 190f);
player.transform.rotation = UnityEngine.Quaternion.Euler(0f, 135f, 0f);

// ---- Sunset by default, night rig kept as the switch.
var lighting = UnityEngine.GameObject.Find("NightLighting");
var moon = lighting.transform.Find("Moon").gameObject; var sun = lighting.transform.Find("Sun").gameObject;
moon.SetActive(false); sun.SetActive(true);
var sunLight = sun.GetComponent<UnityEngine.Light>();
sunLight.color = new UnityEngine.Color(1f, 0.55f, 0.3f); sunLight.intensity = 1.2f; sunLight.shadows = UnityEngine.LightShadows.Soft;
sun.transform.rotation = UnityEngine.Quaternion.Euler(9f, 250f, 0f);   // low in the west-south-west
UnityEngine.RenderSettings.sun = sunLight;
UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
UnityEngine.RenderSettings.ambientLight = new UnityEngine.Color(0.32f, 0.22f, 0.2f);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var sb = new System.Text.StringBuilder("saved=" + saved + " roots=" + scene.rootCount);
sb.Append(" | heights: spawn=" + terrain.SampleHeight(V(245f, 0f, 190f)).ToString("F1") + " ward=" + terrain.SampleHeight(V(65f, 0f, 320f)).ToString("F1") + " valley=" + terrain.SampleHeight(V(15f, 0f, 200f)).ToString("F1") + " cliffMid=" + terrain.SampleHeight(V(33f, 0f, 200f)).ToString("F1") + " campsite3=" + terrain.SampleHeight(V(322f, 0f, 60f)).ToString("F1"));
sb.Append(" | build list: "); foreach (var s in UnityEditor.EditorBuildSettings.scenes) sb.Append(System.IO.Path.GetFileNameWithoutExtension(s.path) + " ");
return sb.ToString();
