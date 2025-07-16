using NUnit.Framework;
using UnityEngine;

namespace MUtility.Test
{
	public class CubicTransformationTests
	{
		[Test]
		public void Aaa_DummyTestMethod()
		{
			Assert.IsTrue(true);
		}

		[Test]
		public void IdentityTest()
		{
			CubicTransformation ct = new(GeneralDirection3D.Up, 0, false);

			Assert.AreEqual(CubicTransformation.identity, ct);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Down);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Left);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Forward);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Back);

			Assert.AreEqual(GeneralDirection3D.Up, ct.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Down, ct.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Right, ct.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Left, ct.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Forward, ct.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Back, ct.InverseTransformDirection(GeneralDirection3D.Back));
		}

		[Test]
		public void TransformDirectionTests()
		{
			CubicTransformation ct1 = new(GeneralDirection3D.Right, 0, false);
			Assert.AreEqual(GeneralDirection3D.Right, ct1.Up);
			Assert.AreEqual(GeneralDirection3D.Left, ct1.Down);
			Assert.AreEqual(GeneralDirection3D.Forward, ct1.Right);
			Assert.AreEqual(GeneralDirection3D.Back, ct1.Left);
			Assert.AreEqual(GeneralDirection3D.Up, ct1.Forward);
			Assert.AreEqual(GeneralDirection3D.Down, ct1.Back);


			CubicTransformation ct2 = new(GeneralDirection3D.Right, 2, false);
			Assert.AreEqual(GeneralDirection3D.Right, ct2.Up);
			Assert.AreEqual(GeneralDirection3D.Left, ct2.Down);
			Assert.AreEqual(GeneralDirection3D.Back, ct2.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct2.Left);
			Assert.AreEqual(GeneralDirection3D.Down, ct2.Forward);
			Assert.AreEqual(GeneralDirection3D.Up, ct2.Back);

			CubicTransformation ct3 = new(GeneralDirection3D.Back, 0, false);
			Assert.AreEqual(GeneralDirection3D.Back, ct3.Up);
			Assert.AreEqual(GeneralDirection3D.Forward, ct3.Down);
			Assert.AreEqual(GeneralDirection3D.Left, ct3.Right);
			Assert.AreEqual(GeneralDirection3D.Right, ct3.Left);
			Assert.AreEqual(GeneralDirection3D.Down, ct3.Forward);
			Assert.AreEqual(GeneralDirection3D.Up, ct3.Back);

			CubicTransformation ct4 = new(GeneralDirection3D.Back, 3, false);
			Assert.AreEqual(GeneralDirection3D.Back, ct4.Up);
			Assert.AreEqual(GeneralDirection3D.Forward, ct4.Down);
			Assert.AreEqual(GeneralDirection3D.Down, ct4.Right);
			Assert.AreEqual(GeneralDirection3D.Up, ct4.Left);
			Assert.AreEqual(GeneralDirection3D.Right, ct4.Forward);
			Assert.AreEqual(GeneralDirection3D.Left, ct4.Back);

			// Mirrored

			CubicTransformation ct5 = new(GeneralDirection3D.Right, 0, true);
			Assert.AreEqual(GeneralDirection3D.Left, ct5.Up);
			Assert.AreEqual(GeneralDirection3D.Right, ct5.Down);
			Assert.AreEqual(GeneralDirection3D.Forward, ct5.Right);
			Assert.AreEqual(GeneralDirection3D.Back, ct5.Left);
			Assert.AreEqual(GeneralDirection3D.Up, ct5.Forward);
			Assert.AreEqual(GeneralDirection3D.Down, ct5.Back);

			CubicTransformation ct6 = new(GeneralDirection3D.Right, 2, true);
			Assert.AreEqual(GeneralDirection3D.Left, ct6.Up);
			Assert.AreEqual(GeneralDirection3D.Right, ct6.Down);
			Assert.AreEqual(GeneralDirection3D.Back, ct6.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct6.Left);
			Assert.AreEqual(GeneralDirection3D.Down, ct6.Forward);
			Assert.AreEqual(GeneralDirection3D.Up, ct6.Back);

			CubicTransformation ct7 = new(GeneralDirection3D.Back, 0, true);
			Assert.AreEqual(GeneralDirection3D.Forward, ct7.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct7.Down);
			Assert.AreEqual(GeneralDirection3D.Left, ct7.Right);
			Assert.AreEqual(GeneralDirection3D.Right, ct7.Left);
			Assert.AreEqual(GeneralDirection3D.Down, ct7.Forward);
			Assert.AreEqual(GeneralDirection3D.Up, ct7.Back);

			CubicTransformation ct8 = new(GeneralDirection3D.Back, 3, true);
			Assert.AreEqual(GeneralDirection3D.Forward, ct8.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct8.Down);
			Assert.AreEqual(GeneralDirection3D.Down, ct8.Right);
			Assert.AreEqual(GeneralDirection3D.Up, ct8.Left);
			Assert.AreEqual(GeneralDirection3D.Right, ct8.Forward);
			Assert.AreEqual(GeneralDirection3D.Left, ct8.Back);
		}

		[Test]
		public void InverseTransformTests()
		{
			CubicTransformation ct1 = new(GeneralDirection3D.Right, 0, false);
			Assert.AreEqual(GeneralDirection3D.Forward, ct1.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Back, ct1.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Up, ct1.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Down, ct1.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Right, ct1.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Left, ct1.InverseTransformDirection(GeneralDirection3D.Back));

			CubicTransformation ct2 = new(GeneralDirection3D.Right, 2, false);
			Assert.AreEqual(GeneralDirection3D.Back, ct2.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Forward, ct2.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Up, ct2.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Down, ct2.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Left, ct2.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Right, ct2.InverseTransformDirection(GeneralDirection3D.Back));

			CubicTransformation ct3 = new(GeneralDirection3D.Back, 0, false);
			Assert.AreEqual(GeneralDirection3D.Back, ct3.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Forward, ct3.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Left, ct3.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Right, ct3.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Down, ct3.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Up, ct3.InverseTransformDirection(GeneralDirection3D.Back));

			CubicTransformation ct4 = new(GeneralDirection3D.Back, 3, false);
			Assert.AreEqual(GeneralDirection3D.Left, ct4.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Right, ct4.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Forward, ct4.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Back, ct4.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Down, ct4.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Up, ct4.InverseTransformDirection(GeneralDirection3D.Back));

			// Mirrored

			CubicTransformation ct5 = new(GeneralDirection3D.Right, 0, true);
			Assert.AreEqual(GeneralDirection3D.Forward, ct5.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Back, ct5.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Down, ct5.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Up, ct5.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Right, ct5.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Left, ct5.InverseTransformDirection(GeneralDirection3D.Back));

			CubicTransformation ct6 = new(GeneralDirection3D.Right, 2, true);
			Assert.AreEqual(GeneralDirection3D.Back, ct6.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Forward, ct6.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Down, ct6.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Up, ct6.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Left, ct6.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Right, ct6.InverseTransformDirection(GeneralDirection3D.Back));

			CubicTransformation ct7 = new(GeneralDirection3D.Back, 0, true);
			Assert.AreEqual(GeneralDirection3D.Back, ct7.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Forward, ct7.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Left, ct7.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Right, ct7.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Up, ct7.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Down, ct7.InverseTransformDirection(GeneralDirection3D.Back));

			CubicTransformation ct8 = new(GeneralDirection3D.Back, 3, true);
			Assert.AreEqual(GeneralDirection3D.Left, ct8.InverseTransformDirection(GeneralDirection3D.Up));
			Assert.AreEqual(GeneralDirection3D.Right, ct8.InverseTransformDirection(GeneralDirection3D.Down));
			Assert.AreEqual(GeneralDirection3D.Forward, ct8.InverseTransformDirection(GeneralDirection3D.Right));
			Assert.AreEqual(GeneralDirection3D.Back, ct8.InverseTransformDirection(GeneralDirection3D.Left));
			Assert.AreEqual(GeneralDirection3D.Up, ct8.InverseTransformDirection(GeneralDirection3D.Forward));
			Assert.AreEqual(GeneralDirection3D.Down, ct8.InverseTransformDirection(GeneralDirection3D.Back));

		}

		[Test]
		public void RotationTests()
		{
			CubicTransformation ct = CubicTransformation.identity;

			ct.TurnPositiveLeftHanded(Axis3D.Y);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Forward);

			ct.TurnPositiveLeftHanded(Axis3D.Z);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Forward);

			ct.TurnPositiveLeftHanded(Axis3D.X);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Forward);

			ct.TurnHalf(Axis3D.Z);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Forward);

			ct.TurnHalf(Axis3D.X);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Forward);

			ct.TurnHalf(Axis3D.Y);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Forward);

			ct.TurnNegativeLeftHanded(Axis3D.X);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Forward);

			ct.TurnNegativeLeftHanded(Axis3D.Z);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Forward);

			ct.TurnNegativeLeftHanded(Axis3D.Y);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Forward);

			ct.Turn(Axis3D.X, 3);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Forward);

			ct.Turn(Axis3D.Z, 5);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Forward);

			ct.Turn(Axis3D.Y, 6);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Forward);

			ct.Turn(Axis3D.Z, -1);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Forward);

			ct.Turn(Axis3D.Y, -101);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Forward);

			ct.Turn(Axis3D.Y, 40000);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Forward);
		}

		[Test]
		public void MirrorTests()
		{
			CubicTransformation ct = CubicTransformation.identity;
			ct.Mirror(Axis3D.X);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Forward);

			ct.Mirror(Axis3D.Y);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Forward);

			ct.Mirror(Axis3D.Z);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Forward);

			ct.TurnNegativeLeftHanded(Axis3D.Y);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Right, ct.Forward);

			ct.Mirror(Axis3D.X);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Back, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Forward);

			ct.Mirror(Axis3D.Z);
			Assert.AreEqual(GeneralDirection3D.Down, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Forward);

			ct.Mirror(Axis3D.Y);
			Assert.AreEqual(GeneralDirection3D.Up, ct.Up);
			Assert.AreEqual(GeneralDirection3D.Forward, ct.Right);
			Assert.AreEqual(GeneralDirection3D.Left, ct.Forward);
		}

		[Test]
		public void OppositeDirectionTest()
		{
			for (byte i = 0; i < CubicTransformation.allTransformationCount; i++)
			{
				CubicTransformation ct = new(i);
				Assert.AreEqual(ct.Down.Opposite(), ct.Up);
				Assert.AreEqual(ct.Up.Opposite(), ct.Down);
				Assert.AreEqual(ct.Left.Opposite(), ct.Right);
				Assert.AreEqual(ct.Right.Opposite(), ct.Left);
				Assert.AreEqual(ct.Forward.Opposite(), ct.Back);
				Assert.AreEqual(ct.Back.Opposite(), ct.Forward);
			}
		}

		[Test]
		public void MatrixOperationTest()
		{
			for (byte i = 0; i < CubicTransformation.allTransformationCount; i++)
			{
				CubicTransformation ct = new(i);
				Matrix4x4 matrixA = ct.GetTransformationMatrix(false);

				for (int di = 0; di < 6; di++)
				{
					GeneralDirection3D direction = (GeneralDirection3D)di;
					Vector3 dA = ct.TransformDirection(direction).ToVector();
					Vector3 dB = matrixA.MultiplyVector(direction.ToVector());

					Assert.IsTrue(dA.CloseEnough(dB));
				}
			}
		}

		[Test]
		public void SetupFromDirectionsTest()
		{
			for (byte i = 0; i < CubicTransformation.allTransformationCount; i++)
			{
				CubicTransformation ct = new(i);

				CubicTransformation ct_xy = CubicTransformation.FromRightUp(ct.Right, ct.Up, ct.isVerticalFlipped);
				Assert.AreEqual(ct, ct_xy, $"Error At: {i}");

				CubicTransformation ct_yz = CubicTransformation.FromUpForward(ct.Up, ct.Forward, ct.isVerticalFlipped);
				Assert.AreEqual(ct, ct_yz, $"Error At: {i}");

				CubicTransformation ct_xyz = CubicTransformation.FromDirections(ct.Right, ct.Up, ct.Forward);
				Assert.AreEqual(ct, ct_xyz, $"Error At: {i}");
			}
		}
	}
}