// Task 7.4 inventory of the bought packs: material count by shader (with shader file path for
// non-built-in shaders), prefab count and names, particle prefabs, scripts, and top folders.
var roots = new[] { "Assets/Effigy GameWorks", "Assets/BK", "Assets/NatureManufacture Assets", "Assets/Revolving Pizza Games/Catacombs", "Assets/PSX Edition - Modular Parking Lot", "Assets/Modular Chain Link Fence", "Assets/PSX Farm Tools Pack", "Assets/PSX Supplies Pack" };
var sb = new System.Text.StringBuilder();
foreach (var root in roots)
{
    if (!UnityEditor.AssetDatabase.IsValidFolder(root)) { sb.Append("\n" + root + ": MISSING"); continue; }
    var mats = UnityEditor.AssetDatabase.FindAssets("t:Material", new[] { root });
    var shaders = new System.Collections.Generic.Dictionary<string, int>();
    foreach (var g in mats)
    {
        var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(UnityEditor.AssetDatabase.GUIDToAssetPath(g));
        string s = m == null || m.shader == null ? "null" : m.shader.name;
        if (m != null && m.shader != null)
        {
            var sp = UnityEditor.AssetDatabase.GetAssetPath(m.shader);
            if (sp.StartsWith("Assets/")) s += " [" + sp + "]";
            if (!m.shader.isSupported || UnityEditor.ShaderUtil.ShaderHasError(m.shader)) s += " BROKEN";
        }
        shaders[s] = shaders.ContainsKey(s) ? shaders[s] + 1 : 1;
    }
    var prefabs = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { root });
    var scripts = UnityEditor.AssetDatabase.FindAssets("t:MonoScript", new[] { root });
    var names = new System.Collections.Generic.List<string>();
    var particleNames = new System.Collections.Generic.List<string>();
    foreach (var g in prefabs)
    {
        var path = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
        names.Add(path.Substring(root.Length + 1));
        var p = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(path);
        if (p != null && p.GetComponentInChildren<UnityEngine.ParticleSystem>(true) != null) particleNames.Add(p.name);
    }
    sb.Append("\n== " + root + ": materials=" + mats.Length + " prefabs=" + prefabs.Length + " scripts=" + scripts.Length + " particlePrefabs=" + particleNames.Count);
    sb.Append("\n   shaders: "); foreach (var kv in shaders) sb.Append(kv.Key + "=" + kv.Value + "; ");
    sb.Append("\n   folders: " + string.Join(", ", UnityEditor.AssetDatabase.GetSubFolders(root).Select(f => f.Substring(root.Length + 1))));
    sb.Append("\n   prefabs: " + string.Join(", ", names));
}
return sb.ToString();
