// Off-map distance layers (Main3.md 2.11): a flat colour lit by the main light and the ambient, then blended a fixed share
// toward the current fog colour, and never fogged by distance. Each layer's share (near ridges less, far ranges more) keeps
// every layer paler than the one in front on the day looks, and past the fog end, where scene fog would erase them.
Shader "DieAlone/Backdrop"
{
    Properties
    {
        _Color ("Colour", Color) = (0.31, 0.29, 0.17, 1)
        _HazeBlend ("Blend toward the fog colour", Range(0, 1)) = 0.55
        _FireLit ("Lit from below by the fire (vertex colour red marks where)", Range(0, 1)) = 0
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
                half _HazeBlend, _FireLit;
            CBUFFER_END
            half4 _DA_SmokeFireColor; float _DA_BackdropFireStrength;   // LookTuning smokeFireColor and backdropFireStrength (LookEnvironment)

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; half4 color : COLOR; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 normalWS : TEXCOORD0; half fire : TEXCOORD1; };

            Varyings vert(Attributes input)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                o.normalWS = TransformObjectToWorldNormal(input.normalOS);
                o.fire = input.color.r * _FireLit;
                return o;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half3 n = normalize(input.normalWS);
                Light mainLight = GetMainLight();
                half3 lit = _Color.rgb * (mainLight.color * saturate(dot(n, mainLight.direction)) + SampleSH(n));
                // the fire under smoke (RebuildSpecs 4.3): added after the haze, so the lit underside reads through the fog
                return half4(lerp(lit, unity_FogColor.rgb, _HazeBlend) + _DA_SmokeFireColor.rgb * _DA_BackdropFireStrength * input.fire, 1);
            }
            ENDHLSL
        }
    }
}
