using System;
using UnityEngine;

namespace MUtility
{
    [Serializable]
    public class CustomPolygon : PathPolygon<Vector3>
    {
        public CustomPolygon() : base(Array.Empty<Vector3>()) { }
        protected override int DefaultDrawingPointCountOnSegment => 1;

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
            for (index = 0; index < n; ++index) {
                int nextIndex = (index + 1) % n;
                Vector2 point = controlPoints[index];
                Vector2 next = controlPoints[nextIndex];
                signedArea += point.x * next.y - next.x * point.y;
            }
            return signedArea;
        }
        
        protected override Vector3 GetControlPointPosition(Vector3 controlPoint) => controlPoint;

        
        protected override Vector3 VirtualControlPointBeforeFirst => controlPoints[0];
        protected override Vector3 VirtualControlPointAfterLast  => controlPoints[controlPoints.Count - 1];
        public override PathInterpolatedPoint InterpolateOnASegment(Vector3 previous, Vector3 a, Vector3 b, Vector3 next, float time01)
        {
            Vector3 p = Vector3.Lerp(a, b, time01);
            Vector3 forward = (b - a).normalized;
            return new PathInterpolatedPoint(p, forward);
        }
        
        public override Pose ControlPointToPose(Vector3 point) => new Pose(point, Quaternion.identity);

        public override Vector3 PoseToControlPoint(Pose pose) => pose.position;
    }
}