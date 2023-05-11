using UnityEngine;

namespace MUtility
{
	public static class SpatialShapeHelper
	{
		public static bool OnDrawHandles<T>(ref T shape, ref Vector3 position, ref Quaternion rotation)
		{
			bool changed = false;

			if (shape is IEasyHandleable handleable)
			{
				rotation.Normalize();
				Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);

				EasyHandles.PushMatrix(matrix);
				changed |= handleable.DrawHandles();
				shape = (T)handleable;
				EasyHandles.PopMatrix(matrix);
			}

			position = EasyHandles.PositionHandle(position, rotation, EasyHandles.Shape.SmallPosition);
			rotation = EasyHandles.RotationHandle(position, rotation);

			return changed;
		}
	}
}