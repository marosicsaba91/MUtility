using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Capsule2D : IPolygon, IDrawable, IEasyHandleable, IArea
	{
		public const int defaultSegmentCount = 50;

		[SerializeField] Vector2 size;

		public Vector2 Size
		{
			get => size;
			set
			{
				size = value;
				if (size.x < 0)
					size.x *= -1;
				if (size.y < 0)
					size.y *= -1;
			}
		}

		public Capsule2D(float x, float y) : this(new Vector2(x, y)) { }

		public Capsule2D(Vector2 size)
		{
			this.size = size;
			Size = size;
		}

		public Capsule2D(CapsuleCollider2D collider)
		{
			Vector2 scale = collider.transform.lossyScale;
			Vector2 colliderSize = collider.size;
			size = new(colliderSize.x * scale.x, colliderSize.y * scale.y);
			if (collider.direction == CapsuleDirection2D.Horizontal ^ size.x > size.y)
			{
				float max = Mathf.Max(size.x, size.y);
				size = new(max, max);
			}
			Size = size;
		}

		public float Width => size.x;
		public float Height => size.y;

		public float XMin => -size.x / 2f;
		public float XMax => +size.x / 2f;
		public float YMin => -size.y / 2f;
		public float YMax => +size.y / 2f;

		public Vector2 Right => new(XMax, 0);
		public Vector2 Left => new(XMin, 0);
		public Vector2 Top => new(0, YMax);
		public Vector2 Bottom => new(0, YMin);

		public Vector2 TopRight => new(XMax, YMax);
		public Vector2 TopLeft => new(XMin, YMax);
		public Vector2 BottomRight => new(XMax, YMin);
		public Vector2 BottomLeft => new(XMin, YMin);

		public float LongAxis => Mathf.Max(size.x, size.y);
		public float ShortAxis => Mathf.Min(size.x, size.y);
		public float Radius => ShortAxis / 2f;

		public Vector2 Extents => size / 2f;

		public CapsuleDirection2D Direction =>
			size.x > size.y ? CapsuleDirection2D.Horizontal : CapsuleDirection2D.Vertical;


		public Vector2 GetRandomPoint()
		{
			float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
			float rad = UnityEngine.Random.Range(0, Radius);

			return rad * GeometryHelper.RadianToVector2D(angle);
		}

		public float Area => ((LongAxis - ShortAxis) * ShortAxis) + Radius * Radius * Mathf.PI;

		public float Length => (LongAxis - ShortAxis) + (2 * Radius * Mathf.PI);

		public Drawable ToDrawable() => new(Points.ToArray());

		public Drawable ToDrawable(int circleSegmentCount) => new(ToPolygon(circleSegmentCount).ToArray());
		public Bounds Bounds => new(Vector3.zero, size);

		public IEnumerable<Vector3> Points => ToPolygon();

		public IEnumerable<Vector3> ToPolygon(int segmentCount = defaultSegmentCount)
		{
			if (Math.Abs(size.x - size.y) < float.Epsilon)
			{
				Circle circle = new(Radius);
				foreach (Vector3 point in circle.ToPolygon(segmentCount))
					yield return point;

				yield break;
			}


			float radius = Radius;
			Vector3 center1, center2;
			float startAngle, endAngle;
			Vector2 extents = Extents;
			bool horizontal = Direction == CapsuleDirection2D.Horizontal;

			if (horizontal)
			{
				center1 = new(extents.x - extents.y, 0);
				center2 = new(-extents.x + extents.y, 0);
				startAngle = 90;
				endAngle = 270;
			}
			else
			{
				center1 = new(0, -extents.y + extents.x);
				center2 = new(0, extents.y - extents.x);
				startAngle = 0;
				endAngle = 180;
			}

			CircleSector sector1 = new(radius, startAngle, endAngle, 1);
			foreach (Vector3 point in sector1.ToPolygon(segmentCount))
				yield return center1 + point;


			CircleSector sector2 = new(radius, endAngle, startAngle, 1);
			foreach (Vector3 point in sector2.ToPolygon(segmentCount))
				yield return center2 + point;


			if (horizontal)
				yield return new(extents.x - extents.y, extents.y);
			else
				yield return new(extents.x, -extents.y + extents.x);

		}

		public bool DrawHandles()
		{
			if (size.x == 0 || size.y == 0)
				return false;

			Vector2 oldSize = size;

			Vector3 right = EasyHandles.PositionHandle(Right);
			Vector3 left = EasyHandles.PositionHandle(Left);
			Vector3 top = EasyHandles.PositionHandle(Top);
			Vector3 bottom = EasyHandles.PositionHandle(Bottom);

			Size = new Vector2(right.x - left.x, top.y - bottom.y);
			Size = EasyHandles.PositionHandle(TopRight) * 2;

			return oldSize != size;
		}

		public Vector2 GetRandomPointInArea()
		{
			float a = UnityEngine.Random.Range(0, 2 * Mathf.PI);
			float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, Radius));

			float x = r * Mathf.Sin(a);
			float y = r * Mathf.Cos(a);
			return new(x, y);
		}

	}

	[Serializable]
	public class SpatialCapsule2D : SpatialPolygon<Capsule2D>
	{
		public SpatialCapsule2D(CapsuleCollider2D collider) : base(collider.transform) => polygon = new(collider);

		public SpatialCapsule2D(Capsule2D capsule, Transform transform) : base(capsule, transform) { }
	}

}
