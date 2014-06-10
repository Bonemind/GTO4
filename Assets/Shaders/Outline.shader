Shader "Custom/OutlineToonShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Bump ("Bump", 2D) = "bump" {}
        _Outline ("Outline", Range(0,1)) = 0.1
        _Color ("Color", color) = (0,0,0,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
 		Pass {
 
            Cull Front
            Lighting Off
            ZWrite On
        //    Tags { "LightMode"="ForwardBase" }
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            //    float3 tangent : TANGENT;
            };
 
            struct v2f
            {
                float4 pos : POSITION;
            };
 
            float _Outline;
 
            v2f vert (a2v v)
            {
                v2f o;
                float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
                float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal); 
                pos.z += 0.1;
                normal.z += 0.1;
                pos = pos + float4(normalize(normal),0) * _Outline;
                o.pos = mul(UNITY_MATRIX_P, pos);
 
                return o;
            }
            
            float4 _Color;
 
            float4 frag (v2f IN) : COLOR
            {
                return _Color;
            }
 
            ENDCG
        }
 
        Pass {
        	Tags { "LightMode" = "ForwardBase" }
            Cull Back
            Lighting On
 
            CGPROGRAM
			#pragma exclude_renderers d3d11 xbox360
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            uniform float4 _LightColor0;
 
            sampler2D _MainTex;
            sampler2D _Bump;
            float4 _MainTex_ST;
            float4 _Bump_ST;
 
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
            };
 
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 lightDirection;
            };
 
            v2f vert (a2v v)
            {
                v2f o;
                TANGENT_SPACE_ROTATION;
 
                o.lightDirection = mul(rotation, ObjSpaceLightDir(v.vertex));
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex); 
                o.uv2 = TRANSFORM_TEX (v.texcoord, _Bump);
                return o;
            }
 
            float4 frag(v2f i) : COLOR 
            {
                float4 c = tex2D (_MainTex, i.uv); 
                float3 n =  UnpackNormal(tex2D (_Bump, i.uv2));
 
                float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
 
                float lengthSq = dot(i.lightDirection, i.lightDirection);
                float atten = 1.0 / (1.0 + lengthSq);
                //Angle to the light
                float diff = saturate (dot (n, normalize(i.lightDirection)));  
                lightColor += _LightColor0.rgb * (diff * atten);
                c.rgb = lightColor * c.rgb * 2;
                return c;
            }
 
            ENDCG
        }
        
        Pass {
        	Tags { "LightMode" = "ForwardAdd" }
            Cull Back
            Lighting On
            Blend One One
 
            CGPROGRAM
			#pragma exclude_renderers d3d11 xbox360
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            uniform float4 _LightColor0;
 
            sampler2D _MainTex;
            sampler2D _Bump;
            float4 _MainTex_ST;
            float4 _Bump_ST;
 
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
            };
 
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 lightDirection;
            };
 
            v2f vert (a2v v)
            {
                v2f o;
                TANGENT_SPACE_ROTATION;
 
                o.lightDirection = mul(rotation, ObjSpaceLightDir(v.vertex));
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex); 
                o.uv2 = TRANSFORM_TEX (v.texcoord, _Bump);
                return o;
            }
 
            float4 frag(v2f i) : COLOR 
            {
                float4 c = tex2D (_MainTex, i.uv); 
                float3 n =  UnpackNormal(tex2D (_Bump, i.uv2));
 
                float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
 
                float lengthSq = dot(i.lightDirection, i.lightDirection);
                float atten = 1.0 / (1.0 + lengthSq);
                //Angle to the light
                float diff = saturate (dot (n, normalize(i.lightDirection)));  
                lightColor += _LightColor0.rgb * (diff * atten);
                c.rgb = lightColor * c.rgb * 2;
                return c;
            }
 
            ENDCG
        }
 
    }
    FallBack "Diffuse"
}