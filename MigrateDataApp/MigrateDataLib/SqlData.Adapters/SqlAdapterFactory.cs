using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.SqlData.Adapters
{
    public static class SqlAdapterFactory
    {
        public static ISqlAdapter CreateSqlAdapter(DbsDataConfig config)
        {
            switch (config.PlatformType)
            {
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_MSSQL:
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_IMSSQL:
                    return new SqlMsSqlAdapter(config) as ISqlAdapter;
                case DbsDataConfigKeys.DATA_PROVIDER_JET3:
                    return new SqlMsJetAdapter(config) as ISqlAdapter;
                case DbsDataConfigKeys.DATA_PROVIDER_SQLITE:
                    return new SqlSqliteAdapter(config) as ISqlAdapter;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
