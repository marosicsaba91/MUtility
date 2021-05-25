#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorProperty<Vector3>), useForChildren: true)]
public class InspectorVector3Drawer :  InspectorPropertyDrawer<Vector3, IInspectorProperty<Vector3>>
{
	protected override Vector3 GetValue(
		Rect position, GUIContent label, Vector3 oldValue,
		IInspectorProperty<Vector3> inspectorProperty, object parentObject) =>
		EditorGUI.Vector3Field(position, label, oldValue);
}
}
#endif