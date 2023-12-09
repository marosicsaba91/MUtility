using UnityEngine;

namespace MUtility
{
	public enum HorizontalDirection
	{
		Right = 1,
		Left = -1
	}

	public enum VerticalDirection
	{
		Down = -1,
		Up = 1
	}

	public enum GeneralDirection2D
	{
		Right = 0,
		Down = 1,
		Left = 2,
		Up = 3
	}

	public enum DiagonalDirection2D
	{
		UpRight = 0,
		DownRight = 1,
		DownLeft = 2,
		UpLeft = 3,
	}

	public enum Direction2D
	{
		RightUp = 0,
		Right = 1,
		RightDown = 2,
		Down = 3,
		LeftDown = 4,
		Left = 5,
		LeftUp = 6,
		Up = 7
	}

	public enum GeneralDirection3D
	{
		Right = 0,
		Down = 1,
		Left = 2,
		Up = 3,
		Forward = 4,
		Back = 5
	}

	public enum Direction3D
	{
		// General
		Right = 0,
		Down = 1,
		Left = 2,
		Up = 3,
		Forward = 4,
		Back = 5,

		// Diagonal Edge
		RightUp = 6,
		RightDown = 7,
		RightForward = 8,
		RightBack = 9,
		LeftUp = 10,
		LeftDown = 11,
		LeftForward = 12,
		LeftBack = 13,
		UpForward = 14,
		UpBack = 15,
		DownForward = 16,
		DownBack = 17,

		// Diagonal Corner
		RightUpForward = 18,
		RightUpBack = 19,
		RightDownForward = 20,
		RightDownBack = 21,
		LeftUpForward = 22,
		LeftUpBack = 23,
		LeftDownForward = 24,
		LeftDownBack = 25,
	}

	public enum StepDirection
	{
		Forward = 1,
		Backward = -1
	}

	public enum Winding
	{
		Clockwise = -1,
		CounterClockwise = 1
	}

	public static class DirectionUtility
	{
		public static readonly Axis3D[] allAxis3D = { Axis3D.X, Axis3D.Y, Axis3D.Z };

		public static readonly GeneralDirection3D[] generalDirection3DValues =
		{
			GeneralDirection3D.Right,
			GeneralDirection3D.Down,
			GeneralDirection3D.Left,
			GeneralDirection3D.Up,
			GeneralDirection3D.Forward,
			GeneralDirection3D.Back
		};

		public static readonly GeneralDirection2D[] generalDirection2DValues =
		{
			GeneralDirection2D.Right,
			GeneralDirection2D.Down,
			GeneralDirection2D.Left,
			GeneralDirection2D.Up,
		};

		public static readonly Direction3D[] direction3DValues =
		{
			// General
			Direction3D.Right,
			Direction3D.Down,
			Direction3D.Left,
			Direction3D.Up,
			Direction3D.Forward,
			Direction3D.Back,

			// Diagonal Edge
			Direction3D.RightUp,
			Direction3D.RightDown,
			Direction3D.RightForward,
			Direction3D.RightBack,
			Direction3D.LeftUp,
			Direction3D.LeftDown,
			Direction3D.LeftForward,
			Direction3D.LeftBack,
			Direction3D.UpForward,
			Direction3D.UpBack,
			Direction3D.DownForward,
			Direction3D.DownBack,

			// Diagonal Corner
			Direction3D.RightUpForward,
			Direction3D.RightUpBack,
			Direction3D.RightDownForward,
			Direction3D.RightDownBack,
			Direction3D.LeftUpForward,
			Direction3D.LeftUpBack ,
			Direction3D.LeftDownForward ,
			Direction3D.LeftDownBack ,
		};

		public static readonly Direction2D[] direction2DValues =
		{
			Direction2D.RightUp,
			Direction2D.Right,
			Direction2D.RightDown,
			Direction2D.Down,
			Direction2D.LeftDown,
			Direction2D.Left,
			Direction2D.LeftUp,
			Direction2D.Up
		};

		public static GeneralDirection3D GeneralDirection3DFromVector(Vector3Int vector) 
		{
			int ax = Mathf.Abs(vector.x);
			int ay = Mathf.Abs(vector.y);
			int az = Mathf.Abs(vector.z);
			if (ax > ay && ax > az)
				return vector.x > 0 ? GeneralDirection3D.Right : GeneralDirection3D.Left;
			if (ay > ax && ay > az)
				return vector.y > 0 ? GeneralDirection3D.Up : GeneralDirection3D.Down;
			return vector.z > 0 ? GeneralDirection3D.Forward : GeneralDirection3D.Back;
		}
	}
}