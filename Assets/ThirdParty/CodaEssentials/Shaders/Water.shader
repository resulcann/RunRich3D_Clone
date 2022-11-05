// Toony Colors Pro+Mobile 2
// (c) 2014-2021 Jean Moreno

Shader "Coda Platform/Toon/Water"
{
	Properties
	{
		[Header(Base)]
		[Space] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		_SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_WaterColor ("Water Color", Color) = (1,1,1,1)
		[Space(15)] _MainTex ("Water Texture", 2D) = "white" {}
		[Vector4FloatsDrawer(Speed,Amplitude,Frequency,Offset)] _MainTex_SinAnimParams ("UV Sine Distortion (Speed, Amplitude, Frequency, Offset)", Float) = (1, 0.05, 1, 0)

		[Header(Ramp Shading)]
		[Space] _RampThreshold ("Threshold", Range(0.01,1)) = 0.15
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.35
		
		[Header(Rim Lighting)]
		[Space] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMinVert ("Rim Min", Range(0,2)) = 0.5
		_RimMaxVert ("Rim Max", Range(0,2)) = 1
		
		[Header(Vertex Waves Animation)]
		[Space] _WavesSpeed ("Speed", Float) = 2
		_WavesHeight ("Height", Float) = 0.1
		_WavesFrequency ("Frequency", Range(0,10)) = 1
		
		[Header(Depth Based Effects)]
		[Space] _DepthColor ("Depth Color", Color) = (0,0,1,1)
		[PowerSlider(5.0)] _DepthColorDistance ("Depth Color Distance", Range(0.01,3)) = 0.5

		[Header(Foam Effects)]
		[Space] _FoamSpread ("Foam Spread", Range(0,5)) = 2
		_FoamStrength ("Foam Strength", Range(0,1)) = 0.8
		_FoamColor ("Foam Color (RGB) Opacity (A)", Color) = (0.9,0.9,0.9,1)
		_FoamTex ("Foam Texture", 2D) = "black" {}
		_FoamSpeed ("Foam Speed", Range(0,5)) = 0.5
		_FoamSmoothness ("Foam Smoothness", Range(0,0.5)) = 0.02
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		
		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Shader Properties
		sampler2D _MainTex;
		sampler2D _FoamTex;
		
		// Shader Properties
		float _WavesFrequency;
		float _WavesHeight;
		float _WavesSpeed;
		float _RimMinVert;
		float _RimMaxVert;
		float4 _MainTex_ST;
		half4 _MainTex_SinAnimParams;
		fixed4 _WaterColor;
		fixed4 _DepthColor;
		float _DepthColorDistance;
		float _FoamSpread;
		float _FoamStrength;
		fixed4 _FoamColor;
		float _FoamSpeed;
		float4 _FoamTex_ST;
		float _FoamSmoothness;
		float _RampThreshold;
		float _RampSmoothing;
		fixed4 _HColor;
		fixed4 _SColor;
		fixed4 _RimColor;
		
		UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

		ENDCG

		// Main Surface Shader

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha noforwardadd interpolateview nolightmap nolppv
		#pragma target 3.0

		//================================================================
		// STRUCTS

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			half4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			float3 worldPos;
			float4 screenPosition;
			float2 sinUvAnimVertexWorldPos;
			half rim;
			float2 texcoord0;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			float3 worldPosUv = mul(unity_ObjectToWorld, v.vertex).xyz;

			// Used for texture UV sine animation (world space)
			float2 sinUvAnimVertexWorldPos = worldPosUv.xy + worldPosUv.yz;
			output.sinUvAnimVertexWorldPos = sinUvAnimVertexWorldPos;

			// Texture Coordinates
			output.texcoord0 = v.texcoord0.xy;
			// Shader Properties Sampling
			float __wavesFrequency = ( _WavesFrequency );
			float __wavesHeight = ( _WavesHeight );
			float __wavesSpeed = ( _WavesSpeed );
			float __rimMinVert = ( _RimMinVert );
			float __rimMaxVert = ( _RimMaxVert );

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			
			// Vertex water waves
			float _waveFrequency = __wavesFrequency;
			float _waveHeight = __wavesHeight;
			float3 _vertexWavePos = worldPos.xyz * _waveFrequency;
			float _phase = _Time.y * __wavesSpeed;
			float waveFactorX = sin(_vertexWavePos.x + _phase) * _waveHeight;
			float waveFactorZ = sin(_vertexWavePos.z + _phase) * _waveHeight;
			v.vertex.y += (waveFactorX + waveFactorZ);
			float4 clipPos = UnityObjectToClipPos(v.vertex);

			//Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			output.screenPosition = screenPos;
			COMPUTE_EYEDEPTH(output.screenPosition.z);
			half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half ndv = abs(dot(viewDir, worldNormal));
			half ndvRaw = ndv;

			half rim = 1 - ndvRaw;
			rim = smoothstep(__rimMinVert, __rimMaxVert, rim);
			output.rim = rim;

		}

		//================================================================

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;

			Input input;
			
			// Shader Properties
			float __rampThreshold;
			float __rampSmoothing;
			float3 __highlightColor;
			float3 __shadowColor;
			float __ambientIntensity;
			float3 __rimColor;
			float __rimStrength;
		};

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{

			float2 uvSinAnim__MainTex = (input.sinUvAnimVertexWorldPos * _MainTex_SinAnimParams.z) + (_Time.yy * _MainTex_SinAnimParams.x);
			// Shader Properties Sampling
			float4 __albedo = (  lerp(float4(1,1,1,1), _WaterColor.rgba, tex2D(_MainTex, input.worldPos.xz * _MainTex_ST.xy + _MainTex_ST.zw + (((sin(0.9 * uvSinAnim__MainTex + _MainTex_SinAnimParams.w) + sin(1.33 * uvSinAnim__MainTex + 3.14 * _MainTex_SinAnimParams.w) + sin(2.4 * uvSinAnim__MainTex + 5.3 * _MainTex_SinAnimParams.w)) / 3) * _MainTex_SinAnimParams.y)).a) );
			float4 __mainColor = ( float4(1,1,1,1) );
			float __alpha = ( __albedo.a * __mainColor.a );
			float3 __depthColor = ( _DepthColor.rgb );
			float __depthColorDistance = ( _DepthColorDistance );
			float __foamSpread = ( _FoamSpread );
			float __foamStrength = ( _FoamStrength );
			float4 __foamColor = ( _FoamColor.rgba );
			float4 __foamSpeed = ( _FoamSpeed );
			float2 __foamTextureBaseUv = ( input.texcoord0.xy );
			float __foamMask = ( .0 );
			float __foamSmoothness = ( _FoamSmoothness );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__ambientIntensity = ( 1.0 );
			output.__rimColor = ( _RimColor.rgb );
			output.__rimStrength = ( 1.0 );

			output.input = input;

			// Sample depth texture and calculate difference with local depth
			float sceneDepth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(input.screenPosition));
			if (unity_OrthoParams.w > 0.0)
			{
				// Orthographic camera
				#if defined(UNITY_REVERSED_Z)
					sceneDepth = 1.0 - sceneDepth;
				#endif
				sceneDepth = (sceneDepth * _ProjectionParams.z) + _ProjectionParams.y;
			}
			else
			{
				// Perspective camera
				sceneDepth = LinearEyeDepth(sceneDepth);
			}
			
			float localDepth = input.screenPosition.z;
			float depthDiff = abs(sceneDepth - localDepth);

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;
			
			output.Albedo *= __mainColor.rgb;
			
			// Depth-based color
			half3 depthColor = __depthColor;
			half3 depthColorDist = __depthColorDistance;
			output.Albedo.rgb = lerp(depthColor, output.Albedo.rgb, saturate(depthColorDist * depthDiff));
			
			// Depth-based water foam
			half foamSpread = __foamSpread;
			half foamStrength = __foamStrength;
			half4 foamColor = __foamColor;
			
			half4 foamSpeed = __foamSpeed;
			float2 foamUV = __foamTextureBaseUv;
			
			float2 foamUV1 = foamUV.xy + _Time.yy * foamSpeed.xy * 0.05;
			half3 foam = ( tex2D(_FoamTex, foamUV1 * _FoamTex_ST.xy + _FoamTex_ST.zw).rgb );
			
			foamUV.xy += _Time.yy * foamSpeed.zw * 0.05;
			half3 foam2 = ( tex2D(_FoamTex, foamUV * _FoamTex_ST.xy + _FoamTex_ST.zw).rgb );
			
			foam = (foam + foam2) / 2.0;
			float foamDepth = saturate(foamSpread * depthDiff) * (1.0 - __foamMask);
			half foamSmooth = __foamSmoothness;
			half foamTerm = (smoothstep(foam.r - foamSmooth, foam.r + foamSmooth, saturate(foamStrength - foamDepth)) * saturate(1 - foamDepth)) * foamColor.a;
			output.Albedo.rgb = lerp(output.Albedo.rgb, foamColor.rgb, foamTerm);
			output.Alpha = lerp(output.Alpha, foamColor.a, foamTerm);

		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, UnityGI gi)
		{
			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				//extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			half3 ramp;
			
			#define		RAMP_THRESHOLD	surface.__rampThreshold
			#define		RAMP_SMOOTH		surface.__rampSmoothing
			ndl = saturate(ndl);
			ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);
			half3 rampGrayscale = ramp;

			//Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;

			//Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			//Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

			// Rim Lighting
			half rim = surface.input.rim;
			rim = ( rim );
			half3 rimColor = surface.__rimColor;
			half rimStrength = surface.__rimStrength;
			//Rim light mask
			color.rgb += ndl * atten * rim * rimColor * rimStrength;

			return color;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			//GI without reflection probes
			gi = UnityGlobalIllumination(data, 1.0, normal); // occlusion is applied in the lighting function, if necessary

			surface.atten = data.atten; // transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb; // remove attenuation

		}

		ENDCG

	}
}