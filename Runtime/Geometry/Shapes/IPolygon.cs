using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public static IEnumerable<Vector3> Transform(this IEnumerable<Vector3> polygon, Transform transform) =>
        polygon.Select(transform.TransformPoint);

    public static IEnumerable<Vector3> Offset(this IEnumerable<Vector3> polygon, Vector3 offset) =>
        polygon.Select(p => p+offset); 

    public static IEnumerable<Vector3> Rotate(this IEnumerable<Vector3> polygon, Quaternion rotate) =>
        polygon.Select(p => rotate * p); 

    public static IEnumerable<Vector3> Scale(this IEnumerable<Vector3> polygon, float scale) =>
        polygon.Select(p => p * scale);

    public static IEnumerable<Vector3> Scale(this IEnumerable<Vector3> polygon, Vector3 scale) =>
    polygon.Select(p => new Vector3(
        scale.x * p.x,
        scale.y * p.y,
        scale.z * p.z));

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
        DrawPolygonDebug(polygon, color, Drawable.DrawingType.Gizmo);

    public static void DrawGizmo(this IEnumerable<Vector3> polygon) =>
        DrawPolygonDebug(polygon, null, Drawable.DrawingType.Gizmo);

    public static void DrawGizmo(this IEnumerable<Vector3> polygon, Color? color) =>
        DrawPolygonDebug(polygon, color, Drawable.DrawingType.Debug);

    public static void DrawDebug(this IEnumerable<Vector3> polygon, Color color) =>
        DrawPolygonDebug(polygon, color, Drawable.DrawingType.Debug);

    public static void DrawDebug(this IEnumerable<Vector3> polygon) =>
        DrawPolygonDebug(polygon, null, Drawable.DrawingType.Debug);

    public static void DrawDebug(this IEnumerable<Vector3> polygon, Color? color) =>
        DrawPolygonDebug(polygon, color, Drawable.DrawingType.Debug);

    // Private

    static void DrawPolygonDebug(IEnumerable<Vector3> polygon, Color? color, Drawable.DrawingType type)
    {
        if (type == Drawable.DrawingType.Debug)
        {
            Vector3? last = null;
            foreach (Vector3 p in polygon)
            {
                Color c = color ?? Color.white;
                if(last != null)
                    Debug.DrawLine(last.Value, p, c); 
                last = p;
            }
        }
        else if (type == Drawable.DrawingType.Gizmo)
        {
            if (color.HasValue)
                Gizmos.color = color.Value;
            
            Vector3? last = null;
            foreach (Vector3 p in polygon)
            { 
                if(last != null)
                    Gizmos.DrawLine(last.Value, p);  
                last = p; 
            }
        }
    }
}
}