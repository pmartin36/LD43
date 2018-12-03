Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Speed("Speed", Float) = 1
		_Opacity("Opacity", Range(0,1)) = 1
		_LightColor("Light Color", Color) = (1,1,1,1)
		_DarkColor("Dark Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { 
			"Queue"="Transparent"
		}

        Pass
        {
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float _Speed;
			float _Opacity;
			float4 _LightColor;
			float4 _DarkColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float VectorToAngle(float2 v)
			{
				v.x = sign(v.x) * max(0.000000000001, abs(v.x));
				v.y = sign(v.y) * max(0.000000000001, abs(v.y));
				float d = degrees(atan2(v.y, v.x));
				d = fmod(d + 360, 360);
				return d;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				
				float2 uvn = i.uv * 2 - 1;
				float uva = VectorToAngle(uvn) / 15.0 + _Time.x * _Speed;

				float step01 = step(0.5, frac(uva));
				col.rgb *= lerp(_LightColor, _DarkColor, step01);
				col.a *= _Opacity;
                return  col;
            }
            ENDCG
        }
    }
}
