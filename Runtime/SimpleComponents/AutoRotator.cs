#if UNITY_EDITOR
using UnityEditor;
#endif

using MUtility;
using UnityEngine;

public class AutoRotator : MonoBehaviour
{
	// TODO: Rigidbody Support
	public enum HandleType
	{
		None,
		Point,
		Rotation
	}

	[SerializeField] float angularSpeed = 360f;
	[SerializeField, ValueChangeCallback(nameof(RegenWorldRotation))] Space rotationSpace = Space.World;
	[SerializeField, EnableIf(nameof(NoHandle))] Vector3 rotationAxis = Vector3.up;

	[Space]
	[SerializeField, ValueChangeCallback(nameof(RegenWorldRotation))] HandleType handle = HandleType.None;
	[SerializeField] bool showGizmos = true;
	[SerializeField, ShowIf(nameof(showGizmos)), Range(0, 1)] float gizmoAlpha = 1;
	[SerializeField, ShowIf(nameof(showGizmos)), Range(-1, 1),] float gizmoDarkness = 0;
	[SerializeField, ShowIf(nameof(showGizmos)), Min(0)] float gizmoRadius = 1;

	bool NoHandle => handle == HandleType.None;

	public Vector3 RotationAxisWorld
	{
		get => rotationSpace == Space.World
			? rotationAxis
			: transform.TransformDirection(rotationAxis);

		set
		{
			const float epsilon = 0.0001f;
			if (Vector3.Distance(RotationAxisWorld, value) < epsilon)
				return;

			rotationAxis = rotationSpace == Space.World ? value : transform.InverseTransformDirection(value);

			if (rotationAxis.x < epsilon && rotationAxis.x > -epsilon)
				rotationAxis.x = 0;
			if (rotationAxis.y < epsilon && rotationAxis.y > -epsilon)
				rotationAxis.y = 0;
			if (rotationAxis.z < epsilon && rotationAxis.z > -epsilon)
				rotationAxis.z = 0;

			if (rotationAxis == Vector3.zero || !IsVector3Valid(rotationAxis))
				rotationAxis = Vector3.up;
		}
	}

	Quaternion _worldRotation;

	public Quaternion WorldRotation
	{
		get => _worldRotation;

		set
		{
			_worldRotation = value;
			rotationAxis = rotationSpace == Space.World
				? (_worldRotation * Vector3.forward) * rotationAxis.magnitude
				: transform.InverseTransformDirection(_worldRotation * Vector3.forward) * rotationAxis.magnitude;
		}
	}

	void RegenWorldRotation()
	{
		_worldRotation = rotationSpace == Space.World
			? Quaternion.LookRotation(rotationAxis)
			: Quaternion.LookRotation(transform.TransformDirection(rotationAxis));
	}

	public Space RotationSpace { get => rotationSpace; set => rotationSpace = value; }
	public HandleType Handle => handle;

	static bool IsVector3Valid(Vector3 v)
	{
		return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z);
	}

	void Update()
	{
		transform.Rotate(rotationAxis, angularSpeed * Time.deltaTime, rotationSpace);
	}


	Color GizmoColor
	{
		get
		{
			//Color based on rotation axis
			float x = Mathf.Abs(rotationAxis.x);
			float y = Mathf.Abs(rotationAxis.y);
			float z = Mathf.Abs(rotationAxis.z);
			float sum = x + y + z;
			float r = x / sum;
			float g = y / sum;
			float b = z / sum;

			if (gizmoDarkness < 0)
			{
				r *= 1 + gizmoDarkness;
				g *= 1 + gizmoDarkness;
				b *= 1 + gizmoDarkness;
			}
			else if (gizmoDarkness > 0)
			{
				r += (1 - r) * gizmoDarkness;
				g += (1 - g) * gizmoDarkness;
				b += (1 - b) * gizmoDarkness;
			}

			return new Color(r, g, b, gizmoAlpha);
		}
	}

	void OnDrawGizmosSelected()
	{
		if (!showGizmos)
			return;

		Vector3 center = transform.position;

		Vector3 axis = RotationAxisWorld;
		Vector3 endA = center + axis;
		Vector3 endB = center - axis;

		Arrow arrow = new Arrow(endB, endA - endB, axis.magnitude * 2);
		arrow.DrawGizmo(GizmoColor);

		// Spiral

		float height;
		if (Mathf.Abs(angularSpeed) < 270)
			height = 0;
		else if (Mathf.Abs(angularSpeed) < 360)
		{
			height = (Mathf.Abs(angularSpeed) - 270) / 90;
			height = Mathf.Pow(height, 2);
		}
		else
			height = Mathf.Abs(angularSpeed) * gizmoRadius / 360;

		height = Mathf.Clamp(height, 0.001f, rotationAxis.magnitude * 2);
		axis.Normalize();
		Vector3 start = center - axis * height / 2;
		Vector3 end = start + axis * height;

		DrawSpiral(angularSpeed, start, end, gizmoRadius, true);
	}

	void DrawSpiral(float fullAngle, Vector3 start, Vector3 end, float radius, bool drawArrow = false)
	{
		float absFullAngle = Mathf.Abs(fullAngle);

		float baseAngleStep =
			absFullAngle < 15 ? 1 :
			absFullAngle < 90 ? 5 :
			absFullAngle < 360 ? 10 : 15;

		Vector3 axis = end - start;
		Vector3 axisDir = axis.normalized;

		int pointCount = Mathf.Max(5, (int)(absFullAngle / baseAngleStep + 1f));
		float angleStep = Mathf.Deg2Rad * fullAngle / pointCount;

		VectorExtensions.LeftHand(axisDir, out Vector3 d1, out Vector3 d2);

		Vector3 axisStep = axis / pointCount;
		Vector3 startPoint = start;
		Vector3 pointBeforeLast = end;
		Vector3 lastPoint = end;
		for (int i = 0; i < pointCount; i++)
		{
			float a = angleStep * i;
			Vector3 horizontal = Mathf.Cos(a) * d1 + Mathf.Sin(a) * d2;
			Vector3 point = startPoint + (radius * horizontal) + (i * axisStep);

			if (i != 0)
				Gizmos.DrawLine(lastPoint, point);

			pointBeforeLast = lastPoint;
			lastPoint = point;
		}

		if (drawArrow)
		{
			Vector3 dir = lastPoint - pointBeforeLast;

			Arrow arrow = new Arrow(lastPoint, dir, 0);
			arrow.ToDrawable();
			arrow.DrawGizmo(Gizmos.color);
		}
	}
}

// Custom Editor
#if UNITY_EDITOR
[CustomEditor(typeof(AutoRotator))]
public class AutoRotatorEditor : Editor
{
	void OnSceneGUI()
	{
		AutoRotator autoRotator = (AutoRotator)target;
		Undo.RecordObject(autoRotator, "Auto Rotator Setup Changed");

		if (autoRotator.Handle == AutoRotator.HandleType.Point)
		{
			Transform transform = autoRotator.transform;
			Vector3 center = transform.position;

			Vector3 axis = autoRotator.RotationAxisWorld;
			Vector3 endA = center + axis;

			Vector3 newEndPoint = Handles.PositionHandle(endA, Quaternion.identity);
			autoRotator.RotationAxisWorld = newEndPoint - center;
		}
		else if (autoRotator.Handle == AutoRotator.HandleType.Rotation)
		{
			Transform transform = autoRotator.transform;
			Vector3 center = transform.position;
			Quaternion rotation = autoRotator.WorldRotation;
			Quaternion newRotation = Handles.DoRotationHandle(rotation, center);

			autoRotator.WorldRotation = newRotation;
		}
	}
}
#endif