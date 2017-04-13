using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Managers;

namespace MistyORM.Database.Compilers
{
    internal sealed class UpdateCompiler<TEntity> : CompilerBase<TEntity> where TEntity : TableEntity
    {
        public UpdateCompiler()
        {
        }

        protected override void CompilerImplementation(TEntity Entity)
        {
            foreach (DBTableField Field in Manager.Cache.GetFields<TEntity>())
            {
                if (Field.IsPrimaryKey)
                {
                    SingleCondition = (Field.Name, new MySqlParameter
                    {
                        ParameterName = "@0",
                        Value = Field.Property.GetValue(Entity)
                    });
                    continue;
                }

                if (Field.IsAutoIncrement)
                    continue;
                
                FieldValueMap.Add(Field.Name, new MySqlParameter
                {
                    ParameterName = "@" + ++ParameterIdentifier,
                    Value = Field.Property.GetValue(Entity)
                });
            }
        }
    }
}
