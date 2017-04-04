using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

using MistyORM.Miscellaneous;

namespace MistyORM.Entities.Builder
{
    internal static class EntityBuilder
    {
        internal static T[] Create<T>(DbDataReader Reader) where T : TableEntity, new()
        {
            List<T> Result = new List<T>();

            PropertyInfo[] Properties = typeof(T).GetEntityProperties();

            while (Reader.Read())
            {
                T Row = new T();

                foreach (PropertyInfo Info in Properties)
                    Info.SetValue(Row, Reader[Info.Name]);

                Result.Add(Row);
            }

            return Result.ToArray();
        }
    }
}
