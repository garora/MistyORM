using System;
using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal class UpdateCompiler<TEntity> : CompilerBase<TEntity> where TEntity : TableEntity
    {
        internal UpdateCompiler()
        {
        }

        protected override void CompileImplementation<T>(T Item)
        {
            PropertyInfo PrimaryProperty = typeof(TEntity).GetEntityProperties().SingleOrDefault(x => x.HasAttribute<PrimaryKeyAttribute>());
            if (PrimaryProperty == null)
                throw new NullReferenceException($"Entity '{typeof(TEntity).Name}' has no or multiple primary key set.");

            AddParameter(PrimaryProperty.Name, new MySqlParameter
            {
                ParameterName = "1",
                Value = PrimaryProperty.GetValue(Item)
            }, ParameterType.Condition);

            PropertyInfo[] Properties = typeof(TEntity).GetEntityProperties().Where(x => !x.HasAttribute<PrimaryKeyAttribute>() && !x.HasAttribute<AutoIncrementAttribute>()).ToArray();

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
