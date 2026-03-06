#if UNITY_EDITOR

using System;
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
		static GUIContent _minContent;
		static GUIContent _maxContent;

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
				propertyInfo?.GetCustomAttributes().FirstOrDefault(a => a is RangeAttribute) is
					RangeAttribute limitsA
					? new RangedFloat(limitsA.min, limitsA.max)
					: null;

			// Draw label the proper way and forward remaining rect
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				position = EditorGUI.PrefixLabel(position, label);

				SerializedProperty minProperty = property.FindPropertyRelative("min");
				SerializedProperty maxProperty = property.FindPropertyRelative("max");

				if (minProperty.propertyType == SerializedPropertyType.Float)
					DrawRangedFloat(position, minProperty, maxProperty, limits);
				else if (minProperty.propertyType == SerializedPropertyType.Integer)
					DrawRangedInt(position, minProperty, maxProperty, limits);
			}
		}

		static void DrawRangedFloat(
			Rect position,
			SerializedProperty minProperty,
			SerializedProperty maxProperty,
			RangedFloat? limits) =>
			DrawAnyRanged(position, minProperty, maxProperty, limits,
				p => p.floatValue,
				(p, f) => p.floatValue = f,
				f => f,
				f => f,
				(rect, gc, f) => EditorGUI.FloatField(rect, gc, f));

		static void DrawRangedInt(
			Rect position,
			SerializedProperty minProperty,
			SerializedProperty maxProperty,
			RangedFloat? limits)
			=> DrawAnyRanged(position, minProperty, maxProperty, limits,
				p => p.intValue,
				(p, i) => p.intValue = i,
				i => Mathf.RoundToInt(i),
				f => f,
				(rect, gc, i) => EditorGUI.IntField(rect, gc, i));

		static void DrawAnyRanged<T>(
			Rect position,
			SerializedProperty minProperty,
			SerializedProperty maxProperty,
			RangedFloat? limits,

			Func<SerializedProperty, T> getValue,
			Action<SerializedProperty, T> setValue,
			Func<float, T> fromFloat,
			Func<T, float> toFloat,
			Func<Rect, GUIContent, T, T> propertyDrawer
			) where T : struct, IComparable<T>
		{
			T oldMin = getValue(minProperty);
			T oldMax = getValue(maxProperty);
			T newMin = oldMin;
			T newMax = oldMax;

			float space = EditorGUIUtility.standardVerticalSpacing;

			if (limits.HasValue)  // Slider view
			{
				float inputFieldWidth = Mathf.Min(50f, (position.width - space * 2) / 6);
				Rect minFieldRect = new(position.x, position.y, inputFieldWidth, position.height);
				Rect sliderRect = new(minFieldRect.xMax + space, position.y, position.width - inputFieldWidth * 2 - space * 2, position.height);
				Rect maxFieldRect = new(sliderRect.xMax + space, position.y, inputFieldWidth, position.height);

				float fMin = toFloat(newMin);
				float fMax = toFloat(newMax);
				EditorGUI.MinMaxSlider(sliderRect, ref fMin, ref fMax, limits.Value.min, limits.Value.max);
				newMin = propertyDrawer(minFieldRect, GUIContent.none, fromFloat(fMin));
				newMax = propertyDrawer(maxFieldRect, GUIContent.none, fromFloat(fMax));
				newMin = Clamp(newMin, fromFloat(limits.Value.min), fromFloat(limits.Value.max));
				newMax = Clamp(newMax, fromFloat(limits.Value.min), fromFloat(limits.Value.max));
			}
			else  // Input Fields only view
			{
				_minContent ??= new GUIContent("Min");
				_maxContent ??= new GUIContent("Max");

				float originalLabelWidth = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 26;
				Rect propertyRect = position;
				propertyRect.x = position.x;
				propertyRect.width = (position.width - space * 2) / 2;

				newMin = propertyDrawer(propertyRect, _minContent, oldMin);
				propertyRect.x = propertyRect.xMax + space * 2;
				newMax = propertyDrawer(propertyRect, _maxContent, oldMax);
				EditorGUIUtility.labelWidth = originalLabelWidth;
			}

			if (newMin.CompareTo(oldMin) != 0)
				setValue(minProperty, Min(newMin, newMax));

			if (newMax.CompareTo(oldMax) != 0)
				setValue(maxProperty, Max(newMin, newMax));

			static T Clamp(T value, T min, T max) =>
				value.CompareTo(min) < 0 ? min :
				value.CompareTo(max) > 0 ? max :
				value;

			static T Min(T a, T b) => a.CompareTo(b) < 0 ? a : b;
			static T Max(T a, T b) => a.CompareTo(b) > 0 ? a : b;
		}
	}
}
#endif