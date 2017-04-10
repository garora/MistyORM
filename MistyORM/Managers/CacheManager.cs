using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MistyORM.Entities;
using MistyORM.Entities.Attributes;
using MistyORM.Miscellaneous;

namespace MistyORM.Managers
{
    internal class DBTableField
    {
        internal string Name { get; set; }
        internal Type Type { get; set; }

        internal bool IsPrimaryKey { get; set; }
        internal bool IsForeignKey { get; set; }
        internal bool IsAutoIncrement { get; set; }
    }

    internal sealed class CacheManager : Singleton<CacheManager>
    {
        private Dictionary<TypeInfo, IEnumerable<DBTableField>> DBFieldCache;
        private bool Cached;

        private CacheManager()
        {
            Cached = false;
        }

        internal void Build()
        {
            if (Cached)
                return;

            DBFieldCache = Assembly.GetEntryAssembly().DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(TableEntity)))
                .ToDictionary(x => x, x => x.GetEntityProperties().Select(a => new DBTableField
                {
                    Name = a.Name,
                    Type = a.PropertyType,

                    IsPrimaryKey = a.HasAttribute<PrimaryKeyAttribute>(),
                    IsForeignKey = a.IsEntity(),
                    IsAutoIncrement = a.HasAttribute<AutoIncrementAttribute>()
                }));
            
            Cached = true;
        }
    }
}
