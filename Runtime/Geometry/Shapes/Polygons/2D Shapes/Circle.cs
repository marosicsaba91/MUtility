using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Circle : IPolygon, IDrawable, IEasyHandleable, IArea
	{
		public const int defaultSegmentCount = 20;

		public float radius;

		public Circle(float radius)
		{
			this.radius = radius;
		}

		public Circle(CircleCollider2D collider)
		{
			Vector2 scale = collider.transform.lossyScale;
			float s = Mathf.Max(scale.x, scale.y);
			radius = collider.radius * s;
		}

		public Vector2 GetRandomPoint()
		{
			float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
			float rad = UnityEngine.Random.Range(0, radius);

			return rad * GeometryHelper.RadianToVector2D(angle);
		}

		public float Diameter
		{
			get => 2f * radius;
			set => radius = value / 2f;
		}

		public float Area => radius * radius * Mathf.PI;

		public float Length => 2f * radius * Mathf.PI;

		public Bounds Bounds => new(Vector3.zero, new Vector3(2 * radius, 2 * radius, 0));

		public bool IsPointInside(Vector2 point) => point.magnitude <= radius;

		public float Distance(Ray2D ray)
		{
			float centerLineDistance = GeometryHelper.DistanceBetweenPointAndLine(Vector3.zero, new Line(ray.origin, ray.origin + ray.direction));
			if (centerLineDistance <= radius)
				return 0;
			return centerLineDistance - radius;
		}

		public Drawable ToDrawable() => new Drawable(Points.ToArray());

		public Drawable ToDrawable(int segmentCount, bool dashedLine = false)
		{
			if (dashedLine)
				return ToDrawable_Dashed(segmentCount);
			return new Drawable(ToPolygon(segmentCount).ToArray());
		}

		public Drawable ToDrawable_Dashed(int segmentCount = defaultSegmentCount)
		{
			segmentCount /= 2;
			Vector2 right = new Vector2(radius, 0);
			Vector2 up = new Vector2(0, radius);

			var polygons = new Vector3[segmentCount][];

			float angle = Mathf.PI * 2f / (segmentCount * 2);
			for (int i = 0; i < polygons.Length; i++)
			{
				float phase1 = angle * 2 * i;
				float phase2 = angle * (2 * i + 1);
				Vector2 p1 = Mathf.Sin(phase1) * right + Mathf.Cos(phase1) * up;
				Vector2 p2 = Mathf.Sin(phase2) * right + Mathf.Cos(phase2) * up;
				polygons[i] = new Vector3[] { p1, p2 };
			}
			return new Drawable(polygons);
		}

		public IEnumerable<Vector3> Points => ToPolygon();

		public IEnumerable<Vector3> ToPolygon(int segmentCount = defaultSegmentCount)
		{
			Vector3 right = new Vector2(radius, 0);
			Vector3 up = new Vector2(0, radius);

			var points = new Vector3[segmentCount + 1];

			float angle = Mathf.PI * 2f / segmentCount;
			for (int i = 0; i < segmentCount; i++)
			{
				float phase = angle * i;
				points[i] = Mathf.Sin(phase) * right + Mathf.Cos(phase) * up;
			}
			points[^1] = points[0];

			return points;
		}


		public bool DrawHandles()
		{
			if (radius == 0)
				return false;
			EasyHandles.fullObjectSize = 2 * radius;
			Vector3 newVector = EasyHandles.PositionHandle(new Vector2(radius, 0), Vector3.right, EasyHandles.Shape.Dot);
			if (Math.Abs(radius - newVector.x) < float.Epsilon)
				return false;
			radius = newVector.x;
			return true;
		}

		public Vector2 GetRandomPointInArea()
		{
			float a = UnityEngine.Random.Range(0, 2 * Mathf.PI);
			float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, radius));

			float x = r * Mathf.Sin(a);
			float y = r * Mathf.Cos(a);
			return new(x, y);
		}
	}

	public class SpatialCircle : SpatialPolygon<Circle>
	{
		public SpatialCircle(Vector3 center, float radius, Vector3 normal)
		{
			position = center;
			rotation = Quaternion.LookRotation(normal);
			polygon = new Circle(radius);
		}
		public SpatialCircle(CircleCollider2D collider) : base(collider.transform) => polygon = new(collider);
	}

}
