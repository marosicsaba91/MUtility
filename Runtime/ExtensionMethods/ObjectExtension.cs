using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace MUtility
{
	public static class ObjectExtension
	{
		public static T TryCopyWithReflection<T>(this T obj, bool copyRecursively) where T : class
		{
			if (obj == null) return null;

			Type type = obj.GetType();
			T copyObj = Activator.CreateInstance(type) as T;

			foreach (PropertyInfo propertyInfo in type.GetProperties())
				if (propertyInfo.GetSetMethod() != null && propertyInfo.GetGetMethod() != null)
				{
					object item = propertyInfo.GetValue(obj);

					if (copyRecursively && propertyInfo.PropertyType.IsClass)
						item = item.TryCopyWithReflection(copyRecursively);

					propertyInfo.SetValue(copyObj, item);
				}
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				object item = fieldInfo.GetValue(obj);

				if (copyRecursively && fieldInfo.FieldType.IsClass)
					item = item.TryCopyWithReflection(copyRecursively);

				fieldInfo.SetValue(copyObj, item);
			}

			return copyObj;
		}

		public static void SetDirtySafe(this ScriptableObject so)
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(so);
#endif
		}
	}
}