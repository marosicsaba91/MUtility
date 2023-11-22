using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class FixRadiusSpline : Spline<Vector3>
	{
		[SerializeField] float radius = 1f;
		[SerializeField] float breakAngle = 15f;
		// [SerializeField] float maxDistanceFromControlPoint = 1f;

		readonly List<Vector3> _segments = new();
		readonly List<Vector3> _directions = new();
		readonly List<Vector3> _curveEnds = new();
		readonly List<Vector3> _centers = new();

		public IReadOnlyList<Vector3> CurveEnds => _curveEnds;
		public IReadOnlyList<Vector3> Centers => _centers;
		public float BreakAngle
		{
			get => breakAngle;
			set
			{
				value = Mathf.Clamp(value, 0, 180);

				if (Math.Abs(value - breakAngle) < float.Epsilon)
					return;

				breakAngle = value;
				IsDirty = true;
			}
		}

		public float Radius
		{
			get => radius;
			set
			{
				value = Mathf.Clamp(value, 0, 180);

				if (Math.Abs(value - radius) < float.Epsilon)
					return;

				radius = value;
				IsDirty = true;
			}
		}

		public override Vector3 PositionToControlPoint(Vector3 point) => point;
		protected override Ray EvaluateSafe(float continuousIndex, int previousIndex, int index, int nextIndex, int next2Index)
		{
			return new Ray(); // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 
		}

		protected override Vector3 ControlPointToPosition(Vector3 controlPoint) => controlPoint;


		protected override void SafeRecalculatePoints(List<InterpolatedPoint> result, out Bounds bounds, out float length)
		{
			_centers.Clear();
			_curveEnds.Clear();
			_segments.Clear();
			_directions.Clear();

			bounds = new Bounds(controlPoints[0], Vector3.zero);
			length = 0;
			result.Add(new InterpolatedPoint(0, controlPoints[0], Vector3.zero, 0));

			if (controlPoints.Count == 2)
			{
				result.Add(new InterpolatedPoint(0, controlPoints[1], Vector3.zero, 0));
				_curveEnds.Add(controlPoints[1]);
				return;
			}

			for (int i = 0; i < controlPoints.Count - 2; i++)
			{
				Vector3 p0 = controlPoints[i];
				Vector3 p1 = controlPoints[i + 1];
				Vector3 p2 = controlPoints[i + 2];

				result.AddRange(GetArc(p0, p1, p2));
			}

			result.Add(new InterpolatedPoint(0, controlPoints[^1], Vector3.zero, 0));
		}

		IEnumerable<InterpolatedPoint> GetArc(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			Vector3 segment1 = p1 - p0;
			Vector3 segment2 = p2 - p1;
			float segment1Length = segment1.magnitude;
			float segment2Length = segment2.magnitude;
			Vector3 dir1 = segment1 / segment1Length;
			Vector3 dir2 = segment2 / segment2Length;

			float angle = Vector3.Angle(segment1, segment2);
			float d = Mathf.Tan(angle * Mathf.Deg2Rad / 2) * radius;

			Vector3 perpendicular1 = Vector3.Cross(dir1, dir2);
			Vector3 arcStart = p1 - dir1 * d;
			Vector3 arcEnd = p1 + dir2 * d;
			Vector3 center = Vector3.Cross(perpendicular1, dir1) * radius + arcStart;
			int arcPointCount = Mathf.CeilToInt(angle / breakAngle) + 1;

			_curveEnds.Add(arcStart);
			_curveEnds.Add(arcEnd);
			_centers.Add(center);

			Vector3 startDir = arcStart - center;
			Vector3 endDir = arcEnd - center;

			yield return new InterpolatedPoint(0, arcStart, Vector3.zero, 0);

			for (int i = 1; i < arcPointCount - 1; i++)
			{
				Vector3 rv = Vector3.Slerp(startDir, endDir, (float)i / arcPointCount);
				Vector3 point = center + rv;
				yield return new InterpolatedPoint(0, point, Vector3.zero, 0);
			}

			yield return new InterpolatedPoint(0, arcEnd, Vector3.zero, 0);
		}

	}
}