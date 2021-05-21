#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorProperty<Vector3>), useForChildren: true)]
public class InspectorVector3Drawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var element = (IInspectorProperty<Vector3>) inspectorElement;
		Vector3 oldValue = element.GetValue(parentObject);
		Vector3 newValue = EditorGUI.Vector3Field(position, label, oldValue);

		if (Mathf.Abs(oldValue.x - newValue.x) < float.Epsilon &&
		    Mathf.Abs(oldValue.y - newValue.y) < float.Epsilon &&
			Mathf.Abs(oldValue.z - newValue.z) < float.Epsilon) return false; 

		element.SetValue(parentObject, newValue);
		return true;
	}
}
}
#endif