using UnityEngine;

namespace MUtility
{
public static class SpacialShapeHelper
{
    public static void OnDrawHandles<T>(ref T shape, ref Vector3 position, ref Quaternion rotation)
    {
        if (shape is IEasyHandleable handleable)
        {
            rotation.Normalize();
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);

            EasyHandles.PushMatrix(matrix);
            handleable.OnDrawHandles();
            shape = (T)handleable;
            EasyHandles.PopMatrix(matrix);
        }
        
        position = EasyHandles.PositionHandle(position, rotation, EasyHandles.Shape.SmallPosition);
        rotation = EasyHandles.RotationHandle(position, rotation);
    }
}
}