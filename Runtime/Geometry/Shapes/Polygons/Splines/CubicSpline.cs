using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public abstract class CubicSpline<TControlPoint> : Spline<TControlPoint>
	{
		protected CubicSpline() : base(Array.Empty<TControlPoint>()) { }
		protected CubicSpline(bool isClosed, params TControlPoint[] nodes) : base(isClosed, nodes) { }
		protected CubicSpline(params TControlPoint[] nodes) : base(nodes) { }

		[SerializeField] int drawingPointPerSegment = 25;
		public int DrawingPointPerSegment
		{
			get => drawingPointPerSegment;
			set
			{
				value = Mathf.Max(1, value);

				if (drawingPointPerSegment == value)
					return;

				drawingPointPerSegment = value;
				IsDirty = true;
			}
		}

		protected override void SafeRecalculatePoints(List<InterpolatedPoint> result, out Bounds bounds, out float length)
		{
			if (drawingPointPerSegment < 1)
				drawingPointPerSegment = 1;

			float step = 1f / drawingPointPerSegment;
			length = 0;
			Ray ray = Evaluate(0);
			Vector3 lastPosition = ray.origin;
			bounds = new Bounds(ray.origin, Vector3.zero);
			result.Add(new InterpolatedPoint(0, ray.origin, ray.direction, 0));

			for (float i = step; i <= SegmentCount + step / 2f; i += step)
			{
				ray = Evaluate(i);
				bounds.Encapsulate(ray.origin);
				length += Vector3.Distance(lastPosition, ray.origin);
				result.Add(new InterpolatedPoint(i, ray.origin, ray.direction, length));
			}
		}
	}
}