using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Cuboid : IMesh, IVolume, IArea
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
			Vector3 size = new(
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

		public Vector3 RightCenter => new(size.x / 2f, 0, 0);
		public Vector3 LeftCenter => new(-(size.x / 2f), 0, 0);
		public Vector3 TopCenter => new(0, size.y / 2f, 0);
		public Vector3 BottomCenter => new(0, -(size.y / 2f), 0);
		public Vector3 FrontCenter => new(0, 0, size.z / 2f);
		public Vector3 BackCenter => new(0, 0, -(size.z / 2f));

		public Vector3[] SideCenters => new[]
		{
			RightCenter, TopCenter, FrontCenter, LeftCenter, BottomCenter, BackCenter
		};

		public Vector3 RightTopFrontCorner => new(size.x / 2f, size.y / 2f, -(size.z / 2f));
		public Vector3 LeftTopFrontCorner => new(-(size.x / 2f), size.y / 2f, -(size.z / 2f));
		public Vector3 RightBottomFrontCorner => new(size.x / 2f, -(size.y / 2f), -(size.z / 2f));
		public Vector3 LeftBottomFrontCorner => new(-(size.x / 2f), -(size.y / 2f), -(size.z / 2f));
		public Vector3 RightTopBackCorner => new(size.x / 2f, size.y / 2f, size.z / 2f);
		public Vector3 LeftTopBackCorner => new(-(size.x / 2f), size.y / 2f, size.z / 2f);
		public Vector3 RightBottomBackCorner => new(size.x / 2f, -(size.y / 2f), size.z / 2f);
		public Vector3 LeftBottomBackCorner => new(-(size.x / 2f), -(size.y / 2f), size.z / 2f);

		public Vector3[] Corners => new[]
		{
			RightTopFrontCorner, LeftTopFrontCorner, RightBottomFrontCorner, LeftBottomFrontCorner,
			RightTopBackCorner, LeftTopBackCorner, RightBottomBackCorner, LeftBottomBackCorner
		};

		public WireShape Slice(Plain plain) => CubeSliceHelper.Slice(
			plain,
			LeftBottomBackCorner,
			LeftBottomFrontCorner,
			LeftTopBackCorner,
			LeftTopFrontCorner,
			RightBottomBackCorner,
			RightBottomFrontCorner,
			RightTopBackCorner,
			RightTopFrontCorner);

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
			Vector3[] vertices = new Vector3[vertexCount];
			Vector3[] normals = new Vector3[vertexCount];
			Vector2[] uv = new Vector2[vertexCount];
			int[] triangles = new int[triangleCount];

			int vertexIndex = 0;
			int triangleIndex = 0;

			foreach (GeneralDirection3D side in DirectionUtility.generalDirection3DValues)
			{
				Vector3 normal = side.ToVector();
				Vector3[] corners = GetSideCorners(side);

				for (int cornerIndex = 0; cornerIndex < 4; cornerIndex++)
				{
					vertices[vertexIndex] = corners[cornerIndex];
					normals[vertexIndex] = normal;
					uv[vertexIndex] = new Vector2(cornerIndex is 1 or 2 ? 0 : 1, cornerIndex < 2 ? 1 : 0);

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

		public Vector3[] GetSideCorners(GeneralDirection3D side)
		{
			Vector3 normal = side.ToVector();
			Axis3D axis = side.GetAxis();
			Vector3 extent = size / 2f;
			if (axis == Axis3D.X) return new Vector3[]
			{
				new Vector3(extent.x * normal.x, extent.y , extent.z ),
				new Vector3(extent.x * normal.x, extent.y , -extent.z),
				new Vector3(extent.x * normal.x, -extent.y, -extent.z),
				new Vector3(extent.x * normal.x, -extent.y, extent.z )
			};
			else if (axis == Axis3D.Y) return new Vector3[]
			{
				new Vector3(extent.x , extent.y * normal.y, extent.z ),
				new Vector3(extent.x , extent.y * normal.y, -extent.z),
				new Vector3(-extent.x, extent.y * normal.y, -extent.z),
				new Vector3(-extent.x, extent.y * normal.y, extent.z )
			};
			else return new Vector3[]
			{
				new Vector3(extent.x , extent.y , extent.z * normal.z),
				new Vector3(extent.x , -extent.y, extent.z * normal.z),
				new Vector3(-extent.x, -extent.y, extent.z * normal.z),
				new Vector3(-extent.x, extent.y , extent.z * normal.z)
			};
		}


		public WireShape ToDrawable() => new(
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
}
