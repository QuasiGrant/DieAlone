Shader "DieAlone/Smoke"
{
    // Unlit alpha-blended smoke for particles. Replaces the four pack smoke shaders.
    // It reads the pack smoke textures as masks, never as colour:
    //   _ShadeTex dotted with _ShadeChannel gives how much a pixel faces up toward the sun.
    //   _MaskTex B is the fire-lit (emission) mask, A is the alpha.
    // Colour = body colour, darker on the unlit side, plus fire colour through the mask.
    // Body colour, fire colour, shadow floor and fire strength are globals set by
    // LookEnvironment from LookTuning. Fog is per material so far columns can read through haze.
    Properties
    {
        _ShadeTex ("Shade Texture", 2D) = "white" {}
        _ShadeChannel ("Shade Channel (RGBA mask)", Vector) = (0, 0, 1, 0)
        _MaskTex ("Mask Texture (B fire, A alpha)", 2D) = "white" {}
        _AlphaMultiplier ("Alpha Multiplier", Float) = 1
        _FogAmount ("Fog Amount", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" "PreviewType" = "Plane" }
        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_ShadeTex); SAMPLER(sampler_ShadeTex);
            TEXTURE2D(_MaskTex); SAMPLER(sampler_MaskTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _ShadeTex_ST;
            float4 _ShadeChannel;
            float4 _MaskTex_ST;
            float _AlphaMultiplier;
            float _FogAmount;
            CBUFFER_END

            // Globals from LookEnvironment (LookTuning smoke fields).
            half4 _DA_SmokeBodyColor;
            half4 _DA_SmokeFireColor;
            float _DA_SmokeShadowFloor;
            float _DA_SmokeFireStrength;

            struct Attributes
            {
                float4 positionOS : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                float fogFactor : TEXCOORD1;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.color = v.color;
                o.uv = v.uv;
                o.fogFactor = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half shade = dot(SAMPLE_TEXTURE2D(_ShadeTex, sampler_ShadeTex, TRANSFORM_TEX(i.uv, _ShadeTex)), _ShadeChannel);
                half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, TRANSFORM_TEX(i.uv, _MaskTex));

                half3 body = _DA_SmokeBodyColor.rgb * lerp(_DA_SmokeShadowFloor, 1.0h, saturate(shade));
                half3 color = body * i.color.rgb + _DA_SmokeFireColor.rgb * mask.b * _DA_SmokeFireStrength;
                half alpha = saturate(mask.a * i.color.a * _AlphaMultiplier);

                half3 fogged = MixFog(color, i.fogFactor);
                color = lerp(color, fogged, _FogAmount);
                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}
