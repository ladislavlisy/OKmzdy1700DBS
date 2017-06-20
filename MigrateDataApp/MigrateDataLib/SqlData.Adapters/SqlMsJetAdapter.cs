using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.SqlData.Adapters
{
    public class SqlMsJetAdapter : BaseSqlAdapter
    {
        public SqlMsJetAdapter(DbsDataConfig config) : base(config)
        {
            m_connString = GetConnectString();
        }

        public override string GetConnectString()
        {
            string connectString = "";

            string connectFormat = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:System Database={1};User Id={2};Password={3};";
            //connectString = String.Format(connectFormat, m_config.DataFileName, m_config.SystFileName, m_config.UserName, m_config.PlainUsersPsw());
            connectString = String.Format(connectFormat, m_config.DataFileName, m_config.SystFileName, m_config.OwnerName, m_config.PlainOwnerPsw());

            return connectString;
        }

        public override string GetShortConnectString()
        {
            string connectString = "";

            string connectFormat = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:System Database={1};";
            connectString = String.Format(connectFormat, m_config.DataFileName, m_config.SystFileName);

            return connectString;
        }

        public override void CreateDatabase()
        {
        }

        public override DbConnection CreateConnection()
        {
            m_conn = new System.Data.OleDb.OleDbConnection(m_connString);

            return m_conn;
        }

        public override DbDataAdapter GetAdapter(string dataFilter)
        {
            return new System.Data.OleDb.OleDbDataAdapter(dataFilter, m_conn as System.Data.OleDb.OleDbConnection);
        }

        public override DbCommand GetCommand(string commandSql)
        {
            return new System.Data.OleDb.OleDbCommand(commandSql, m_conn as System.Data.OleDb.OleDbConnection);
        }
    }
}
