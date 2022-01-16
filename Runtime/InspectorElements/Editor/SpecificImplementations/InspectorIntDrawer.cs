#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorInt), useForChildren: true)]
public class InspectorIntDrawer :  InspectorPropertyDrawer<int, IInspectorInt>
{
	protected override int GetValue(
		Rect position, GUIContent label, int oldValue,
		IInspectorInt inspectorProperty, object parentObject)
	{
		bool isMin = inspectorProperty.TryGetMin(parentObject, out int min);
		bool isMax = inspectorProperty.TryGetMax(parentObject, out int max);
		
		if (isMin && isMax)
			return EditorGUI.IntSlider(position, label, oldValue, min, max);
		
		int value = EditorGUI.IntField(position, label, oldValue);
		if (isMin)
			return Mathf.Max(value, min);
		if (isMax)
			return Mathf.Min(value, min);

		return value;
	}
}
}
#endif