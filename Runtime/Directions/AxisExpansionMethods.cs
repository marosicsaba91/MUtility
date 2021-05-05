using System;
using UnityEngine;

namespace MUtility
{
    public static class AxisExpansionMethods
    {
        public static Vector3 ToPositiveVector(this Axis3D axis)
        {
            switch (axis)
            {
                case Axis3D.X: return Vector3.right;
                case Axis3D.Y: return Vector3.up;
                case Axis3D.Z: return Vector3.forward;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public static GeneralDirection3D ToPositiveDirection(this Axis3D axis)
        {
            switch (axis)
            {
                case Axis3D.X: return GeneralDirection3D.Right;
                case Axis3D.Y: return GeneralDirection3D.Up;
                case Axis3D.Z: return GeneralDirection3D.Forward;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public static Vector3 ToNegativeVector(this Axis3D axis)
        {
            switch (axis)
            {
                case Axis3D.X: return Vector3.left;
                case Axis3D.Y: return Vector3.down;
                case Axis3D.Z: return Vector3.back;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public static GeneralDirection3D ToNegativeDirection(this Axis3D axis)
        {
            switch (axis)
            {
                case Axis3D.X: return GeneralDirection3D.Left;
                case Axis3D.Y: return GeneralDirection3D.Down;
                case Axis3D.Z: return GeneralDirection3D.Back;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public static Color GetAxisColor(this Axis3D axis)
        {
            switch (axis)
            {
                case Axis3D.X: return Color.red;
                case Axis3D.Y: return Color.green;
                case Axis3D.Z: return Color.blue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }
    }
}