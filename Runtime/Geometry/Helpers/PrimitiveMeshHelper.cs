using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
public static class PrimitiveMeshHelper
{
    static readonly Dictionary<PrimitiveType, Mesh> primitiveMeshes = new Dictionary<PrimitiveType, Mesh>();

    static GameObject CreatePrimitive(PrimitiveType type, bool withCollider)
    {
        if (withCollider)
        {
            return GameObject.CreatePrimitive(type);
        }

        var gameObject = new GameObject(type.ToString());
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = GetPrimitiveMesh(type);
        gameObject.AddComponent<MeshRenderer>();

        return gameObject;
    }

    public static Mesh GetPrimitiveMesh(PrimitiveType type)
    {
        if (!primitiveMeshes.ContainsKey(type))
            CreatePrimitiveMesh(type);

        return primitiveMeshes[type];
    }

    static void CreatePrimitiveMesh(PrimitiveType type)
    {
        var gameObject = GameObject.CreatePrimitive(type);
        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        Object.DestroyImmediate(gameObject); 
        primitiveMeshes[type] = mesh; 
    }
}
}