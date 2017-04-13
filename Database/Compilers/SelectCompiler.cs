using MistyORM.Entities;
using MistyORM.Managers;

namespace MistyORM.Database.Compilers
{
    internal sealed class SelectCompiler<TEntity> : CompilerBase<TEntity> where TEntity : TableEntity
    {
        public SelectCompiler()
        {
        }

        protected override void CompilerImplementation(TEntity Entity)
        {
            foreach (DBTableField Field in Manager.Cache.GetFields<TEntity>())
                FieldValueMap.Add(Field.Name, null);
        }
    }
}
