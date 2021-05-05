using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    // Kb. ugyanaz mint a Ray, de rajzolásra.

    [Serializable]
    public struct Arrow : IDrawable, IHandleable
    {
        public Vector3 origin;
        [SerializeField] Vector3 normalDirection;
        
        public Vector3 NormalDirection
        {
            get => normalDirection.normalized;
            set => normalDirection = value == Vector3.zero ? Vector3.zero : value.normalized;
        }
        public float magnitude;
        
        
        public Arrow(Ray ray)
        {
            origin = ray.origin;
            magnitude = ray.direction.magnitude;

            if (ray.direction == Vector3.zero)
                normalDirection = Vector3.up;
            else
                normalDirection = ray.direction.normalized;
        }
        
        public Arrow(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            magnitude = direction.magnitude;

            if (direction == Vector3.zero)
                normalDirection = Vector3.up;
            else
                normalDirection = direction.normalized;
        }


        public Arrow(Vector3 origin, Vector3 direction, float magnitude)
        {
            this.origin = origin;
            this.magnitude = magnitude;

            if (direction == Vector3.zero)
                normalDirection = Vector3.up;
            else
                normalDirection = direction.normalized;
        }

        Vector3 Head
        {
            get => origin + (NormalDirection * magnitude);
            set
            {
                Vector2 vec = value - origin;
                NormalDirection = vec.normalized;
                magnitude = vec.magnitude;
            }
        }

        public Vector3 DirectionVector
        {
            get => NormalDirection * magnitude;
            set
            {
                NormalDirection = value;
                magnitude = value.magnitude;
            }
        }
        Ray ToRay => new Ray(origin, DirectionVector);

        public Drawable ToDrawable() => ToDrawable(Vector3.forward, DrawingSettings.Default);

        public Drawable ToDrawable(Vector3 normal) => ToDrawable(normal, DrawingSettings.Default);

        public Drawable ToDrawable(DrawingSettings visulas) => ToDrawable(Vector3.forward, visulas);

        public Drawable ToDrawable(Vector3 normal, DrawingSettings visulas)
        {
            if (normal == Vector3.zero) { normal = Vector3.forward; }
            if (magnitude < 0)
                magnitude = 0;

            float arrowHeadLength = magnitude == 0 ? visulas.maxArrowHeadLength : Mathf.Min(visulas.maxArrowHeadLength, magnitude);
            bool showArrowHead = visulas.maxArrowHeadLength > 0 && visulas.arrowHeadAngleInDeg > 0 && visulas.arrowHeadAngleInDeg < 180;
            bool headOnly = showArrowHead && (magnitude <= visulas.maxArrowHeadLength || magnitude==0);
            bool showBaseLine = visulas.baseLineLength > 0 && !headOnly;
            bool showMiddleLine = !headOnly;

            int polygonIndex = 0;
            int polygonCount =
                (showArrowHead ? 1 : 0) +
                (showBaseLine ? 1 : 0) +
                (showMiddleLine ? 1 : 0);
            Vector3[][] polygons = new Vector3[polygonCount][];

            var normalDir = NormalDirection;
            float middleLength = magnitude - (showArrowHead ? arrowHeadLength : 0);
            Vector3 backOfHead = origin + (middleLength * normalDir);
            Vector3 perpendicular = Vector3.Cross(normalDir, normal).normalized;
            if (perpendicular == Vector3.zero)
            {
                perpendicular = Math.Abs(Mathf.Abs(normalDir.x) - 1) > 0.0001f ? Vector3.right : Vector3.up;
            }

            if (showArrowHead)
            {
                float arrowHeadBack = arrowHeadLength * 2 * Mathf.Tan(visulas.arrowHeadAngleInDeg * Mathf.Deg2Rad / 2f);
                Vector3 halfArrowBackVec = perpendicular * (0.5f * arrowHeadBack);
                Vector3 headA = backOfHead + halfArrowBackVec;
                Vector3 headB = backOfHead - halfArrowBackVec;
                polygons[polygonIndex] = new Vector3[] { Head, headA, headB, Head };
                polygonIndex++;
            }

            if (showBaseLine)
            {
                Vector3 halfBaseLineVec = perpendicular * 0.5f * visulas.baseLineLength;
                Vector3 baseLineA = origin + halfBaseLineVec;
                Vector3 baseLineB = origin - halfBaseLineVec;
                polygons[polygonIndex] = new Vector3[] { baseLineA, baseLineB };
                polygonIndex++;
            }

            if (showMiddleLine)
            {
                polygons[polygonIndex] = new Vector3[] { origin, backOfHead };
                polygonIndex++;
            }

            return new Drawable(polygons);
        }

        public List<HandlePoint> GetHandles() =>
            new List<HandlePoint> {
                new HandlePoint(origin, HandlePoint.Shape.Rectangle),
                new HandlePoint(Head, HandlePoint.Shape.Circle) };
        
        public void SetHandle(int index, Vector3 point)
        {
            if (index == 0)
                origin = point;
            else
                Head = point;
        } 

        [Serializable]
        public struct DrawingSettings
        {
            const float defaultBaseLineLength =0.2f;
            const float defaultHeadLength = 0.1f;
            const float defaultHeadAngle = 90f;

            public static DrawingSettings Default => new DrawingSettings(defaultBaseLineLength, defaultHeadLength, defaultHeadAngle);

            public float baseLineLength;
            public float maxArrowHeadLength;
            public float arrowHeadAngleInDeg;

            public DrawingSettings(float baseLineLength, float arrowHeadLength, float arrowHeadAngle)
            {
                this.baseLineLength = baseLineLength;
                this.maxArrowHeadLength = arrowHeadLength;
                this.arrowHeadAngleInDeg = arrowHeadAngle;
            }
        }
    }
}