using System;
using UnityEngine;

namespace MUtility
{
public static class VectorExtensions
{
    public static Vector2 Round(this Vector2 vector) =>
        new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    

    public static UnityEngine.Vector3 Round(this UnityEngine.Vector3 vector) =>
        new UnityEngine.Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    

    public static Vector2 ToVector2(this UnityEngine.Vector3 vector, Axis3D deletableAxis)
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

    public static UnityEngine.Vector3 ToVector3(this Vector2 vector, float newValue, Axis3D newAxis)
    {
        switch (newAxis)
        {
            case Axis3D.X: return new UnityEngine.Vector3(newValue, vector.x, vector.y);
            case Axis3D.Y: return new UnityEngine.Vector3(vector.x, newValue, vector.y);
            case Axis3D.Z: return new UnityEngine.Vector3(vector.x, vector.y, newValue);
            default:
                throw new ArgumentOutOfRangeException(nameof(newAxis), newAxis, null);
        }
    }

    public static UnityEngine.Vector3 MultiplyAllAxis(this UnityEngine.Vector3 vector, UnityEngine.Vector3 multiplier) =>
        new UnityEngine.Vector3(vector.x * multiplier.x, vector.y * multiplier.y, vector.z * multiplier.z);
    

    public static Vector2 MultiplyAllAxis(this Vector2 vector, Vector2 multiplier) =>
        new Vector2(vector.x * multiplier.x, vector.y * multiplier.y);
    

    public static float GetAngle(this Vector2 original)
    {
        float angle = Vector2.Angle(Vector2.right, original);
        return original.y > 0 ? angle : 360f - angle;
    }

    public static float GetAngle(this UnityEngine.Vector3 original) =>
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

    public static UnityEngine.Vector3 Clamp(this UnityEngine.Vector3 input, float min, float max)
    {
        return new UnityEngine.Vector3(
            input.x < min ? min : input.x > max ? max : input.x,
            input.y < min ? min : input.y > max ? max : input.y,
            input.z < min ? min : input.z > max ? max : input.z);
    }

    public static UnityEngine.Vector3 Clamp(this UnityEngine.Vector3 input, UnityEngine.Vector3 min, UnityEngine.Vector3 max)
    {
        return new UnityEngine.Vector3(
            input.x < min.x ? min.x : input.x > max.x ? max.x : input.x,
            input.y < min.y ? min.y : input.y > max.y ? max.y : input.y,
            input.z < min.z ? min.z : input.z > max.y ? max.z : input.z);
    }

    public static Vector2 Clamp01(this Vector2 areaPos) => areaPos.Clamp(0, 1);

    public static UnityEngine.Vector3 Clamp01(this UnityEngine.Vector3 areaPos) => areaPos.Clamp(0, 1);
}
}