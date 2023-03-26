using UnityEngine;

namespace MUtility
{
    public static class BoundsExtension
    {
        
        
        
        public static Bounds Transform(this Bounds bounds, Transform transform)
        {
            Bounds b = bounds;
            b.center = transform.TransformPoint(b.center);
            b.extents = transform.TransformVector(b.extents);
            return b;
        }
        
        
        public static Bounds Transform(this Bounds bounds, Vector3 offset, Quaternion rotate)
        {
            Bounds b = bounds;
            b.center = rotate * b.center + offset;
            b.extents = rotate * b.extents;
            return b;
        }
        
        
        public static Bounds Transform(this Bounds bounds, Vector3 offset, Quaternion rotate, Vector3 scale)
        {
            Bounds b = bounds;
            b.center = rotate * b.center + offset;
            b.extents = rotate * b.extents;
            b.extents.Scale(scale);
            return b;
        }
    }
}