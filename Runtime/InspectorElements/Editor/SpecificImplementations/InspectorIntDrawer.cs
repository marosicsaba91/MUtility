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
		if (inspectorProperty.TryGetRange(parentObject, out int min, out int max))
			return EditorGUI.IntSlider(position, label, oldValue, min, max);

		return EditorGUI.IntField(position, label, oldValue);
	}
}
}
#endif