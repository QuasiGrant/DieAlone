// Inventory of the imported packs: materials by shader, prefab counts, and a keyword
// search for the things task 5.1 asks about. Also lists any script assets, which a
// pack must not add without the owner knowing.
var roots = new[] { "Assets/Celestia_Studio", "Assets/Revolving Pizza Games/Campsite", "Assets/Revolving Pizza Games/Cabin In The Woods", "Assets/suffercord" };
var sb = new System.Text.StringBuilder();
foreach (var root in roots)
{
    if (!UnityEditor.AssetDatabase.IsValidFolder(root)) { sb.Append("\n" + root + ": MISSING"); continue; }
    var mats = UnityEditor.AssetDatabase.FindAssets("t:Material", new[] { root });
    var shaders = new System.Collections.Generic.Dictionary<string, int>();
    foreach (var g in mats)
    {
        var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(UnityEditor.AssetDatabase.GUIDToAssetPath(g));
        string s = m != null && m.shader != null ? m.shader.name : "null";
        shaders[s] = shaders.ContainsKey(s) ? shaders[s] + 1 : 1;
    }
    var prefabs = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { root });
    var scripts = UnityEditor.AssetDatabase.FindAssets("t:MonoScript", new[] { root });
    var particles = 0; var particleNames = new System.Collections.Generic.List<string>();
    foreach (var g in prefabs)
    {
        var p = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(g));
        if (p != null && p.GetComponentInChildren<UnityEngine.ParticleSystem>(true) != null) { particles++; if (particleNames.Count < 6) particleNames.Add(p.name); }
    }
    sb.Append("\n== " + root + ": materials=" + mats.Length + " prefabs=" + prefabs.Length + " scripts=" + scripts.Length + " prefabsWithParticles=" + particles);
    sb.Append("\n   shaders: "); foreach (var kv in shaders) sb.Append(kv.Key + "=" + kv.Value + "; ");
    if (particleNames.Count > 0) sb.Append("\n   particle prefabs: " + string.Join(", ", particleNames));
    // Keyword search over prefab names for the things the task asks about.
    foreach (var key in new[] { "grass", "fire", "flame", "campfire", "generator", "rock", "cliff", "boulder", "stone", "smoke", "tent", "tree", "bush", "fern", "lantern", "log", "stump", "cabin", "door" })
    {
        var hits = new System.Collections.Generic.List<string>();
        foreach (var g in prefabs)
        {
            string p = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
            string n = System.IO.Path.GetFileNameWithoutExtension(p);
            if (n.ToLowerInvariant().Contains(key)) hits.Add(n);
        }
        if (hits.Count > 0) sb.Append("\n   " + key + " (" + hits.Count + "): " + string.Join(", ", hits.GetRange(0, System.Math.Min(hits.Count, 8))));
    }
    // Top-level layout of the pack folder.
    sb.Append("\n   folders: " + string.Join(", ", UnityEditor.AssetDatabase.GetSubFolders(root).Select(f => f.Substring(root.Length + 1))));
}
return sb.ToString();
