using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Entities;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        public static string Count<TEntity>(SelectCompiler<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT COUNT(*) FROM `{Compiler.GetTableName()}`");

            if (Compiler.Visited)
                Builder.Append($" WHERE {Compiler.GetConditions()}");
            
            Builder.Append(";");

            return Builder.ToString();
        }

        public static string First<TEntity>(SelectCompiler<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {Compiler.GetFields()} FROM `{Compiler.GetTableName()}`");

            if (Compiler.Visited)
                Builder.Append($" WHERE {Compiler.GetConditions()}");
            
            Builder.Append(" LIMIT 0, 1;");

            return Builder.ToString();
        }

        public static string Select<TEntity>(SelectCompiler<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {Compiler.GetFields()} FROM `{Compiler.GetTableName()}`");

            if (Compiler.Visited)
                Builder.Append($" WHERE {Compiler.GetConditions()}");

            Builder.Append(";");

            return Builder.ToString();
        }
    }
}
