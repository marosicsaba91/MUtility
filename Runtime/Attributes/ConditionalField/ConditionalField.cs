using System; 
using UnityEngine; 

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace MUtility
{

[AttributeUsage(AttributeTargets.Field)]
public class ConditionalFieldAttribute : PropertyAttribute
{
	public readonly string[] membersToCheck;
 
	public ConditionalFieldAttribute(string elementToCheck)
		=> membersToCheck = membersToCheck = new[] { elementToCheck };


	public ConditionalFieldAttribute(string[] membersToCheck)
		=> this.membersToCheck = membersToCheck;
	
	
	
#if UNITY_EDITOR

	const BindingFlags bindings =
		BindingFlags.Instance |
		BindingFlags.Static |
		BindingFlags.Public |
		BindingFlags.NonPublic;

	public bool IsConditionMatch(object owner)
	{
		if (membersToCheck.IsNullOrEmpty()) return true;
 
		Type ownerType = owner.GetType();
		foreach (string memberName in membersToCheck)
		{
			if (memberName.IsNullOrEmpty()) 		
			{
				Debug.LogWarning($"No condition name in type {ownerType}");
			}
			else if (TryGetMethodInfo(ownerType, memberName, out MethodInfo methodInfo))
			{
				if ((bool)methodInfo.Invoke(owner, Array.Empty<object>()))
					return false;
			}
			else if (TryGetFieldInfo(ownerType, memberName, out FieldInfo fieldInfo))
			{
				if ((bool)fieldInfo.GetValue(owner))
					return false;
			}
			else if (TryGetPropertyInfo(ownerType, memberName, out PropertyInfo propertyInfo))
			{
				if ((bool)propertyInfo.GetValue(owner))
					return false;
			}
			else
			{
				Debug.LogWarning($"No member named {memberName} in type {ownerType}");
			}
		}

		return true;
	}

	bool TryGetMethodInfo(Type ownerType, string name, out MethodInfo methodInfo)
	{
		MethodInfo method = ownerType.GetMethod(name, bindings);

		if (method != null && method.ReturnType == typeof(bool) && method.GetParameters().IsNullOrEmpty())
		{
			methodInfo = method;
			return true;
		}
		
		methodInfo = null;
		return false;
	}
	
	bool TryGetFieldInfo(Type ownerType, string name, out FieldInfo fieldInfo)
	{
		FieldInfo field = ownerType.GetField(name, bindings);

		if (field != null && field.FieldType == typeof(bool))
		{
			fieldInfo = field;
			return true;
		}

		fieldInfo = null; 
		return false;
	}	
	
	bool TryGetPropertyInfo(Type ownerType, string name, out PropertyInfo propertyInfo)
	{
		PropertyInfo property  = ownerType.GetProperty(name, bindings);
		
		if (property != null && property.PropertyType == typeof(bool))
		{
			propertyInfo = property;
			return true;
		}

		propertyInfo = null; 
		return false;
	}
#endif

	
	
	
}
 

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldAttributeDrawer : PropertyDrawer
{
	bool _toShow = true; 

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (!(attribute is ConditionalFieldAttribute conditional)) return EditorGUI.GetPropertyHeight(property);
 
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
#endif
}