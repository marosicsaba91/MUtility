using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct CircleSector : IShape2D //, IHandleable
{
    const int defaultFragmentCount = 75;

    public float radius;
    public float startAngleDeg;
    [SerializeField, Range(0, 1)] float fullRate;
    public Winding winding;
    public float innerRadius;

    public CircleSector(float radius, float startAngleDeg, float fullRate, Winding winding, float innerRadius = 0)
    {
        this.radius = radius;
        this.startAngleDeg = startAngleDeg;
        this.fullRate = Mathf.Clamp01(fullRate);
        this.winding = winding;
        this.innerRadius = innerRadius;
    }

    public CircleSector(float radius, float startAngleDeg, float endAngleDeg, float innerRadius = 0)
    {
        this.radius = radius;
        this.startAngleDeg = startAngleDeg;
        fullRate = (endAngleDeg - startAngleDeg) / 360;

        winding = fullRate < 0 ? Winding.Clockwise : Winding.CounterClockwise;
        fullRate = Mathf.Clamp01(Mathf.Abs((endAngleDeg - startAngleDeg) / 360));
        this.innerRadius = innerRadius;
    }

    public float FullRate
    {
        get => fullRate;
        set => fullRate = Mathf.Clamp01(value);
    }

    public float DeltaAngleDeg
    {
        get => (winding == Winding.Clockwise ? -1 : 1) * FullRate * 360f;
        set => FullRate = (winding == Winding.Clockwise ? -1 : 1) * value / 360f;
    }

    public float Circumference => 2 * ((radius + innerRadius) * Mathf.PI * FullRate + (radius - innerRadius));

    public float Area => (radius * radius * Mathf.PI) - (innerRadius * innerRadius * Mathf.PI) * FullRate;

    public float EndAngleDeg
    {
        get
        {
            float angle = startAngleDeg;
            angle += winding == Winding.Clockwise ? -DeltaAngleDeg : DeltaAngleDeg;
            return GeometryHelper.SimplifyAngle(angle);
        }
    }

    public IEnumerable<Vector3> Points => ToPolygon();

    public IEnumerable<Vector3> ToPolygon(int fullCircleFragmentCount = defaultFragmentCount)
    {
        if (fullRate >= 1)
            return new Circle(radius).ToPolygon(fullCircleFragmentCount);

        if (fullRate <= 0)
        {
            Vector2 b = GeometryHelper.RadianToVector2D(startAngleDeg * Mathf.Deg2Rad) * radius;
            return new LineSegment(Vector3.zero, b).ToDrawable().polygons[0];
        }


        float segmentLength = DeltaAngleDeg;

        float startAngleInRad = (-startAngleDeg + 90) * Mathf.Deg2Rad;
        int circleFragmentCount = Mathf.Max(2,
            Mathf.CeilToInt((fullCircleFragmentCount + 1) * (Mathf.Abs(segmentLength) / 360)));

        segmentLength *= Mathf.Deg2Rad;

        var points = new Vector3[
            innerRadius >= radius ? circleFragmentCount + 2 :
            innerRadius > 0 ? circleFragmentCount * 2 + 1 :
            circleFragmentCount];

        Vector3 right = new Vector2(radius, 0);
        Vector3 up = new Vector2(0, radius);

        int pointIndex = 0;
        AddSegmentPoints(true);

        if (innerRadius >= radius)
        {
            points[pointIndex++] = Vector3.zero;
            points[pointIndex] = points[0];
        }
        else if (innerRadius > 0)
        {
            right = new Vector2(innerRadius, 0);
            up = new Vector2(0, innerRadius);

            AddSegmentPoints(false);
            points[pointIndex] = points[0];
        }

        return points;

        void AddSegmentPoints(bool dir)
        {
            for (int i = 0; i < circleFragmentCount; i++)
            {
                float rate = (float)i / (circleFragmentCount - 1);
                if (!dir) rate = 1 - rate;
                float phase = startAngleInRad - rate * segmentLength;
                points[pointIndex] = Mathf.Sin(phase) * right + Mathf.Cos(phase) * up;
                pointIndex++;
            }
        }
    }

/*
    public IEnumerable<EasyHandle> GetHandles()
    {
        Vector3 p = GeometryHelper.RadianToVector2D(startAngleDeg * Mathf.Deg2Rad) * radius;
        yield return new EasyHandle() { position = p};
    }

    public void SetHandle(int i, HandleResult result)
    {
        Vector2 dir = result.startPosition;
        radius = dir.magnitude;
        startAngleDeg = GeometryHelper.Vector2DToRadian(dir) * Mathf.Rad2Deg;
    }
*/

    public Vector2 GetRandomPointInArea()
    {
        float a = UnityEngine.Random.Range(0, 2 * Mathf.PI); // TODO: Just On Segment
        float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, radius));

        float x = r * Mathf.Sin(a);
        float y = r * Mathf.Cos(a);

        return new Vector2(x, y);
    }
}

[Serializable]
public class SpacialCircleSector : SpacialShape2D<CircleSector>
{
}
}