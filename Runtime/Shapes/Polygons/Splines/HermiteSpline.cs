using System;
using UnityEngine;

namespace MUtility
{

	[Serializable]
	public struct ControlPoint
	{
		public enum Type
		{
			Mirror,  // C1 G1 (Use SpeedIn only)
			Smooth,  // C0 G1 (Use SpeedIn & SpeedOut length)
			Break,   // C0 G0 (Use SpeedIn & SpeedOut)
			Corner,  // C0 G0 (NO SpeedIn & SpeedOut)
		}
		const int enumLength = 4;

		public Vector3 position;

		public Vector3 speedIn;
		public Vector3 speedOut;
		public Type type;

		public Vector3 SpeedIn
		{
			get => type == Type.Corner ? Vector3.zero : speedIn;
			set => speedIn = value;
		}

		public Vector3 SpeedOut
		{
			get => type switch
			{
				Type.Corner => Vector3.zero,
				Type.Mirror => speedIn,
				Type.Smooth => speedIn.normalized * speedOut.magnitude,
				_ => speedOut
			};
			set
			{
				if (type == Type.Mirror)
					speedIn = value;

				if (type == Type.Smooth)
					speedIn = value.normalized * speedIn.magnitude;


				speedOut = value;
			}
		}

		public void NextType()
		{
			type = (Type)(((int)type + 1) % enumLength);
		}
	}

	[Serializable]
	public class HermiteSpline : CubicSpline<ControlPoint>
	{
		public HermiteSpline() : base(Array.Empty<ControlPoint>()) { }
		public HermiteSpline(bool isClosed, params ControlPoint[] nodes) : base(isClosed, nodes) { }
		public HermiteSpline(params ControlPoint[] nodes) : base(nodes) { }

		protected override Vector3 ControlPointToPosition(ControlPoint controlPoint) => controlPoint.position;

		protected override Ray EvaluateSafe(float continuousIndex, int previousIndex, int index, int nextIndex, int next2Index)
		{
			ControlPoint a = controlPoints[index];
			ControlPoint b = controlPoints[nextIndex];
			return EvaluateOnSegment(a, b, continuousIndex);
		}

		public Ray EvaluateOnSegment(ControlPoint a, ControlPoint b, float controlPointIndex)
		{
			EvaluateOnSegment(a, b, controlPointIndex, out Vector3 position, out Vector3 direction);
			return new Ray(position, direction);
		}

		public void EvaluateOnSegment(ControlPoint a, ControlPoint b, float controlPointIndex, out Vector3 position, out Vector3 direction)
		{
			float t = controlPointIndex % 1;
			float t2 = t * t;
			float t3 = t * t2;

			Vector3 p0 = a.position;
			Vector3 p1 = b.position;
			Vector3 v0 = a.SpeedOut;
			Vector3 v1 = b.SpeedIn;

			position =
				(2 * t3 - 3 * t2 + 1) * p0 +
				(t3 - 2 * t2 + t) * v0 +
				(-2 * t3 + 3 * t2) * p1 +
				(t3 - t2) * v1;

			direction = Vector3.Lerp(v0, v1, t);
		}

		public override ControlPoint PositionToControlPoint(Vector3 point) => new()
		{ position = point, SpeedIn = Vector3.one, SpeedOut = Vector3.one };

		public override bool DrawHandles()
		{
			bool isChanged = base.DrawHandles();
			const float mult = 3;

			for (int i = 0; i < controlPoints.Count; i++)
			{
				ControlPoint cp = controlPoints[i];
				Vector3 position = cp.position;

				if (cp.type == ControlPoint.Type.Corner)
					continue;

				// IN
				Vector3 p1 = position - cp.SpeedIn / mult;
				EasyHandles.DrawLine(position, p1);
				Vector3 p1New = EasyHandles.PositionHandle(p1);
				if (!Equals(p1New, p1))
				{
					Vector3 distance = (position - p1New) * mult;
					cp.SpeedIn = distance;
					controlPoints[i] = cp;
					isChanged = true;
					IsDirty = true;
				}

				// OUT
				Vector3 p2 = cp.position + cp.SpeedOut / mult;
				EasyHandles.DrawLine(position, p2);
				Vector3 p2New = EasyHandles.PositionHandle(p2);
				if (!Equals(p2New, p2))
				{
					Vector3 distance = (p2New - position) * mult;
					cp.SpeedOut = distance;
					controlPoints[i] = cp;
					isChanged = true;
					IsDirty = true;
				}
			}

			return isChanged;
		}

		protected override bool LeftClickOnControlPoint(int index)
		{
			ControlPoint cp = ControlPoints[index];
			cp.NextType();
			controlPoints[index] = cp;
			return true;
		}

		protected override bool Move(Vector3 newPos, int index)
		{
			ControlPoint cp = controlPoints[index];
			cp.position = newPos;
			controlPoints[index] = cp;
			return true;
		}
	}
}