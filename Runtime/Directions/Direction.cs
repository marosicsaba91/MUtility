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
	}
}