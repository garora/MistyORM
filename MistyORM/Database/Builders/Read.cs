using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Entities;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        internal static string Count<TEntity>(CompilerBase<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT COUNT(*) FROM `{typeof(TEntity).Name}`");

            if (Compiler.Compiled)
                Builder.Append($" WHERE {Compiler.ToConditionValues()}");
            
            Builder.Append(";");

            return Builder.ToString();
        }

        internal static string First<TEntity>(CompilerBase<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {Compiler.ToMemberFields()} FROM `{typeof(TEntity).Name}`");

            if (Compiler.Compiled)
                Builder.Append($" WHERE {Compiler.ToConditionValues()}");
            
            Builder.Append(" LIMIT 0, 1;");

            return Builder.ToString();
        }

        internal static string Select<TEntity>(CompilerBase<TEntity> Compiler) where TEntity : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {Compiler.ToMemberFields()} FROM `{typeof(TEntity).Name}`");

            if (Compiler.Compiled)
                Builder.Append($" WHERE {Compiler.ToConditionValues()}");

            Builder.Append(";");

            return Builder.ToString();
        }
    }
}
