using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{
	[Serializable]
	public struct Optional<T>
	{
		[SerializeField] bool enabled;
		[SerializeField] T value;

		internal static string EnabledPropertyName => nameof(enabled);
		internal static string ValuePropertyName => nameof(value);

		public bool Enabled => enabled;
		public T Value => value;
		public T ValueOrDefault => enabled ? value : default;

		public Optional(T value, bool enabled = true)
		{
			this.value = value;
			this.enabled = enabled;
		}

		public static implicit operator Optional<T>(T value) => new(value, true);
		public static implicit operator T(Optional<T> optional) => optional.value;
		public static implicit operator bool(Optional<T> optional) => optional.enabled;
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(Optional<>))]
	class OptionalDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty valueProp = property.FindPropertyRelative(Optional<bool>.ValuePropertyName);
			return EditorGUI.GetPropertyHeight(valueProp, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty enabledProp = property.FindPropertyRelative(Optional<bool>.EnabledPropertyName);
			SerializedProperty valueProp = property.FindPropertyRelative(Optional<bool>.ValuePropertyName);

			position = EditorGUI.PrefixLabel(position, label);

			Rect toggleRect = position;
			toggleRect.width = 16;
			EditorGUI.PropertyField(toggleRect, enabledProp, GUIContent.none);

			Rect valueRect = position;
			valueRect.x += 20;
			valueRect.width -= 20;
			EditorGUIUtility.labelWidth = 0;
			Color color = GUI.color;
			if (!enabledProp.boolValue)
				GUI.color = new Color(color.r, color.g, color.b, color.a / 2);

			EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

			GUI.color = color;
		}
	}
#endif
}
