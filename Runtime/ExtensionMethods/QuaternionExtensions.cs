using System;
using UnityEngine;

namespace Utility
{
public static class QuaternionExtensions
{
    public static bool CompareApproximately(this Quaternion one, Quaternion other)
    {
        const float tolerance = 0.0000001f;
        if (Math.Abs(one.x - other.x) > tolerance) return false;
        if (Math.Abs(one.y - other.y) > tolerance) return false;
        if (Math.Abs(one.z - other.z) > tolerance) return false;
        if (Math.Abs(one.w - other.w) > tolerance) return false; 
        return true;
    }
}
}