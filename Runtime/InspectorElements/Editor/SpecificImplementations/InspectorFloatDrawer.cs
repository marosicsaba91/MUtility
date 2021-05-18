#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorFloatProperty), useForChildren: true)]
public class InspectorFloatDrawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var floatElement = (IInspectorFloatProperty) inspectorElement;
		float oldValue = floatElement.GetValue(parentObject);
		float newValue;
		if (floatElement.TryGetRange(parentObject, out float min, out float max))
		{
			newValue = EditorGUI.Slider(
				position,
				label,
				oldValue,
				min,
				max);
		}
		else
			newValue = EditorGUI.FloatField(position, label, oldValue);
		
		if (Math.Abs(newValue - oldValue) < float.Epsilon) return false;

		floatElement.SetValue(parentObject, newValue);
		return true;
	}
}
}
#endif