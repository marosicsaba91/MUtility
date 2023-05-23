using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Plain : IDrawable
	{
		public Vector3 origin;
		public Vector3 normal;

		public Plain(Vector3 position, Vector3 normal)
		{
			origin = position;
			this.normal = normal;
		}

		public Vector3 Intersect(Line line) => Intersect(line.a, line.b, false).Value;
		public Vector3 Intersect(Ray ray) => Intersect(ray.origin, ray.origin + ray.direction, false).Value;

		public Vector3? Intersect(LineSegment segment) => Intersect(segment.a, segment.b, true);

		public Vector3? Intersect(Vector3 a, Vector3 b, bool isSegment)
		{
			Vector3 u = b - a;
			Vector3 w = a - origin;

			float t = Vector3.Dot(-normal, w) / Vector3.Dot(normal, u);

			if (isSegment && (t < 0 || t > 1))
				return null;

			return a + t * u;
		}

		const float defaultRadius = 100;
		public Drawable ToDrawable() => ToDrawable(defaultRadius, defaultRadius);

		public Drawable ToDrawable(float radius) => ToDrawable(radius, radius);

		public Drawable ToDrawable(float radius, float normalLength)
		{
			Vector3 v1 = normal.x == 0 && normal.y == 0 ? Vector3.up : Vector3.forward;

			Vector3 up = Vector3.Cross(v1, normal).normalized;
			Vector3 right = Vector3.Cross(up, normal).normalized;
			up *= radius * 0.5f;
			right *= radius * 0.5f;

			const int points = 25;
			var polygon = new Vector3[points];
			float angle = Mathf.PI * 2 / (points - 1);
			for (int i = 0; i < points - 1; i++)
			{
				float phase = i * angle;
				polygon[i] = origin + (Mathf.Sin(phase) * up + Mathf.Cos(phase) * right);
			}
			polygon[points - 1] = polygon[0];
			var arrowDrawable = new Arrow(origin, normal, radius).ToDrawable();
			var plainDrawable = new Drawable(polygon);
			plainDrawable.Merge(arrowDrawable);
			return plainDrawable;
		}

		public float DistanceToPoint(Vector3 point) =>
			Vector3.Dot(point - origin, normal.normalized);


		internal void Normalise() => normal = normal.normalized;

		internal float DistanceToPoint(object lastPosition)
		{
			throw new NotImplementedException();
		}
	}
}
