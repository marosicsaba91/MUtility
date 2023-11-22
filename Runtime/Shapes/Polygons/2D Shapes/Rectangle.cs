using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Rectangle : IPolygon, IWireShape, IEasyHandleable, IArea
	{
		public Vector2 size;

		public Rectangle(Vector2 size)
		{
			this.size = size;
		}

		public Rectangle(float width, float height)
		{
			size = new Vector2(width, height);
		}

		public Rectangle(BoxCollider2D boxCollider)
		{
			Transform transform = boxCollider.transform;
			Vector3 lossyScale = transform.lossyScale;
			Vector2 colliderSize = boxCollider.size;
			float width = colliderSize.x * lossyScale.x;
			float height = colliderSize.y * lossyScale.y;
			size = new(width, height);
		}


		public float Area => size.x * size.y;

		public float Length => 2f * (size.x + size.y);

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

		public bool DrawHandles()
		{
			if (size.x == 0 || size.y == 0)
				return false;
			Rectangle old = this;

			Vector3 right = EasyHandles.PositionHandle(Right);
			Vector3 left = EasyHandles.PositionHandle(Left);
			Vector3 top = EasyHandles.PositionHandle(Top);
			Vector3 bottom = EasyHandles.PositionHandle(Bottom);

			size = new Vector2(right.x - left.x, top.y - bottom.y);
			size = EasyHandles.PositionHandle(TopRight) * 2;
			return !Equals(old, this);
		}

		public Vector2 GetRandomPointInArea() => new(
			UnityEngine.Random.Range(size.y / -2f, size.x / 2f),
			UnityEngine.Random.Range(size.y / -2f, size.y / 2f));


		public bool IsPointInside(Vector2 point) =>
			Mathf.Abs(point.x) < size.x / 2f &&
			Mathf.Abs(point.y) < size.y / 2f;

		internal Rect ToRect() => new(size / 2f, size);

		public Bounds Bounds => new(Vector3.zero, size);

		public IEnumerable<Vector3> Points
		{
			get
			{
				yield return TopRight;
				yield return TopLeft;
				yield return BottomLeft;
				yield return BottomRight;
				yield return TopRight;
			}
		}

		public WireShape ToWireShape() => Points.ToDrawable();
	}
}
