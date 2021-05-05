using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
    [Serializable]
    public struct CircleSector : IDrawable, IHandleable, I2DArea, I2DCircumference
    {
        const int defaultFragmentCount = 75;
        
        public Vector2 center;
        public float radius;
        public float startAngleDeg;
        [SerializeField, Range(0,1)] float fullRate;
        public Winding winding;

        public CircleSector(Vector2 center, float radius, float startAngleDeg, float fullRate, Winding winding)
        {
            this.center = center;
            this.radius = radius;
            this.startAngleDeg = startAngleDeg;
            this.fullRate = Mathf.Clamp01(fullRate) ;
            this.winding = winding;
        }
        
        public CircleSector(Vector2 center, float radius, float startAngleDeg, float endAngleDeg)
        {
            this.center = center;
            this.radius = radius;
            this.startAngleDeg = startAngleDeg;
            fullRate = (endAngleDeg - startAngleDeg) / 360;

            winding = fullRate < 0 ? Winding.Clockwise : Winding.CounterClockwise;
            fullRate = Mathf.Clamp01(Mathf.Abs((endAngleDeg - startAngleDeg) / 360)) ;
        }
        
        public float FullRate {
            get => fullRate;
            set => fullRate = Mathf.Clamp01(value);
        }

        public float DeltaAngleDeg
        {
            get => (winding == Winding.Clockwise ? -1 : 1) * FullRate * 360f;
            set => FullRate = (winding == Winding.Clockwise ? -1 : 1) * value / 360f;
        }

        public float Circumference => 2 * radius * Mathf.PI * FullRate;

        public float Area => radius * radius * Mathf.PI * FullRate;

        public float EndAngleDeg {
            get
            {
                float angle = startAngleDeg;
                angle += winding == Winding.Clockwise ? -DeltaAngleDeg : DeltaAngleDeg;
                return GeometryHelper.SimplifyAngle(angle);
            }
        }

        public Drawable ToDrawable() => new Drawable ( ToPolygon() );
        
        public Drawable ToDrawable(int fullCircleFragmentCount, float width = 0) =>
            new Drawable ( ToPolygon(fullCircleFragmentCount, width));
        
        public Vector3[] ToPolygon(int fullCircleFragmentCount = defaultFragmentCount, float width = 0)
        {
            if (fullRate >= 1)
                return new Circle(center, radius).ToPolygon(fullCircleFragmentCount);
            
            if (fullRate <= 0)
            {
                Vector2 b = center + (GeometryHelper.RadianToVector2D(startAngleDeg * Mathf.Deg2Rad) * radius);
                return new LineSegment(center, b).ToDrawable().polygons[0];
            }
            

            float segmentLength = DeltaAngleDeg;
 
            float startAngleInRad = (-startAngleDeg + 90) * Mathf.Deg2Rad;
            int circleFragmentCount = Mathf.Max(2, Mathf.CeilToInt((fullCircleFragmentCount + 1) * (Mathf.Abs(segmentLength) / 360)));

            segmentLength *= Mathf.Deg2Rad;

            var points = new Vector3[
                width >= radius ? circleFragmentCount + 2 :
                width > 0 ? (circleFragmentCount * 2 + 1) :
                circleFragmentCount];

            Vector3 right = new Vector2(radius, 0);
            Vector3 up = new Vector2(0, radius);

            var pointIndex = 0;
            AddSegmentPoints( center, true);

            if (width >= radius)
            {
                points[pointIndex++] = center;
                points[pointIndex] = points[0];
            }
            else if (width > 0)
            {
                right = new Vector2(radius-width, 0);
                up = new Vector2(0, radius-width);

                AddSegmentPoints(center, false);
                points[pointIndex] = points[0];
            } 

            return points;
            
            void AddSegmentPoints(Vector3 center, bool dir)
            {   
                for (var i = 0; i < circleFragmentCount; i++)
                {
                    float rate = (float) i / (circleFragmentCount - 1);
                    if (!dir) rate = 1 - rate;
                    float phase = startAngleInRad - (rate * segmentLength);
                    points[pointIndex] = (Mathf.Sin(phase) * right) + (Mathf.Cos(phase) * up) + center;
                    pointIndex++;
                }
            }
        }


        public List<HandlePoint> GetHandles()
        {
            return new List<HandlePoint> {
                new HandlePoint(center,  HandlePoint.Shape.Rectangle),
                new HandlePoint(center + (GeometryHelper.RadianToVector2D(startAngleDeg* Mathf.Deg2Rad) * radius))
            };
        }

        public void SetHandle(int i, Vector3 newPoint)
        {
            if (i == 0)
            {
                center = newPoint;
            }
            else if (i == 1)
            {
                Vector2 dir = (Vector2)newPoint - center;
                radius = dir.magnitude;
                startAngleDeg = GeometryHelper.Vector2DToRadian(dir) * Mathf.Rad2Deg;
            }
        }

        public Vector2 GetRandomPointInArea()
        {
            float a = UnityEngine.Random.Range(0, 2 * Mathf.PI); // TODO: Just On Segment
            float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, radius));

            float x = (r * Mathf.Sin(a)) + center.x;
            float y = (r * Mathf.Cos(a)) + center.y;

            return new Vector2(x, y);
        }

        public bool IsPointInside(Vector2 point)
        {
            throw new NotImplementedException();
        }
    }
}