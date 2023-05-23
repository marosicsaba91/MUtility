using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace MUtility
{
	public interface IPolygon
	{
		IEnumerable<Vector3> Points { get; }
		Bounds Bounds { get; }
		float Length { get; }
	}

	public static class PolygonExtensions
	{
		public static Drawable ToDrawable(this IEnumerable<Vector3> polygon)
		{
			Vector3[] points = polygon.ToArray();
			return new Drawable(points);
		}

		public static IEnumerable<Vector3> TransformAll(this IEnumerable<Vector3> polygon, Transform transform) =>
			polygon.Select(transform.TransformPoint);

		public static IEnumerable<Vector3> TranslateAll(this IEnumerable<Vector3> polygon, Vector3 offset) =>
			polygon.Select(p => p + offset);

		public static IEnumerable<Vector3> RotateAll(this IEnumerable<Vector3> polygon, Quaternion rotate) =>
			polygon.Select(p => rotate * p);

		public static IEnumerable<Vector3> ScaleAll(this IEnumerable<Vector3> polygon, float scale) =>
			polygon.Select(p => p * scale);

		public static IEnumerable<Vector3> ScaleAll(this IEnumerable<Vector3> polygon, Vector3 scale) =>
		polygon.Select(p => new Vector3(
			scale.x * p.x,
			scale.y * p.y,
			scale.z * p.z));


		public static void Transform(this Vector3[] polygon, Transform transform)
		{
			for (int i = 0; i < polygon.Length; i++)
				polygon[i] = transform.TransformPoint(polygon[i]);
		}

		public static void Translate(this Vector3[] polygon, Vector3 offset)
		{
			for (int i = 0; i < polygon.Length; i++)
				polygon[i] = polygon[i] + offset;
		}

		public static void Rotate(this Vector3[] polygon, Quaternion rotate)
		{
			for (int i = 0; i < polygon.Length; i++)
				polygon[i] = rotate * polygon[i];
		}

		public static void Scale(this Vector3[] polygon, float scale)
		{
			for (int i = 0; i < polygon.Length; i++)
				polygon[i] = scale * polygon[i];
		}

		public static void Scale(this Vector3[] polygon, Vector3 scale)
		{
			for (int i = 0; i < polygon.Length; i++)
				polygon[i] = polygon[i].MultiplyAllAxis(scale);
		}

		public static void Transform(this Vector3[] polygon, Vector3 offset, Quaternion rotate, float scale = 1)
		{
			for (int i = 0; i < polygon.Length; i++)
			{
				Vector3 point = polygon[i];
				Vector3 p = rotate * point;
				polygon[i] = new Vector3(
					scale * p.x + offset.x,
					scale * p.y + offset.y,
					scale * p.z + offset.z);
			}
		}

		public static void Transform(this Vector3[] polygon, Vector3 offset, Quaternion rotate, Vector3 scale)
		{
			for (int i = 0; i < polygon.Length; i++)
			{
				Vector3 point = polygon[i];
				Vector3 p = rotate * point;
				polygon[i] = new Vector3(
					scale.x * p.x + offset.x,
					scale.y * p.y + offset.y,
					scale.z * p.z + offset.z);
			}
		}

		public static IEnumerable<Vector3> Transform(this IEnumerable<Vector3> polygon, Vector3 offset, Quaternion rotate, float scale = 1)
		{
			foreach (Vector3 point in polygon)
			{
				Vector3 p = rotate * point;
				yield return new Vector3(
					scale * p.x + offset.x,
					scale * p.y + offset.y,
					scale * p.z + offset.z);
			}
		}

		public static IEnumerable<Vector3> Transform(this IEnumerable<Vector3> polygon, Vector3 offset, Quaternion rotate, Vector3 scale)
		{
			foreach (Vector3 point in polygon)
			{
				Vector3 p = rotate * point;
				yield return new Vector3(
					scale.x * p.x + offset.x,
					scale.y * p.y + offset.y,
					scale.z * p.z + offset.z);
			}
		}

		public static void DrawGizmo(this IEnumerable<Vector3> polygon, Color color) =>
			DrawPolygon(polygon, color, Drawable.DrawingType.Gizmo);

		public static void DrawGizmo(this IEnumerable<Vector3> polygon) =>
			DrawPolygon(polygon, default, Drawable.DrawingType.Gizmo);


		public static void DrawDebug(this IEnumerable<Vector3> polygon, Color color) =>
			DrawPolygon(polygon, color, Drawable.DrawingType.Debug);

		public static void DrawDebug(this IEnumerable<Vector3> polygon) =>
			DrawPolygon(polygon, default, Drawable.DrawingType.Debug);


		public static void DrawHandle(this IEnumerable<Vector3> polygon, Color color) =>
			DrawPolygon(polygon, color, Drawable.DrawingType.Handle);

		public static void DrawHandle(this IEnumerable<Vector3> polygon) =>
			DrawPolygon(polygon, default, Drawable.DrawingType.Handle);


		// Private

		static void DrawPolygon(IEnumerable<Vector3> polygon, Color color, Drawable.DrawingType type)
		{
			if (type == Drawable.DrawingType.Debug)
			{
				Vector3? last = null;
				Color c = color == default ? color : Color.white;
				foreach (Vector3 p in polygon)
				{
					if (last != null)
						Debug.DrawLine(last.Value, p, c);
					last = p;
				}
			}
			else if (type == Drawable.DrawingType.Gizmo)
			{
				if (color != default)
					Gizmos.color = color;

				Vector3? last = null;
				foreach (Vector3 p in polygon)
				{
					if (last != null)
						Gizmos.DrawLine(last.Value, p);
					last = p;
				}
			}
			else if (type == Drawable.DrawingType.Handle)
			{
#if UNITY_EDITOR

				if (color != default)
					UnityEditor.Handles.color = color;

				Vector3? last = null;
				foreach (Vector3 p in polygon)
				{
					if (last != null)
						UnityEditor.Handles.DrawLine(last.Value, p);
					last = p;
				}

#endif
			}
		}
	}
}