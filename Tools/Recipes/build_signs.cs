if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
if (UnityEngine.GameObject.Find("Signs") != null) return "Signs already exist";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");

// ---- Tent icon, drawn in code: a triangle outline with a door slit. Saved as a sprite.
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Textures/UI")) UnityEditor.AssetDatabase.CreateFolder("Assets/Textures", "UI");
const int S = 64;
var tex = new UnityEngine.Texture2D(S, S, UnityEngine.TextureFormat.RGBA32, false);
var px = new UnityEngine.Color32[S * S]; for (int i = 0; i < px.Length; i++) px[i] = new UnityEngine.Color32(0, 0, 0, 0);
void Dot(int x, int y) { for (int ox = -2; ox <= 2; ox++) for (int oy = -2; oy <= 2; oy++) { int X = x + ox, Y = y + oy; if (X >= 0 && Y >= 0 && X < S && Y < S) px[Y * S + X] = new UnityEngine.Color32(255, 255, 255, 255); } }
void Line(int x0, int y0, int x1, int y1) { int n = System.Math.Max(System.Math.Abs(x1 - x0), System.Math.Abs(y1 - y0)); for (int i = 0; i <= n; i++) Dot(x0 + (x1 - x0) * i / n, y0 + (y1 - y0) * i / n); }
Line(6, 8, 32, 56); Line(32, 56, 58, 8); Line(6, 8, 58, 8);      // tent outline
Line(32, 8, 32, 34); Line(24, 8, 32, 34); Line(40, 8, 32, 34);    // door flap
tex.SetPixels32(px); tex.Apply();
string iconPath = "Assets/Textures/UI/TentIcon.png";
System.IO.File.WriteAllBytes(iconPath, tex.EncodeToPNG()); UnityEngine.Object.DestroyImmediate(tex);
UnityEditor.AssetDatabase.ImportAsset(iconPath);
var imp = UnityEditor.AssetImporter.GetAtPath(iconPath) as UnityEditor.TextureImporter;
imp.textureType = UnityEditor.TextureImporterType.Sprite; imp.spriteImportMode = UnityEditor.SpriteImportMode.Single; imp.filterMode = UnityEngine.FilterMode.Point; imp.mipmapEnabled = false; imp.SaveAndReimport();
var tentSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>(iconPath);
if (tentSprite == null) return "tent sprite failed to import";

var signMatDark = new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Lit"));
signMatDark.SetColor("_BaseColor", new UnityEngine.Color(0.45f, 0.08f, 0.06f)); signMatDark.SetFloat("_Smoothness", 0.1f);
UnityEditor.AssetDatabase.CreateAsset(signMatDark, "Assets/Materials/SignRed.mat");

var root = new UnityEngine.GameObject("Signs");

// A signpost: a post, and boards that each point along a world direction. Text faces back along
// the approach direction so the player reads it as they arrive.
UnityEngine.GameObject Post(string name, float x, float z)
{
    var post = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder);
    post.name = name; post.transform.SetParent(root.transform, false);
    post.transform.position = V(x, H(x, z) + 1.1f, z); post.transform.localScale = V(0.14f, 1.1f, 0.14f);
    post.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
    return post;
}
void Board(UnityEngine.GameObject post, float height, UnityEngine.Vector3 pointTo, UnityEngine.Vector3 readFrom, string text, bool tent, bool red)
{
    var origin = post.transform.position; origin.y = post.transform.position.y - 1.1f + height;
    var dir = pointTo - origin; dir.y = 0f; dir.Normalize();
    var board = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    board.name = "Board"; board.transform.SetParent(post.transform.parent, false);
    board.transform.position = origin + dir * 0.55f;
    board.transform.rotation = UnityEngine.Quaternion.LookRotation(UnityEngine.Vector3.Cross(dir, UnityEngine.Vector3.up), UnityEngine.Vector3.up);
    board.transform.localScale = V(0.04f, 0.3f, 1.05f);
    board.GetComponent<UnityEngine.Renderer>().sharedMaterial = red ? signMatDark : planks;
    UnityEngine.Object.DestroyImmediate(board.GetComponent<UnityEngine.Collider>());
    // Arrow tip: a small prism at the pointing end.
    var tip = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var tg = tip.gameObject; tg.name = "Tip"; tg.transform.SetParent(board.transform, false);
    var pos = new System.Collections.Generic.List<UnityEngine.Vector3>(tip.positions);
    for (int i = 0; i < pos.Count; i++) { var p = pos[i]; pos[i] = new UnityEngine.Vector3(p.x, p.z > 0f ? 0f : p.y, p.z); }   // collapse the far edge to a point
    tip.positions = pos; tip.ToMesh(); tip.Refresh();
    tg.transform.localPosition = V(0f, 0f, 0.62f); tg.transform.localScale = V(1f, 1f, 0.25f);
    tg.GetComponent<UnityEngine.Renderer>().sharedMaterial = board.GetComponent<UnityEngine.Renderer>().sharedMaterial;
    var tc = tg.GetComponent<UnityEngine.Collider>(); if (tc != null) UnityEngine.Object.DestroyImmediate(tc);
    // Face: world-space canvas on the side facing the reader.
    var faceDir = readFrom - board.transform.position; faceDir.y = 0f; faceDir.Normalize();
    bool front = UnityEngine.Vector3.Dot(faceDir, board.transform.right) > 0f;
    var canvasGo = new UnityEngine.GameObject("Face", typeof(UnityEngine.RectTransform));
    canvasGo.transform.SetParent(board.transform, false);
    var canvas = canvasGo.AddComponent<UnityEngine.Canvas>(); canvas.renderMode = UnityEngine.RenderMode.WorldSpace;
    var rt = canvasGo.GetComponent<UnityEngine.RectTransform>(); rt.sizeDelta = new UnityEngine.Vector2(100f, 26f);
    rt.localScale = new UnityEngine.Vector3(0.01f / board.transform.localScale.z, 0.01f / board.transform.localScale.y, 1f);
    rt.localPosition = new UnityEngine.Vector3(front ? 0.55f : -0.55f, 0f, 0f);
    rt.localRotation = UnityEngine.Quaternion.Euler(0f, front ? -90f : 90f, 0f);
    var row = new UnityEngine.GameObject("Row", typeof(UnityEngine.RectTransform)); row.transform.SetParent(canvasGo.transform, false);
    var rr = row.GetComponent<UnityEngine.RectTransform>(); rr.anchorMin = UnityEngine.Vector2.zero; rr.anchorMax = UnityEngine.Vector2.one; rr.offsetMin = new UnityEngine.Vector2(4f, 2f); rr.offsetMax = new UnityEngine.Vector2(-4f, -2f);
    var hl = row.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>(); hl.childAlignment = UnityEngine.TextAnchor.MiddleCenter; hl.spacing = 4f; hl.childControlWidth = true; hl.childControlHeight = true; hl.childForceExpandWidth = false; hl.childForceExpandHeight = true;
    if (tent)
    {
        var icon = new UnityEngine.GameObject("Tent", typeof(UnityEngine.RectTransform)); icon.transform.SetParent(row.transform, false);
        var img = icon.AddComponent<UnityEngine.UI.Image>(); img.sprite = tentSprite; img.preserveAspect = true; img.color = red ? UnityEngine.Color.white : new UnityEngine.Color(0.95f, 0.9f, 0.8f);
        icon.AddComponent<UnityEngine.UI.LayoutElement>().preferredWidth = 20f;
    }
    var label = new UnityEngine.GameObject("Text", typeof(UnityEngine.RectTransform)); label.transform.SetParent(row.transform, false);
    var t = label.AddComponent<UnityEngine.UI.Text>(); t.font = font; t.fontSize = 14; t.fontStyle = UnityEngine.FontStyle.Bold; t.alignment = UnityEngine.TextAnchor.MiddleCenter; t.color = red ? UnityEngine.Color.white : new UnityEngine.Color(0.95f, 0.9f, 0.8f); t.text = text; t.horizontalOverflow = UnityEngine.HorizontalWrapMode.Overflow;
}

// ---- Camp signpost: in front of the cabin, where both trails leave.
var camp = Post("Sign_Camp", 247.5f, 184.5f);
Board(camp, 1.85f, V(255f, 0f, 168f), V(245f, 0f, 187f), "CAMPSITES", true, false);
Board(camp, 1.45f, V(238f, 0f, 185f), V(245f, 0f, 187f), "DO NOT ENTER", false, true);
// ---- Fork signpost: one tent board per branch.
var fork = Post("Sign_Fork", 271.5f, 131.5f);
Board(fork, 1.95f, V(295f, 0f, 128f), V(265f, 0f, 145f), "CAMPSITE", true, false);
Board(fork, 1.6f, V(258f, 0f, 108f), V(265f, 0f, 145f), "CAMPSITE", true, false);
Board(fork, 1.25f, V(290f, 0f, 100f), V(265f, 0f, 145f), "CAMPSITE", true, false);
// ---- Ward end: the warning again, facing anyone walking out of the trees onto the ledge.
var ward = Post("Sign_Ward", 82f, 312f);
Board(ward, 1.7f, V(66f, 0f, 323f), V(110f, 0f, 300f), "DO NOT ENTER", false, true);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " signs=" + root.transform.childCount + " sprite=" + (tentSprite != null);
