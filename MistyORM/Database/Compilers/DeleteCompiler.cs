using System;
using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal class DeleteCompiler<TEntity> : CompilerBase<TEntity> where TEntity : TableEntity
    {
        internal DeleteCompiler()
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
        }
    }
}
