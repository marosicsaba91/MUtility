using System;
using System.Collections.Generic;
using System.Linq;

namespace MUtility
{
    [Serializable]
    public struct CatmullRomSpine : IDrawable, IHandleable
    {
        const int defaultDrawingPointsOnSegment = 25;

        public List<UnityEngine.Vector3> points;
        
        public void Clear()
        {
            if (points == null)
                points = new List<UnityEngine.Vector3>();
            else
                points.Clear();
        }
        
        public CatmullRomSpine(bool isClosed, params UnityEngine.Vector3[] points)
        {
            this.points = new List<UnityEngine.Vector3>(points); 
            this.isClosed = isClosed;
        }

        public CatmullRomSpine(params UnityEngine.Vector3[] points )
        {
            this.points = new List<UnityEngine.Vector3>(points); 
            isClosed = false;
        }

        
        public bool isClosed;

        int SegmentCount => isClosed ? points.Count : points.Count - 1;

        public Drawable ToDrawable() => ToDrawable(defaultDrawingPointsOnSegment);

        public Drawable ToDrawable(int drawingPointsOnSegment) => ToDrawable(drawingPointsOnSegment, 0);

        public Drawable ToDrawable(int drawingPointsOnSegment, float derivativeSizeMultiplier)
        {
            if (points == null || points.Count <= 1)
                return new Drawable(new UnityEngine.Vector3[0]);

            var result1 = new List<UnityEngine.Vector3[]>();
            var result = new List<UnityEngine.Vector3>();

            int segmentCount = SegmentCount;

            for (var i = 0; i < segmentCount; i++)
            {
                GetControlPoints(i, out UnityEngine.Vector3 a, out UnityEngine.Vector3 b, out UnityEngine.Vector3 c, out UnityEngine.Vector3 d);

                UnityEngine.Vector3 position;
                UnityEngine.Vector3 derivative;
                if (i == 0) {
                    position = PositionOnASegment(a, b, c, d, 0);
                    result.Add(position);
                    if (derivativeSizeMultiplier > 0)
                    {
                        derivative = DerivativeOnASegment(a, b, c, d, 0);
                        result1.Add(new[] { position, position + derivative * derivativeSizeMultiplier });
                    }
                }

                for (var j = 1; j <= drawingPointsOnSegment; j++)
                {
                    float rate = (float)j / drawingPointsOnSegment;
                    position = PositionOnASegment(a, b, c, d, rate);
                    result.Add(position);
                    if (derivativeSizeMultiplier>0)
                    {
                        derivative = DerivativeOnASegment(a, b, c, d, rate);
                        result1.Add(new[] { position, position + derivative * derivativeSizeMultiplier });
                    }
                } 
            } 

            result1.Add(result.ToArray());
            return new Drawable(result1.ToArray());
        }

        public UnityEngine.Vector3 EvaluatePositionByNormalizedTime(float normalizedTime) => EvaluateByNormalizedTime(normalizedTime, PositionOnASegment);

        public UnityEngine.Vector3 EvaluateDerivativeByNormalizedTime(float normalizedTime) => EvaluateByNormalizedTime(normalizedTime, DerivativeOnASegment);

        public UnityEngine.Vector3 EvaluatePositionByTime(float normalizedTime) => EvaluateByTime(normalizedTime, PositionOnASegment);

        public UnityEngine.Vector3 EvaluateDerivativeByTime(float normalizedTime) => EvaluateByTime(normalizedTime, DerivativeOnASegment);

        UnityEngine.Vector3 EvaluateByNormalizedTime (float normalizedTime, Func<UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Vector3, float, UnityEngine.Vector3> segmentFunction)
        {
            if (points!=null && points.Count>0)
                return UnityEngine.Vector3.zero;
            if (points.Count == 1)
                return points[0];

            int segmentCount = SegmentCount;
            int segmentIndex;
            float inSegmentTime;
            if (normalizedTime >= 1)
            {
                inSegmentTime = 1;
                segmentIndex = SegmentCount-1;
            }
            else if (normalizedTime <= 0)
            {
                inSegmentTime = 0;
                segmentIndex = 0;
            }
            else
            { 
                segmentIndex = (int)(segmentCount * normalizedTime);
                inSegmentTime = normalizedTime % (1f / segmentCount) / (1f / segmentCount);
            } 

            GetControlPoints(segmentIndex, out UnityEngine.Vector3 a, out UnityEngine.Vector3 b,  out UnityEngine.Vector3 c, out UnityEngine.Vector3 d); 
            return segmentFunction(a, b, c, d, inSegmentTime);
        }
         
        UnityEngine.Vector3 EvaluateByTime(float time, Func<UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Vector3, float, UnityEngine.Vector3> segmentFunction)
        {
            if (points!=null && points.Count>0)
                return UnityEngine.Vector3.zero;
            if (points.Count == 1)
                return points[0]; 

            time = MathHelper.Mod(time, SegmentCount);
            var segmentIndex = (int)time; 
            GetControlPoints(segmentIndex, out UnityEngine.Vector3 previous, out UnityEngine.Vector3 start, out UnityEngine.Vector3 end, out UnityEngine.Vector3 next); 

            float inSegmentTime = time % 1f;
            return segmentFunction(previous, start,  end, next, inSegmentTime);
        }

        void GetControlPoints(int segmentIndex, out UnityEngine.Vector3 previous,  out UnityEngine.Vector3 start, out UnityEngine.Vector3 end, out UnityEngine.Vector3 next)
        {
            int segmentCount = SegmentCount;

            if (isClosed)
            {
                previous = points[MathHelper.Mod(segmentIndex - 1, segmentCount)];
                start = points[segmentIndex];
                end = points[(segmentIndex + 1) % segmentCount];
                next = points[(segmentIndex + 2) % segmentCount];
            }
            else
            {
                if (segmentIndex > 0)
                    previous = points[segmentIndex - 1];
                else
                    previous = points[segmentIndex] + (points[segmentIndex + 1] - points[segmentIndex]);

                start = points[segmentIndex];
                end = points[segmentIndex + 1];
                if (segmentIndex + 2 < points.Count)
                    next = points[segmentIndex + 2];
                else
                    next = points[segmentIndex + 1] + (points[segmentIndex] - points[segmentIndex + 1]);
            }
        }

        public List<HandlePoint> GetHandles()
        {
            var result = new List<HandlePoint>(points.Count);
            result.AddRange(points.Select(t => new HandlePoint(t, HandlePoint.Shape.Rectangle)));
            return result;
        }
         
        public void SetHandle(int index, UnityEngine.Vector3 point)
        {
            points[index ] = point;
        }

        public static UnityEngine.Vector3 PositionOnASegment(UnityEngine.Vector3 prev, UnityEngine.Vector3 start, UnityEngine.Vector3 end, UnityEngine.Vector3 next, float normalizedTime)
        {
            float progressSquared = normalizedTime * normalizedTime;
            float progressCubed = progressSquared * normalizedTime;

            UnityEngine.Vector3 result = prev * (-0.5f * progressCubed + progressSquared + -0.5f * normalizedTime);
            result += start * (1.5f * progressCubed + -2.5f * progressSquared + 1.0f);
            result += end * (-1.5f * progressCubed + 2.0f * progressSquared + 0.5f * normalizedTime);
            result += next * (0.5f * progressCubed + -0.5f * progressSquared);

            return result;
        }
        
        public static UnityEngine.Vector3 DerivativeOnASegment
            (UnityEngine.Vector3 previous, UnityEngine.Vector3 start, UnityEngine.Vector3 end, UnityEngine.Vector3 next, float normalizedTime)
        {
            float progressSquared = normalizedTime * normalizedTime;

            UnityEngine.Vector3 result = previous * (-1.5f * progressSquared + 2.0f * normalizedTime + -0.5f);
            result += start * (4.5f * progressSquared + -5.0f * normalizedTime);
            result += end * (-4.5f * progressSquared + 4.0f * normalizedTime + 0.5f);
            result += next * (1.5f * progressSquared - normalizedTime);

            return result;
        }
         
    }
}
