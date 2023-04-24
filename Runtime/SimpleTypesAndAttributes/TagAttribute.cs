﻿// ---------------------------------------------------------------------------- 
// Author: Kaynn, Yeo Wen Qin
// https://github.com/Kaynn-Cahya
// Date:   11/02/2019
// ----------------------------------------------------------------------------

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{
	public class TagAttribute : PropertyAttribute
	{
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(TagAttribute))]
	public class TagAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.String)
			{
				if (!_checked)
					Warning(property);
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
		}

		bool _checked;

		void Warning(SerializedProperty property)
		{
			Debug.LogWarning(string.Format(
				"Property <color=brown>{0}</color> in object <color=brown>{1}</color> is of wrong type. Expected: String",
				property.name, property.serializedObject.targetObject));
			_checked = true;
		}
	}
#endif
}
