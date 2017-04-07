using System;
using System.Linq;
using System.Linq.Expressions;
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
            CompilerBase Compiler = new InsertCompiler();

            Compiler.Compile(Item);

            int ScalarResult = await InsertAsync(QueryBuilder.Insert<T>(Compiler), Compiler.ToParameters());

            PropertyInfo AutoIncrementProperty = typeof(T).GetEntityProperties().SingleOrDefault(x => x.GetCustomAttribute<AutoIncrementAttribute>() != null);
            if (AutoIncrementProperty != null)
                AutoIncrementProperty.SetValue(Item, ScalarResult);
        }

        public async Task<bool> Delete<T>(T Item) where T : TableEntity
        {
            CompilerBase Compiler = new DeleteCompiler();

            Compiler.Compile(Item);

            return await ExecuteAsync(QueryBuilder.Delete<T>(Compiler), Compiler.ToParameters());
        }

        public async Task<bool> Delete<T>(Expression<Func<T, bool>> Expression) where T : TableEntity
        {
            ConditionCompiler Compiler = new ConditionCompiler();

            Compiler.Compile(Expression);

            return await ExecuteAsync(QueryBuilder.Delete<T>(Compiler), Compiler.ToParameters());
        }

        public async Task<bool> Update<T>(T Item) where T : TableEntity
        {
            CompilerBase Compiler = new UpdateCompiler();

            Compiler.Compile(Item);

            return await ExecuteAsync(QueryBuilder.Update<T>(Compiler), Compiler.ToParameters());
        }
    }
}
