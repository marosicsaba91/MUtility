#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorProperty<Enum>), useForChildren: true)]
public class InspectorEnumDrawer : InspectorPropertyDrawer<Enum, IInspectorProperty<Enum>>
{
	protected override Enum GetValue(
		Rect position, GUIContent label, Enum oldValue, 
		IInspectorProperty<Enum> inspectorProperty, object parentObject) =>
		EditorGUI.EnumPopup(position, label, oldValue);
}
}
#endif