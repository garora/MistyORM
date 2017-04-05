using System.Linq;
using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Entities;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        internal static string Count<T>(ConditionCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT COUNT(*) FROM `{typeof(T).Name}`");

            if (Compiler.Compiled)
                Builder.Append($" WHERE {Compiler.ToConditions()}");
            
            Builder.Append(";");

            return Builder.ToString();
        }

        internal static string First<T>(ConditionCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {string.Join(", ", typeof(T).GetEntityProperties().Select(x => $"`{x.Name}`"))} FROM `{typeof(T).Name}`");

            if (Compiler.Compiled)
                Builder.Append($" WHERE {Compiler.ToConditions()}");
            
            Builder.Append(" LIMIT 0, 1;");

            return Builder.ToString();
        }

        internal static string Select<T>(ConditionCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {string.Join(", ", typeof(T).GetEntityProperties().Select(x => $"`{x.Name}`"))} FROM `{typeof(T).Name}`");

            if (Compiler.Compiled)
                Builder.Append($" WHERE {Compiler.ToConditions()}");

            Builder.Append(";");

            return Builder.ToString();
        }
    }
}
