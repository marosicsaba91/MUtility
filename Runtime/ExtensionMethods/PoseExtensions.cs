using UnityEngine;

namespace MUtility
{
	public static class PoseExtensions
	{
		public static void DrawGizmo(this Pose pose, float size = 1) => DrawGizmo(pose, Color.white, size);
		public static void DrawGizmo(this Pose pose, Color color, float size = 1)
		{
			Quaternion r = pose.rotation;
			Color tempColor = Gizmos.color;

			Mesh cubeMesh = PrimitiveMeshHelper.GetPrimitiveMesh(PrimitiveType.Cube);

			const float cubeSize = 0.35f;
			var cubeSizeVec = new Vector3(size * cubeSize, size * cubeSize, size * cubeSize);
			Gizmos.color = color;
			Gizmos.DrawMesh(cubeMesh, pose.position, pose.rotation, cubeSizeVec);
			DrawCylinder(Vector3.right, Color.red);
			DrawCylinder(Vector3.up, Color.green);
			DrawCylinder(Vector3.forward, Color.blue);


			Gizmos.color = tempColor;

			void DrawCylinder(Vector3 up, Color lineColor)
			{
				Gizmos.color = lineColor;
				up = r * up;
				Vector3 start = pose.position + size * cubeSize * 0.5f * up;
				Vector3 end = pose.position + up * size;
				Gizmos.DrawLine(start, end);
			}
		}
	}
}