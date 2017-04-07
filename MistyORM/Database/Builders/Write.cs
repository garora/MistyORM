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

            Builder.Append($"INSERT INTO `{typeof(T).Name}` ({Compiler.ToMemberFields()}) VALUES ({Compiler.ToMemberValues()});");

            return Builder.ToString();
        }

        internal static string Delete<T>(CompilerBase Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(T).Name}` WHERE {Compiler.ToConditionValues()};");

            return Builder.ToString();
        }

        internal static string Delete<T>(ConditionCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"DELETE FROM `{typeof(T).Name}` WHERE {Compiler.ToConditionValues()};");

            return Builder.ToString();
        }

        internal static string Update<T>(CompilerBase Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"UPDATE `{typeof(T).Name}` SET {Compiler.ToMemberValues()} WHERE {Compiler.ToConditionValues()};");

            return Builder.ToString();
        }
    }
}
