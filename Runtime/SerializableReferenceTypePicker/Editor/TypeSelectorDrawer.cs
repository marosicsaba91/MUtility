using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MUtility;
using UnityEditor; 
using UnityEngine;

namespace Asteroids.Editor
{
[CustomPropertyDrawer(typeof( TypePickerAttribute))]
public class TypeSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property,
        GUIContent label)
    {
        TypePickerAttribute att = attribute as TypePickerAttribute;
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            Debug.LogWarning($"{property.serializedObject.targetObject.name} / " +
                             $"{property.serializedObject.targetObject.GetType()} / " +
                             $"{property.name}" +
                             $" field is not Managed Reference Types." +
                             $"Use [TypePickerAttribute] only with [SerializeReference]");
        }
        else
        {

            Type managedReferenceFieldType = GetManagedReferenceFieldType(property);
            if (managedReferenceFieldType == null)
                return;

            List<Type> inheritedTypes = GetInheritedNonAbstractTypes(managedReferenceFieldType);

            int currentTypeIndex;
            Rect popupPosition = EditorHelper.ContentRect(position);

            Type currentType = GetRealTypeFromTypename(property.managedReferenceFullTypename);
            if (currentType == null)
                currentTypeIndex = 0;
            else
            {
                currentTypeIndex = inheritedTypes.IndexOf(currentType) + 1;

                if (!property.IsExpandable() || att.forceSmall)
                {
                    popupPosition.width = 20;
                    popupPosition.x -= 20;
                }
            }

            IEnumerable<string> typesNames =
                inheritedTypes.Select(t => t == null ? "- Select Type -" : t.ToString());
            string[] options = new[] { "- Select Type -" }.Concat(typesNames).ToArray();

            int tempIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            int resultIndex = EditorGUI.Popup(popupPosition, currentTypeIndex, options);
            EditorGUI.indentLevel = tempIndent;
            
            if (resultIndex != currentTypeIndex)
            {
                Undo.RecordObject(property.serializedObject.targetObject, "Reference Type Changed");
                if (resultIndex == 0)
                {
                    property.managedReferenceValue = null;
                }
                else
                {
                    Type newType = inheritedTypes[resultIndex - 1];
                    object newInstance = Activator.CreateInstance(newType);
                    TrySetupProperties(property, newInstance, newType);
                    property.managedReferenceValue = newInstance;
                }
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.PropertyField(position, property, label, includeChildren: true);
    }

    static void TrySetupProperties(SerializedProperty oldValue, object newInstance, Type newType)
    {
        try
        {
            IEnumerable<FieldInfo> allFieldsOfNewType = AllFields(newType).ToArray();
            object oldInstance = oldValue.GetObjectOfProperty();
            Type oldType = oldInstance.GetType();
            IEnumerable<FieldInfo> allFieldsOfOldType = AllFields(oldType).ToArray();
            foreach (FieldInfo field in allFieldsOfNewType)
            {
                if (!allFieldsOfOldType.Contains(field))
                    continue;
                object newValue = field.GetValue(oldInstance);
                field.SetValue(newInstance, newValue);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public static IEnumerable<FieldInfo> AllFields(Type type)
    {
        const BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        FieldInfo[] self = type.GetFields(binding);
        if (type.BaseType == null)
            return self;
        return self.Concat(AllFields(type.BaseType));
    }

    static readonly Dictionary<Type, List<Type>> inheritedNonAbstractTypes = new Dictionary<Type, List<Type>>();
    static List<Type> GetInheritedNonAbstractTypes(Type baseType)
    {
        if (inheritedNonAbstractTypes.TryGetValue(baseType, out List<Type> inherited))
            return inherited;

        List<Type> inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .ToList();

        inheritedNonAbstractTypes.Add(baseType, inheritedTypes);
        
        return inheritedTypes;
    } 

    static Type GetPropertyFieldType(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return typeof(bool);
            case SerializedPropertyType.Float:
                return typeof(float);
            case SerializedPropertyType.Integer:
                return typeof(int);
            case SerializedPropertyType.String:
                return typeof(string);
            case SerializedPropertyType.Bounds:
                return typeof(Bounds);
            case SerializedPropertyType.Character:
                return typeof(char);
            case SerializedPropertyType.Color:
                return typeof(Color);
            case SerializedPropertyType.Enum:
                return typeof(Enum);
            case SerializedPropertyType.Gradient:
                return typeof(Gradient);
            case SerializedPropertyType.Quaternion:
                return typeof(Quaternion);
            case SerializedPropertyType.Rect:
                return typeof(Rect);
            case SerializedPropertyType.Vector2:
                return typeof(Vector2);
            case SerializedPropertyType.Vector3:
                return typeof(Vector3);
            case SerializedPropertyType.Vector4:
                return typeof(Vector4);
            case SerializedPropertyType.AnimationCurve:
                return typeof(AnimationCurve);
            case SerializedPropertyType.BoundsInt:
                return typeof(BoundsInt);
            case SerializedPropertyType.LayerMask:
                return typeof(LayerMask);
            case SerializedPropertyType.RectInt:
                return typeof(RectInt);
            case SerializedPropertyType.Vector2Int:
                return typeof(Vector2Int);
            case SerializedPropertyType.Vector3Int:
                return typeof(Vector3Int);
            case SerializedPropertyType.ManagedReference:
                return GetManagedReferenceFieldType(property);
            default:
                return property.GetObjectOfProperty()?.GetType();
        }
    } 
    
    static Type GetManagedReferenceFieldType(SerializedProperty property)
    { 
        Type realPropertyType = GetRealTypeFromTypename(property.managedReferenceFieldTypename);
        if (realPropertyType != null)
            return realPropertyType; 
        return null;
    }
    
    static Type GetRealTypeFromTypename(string stringType)
    {
        (string AssemblyName, string ClassName) names = GetSplitNamesFromTypename(stringType);
        Type realType = Type.GetType($"{names.ClassName}, {names.AssemblyName}");
        return realType;
    } 
    
    static (string AssemblyName, string ClassName) GetSplitNamesFromTypename(string typename)
    {
        if (string.IsNullOrEmpty(typename))
            return ("","");

        string[] typeSplitString = typename.Split(char.Parse(" "));
        string typeClassName = typeSplitString[1];
        string typeAssemblyName = typeSplitString[0];
        return (typeAssemblyName,  typeClassName);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }
}
}