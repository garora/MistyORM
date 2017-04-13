using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MistyORM.Entities;

namespace MistyORM.Miscellaneous
{
    internal static class Extensions
    {
        public static IEnumerable<PropertyInfo> GetEntityProperties(this TypeInfo TypeInfo)
        {
            return TypeInfo.GetProperties().Where(x => !x.GetMethod.IsVirtual);
        }

        public static bool HasAttribute<T>(this PropertyInfo Info) where T : Attribute
        {
            return Info.GetCustomAttribute<T>() != null;
        }

        public static bool IsEntity(this PropertyInfo Info)
        {
            return Info.PropertyType.GetTypeInfo().IsSubclassOf(typeof(TableEntity));
        }
    }
}
