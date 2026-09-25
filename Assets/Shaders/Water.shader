Shader "DieAlone/Water"
{
    // Still lake water for the PS1 look: dark body, crossing sine waves, a sun glint band
    // from a faked wave normal, a little sky at grazing angles, scene fog. No depth or reflection textures.
    Properties
    {
        _DeepColor ("Deep Color", Color) = (0.02, 0.05, 0.08, 1)
        _SkyColor ("Sky Tint", Color) = (0.45, 0.22, 0.12, 1)
        _GlintColor ("Glint Color", Color) = (1.0, 0.6, 0.3, 1)
        _WaveScale ("Wave Scale", Float) = 0.9
        _WaveSpeed ("Wave Speed", Float) = 0.9
        _WaveHeight ("Wave Normal Strength", Range(0, 1)) = 0.35
        _GlintPower ("Glint Tightness", Range(4, 200)) = 40
        _GlintStrength ("Glint Strength", Range(0, 2)) = 0.9
        _Opacity ("Opacity", Range(0, 1)) = 0.88
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            half4 _DeepColor;
            half4 _SkyColor;
            half4 _GlintColor;
            float _WaveScale;
            float _WaveSpeed;
            float _WaveHeight;
            float _GlintPower;
            float _GlintStrength;
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

            // Two crossing wave trains; returns height and its slope so a normal can be faked.
            float3 Waves(float2 p, float t)
            {
                float a1 = p.x * _WaveScale + p.y * _WaveScale * 0.35 + t * 1.1;
                float a2 = p.y * _WaveScale * 1.3 - p.x * _WaveScale * 0.25 - t * 0.8;
                float h = sin(a1) * 0.6 + sin(a2) * 0.4;
                float dx = cos(a1) * 0.6 * _WaveScale - cos(a2) * 0.4 * _WaveScale * 0.25;
                float dz = cos(a1) * 0.6 * _WaveScale * 0.35 + cos(a2) * 0.4 * _WaveScale * 1.3;
                return float3(h, dx, dz);
            }

            half4 frag(Varyings i) : SV_Target
            {
                float t = _Time.y * _WaveSpeed;
                float3 w = Waves(i.positionWS.xz, t);
                float3 n = normalize(float3(-w.y * _WaveHeight, 1.0, -w.z * _WaveHeight));
                float3 viewDir = normalize(GetCameraPositionWS() - i.positionWS);
                Light sun = GetMainLight();
                float3 r = reflect(-viewDir, n);
                float glint = pow(saturate(dot(r, sun.direction)), _GlintPower) * _GlintStrength;
                float fresnel = pow(1.0 - saturate(dot(viewDir, n)), 3.0);
                half3 col = lerp(_DeepColor.rgb, _SkyColor.rgb, saturate(fresnel * 0.55));
                col += _GlintColor.rgb * glint * sun.color;
                col += w.x * 0.015;   // faint band shading so the surface reads even without a glint
                col = MixFog(col, i.fogFactor);
                return half4(col, _Opacity);
            }
            ENDHLSL
        }
    }
}
