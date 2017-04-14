using System.Linq;

using MySql.Data.MySqlClient;

using MistyORM.Entities;
using MistyORM.Managers;

namespace MistyORM.Database.Compilers
{
    internal sealed class DeleteCompiler<TEntity> : CompilerBase<TEntity> where TEntity : TableEntity
    {
        public DeleteCompiler()
        {
        }

        protected override void CompilerImplementation(TEntity Entity)
        {
            DBTableField PrimaryField = Manager.Cache.GetFields<TEntity>().SingleOrDefault(x => x.IsPrimaryKey);

            SingleCondition = (PrimaryField.Name, new MySqlParameter
            {
                ParameterName = "@0",
                Value = PrimaryField.Property.GetValue(Entity)
            });
        }
    }
}
