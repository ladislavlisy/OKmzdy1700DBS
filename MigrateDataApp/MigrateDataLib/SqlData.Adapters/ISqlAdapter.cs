using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.DefCopyItems;

namespace MigrateDataLib.SqlData.Adapters
{
    public interface ISqlAdapter : IDisposable
    {
        DbsDataConfig Config();

        string GetConnectString();
        string GetShortConnectString();

        UInt32 PlatformType();
        bool IsMsSQLType();
        string DatabaseName();
        string DatabaseOwnr();
        string DatabaseUser();

        DbConnection DbConnection();

        void OpenConnection();

        void CloseConnection();

        void CreateDatabase();
        DbConnection CreateConnection();
        DbDataAdapter GetAdapter(string dataFilter);
        DbCommand GetCommand(string commandSql);
        Int32 SelectRowCountTableData(TableDefInfo tableInfo);
        Int32 SelectRowCountTableData(string tableFrom, string tableName);
        Int32 SelectDiffCount(string diffCommand);
    }
}
