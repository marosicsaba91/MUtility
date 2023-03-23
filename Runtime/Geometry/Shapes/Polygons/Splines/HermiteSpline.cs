using System;
using UnityEngine;

namespace MUtility
{

[Serializable]
public struct ControlPoint
{
    public enum Type
    {
        Mirror,  // C1 G1 (Use SpeedIn only)
        Smooth,  // C0 G1 (Use SpeedIn & SpeedOut length)
        Break,   // C0 G0 (Use SpeedIn & SpeedOut)
        Corner,  // C0 G0 (NO SpeedIn & SpeedOut)
    }
    
    public Vector3 position;
    
    public Vector3 speedIn;
    public Vector3 speedOut;
    public Type type;
    
}

[Serializable]
public class HermiteSpline : CubicSpline<ControlPoint>
{
    public HermiteSpline() : base(Array.Empty<ControlPoint>()) { }
    public HermiteSpline(bool isClosed, params ControlPoint[] nodes) : base(isClosed, nodes) { }
    public HermiteSpline(params ControlPoint[] nodes) : base(nodes) { }
    
    protected override Vector3 ControlPointToPosition(ControlPoint controlPoint) => controlPoint.position;

    protected override Ray EvaluateSafe(float continuousIndex, int previousIndex, int index, int nextIndex, int next2Index)
    {
        ControlPoint a = controlPoints[index];
        ControlPoint b = controlPoints[nextIndex]; 
        return EvaluateOnSegment(a,  b, continuousIndex);
    }
    
    public Ray EvaluateOnSegment(ControlPoint a, ControlPoint b, float controlPointIndex)
    {
        EvaluateOnSegment(a, b, controlPointIndex, out Vector3 position, out Vector3 direction);
        return new Ray( position, direction);
    }
    
    public void EvaluateOnSegment(ControlPoint a, ControlPoint b, float controlPointIndex, out Vector3 position, out Vector3 direction)
    {
        float t = controlPointIndex % 1;
        float t2 = t * t;
        float t3 = t * t2;
        
        Vector3 p0 = a.position;
        Vector3 p1 = b.position;
        Vector3 v0 = a.speedOut;
        Vector3 v1 = b.speedIn;

        position =
            (2 * t3 - 3 * t2 + 1) * p0 +
            (t3 - 2 * t2 + t) * v0 +
            (-2 * t3 + 3 * t2) * p1 +
            (t3 - t2) * v1;
         
        direction = Vector3.Lerp(v0, v1, t);
    }
    
    public override Pose ControlPointToPose(ControlPoint point) => new Pose(point.position, Quaternion.identity);

    public override ControlPoint PoseToControlPoint(Pose pose) => new ControlPoint(){position = pose.position};

    protected override ControlPoint DrawControlPointHandle(ControlPoint controlPoint)
    { 
        controlPoint.position = EasyHandles.PositionHandle(controlPoint.position);
        return controlPoint;
    }
}
}