using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public abstract class Spline : IPolygon, IEasyHandleable, IDrawable
	{
		[SerializeField] protected bool isLoop = false;
		public event Action Regenerated;

		public bool IsLoop
		{
			get => isLoop;
			set
			{
				isLoop = value;
				IsDirty = true;
			}
		}

		protected List<InterpolatedPoint> interpolatedPoints = new();

		bool _isDirty = true;

		public bool IsDirty
		{
			get => _isDirty;
			protected set => _isDirty = value;
		}


		public void SetDirty() => IsDirty = true;

		public IReadOnlyList<InterpolatedPoint> InterpolatedPoints
		{
			get
			{
				TryRecalculatePoints();
				return interpolatedPoints;
			}
		}
		public abstract IEnumerable<Vector3> ControlPointPositions { get; }


		Bounds _bounds;

		public IEnumerable<Vector3> Points
		{
			get
			{
				TryRecalculatePoints();
				return interpolatedPoints.Select(p => p.position);

			}
		}

		public Bounds Bounds
		{
			get
			{
				TryRecalculatePoints();
				return _bounds;
			}
		}

		float _length;
		public float Length
		{
			get
			{
				TryRecalculatePoints();
				return _length;
			}
		}

		protected void TryRecalculatePoints()
		{
			if (!IsDirty)
				return;
			ForceRecalculatePoints();
		}

		public void ForceRecalculatePoints()
		{
			IsDirty = false;
			interpolatedPoints.Clear();
			RecalculatePoints(interpolatedPoints, out Bounds b, out float l);
			_bounds = b;
			_length = l;
		}

		protected abstract void RecalculatePoints(List<InterpolatedPoint> result, out Bounds bounds, out float length);
		public abstract bool DrawHandles();
		public Drawable ToDrawable() => new(ControlPointPositions.ToArray());

		protected void OnRegenerated()
		{
			Regenerated?.Invoke();
		}

	}

	[Serializable]
	public abstract class Spline<TControlPoint> : Spline
	{
		[SerializeField] protected List<TControlPoint> controlPoints;

		// ------------------- Constructors -------------------

		protected Spline(bool isLoop, params TControlPoint[] controlPoint)
		{
			controlPoints = new List<TControlPoint>(controlPoint);
			this.isLoop = isLoop;
		}

		protected Spline(params TControlPoint[] nodes) : this(false, nodes) { }

		// ------------------- Methods -------------------
		public IReadOnlyList<TControlPoint> ControlPoints => controlPoints;

		public override IEnumerable<Vector3>
			ControlPointPositions => controlPoints.Select(ControlPointToPosition);

		public void ClearControlPoints()
		{
			controlPoints.Clear();
			IsDirty = true;
		}

		public void AddControlPoint(TControlPoint controlPoint)
		{
			controlPoints.Add(controlPoint);
			IsDirty = true;
		}

		public void SetControlPoint(int index, TControlPoint controlPoint)
		{
			controlPoints[index] = controlPoint;
			IsDirty = true;
		}


		public void AddControlPointRange(IEnumerable<TControlPoint> points)
		{
			controlPoints.AddRange(points);
			IsDirty = true;
		}

		public void RemoveControlPoint(TControlPoint controlPoint)
		{
			controlPoints.Remove(controlPoint);
			IsDirty = true;
		}

		public void RemoveControlPointAt(int index)
		{
			controlPoints.RemoveAt(index);
			IsDirty = true;
		}

		public void InsertControlPoint(int index, TControlPoint controlPoint)
		{
			controlPoints.Insert(index, controlPoint);
			IsDirty = true;
		}

		public int SegmentCount => isLoop ? controlPoints.Count : controlPoints.Count - 1;


		// ------------------------- Interpolation -------------------------

		public Ray Evaluate(float controlPointIndex)
		{
			if (controlPoints is { Count: 0 })
				return new Ray();
			if (controlPoints.Count == 1)
				return new Ray(ControlPointToPosition(controlPoints[0]), Vector3.zero);

			controlPointIndex = isLoop
				? MathHelper.Mod(controlPointIndex, SegmentCount)
				: Mathf.Clamp(controlPointIndex, 0, SegmentCount);

			int segmentCount = SegmentCount;
			int i = (int)controlPointIndex;
			int pi, ni1, ni2;
			if (isLoop)
			{
				pi = MathHelper.Mod(i - 1, segmentCount);
				ni1 = (i + 1) % segmentCount;
				ni2 = (i + 2) % segmentCount;
			}
			else
			{
				pi = i - 1;
				ni1 = Mathf.Min(i + 1, controlPoints.Count - 1);
				ni2 = ni1 == controlPoints.Count - 1 ? -1 : ni1 + 1;
			}

			return EvaluateSafe(controlPointIndex, pi, i, ni1, ni2);
		}

		public Ray Evaluate01(float time01) => Evaluate(time01 * SegmentCount);

		public float EvaluateDistance(float controlPointIndex)
		{
			TryRecalculatePoints();

			int index = (int)controlPointIndex;
			int nextIndex =
				index < controlPoints.Count - 1 ? index + 1 :
				isLoop ? 0 : index;

			if (index >= controlPoints.Count || nextIndex >= controlPoints.Count)
				return default;

			InterpolatedPoint p1 = interpolatedPoints[index];
			InterpolatedPoint p2 = interpolatedPoints[nextIndex];

			return InterpolatedPoint.Lerp(p1, p2, controlPointIndex - index).distance;
		}

		protected sealed override void RecalculatePoints(List<InterpolatedPoint> result, out Bounds bounds, out float length)
		{
			if (controlPoints == null || controlPoints.Count == 0)
			{
				bounds = default;
				length = 0;
				return;
			}

			if (controlPoints.Count == 1)
			{
				bounds = new Bounds(ControlPointToPosition(controlPoints[0]), Vector3.zero);
				length = 0;
				result.Add(new InterpolatedPoint(0, ControlPointToPosition(controlPoints[0]), Vector3.zero, 0));
			}
			else
				SafeRecalculatePoints(result, out bounds, out length);

			OnRegenerated();
		}


		// controlPoints are not null and have at least 2 elements
		protected abstract void SafeRecalculatePoints(List<InterpolatedPoint> result, out Bounds bounds, out float length);

		public override bool DrawHandles()
		{
			for (int i = 0; i < controlPoints.Count; i++)
			{
				TControlPoint cp = controlPoints[i];
				Vector3 pos = ControlPointToPosition(cp);
				Vector3 newPos = EasyHandles.PositionHandle(pos);

				if (!Equals(pos, newPos) && EasyHandles.LastEvent == HandleEvent.LmbDrag)
				{
					if (Move(newPos, i))
					{
						IsDirty = true;
						return true;
					}
				}

				if (EasyHandles.LastEvent == HandleEvent.LmbClick)
				{
					if (LeftClickOnControlPoint(i))
					{
						IsDirty = true;
						return true;
					}
				}

				if (EasyHandles.LastEvent == HandleEvent.RmbClick)
				{
					if (RightClickOnControlPoint(i))
					{
						IsDirty = true;
						return true;
					}
				}

				// MIDDLE
				Ray mid = Evaluate(i + 0.5f);
				EasyHandles.PositionHandle(mid.origin, EasyHandles.Shape.Sphere);
				if (EasyHandles.LastEvent == HandleEvent.LmbClick)
				{
					if (LeftClickOnTheMiddlePoint(i, mid.origin))
					{
						IsDirty = true;
						return true;
					}
				}
				if (EasyHandles.LastEvent == HandleEvent.RmbClick)
				{
					if (RightClickOnTheMiddlePoint(i, mid.origin))
					{
						IsDirty = true;
						return true;
					}
				}

			}

			return false;
		}

		protected virtual bool Move(Vector3 newPos, int i)
		{
			TControlPoint cp = PositionToControlPoint(newPos);
			controlPoints[i] = cp;
			IsDirty = true;
			return true;
		}

		protected virtual bool RightClickOnControlPoint(int index)
		{
			// REMOVE
			if (controlPoints.Count > 2)
			{
				controlPoints.RemoveAt(index);
				return true;
			}
			return false;
		}

		protected virtual bool LeftClickOnControlPoint(int index) => false;



		protected virtual bool LeftClickOnTheMiddlePoint(int index, Vector3 pos)
		{
			// ADD 
			TControlPoint newPoint = PositionToControlPoint(pos);
			controlPoints.Insert(index + 1, newPoint);
			return true;
		}

		protected virtual bool RightClickOnTheMiddlePoint(int index, Vector3 pos) => false;



		// ------------------------- Abstract Methods -------------------------

		protected abstract Ray EvaluateSafe(float continuousIndex, int previousIndex, int index, int nextIndex, int next2Index);
		protected abstract Vector3 ControlPointToPosition(TControlPoint controlPoint);
		public abstract TControlPoint PositionToControlPoint(Vector3 pose);
	}
}