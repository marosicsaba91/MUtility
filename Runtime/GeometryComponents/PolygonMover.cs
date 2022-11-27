/*
using MUtility;
using UnityEngine;

[RequireComponent(typeof(PolygonComponent))]
public class PolygonMover : MonoBehaviour
{    
    [SerializeField] PolygonComponent polygon;
 
     void OnValidate()
     {
         if (polygon == null)
             polygon = GetComponent<PolygonComponent>();
     }
     
    public enum MovementType
    {
        Transform,
        Rigidbody,
        Rigidbody2D, 
    }
    
    public enum RigidBodyMovementType
    {
        Position, 
        Velocity,
        Force, 
    }
    
    public enum PathMovementType
    {
        Loop,
        PingPong,
        Once
    }
    
    [SerializeField] MovementType movementType = MovementType.Transform;
    
    [SerializeField, ShowIf(nameof(ShowRigidbody))] Rigidbody rigidBody;
    [SerializeField, ShowIf(nameof(ShowRigidbody2D))] Rigidbody2D rigidBody2D;
    [SerializeField, ShowIf(nameof(ShowRigidbodySettings))] RigidBodyMovementType rigidBodyMovementType;
    
    bool ShowRigidbody => polygon!=null && movementType == MovementType.Rigidbody;
    bool ShowRigidbody2D => polygon!=null && movementType  == MovementType.Rigidbody2D;
    
    bool ShowRigidbodySettings => polygon!=null && movementType != MovementType.Transform;

    [SerializeField] bool rotate;
    
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
    
    //[Space]
    //[SerializeField] MovementDirection direction = MovementDirection.Forward;
    //[SerializeField] SpeedType speedType;
    //[SerializeField] float speed = 1f;
    
    //[Space]
    //[SerializeField] bool faceDirection;
    //[SerializeField] MovementType rotation = MovementType.Transform;
}
*/