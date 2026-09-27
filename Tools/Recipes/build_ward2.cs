if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.GameObject.Find("Terrain").GetComponent<UnityEngine.Terrain>();
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));

// ---- Rune texture: 3 x 4 cells of big glyphs, varied stroke, size and slant, a quarter of the cells left blank.
const int size = 512; const int cols = 3, rows = 4;
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
var rng = new System.Random(19);
int cw = size / cols, ch = size / rows; int glyphs = 0;
for (int cy = 0; cy < rows; cy++) for (int cx = 0; cx < cols; cx++)
{
    if (rng.NextDouble() < 0.25) continue;
    int gh = (int)(ch * (0.55 + rng.NextDouble() * 0.4)), gw = (int)(cw * (0.35 + rng.NextDouble() * 0.4));
    int ox = cx * cw + (cw - gw) / 2 + rng.Next(-cw / 10, cw / 10), oy = cy * ch + (ch - gh) / 2 + rng.Next(-ch / 12, ch / 12);
    int thick = rng.Next(3, 8);
    int stemX = ox + gw / 2, slant = rng.Next(-gw / 6, gw / 6);
    Line(stemX - slant, oy, stemX + slant, oy + gh, thick);
    int branches = rng.Next(1, 5);
    for (int b = 0; b < branches; b++)
    {
        int y = oy + rng.Next(gh / 8, gh - gh / 8);
        int dir = rng.Next(0, 2) == 0 ? -1 : 1;
        int len = rng.Next(gw / 3, gw / 2 + 1);
        int dy = rng.Next(-len, len + 1);
        Line(stemX, y, stemX + dir * len, y + dy, System.Math.Max(2, thick - rng.Next(0, 3)));
        if (rng.NextDouble() < 0.4) Line(stemX + dir * len, y + dy, stemX + dir * len, y + dy + rng.Next(-gh / 4, gh / 4), System.Math.Max(2, thick - 2));
    }
    if (rng.NextDouble() < 0.3) { int r = rng.Next(gw / 8, gw / 5); int cxr = stemX + rng.Next(-gw / 3, gw / 3), cyr = oy + rng.Next(gh / 5, gh * 4 / 5); for (int a = 0; a < 24; a++) { double t0 = a / 24.0 * 6.2832, t1 = (a + 1) / 24.0 * 6.2832; Line(cxr + (int)(System.Math.Cos(t0) * r), cyr + (int)(System.Math.Sin(t0) * r), cxr + (int)(System.Math.Cos(t1) * r), cyr + (int)(System.Math.Sin(t1) * r), 2); } }
    glyphs++;
}
tex.SetPixels32(pixels); tex.Apply();
string runePath = "Assets/Textures/Runes/Runes_Emissive.png";
System.IO.File.WriteAllBytes(runePath, tex.EncodeToPNG());
UnityEngine.Object.DestroyImmediate(tex);
UnityEditor.AssetDatabase.ImportAsset(runePath, UnityEditor.ImportAssetOptions.ForceUpdate);
var runeTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(runePath);

// ---- Material: flat stone base colour, concrete as a detail map tiled per metre, runes as emission stretched per stone.
var mat = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Materials/WardStone.mat");
var concrete = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>("Assets/Textures/Concrete034/Concrete034_Color.jpg");
mat.SetTexture("_BaseMap", null);
mat.SetTextureScale("_BaseMap", UnityEngine.Vector2.one); mat.SetTextureOffset("_BaseMap", UnityEngine.Vector2.zero);
mat.SetColor("_BaseColor", new UnityEngine.Color(0.5f, 0.5f, 0.55f));
mat.SetTexture("_DetailAlbedoMap", concrete);
mat.SetTextureScale("_DetailAlbedoMap", new UnityEngine.Vector2(0.5f, 0.5f));
mat.SetFloat("_DetailAlbedoMapScale", 1f);
mat.EnableKeyword("_DETAIL_MULX2");
mat.SetTexture("_EmissionMap", runeTex);
mat.SetTextureScale("_EmissionMap", UnityEngine.Vector2.one);
UnityEditor.EditorUtility.SetDirty(mat);

// ---- Three stones in a row near the cliff edge (wall at x 40), uneven heights, the tall one is the usable.
foreach (var rootGo in scene.GetRootGameObjects()) if (rootGo.name == "Ward") UnityEngine.Object.DestroyImmediate(rootGo);
var ward = new UnityEngine.GameObject("Ward");
var stones = new (float x, float z, float w, float h, float d, float yaw, float tilt, float taper)[] {
    (46.0f, 328.5f, 2.8f, 7.5f, 1.8f, 15f, 5f, 0.7f),
    (47.5f, 322.5f, 3.4f, 12.0f, 2.2f, -10f, 3f, 0.55f),
    (46.5f, 316.5f, 3.0f, 9.5f, 2.0f, 30f, 7f, 0.75f),
};
int idx = 0; UnityEngine.GameObject usable = null;
foreach (var s in stones)
{
    idx++;
    var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var go = pb.gameObject; go.name = "Stone_" + idx; go.transform.SetParent(ward.transform, false);
    var pos = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    for (int i = 0; i < pos.Count; i++)
    {
        var p = pos[i]; float taper = p.y > 0f ? s.taper : 1f;
        pos[i] = new UnityEngine.Vector3(p.x * s.w * taper, (p.y + 0.5f) * s.h - 1f, p.z * s.d * taper);
    }
    pb.positions = pos; pb.ToMesh(); pb.Refresh();
    go.transform.position = V(s.x, H(s.x, s.z), s.z);
    go.transform.rotation = UnityEngine.Quaternion.Euler(s.tilt, s.yaw, 0f);
    var r = go.GetComponent<UnityEngine.Renderer>(); r.sharedMaterial = mat;
    // Per stone: the rune sheet spans the stone once (about w across, h up) with a random offset so the three differ.
    var block = new UnityEngine.MaterialPropertyBlock();
    block.SetVector("_BaseMap_ST", new UnityEngine.Vector4(1f / (s.w * 1.4f), 1f / (s.h - 1f), (float)rng.NextDouble(), (float)rng.NextDouble()));
    r.SetPropertyBlock(block);
    var col = go.AddComponent<UnityEngine.MeshCollider>(); col.sharedMesh = go.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
    if (idx == 2) usable = go;
}
var ws = usable.AddComponent<WardStone>();
var so = new UnityEditor.SerializedObject(ws);
so.FindProperty("prompt").stringValue = "Touch the stone";
so.FindProperty("target").objectReferenceValue = usable.GetComponent<UnityEngine.Renderer>();
so.ApplyModifiedPropertiesWithoutUndo();

// Warp point: on the ledge east of the stones, looking at them.
var warpsRoot = UnityEngine.GameObject.Find("DevWarps").transform; var warp = warpsRoot.Find("Ward"); if (warp == null) { warp = new UnityEngine.GameObject("Ward").transform; warp.SetParent(warpsRoot, false); }
warp.position = V(62f, H(62f, 322f) + 0.2f, 322f);
warp.rotation = UnityEngine.Quaternion.LookRotation(V(47f, 0f, 322.5f) - V(62f, 0f, 322f), UnityEngine.Vector3.up);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " glyphs=" + glyphs + " stones=" + idx + " usable=" + usable.name + " warp=" + warp.position.ToString("F1");
