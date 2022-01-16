using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MUtility
{
    [Serializable]
    public struct Polygon : I2DArea, IHandleable, I2DCircumference, IDrawable
    {
        [FormerlySerializedAs("verticles")]
        public List<UnityEngine.Vector3> points;
        public bool closed;

        int SegmentCount => closed ? points.Count : points.Count - 1;

        public float Area => Mathf.Abs(SignedDoubleArea() * 0.5f);

        public void Clear()
        {
            if (points == null)
                points = new List<UnityEngine.Vector3>();
            else
                points.Clear();
        }

        public float Circumference
        {
            get
            {
                int index, nextIndex;
                int n = points.Count;
                Vector2 point, next;
                float district = 0;
                for (index = 0; index < n; ++index)
                {
                    nextIndex = (index + 1) % n;
                    point = points[index];
                    next = points[nextIndex];
                    district += (point - next).magnitude;
                }
                return district;
            }
        }

        public Winding? GetWinding()
        {
            float signedDoubleArea = SignedDoubleArea();
            if (signedDoubleArea < 0)
            {
                return Winding.Clockwise;
            }
            if (signedDoubleArea > 0)
            {
                return Winding.CounterClockwise;
            }
            return null;
        }

        float SignedDoubleArea()
        {
            int index, nextIndex;
            int n = points.Count;
            Vector2 point, next;
            float signedArea = 0;
            for (index = 0; index < n; ++index) {
                nextIndex = (index + 1) % n;
                point = points[index];
                next = points[nextIndex];
                signedArea += point.x * next.y - next.x * point.y;
            }
            return signedArea;
        }

        public Drawable ToDrawable()
        {
            if (points == null || points.Count <= 1)
                return new Drawable(new UnityEngine.Vector3 [0][]);
             
            var polygon = new UnityEngine.Vector3[SegmentCount+1];

            for (var i = 0; i < points.Count; i++)            
                polygon[i] = points[i];
            
            if (closed)
                polygon[polygon.Length-1] = polygon[0];

            return new Drawable(polygon);
        }

        public List<HandlePoint> GetHandles()
        {
            var result = new List<HandlePoint>(points.Count);
            for (var i = 0; i < points.Count; i++)
            {
                result.Add(new HandlePoint(points[i], HandlePoint.Shape.Rectangle));
            }
            return result;
        }

        public void SetHandle(int index, UnityEngine.Vector3 point)
        {
            points[index] = point;
        }

        public Vector2 GetRandomPointInArea()
        {
            throw new NotImplementedException();
        }

        public bool IsPointInside(Vector2 point)
        {
            throw new NotImplementedException();
        }
    }
}