using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MistyORM.Database.Builders;
using MistyORM.Database.Visitor;
using MistyORM.Entities;
using MistyORM.Entities.Builder;

namespace MistyORM.Database
{
    public partial class Db
    {
        public async Task<bool> Any<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity
        {
            ConditionVisitor Visitor = new ConditionVisitor();

            if (Expression != null)
                Visitor.Visit(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count<T>(Visitor), Visitor.Parameters);

            return Reader.HasRows;
        }

        public async Task<uint> Count<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity
        {
            ConditionVisitor Visitor = new ConditionVisitor();

            if (Expression != null)
                Visitor.Visit(Expression);

            DbDataReader Reader = await SelectAsync(QueryBuilder.Count<T>(Visitor), Visitor.Parameters);

            await Reader.ReadAsync();

            return Convert.ToUInt32(Reader[0]);
        }

        public async Task<T> First<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity, new()
        {
            ConditionVisitor Visitor = new ConditionVisitor();

            if (Expression != null)
                Visitor.Visit(Expression);

            T[] Result = EntityBuilder.Create<T>(await SelectAsync(QueryBuilder.First<T>(Visitor), Visitor.Parameters));

            return Result.Length > 0 ? Result[0] : null;
        }

        public async Task<T[]> Select<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity, new()
        {
            ConditionVisitor Visitor = new ConditionVisitor();

            if (Expression != null)
                Visitor.Visit(Expression);

            return EntityBuilder.Create<T>(await SelectAsync(QueryBuilder.Select<T>(Visitor), Visitor.Parameters));
        }

        public async Task<T> Single<T>(Expression<Func<T, bool>> Expression = null) where T : TableEntity, new()
        {
            ConditionVisitor Visitor = new ConditionVisitor();

            if (Expression != null)
                Visitor.Visit(Expression);

            T[] Result = EntityBuilder.Create<T>(await SelectAsync(QueryBuilder.Select<T>(Visitor), Visitor.Parameters));
            
            return Result.Length != 1 ? null : Result[0];
        }
    }
}
