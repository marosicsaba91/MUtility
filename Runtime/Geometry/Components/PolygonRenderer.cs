using System;
using System.Linq;
using UnityEngine;

namespace MUtility
{

[ExecuteAlways]
[RequireComponent(typeof(PolygonComponent))]
public class PolygonRenderer : MonoBehaviour
{
    [SerializeField] PolygonComponent polygonComponent;
    [SerializeField] LineRenderer lineRenderer;

    void OnValidate()
    {
        if (polygonComponent == null)
            polygonComponent = GetComponent<PolygonComponent>();
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }
    
    IPolygon _lastPolygon;

    void Update()
    {
        if(polygonComponent == null || lineRenderer == null)
            return;

        if (polygonComponent.Polygon != _lastPolygon)
        {
            UpdateLine();
            _lastPolygon = polygonComponent.Polygon;
        }
        
        
        UpdateLine();
    }

    public void UpdateLine()
    {
        Vector3[] points = polygonComponent.Points.ToArray();
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        lineRenderer.useWorldSpace = true;
    }
}
}