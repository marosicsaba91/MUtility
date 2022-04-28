// ------------------------------------------- TERMINOLOGY & STRUCTURE -------------------------------------------------

// Shader: .shader Script file
//		Properties: A material property is a property that Unity stores as part of the material asset. 
//		SubShader: Different versions of the shader compatible with "Environment".
//				   When running Unity will choose the first one, that is compatible with current "Environment"
//			Compatibility Information:	Hardware
//										Render pipelines
//										Runtime settings
//			SubShader Tags: Key-value pairs that provide information about the SubShader
//			Render State Setup for all passes: (See down at*)
//			Passes:At least one, More Pass means rendering more than one image from a mesh
//				Pass Tags: Key-value pairs that provide information about the Pass
//				Render State Setup: *Instructions for updating the render state before running its shader programs
//					Shader program (The part between CGPROGRAM & ENDCG)
//						Shader functions compilation directives: (vertex, fragment, geometry, hull, domain )
//						Other compilation directives:
//						Shader Variables (Only Compiled)
//							(Shader variants comes from all the possible Keywords combinations of the shader)
// 
// Replacement shader : A shader that gets applied to every object being rendered.


// -------------------------------------------------- THE SHADER -------------------------------------------------------

// Name for Unity to organize Shader
// If starts with Hidden, it's not going to be in Shader Dropdown menu 
Shader "Hidden/Main Category/Secondary Category/Shader Name" 
{ 
	
	
	
	// ------------------------------------------ MATERIAL PROPERTIES --------------------------------------------------
	
	Properties
	{ 
		[Header(AllProperties)]

		_IntProperty ("Int Display Name", Int) = 12 
        [Toggle] _BoolProperty ("Bool Display Name", Int) = 0
		[Toggle(SOME_SHADER_KEYWORD)] _ShaderKeywordProperty ("Shader Keyword Display Name", Int) = 0 
			// Will set shader keyword to a specific value set.
		[Enum(MarosiUtility.Direction2D)] _EnumProperty ("Enum Display Name", Int) = 0	
        [KeywordEnum(None, Add, Multiply)] _KeywordEnumProperty ("Keyword Enum Display Name", Int) = 0
		_FloatProperty ("Float Display Name", Float) = 2.5
		_RangedFloatProperty ("Ranged Float Display Name", Range (0, 1)) = 0.5
        _ColorProperty ("Color Display Name", Color) = (1,1,1,1) 
			// [HDR] - Unity Editor uses the HDR color picker
        [MainColor] _Color ("Main Color Display Name", Color) = (1,0,0,1) 
			// By default, Unity considers a Color property named _Color
			// You can set something to be Main Color by using [MainColor] attribute or the name _Color
			// Mani Color is accessible from Material.color.
		_Texture2DProperty ("Texture2D Display Name", 2D) = "red" {} 	
			// Default: “white”, “black”, “gray”, “bump”  or “red”.
			// [NoScaleOffset] - Hide tiling and offset fields
			// [Normal] - Expects a normal map
			// [HDR] - Expects a high-dynamic range (HDR) texture.
		[MainTexture] _MainTex ("Main Texture 2D Display Name", 2D) = "" {}
			// By default, Unity considers a texture property named _MainTex
			// You can set something to be Main Texture by using [MainTexture] attribute or the name _MainTex
			// Main texture is accessible from Material.mainTexture.
		_Texture2DArrayProperty ("Texture2DArray Display Name", 2DArray) = "" {}
		_Texture3DDProperty ("Texture3D Display Name", 3D) = "" {}
		_CubemapProperty ("Cubemap Display Name", Cube) = "" {}
		_CubemapArrayProperty ("Cubemap Array Display Name", CubeArray) = "" {}
		// [Vector2] _Vector2Property ("2D Vector Display Name", Vector) = (1, 1, 0, 0)  // [Vector2] is in MarosiUtility
		// [Vector3] _Vector3Property ("3D Vector Display Name", Vector) = (1, 1, 1, 0)  // [Vector3] is in MarosiUtility
		// [Vector4] _Vector4Property ("4D Vector Display Name", Vector) = (1, 1, 1, 1)  // [Vector4] is in MarosiUtility (Just for formatting)
		// [Vector2Int] _Vector2IntProperty ("2D Int Vector Display Name", Vector) = (1, 1, 0, 0)  // [Vector2Int] is in MarosiUtility
		// [Vector3Int] _Vector3IntProperty ("3D Int Vector Display Name", Vector) = (1, 1, 1, 0)  // [Vector3Int] is in MarosiUtility 
		
		// Universal Property Attributes:
        // [HideInInspector] - Does not show the property in the material inspector.
		// [PerRendererData] - Read only in inspector.
		//					   Texture property will be coming from per-renderer data in the form of a MaterialPropertyBlock.
		//					   MaterialPropertyBlock accessible from C# code GetInt, SetTexture...
        
		// Later property values can be accessed
	}
	
	
	
	// --------------------------------------------- SUB-SHADERS -------------------------------------------------------
	
	SubShader
	{
		
		// ------------- SubShader Tags ------------- 
		// Tags tell about Sub-shaders how and when they expect to be rendered to the rendering engine.
		Tags
		{ 
			"Queue" = "Geometry" // Rendering Order. Each of the following Queues represents a number 
				// Background: This render queue is rendered before any others. (1000)
				// Geometry: (Default) this is used for most objects. Opaque geometry uses this queue. (2000)
			    // AlphaTest: Alpha tested geometry uses this queue. Rendered after all solid ones are drawn. (2450)
		        // Transparent: This render queue is rendered after Geometry and AlphaTest, in back-to-front order.
				//			     Anything alpha-blended (Shaders that don’t write to depth buffer. (3000)
				// Overlay: This render queue is meant for overlay effects. (4000)
				// Any special integer number: "42", or "Overlay+1"
			 
			"RenderType" = "Opaque" // Use the RenderType tag to override the behavior wit Shader replacements.
				// Opaque: (Default) Most of the shaders (Normal, Self Illuminated, Reflective, terrain shaders).
				// Transparent: Most semitransparent shaders (Transparent, Particle, Font, terrain additive pass shaders).
				// TransparentCutout: Masked transparency shaders (Transparent Cutout, two pass vegetation shaders).
				// Background: Skybox shaders.
				// Overlay: GUITexture, Halo, Flare shaders.
				// TreeOpaque: Terrain engine tree bark.
				// TreeTransparentCutout: Terrain engine tree leaves.
				// TreeBillboard: Terrain engine billboarded trees.
				// Grass: terrain Engine grass.
				// GrassBillboard: Terrain engine billboarded grass.
			
			"ForceNoShadowCasting" = "False" // Default: false (Useful For Replacement Shaders)
			"IgnoreProjector" = "False" // Default: false (A Projector allows you to project a Material onto objects)
			"CanUseSpriteAtlas" = "True" // Default: true
			"DisableBatching" = "False" // Default: false
			"PreviewType" = "Spheres" // PreviewType indicates how the material inspector preview should display the material. 
									  // Sphere (Default)   /   Plane   /   Skybox
			"RenderPipeline" = "" // Tells Unity whether this SubShader is compatible with URP or HDRP.
				// UniversalRenderPipeline
				// UHighDefinitionRenderPipeline
				// (any other value, or not declared) : This SubShader is not compatible with URP or HDRP.
		}
		
		//  -------------  Commands for setting Render State for all inner Passes ------------- 
		// ... (See options inside the Pass)
		

		
		// --------------------------------------- SUB-SHADERS PASS ----------------------------------------------------

		// ------------- Pass commands ------------- 
		
		GrabPass { "<TextureName>" } // Will grab current screen contents into a texture. 
				// The texture can be accessed in further passes by <TextureName> name.

		UsePass "VertexLit/SHADOWCASTER"        // Inserts all passes with a given name from a given shader. 
				// ShaderName/PassName contains the name of the shader and the name of the pass, separated by a slash character. 
				// Note that only first supported subshader is taken into account.
		
		Pass
		{
            Name "Pass Name"
			// ------------- Pass Tags ------------- 
            Tags 
			{ 
				// These tags are for Built-in Render Pipeline
            	"LightMode " = "Always"
					// Always: Always rendered; no lighting is applied.
		            // ForwardBase: Used in Forward rendering, ambient, main directional light, vertex/SH lights and lightmaps are applied.
		            // ForwardAdd: Used in Forward rendering; additive per-pixel lights are applied, one pass per light.
		            // Deferred: Used in Deferred Shading; renders g-buffer.
		            // ShadowCaster: Renders object depth into the shadowmap or a depth texture.
		            // PrepassBase: Used in legacy Deferred Lighting, renders normals and specular exponent.
		            // PrepassFinal: Used in legacy Deferred Lighting, renders final color by combining textures, lighting and emission.
		            // Vertex: Used in legacy Vertex Lit rendering when object is not lightmapped; all vertex lights are applied.
		            // VertexLMRGBM: Used in legacy Vertex Lit rendering when object is lightmapped; on platforms where lightmap is RGBM encoded (PC & console).
		            // VertexLM: Used in legacy Vertex Lit rendering when object is lightmapped; on platforms where lightmap is double-LDR encoded (mobile platforms).
            	"PassFlags" = "OnlyDirectional" // (Only 1 options)
				"RequireOptions" = "SoftVegetation"	// (Only 1 options)
			}
			
			// ------------- Commands for setting Render State ------------- 
			Cull Off					// Culling:	Back | Front | Off
			ZWrite Off					// Off | On
			ZClip Off					// Off | On
			ZTest Less					// Less | Greater | LEqual | GEqual | Equal | NotEqual | Always
			Lighting Off				// Off | On
			AlphaToMask Off				// Off | On
            ColorMask 0					// R |  G |  B |  A | 0 (All) | any combination of R, G, B, A: For example: RB
			
			// In the following cases see documentation for the possible values:
			Blend SrcAlpha One 			// Enables and configures alpha blending
										// How rendered object interacts with it's background 
            BlendOp	RevSub				// Sets the operation used by the Blend command.
			Offset 0, 1					// Set depth buffer writing mode. 
			Stencil						// Configures the stencil test, and what to write to the stencil buffer
				{ 
					Ref 2
					Comp Less
				} 

			// ------------------------------------- SHADERS CODE ------------------------------------------------------
			
			// ------------- Shader Include Block -------------
			CGINCLUDE
				// CG code that you want to share
				// Unity includes this code in all shader programs in CGPROGRAM blocks, anywhere in this source file.
			ENDCG
			
			// ------------- Shader program block -------------
			CGPROGRAM

				// ------------- Shader snippets ------------- 
				#pragma vertex vert			// Compile function name "vert" as the vertex shader.
				#pragma fragment frag		// Compile function name "frag" as the fragment shader.
				// Optionals:
				// #pragma geometry geom	// Compile function name "geom" as DX10 geometry shader.
											// Automatically turns on #pragma target 4.0, described below.
	            // #pragma hull hull		// Compile function name "hull" as DX11 hull shader.
											// Automatically turns on #pragma target 5.0, described below.
	            // #pragma domain dom		// Compile function "dom" name as DX11 domain shader.
											// Automatically turns on #pragma target 5.0, described below.

				// ------------- Render Pipelines working -------------
				//	Vertex Data		→	[Vertex Program]
				//							↓ Processed Vertices
				//	Triangle Indices →	[Hull Program] (optional)
				//							↓ Processed Vertices
				//						[Tessellation] (in background)
				//							↓ Patch Data
				//						[Domain Program] (optional)
				//							↓ Generated Triangles & Vertices
				//						[Geometry Program] (optional)
				//							↓ Processed Vertices
				//						[Interpolation] (in background)
				//							↓ Interpolated Vertex info
				//						[Fragment Program]
				//							↓ Processed Fragment or Pixels
				//						IMAGE OUTPUT
			

				//  ------------- Other compilation directives ------------- 
				// See documentation (There's a lot)
				
				//  ------------- Included libraries ------------- 
				#include "UnityCG.cginc"

				// ------------- Variables ------------- 

				// Variables map to Material Properties with the same name 
					// Color, Vector -> float4, half4, fixed4
					// Range, Float -> float, half, fixed
					// 2D (Textures) -> sampler2D
					// 3D (Textures) -> sampler3D
					// Cubemap -> samplerCUBE

				fixed4 _BaseColor;
				sampler2D _MainTex; 
				int _BoolProperty;

				// ------------- Vertex input data struct definition ------------- 
				// Vertex input
				struct appdata
				{
					// Initial point of shader program
					// Providing vertex data to vertex programs
					// Semantics (Upper case key) are required on all variables passed between shader stages.
					
					// Vertex shader Input Semantics:
					float4 vertex : POSITION; // Vertex position in the meshes local space, (a float3 or float4.)
					float4 color : COLOR;	// Per-vertex color
					float2 texcoord : TEXCOORD0; // TEXCOORD0 - TEXCOORD3 are 1st - 4th UV coordinates, respectively.  
					float3 normal : NORMAL; // is the vertex normal
					float4 tangent : TANGENT; // is the tangent vector (used for normal mapping), typically a float4.
				};
			
				// Predefined structures:
					// appdata_base: position, normal and one texture coordinate.
					// appdata_tan: position, tangent, normal and one texture coordinate.
					// appdata_full: position, tangent, normal, four texture coordinates and color.

			
				// ------------- Struct to transfer data from vertex to fragment shader -------------
				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color    : COLOR;
					float2 uv		: TEXCOORD0; 
				};

				// ------------- Vertex Shader -------------				
				// TODO: Example Usage
				v2f vert(appdata input)
				{
					v2f output;
					output.vertex = UnityObjectToClipPos(input.vertex); // Screen or projection Position ()
					output.uv = input.texcoord;
					output.color = input.color * _BaseColor; 
					return output;
				}
			
				// ------------- Fragment Shader -------------
				// TODO: Example Usage
				fixed3 frag(v2f input) : SV_Target
				{ 
					fixed4 col = tex2D(_MainTex, input.uv); 
					return col;
				}
			ENDCG
		}
	}
	
	// ------------- Fallback Shader ------------- 
    Fallback "Diffuse" 
}