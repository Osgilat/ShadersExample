﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparateGrab" {

	Properties
	{
		_MainTex("Base(RGB) Trans(A)", 2D) = "white{}"
		_BumpMap("Noise text", 2D) = "bump"{}
		_Magnitude("Magnitude", Range(0,1)) = 0.05
	}

	SubShader{

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Opaque"
		}

		GrabPass{}

		Pass
			{
				CGPROGRAM

				#pragma target      5.0
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _BumpMap;
				float _Magnitude;

				struct vertInput
				{
					float4 vertex: POSITION;
					float2 texcoord: TEXCOORD0;
				};

				struct vertOutput
				{
					float4 vertex: POSITION;
					float4 uvgrab: TEXCOORD1;
					float2 texcoord: TEXCOORD0;
				}

				//Vertex function
				vertInput vert(vertexInput v)
				{
					vertexOutput o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uvgrab = ComputeGrabScreenPos(o.vertex);
					return o;
				}

				//Fragment function
				half4 frag(vertexOutput i) : COLOR
				{
					half4 mainColour = tex2D(_MainTex, i.texcoord);

					half4 bump = tex2D(_BumpMap, i.texcoord);
					half2 distortion = UnpackNormal(bump).rg;

					i.uvgrab.xy += distortion * _Magnitude;

					fixed4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
					return col * mainColour * _Colour;
				}

				ENDCG
			}

	}
}
