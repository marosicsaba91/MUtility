using UnityEngine;

namespace MUtility
{
    struct Pyramid : IDrawable
    {
        public UnityEngine.Vector3 basePos;
        public Quaternion rotation;
        public float fullHeight;
        public Vector2 baseRect;
        public float frustum;

        public UnityEngine.Vector3 Apex => basePos + BaseNormal * fullHeight;
        public LineSegment BalseLine => new LineSegment(basePos, Apex);
        public UnityEngine.Vector3 BaseNormal => (rotation * UnityEngine.Vector3.up).normalized;
        public UnityEngine.Vector3 BaseXDirection => (rotation * UnityEngine.Vector3.right).normalized * (baseRect.x * 0.5f);
        public UnityEngine.Vector3 BaseYDirection => (rotation * UnityEngine.Vector3.forward).normalized * (baseRect.y * 0.5f);

        UnityEngine.Vector3 BaseRightTop => basePos + BaseXDirection + BaseYDirection;
        UnityEngine.Vector3 BaseRightBottom => basePos + BaseXDirection - BaseYDirection;
        UnityEngine.Vector3 BaseLeftTop => basePos - BaseXDirection + BaseYDirection;
        UnityEngine.Vector3 BaseLeftBottom => basePos - BaseXDirection - BaseYDirection;

        UnityEngine.Vector3 FrustumRightTop => UnityEngine.Vector3.LerpUnclamped(Apex, BaseRightTop,  frustum);
        UnityEngine.Vector3 FrustumRightBottom => UnityEngine.Vector3.LerpUnclamped(Apex, BaseRightBottom, frustum);
        UnityEngine.Vector3 FrustumLeftTop => UnityEngine.Vector3.LerpUnclamped(Apex, BaseLeftTop, frustum);
        UnityEngine.Vector3 FrustumLeftBottom => UnityEngine.Vector3.LerpUnclamped(Apex, BaseLeftBottom, frustum);
        
        public Pyramid(UnityEngine.Vector3 basePos, Vector2 baseRect, Quaternion rotation, float fullHeight, float frustum = 0)
        {
            this.basePos = basePos;
            this.rotation = rotation;
            this.baseRect = baseRect;
            this.fullHeight = fullHeight;
            this.frustum = Mathf.Clamp01(frustum);
        }
        public Pyramid(Camera camera)
        {
            fullHeight = camera.farClipPlane;
            rotation = camera.transform.rotation * Quaternion.Euler(new UnityEngine.Vector3(-90,0,0));
            basePos = camera.transform.TransformPoint(new UnityEngine.Vector3(0,0,fullHeight));
            float alphaRad = camera.fieldOfView * Mathf.Deg2Rad * 0.5f;
            float h = 2f * Mathf.Tan(alphaRad) * fullHeight;
            float w = h * camera.aspect;
            baseRect = new Vector2(w, h);
            frustum = (camera.nearClipPlane) / fullHeight;
        }

        public Drawable Slice(Plain plain) => CubeSliceHelper.Slice(plain,
                FrustumLeftBottom,
                BaseLeftBottom,
                FrustumLeftTop,
                BaseLeftTop,
                FrustumRightBottom,
                BaseRightBottom,
                FrustumRightTop,
                BaseRightTop);
        

        public Drawable ToDrawable()
        {
            var ap = new UnityEngine.Vector3(0,fullHeight, 0);

            var p1 = new UnityEngine.Vector3(baseRect.x / 2f, 0, baseRect.y / 2f);
            var p2 = new UnityEngine.Vector3(baseRect.x / 2f, 0, -baseRect.y / 2f);
            var p3 = new UnityEngine.Vector3(-baseRect.x / 2f, 0, baseRect.y / 2f);
            var p4 = new UnityEngine.Vector3(-baseRect.x / 2f, 0, -baseRect.y / 2f);

            var d = new Drawable(
                new UnityEngine.Vector3[] { p1, p2, p4, p3, p1 },
                new UnityEngine.Vector3[] { p1, ap },
                new UnityEngine.Vector3[] { p2, ap },
                new UnityEngine.Vector3[] { p3, ap },
                new UnityEngine.Vector3[] { p4, ap });

            return d.Rotate(rotation).Offset(basePos);
        } 
    }
}
