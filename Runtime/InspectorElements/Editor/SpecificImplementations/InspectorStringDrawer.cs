#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine; 
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(IInspectorProperty<string>), useForChildren: true)]
public class InspectorStringDrawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var stringElement = (IInspectorProperty<string>) inspectorElement;
		string oldValue = stringElement.GetValue(parentObject);
		string newValue = EditorGUI.TextField(position, label, oldValue);
		
		if (newValue == oldValue) return false;

		stringElement.SetValue(parentObject, newValue);
		return true;
	}
}
#endif