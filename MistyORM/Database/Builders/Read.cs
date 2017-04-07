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

            if (Compiler != null)
                Builder.Append($" WHERE {Compiler.ToConditionValues()}");
            
            Builder.Append(";");

            return Builder.ToString();
        }

        internal static string First<T>(ConditionCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT * FROM `{typeof(T).Name}`");

            if (Compiler != null)
                Builder.Append($" WHERE {Compiler.ToConditionValues()}");
            
            Builder.Append(" LIMIT 0, 1;");

            return Builder.ToString();
        }

        internal static string Select<T>(ConditionCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT * FROM `{typeof(T).Name}`");

            if (Compiler != null)
                Builder.Append($" WHERE {Compiler.ToConditionValues()}");

            Builder.Append(";");

            return Builder.ToString();
        }
    }
}
