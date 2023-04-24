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
			EditorGUI.LabelField(position, label);
			Rect contentRect = EditorHelper.ContentRect(position);

			SerializedProperty sourceTypeProperty = property.FindPropertyRelative(nameof(Position3.sourceType));
			SerializedProperty transformSourceProperty = property.FindPropertyRelative(nameof(Position3.transformSource));
			SerializedProperty vectorSourceProperty = property.FindPropertyRelative(nameof(Position3.vectorSource));

			contentRect.Split(rate: 0.25f, out Rect typeRect, out Rect valueRect);
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