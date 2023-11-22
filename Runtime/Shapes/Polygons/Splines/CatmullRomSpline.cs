using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class CatmullRomSpline : CubicSpline<Vector3>
	{
		public CatmullRomSpline(bool isClosed, params Vector3[] points) : base(isClosed, points) { }
		public CatmullRomSpline(params Vector3[] points) : base(points) { }
		public CatmullRomSpline() : base(Array.Empty<Vector3>()) { }

		protected override Ray EvaluateSafe(float continuousIndex, int previousIndex, int index, int nextIndex, int next2Index)
		{
			Vector3 previous = previousIndex >= 0 ? controlPoints[previousIndex] : VirtualControlPointBeforeFirst;
			Vector3 start = controlPoints[index];
			Vector3 end = controlPoints[nextIndex];
			Vector3 next = next2Index >= 0 ? controlPoints[next2Index] : VirtualControlPointAfterLast;
			return InterpolateOnASegment(previous, start, end, next, continuousIndex);
		}

		protected override Vector3 ControlPointToPosition(Vector3 controlPoint) => controlPoint;

		protected Vector3 VirtualControlPointBeforeFirst => controlPoints[0] + (controlPoints[0 + 1] - controlPoints[0]);

		protected Vector3 VirtualControlPointAfterLast
		{
			get
			{
				int count = controlPoints.Count;
				Vector3 last = controlPoints[count - 1];
				Vector3 beforeLast = controlPoints[count - 2];
				return last + (beforeLast - last);
			}
		}

		public override Vector3 PositionToControlPoint(Vector3 point) => point;

		public void GetSegmentInfo(float controlPointIndex, out Vector3 previous, out Vector3 a, out Vector3 b, out Vector3 next)
		{
			int segmentCount = SegmentCount;
			int segmentIndex = (int)controlPointIndex;
			if (isLoop)
			{
				previous = controlPoints[MathHelper.Mod(segmentIndex - 1, segmentCount)];
				a = controlPoints[segmentIndex];
				b = controlPoints[(segmentIndex + 1) % segmentCount];
				next = controlPoints[(segmentIndex + 2) % segmentCount];
			}
			else
			{
				previous = segmentIndex > 0 ? controlPoints[segmentIndex - 1] : VirtualControlPointBeforeFirst;
				a = controlPoints[segmentIndex];
				b = controlPoints[segmentIndex + 1];
				next = segmentIndex + 2 < controlPoints.Count ? controlPoints[segmentIndex + 2] : VirtualControlPointAfterLast;
			}
		}

		public Ray InterpolateOnASegment(Vector3 previous, Vector3 a, Vector3 b, Vector3 next, float controlPointIndex)
		{
			float t = controlPointIndex % 1;

			float progressSquared = t * t;
			float progressCubed = progressSquared * t;

			Vector3 position = previous * (-0.5f * progressCubed + progressSquared + -0.5f * t);
			position += a * (1.5f * progressCubed + -2.5f * progressSquared + 1.0f);
			position += b * (-1.5f * progressCubed + 2.0f * progressSquared + 0.5f * t);
			position += next * (0.5f * progressCubed + -0.5f * progressSquared);


			Vector3 derivative = previous * (-1.5f * progressSquared + 2.0f * t + -0.5f);
			derivative += a * (4.5f * progressSquared + -5.0f * t);
			derivative += b * (-4.5f * progressSquared + 4.0f * t + 0.5f);
			derivative += next * (1.5f * progressSquared - t);


			return new Ray(position, derivative);
		}
	}
}