#if UNITY_EDITOR

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MUtility.Editor
{

	[CustomPropertyDrawer(typeof(FloatRange))]
	public class FloatRangeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			=> RangedNumberDrawer.DrawRanged(position, property, label);
	}

	[CustomPropertyDrawer(typeof(IntRange))]
	public class IntRangeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			=> RangedNumberDrawer.DrawRanged(position, property, label);
	}

	public static class RangedNumberDrawer
	{
		public static void DrawRanged(Rect position, SerializedProperty property, GUIContent label)
		{
			object o = property?.GetObjectOfProperty();
			if (o == null)
				return;

			// Get Range Attribute

			Type parentType = property.GetObjectWithProperty().GetType();
			const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
			FieldInfo propertyInfo = parentType.GetField(property.name, flags);
			var limitsA = propertyInfo?.GetCustomAttributes().FirstOrDefault(a => a is RangeLimitsAttribute) as RangeLimitsAttribute;
			FloatRange? limits = limitsA != null ? new FloatRange(limitsA.min, limitsA.max) : null;

			// Label
			Rect labelRect = position;
			labelRect.width = EditorHelper.LabelWidth;
			EditorGUI.LabelField(labelRect, label);

			Rect propertyRect = position;
			propertyRect.x = EditorHelper.ContentStartX;
			propertyRect.width = (EditorHelper.ContentWidth(position) - EditorGUIUtility.standardVerticalSpacing * 2) / 2;

			float cachedLabelW = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 27;
			int cachedIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			SerializedProperty minProperty = property.FindPropertyRelative(nameof(FloatRange.min));
			SerializedProperty maxProperty = property.FindPropertyRelative(nameof(FloatRange.max));

			float oldMin = minProperty.floatValue;
			float oldMax = maxProperty.floatValue;

			float newMin = EditorGUI.FloatField(propertyRect, "Min", oldMin);

			propertyRect.x = propertyRect.xMax + EditorGUIUtility.standardVerticalSpacing * 2;
			float newMax = EditorGUI.FloatField(propertyRect, "Max", oldMax);

			newMin = limits.HasValue ? Mathf.Max(newMin, limits.Value.min) : newMin;
			newMax = limits.HasValue ? Mathf.Min(newMax, limits.Value.max) : newMax;
			newMax = limits.HasValue ? Mathf.Max(newMax, limits.Value.min) : newMax;
			newMin = limits.HasValue ? Mathf.Min(newMin, limits.Value.max) : newMin;

			if (newMin != oldMin)
			{
				newMin = Mathf.Min(newMin, newMax);
				minProperty.floatValue = newMin;
			}
			if (newMax != oldMax)
			{
				newMax = Mathf.Max(newMin, newMax);
				maxProperty.floatValue = newMax;
			}

			EditorGUIUtility.labelWidth = cachedLabelW;
			EditorGUI.indentLevel = cachedIndent;
		}
	}
}
#endif