using System;
using UnityEngine;

namespace MUtility
{
public static class VectorExtensions
{
    public static Vector2 Round(this Vector2 vector) =>
        new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    

    public static Vector3 Round(this Vector3 vector) =>
        new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    

    public static Vector2 ToVector2(this Vector3 vector, Axis3D deletableAxis)
    {
        switch (deletableAxis)
        {
            case Axis3D.X: return new Vector2(vector.y, vector.z);
            case Axis3D.Y: return new Vector2(vector.x, vector.z);
            case Axis3D.Z: return new Vector2(vector.x, vector.y);
            default:
                throw new ArgumentOutOfRangeException(nameof(deletableAxis), deletableAxis, null);
        }
    }

    public static Vector3 ToVector3(this Vector2 vector, float newValue, Axis3D newAxis)
    {
        switch (newAxis)
        {
            case Axis3D.X: return new Vector3(newValue, vector.x, vector.y);
            case Axis3D.Y: return new Vector3(vector.x, newValue, vector.y);
            case Axis3D.Z: return new Vector3(vector.x, vector.y, newValue);
            default:
                throw new ArgumentOutOfRangeException(nameof(newAxis), newAxis, null);
        }
    }

    public static Vector3 MultiplyAllAxis(this Vector3 vector, Vector3 multiplier) =>
        new Vector3(vector.x * multiplier.x, vector.y * multiplier.y, vector.z * multiplier.z);
    

    public static Vector2 MultiplyAllAxis(this Vector2 vector, Vector2 multiplier) =>
        new Vector2(vector.x * multiplier.x, vector.y * multiplier.y);
    

    public static float GetAngle(this Vector2 original)
    {
        float angle = Vector2.Angle(Vector2.right, original);
        return original.y > 0 ? angle : 360f - angle;
    }

    public static float GetAngle(this Vector3 original) =>
        ((Vector2) original).GetAngle();
    

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = cos * tx - sin * ty;
        v.y = sin * tx + cos * ty;
        return v;
    }


    // Clamp vector
    public static Vector2 Clamp(this Vector2 input, float min, float max)
    {
        return new Vector2(
            input.x < min ? min : input.x > max ? max : input.x,
            input.y < min ? min : input.y > max ? max : input.y);
    }

    public static Vector2 Clamp(this Vector2 input, Vector2 min, Vector2 max)
    {
        return new Vector2(
            input.x < min.x ? min.x : input.x > max.x ? max.x : input.x,
            input.y < min.y ? min.y : input.y > max.y ? max.y : input.y);
    }

    public static Vector3 Clamp(this Vector3 input, float min, float max)
    {
        return new Vector3(
            input.x < min ? min : input.x > max ? max : input.x,
            input.y < min ? min : input.y > max ? max : input.y,
            input.z < min ? min : input.z > max ? max : input.z);
    }

    public static Vector3 Clamp(this Vector3 input, Vector3 min, Vector3 max)
    {
        return new Vector3(
            input.x < min.x ? min.x : input.x > max.x ? max.x : input.x,
            input.y < min.y ? min.y : input.y > max.y ? max.y : input.y,
            input.z < min.z ? min.z : input.z > max.y ? max.z : input.z);
    }

    public static Vector2 Clamp01(this Vector2 areaPos) => areaPos.Clamp(0, 1);

    public static Vector3 Clamp01(this Vector3 areaPos) => areaPos.Clamp(0, 1);
    
    public static void LeftHand(this Vector3 thumb, Vector3 index, out Vector3 middle)
    {
        middle = Vector3.Cross(thumb, index).normalized; 
    }
    
    public static void LeftHand(this Vector3 thumb, out Vector3 index, Vector3 middle)
    {
        index = Vector3.Cross(middle, thumb).normalized; 
    }
    
    public static void LeftHand(out Vector3 thumb, Vector3 index, Vector3 middle)
    {
        thumb = Vector3.Cross(index, middle).normalized; 
    }
    
    public static void LeftHand(this Vector3 thumb, out Vector3 index, out Vector3 middle)
    {
        thumb.Normalize();
        index = thumb != Vector3.forward && thumb != Vector3.back
            ? Vector3.Cross(Vector3.forward, thumb).normalized
            : Vector3.Cross(new Vector3(-.001f,-.001f,1), thumb).normalized;
        
        LeftHand( thumb,  index, out middle);
    }
    
    public static void LeftHand(out Vector3 thumb, Vector3 index, out Vector3 middle)
    {
        index.Normalize();
        middle = index != Vector3.forward && index != Vector3.back
            ? Vector3.Cross(Vector3.forward, index).normalized
            : Vector3.Cross(new Vector3(-.001f,-.001f,1), index).normalized;
        
        LeftHand( out thumb,  index, middle);
    }
    
    public static void LeftHand(out Vector3 thumb, out Vector3 index, Vector3 middle)
    {
        middle.Normalize();
        thumb = middle != Vector3.forward && middle != Vector3.back
            ? Vector3.Cross(Vector3.forward, middle).normalized
            : Vector3.Cross(new Vector3(-.001f,-.001f,1), middle).normalized;
        
        LeftHand( thumb,  out index, middle);
    }
}
}