using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.SqlData.Adapters
{
    public class SqlMsSqlAdapter : BaseSqlAdapter
    {
        public SqlMsSqlAdapter(DbsDataConfig config) : base(config)
        {
            m_connString = GetConnectString();
        }

        public override string GetConnectString()
        {
            string connectString = "";
            if (m_config.PlatformType == DbsDataConfigKeys.DATA_PROVIDER_ODBC_MSSQL)
            {
                string connectFormat = @"server={0};User Id={1};Password={2};database={3};";
                //connectString = String.Format(connectFormat, m_config.DbServerName, m_config.UserName, m_config.PlainUsersPsw(), m_config.DatabaseName);
                connectString = String.Format(connectFormat, m_config.DbServerName, m_config.OwnerName, m_config.PlainOwnerPsw(), m_config.DatabaseName);
            }
            else if (m_config.PlatformType == DbsDataConfigKeys.DATA_PROVIDER_ODBC_IMSSQL)
            {
                string connectFormat = @"server={0};integrated security=SSPI;database={1};";
                connectString = String.Format(connectFormat, m_config.DbServerName, m_config.DatabaseName);
            }
            return connectString;
        }

        public override string GetShortConnectString()
        {
            string connectString = "";
            if (m_config.PlatformType == DbsDataConfigKeys.DATA_PROVIDER_ODBC_MSSQL)
            {
                string connectFormat = @"server={0};database={3};";
                connectString = String.Format(connectFormat, m_config.DbServerName, m_config.OwnerName, m_config.PlainOwnerPsw(), m_config.DatabaseName);
            }
            else if (m_config.PlatformType == DbsDataConfigKeys.DATA_PROVIDER_ODBC_IMSSQL)
            {
                string connectFormat = @"server={0};integrated security=SSPI;database={1};";
                connectString = String.Format(connectFormat, m_config.DbServerName, m_config.DatabaseName);
            }
            return connectString;
        }

        public override void CreateDatabase()
        {
        }

        public override DbConnection CreateConnection()
        {
            m_conn = new System.Data.SqlClient.SqlConnection(m_connString);

            return m_conn;
        }

        public override DbDataAdapter GetAdapter(string dataFilter)
        {
            return new System.Data.SqlClient.SqlDataAdapter(dataFilter, m_conn as System.Data.SqlClient.SqlConnection);
        }

        public override DbCommand GetCommand(string commandSql)
        {
            return new System.Data.SqlClient.SqlCommand(commandSql, m_conn as System.Data.SqlClient.SqlConnection);
        }
    }
}
