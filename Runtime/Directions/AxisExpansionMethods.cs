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


		public static Vector3 ToVector(this Axis3D axis)
		{
			switch (axis)
			{
				case Axis3D.X:
					return Vector3.right;
				case Axis3D.Y:
					return Vector3.up;
				case Axis3D.Z:
					return Vector3.forward;
				default:
					throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
			}
		}

		public static Vector3Int ToIntVector(this Axis3D axis)
		{
			switch (axis)
			{
				case Axis3D.X:
					return Vector3Int.right;
				case Axis3D.Y:
					return Vector3Int.up;
				case Axis3D.Z:
					return Vector3Int.forward;
				default:
					throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
			}
		}

		public static GeneralDirection3D ToPositiveDirection(this Axis3D axis)
		{
			switch (axis)
			{
				case Axis3D.X:
					return GeneralDirection3D.Right;
				case Axis3D.Y:
					return GeneralDirection3D.Up;
				case Axis3D.Z:
					return GeneralDirection3D.Forward;
				default:
					throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
			}
		}

		public static Vector3 ToNegativeVector(this Axis3D axis)
		{
			switch (axis)
			{
				case Axis3D.X:
					return Vector3.left;
				case Axis3D.Y:
					return Vector3.down;
				case Axis3D.Z:
					return Vector3.back;
				default:
					throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
			}
		}

		public static GeneralDirection3D ToNegativeDirection(this Axis3D axis)
		{
			switch (axis)
			{
				case Axis3D.X:
					return GeneralDirection3D.Left;
				case Axis3D.Y:
					return GeneralDirection3D.Down;
				case Axis3D.Z:
					return GeneralDirection3D.Back;
				default:
					throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
			}
		}

		public static Color GetAxisColor(this Axis3D axis)
		{
			switch (axis)
			{
				case Axis3D.X:
					return Color.red;
				case Axis3D.Y:
					return Color.green;
				case Axis3D.Z:
					return Color.blue;
				default:
					throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
			}
		}

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
	}
}