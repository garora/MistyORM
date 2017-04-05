using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal class InsertCompiler : CompilerBase
    {
        internal InsertCompiler()
        {
            FieldParameterHolder = new Dictionary<string, DbParameter>();
        }

        internal override void Compile<T>(T Item)
        {
            PropertyInfo[] Properties = typeof(T).GetEntityProperties().Where(x => !x.HasAttribute<AutoIncrementAttribute>()).ToArray();

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
