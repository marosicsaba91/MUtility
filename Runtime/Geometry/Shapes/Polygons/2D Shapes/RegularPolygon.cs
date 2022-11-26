﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct RegularPolygon : IPolygon, IDrawable, IEasyHandleable, ICircumference, IArea
{
    [SerializeField, Min(3)] int sides;
    [SerializeField] float radius;

    public float SideLength => Mathf.Sin(Mathf.Deg2Rad * 180f / sides) * radius * 2;

    public float InnerAngle => 180 - (360f / sides);
    public float OuterAngle => 360 - InnerAngle;

    public float Circumference => SideLength * sides;

    public float Area
    {
        get
        { 
            float a = SideLength / 2; 
            float b = Mathf.Cos(Mathf.Deg2Rad * 180f / sides) * radius;
            float rectArea = a * b;
            return rectArea * sides; 
        }
    }

    public IEnumerable<Vector3> Points 
    {
        get
        {
            for (int i = 0; i <= sides; i++)
            {
                float angle = i * 2 * Mathf.PI / sides;
                yield return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
            }
        }
    }
    public Drawable ToDrawable() => Points.ToDrawable();
    public void OnDrawHandles()
    {
        radius = EasyHandles.PositionHandle(Vector3.up* radius, Vector3.up, EasyHandles.Shape.Dot).y;
    }
}
}