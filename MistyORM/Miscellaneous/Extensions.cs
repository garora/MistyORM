using System;
using System.Linq;
using System.Reflection;

namespace MistyORM.Miscellaneous
{
    internal static class Extensions
    {
        internal static PropertyInfo[] GetEntityProperties(this Type Type)
        {
            return Type.GetTypeInfo().GetProperties().Where(x => !x.GetMethod.IsVirtual).ToArray();
        }

        internal static bool HasAttribute<T>(this PropertyInfo Info) where T : Attribute
        {
            return Info.GetCustomAttribute<T>() != null;
        }
    }
}
