using System;
using System.Linq;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Cuboid : IMesh, /*IEasyHandleable,*/ IVolume, IArea
	{
		public Vector3 size;

		public Cuboid(Vector3 size)
		{
			this.size = size;
		}

		public static Cuboid FromCamera(Camera camera, out Vector3 center, out Quaternion rotation)
		{
			Transform cameraTransform = camera.transform;
			Vector3 pos = cameraTransform.position;
			Vector3 forward = cameraTransform.forward;
			Vector3 near = pos + forward * camera.nearClipPlane;
			Vector3 far = pos + forward * camera.farClipPlane;

			center = (near + far) / 2;
			Vector3 size = new Vector3(
				camera.orthographicSize * camera.aspect * 2,
				camera.orthographicSize * 2,
				camera.farClipPlane - camera.nearClipPlane);

			rotation = cameraTransform.rotation;
			return new Cuboid(size);
		}

		public float Volume => size.x * size.y * size.z;

		public float Area => 2f * (size.x * size.y + size.y * size.z + size.z * size.x);

		public float Width => size.x;
		public float Height => size.y;
		public float Length => size.z;

		public Vector3 RightCenter => new Vector3(size.x / 2f, 0, 0);
		public Vector3 LeftCenter => new Vector3(-(size.x / 2f), 0, 0);
		public Vector3 TopCenter => new Vector3(0, size.y / 2f, 0);
		public Vector3 BottomCenter => new Vector3(0, -(size.y / 2f), 0);
		public Vector3 FrontCenter => new Vector3(0, 0, size.z / 2f);
		public Vector3 BackCenter => new Vector3(0, 0, -(size.z / 2f));

		public Vector3[] SideCenters => new[]
		{
		RightCenter, TopCenter, FrontCenter, LeftCenter, BottomCenter, BackCenter
	};

		public Vector3 RightTopFrontCorner => new Vector3(size.x / 2f, size.y / 2f, -(size.z / 2f));
		public Vector3 LeftTopFrontCorner => new Vector3(-(size.x / 2f), size.y / 2f, -(size.z / 2f));
		public Vector3 RightBottomFrontCorner => new Vector3(size.x / 2f, -(size.y / 2f), -(size.z / 2f));
		public Vector3 LeftBottomFrontCorner => new Vector3(-(size.x / 2f), -(size.y / 2f), -(size.z / 2f));
		public Vector3 RightTopBackCorner => new Vector3(size.x / 2f, size.y / 2f, size.z / 2f);
		public Vector3 LeftTopBackCorner => new Vector3(-(size.x / 2f), size.y / 2f, size.z / 2f);
		public Vector3 RightBottomBackCorner => new Vector3(size.x / 2f, -(size.y / 2f), size.z / 2f);
		public Vector3 LeftBottomBackCorner => new Vector3(-(size.x / 2f), -(size.y / 2f), size.z / 2f);

		public Vector3[] Corners => new[]
		{
		RightTopFrontCorner, LeftTopFrontCorner, RightBottomFrontCorner, LeftBottomFrontCorner,
		RightTopBackCorner, LeftTopBackCorner, RightBottomBackCorner, LeftBottomBackCorner
	};

		public SpatialRectangle RightSide => new SpatialRectangle()
		{
			polygon = new Rectangle(size.z, size.y),
			position = RightCenter,
			rotation = Quaternion.LookRotation(Vector3.right, Vector3.up)
		};

		public SpatialRectangle LeftSide => new SpatialRectangle()
		{
			polygon = new Rectangle(size.z, size.y),
			position = LeftCenter,
			rotation = Quaternion.LookRotation(Vector3.left, Vector3.up)
		};

		public SpatialRectangle TopSide => new SpatialRectangle()
		{
			polygon = new Rectangle(size.x, size.z),
			position = TopCenter,
			rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward)
		};

		public SpatialRectangle BottomSide => new SpatialRectangle()
		{
			polygon = new Rectangle(size.x, size.z),
			position = BottomCenter,
			rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward)
		};

		public SpatialRectangle FrontSide => new SpatialRectangle()
		{
			polygon = new Rectangle(size.x, size.y),
			position = FrontCenter,
			rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up)
		};

		public SpatialRectangle BackSide => new SpatialRectangle()
		{
			polygon = new Rectangle(size.x, size.y),
			position = BackCenter,
			rotation = Quaternion.LookRotation(Vector3.back, Vector3.up)
		};

		public SpatialRectangle[] Sides => new[]
		{
		RightSide, TopSide, FrontSide, LeftSide, BottomSide, BackSide
	};


		public Drawable Slice(Plain plain)
		{
			return CubeSliceHelper.Slice(
				plain,
				LeftBottomBackCorner,
				LeftBottomFrontCorner,
				LeftTopBackCorner,
				LeftTopFrontCorner,
				RightBottomBackCorner,
				RightBottomFrontCorner,
				RightTopBackCorner,
				RightTopFrontCorner);
		}

		/*
		public IEnumerable<EasyHandle> GetHandles()
		{
			yield return new EasyHandle(RightCenter); // 0 Right
			yield return new EasyHandle(LeftCenter); // 1 Left
			yield return new EasyHandle(TopCenter); // 2 Top
			yield return new EasyHandle(TopCenter); // 3 Bottom
			yield return new EasyHandle(BackCenter); // 4 Back
			yield return new EasyHandle(FrontCenter); // 5 Front
			yield return new EasyHandle(RightTopBackCorner); // 6 Rescale 
		}

		public void SetHandle(int i, HandleResult result)
		{
			float rescale = 0;
			Vector3 newPoint = result.startPosition;

			switch (i)
			{
				case 0: // Right
					rescale = newPoint.x + size.x / 2f;
					break;
				case 1: // Left
					rescale = -newPoint.x - size.x / 2f;
					break;
				case 2: // Top
					rescale = newPoint.y + size.y / 2f;
					break;
				case 3: // Bottom
					rescale = -newPoint.y - size.y / 2f;
					break;
				case 4: // Back
					rescale = -newPoint.z - size.z / 2f;
					break;
				case 5: // Front
					rescale = newPoint.z + size.z / 2f;
					break;
				case 6: // Rescale
					float rescaleX = newPoint.x * 2;
					float rescaleY = newPoint.y * 2;
					float rescaleZ = newPoint.z * 2;
					size = new Vector3(rescaleX, rescaleY, rescaleZ);
					return;
			}

			switch (i)
			{
				// Horizontal
				case 0:
				case 1:
					size = new Vector3(size.x + rescale, size.y, size.z);
					break;
				// Vertical
				case 2:
				case 3:
					size = new Vector3(size.x, size.y + rescale, size.z);
					break;
				// Dept
				case 4:
				case 5:
					size = new Vector3(size.x, size.y, size.z + rescale);
					break;
			}
		}
		*/

		public bool IsPointInside(Vector3 point) =>
			Mathf.Abs(point.x) < size.x / 2f &&
			Mathf.Abs(point.y) < size.y / 2f &&
			Mathf.Abs(point.z) < size.z / 2f;

		public void ToMesh(Mesh result) => ToMesh(result, true);

		public void ToMesh(Mesh resultMesh, bool normalOut)
		{
			resultMesh.Clear();
			const int vertexCount = 24;
			const int triangleCount = 36;
			var vertices = new Vector3[vertexCount];
			var normals = new Vector3[vertexCount];
			var uv = new Vector2[vertexCount];
			int[] triangles = new int[triangleCount];

			int vertexIndex = 0;
			int triangleIndex = 0;
			SpatialRectangle[] sides = Sides;
			foreach (SpatialRectangle side in sides)
			{
				Vector3[] corners = side.Points.ToArray();
				Vector3 normal = side.rotation * (normalOut ? Vector3.up : Vector3.down);
				for (int cornerIndex = 0; cornerIndex < 4; cornerIndex++)
				{
					vertices[vertexIndex] = corners[cornerIndex];
					normals[vertexIndex] = normal;
					uv[vertexIndex] = new Vector2(cornerIndex == 1 || cornerIndex == 2 ? 0 : 1, cornerIndex < 2 ? 1 : 0);

					vertexIndex++;
				}

				if (normalOut)
				{
					triangles[triangleIndex * 6 + 0] = vertexIndex - 1;
					triangles[triangleIndex * 6 + 1] = vertexIndex - 4;
					triangles[triangleIndex * 6 + 2] = vertexIndex - 2;

					triangles[triangleIndex * 6 + 3] = vertexIndex - 3;
					triangles[triangleIndex * 6 + 4] = vertexIndex - 2;
					triangles[triangleIndex * 6 + 5] = vertexIndex - 4;
				}
				else
				{
					triangles[triangleIndex * 6 + 0] = vertexIndex - 2;
					triangles[triangleIndex * 6 + 1] = vertexIndex - 4;
					triangles[triangleIndex * 6 + 2] = vertexIndex - 1;

					triangles[triangleIndex * 6 + 3] = vertexIndex - 4;
					triangles[triangleIndex * 6 + 4] = vertexIndex - 2;
					triangles[triangleIndex * 6 + 5] = vertexIndex - 3;
				}

				triangleIndex++;
			}

			resultMesh.vertices = vertices;
			resultMesh.normals = normals;
			resultMesh.uv = uv;
			resultMesh.triangles = triangles;
			resultMesh.RecalculateBounds();
			resultMesh.RecalculateTangents();
		}


		public Drawable ToDrawable() => new Drawable(
			new[]
			{
			RightTopFrontCorner, LeftTopFrontCorner, LeftBottomFrontCorner, RightBottomFrontCorner, RightTopFrontCorner
			},
			new[]
			{
			RightTopBackCorner, LeftTopBackCorner, LeftBottomBackCorner, RightBottomBackCorner, RightTopBackCorner
			},
			new[] { RightTopFrontCorner, RightTopBackCorner },
			new[] { LeftTopFrontCorner, LeftTopBackCorner },
			new[] { LeftBottomFrontCorner, LeftBottomBackCorner },
			new[] { RightBottomFrontCorner, RightBottomBackCorner }
		);
	}

	[Serializable]
	public class SpatialCuboid : SpatialMesh<Cuboid> { }
}
