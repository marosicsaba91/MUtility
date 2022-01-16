using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct HermiteCurve : IDrawable
{
    const int defaultDrawingPointsOnSegment = 25;

    [Serializable]
    public struct Node
    {
        public enum EditingStyle
        {
            Free,
            Sharp,
            Smooth,
            Equal,
            Auto
        }

        public UnityEngine.Vector3 position;
        public UnityEngine.Vector3 speedIn;
        public UnityEngine.Vector3 speedOut;
        public EditingStyle editingStyle;
    }

    public bool isClosed;
    public List<Node> nodes;


    public HermiteCurve(bool isClosed, params Node[] nodes)
    {
        this.nodes = new List<Node>(nodes);
        this.isClosed = isClosed;
    }

    public HermiteCurve(params Node[] nodes)
    {
        this.nodes = new List<Node>(nodes);
        isClosed = false;
    }

    int SegmentCount => isClosed ? nodes.Count : nodes.Count - 1;

    public UnityEngine.Vector3 Evaluate(float time)
    {
        int count = nodes.Count;
        if (count == 0) return UnityEngine.Vector3.zero;
        int index0 = Mathf.FloorToInt(time);
        int index1 = Mathf.CeilToInt(time);

        if (isClosed)
        {
            index0 = MathHelper.Mod(index0, count);
            index1 = MathHelper.Mod(index1, count);
        }
        else
        {
            if (index0 < 0) return nodes[0].position;
            if (index1 > count - 1) return nodes[count - 1].position;
        }

        if (index0 == index1) return nodes[index0].position;

        Node node0 = nodes[index0];
        Node node1 = nodes[index1];
        UnityEngine.Vector3 p0 = node0.position;
        UnityEngine.Vector3 p1 = node1.position;
        UnityEngine.Vector3 v0 = node0.speedOut;
        UnityEngine.Vector3 v1 = node1.speedIn;

        float t = time % 1;
        float t2 = t * t;
        float t3 = t * t2;

        return
            (2 * t3 - 3 * t2 + 1) * p0 +
            (t3 - 2 * t2 + t) * v0 +
            (-2 * t3 + 3 * t2) * p1 +
            (t3 - t2) * v1;
    }

    public Drawable ToDrawable() => ToDrawable(defaultDrawingPointsOnSegment);

    public Drawable ToDrawable(int drawingPointsOnSegment) => new Drawable(new List<UnityEngine.Vector3[]>
    {
        ToVectorArray(drawingPointsOnSegment)
    });

    public UnityEngine.Vector3[] ToVectorArray() => ToVectorArray(defaultDrawingPointsOnSegment);

    public UnityEngine.Vector3[] ToVectorArray(int drawingPointsOnSegment)
    {
        if (nodes == null) return default;
        int nodeCount = nodes.Count;
        if (nodeCount == 0) return default;

        int curvePointCount = drawingPointsOnSegment * SegmentCount + 1;
        var points = new UnityEngine.Vector3[curvePointCount];
        for (var i = 0; i < curvePointCount; i++)
        {
            float t = (float) i / drawingPointsOnSegment;
            points[i] = Evaluate(t);
        }

        return points;
    }
}
}