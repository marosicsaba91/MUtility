using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
    [Serializable]
    public struct Ellipse : IDrawable, IHandleable, I2DArea, I2DCircumference
    {
        const int defaultFragmentCount = 30;

        public Vector2 center;
        public float radiusHorizontal;
        public float radiusVertical;

        public Ellipse(UnityEngine.Vector3 center, float radiusHorizontal, float radiusVertical)
        {
            this.center = center;
            this.radiusHorizontal = radiusHorizontal;
            this.radiusVertical = radiusVertical;
        }

        public float Circumference
        {
            get
            {
                var a = radiusHorizontal;
                var b = radiusVertical;
                var sqrt = Mathf.Sqrt((a * a + b * b) / 2f);
                var v = 2 * Mathf.PI * sqrt + Mathf.PI * (a + b);
                return v / 2;
            }
        }
        public float Area => radiusVertical * radiusHorizontal * Mathf.PI;

        public bool IsInsideShape(Vector2 point)
        {
            Vector2 vec = point - center;
            vec = new Vector2(vec.x / radiusHorizontal, vec.y / radiusVertical);
            return vec.magnitude <= 1;
        }

        public Drawable ToDrawable() => new Drawable (ToPolygon() );

        public Drawable ToDrawable(int fragmentCount) => new Drawable ( ToPolygon(fragmentCount) );

        public UnityEngine.Vector3[] ToPolygon(int fragmentCount = defaultFragmentCount)
        {
            var points = new UnityEngine.Vector3[fragmentCount];

            float angle = Mathf.PI * 2f / (fragmentCount - 1);
            for (var i = 0; i < fragmentCount - 1; i++)
            {
                float phase = i * angle;
                points[i] = Mathf.Sin(phase) * UnityEngine.Vector3.right + Mathf.Cos(phase) * UnityEngine.Vector3.up;
                points[i].x *= radiusHorizontal;
                points[i].y *= radiusVertical;
                points[i].x += center.x;
                points[i].y += center.y;
            }
            points[fragmentCount - 1] = points[0];

            return points;
        }


        public List<HandlePoint> GetHandles()
        {
            Vector2 ph = center + Vector2.right * radiusHorizontal;
            Vector2 pv = center + Vector2.up * radiusVertical;
            return new List<HandlePoint> {
                new HandlePoint(center, HandlePoint.Shape.Rectangle ),
                new HandlePoint(ph, HandlePoint.Shape.Circle),
                new HandlePoint(pv, HandlePoint.Shape.Circle)
            };
        }

        public void SetHandle(int i, UnityEngine.Vector3 newPoint)
        {
            switch (i)
            {
                case 0:
                    center = newPoint;
                    return;
                case 1:
                    radiusHorizontal = (newPoint - (UnityEngine.Vector3)center ).magnitude;
                    return;
                case 2:
                    radiusVertical = (newPoint - (UnityEngine.Vector3)center ).magnitude;
                    return;
            }
        }

        public Vector2 GetRandomPointInArea()
        {
            float a = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            float r = Mathf.Sqrt(UnityEngine.Random.Range(0f,1f));

            float x = r * Mathf.Sin(a) * radiusHorizontal + center.x;
            float y = r * Mathf.Cos(a) * radiusVertical + center.y;

            return new Vector2(x, y);
        }

        public bool IsPointInside(Vector2 point)
        {
            throw new NotImplementedException();
        }
    }
}
