using System;
using System.Collections.Generic;
using System.Reflection;

namespace MUtility
{
	public static class TypeExtensions
	{
		public static List<Type> GetSubClassesInSameAssembly(this Type baseType)
		{
			return GetSubClasses(baseType, Assembly.GetAssembly(baseType));
		}

		static List<Type> GetSubClasses(this Type baseType, Assembly assembly)
		{
			Type[] typesInAssembly = assembly.GetTypes();

			var result = new List<Type>();
			foreach (Type typeToTest in typesInAssembly)
			{
				if (!typeToTest.IsClass)
					continue;
				if (typeToTest.IsAbstract)
					continue;

				if (typeToTest.IsSubclassOf_GenericsSupported(baseType))
				{
					result.Add(typeToTest);
				}
			}

			return result;
		}

		public static bool IsBaseclassOf_GenericsSupported(this Type baseType, Type subType) =>
			subType.IsSubclassOf_GenericsSupported(baseType);

		public static bool IsSubclassOf_GenericsSupported(this Type subType, Type baseType)
		{
			if (baseType.IsGenericType)
			{
				return subType.IsSubclassOfRawGeneric(baseType);
			}
			return subType.IsSubclassOf(baseType);
		}

		static bool IsSubclassOfRawGeneric(this Type typeToTest, Type genericBaseClass)
		{
			while (typeToTest != null && typeToTest != typeof(object))
			{
				Type cur = typeToTest.IsGenericType ? typeToTest.GetGenericTypeDefinition() : typeToTest;
				if (genericBaseClass == cur)
				{
					return true;
				}

				typeToTest = typeToTest.BaseType;
			}

			return false;
		}
	}
}