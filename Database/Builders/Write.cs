using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Entities;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        public static string Insert<TEntity>(InsertCompiler<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"INSERT INTO `{Compiler.GetTableName()}` ({Compiler.GetFields()}) VALUES ({Compiler.GetValueParameters()});");

            return Builder.ToString();
        }

        public static string Delete<TEntity>(DeleteCompiler<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{Compiler.GetTableName()}` WHERE {Compiler.GetConditions()};");

            return Builder.ToString();
        }

        public static string Update<TEntity>(UpdateCompiler<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"UPDATE `{Compiler.GetTableName()}` SET {Compiler.GetFieldValuePairs()} WHERE {Compiler.GetConditions()};");

            return Builder.ToString();
        }
    }
}
