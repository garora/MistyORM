using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using MistyORM.Logging;
using MistyORM.Server;

namespace MistyORM.Database
{
    public partial class Db
    {
        private readonly string ConnectionString;
        private readonly ILog Log;

        public bool IsInitialized { get; private set; }

        internal Db(string DatabaseName, DbServerInfo ServerInfo)
        {
            ConnectionString = $"Server={ServerInfo.Host};User Id={ServerInfo.Username};Port={ServerInfo.Port};Password={ServerInfo.Password};Database={DatabaseName};Allow Zero Datetime=True;Pooling={ServerInfo.Pooling};CharSet=utf8;";

            if (ServerInfo.Pooling)
                ConnectionString += $"Min Pool Size={ServerInfo.MinPoolSize};Max Pool Size={ServerInfo.MaxPoolSize}";

            Log = new Log();

            IsInitialized = CreateConnection().State == ConnectionState.Open;
        }

        private DbConnection CreateConnection()
        {
            DbConnection Connection = new MySqlConnection();

            Connection.ConnectionString = ConnectionString;
            Connection.Open();

            return Connection;
        }

        private DbCommand CreateCommand(DbConnection Connection, string Sql, MySqlParameter[] Parameters)
        {
            DbCommand Command = new MySqlCommand();

            Command.CommandText = Sql;
            Command.Connection = Connection;
            Command.CommandTimeout = 30;

            Command.Parameters.AddRange(Parameters);

            return Command;
        }

        internal async Task<bool> ExecuteAsync(string Sql, MySqlParameter[] Parameters)
        {
            try
            {
                using (DbCommand Command = CreateCommand(CreateConnection(), Sql, Parameters))
                    return await Command.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception e)
            {
                Log.Out(e.ToString());
                return false;
            }
        }

        internal async Task<DbDataReader> SelectAsync(string Sql, MySqlParameter[] Parameters)
        {
            try
            {
                using (DbCommand Command = CreateCommand(CreateConnection(), Sql, Parameters))
                    return await Command.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                Log.Out(e.ToString());
                return null;
            }
        }
    }
}
