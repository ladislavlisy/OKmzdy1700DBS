using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.Schema.Builder
{
    public class SqlBuilderFactory
    {
        public static ISqlScriptBuilder CreateSqlBuilder(DbsDataConfig config)
        {
            switch (config.PlatformType)
            {
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_MSSQL:
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_IMSSQL:
                    return new ScriptBuilderMsSql(config) as ISqlScriptBuilder;
                case DbsDataConfigKeys.DATA_PROVIDER_JET3:
                    return new ScriptBuilderMsJet(config) as ISqlScriptBuilder;
                case DbsDataConfigKeys.DATA_PROVIDER_SQLITE:
                    return new ScriptBuilderSqlite(config) as ISqlScriptBuilder;
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_ORACLE:
                    return new ScriptBuilderOracle(config) as ISqlScriptBuilder;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

