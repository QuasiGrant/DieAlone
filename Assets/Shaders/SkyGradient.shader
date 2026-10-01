Shader "DieAlone/SkyGradient"
{
    // Three-band sky gradient with a soft glow around the sun direction. Written for
    // this project; colors come from LookTuning through LookEnvironment.
    Properties
    {
        _TopColor ("Top", Color) = (0.25, 0.12, 0.10, 1)
        _HorizonColor ("Horizon", Color) = (0.85, 0.42, 0.20, 1)
        _GroundColor ("Ground", Color) = (0.30, 0.16, 0.10, 1)
        _SunGlowColor ("Sun Glow", Color) = (1.0, 0.6, 0.3, 1)
        _SunGlowSize ("Sun Glow Size", Float) = 24
        _SunDir ("Sun Direction", Vector) = (0, 0.25, -1, 0)
        _SkyBandSin ("Sine of the horizon band height", Float) = 0
        _SkyBlendRate ("Blend rate from the band to the top", Float) = 1.8
        _WestGlowColor ("Western fire glow", Color) = (0, 0, 0, 1)
        _WestGlowCos ("Cosine of the glow's half width from due west", Float) = 0.225
        _WestGlowTopSin ("Sine of the height where the glow has faded", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _TopColor, _HorizonColor, _GroundColor, _SunGlowColor, _WestGlowColor;
            float _SunGlowSize, _SkyBandSin, _SkyBlendRate, _WestGlowCos, _WestGlowTopSin;
            float4 _SunDir;

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 dirWS : TEXCOORD0; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.dirWS = TransformObjectToWorld(v.positionOS.xyz) - GetCameraPositionWS();
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 d = normalize(i.dirWS);
                float up = d.y;
                float3 col = up >= 0.0
                    ? lerp(_HorizonColor.rgb, _TopColor.rgb, saturate((up - _SkyBandSin) * _SkyBlendRate))   // holds the horizon colour up to the band, then blends at _SkyBlendRate (LookTuning.skyBlendRate)
                    : lerp(_HorizonColor.rgb, _GroundColor.rgb, saturate(-up * 4.0));
                float toSun = saturate(dot(d, normalize(-_SunDir.xyz)));
                col += _SunGlowColor.rgb * pow(toSun, _SunGlowSize);
                // the fire front's glow on the western horizon (RebuildSpecs 4.4): full across the front's bearings, fading to nothing by
                // _WestGlowTopSin up and softly at the front's ends
                float2 flat = normalize(d.xz + float2(1e-5, 0));
                float across = smoothstep(_WestGlowCos, _WestGlowCos + 0.15, -flat.x);
                col += _WestGlowColor.rgb * across * saturate(1.0 - max(up, 0.0) / _WestGlowTopSin);
                return half4(col, 1);
            }
            ENDHLSL
        }
    }
}
