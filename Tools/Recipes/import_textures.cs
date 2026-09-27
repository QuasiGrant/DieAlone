UnityEditor.AssetDatabase.Refresh();
var sb = new System.Text.StringBuilder();
foreach (var id in new[] { "Ground054", "PaintedMetal016", "Concrete034", "Planks023A" })
{
    string path = "Assets/Textures/" + id + "/" + id + "_Color.jpg";
    var imp = UnityEditor.AssetImporter.GetAtPath(path) as UnityEditor.TextureImporter;
    if (imp == null) { sb.Append(id + ": importer missing | "); continue; }
    imp.maxTextureSize = 512;
    imp.sRGBTexture = true;
    imp.wrapMode = UnityEngine.TextureWrapMode.Repeat;
    imp.mipmapEnabled = true;
    imp.SaveAndReimport();
    var tex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(path);
    sb.Append(id + ": " + (tex != null ? tex.width + "x" + tex.height : "load failed") + " | ");
}
return sb.ToString();
