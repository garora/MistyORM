using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

using MySql.Data.MySqlClient;

using MistyORM.Entities;

namespace MistyORM.Database.Compilers
{
    internal abstract class CompilerBase<TEntity> where TEntity : TableEntity
    {
        protected CompilerBase()
        {
        }

        internal void Compile<T>(T Item)
        {
            CompileImplementation(Item);

            Compiled = true;
        }

        protected virtual void CompileImplementation<T>(T Item)
        {
        }

        protected string AddParameter(string Name, DbParameter Parameter, ParameterType Type)
        {
            MySqlParameter NewParameter = new MySqlParameter
            {
                ParameterName = $"@{ParameterAbbreviation[Type]}{Parameter?.ParameterName ?? string.Empty}",
                Value = Parameter?.Value ?? new object()
            };

            Parameters.Add((Name, NewParameter, Type));

            return NewParameter.ParameterName;
        }

        internal IEnumerable<DbParameter> ToParameters() => Parameters.Select(x => x.Parameter);

        internal string ToMemberParameters() => string.Join(", ", Parameters.Where(x => x.Type == ParameterType.Member).Select(x => $"{x.Parameter.ParameterName}"));
        internal string ToMemberFields() => string.Join(", ", Parameters.Where(x => x.Type == ParameterType.Member).Select(x => $"`{x.Name}`"));
        internal string ToMemberValues() => string.Join(", ", Parameters.Where(x => x.Type == ParameterType.Member).Select(x => $"`{x.Name}` = {x.Parameter.ParameterName}"));

        internal string ToConditionValues()
        {
            if (this is ConditionCompiler<TEntity>)
                return (this as ConditionCompiler<TEntity>).ConditionBuilder.ToString();

            return string.Join(", ", Parameters.Where(x => x.Type == ParameterType.Condition).Select(x => $"`{x.Name}` = {x.Parameter.ParameterName}"));
        }

        protected enum ParameterType
        {
            Member = 1,
            Condition = 2
        }

        private readonly Dictionary<ParameterType, char> ParameterAbbreviation = new Dictionary<ParameterType, char>()
        {
            { ParameterType.Condition, 'c' },
            { ParameterType.Member, 'm' }
        };

        private readonly IList<(string Name, DbParameter Parameter, ParameterType Type)> Parameters = new List<(string, DbParameter, ParameterType)>();

        internal bool Compiled = false;
    }
}
