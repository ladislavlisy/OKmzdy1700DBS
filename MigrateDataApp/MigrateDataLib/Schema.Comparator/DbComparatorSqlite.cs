using System;
using System.Collections.Generic;

namespace MigrateDataLib.Schema.Comparator
{
    public class DbComparatorSqlite : BaseDbComparator
    {
        public override string GetDiffTablesCompareSql()
        {
            throw new NotImplementedException();
        }
        public override string[] GetDiffTablesCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }

        public override string GetDiffTableColumnsCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffTableColumnsCompareNames()
        {
            throw new NotImplementedException();
        }

        public override string GetDiffViewsCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffViewsCompareNames()
        {
            throw new NotImplementedException();
        }
        public override string GetDiffViewTablesCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffViewTablesCompareNames()
        {
            throw new NotImplementedException();
        }

        public override string GetDiffViewColumnsCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffViewColumnsCompareNames()
        {
            throw new NotImplementedException();
        }

        public override string GetDiffTableConstaintsCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffTableConstaintsCompareNames()
        {
            throw new NotImplementedException();
        }

        public override string GetDiffReferConstaintsCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffReferConstaintsCompareNames()
        {
            throw new NotImplementedException();
        }

        public override string GetDiffKeyConstaintsCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffKeyConstaintsCompareNames()
        {
            throw new NotImplementedException();
        }

        public override string GetDiffCheckConstaintsCompareSql()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDiffCheckConstaintsCompareNames()
        {
            throw new NotImplementedException();
        }

        public override string GetTableNamesSelectSql()
        {
            throw new NotImplementedException();
        }
        public override string GetCountRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string commandSql = "";
            return commandSql;
        }
        public override string GetCountRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string commandSql = "";
            return commandSql;
        }
        public override string GetTableRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string commandSql = "";
            return commandSql;
        }
        public override string GetTableRowsPKDataExist(string catalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes, IList<Tuple<string, string>> dataCollPKs)
        {
            string commandSql = "";
            return commandSql;
        }
        public override string GetTableRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string commandSql = "";
            return commandSql;
        }
    }
}