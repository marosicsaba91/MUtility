using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Ellipse : IPolygon, IDrawable, IEasyHandleable, IArea
	{
		const int defaultFragmentCount = 30;

		public Vector2 size;
		public Ellipse(float x, float y)
		{
			size.x = x;
			size.y = y;
		}

		public Ellipse(Vector2 size)
		{
			this.size = size;
		}

		public float Length
		{
			get
			{
				float a = size.x;
				float b = size.y;
				float sqrt = Mathf.Sqrt((a * a + b * b) / 2f);
				float v = 2 * Mathf.PI * sqrt + Mathf.PI * (a + b);
				return v / 2;
			}
		}

		public float Area => size.y * size.x * Mathf.PI;

		public Bounds Bounds => new(new Vector3(0, 0, 0), new Vector3(size.x, size.y, 0));

		public bool IsInsideShape(Vector2 point)
		{
			Vector2 vec = point;
			vec = new Vector2(vec.x / size.x, vec.y / size.y);
			return vec.magnitude <= 1;
		}

		public IEnumerable<Vector3> Points => ToPolygon();

		IEnumerable<Vector3> ToPolygon(int fragmentCount = defaultFragmentCount)
		{
			var points = new Vector3[fragmentCount];

			float angle = Mathf.PI * 2f / (fragmentCount - 1);
			for (int i = 0; i < fragmentCount - 1; i++)
			{
				float phase = i * angle;
				points[i] = Mathf.Sin(phase) * Vector3.right + Mathf.Cos(phase) * Vector3.up;
				points[i].x *= size.x;
				points[i].y *= size.y;
			}

			points[fragmentCount - 1] = points[0];

			return points;
		}

		public bool DrawHandles()
		{
			if (size.x == 0 || size.y == 0)
				return false;
			Ellipse old = this;

			Vector2 vx = Vector2.right * size.x;
			Vector2 vy = Vector2.up * size.y;
			float ax = Mathf.Abs(size.x);
			float ay = Mathf.Abs(size.y);

			float longer = Mathf.Max(ax, ay);
			float average = (ax + ay) / 2f;

			float handleSize = Mathf.Max(average, longer * 0.75f);
			EasyHandles.fullObjectSize = handleSize;

			size.x = EasyHandles.PositionHandle(vx, vx.normalized).x;
			size.y = EasyHandles.PositionHandle(vy, vy.normalized).y;

			return !Equals(old, this);
		}

		public Vector2 GetRandomPointInArea()
		{
			float a = UnityEngine.Random.Range(0, 2 * Mathf.PI);
			float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));

			float x = r * Mathf.Sin(a) * size.x;
			float y = r * Mathf.Cos(a) * size.y;

			return new Vector2(x, y);
		}

		public Drawable ToDrawable() => new Drawable(Points.ToArray());
	}


	[Serializable]
	public class SpatialEllipse : SpatialPolygon<Ellipse>
	{
	}
}