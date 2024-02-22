Shader "Unlit/Galaxy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float field(inout float3 p, float s, int iter)
            {
                float accum = s / 4.0f;
                float prev = 0.0f;
                float tw = 0.0f;
                for (int i = 0; i < 18; i++)
                {
                    if (i >= iter)
                    {
                        break;
                    }
                    float mag = dot(p, p);
                    p = (abs(p) / mag.xxx) + float3(-0.5f, -0.4, -1.486999988555908203125f);
                    float w = exp((-float(i)) / 5.0f);
                    accum += (w * exp((-9.025) * pow(abs(mag - prev), 2.2)));
                    tw += w;
                    prev = mag;
                }
                return max(0.0f, ((5.2 * accum) / tw) - 0.65);
            }

            float3 nrand3(float2 co)
            {
                float3 a = frac(float3(130000.0f, 470000.0f, 290000.0f) * cos((co.x * 0.008299999870359897613525390625f) + co.y));
                float3 b = frac(float3(810000.0f, 100000.0f, 10000.0f) * sin((co.x * 0.0003000000142492353916168212890625f) + co.y));
                float3 c = lerp(a, b, 0.5f.xxx);
                return c;
            }

            float4 starLayer(float2 p, float time)
            {
                float2 seed = p * 1.9;
                seed = floor((seed * max(1, 600.0f)) / 1.5f.xx);
                float2 param = seed;
                float3 rnd = nrand3(param);
                float4 col = pow(rnd.y, 17.0f).xxxx;
                float mul = 10.0f * rnd.x;
                float3 _197 = col.xyz * ((sin((time * mul) + mul) * 0.25f) + 1.0f);
                col = float4(_197.x, _197.y, _197.z, col.w);
                return col;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y ;
                float2 uv = ((i.uv * 2.0f) / 1) - 1.0f.xx;
                float2 uvs = (uv * 4) ;
                float3 p = float3(uvs / 2.5f.xx, 0.0f) + float3(0.8, -1.3, 0.0f);
                p += (float3(sin(time / 32.0f), sin(time / 24.0f), sin(time / 64.0f)) * 0.45);
                p.x += lerp(-0.02, 0.02, 2);
                p.y += lerp(-0.02, 0.02, 2);
                float freqs[4];
                freqs[0] = 0.45;
                freqs[1] = 0.4;
                freqs[2] = 0.15;
                freqs[3] = 0.9;
                float3 param = p;
                float param_1 = freqs[2];
                int param_2 = 13;
                float _306 = field(param, param_1, param_2);
                float t = _306;
                float v = (1.0f - exp((abs(uv.x) - 1.0f) * 6.0f)) * (1.0f - exp((abs(uv.y) - 1.0f) * 6.0f));
                float3 p2 = float3(uvs / ((((4.0f + (sin(time * 0.11) * 0.2)) + 0.2) + (sin(time * 0.15) * 0.3)) + 0.4).xx, 4.0f) + float3(2.0f, -1.3, -1.0f);
                p2 += (float3(sin(time / 32.0f), sin(time / 24.0f), sin(time / 64.0f)) * 0.16);
                p2.x += lerp(-0.01, 0.01, 2);
                p2.y += lerp(-0.01, 0.01, 2);
                float3 param_3 = p2;
                float param_4 = freqs[3];
                int param_5 = 18;
                float _392 = field(param_3, param_4, param_5);
                float t2 = _392;
                float4 c2 = float4(((5.5f * t2) * t2) * t2, (2.1 * t2) * t2, (2.2 * t2) * freqs[0], t2) * lerp(0.5f, 0.2, v);
                float4 starColour = 0.0f.xxxx;
                float2 param_6 = p.xy;
                float param_7 = time;
                starColour += starLayer(param_6, param_7);
                float2 param_8 = p2.xy;
                float param_9 = time;
                starColour += starLayer(param_8, param_9);
                float4 colour = ((float4((((1.5f * freqs[2]) * t) * t) * t, ((1.2 * freqs[1]) * t) * t, freqs[3] * t, 1.0f) * lerp(freqs[3] - 0.3, 1.0f, v)) + c2) + starColour;
                return float4(colour.xyz * 1.0f, 1.0f);
            }
            ENDCG
        }
    }
}
