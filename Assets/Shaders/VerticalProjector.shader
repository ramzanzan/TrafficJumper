// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/VerticalProjector" {
	Properties {
		//_Color ("Main Color", Color) = (1,1,1,1)
		//_FalloffTex ("FallOff",2D) = ""{}
		_ShadowTex ("Arrow", 2D) = "" {}
		
	}
	
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha SrcAlpha
			Offset -1, -1
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				float4 pos : SV_POSITION;
			};
			
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.uvShadow = mul (unity_Projector, vertex);
				o.uvFalloff = mul (unity_ProjectorClip, vertex);
				return o;
			}
			
			//fixed4 _Color;
			//sampler2D _FalloffTex;
			sampler2D _ShadowTex;
			
			fixed4 frag (v2f i) : SV_Target
			{				
				if(i.uvFalloff.r>1)
				    return fixed4(0,0,0,1);
				else
                {
                    fixed4 texS = tex2D(_ShadowTex, i.uvShadow);
                    //todo 
                    //запилить правильные текстуры
                    texS.a=1;
                    return texS;
                }
			}
			ENDCG
		}
	}
}
