using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct LineSegment : IDrawable/*, IEasyHandleable*/
	{
		public Vector3 a;
		public Vector3 b;

		public LineSegment(Vector3 a, Vector3 b)
		{
			this.a = a;
			this.b = b;
		}

		public float Magnitude => (b - a).magnitude;

		public float SqrMagnitude => (b - a).sqrMagnitude;

		Vector3 LerpOnLineUnclamped(float value) => Vector3.LerpUnclamped(a, b, value);

		Vector3 LerpOnLine(float value) => Vector3.Lerp(a, b, value);

		public Drawable ToDrawable() => new Drawable(new[] { a, b });

		public Vector3 ClosestPointOnSegmentToPoint(Vector3 point) =>
			Vector3.Lerp(a, b, Line.ClosestPointOnLineToPointRate(a, b, point));

		public Vector3 ClosestPointOnLineToPoint(Vector3 point) =>
			Vector3.LerpUnclamped(a, b, Line.ClosestPointOnLineToPointRate(a, b, point));

		public bool TryGetShortestSegmentToLine(Line l2, out LineSegment shortest) =>
			Line.TryGetShortestSegmentBetweenLines(
				new Line(a, b),
				new Line(l2.a, l2.b),
				l1AClosed: true,
				l1BClosed: true,
				l2AClosed: false,
				l2BClosed: false,
				out shortest);

		public bool TryGetShortestSegmentToSegment(LineSegment s2, out LineSegment shortest) =>
			Line.TryGetShortestSegmentBetweenLines(
				new Line(a, b),
				new Line(s2.a, s2.b),
				l1AClosed: true,
				l1BClosed: true,
				l2AClosed: true,
				l2BClosed: true,
				out shortest);

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

	}
}
