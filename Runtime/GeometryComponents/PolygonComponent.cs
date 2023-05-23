using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	public class PolygonComponent : MonoBehaviourWithHandles
	{

		[SerializeReference, TypePicker(nameof(TypeFilter))]
		IPolygon _polygon;

		internal void SetShapeType(Type type)
		{
			Type bt = typeof(IPolygon);
			if (!type.IsAbstract && bt.IsAssignableFrom(type))
				_polygon = (IPolygon)Activator.CreateInstance(type);
		}

		public Space space = Space.Self;

		public event Action Updated;

		public IEnumerable<Vector3> Points
		{
			get
			{
				if (_polygon == null)
					yield break;


				IEnumerable<Vector3> points = PointsLocal;

				if (DrawHandlesInSelfSpace)
					points = points.TransformAll(transform);

				foreach (Vector3 point in points)
					yield return point;
			}
		}

		public override bool DrawHandlesInSelfSpace => space == Space.Self;


		public IEnumerable<Vector3> PointsLocal => _polygon.Points;


		void OnValidate()
		{
			if (_polygon is Spline spline)
				spline.SetDirty();
			Updated?.Invoke();
		}

		public IPolygon Polygon => _polygon;


		[Header("Gizmos")][SerializeField] bool drawGizmos = true;
		[SerializeField] Color gizmoColor = Color.white;
		[SerializeField] bool drawHandles = true;

		public Type GetPolygonType() => _polygon?.GetType();

		bool TypeFilter(Type type)
		{
			if (IsSubclassOfRawGeneric(type, typeof(SpatialPolygon<>)))
				return false;
			if (IsSubclassOfRawGeneric(type, typeof(SpatialMesh<>)))
				return false;
			return true;
		}

		static bool IsSubclassOfRawGeneric(Type subclass, Type generic)
		{
			while (subclass != null && subclass != typeof(object))
			{
				Type cur = subclass.IsGenericType ? subclass.GetGenericTypeDefinition() : subclass;
				if (generic == cur)
					return true;
				subclass = subclass.BaseType;
			}

			return false;
		}

		void OnDrawGizmos()
		{
			if (!drawGizmos)
				return;
			if (_polygon == null)
				return;
			Gizmos.color = gizmoColor;

			Points.DrawGizmo();
		}

		public override void OnDrawHandles()
		{
			bool changed = false;

			if (drawHandles)
			{
				Vector3 size = _polygon.Bounds.size;
				EasyHandles.fullObjectSize = Mathf.Max(1, (size.x + size.y + size.z) / 3f);
				EasyHandles.sizeMultiplier = 1;
				if (_polygon is IEasyHandleable handleableObject)
				{
					changed |= handleableObject.DrawHandles();
					_polygon = (IPolygon)handleableObject;
				}
			}

			if (changed)
				Updated?.Invoke();
		}
	}
}