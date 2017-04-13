using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MistyORM.Database.Builders;
using MistyORM.Database.Compilers;
using MistyORM.Entities;
using MistyORM.Managers;

namespace MistyORM.Database
{
    public partial class Db
    {
        public async Task Insert<TEntity>(TEntity Item) where TEntity : TableEntity
        {
            InsertCompiler<TEntity> Compiler = new InsertCompiler<TEntity>();

            Compiler.Compile(Item);

            int ScalarResult = await InsertAsync(QueryBuilder.Insert(Compiler), Compiler.GetParameters());

            DBTableField AutoIncrementField = Manager.Cache.GetFields<TEntity>().SingleOrDefault(x => x.IsAutoIncrement);
            if (AutoIncrementField != null)
                AutoIncrementField.Property.SetValue(Item, ScalarResult);
        }

        public async Task<bool> Delete<TEntity>(TEntity Item) where TEntity : TableEntity
        {
            DeleteCompiler<TEntity> Compiler = new DeleteCompiler<TEntity>();

            Compiler.Compile(Item);

            return await ExecuteAsync(QueryBuilder.Delete(Compiler), Compiler.GetParameters());
        }

        public async Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> Expression) where TEntity : TableEntity
        {
            DeleteCompiler<TEntity> Compiler = new DeleteCompiler<TEntity>();

            Compiler.Compile(Expression);

            return await ExecuteAsync(QueryBuilder.Delete(Compiler), Compiler.GetParameters());
        }

        public async Task<bool> Update<TEntity>(TEntity Item) where TEntity : TableEntity
        {
            UpdateCompiler<TEntity> Compiler = new UpdateCompiler<TEntity>();

            Compiler.Compile(Item);

            return await ExecuteAsync(QueryBuilder.Update(Compiler), Compiler.GetParameters());
        }
    }
}
