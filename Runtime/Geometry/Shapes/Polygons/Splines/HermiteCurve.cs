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

public class HermiteCurve : PathCurve<HermiteNode>
{
    protected override int DefaultDrawingPointsOnSegment => 25;

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


    /*
    public Drawable ToDrawable() => ToDrawable(DefaultDrawingPointsOnSegment);

    public Drawable ToDrawable(int drawingPointsOnSegment) => new Drawable(new List<Vector3[]>
    {
        ToVectorArray(drawingPointsOnSegment)
    });

    public Vector3[] ToVectorArray() => ToVectorArray(DefaultDrawingPointsOnSegment);

    public Vector3[] ToVectorArray(int drawingPointsOnSegment)
    {
        if (controlPoints == null) return default;
        int nodeCount = controlPoints.Count;
        if (nodeCount == 0) return default;

        int curvePointCount = drawingPointsOnSegment * SegmentCount + 1;
        var points = new Vector3[curvePointCount];
        for (int i = 0; i < curvePointCount; i++)
        {
            float t = (float) i / drawingPointsOnSegment;
            points[i] = Evaluate(t);
        }

        return points;
    }
    */
}
}