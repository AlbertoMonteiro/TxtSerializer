using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace TxtFileGenerator
{
    internal static class ExtensionMethods
    {
        public static T GetAttribute<T>(this MemberInfo property)
        {
            return property.GetCustomAttributes(typeof(T), false).Cast<T>().SingleOrDefault();
        }

        public static Type GetInterface<T>(this Type property)
        {
            return property.GetInterface(typeof(T).Name, true);
        }

        public static bool IsEnumerable(this Type propertyType)
        {
            return propertyType.GetInterface<IEnumerable>() != null;
        }

    }
}
