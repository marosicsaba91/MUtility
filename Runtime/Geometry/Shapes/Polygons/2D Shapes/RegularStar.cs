using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct RegularStar : IPolygon, IDrawable, IEasyHandleable, ICircumference, IArea
{
    [SerializeField, Min(3)] int pointsCounts;
    [SerializeField] float radius;
    [SerializeField, Range(0,1)] float innerRadiusRate;
    
    public float Circumference => SideLength * pointsCounts * 2;
    
    public float PointInnerAngle  => (180 - (360f / pointsCounts)) * innerRadiusRate;
    
    public float InnerRadius
    {
        get
        { 
            float alpha = Mathf.Deg2Rad * 180f / pointsCounts;
            float b = radius * Mathf.Cos(alpha); 
            return b * innerRadiusRate;
        }
        set 
        { 
            float alpha = Mathf.Deg2Rad * 180f / pointsCounts;
            float b = radius * Mathf.Cos(alpha); 
            innerRadiusRate = Mathf.Clamp01( value / b);
        } 
    }
    
    public  float InnerRadiusRate
    {
        get => innerRadiusRate;
        set => innerRadiusRate = Mathf.Clamp01(value);
    }
    
    public float Radius
    {
        get => radius;
        set => radius = Mathf.Clamp01(value);
    }

    public int PointCounts 
    {
        get => pointsCounts;
        set => pointsCounts = Mathf.Clamp(value, 3, int.MaxValue);
    }

    public float SideLength
    {
        get
        { 
            float polygonSideLength = Mathf.Sin(Mathf.Deg2Rad * 180f / pointsCounts) * radius * 2;
            Debug.Log(polygonSideLength);
            float a = polygonSideLength / 2; 
            float bBig = Mathf.Cos(Mathf.Deg2Rad * 180f / pointsCounts) * radius;
            float bSmall = bBig * (1-innerRadiusRate);
            float cSmall = Mathf.Sqrt(a * a + bSmall * bSmall);
            Debug.Log(cSmall);
            return cSmall;
        }
    }
    
    public float Area
    {
        get
        { 
            float polygonSideLength = Mathf.Sin(Mathf.Deg2Rad * 180f / pointsCounts) * radius * 2;
            float a = polygonSideLength / 2; 
            float bBig = Mathf.Cos(Mathf.Deg2Rad * 180f / pointsCounts) * radius;
            float rectBig = a *bBig;
            float bSmall = bBig * (1-innerRadiusRate);
            float rectSmall = a * bSmall;
            return (rectBig- rectSmall) * pointsCounts; 
        }
    }

    public IEnumerable<Vector3> Points 
    {
        get
        {
            for (int i = 0; i <= pointsCounts*2; i++)
            {
                float angle = i * Mathf.PI / pointsCounts;
                
                float r = i % 2 == 0 ? radius : InnerRadius;
                
                yield return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * r;
            }
        }
    }
    public Drawable ToDrawable() => Points.ToDrawable();
    public void OnDrawHandles()
    {
        radius = EasyHandles.PositionHandle(Vector3.up* radius, Vector3.up, EasyHandles.Shape.Dot).y;
        
        float angle = Mathf.PI / pointsCounts;
        Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        InnerRadius = EasyHandles.PositionHandle(dir * InnerRadius, dir, EasyHandles.Shape.Dot).magnitude;
    }
}
}