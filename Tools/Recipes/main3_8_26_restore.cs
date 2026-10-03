// Main3 8.26 restore (edit mode, Main3; 2026-10-03): the first 8.26 run's keep-out sweep took three pieces it should have kept (K5 drawn
// 1.5 m too wide on its east end; K3's stump at 2.09 m, which Camp3Layout C7 keeps). This puts each back from the scene as committed at
// 'from' (a copy of that Main3.unity, LFS smudged, at Temp/main3_head.unity): the PrefabInstance whose local position matches, its source
// prefab, its parent by path and its local position, rotation and scale. One-off; saves the scene.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
const string from = "Temp/main3_head.unity"; const float tol = 0.006f;   // the targets are given to 0.01 m
var targets = new (string name, float x, float z)[] { ("RedFir8", 96.88f, 136.77f), ("RedFir6", 98.00f, 138.52f), ("CITW_Tree_Stump", 56.25f, 151.14f) };
var lines = System.IO.File.ReadAllLines(from);
// the YAML objects: id -> (type, lines)
var blocks = new System.Collections.Generic.Dictionary<long, (int type, int start, int end)>(); long cur = 0; int ctype = 0, cstart = 0;
for (int i = 0; i < lines.Length; i++)
{
    var l = lines[i]; if (!l.StartsWith("--- !u!")) continue;
    if (cur != 0) blocks[cur] = (ctype, cstart, i); var parts = l.Substring(7).Split(new[] { " &" }, System.StringSplitOptions.None); ctype = int.Parse(parts[0]); var idp = parts[1].Split(' ')[0]; cur = long.Parse(idp); cstart = i;
}
if (cur != 0) blocks[cur] = (ctype, cstart, lines.Length);
string Field(int s, int e, string key) { for (int i = s; i < e; i++) { var t = lines[i].Trim(); if (t.StartsWith(key + ":")) return t.Substring(key.Length + 1).Trim(); } return null; }
long FileId(string v) { if (v == null) return 0; int a = v.IndexOf("fileID: "); if (a < 0) return 0; var s = v.Substring(a + 8); int b = s.IndexOfAny(new[] { ',', '}' }); return long.Parse(s.Substring(0, b).Trim()); }
string Guid(string v) { if (v == null) return null; int a = v.IndexOf("guid: "); if (a < 0) return null; var s = v.Substring(a + 6); int b = s.IndexOfAny(new[] { ',', '}' }); return s.Substring(0, b).Trim(); }
// a transform's path: its GameObject's name and its father's, up to the root; a stripped transform belongs to a prefab instance (name from its m_Name modification)
string NameOfGo(long goId) { if (!blocks.TryGetValue(goId, out var b)) return "?"; return Field(b.start, b.end, "m_Name") ?? "?"; }
string PathOfTransform(long tId)
{
    var names = new System.Collections.Generic.List<string>(); long t = tId; int guard = 0;
    while (t != 0 && guard++ < 50 && blocks.TryGetValue(t, out var b))
    {
        var go = FileId(Field(b.start, b.end, "m_GameObject")); names.Insert(0, NameOfGo(go)); t = FileId(Field(b.start, b.end, "m_Father"));
    }
    return string.Join("/", names);
}
var o = new System.Text.StringBuilder(); int put = 0; var parentCache = new System.Collections.Generic.Dictionary<long, UnityEngine.Transform>();
foreach (var kv in blocks)
{
    if (kv.Value.type != 1001) continue; int s = kv.Value.start, e = kv.Value.end;
    float px = float.NaN, py = float.NaN, pz = float.NaN, rx = 0, ry = 0, rz = 0, rw = 1, sx = 1, sy = 1, sz = 1; string mName = null;
    for (int i = s; i < e - 1; i++)
    {
        var t = lines[i].Trim(); if (!t.StartsWith("propertyPath:")) continue; var prop = t.Substring(13).Trim(); var vline = lines[i + 1].Trim(); if (!vline.StartsWith("value:")) continue; var val = vline.Substring(6).Trim();
        float f; bool num = float.TryParse(val, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out f);
        switch (prop) { case "m_LocalPosition.x": px = f; break; case "m_LocalPosition.y": py = f; break; case "m_LocalPosition.z": pz = f; break; case "m_LocalRotation.x": rx = f; break; case "m_LocalRotation.y": ry = f; break; case "m_LocalRotation.z": rz = f; break; case "m_LocalRotation.w": rw = f; break; case "m_LocalScale.x": sx = f; break; case "m_LocalScale.y": sy = f; break; case "m_LocalScale.z": sz = f; break; case "m_Name": mName = val; break; }
    }
    if (float.IsNaN(px)) continue; long parentId = FileId(Field(s, e, "m_TransformParent")); if (!parentCache.TryGetValue(parentId, out var parentT)) { parentT = Main3AreaSet.At(scene, PathOfTransform(parentId)); parentCache[parentId] = parentT; }
    if (parentT == null) continue; var world = parentT.TransformPoint(new UnityEngine.Vector3(px, py, pz));
    foreach (var (name, x, z) in targets)
    {
        if (UnityEngine.Mathf.Abs(world.x - x) > tol || UnityEngine.Mathf.Abs(world.z - z) > tol) continue;
        var guid = Guid(Field(s, e, "m_SourcePrefab")); var src = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
        if (src == null || !src.name.StartsWith(name)) { o.Append("skip " + name + " candidate (prefab " + (src != null ? src.name : guid) + "); "); continue; }
        var parentPath = PathOfTransform(parentId); var parent = parentT;
        if (parent == null) { o.Append("no parent " + parentPath + " for " + name + "; "); continue; }
        bool there = false; foreach (UnityEngine.Transform c in parent) if (c.name.StartsWith(name) && (c.localPosition - new UnityEngine.Vector3(px, py, pz)).magnitude < tol) there = true;
        if (there) { o.Append(name + " already at " + parentPath + "; "); continue; }
        var g = (UnityEngine.GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(src, parent); if (mName != null) g.name = mName;
        g.transform.localPosition = new UnityEngine.Vector3(px, py, pz); g.transform.localRotation = new UnityEngine.Quaternion(rx, ry, rz, rw); g.transform.localScale = new UnityEngine.Vector3(sx, sy, sz);
        put++; o.Append("put back " + g.name + " under " + parentPath + " at (" + g.transform.position.x.ToString("F2") + ", " + g.transform.position.z.ToString("F2") + "); ");
    }
}
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene); bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | restored " + put + " of " + targets.Length + " | " + o;
