Shader "DieAlone/HorizonGlow"
{
    // Additive, unlit, and ignores fog on purpose: fire glow that reads through haze.
    // Alpha falls off toward the top of the quad so a strip fades into the sky.
    Properties
    {
        _Color ("Color", Color) = (1, 0.45, 0.12, 1)
        _Intensity ("Intensity", Float) = 1
        _Falloff ("Falloff", Float) = 1.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend One One
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _Color;
            float _Intensity, _Falloff;

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float a = pow(saturate(1.0 - i.uv.y), _Falloff);
                float edge = sin(i.uv.x * 3.14159);          // soften the left and right ends
                return half4(_Color.rgb * _Intensity * a * edge, 1);
            }
            ENDHLSL
        }
    }
}
