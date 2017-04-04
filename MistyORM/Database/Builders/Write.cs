using System;
using System.Linq;
using System.Reflection;
using System.Text;

using MistyORM.Database.Compilers;
using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Builders
{
    internal static partial class QueryBuilder
    {
        internal static string Insert<T>(ParameterCompiler Compiler) where T : TableEntity
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append($"INSERT INTO `{typeof(T).Name}` ({string.Join(", ", Compiler.Fields.Select(x => $"`{x}`"))}) VALUES ({string.Join(", ", Compiler.Parameters.Select(x => x.ParameterName))});");

            return Builder.ToString();
        }
    }
}
