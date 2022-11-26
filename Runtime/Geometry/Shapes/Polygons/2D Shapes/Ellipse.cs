using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct Ellipse : IPolygon, IDrawable, IEasyHandleable, ICircumference, IArea
{
    const int defaultFragmentCount = 30;

    public float radiusHorizontal;
    public float radiusVertical;

    public Ellipse(float radiusHorizontal, float radiusVertical)
    {
        this.radiusHorizontal = radiusHorizontal;
        this.radiusVertical = radiusVertical;
    }

    public float Circumference
    {
        get
        {
            float a = radiusHorizontal;
            float b = radiusVertical;
            float sqrt = Mathf.Sqrt((a * a + b * b) / 2f);
            float v = 2 * Mathf.PI * sqrt + Mathf.PI * (a + b);
            return v / 2;
        }
    }

    public float Area => radiusVertical * radiusHorizontal * Mathf.PI;

    public bool IsInsideShape(Vector2 point)
    {
        Vector2 vec = point;
        vec = new Vector2(vec.x / radiusHorizontal, vec.y / radiusVertical);
        return vec.magnitude <= 1;
    }

    public IEnumerable<Vector3> Points => ToPolygon();

    IEnumerable<Vector3> ToPolygon(int fragmentCount = defaultFragmentCount)
    {
        var points = new Vector3[fragmentCount];

        float angle = Mathf.PI * 2f / (fragmentCount - 1);
        for (int i = 0; i < fragmentCount - 1; i++)
        {
            float phase = i * angle;
            points[i] = Mathf.Sin(phase) * Vector3.right + Mathf.Cos(phase) * Vector3.up;
            points[i].x *= radiusHorizontal;
            points[i].y *= radiusVertical;
        }

        points[fragmentCount - 1] = points[0];

        return points;
    }

    public void OnDrawHandles()
    {
        if (radiusHorizontal == 0 || radiusVertical == 0) return; 
        
        Vector2 vx = Vector2.right * radiusHorizontal;
        Vector2 vy = Vector2.up * radiusVertical;
        float ax = Mathf.Abs(radiusHorizontal);
        float ay = Mathf.Abs(radiusVertical);
        
        float longer = Mathf.Max(ax, ay); 
        float average = (ax + ay) / 2f;
        float size = Mathf.Max(average, longer * 0.75f);
        EasyHandles.fullObjectSize = size;
        
        radiusHorizontal = EasyHandles.PositionHandle(vx,  vx.normalized).x;
        radiusVertical = EasyHandles.PositionHandle(vy, vy.normalized).y; 
    } 

    public Vector2 GetRandomPointInArea()
    {
        float a = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));

        float x = r * Mathf.Sin(a) * radiusHorizontal;
        float y = r * Mathf.Cos(a) * radiusVertical;

        return new Vector2(x, y);
    }

    public Drawable ToDrawable() => Points.ToDrawable();
}


[Serializable]
public class SpacialEllipse : SpacialPolygon<Ellipse>
{
}
}