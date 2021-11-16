Shader "Hidden/Empty Shader" 
{ 
	// OPTIONAL:
	// Properties	{ 	}
	
	SubShader
	{
		// OPTIONAL:
		// Tags { "TagName1" = "Value1" "TagName2" = "Value2" }

		Cull Off
		Lighting Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			// OPTIONAL :
            // Tags { "TagName1" = "Value1" "TagName2" = "Value2" }
			
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex   : POSITION; 
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION; 
			};
			
			// OPTIONAL:
			// Variables & Material Properties

			v2f vert(appdata input)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(input.vertex); 
				return OUT;
			}

			fixed3 frag(v2f IN) : SV_Target
			{
				return float4(1,1,1,1);
			}
		ENDCG
		}
	}
	
	// OPTIONAL:
    // Fallback "Diffuse"
}