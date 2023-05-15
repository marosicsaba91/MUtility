using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{
	public enum ForcedAxisMode
	{
		Non,
		Line,
		Plane
	}

	public static partial class EasyHandles
	{
		static readonly Color _focusedColorMultiplier = new Color(0.9f, 0.9f, 0.9f, 0.75f);
		static readonly Color _colorSelectedMultiplier = new Color(0.8f, 0.8f, 0.8f, 0.5f);
		static Color MultiplyColor(Color a, Color b) => new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);

		public static Color color = Color.white;
		public static float fullObjectSize = 1;
		public static float sizeMultiplier = 1;

		static Matrix4x4 _drawingSpaceMatrix = Matrix4x4.identity;

		public static void PushMatrix(Matrix4x4 matrix) =>
			_drawingSpaceMatrix *= matrix;

		public static void PopMatrix(Matrix4x4 matrix) =>
			_drawingSpaceMatrix *= matrix.inverse;


		public static void ClearSettings()
		{
			color = Color.white;
			fullObjectSize = 1;
			sizeMultiplier = 1;
			_drawingSpaceMatrix = Matrix4x4.identity;
		}

		public static Vector3 PositionHandle(Vector3 position, Shape shape = Shape.Dot) =>
			PositionHandle(position, Quaternion.identity, shape);

		public static Vector3 PositionHandle(Vector3 position, Vector3 axis, Shape shape)
		{
			return PositionHandle(position, Quaternion.LookRotation(axis), ForcedAxisMode.Line, shape);
		}

		public static bool TryPositionHandle(Vector3 position, Vector3 axis, Shape shape, out Vector3 result)
		{
			result = PositionHandle(position, Quaternion.LookRotation(axis), ForcedAxisMode.Line, shape);
			return position != result;
		}

		public static Vector3 PositionHandle(Vector3 position, Vector3 axis, ForcedAxisMode mode = ForcedAxisMode.Line, Shape shape = Shape.Cone)
		{
			if (mode == ForcedAxisMode.Plane)
				return PositionHandle(position, Quaternion.LookRotation(axis), mode, shape);

			return PositionHandle(position, Quaternion.LookRotation(axis), mode, shape);
		}

		public static Vector3 PositionHandle(Vector3 position, Quaternion rotation, Shape shape = Shape.Cube)
		{
			return PositionHandle(position, rotation, ForcedAxisMode.Non, shape);
		}

		public static bool Handle(Vector3 position, Quaternion rotation, Shape shape = Shape.Cube)
		{
			//return PositionHandle(position, rotation, ForcedAxisMode.Non, shape);
			return false;
		}

		public static void DrawLine(Vector3 start, Vector3 end)
		{
#if UNITY_EDITOR
			Handles.DrawLine(start, end);
#endif
		}

		// ----------- PRIVATE -------------

		static Vector3 PositionHandle(Vector3 position, Quaternion rotation, ForcedAxisMode mode, Shape shape)
		{
#if UNITY_EDITOR

			Handles.matrix = _drawingSpaceMatrix;

			if (shape == Shape.FullPosition)
				position = Handles.PositionHandle(position, rotation);
			else
			{
				if (shape == Shape.SmallPosition)
					position = DrawSmallPositionHandle(position, rotation, mode);
				else
				{
					Color selectedColor = MultiplyColor(color, _colorSelectedMultiplier);
					Color focusedColor = MultiplyColor(color, _focusedColorMultiplier);

					float offset = shape == Shape.Cone ? 0.5f : 0;
					position = DrawPositionHandle(
						position, rotation, shape, mode,
						color, selectedColor, focusedColor, offset);
				}
			}
#endif
			return position;
		}

		static Vector3 DrawSmallPositionHandle(Vector3 position, Quaternion rotation, ForcedAxisMode mode)
		{
			Vector3 Arrow(Vector3 dir, Color dirColor)
			{
				Quaternion rot = rotation * Quaternion.Euler(dir * 90);
				Color c = MultiplyColor(color, dirColor);
				Color sc = MultiplyColor(c, _colorSelectedMultiplier);
				Color fc = MultiplyColor(c, _focusedColorMultiplier);
				return DrawPositionHandle(position, rot, Shape.Cone, ForcedAxisMode.Line, c, sc, fc, 1.5f);
			}

			Color red = new Color(1, 0.5f, 0.5f);
			Color green = new Color(0.5f, 1, 0.5f);
			Color blue = new Color(0.5f, 0.5f, 1);

			Vector3 px = Arrow(Vector3.up, red);
			Vector3 py = Arrow(Vector3.left, green);
			Vector3 pz = Arrow(Vector3.forward, blue);
			Color selectedColor = MultiplyColor(color, _colorSelectedMultiplier);
			Color focusedColor = MultiplyColor(color, _focusedColorMultiplier);
			Vector3 p = DrawPositionHandle(position, rotation, Shape.Cube, mode, color, selectedColor, focusedColor);

			if (px != position)
				return px;
			if (py != position)
				return py;
			if (pz != position)
				return pz;
			return p;
		}

		static HandleEvent _lastEvent = HandleEvent.None;
		public static HandleEvent LastEvent => _lastEvent;

		static Vector3 DrawPositionHandle(
			Vector3 position, Quaternion rotation, Shape shape, ForcedAxisMode mode,
			Color color, Color focusedColor, Color selectedColor, float offset = 0)
		{
#if UNITY_EDITOR
			float size = 0.05f * Mathf.Abs(fullObjectSize);
			size *= sizeMultiplier;
			Handles.CapFunction capFunction = shape.ToCapFunction();
			size *= shape.GetSizeMultiplier();

			Vector3 inputPosition = position;

			if (offset != 0)
				position += rotation * Vector3.forward * (size * offset);

			HandleResult result = AdvancedHandles.Handle(
				position,
				rotation,
				size,
				capFunction,
				color,
				focusedColor, selectedColor);

			position = result.newPosition;
			_lastEvent = result.handleEvent;

			if (offset != 0)
				position += rotation * Vector3.back * (size * offset);

			if (mode == ForcedAxisMode.Line)
				position =
					Vector3.Dot(position - inputPosition, rotation * Vector3.forward) * (rotation * Vector3.forward)
					+ inputPosition;
			else if (mode == ForcedAxisMode.Plane)
				position =
					Vector3.ProjectOnPlane(position - inputPosition, rotation * Vector3.forward)
					+ inputPosition;


#endif
			return position;
		}

		public static Quaternion RotationHandle(Vector3 position, Quaternion rotation)
		{
#if UNITY_EDITOR
			rotation = Handles.RotationHandle(rotation, position);
#endif
			return rotation;
		}


		public static void RecordObject(Object objectToRecord)
		{
#if UNITY_EDITOR
			Undo.RecordObject(objectToRecord, "Handle Changed");
#endif
		}
	}
}