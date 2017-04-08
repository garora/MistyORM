using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

using MistyORM.Miscellaneous;

namespace MistyORM.Entities.Builder
{
    internal static class EntityBuilder
    {
        internal static TEntity[] Create<TEntity>(DbDataReader Reader) where TEntity : TableEntity, new()
        {
            List<TEntity> Result = new List<TEntity>();

            PropertyInfo[] Properties = typeof(TEntity).GetEntityProperties();

            while (Reader.Read())
            {
                TEntity Row = new TEntity();

                foreach (PropertyInfo Info in Properties)
                    Info.SetValue(Row, Reader[Info.Name]);

                Result.Add(Row);
            }

            return Result.ToArray();
        }
    }
}
