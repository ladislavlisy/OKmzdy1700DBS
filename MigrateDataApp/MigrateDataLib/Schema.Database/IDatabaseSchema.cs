using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.Schema.Database
{
    public interface IDatabaseSchema
    {
        IList<TableDefInfo> CreateFilteredTableList(IList<string> filterList);
        IList<TableDefInfo> CreateFilteredTableCloneList(IList<string> filterList);
        IList<TableDefInfo> CreateSubsetTableList(IList<string> filterList);
        IList<TableDefInfo> CreateSubsetTableCloneList(IList<string> filterList);
        IList<QueryDefInfo> CreateFilteredQueryList(IList<string> filterList);
        IList<QueryDefInfo> CreateFilteredQueryCloneList(IList<string> filterList);
        IList<QueryDefInfo> CreateSubsetQueryList(IList<string> filterList);
        IList<QueryDefInfo> CreateSubsetQueryCloneList(IList<string> filterList);
        IDictionary<string, TableDefInfo> CreateTableDictionary();
        IDictionary<string, QueryDefInfo> CreateQueryDictionary();
    }
}
