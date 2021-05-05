#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility.SpecificImplementations
{
[CustomPropertyDrawer(typeof(IInspectorInt), useForChildren: true)]
public class InspectorIntDrawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var intElement = (IInspectorInt) inspectorElement;
		int oldValue = intElement.GetValue(parentObject);
		int newValue;
		if (intElement.TryGetRange(parentObject, out int min, out int max))
		{
			newValue = EditorGUI.IntSlider(
				position,
				label,
				oldValue,
				min,
				max);
		}
		else
			newValue = EditorGUI.IntField(position, label, oldValue);
		
		if (Math.Abs(newValue - oldValue) < float.Epsilon) return false;

		intElement.SetValue(parentObject, newValue);
		return true;
	}
}
}
#endif