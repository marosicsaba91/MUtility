using System;
using UnityEngine;

namespace MUtility
{
	public enum Equator
	{
		East = 1,
		South = 3,
		West = 5,
		North = 7
	}

	public static class EquatorHelper
	{
		public static Equator FromAngle(float angle)
		{
			angle = MathHelper.Mod(angle, 360);

			if (angle >= 315 || angle < 45)
			{
				return Equator.North;
			}

			if (angle >= 45 && angle < 135)
			{
				return Equator.West;
			}

			if (angle >= 135 && angle < 225)
			{
				return Equator.South;
			}

			if (angle >= 225 && angle < 315)
			{
				return Equator.East;
			}

			return Equator.North;
		}

		public static int ToAngle(this Equator dir)
		{
			switch (dir)
			{
				case Equator.North:
					return 0;
				case Equator.West:
					return 90;
				case Equator.South:
					return 180;
				case Equator.East:
					return 270;
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		public static Vector2 ToVector2(this Equator dir)
		{
			switch (dir)
			{
				case Equator.North:
					return Vector2.up;
				case Equator.West:
					return Vector2.left;
				case Equator.South:
					return Vector2.right;
				case Equator.East:
					return Vector2.down;
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}

		public static Vector3 Rotate(this Equator dir, Vector3 inputVector)
		{
			switch (dir)
			{
				case Equator.North:
					return inputVector;
				case Equator.West:
					return new Vector3(inputVector.z, 0, -inputVector.x);
				case Equator.South:
					return -inputVector;
				case Equator.East:
					return new Vector3(-inputVector.z, 0, inputVector.x);
				default:
					throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
			}
		}
	}
}