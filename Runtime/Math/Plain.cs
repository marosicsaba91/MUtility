using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Plain
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

		public float DistanceToPoint(Vector3 point) =>
			Vector3.Dot(point - origin, normal.normalized);

		internal void Normalise() => normal = normal.normalized;

		internal float DistanceToPoint(object lastPosition)
		{
			throw new NotImplementedException();
		}
	}
}
