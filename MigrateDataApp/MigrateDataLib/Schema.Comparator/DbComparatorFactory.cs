using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.Schema.Comparator
{
    public class DbComparatorFactory
    {
        public static IDbComparator CreateComparator(DbsDataConfig config)
        {
            switch (config.PlatformType)
            {
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_MSSQL:
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_IMSSQL:
                    return new DbComparatorMsSql() as IDbComparator;
                case DbsDataConfigKeys.DATA_PROVIDER_JET3:
                    return new DbComparatorMsJet() as IDbComparator;
                case DbsDataConfigKeys.DATA_PROVIDER_SQLITE:
                    return new DbComparatorSqlite() as IDbComparator;
                case DbsDataConfigKeys.DATA_PROVIDER_ODBC_ORACLE:
                    return new DbComparatorOracle() as IDbComparator;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
