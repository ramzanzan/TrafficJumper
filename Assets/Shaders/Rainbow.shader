Shader "My/Rainbow"
{
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

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (float4 v : POSITION)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    
				fixed4 col = fixed4(_SinTime.x,_SinTime.y,_SinTime.z,0);
				return col;
			}
			ENDCG
		}
	}
}
