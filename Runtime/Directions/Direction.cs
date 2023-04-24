using System.Collections.Generic;

namespace MUtility
{
	public enum HorizontalDirection
	{
		Right = 1,
		Left = 5
	}

	public enum VerticalDirection
	{
		Down = 3,
		Up = 7
	}

	public enum GeneralDirection2D
	{
		Right = 1,
		Down = 3,
		Left = 5,
		Up = 7
	}

	public enum DiagonalDirection2D
	{
		UpRight = 0,
		DownRight = 2,
		DownLeft = 4,
		UpLeft = 6,
	}

	public enum Direction2D
	{
		UpRight = 0,
		Right = 1,
		DownRight = 2,
		Down = 3,
		DownLeft = 4,
		Left = 5,
		UpLeft = 6,
		Up = 7
	}

	public enum GeneralDirection3D
	{
		Right = 1,
		Down = 3,
		Left = 5,
		Up = 7,
		Forward = 8,
		Back = 9
	}

	public enum Direction3D
	{
		// General
		Right = 1,
		Down = 3,
		Left = 5,
		Up = 7,
		Forward = 8,
		Back = 9,

		// Diagonal Edge
		RightUp = 10,
		RightDown = 11,
		RightForward = 12,
		RightBack = 13,
		LeftUp = 14,
		LeftDown = 15,
		LeftForward = 16,
		LeftBack = 17,
		UpForward = 18,
		UpBack = 19,
		DownForward = 20,
		DownBack = 21,

		// Diagonal Corner
		RightUpForward = 22,
		RightUpBack = 23,
		RightDownForward = 24,
		RightDownBack = 25,
		LeftUpForward = 26,
		LeftUpBack = 27,
		LeftDownForward = 28,
		LeftDownBack = 29,
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
	}
}