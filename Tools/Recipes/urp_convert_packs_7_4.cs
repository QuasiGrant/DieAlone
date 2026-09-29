// Task 7.4: convert Built-in materials in the bought packs to URP with URP's own material upgraders.
// The Render Pipeline Converter window fails in eval, so this collects the upgraders from URP's
// internal *MaterialUpgraderProvider classes by reflection and calls the public
// UnityEditor.Rendering.MaterialUpgrader.Upgrade(material, list, flags, ref message) per material.
// Upgrade returns true even when no upgrader matches, so success is judged by the shader changing.
// Materials on pack-supplied custom shaders are left alone and reported. Safe to rerun.
var roots = new[] { "Assets/Effigy GameWorks", "Assets/BK", "Assets/NatureManufacture Assets", "Assets/Revolving Pizza Games/Catacombs", "Assets/PSX Edition - Modular Parking Lot", "Assets/Modular Chain Link Fence", "Assets/PSX Farm Tools Pack", "Assets/PSX Supplies Pack" };
var asm = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Unity.RenderPipelines.Universal.Editor");
var upgraders = new System.Collections.Generic.List<UnityEditor.Rendering.MaterialUpgrader>();
foreach (var name in new[] { "StandardMaterialUpgraderProvider", "StandardSimpleLightingUpgraderProvider", "MobileMaterialUpgraderProviders", "TerrainMaterialUpgraderProvider", "ParticleMaterialUpgraderProvider", "AutodeskMaterialUpgraderProvider" })
{
    var t = asm.GetType("UnityEditor.Rendering.Universal." + name);
    var provider = System.Activator.CreateInstance(t, true);
    var list = (System.Collections.IEnumerable)t.GetMethod("GetUpgraders").Invoke(provider, null);
    foreach (var u in list) upgraders.Add((UnityEditor.Rendering.MaterialUpgrader)u);
}
var sb = new System.Text.StringBuilder("upgraders=" + upgraders.Count);
int done = 0;
var skipped = new System.Collections.Generic.Dictionary<string, int>();
foreach (var g in UnityEditor.AssetDatabase.FindAssets("t:Material", roots))
{
    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    if (m == null || m.shader == null) continue;
    var shaderName = m.shader.name;
    if (shaderName.StartsWith("Universal Render Pipeline/")) continue;
    string msg = null;
    UnityEditor.Rendering.MaterialUpgrader.Upgrade(m, upgraders, UnityEditor.Rendering.MaterialUpgrader.UpgradeFlags.None, ref msg);
    if (m.shader.name != shaderName)
    {
        UnityEditor.EditorUtility.SetDirty(m);
        done++;
    }
    else skipped[shaderName] = skipped.ContainsKey(shaderName) ? skipped[shaderName] + 1 : 1;
}
UnityEditor.AssetDatabase.SaveAssets();
sb.Append(" converted=" + done + " left: ");
foreach (var kv in skipped) sb.Append(kv.Key + "=" + kv.Value + "; ");
return sb.ToString();
