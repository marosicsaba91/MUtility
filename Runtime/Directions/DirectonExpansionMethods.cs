using System;
using UnityEngine;

namespace MUtility
{
	public static class DirectionExpanded
	{
		static readonly float diagonal2DLength;
		static readonly float diagonal3DLength;

		static DirectionExpanded()
		{
			diagonal2DLength = Mathf.Sqrt(2);
			diagonal3DLength = Mathf.Sqrt(3);

		}

		//Perpendicular
		public static GeneralDirection3D GetPerpendicularRight(this GeneralDirection3D dir, GeneralDirection3D dir2) => (dir, dir2) switch
		{
			(GeneralDirection3D.Up, GeneralDirection3D.Forward) => GeneralDirection3D.Left,
			(GeneralDirection3D.Up, GeneralDirection3D.Back) => GeneralDirection3D.Right,
			(GeneralDirection3D.Up, GeneralDirection3D.Right) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Up, GeneralDirection3D.Left) => GeneralDirection3D.Back,
			(GeneralDirection3D.Down, GeneralDirection3D.Forward) => GeneralDirection3D.Right,
			(GeneralDirection3D.Down, GeneralDirection3D.Back) => GeneralDirection3D.Left,
			(GeneralDirection3D.Down, GeneralDirection3D.Right) => GeneralDirection3D.Back,
			(GeneralDirection3D.Down, GeneralDirection3D.Left) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Right, GeneralDirection3D.Forward) => GeneralDirection3D.Up,
			(GeneralDirection3D.Right, GeneralDirection3D.Back) => GeneralDirection3D.Down,
			(GeneralDirection3D.Right, GeneralDirection3D.Up) => GeneralDirection3D.Back,
			(GeneralDirection3D.Right, GeneralDirection3D.Down) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Left, GeneralDirection3D.Forward) => GeneralDirection3D.Down,
			(GeneralDirection3D.Left, GeneralDirection3D.Back) => GeneralDirection3D.Up,
			(GeneralDirection3D.Left, GeneralDirection3D.Up) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Left, GeneralDirection3D.Down) => GeneralDirection3D.Back,
			(GeneralDirection3D.Forward, GeneralDirection3D.Right) => GeneralDirection3D.Down,
			(GeneralDirection3D.Forward, GeneralDirection3D.Left) => GeneralDirection3D.Up,
			(GeneralDirection3D.Forward, GeneralDirection3D.Up) => GeneralDirection3D.Right,
			(GeneralDirection3D.Forward, GeneralDirection3D.Down) => GeneralDirection3D.Left,
			(GeneralDirection3D.Back, GeneralDirection3D.Right) => GeneralDirection3D.Up,
			(GeneralDirection3D.Back, GeneralDirection3D.Left) => GeneralDirection3D.Down,
			(GeneralDirection3D.Back, GeneralDirection3D.Up) => GeneralDirection3D.Left,
			(GeneralDirection3D.Back, GeneralDirection3D.Down) => GeneralDirection3D.Right,
			_ => throw new ArgumentException("Invalid direction combination: Not perpendicular")
		};


		public static GeneralDirection3D GetPerpendicularLeftHand(this GeneralDirection3D dir, GeneralDirection3D dir2) => (dir, dir2) switch
		{
			(GeneralDirection3D.Up, GeneralDirection3D.Forward) => GeneralDirection3D.Right,
			(GeneralDirection3D.Up, GeneralDirection3D.Back) => GeneralDirection3D.Left,
			(GeneralDirection3D.Up, GeneralDirection3D.Right) => GeneralDirection3D.Back,
			(GeneralDirection3D.Up, GeneralDirection3D.Left) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Down, GeneralDirection3D.Forward) => GeneralDirection3D.Left,
			(GeneralDirection3D.Down, GeneralDirection3D.Back) => GeneralDirection3D.Right,
			(GeneralDirection3D.Down, GeneralDirection3D.Right) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Down, GeneralDirection3D.Left) => GeneralDirection3D.Back,
			(GeneralDirection3D.Right, GeneralDirection3D.Forward) => GeneralDirection3D.Down,
			(GeneralDirection3D.Right, GeneralDirection3D.Back) => GeneralDirection3D.Up,
			(GeneralDirection3D.Right, GeneralDirection3D.Up) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Right, GeneralDirection3D.Down) => GeneralDirection3D.Back,
			(GeneralDirection3D.Left, GeneralDirection3D.Forward) => GeneralDirection3D.Up,
			(GeneralDirection3D.Left, GeneralDirection3D.Back) => GeneralDirection3D.Down,
			(GeneralDirection3D.Left, GeneralDirection3D.Up) => GeneralDirection3D.Back,
			(GeneralDirection3D.Left, GeneralDirection3D.Down) => GeneralDirection3D.Forward,
			(GeneralDirection3D.Forward, GeneralDirection3D.Right) => GeneralDirection3D.Up,
			(GeneralDirection3D.Forward, GeneralDirection3D.Left) => GeneralDirection3D.Down,
			(GeneralDirection3D.Forward, GeneralDirection3D.Up) => GeneralDirection3D.Left,
			(GeneralDirection3D.Forward, GeneralDirection3D.Down) => GeneralDirection3D.Right,
			(GeneralDirection3D.Back, GeneralDirection3D.Right) => GeneralDirection3D.Down,
			(GeneralDirection3D.Back, GeneralDirection3D.Left) => GeneralDirection3D.Up,
			(GeneralDirection3D.Back, GeneralDirection3D.Up) => GeneralDirection3D.Right,
			(GeneralDirection3D.Back, GeneralDirection3D.Down) => GeneralDirection3D.Left,
			_ => throw new ArgumentException("Invalid direction combination: Not perpendicular")
		};

		public static GeneralDirection3D Next(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Right => GeneralDirection3D.Up,
			GeneralDirection3D.Up => GeneralDirection3D.Forward,
			GeneralDirection3D.Forward => GeneralDirection3D.Left,
			GeneralDirection3D.Left => GeneralDirection3D.Down,
			GeneralDirection3D.Down => GeneralDirection3D.Back,
			GeneralDirection3D.Back => GeneralDirection3D.Right,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};


		//Perpendicular
		public static GeneralDirection3D Previous(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Right => GeneralDirection3D.Back,
			GeneralDirection3D.Up => GeneralDirection3D.Right,
			GeneralDirection3D.Forward => GeneralDirection3D.Up,
			GeneralDirection3D.Left => GeneralDirection3D.Forward,
			GeneralDirection3D.Down => GeneralDirection3D.Left,
			GeneralDirection3D.Back => GeneralDirection3D.Down,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};



		// Equals
		public static bool Equals(this GeneralDirection2D self, GeneralDirection3D dir) => self switch
		{
			GeneralDirection2D.Right => dir == GeneralDirection3D.Right,
			GeneralDirection2D.Up => dir == GeneralDirection3D.Up,
			GeneralDirection2D.Left => dir == GeneralDirection3D.Left,
			GeneralDirection2D.Down => dir == GeneralDirection3D.Down,
			_ => false
		};


		public static bool Equals(this GeneralDirection2D self, Direction2D dir) => self switch
		{
			GeneralDirection2D.Right => dir == Direction2D.Right,
			GeneralDirection2D.Up => dir == Direction2D.Up,
			GeneralDirection2D.Left => dir == Direction2D.Left,
			GeneralDirection2D.Down => dir == Direction2D.Down,
			_ => false
		};



		public static bool Equals(this GeneralDirection3D self, GeneralDirection2D dir) => dir switch
		{
			GeneralDirection2D.Right => self == GeneralDirection3D.Right,
			GeneralDirection2D.Up => self == GeneralDirection3D.Up,
			GeneralDirection2D.Left => self == GeneralDirection3D.Left,
			GeneralDirection2D.Down => self == GeneralDirection3D.Down,
			_ => false
		};


		public static bool Equals(this GeneralDirection3D self, Direction2D dir) => self switch
		{
			GeneralDirection3D.Right => dir == Direction2D.Right,
			GeneralDirection3D.Left => dir == Direction2D.Left,
			GeneralDirection3D.Up => dir == Direction2D.Up,
			GeneralDirection3D.Down => dir == Direction2D.Down,
			_ => false
		};

		public static bool Equals(this Direction2D self, GeneralDirection2D dir) => dir switch
		{
			GeneralDirection2D.Right => self == Direction2D.Right,
			GeneralDirection2D.Left => self == Direction2D.Left,
			GeneralDirection2D.Up => self == Direction2D.Up,
			GeneralDirection2D.Down => self == Direction2D.Down,
			_ => false
		};

		public static bool Equals(this Direction2D self, GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Right => self == Direction2D.Right,
			GeneralDirection3D.Left => self == Direction2D.Left,
			GeneralDirection3D.Up => self == Direction2D.Up,
			GeneralDirection3D.Down => self == Direction2D.Down,
			_ => false
		};

		public static bool Equals(this GeneralDirection3D self, Direction3D dir) => self switch
		{
			GeneralDirection3D.Right => dir == Direction3D.Right,
			GeneralDirection3D.Left => dir == Direction3D.Left,
			GeneralDirection3D.Up => dir == Direction3D.Up,
			GeneralDirection3D.Down => dir == Direction3D.Down,
			GeneralDirection3D.Forward => dir == Direction3D.Forward,
			GeneralDirection3D.Back => dir == Direction3D.Back,
			_ => false
		};

		public static bool Equals(this GeneralDirection2D self, Direction3D dir) => self switch
		{
			GeneralDirection2D.Right => dir == Direction3D.Right,
			GeneralDirection2D.Left => dir == Direction3D.Left,
			GeneralDirection2D.Up => dir == Direction3D.Up,
			GeneralDirection2D.Down => dir == Direction3D.Down, 
			_ => false
		};

		public static bool Equals(this Direction2D self, Direction3D dir) => self switch
		{
			Direction2D.RightUp => dir == Direction3D.RightUp,
			Direction2D.Right => dir == Direction3D.Right,
			Direction2D.RightDown => dir == Direction3D.RightDown,
			Direction2D.Down => dir == Direction3D.Down, 
			Direction2D.LeftDown => dir == Direction3D.LeftDown,
			Direction2D.Left => dir == Direction3D.Left,
			Direction2D.LeftUp => dir == Direction3D.LeftUp,
			Direction2D.Up => dir == Direction3D.Up,
			_ => false
		};

		// Convert
		public static Direction2D ToDirection2D(this GeneralDirection2D self) => self switch
		{
			GeneralDirection2D.Right => Direction2D.Right,
			GeneralDirection2D.Left => Direction2D.Left,
			GeneralDirection2D.Up => Direction2D.Up,
			GeneralDirection2D.Down => Direction2D.Down,
			_ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
		};

		public static GeneralDirection3D ToGeneralDirection3D(this GeneralDirection2D self) => self switch
		{ 
			GeneralDirection2D.Right => GeneralDirection3D.Right,
			GeneralDirection2D.Left => GeneralDirection3D.Left,
			GeneralDirection2D.Up => GeneralDirection3D.Up,
			GeneralDirection2D.Down => GeneralDirection3D.Down,
			_ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
		};

		public static Direction3D ToDirection3D(this GeneralDirection2D self) => self switch
		{
			GeneralDirection2D.Right => Direction3D.Right,
			GeneralDirection2D.Left => Direction3D.Left,
			GeneralDirection2D.Up => Direction3D.Up,
			GeneralDirection2D.Down => Direction3D.Down,
			_ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
		};


		public static Direction3D ToDirection3D(this GeneralDirection3D self) => self switch
		{ 
			GeneralDirection3D.Right => Direction3D.Right,
			GeneralDirection3D.Left => Direction3D.Left,
			GeneralDirection3D.Up => Direction3D.Up,
			GeneralDirection3D.Down => Direction3D.Down,
			GeneralDirection3D.Forward => Direction3D.Forward,
			GeneralDirection3D.Back => Direction3D.Back,
			_ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
		};

		public static GeneralDirection3D ToGeneralDirection3D(this HorizontalDirection self) => self switch
		{
			HorizontalDirection.Right => GeneralDirection3D.Right,
			HorizontalDirection.Left => GeneralDirection3D.Left,
			_ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
		};


		// To Vector Float
		public static Vector2 ToVector(this GeneralDirection2D dir) => dir switch
		{
			GeneralDirection2D.Up => new Vector2(0, 1),
			GeneralDirection2D.Down => new Vector2(0, -1),
			GeneralDirection2D.Right => new Vector2(1, 0),
			GeneralDirection2D.Left => new Vector2(-1, 0),
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		public static Vector3 ToVector(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Up => new Vector3(0, 1, 0),
			GeneralDirection3D.Down => new Vector3(0, -1, 0),
			GeneralDirection3D.Right => new Vector3(1, 0, 0),
			GeneralDirection3D.Left => new Vector3(-1, 0, 0),
			GeneralDirection3D.Forward => new Vector3(0, 0, 1),
			GeneralDirection3D.Back => new Vector3(0, 0, -1),
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		public static Vector3 ToVector(this Direction3D dir) => dir switch
		{
			Direction3D.Up => new Vector3(0, 1, 0),
			Direction3D.Down => new Vector3(0, -1, 0),
			Direction3D.Right => new Vector3(1, 0, 0),
			Direction3D.Left => new Vector3(-1, 0, 0),
			Direction3D.Forward => new Vector3(0, 0, 1),
			Direction3D.Back => new Vector3(0, 0, -1),

			Direction3D.RightUp => new Vector3(1, 1, 0),
			Direction3D.RightDown => new Vector3(1, -1, 0),
			Direction3D.LeftDown => new Vector3(-1, -1, 0),
			Direction3D.LeftUp => new Vector3(-1, 1, 0),
			Direction3D.UpForward => new Vector3(0, 1, 1),
			Direction3D.DownForward => new Vector3(0, -1, 1),
			Direction3D.UpBack => new Vector3(0, 1, -1),
			Direction3D.DownBack => new Vector3(0, -1, -1),
			Direction3D.RightForward => new Vector3(1, 0, 1),
			Direction3D.LeftForward => new Vector3(-1, 0, 1),
			Direction3D.RightBack => new Vector3(1, 0, -1),
			Direction3D.LeftBack => new Vector3(-1, 0, -1),

			Direction3D.RightUpForward => new Vector3(1, 1, 1),
			Direction3D.RightDownForward => new Vector3(1, -1, 1),
			Direction3D.LeftDownForward => new Vector3(-1, -1, 1),
			Direction3D.LeftUpForward => new Vector3(-1, 1, 1),
			Direction3D.RightUpBack => new Vector3(1, 1, -1),
			Direction3D.RightDownBack => new Vector3(1, -1, -1),
			Direction3D.LeftDownBack => new Vector3(-1, -1, -1),
			Direction3D.LeftUpBack => new Vector3(-1, 1, -1),

			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};

		public static Vector3Int ToVectorInt(this Direction3D dir) => dir switch
		{
			Direction3D.Up => new Vector3Int(0, 1, 0),
			Direction3D.Down => new Vector3Int(0, -1, 0),
			Direction3D.Right => new Vector3Int(1, 0, 0),
			Direction3D.Left => new Vector3Int(-1, 0, 0),
			Direction3D.Forward => new Vector3Int(0, 0, 1),
			Direction3D.Back => new Vector3Int(0, 0, -1),

			Direction3D.RightUp => new Vector3Int(1, 1, 0),
			Direction3D.RightDown => new Vector3Int(1, -1, 0),
			Direction3D.LeftDown => new Vector3Int(-1, -1, 0),
			Direction3D.LeftUp => new Vector3Int(-1, 1, 0),
			Direction3D.UpForward => new Vector3Int(0, 1, 1),
			Direction3D.DownForward => new Vector3Int(0, -1, 1),
			Direction3D.UpBack => new Vector3Int(0, 1, -1),
			Direction3D.DownBack => new Vector3Int(0, -1, -1),
			Direction3D.RightForward => new Vector3Int(1, 0, 1),
			Direction3D.LeftForward => new Vector3Int(-1, 0, 1),
			Direction3D.RightBack => new Vector3Int(1, 0, -1),
			Direction3D.LeftBack => new Vector3Int(-1, 0, -1),

			Direction3D.RightUpForward => new Vector3Int(1, 1, 1),
			Direction3D.RightDownForward => new Vector3Int(1, -1, 1),
			Direction3D.LeftDownForward => new Vector3Int(-1, -1, 1),
			Direction3D.LeftUpForward => new Vector3Int(-1, 1, 1),
			Direction3D.RightUpBack => new Vector3Int(1, 1, -1),
			Direction3D.RightDownBack => new Vector3Int(1, -1, -1),
			Direction3D.LeftDownBack => new Vector3Int(-1, -1, -1),
			Direction3D.LeftUpBack => new Vector3Int(-1, 1, -1),

			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};

		public static Vector2 ToVector(this Direction2D dir) => dir switch
		{
			Direction2D.Up => new Vector2(0, 1),
			Direction2D.Down => new Vector2(0, -1),
			Direction2D.Right => new Vector2(1, 0),
			Direction2D.Left => new Vector2(-1, 0),
			Direction2D.RightUp => new Vector2(1, 1),
			Direction2D.RightDown => new Vector2(1, -1),
			Direction2D.LeftDown => new Vector2(-1, -1),
			Direction2D.LeftUp => new Vector2(-1, 1),
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		public static Vector2Int ToVectorInt(this Direction2D dir) => dir switch
		{
			Direction2D.Up => new Vector2Int(0, 1),
			Direction2D.Down => new Vector2Int(0, -1),
			Direction2D.Right => new Vector2Int(1, 0),
			Direction2D.Left => new Vector2Int(-1, 0),
			Direction2D.RightUp => new Vector2Int(1, 1),
			Direction2D.RightDown => new Vector2Int(1, -1),
			Direction2D.LeftDown => new Vector2Int(-1, -1),
			Direction2D.LeftUp => new Vector2Int(-1, 1),
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		public static Vector2Int ToVectorInt(this GeneralDirection2D dir) => dir switch
		{
			GeneralDirection2D.Up => Vector2Int.up,
			GeneralDirection2D.Down => Vector2Int.down,
			GeneralDirection2D.Right => Vector2Int.right,
			GeneralDirection2D.Left => Vector2Int.left,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)

		};

		public static Vector3Int ToVectorInt(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Up => Vector3Int.up,
			GeneralDirection3D.Down => Vector3Int.down,
			GeneralDirection3D.Right => Vector3Int.right,
			GeneralDirection3D.Left => Vector3Int.left,
			GeneralDirection3D.Forward => Vector3Int.forward,
			GeneralDirection3D.Back => Vector3Int.back,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		// To angle (Right = 0, Up = 90)
		public static int GetAngle(this GeneralDirection2D dir) => dir switch
		{
			GeneralDirection2D.Up => 90,
			GeneralDirection2D.Down => 270,
			GeneralDirection2D.Right => 0,
			GeneralDirection2D.Left => 180,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};

		public static int GetAngle(this Direction2D dir) => dir switch
		{
			Direction2D.Up => 90,
			Direction2D.Down => 270,
			Direction2D.Right => 0,
			Direction2D.Left => 180,
			Direction2D.RightUp => 45,
			Direction2D.RightDown => 315,
			Direction2D.LeftDown => 225,
			Direction2D.LeftUp => 135,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		// Opposite
		public static HorizontalDirection Opposite(this HorizontalDirection dir) => dir switch
		{
			HorizontalDirection.Right => HorizontalDirection.Left,
			_ => HorizontalDirection.Right
		};
		public static VerticalDirection Opposite(this VerticalDirection dir) => dir switch
		{
			VerticalDirection.Up => VerticalDirection.Down,
			_ => VerticalDirection.Down
		};

		public static GeneralDirection2D Opposite(this GeneralDirection2D dir) => dir switch
		{
			GeneralDirection2D.Up => GeneralDirection2D.Down,
			GeneralDirection2D.Down => GeneralDirection2D.Up,
			GeneralDirection2D.Right => GeneralDirection2D.Left,
			GeneralDirection2D.Left => GeneralDirection2D.Right,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};

		public static GeneralDirection3D Opposite(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Up => GeneralDirection3D.Down,
			GeneralDirection3D.Down => GeneralDirection3D.Up,
			GeneralDirection3D.Right => GeneralDirection3D.Left,
			GeneralDirection3D.Left => GeneralDirection3D.Right,
			GeneralDirection3D.Forward => GeneralDirection3D.Back,
			GeneralDirection3D.Back => GeneralDirection3D.Forward,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};

		public static Direction3D Opposite(this Direction3D dir) => dir switch
		{
			Direction3D.Up => Direction3D.Down,
			Direction3D.Down => Direction3D.Up,
			Direction3D.Right => Direction3D.Left,
			Direction3D.Left => Direction3D.Right,
			Direction3D.Forward => Direction3D.Back,
			Direction3D.Back => Direction3D.Forward,

			Direction3D.RightUp => Direction3D.LeftDown,
			Direction3D.RightDown => Direction3D.LeftUp,
			Direction3D.LeftDown => Direction3D.RightUp,
			Direction3D.LeftUp => Direction3D.RightDown,
			Direction3D.RightForward => Direction3D.LeftBack,
			Direction3D.RightBack => Direction3D.LeftForward,
			Direction3D.LeftBack => Direction3D.RightForward,
			Direction3D.LeftForward => Direction3D.RightBack,
			Direction3D.UpForward => Direction3D.DownBack,
			Direction3D.UpBack => Direction3D.DownForward,
			Direction3D.DownBack => Direction3D.UpForward,
			Direction3D.DownForward => Direction3D.UpBack,

			Direction3D.RightUpForward => Direction3D.LeftDownBack,
			Direction3D.RightUpBack => Direction3D.LeftDownForward,
			Direction3D.RightDownBack => Direction3D.LeftUpForward,
			Direction3D.RightDownForward => Direction3D.LeftUpBack,
			Direction3D.LeftDownBack => Direction3D.RightUpForward,
			Direction3D.LeftDownForward => Direction3D.RightUpBack,
			Direction3D.LeftUpForward => Direction3D.RightDownBack,
			Direction3D.LeftUpBack => Direction3D.RightDownForward,

			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};

		public static Direction2D Opposite(this Direction2D dir) => dir switch
		{
			Direction2D.Up => Direction2D.Down,
			Direction2D.Down => Direction2D.Up,
			Direction2D.Right => Direction2D.Left,
			Direction2D.Left => Direction2D.Right,

			Direction2D.RightUp => Direction2D.LeftDown,
			Direction2D.RightDown => Direction2D.LeftUp,
			Direction2D.LeftDown => Direction2D.RightUp,
			Direction2D.LeftUp => Direction2D.RightDown,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};

		// Right
		public static GeneralDirection2D Right(this GeneralDirection2D dir, int step) => (GeneralDirection2D)(MathHelper.Mod((int)dir + step, 4));

		public static Direction2D Right(this Direction2D dir, int step) => (Direction2D)(MathHelper.Mod((int)dir + step, 8));

		// Left
		public static GeneralDirection2D Left(this GeneralDirection2D dir, int step) => (GeneralDirection2D)(MathHelper.Mod((int)dir - step, 4));

		public static Direction2D Left(this Direction2D dir, int step) => (Direction2D)(MathHelper.Mod((int)dir - step, 8));


		// Main of Diagonal 
		public static bool IsMainDirection(this Direction2D dir) =>
			dir is Direction2D.Up or Direction2D.Down or Direction2D.Left or Direction2D.Right;

		public static bool IsDiagonal(this Direction2D dir) => !IsMainDirection(dir);

		// Vertical or Horizontal
		public static bool IsVertical(this Direction2D dir) => dir is Direction2D.Up or Direction2D.Down;


		public static bool IsHorizontal(this Direction2D dir) => dir is Direction2D.Left or Direction2D.Right;


		public static bool IsVertical(this GeneralDirection2D dir) => dir is GeneralDirection2D.Up or GeneralDirection2D.Down;


		public static bool IsHorizontal(this GeneralDirection2D dir) => dir is GeneralDirection2D.Left or GeneralDirection2D.Right;


		//IsPositive
		public static bool IsPositive(this GeneralDirection3D dir) =>
			dir is GeneralDirection3D.Right or GeneralDirection3D.Up or GeneralDirection3D.Forward;

		public static bool IsPositive(this GeneralDirection2D dir) =>
			dir is GeneralDirection2D.Right or GeneralDirection2D.Up;

		public static bool IsPositive(this Direction2D dir) =>
			dir is Direction2D.Right or Direction2D.Up;


		// GetAxis    
		public static Axis2D GetAxis(this GeneralDirection2D dir) => dir switch
		{
			GeneralDirection2D.Right or GeneralDirection2D.Left => Axis2D.Horizontal,
			GeneralDirection2D.Up or GeneralDirection2D.Down => Axis2D.Vertical,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		public static Axis3D GetAxis(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Right or GeneralDirection3D.Left => Axis3D.X,
			GeneralDirection3D.Up or GeneralDirection3D.Down => Axis3D.Y,
			GeneralDirection3D.Forward or GeneralDirection3D.Back => Axis3D.Z,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};


		public static GeneralDirection3D LeftHandedRotate(this GeneralDirection3D self, Axis3D axis, int step)
		{
			if (self.GetAxis() == axis)
				return self;
			step %= 4;
			if (step < 0)
				step += 4;

			if (step == 0)
				return self;
			if (step == 2)
				return self.Opposite();
			if (step == 3)
				self = self.Opposite();


			// One Step In Positive Direction:
			switch (axis)
			{
				case Axis3D.X:
					switch (self)
					{
						case GeneralDirection3D.Up:
							return GeneralDirection3D.Forward;
						case GeneralDirection3D.Forward:
							return GeneralDirection3D.Down;
						case GeneralDirection3D.Down:
							return GeneralDirection3D.Back;
						case GeneralDirection3D.Back:
							return GeneralDirection3D.Up;
					}

					break;
				case Axis3D.Y:
					switch (self)
					{
						case GeneralDirection3D.Right:
							return GeneralDirection3D.Back;
						case GeneralDirection3D.Back:
							return GeneralDirection3D.Left;
						case GeneralDirection3D.Left:
							return GeneralDirection3D.Forward;
						case GeneralDirection3D.Forward:
							return GeneralDirection3D.Right;
					}

					break;
				case Axis3D.Z:
					switch (self)
					{
						case GeneralDirection3D.Right:
							return GeneralDirection3D.Up;
						case GeneralDirection3D.Up:
							return GeneralDirection3D.Left;
						case GeneralDirection3D.Left:
							return GeneralDirection3D.Down;
						case GeneralDirection3D.Down:
							return GeneralDirection3D.Right;
					}

					break;
			}

			throw new Exception("Unreachable Code");
		}

		public static float GetLength(this Direction3D self) => self switch
		{
			Direction3D.Right or
			Direction3D.Left or
			Direction3D.Up or
			Direction3D.Down or
			Direction3D.Forward or
			Direction3D.Back => 1,

			Direction3D.RightUp or
			Direction3D.LeftUp or
			Direction3D.RightDown or
			Direction3D.LeftDown or
			Direction3D.UpForward or
			Direction3D.DownForward or
			Direction3D.UpBack or
			Direction3D.DownBack or
			Direction3D.RightForward or
			Direction3D.LeftForward or
			Direction3D.RightBack or
			Direction3D.LeftBack => diagonal2DLength,

			Direction3D.RightUpForward or
			Direction3D.LeftUpForward or
			Direction3D.RightDownForward or
			Direction3D.LeftDownForward or
			Direction3D.RightUpBack or
			Direction3D.LeftUpBack or
			Direction3D.RightDownBack or
			Direction3D.LeftDownBack => diagonal3DLength,

			_ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
		};

		public static float GetLength(this Direction2D self) => self switch
		{
			Direction2D.Right or
			Direction2D.Left or
			Direction2D.Up or
			Direction2D.Down or

			Direction2D.RightUp or
			Direction2D.LeftUp or
			Direction2D.RightDown or
			Direction2D.LeftDown => diagonal2DLength,

			_ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
		};


		public static GeneralDirection3D Transform(this GeneralDirection3D dir, Flip3D flip, Vector3Int rotation90)
		{
			Vector3Int vectorDir = dir.ToVectorInt();
			if (flip == Flip3D.X)
				vectorDir.x *= -1;
			else if (flip == Flip3D.Y)
				vectorDir.y *= -1;
			else if (flip == Flip3D.Z)
				vectorDir.z *= -1;

			int x = MathHelper.Mod(rotation90.x, 4);
			int y = MathHelper.Mod(rotation90.y, 4);
			int z = MathHelper.Mod(rotation90.z, 4);
			if (x == 1)
			{
				int temp = vectorDir.y;
				y = vectorDir.z;
				z = -temp;
			}
			else if (x == 2)
			{
				vectorDir.y *= -1;
				vectorDir.z *= -1;
			}
			else if (x == 3)
			{
				int temp = vectorDir.y;
				vectorDir.y = -vectorDir.z;
				vectorDir.z = temp;
			}

			if (y == 1)
			{
				int temp = vectorDir.x;
				vectorDir.x = vectorDir.z;
				vectorDir.z = -temp;
			}
			else if (y == 2)
			{
				vectorDir.x *= -1;
				vectorDir.z *= -1;
			}
			else if (y == 3)
			{
				int temp = vectorDir.x;
				vectorDir.x = -vectorDir.z;
				vectorDir.z = temp;
			}

			if (z == 1)
			{
				int temp = vectorDir.x;
				vectorDir.x = vectorDir.y;
				vectorDir.y = -temp;
			}
			else if (z == 2)
			{
				vectorDir.x *= -1;
				vectorDir.y *= -1;
			}
			else if (z == 3)
			{
				int temp = vectorDir.x;
				vectorDir.x = -vectorDir.y;
				vectorDir.y = temp;
			}

			if (vectorDir.x == 1)
				return GeneralDirection3D.Right;
			else if (vectorDir.x == -1)
				return GeneralDirection3D.Left;
			else if (vectorDir.y == 1)
				return GeneralDirection3D.Up;
			else if (vectorDir.y == -1)
				return GeneralDirection3D.Down;
			else if (vectorDir.z == 1)
				return GeneralDirection3D.Forward;
			else if (vectorDir.z == -1)
				return GeneralDirection3D.Back;

			throw new NotImplementedException();
		}

		public static GeneralDirection3D InverseTransform(this GeneralDirection3D dir, Flip3D flip, Vector3Int rotation90)
			=> dir.Transform(flip, -rotation90);

		public static Vector3 ToScale(this Flip3D flip) => flip switch
		{
			Flip3D.X => new Vector3(-1, 1, 1),
			Flip3D.Y => new Vector3(1, -1, 1),
			Flip3D.Z => new Vector3(1, 1, -1),
			_ => Vector3.one,
		};
		public static Matrix4x4 ToMatrix(Flip3D flip) => flip switch
		{
			Flip3D.X => new Matrix4x4(new Vector4(-1, 0, 0, -1), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1)),
			Flip3D.Y => new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1)),
			Flip3D.Z => new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, 0, 1)),
			_ => Matrix4x4.identity,

		};

	}
}