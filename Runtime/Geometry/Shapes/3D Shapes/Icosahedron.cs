/*

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
public struct Icosahedron : IDrawable
{
    public float side;

    const float circumscribedSphereRadiusToSide = 0.9510565163f;

    public float CircumscribedSphereRadius
    {
        get => circumscribedSphereRadiusToSide * side;
        set => side = value / circumscribedSphereRadiusToSide;
    }

    public float Surface => 8.66025404f * (side * side);

    public float Volume => 2.18169499f * (side * side * side);

    public Drawable ToDrawable()
    {
        List<Vector3> vertList = new List<Vector3>();
        float t = (1f + Mathf.Sqrt(5f)) / 2f;
        float radius = CircumscribedSphereRadius;

        vertList.Add(new Vector3(-1f, t, 0f).normalized * radius);
        vertList.Add(new Vector3(1f, t, 0f).normalized * radius);
        vertList.Add(new Vector3(-1f, -t, 0f).normalized * radius);
        vertList.Add(new Vector3(1f, -t, 0f).normalized * radius);

        vertList.Add(new Vector3(0f, -1f, t).normalized * radius);
        vertList.Add(new Vector3(0f, 1f, t).normalized * radius);
        vertList.Add(new Vector3(0f, -1f, -t).normalized * radius);
        vertList.Add(new Vector3(0f, 1f, -t).normalized * radius);

        vertList.Add(new Vector3(t, 0f, -1f).normalized * radius);
        vertList.Add(new Vector3(t, 0f, 1f).normalized * radius);
        vertList.Add(new Vector3(-t, 0f, -1f).normalized * radius);
        vertList.Add(new Vector3(-t, 0f, 1f).normalized * radius);


        // create 20 triangles of the icosahedron
        List<Vector3Int> faces = new List<Vector3Int>();

        // 5 faces around point 0
        faces.Add(new Vector3Int(0, 11, 5));
        faces.Add(new Vector3Int(0, 5, 1));
        faces.Add(new Vector3Int(0, 1, 7));
        faces.Add(new Vector3Int(0, 7, 10));
        faces.Add(new Vector3Int(0, 10, 11));

        // 5 adjacent faces 
        faces.Add(new Vector3Int(1, 5, 9));
        faces.Add(new Vector3Int(5, 11, 4));
        faces.Add(new Vector3Int(11, 10, 2));
        faces.Add(new Vector3Int(10, 7, 6));
        faces.Add(new Vector3Int(7, 1, 8));

        // 5 faces around point 3
        faces.Add(new Vector3Int(3, 9, 4));
        faces.Add(new Vector3Int(3, 4, 2));
        faces.Add(new Vector3Int(3, 2, 6));
        faces.Add(new Vector3Int(3, 6, 8));
        faces.Add(new Vector3Int(3, 8, 9));

        // 5 adjacent faces 
        faces.Add(new Vector3Int(4, 9, 5));
        faces.Add(new Vector3Int(2, 4, 11));
        faces.Add(new Vector3Int(6, 2, 10));
        faces.Add(new Vector3Int(8, 6, 7));
        faces.Add(new Vector3Int(9, 8, 1));
        
        return faces.ToDrawable(vertList);
    }
}


// [Serializable] public class SpatialIcosahedron : SpatialMesh<Icosahedron> { }

}
*/