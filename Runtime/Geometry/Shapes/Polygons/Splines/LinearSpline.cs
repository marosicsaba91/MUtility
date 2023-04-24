using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class LinearSpline : Spline<Vector3>
	{
		public LinearSpline() : base(Array.Empty<Vector3>())
		{
		}

		public float Area => Mathf.Abs(SignedDoubleArea() * 0.5f);

		public Winding? GetWinding()
		{
			float signedDoubleArea = SignedDoubleArea();
			if (signedDoubleArea < 0)
			{
				return Winding.Clockwise;
			}

			if (signedDoubleArea > 0)
			{
				return Winding.CounterClockwise;
			}

			return null;
		}

		float SignedDoubleArea()
		{
			int index;
			int n = controlPoints.Count;
			float signedArea = 0;
			for (index = 0; index < n; ++index)
			{
				int nextIndex = (index + 1) % n;
				Vector2 point = controlPoints[index];
				Vector2 next = controlPoints[nextIndex];
				signedArea += point.x * next.y - next.x * point.y;
			}

			return signedArea;
		}

		protected override Ray EvaluateSafe(float continuousIndex, int previousIndex, int index, int nextIndex, int next2Index)
		{
			Vector3 start = controlPoints[index];
			Vector3 end = controlPoints[nextIndex];
			float t = continuousIndex - index;

			Vector3 position = Vector3.Lerp(start, end, t);
			Vector3 forward = end - start;

			return new Ray(position, forward);
		}

		protected override Vector3 ControlPointToPosition(Vector3 controlPoint) => controlPoint;

		public override Vector3 PositionToControlPoint(Vector3 point) => point;

		protected override void SafeRecalculatePoints(List<InterpolatedPoint> result, out Bounds bounds, out float length)
		{
			length = 0;
			Vector2 min = controlPoints[0];
			Vector2 max = controlPoints[0];

			for (int i = 0; i < controlPoints.Count - 1; i++)
			{
				Vector3 point = controlPoints[i];
				Vector3 next = controlPoints[i + 1];
				Vector3 forward = next - point;
				min = Vector2.Min(min, point);
				max = Vector2.Max(max, point);

				result.Add(new InterpolatedPoint(i, point, forward, length));
				length += forward.magnitude;
			}

			if (isLoop)
			{
				Vector3 point = controlPoints[^1];
				Vector3 first = controlPoints[0];
				Vector3 forward = first - point;
				result.Add(new InterpolatedPoint(controlPoints.Count - 1, point, forward, length));
				length += forward.magnitude;

				Vector3 next = controlPoints[1];
				forward = next - first;
				result.Add(new InterpolatedPoint(controlPoints.Count, first, forward, length));
			}
			else
			{
				Vector3 point = controlPoints[^1];
				Vector3 forward = controlPoints[^1] - controlPoints[^2];
				result.Add(new InterpolatedPoint(controlPoints.Count - 1, point, forward, length));
			}

			bounds = new Bounds((min + max) * 0.5f, max - min);
		}

	}
}