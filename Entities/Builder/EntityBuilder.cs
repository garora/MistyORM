using System;
using System.Collections.Generic;
using System.Data.Common;

using MistyORM.Managers;

namespace MistyORM.Entities.Builder
{
    internal static class EntityBuilder
    {
        public static IEnumerable<TEntity> Create<TEntity>(DbDataReader Reader) where TEntity : TableEntity, new()
        {
            IEnumerable<DBTableField> Fields = Manager.Cache.GetFields<TEntity>();

            while (Reader.Read())
            {
                TEntity Row = new TEntity();

                foreach (DBTableField Field in Fields)
                    Field.Property.SetValue(Row, Reader[Field.Name]);

                yield return Row;
            }
        }
    }
}
