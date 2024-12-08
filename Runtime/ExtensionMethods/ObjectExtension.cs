using System;

namespace MUtility
{
	public static class ObjectExtension
	{
		public static T TryGetCopy<T>(this T obj) where T : class
		{ 
			Type type = obj.GetType();
			T copyObj = Activator.CreateInstance(type) as T;

			foreach (System.Reflection.PropertyInfo item in type.GetProperties())
				if (item.GetSetMethod() != null && item.GetGetMethod() != null)
					item.SetValue(copyObj, item.GetValue(obj));

			foreach (System.Reflection.FieldInfo item in type.GetFields())
				item.SetValue(copyObj, item.GetValue(obj));

			return copyObj;
		}
	}
}