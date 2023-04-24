
Shader "ShaderMan/Wave"
	{

	Properties{
	//Properties
	}

	SubShader
	{
	Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

	Pass
	{
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

	struct VertexInput {
    fixed4 vertex : POSITION;
	fixed2 uv:TEXCOORD0;
    fixed4 tangent : TANGENT;
    fixed3 normal : NORMAL;
	//VertexInput
	};


	struct VertexOutput {
	fixed4 pos : SV_POSITION;
	fixed2 uv:TEXCOORD0;
	//VertexOutput
	};

	//Variables

	fixed3 hsb2rgb(in fixed3 c)
{
    fixed3 rgb = clamp(abs(fmod(c.x*6.0+fixed3(0.0,4.0,2.0),
                             6.0)-3.0)-1.0,
                     0.0,
                     1.0 );
    rgb = rgb*rgb*(3.0-2.0*rgb);
    return c.z * lerp( fixed3(1.0,1.0,1.0), rgb, c.y);
}





	VertexOutput vert (VertexInput v)
	{
	VertexOutput o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.uv;
	//VertexFactory
	return o;
	}
	fixed4 frag(VertexOutput i) : SV_Target
	{
	   
    fixed2 p = (2.0*i.uv-1)/1;
    
    fixed r = length(p) * 0.9;
	fixed3 color = hsb2rgb(fixed3(0.24, 0.7, 0.4));
    
    fixed a = pow(r, 2.0);
    fixed b = sin(r * 0.8 - 1.6);
    fixed c = sin(r - 0.010);
    fixed s = sin(a - _Time.y * 3.0 + b) * c;
    
    color *= abs(1.0 / (s * 10.8)) - 0.01;

	fixed4 B = fixed4(0, 0, 0, 0);

	if (color.r + B.r >= 1.0 || color.g + B.g >= 1.0f || color.b + B.b >= 1.0f)return fixed4(color, 1.);
	else return B;

	

	}
	ENDCG
	}
  }
}

