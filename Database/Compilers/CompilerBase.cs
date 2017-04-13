using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Database.Visitors;
using MistyORM.Miscellaneous;

namespace MistyORM.Database.Compilers
{
    internal abstract class CompilerBase<TEntity> where TEntity : TableEntity
    {
        public bool Compiled { get; private set; } = false;

        protected readonly IDictionary<string, MySqlParameter> FieldValueMap = new Dictionary<string, MySqlParameter>();
        protected (string Field, MySqlParameter Parameter) SingleCondition;

        protected ConditionVisitor Visitor;
        public bool Visited { get; private set; } = false;

        protected int ParameterIdentifier = 0;

        public CompilerBase()
        {
        }
        
        public void Compile(object Input)
        {
            if (Input is Expression)
            {
                Visitor = new ConditionVisitor();
                Visitor.Visit(Input as Expression);

                foreach (MySqlParameter Parameter in Visitor.GetParameters())
                    FieldValueMap.Add(Parameter.ParameterName, Parameter);
                
                Visited = true;
            }

            CompilerImplementation(Input as TEntity);

            Compiled = true;
        }

        protected virtual void CompilerImplementation(TEntity Entity)
        {
        }

        public string GetFields() => string.Join(", ", FieldValueMap.Keys.Select(x => $"`{x}`"));
        public string GetFieldValuePairs() => string.Join(", ", FieldValueMap.Select(x => $"`{x.Key}` = {x.Value.ParameterName}"));
        public string GetConditions() => Visited ? Visitor.GetConditions() : $"`{SingleCondition.Field}` = {SingleCondition.Parameter.ParameterName}";
        public string GetTableName() => typeof(TEntity).Name;
        public string GetValueParameters() => string.Join(", ", FieldValueMap.Values.Select(x => x.ParameterName));
        public IEnumerable<DbParameter> GetParameters()
        {
            if (Visited)
                return Visitor.GetParameters();

            if (SingleCondition.Parameter != null)
                return FieldValueMap.Values.Append(SingleCondition.Parameter);
            
            return FieldValueMap.Values;
        }
    }
}
