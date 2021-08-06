#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{

public abstract class InspectorPropertyDrawer<T, TProperty> : InspectorElementDrawer
	where TProperty : IInspectorProperty<T>
{
	public override bool Draw(
		Rect position,
		IInspectorElement inspectorElement,
		SerializedProperty property,
		object parentObject,
		Object serializedObject,
		GUIContent label)
	{
		var inspectorProperty = (TProperty) inspectorElement;
		IList<T> options = inspectorProperty.PopupElements(parentObject);
		
		T oldValue = inspectorProperty.GetValue(parentObject);
		T newValue = options == null 
			? GetValue(position, label, oldValue, (TProperty) inspectorElement, parentObject) 
			: DrawPopup(position, label, oldValue, inspectorProperty, options, parentObject);

		if (Equals( oldValue, newValue)) return false;

		inspectorProperty.SetValue(parentObject, newValue);
		return true;
	}

	protected abstract T GetValue(
		Rect position, GUIContent label, T oldValue, TProperty inspectorProperty, object parentObject);

	 
	T DrawPopup(Rect position, GUIContent label, T oldValue, TProperty inspectorProperty, IList<T > options, object parentObject)
	{
		const float popupWidth = 20f;
		float objectFieldWidth = position.width - popupWidth - EditorGUIUtility.standardVerticalSpacing;
		var objectFieldRect = new Rect(position.x, position.y, objectFieldWidth, position.height);
		T newValue = GetValue(objectFieldRect, label, oldValue, inspectorProperty, parentObject);
		int oldIndex = options.IndexOf(oldValue);
		var popupRect = new Rect(position.xMax - popupWidth, position.y, popupWidth, position.height);
		var optionStrings = new List<string> ();
		optionStrings.AddRange(options.Select(o => o == null ? "None" : o.ToString()).ToArray());
		int newIndex = EditorGUI.Popup(popupRect, oldIndex , optionStrings.ToArray()) ;
		if (newIndex < 0) newIndex = -1;

		if (oldIndex != newIndex) return newIndex < 0 ? oldValue : options[newIndex];
		if (Equals(oldValue, newValue)) return oldValue;
		if (!options.Contains(newValue)) return oldValue;
		return newValue;

	}

}
}
#endif

 