using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.SqlData.Adapters
{
    public class SqlSqliteAdapter : BaseSqlAdapter
    {
        public SqlSqliteAdapter(DbsDataConfig config) : base(config)
        {
            m_connString = GetConnectString();
        }
        public override string GetConnectString()
        {
            string connectString = "";

            string connectFormat = @"Data Source={0};Version=3;";
            connectString = String.Format(connectFormat, m_config.DataFileName);

            return connectString;
        }
        public override string GetShortConnectString()
        {
            string connectString = "";

            string connectFormat = @"Data Source={0};Version=3;";
            connectString = String.Format(connectFormat, m_config.DataFileName);

            return connectString;
        }
        public override void CreateDatabase()
        {
            System.Data.SQLite.SQLiteConnection.CreateFile(m_databaseName);
        }

        public override DbConnection CreateConnection()
        {
            m_conn = new System.Data.SQLite.SQLiteConnection(m_connString);

            return m_conn;
        }

        public override DbDataAdapter GetAdapter(string dataFilter)
        {
            return new System.Data.SQLite.SQLiteDataAdapter(dataFilter, m_conn as System.Data.SQLite.SQLiteConnection);
        }

        public override DbCommand GetCommand(string commandSql)
        {
            return new System.Data.SQLite.SQLiteCommand(commandSql, m_conn as System.Data.SQLite.SQLiteConnection);
        }
    }
}
