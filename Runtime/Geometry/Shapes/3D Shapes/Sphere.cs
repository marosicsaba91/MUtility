using System;
using System.Linq;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Sphere : IMesh/*, IEasyHandleable*/
	{
		public float radius;

		public Sphere(float radius)
		{
			this.radius = radius;
		}

		public float DragCoefficient => 0.47f;
		public float CrossSectionArea(Vector3 direction) => radius * radius * Mathf.PI;

		public float Surface => 4f * Mathf.PI * radius * radius;

		public float Volume => 4f / 3f * Mathf.PI * radius * radius * radius;

		public bool IsPointInside(Vector3 point) => point.sqrMagnitude <= radius * radius;

		/*
		public void SetHandle(int i, HandleResult result)
		{ 
			radius = result.startPosition.x;
		}

		public IEnumerable<EasyHandle> GetHandles()
		{
			yield return new EasyHandle()
			{
				position = Vector3.right * radius,
			};
		}
		*/

		const int defaultGizmoComplexity = 5;

		public Drawable ToDrawable() => ToDrawable(defaultGizmoComplexity, Circle.defaultSegmentCount);

		public Drawable ToDrawable(int complexity, int circleSegmentCount)
		{
			var polygons = new Vector3[complexity * 2][];
			Circle unitCircle = new Circle(radius);
			for (int i = 0; i < complexity; i++)
			{
				// Width
				float h = -radius + (1 + i) * (2 * radius) / (complexity + 1);
				float circleRadius = Mathf.Sqrt(radius * radius - h * h);
				Circle c1 = new Circle(circleRadius);
				Vector3[] polygon = c1.ToPolygon(circleSegmentCount).ToArray();

				polygon.Rotate(Quaternion.Euler(90, 0, 0));
				polygon.Translate(new Vector3(0, h, 0));
				polygons[i] = polygon;

				// Height
				float phase = i * 180f / complexity;
				polygon = unitCircle.ToPolygon(circleSegmentCount).ToArray();
				polygon.Rotate(Quaternion.Euler(0, phase, 0));
				polygons[complexity + i] = polygon;
			}

			return new Drawable(polygons);
		}


		const int defaultComplexity = 5;

		// Source : http://stackoverflow.com/questions/4081898/procedurally-generate-a-sphere-mesh
		public void ToMesh(Mesh resultMesh) => ToMesh(resultMesh, defaultComplexity);

		public void ToMesh(Mesh resultMesh, int complexity)
		{
			// Longitude |||
			int nbLong = complexity * 3;
			// Latitude ---
			int nbLat = complexity * 2;

			resultMesh.Clear();

			#region Vertices

			var vertices = new Vector3[(nbLong + 1) * nbLat + 2];
			float pi = Mathf.PI;
			float _2pi = pi * 2f;

			vertices[0] = Vector3.up * radius;
			for (int lat = 0; lat < nbLat; lat++)
			{
				float a1 = pi * (lat + 1) / (nbLat + 1);
				float sin1 = Mathf.Sin(a1);
				float cos1 = Mathf.Cos(a1);

				for (int lon = 0; lon <= nbLong; lon++)
				{
					float a2 = _2pi * (lon == nbLong ? 0 : lon) / nbLong;
					float sin2 = Mathf.Sin(a2);
					float cos2 = Mathf.Cos(a2);

					vertices[lon + lat * (nbLong + 1) + 1] =
						new Vector3(
							sin1 * cos2 * radius,
							cos1 * radius,
							sin1 * sin2 * radius);
				}
			}

			vertices[vertices.Length - 1] = Vector3.up * -radius;

			#endregion

			#region Normales

			var normales = new Vector3[vertices.Length];
			for (int n = 0; n < vertices.Length; n++)
				normales[n] = vertices[n].normalized;

			#endregion

			#region UVs

			var uvs = new Vector2[vertices.Length];
			uvs[0] = Vector2.up;
			uvs[uvs.Length - 1] = Vector2.zero;
			for (int lat = 0; lat < nbLat; lat++)
				for (int lon = 0; lon <= nbLong; lon++)
					uvs[lon + lat * (nbLong + 1) + 1] =
						new Vector2((float)lon / nbLong + 0.5f, 1f - (float)(lat + 1) / (nbLat + 1));

			#endregion

			#region Triangles

			int nbFaces = vertices.Length;
			int nbTriangles = nbFaces * 2;
			int nbIndexes = nbTriangles * 3;
			int[] triangles = new int[nbIndexes];

			//Top Cap
			int i = 0;
			for (int lon = 0; lon < nbLong; lon++)
			{
				triangles[i++] = lon + 2;
				triangles[i++] = lon + 1;
				triangles[i++] = 0;
			}

			//Middle
			for (int lat = 0; lat < nbLat - 1; lat++)
			{
				for (int lon = 0; lon < nbLong; lon++)
				{
					int current = lon + lat * (nbLong + 1) + 1;
					int next = current + nbLong + 1;

					triangles[i++] = current;
					triangles[i++] = current + 1;
					triangles[i++] = next + 1;

					triangles[i++] = current;
					triangles[i++] = next + 1;
					triangles[i++] = next;
				}
			}

			//Bottom Cap
			for (int lon = 0; lon < nbLong; lon++)
			{
				triangles[i++] = vertices.Length - 1;
				triangles[i++] = vertices.Length - (lon + 2) - 1;
				triangles[i++] = vertices.Length - (lon + 1) - 1;
			}

			#endregion

			resultMesh.vertices = vertices;
			resultMesh.normals = normales;
			resultMesh.uv = uvs;
			resultMesh.triangles = triangles;

			resultMesh.RecalculateBounds();
		}

		public float GetDrag(Vector3 direction)
		{
			throw new NotImplementedException();
		}
	}


	[Serializable]
	public class SpatialSphere : SpatialMesh<Sphere> { }
}