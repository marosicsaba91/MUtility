using System;
using UnityEngine;

namespace MUtility
{
	public static class QuaternionExtensions
	{
		public static bool CompareApproximately(this Quaternion one, Quaternion other)
		{
			const float tolerance = 0.0000001f;
			if (Math.Abs(one.x - other.x) > tolerance)
				return false;
			if (Math.Abs(one.y - other.y) > tolerance)
				return false;
			if (Math.Abs(one.z - other.z) > tolerance)
				return false;
			if (Math.Abs(one.w - other.w) > tolerance)
				return false;
			return true;
		}

		public static void DrawGizmo(this Quaternion rotation, Vector3 position, float size = 1) =>
			DrawGizmo(rotation, position, Color.white, size);

		public static void DrawGizmo(this Quaternion rotation, Vector3 position, Color color, float size = 1) =>
			new Pose(position, rotation).DrawGizmo(color, size);

		public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Quaternion velocity, float smoothTime, float deltaTime)
		{
			float dot = Quaternion.Dot(current, target);
			float multi = dot > 0f ? 1f : -1f;
			target.x *= multi;
			target.y *= multi;
			target.z *= multi;
			target.w *= multi;

			// smooth damp (lerp approx)
			Vector4 result = new Vector4(
				Mathf.SmoothDamp(current.x, target.x, ref velocity.x, smoothTime, float.PositiveInfinity, deltaTime),
				Mathf.SmoothDamp(current.y, target.y, ref velocity.y, smoothTime, float.PositiveInfinity, deltaTime),
				Mathf.SmoothDamp(current.z, target.z, ref velocity.z, smoothTime, float.PositiveInfinity, deltaTime),
				Mathf.SmoothDamp(current.w, target.w, ref velocity.w, smoothTime, float.PositiveInfinity, deltaTime)
			).normalized;

			// ensure velocity is tangent
			Vector4 velocityError = Vector4.Project(new Vector4(velocity.x, velocity.y, velocity.z, velocity.w), result);
			velocity.x -= velocityError.x;
			velocity.y -= velocityError.y;
			velocity.z -= velocityError.z;
			velocity.w -= velocityError.w;

			return new Quaternion(result.x, result.y, result.z, result.w);
		}

		public static bool IsNaN(this Quaternion q) =>
			float.IsNaN(q.x) && float.IsNaN(q.y) && float.IsNaN(q.z) && float.IsNaN(q.w);
	}
}