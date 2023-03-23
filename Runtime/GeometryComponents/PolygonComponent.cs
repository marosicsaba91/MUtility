using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MUtility
{
    public class PolygonComponent : MonoBehaviourWithHandles
    {
        public enum PolygonSpace
        {
            Self,
            World,
            SelfWithPose,
            WorldWithPose,
        }

        [SerializeReference, TypePicker(nameof(TypeFilter))] IPolygon _polygon;
        internal void SetShapeType(Type type)
        {
            Type bt = typeof(IPolygon);
            if (!type.IsAbstract && bt.IsAssignableFrom(type))
                _polygon = (IPolygon)Activator.CreateInstance(type);
        }

        [SerializeField] PolygonSpace space;
        [SerializeField, ShowIf(nameof(HavePose))] Vector3 position;
        [SerializeField, ShowIf(nameof(HavePose))] Quaternion rotation;
        [SerializeField, ShowIf(nameof(HavePose))] bool drawPoseHandles = true;

        public event Action Updated;
        public PolygonSpace Space => space;

        bool HavePose =>
            space == PolygonSpace.WorldWithPose ||
            space == PolygonSpace.SelfWithPose;

        public override bool DrawHandlesInSelfSpace => space == PolygonSpace.Self || space == PolygonSpace.SelfWithPose;

        public IEnumerable<Vector3> Points
        {
            get
            {
                if (_polygon == null) yield break;


                IEnumerable<Vector3> points = PointsLocal; 

                if (DrawHandlesInSelfSpace)
                    points = points.Transform(transform);

                foreach (Vector3 point in points)
                    yield return point;
            }
        }


        public IEnumerable<Vector3> PointsLocal  
        {
            get
            {
                if (_polygon == null) yield break;

                IEnumerable<Vector3> points = _polygon.Points;

                if (HavePose)
                    points = points.Rotate(rotation).Offset(position);

                foreach (Vector3 point in points)
                    yield return point;
            }
        }


        void OnValidate()
        {
            if (_polygon is Spline spline)
                spline.SetDirty();
            Updated?.Invoke();
        }

        public IPolygon Polygon => _polygon;


        [Header("Gizmos")]

        [SerializeField] bool drawGizmos = true;
        [SerializeField] Color gizmoColor = Color.white;
        [FormerlySerializedAs("drawHandles")][SerializeField] bool drawShapeHandles = true;

        public Type GetPolygonType() => _polygon?.GetType();

        bool TypeFilter(Type type)
        {
            if (IsSubclassOfRawGeneric(type, typeof(SpacialPolygon<>)))
                return false;
            if (IsSubclassOfRawGeneric(type, typeof(SpacialMesh<>)))
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
            if (!drawGizmos) return;
            if (_polygon == null) return;
            Gizmos.color = gizmoColor;

            Points.DrawGizmo();
        }

        public override void OnDrawHandles()
        {
            bool changed = false;

            if (drawShapeHandles)
            {
                EasyHandles.fullObjectSize = 10f;
                if (_polygon is IEasyHandleable handleableObject)
                {
                    rotation.Normalize();
                    if (HavePose)
                    {
                        Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
                        EasyHandles.PushMatrix(matrix);
                        changed |= handleableObject.DrawHandles();
                        _polygon = (IPolygon)handleableObject;
                        EasyHandles.PopMatrix(matrix);
                    }
                    else
                    {
                        changed |= handleableObject.DrawHandles();
                        _polygon = (IPolygon)handleableObject;
                    }
                }
            }

            if (HavePose && drawPoseHandles)
            {
                Vector3 newPosition = EasyHandles.PositionHandle(position, rotation, EasyHandles.Shape.SmallPosition);
                Quaternion newRotation = EasyHandles.RotationHandle(position, rotation);

                if (position != newPosition || rotation != newRotation)
                {
                    position = newPosition;
                    rotation = newRotation;
                    changed = true;
                }
            }

            if (changed)
                Updated?.Invoke();
        }
    }
}