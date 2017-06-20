using System;
using System.Collections.Generic;

namespace MigrateDataLib.Schema.Comparator
{
    internal class DbComparatorOracle : BaseDbComparator
    {
        public override string GetDiffTablesCompareSql()
        {
            string commandSql = "SELECT [TABLE_NAME] AS COMP_NAME1" +
                          ",[TABLE_TYPE] AS COMP_TYPE" +
                          " FROM [INFORMATION_SCHEMA].[TABLES] [TABLES]" +
                          " WHERE [TABLE_TYPE] = 'BASE TABLE' AND [TABLE_CATALOG] = '{0}' AND [TABLE_SCHEMA] = '{1}'" +
                          " ORDER BY [TABLE_NAME]";
            return commandSql;
        }
        public override string[] GetDiffTablesCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }

        public override string GetDiffTableColumnsCompareSql()
        {
            string commandSql = "SELECT COMP_NAME1, COMP_NAME2, COMP_TYPE FROM " +
                          " (SELECT C.OWNER AS OWNER, C.TABLE_NAME AS COMP_NAME1, C.COLUMN_NAME AS COMP_NAME2, C.COLUMN_NAME AS COMP_TYPE" +
                          " FROM SYS.ALL_TAB_COLUMNS C INNER JOIN ALL_TABLES T ON T.OWNER = C.OWNER AND T.TABLE_NAME = C.TABLE_NAME) COLUMNS" +
                          " WHERE OWNER = '{2}'" +
                          " ORDER BY COMP_NAME1, COMP_NAME2, COMP_TYPE";
            return commandSql;
        }

        public override string[] GetDiffTableColumnsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffViewsCompareSql()
        {
            string commandSql = "SELECT VIEW_NAME AS COMP_NAME1" +
                            ",'VIEW' AS COMP_TYPE" +
                            " FROM ALL_VIEWS TABLES" +
                            " WHERE OWNER = '{2}'" +
                            " ORDER BY VIEW_NAME";
            return commandSql;
        }

        public override string[] GetDiffViewsCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }
        public override string GetDiffViewTablesCompareSql()
        {
            string commandSql = "SELECT [VIEW_NAME] AS COMP_NAME1, [TABLE_NAME] AS COMP_NAME2" +
                            ",[TABLE_NAME] AS COMP_TYPE" +
                            " FROM [INFORMATION_SCHEMA].[VIEW_TABLE_USAGE] " +
                            " WHERE [TABLE_CATALOG] = '{0}' AND [TABLE_SCHEMA] = '{1}'" +
                            " ORDER BY [VIEW_SCHEMA],[VIEW_NAME],[TABLE_NAME]";
            return commandSql;
        }

        public override string[] GetDiffViewTablesCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffViewColumnsCompareSql()
        {
            string commandSql = "SELECT COMP_NAME1, COMP_NAME2, COMP_TYPE FROM " +
                            " (SELECT C.OWNER AS OWNER, C.TABLE_NAME AS COMP_NAME1, C.COLUMN_NAME AS COMP_NAME2, C.COLUMN_NAME AS COMP_TYPE" +
                            " FROM SYS.ALL_TAB_COLUMNS C INNER JOIN ALL_VIEWS W ON W.OWNER = C.OWNER AND W.VIEW_NAME = C.TABLE_NAME) VIEW_COLUMN_USAGE" +
                            " WHERE OWNER = '{2}'" +
                            " ORDER BY COMP_NAME1, COMP_NAME2, COMP_TYPE";
            return commandSql;
        }

        public override string[] GetDiffViewColumnsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }
        public override string GetDiffTableConstaintsCompareSql()
        {
            string commandSql = "SELECT TABLE_NAME AS COMP_NAME1, CONSTRAINT_NAME AS COMP_NAME2" +
                          ",CONSTRAINT_TYPE AS COMP_TYPE" +
                          " FROM ALL_CONSTRAINTS TABLE_CONSTRAINTS" +
                          " WHERE OWNER = '{2}' AND (CONSTRAINT_TYPE = 'P' OR CONSTRAINT_TYPE = 'R')" +
                          " ORDER BY OWNER, TABLE_NAME, CONSTRAINT_NAME";
            return commandSql;
        }

        public override string[] GetDiffTableConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffReferConstaintsCompareSql()
        {
            string commandSql = "SELECT CONSTRAINT_NAME AS COMP_NAME1" +
                          ",R_CONSTRAINT_NAME AS COMP_TYPE" +
                          " FROM ALL_CONSTRAINTS REFERENTIAL_CONSTRAINTS" +
                          " WHERE OWNER = '{2}' AND CONSTRAINT_TYPE = 'R'" +
                          " ORDER BY OWNER, CONSTRAINT_NAME";
            return commandSql;
        }

        public override string[] GetDiffReferConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }

        public override string GetDiffKeyConstaintsCompareSql()
        {
            string commandSql = "SELECT TABLE_NAME AS COMP_NAME1, COLUMN_NAME AS COMP_NAME2" +
                          ",CONSTRAINT_NAME AS COMP_TYPE" +
                          " FROM ALL_CONS_COLUMNS KEY_COLUMN_USAGE" +
                          " WHERE OWNER = '{2}'" +
                          " ORDER BY OWNER, CONSTRAINT_NAME";
            return commandSql;
        }

        public override string[] GetDiffKeyConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffCheckConstaintsCompareSql()
        {
            string commandSql = "SELECT CONSTRAINT_NAME AS COMP_NAME1" +
                          ",TABLE_NAME AS COMP_TYPE" +
                          " FROM ALL_CONSTRAINTS CHECK_CONSTRAINTS" +
                          " WHERE OWNER = '{2}' AND CONSTRAINT_TYPE = 'C'" +
                          " ORDER BY OWNER, CONSTRAINT_NAME";
            return commandSql;
        }

        public override string[] GetDiffCheckConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }

        public override string GetTableNamesSelectSql()
        {
            string commandSql = "SELECT TABLE_NAME FROM ALL_TABLES TABLES" +
                               " WHERE OWNER = '{2}'" +
                               " ORDER BY TABLE_NAME";
            return commandSql;
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