using System;
using System.Reflection;

namespace MistyORM.Miscellaneous
{
    internal abstract class Singleton<T> where T : class
    {
        internal static T Instance => LazyInstance.Value;
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(() =>
        {
            ConstructorInfo[] Constructors = typeof(T).GetTypeInfo().GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            return Constructors[0].Invoke(new object[0]) as T;
        });
    }
}
