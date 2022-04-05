#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
public static class SerializedPropertyExtensions
{
    public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property)
    {
        property = property.Copy();
        SerializedProperty nextElement = property.Copy();
        bool hasNextElement = nextElement.NextVisible(false);
        if (!hasNextElement)
        {
            nextElement = null;
        }

        property.NextVisible(true);
        while (true)
        {
            if ((SerializedProperty.EqualContents(property, nextElement)))
            {
                yield break;
            }

            yield return property;

            bool hasNext = property.NextVisible(false);
            if (!hasNext)
            {
                break;
            }
        }
    }

    public static void Record(this SerializedProperty property)
    {
        Object targetObject = property.serializedObject.targetObject;
        Undo.RecordObject(targetObject, $"{property.name} has changed on {targetObject.name}");
    }

    public static Type GetTargetType(this SerializedObject obj)
    {
        if (obj == null) return null;

        if (obj.isEditingMultipleObjects)
        {
            Object c = obj.targetObjects[0];
            return c.GetType();
        }

        return obj.targetObject.GetType();
    }

    /// Gets the object the property represents.
    public static object GetObjectOfProperty(this SerializedProperty prop)
    {
        if (prop == null) return null;

        string path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        string[] elements = path.Split('.');
        foreach (string element in elements)
        {
            if (element.Contains("["))
            {
                string elementName = element.Substring(0, element.IndexOf("["));
                var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }

        return obj;
    }

    /// Gets the object that the property is a member of
    public static object GetObjectWithProperty(this SerializedProperty prop)
    {
        string path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        string[] elements = path.Split('.');
        foreach (string element in elements.Take(elements.Length - 1))
        {
            if (element.Contains("["))
            {
                string elementName = element.Substring(0, element.IndexOf("["));
                var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }

        return obj;
    }

    static object GetValue_Imp(object source, string name)
    {
        if (source == null)
            return null;
        Type type = source.GetType();

        while (type != null)
        {
            FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
                return f.GetValue(source);

            PropertyInfo p =
                type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (p != null)
                return p.GetValue(source, null);

            type = type.BaseType;
        }

        return null;
    }


    static object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as IEnumerable;
        if (enumerable == null) return null;
        IEnumerator enm = enumerable.GetEnumerator();

        for (var i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }

        return enm.Current;
    }

    public static object GetPropertyValue(this SerializedProperty prop)
    {
        if (prop == null) throw new ArgumentNullException("prop");

        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                return prop.intValue;
            case SerializedPropertyType.Boolean:
                return prop.boolValue;
            case SerializedPropertyType.Float:
                return prop.floatValue;
            case SerializedPropertyType.String:
                return prop.stringValue;
            case SerializedPropertyType.Color:
                return prop.colorValue;
            case SerializedPropertyType.ObjectReference:
                return prop.objectReferenceValue;
            case SerializedPropertyType.LayerMask:
                return (LayerMask) prop.intValue;
            case SerializedPropertyType.Enum:
                return prop.enumValueIndex;
            case SerializedPropertyType.Vector2:
                return prop.vector2Value;
            case SerializedPropertyType.Vector3:
                return prop.vector3Value;
            case SerializedPropertyType.Vector4:
                return prop.vector4Value;
            case SerializedPropertyType.Rect:
                return prop.rectValue;
            case SerializedPropertyType.ArraySize:
                return prop.arraySize;
            case SerializedPropertyType.Character:
                return (char) prop.intValue;
            case SerializedPropertyType.AnimationCurve:
                return prop.animationCurveValue;
            case SerializedPropertyType.Bounds:
                return prop.boundsValue;
            case SerializedPropertyType.Gradient:
                throw new InvalidOperationException("Can not handle Gradient types.");

        }

        return null;
    }

    public static void CopyPropertyValueTo(this SerializedProperty source, SerializedProperty destination)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (destination == null) throw new ArgumentNullException(nameof(destination));
        switch (source.propertyType)
        {
            case SerializedPropertyType.Integer:
            case SerializedPropertyType.LayerMask:
            case SerializedPropertyType.Character:
                destination.intValue = source.intValue;
                break;
            case SerializedPropertyType.Boolean:
                destination.boolValue = source.boolValue;
                break;
            case SerializedPropertyType.Float:
                destination.floatValue = source.floatValue;
                break;
            case SerializedPropertyType.String:
                destination.stringValue = source.stringValue;
                break;
            case SerializedPropertyType.Color:
                destination.colorValue = source.colorValue;
                break;
            case SerializedPropertyType.ObjectReference:
                destination.objectReferenceValue = source.objectReferenceValue;
                break;
            case SerializedPropertyType.ExposedReference:
                destination.exposedReferenceValue = source.exposedReferenceValue;
                break;
            case SerializedPropertyType.Enum:
                destination.enumValueIndex = source.enumValueIndex;
                break;
            case SerializedPropertyType.Vector2:
                destination.vector2Value = source.vector2Value;
                break;
            case SerializedPropertyType.Vector3:
                destination.vector3Value = source.vector3Value;
                break;
            case SerializedPropertyType.Vector4:
                destination.vector4Value = source.vector4Value;
                break;
            case SerializedPropertyType.Vector2Int:
                destination.vector2IntValue = source.vector2IntValue;
                break;
            case SerializedPropertyType.Vector3Int:
                destination.vector3IntValue = source.vector3IntValue;
                break;
            case SerializedPropertyType.Rect:
                destination.rectValue = source.rectValue;
                break;
            case SerializedPropertyType.Bounds:
                destination.boundsValue = source.boundsValue;
                break;
            case SerializedPropertyType.RectInt:
                destination.rectIntValue = source.rectIntValue;
                break;
            case SerializedPropertyType.BoundsInt:
                destination.boundsIntValue = source.boundsIntValue;
                break;

            case SerializedPropertyType.AnimationCurve:
                destination.animationCurveValue = source.animationCurveValue;
                break;
            case SerializedPropertyType.Quaternion:
                destination.quaternionValue = source.quaternionValue;
                break;
            case SerializedPropertyType.Generic:
            {
                IEnumerator<SerializedProperty> sourceEnumerator = source.GetChildren().GetEnumerator();
                IEnumerator<SerializedProperty> destinationEnumerator = destination.GetChildren().GetEnumerator();
                for (var i = 0; i < source.GetChildPropertyCount(includeGrandChildren: false); i++)
                {
                    sourceEnumerator.MoveNext();
                    destinationEnumerator.MoveNext();
                    sourceEnumerator.Current.CopyPropertyValueTo(destinationEnumerator.Current);
                }
                sourceEnumerator.Dispose();
                sourceEnumerator.Dispose();

                break;
            }
        }

        if (!source.isArray || !destination.isArray) return;

        for (var i = 0; i < source.arraySize; i++)
        {
            SerializedProperty sourceElement = source.GetArrayElementAtIndex(i);
            if (destination.arraySize - 1 < i)
                destination.InsertArrayElementAtIndex(i);
            SerializedProperty destinationElement = destination.GetArrayElementAtIndex(i);
            sourceElement.CopyPropertyValueTo(destinationElement);
        }

        for (int i = destination.arraySize - 1; i > source.arraySize - 1; i--)
            destination.DeleteArrayElementAtIndex(i);
    }


    public static double GetNumericValue(this SerializedProperty prop)
    {
        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                return prop.intValue;
            case SerializedPropertyType.Boolean:
                return prop.boolValue ? 1d : 0d;
            case SerializedPropertyType.Float:
                return prop.type == "double" ? prop.doubleValue : prop.floatValue;
            case SerializedPropertyType.ArraySize:
                return prop.arraySize;
            case SerializedPropertyType.Character:
                return prop.intValue;
            default:
                return 0d;
        }
    }

    public static bool IsNumericValue(this SerializedProperty prop)
    {
        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
            case SerializedPropertyType.Boolean:
            case SerializedPropertyType.Float:
            case SerializedPropertyType.ArraySize:
            case SerializedPropertyType.Character:
                return true;
            default:
                return false;
        }
    }

    public static int GetChildPropertyCount(this SerializedProperty property, bool includeGrandChildren = false)
    {
        SerializedProperty pStart = property.Copy();
        SerializedProperty pEnd = property.GetEndProperty();
        var cnt = 0;

        pStart.Next(true);
        while (!SerializedProperty.EqualContents(pStart, pEnd))
        {
            cnt++;
            pStart.Next(includeGrandChildren);
        }

        return cnt;
    }

    public static FieldInfo GetFieldInfo(this SerializedProperty prop)
    {
        if (prop == null) return null;

        Type tp = GetTargetType(prop.serializedObject);
        if (tp == null) return null;

        string path = prop.propertyPath.Replace(".Array.data[", "[");
        string[] elements = path.Split('.');
        foreach (string element in elements.Take(elements.Length - 1))
        {
            FieldInfo field;
            if (element.Contains("["))
            {
                string elementName = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));

                field = tp.GetMember(elementName, MemberTypes.Field,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault() as FieldInfo;
                if (field == null) return null;
                tp = field.FieldType;
            }
            else
            {
                field = tp.GetMember(element, MemberTypes.Field,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault() as FieldInfo;
                if (field == null) return null;
                tp = field.FieldType;
            }
        }

        return null;
    }
    
    public static bool IsExpandable(this SerializedProperty property)
    {
        if (property.propertyType == SerializedPropertyType.Generic ||
            property.propertyType == SerializedPropertyType.Vector4 ||
            property.propertyType == SerializedPropertyType.Quaternion)
            return true;
        return false;
    }
}
}
#endif