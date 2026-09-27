UnityEditor.AssetDatabase.Refresh();
string newPath = "Assets/Textures/PaintedMetal006/PaintedMetal006_Color.jpg";
var imp = UnityEditor.AssetImporter.GetAtPath(newPath) as UnityEditor.TextureImporter;
if (imp == null) return "new texture not imported yet";
imp.maxTextureSize = 512; imp.sRGBTexture = true; imp.wrapMode = UnityEngine.TextureWrapMode.Repeat; imp.mipmapEnabled = true;
imp.SaveAndReimport();
var newTex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(newPath);
if (newTex == null) return "new texture failed to load";

int retargeted = 0;
foreach (var guid in UnityEditor.AssetDatabase.FindAssets("PaintedMetal016 t:Material", new[] { "Assets/Materials" }))
{
    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
    var m = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(path);
    m.SetTexture("_BaseMap", newTex);
    UnityEditor.EditorUtility.SetDirty(m);
    string newName = System.IO.Path.GetFileNameWithoutExtension(path).Replace("PaintedMetal016", "PaintedMetal006");
    string err = UnityEditor.AssetDatabase.RenameAsset(path, newName);
    if (!string.IsNullOrEmpty(err)) return "rename failed: " + err;
    retargeted++;
}
UnityEditor.AssetDatabase.SaveAssets();
bool deleted = UnityEditor.AssetDatabase.DeleteAsset("Assets/Textures/PaintedMetal016");
UnityEditor.AssetDatabase.SaveAssets();
// Sanity: nothing left pointing at the old texture.
int stale = UnityEditor.AssetDatabase.FindAssets("PaintedMetal016", new[] { "Assets" }).Length;
return "retargeted=" + retargeted + " oldFolderDeleted=" + deleted + " staleRefs=" + stale + " newTex=" + newTex.width + "x" + newTex.height;
