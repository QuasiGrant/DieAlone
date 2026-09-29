// Off-map distance layers (Main3.md 2.11): a flat colour lit by the main light and the ambient, then blended a fixed share
// toward the current fog colour, and never fogged by distance. Each layer's share (near ridges less, far ranges more) keeps
// every layer paler than the one in front on the day looks, and past the fog end, where scene fog would erase them.
Shader "DieAlone/Backdrop"
{
    Properties
    {
        _Color ("Colour", Color) = (0.31, 0.29, 0.17, 1)
        _HazeBlend ("Blend toward the fog colour", Range(0, 1)) = 0.55
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }
        Pass
        {
            Name "Backdrop"
            Tags { "LightMode" = "UniversalForward" }
            Cull Off   // bands and skirts are seen from either side
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half _HazeBlend;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 normalWS : TEXCOORD0; };

            Varyings vert(Attributes input)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                o.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return o;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half3 n = normalize(input.normalWS);
                Light mainLight = GetMainLight();
                half3 lit = _Color.rgb * (mainLight.color * saturate(dot(n, mainLight.direction)) + SampleSH(n));
                return half4(lerp(lit, unity_FogColor.rgb, _HazeBlend), 1);
            }
            ENDHLSL
        }
    }
}
