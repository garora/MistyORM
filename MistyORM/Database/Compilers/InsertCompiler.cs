using System.Linq;
using System.Reflection;

using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal class InsertCompiler<TEntity> : CompilerBase<TEntity> where TEntity : TableEntity
    {
        internal InsertCompiler()
        {
        }

        protected override void CompileImplementation<T>(T Item)
        {
            PropertyInfo[] Properties = typeof(TEntity).GetEntityProperties().Where(x => !x.HasAttribute<AutoIncrementAttribute>()).ToArray();

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
