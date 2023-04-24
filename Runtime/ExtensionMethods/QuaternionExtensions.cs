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
	}
}