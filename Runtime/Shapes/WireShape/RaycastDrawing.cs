using UnityEngine;

namespace MUtility
{
	public static class RaycastDrawing
	{
		// REYCAST & DRAW
		public static RaycastHit2D RaycastAndDraw2D(
			Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance,
			LayerMask mask, Color color)
		{
			Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
			return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
		}

		public static RaycastHit? RaycastAndDraw3D(
			Vector3 rayOriginPoint, Vector3 rayDirection, float rayDistance, LayerMask mask,
			Color color)
		{
			Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance);
			if (Physics.Raycast(rayOriginPoint, rayDirection, out RaycastHit hit, rayDistance, mask))
				return hit;
			return null;
		}
	}
}