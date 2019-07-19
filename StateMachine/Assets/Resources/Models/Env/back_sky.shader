Shader "Custom/BackSky"
{
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader {
		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Geometry"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass {
			Lighting Off
			SetTexture [_MainTex] { combine texture } 
		}
	}
}
