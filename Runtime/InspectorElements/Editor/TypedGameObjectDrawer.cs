#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MUtility
{
[CustomPropertyDrawer(typeof(TypedGameObject), useForChildren: true)]
public class TypedGameObjectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var typedGameObject = (TypedGameObject) property.GetObjectOfProperty();
        var newValue = (GameObject) EditorGUI.ObjectField(
            position,
            typedGameObject.GameObject,
            typeof(GameObject),
            allowSceneObjects: true);

        Object so = property.serializedObject.targetObject;
        Undo.RecordObject(so, "Typed Game Object Reference Changed");
        if (typedGameObject.TrySetGameObject(newValue))
            EditorUtility.SetDirty(so);
    }
}
}
#endif