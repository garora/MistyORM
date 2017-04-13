using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
            SelectCompiler<TEntity> Compiler = new SelectCompiler<TEntity>();

            Compiler.Compile(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count(Compiler), Compiler.GetParameters());

            return Reader.HasRows;
        }

        public async Task<uint> Count<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity
        {
            SelectCompiler<TEntity> Compiler = new SelectCompiler<TEntity>();

            Compiler.Compile(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count(Compiler), Compiler.GetParameters());

            await Reader.ReadAsync();

            return Convert.ToUInt32(Reader[0]);
        }

        public async Task<TEntity> First<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity, new()
        {
            SelectCompiler<TEntity> Compiler = new SelectCompiler<TEntity>();

            Compiler.Compile(Expression);

            return EntityBuilder.Create<TEntity>(await SelectAsync(QueryBuilder.First(Compiler), Compiler.GetParameters())).FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> Select<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity, new()
        {
            SelectCompiler<TEntity> Compiler = new SelectCompiler<TEntity>();

            Compiler.Compile(Expression);

            return EntityBuilder.Create<TEntity>(await SelectAsync(QueryBuilder.Select(Compiler), Compiler.GetParameters()));
        }

        public async Task<TEntity> Single<TEntity>(Expression<Func<TEntity, bool>> Expression = null) where TEntity : TableEntity, new()
        {
            SelectCompiler<TEntity> Compiler = new SelectCompiler<TEntity>();

            Compiler.Compile(Expression);

            return EntityBuilder.Create<TEntity>(await SelectAsync(QueryBuilder.Select(Compiler), Compiler.GetParameters())).SingleOrDefault();
        }
    }
}
