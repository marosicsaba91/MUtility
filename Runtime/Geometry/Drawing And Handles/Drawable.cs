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

		// ------------------------------------------------------------

		public void Transform(Transform transform) =>
			Each(polygon => polygon.Transform(transform));

		public void Translate(Vector3 offset) =>
			Each((polygon) => polygon.Translate(offset));

		public void Rotate(Quaternion rotate) =>
			Each(polygon => polygon.Rotate(rotate));


		public void Scale(float scale) =>
			Each((polygon) => polygon.Scale(scale));

		public void Scale(Vector3 scale) =>
			Each((polygon) => polygon.Scale(scale));

		public void Transform(Vector3 offset, Quaternion rotate, float scale) =>
			Each((polygon) => polygon.Transform(offset, rotate, scale));

		public void Transform(Vector3 offset, Quaternion rotate, Vector3 scale) =>
			Each((polygon) => polygon.Transform(offset, rotate, scale));

		// ------------------------------------------------------------

		public Drawable GetTransformed(Transform transform) =>
			CreateCopy(v3 => transform.TransformPoint(v3));

		public Drawable GetTranslated(Vector3 offset) =>
			CreateCopy(v3 => offset + v3);

		public Drawable GetRotated(Quaternion rotate) =>
			CreateCopy(v3 => rotate * v3);


		public Drawable GetScaled(float scale) =>
			CreateCopy(v3 => scale * v3);

		public Drawable GetScaled(Vector3 scale) =>
			CreateCopy(v3 => v3.MultiplyAllAxis(scale));

		public Drawable GetTransformed(Vector3 offset, Quaternion rotate, float scale) =>
			CreateCopy(v3 => (rotate * (offset + v3)) * scale);

		public Drawable GetTransformed(Vector3 offset, Quaternion rotate, Vector3 scale) =>
			CreateCopy(v3 => (rotate * (offset + v3)).MultiplyAllAxis(scale));

		// ------------------------------------------------------------------

		public Drawable To3D(Axis3D normalAxis)
		{
			if (normalAxis == Axis3D.X)
				return GetRotated(Quaternion.Euler(x: 0, y: 90, z: 90));
			if (normalAxis == Axis3D.Y)
				return GetRotated(Quaternion.Euler(x: 90, y: 0, z: 90));
			return this;
		}

		public void DrawGizmo(Transform transform, Color color) =>
			GetTransformed(transform).DrawGizmo(color);

		public void DrawGizmo(Transform transform) =>
			GetTransformed(transform).DrawGizmo();

		public void DrawDebug(Transform transform) =>
			GetTransformed(transform).DrawDebug();

		public void DrawDebug(Transform transform, Color color) =>
			GetTransformed(transform).DrawDebug(color);

		public void DrawGizmo(Transform transform, Space space)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawGizmo();
			else
				GetTransformed(transform).DrawGizmo();
		}

		public void DrawGizmo(Transform transform, Space space, Color color)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawGizmo(color);
			else
				GetTransformed(transform).DrawGizmo(color);
		}

		public void DrawDebug(Transform transform, Space space)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawDebug();
			else
				GetTransformed(transform).DrawDebug();
		}

		public void DrawDebug(Transform transform, Space space, Color color)
		{
			if (transform == null)
				space = Space.World;
			if (space == Space.World)
				DrawDebug(color);
			else
				GetTransformed(transform).DrawDebug(color);
		}

		public void DrawGizmo(Vector3 offset, Color color) =>
			GetTranslated(offset).DrawGizmo(color);

		public void DrawDebug(Vector3 offset, Color color) =>
			GetTranslated(offset).DrawDebug(color);

		public void DrawGizmo(Quaternion rotate, Color color) =>
			GetRotated(rotate).DrawGizmo(color);

		public void DrawDebug(Quaternion rotate, Color color) =>
			GetRotated(rotate).DrawDebug(color);

		public void DrawGizmo(Vector3 offset, Quaternion rotate, float scale, Color color) =>
			GetTransformed(offset, rotate, scale).DrawGizmo(color);

		public void DrawDebug(Vector3 offset, Quaternion rotate, float scale, Color color) =>
			GetTransformed(offset, rotate, scale).DrawDebug(color);

		public void DrawGizmo(Vector3 offset, Quaternion rotate, Vector3 scale, Color color) =>
			GetTransformed(offset, rotate, scale).DrawGizmo(color);

		public void DrawDebug(Vector3 offset, Quaternion rotate, Vector3 scale, Color color) =>
			GetTransformed(offset, rotate, scale).DrawDebug(color);

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

		Drawable CreateCopy(Func<Vector3, Vector3> action)
		{
			List<Vector3[]> newPolygons = new(polygons.Count);
			for (int i = 0; i < polygons.Count; i++) 
			{
				Vector3[] originalPolygon = polygons[i];
				Vector3[] newPolygon = new Vector3[originalPolygon.Length];
				for (int j = 0; j < originalPolygon.Length; j++)
				{ 
					newPolygon[j] = action(originalPolygon[j]);
				}
				newPolygons.Add(newPolygon); 
			}
			return new Drawable(newPolygons);
		}
	}
}