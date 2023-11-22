using UnityEngine;

namespace MUtility
{
	public struct InterpolatedPoint
	{
		public float controlPointIndex;
		public Vector3 position;
		public Vector3 direction;
		public float distance;

		public InterpolatedPoint(float controlPointIndex, Vector3 position, Vector3 direction, float distance)
		{
			this.controlPointIndex = controlPointIndex;

			this.position = position;
			this.direction = direction;
			this.distance = distance;
		}

		public static InterpolatedPoint Lerp(InterpolatedPoint p1, InterpolatedPoint p2, float t)
		{
			return new InterpolatedPoint(
				Mathf.Lerp(p1.controlPointIndex, p2.controlPointIndex, t),
				Vector3.Lerp(p1.position, p2.position, t),
				Vector3.Lerp(p1.direction, p2.direction, t),
				Mathf.Lerp(p1.distance, p2.distance, t));
		}

	}
}