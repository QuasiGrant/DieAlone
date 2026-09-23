using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

/// URP renderer feature for the VHS look. Runs on the game camera only, before
/// post-processing, so screen-space overlay UI stays sharp. Reads every number
/// from the LookTuning asset each frame, so Play-mode edits show at once.
///
/// Pass order: camera color -> low-resolution texture (bilinear downsample)
///             low-resolution texture -> camera color through LookFilter.shader (bilinear upsample)
public class LookFilterFeature : ScriptableRendererFeature
{
    [SerializeField] private LookTuning tuning;
    [SerializeField] private Shader shader;

    private Material material;
    private LookFilterPass pass;

    public override void Create()
    {
        if (shader != null) material = CoreUtils.CreateEngineMaterial(shader);
        pass = new LookFilterPass { renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material == null || tuning == null || !tuning.filterEnabled) return;
        if (renderingData.cameraData.cameraType != CameraType.Game) return;
        pass.Setup(material, tuning);
        renderer.EnqueuePass(pass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(material);
        material = null;
    }

    private class LookFilterPass : ScriptableRenderPass
    {
        private Material material;
        private LookTuning tuning;

        public void Setup(Material m, LookTuning t)
        {
            material = m;
            tuning = t;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resources = frameData.Get<UniversalResourceData>();
            var camera = frameData.Get<UniversalCameraData>();
            if (resources.isActiveTargetBackBuffer) return;

            TextureHandle source = resources.activeColorTexture;

            var desc = camera.cameraTargetDescriptor;
            int height = Mathf.Clamp(tuning.lowResHeight, 64, desc.height);
            int width = Mathf.Max(64, Mathf.RoundToInt(height * (float)desc.width / desc.height));
            desc.width = width;
            desc.height = height;
            desc.depthBufferBits = 0;
            desc.msaaSamples = 1;
            TextureHandle low = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "_LookLowRes", false, FilterMode.Bilinear);

            renderGraph.AddBlitPass(source, low, Vector2.one, Vector2.zero, passName: "Look Downsample");

            material.SetFloat("_ColorBleed", tuning.colorBleed);
            material.SetFloat("_WashOut", tuning.washOut);
            material.SetFloat("_CrushBlacks", tuning.crushBlacks);
            material.SetVector("_LookTexel", new Vector4(1f / width, 1f / height, width, height));

            material.SetFloat("_GrainStrength", tuning.grainStrength);
            material.SetFloat("_GrainSpeed", tuning.grainSpeed);
            material.SetFloat("_BandStrength", tuning.noiseBandStrength);
            material.SetFloat("_BandSpeed", tuning.noiseBandSpeed);
            material.SetFloat("_BandInterval", tuning.noiseBandInterval);
            material.SetFloat("_LookTime", Time.unscaledTime);

            var upsample = new RenderGraphUtils.BlitMaterialParameters(low, source, material, 0);
            renderGraph.AddBlitPass(upsample, passName: "Look Filter");
        }
    }
}
