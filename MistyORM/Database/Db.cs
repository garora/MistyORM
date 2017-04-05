using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using MistyORM.Logging;
using MistyORM.Server;

namespace MistyORM.Database
{
    public partial class Db
    {
        private readonly string ConnectionString;
        private readonly ILogger Logger;

        internal Db(string DatabaseName, DbServerInfo ServerInfo)
        {
            ConnectionString = $"Server={ServerInfo.Host};User Id={ServerInfo.Username};Port={ServerInfo.Port};Password={ServerInfo.Password};Database={DatabaseName};Pooling={ServerInfo.Pooling};CharSet=utf8;";

            if (ServerInfo.Pooling)
                ConnectionString += $"Min Pool Size={ServerInfo.MinPoolSize};Max Pool Size={ServerInfo.MaxPoolSize}";

            Logger = new ConsoleLogger();
        }

        private DbConnection CreateConnection()
        {
            DbConnection Connection = new MySqlConnection();

            Connection.ConnectionString = ConnectionString;
            Connection.Open();

            return Connection;
        }

        private DbCommand CreateCommand(DbConnection Connection, string Sql, IEnumerable<DbParameter> Parameters)
        {
            DbCommand Command = new MySqlCommand();

            Command.CommandText = Sql;
            Command.Connection = Connection;
            Command.CommandTimeout = 30;

            Command.Parameters.AddRange(Parameters.ToArray());

            return Command;
        }

        private async Task<bool> ExecuteAsync(string Sql, IEnumerable<DbParameter> Parameters)
        {
            try
            {
                using (DbCommand Command = CreateCommand(CreateConnection(), Sql, Parameters))
                    return await Command.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception e)
            {
                Logger.Out(e.ToString());
                return false;
            }
        }

        private async Task<DbDataReader> SelectAsync(string Sql, IEnumerable<DbParameter> Parameters)
        {
            try
            {
                using (DbCommand Command = CreateCommand(CreateConnection(), Sql, Parameters))
                    return await Command.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                Logger.Out(e.ToString());
                return null;
            }
        }

        private async Task<int> InsertAsync(string Sql, IEnumerable<DbParameter> Parameters)
        {
            try
            {
                using (DbCommand Command = CreateCommand(CreateConnection(), Sql + " SELECT LAST_INSERT_ID();", Parameters))
                    return Convert.ToInt32(await Command.ExecuteScalarAsync());
            }
            catch (Exception e)
            {
                Logger.Out(e.ToString());
                return -1;
            }
        }
    }
}
