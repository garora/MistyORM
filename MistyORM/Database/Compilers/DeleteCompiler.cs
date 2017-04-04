using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;
using System;

namespace MistyORM.Database.Compilers
{
    internal class DeleteCompiler : ICompiler
    {
        private readonly Dictionary<string, DbParameter> FieldParameterHolder;

        internal DeleteCompiler()
        {
            FieldParameterHolder = new Dictionary<string, DbParameter>();
        }

        void ICompiler.Compile<T>(T Item)
        {
            PropertyInfo PrimaryProperty = typeof(T).GetEntityProperties().SingleOrDefault(x => x.GetCustomAttribute<PrimaryKeyAttribute>() != null);
            if (PrimaryProperty == null)
                throw new NullReferenceException($"Entity '{typeof(T).Name}' has no primary key set.");

            FieldParameterHolder.Add(PrimaryProperty.Name, new MySqlParameter
            {
                ParameterName = "@1",
                Value = PrimaryProperty.GetValue(Item)
            });
        }

        string[] ICompiler.GetFields() => FieldParameterHolder.Keys.ToArray();

        DbParameter[] ICompiler.GetParameters() => FieldParameterHolder.Values.ToArray();
    }
}
