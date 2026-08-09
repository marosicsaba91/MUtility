using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MUtility
{
	[Serializable]
	public struct Remap
	{
		[SerializeField] float inputMin;
		[SerializeField] float inputMax;
		[SerializeField] float outputMin;
		[SerializeField] float outputMax;
		[SerializeField] bool clamped;

		internal static string InputMinPropertyName => nameof(inputMin);
		internal static string InputMaxPropertyName => nameof(inputMax);
		internal static string OutputMinPropertyName => nameof(outputMin);
		internal static string OutputMaxPropertyName => nameof(outputMax);
		internal static string ClampedPropertyName => nameof(clamped);

		public float InputMin => inputMin;
		public float InputMax => inputMax;
		public float OutputMin => outputMin;
		public float OutputMax => outputMax;
		public bool Clamped => clamped;

		public Remap(float inputMin, float inputMax, float outputMin, float outputMax, bool clamped = true)
		{
			this.inputMin = inputMin;
			this.inputMax = inputMax;
			this.outputMin = outputMin;
			this.outputMax = outputMax;
			this.clamped = clamped;
		}

		public float Evaluate(float value)
		{
			float interpolation = inputMin == inputMax ? 0 : (value - inputMin) / (inputMax - inputMin);
			if (clamped)
				interpolation = Mathf.Clamp01(interpolation);

			return Mathf.LerpUnclamped(outputMin, outputMax, interpolation);
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(Remap))]
	class RemapDrawer : PropertyDrawer
	{
		const float ClampWidth = 16;

		static readonly GUIContent _inputMinLabel = new("In", "Input minimum");
		static readonly GUIContent _inputMaxLabel = new("to", "Input maximum");
		static readonly GUIContent _outputMinLabel = new("Out", "Output minimum");
		static readonly GUIContent _outputMaxLabel = new("to", "Output maximum");

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				position = EditorGUI.PrefixLabel(position, label);
				float spacing = EditorGUIUtility.standardVerticalSpacing;
				SerializedProperty clampedProperty = property.FindPropertyRelative(Remap.ClampedPropertyName);
				Rect clampRect = new(position.xMax - ClampWidth, position.y, ClampWidth, position.height);
				position.xMax = clampRect.xMin - spacing;

				DrawValues(position, property);
				EditorGUI.PropertyField(clampRect, clampedProperty, GUIContent.none);
			}
		}

		static void DrawValues(Rect position, SerializedProperty property)
		{
			float spacing = EditorGUIUtility.standardVerticalSpacing;
			float fieldWidth = (position.width - spacing * 3) / 4;
			float labelWidth = EditorGUIUtility.labelWidth;
			int indentLevel = EditorGUI.indentLevel;
			EditorGUIUtility.labelWidth = 24;
			EditorGUI.indentLevel = 0;

			DrawFloat(Remap.InputMinPropertyName, _inputMinLabel, 0);
			DrawFloat(Remap.InputMaxPropertyName, _inputMaxLabel, 1);
			DrawFloat(Remap.OutputMinPropertyName, _outputMinLabel, 2);
			DrawFloat(Remap.OutputMaxPropertyName, _outputMaxLabel, 3);

			EditorGUI.indentLevel = indentLevel;
			EditorGUIUtility.labelWidth = labelWidth;

			void DrawFloat(string propertyName, GUIContent fieldLabel, int index)
			{
				Rect fieldRect = new(position.x + index * (fieldWidth + spacing), position.y, fieldWidth, position.height);
				EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative(propertyName), fieldLabel);
			}
		}
	}
#endif
}
