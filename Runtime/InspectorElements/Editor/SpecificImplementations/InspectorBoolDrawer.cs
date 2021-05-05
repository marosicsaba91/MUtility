#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine; 
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(IInspectorProperty<bool>), useForChildren: true)]
public class InspectorBoolDrawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var boolElement = (IInspectorProperty<bool>) inspectorElement;
		bool oldValue = boolElement.GetValue(parentObject);
		bool newValue = EditorGUI.Toggle(position, label, oldValue);
		
		if (newValue == oldValue) return false;

		boolElement.SetValue(parentObject, newValue);
		return true;
	}
}
#endif
