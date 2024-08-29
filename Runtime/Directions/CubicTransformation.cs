using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct CubicTransformation
	{
		// There is 48 possible transformations: 6 directions * 4 rotations * 2 (mirrored or not)

		// In Left handed coordinate system
		// If No Rotation:
		//	Thumb = Right
		//	Index = Up
		//	Middle = Forward
		// Clockwise rotation around Y axis

		public GeneralDirection3D upDirection;
		[Range(0, 3)] public int verticalRotation;
		public bool isVerticalFlipped;

		public const int allTransformationCount = 48;

		public CubicTransformation(GeneralDirection3D upDirection, int verticalRotation, bool isVerticalFlipped)
		{
			this.upDirection = upDirection;
			this.verticalRotation = verticalRotation % 4;
			this.isVerticalFlipped = isVerticalFlipped;
		}

		public CubicTransformation(byte cubicTransformationIndex)
		{
			isVerticalFlipped = cubicTransformationIndex % 2 == 1;
			cubicTransformationIndex /= 2;
			verticalRotation = cubicTransformationIndex % 4;
			cubicTransformationIndex /= 4;
			upDirection = (GeneralDirection3D)(cubicTransformationIndex % 6);
		}

		public static CubicTransformation FromUpForward(GeneralDirection3D up, GeneralDirection3D forward, bool mirror = false)
		{
			if (mirror)
				up = up.Opposite();

			GeneralDirection3D rightWithoutRotation = GetRightDirectionWithoutRotation(up);
			GeneralDirection3D forwardWithoutRotation = GetForwardDirectionWithoutRotation(up);

			int rotation;
			if (forward == forwardWithoutRotation)
				rotation = 0;
			else if (forward == rightWithoutRotation)
				rotation = 1;
			else if (forward == forwardWithoutRotation.Opposite())
				rotation = 2;
			else
				rotation = 3;

			return new CubicTransformation(up, rotation, mirror);
		}

		public static CubicTransformation FromRightUp(GeneralDirection3D right, GeneralDirection3D up, bool mirror = false)
		{
			if (mirror)
				up = up.Opposite();

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

			return new CubicTransformation(up, rotation, mirror);
		}

		public static CubicTransformation FromDirections(GeneralDirection3D right, GeneralDirection3D up, GeneralDirection3D forward)
			=> FromUpForward(up, forward, !DirectionUtility.IsLeftHanded(right, up, forward));

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

		public byte ToByte() => (byte)((int)upDirection * 8 + (verticalRotation % 4) * 2 + (isVerticalFlipped ? 1 : 0));

		public static readonly CubicTransformation identity = new(GeneralDirection3D.Up, 0, false);

		static readonly Matrix4x4 rightToLeftHanded = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 0, 0), new(-1, -1, 1));

		public Matrix4x4 GetTransformationMatrix(bool fromRightHanded = false)
		{
			Vector3Int upVector = upDirection.ToVectorInt();
			Vector3Int forwardVector = GetForwardDirectionWithoutRotation(upDirection).ToVectorInt();

			verticalRotation %= 4;
			Quaternion rotation = Quaternion.LookRotation(forwardVector, upVector);
			rotation *= Quaternion.Euler(0, verticalRotation * 90, 0);

			Vector3Int scale = isVerticalFlipped ? new Vector3Int(1, -1, 1) : Vector3Int.one;

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
				return isVerticalFlipped ? upDirection.Opposite() : upDirection;
			if (localDir == GeneralDirection3D.Down)
				return isVerticalFlipped ? upDirection : upDirection.Opposite();

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

		public GeneralDirection3D Up => isVerticalFlipped ? upDirection.Opposite() : upDirection;
		public GeneralDirection3D Down => isVerticalFlipped ? upDirection : upDirection.Opposite();
		public GeneralDirection3D Right => TransformDirection(GeneralDirection3D.Right);
		public GeneralDirection3D Left => TransformDirection(GeneralDirection3D.Left);
		public GeneralDirection3D Forward => TransformDirection(GeneralDirection3D.Forward);
		public GeneralDirection3D Back => TransformDirection(GeneralDirection3D.Back);


		public Vector3 Vector_R => Right.ToVector();
		public Vector3 Vector_L => Left.ToVector();
		public Vector3 Vector_U => Up.ToVector();
		public Vector3 Vector_D => Down.ToVector();
		public Vector3 Vector_F => Forward.ToVector();
		public Vector3 Vector_B => Back.ToVector();


		public Vector3 Vector_RU => Up.ToVector() + Right.ToVector();
		public Vector3 Vector_RD => Down.ToVector() + Right.ToVector();
		public Vector3 Vector_RF => Right.ToVector() + Forward.ToVector();
		public Vector3 Vector_RB => Right.ToVector() + Back.ToVector();
		public Vector3 Vector_LU => Up.ToVector() + Left.ToVector();
		public Vector3 Vector_LD => Down.ToVector() + Left.ToVector();
		public Vector3 Vector_LF => Left.ToVector() + Forward.ToVector();
		public Vector3 Vector_LB => Left.ToVector() + Back.ToVector();
		public Vector3 Vector_UF => Up.ToVector() + Forward.ToVector();
		public Vector3 Vector_UB => Up.ToVector() + Back.ToVector();
		public Vector3 Vector_DF => Down.ToVector() + Forward.ToVector();
		public Vector3 Vector_DB => Down.ToVector() + Back.ToVector();


		public Vector3 Vector_RUF => Right.ToVector() + Up.ToVector() + Forward.ToVector();
		public Vector3 Vector_RUB => Right.ToVector() + Up.ToVector() + Back.ToVector();
		public Vector3 Vector_RDF => Right.ToVector() + Down.ToVector() + Forward.ToVector();
		public Vector3 Vector_RDB => Right.ToVector() + Down.ToVector() + Back.ToVector();
		public Vector3 Vector_LUF => Left.ToVector() + Up.ToVector() + Forward.ToVector();
		public Vector3 Vector_LUB => Left.ToVector() + Up.ToVector() + Back.ToVector();
		public Vector3 Vector_LDF => Left.ToVector() + Down.ToVector() + Forward.ToVector();
		public Vector3 Vector_LDB => Left.ToVector() + Down.ToVector() + Back.ToVector();



		/// <summary>
		/// Generate local direction from world direction
		/// </summary>
		/// <param name="worldDir">Global Direction</param>
		/// <returns>Local Direction</returns>
		public GeneralDirection3D InverseTransformDirection(GeneralDirection3D worldDir)
		{
			//Generate Inverse Transformation Function based on TransformDirection

			if (worldDir == upDirection)
				return isVerticalFlipped ? GeneralDirection3D.Down : GeneralDirection3D.Up;
			if (worldDir == upDirection.Opposite())
				return isVerticalFlipped ? GeneralDirection3D.Up : GeneralDirection3D.Down;

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
		public static bool operator ==(CubicTransformation a, CubicTransformation b) => a.upDirection == b.upDirection && a.verticalRotation == b.verticalRotation && a.isVerticalFlipped == b.isVerticalFlipped;
		public static bool operator !=(CubicTransformation a, CubicTransformation b) => !(a == b);
		public override bool Equals(object obj) => obj is CubicTransformation other && this == other;
		public override int GetHashCode() => upDirection.GetHashCode() ^ verticalRotation.GetHashCode() ^ isVerticalFlipped.GetHashCode();

		public override string ToString() => $"Up: {upDirection},   Vertical Flip: {isVerticalFlipped},   Rotation: {verticalRotation}";

		public void Mirror(Axis3D axis)
		{
			isVerticalFlipped = !isVerticalFlipped;
			Axis3D upAxis = upDirection.GetAxis();
			if (axis == upAxis) return;

			bool upPositive = upDirection.IsPositive() ^ !isVerticalFlipped;
			int rotation = (axis.Next() == upAxis) ^ upPositive ^ isVerticalFlipped ? 3 : 1;

			verticalRotation = (verticalRotation + rotation) % 4;
			upDirection = upDirection.Opposite();
		}

		public void Turn(Axis3D axis, int leftHandedTurnCount)
		{
			leftHandedTurnCount %= 4;
			if (leftHandedTurnCount == 0) return;

			if (leftHandedTurnCount < 0)
				leftHandedTurnCount += 4;

			if (leftHandedTurnCount == 1)
				TurnPositiveLeftHanded(axis);
			else if (leftHandedTurnCount == 2)
				TurnHalf(axis);
			else if (leftHandedTurnCount == 3)
				TurnNegativeLeftHanded(axis);
		}

		public void TurnHalf(Axis3D axis)
		{
			Axis3D upAxis = upDirection.GetAxis();
			if (axis == upAxis)
			{
				verticalRotation = (verticalRotation + 2) % 4;
			}
			else if (axis.Next() == upAxis)
			{
				upDirection = upDirection.Opposite();
				int turn = upDirection.IsPositive() ? 3 : 1;
				verticalRotation = (verticalRotation + turn) % 4;
			}
			else
			{
				upDirection = upDirection.Opposite();
				int turn = upDirection.IsPositive() ? 1 : 3;
				verticalRotation = (verticalRotation + turn) % 4;
			}
		}

		public void Turn(Axis3D axis, bool leftHandedPositive)
		{
			if (leftHandedPositive)
				TurnPositiveLeftHanded(axis);
			else
				TurnNegativeLeftHanded(axis);
		}

		public void TurnPositiveLeftHanded(Axis3D axis)
		{
			Axis3D upAxis = upDirection.GetAxis();
			if (axis == upAxis)
			{
				int rotations = axis.Next() == upAxis ^ upDirection.IsPositive() ? 1 : 3;
				verticalRotation = (verticalRotation + rotations) % 4;
			}
			else if (axis.Next() == upAxis)
			{
				upDirection = upDirection.GetPerpendicularNext();
				int turn = upDirection.IsPositive() ? 3 : 1;
				verticalRotation = (verticalRotation + turn) % 4;
			}
			else
			{
				upDirection = upDirection.GetPerpendicularPrevious().Opposite();
				verticalRotation = (verticalRotation + 2) % 4;
			}
		}
		public void TurnNegativeLeftHanded(Axis3D axis)
		{
			Axis3D upAxis = upDirection.GetAxis();
			if (axis == upAxis)
			{
				int rotations = axis.Next() == upAxis ^ upDirection.IsPositive() ? 3 : 1;
				verticalRotation = (verticalRotation + rotations) % 4;
			}
			else if (axis.Next() == upAxis)
			{
				upDirection = upDirection.GetPerpendicularNext().Opposite();
				int turn = 2;
				verticalRotation = (verticalRotation + turn) % 4;
			}
			else
			{
				upDirection = upDirection.GetPerpendicularPrevious();
				int turn = upDirection.IsPositive() ? 1 : 3;
				verticalRotation = (verticalRotation + turn) % 4;
			}
		}


	}
}