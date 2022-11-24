using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUtility
{
[Serializable]
public abstract class PathCurve
{
    public abstract bool IsLoop { get; }
    protected abstract int DefaultDrawingPointsOnSegment { get; }
    public abstract IEnumerable<PathInterpolatedPoint> EvaluateFull(int drawingPointsOnSegment); 
    
    public IEnumerable<PathInterpolatedPoint> GetInterpolatedPoints()
        => EvaluateFull(DefaultDrawingPointsOnSegment);
}

[Serializable]
public abstract class PathCurve<TControlPoint> : PathCurve, IDrawable
{
    [SerializeField] protected bool isLoop = false;
    [SerializeField] protected readonly List<TControlPoint> controlPoints = new List<TControlPoint>();

    // ------------------- Constructors -------------------

    protected PathCurve(bool isLoop, params TControlPoint[] controlPoint)
    {
        controlPoints = new List<TControlPoint>(controlPoint);
        this.isLoop = isLoop;
    }

    protected PathCurve(params TControlPoint[] nodes) : this(false, nodes) { }
    
    // ------------------- Methods -------------------
    
    public override bool IsLoop => isLoop;
    public void Clear() => controlPoints.Clear();
    public int SegmentCount => isLoop ? controlPoints.Count : controlPoints.Count - 1;
    
    
    // ------------------------- Abstract Methods -------------------------

    protected abstract Vector3 GetControlPointPosition(TControlPoint controlPoint);
    protected abstract TControlPoint VirtualControlPointBeforeFirst { get; }
    protected abstract TControlPoint VirtualControlPointAfterLast { get; }
    
    public abstract PathInterpolatedPoint InterpolateOnASegment
        (TControlPoint previous, TControlPoint a, TControlPoint b, TControlPoint next, float time01);

    // ------------------------- Interpolation -------------------------
    
    public PathInterpolatedPoint Evaluate(float controlPointIndex)
    {
        if (controlPoints!=null && controlPoints.Count > 0)
            return PathInterpolatedPoint.zero;
        if (controlPoints.Count == 1)
            return new PathInterpolatedPoint(GetControlPointPosition(controlPoints[0])); 

        controlPointIndex = isLoop
            ? MathHelper.Mod(controlPointIndex, SegmentCount)
            : Mathf.Clamp(controlPointIndex, 0, SegmentCount);
        int segmentIndex = (int)controlPointIndex; 
        GetControlPoints(segmentIndex, out TControlPoint previous, out TControlPoint start, out TControlPoint end, out TControlPoint next); 

        float inSegmentTime = controlPointIndex % 1f;
        return InterpolateOnASegment(previous, start,  end, next, inSegmentTime);
    }
    
    public PathInterpolatedPoint Evaluate01(float time01) => Evaluate(time01 * SegmentCount);

    public void GetControlPoints
        (int segmentIndex, out TControlPoint previous, out TControlPoint a, out TControlPoint b, out TControlPoint next)
    {
        int segmentCount = SegmentCount;

        if (isLoop)
        {
            previous = controlPoints[MathHelper.Mod(segmentIndex - 1, segmentCount)];
            a = controlPoints[segmentIndex];
            b = controlPoints[(segmentIndex + 1) % segmentCount];
            next = controlPoints[(segmentIndex + 2) % segmentCount];
        }
        else
        {
            previous = segmentIndex > 0 ? controlPoints[segmentIndex - 1] : VirtualControlPointBeforeFirst;
            a = controlPoints[segmentIndex];
            b = controlPoints[segmentIndex + 1];
            next = segmentIndex + 1 <= controlPoints.Count ? controlPoints[segmentIndex + 2] : VirtualControlPointAfterLast;
        }
    }

    public override IEnumerable<PathInterpolatedPoint> EvaluateFull(int drawingPointsOnSegment)
    {
        if (controlPoints == null || controlPoints.Count <= 1)
            yield break;

        int segmentCount = SegmentCount;

        for (int i = 0; i < segmentCount; i++)
        {
            GetControlPoints(i, out TControlPoint previous, out TControlPoint a, out TControlPoint b,
                out TControlPoint next);

            for (int j = 0; j < drawingPointsOnSegment; j++)
            {
                float rate = (float)j / drawingPointsOnSegment - 1;
                yield return InterpolateOnASegment(previous, a, b, next, rate);
            }
        }
    }


    // -------------------------- IDrawable --------------------------

    public Drawable ToDrawable() => ToDrawable(DefaultDrawingPointsOnSegment, 0);
    
    public Drawable ToDrawable(int drawingPointsOnSegment) => ToDrawable(drawingPointsOnSegment, 0);

    public Drawable ToDrawable(int drawingPointsOnSegment, float forwardSizeMultiplier)
    {
        if (controlPoints == null || controlPoints.Count <= 1)
            return new Drawable(Array.Empty<Vector3>());

        var forwards = new List<Vector3[]>();
        var spline = new List<Vector3>(); 

        foreach (PathInterpolatedPoint point in EvaluateFull(drawingPointsOnSegment))
        {
            Vector3 position = point.position;
            spline.Add(position);
            if (forwardSizeMultiplier > 0)
            {
                Vector3 forward = point.forward * forwardSizeMultiplier;
                forwards.Add(new[] { position, position + forward * forwardSizeMultiplier });
            }
        }

        forwards.Add(spline.ToArray());
        return new Drawable(forwards.ToArray());
    } 
    
    // -------------------------- Handles --------------------------
    
    IEnumerable<Pose> GetHandles() => controlPoints.Select(ControlPointToPose);

    void SetHandle(int index, Vector3 position)
    {
        controlPoints[index] = PoseToControlPoint(new Pose(position, Quaternion.identity));
    }
    
    public abstract Pose ControlPointToPose(TControlPoint point);
    public abstract TControlPoint PoseToControlPoint(Pose pose);
}

public struct PathInterpolatedPoint
{
    public Vector3 position;
    public Vector3 forward;
    public Vector3 up;
    
    public PathInterpolatedPoint(Vector3 position, Vector3 forward, Vector3 up)
    {
        this.position = position;
        this.forward = forward;
        this.up = up;
    }
    
    public PathInterpolatedPoint(Vector3 position, Vector3 forward)
    {
        this.position = position;
        this.forward = forward;
        VectorExtensions.LeftHand(forward, out up, Vector3.forward);
    }
    
    public PathInterpolatedPoint(Vector3 position)
    {
        this.position = position;
        forward = Vector3.forward;
        up = Vector3.up;
    }
    
    public static readonly PathInterpolatedPoint zero = new PathInterpolatedPoint(Vector3.zero);
}
}