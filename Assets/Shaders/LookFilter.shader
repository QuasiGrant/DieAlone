Shader "DieAlone/LookFilter"
{
    // Full-screen filter for the VHS look. Pass 0 upsamples the low-resolution picture
    // with bilinear filtering (the soft look) and applies the tape color effects.
    // Every number comes from LookTuning through LookFilterFeature.
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off Cull Off ZTest Always

        Pass
        {
            Name "LookFilter"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            float _ColorBleed;     // 0..1, sideways chroma smear
            float _WashOut;        // 0..1, saturation loss
            float _CrushBlacks;    // 0..1, shadow crush
            float4 _LookTexel;     // xy = 1/size of the low-res picture, zw = size

            static const float3 LUMA = float3(0.299, 0.587, 0.114);

            float3 SampleColor(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).rgb;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord;

                float3 sharp = SampleColor(uv);
                float luma = dot(sharp, LUMA);

                // Color bleed: keep brightness sharp, average the color part over a sideways span.
                // Tape stored color at far lower detail than brightness, so it smears horizontally.
                float bleedPixels = _ColorBleed * 6.0;
                float3 chroma = 0;
                [unroll]
                for (int i = -3; i <= 3; i++)
                {
                    float2 offset = float2(i * bleedPixels / 3.0 * _LookTexel.x, 0);
                    float3 c = SampleColor(uv + offset);
                    chroma += c - dot(c, LUMA);
                }
                chroma /= 7.0;
                float3 color = luma + chroma;

                // Washed out: pull color toward its own gray.
                color = lerp(color, luma.xxx, _WashOut * 0.7);

                // Crushed blacks: raise the black point so shadow detail collapses.
                // Done in gamma space, where the eye judges it; in linear the same shift looks huge.
                float crush = _CrushBlacks * 0.2;
                float3 gammaColor = LinearToSRGB(color);
                gammaColor = saturate((gammaColor - crush) / max(1.0 - crush, 0.001));
                color = SRGBToLinear(gammaColor);

                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
}
