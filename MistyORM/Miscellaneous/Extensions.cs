using System;
using System.Linq;
using System.Reflection;

namespace MistyORM.Miscellaneous
{
    internal static class Extensions
    {
        internal static PropertyInfo[] GetProperties(this Type Type)
        {
            return Type.GetRuntimeProperties().Where(x => !x.GetMethod.IsVirtual).ToArray();
        }
    }
}
