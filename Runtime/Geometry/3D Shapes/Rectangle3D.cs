using System;
using UnityEngine;

namespace MUtility
{
    [Serializable]
    public struct Rectangle3D : IEquatable<Rectangle3D>, IDrawable, I2DArea, I2DCircumference
    {
        public UnityEngine.Vector3 center;
        public Vector2 size;
        public Quaternion orientation;

        public Rectangle3D(RectTransform rectTransform)
        {
            Rect rect =  rectTransform.rect;
            UnityEngine.Vector3 lossyScale = rectTransform.lossyScale;
            center = rectTransform.TransformPoint(rect.center);
             
            size = new Vector2(rect.size.x * lossyScale.x, rect.size.y * lossyScale.y);
            orientation = rectTransform.rotation;
        }

        public Rectangle3D(UnityEngine.Vector3 center, Vector2 size)
        {
            this.center = center;
            this.size = size;
            orientation = Quaternion.identity;
        }
        public Rectangle3D(UnityEngine.Vector3 center, Vector2 size, Quaternion orientation)
        {
            this.center = center;
            this.size = size;
            this.orientation = orientation;
        }
        public Rectangle3D(UnityEngine.Vector3 center, Vector2 size, UnityEngine.Vector3 normal)
        {
            this.center = center;
            this.size = size;
            orientation = Quaternion.LookRotation(normal);
        }

        public Rectangle3D(UnityEngine.Vector3 center, Vector2 size, UnityEngine.Vector3 normal, UnityEngine.Vector3 up)
        {
            this.center = center;
            this.size = size;
            orientation = Quaternion.LookRotation(normal, up);
        }

        public UnityEngine.Vector3 Normal => orientation * UnityEngine.Vector3.forward;

        public UnityEngine.Vector3 Right => center + orientation * new UnityEngine.Vector3(size.x / 2, 0);
        public UnityEngine.Vector3 Left => center + orientation * new UnityEngine.Vector3(-size.x / 2, 0);
        public UnityEngine.Vector3 Top => center + orientation * new UnityEngine.Vector3(0, size.y / 2);
        public UnityEngine.Vector3 Bottom => center + orientation * new UnityEngine.Vector3(0, -size.y / 2);

        public UnityEngine.Vector3 TopRight => center + orientation * new UnityEngine.Vector3(size.x / 2, size.y / 2);
        public UnityEngine.Vector3 TopLeft => center + orientation * new UnityEngine.Vector3(-size.x / 2, size.y / 2);
        public UnityEngine.Vector3 BottomRight => center + orientation * new UnityEngine.Vector3(size.x / 2, -size.y / 2);
        public UnityEngine.Vector3 BottomLeft => center + orientation * new UnityEngine.Vector3(-size.x / 2, -size.y / 2);

        public UnityEngine.Vector3[] Corners => new[] { TopRight, TopLeft, BottomLeft, BottomRight};

        public Rectangle To2DRectangle => new Rectangle(center, size);

        public float Area => To2DRectangle.Area;

        public float Circumference => To2DRectangle.Circumference;

        public Vector2 GetRandomPointInArea() => To2DRectangle.GetRandomPointInArea(); 

        public bool IsPointInside(Vector2 point) => To2DRectangle.IsPointInside(point);

        public Drawable ToDrawable() =>
            new Drawable(new[] { TopRight, TopLeft, BottomLeft, BottomRight, TopRight });

        public static Rectangle3D Lerp(Rectangle3D startPosition, Rectangle3D endPosition, float rate)
        {
            if (rate <= 0) return startPosition;
            if (rate >= 1) return endPosition;
 
            UnityEngine.Vector3 center = 
                (startPosition.center == endPosition.center)? startPosition.center: 
                UnityEngine.Vector3.Lerp(startPosition.center, endPosition.center, rate);
             
            Vector2 size =
                (startPosition.size == endPosition.size)? startPosition.size:
                Vector2.Lerp(startPosition.size, endPosition.size, rate);
 
            Quaternion orientation = startPosition.orientation.CompareApproximately(endPosition.orientation) 
                    ? startPosition.orientation 
                    : Quaternion.Lerp(startPosition.orientation, endPosition.orientation, rate);
            
            
            return new Rectangle3D {center = center, size = size, orientation = orientation};
        }

        public bool Equals(Rectangle3D other) =>
            center.Equals(other.center) && 
            size.Equals(other.size) && 
            orientation.Equals(other.orientation);

        public override bool Equals(object obj) => obj is Rectangle3D other && Equals(other);

        public static bool operator == (Rectangle3D a, Rectangle3D b) => a.Equals(b);

        public static bool operator !=(Rectangle3D a, Rectangle3D b) => !a.Equals(b);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = center.GetHashCode();
                hashCode = (hashCode * 397) ^ size.GetHashCode();
                hashCode = (hashCode * 397) ^ orientation.GetHashCode();
                return hashCode;
            }
        }
    }
}
