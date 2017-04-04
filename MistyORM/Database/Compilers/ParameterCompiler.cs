using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal class ParameterCompiler
    {
        private readonly Dictionary<string, DbParameter> FieldParameterHolder;

        internal DbParameter[] Parameters => FieldParameterHolder.Values.ToArray();
        internal string[] Fields => FieldParameterHolder.Keys.ToArray();

        internal ParameterCompiler()
        {
            FieldParameterHolder = new Dictionary<string, DbParameter>();
        }

        internal void Compile<T>(T Item) where T : TableEntity
        {
            PropertyInfo[] Properties = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<AutoIncrementAttribute>() == null).ToArray();

            for (int i = 1; i <= Properties.Length; ++i)
            {
                PropertyInfo Property = Properties[i - 1];

                FieldParameterHolder.Add(Property.Name, new MySqlParameter
                {
                    ParameterName = "@" + i,
                    Value = Property.GetValue(Item)
                });
            }
        }
    }
}
