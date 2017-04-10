using System;
using System.Linq;
using System.Reflection;

using MistyORM.Entities;

namespace MistyORM.Miscellaneous
{
    internal static class Extensions
    {
        internal static PropertyInfo[] GetEntityProperties(this TypeInfo TypeInfo)
        {
            return TypeInfo.GetProperties().Where(x => !x.GetMethod.IsVirtual).ToArray();
        }

        internal static bool HasAttribute<T>(this PropertyInfo Info) where T : Attribute
        {
            return Info.GetCustomAttribute<T>() != null;
        }

        internal static bool IsEntity(this PropertyInfo Info)
        {
            return Info.PropertyType.GetTypeInfo().IsSubclassOf(typeof(TableEntity));
        }
    }
}
