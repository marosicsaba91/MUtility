﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
 
namespace MUtility
{
public abstract class InspectorElementDrawer : PropertyDrawer
{
	Color _tempColor;
	bool _tempEnabled;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		object containerObject = property.GetObjectWithProperty();
		var inspectorElement = (IInspectorElement) property.GetObjectOfProperty();
		Object serializedObject = property.serializedObject.targetObject;

		if (!inspectorElement.IsVisible(containerObject))
			return -EditorGUIUtility.standardVerticalSpacing;

		float? height = GetPropertyHeight(inspectorElement, property, containerObject, serializedObject, label);
		return
			height == null ? base.GetPropertyHeight(property, label) :
			height.Value <= 0 ? -EditorGUIUtility.standardVerticalSpacing
			: height.Value;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		_tempColor = GUI.color;
		_tempEnabled = GUI.enabled;
		var inspectorElement = (IInspectorElement) property.GetObjectOfProperty();
		object containerObject = property.GetObjectWithProperty();
		if (!inspectorElement.IsVisible(containerObject)) return;

		Object serializedObject = property.serializedObject.targetObject;
		label = EditorGUI.BeginProperty(position, label, property);

		if (!inspectorElement.IsEnabled(containerObject))
			GUI.enabled = false;

		GUI.color = inspectorElement.GetGUIColor(containerObject);
		string labelText = inspectorElement.Text(containerObject);
		if (!string.IsNullOrEmpty(labelText))
			label.text = labelText;

		Object[] changeableObjects =
			inspectorElement.ChangeableObjects(containerObject) ?? new[] {serializedObject};

		Undo.RecordObjects(changeableObjects, "Inspector Property Changed!");

		bool stateChanged = Draw(position, inspectorElement, property, containerObject, serializedObject, label);

		if (stateChanged)
		{
			foreach (Object undoable in changeableObjects)
				EditorUtility.SetDirty(undoable);
		}

		GUI.color = _tempColor;
		GUI.enabled = _tempEnabled;
		EditorGUI.EndProperty();
	}

	public abstract bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label);
	

	// if under 0 => default property Height
	public virtual float? GetPropertyHeight(
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
		=> null;
}
}
#endif