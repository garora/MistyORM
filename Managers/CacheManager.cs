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
        public string Name { get; set; }
        public Type Type { get; set; }

        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsAutoIncrement { get; set; }

        public PropertyInfo Property { get; set; }
    }

    internal sealed class CacheManager : Singleton<CacheManager>
    {
        private readonly Dictionary<Type, IEnumerable<DBTableField>> DBFieldCache;
        private bool Cached;

        private CacheManager()
        {
            DBFieldCache = new Dictionary<Type, IEnumerable<DBTableField>>();
            Cached = false;
        }

        public void Build()
        {
            if (Cached)
                return;
            
            foreach (TypeInfo TypeInfo in Assembly.GetEntryAssembly().DefinedTypes)
            {
                if (!TypeInfo.IsSubclassOf(typeof(TableEntity)))
                    continue;

                if (TypeInfo.GetEntityProperties().SingleOrDefault(x => x.HasAttribute<PrimaryKeyAttribute>()) == null)
                    throw new Exception($"Entity '{typeof(TableEntity).Name}' has no or multiple primary keys defined.");

                DBFieldCache.Add(TypeInfo.AsType(), TypeInfo.GetEntityProperties().Select(a => new DBTableField
                {
                    Name = a.Name,
                    Type = a.PropertyType,

                    IsPrimaryKey = a.HasAttribute<PrimaryKeyAttribute>(),
                    IsForeignKey = a.IsEntity(),
                    IsAutoIncrement = a.HasAttribute<AutoIncrementAttribute>(),

                    Property = a
                }));

            }
            
            Cached = true;
        }

        public IEnumerable<DBTableField> GetFields<T>() where T : TableEntity => DBFieldCache[typeof(T)];
    }
}
