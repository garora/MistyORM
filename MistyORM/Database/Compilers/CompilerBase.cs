using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

using MistyORM.Entities;

namespace MistyORM.Database.Compilers
{
    internal abstract class CompilerBase
    {
        internal virtual IDictionary<string, DbParameter> FieldParameterHolder { get; set; }

        internal virtual void Compile<T>(T Item)
        {
        }

        internal IEnumerable<string> GetFields() => FieldParameterHolder.Keys;
        internal IEnumerable<DbParameter> GetParameters() => FieldParameterHolder.Values;
    }
}
