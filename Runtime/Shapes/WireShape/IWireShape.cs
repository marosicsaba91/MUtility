using UnityEngine;

namespace MUtility
{
	public interface IWireShape
	{
		WireShape ToWireShape();
	}

	public static class WireShapeExtensions
	{
		public static void DrawGizmo(this IWireShape self) => self.ToWireShape().DrawGizmo();
		public static void DrawDebug(this IWireShape self) => self.ToWireShape().DrawDebug();

		public static void DrawGizmo(this IWireShape self, Color color) => self.ToWireShape().DrawGizmo(color);
		public static void DrawDebug(this IWireShape self, Color color) => self.ToWireShape().DrawDebug(color);

		public static void DrawGizmo(this IWireShape self, Transform transform) =>
			self.ToWireShape().DrawGizmo(transform);
		public static void DrawGizmo(this IWireShape self, Transform transform, Color color) =>
			self.ToWireShape().DrawGizmo(transform, color);
		public static void DrawDebug(this IWireShape self, Transform transform) =>
			self.ToWireShape().DrawDebug(transform);
		public static void DrawDebug(this IWireShape self, Transform transform, Color color) =>
			self.ToWireShape().DrawDebug(transform, color);

		public static void DrawGizmo(this IWireShape self, Transform transform, Space space) =>
			self.ToWireShape().DrawGizmo(transform, space);
		public static void DrawGizmo(this IWireShape self, Transform transform, Space space, Color color) =>
			self.ToWireShape().DrawGizmo(transform, space, color);
		public static void DrawDebug(this IWireShape self, Transform transform, Space space) =>
			self.ToWireShape().DrawDebug(transform, space);
		public static void DrawDebug(this IWireShape self, Transform transform, Space space, Color color) =>
			self.ToWireShape().DrawDebug(transform, space, color);

		public static void DrawGizmo(this IWireShape self, Vector3 offset, Color color) =>
			self.ToWireShape().DrawGizmo(offset, color);
		public static void DrawDebug(this IWireShape self, Vector3 offset, Color color) =>
			self.ToWireShape().DrawDebug(offset, color);

		public static void DrawGizmo(this IWireShape self, Quaternion rotate, Color color) =>
			self.ToWireShape().DrawGizmo(rotate, color);
		public static void DrawDebug(this IWireShape self, Quaternion rotate, Color color) =>
			self.ToWireShape().DrawDebug(rotate, color);

		public static void DrawGizmo(this IWireShape self, Vector3 offset, Quaternion rotate, float scale, Color color) =>
			self.ToWireShape().DrawGizmo(offset, rotate, scale, color);
		public static void DrawDebug(this IWireShape self, Vector3 offset, Quaternion rotate, float scale, Color color) =>
			self.ToWireShape().DrawDebug(offset, rotate, scale, color);

		public static void DrawGizmo(this IWireShape self, Vector3 offset, Quaternion rotate, Vector3 scale, Color color) =>
			self.ToWireShape().DrawGizmo(offset, rotate, scale, color);
		public static void DrawDebug(this IWireShape self, Vector3 offset, Quaternion rotate, Vector3 scale, Color color) =>
			self.ToWireShape().DrawDebug(offset, rotate, scale, color);
	}
}