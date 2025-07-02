using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{
	[Serializable]
	public struct RangedFloat
	{
		public float min;
		public float max;

		public RangedFloat(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float Clamp(float pitch) => Mathf.Clamp(pitch, min, max);
		public float LerpFromRange(float t) => Mathf.Lerp(min, max, t);

		public float LerpFromRangeUnclamped(float t) => Mathf.LerpUnclamped(min, max, t);

		public bool Contains(float value) => value >= min && value <= max;

		public float AbsoluteDistanceOutOfRange(float pitch) =>
			pitch < min ? min - pitch :
			pitch > max ? pitch - max :
			0;
	}

	[Serializable]
	public struct RangedInt
	{
		public int min;
		public int max;

		public RangedInt(int min, int max)
		{
			this.min = min;
			this.max = max;
		}
		public float LerpFromRange(float t) => Mathf.Lerp(min, max, t);

		public float LerpFromRangeUnclamped(float t) => Mathf.LerpUnclamped(min, max, t);
		public bool Contains(int value) => value >= min && value <= max;
		public int AbsoluteDistanceOutOfRange(int pitch) =>
			pitch < min ? min - pitch :
			pitch > max ? pitch - max :
			0;
		public bool Contains(float value) => value >= min && value <= max;
		public float AbsoluteDistanceOutOfRange(float pitch) =>
			pitch < min ? min - pitch :
			pitch > max ? pitch - max :
			0;
	}

	public class MinMaxRangeAttribute : PropertyAttribute
	{
		public readonly float min;
		public readonly float max;

		public MinMaxRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}

#if UNITY_EDITOR
namespace MUtility.Internal
{
	[CustomPropertyDrawer(typeof(RangedInt))]
	public class RangedIntDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty minProp = property.FindPropertyRelative(nameof(RangedInt.min));
			SerializedProperty maxProp = property.FindPropertyRelative(nameof(RangedInt.max));

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);             // WOW

			float space = EditorGUIUtility.standardVerticalSpacing;
			float width = position.width;

			Rect minRect = new(position);
			Rect maxRect = new(position);
			minRect.width = (width - space) / 2f;
			maxRect.xMin += (width / 2f) + space;
			maxRect.width = (width - space) / 2f;

			float labelW = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 30;

			EditorGUI.BeginChangeCheck();

			int minValue = EditorGUI.IntField(minRect, new GUIContent("Min"), minProp.intValue);
			int maxValue = EditorGUI.IntField(maxRect, new GUIContent("Max"), maxProp.intValue);

			if (EditorGUI.EndChangeCheck())
			{
				minProp.intValue = minValue;
				maxProp.intValue = maxValue;
			}
			EditorGUIUtility.labelWidth = labelW;

			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(RangedFloat))]
	public class RangedFloatDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty minProp = property.FindPropertyRelative(nameof(RangedFloat.min));
			SerializedProperty maxProp = property.FindPropertyRelative(nameof(RangedFloat.max));

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);             // WOW

			float space = EditorGUIUtility.standardVerticalSpacing;
			float width = position.width;

			Rect minRect = new(position);
			Rect maxRect = new(position);
			minRect.width = (width - space) / 2f;
			maxRect.xMin += (width / 2f) + space;
			maxRect.width = (width - space) / 2f;

			float labelW = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 30;

			EditorGUI.BeginChangeCheck();
			float minValue = EditorGUI.FloatField(minRect, new GUIContent("Min"), minProp.floatValue);
			float maxValue = EditorGUI.FloatField(maxRect, new GUIContent("Max"), maxProp.floatValue);

			if (EditorGUI.EndChangeCheck())
			{
				minProp.floatValue = minValue;
				maxProp.floatValue = maxValue;
			}
			EditorGUIUtility.labelWidth = labelW;
			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	public class MinMaxRangeIntAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty minProp = property.FindPropertyRelative(nameof(RangedInt.min));
			SerializedProperty maxProp = property.FindPropertyRelative(nameof(RangedInt.max));
			MinMaxRangeAttribute rangeAttribute = (MinMaxRangeAttribute)attribute;

			if (minProp == null || maxProp == null)
			{
				EditorGUI.LabelField(position, label.text, "Use MinMaxRange only with RangedInt or RangedFloat.");
				return;
			}

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			bool isInt = minProp.propertyType == SerializedPropertyType.Integer;

			float minValue = isInt ? minProp.intValue : minProp.floatValue;
			float maxValue = isInt ? maxProp.intValue : maxProp.floatValue;
			float rangeMin = rangeAttribute.min;
			float rangeMax = rangeAttribute.max;

			const float numberW = 40f;
			const float margin = 6f;

			Rect minRect = new(position);
			minRect.width = numberW;
			position.xMin += numberW + margin;
			Rect maxRect = new(position);
			maxRect.xMin = maxRect.xMax - numberW;
			position.xMax -= numberW + margin;
			Rect sliderRect = new(position);

			EditorGUI.BeginChangeCheck();

			if (isInt)
				minValue = EditorGUI.IntField(minRect, GUIContent.none, Mathf.RoundToInt(minValue));
			else
				minValue = EditorGUI.FloatField(minRect, GUIContent.none, minValue);

			if (isInt)
				maxValue = EditorGUI.IntField(maxRect, GUIContent.none, Mathf.RoundToInt(maxValue));
			else
				maxValue = EditorGUI.FloatField(maxRect, GUIContent.none, maxValue);


			EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, rangeMin, rangeMax);

			minValue = Mathf.Clamp(minValue, rangeMin, rangeMax);
			maxValue = Mathf.Clamp(maxValue, minValue, rangeMax);

			if (EditorGUI.EndChangeCheck())
			{
				if (isInt)
				{
					minProp.intValue = Mathf.RoundToInt(minValue);
					maxProp.intValue = Mathf.RoundToInt(maxValue);
				}
				else
				{
					minProp.floatValue = minValue;
					maxProp.floatValue = maxValue;
				}
			}

			EditorGUI.EndProperty();
		}
	}
}
#endif