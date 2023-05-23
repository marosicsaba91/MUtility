using System;
using UnityEngine;

namespace MUtility
{
	public static class VectorExtensions
	{
		public static Vector2 Round(this Vector2 vector) =>
			new(Mathf.Round(vector.x), Mathf.Round(vector.y));

		public static Vector3 Round(this Vector3 vector) =>
			new(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));


		public static Vector2Int RoundToInt(this Vector2 vector) =>
			new(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));

		public static Vector3Int RoundToInt(this Vector3 vector) =>
			new(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));


		public static Vector2 ToVector2(this Vector3 vector, Axis3D deleteAxis) => deleteAxis switch
		{
			Axis3D.X => new Vector2(vector.y, vector.z),
			Axis3D.Y => new Vector2(vector.x, vector.z),
			Axis3D.Z => new Vector2(vector.x, vector.y),
			_ => throw new ArgumentOutOfRangeException(nameof(deleteAxis), deleteAxis, null),
		};

		public static Vector3 ToVector3(this Vector2 vector, float newValue, Axis3D newAxis) => newAxis switch
		{
			Axis3D.X => new Vector3(newValue, vector.x, vector.y),
			Axis3D.Y => new Vector3(vector.x, newValue, vector.y),
			Axis3D.Z => new Vector3(vector.x, vector.y, newValue),
			_ => throw new ArgumentOutOfRangeException(nameof(newAxis), newAxis, null),
		};

		public static float GetAngle(this Vector2 original)
		{
			float angle = Vector2.Angle(Vector2.right, original);
			return original.y > 0 ? angle : 360f - angle;
		}

		public static float GetAngle(this Vector3 original) =>
			((Vector2)original).GetAngle();


		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
			float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

			float tx = v.x;
			float ty = v.y;
			v.x = cos * tx - sin * ty;
			v.y = sin * tx + cos * ty;
			return v;
		}


		// Clamp vector
		public static Vector2 Clamp(this Vector2 input, float min, float max) => new(
				input.x < min ? min : input.x > max ? max : input.x,
				input.y < min ? min : input.y > max ? max : input.y);

		public static Vector2 Clamp(this Vector2 input, Vector2 min, Vector2 max) => new(
				input.x < min.x ? min.x : input.x > max.x ? max.x : input.x,
				input.y < min.y ? min.y : input.y > max.y ? max.y : input.y);

		public static Vector3 Clamp(this Vector3 input, float min, float max) => new(
				input.x < min ? min : input.x > max ? max : input.x,
				input.y < min ? min : input.y > max ? max : input.y,
				input.z < min ? min : input.z > max ? max : input.z);

		public static Vector3 Clamp(this Vector3 input, Vector3 min, Vector3 max) => new(
				input.x < min.x ? min.x : input.x > max.x ? max.x : input.x,
				input.y < min.y ? min.y : input.y > max.y ? max.y : input.y,
				input.z < min.z ? min.z : input.z > max.y ? max.z : input.z);

		public static Vector3Int Clamp(this Vector3Int input, Vector3Int min, Vector3Int max) => new(
				input.x < min.x ? min.x : input.x > max.x ? max.x : input.x,
				input.y < min.y ? min.y : input.y > max.y ? max.y : input.y,
				input.z < min.z ? min.z : input.z > max.y ? max.z : input.z);
		public static Vector2Int Clamp(this Vector2Int input, Vector2Int min, Vector2Int max) => new(
				input.x < min.x ? min.x : input.x > max.x ? max.x : input.x,
				input.y < min.y ? min.y : input.y > max.y ? max.y : input.y);

		public static Vector3Int Clamp(this Vector3Int input, int min, int max) => new(
				input.x < min ? min : input.x > max ? max : input.x,
				input.y < min ? min : input.y > max ? max : input.y,
				input.z < min ? min : input.z > max ? max : input.z);
		public static Vector2Int Clamp(this Vector2Int input, int min, int max) => new(
				input.x < min ? min : input.x > max ? max : input.x,
				input.y < min ? min : input.y > max ? max : input.y);

		public static Vector2 Clamp01(this Vector2 areaPos) => areaPos.Clamp(0, 1);

		public static Vector3 Clamp01(this Vector3 areaPos) => areaPos.Clamp(0, 1);

		public static void LeftHand(Vector3 thumb, Vector3 index, out Vector3 middle) =>
			middle = Vector3.Cross(thumb, index).normalized;

		public static void LeftHand(Vector3 thumb, out Vector3 index, Vector3 middle) =>
			index = Vector3.Cross(middle, thumb).normalized;

		public static void LeftHand(out Vector3 thumb, Vector3 index, Vector3 middle) =>
			thumb = Vector3.Cross(index, middle).normalized;

		public static void LeftHand(Vector3 thumb, out Vector3 index, out Vector3 middle)
		{
			thumb.Normalize();
			index = thumb != Vector3.forward && thumb != Vector3.back
				? Vector3.Cross(Vector3.forward, thumb).normalized
				: Vector3.Cross(new Vector3(-.001f, -.001f, 1), thumb).normalized;

			LeftHand(thumb, index, out middle);
		}

		public static void LeftHand(out Vector3 thumb, Vector3 index, out Vector3 middle)
		{
			index.Normalize();
			middle = index != Vector3.forward && index != Vector3.back
				? Vector3.Cross(Vector3.forward, index).normalized
				: Vector3.Cross(new Vector3(-.001f, -.001f, 1), index).normalized;

			LeftHand(out thumb, index, middle);
		}

		public static void LeftHand(out Vector3 thumb, out Vector3 index, Vector3 middle)
		{
			middle.Normalize();
			thumb = middle != Vector3.forward && middle != Vector3.back
				? Vector3.Cross(Vector3.forward, middle).normalized
				: Vector3.Cross(new Vector3(-.001f, -.001f, 1), middle).normalized;

			LeftHand(thumb, out index, middle);
		}





		public static Vector2 MultiplyAllAxis(this Vector2 s, Vector2 v) => new(s.x * v.x, s.y * v.y);
		public static Vector2 MultiplyAllAxis(this Vector2 s, Vector2Int v) => new(s.x * v.x, s.y * v.y);
		public static Vector2 MultiplyAllAxis(this Vector2 s, float x, float y) => new(s.x * x, s.y * y);


		public static Vector2 MultiplyAllAxis(this Vector2Int s, Vector2 v) => new(s.x * v.x, s.y * v.y);
		public static Vector2Int MultiplyAllAxis(this Vector2Int s, Vector2Int v) => new(s.x * v.x, s.y * v.y);
		public static Vector2Int MultiplyAllAxis(this Vector2Int s, int x, int y) => new(s.x * x, s.y * y);
		public static Vector2 MultiplyAllAxis(this Vector2Int s, float x, float y) => new(s.x * x, s.y * y);



		public static Vector3 MultiplyAllAxis(this Vector3 s, Vector3 v) => new(s.x * v.x, s.y * v.y, s.z * v.z);
		public static Vector3 MultiplyAllAxis(this Vector3 s, Vector3Int v) => new(s.x * v.x, s.y * v.y, s.z * v.z);
		public static Vector3 MultiplyAllAxis(this Vector3 s, float x, float y, float z) => new(s.x * x, s.y * y, s.z * z);

		public static Vector3 MultiplyAllAxis(this Vector3Int s, Vector3 v) => new(s.x * v.x, s.y * v.y, s.z * v.z);
		public static Vector3Int MultiplyAllAxis(this Vector3Int s, Vector3Int v) => new(s.x * v.x, s.y * v.y, s.z * v.z);
		public static Vector3Int MultiplyAllAxis(this Vector3Int s, int x, int y, int z) => new(s.x * x, s.y * y, s.z * z);
		public static Vector3 MultiplyAllAxis(this Vector3Int s, float x, float y, float z) => new(s.x * x, s.y * y, s.z * z);
		 
		public static Vector3Int Rotate(this Vector3Int vector, Axis3D axis, int rotations90)
		{
			rotations90 = (rotations90 % 4 + 4) % 4;
			if (rotations90 == 0)
				return vector;

			int x = vector.x;
			int y = vector.y;
			int z = vector.z;

			switch (axis)
			{
				case Axis3D.X:
					switch (rotations90)
					{
						case 1:
							vector.y = -z;
							vector.z = y;
							break;
						case 2:
							vector.y = -y;
							vector.z = -z;
							break;
						case 3:
							vector.y = z;
							vector.z = -y;
							break;
					}
					break;
				case Axis3D.Y:
					switch (rotations90)
					{
						case 1:
							vector.z = -x;
							vector.x = z;
							break;
						case 2:
							vector.z = -z;
							vector.x = -x;
							break;
						case 3:
							vector.z = x;
							vector.x = -z;
							break;
					}
					break;
				case Axis3D.Z:
					switch (rotations90)
					{
						case 1:
							vector.x = -y;
							vector.y = x;
							break;
						case 2:
							vector.x = -x;
							vector.y = -y;
							break;
						case 3:
							vector.x = y;
							vector.y = -x;
							break;
					}
					break;
			}

			return vector;
		}

		public static Vector3Int Abs(this Vector3Int vector)
		{
			vector.x = Mathf.Abs(vector.x);
			vector.y = Mathf.Abs(vector.y);
			vector.z = Mathf.Abs(vector.z);
			return vector;
		}

		public static float Mean(this Vector2 vector) => (vector.x + vector.y) / 2f;
		public static float AbsMean(this Vector2 vector) =>
			(Mathf.Abs(vector.x) + Mathf.Abs(vector.y)) / 2f;

		public static float Mean(this Vector3 vector) => (vector.x + vector.y + vector.z) / 3f;
		public static float AbsMean(this Vector3 vector) =>
			(Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z)) / 3f;

		public static float Mean(this Vector2Int vector) => (vector.x + vector.y) / 2f;
		public static float AbsMean(this Vector2Int vector) =>
			(Mathf.Abs(vector.x) + Mathf.Abs(vector.y)) / 2f;

		public static float Mean(this Vector3Int vector) => (vector.x + vector.y + vector.z) / 3f;
		public static float AbsMean(this Vector3Int vector) =>
			(Mathf.Abs(vector.x) + Mathf.Abs(vector.y) + Mathf.Abs(vector.z)) / 3f;

		public static int GetAxis(this Vector3Int vector, Axis3D axis) => axis switch
		{
			Axis3D.X => vector.x,
			Axis3D.Y => vector.y,
			Axis3D.Z => vector.z,
			_ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
		};

		public static Vector3 GetPerpendicular(this Vector3 dir)
		{
			dir.Normalize();
			if(dir == Vector3.up || dir == Vector3Int.down)
				return Vector3.Cross(Vector3.right, dir);
			return Vector3.Cross(Vector3.up, dir);
		}

		public static void SetAxis(this ref Vector3Int v, Axis3D axis, int value)
		{
			if (axis == Axis3D.X)
				v.x = value;
			else if (axis == Axis3D.Y)
				v.y = value;
			else if (axis == Axis3D.Z)
				v.z = value;
			else
				throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
		}

		public static void SetAxis(this ref Vector3 v, Axis3D axis, float value)
		{
			if (axis == Axis3D.X)
				v.x = value;
			else if (axis == Axis3D.Y)
				v.y = value;
			else if (axis == Axis3D.Z)
				v.z = value;
			else
				throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
		}

		public static void SetAxis(this ref Vector2Int v, Axis2D axis, int value)
		{
			if (axis == Axis2D.Horizontal)
				v.x = value;
			else if (axis == Axis2D.Vertical)
				v.y = value;
			else
				throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
		}

		public static void SetAxis(this ref Vector2 v, Axis2D axis, float value)
		{
			if (axis == Axis2D.Horizontal)
				v.x = value;
			else if (axis == Axis2D.Vertical)
				v.y = value; 
			else
				throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
		}
	}
}