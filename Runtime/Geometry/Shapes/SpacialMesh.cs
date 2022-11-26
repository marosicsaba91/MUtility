using UnityEngine;

namespace MUtility
{
public abstract class SpacialMesh<TShape> where TShape : IMesh
{
    public TShape shape;
    public Vector3 position;
    [EulerAngles] public Quaternion rotation;

    public void OnDrawHandles() => SpacialShapeHelper.OnDrawHandles(ref shape, ref position, ref rotation);
}
}