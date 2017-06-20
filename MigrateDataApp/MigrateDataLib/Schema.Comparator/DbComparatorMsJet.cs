using System;
using System.Collections.Generic;
using System.Text;

namespace MigrateDataLib.Schema.Comparator
{
    public class DbComparatorMsJet : BaseDbComparator
    {
        public override string GetDiffTablesCompareSql()
        {
            string commandSql = "SELECT [Name] AS COMP_NAME1" +
                            ",[Type] AS COMP_TYPE" +
                            " FROM [MSysObjects] [TABLES]" +
                            " WHERE [Type] = 1 AND Left([Name],1) <> \"~\"" +
                            " ORDER BY [Name]";
            return commandSql;
        }
        public override string[] GetDiffTablesCompareNames()
        {
            return new string[] { "TABLE_NAME", null, "TABLE_TYPE" };
        }

        public override string GetDiffTableColumnsCompareSql()
        {
            string commandSql = "";
            return commandSql;
        }

        public override string[] GetDiffTableColumnsCompareNames()
        {
            return new string[] { "TABLE_NAME", "COLUMN_NAME", "COLUMN_NAME" };
        }

        public override string GetDiffViewsCompareSql()
        {
            string commandSql = "SELECT [Name] AS COMP_NAME1" +
                            ",[Type] AS COMP_TYPE" +
                            " FROM [MSysObjects] [TABLES]" +
                            " WHERE [Type] = 5 AND Left([Name],1) <> \"~\"" +
                            " ORDER BY [Name]";
            return commandSql;
        }

        public override string[] GetDiffViewsCompareNames()
        {
            return new string[] { "TABLE_NAME", null, "TABLE_TYPE" };
        }

        public override string GetDiffViewTablesCompareSql()
        {
            string commandSql = "";
            return commandSql;
        }

        public override string[] GetDiffViewTablesCompareNames()
        {
            return new string[] { "VIEW_NAME", "TABLE_NAME", "TABLE_NAME" };
        }

        public override string GetDiffViewColumnsCompareSql()
        {
            string commandSql = "";
            return commandSql;
        }

        public override string[] GetDiffViewColumnsCompareNames()
        {
            return new string[] { "VIEW_NAME", "COLUMN_NAME", "COLUMN_NAME" };
        }

        public override string GetDiffTableConstaintsCompareSql()
        {
            string commandSql = "SELECT COMP_NAME1, COMP_NAME2, COMP_TYPE " +
                          " FROM (SELECT [Constraints].[Name] AS COMP_NAME1, [ConstrTables].[Name] AS COMP_NAME2, [Constraints].[Name] AS COMP_TYPE" +
                          " FROM [MSysObjects] [Constraints] INNER JOIN [MSysObjects] [ConstrTables] on [ConstrTables].[Id] = [Constraints].[ParentId]" +
                          " WHERE [Constraints].[Type] = 8) TABLE_CONSTRAINTS " +
                          " ORDER BY COMP_NAME1, COMP_NAME2, COMP_TYPE";
            return commandSql;
        }

        public override string[] GetDiffTableConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
            //return new string[] { "TABLE_NAME", "CONSTRAINT_NAME", "CONSTRAINT_TYPE" };
        }

        public override string GetDiffReferConstaintsCompareSql()
        {
            string commandSql = "";
            return commandSql;
        }

        public override string[] GetDiffReferConstaintsCompareNames()
        {
            //return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
            return new string[] { "CONSTRAINT_NAME", null, "UNIQUE_CONSTRAINT_NAME" };
        }

        public override string GetDiffKeyConstaintsCompareSql()
        {
            string commandSql = "SELECT [szObject] AS COMP_NAME1, [szColumn] AS COMP_NAME2, [szRelationship] AS COMP_TYPE" +
                          " FROM [MSysRelationships] [KEY_COLUMN_USAGE]" +
                          " ORDER BY [szRelationship], [szColumn]";
            return commandSql;
        }

        public override string[] GetDiffKeyConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
            //return new string[] { "TABLE_NAME", "COLUMN_NAME", "CONSTRAINT_NAME" };
        }

        public override string GetDiffCheckConstaintsCompareSql()
        {
            string commandSql = "";
            return commandSql;
        }

        public override string[] GetDiffCheckConstaintsCompareNames()
        {
            //return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
            return new string[] { "CONSTRAINT_NAME", null, "CHECK_CLAUSE" };
        }
        public override string GetTableNamesSelectSql()
        {
            string commandSql = "SELECT [Name] AS [TABLE_NAME] FROM [MSysObjects] [TABLES]" +
                              " WHERE [Type] = 1 AND Left([Name],1) <> \"~\"" +
                              " AND [Name] NOT LIKE 'MSys%'" +
                              " ORDER BY [Name]";
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