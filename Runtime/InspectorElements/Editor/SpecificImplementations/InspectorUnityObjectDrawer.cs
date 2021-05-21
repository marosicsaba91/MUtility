#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
[CustomPropertyDrawer(typeof(IInspectorUnityObject), useForChildren: true)]
public class InspectorUnityObjectDrawer : InspectorElementDrawer
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var popup = (IInspectorUnityObject) inspectorElement;
		Object oldValue = popup.GetValue(parentObject);
		Object newValue;
		Type contentType = popup.ContentType;
		Type objectType = typeof(Object);
		if (contentType != objectType && !contentType.IsSubclassOf(objectType))
		{
			EditorGUI.LabelField(position,
				$"This is not a UnityEngine.Object type: {contentType}");
			return false;
		}

		IList<Object> options = popup.NotNullPopupElements(parentObject);
		if (options != null)
		{
			const float popupWidth = 20f; 
			var labelRect = new Rect(position.x, position.y, EditorHelper.LabelWidth, position.height);
			EditorGUI.LabelField(labelRect, label.text);
			var objectFieldRect =
				new Rect(labelRect.xMax, position.y, EditorHelper.ContentWidth - popupWidth, position.height);
			newValue =
				EditorGUI.ObjectField(objectFieldRect, oldValue, contentType, allowSceneObjects: true);
			int oldIndex = options.IndexOf(oldValue);
			var popupRect = new Rect(objectFieldRect.xMax, position.y, popupWidth, position.height);
			var optionStrings = new List<string> {"-"};
			optionStrings.AddRange(options.Select(o => o.ToString()).ToArray());
			int newIndex = EditorGUI.Popup(popupRect, oldIndex + 1, optionStrings.ToArray()) - 1;
			if (newIndex < 0) newIndex = -1;

			if (oldIndex == newIndex && oldValue == newValue) return false;
			if (newValue != null && !options.Contains(newValue)) return false;
			if (oldValue == newValue && oldIndex != newIndex)
				newValue = newIndex < 0 ? null : options[newIndex];
		}
		else
			newValue = EditorGUI.ObjectField(position, label, oldValue, contentType, allowSceneObjects: true);


		if (oldValue != newValue)
			popup.SetValue(parentObject, newValue); 
		
		newValue = popup.GetValue(parentObject);
		return oldValue != newValue;
	}
}
}
#endif