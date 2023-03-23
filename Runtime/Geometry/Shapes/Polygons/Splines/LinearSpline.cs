using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
    [Serializable]
    public class LinearSpline : Spline<Vector3>
    {
        public LinearSpline() : base(Array.Empty<Vector3>())
        {
        }

        public float Area => Mathf.Abs(SignedDoubleArea() * 0.5f);

        public float Length
        {
            get
            {
                int index;
                int n = controlPoints.Count;
                float district = 0;
                for (index = 0; index < n; ++index)
                {
                    int nextIndex = (index + 1) % n;
                    Vector2 point = controlPoints[index];
                    Vector2 next = controlPoints[nextIndex];
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
            int index;
            int n = controlPoints.Count;
            float signedArea = 0;
            for (index = 0; index < n; ++index)
            {
                int nextIndex = (index + 1) % n;
                Vector2 point = controlPoints[index];
                Vector2 next = controlPoints[nextIndex];
                signedArea += point.x * next.y - next.x * point.y;
            }

            return signedArea;
        }

        protected override Ray EvaluateSafe(float continuousIndex, int previousIndex, int index, int nextIndex,
            int next2Index)
        {
            return
                new Ray(); // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 
        }

        protected override Vector3 ControlPointToPosition(Vector3 controlPoint) => controlPoint;

        public override Pose ControlPointToPose(Vector3 point) => new(point, Quaternion.identity);

        public override Vector3 PoseToControlPoint(Pose pose) => pose.position;

        protected override void SafeRecalculatePoints(List<InterpolatedPoint> result)
        {
            float totalLength = Length;

            for (var i = 0; i < controlPoints.Count - 1; i++)
            {
                Vector3 point = controlPoints[i];
                Vector3 next = controlPoints[i + 1];
                Vector3 forward = next - point;

                result.Add(new InterpolatedPoint(i, point, forward, totalLength));
                totalLength += forward.magnitude;
            }

            if (isLoop)
            {
                Vector3 point = controlPoints[^1];
                Vector3 first = controlPoints[0];
                Vector3 forward = first - point;
                result.Add(new InterpolatedPoint(controlPoints.Count - 1, point, forward, totalLength));
                totalLength += forward.magnitude;

                Vector3 next = controlPoints[1];
                forward = next - first;
                result.Add(new InterpolatedPoint(controlPoints.Count, first, forward, totalLength));
            }
            else
            {
                Vector3 point = controlPoints[^1];
                Vector3 forward = controlPoints[^1] - controlPoints[^2];
                result.Add(new InterpolatedPoint(controlPoints.Count - 1, point, forward, totalLength));
            }
        }
        
    }
}