using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;
using MigrateDataLib.Schema.DefCopyItems;

namespace MigrateDataLib.SqlData.Adapters
{
    public abstract class BaseSqlAdapter : ISqlAdapter
    { 
        protected DbsDataConfig m_config;

        protected DbConnection m_conn;

        protected UInt32 m_platformType;

        protected string m_connString;

        protected string m_databaseName;

        protected string m_databaseOwnr;

        protected string m_databaseUser;

        public BaseSqlAdapter(DbsDataConfig config)
        {
            m_config = config;

            m_platformType = config.PlatformType;

            m_databaseName = config.DatabaseName;

            m_databaseOwnr = config.OwnerName;

            m_databaseUser = config.UserName;
        }

        public abstract string GetConnectString();
        public abstract string GetShortConnectString();

        public DbsDataConfig Config()
        {
            return m_config;
        }

        public UInt32 PlatformType()
        {
            return m_platformType;
        }
        public bool IsMsSQLType()
        {
            return (m_platformType == DBPlatform.DATA_PROVIDER_ODBC_MSSQL || m_platformType == DBPlatform.DATA_PROVIDER_ODBC_IMSSQL);
        }
        public bool IsOracleType()
        {
            return (m_platformType == DBPlatform.DATA_PROVIDER_ODBC_ORACLE);
        }

        public string DatabaseName()
        {
            return m_databaseName;
        }

        public string DatabaseOwnr()
        {
            return m_databaseOwnr;
        }
        public string DatabaseUser()
        {
            return m_databaseUser;
        }

        public DbConnection DbConnection()
        {
            return m_conn;
        }

        public void OpenConnection()
        {
            m_conn.Open();
        }

        public void CloseConnection()
        {
            m_conn.Close();
        }

        public abstract void CreateDatabase();
        public abstract DbConnection CreateConnection();
        public abstract DbDataAdapter GetAdapter(string dataFilter);
        public abstract DbCommand GetCommand(string commandSql);
        public Int32 SelectRowCountTableData(TableDefInfo tableInfo)
        {
            string sqlSelectCountTableRows = CreateSelectCountTableRow(tableInfo);

            DbCommand countCD = GetCommand(sqlSelectCountTableRows);

            Int32 commandResult = 0;

            using (var reader = countCD.ExecuteReader())
            {
                if (reader.Read())
                {
                    commandResult = GetCountFromReader(reader, "POCET");
                }
            }
            return commandResult;
        }
        public Int32 SelectRowCountTableData(string tableName, string tableFrom)
        {
            string sqlSelectCountTableRows = CreateSelectCountTableRow(tableFrom);

            DbCommand countCD = GetCommand(sqlSelectCountTableRows);

            Int32 commandResult = 0;

            using (var reader = countCD.ExecuteReader())
            {
                if (reader.Read())
                {
                    commandResult = GetCountFromReader(reader, "POCET");
                }
            }
            return commandResult;
        }
        public Int32 SelectDiffCount(string diffCommand)
        {
            DbCommand countCD = GetCommand(diffCommand);

            Int32 diffCount = 0;

            using (var reader = countCD.ExecuteReader())
            {
                if (reader.Read())
                {
                    diffCount = GetCountFromReader(reader, "DIFF_COUNT");
                }
            }
            return diffCount;
        }

        private Int32 GetCountFromReader(DbDataReader reader, string columnName)
        {
            Int32 commandResult = 0;

            int columnOrdinal = reader.GetOrdinal(columnName);

            if (IsOracleType())
            {
                object commandResultObj = reader.GetValue(columnOrdinal);
                string commandResultStr = commandResultObj.ToString();

                commandResult = Int32.Parse(commandResultStr);
            }
            else
            {
                try
                {
                    commandResult = reader.GetInt32(columnOrdinal);
                }
                catch (InvalidCastException e)
                {
                    object commandResultObj = reader.GetValue(columnOrdinal);
                    string commandResultStr = commandResultObj.ToString();

                    commandResult = Int32.Parse(commandResultStr);
                }
            }
            return commandResult;
        }

        private string CreateSelectCountTableRow(TableDefInfo tableInfo)
        {
            StringBuilder strSQL = new StringBuilder("SELECT count(*) AS POCET FROM ");

            strSQL.Append(tableInfo.TableName());

            return strSQL.ToString();
        }

        private string CreateSelectCountTableRow(string tableFrom)
        {
            StringBuilder strSQL = new StringBuilder("SELECT count(*) AS POCET ");

            strSQL.Append(tableFrom);

            return strSQL.ToString();
        }

        public void Dispose()
        {
            if (m_conn != null)
            {
                m_conn.Close();

                m_conn.Dispose();
            }
        }
    }
}

