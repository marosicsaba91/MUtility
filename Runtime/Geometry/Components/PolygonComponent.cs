using System;
using System.Collections.Generic;

using UnityEngine;

namespace MUtility
{

public class PolygonComponent : MonoBehaviourWithHandles
{ 
    [SerializeReference, TypePicker] IPolygon _polygon;
    internal void SetShapeType(Type type)
    {
        Type bt = typeof(IPolygon);
        if(!type.IsAbstract && bt.IsAssignableFrom(type))
            _polygon = (IPolygon)Activator.CreateInstance(type);
    }
    
    [Header("Gizmos")]
    [SerializeField] bool drawGizmos = true;
    [SerializeField] Color gizmoColor = Color.white;
    [SerializeField] bool drawHandles = true;
    
    public Type GetPolygonType() => _polygon?.GetType();

    void OnDrawGizmos()
    {
        if(!drawGizmos) return;

        if (_polygon == null) return;
        Gizmos.color = gizmoColor;

        IEnumerable<Vector3> points = _polygon.Points;
        
        points = points.Transform(transform); 
        points.DrawGizmo();
    } 

    public override void OnDrawHandles()
    {
        if(!drawHandles) return;
        
        if (_polygon is IRuntimeHandleable handleableObject)
            handleableObject.OnDrawHandles();
    }
}
}