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
        public async Task<bool> Any<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity
        {
            ConditionCompiler Compiler = new ConditionCompiler();

            if (Expression != null)
                Compiler.Compile(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count<T>(Compiler), Compiler.GetParameters());

            return Reader.HasRows;
        }

        public async Task<uint> Count<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity
        {
            ConditionCompiler Compiler = new ConditionCompiler();

            if (Expression != null)
                Compiler.Compile(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count<T>(Compiler), Compiler.GetParameters());

            await Reader.ReadAsync();

            return Convert.ToUInt32(Reader[0]);
        }

        public async Task<T> First<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity, new()
        {
            ConditionCompiler Compiler = new ConditionCompiler();

            if (Expression != null)
                Compiler.Compile(Expression);

            T[] Result = EntityBuilder.Create<T>(await SelectAsync(QueryBuilder.First<T>(Compiler), Compiler.GetParameters()));

            return Result.Length > 0 ? Result[0] : null;
        }

        public async Task<T[]> Select<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity, new()
        {
            ConditionCompiler Compiler = new ConditionCompiler();

            if (Expression != null)
                Compiler.Compile(Expression);

            return EntityBuilder.Create<T>(await SelectAsync(QueryBuilder.Select<T>(Compiler), Compiler.GetParameters()));
        }

        public async Task<T> Single<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity, new()
        {
            ConditionCompiler Compiler = new ConditionCompiler();

            if (Expression != null)
                Compiler.Compile(Expression);

            T[] Result = EntityBuilder.Create<T>(await SelectAsync(QueryBuilder.Select<T>(Compiler), Compiler.GetParameters()));
            
            return Result.Length != 1 ? null : Result[0];
        }
    }
}
