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
			Vector3 cubeSizeVec = new(size * cubeSize, size * cubeSize, size * cubeSize);
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


		public static Pose Lerp(this Pose a, Pose b, float t)
		{
			Vector3 position = Vector3.Lerp(a.position, b.position, t);
			Quaternion rotation = Quaternion.Lerp(a.rotation, b.rotation, t);
			return new Pose(position, rotation);
		}

		public static Vector3 TransformVector(this Pose pose, Vector3 localVector)
			=> pose.rotation * localVector;

		public static Vector3 TransformPosition(this Pose pose, Vector3 localVector)
			=> pose.rotation * localVector + pose.position; 
	}
}