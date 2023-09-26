﻿using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{
	[Serializable]
	public class CustomMeshPreview
	{
		[SerializeField] Vector2 textureSize = new(200, 200);
		[SerializeField] List<Material> materials = new();
		[SerializeField] CameraClearFlags backgroundType = CameraClearFlags.Skybox;
		[SerializeField] Color backgroundColor = new(0.3f, 0.3f, 0.3f);

		[SerializeField] Vector2 cameraAngle = new(25, 25);
		[SerializeField] Vector2 lightAngle = new(45, 55);
		[SerializeField] Quaternion rotateMesh;

		[SerializeField] float fieldOfView = 30;
		[SerializeField] float zoom = 1;

		public bool isExpandable = true;
		public bool areChangesLogged = false;

		public Func<Mesh> meshGetter;


		PreviewRenderUtility renderer;
		static Material standardMaterial;
		public Texture previewTexture;

		public Vector2 TextureSize
		{
			get => textureSize;
			set
			{
				if (textureSize == value) return;
				textureSize = value;
				isDirty = true;
			}
		}

		public Mesh Mesh => meshGetter?.Invoke();

		public List<Material> Materials
		{
			get => materials;
			set
			{
				if (materials == value) return;
				materials = value;
				isDirty = true;
			}
		}

		public Material Material
		{
			get => materials.Count >= 1 ? materials[0] : null;
			set
			{
				if (Material == value) return;
				if(materials.Count == 0)
					materials.Add(value);
				else
					materials[0] = value;

				isDirty = true;
			}
		}

		public CameraClearFlags BackgroundType
		{
			get => backgroundType;
			set
			{
				if (backgroundType == value) return;
				backgroundType = value;
				isDirty = true;
			}
		}

		public Color BackgroundColor
		{
			get => backgroundColor;
			set
			{
				if (backgroundColor == value) return;
				backgroundColor = value;
				isDirty = true;
			}
		}

		public bool IsExpandable
		{
			get => isExpandable;
			set
			{
				if (isExpandable == value) return;
				isExpandable = value;
				isDirty = true;
			}
		}

		public Vector2 CameraAngle
		{
			get => cameraAngle;
			set
			{
				if (cameraAngle == value) return;
				cameraAngle = value;
				isDirty = true;
			}
		}

		public Vector2 LightAngle
		{
			get => lightAngle;
			set
			{
				if (lightAngle == value) return;
				lightAngle = value;
				isDirty = true;
			}
		}

		public Quaternion RotateMesh
		{
			get => rotateMesh;
			set
			{
				if (rotateMesh == value) return;
				rotateMesh = value;
				isDirty = true;
			}
		}

		public float FieldOfView
		{
			get => fieldOfView;
			set
			{
				if (fieldOfView == value) return;
				fieldOfView = value;
				isDirty = true;
			}
		}

		public float Zoom
		{
			get => zoom;
			set
			{
				if (zoom == value) return;
				zoom = value;
				isDirty = true;
			}
		}



		//-------------------------------------

		bool isDirty = true;

		public Texture PreviewTexture
		{
			get
			{
				if (previewTexture == null || isDirty)
				{
					renderer ??= new PreviewRenderUtility();
					Render();
				}

				return previewTexture;
			}

			private set => previewTexture = value;
		}

		public void SetMaterials(params Material[] materials)
		{
			this.materials.Clear();
			this.materials.AddRange(materials);
		}


		public void Render()
		{
#if UNITY_EDITOR
			AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
			AssemblyReloadEvents.beforeAssemblyReload += Dispose;

			if (standardMaterial == null)
				standardMaterial = new(Shader.Find("Standard"));

			renderer ??= new PreviewRenderUtility();
			if (previewTexture != null)
				UnityEngine.Object.DestroyImmediate(previewTexture);
			previewTexture = Render(this, renderer);
			isDirty = false;

#endif
		}

#if UNITY_EDITOR
		public void Dispose()
		{
			renderer?.Cleanup();
			renderer = null;

			if (previewTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(previewTexture);
				previewTexture = null;
			}
			if (standardMaterial != null)
			{
				UnityEngine.Object.DestroyImmediate(standardMaterial);
				standardMaterial = null;
			}
		}

		static Texture Render(CustomMeshPreview preview, PreviewRenderUtility renderer)
		{
			Rect position = new(0, 0, preview.textureSize.x, preview.textureSize.y);
			renderer.BeginPreview(position, GUIStyle.none);

			Camera cam = renderer.camera;
			Light light = renderer.lights[0];

			cam.backgroundColor = preview.backgroundColor;
			cam.clearFlags = preview.backgroundType;
			light.intensity = 1;
			light.transform.rotation = ToQuaternion(preview.lightAngle);

			Mesh mesh = preview.meshGetter?.Invoke();
			Bounds bounds = mesh != null ? mesh.bounds : new Bounds(Vector3.zero, Vector3.one);

			Vector3 sizeV = bounds.size;
			float size = MathHelper.Average(sizeV.x, sizeV.y, sizeV.z);
			float zoomedFieldOfView = preview.fieldOfView / preview.zoom;
			cam.fieldOfView = zoomedFieldOfView;

			float distance = size / Mathf.Tan(preview.fieldOfView * 0.5f * Mathf.Deg2Rad);
			if (float.IsInfinity(distance))
				distance = 10;

			cam.transform.rotation = ToQuaternion(preview.cameraAngle);
			cam.orthographic = false;
			cam.transform.position = bounds.center - cam.transform.forward * distance;
			cam.nearClipPlane = distance / 100;
			cam.farClipPlane = distance * 10;

			if (mesh != null && preview.materials != null)
			{
				for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; subMeshIndex++)
				{
					int materialIndex = Mathf.Min(subMeshIndex, preview.materials.Count - 1);
					Material material = materialIndex == -1 ? standardMaterial : preview.materials[materialIndex];

					renderer.DrawMesh(mesh, Matrix4x4.identity, material, 0);
				}

			}

			cam.Render();
			return renderer.EndPreview();

		}


#endif

		static Quaternion ToQuaternion(Vector2 rotation)
		{
			rotation.x = MathHelper.Mod(rotation.x, 360);
			rotation.y = MathHelper.Mod(rotation.y, 360);
			if (rotation.x < 0)
				rotation.x += 360;
			if (rotation.y < 0)
				rotation.y += 360;

			return Quaternion.Euler(rotation.y, -rotation.x, 0);
		}

		public void SetDirty() => isDirty = true;
	}
}