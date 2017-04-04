using System.Data.Common;

using MistyORM.Entities;

namespace MistyORM.Database.Compilers
{
    internal interface ICompiler
    {
        string[] GetFields();
        DbParameter[] GetParameters();
        void Compile<T>(T Item) where T : TableEntity;
    }
}
