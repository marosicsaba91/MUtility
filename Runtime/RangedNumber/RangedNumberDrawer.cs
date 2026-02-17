#if UNITY_EDITOR

using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MUtility.Editor
{
	[CustomPropertyDrawer(typeof(RangedFloat))]
	public class FloatRangeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			=> RangedNumberDrawer.DrawRanged(position, property, label);
	}

	[CustomPropertyDrawer(typeof(RangedInt))]
	public class IntRangeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			=> RangedNumberDrawer.DrawRanged(position, property, label);
	}

	public static class RangedNumberDrawer
	{
		public static void DrawRanged(Rect position, SerializedProperty property, GUIContent label)
		{
			// Get Range Attribute
			string fieldName = property.name;
			// Handle array elements (e.g., "myArray.Array.data[0]")
			if (fieldName == "data" && property.propertyPath.Contains(".Array.data["))
			{
				string[] pathParts = property.propertyPath.Split('.');
				for (int i = pathParts.Length - 1; i >= 0; i--)
				{
					if (pathParts[i].StartsWith("Array") && i > 0)
					{
						fieldName = pathParts[i - 1];
						break;
					}
				}
			}

			FieldInfo propertyInfo = property.serializedObject.targetObject.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			RangedFloat? limits =
				propertyInfo?.GetCustomAttributes().FirstOrDefault(a => a is RangeLimitsAttribute) is
					RangeLimitsAttribute limitsA
					? new RangedFloat(limitsA.min, limitsA.max)
					: null;

			// Label
			Rect labelRect = position;
			labelRect.width = EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(labelRect, label);

			Rect propertyRect = position;
			propertyRect.x = labelRect.xMax;
			propertyRect.width = (position.width - EditorGUIUtility.labelWidth - EditorGUIUtility.standardVerticalSpacing * 2) / 2;

			float cachedLabelW = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 27;
			int cachedIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			SerializedProperty minProperty = property.FindPropertyRelative("min");
			SerializedProperty maxProperty = property.FindPropertyRelative("max");

			switch (minProperty.propertyType)
			{
				case SerializedPropertyType.Float:
				{
					float oldMin = minProperty.floatValue;
					float oldMax = maxProperty.floatValue;

					float newMin = EditorGUI.FloatField(propertyRect, "Min", oldMin);
					propertyRect.x = propertyRect.xMax + EditorGUIUtility.standardVerticalSpacing * 2;
					float newMax = EditorGUI.FloatField(propertyRect, "Max", oldMax);

					if (limits.HasValue)
					{
						newMin = Mathf.Clamp(newMin, limits.Value.min, limits.Value.max);
						newMax = Mathf.Clamp(newMax, limits.Value.min, limits.Value.max);
					}

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

					break;
				}
				case SerializedPropertyType.Integer:
				{
					int oldMin = minProperty.intValue;
					int oldMax = maxProperty.intValue;

					int newMin = EditorGUI.IntField(propertyRect, "Min", oldMin);
					propertyRect.x = propertyRect.xMax + EditorGUIUtility.standardVerticalSpacing * 2;
					int newMax = EditorGUI.IntField(propertyRect, "Max", oldMax);

					if (limits.HasValue)
					{
						newMin = Mathf.Clamp(newMin, (int)limits.Value.min, (int)limits.Value.max);
						newMax = Mathf.Clamp(newMax, (int)limits.Value.min, (int)limits.Value.max);
					}

					if (newMin != oldMin)
					{
						newMin = Mathf.Min(newMin, newMax);
						minProperty.intValue = newMin;
					}

					if (newMax != oldMax)
					{
						newMax = Mathf.Max(newMin, newMax);
						maxProperty.intValue = newMax;
					}

					break;
				}
			}

			EditorGUIUtility.labelWidth = cachedLabelW;
			EditorGUI.indentLevel = cachedIndent;
		}
	}
}
#endif