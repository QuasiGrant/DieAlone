if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first: " + scene.path;
if (UnityEngine.GameObject.Find("Ward") != null) return "Ward already exists";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));

// ---- Rune texture, generated: dark stone-gray field with bright glyph strokes in a grid.
if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Textures/Runes")) UnityEditor.AssetDatabase.CreateFolder("Assets/Textures", "Runes");
const int size = 512; const int cells = 4;
var tex = new UnityEngine.Texture2D(size, size, UnityEngine.TextureFormat.RGB24, false);
var pixels = new UnityEngine.Color32[size * size];
for (int i = 0; i < pixels.Length; i++) pixels[i] = new UnityEngine.Color32(0, 0, 0, 255);
void Line(int x0, int y0, int x1, int y1, int thick)
{
    int dx = System.Math.Abs(x1 - x0), dy = -System.Math.Abs(y1 - y0), sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1, err = dx + dy;
    while (true)
    {
        for (int ox = -thick; ox <= thick; ox++) for (int oy = -thick; oy <= thick; oy++)
        { int px = x0 + ox, py = y0 + oy; if (px >= 0 && py >= 0 && px < size && py < size) pixels[py * size + px] = new UnityEngine.Color32(255, 255, 255, 255); }
        if (x0 == x1 && y0 == y1) break;
        int e2 = 2 * err; if (e2 >= dy) { err += dy; x0 += sx; } if (e2 <= dx) { err += dx; y0 += sy; }
    }
}
var rng = new System.Random(7);
int cell = size / cells;
for (int cy = 0; cy < cells; cy++) for (int cx = 0; cx < cells; cx++)
{
    int ox = cx * cell + cell / 6, oy = cy * cell + cell / 6, span = cell - cell / 3;
    // A vertical stem plus 2 to 4 diagonal or horizontal branches, the classic rune shape.
    int stemX = ox + span / 2;
    Line(stemX, oy, stemX, oy + span, 3);
    int branches = rng.Next(2, 5);
    for (int b = 0; b < branches; b++)
    {
        int y = oy + rng.Next(span / 6, span - span / 6);
        int dir = rng.Next(0, 2) == 0 ? -1 : 1;
        int len = rng.Next(span / 4, span / 2);
        int dy = rng.Next(-len / 2, len / 2 + 1);
        Line(stemX, y, stemX + dir * len, y + dy, 3);
    }
}
tex.SetPixels32(pixels); tex.Apply();
string runePath = "Assets/Textures/Runes/Runes_Emissive.png";
System.IO.File.WriteAllBytes(runePath, tex.EncodeToPNG());
UnityEngine.Object.DestroyImmediate(tex);
UnityEditor.AssetDatabase.ImportAsset(runePath);
var runeImp = UnityEditor.AssetImporter.GetAtPath(runePath) as UnityEditor.TextureImporter;
runeImp.sRGBTexture = true; runeImp.wrapMode = UnityEngine.TextureWrapMode.Repeat; runeImp.maxTextureSize = 512; runeImp.SaveAndReimport();
var runeTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(runePath);

// ---- Stone material: concrete base, rune emission.
var lit = UnityEngine.Shader.Find("Universal Render Pipeline/Lit");
var stoneMat = new UnityEngine.Material(lit);
stoneMat.SetTexture("_BaseMap", UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>("Assets/Textures/Concrete034/Concrete034_Color.jpg"));
stoneMat.SetTextureScale("_BaseMap", new UnityEngine.Vector2(1f, 3f));
stoneMat.SetColor("_BaseColor", new UnityEngine.Color(0.55f, 0.55f, 0.6f));
stoneMat.SetFloat("_Smoothness", 0.15f);
stoneMat.EnableKeyword("_EMISSION");
stoneMat.globalIlluminationFlags = UnityEngine.MaterialGlobalIlluminationFlags.RealtimeEmissive;
stoneMat.SetTexture("_EmissionMap", runeTex);
stoneMat.SetTextureScale("_EmissionMap", new UnityEngine.Vector2(1f, 3f));
stoneMat.SetColor("_EmissionColor", new UnityEngine.Color(0.05f, 0.25f, 0.2f));
UnityEditor.AssetDatabase.CreateAsset(stoneMat, "Assets/Materials/WardStone.mat");

// ---- Six tapered monoliths on the ledge, ring around (65, 320) open toward the cliff.
var ward = new UnityEngine.GameObject("Ward");
var stones = new (float x, float z, float w, float h, float d, float yaw, float tilt)[] {
    (52f, 331f, 3.2f, 13f, 2.0f, 20f, 4f),
    (58f, 339f, 2.6f, 9f, 1.8f, -35f, 3f),
    (70f, 341f, 3.8f, 15f, 2.4f, 10f, 5f),
    (81f, 333f, 3.0f, 11f, 2.0f, 55f, 2f),
    (83f, 318f, 2.4f, 8f, 1.6f, -20f, 6f),
    (73f, 305f, 3.4f, 12f, 2.2f, 40f, 3f),
};
int idx = 0;
UnityEngine.GameObject usable = null;
foreach (var s in stones)
{
    idx++;
    var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var go = pb.gameObject; go.name = "Stone_" + idx; go.transform.SetParent(ward.transform, false);
    // Taper: pull the top vertices inward, then push the whole thing down 1 m into the ground.
    var pos = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    for (int i = 0; i < pos.Count; i++)
    {
        var p = pos[i];
        float taper = p.y > 0f ? 0.65f : 1f;
        pos[i] = new UnityEngine.Vector3(p.x * s.w * taper, (p.y + 0.5f) * s.h - 1f, p.z * s.d * taper);
    }
    pb.positions = pos; pb.ToMesh(); pb.Refresh();
    go.transform.position = V(s.x, H(s.x, s.z), s.z);
    go.transform.rotation = UnityEngine.Quaternion.Euler(s.tilt, s.yaw, 0f);
    go.GetComponent<UnityEngine.Renderer>().sharedMaterial = stoneMat;
    var col = go.AddComponent<UnityEngine.MeshCollider>();
    col.sharedMesh = go.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
    if (idx == 3) usable = go;
}
var ws = usable.AddComponent<WardStone>();
var so = new UnityEditor.SerializedObject(ws);
so.FindProperty("prompt").stringValue = "Touch the stone";
so.FindProperty("target").objectReferenceValue = usable.GetComponent<UnityEngine.Renderer>();
so.ApplyModifiedPropertiesWithoutUndo();

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " stones=" + idx + " usable=" + usable.name + " runeTex=" + runeTex.width + " ledgeH=" + H(65f, 320f).ToString("F1");
