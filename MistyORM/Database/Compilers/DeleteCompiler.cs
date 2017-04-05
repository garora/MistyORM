using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal class DeleteCompiler : CompilerBase
    {
        internal DeleteCompiler()
        {
            FieldParameterHolder = new Dictionary<string, DbParameter>();
        }

        internal override void Compile<T>(T Item)
        {
            PropertyInfo PrimaryProperty = typeof(T).GetEntityProperties().SingleOrDefault(x => x.HasAttribute<PrimaryKeyAttribute>());
            if (PrimaryProperty == null)
                throw new NullReferenceException($"Entity '{typeof(T).Name}' has no or multiple primary key set.");

            FieldParameterHolder.Add(PrimaryProperty.Name, new MySqlParameter
            {
                ParameterName = "@1",
                Value = PrimaryProperty.GetValue(Item)
            });
        }
    }
}
