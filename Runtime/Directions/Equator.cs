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
			angle = MathHelper.ModuloPositive(angle, 360);

			if (angle is >= 315 or < 45)
				return Equator.North;

			if (angle is >= 45 and < 135)
				return Equator.West;

			if (angle is >= 135 and < 225)
				return Equator.South;

			if (angle is >= 225 and < 315)
				return Equator.East;

			return Equator.North;
		}

		public static int ToAngle(this Equator dir) => dir switch
		{
			Equator.North => 0,
			Equator.West => 90,
			Equator.South => 180,
			Equator.East => 270,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		public static Vector2 ToVector2(this Equator dir) => dir switch
		{
			Equator.North => Vector2.up,
			Equator.West => Vector2.left,
			Equator.South => Vector2.right,
			Equator.East => Vector2.down,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};

		public static Vector3 Rotate(this Equator dir, Vector3 inputVector) => dir switch
		{
			Equator.North => inputVector,
			Equator.West => new Vector3(inputVector.z, 0, -inputVector.x),
			Equator.South => -inputVector,
			Equator.East => new Vector3(-inputVector.z, 0, inputVector.x),
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
		};
	}
}