// Main3 8.27 wall restore (edit mode, Main3; 2026-10-03). The first main3_8_27_cave.cs removed 8.8's Entrance/Wall_E across the niche and
// 8.17's SideRoom/Wall_E_Future and built their split pieces under Layout827; a second run rebuilt Layout827 empty of them (Fresh) and found
// no wall to split, leaving both openings bare. The recipe now keeps the originals and hides them. This puts the originals back from the
// scene as committed at 'from' (Main3.unity at 1de0f75, LFS smudged): each box named in 'names' under Cave/Entrance or Cave/SideRoom that is
// missing now, as a cube with its own collider, local position, rotation, scale, layer and material. One-off; saves the scene.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != "Assets/Scenes/Main3.unity") return "open Main3 first";
const string from = "Temp/main3_1de0f75.unity"; const float tol = 0.01f;
var names = new[] { ("Cave/Entrance", "Wall_E"), ("Cave/SideRoom", "Wall_E_Future") };
var lines = System.IO.File.ReadAllLines(from);
var blocks = new System.Collections.Generic.Dictionary<long, (int type, int start, int end)>(); long cur = 0; int ctype = 0, cstart = 0;
for (int i = 0; i < lines.Length; i++)
{
    var l = lines[i]; if (!l.StartsWith("--- !u!")) continue;
    if (cur != 0) blocks[cur] = (ctype, cstart, i); var parts = l.Substring(7).Split(new[] { " &" }, System.StringSplitOptions.None); ctype = int.Parse(parts[0]); cur = long.Parse(parts[1].Split(' ')[0]); cstart = i;
}
if (cur != 0) blocks[cur] = (ctype, cstart, lines.Length);
string Field(int s, int e, string key) { for (int i = s; i < e; i++) { var t = lines[i].Trim(); if (t.StartsWith(key + ":")) return t.Substring(key.Length + 1).Trim(); } return null; }
long FileId(string v) { if (v == null) return 0; int a = v.IndexOf("fileID: "); if (a < 0) return 0; var s = v.Substring(a + 8); int b = s.IndexOfAny(new[] { ',', '}' }); return long.Parse(s.Substring(0, b).Trim()); }
string GuidOf(string v) { if (v == null) return null; int a = v.IndexOf("guid: "); if (a < 0) return null; var s = v.Substring(a + 6); int b = s.IndexOfAny(new[] { ',', '}' }); return s.Substring(0, b).Trim(); }
float Num(string v, string k) { int a = v.IndexOf(k + ": "); var s = v.Substring(a + k.Length + 2); int b = s.IndexOfAny(new[] { ',', '}' }); return float.Parse(s.Substring(0, b).Trim(), System.Globalization.CultureInfo.InvariantCulture); }
string NameOfGo(long goId) => blocks.TryGetValue(goId, out var b) ? Field(b.start, b.end, "m_Name") ?? "?" : "?";
string PathOf(long tId) { var n = new System.Collections.Generic.List<string>(); long t = tId; int g = 0; while (t != 0 && g++ < 50 && blocks.TryGetValue(t, out var b)) { n.Insert(0, NameOfGo(FileId(Field(b.start, b.end, "m_GameObject")))); t = FileId(Field(b.start, b.end, "m_Father")); } return string.Join("/", n); }
// each GameObject's MeshRenderer material (the first)
var matOf = new System.Collections.Generic.Dictionary<long, string>();
foreach (var kv in blocks) if (kv.Value.type == 23) { int s = kv.Value.start, e = kv.Value.end; long go = FileId(Field(s, e, "m_GameObject")); for (int i = s; i < e - 1; i++) if (lines[i].Trim() == "m_Materials:") { matOf[go] = GuidOf(lines[i + 1]); break; } }
var o = new System.Text.StringBuilder(); int put = 0;
foreach (var kv in blocks)
{
    if (kv.Value.type != 4) continue; int s = kv.Value.start, e = kv.Value.end; long go = FileId(Field(s, e, "m_GameObject")); string name = NameOfGo(go);
    foreach (var (parentPath, want) in names)
    {
        if (name != want) continue; var path = PathOf(kv.Key); if (path != parentPath + "/" + want) continue;
        var parent = Main3AreaSet.At(scene, parentPath); if (parent == null) { o.Append("no " + parentPath + "; "); continue; }
        var lp = Field(s, e, "m_LocalPosition"); var lr = Field(s, e, "m_LocalRotation"); var ls = Field(s, e, "m_LocalScale");
        var pos = new UnityEngine.Vector3(Num(lp, "x"), Num(lp, "y"), Num(lp, "z")); var rot = new UnityEngine.Quaternion(Num(lr, "x"), Num(lr, "y"), Num(lr, "z"), Num(lr, "w")); var scl = new UnityEngine.Vector3(Num(ls, "x"), Num(ls, "y"), Num(ls, "z"));
        bool there = false; foreach (UnityEngine.Transform c in parent) if (c.name == want && (c.localPosition - pos).magnitude < tol) there = true;
        if (there) { o.Append(parentPath + "/" + want + " at " + pos + " already there; "); continue; }
        var g = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube); g.name = want; g.transform.SetParent(parent, false); g.transform.localPosition = pos; g.transform.localRotation = rot; g.transform.localScale = scl;
        var goBlock = blocks[go]; var layer = Field(goBlock.start, goBlock.end, "m_Layer"); if (layer != null) g.layer = int.Parse(layer);
        if (matOf.TryGetValue(go, out var mg) && mg != null) { var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(UnityEditor.AssetDatabase.GUIDToAssetPath(mg)); if (m != null) g.GetComponent<UnityEngine.Renderer>().sharedMaterial = m; else o.Append("no material " + mg + "; "); }
        put++; o.Append("put back " + parentPath + "/" + want + " at world " + g.transform.position + " size " + scl + " (" + g.GetComponent<UnityEngine.Renderer>().sharedMaterial.name + "); ");
    }
}
UnityEngine.Physics.SyncTransforms(); UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene); bool saved = UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return "saved=" + saved + " | restored " + put + " | " + o;
