Shader "DieAlone/Water"
{
    // Still lake water for the PS1 look: flat colour that tips toward the sky at grazing angles,
    // a slow sine ripple, scene fog. No reflections, no depth texture, cheap.
    Properties
    {
        _DeepColor ("Deep Color", Color) = (0.03, 0.07, 0.09, 1)
        _SkyColor ("Sky Tint", Color) = (0.55, 0.28, 0.16, 1)
        _RippleScale ("Ripple Scale", Float) = 0.6
        _RippleSpeed ("Ripple Speed", Float) = 0.5
        _RippleStrength ("Ripple Strength", Range(0, 0.3)) = 0.08
        _Opacity ("Opacity", Range(0, 1)) = 0.9
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        Pass
        {
            Name "Forward"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            half4 _DeepColor;
            half4 _SkyColor;
            float _RippleScale;
            float _RippleSpeed;
            float _RippleStrength;
            float _Opacity;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float fogFactor : TEXCOORD1;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionWS = TransformObjectToWorld(v.positionOS.xyz);
                o.positionCS = TransformWorldToHClip(o.positionWS);
                o.fogFactor = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float t = _Time.y * _RippleSpeed;
                float ripple = sin(i.positionWS.x * _RippleScale + t) * sin(i.positionWS.z * _RippleScale * 1.37 - t * 0.8);
                float3 viewDir = normalize(GetCameraPositionWS() - i.positionWS);
                float fresnel = pow(1.0 - saturate(viewDir.y), 2.5);
                half3 col = lerp(_DeepColor.rgb, _SkyColor.rgb, saturate(fresnel * 0.8 + ripple * _RippleStrength));
                col = MixFog(col, i.fogFactor);
                return half4(col, _Opacity);
            }
            ENDHLSL
        }
    }
}
