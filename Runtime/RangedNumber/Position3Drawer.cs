#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine;

namespace MUtility.Editor
{
	[CustomPropertyDrawer(typeof(Position3))]
	public class Position3Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Label
			Rect labelRect = position;
			labelRect.width = EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(labelRect, label);

			// Property Rect
			Rect propertyRect = position;
			propertyRect.xMin += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;

			// Properties
			SerializedProperty sourceTypeProperty = property.FindPropertyRelative("sourceType");
			SerializedProperty transformSourceProperty = property.FindPropertyRelative("transformSource");
			SerializedProperty vectorSourceProperty = property.FindPropertyRelative("vectorSource");

			// Layout
			float typeWidth = propertyRect.width * 0.25f;
			Rect typeRect = new Rect(propertyRect.x, propertyRect.y, typeWidth, propertyRect.height);
			Rect valueRect = new Rect(typeRect.xMax + EditorGUIUtility.standardVerticalSpacing, propertyRect.y, propertyRect.width - typeWidth - EditorGUIUtility.standardVerticalSpacing, propertyRect.height);

			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			EditorGUI.PropertyField(typeRect, sourceTypeProperty, GUIContent.none);

			Position3.SourceType t = (Position3.SourceType)sourceTypeProperty.enumValueIndex;

			if (t == Position3.SourceType.Transform)
				EditorGUI.PropertyField(valueRect, transformSourceProperty, GUIContent.none);
			else if (t == Position3.SourceType.Vector)
				EditorGUI.PropertyField(valueRect, vectorSourceProperty, GUIContent.none);
			
			EditorGUI.indentLevel = indent;
		}
	}
}
#endif