Shader "DieAlone/LookFilter"
{
    // Full-screen filter for the VHS look. Pass 0 upsamples the low-resolution picture
    // with bilinear filtering (the soft look), then applies tape color, grain, and the
    // rolling noise band. Every number comes from LookTuning through LookFilterFeature.
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
            float _GrainStrength;  // 0..1
            float _GrainSpeed;     // pattern changes per second
            float _BandStrength;   // 0..1
            float _BandSpeed;      // screen heights per second
            float _BandInterval;   // seconds between bands
            float _LookTime;       // seconds, unscaled
            float4 _LookTexel;     // xy = 1/size of the low-res picture, zw = size

            static const float3 LUMA = float3(0.299, 0.587, 0.114);

            // Integer hash (PCG style), 0..1. Stays random on whole-number inputs,
            // which the simpler fractional hashes do not.
            uint Pcg(uint v)
            {
                uint state = v * 747796405u + 2891336453u;
                uint word = ((state >> ((state >> 28u) + 4u)) ^ state) * 277803737u;
                return (word >> 22u) ^ word;
            }
            float Hash(float2 p)
            {
                uint2 q = uint2(int2(floor(p)) + 32768);
                uint h = Pcg(q.x ^ Pcg(q.y + 0x9E3779B9u));
                return h * (1.0 / 4294967296.0);
            }

            float3 SampleColor(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).rgb;
            }

            // Returns 0..1 how much this row sits inside the current noise band, and writes
            // a per-row sideways tear offset in low-res pixels.
            float BandMask(float y, out float tear)
            {
                tear = 0;
                if (_BandStrength <= 0.0) return 0;
                float interval = max(_BandInterval, 0.1);
                float cycle = floor(_LookTime / interval);
                float phase = frac(_LookTime / interval);
                // Each cycle the band starts at a random moment and rolls bottom to top.
                float start = Hash(float2(cycle, 7.0)) * 0.6;
                float travel = (phase - start) * interval * _BandSpeed;   // screen heights travelled
                if (travel < 0.0 || travel > 1.15) return 0;
                float center = travel - 0.075;
                float halfHeight = 0.045;
                float mask = 1.0 - smoothstep(halfHeight * 0.4, halfHeight, abs(y - center));
                float rowSeed = floor(y * _LookTexel.w) + cycle * 31.0;
                tear = (Hash(float2(rowSeed, _LookTime * 60.0)) - 0.5) * 6.0 * mask;
                return mask;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord;

                float tear;
                float band = BandMask(uv.y, tear);
                uv.x += tear * _LookTexel.x;

                float3 sharp = SampleColor(uv);
                float luma = dot(sharp, LUMA);

                // Color bleed: keep brightness sharp, average the color part over a sideways span.
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

                // Work in gamma space from here, where the eye judges brightness.
                float3 gammaColor = LinearToSRGB(color);

                // Crushed blacks: raise the black point so shadow detail collapses.
                float crush = _CrushBlacks * 0.2;
                gammaColor = saturate((gammaColor - crush) / max(1.0 - crush, 0.001));

                // Grain: per low-res pixel, pattern re-rolled grainSpeed times a second.
                float2 cell = floor(uv * _LookTexel.zw);
                float frame = floor(_LookTime * _GrainSpeed);
                float grain = Hash(cell + frame * 17.0) - 0.5;
                gammaColor += grain * _GrainStrength * 0.25;

                // Noise band: static mixed in where the band is.
                float staticNoise = Hash(cell + float2(floor(_LookTime * 120.0) * 13.0, 977.0));
                gammaColor = lerp(gammaColor, staticNoise.xxx * 0.9 + 0.05, band * _BandStrength);

                color = SRGBToLinear(saturate(gammaColor));
                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
}
