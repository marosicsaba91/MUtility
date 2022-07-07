#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MUtility
{
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class FormatAttributeDrawer : PropertyDrawer
{
    bool _toShow = true; 

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!(attribute is ShowIfAttribute conditional)) return EditorGUI.GetPropertyHeight(property);
 
        _toShow = conditional.IsConditionMatch(property.GetParent());
        if (!_toShow) return 0;

        return EditorGUI.GetPropertyHeight(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!_toShow) return;
        EditorGUI.PropertyField(position, property, label, true);
    }
}
}
#endif