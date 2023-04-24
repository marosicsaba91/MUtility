using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Arrow : IDrawable, IEasyHandleable
	{
		public Vector3 origin;
		public Vector3 direction;
		public float magnitude;
		public Vector3? normalDirection;

		public Vector3 DirectionNormalized
		{
			get => direction.normalized;
			set => direction = value == Vector3.zero ? Vector3.zero : value.normalized;
		}

		public Arrow(Ray ray, Vector3? normalDirection = null) => Init(
			ray,
			normalDirection,
			out origin,
			out direction,
			out magnitude,
			out this.normalDirection);


		static void Init(Ray ray, Vector3? normalDirectionValue,
			out Vector3 origin, out Vector3 directionNormalized, out float magnitude, out Vector3? normalDirection)
		{
			origin = ray.origin;
			magnitude = ray.direction.magnitude;

			directionNormalized = ray.direction == Vector3.zero ? Vector3.up : ray.direction.normalized;
			normalDirection = normalDirectionValue;
		}

		public Arrow(Vector3 origin, Vector3 direction, Vector3? normalDirection = null)
		{
			this.origin = origin;
			magnitude = direction.magnitude;

			if (direction == Vector3.zero)
				this.direction = Vector3.up;
			else
				this.direction = direction.normalized;
			this.normalDirection = normalDirection;
		}


		public Arrow(Vector3 origin, Vector3 direction, float magnitude, Vector3? normalDirection = null)
		{
			this.origin = origin;
			this.magnitude = magnitude;

			if (direction == Vector3.zero)
				this.direction = Vector3.up;
			else
				this.direction = direction.normalized;
			this.normalDirection = normalDirection;
		}

		Vector3 Head
		{
			get => origin + DirectionNormalized * magnitude;
			set
			{
				Vector3 vec = value - origin;
				DirectionNormalized = vec.normalized;
				magnitude = vec.magnitude;
			}
		}

		public Vector3 DirectionVector
		{
			get => DirectionNormalized * magnitude;
			set
			{
				DirectionNormalized = value;
				magnitude = value.magnitude;
			}
		}
		Ray ToRay => new Ray(origin, DirectionVector);

		public Drawable ToDrawable() => ToDrawable(DrawingSettings.Default);
		public Drawable ToDrawable(DrawingSettings visuals)
		{
			Vector3 normal;
			if (normalDirection == null)
			{
				Camera c = Camera.current;
				normal = c == null ? Vector3.forward : (c.transform.position - origin).normalized;
			}
			else
				normal = normalDirection.Value;

			if (magnitude < 0)
				magnitude = 0;

			float arrowHeadLength = magnitude == 0 ? visuals.maxArrowHeadLength : Mathf.Min(visuals.maxArrowHeadLength, magnitude);
			bool showArrowHead = visuals.maxArrowHeadLength > 0 && visuals.arrowHeadAngleInDeg > 0 && visuals.arrowHeadAngleInDeg < 180;
			bool headOnly = showArrowHead && (magnitude <= visuals.maxArrowHeadLength || magnitude == 0);
			bool showBaseLine = visuals.baseLineLength > 0 && !headOnly;
			bool showMiddleLine = !headOnly;

			int polygonIndex = 0;
			int polygonCount =
				(showArrowHead ? 1 : 0) +
				(showBaseLine ? 1 : 0) +
				(showMiddleLine ? 1 : 0);
			var polygons = new Vector3[polygonCount][];

			Vector3 normalizedDirection = DirectionNormalized;
			float middleLength = magnitude - (showArrowHead ? arrowHeadLength : 0);
			Vector3 backOfHead = origin + middleLength * normalizedDirection;
			Vector3 perpendicular = Vector3.Cross(normalizedDirection, normal).normalized;
			if (perpendicular == Vector3.zero)
			{
				perpendicular = Math.Abs(Mathf.Abs(normalizedDirection.x) - 1) > 0.0001f ? Vector3.right : Vector3.up;
			}

			if (showArrowHead)
			{
				float arrowHeadBack = arrowHeadLength * 2 * Mathf.Tan(visuals.arrowHeadAngleInDeg * Mathf.Deg2Rad / 2f);
				Vector3 halfArrowBackVec = perpendicular * (0.5f * arrowHeadBack);
				Vector3 headA = backOfHead + halfArrowBackVec;
				Vector3 headB = backOfHead - halfArrowBackVec;
				polygons[polygonIndex] = new[] { Head, headA, headB, Head };
				polygonIndex++;
			}

			if (showBaseLine)
			{
				Vector3 halfBaseLineVec = perpendicular * (0.5f * visuals.baseLineLength);
				Vector3 baseLineA = origin + halfBaseLineVec;
				Vector3 baseLineB = origin - halfBaseLineVec;
				polygons[polygonIndex] = new[] { baseLineA, baseLineB };
				polygonIndex++;
			}

			if (showMiddleLine)
				polygons[polygonIndex] = new[] { origin, backOfHead };

			return new Drawable(polygons);
		}


		public bool DrawHandles()
		{
			Arrow old = this;

			origin = EasyHandles.PositionHandle(origin);
			Head = EasyHandles.PositionHandle(Head);

			return !Equals(old, this);
		}


		[Serializable]
		public struct DrawingSettings
		{
			const float defaultBaseLineLength = 0.2f;
			const float defaultHeadLength = 0.1f;
			const float defaultHeadAngle = 90f;

			public static DrawingSettings Default => new DrawingSettings(defaultBaseLineLength, defaultHeadLength, defaultHeadAngle);

			public float baseLineLength;
			public float maxArrowHeadLength;
			public float arrowHeadAngleInDeg;

			public DrawingSettings(float baseLineLength, float arrowHeadLength, float arrowHeadAngle)
			{
				this.baseLineLength = baseLineLength;
				maxArrowHeadLength = arrowHeadLength;
				arrowHeadAngleInDeg = arrowHeadAngle;
			}
		}
	}
}