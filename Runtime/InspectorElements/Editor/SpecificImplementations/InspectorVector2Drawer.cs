#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine; 
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(IInspectorProperty<Vector2>), useForChildren: true)]
public class InspectorVector2Drawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var boolElement = (IInspectorProperty<Vector2>) inspectorElement;
		Vector2 oldValue = boolElement.GetValue(parentObject);
		Vector2 newValue = EditorGUI.Vector2Field(position, label, oldValue);

		if (Mathf.Abs(oldValue.x - newValue.x) < float.Epsilon &&
		    Mathf.Abs(oldValue.y - newValue.y) < float.Epsilon) return false; 

		boolElement.SetValue(parentObject, newValue);
		return true;
	}
}
#endif