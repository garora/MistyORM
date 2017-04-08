using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MistyORM.Database.Builders;
using MistyORM.Database.Compilers;
using MistyORM.Entities;
using MistyORM.Entities.Builder;

namespace MistyORM.Database
{
    public partial class Db
    {
        public async Task<bool> Any<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity
        {
            CompilerBase<TEntity> Compiler = new ConditionCompiler<TEntity>();

            if (Expression != null)
                Compiler.Compile(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count(Compiler), Compiler.ToParameters());

            return Reader.HasRows;
        }

        public async Task<uint> Count<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity
        {
            CompilerBase<TEntity> Compiler = new ConditionCompiler<TEntity>();

            if (Expression != null)
                Compiler.Compile(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count(Compiler), Compiler.ToParameters());

            await Reader.ReadAsync();

            return Convert.ToUInt32(Reader[0]);
        }

        public async Task<TEntity> First<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity, new()
        {
            CompilerBase<TEntity> Compiler = new ConditionCompiler<TEntity>();

            if (Expression != null)
                Compiler.Compile(Expression);

            TEntity[] Result = EntityBuilder.Create<TEntity>(await SelectAsync(QueryBuilder.First(Compiler), Compiler.ToParameters()));

            return Result.Length > 0 ? Result[0] : null;
        }

        public async Task<TEntity[]> Select<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity, new()
        {
            CompilerBase<TEntity> Compiler = new ConditionCompiler<TEntity>();

            if (Expression != null)
                Compiler.Compile(Expression);

            return EntityBuilder.Create<TEntity>(await SelectAsync(QueryBuilder.Select(Compiler), Compiler.ToParameters()));
        }

        public async Task<TEntity> Single<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity, new()
        {
            CompilerBase<TEntity> Compiler = new ConditionCompiler<TEntity>();

            if (Expression != null)
                Compiler.Compile(Expression);

            TEntity[] Result = EntityBuilder.Create<TEntity>(await SelectAsync(QueryBuilder.Select(Compiler), Compiler.ToParameters()));
            
            return Result.Length != 1 ? null : Result[0];
        }
    }
}
