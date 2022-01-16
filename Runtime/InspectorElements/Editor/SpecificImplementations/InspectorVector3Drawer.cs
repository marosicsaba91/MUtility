#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorProperty<UnityEngine.Vector3>), useForChildren: true)]
public class InspectorVector3Drawer :  InspectorPropertyDrawer<UnityEngine.Vector3, IInspectorProperty<UnityEngine.Vector3>>
{
	protected override UnityEngine.Vector3 GetValue(
		Rect position, GUIContent label, UnityEngine.Vector3 oldValue,
		IInspectorProperty<UnityEngine.Vector3> inspectorProperty, object parentObject) =>
		EditorGUI.Vector3Field(position, label, oldValue);
}
}
#endif