Shader "DieAlone/FireStandIn"
{
    // Gray-blockout stand-in fire: flat unlit colour that ignores fog on purpose, so the far ridge and valley fires
    // read through the scene's distance fog at night and on day two. Colour and strength come from LookTuning
    // (fireGlowColor, fireGlowIntensity), written into the material by Tools/Recipes/main3_8_7_ward.cs.
    Properties
    {
        _Color ("Color", Color) = (1, 0.42, 0.1, 1)
        _Intensity ("Intensity", Float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Geometry" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _Color;
            float _Intensity;

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionCS : SV_POSITION; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return half4(_Color.rgb * _Intensity, 1);   // no MixFog: fog never hides the fire
            }
            ENDHLSL
        }
    }
}
