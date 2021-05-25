#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IFloatProperty), useForChildren: true)]
public class InspectorFloatDrawer :  InspectorPropertyDrawer<float, IFloatProperty>
{
 

	protected override float GetValue(
		Rect position, GUIContent label, float oldValue,
		IFloatProperty inspectorProperty, object parentObject)
	{
		if (inspectorProperty.TryGetRange(parentObject, out float min, out float max))
			return EditorGUI.Slider(position, label, oldValue, min, max);

		return EditorGUI.FloatField(position, label, oldValue);
	}
}
}
#endif