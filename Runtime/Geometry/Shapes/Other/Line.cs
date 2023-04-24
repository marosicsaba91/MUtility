using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Line : IDrawable/*, IEasyHandleable*/
	{
		public Vector3 a;
		public Vector3 b;

		public Line(Vector3 a, Vector3 b)
		{
			this.a = a;
			this.b = b;
		}

		public Line(LineSegment segment)
		{
			a = segment.a;
			b = segment.b;
		}

		public Line(Ray ray)
		{
			a = ray.origin;
			b = ray.origin + ray.direction;
		}

		Vector3 DirectionNormal
		{
			get => (b - a).normalized;
			set => b = a + value;
		}

		const float defaultLength = 100f;

		public Drawable ToDrawable() => ToDrawable(defaultLength);

		public Drawable ToDrawable(float length)
		{
			Vector3 center = (a + b) / 2f;
			Vector3 dir = DirectionNormal * (length / 2f);
			return new Drawable(new[] { center - dir, center + dir });
		}

		/*
		public IEnumerable<EasyHandle> GetHandles()
		{
			yield return new EasyHandle() { position = a };
			yield return new EasyHandle() { position = b }; 
		}

		public void SetHandle(int index, HandleResult result)
		{
			if (index == 0)
				a = result.startPosition;
			else
				b = result.startPosition;
		}
		*/

		public float ClosestPointOnLineToPointRate(Vector3 point) =>
			ClosestPointOnLineToPointRate(a, b, point);

		public Vector3 ClosestPointOnLineToPoint(Vector3 point) =>
			Vector3.LerpUnclamped(a, b, ClosestPointOnLineToPointRate(point));

		public bool TryGetShortestSegmentToLine(Line l2, out LineSegment shortest) =>
			TryGetShortestSegmentBetweenLines(this, l2, out shortest);

		public bool TryGetShortestSegmentToSegment(LineSegment s2, out LineSegment shortest) =>
			TryGetShortestSegmentBetweenLines(
				this,
				new Line(s2.a, s2.b),
				l1AClosed: false,
				l1BClosed: false,
				l2AClosed: true,
				l2BClosed: true,
				out shortest);


		// STATIC

		public static float ClosestPointOnLineToPointRate(Vector3 lineA, Vector3 lineB, Vector3 point)
		{
			Vector3 u = lineB - lineA;
			Vector3 v = point - lineA;
			return Vector3.Dot(v, u) / u.sqrMagnitude;
		}

		public static bool TryGetShortestSegmentBetweenLines(Line l1, Line l2, out LineSegment shortest) =>
			TryGetShortestSegmentBetweenLines(
				l1, l2,
				l1AClosed: false,
				l1BClosed: false,
				l2AClosed: false,
				l2BClosed: false,
				out shortest);

		public static bool TryGetShortestSegmentBetweenLines(
			Line l1, Line l2,
			bool l1AClosed,
			bool l1BClosed,
			bool l2AClosed,
			bool l2BClosed,
			out LineSegment shortest)
		{
			if (TryGetShortestSegmentBetweenLines(
				l1, l2,
				l1AClosed, l1BClosed, l2AClosed, l2BClosed,
				out float rate1, out float rate2))
			{
				Vector3 resultA = Vector3.Lerp(l1.a, l1.b, rate1);
				Vector3 resultB = Vector3.Lerp(l2.a, l2.b, rate2);
				shortest = new LineSegment(resultA, resultB);
				return true;
			}

			shortest = default;
			return false;
		}

		public static bool TryGetShortestSegmentBetweenLines(
			Line l1, Line l2,
			bool l1AClosed,
			bool l1BClosed,
			bool l2AClosed,
			bool l2BClosed,
			out float rate1,
			out float rate2)
		{

			if (!TryGetShortestSegmentBetweenLines(l1, l2, out rate1, out rate2))
				return false;

			if (l1AClosed && rate1 < 0)
			{
				rate1 = 0;
				rate2 = ClosestPointOnLineToPointRate(l1.a, l2.b, l1.a);
				return true;
			}

			if (l1BClosed && rate1 > 1)
			{
				rate1 = 1;
				rate2 = ClosestPointOnLineToPointRate(l2.a, l2.b, l1.b);
				return true;
			}

			if (l2AClosed && rate2 < 0)
			{
				rate2 = 0;
				rate1 = ClosestPointOnLineToPointRate(l1.a, l1.b, l2.a);
				return true;
			}

			if (l2BClosed && rate1 > 1)
			{
				rate2 = 1;
				rate1 = ClosestPointOnLineToPointRate(l1.a, l1.b, l2.b);
				return true;
			}

			return true;
		}

		public static bool TryGetShortestSegmentBetweenLines(Line l1, Line l2, out float rate1, out float rate2)
		{
			const float epsilon = float.Epsilon;
			rate1 = 0;
			rate2 = 0;

			Vector3 v1 = l1.a - l2.a;
			Vector3 v2 = l2.b - l2.a;

			if (v2.sqrMagnitude < epsilon)
				return false;

			Vector3 v3 = l1.b - l1.a;

			if (v3.sqrMagnitude < epsilon)
				return false;

			float d12 = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
			float d23 = v2.x * v3.x + v2.y * v3.y + v2.z * v3.z;
			float d13 = v1.x * v3.x + v1.y * v3.y + v1.z * v3.z;
			float d22 = v2.x * v2.x + v2.y * v2.y + v2.z * v2.z;
			float d33 = v3.x * v3.x + v3.y * v3.y + v3.z * v3.z;

			float denominator = d33 * d22 - d23 * d23;

			if (Mathf.Abs(denominator) < epsilon)
				return false;

			float n = d12 * d23 - d13 * d22;

			rate1 = n / denominator;
			rate2 = (d12 + d23 * rate1) / d22;
			return true;
		}
	}
}