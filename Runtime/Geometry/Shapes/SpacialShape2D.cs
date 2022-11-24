using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
public abstract class SpacialShape2D<TShape> : IRuntimeHandleable, IPolygon where TShape : IShape2D
{
    public TShape shape;
    public Vector3 position; 
    
    [EulerAngles] public Quaternion rotation;

    public IEnumerable<Vector3> Points => shape.Points.Transform(position, rotation, 1);

    public void OnDrawHandles() => SpacialShapeHelper.OnDrawHandles(ref shape, ref position, ref rotation);

    public Vector3 Right => rotation * Vector3.right;
    public Vector3 Up => rotation * Vector3.up;
    public Vector3 Forward => rotation * Vector3.forward;

}
}