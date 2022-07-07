using System;
using System.Collections.Generic;

namespace MUtility
{
[Serializable]
public struct LineSegment : IDrawable, IHandleable
{
    public UnityEngine.Vector3 a;
    public UnityEngine.Vector3 b;

    public LineSegment(UnityEngine.Vector3 a, UnityEngine.Vector3 b)
    {
        this.a = a;
        this.b = b;
    }

    public float Magnitude => (b - a).magnitude;

    public float SqrMagnitude => (b - a).sqrMagnitude;

    UnityEngine.Vector3 LerpOnLineUnclamped(float value) => UnityEngine.Vector3.LerpUnclamped(a, b, value);

    UnityEngine.Vector3 LerpOnLine(float value) => UnityEngine.Vector3.Lerp(a, b, value);

    public Drawable ToDrawable() => new Drawable(new[] {a, b});

    public List<HandlePoint> GetHandles() =>
        new List<HandlePoint> {new HandlePoint(a), new HandlePoint(b)};

    public UnityEngine.Vector3 ClosestPointOnSegmentToPoint(UnityEngine.Vector3 point) =>
        UnityEngine.Vector3.Lerp(a, b, Line.ClosestPointOnLineToPointRate(a, b, point));

    public UnityEngine.Vector3 ClosestPointOnLineToPoint(UnityEngine.Vector3 point) =>
        UnityEngine.Vector3.LerpUnclamped(a, b, Line.ClosestPointOnLineToPointRate(a, b, point));

    public bool TryGetShortestSegmentToLine(Line l2, out LineSegment shortest) =>
        Line.TryGetShortestSegmentBetweenLines(
            new Line(a, b),
            new Line(l2.a, l2.b),
            l1AClosed: true, 
            l1BClosed: true, 
            l2AClosed: false, 
            l2BClosed: false,
            out shortest);

    public bool TryGetShortestSegmentToSegment(LineSegment s2, out LineSegment shortest) =>
        Line.TryGetShortestSegmentBetweenLines(
            new Line(a, b),
            new Line(s2.a, s2.b),
            l1AClosed: true, 
            l1BClosed: true, 
            l2AClosed: true, 
            l2BClosed: true,
            out shortest);
    
    public void SetHandle(int index, UnityEngine.Vector3 point)
    {
        if (index == 0)
            a = point;
        else
            b = point;
    }
}
}
