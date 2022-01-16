using UnityEngine;

namespace MUtility
{
    public static class PolygonHelper
    {
        public static UnityEngine.Vector3[] Transform(this UnityEngine.Vector3[] polygon, Transform transform)
        {
            for (var i = 0; i < polygon.Length; i++)
                polygon[i] = transform.TransformPoint(polygon[i]);
            return polygon;
        }

        public static UnityEngine.Vector3[] Offset(this UnityEngine.Vector3[] polygon, UnityEngine.Vector3 offset)
        {
            for (var i = 0; i < polygon.Length; i++)
                polygon[i] += offset;
            return polygon;
        }

        public static UnityEngine.Vector3[] Rotate(this UnityEngine.Vector3[] polygon, Quaternion rotate)
        {
            for (var i = 0; i < polygon.Length; i++)
                polygon[i] = rotate * polygon[i];
            return polygon;
        }

        public static UnityEngine.Vector3[] Scale(this UnityEngine.Vector3[] polygon, float scale)
        {
            for (var i = 0; i < polygon.Length; i++)
                polygon[i] = new UnityEngine.Vector3(
                    scale * polygon[i].x,
                    scale * polygon[i].y,
                    scale * polygon[i].z);
            return polygon;
        }

        public static UnityEngine.Vector3[] Scale(this UnityEngine.Vector3[] polygon, UnityEngine.Vector3 scale)
        {
            for (var i = 0; i < polygon.Length; i++)
                polygon[i] = new UnityEngine.Vector3(
                    scale.x * polygon[i].x,
                    scale.y * polygon[i].y,
                    scale.z * polygon[i].z);
            return polygon;
        }

        public static UnityEngine.Vector3[] Transform(this UnityEngine.Vector3[] polygon, UnityEngine.Vector3 offset, Quaternion rotate, float scale)
        {
            for (var i = 0; i < polygon.Length; i++)
            {
                polygon[i] = rotate * polygon[i];
                polygon[i] = new UnityEngine.Vector3(
                    (scale * polygon[i].x) + offset.x,
                    (scale * polygon[i].y) + offset.y,
                    (scale * polygon[i].z) + offset.z);
            }
            return polygon;
        }

        public static UnityEngine.Vector3[] Transform(this UnityEngine.Vector3[] polygon, UnityEngine.Vector3 offset, Quaternion rotate, UnityEngine.Vector3 scale)
        {
            for (var i = 0; i < polygon.Length; i++)
            {
                polygon[i] = rotate * polygon[i];
                polygon[i] = new UnityEngine.Vector3(
                    (scale.x * polygon[i].x) + offset.x,
                    (scale.y * polygon[i].y) + offset.y,
                    (scale.z * polygon[i].z) + offset.z);
            }
            return polygon;
        }

        public static void DrawGizmo(this UnityEngine.Vector3[] polygon, Color color) =>
            DrawPolygonDebug(polygon, color, DrawingType.Gizmo);

        public static void DrawDebug(this UnityEngine.Vector3[] polygon, Color color) =>
            DrawPolygonDebug(polygon, color, DrawingType.Debug);

        // Private

        enum DrawingType { Gizmo, Debug }
        static void DrawPolygonDebug(UnityEngine.Vector3[] polygon, Color color, DrawingType type)
        {
            if (polygon.Length <= 1) { return; }

            if (type == DrawingType.Debug)
            {
                for (var i = 0; i < polygon.Length - 1; i++)
                {
                    Debug.DrawLine(polygon[i], polygon[i + 1], color);
                }
            }
            else
            {
                Gizmos.color = color;
                for (var i = 0; i < polygon.Length - 1; i++)
                {
                    Gizmos.DrawLine(polygon[i], polygon[i + 1]);
                }
            }
        }
    }
}