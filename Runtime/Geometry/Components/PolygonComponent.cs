using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{

public class PolygonComponent : MonoBehaviourWithHandles
{ 
    public enum Space 
    {
        Self,
        World,
        Center,
        TransformWithOffset
    }
    
    [SerializeReference, TypePicker(nameof(TypeFilter))] IPolygon _polygon;
    internal void SetShapeType(Type type)
    {
        Type bt = typeof(IPolygon);
        if(!type.IsAbstract && bt.IsAssignableFrom(type))
            _polygon = (IPolygon)Activator.CreateInstance(type);
    }

    [SerializeField] Space space;
    [SerializeField, ShowIf(nameof(HaveLocalPose))] Vector3 position;
    [SerializeField, ShowIf(nameof(HaveLocalPose)), EulerAngles] Quaternion rotation;
    
    bool HaveLocalPose => space == Space.World || space == Space.TransformWithOffset; 

    public override bool InSelfSpace => space == Space.Self || space == Space.TransformWithOffset;
    public IEnumerable<Vector3> Points 
    { 
        get
        {
            if(_polygon == null) yield break;
            
            
            IEnumerable<Vector3> points = _polygon.Points;
        
            if(HaveLocalPose) 
                points = points.Rotate(rotation).Offset(position);
        
            if (InSelfSpace)
                points = points.Transform(transform);
            
            foreach (Vector3 point in points)
                yield return point;
        }
    }

    public IPolygon Polygon => _polygon;


    [Header("Gizmos")]
    [SerializeField] bool drawGizmos = true;
    [SerializeField] Color gizmoColor = Color.white;
    [SerializeField] bool drawHandles = true;
    
    public Type GetPolygonType() => _polygon?.GetType();
    
    bool TypeFilter(Type type)
    {
        if (IsSubclassOfRawGeneric(type, typeof(SpacialPolygon<>)))
            return false;
        if (IsSubclassOfRawGeneric(type, typeof(SpacialMesh<>)))
            return false;
        return true;
    }
    
    static bool IsSubclassOfRawGeneric( Type subclass, Type generic)
    {
        while (subclass != null && subclass != typeof(object)) {
            Type cur = subclass.IsGenericType ? subclass.GetGenericTypeDefinition() : subclass;
            if (generic == cur)
                return true;
            subclass = subclass.BaseType;
        }
        return false;
    }
    

    void OnDrawGizmos()
    {
        if(!drawGizmos) return;
        if (_polygon == null) return;
        
        Gizmos.color = gizmoColor;
        
        Points.DrawGizmo();
    } 

    public static void OnDrawHandles<T>(ref T shape, ref Vector3 position, ref Quaternion rotation)
    { 
        
        if (shape is IEasyHandleable handleable)
        {
            rotation.Normalize();
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);

            EasyHandles.PushMatrix(matrix);
            handleable.OnDrawHandles();
            shape = (T)handleable;
            EasyHandles.PopMatrix(matrix);
        }
        
        position = EasyHandles.PositionHandle(position, rotation, EasyHandles.Shape.SmallPosition);
        rotation = EasyHandles.RotationHandle(position, rotation);
    }
    
    public override void OnDrawHandles()
    {
        if(!drawHandles) return;
        
        if (_polygon is IEasyHandleable handleableObject)
        {
            rotation.Normalize(); 
            if (HaveLocalPose)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
                EasyHandles.PushMatrix(matrix);
                handleableObject.OnDrawHandles();
                _polygon = (IPolygon) handleableObject;
                EasyHandles.PopMatrix(matrix);
            }
            else
            {
                handleableObject.OnDrawHandles();
                _polygon = (IPolygon) handleableObject;
            }
        }

        if (HaveLocalPose)
        {
            position = EasyHandles.PositionHandle(position, rotation, EasyHandles.Shape.SmallPosition);
            rotation = EasyHandles.RotationHandle(position, rotation);
        }
    }
}
}