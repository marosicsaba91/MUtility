using UnityEngine;

namespace MUtility
{
	public static class GeometryHelper
	{
		public static Vector2 RotatePointAroundPivot_Rad(Vector2 point, Vector2 pivot, float angle)
		{
			float rotatedX = Mathf.Cos(angle) * (point.x - pivot.x) - Mathf.Sin(angle) * (point.y - pivot.y) + pivot.x;
			float rotatedY = Mathf.Sin(angle) * (point.x - pivot.x) + Mathf.Cos(angle) * (point.y - pivot.y) + pivot.y;
			return new Vector2(rotatedX, rotatedY);
		}

		public static Vector3 RotatePointAroundPivot_Rad(Vector3 point, Vector3 pivot, Vector3 angle)
		{
			Vector3 direction = point - pivot;
			direction = Quaternion.Euler(angle) * direction;
			return direction + pivot;
		}

		public static Vector2 RotateVector2_Rad(Vector2 vector, float angle)
		{
			if (angle == 0)
				return vector;

			float sinus = Mathf.Sin(angle);
			float cosinus = Mathf.Cos(angle);

			float oldX = vector.x;
			float oldY = vector.y;
			vector.x = cosinus * oldX - sinus * oldY;
			vector.y = sinus * oldX + cosinus * oldY;
			return vector;
		}

		public static Vector2 RadianToVector2D(float angle) =>
			new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));


		public static float Vector2DToRadian(Vector2 vec)
			=> Mathf.Atan2(vec.y, vec.x);

		public static float SignedAngleBetween_Deg(Vector2 vectorA, Vector2 vectorB)
		{
			float angle = Vector2.Angle(vectorA, vectorB);
			Vector3 cross = Vector3.Cross(vectorA, vectorB);

			return cross.z > 0 ? 360 - angle : angle;
		}

		public static float DistanceBetweenPointAndLine(Vector3 point, Line line) =>
			Vector3.Magnitude(ProjectPointOnLine(point, line) - point);



		public static Vector3 ProjectPointOnLine(Vector3 point, Line line)
		{
			Vector3 rhs = point - line.a;
			Vector3 vector2 = line.b - line.a;
			float magnitude = vector2.magnitude;
			Vector3 lhs = vector2;
			if (magnitude > 1E-06f)
			{
				lhs = lhs / magnitude;
			}
			float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
			return line.a + lhs * num2;
		}

		public static bool TryGet2DLineSegmentsIntersectionPoint(LineSegment line1, LineSegment line2, out Vector2 intersection)
		{
			intersection = Vector2.zero;

			float d =
				(line1.b.x - line1.a.x) * (line2.b.y - line2.a.y) -
				(line1.b.y - line1.a.y) * (line2.b.x - line2.a.x);

			if (d == 0.0f)
				return false;

			float u =
				((line2.a.x - line1.a.x) * (line2.b.y - line2.a.y)
				- (line2.a.y - line1.a.y) * (line2.b.x - line2.a.x)) / d;

			float v =
				((line2.a.x - line1.a.x) * (line1.b.y - line1.a.y)
				- (line2.a.y - line1.a.y) * (line1.b.x - line1.a.x)) / d;

			if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
				return false;

			intersection.x = line1.a.x + u * (line1.b.x - line1.a.x);
			intersection.y = line1.a.y + u * (line1.b.y - line1.a.y);

			return true;
		}

		public static float SimplifyAngle(float angle)
		{
			//TODO

			while (angle > 180f)
				angle -= 360f;

			while (angle < -180f)
				angle += 360f;

			return angle;
		}

		public static float GetWeightedAverageAngle(float angleA, float angleB, float ratio = 0.5f)
		{
			return angleA + SimplifyAngle(angleB - angleA) * ratio;
		}
	}
}