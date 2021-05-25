#if UNITY_EDITOR
using UnityEditor;
using UnityEngine; 

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorProperty<Vector2>), useForChildren: true)]
public class InspectorVector2Drawer :  InspectorPropertyDrawer<Vector2, IInspectorProperty<Vector2>>
{
	protected override Vector2 GetValue(
		Rect position, GUIContent label, Vector2 oldValue,
		IInspectorProperty<Vector2> inspectorProperty, object parentObject) =>
		EditorGUI.Vector2Field(position, label, oldValue);
}
}
#endif