// Lists every material under Assets by shader name and counts the ones still on a pack shader
// (BK/*, NatureManufacture/*, Legacy Shaders/*). Task 7.6 done-check: pack shaders = 0.
// Also flags any material whose shader failed to compile or is missing (renders pink).
var packPrefixes = new[] { "BK/", "NatureManufacture/", "Legacy Shaders/" };
var counts = new System.Collections.Generic.SortedDictionary<string, int>();
var pack = new System.Collections.Generic.List<string>();
var broken = new System.Collections.Generic.List<string>();
foreach (var g in UnityEditor.AssetDatabase.FindAssets("t:Material", new[] { "Assets" }))
{
    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
    if (!path.EndsWith(".mat")) continue;
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    var name = m == null || m.shader == null ? "(missing)" : m.shader.name;
    counts[name] = counts.ContainsKey(name) ? counts[name] + 1 : 1;
    if (packPrefixes.Any(p => name.StartsWith(p))) pack.Add(name + " | " + path);
    if (m == null || m.shader == null || !m.shader.isSupported || UnityEditor.ShaderUtil.ShaderHasError(m.shader) || name == "Hidden/InternalErrorShader") broken.Add(name + " | " + path);
}
var sb = new System.Text.StringBuilder();
foreach (var kv in counts) sb.AppendLine(kv.Key + " = " + kv.Value);
sb.AppendLine("pack shaders = " + pack.Count);
foreach (var p in pack) sb.AppendLine("  " + p);
sb.AppendLine("broken shaders = " + broken.Count);
foreach (var b in broken) sb.AppendLine("  " + b);
return sb.ToString();
