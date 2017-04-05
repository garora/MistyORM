using System;
using System.Linq;
using System.Reflection;
using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Entities;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        internal static string Insert<T>(CompilerBase Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"INSERT INTO `{typeof(T).Name}` ({string.Join(", ", Compiler.GetFields().Select(x => $"`{x}`"))}) VALUES ({string.Join(", ", Compiler.GetParameters().Select(x => x.ParameterName))});");

            return Builder.ToString();
        }

        internal static string Delete<T>(CompilerBase Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(T).Name}` WHERE `{Compiler.GetFields().First()}` = @1;");

            return Builder.ToString();
        }

        internal static string Delete<T>(ConditionCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(T).Name}` WHERE {Compiler.ToConditions()};");

            return Builder.ToString();
        }
    }
}
