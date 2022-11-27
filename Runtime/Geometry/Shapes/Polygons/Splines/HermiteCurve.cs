using System;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct HermiteNode
{
    public enum EditingStyle
    {
        Free,
        Sharp,
        Smooth,
        Equal,
        Auto
    }

    public Vector3 position;
    public Vector3 speedIn;
    public Vector3 speedOut;
    public EditingStyle editingStyle;
}

[Serializable]
public class HermiteCurve : PathPolygon<HermiteNode>
{
    
    protected override int DefaultDrawingPointCountOnSegment => 25;

    public HermiteCurve() : base(Array.Empty<HermiteNode>()) { }
    public HermiteCurve(bool isClosed, params HermiteNode[] nodes) : base(isClosed, nodes) { }
    public HermiteCurve(params HermiteNode[] nodes) : base(nodes) { }
    
    protected override Vector3 GetControlPointPosition(HermiteNode controlPoint) => controlPoint.position;

    protected override HermiteNode VirtualControlPointBeforeFirst => controlPoints[0];
    protected override HermiteNode VirtualControlPointAfterLast => controlPoints[controlPoints.Count - 1];

    public override PathInterpolatedPoint InterpolateOnASegment
    (HermiteNode previous, HermiteNode a, HermiteNode b, HermiteNode next, float time01)
    {
        Vector3 p0 = a.position;
        Vector3 p1 = b.position;
        Vector3 v0 = a.speedOut;
        Vector3 v1 = b.speedIn;
        float ta = time01 * time01;
        float tb = time01 * ta;

        Vector3 position =
            (2 * tb - 3 * ta + 1) * p0 +
            (tb - 2 * ta + time01) * v0 +
            (-2 * tb + 3 * ta) * p1 +
            (tb - ta) * v1;
         
        Vector3 forward =Vector3.Lerp(v0, v1, time01);
        
        return new PathInterpolatedPoint(position, forward);
    }

    public override Pose ControlPointToPose(HermiteNode point) => new Pose(point.position, Quaternion.identity);

    public override HermiteNode PoseToControlPoint(Pose pose) => new HermiteNode(){position = pose.position};

    protected override HermiteNode DrawControlPointHandle(HermiteNode controlPoint)
    { 
        controlPoint.position = EasyHandles.PositionHandle(controlPoint.position);
        return controlPoint;
    }
}
}