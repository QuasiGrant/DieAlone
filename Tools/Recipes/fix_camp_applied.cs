if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main.unity") return "open Main first";
var V = new System.Func<float, float, float, UnityEngine.Vector3>((x, y, z) => new UnityEngine.Vector3(x, y, z));
var terrain = UnityEngine.Terrain.activeTerrain;
float H(float x, float z) => terrain.SampleHeight(V(x, 0, z));
var sb = new System.Text.StringBuilder();

UnityEngine.GameObject Find(string name)
{
    foreach (var g in UnityEditor.AssetDatabase.FindAssets(name + " t:Prefab", new[] { "Assets/Revolving Pizza Games" }))
    { var p = UnityEditor.AssetDatabase.GUIDToAssetPath(g); if (System.IO.Path.GetFileNameWithoutExtension(p) == name) return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(p); }
    throw new System.Exception("no prefab named " + name);
}

// ---- 1. Opaque copies of the pack materials for use on plain geometry.
UnityEngine.Material Opaque(string srcPrefab, string outName)
{
    string path = "Assets/Materials/" + outName + ".mat";
    var existing = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (existing != null) return existing;
    var src = Find(srcPrefab).GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;
    var m = new UnityEngine.Material(src);
    m.SetFloat("_AlphaClip", 0f); m.DisableKeyword("_ALPHATEST_ON"); m.renderQueue = -1;
    UnityEditor.AssetDatabase.CreateAsset(m, path);
    return m;
}
var logMat = Opaque("CITW_Log_Wall", "CabinLog");
var plankMat = Opaque("CITW_Plank_Wall", "CabinPlank");
var packLog = Find("CITW_Log_Wall").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;
var packPlank = Find("CITW_Plank_Wall").GetComponentInChildren<UnityEngine.Renderer>().sharedMaterial;

// Swap every plain box under Camp and Era_Modern that carries the clipped pack material.
int swapped = 0;
foreach (var rootName in new[] { "Camp", "Era_Modern" })
{
    var root = UnityEngine.GameObject.Find(rootName); if (root == null) continue;
    foreach (var mr in root.GetComponentsInChildren<UnityEngine.MeshRenderer>(true))
    {
        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(mr.gameObject)) continue;   // pack pieces keep their own materials
        if (mr.sharedMaterial == packLog) { mr.sharedMaterial = logMat; swapped++; }
        else if (mr.sharedMaterial == packPlank) { mr.sharedMaterial = plankMat; swapped++; }
    }
}
sb.Append("materialsSwapped=" + swapped);

// ---- 2. Cabin roof: overhanging slabs, ridge beam, triangular gables.
var cabin = UnityEngine.GameObject.Find("Camp/Cabin").transform;
foreach (var n in new[] { "Roof_S", "Roof_N", "Gable_E", "Gable_W" }) { var t = cabin.Find(n); if (t != null) UnityEngine.Object.DestroyImmediate(t.gameObject); }
UnityEngine.GameObject Box(string name, UnityEngine.Transform parent, UnityEngine.Vector3 pos, UnityEngine.Vector3 size, UnityEngine.Material mat, UnityEngine.Vector3? euler = null)
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.name = name; go.transform.SetParent(parent, false); go.transform.localPosition = pos; go.transform.localScale = size;
    if (euler.HasValue) go.transform.localRotation = UnityEngine.Quaternion.Euler(euler.Value);
    go.GetComponent<UnityEngine.Renderer>().sharedMaterial = mat; return go;
}
float wallTop = 3.0f, rise = 1.5f, halfDepth = 2.0f, overhang = 0.4f;
float run = halfDepth + overhang; float pitch = UnityEngine.Mathf.Atan2(rise * (run / halfDepth), run) * UnityEngine.Mathf.Rad2Deg;
float ridgeY = wallTop + rise * (run / halfDepth) - rise * (overhang / halfDepth);  // ridge stays at wallTop + rise
ridgeY = wallTop + rise;
float eaveY = ridgeY - rise * (run / halfDepth);
float slabLen = UnityEngine.Mathf.Sqrt(run * run + (ridgeY - eaveY) * (ridgeY - eaveY));
float slabMidY = (ridgeY + eaveY) * 0.5f + 0.08f, slabMidZ = run * 0.5f;
Box("Roof_S", cabin, V(0f, slabMidY, -slabMidZ), V(7.0f, 0.16f, slabLen), plankMat, V(-pitch, 0f, 0f));
Box("Roof_N", cabin, V(0f, slabMidY, slabMidZ), V(7.0f, 0.16f, slabLen), plankMat, V(pitch, 0f, 0f));
Box("RidgeBeam", cabin, V(0f, ridgeY + 0.12f, 0f), V(7.1f, 0.22f, 0.3f), logMat);
// Gable triangles: ProBuilder cube, top edge collapsed to a line at the ridge.
foreach (var side in new[] { -3f, 3f })
{
    var pb = UnityEngine.ProBuilder.ShapeGenerator.CreateShape(UnityEngine.ProBuilder.ShapeType.Cube);
    var go = pb.gameObject; go.name = side < 0 ? "Gable_W" : "Gable_E"; go.transform.SetParent(cabin, false);
    var pos = new System.Collections.Generic.List<UnityEngine.Vector3>(pb.positions);
    for (int i = 0; i < pos.Count; i++)
    {
        var p = pos[i];
        float z = p.y > 0f ? 0f : p.z * 4.4f;                 // top vertices meet at the ridge
        pos[i] = new UnityEngine.Vector3(p.x * 0.3f, (p.y + 0.5f) * rise, z);
    }
    pb.positions = pos; pb.ToMesh(); pb.Refresh();
    go.transform.localPosition = V(side, wallTop - 0.02f, 0f);
    go.GetComponent<UnityEngine.Renderer>().sharedMaterial = logMat;
    var mc = go.AddComponent<UnityEngine.MeshCollider>(); mc.sharedMesh = go.GetComponent<UnityEngine.MeshFilter>().sharedMesh;
}
// Window frames in the four window openings.
foreach (var (pos, yaw) in new (UnityEngine.Vector3, float)[] { (V(-2f, 0f, 2f), 180f), (V(2f, 0f, 2f), 180f), (V(3f, 0f, 1f), 90f) })
{
    var f = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Find("CITW_Window_Frame"), scene);
    f.transform.SetParent(cabin, false); f.transform.localPosition = pos; f.transform.localRotation = UnityEngine.Quaternion.Euler(0f, yaw, 0f);
}

// ---- 3. Pull the camp in around the cabin. Cabin stays at (245, 190); door faces south.
var camp = UnityEngine.GameObject.Find("Camp").transform; var era = UnityEngine.GameObject.Find("Era_Modern").transform;
float fx = 251.5f, fz = 182.5f;
var fire = camp.Find("FirePit"); fire.position = V(fx, H(fx, fz), fz);
for (int i = 0; i < 4; i++)
{
    var seat = camp.Find("Seat_" + (i + 1)); float ang = 45f + i * 90f, r = 2.3f;
    var p = V(fx + UnityEngine.Mathf.Sin(ang * UnityEngine.Mathf.Deg2Rad) * r, 0f, fz + UnityEngine.Mathf.Cos(ang * UnityEngine.Mathf.Deg2Rad) * r); p.y = H(p.x, p.z);
    seat.position = p;
}
var gen = era.Find("Generator"); gen.position = V(243f, H(243f, 194.5f) + 0.45f, 194.5f); gen.rotation = UnityEngine.Quaternion.Euler(0f, 5f, 0f);
foreach (UnityEngine.Transform child in era)
{
    if (child.name.StartsWith("CS_Lantern")) child.position = V(fx + 1.6f, H(fx + 1.6f, fz + 2.4f), fz + 2.4f);
    if (child.name.StartsWith("CS_Table_Small")) child.position = V(fx + 4.2f, H(fx + 4.2f, fz + 0.8f), fz + 0.8f);
}
var tower = camp.Find("FirewatchTower"); float tx = 236f, tz = 200f; tower.position = V(tx, H(tx, tz), tz);

// ---- 4. Clear trees under the moved tower and its stairs, keep the rest.
var data = terrain.terrainData; float size = data.size.x;
var keep = new System.Collections.Generic.List<UnityEngine.TreeInstance>(); int removed = 0;
foreach (var t in data.treeInstances)
{
    var p = new UnityEngine.Vector2(t.position.x * size, t.position.z * size);
    bool drop = UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(tx, tz)) < 7f || UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(tx, tz - 4.5f)) < 4.5f || UnityEngine.Vector2.Distance(p, new UnityEngine.Vector2(243f, 194.5f)) < 2f;
    if (drop) removed++; else keep.Add(t);
}
data.SetTreeInstances(keep.ToArray(), true); UnityEditor.EditorUtility.SetDirty(data);

UnityEditor.AssetDatabase.SaveAssets();
bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
sb.Append(" | roof: eaveY=" + eaveY.ToString("F2") + " ridgeY=" + ridgeY.ToString("F2") + " pitch=" + pitch.ToString("F1") + " | fire=" + fire.position.ToString("F1") + " tower=" + tower.position.ToString("F1") + " gen=" + gen.position.ToString("F1") + " treesRemoved=" + removed + " saved=" + saved);
return sb.ToString();
