// Main3 8.9e check, edit mode: every trail leg's walked length (along its 2 m centre points) and steepest 10 m of ground,
// the J to Ward climb's height at the pass and the lip, and the giant groves (clusters of giants within 20 m of each other)
// with their distance from the tower. Read-only.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
UnityEngine.GameObject Root(string name) { foreach (var r in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) if (r.name == name) return r; return null; }
var terrain = UnityEngine.Terrain.activeTerrain; float H(float x, float z) => terrain.SampleHeight(new UnityEngine.Vector3(x, 0f, z)) + terrain.transform.position.y;
var sb = new System.Text.StringBuilder();
foreach (UnityEngine.Transform leg in Root("Trails").transform)
{
    if (leg.childCount < 2) continue;
    var s = new System.Collections.Generic.List<UnityEngine.Vector3>();
    for (int i = 1; i < leg.childCount; i++)
    {
        var a = leg.GetChild(i - 1).position; var b = leg.GetChild(i).position; var d = b - a; float l = new UnityEngine.Vector2(d.x, d.z).magnitude;
        int n = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(l / 0.5f));
        for (int k = (i == 1 ? 0 : 1); k <= n; k++) { var q = a + d * (k / (float)n); q.y = H(q.x, q.z); s.Add(q); }
    }
    var acc = new float[s.Count]; for (int i = 1; i < s.Count; i++) acc[i] = acc[i - 1] + new UnityEngine.Vector2(s[i].x - s[i - 1].x, s[i].z - s[i - 1].z).magnitude;
    float worst = 0f, worstAt = 0f; var worstP = UnityEngine.Vector3.zero;
    for (int i = 0, j = 0; i < s.Count; i++) { while (j < s.Count - 1 && acc[j] - acc[i] < 10f) j++; if (acc[j] - acc[i] >= 9.9f) { float g = UnityEngine.Mathf.Abs(s[j].y - s[i].y) / (acc[j] - acc[i]); if (g > worst) { worst = g; worstAt = acc[i]; worstP = s[i]; } } }
    sb.Append(leg.name + ": " + acc[s.Count - 1].ToString("F1") + " m between its first and last points, steepest 10 m " + (worst * 100f).ToString("F1") + "% at " + worstAt.ToString("F0") + " m " + worstP.ToString("F1") + "\n");
    if (leg.name == "J to Ward")
    {
        float passH = float.MinValue; foreach (var q in s) if (UnityEngine.Vector2.Distance(new UnityEngine.Vector2(q.x, q.z), new UnityEngine.Vector2(112f, 262f)) < 3f) passH = UnityEngine.Mathf.Max(passH, q.y);
        sb.Append("  J to Ward: pass ground " + passH.ToString("F1") + ", end " + s[s.Count - 1].ToString("F1") + ", walk " + (acc[s.Count - 1] / 2.5f).ToString("F0") + " s at 2.5 m/s\n");
    }
}
// groves: giants linked when within 20 m
var gs = new System.Collections.Generic.List<UnityEngine.Vector2>();
foreach (UnityEngine.Transform g in Root("Giants").transform) if (g.name.StartsWith("Giant_")) gs.Add(new UnityEngine.Vector2(g.position.x, g.position.z));
var grp = new int[gs.Count]; for (int i = 0; i < gs.Count; i++) grp[i] = i;
int Find(int i) { while (grp[i] != i) i = grp[i] = grp[grp[i]]; return i; }
for (int i = 0; i < gs.Count; i++) for (int j = i + 1; j < gs.Count; j++) if (UnityEngine.Vector2.Distance(gs[i], gs[j]) <= 20.5f) grp[Find(i)] = Find(j);
var sets = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<UnityEngine.Vector2>>();
for (int i = 0; i < gs.Count; i++) { int r = Find(i); if (!sets.ContainsKey(r)) sets[r] = new System.Collections.Generic.List<UnityEngine.Vector2>(); sets[r].Add(gs[i]); }
var tower = new UnityEngine.Vector2(164f, 166f); int groves = 0;
sb.Append("Giants " + gs.Count + ": ");
foreach (var kv in sets)
{
    var c = UnityEngine.Vector2.zero; foreach (var q in kv.Value) c += q; c /= kv.Value.Count;
    if (kv.Value.Count >= 4) groves++;
    sb.Append(kv.Value.Count + " at (" + c.x.ToString("F0") + ", " + c.y.ToString("F0") + ") " + UnityEngine.Vector2.Distance(c, tower).ToString("F0") + " m; ");
}
sb.Append("\ngroves of 4 or more: " + groves);
return sb.ToString();
