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

            float4 _TopColor, _HorizonColor, _GroundColor, _SunGlowColor;
            float _SunGlowSize;
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
                    ? lerp(_HorizonColor.rgb, _TopColor.rgb, saturate(up * 1.8))
                    : lerp(_HorizonColor.rgb, _GroundColor.rgb, saturate(-up * 4.0));
                float toSun = saturate(dot(d, normalize(-_SunDir.xyz)));
                col += _SunGlowColor.rgb * pow(toSun, _SunGlowSize);
                return half4(col, 1);
            }
            ENDHLSL
        }
    }
}
