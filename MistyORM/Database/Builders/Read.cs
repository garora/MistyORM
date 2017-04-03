using System.Linq;
using System.Text;

using MistyORM.Database.Visitor;
using MistyORM.Entities;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        internal static string Count<T>(ConditionVisitor Visitor) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT COUNT(*) FROM `{typeof(T).Name}`");

            if (Visitor.Visited)
                Builder.Append($" WHERE {Visitor.Conditions}");
            
            Builder.Append(";");

            return Builder.ToString();
        }

        internal static string First<T>(ConditionVisitor Visitor) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {string.Join(", ", typeof(T).GetProperties().Select(x => $"`{x.Name}`"))} FROM `{typeof(T).Name}`");

            if (Visitor.Visited)
                Builder.Append($" WHERE {Visitor.Conditions}");
            
            Builder.Append(" LIMIT 0, 1;");

            return Builder.ToString();
        }

        internal static string Select<T>(ConditionVisitor Visitor) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"SELECT {string.Join(", ", typeof(T).GetProperties().Select(x => $"`{x.Name}`"))} FROM `{typeof(T).Name}`");

            if (Visitor.Visited)
                Builder.Append($" WHERE {Visitor.Conditions}");

            Builder.Append(";");

            return Builder.ToString();
        }
    }
}
