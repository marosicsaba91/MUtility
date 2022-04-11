#if UNITY_EDITOR

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
        if (o == null) return;


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

        EditorGUI.PropertyField(propertyRect, property.FindPropertyRelative(nameof(position.min)),
            new GUIContent("Min"));
        propertyRect.x = propertyRect.xMax + EditorGUIUtility.standardVerticalSpacing * 2;
        EditorGUI.PropertyField(propertyRect, property.FindPropertyRelative(nameof(position.max)),
            new GUIContent("Max"));
        EditorGUIUtility.labelWidth = cachedLabelW;
        EditorGUI.indentLevel = cachedIndent;
    }
}
}
#endif