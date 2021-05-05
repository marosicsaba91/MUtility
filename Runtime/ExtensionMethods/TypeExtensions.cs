using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
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
            foreach (var typeToTest in typesInAssembly)
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

        public static bool IsBaseclassOf_GenericsSupported(this Type baseType, Type subType)
        {
            return subType.IsSubclassOf_GenericsSupported(baseType);
        }

        public static bool IsSubclassOf_GenericsSupported(this Type subType, Type baseType)
        {
            if (baseType.IsGenericType)
            {
                return IsSubclassOfRawGeneric(baseType, subType);
            }
            else
            {
                return subType.IsSubclassOf(baseType);
            }
        }

        static bool IsSubclassOfRawGeneric(Type genericBaseClass, Type typeTotest)
        {
            while (typeTotest != null && typeTotest != typeof(object))
            {
                var cur = typeTotest.IsGenericType ? typeTotest.GetGenericTypeDefinition() : typeTotest;
                if (genericBaseClass == cur)
                {
                    return true;
                }

                typeTotest = typeTotest.BaseType;
            }

            return false;
        }
    }
}