using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public struct Pyramid : IDrawable
	{
		public Vector3 basePos;
		public Quaternion rotation;
		public float fullHeight;
		public Vector2 baseRect;
		public float frustum;

		public Vector3 Apex => basePos + BaseNormal * fullHeight;
		public LineSegment BalseLine => new LineSegment(basePos, Apex);
		public Vector3 BaseNormal => (rotation * Vector3.up).normalized;
		public Vector3 BaseXDirection => (rotation * Vector3.right).normalized * (baseRect.x * 0.5f);
		public Vector3 BaseYDirection => (rotation * Vector3.forward).normalized * (baseRect.y * 0.5f);

		Vector3 BaseRightTop => basePos + BaseXDirection + BaseYDirection;
		Vector3 BaseRightBottom => basePos + BaseXDirection - BaseYDirection;
		Vector3 BaseLeftTop => basePos - BaseXDirection + BaseYDirection;
		Vector3 BaseLeftBottom => basePos - BaseXDirection - BaseYDirection;

		Vector3 FrustumRightTop => Vector3.LerpUnclamped(Apex, BaseRightTop, frustum);
		Vector3 FrustumRightBottom => Vector3.LerpUnclamped(Apex, BaseRightBottom, frustum);
		Vector3 FrustumLeftTop => Vector3.LerpUnclamped(Apex, BaseLeftTop, frustum);
		Vector3 FrustumLeftBottom => Vector3.LerpUnclamped(Apex, BaseLeftBottom, frustum);

		public Pyramid(Vector3 basePos, Vector2 baseRect, Quaternion rotation, float fullHeight, float frustum = 0)
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
			rotation = camera.transform.rotation * Quaternion.Euler(new Vector3(-90, 0, 0));
			basePos = camera.transform.TransformPoint(new Vector3(0, 0, fullHeight));
			float alphaRad = camera.fieldOfView * Mathf.Deg2Rad * 0.5f;
			float h = 2f * Mathf.Tan(alphaRad) * fullHeight;
			float w = h * camera.aspect;
			baseRect = new Vector2(w, h);
			frustum = camera.nearClipPlane / fullHeight;
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
			Vector3 ap = new Vector3(0, fullHeight, 0);

			Vector3 p1 = new Vector3(baseRect.x / 2f, 0, baseRect.y / 2f);
			Vector3 p2 = new Vector3(baseRect.x / 2f, 0, -baseRect.y / 2f);
			Vector3 p3 = new Vector3(-baseRect.x / 2f, 0, baseRect.y / 2f);
			Vector3 p4 = new Vector3(-baseRect.x / 2f, 0, -baseRect.y / 2f);

			Drawable d = new Drawable(
				new Vector3[] { p1, p2, p4, p3, p1 },
				new Vector3[] { p1, ap },
				new Vector3[] { p2, ap },
				new Vector3[] { p3, ap },
				new Vector3[] { p4, ap });

			return d.GetRotated(rotation).GetTranslated(basePos);
		}

		public float Surface => 0; // TODO
		public float Volume => 0; // TODO
	}

	// [Serializable] public class SpatialPyramid : SpatialMesh<Pyramid> { }
}
