using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
public abstract class SpacialPolygon<TPolygon> : IEasyHandleable, IPolygon where TPolygon : IPolygon
{
    public TPolygon polygon;
    public Vector3 position; 
    [EulerAngles] public Quaternion rotation;

    public IEnumerable<Vector3> Points => polygon.Points.Transform(position, rotation, 1);

    public void OnDrawHandles() => SpacialShapeHelper.OnDrawHandles(ref polygon, ref position, ref rotation);

    public Vector3 Right => rotation * Vector3.right;
    public Vector3 Up => rotation * Vector3.up;
    public Vector3 Forward => rotation * Vector3.forward;
}
}