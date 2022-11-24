using UnityEngine;

namespace MUtility
{
public abstract class SpacialShape3D<TShape> where TShape : IShape3D
{
    public TShape shape;
    public Vector3 position;
    [EulerAngles] public Quaternion rotation;

    public void OnDrawHandles() => SpacialShapeHelper.OnDrawHandles(ref shape, ref position, ref rotation);
}
}