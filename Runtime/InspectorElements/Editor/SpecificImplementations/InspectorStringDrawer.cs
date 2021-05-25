#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorProperty<string>), useForChildren: true)]
public class InspectorStringDrawer :  InspectorPropertyDrawer<string, IInspectorProperty<string>>
{
	protected override string GetValue(
		Rect position, GUIContent label, string oldValue,
		IInspectorProperty<string> inspectorProperty, object parentObject) =>
		EditorGUI.TextField(position, label, oldValue);
}
}
#endif