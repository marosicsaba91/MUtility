using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MUtility
{
	public abstract class SpatialPolygon<TPolygon> : IEasyHandleable, IPolygon, IDrawable where TPolygon : IPolygon
	{
		public TPolygon polygon;
		public Vector3 position;
		[EulerAngles] public Quaternion rotation;

		protected SpatialPolygon() { }

		protected SpatialPolygon(Transform transform)
		{
			position = transform.position;
			rotation = transform.rotation;
		}

		protected SpatialPolygon(TPolygon polygon, Transform transform)
		{
			this.polygon = polygon;
			position = transform.position;
			rotation = transform.rotation;
		}

		protected SpatialPolygon(TPolygon polygon, Vector3 position = default, Quaternion rotation = default)
		{
			this.polygon = polygon;
			this.position = position;
			this.rotation = rotation;
		}


		public IEnumerable<Vector3> Points => polygon.Points.Transform(position, rotation, 1);
		public Bounds Bounds => polygon.Bounds.Transform(position, rotation);
		public float Length => polygon.Length;

		public bool DrawHandles() => SpatialShapeHelper.OnDrawHandles(ref polygon, ref position, ref rotation);

		public Vector3 Right => rotation * Vector3.right;
		public Vector3 Up => rotation * Vector3.up;
		public Vector3 Forward => rotation * Vector3.forward;
		public Drawable ToDrawable() => polygon.Points.Transform(position, rotation).ToDrawable();
	}
}