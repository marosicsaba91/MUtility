#if UNITY_EDITOR
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
		bool isMin = inspectorProperty.TryGetMin(parentObject, out float min);
		bool isMax = inspectorProperty.TryGetMax(parentObject, out float max);
		
		if (isMin && isMax)
			return EditorGUI.Slider(position, label, oldValue, min, max);
		
		float value = EditorGUI.FloatField(position, label, oldValue);
		if (isMin)
			return Mathf.Max(value, min);
		if (isMax)
			return Mathf.Min(value, min);

		return value;
	}
}
}
#endif