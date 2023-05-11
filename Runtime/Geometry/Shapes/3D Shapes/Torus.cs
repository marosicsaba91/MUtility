using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Torus : IMesh, IArea
	{
		const int defaultComplexityBigCircle = 20;
		const int defaultComplexitySmallCircle = 10;

		public float radius;
		public float thickness;

		public Torus(float radius = 1, float thickness = 0.5f)
		{
			this.radius = radius;
			this.thickness = thickness;
		}

		public float Area => 4 * Mathf.PI * Mathf.PI * radius * thickness;

		public float Volume => 2 * Mathf.PI * Mathf.PI * radius * thickness * thickness;

		public void ToMesh(Mesh resultMesh) => ToMesh(resultMesh, defaultComplexityBigCircle, defaultComplexitySmallCircle);

		public void ToMesh(Mesh resultMesh, int nbRadSeg, int nbSides)
		{
			resultMesh.Clear();

			#region Vertices

			var vertices = new Vector3[(nbRadSeg + 1) * (nbSides + 1)];
			float _2pi = Mathf.PI * 2f;
			for (int seg = 0; seg <= nbRadSeg; seg++)
			{
				int currSeg = seg == nbRadSeg ? 0 : seg;

				float t1 = (float)currSeg / nbRadSeg * _2pi;
				Vector3 r1 = new Vector3(Mathf.Cos(t1) * radius, 0f, Mathf.Sin(t1) * radius);

				for (int side = 0; side <= nbSides; side++)
				{
					int currSide = side == nbSides ? 0 : side;

					float t2 = (float)currSide / nbSides * _2pi;
					Vector3 r2 = Quaternion.AngleAxis(-t1 * Mathf.Rad2Deg, Vector3.up) *
								 new Vector3(Mathf.Sin(t2) * thickness, Mathf.Cos(t2) * thickness);

					vertices[side + seg * (nbSides + 1)] = r1 + r2;
				}
			}

			#endregion

			#region Normales

			var normals = new Vector3[vertices.Length];
			for (int seg = 0; seg <= nbRadSeg; seg++)
			{
				int currSeg = seg == nbRadSeg ? 0 : seg;

				float t1 = (float)currSeg / nbRadSeg * _2pi;
				Vector3 r1 = new Vector3(Mathf.Cos(t1) * radius, 0f, Mathf.Sin(t1) * radius);

				for (int side = 0; side <= nbSides; side++)
				{
					normals[side + seg * (nbSides + 1)] = (vertices[side + seg * (nbSides + 1)] - r1).normalized;
				}
			}

			#endregion

			#region UVs

			var uvs = new Vector2[vertices.Length];
			for (int seg = 0; seg <= nbRadSeg; seg++)
				for (int side = 0; side <= nbSides; side++)
					uvs[side + seg * (nbSides + 1)] = new Vector2((float)seg / nbRadSeg, (float)side / nbSides);

			#endregion

			#region Triangles

			int nbFaces = vertices.Length;
			int nbTriangles = nbFaces * 2;
			int nbIndexes = nbTriangles * 3;
			int[] triangles = new int[nbIndexes];

			int i = 0;
			for (int seg = 0; seg <= nbRadSeg; seg++)
			{
				for (int side = 0; side <= nbSides - 1; side++)
				{
					int current = side + seg * (nbSides + 1);
					int next = side + (seg < nbRadSeg ? (seg + 1) * (nbSides + 1) : 0);

					if (i < triangles.Length - 6)
					{
						triangles[i++] = current;
						triangles[i++] = next;
						triangles[i++] = next + 1;

						triangles[i++] = current;
						triangles[i++] = next + 1;
						triangles[i++] = current + 1;
					}
				}
			}

			#endregion

			resultMesh.vertices = vertices;
			resultMesh.normals = normals;
			resultMesh.uv = uvs;
			resultMesh.triangles = triangles;

			resultMesh.RecalculateBounds();
		}
	}
	[Serializable]
	public class SpatialTorus : SpatialMesh<Torus> { }
}