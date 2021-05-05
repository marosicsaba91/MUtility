using UnityEngine;

namespace MUtility
{
    public static class PolygonHelper
    {
        public static Vector3[] Transform(this Vector3[] polygon, Transform transform)
        {
            for (int i = 0; i < polygon.Length; i++)
                polygon[i] = transform.TransformPoint(polygon[i]);
            return polygon;
        }

        public static Vector3[] Offset(this Vector3[] polygon, Vector3 offset)
        {
            for (int i = 0; i < polygon.Length; i++)
                polygon[i] += offset;
            return polygon;
        }

        public static Vector3[] Rotate(this Vector3[] polygon, Quaternion rotate)
        {
            for (int i = 0; i < polygon.Length; i++)
                polygon[i] = rotate * polygon[i];
            return polygon;
        }

        public static Vector3[] Scale(this Vector3[] polygon, float scale)
        {
            for (int i = 0; i < polygon.Length; i++)
                polygon[i] = new Vector3(
                    scale * polygon[i].x,
                    scale * polygon[i].y,
                    scale * polygon[i].z);
            return polygon;
        }

        public static Vector3[] Scale(this Vector3[] polygon, Vector3 scale)
        {
            for (int i = 0; i < polygon.Length; i++)
                polygon[i] = new Vector3(
                    scale.x * polygon[i].x,
                    scale.y * polygon[i].y,
                    scale.z * polygon[i].z);
            return polygon;
        }

        public static Vector3[] Transform(this Vector3[] polygon, Vector3 offset, Quaternion rotate, float scale)
        {
            for (int i = 0; i < polygon.Length; i++)
            {
                polygon[i] = rotate * polygon[i];
                polygon[i] = new Vector3(
                    (scale * polygon[i].x) + offset.x,
                    (scale * polygon[i].y) + offset.y,
                    (scale * polygon[i].z) + offset.z);
            }
            return polygon;
        }

        public static Vector3[] Transform(this Vector3[] polygon, Vector3 offset, Quaternion rotate, Vector3 scale)
        {
            for (int i = 0; i < polygon.Length; i++)
            {
                polygon[i] = rotate * polygon[i];
                polygon[i] = new Vector3(
                    (scale.x * polygon[i].x) + offset.x,
                    (scale.y * polygon[i].y) + offset.y,
                    (scale.z * polygon[i].z) + offset.z);
            }
            return polygon;
        }

        public static void DrawGizmo(this Vector3[] polygon, Color color) =>
            DrawPolygonDebug(polygon, color, DrawingType.Gizmo);

        public static void DrawDebug(this Vector3[] polygon, Color color) =>
            DrawPolygonDebug(polygon, color, DrawingType.Debug);

        // Private

        enum DrawingType { Gizmo, Debug }
        static void DrawPolygonDebug(Vector3[] polygon, Color color, DrawingType type)
        {
            if (polygon.Length <= 1) { return; }

            if (type == DrawingType.Debug)
            {
                for (int i = 0; i < polygon.Length - 1; i++)
                {
                    Debug.DrawLine(polygon[i], polygon[i + 1], color);
                }
            }
            else
            {
                Gizmos.color = color;
                for (int i = 0; i < polygon.Length - 1; i++)
                {
                    Gizmos.DrawLine(polygon[i], polygon[i + 1]);
                }
            }
        }
    }
}