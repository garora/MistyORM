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
    internal class UpdateCompiler : CompilerBase
    {
        internal UpdateCompiler() : base()
        {
        }

        internal override void Compile<T>(T Item)
        {
            PropertyInfo PrimaryProperty = typeof(T).GetEntityProperties().SingleOrDefault(x => x.HasAttribute<PrimaryKeyAttribute>());
            if (PrimaryProperty == null)
                throw new NullReferenceException($"Entity '{typeof(T).Name}' has no or multiple primary key set.");

            AddParameter(PrimaryProperty.Name, new MySqlParameter
            {
                ParameterName = "1",
                Value = PrimaryProperty.GetValue(Item)
            }, ParameterType.Condition);

            PropertyInfo[] Properties = typeof(T).GetEntityProperties().Where(x => !x.HasAttribute<PrimaryKeyAttribute>() && !x.HasAttribute<AutoIncrementAttribute>()).ToArray();

            for (int i = 1; i <= Properties.Length; ++i)
            {
                PropertyInfo Property = Properties[i - 1];

                AddParameter(Property.Name, new MySqlParameter
                {
                    ParameterName = i.ToString(),
                    Value = Property.GetValue(Item)
                }, ParameterType.Member);
            }
        }
    }
}
