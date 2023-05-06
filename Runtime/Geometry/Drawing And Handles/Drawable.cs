using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	public readonly struct Drawable
	{
		public enum DrawingType { Gizmo, Debug, Handle}
		public readonly List<Vector3[]> polygons;

		public Drawable(List<Vector3[]> polygons)
		{
			this.polygons = polygons;
		}

		public Drawable(params Vector3[][] polygons)
		{
			this.polygons = new List<Vector3[]>();
			foreach (Vector3[] t in polygons)
				this.polygons.Add(t);
		}

		public void Merge(Drawable other)
		{
			polygons.AddRange(other.polygons);
		}

		public void AddPolygon(Vector3[] polygon) =>
			polygons.Add(polygon);

		public Drawable Transform(Transform transform) =>
			ModifyEach((polygon) => polygon.Transform(transform)); 

		public Drawable Offset(Vector3 offset) =>
			ModifyEach((polygon) => polygon.Offset(offset));

		public Drawable Rotate(Quaternion rotate) =>
			ModifyEach(polygon => polygon.Rotate(rotate));

		public Drawable To3D(Axis3D normalAxis)
		{
			if (normalAxis == Axis3D.X)
				return Rotate(Quaternion.Euler(x: 0, y: 90, z: 90));
			if (normalAxis == Axis3D.Y)
				return Rotate(Quaternion.Euler(x: 90, y: 0, z: 90));
			return this;
		}

		public Drawable Scale(float scale) =>
			ModifyEach((polygon) => polygon.Scale(scale));

		public Drawable Scale(Vector3 scale) =>
			ModifyEach((polygon) => polygon.Scale(scale));

		public Drawable Transform(Vector3 offset, Quaternion rotate, float scale) =>
			ModifyEach((polygon) => polygon.Transform(offset, rotate, scale));

		public Drawable Transform(Vector3 offset, Quaternion rotate, Vector3 scale) =>
			ModifyEach((polygon) => polygon.Transform(offset, rotate, scale));

		public void DrawGizmo(Transform transform, Color color) =>
			Transform(transform).DrawGizmo(color);

		public void DrawGizmo(Transform transform) =>
			Transform(transform).DrawGizmo();

		public void DrawDebug(Transform transform) =>
			Transform(transform).DrawDebug();

		public void DrawDebug(Transform transform, Color color) =>
			Transform(transform).DrawDebug(color);

		public void DrawGizmo(Transform transform, Space space)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawGizmo();
			else
				Transform(transform).DrawGizmo();
		}

		public void DrawGizmo(Transform transform, Space space, Color color)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawGizmo(color);
			else
				Transform(transform).DrawGizmo(color);
		}

		public void DrawDebug(Transform transform, Space space)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawDebug();
			else
				Transform(transform).DrawDebug();
		}

		public void DrawDebug(Transform transform, Space space, Color color)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawDebug(color);
			else
				Transform(transform).DrawDebug(color);
		}

		public void DrawGizmo(Vector3 offset, Color color) =>
			Offset(offset).DrawGizmo(color);

		public void DrawDebug(Vector3 offset, Color color) =>
			Offset(offset).DrawDebug(color);

		public void DrawGizmo(Quaternion rotate, Color color) =>
			Rotate(rotate).DrawGizmo(color);

		public void DrawDebug(Quaternion rotate, Color color) =>
			Rotate(rotate).DrawDebug(color);

		public void DrawGizmo(Vector3 offset, Quaternion rotate, float scale, Color color) =>
			Transform(offset, rotate, scale).DrawGizmo(color);

		public void DrawDebug(Vector3 offset, Quaternion rotate, float scale, Color color) =>
			Transform(offset, rotate, scale).DrawDebug(color);

		public void DrawGizmo(Vector3 offset, Quaternion rotate, Vector3 scale, Color color) =>
			Transform(offset, rotate, scale).DrawGizmo(color);

		public void DrawDebug(Vector3 offset, Quaternion rotate, Vector3 scale, Color color) =>
			Transform(offset, rotate, scale).DrawDebug(color);

		public void DrawGizmo(Color color) =>
			Each((polygon) => polygon.DrawGizmo(color));

		public void DrawGizmo() =>
			Each((polygon) => polygon.DrawGizmo());

		public void DrawDebug(Color color) =>
			Each((polygon) => polygon.DrawDebug(color));

		public void DrawDebug() =>
			Each((polygon) => polygon.DrawDebug());

		public void DrawHandle(Color color) =>
			Each((polygon) => polygon.DrawHandle(color));

		public void DrawHandle() =>
			Each((polygon) => polygon.DrawHandle());


		public void Draw(DrawingType drawingType, Color color = default)
		{
			if (drawingType == DrawingType.Gizmo)
				Each((polygon) => polygon.DrawGizmo(color));
			else if (drawingType == DrawingType.Debug)
				Each((polygon) => polygon.DrawDebug(color));
			else if (drawingType == DrawingType.Handle)
				Each((polygon) => polygon.DrawHandle(color));
		}

		void Each(Action<Vector3[]> action)
		{
			for (int i = 0; i < polygons.Count; i++)
				action(polygons[i]);
		}

		Drawable ModifyEach(Action<Vector3[]> action)
		{
			for (int i = 0; i < polygons.Count; i++)
				action(polygons[i]);
			return new Drawable(polygons);
		}
	}
}