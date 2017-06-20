using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.Comparator
{
    public interface IDbComparator
    {
        string GetDiffTablesCompareSql();
        string[] GetDiffTablesCompareNames();
        string GetDiffTableColumnsCompareSql();
        string[] GetDiffTableColumnsCompareNames();
        string GetDiffViewsCompareSql();
        string[] GetDiffViewsCompareNames();
        string GetDiffViewTablesCompareSql();
        string[] GetDiffViewTablesCompareNames();
        string GetDiffViewColumnsCompareSql();
        string[] GetDiffViewColumnsCompareNames();
        string GetDiffTableConstaintsCompareSql();
        string[] GetDiffTableConstaintsCompareNames();
        string GetDiffReferConstaintsCompareSql();
        string[] GetDiffReferConstaintsCompareNames();
        string GetDiffKeyConstaintsCompareSql();
        string[] GetDiffKeyConstaintsCompareNames();
        string GetDiffCheckConstaintsCompareSql();
        string[] GetDiffCheckConstaintsCompareNames();
        string GetTableNamesSelectSql();
        string GetTableRowsCountSql(string tableName);
        string GetWhereCondition(string op, string compAlias, string tempAlias, string collName, Int32 collType);
        string GetWhereDataCondition(string op, string tempAlias, string collName, Int32 collType, string compData);

        string GetCountRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
        string GetCountRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
        string GetTableRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
        string GetTableRowsPKDataExist(string catalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes, IList<Tuple<string, string>> dataCollPKs);
        string GetTableRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
    }
}
