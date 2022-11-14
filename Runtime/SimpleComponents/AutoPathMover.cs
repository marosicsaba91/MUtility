/*
using System;
using System.Collections.Generic;
using MUtility;
using UnityEngine;

public class AutoPathMover : MonoBehaviour
{
    public enum Dimensions
    {
        _2Dimension,
        _3Dimension,
    }
    
    public enum MovementType
    {
        Transform,
        RigidbodyPosition, 
        RigidbodyVelocity,
        RigidbodyForce, 
    }
    
    
    [SerializeField] Dimensions dimensions = Dimensions._3Dimension;
    [SerializeField] MovementType movementType = MovementType.Transform;
    [SerializeField, ShowIf(nameof(ShowAxis))] Axis3D axisToIgnore = Axis3D.Z;
    [SerializeField, ShowIf(nameof(ShowRigidbody))] new Rigidbody rigidbody;
    [SerializeField, ShowIf(nameof(ShowRigidbody2D))] new Rigidbody2D rigidbody2D;    
    bool ShowAxis => movementType == MovementType.Transform && dimensions == Dimensions._2Dimension;
    bool ShowRigidbody => movementType != MovementType.Transform && dimensions == Dimensions._3Dimension;
    bool ShowRigidbody2D => movementType != MovementType.Transform && dimensions == Dimensions._2Dimension;
    
    public enum SpaceType
    {
        World,
        Parent
    }
    [Space] 
    [SerializeField] Path path;
    [SerializeField] SpaceType pathSpace = SpaceType.Parent;
    [SerializeField] Transform anchor = null;
    
    
    public enum SpeedType
    {
        Ratio,
        Distance,
        Uniform,
        ByPoint
    }
    public enum MovementDirection
    {
        Stop,
        Forward,
        Backward
    }
    [Space]
    [SerializeField] MovementDirection direction = MovementDirection.Forward;
    [SerializeField] SpeedType speedType;
    [SerializeField] float speed = 1f;
    
    [Space]
    [SerializeField] bool faceDirection;
    [SerializeField] MovementType rotation = MovementType.Transform;
    
    [Space]
    [SerializeField] DisplayMember displayMember = new DisplayMember(nameof(Text) );
    
    string Text => "TODO";
}

[Serializable]
class Path
{   
    public enum PathType
    {
        Loop,
        PingPong,
        Once
    }
    
    public enum Interpolation
    {
        Linear,
        CatmullRom,
        Hermite
    }

    [SerializeField] List<PathPoint> path = new List<PathPoint>();
    [SerializeField] bool isLoop = false;
    [SerializeField] PathType pathType;
    [SerializeField] Interpolation interpolation;
    
}

[Serializable]
struct PathPoint
{
    public Vector3 position;
    public Vector3 normal;
    public float multiplier;
}
*/