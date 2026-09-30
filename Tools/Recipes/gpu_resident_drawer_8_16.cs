// 8.16 (edit mode; Wren, frame rate): turns on URP's GPU Resident Drawer (Unity 6000.3, render-pipelines.core GPUDriven), which draws
// MeshRenderers through BatchRendererGroup. Its validator (GPUResidentDrawer.Validator.cs) needs the Forward+ path (PC_Renderer is
// ForwardPlus) and the "BatchRendererGroup Variants" graphics setting on Keep All (m_BrgStripping 2; EditorGraphicsSettings exposes it
// read-only, so it is set on the GraphicsSettings asset); the URP asset's gpuResidentDrawerMode goes to InstancedDrawing.
if (UnityEngine.Application.isPlaying) return "stop play mode first";
const int brgKeepAll = 2;   // UnityEditor.Rendering.BatchRendererGroupStrippingMode.KeepAll
var ua = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset; if (ua == null) return "no URP asset";
var gs = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset"); if (gs == null || gs.Length == 0) return "no GraphicsSettings asset";
var so = new UnityEditor.SerializedObject(gs[0]); var brg = so.FindProperty("m_BrgStripping"); if (brg == null) return "no m_BrgStripping";
brg.intValue = brgKeepAll; so.ApplyModifiedPropertiesWithoutUndo();
ua.gpuResidentDrawerMode = UnityEngine.Rendering.GPUResidentDrawerMode.InstancedDrawing;
UnityEditor.EditorUtility.SetDirty(ua); UnityEditor.AssetDatabase.SaveAssetIfDirty(ua); UnityEditor.AssetDatabase.SaveAssets();
return "gpuResidentDrawerMode " + ua.gpuResidentDrawerMode + ", BRG variants " + UnityEditor.Rendering.EditorGraphicsSettings.batchRendererGroupShaderStrippingMode;
