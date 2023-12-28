using System;
using UnityEngine;

namespace MUtility
{

	[Serializable]
	public struct CubicTransformation
	{
		// There is 48 possible transformations: 6 directions * 4 rotations * 2 flips

		// In Left handed coordinate system
		// If No Rotation:
		//	Thumb = Right
		//	Index = Up
		//	Middle = Forward
		// Clockwise rotation around Y axis

		public GeneralDirection3D upDirection;
		[Range(0, 3)] public int verticalRotation;
		public bool verticalFlip;

		public const int allTransformationCount = 48;

		public CubicTransformation(GeneralDirection3D upDirection, int verticalRotation, bool verticalFlip)
		{
			this.upDirection = upDirection;
			this.verticalRotation = verticalRotation % 4;
			this.verticalFlip = verticalFlip;
		}

		public CubicTransformation(byte cubicTransformationIndex)
		{
			verticalFlip = cubicTransformationIndex % 2 == 1;
			cubicTransformationIndex /= 2;
			verticalRotation = cubicTransformationIndex % 4;
			cubicTransformationIndex /= 4;
			upDirection = (GeneralDirection3D)cubicTransformationIndex;
		}

		public static CubicTransformation FromUpForward(GeneralDirection3D up, GeneralDirection3D forward, bool mirror = false)
		{
			Axis3D verticalAxis = up.GetAxis();
			Axis3D forwardAxis = forward.GetAxis();
			if (verticalAxis == forwardAxis)
				throw new ArgumentException($"Up and Forward direction must be perpendicular: {up} {forward}");

			GeneralDirection3D rightWithoutRotation = GetRightDirectionWithoutRotation(up);
			GeneralDirection3D forwardWithoutRotation = GetForwardDirectionWithoutRotation(up);

			// Calculate rotation
			int rotation;
			if (forward == forwardWithoutRotation)
				rotation = 0;
			else if (forward == rightWithoutRotation)
				rotation = 1;
			else if (forward == forwardWithoutRotation.Opposite())
				rotation = 2;
			else
				rotation = 3;

			if (mirror)
				up = up.Opposite();

			return new CubicTransformation(up, rotation, mirror);

		}

		public static CubicTransformation FromRightUp(GeneralDirection3D right, GeneralDirection3D up, bool mirror = false)
		{
			Axis3D verticalAxis = up.GetAxis();
			Axis3D rightAxis = right.GetAxis();
			if (verticalAxis == rightAxis)
				throw new ArgumentException("Right and Up direction must be perpendicular");

			GeneralDirection3D rightWithoutRotation = GetRightDirectionWithoutRotation(up);
			GeneralDirection3D forwardWithoutRotation = GetForwardDirectionWithoutRotation(up);

			int rotation;
			if (right == rightWithoutRotation)
				rotation = 0;
			else if (right == forwardWithoutRotation)
				rotation = 3;
			else if (right == rightWithoutRotation.Opposite())
				rotation = 2;
			else
				rotation = 1;

			if (mirror)
				up = up.Opposite();

			return new CubicTransformation(up, rotation, mirror);
		}

		public static CubicTransformation FromDirections(
			GeneralDirection3D right,
			GeneralDirection3D up,
			GeneralDirection3D forward)
		{
			CubicTransformation result = FromUpForward(up, forward);
			GeneralDirection3D resultRight = result.TransformDirection(GeneralDirection3D.Right);

			if (resultRight == right)
			{
				//Debug.Log($"Left Handed:  {right} {up} {forward}");
				return result;
			}
			if (resultRight == right.Opposite())
			{
				//Debug.Log($"Right Handed:  {right} {up} {forward}");
				result.verticalFlip = true;
				result.upDirection = up.Opposite();
				result.verticalRotation = (4 - result.verticalRotation) % 4;
				return result;
			}

			// Error:
			GeneralDirection3D resultUp = result.upDirection;
			GeneralDirection3D resultForward = result.TransformDirection(GeneralDirection3D.Forward);
			throw new ArgumentException(
				$"Directions are not valid: " +
				$"{right} {up} {forward} - " +
				$"{resultRight} {resultUp} {resultForward}");
		}


		static GeneralDirection3D GetForwardDirectionWithoutRotation(GeneralDirection3D upDirection) => upDirection switch
		{
			GeneralDirection3D.Up => GeneralDirection3D.Forward,
			GeneralDirection3D.Right => GeneralDirection3D.Up,
			GeneralDirection3D.Forward => GeneralDirection3D.Right,
			GeneralDirection3D.Down => GeneralDirection3D.Left,
			GeneralDirection3D.Left => GeneralDirection3D.Back,
			GeneralDirection3D.Back => GeneralDirection3D.Down,
			_ => GeneralDirection3D.Forward
		};

		static GeneralDirection3D GetRightDirectionWithoutRotation(GeneralDirection3D upDirection) => upDirection switch
		{
			GeneralDirection3D.Up => GeneralDirection3D.Right,
			GeneralDirection3D.Right => GeneralDirection3D.Forward,
			GeneralDirection3D.Forward => GeneralDirection3D.Up,
			GeneralDirection3D.Down => GeneralDirection3D.Back,
			GeneralDirection3D.Left => GeneralDirection3D.Down,
			GeneralDirection3D.Back => GeneralDirection3D.Left,
			_ => GeneralDirection3D.Forward
		};

		public byte GetIndex() => (byte)((int)upDirection * 8 + (verticalRotation % 4) * 2 + (verticalFlip ? 1 : 0));

		static readonly Matrix4x4 rightToLeftHanded =
			Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 0, 0), new(-1, -1, 1));

		public static readonly CubicTransformation identity = new(GeneralDirection3D.Up, 0, false);

		public Matrix4x4 GetTransformationMatrix(bool fromRightHanded = false)
		{
			Vector3Int upVector = upDirection.ToVectorInt();
			Vector3Int forwardVector = GetForwardDirectionWithoutRotation(upDirection).ToVectorInt();

			verticalRotation %= 4;
			Quaternion rotation = Quaternion.LookRotation(forwardVector, upVector);
			rotation *= Quaternion.Euler(0, verticalRotation * 90, 0);

			Vector3Int scale = verticalFlip ? new Vector3Int(1, -1, 1) : Vector3Int.one;

			Matrix4x4 result = Matrix4x4.TRS(Vector3.zero, rotation, scale);
			if (fromRightHanded)
				result *= rightToLeftHanded;
			return result;
		}

		/// <summary>
		/// Generate world direction from local direction
		/// </summary>
		/// <param name="localDir">Local Direction</param>
		/// <returns>World Direction</returns>
		public GeneralDirection3D TransformDirection(GeneralDirection3D localDir)
		{
			if (localDir == GeneralDirection3D.Up)
				return verticalFlip ? upDirection.Opposite() : upDirection;
			if (localDir == GeneralDirection3D.Down)
				return verticalFlip ? upDirection : upDirection.Opposite();

			GeneralDirection3D localForward = GetForwardDirectionWithoutRotation(upDirection);
			GeneralDirection3D localRight = GetRightDirectionWithoutRotation(upDirection);

			// Clockwise Rotation
			if (verticalRotation == 1)
			{
				GeneralDirection3D f = localForward;
				localForward = localRight;
				localRight = f.Opposite();

			}
			else if (verticalRotation == 2)
			{
				localForward = localForward.Opposite();
				localRight = localRight.Opposite();
			}
			else if (verticalRotation == 3)
			{
				GeneralDirection3D f = localForward;
				localForward = localRight.Opposite();
				localRight = f;
			}

			if (localDir == GeneralDirection3D.Forward)
				return localForward;
			if (localDir == GeneralDirection3D.Back)
				return localForward.Opposite();
			if (localDir == GeneralDirection3D.Right)
				return localRight;
			if (localDir == GeneralDirection3D.Left)
				return localRight.Opposite();

			return localDir;
		}


		/// <summary>
		/// Generate local direction from world direction
		/// </summary>
		/// <param name="worldDir">Global Direction</param>
		/// <returns>Local Direction</returns>
		public GeneralDirection3D InverseTransformDirection(GeneralDirection3D worldDir)
		{
			//Generate Inverse Transformation Function based on TransformDirection

			if (worldDir == upDirection)
				return verticalFlip ? GeneralDirection3D.Down : GeneralDirection3D.Up;
			if (worldDir == upDirection.Opposite())
				return verticalFlip ? GeneralDirection3D.Up : GeneralDirection3D.Down;

			GeneralDirection3D localForward = GetForwardDirectionWithoutRotation(upDirection);
			GeneralDirection3D localRight = GetRightDirectionWithoutRotation(upDirection);

			if (verticalRotation == 1)
			{
				GeneralDirection3D f = localForward;
				localForward = localRight;
				localRight = f.Opposite();
			}
			else if (verticalRotation == 2)
			{
				localForward = localForward.Opposite();
				localRight = localRight.Opposite();
			}
			else if (verticalRotation == 3)
			{
				GeneralDirection3D f = localForward;
				localForward = localRight.Opposite();
				localRight = f;
			}

			if (worldDir == localForward)
				return GeneralDirection3D.Forward;
			if (worldDir == localForward.Opposite())
				return GeneralDirection3D.Back;
			if (worldDir == localRight)
				return GeneralDirection3D.Right;
			if (worldDir == localRight.Opposite())
				return GeneralDirection3D.Left;

			return worldDir;
		}
	}
}