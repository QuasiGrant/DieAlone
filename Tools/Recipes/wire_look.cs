UnityEditor.AssetDatabase.Refresh();
var sb = new System.Text.StringBuilder();
var shader = UnityEngine.Shader.Find("DieAlone/LookFilter");
if (shader == null) return "shader not found";
bool shaderError = UnityEditor.ShaderUtil.ShaderHasError(shader);
sb.Append("shaderHasError=" + shaderError);
if (shaderError)
{
    foreach (var msg in UnityEditor.ShaderUtil.GetShaderMessages(shader)) sb.Append(" | " + msg.severity + " line " + msg.line + ": " + msg.message);
    return sb.ToString();
}

const string tuningPath = "Assets/Settings/LookTuning.asset";
var tuning = UnityEditor.AssetDatabase.LoadAssetAtPath<LookTuning>(tuningPath);
if (tuning == null)
{
    tuning = UnityEngine.ScriptableObject.CreateInstance<LookTuning>();
    UnityEditor.AssetDatabase.CreateAsset(tuning, tuningPath);
    sb.Append(" | created LookTuning.asset");
}

var data = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.Universal.ScriptableRendererData>("Assets/Settings/PC_Renderer.asset");
foreach (var f in data.rendererFeatures) if (f is LookFilterFeature) return sb + " | feature already on PC_Renderer";

var feature = UnityEngine.ScriptableObject.CreateInstance<LookFilterFeature>();
feature.name = "LookFilterFeature";
var soF = new UnityEditor.SerializedObject(feature);
soF.FindProperty("tuning").objectReferenceValue = tuning;
soF.FindProperty("shader").objectReferenceValue = shader;
soF.ApplyModifiedPropertiesWithoutUndo();

UnityEditor.AssetDatabase.AddObjectToAsset(feature, data);
UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(feature, out string guid, out long localId);
var so = new UnityEditor.SerializedObject(data);
var feats = so.FindProperty("m_RendererFeatures");
feats.arraySize++;
feats.GetArrayElementAtIndex(feats.arraySize - 1).objectReferenceValue = feature;
var map = so.FindProperty("m_RendererFeatureMap");
map.arraySize++;
map.GetArrayElementAtIndex(map.arraySize - 1).longValue = localId;
so.ApplyModifiedPropertiesWithoutUndo();
UnityEditor.EditorUtility.SetDirty(data);
UnityEditor.AssetDatabase.SaveAssets();
// Ask the renderer to rebuild with the new feature list (internal SetDirty on ScriptableRendererData).
var m = typeof(UnityEngine.Rendering.Universal.ScriptableRendererData).GetMethod("SetDirty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
if (m != null) m.Invoke(data, null);

sb.Append(" | features now=" + data.rendererFeatures.Count + " tuning=" + tuning.filterEnabled + "/" + tuning.lowResHeight);
return sb.ToString();
