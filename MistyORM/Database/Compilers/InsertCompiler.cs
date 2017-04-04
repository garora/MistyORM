using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal class InsertCompiler : ICompiler
    {
        private readonly Dictionary<string, DbParameter> FieldParameterHolder;

        internal InsertCompiler()
        {
            FieldParameterHolder = new Dictionary<string, DbParameter>();
        }

        void ICompiler.Compile<T>(T Item)
        {
            PropertyInfo[] Properties = typeof(T).GetEntityProperties().Where(x => x.GetCustomAttribute<AutoIncrementAttribute>() == null).ToArray();

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

        string[] ICompiler.GetFields() => FieldParameterHolder.Keys.ToArray();

        DbParameter[] ICompiler.GetParameters() => FieldParameterHolder.Values.ToArray();
    }
}
