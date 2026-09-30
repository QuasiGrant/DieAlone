// 8.14 prep (edit mode, read-only): how opaque a crest belt of owned BK Redwood trees really is. Builds a test belt in
// an unsaved preview scene (EditorSceneManager.NewPreviewScene; the open scene is not touched), renders it at Grant's
// 3840 x 1976 with the Main3 camera's FOV against a flat magenta background, and counts background pixels (gaps)
// in the band the F-1 rays cross: BandLow to BandHigh metres over the belt ground, over the belt's middle stretch.
// The camera sits BelowCrest metres under the crest per 150 m of distance and aims at the band middle, as the tower
// deck looks up at the W crest (Valley.md rev 8: land 7 m short). Belts: rows across a BeltDepth band, trees on a
// jittered grid at each Spacing, firs scaled to 18 to 24 m, a giant (Sequoia, 30 to 38 m) every GiantEvery trees.
// Each is measured without and with the look filter (Game camera type; its low resolution blurs small gaps shut).
// A pixel is a gap when magenta dominates it (green under a third of red and blue). Frames go to Temp/tree_cover_*.jpg. LODs pick themselves from the render (QualitySettings.lodBias applies). Returns one line per spacing and distance.
if (UnityEngine.Application.isPlaying) return "edit mode only";
const int W = 3840, H = 1976, Seed = 8014, GiantEvery = 12, GapHue = 3;
const float Fov = 60f, BeltLength = 120f, BeltDepth = 25f, BandLow = 2f, BandHigh = 20f, MeasureLength = 60f, BelowCrest = 9f;
float[] spacings = { 4f, 6f, 9f };
float[] distances = { 150f, 300f, 600f };
string dir = "Assets/BK/PureNature_Redwood/Models/Trees/";
string[] firs = { "RedFir/RedFir1/RedFir1.fbx", "RedFir/RedFir2/RedFir2.fbx", "RedFir/RedFir3/RedFir3.fbx", "RedFir/RedFir4/RedFir4.fbx", "RedFir/RedFir5/RedFir5.fbx", "RedFir/RedFir6/RedFir6.fbx", "RedFir/RedFir7/RedFir7.fbx", "RedFir/RedFir8/RedFir8.fbx" };
string[] giants = { "Sequoia/Sequoia1/Sequoia1.fbx", "Sequoia/Sequoia3/Sequoia3.fbx", "Sequoia/Sequoia4/Sequoia4.fbx" };
var magenta = new UnityEngine.Color(1f, 0f, 1f, 1f);
var sb = new System.Text.StringBuilder("gap share in the band " + BandLow + " to " + BandHigh + " m over the belt ground, belt " + BeltDepth + " m deep, at " + W + " x " + H + ", lodBias " + UnityEngine.QualitySettings.lodBias + "\n");

float Height(UnityEngine.GameObject g) { var b = new UnityEngine.Bounds(); bool f = true; foreach (var r in g.GetComponentsInChildren<UnityEngine.Renderer>()) { if (f) { b = r.bounds; f = false; } else b.Encapsulate(r.bounds); } return b.size.y; }
var rt = new UnityEngine.RenderTexture(W, H, 24, UnityEngine.RenderTextureFormat.ARGB32);
var tex = new UnityEngine.Texture2D(W, H, UnityEngine.TextureFormat.RGB24, false);
foreach (var spacing in spacings)
{
    var scene = UnityEditor.SceneManagement.EditorSceneManager.NewPreviewScene();
    try
    {
        var rng = new System.Random(Seed);
        var lightGo = new UnityEngine.GameObject("Sun"); UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(lightGo, scene);
        var light = lightGo.AddComponent<UnityEngine.Light>(); light.type = UnityEngine.LightType.Directional; lightGo.transform.rotation = UnityEngine.Quaternion.Euler(32f, 200f, 0f);
        int n = 0, trees = 0;
        for (float x = -BeltLength / 2f; x <= BeltLength / 2f; x += spacing)
            for (float z = 0f; z <= BeltDepth; z += spacing)
            {
                bool giant = ++n % GiantEvery == 0;
                var path = dir + (giant ? giants[rng.Next(giants.Length)] : firs[rng.Next(firs.Length)]);
                var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path);
                var t = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, scene);
                float want = giant ? 30f + (float)rng.NextDouble() * 8f : 18f + (float)rng.NextDouble() * 6f;
                t.transform.localScale = UnityEngine.Vector3.one * (want / Height(t));
                float j = spacing * 0.35f;
                t.transform.position = new UnityEngine.Vector3(x + ((float)rng.NextDouble() * 2f - 1f) * j, 0f, z + ((float)rng.NextDouble() * 2f - 1f) * j);
                t.transform.rotation = UnityEngine.Quaternion.Euler(0f, (float)rng.NextDouble() * 360f, 0f);
                trees++;
            }
        var camGo = new UnityEngine.GameObject("Cam"); UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(camGo, scene);
        var cam = camGo.AddComponent<UnityEngine.Camera>(); cam.scene = scene; cam.fieldOfView = Fov; cam.farClipPlane = 3500f; cam.nearClipPlane = 0.3f;
        cam.clearFlags = UnityEngine.CameraClearFlags.SolidColor; cam.backgroundColor = magenta; cam.targetTexture = rt;
        var line = new System.Text.StringBuilder("spacing " + spacing + " m (" + trees + " trees, " + (trees / (BeltLength * BeltDepth) * 1000f).ToString("F1") + " per 1000 m2):");
        foreach (var filtered in new[] { false, true }) foreach (var d in distances)
        {
            var eye = new UnityEngine.Vector3(0f, -BelowCrest * d / 150f, -d);
            var aim = new UnityEngine.Vector3(0f, (BandLow + BandHigh) * 0.5f, 0f);
            camGo.transform.position = eye; camGo.transform.rotation = UnityEngine.Quaternion.LookRotation(aim - eye);
            cam.cameraType = filtered ? UnityEngine.CameraType.Game : UnityEngine.CameraType.Preview;   // the look filter runs on Game cameras only
            cam.Render();
            UnityEngine.RenderTexture.active = rt; tex.ReadPixels(new UnityEngine.Rect(0, 0, W, H), 0, 0); tex.Apply(); UnityEngine.RenderTexture.active = null;
            var px = tex.GetPixels32();
            System.IO.File.WriteAllBytes(System.IO.Path.GetFullPath("Temp/tree_cover_" + spacing + "m_" + d + "m" + (filtered ? "_filter" : "") + ".jpg"), tex.EncodeToJPG(80));   // frames for a look
            // screen box of the band on the belt's front face
            var a = cam.WorldToScreenPoint(new UnityEngine.Vector3(-MeasureLength / 2f, BandLow, 0f)); var b = cam.WorldToScreenPoint(new UnityEngine.Vector3(MeasureLength / 2f, BandHigh, 0f));
            int x0 = UnityEngine.Mathf.Clamp((int)a.x, 0, W - 1), x1 = UnityEngine.Mathf.Clamp((int)b.x, 0, W - 1), y0 = UnityEngine.Mathf.Clamp((int)a.y, 0, H - 1), y1 = UnityEngine.Mathf.Clamp((int)b.y, 0, H - 1);
            int total = 0, gaps = 0, widestRun = 0;
            for (int y = y0; y <= y1; y++) { int run = 0; for (int x = x0; x <= x1; x++) { var c = px[y * W + x]; bool gap = c.g * GapHue < UnityEngine.Mathf.Min(c.r, c.b); total++; if (gap) { gaps++; run++; if (run > widestRun) widestRun = run; } else run = 0; } }
            float metresPerPx = MeasureLength / UnityEngine.Mathf.Max(1, x1 - x0);
            line.Append("  " + (filtered ? "filter " : "") + d + " m: gaps " + (100f * gaps / UnityEngine.Mathf.Max(1, total)).ToString("F1") + " pct, widest " + (widestRun * metresPerPx).ToString("F1") + " m (" + (y1 - y0) + " px tall)");
        }
        sb.Append(line + "\n");
    }
    finally { UnityEditor.SceneManagement.EditorSceneManager.ClosePreviewScene(scene); }
}
UnityEngine.Object.DestroyImmediate(rt); UnityEngine.Object.DestroyImmediate(tex);
return sb.ToString();
