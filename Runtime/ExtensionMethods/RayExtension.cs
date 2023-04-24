using UnityEngine;

namespace MUtility
{
	public static class RayExtension
	{

		public static Ray Transform(this Ray ray, Matrix4x4 matrix) =>
			new Ray(matrix.MultiplyPoint(ray.origin), matrix.MultiplyVector(ray.direction));

		public static bool IntersectPlane(this Ray ray, Vector3 planeNormal, Vector3 planeOrigin, out Vector3 hit)
		{
			return ray.IntersectPlaneFast(planeNormal.normalized, planeOrigin.normalized, out hit);
		}

		/// <summary>
		/// In this method we assume that all input vectors are all normalized
		/// </summary>
		/// 
		public static bool IntersectPlaneFast(this Ray ray, Vector3 planeNormal, Vector3 planeOrigin, out Vector3 hit)
		{
			// assuming vectors are all normalized
			float denom = Vector3.Dot(planeNormal, ray.direction);
			const float epsylon = 0.0001f;
			if (denom < epsylon)
			{
				hit = default;
				return false;
			}

			Vector3 p0L0 = planeOrigin - ray.origin;
			float t = Vector3.Dot(p0L0, planeNormal) / denom;
			hit = (ray.origin + (ray.direction * t)) - planeOrigin;
			return true;
		}
	}
}
