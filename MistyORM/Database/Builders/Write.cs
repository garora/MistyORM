using System;
using System.Linq;
using System.Reflection;
using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Database.Visitor;
using MistyORM.Entities;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        internal static string Insert<T>(ICompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"INSERT INTO `{typeof(T).Name}` ({string.Join(", ", Compiler.GetFields().Select(x => $"`{x}`"))}) VALUES ({string.Join(", ", Compiler.GetParameters().Select(x => x.ParameterName))});");

            return Builder.ToString();
        }

        internal static string Delete<T>(ICompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(T).Name}` WHERE `{Compiler.GetFields().First()}` = @1;");

            return Builder.ToString();
        }

        internal static string Delete<T>(ConditionVisitor Visitor) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(T).Name}` WHERE {Visitor.Conditions};");

            return Builder.ToString();
        }
    }
}
