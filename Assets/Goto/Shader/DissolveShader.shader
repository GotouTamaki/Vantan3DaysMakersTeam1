Shader "Custom/DissolveShader"
{
    Properties
    {
        [HDR] _MainColor ("Main Color", Color) = (0, 0, 0, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1.2)) = 0.5
        [HDR] _EdgeColor ("Edge Color", Color) = (0, 0, 1, 1)
        _EdgeWidth ("Edge Width", Range(0,0.2)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="AlphaTest" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex); SAMPLER(sampler_NoiseTex);
            float4 _MainColor;
            float _DissolveAmount;
            float4 _EdgeColor;
            float _EdgeWidth;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 baseColor = _MainColor * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                half noiseValue = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, IN.uv).r;

                // �G�b�W�̌v�Z
                float edge = smoothstep(noiseValue, _DissolveAmount + _EdgeWidth, _EdgeWidth);

                // �N���b�s���O�iAlphaClip�j
                clip(noiseValue - _DissolveAmount); // �������l�𒴂��������̂ݕ`��

                // �G�b�W�����̃J���[
                half4 finalColor = lerp(_EdgeColor, baseColor, edge);

                return finalColor;
            }
            ENDHLSL
        }
    }
}
