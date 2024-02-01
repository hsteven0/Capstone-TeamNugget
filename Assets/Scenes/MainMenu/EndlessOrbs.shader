Shader "Unlit/EndlessOrbs"
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
            // make fog work
            #pragma multi_compile_fog

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

            float3 mhash3( float n )
            {
                return -1.0+2.0*frac(sin(n+float3(0.0,15.2,27.3))*158.3453123);
            }

            float mhash( float n )
            {
                return -1.0+2.0*frac(sin(n)*158.3453123);
            }

            float3 getColor(float3 p)
            {
                float jitter = mhash(p.x+50.0*p.y+2500.0*p.z+12121.0);

                float f = p.y + 4.0*jitter;

                float3 col;
                    
                if (f > 4.0) col = float3(0.224, 1.0, 0.741); 
                else if (f > 0.0) col = float3(0.339,0.214,1.);
                else if (f > -4.0) col = float3(0.224, 1.0, 0.741);
                else col = float3(0.349,0.224,1.);

                return col;
            }

            float4 trace_spheres( in float3 rayo, in float3 rayd )
            {
                float3 p = rayo;
                const float3 voxelSize = float3(1.0,1.0,1.0);

                float3 V = floor(p);
                float3 V0 = V;
                float3 step = sign(rayd);

                float3 lp = p - V; 

                float3 tmax;

                if (step.x > 0.0) tmax.x = voxelSize.x - lp.x; else tmax.x = lp.x;
                if (step.y > 0.0) tmax.y = voxelSize.y - lp.y; else tmax.y = lp.y;
                if (step.z > 0.0) tmax.z = voxelSize.z - lp.z; else tmax.z = lp.z;

                tmax /= abs(rayd);

                float3 tdelta = abs(voxelSize / rayd);

                for(int i=0; i<60; i++) {
                    if (tmax.x < tmax.y) {
                        if (tmax.x < tmax.z) {
                            V.x += step.x;
                            tmax.x += tdelta.x;
                        } else {
                            V.z += step.z;
                            tmax.z += tdelta.z;
                        }
                    } else {
                        if (tmax.y < tmax.z) {
                            V.y += step.y;
                            tmax.y += tdelta.y;
                        } else {
                            V.z += step.z;
                            tmax.z += tdelta.z;
                        }
                    }

                    if (V.x > -1.0 && V.x < 1.0 && V.y > -1.0 && V.y < 1.0) continue; 

                    float3 c = V + voxelSize*0.5 + 0.4*mhash3(V.x+50.0*V.y+2500.0*V.z);

                    float r = voxelSize.x*0.10;
                    float r2 = r*r;

                    float3 p_minus_c = p - c;
                    float p_minus_c2 = dot(p_minus_c, p_minus_c);
                    float d = dot(rayd, p_minus_c);
                    float d2 = d*d;
                    float root = d2 - p_minus_c2 + r2;
                    float dist;

                    const float divFogRange = 1.0/30.0;
                    const float3 fogCol = float3(0.3, 0.3, 0.6);
                    const float3 sunDir = float3(-0.707106, 0.707106, 0.0);

                    if (root >= 0.0) {
                        dist = -d - sqrt(root);
                        float z = max(0.0, 2.5*(dist-20.0)*divFogRange);
                        float fog = clamp(exp(-z*z), 0.0, 1.0);

                        float3 col = getColor(V);

                        float3 normal = normalize(p + rayd*dist - c);
                        float light = 0.7 + 1.0 * clamp(dot(normal, sunDir), 0.0, 1.0);

                        col = clamp(light*col, 0.0, 1.0);

                        col = lerp(fogCol, col, fog); 

                
                        return float4( col, 1.0);
                    }

                    if ( dot(V-V0,V-V0) > 2500.0) break;
                }

                return float4(0.3, 0.3, 0.6, 1.0);
            } 

            float3x3 setCamera(in float3 ro, in float3 ta, float cr)
            {
                float3 cw = normalize(ta-ro);
                float3 cp = float3(sin(cr), cos(cr),0.0);
                float3 cu = normalize( cross(cw,cp) );
                float3 cv = normalize( cross(cu,cw) );
                return float3x3( cu, cv, cw );
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 cam_angle = -2 * (float2(i.uv.x, 1.0 - i.uv.y) * 2 - 1);
                float2 m = float2(0.0, -0.5);
                
                float3 ro = 4.0*normalize(float3(sin(3.0*m.x), 0.4*m.y, cos(3.0*m.x)));
                float3 ta = float3(-3.0*cos(_Time.y*0.1), -1.0, 0.0);
                float3x3 ca = setCamera( ro, ta, 0.0 );

                float3 rd = mul(ca, normalize( float3(cam_angle.xy,2.0)));
                
                ro.z -= _Time.y;
                return trace_spheres(ro + float3(0.5, 1.5, 0.0), rd);
            }
            ENDCG
        }
    }
}
