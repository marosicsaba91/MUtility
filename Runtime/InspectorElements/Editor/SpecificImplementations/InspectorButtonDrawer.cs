#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MUtility.SpecificImplementations
{
[CustomPropertyDrawer(typeof(IInspectorButton), useForChildren: true)]
public class InspectorButtonDrawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var button = (IInspectorButton) inspectorElement;
		if (!GUI.Button(position, label)) return false;
		 
		string warning = button.WarningMessage(parentObject);
		if (!string.IsNullOrEmpty(warning) && !DisplayDialog(warning, label.text)) return false;

		button.OnClick(parentObject);
		return true;
	}

	bool DisplayDialog(string warningMessage, string label)
	{
		return EditorUtility.DisplayDialog(label, warningMessage, "OK", "Cancel");
	}
}
}
#endif