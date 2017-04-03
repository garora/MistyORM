using System;
using System.Collections.Concurrent;

using MistyORM.Database;
using MistyORM.Entities;

namespace MistyORM.Server
{
    public class DbServerInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool Pooling { get; set; }
        public int MinPoolSize { get; set; }
        public int MaxPoolSize { get; set; }
    }

    public sealed class DbServer
    {
        private readonly ConcurrentDictionary<Type, Db> DbStorage;
        private readonly DbServerInfo ServerInfo;

        public DbServer(DbServerInfo Info)
        {
            DbStorage = new ConcurrentDictionary<Type, Db>();
            ServerInfo = Info;
        }

        public Db Database<T>() where T : DbEntity
        {
            return DbStorage.GetOrAdd(typeof(T), new Db(typeof(T).GetType().Name, ServerInfo));
        }
    }
}
