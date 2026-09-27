if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
var data = terrain.terrainData;
int res = data.heightmapResolution; float sizeXZ = data.size.x, sizeY = data.size.y;
float baseH = 24f, cliffX = 40f, rise = 16f;
var wardC = new UnityEngine.Vector2(66f, 323f);
var clearings = new (UnityEngine.Vector2 c, float r)[] { (new(250,180), 18f), (new(312,124), 9f), (new(246,88), 9f), (new(322,60), 9f), (new(65,320), 26f) };
var pathA = new UnityEngine.Vector2[] { new(75,315), new(110,300), new(130,270), new(115,240), new(140,215), new(175,225), new(200,200), new(225,195), new(238,185) };
var pathB = new UnityEngine.Vector2[] { new(255,168), new(265,145), new(270,130) };
var b1 = new UnityEngine.Vector2[] { new(270,130), new(295,128), new(312,124) };
var b2 = new UnityEngine.Vector2[] { new(270,130), new(258,108), new(246,88) };
var b3 = new UnityEngine.Vector2[] { new(270,130), new(290,100), new(312,75), new(322,60) };
var trails = new[] { pathA, pathB, b1, b2, b3 };

// Old heights at the objects that sit on the raised ground, so they can be re-seated after.
var reseat = new System.Collections.Generic.List<(UnityEngine.Transform tr, float off)>();
foreach (var name in new[] { "Signs/Sign_Ward", "DevWarps/Ward", "Ward/Stone_1", "Ward/Stone_2", "Ward/Stone_3", "Ward/Stone_4", "Ward/Stone_5", "Ward/Stone_6" })
{ var go = UnityEngine.GameObject.Find(name); if (go != null) reseat.Add((go.transform, go.transform.position.y - terrain.SampleHeight(go.transform.position))); }
var signs = UnityEngine.GameObject.Find("Signs").transform;
foreach (UnityEngine.Transform c in signs) if (c.name == "Board" && c.position.x < 100f) reseat.Add((c, c.position.y - terrain.SampleHeight(c.position)));

float Plateau(UnityEngine.Vector2 p) { float d = UnityEngine.Vector2.Distance(p, wardC); return rise * UnityEngine.Mathf.SmoothStep(0f, 1f, 1f - UnityEngine.Mathf.Clamp01((d - 45f) / 85f)); }
(float dist, UnityEngine.Vector2 q) Nearest(UnityEngine.Vector2 p)
{
    float best = float.MaxValue; var bq = p;
    foreach (var pts in trails)
        for (int i = 0; i < pts.Length - 1; i++)
        {
            var a = pts[i]; var b = pts[i + 1]; var ab = b - a;
            float t = UnityEngine.Mathf.Clamp01(UnityEngine.Vector2.Dot(p - a, ab) / UnityEngine.Mathf.Max(ab.sqrMagnitude, 0.0001f));
            var q = a + ab * t; float d = UnityEngine.Vector2.Distance(p, q);
            if (d < best) { best = d; bq = q; }
        }
    return (best, bq);
}
var heights = new float[res, res];
for (int zi = 0; zi < res; zi++)
    for (int xi = 0; xi < res; xi++)
    {
        float x = xi * sizeXZ / (res - 1), z = zi * sizeXZ / (res - 1);
        var p = new UnityEngine.Vector2(x, z);
        float roll = (UnityEngine.Mathf.PerlinNoise(x / 55f + 3.1f, z / 55f + 7.7f) - 0.5f) * 3.0f
                   + (UnityEngine.Mathf.PerlinNoise(x / 14f + 1.3f, z / 14f + 2.9f) - 0.5f) * 0.6f;
        float h = baseH + Plateau(p) + roll;
        // Clearings: flat at the clearing centre's height.
        foreach (var c in clearings)
        {
            float w = 1f - UnityEngine.Mathf.Clamp01((UnityEngine.Vector2.Distance(p, c.c) - c.r) / 6f);
            if (w > 0f) h = UnityEngine.Mathf.Lerp(h, baseH + Plateau(c.c), w);
        }
        // Trails: flat across, following the slope along. 1.2 m flat each side, 2.5 m blend.
        var n = Nearest(p);
        float tw = 1f - UnityEngine.Mathf.Clamp01((n.dist - 1.2f) / 2.5f);
        if (tw > 0f) h = UnityEngine.Mathf.Lerp(h, baseH + Plateau(n.q), tw);
        if (x < cliffX)
        {
            float t = UnityEngine.Mathf.Clamp01((cliffX - x) / 14f);
            float valley = 1.5f + (UnityEngine.Mathf.PerlinNoise(x / 30f, z / 30f) - 0.5f) * 2f;
            h = UnityEngine.Mathf.Lerp(h, valley, UnityEngine.Mathf.SmoothStep(0f, 1f, t));
        }
        heights[zi, xi] = UnityEngine.Mathf.Clamp01(h / sizeY);
    }
data.SetHeights(0, 0, heights);

// Trail paint: narrower, about 1.4 m solid with a 0.6 m soft edge.
int ares = data.alphamapResolution;
var alpha = new float[ares, ares, 2];
for (int zi = 0; zi < ares; zi++)
    for (int xi = 0; xi < ares; xi++)
    {
        float x = (xi + 0.5f) * sizeXZ / ares, z = (zi + 0.5f) * sizeXZ / ares;
        float trail = 1f - UnityEngine.Mathf.Clamp01((Nearest(new UnityEngine.Vector2(x, z)).dist - 0.7f) / 0.6f);
        alpha[zi, xi, 0] = 1f - trail; alpha[zi, xi, 1] = trail;
    }
data.SetAlphamaps(0, 0, alpha);
UnityEngine.Physics.SyncTransforms();

foreach (var r in reseat) { var p = r.tr.position; r.tr.position = V(p.x, terrain.SampleHeight(p) + r.off, p.z); }
var wall = UnityEngine.GameObject.Find("Bounds/Wall_Cliff"); wall.transform.position = V(cliffX, 40f, 200f); wall.GetComponent<UnityEngine.BoxCollider>().size = V(1f, 60f, 400f);

UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var sb = new System.Text.StringBuilder("saved=" + saved + " reseated=" + reseat.Count);
sb.Append(" | H ward=" + H(66f,323f).ToString("F1") + " sign=" + H(82f,312f).ToString("F1") + " (110,300)=" + H(110f,300f).ToString("F1") + " (130,270)=" + H(130f,270f).ToString("F1") + " (115,240)=" + H(115f,240f).ToString("F1") + " (140,215)=" + H(140f,215f).ToString("F1") + " camp=" + H(250f,180f).ToString("F1") + " tower=" + H(228f,208f).ToString("F1") + " cabin=" + H(245f,190f).ToString("F1"));
// Steepest 1 m step along path A.
float steep = 0f; UnityEngine.Vector2 where = default;
for (int i = 0; i < pathA.Length - 1; i++) { var a = pathA[i]; var b = pathA[i + 1]; int n = (int)UnityEngine.Vector2.Distance(a, b); for (int k = 0; k < n; k++) { var p0 = UnityEngine.Vector2.Lerp(a, b, (float)k / n); var p1 = UnityEngine.Vector2.Lerp(a, b, (float)(k + 1) / n); float g = UnityEngine.Mathf.Abs(H(p1.x, p1.y) - H(p0.x, p0.y)) / UnityEngine.Vector2.Distance(p0, p1); if (g > steep) { steep = g; where = p0; } } }
sb.Append(" | steepest pathA grade=" + (steep * 100f).ToString("F0") + "% at " + where);
return sb.ToString();
