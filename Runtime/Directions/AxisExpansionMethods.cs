using System;
using UnityEngine;

namespace MUtility
{
	public static class AxisExpansionMethods
	{
		public static Axis3D ToAxis(this Vector3 vector)
		{
			float absX = Mathf.Abs(vector.x);
			float absY = Mathf.Abs(vector.y);
			float absZ = Mathf.Abs(vector.z);

			if (absX > absY && absX > absZ)
				return Axis3D.X;
			if (absY > absX && absY > absZ)
				return Axis3D.Y;
			return Axis3D.Z;
		}

		public static Axis3D ToAxis(this Vector3Int vector)
		{
			float absX = Mathf.Abs(vector.x);
			float absY = Mathf.Abs(vector.y);
			float absZ = Mathf.Abs(vector.z);

			if (absX > absY && absX > absZ)
				return Axis3D.X;
			if (absY > absX && absY > absZ)
				return Axis3D.Y;
			return Axis3D.Z;
		}


		public static Vector3 ToVector(this Axis3D axis) => axis switch
		{
			Axis3D.X => Vector3.right,
			Axis3D.Y => Vector3.up,
			Axis3D.Z => Vector3.forward,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
		};

		public static Vector3Int ToIntVector(this Axis3D axis) => axis switch
		{
			Axis3D.X => Vector3Int.right,
			Axis3D.Y => Vector3Int.up,
			Axis3D.Z => Vector3Int.forward,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
		};

		public static GeneralDirection3D ToPositiveDirection(this Axis3D axis) => axis switch
		{
			Axis3D.X => GeneralDirection3D.Right,
			Axis3D.Y => GeneralDirection3D.Up,
			Axis3D.Z => GeneralDirection3D.Forward,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
		};

		public static Vector3 ToNegativeVector(this Axis3D axis) => axis switch
		{
			Axis3D.X => Vector3.left,
			Axis3D.Y => Vector3.down,
			Axis3D.Z => Vector3.back,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
		};

		public static GeneralDirection3D ToNegativeDirection(this Axis3D axis) => axis switch
		{
			Axis3D.X => GeneralDirection3D.Left,
			Axis3D.Y => GeneralDirection3D.Down,
			Axis3D.Z => GeneralDirection3D.Back,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
		};

		public static Color GetAxisColor(this Axis3D axis) => axis switch
		{
			Axis3D.X => Color.red,
			Axis3D.Y => Color.green,
			Axis3D.Z => Color.blue,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
		};

		public static Axis3D Next(this Axis3D axis) => axis switch
		{ 
			Axis3D.X => Axis3D.Y, 
			Axis3D.Y => Axis3D.Z, 
			Axis3D.Z => Axis3D.X,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null) 			
		};

		public static Axis3D Previous(this Axis3D axis) => axis switch
		{
			Axis3D.X => Axis3D.Z, 
			Axis3D.Y => Axis3D.X, 
			Axis3D.Z => Axis3D.Y,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
		};

		public static Axis2D Opposite(this Axis2D dir) => dir switch
		{
			Axis2D.Horizontal => Axis2D.Vertical,
			Axis2D.Vertical => Axis2D.Horizontal,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};
	}
}