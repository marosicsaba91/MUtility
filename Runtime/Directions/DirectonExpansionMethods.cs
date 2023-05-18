using System;
using UnityEngine;

namespace MUtility
{
	public static class DirectionExpanded
	{
		public static GeneralDirection3D GetPerpendicularRightHand(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Right => GeneralDirection3D.Down,
			GeneralDirection3D.Up => GeneralDirection3D.Back,
			GeneralDirection3D.Forward => GeneralDirection3D.Left,
			GeneralDirection3D.Left => GeneralDirection3D.Forward,
			GeneralDirection3D.Down => GeneralDirection3D.Right,
			GeneralDirection3D.Back => GeneralDirection3D.Up,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};


		//Perpendicular
		public static GeneralDirection3D GetPerpendicularLeftHand(this GeneralDirection3D dir) => dir switch
		{
			GeneralDirection3D.Right => GeneralDirection3D.Back,
			GeneralDirection3D.Up => GeneralDirection3D.Left,
			GeneralDirection3D.Forward => GeneralDirection3D.Down,
			GeneralDirection3D.Left => GeneralDirection3D.Up,
			GeneralDirection3D.Down => GeneralDirection3D.Forward,
			GeneralDirection3D.Back => GeneralDirection3D.Right,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
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
		public static bool Equals(this GeneralDirection2D self, GeneralDirection3D dir) =>
			(int)self == (int)dir;


		public static bool Equals(this GeneralDirection2D self, Direction2D dir) =>
			(int)self == (int)dir;


		public static bool Equals(this GeneralDirection3D self, GeneralDirection2D dir) =>
			(int)self == (int)dir;


		public static bool Equals(this GeneralDirection3D self, Direction2D dir) =>
			(int)self == (int)dir;


		public static bool Equals(this Direction2D self, GeneralDirection2D dir) =>
			(int)self == (int)dir;


		public static bool Equals(this Direction2D self, GeneralDirection3D dir) =>
			(int)self == (int)dir;

		public static bool Equals(this GeneralDirection3D self, Direction3D dir) =>
			(int)self == (int)dir;

		public static bool Equals(this GeneralDirection2D self, Direction3D dir) =>
			(int)self == (int)dir;

		public static bool Equals(this Direction2D self, Direction3D dir) =>
			(int)self == (int)dir;


		// Convert
		public static Direction2D ToDirection2D(this GeneralDirection2D self) =>
			(Direction2D)(int)self;

		public static GeneralDirection3D ToGeneralDirection3D(this GeneralDirection2D self) =>
			(GeneralDirection3D)(int)self;

		public static Direction3D ToDirection3D(this GeneralDirection2D self) =>
			(Direction3D)(int)self;


		public static Direction3D ToDirection3D(this GeneralDirection3D self) =>
			(Direction3D)(int)self;


		// To Vector Float
		public static Vector2 ToVector(this GeneralDirection2D dir)
		{
			switch (dir)
			{
				case GeneralDirection2D.Up:
					return new Vector2(0, 1);
				case GeneralDirection2D.Down:
					return new Vector2(0, -1);
				case GeneralDirection2D.Right:
					return new Vector2(1, 0);
				case GeneralDirection2D.Left:
					return new Vector2(-1, 0);
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		public static Vector3 ToVector(this GeneralDirection3D dir)
		{
			switch (dir)
			{
				case GeneralDirection3D.Up:
					return new Vector3(0, 1, 0);
				case GeneralDirection3D.Down:
					return new Vector3(0, -1, 0);
				case GeneralDirection3D.Right:
					return new Vector3(1, 0, 0);
				case GeneralDirection3D.Left:
					return new Vector3(-1, 0, 0);
				case GeneralDirection3D.Forward:
					return new Vector3(0, 0, 1);
				case GeneralDirection3D.Back:
					return new Vector3(0, 0, -1);
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		public static Vector3 ToVector(this Direction3D dir)
		{
			switch (dir)
			{
				case Direction3D.Up:
					return new Vector3(0, 1, 0);
				case Direction3D.Down:
					return new Vector3(0, -1, 0);
				case Direction3D.Right:
					return new Vector3(1, 0, 0);
				case Direction3D.Left:
					return new Vector3(-1, 0, 0);
				case Direction3D.Forward:
					return new Vector3(0, 0, 1);
				case Direction3D.Back:
					return new Vector3(0, 0, -1);
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		public static Vector2 ToVector(this Direction2D dir)
		{
			switch (dir)
			{
				case Direction2D.Up:
					return new Vector2(0, 1);
				case Direction2D.Down:
					return new Vector2(0, -1);
				case Direction2D.Right:
					return new Vector2(1, 0);
				case Direction2D.Left:
					return new Vector2(-1, 0);
				case Direction2D.UpRight:
					return new Vector2(1, 1);
				case Direction2D.DownRight:
					return new Vector2(1, -1);
				case Direction2D.DownLeft:
					return new Vector2(-1, -1);
				case Direction2D.UpLeft:
					return new Vector2(-1, 1);
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		// To Vector Int
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

		public static Vector2Int ToVectorInt(this Direction2D dir) => dir switch
		{
			Direction2D.Up => new Vector2Int(0, 1),
			Direction2D.Down => new Vector2Int(0, -1),
			Direction2D.Right => new Vector2Int(1, 0),
			Direction2D.Left => new Vector2Int(-1, 0),
			Direction2D.UpRight => new Vector2Int(1, 1),
			Direction2D.DownRight => new Vector2Int(1, -1),
			Direction2D.DownLeft => new Vector2Int(-1, -1),
			Direction2D.UpLeft => new Vector2Int(-1, 1),
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
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

		public static int GetAngle(this Direction2D dir)
		{
			switch (dir)
			{
				case Direction2D.Up:
					return 90;
				case Direction2D.Down:
					return 270;
				case Direction2D.Right:
					return 0;
				case Direction2D.Left:
					return 180;
				case Direction2D.UpRight:
					return 45;
				case Direction2D.DownRight:
					return 315;
				case Direction2D.DownLeft:
					return 225;
				case Direction2D.UpLeft:
					return 135;
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		// Opposite
		public static HorizontalDirection Opposite(this HorizontalDirection dir)
		{
			return dir == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left;
		}

		public static VerticalDirection Opposite(this VerticalDirection dir)
		{
			return dir == VerticalDirection.Down ? VerticalDirection.Up : VerticalDirection.Down;
		}

		public static GeneralDirection2D Opposite(this GeneralDirection2D dir)
		{
			return (GeneralDirection2D)(((int)dir + 4) % 8);
		}

		public static GeneralDirection3D Opposite(this GeneralDirection3D dir)
		{
			if ((int)dir < 8)
			{
				return (GeneralDirection3D)(((int)dir + 4) % 8);
			}
			else
			{
				return dir == GeneralDirection3D.Forward
					? GeneralDirection3D.Back
					: GeneralDirection3D.Forward;
			}
		}



		public static Direction2D Opposite(this Direction2D dir)
		{
			return (Direction2D)(((int)dir + 4) % 8);
		}

		// Right
		public static GeneralDirection2D Right(this GeneralDirection2D dir, int step)
		{
			return (GeneralDirection2D)((int)(dir + step * 2) % 8);
		}

		public static Direction2D Right(this Direction2D dir, int step)
		{
			return (Direction2D)((int)(dir + step) % 8);
		}

		// Left
		public static GeneralDirection2D Left(this GeneralDirection2D dir, int step)
		{
			int n = (int)(dir - step * 2) % 8;
			if (n < 0)
				n = 8 + n;
			return (GeneralDirection2D)n;
		}

		public static Direction2D Left(this Direction2D dir, int step)
		{
			int n = (int)(dir - step) % 8;
			if (n < 0)
				n = 8 + n;
			return (Direction2D)n;
		}

		// Main of Diagonal 
		public static bool IsMainDirection(this Direction2D dir)
		{
			return dir == Direction2D.Up || dir == Direction2D.Down || dir == Direction2D.Left ||
				   dir == Direction2D.Right;
		}

		public static bool IsDiagonal(this Direction2D dir)
		{
			return !IsMainDirection(dir);
		}

		// Vertical or Horizontal
		public static bool IsVertical(this Direction2D dir) =>
			dir == Direction2D.Up || dir == Direction2D.Down;


		public static bool IsHorizontal(this Direction2D dir) =>
			dir == Direction2D.Left || dir == Direction2D.Right;


		public static bool IsVertical(this GeneralDirection2D dir) =>
			dir == GeneralDirection2D.Up || dir == GeneralDirection2D.Down;


		public static bool IsHorizontal(this GeneralDirection2D dir) =>
			dir == GeneralDirection2D.Left || dir == GeneralDirection2D.Right;


		//IsPositive
		public static bool IsPositive(this GeneralDirection3D dir) =>
			dir == GeneralDirection3D.Right || dir == GeneralDirection3D.Up || dir == GeneralDirection3D.Forward;

		public static bool IsPositive(this GeneralDirection2D dir) =>
			dir == GeneralDirection2D.Right || dir == GeneralDirection2D.Up;

		public static bool IsPositive(this Direction2D dir) =>
			dir == Direction2D.Right || dir == Direction2D.Up;


		// GetAxis    
		public static Axis2D GetAxis(this GeneralDirection2D dir)
		{
			switch (dir)
			{
				case GeneralDirection2D.Right:
				case GeneralDirection2D.Left:
					return Axis2D.Horizontal;
				case GeneralDirection2D.Up:
				case GeneralDirection2D.Down:
					return Axis2D.Vertical;
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		public static Axis3D GetAxis(this GeneralDirection3D dir)
		{
			switch (dir)
			{
				case GeneralDirection3D.Right:
				case GeneralDirection3D.Left:
					return Axis3D.X;
				case GeneralDirection3D.Up:
				case GeneralDirection3D.Down:
					return Axis3D.Y;
				case GeneralDirection3D.Forward:
				case GeneralDirection3D.Back:
					return Axis3D.Z;
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}


		public static GeneralDirection3D LeftHandedRotate(this GeneralDirection3D self, Axis3D axis, int step)
		{
			if (self.GetAxis() == axis)
				return self;
			step = step % 4;
			if (step < 0)
			{
				step += 4;
			}

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
	}
}