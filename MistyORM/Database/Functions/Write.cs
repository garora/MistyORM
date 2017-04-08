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
        public async Task Insert<TEntity>(TEntity Item) where TEntity : TableEntity
        {
            CompilerBase<TEntity> Compiler = new InsertCompiler<TEntity>();

            Compiler.Compile(Item);

            int ScalarResult = await InsertAsync(QueryBuilder.Insert(Compiler), Compiler.ToParameters());

            PropertyInfo AutoIncrementProperty = typeof(TEntity).GetEntityProperties().SingleOrDefault(x => x.GetCustomAttribute<AutoIncrementAttribute>() != null);
            if (AutoIncrementProperty != null)
                AutoIncrementProperty.SetValue(Item, ScalarResult);
        }

        public async Task<bool> Delete<TEntity>(TEntity Item) where TEntity : TableEntity
        {
            CompilerBase<TEntity> Compiler = new DeleteCompiler<TEntity>();

            Compiler.Compile(Item);

            return await ExecuteAsync(QueryBuilder.Delete(Compiler), Compiler.ToParameters());
        }

        public async Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> Expression) where TEntity : TableEntity
        {
            CompilerBase<TEntity> Compiler = new ConditionCompiler<TEntity>();

            Compiler.Compile(Expression);

            return await ExecuteAsync(QueryBuilder.Delete(Compiler), Compiler.ToParameters());
        }

        public async Task<bool> Update<TEntity>(TEntity Item) where TEntity : TableEntity
        {
            CompilerBase<TEntity> Compiler = new UpdateCompiler<TEntity>();

            Compiler.Compile(Item);

            return await ExecuteAsync(QueryBuilder.Update(Compiler), Compiler.ToParameters());
        }
    }
}
