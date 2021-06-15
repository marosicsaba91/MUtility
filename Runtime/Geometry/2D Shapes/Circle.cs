using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
    [Serializable]
    public struct Circle : I2DArea, I2DCircumference, IHandleable, IDrawable
    {
        public const int defaultSegmentCount = 20;

        public Vector2 center;
        public float radius;

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
        
        public Circle(float radius)
        {
            center = Vector2.zero;
            this.radius = radius;
        }

        public Vector2 GetRandomPoint()
        {
            float angle = UnityEngine.Random.Range(0f, 2*Mathf.PI);
            float rad = UnityEngine.Random.Range(0, radius);

            return center + (rad * GeometryHelper.RadianToVector2D(angle));
        }

        public float Diameter
        {
            get => 2f * radius;
            set => radius = value / 2f;
        }

        public float Area => radius * radius * Mathf.PI; 

        public float Circumference => 2f * radius * Mathf.PI; 

        public bool IsPointInside(Vector2 point) => (point - center).magnitude <= radius;

        public float Distance(Ray ray)
        {
            float centerLineDistance = GeometryHelper.DistanceBetweenPointAndLine(center, new Line(ray.origin, (ray.origin + ray.direction)));
            if (centerLineDistance <= radius)
                return 0;
            return centerLineDistance - radius;
        }

        public Drawable ToDrawable() => new Drawable ( ToPolygon() );

        public Drawable ToDrawable(int fragmentCount, bool dashedLine = false)
        {
            if (dashedLine)
                return ToDrawable_Dashed(fragmentCount);
            return new Drawable(ToPolygon(fragmentCount));
        }
         
        public Drawable ToDrawable_Dashed( int fregmentCount = defaultSegmentCount)
        {
            fregmentCount /= 2;
            var right = new Vector2(radius, 0); ;
            var up = new Vector2(0, radius);

            var polygons = new Vector3[fregmentCount][];

            float angle = Mathf.PI * 2f / (fregmentCount*2);
            for (var i = 0; i < polygons.Length; i++)
            {
                float phase1 = angle * 2 * i;
                float phase2 = angle * ((2 * i) + 1);
                Vector2 p1 = (Mathf.Sin(phase1) * right) + (Mathf.Cos(phase1) * up) + center;
                Vector2 p2 = (Mathf.Sin(phase2) * right) + (Mathf.Cos(phase2) * up) + center;
                polygons[i] = new Vector3[] { p1, p2 };
            }
            return new Drawable(polygons);
        }

        public Vector3[] ToPolygon(int segmentCount = defaultSegmentCount)
        {
            Vector3 right = new Vector2(radius, 0); ;
            Vector3 up = new Vector2(0, radius);

            var points = new Vector3[segmentCount + 1];

            float angle = Mathf.PI * 2f / segmentCount;
            for (var i = 0; i < segmentCount; i++)
            {
                float phase = angle * i;
                points[i] = (Mathf.Sin(phase) * right) + (Mathf.Cos(phase) * up);
                points[i] += (Vector3)center;
            }
            points[points.Length - 1] = points[0];

            return points;
        }

        public List<HandlePoint> GetHandles()
        { 
            return new List<HandlePoint> {
                new HandlePoint(center,  HandlePoint.Shape.Rectangle),
                new HandlePoint(center + (Vector2.right * radius), HandlePoint.Shape.Circle)
            };
        }

        public void SetHandle(int i, Vector3 newPoint)
        { 
            if (i == 0) 
                center = newPoint; 
            else 
                radius = newPoint.x - center.x; 
        }

        public Vector2 GetRandomPointInArea()
        {
            float a = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, radius));

            float x = (r * Mathf.Sin(a)) + center.x;
            float y = (r * Mathf.Cos(a)) + center.y;

            return new Vector2(x, y);
        }
    }

}
