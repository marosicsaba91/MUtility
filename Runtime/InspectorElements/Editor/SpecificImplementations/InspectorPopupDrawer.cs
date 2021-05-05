#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IInspectorPopup), useForChildren: true)]
public class InspectorPopupDrawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var popup = (IInspectorPopup) inspectorElement;
		int oldValue = popup.GetSelectedElement(parentObject);
		string[] options = popup.Elements(parentObject).Select(o => o.ToString()).ToArray();
		int newValue = EditorGUI.Popup(position, label.text, oldValue, options);

		if (oldValue == newValue) return false;

		popup.SetSelectedElement(parentObject, newValue);
		return true;
	}
}
#endif