using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Entities;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        internal static string Insert<TEntity>(CompilerBase<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"INSERT INTO `{typeof(TEntity).Name}` ({Compiler.ToMemberFields()}) VALUES ({Compiler.ToMemberValues()});");

            return Builder.ToString();
        }

        internal static string Delete<TEntity>(CompilerBase<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(TEntity).Name}` WHERE {Compiler.ToConditionValues()};");

            return Builder.ToString();
        }

        internal static string Delete<TEntity>(ConditionCompiler<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(TEntity).Name}` WHERE {Compiler.ToConditionValues()};");

            return Builder.ToString();
        }

        internal static string Update<TEntity>(CompilerBase<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"UPDATE `{typeof(TEntity).Name}` SET {Compiler.ToMemberValues()} WHERE {Compiler.ToConditionValues()};");

            return Builder.ToString();
        }
    }
}
