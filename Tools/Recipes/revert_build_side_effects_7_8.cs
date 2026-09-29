// Task 7.8: undo what a release player build writes into the settings, through the Editor.
// A build recomputes the URP shader prefilter flags on PC_RPAsset, adds the recreated
// Assets/DefaultVolumeProfile.asset to PlayerSettings preloaded assets, and rewrites
// Assets/Settings/UniversalRenderPipelineGlobalSettings.asset (default volume profile and the
// m_RuntimeSettings list). This fixes the first two. The global settings file cannot be put back from
// the Editor: it holds the runtime list empty and saves it as m_List: [], never the committed build
// snapshot, so this recipe does not touch it. Grant restores it with
// git checkout -- Assets/Settings/UniversalRenderPipelineGlobalSettings.asset
// and the recreated profile is moved out of Assets to Archive/<date>-test-build/.
// Edit mode only. Safe to rerun.
if (UnityEditor.EditorApplication.isPlaying) return "edit mode only";
const string RpAssetPath = "Assets/Settings/PC_RPAsset.asset";
var log = new System.Text.StringBuilder();

// 1. Preloaded assets back to empty.
UnityEditor.PlayerSettings.SetPreloadedAssets(new UnityEngine.Object[0]);
log.AppendLine("preloadedAssets cleared");

// 2. Shader prefilter flags on the pipeline asset, committed values.
var rp = UnityEditor.AssetDatabase.LoadMainAssetAtPath(RpAssetPath);
var rpSo = new UnityEditor.SerializedObject(rp);
void SetInt(string name, int v) { rpSo.FindProperty(name).intValue = v; log.AppendLine(name + " = " + v); }
SetInt("m_PrefilteringModeScreenSpaceOcclusion", 2);
SetInt("m_PrefilterSSAODepthNormals", 0);
SetInt("m_PrefilterSSAOBlueNoise", 0);
SetInt("m_PrefilterSSAOSampleCountMedium", 0);
rpSo.ApplyModifiedPropertiesWithoutUndo();

UnityEditor.EditorUtility.SetDirty(rp);
UnityEditor.AssetDatabase.SaveAssetIfDirty(rp);
UnityEditor.AssetDatabase.SaveAssets();
return log.ToString();
