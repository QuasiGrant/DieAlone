if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var signs = UnityEngine.GameObject.Find("Signs").transform;
var planks = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/Planks023A_1.0x1.0.mat");
var red = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/SignRed.mat");
var font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("LegacyRuntime.ttf");
var tentSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>("Assets/Textures/UI/TentIcon.png");

// ---- Remove the camp post and its two boards.
var camp = signs.Find("Sign_Camp"); int removed = 0;
if (camp != null)
{
    var campPos = camp.position;
    for (int i = signs.childCount - 1; i >= 0; i--) { var c = signs.GetChild(i); if (c.name == "Board" && UnityEngine.Vector3.Distance(new UnityEngine.Vector3(c.position.x, 0, c.position.z), new UnityEngine.Vector3(campPos.x, 0, campPos.z)) < 1.5f) { UnityEngine.Object.DestroyImmediate(c.gameObject); removed++; } }
    UnityEngine.Object.DestroyImmediate(camp.gameObject); removed++;
}

// ---- Flat trailhead board: post plus one board whose face points at the reader.
void Trailhead(string name, float x, float z, UnityEngine.Vector3 readFrom, string text, bool tent, bool isRed)
{
    var post = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cylinder);
    post.name = name; post.transform.SetParent(signs, false);
    post.transform.position = V(x, H(x, z) + 1.05f, z); post.transform.localScale = V(0.14f, 1.05f, 0.14f);
    post.GetComponent<UnityEngine.Renderer>().sharedMaterial = planks;
    var faceDir = readFrom - post.transform.position; faceDir.y = 0f; faceDir.Normalize();
    var board = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    board.name = "Board"; board.transform.SetParent(signs, false);
    board.transform.rotation = UnityEngine.Quaternion.LookRotation(UnityEngine.Vector3.Cross(faceDir, UnityEngine.Vector3.up), UnityEngine.Vector3.up);
    board.transform.position = V(x, H(x, z) + 1.75f, z) + faceDir * 0.09f;
    board.transform.localScale = V(0.04f, 0.34f, 1.15f);
    board.GetComponent<UnityEngine.Renderer>().sharedMaterial = isRed ? red : planks;
    UnityEngine.Object.DestroyImmediate(board.GetComponent<UnityEngine.Collider>());
    var canvasGo = new UnityEngine.GameObject("Face", typeof(UnityEngine.RectTransform)); canvasGo.transform.SetParent(board.transform, false);
    var canvas = canvasGo.AddComponent<UnityEngine.Canvas>(); canvas.renderMode = UnityEngine.RenderMode.WorldSpace;
    var rt = canvasGo.GetComponent<UnityEngine.RectTransform>(); rt.sizeDelta = new UnityEngine.Vector2(112f, 30f);
    rt.localScale = new UnityEngine.Vector3(0.01f / board.transform.localScale.z, 0.01f / board.transform.localScale.y, 1f);
    rt.localPosition = new UnityEngine.Vector3(0.55f, 0f, 0f); rt.localRotation = UnityEngine.Quaternion.Euler(0f, -90f, 0f);
    var row = new UnityEngine.GameObject("Row", typeof(UnityEngine.RectTransform)); row.transform.SetParent(canvasGo.transform, false);
    var rr = row.GetComponent<UnityEngine.RectTransform>(); rr.anchorMin = UnityEngine.Vector2.zero; rr.anchorMax = UnityEngine.Vector2.one; rr.offsetMin = new UnityEngine.Vector2(4f, 2f); rr.offsetMax = new UnityEngine.Vector2(-4f, -2f);
    var hl = row.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>(); hl.childAlignment = UnityEngine.TextAnchor.MiddleCenter; hl.spacing = 3f; hl.childControlWidth = true; hl.childControlHeight = true; hl.childForceExpandWidth = false; hl.childForceExpandHeight = true;
    if (tent)
    {
        var icon = new UnityEngine.GameObject("Tent", typeof(UnityEngine.RectTransform)); icon.transform.SetParent(row.transform, false);
        var img = icon.AddComponent<UnityEngine.UI.Image>(); img.sprite = tentSprite; img.color = new UnityEngine.Color(0.95f, 0.9f, 0.8f);
        icon.AddComponent<UnityEngine.UI.LayoutElement>().preferredWidth = 26f;
    }
    var label = new UnityEngine.GameObject("Text", typeof(UnityEngine.RectTransform)); label.transform.SetParent(row.transform, false);
    var t = label.AddComponent<UnityEngine.UI.Text>(); t.font = font; t.fontSize = 13; t.fontStyle = UnityEngine.FontStyle.Bold; t.alignment = UnityEngine.TextAnchor.MiddleCenter; t.color = isRed ? UnityEngine.Color.white : new UnityEngine.Color(0.95f, 0.9f, 0.8f); t.text = text;
}
// Path A leaves the clearing at about (238,185) heading north-west; path B leaves at about (255,168) heading south.
Trailhead("Sign_TrailA", 240.5f, 183.0f, V(247f, 0f, 183f), "DO NOT ENTER", false, true);
Trailhead("Sign_TrailB", 253.0f, 171.5f, V(250f, 0f, 181f), "CAMPSITES", true, false);

bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "removed=" + removed + " saved=" + saved + " signs=" + signs.childCount;
