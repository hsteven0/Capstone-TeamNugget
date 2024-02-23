Shader "Unlit/Desert"
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
            
            static float gT;
            
            float mod(float x, float y)
            {
                return x - y * floor(x / y);
            }
            
            float2 mod(float2 x, float2 y)
            {
                return x - y * floor(x / y);
            }
            
            float3 mod(float3 x, float3 y)
            {
                return x - y * floor(x / y);
            }
            
            float4 mod(float4 x, float4 y)
            {
                return x - y * floor(x / y);
            }
            
            float2 path(float z)
            {
                return float2(4.0f * sin(z * 0.100000001490116119384765625f), 0.0f);
            }
            
            float n2D(inout float2 p)
            {
                float2 i = floor(p);
                p -= i;
                p *= (p * (3.0f.xx - (p * 2.0f)));
                float4 _341 = frac(sin(mod(float4(0.0f, 1.0f, 113.0f, 114.0f) + dot(i, float2(1.0f, 113.0f)).xxxx, 6.283185482025146484375f.xxxx)) * 43758.546875f);
                return dot(mul(float2(1.0f - p.y, p.y), float2x2(float2(_341.xy), float2(_341.zw))), float2(1.0f - p.x, p.x));
            }
            
            float camSurfFunc(inout float3 p)
            {
                p /= 2.5f.xxx;
                float2 param = p.xz * 0.20000000298023223876953125f;
                float _597 = n2D(param);
                float layer1 = (_597 * 2.0f) - 0.5f;
                layer1 = smoothstep(0.0f, 1.0499999523162841796875f, layer1);
                float2 param_1 = p.xz * 0.2750000059604644775390625f;
                float _607 = n2D(param_1);
                float layer2 = _607;
                layer2 = 1.0f - (abs(layer2 - 0.5f) * 2.0f);
                layer2 = smoothstep(0.20000000298023223876953125f, 1.0f, layer2 * layer2);
                float res = ((layer1 * 0.699999988079071044921875f) + (layer2 * 0.25f)) / 0.949999988079071044921875f;
                return res;
            }
            
            float2x2 rot2(float a)
            {
                float c = cos(a);
                float s = sin(a);
                return float2x2(float2(c, s), float2(-s, c));
            }
            
            float surfFunc(inout float3 p)
            {
                p /= 2.5f.xxx;
                float2 param = p.xz * 0.20000000298023223876953125f;
                float _547 = n2D(param);
                float layer1 = (_547 * 2.0f) - 0.5f;
                layer1 = smoothstep(0.0f, 1.0499999523162841796875f, layer1);
                float2 param_1 = p.xz * 0.2750000059604644775390625f;
                float _559 = n2D(param_1);
                float layer2 = _559;
                layer2 = 1.0f - (abs(layer2 - 0.5f) * 2.0f);
                layer2 = smoothstep(0.20000000298023223876953125f, 1.0f, layer2 * layer2);
                float2 param_2 = (p.xz * 0.5f) * 3.0f;
                float _575 = n2D(param_2);
                float layer3 = _575;
                float res = ((layer1 * 0.699999988079071044921875f) + (layer2 * 0.25f)) + (layer3 * 0.0500000007450580596923828125f);
                return res;
            }
            
            float map(float3 p)
            {
                float3 param = p;
                float _631 = surfFunc(param);
                float sf = _631;
                return p.y + ((0.5f - sf) * 2.0f);
            }
            
            float trace(float3 ro, float3 rd)
            {
                float t = 0.0f;
                for (int i = 0; i < 96; i++)
                {
                    float3 param = ro + (rd * t);
                    float h = map(param);
                    if ((abs(h) < (0.001000000047497451305389404296875f * ((t * 0.125f) + 1.0f))) || (t > 80.0f))
                    {
                        break;
                    }
                    t += h;
                }
                return min(t, 80.0f);
            }
            
            float3 normal(float3 p, float ef)
            {
                float sgn = 1.0f;
                float3 e = float3(0.001000000047497451305389404296875f * ef, 0.0f, 0.0f);
                float3 mp = e.zzz;
                for (int i = min(2, 0); i < 6; i++)
                {
                    float3 param = p + (e * sgn);
                    mp.x += (map(param) * sgn);
                    sgn = -sgn;
                    if ((i & 1) == 1)
                    {
                        mp = mp.yzx;
                        e = e.zxy;
                    }
                }
                return normalize(mp);
            }
            
            float2 hash22(inout float2 p)
            {
                float n = sin(dot(p, float2(113.0f, 1.0f)));
                p = (frac(float2(2097152.0f, 262144.0f) * n) * 2.0f) - 1.0f.xx;
                return p;
            }
            
            float gradN2D(inout float2 f)
            {
                float2 p = floor(f);
                f -= p;
                float2 w = (f * f) * (3.0f.xx - (f * 2.0f));
                float2 param = p + 0.0f.xx;
                float2 _247 = hash22(param);
                float2 param_1 = p + float2(1.0f, 0.0f);
                float2 _255 = hash22(param_1);
                float2 param_2 = p + float2(0.0f, 1.0f);
                float2 _268 = hash22(param_2);
                float2 param_3 = p + 1.0f.xx;
                float2 _276 = hash22(param_3);
                float c = lerp(lerp(dot(_247, f - 0.0f.xx), dot(_255, f - float2(1.0f, 0.0f)), w.x), lerp(dot(_268, f - float2(0.0f, 1.0f)), dot(_276, f - 1.0f.xx), w.x), w.y);
                return (c * 0.5f) + 0.5f;
            }
            
            float grad(inout float x, float offs)
            {
                x = abs(frac(((x / 6.28299999237060546875f) + offs) - 0.25f) - 0.5f) * 2.0f;
                float x2 = clamp((x * x) * ((-1.0f) + (2.0f * x)), 0.0f, 1.0f);
                x = smoothstep(0.0f, 1.0f, x);
                return lerp(x, x2, 0.1500000059604644775390625f);
            }
            
            float sandL(float2 p)
            {
                float param = 0.17453277111053466796875f;
                float2 q = mul(p, rot2(param));
                float2 param_1 = q * 18.0f;
                float _403 = gradN2D(param_1);
                q.y += ((_403 - 0.5f) * 0.0500000007450580596923828125f);
                float param_2 = q.y * 80.0f;
                float param_3 = 0.0f;
                float _418 = grad(param_2, param_3);
                float grad1 = _418;
                float param_4 = -0.15707950294017791748046875f;
                q = mul(p, rot2(param_4));
                float2 param_5 = q * 12.0f;
                float _428 = gradN2D(param_5);
                q.y += ((_428 - 0.5f) * 0.0500000007450580596923828125f);
                float param_6 = q.y * 80.0f;
                float param_7 = 0.5f;
                float _441 = grad(param_6, param_7);
                float grad2 = _441;
                float param_8 = 0.78539752960205078125f;
                q = mul(p, rot2(param_8));
                float a2 = dot(sin((q * 12.0f) - cos(q.yx * 12.0f)), 0.25f.xx) + 0.5f;
                float a1 = 1.0f - a2;
                float c = 1.0f - ((1.0f - (grad1 * a1)) * (1.0f - (grad2 * a2)));
                return c;
            }
            
            float sand(inout float2 p)
            {
                p = (float2(p.y - p.x, p.x + p.y) * 0.707099974155426025390625f) / 4.0f.xx;
                float2 param = p;
                float c1 = sandL(param);
                float param_1 = 0.261799156665802001953125f;
                float2 q = mul(p, rot2(param_1));
                float2 param_2 = q * 1.25f;
                float c2 = sandL(param_2);
                float2 param_3 = p * 4.0f.xx;
                float _515 = gradN2D(param_3);
                c1 = lerp(c1, c2, smoothstep(0.100000001490116119384765625f, 0.89999997615814208984375f, _515));
                return c1 / (1.0f + ((gT * gT) * 0.014999999664723873138427734375f));
            }
            
            float bumpSurf3D(float3 p)
            {
                float3 param = p;
                float _816 = surfFunc(param);
                float n = _816;
                float3 px = p + float3(0.001000000047497451305389404296875f, 0.0f, 0.0f);
                float3 param_1 = px;
                float _824 = surfFunc(param_1);
                float nx = _824;
                float3 pz = p + float3(0.0f, 0.0f, 0.001000000047497451305389404296875f);
                float3 param_2 = pz;
                float _832 = surfFunc(param_2);
                float nz = _832;
                float2 param_3 = p.xz + ((float2(n - nx, n - nz) / 0.001000000047497451305389404296875f.xx) * 1.0f);
                float _847 = sand(param_3);
                return _847;
            }
            
            float3 doBumpMap(float3 p, float3 nor, float bumpfactor)
            {
                float3 param = p;
                float ref = bumpSurf3D(param);
                float3 param_1 = p - float3(0.001000000047497451305389404296875f, 0.0f, 0.0f);
                float3 param_2 = p - float3(0.0f, 0.001000000047497451305389404296875f, 0.0f);
                float3 param_3 = p - float3(0.0f, 0.0f, 0.001000000047497451305389404296875f);
                float3 grad_1 = (float3(bumpSurf3D(param_1), bumpSurf3D(param_2), bumpSurf3D(param_3)) - ref.xxx) / 0.001000000047497451305389404296875f.xxx;
                grad_1 -= (nor * dot(nor, grad_1));
                return normalize(nor + (grad_1 * bumpfactor));
            }
            
            float softShadow(float3 ro, float3 lp, float k, float t)
            {
                float3 rd = lp - ro;
                float shade = 1.0f;
                float dist = 0.00150000001303851604461669921875f;
                float end = max(length(rd), 9.9999997473787516355514526367187e-05f);
                rd /= end.xxx;
                for (int i = 0; i < 24; i++)
                {
                    float3 param = ro + (rd * dist);
                    float h = map(param);
                    shade = min(shade, (k * h) / dist);
                    h = clamp(h, 0.100000001490116119384765625f, 0.5f);
                    dist += h;
                    if ((shade < 0.001000000047497451305389404296875f) || (dist > end))
                    {
                        break;
                    }
                }
                return min(max(shade, 0.0f) + 0.0500000007450580596923828125f, 1.0f);
            }
            
            float calcAO(float3 p, float3 n)
            {
                float ao = 0.0f;
                for (float i = 1.0f; i < 5.5f; i += 1.0f)
                {
                    float l = (((i + 0.0f) * 0.5f) / 5.0f) * 4.0f;
                    float3 param = p + (n * l);
                    ao += (l - map(param));
                }
                return clamp(1.0f - (ao / 5.0f), 0.0f, 1.0f);
            }
            
            float fBm(float2 p)
            {
                float2 param = p;
                float _294 = gradN2D(param);
                float2 param_1 = p * 2.0f;
                float _300 = gradN2D(param_1);
                float2 param_2 = p * 4.0f;
                float _308 = gradN2D(param_2);
                return ((_294 * 0.569999992847442626953125f) + (_300 * 0.2800000011920928955078125f)) + (_308 * 0.1500000059604644775390625f);
            }
            
            float hash(float3 p)
            {
                return frac(sin(dot(p, float3(21.70999908447265625f, 157.970001220703125f, 113.43000030517578125f))) * 45758.546875f);
            }
            
            float3 getSky(float3 ro, inout float3 rd, float3 ld)
            {
                float3 col = float3(0.800000011920928955078125f, 0.699999988079071044921875f, 0.5f);
                float3 col2 = float3(0.4000000059604644775390625f, 0.60000002384185791015625f, 0.89999997615814208984375f);
                float3 sky = lerp(col, col2, pow(max(rd.y + 0.1500000059604644775390625f, 0.0f), 0.5f).xxx);
                sky *= float3(0.839999973773956298828125f, 1.0f, 1.16999995708465576171875f);
                float sun = clamp(dot(ld, rd), 0.0f, 1.0f);
                sky += ((float3(1.0f, 0.699999988079071044921875f, 0.4000000059604644775390625f) * pow(sun, 16.0f).xxx) * 0.20000000298023223876953125f);
                sun = pow(sun, 32.0f);
                sky += ((float3(1.0f, 0.89999997615814208984375f, 0.60000002384185791015625f) * pow(sun, 32.0f).xxx) * 0.3499999940395355224609375f);
                rd.z *= (1.0f + (length(rd.xy) * 0.1500000059604644775390625f));
                rd = normalize(rd);
                float t = ((100000.0f - ro.y) - 0.1500000059604644775390625f) / (rd.y + 0.1500000059604644775390625f);
                float2 uv = (ro + (rd * t)).xz;
                if (t > 0.0f)
                {
                    float2 param = (uv * 1.5f) / 100000.0f.xx;
                    sky = lerp(sky, 2.0f.xxx, ((smoothstep(0.449999988079071044921875f, 1.0f, fBm(param)) * smoothstep(0.449999988079071044921875f, 0.550000011920928955078125f, (rd.y * 0.5f) + 0.5f)) * 0.4000000059604644775390625f).xxx);
                }
                return sky;
            }
            
            float noise3D(inout float3 p)
            {
                float3 ip = floor(p);
                float4 h = float4(0.0f, 157.0f, 1.0f, 158.0f) + dot(ip, float3(113.0f, 157.0f, 1.0f)).xxxx;
                p -= ip;
                p = (p * p) * (3.0f.xxx - (p * 2.0f));
                h = lerp(frac(sin(h) * 43758.546875f), frac(sin(h + 113.0f.xxxx) * 43758.546875f), p.x.xxxx);
                float2 _1131 = lerp(h.xz, h.yw, p.y.xx);
                h = float4(_1131.x, _1131.y, h.z, h.w);
                return lerp(h.x, h.y, p.z);
            }
            
            float getMist(float3 ro, float3 rd, float3 lp, float t)
            {
                float mist = 0.0f;
                float t0 = 0.0f;
                for (int i = 0; i < 24; i++)
                {
                    if (t0 > t)
                    {
                        break;
                    }
                    float sDi = length(lp - ro) / 80.0f;
                    float sAtt = 1.0f / (1.0f + (sDi * 0.25f));
                    float3 ro2 = (ro + (rd * t0)) * 2.5f;
                    float3 param = ro2;
                    float _1191 = noise3D(param);
                    float3 param_1 = ro2 * 3.0f;
                    float _1197 = noise3D(param_1);
                    float3 param_2 = ro2 * 9.0f;
                    float _1204 = noise3D(param_2);
                    float c = ((_1191 * 0.64999997615814208984375f) + (_1197 * 0.25f)) + (_1204 * 0.100000001490116119384765625f);
                    float n = c;
                    mist += (n * sAtt);
                    t0 += clamp(c * 0.25f, 0.100000001490116119384765625f, 1.0f);
                }
                return max(mist / 88.0f, 0.0f);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 u = (i.uv - (1 * 0.3f)) / 1;
                float3 ro = float3(0.0f, 1.2000000476837158203125f, _Time.y);
                float3 lookAt = ro + float3(0.0f, -0.1500000059604644775390625f, 0.5f);
                float param = ro.z;
                float2 _1257 = ro.xy + path(param);
                ro = float3(_1257.x, _1257.y, ro.z);
                float param_1 = lookAt.z;
                float2 _1266 = lookAt.xy + path(param_1);
                lookAt = float3(_1266.x, _1266.y, lookAt.z);
                float3 param_2 = ro;
                float _1272 = camSurfFunc(param_2);
                float sfH = _1272;
                float3 param_3 = lookAt;
                float _1276 = camSurfFunc(param_3);
                float sfH2 = _1276;
                float slope = (sfH2 - sfH) / length(lookAt - ro);
                ro.y += sfH2;
                lookAt.y += sfH2;
                float FOV = 1.256637096405029296875f;
                float3 forward = normalize(lookAt - ro);
                float3 right = normalize(float3(forward.z, 0.0f, -forward.x));
                float3 up = cross(forward, right);
                float3 rd = normalize((forward + (right * (FOV * u.x))) + (up * (FOV * u.y)));
                float param_4 = lookAt.z;
                float param_5 = path(param_4).x / 96.0f;
                float2 _1343 = mul(rd.xy, rot2(param_5));
                rd = float3(_1343.x, _1343.y, rd.z);
                float param_6 = (-slope) / 3.0f;
                float2 _1353 = mul(rd.yz, rot2(param_6));
                rd = float3(rd.x, _1353.x, _1353.y);
                float3 lp = float3(20.0f, 20.0f, 80.0f) + float3(0.0f, 0.0f, ro.z);
                float3 param_7 = ro;
                float3 param_8 = rd;
                float t = trace(param_7, param_8);
                gT = t;
                float3 col = 0.0f.xxx;
                float3 sp = ro + (rd * t);
                float pathHeight = sp.y;
                if (t < 80.0f)
                {
                    float3 param_9 = sp;
                    float param_10 = 1.0f;
                    float3 sn = normal(param_9, param_10);
                    float3 ld = lp - sp;
                    float lDist = max(length(ld), 0.001000000047497451305389404296875f);
                    ld /= lDist.xxx;
                    lDist /= 80.0f;
                    float atten = 1.0f / (1.0f + ((lDist * lDist) * 0.02500000037252902984619140625f));
                    float3 param_11 = sp;
                    float3 param_12 = sn;
                    float param_13 = 0.070000000298023223876953125f;
                    sn = doBumpMap(param_11, param_12, param_13);
                    float bf = 0.00999999977648258209228515625f;
                    float3 param_14 = sp + (sn * 0.00200000009499490261077880859375f);
                    float3 param_15 = lp;
                    float param_16 = 6.0f;
                    float param_17 = t;
                    float sh = softShadow(param_14, param_15, param_16, param_17);
                    float3 param_18 = sp;
                    float3 param_19 = sn;
                    float ao = calcAO(param_18, param_19);
                    sh = min(sh + (ao * 0.25f), 1.0f);
                    float dif = max(dot(ld, sn), 0.0f);
                    float spe = pow(max(dot(reflect(-ld, sn), -rd), 0.0f), 5.0f);
                    float fre = clamp(1.0f + dot(rd, sn), 0.0f, 1.0f);
                    float Schlick = pow(1.0f - max(dot(rd, normalize(rd + ld)), 0.0f), 5.0f);
                    float fre2 = lerp(0.20000000298023223876953125f, 1.0f, Schlick);
                    float amb = ao * 0.3499999940395355224609375f;
                    float2 param_20 = sp.xz * 16.0f;
                    col = lerp(float3(1.0f, 0.949999988079071044921875f, 0.699999988079071044921875f), float3(0.89999997615814208984375f, 0.60000002384185791015625f, 0.4000000059604644775390625f), fBm(param_20).xxx);
                    float2 param_21 = (sp.xz * 32.0f) - 0.5f.xx;
                    col = lerp(col * 1.39999997615814208984375f, col * 0.60000002384185791015625f, fBm(param_21).xxx);
                    float3 param_22 = sp;
                    float bSurf = bumpSurf3D(param_22);
                    col *= ((bSurf * 0.75f) + 0.5f);
                    float3 param_23 = floor(sp * 96.0f);
                    float3 param_24 = floor(sp * 192.0f);
                    col = lerp((col * 0.699999988079071044921875f) + (((hash(param_23) * 0.699999988079071044921875f) + (hash(param_24) * 0.300000011920928955078125f)) * 0.300000011920928955078125f).xxx, col, min((t * t) / 80.0f, 1.0f).xxx);
                    col *= float3(1.2000000476837158203125f, 1.0f, 0.89999997615814208984375f);
                    col = (col * ((dif + amb).xxx + (((float3(1.0f, 0.9700000286102294921875f, 0.920000016689300537109375f) * fre2) * spe) * 2.0f))) * atten;
                    float3 param_25 = sp;
                    float3 param_26 = reflect(rd, sn);
                    float3 param_27 = ld;
                    float3 _1573 = getSky(param_25, param_26, param_27);
                    float3 refSky = _1573;
                    col += (((col * refSky) * 0.0500000007450580596923828125f) + ((((refSky * fre) * fre2) * atten) * 0.1500000059604644775390625f));
                    col *= (sh * ao);
                }
                float3 param_28 = ro;
                float3 param_29 = rd;
                float3 param_30 = lp;
                float param_31 = t;
                float dust = getMist(param_28, param_29, param_30, param_31) * (1.0f - smoothstep(0.0f, 1.0f, pathHeight * 0.0500000007450580596923828125f));
                float3 gLD = normalize(lp - float3(0.0f, 0.0f, ro.z));
                float3 param_32 = ro;
                float3 param_33 = rd;
                float3 param_34 = gLD;
                float3 _1623 = getSky(param_32, param_33, param_34);
                float3 sky = _1623;
                col = lerp(col, sky, smoothstep(0.0f, 0.949999988079071044921875f, t / 80.0f).xxx);
                float3 mistCol = float3(1.0f, 0.949999988079071044921875f, 0.89999997615814208984375f);
                col += ((float3(1.0f, 0.60000002384185791015625f, 0.20000000298023223876953125f) * pow(max(dot(rd, gLD), 0.0f), 16.0f)) * 0.449999988079071044921875f);
                col = (col * 0.75f) + ((((col + float3(0.300000011920928955078125f, 0.25f, 0.2249999940395355224609375f)) * mistCol) * dust) * 1.5f);
                u = i.uv / 2;
                col = min(col, 1.0f.xxx) * pow((((16.0f * u.x) * u.y) * (1.0f - u.x)) * (1.0f - u.y), 0.0625f);
                return float4(sqrt(clamp(col, 0.0f.xxx, 1.0f.xxx)), 1.0f);
            }        
            ENDCG
        }
    }
}
