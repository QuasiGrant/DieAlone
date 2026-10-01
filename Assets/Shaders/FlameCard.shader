// Flame cards for the far burning ridge and valley (8.9f): one frame of a flipbook flame texture on a vertical quad,
// added over the scene (additive), unlit and never fogged, so the fire front reads through the night fog and the day-two
// haze with the soft, broken tops of real flames. Vertex colour alpha scales a card; _Intensity comes from LookTuning; with _Ramp
// on, the colour follows the brightness through LookTuning's flame colours; the global _DA_FireCardDim dims every card in a look.
Shader "DieAlone/FlameCard"
{
    Properties
    {
        _BaseMap ("Flame flipbook", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 1
        _Ramp ("Colour by brightness (1) or tint only (0)", Float) = 0
        _RampBase ("Core", Color) = (1, 0.91, 0.753, 1)
        _RampBody ("Body", Color) = (1, 0.42, 0.102, 1)
        _RampTip ("Tips", Color) = (0.722, 0.282, 0.11, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" }
        Blend One One
        ZWrite Off
        Cull Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _Color;
                half _Intensity, _Ramp;
                half4 _RampBase, _RampBody, _RampTip;
            CBUFFER_END
            float _DA_FireCardDim;   // LookTuning.fireCardDim, set by LookEnvironment (0 in edit mode: full)

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; half4 color : COLOR; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; half4 color : COLOR; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.color = v.color;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 t = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                half3 c = t.rgb * t.a * _Color.rgb;
                if (_Ramp > 0.5)   // RebuildSpecs 4.1: the brightest part the core, then the body, the dimmest the fading tips
                {
                    half l = saturate(dot(t.rgb, half3(0.299, 0.587, 0.114)) * t.a);
                    half3 ramp = l > 0.66 ? lerp(_RampBody.rgb, _RampBase.rgb, (l - 0.66) / 0.34) : l > 0.33 ? lerp(_RampTip.rgb, _RampBody.rgb, (l - 0.33) / 0.33) : _RampTip.rgb * (l / 0.33);
                    c = ramp * l;
                }
                return half4(c * _Intensity * i.color.a * (1.0 - _DA_FireCardDim), 1);   // no MixFog on purpose
            }
            ENDHLSL
        }
    }
}
