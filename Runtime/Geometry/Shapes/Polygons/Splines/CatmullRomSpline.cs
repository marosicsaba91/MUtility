using System;

using UnityEngine;


namespace MUtility
{
    [Serializable]
    public class CatmullRomSpline : PathPolygon<Vector3>
    {
        protected override int DefaultDrawingPointCountOnSegment => 20;
        
        public CatmullRomSpline(bool isClosed, params Vector3[] points) : base(isClosed, points) { }
        public CatmullRomSpline(params Vector3[] points ) : base(points) { }
        public CatmullRomSpline() : base(Array.Empty<Vector3>()) { }

        protected override Vector3 GetControlPointPosition(Vector3 controlPoint) => controlPoint;

        protected override Vector3 VirtualControlPointBeforeFirst => controlPoints[0] + (controlPoints[0 + 1] - controlPoints[0]);
        
        protected override Vector3 VirtualControlPointAfterLast
        {
            get
            {
                int count = controlPoints.Count;
                Vector3 last = controlPoints[count - 1];
                Vector3 beforeLast = controlPoints[count - 2];
                return last + (beforeLast - last);
            }
        }

        public override PathInterpolatedPoint InterpolateOnASegment(Vector3 previous, Vector3 a, Vector3 b, Vector3 next, float time01)
        {
            float progressSquared = time01 * time01;
            float progressCubed = progressSquared * time01;

            Vector3 position = previous * (-0.5f * progressCubed + progressSquared + -0.5f * time01);
            position += a * (1.5f * progressCubed + -2.5f * progressSquared + 1.0f);
            position += b * (-1.5f * progressCubed + 2.0f * progressSquared + 0.5f * time01);
            position += next * (0.5f * progressCubed + -0.5f * progressSquared);
            
            
            Vector3 derivative = previous * (-1.5f * progressSquared + 2.0f * time01 + -0.5f);
            derivative += a * (4.5f * progressSquared + -5.0f * time01);
            derivative += b * (-4.5f * progressSquared + 4.0f * time01 + 0.5f);
            derivative += next * (1.5f * progressSquared - time01);
            
            return new PathInterpolatedPoint(position, derivative);
        }

        public override Pose ControlPointToPose(Vector3 point) => new Pose(point, Quaternion.identity);

        public override Vector3 PoseToControlPoint(Pose pose) => pose.position;
    }
}
