using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using MistyORM.Database.Builders;
using MistyORM.Database.Compilers;
using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database
{
    public partial class Db
    {
        public async Task Insert<T>(T Item) where T : TableEntity
        {
            ParameterCompiler Compiler = new ParameterCompiler();

            Compiler.Compile(Item);

            int ScalarResult = await InsertAsync(QueryBuilder.Insert<T>(Compiler), Compiler.Parameters);

            PropertyInfo AutoIncrementProperty = typeof(T).GetProperties().SingleOrDefault(x => x.GetCustomAttribute<AutoIncrementAttribute>() != null);
            if (AutoIncrementProperty != null)
                AutoIncrementProperty.SetValue(Item, ScalarResult);
        }
    }
}
