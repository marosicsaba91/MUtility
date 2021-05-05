using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public struct Rectangle : IDrawable, IHandleable, I2DArea, I2DCircumference
    {
        public Vector2 center;
        public Vector2 size;

        public Rectangle(Vector2 center, Vector2 size)
        {
            this.center = center;
            this.size = size;
        }

        public float Area => size.x * size.y;

        public float Circumference => 2f * (size.x + size.y);

        public float Width => size.x;
        public float Height => size.y;

        public float XMin => center.x - (size.x / 2f);
        public float XMax => center.x + (size.x / 2f);
        public float YMin => center.y - (size.y / 2f);
        public float YMax => center.y + (size.y / 2f);

        public Vector2 Right =>         new Vector2(XMax, center.y);
        public Vector2 Left =>          new Vector2(XMin, center.y);
        public Vector2 Top =>           new Vector2(center.x, YMax);
        public Vector2 Bottom =>        new Vector2(center.x, YMin);

        public Vector2 TopRight =>      new Vector2(XMax, YMax);
        public Vector2 TopLeft =>       new Vector2(XMin, YMax);
        public Vector2 BottomRight =>   new Vector2(XMax, YMin);
        public Vector2 BottomLeft =>    new Vector2(XMin, YMin);

        public List<HandlePoint> GetHandles()
        { 

            return new List<HandlePoint> {
                new HandlePoint(center, HandlePoint.Shape.Rectangle),        // 0 Center
                new HandlePoint(Right, HandlePoint.Shape.Circle),            // 1 Right
                new HandlePoint(Left, HandlePoint.Shape.Circle),             // 2 Left
                new HandlePoint(Top, HandlePoint.Shape.Circle),              // 3 Top
                new HandlePoint(Bottom, HandlePoint.Shape.Circle),           // 4 Bottom
                new HandlePoint(TopRight, HandlePoint.Shape.Rectangle),      // 5 Rescale
            };
        }

        public Vector2 GetRandomPointInArea() => new Vector2(
            UnityEngine.Random.Range(size.y / -2f, size.x / 2f) + center.x,
            UnityEngine.Random.Range(size.y / -2f, size.y / 2f) + center.y);
        

        public bool IsPointInside(Vector2 point) =>
            Mathf.Abs(center.x - point.x) < (size.x / 2f) &&
            Mathf.Abs(center.y - point.y) < (size.y / 2f);

        public void SetHandle(int i, Vector3 newPoint)
        {
            float rescale = 0;
            float offset = 0;

            switch (i)
            {
                case 0: // Center
                    center = newPoint;
                    return;
                case 1: // Right
                    rescale = ((newPoint.x - center.x) - (size.x / 2f));
                    offset = (newPoint.x - (center.x + (size.x / 2))) / 2f;
                    break;
                case 2: // Left
                    rescale = ((center.x - newPoint.x) - (size.x / 2f));
                    offset = (newPoint.x - (center.x - (size.x / 2))) / 2f;
                    break;
                case 3: // Top
                    rescale = ((newPoint.y - center.y) - (size.y / 2f));
                    offset = (newPoint.y - (center.y + (size.y / 2))) / 2f;
                    break;
                case 4: // Bottom
                    rescale = ((center.y - newPoint.y) - (size.y / 2f));
                    offset = (newPoint.y - (center.y - (size.y / 2))) / 2f;
                    break;
                case 5: // Rescale
                    float rescaleX = (newPoint.x - center.x) * 2;
                    float rescaleY = (newPoint.y - center.y) * 2;
                    size = new Vector2(rescaleX, rescaleY);
                    return;
            }

            if (i == 1 || i == 2) // Horizontal
            {
                size = new Vector2(size.x + rescale, size.y);
                center = new Vector2(center.x + offset, center.y);
            }
            if (i == 3 || i == 4) // Vertical
            {
                size = new Vector2(size.x, size.y + rescale);
                center = new Vector2(center.x, center.y + offset);
            }
        }

        internal Rect ToRect() => new Rect(center - (size / 2f), size);        

        public Drawable ToDrawable() =>
            new Drawable(new Vector3[] { TopRight, TopLeft, BottomLeft, BottomRight, TopRight });

        static readonly Vector2Int defaultMeshGridSize = new Vector2Int(10,10);
    }
}
