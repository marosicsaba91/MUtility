using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct CircleSector : IPolygon, IDrawable, IEasyHandleable, ICircumference, IArea
{
    const int defaultFragmentCount = 75;

    [SerializeField, Min(0)] float radius;
    [SerializeField, Range(0, 1)] float innerRadiusRate;
    [SerializeField, Range(0, 360)] float startAngleDeg;
    [SerializeField, Range(0, 360)] float endAngleDeg;
    [SerializeField] Winding winding;
    
    float WindingSign => winding == Winding.Clockwise ? -1 : 1;
    
    public CircleSector(float radius, float startAngleDeg, float endAngleDeg, float innerRadiusRate = 0.5f, Winding winding = Winding.Clockwise)
    {
        this.radius = radius;
        this.innerRadiusRate = Mathf.Clamp01(innerRadiusRate);
        
        this.startAngleDeg = startAngleDeg;
        this.endAngleDeg = endAngleDeg;
        this.winding = winding;
    }
    
    public float Radius
    {
        get => radius;
        set => radius = Mathf.Max(0, value);
    }
    
    public float StartAngleDeg
    {
        get => startAngleDeg;
        set => startAngleDeg = MathHelper.Mod(value, 360);
    }
    public float EndAngleDeg
    {
        get => endAngleDeg;
        set => endAngleDeg = MathHelper.Mod(value, 360);
    }
    
    public float DeltaAngleRate
    {
        get => DeltaAngleDeg / 360;
        set => DeltaAngleDeg = value * 360;
    }

    public float DeltaAngleDeg
    {
        get => WindingSign * MathHelper.Mod(WindingSign * (endAngleDeg - startAngleDeg), 360);
        set => endAngleDeg = startAngleDeg + (Mathf.Clamp01( WindingSign * value) * WindingSign);
    }  
    
    public float InnerRadiusRate
    {
        get => innerRadiusRate;
        set => innerRadiusRate = Mathf.Clamp01(value);
    }
    
    public float InnerRadius
    {
        get => radius * innerRadiusRate;
        set => InnerRadiusRate = value / radius;
    }
    
    public float Circumference => 2 * ((radius + InnerRadius) * Mathf.PI * DeltaAngleRate + (radius - InnerRadius));

    public float Area => (radius * radius * Mathf.PI) - (InnerRadius * InnerRadius * Mathf.PI) * DeltaAngleRate;

    public IEnumerable<Vector3> Points => ToPolygon();

    public IEnumerable<Vector3> ToPolygon(int fullCircleFragmentCount = defaultFragmentCount)
    {
        Winding wind = winding;
        float segmentLength = DeltaAngleDeg;

        float startAngleInRad = (-startAngleDeg + 90) * Mathf.Deg2Rad;
        int segmentCount = Mathf.Max(2,
            Mathf.CeilToInt((fullCircleFragmentCount + 1) * (Mathf.Abs(segmentLength) / 360)));

        segmentLength *= Mathf.Deg2Rad;
        
        var points = new Vector3[segmentCount * 2 + 1];

        Vector3 right = new Vector2(radius, 0);
        Vector3 up = new Vector2(0, radius);

        int pointIndex = 0;
        AddSegmentPoints(true);

        right = new Vector2(InnerRadius, 0);
        up = new Vector2(0, InnerRadius);
 
        AddSegmentPoints(false);
        points[pointIndex] = points[0];

        return points;
       
        void AddSegmentPoints(bool bigRound)
        {  
            for (int i = 0; i < segmentCount; i++)
            {
                float rate = (float)i / (segmentCount - 1);
                
                if (!bigRound) 
                    rate = 1 - rate;
                
                float phase = startAngleInRad - rate * segmentLength;
                points[pointIndex] = Mathf.Sin(phase) * right + Mathf.Cos(phase) * up;
                pointIndex++;
            }
        }
    }
 
    public void OnDrawHandles()
    {
        Normalize ();
        
        float middleAngle = startAngleDeg + (DeltaAngleDeg / 2);
        Vector3 middleDirection = GeometryHelper.RadianToVector2D(middleAngle * Mathf.Deg2Rad);
        radius = EasyHandles.PositionHandle(middleDirection * radius, middleDirection, EasyHandles.Shape.Dot).magnitude;
        
        InnerRadius = EasyHandles.PositionHandle(middleDirection * InnerRadius, middleDirection, ForcedAxisMode.Line, EasyHandles.Shape.Dot).magnitude;
        
        float middleRadius = (radius + InnerRadius) / 2;
        Vector3 angle1 = GeometryHelper.RadianToVector2D(startAngleDeg * Mathf.Deg2Rad);
        startAngleDeg = EasyHandles.PositionHandle(angle1 * middleRadius).GetAngle(); 
        
        Vector3 angle2 = GeometryHelper.RadianToVector2D(endAngleDeg * Mathf.Deg2Rad);
        endAngleDeg = EasyHandles.PositionHandle(angle2 * middleRadius).GetAngle();
    }

    void Normalize()
    {
        if (float.IsNaN(startAngleDeg) || float.IsNaN(endAngleDeg) || float.IsNaN(radius) || float.IsNaN(InnerRadiusRate))
        {
            startAngleDeg = 0;
            endAngleDeg = 180;
            radius = 1;
            InnerRadiusRate = 0.5f;
            winding = Winding.CounterClockwise;
        } 
    }

    public Vector2 GetRandomPointInArea()
    {
        float a = UnityEngine.Random.Range(0, 2 * Mathf.PI); // TODO: Just On Segment
        float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, radius));

        float x = r * Mathf.Sin(a);
        float y = r * Mathf.Cos(a);

        return new Vector2(x, y);
    }

    public Drawable ToDrawable() => Points.ToDrawable();
}

[Serializable]
public class SpacialCircleSector : SpacialPolygon<CircleSector> { }

}