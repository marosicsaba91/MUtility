using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
[Serializable]
    public struct Arrow : IDrawable, IHandleable
    {
        public UnityEngine.Vector3 origin;
        public UnityEngine.Vector3 direction;
        public float magnitude;
        public UnityEngine.Vector3? normalDirection;
        
        public UnityEngine.Vector3 DirectionNormalized
        {
            get => direction.normalized;
            set => direction = value == UnityEngine.Vector3.zero ? UnityEngine.Vector3.zero : value.normalized;
        }

        public Arrow(Ray ray, UnityEngine.Vector3? normalDirection = null) => Init(
	        ray, 
	        normalDirection,
	        out origin, 
	        out direction, 
	        out magnitude,
	        out this.normalDirection);
        
        
        static void Init(Ray ray, UnityEngine.Vector3? normalDirectionValue, 
	        out UnityEngine.Vector3 origin, out  UnityEngine.Vector3 directionNormalized, out float magnitude, out UnityEngine.Vector3? normalDirection)
        {
            origin = ray.origin;
            magnitude = ray.direction.magnitude;

            directionNormalized = ray.direction == UnityEngine.Vector3.zero ? UnityEngine.Vector3.up : ray.direction.normalized;
            normalDirection = normalDirectionValue;
        }

        public Arrow(UnityEngine.Vector3 origin, UnityEngine.Vector3 direction, UnityEngine.Vector3? normalDirection = null)
        {
	        this.origin = origin;
	        magnitude = direction.magnitude;

	        if (direction == UnityEngine.Vector3.zero)
		        this.direction = UnityEngine.Vector3.up;
	        else
		        this.direction = direction.normalized;
	        this.normalDirection = normalDirection;
        }


        public Arrow(UnityEngine.Vector3 origin, UnityEngine.Vector3 direction, float magnitude, UnityEngine.Vector3? normalDirection = null )
        {
	        this.origin = origin;
	        this.magnitude = magnitude;

	        if (direction == UnityEngine.Vector3.zero)
		        this.direction = UnityEngine.Vector3.up;
	        else
		        this.direction = direction.normalized;
	        this.normalDirection = normalDirection;
        }
        
        UnityEngine.Vector3 Head
        {
            get => origin + DirectionNormalized * magnitude;
            set
            {
                UnityEngine.Vector3 vec = value - origin;
                DirectionNormalized = vec.normalized;
                magnitude = vec.magnitude;
            }
        }

        public UnityEngine.Vector3 DirectionVector
        {
            get => DirectionNormalized * magnitude;
            set
            {
                DirectionNormalized = value;
                magnitude = value.magnitude;
            }
        }
        Ray ToRay => new Ray(origin, DirectionVector);

        public Drawable ToDrawable() => ToDrawable(DrawingSettings.Default); 
        public Drawable ToDrawable( DrawingSettings visuals)
        {
	        UnityEngine.Vector3 normal;
	        if (normalDirection == null)
	        {
		        Camera c = Camera.current;
		        normal = c == null ? UnityEngine.Vector3.forward : (c.transform.position - origin).normalized;
	        }
	        else
		        normal = normalDirection.Value;
	        
            if (magnitude < 0)
                magnitude = 0;

            float arrowHeadLength = magnitude == 0 ? visuals.maxArrowHeadLength : Mathf.Min(visuals.maxArrowHeadLength, magnitude);
            bool showArrowHead = visuals.maxArrowHeadLength > 0 && visuals.arrowHeadAngleInDeg > 0 && visuals.arrowHeadAngleInDeg < 180;
            bool headOnly = showArrowHead && (magnitude <= visuals.maxArrowHeadLength || magnitude==0);
            bool showBaseLine = visuals.baseLineLength > 0 && !headOnly;
            bool showMiddleLine = !headOnly;

            var polygonIndex = 0;
            int polygonCount =
                (showArrowHead ? 1 : 0) +
                (showBaseLine ? 1 : 0) +
                (showMiddleLine ? 1 : 0);
            var polygons = new UnityEngine.Vector3[polygonCount][];

            UnityEngine.Vector3 normalizedDirection = DirectionNormalized;
            float middleLength = magnitude - (showArrowHead ? arrowHeadLength : 0);
            UnityEngine.Vector3 backOfHead = origin + middleLength * normalizedDirection;
            UnityEngine.Vector3 perpendicular = UnityEngine.Vector3.Cross(normalizedDirection, normal).normalized;
            if (perpendicular == UnityEngine.Vector3.zero)
            {
                perpendicular = Math.Abs(Mathf.Abs(normalizedDirection.x) - 1) > 0.0001f ? UnityEngine.Vector3.right : UnityEngine.Vector3.up;
            }

            if (showArrowHead)
            {
                float arrowHeadBack = arrowHeadLength * 2 * Mathf.Tan(visuals.arrowHeadAngleInDeg * Mathf.Deg2Rad / 2f);
                UnityEngine.Vector3 halfArrowBackVec = perpendicular * (0.5f * arrowHeadBack);
                UnityEngine.Vector3 headA = backOfHead + halfArrowBackVec;
                UnityEngine.Vector3 headB = backOfHead - halfArrowBackVec;
                polygons[polygonIndex] = new[] { Head, headA, headB, Head };
                polygonIndex++;
            }

            if (showBaseLine)
            {
                UnityEngine.Vector3 halfBaseLineVec = perpendicular * (0.5f * visuals.baseLineLength);
                UnityEngine.Vector3 baseLineA = origin + halfBaseLineVec;
                UnityEngine.Vector3 baseLineB = origin - halfBaseLineVec;
                polygons[polygonIndex] = new[] { baseLineA, baseLineB };
                polygonIndex++;
            }

            if (showMiddleLine)
	            polygons[polygonIndex] = new[] { origin, backOfHead };

            return new Drawable(polygons);
        }

        public List<HandlePoint> GetHandles() =>
            new List<HandlePoint> {
                new HandlePoint(origin, HandlePoint.Shape.Rectangle),
                new HandlePoint(Head) };
        
        public void SetHandle(int index, UnityEngine.Vector3 point)
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
                maxArrowHeadLength = arrowHeadLength;
                arrowHeadAngleInDeg = arrowHeadAngle;
            }
        }
    }
}