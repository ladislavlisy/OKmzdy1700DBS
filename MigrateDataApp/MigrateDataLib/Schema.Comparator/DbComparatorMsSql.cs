using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.Comparator
{
    public class DbComparatorMsSql : BaseDbComparator
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
            string commandSql = "SELECT  [TABLE_NAME] AS COMP_NAME1, [COLUMN_NAME] AS COMP_NAME2" +
                              ",[COLUMN_NAME] AS COMP_TYPE" +
                              " FROM [INFORMATION_SCHEMA].[COLUMNS] [COLUMNS]" +
                              " WHERE [TABLE_CATALOG] = '{0}' AND [TABLE_SCHEMA] = '{1}'" +
                              " ORDER BY [TABLE_SCHEMA], [TABLE_NAME], [COLUMN_NAME]";
            return commandSql;
        }

        public override string[] GetDiffTableColumnsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffViewsCompareSql()
        {
            string commandSql = "SELECT [TABLE_NAME] AS COMP_NAME1" +
                            ",[TABLE_TYPE] AS COMP_TYPE" +
                            " FROM [INFORMATION_SCHEMA].[TABLES] [TABLES]" +
                            " WHERE [TABLE_TYPE] = 'VIEW' AND [TABLE_CATALOG] = '{0}' AND [TABLE_SCHEMA] = '{1}'" +
                            " ORDER BY [TABLE_NAME]";
            return commandSql;
        }

        public override string[] GetDiffViewsCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }
        public override string GetDiffViewTablesCompareSql()
        {
            string commandSql = commandSql = "SELECT [VIEW_NAME] AS COMP_NAME1, [TABLE_NAME] AS COMP_NAME2" +
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
            string commandSql = "SELECT  [VIEW_NAME] AS COMP_NAME1, [COLUMN_NAME] AS COMP_NAME2" +
                            ",[COLUMN_NAME] AS COMP_TYPE" +
                            " FROM [INFORMATION_SCHEMA].[VIEW_COLUMN_USAGE] [VIEW_COLUMN_USAGE]" +
                            " WHERE [TABLE_CATALOG] = '{0}' AND [TABLE_SCHEMA] = '{1}'" +
                            " ORDER BY [VIEW_SCHEMA],[VIEW_NAME],[COLUMN_NAME]";
            return commandSql;
        }

        public override string[] GetDiffViewColumnsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffTableConstaintsCompareSql()
        {
            string commandSql = "SELECT [TABLE_NAME] AS COMP_NAME1, [CONSTRAINT_NAME] AS COMP_NAME2" +
                          ",[CONSTRAINT_TYPE] AS COMP_TYPE" +
                          " FROM [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS] [TABLE_CONSTRAINTS]" +
                          " WHERE [TABLE_CATALOG] = '{0}' AND [TABLE_SCHEMA] = '{1}'" +
                          " ORDER BY [TABLE_SCHEMA], [TABLE_NAME], [CONSTRAINT_NAME]";
            return commandSql;
        }

        public override string[] GetDiffTableConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffReferConstaintsCompareSql()
        {
            string commandSql = "SELECT [CONSTRAINT_NAME] AS COMP_NAME1" +
                          ",[UNIQUE_CONSTRAINT_NAME] AS COMP_TYPE" +
                          " FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] [REFERENTIAL_CONSTRAINTS]" +
                          " WHERE [CONSTRAINT_CATALOG] = '{0}' AND [CONSTRAINT_SCHEMA] = '{1}'" +
                          " ORDER BY [CONSTRAINT_SCHEMA], [CONSTRAINT_NAME]";
            return commandSql;
        }

        public override string[] GetDiffReferConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }

        public override string GetDiffKeyConstaintsCompareSql()
        {
            string commandSql = "SELECT [TABLE_NAME] AS COMP_NAME1, [COLUMN_NAME] AS COMP_NAME2" +
                          ",[CONSTRAINT_NAME] AS COMP_TYPE" +
                          " FROM [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE] [KEY_COLUMN_USAGE]" +
                          " WHERE [CONSTRAINT_CATALOG] = '{0}' AND [CONSTRAINT_SCHEMA] = '{1}'" +
                          " ORDER BY [CONSTRAINT_SCHEMA], [CONSTRAINT_NAME], [COLUMN_NAME]";
            return commandSql;
        }

        public override string[] GetDiffKeyConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", "COMP_NAME2", "COMP_TYPE" };
        }

        public override string GetDiffCheckConstaintsCompareSql()
        {
            string commandSql = "SELECT [CONSTRAINT_NAME] AS COMP_NAME1" +
                          ",[CHECK_CLAUSE] AS COMP_TYPE" +
                          " FROM [INFORMATION_SCHEMA].[CHECK_CONSTRAINTS] " +
                          " WHERE [CONSTRAINT_CATALOG] = '{0}' AND [CONSTRAINT_SCHEMA] = '{1}'" +
                          " ORDER BY [CONSTRAINT_SCHEMA], [CONSTRAINT_NAME]";
            return commandSql;
        }

        public override string[] GetDiffCheckConstaintsCompareNames()
        {
            return new string[] { "COMP_NAME1", null, "COMP_TYPE" };
        }

        public override string GetTableNamesSelectSql()
        {
            string commandSql = "SELECT [TABLE_NAME] FROM [INFORMATION_SCHEMA].[TABLES] [TABLES]" +
                               " WHERE [TABLE_TYPE] = 'BASE TABLE' AND [TABLE_CATALOG] = '{0}' " +
                               " AND [TABLE_SCHEMA] = '{1}'" +
                               " ORDER BY [TABLE_NAME]";
            return commandSql;
        }

        public override string GetWhereCondition(string op, string compAlias, string tempAlias, string collName, Int32 collType)
        {
            StringBuilder builderCol = new StringBuilder();
            string collPrev = "ISNULL(";
            string collPost = "";
            bool appendPart = true;
            switch (collType)
            {
                case DatabaseDef.DB_BOOLEAN:
                case DatabaseDef.DB_LONG:
                case DatabaseDef.DB_INTEGER:
                case DatabaseDef.DB_BYTE:
                case DatabaseDef.DB_DOUBLE:
                    collPost = ",0)";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    collPrev = "";
                    collPost = "";
                    appendPart = false;
                    break;
                case DatabaseDef.DB_MEMO:
                    collPrev = "";
                    collPost = "";
                    appendPart = false;
                    break;
                case DatabaseDef.DB_TEXT:
                    collPost = ",'NULL')";
                    break;
                case DatabaseDef.DB_DATE:
                    collPost = ",'1/1/2900')";
                    break;
            }
            if (appendPart)
            {
                builderCol.Append(collPrev).Append(compAlias).Append(collName).Append(collPost).Append(op).Append(collPrev).Append(tempAlias).Append(collName).Append(collPost);
            }
            return builderCol.ToString();
        }

        public override string GetWhereDataCondition(string op, string tempAlias, string collName, Int32 collType, string compData)
        {
            StringBuilder builderCol = new StringBuilder();
            string collPrev = "ISNULL(";
            string collPost = "";
            string collData = "";
            bool appendPart = true;
            switch (collType)
            {
                case DatabaseDef.DB_BOOLEAN:
                case DatabaseDef.DB_LONG:
                case DatabaseDef.DB_INTEGER:
                case DatabaseDef.DB_BYTE:
                case DatabaseDef.DB_DOUBLE:
                    collPost = ",0)";
                    collData = compData;
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    collPrev = "";
                    collPost = "";
                    collData = "";
                    appendPart = false;
                    break;
                case DatabaseDef.DB_MEMO:
                    collPrev = "";
                    collPost = "";
                    collData = "";
                    appendPart = false;
                    break;
                case DatabaseDef.DB_TEXT:
                    collPost = ",'NULL')";
                    collData = "'" + compData + "'";
                    break;
                case DatabaseDef.DB_DATE:
                    collPost = ",'1/1/2900')";
                    collData = "'" + compData + "'";
                    break;
            }
            if (appendPart)
            {
                builderCol.Append(collData).Append(op).Append(collPrev).Append(tempAlias).Append(collName).Append(collPost);
            }
            return builderCol.ToString();
        }
        public override string GetCountRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            bool firstColumn = true;

            bool existsPKCol = (columnsPKs.Count != 0);
            bool existsNPCol = (columnsDat.Count != 0);

            StringBuilder builderSql = new StringBuilder("SELECT count(*) AS DIFF_COUNT FROM ");
            StringBuilder builderCol = new StringBuilder();

            builderSql.Append(catalogName1).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderSql.Append(" TC ");
            builderSql.Append(" WHERE NOT EXISTS (SELECT * FROM ").Append(catalogName2).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderSql.Append(" TT WHERE ");

            foreach (var col in columnsPKs)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("=", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        builderSql.Append(colCondition);
                    }
                    else
                    {
                        builderSql.Append(" AND ").Append(colCondition);
                    }
                    firstColumn = false;
                }
            }
            foreach (var col in columnsDat)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("=", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        if (!existsPKCol)
                        {
                            builderSql.Append(colCondition);
                        }
                    }
                    else
                    {
                        if (!existsPKCol)
                        {
                            builderSql.Append(" AND ").Append(colCondition);
                        }
                    }
                    firstColumn = false;
                }
            }
            builderSql.Append(" )");
            if (!existsPKCol && !existsNPCol)
            {
                return "";
            }
            return builderSql.ToString();
        }
        public override string GetCountRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            bool firstColumn = true;

            bool existsPKCol = (columnsPKs.Count != 0);
            bool existsNPCol = (columnsDat.Count != 0);

            if (!existsNPCol)
            {
                return "";
            }

            StringBuilder builderSql = new StringBuilder("SELECT count(*) AS DIFF_COUNT FROM ");
            StringBuilder builderCol = new StringBuilder();

            builderSql.Append(catalogName1).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderSql.Append(" TC ");
            builderSql.Append(" WHERE EXISTS (SELECT * FROM ").Append(catalogName2).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderSql.Append(" TT WHERE ");

            foreach (var col in columnsPKs)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("=", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        builderSql.Append(colCondition);
                    }
                    else
                    {
                        builderSql.Append(" AND ").Append(colCondition);
                    }
                    firstColumn = false;
                }
            }
            if (firstColumn)
            {
                builderSql.Append(" (");
            }
            else
            {
                builderSql.Append(" AND (");
            }
            firstColumn = true;
            foreach (var col in columnsDat)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("<>", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        builderSql.Append(colCondition);
                    }
                    else
                    {
                        builderSql.Append(" OR ").Append(colCondition);
                    }
                    firstColumn = false;
                }
            }
            builderSql.Append(" ) )");
            if (!existsPKCol)
            {
                return "";
            }
            return builderSql.ToString();
        }

        public override string GetTableRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            bool firstColumn = true;

            bool existsPKCol = (columnsPKs.Count != 0);
            bool existsNPCol = (columnsDat.Count != 0);

            StringBuilder builderSql = new StringBuilder("SELECT  ");
            StringBuilder builderQry = new StringBuilder(" WHERE NOT EXISTS (SELECT * FROM ");
            StringBuilder builderCol = new StringBuilder();

            builderQry.Append(catalogName2).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderQry.Append(" TT WHERE ");

            foreach (var col in columnsPKs)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("=", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        builderQry.Append(colCondition);
                        builderCol.Append("TC.").Append(col);
                    }
                    else
                    {
                        builderQry.Append(" AND ").Append(colCondition);
                        builderCol.Append(",").Append("TC.").Append(col);
                    }
                    firstColumn = false;
                }
            }
            foreach (var col in columnsDat)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("=", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        if (!existsPKCol)
                        {
                            builderQry.Append(colCondition);
                        }
                        builderCol.Append("TC.").Append(col);
                    }
                    else
                    {
                        if (!existsPKCol)
                        {
                            builderQry.Append(" AND ").Append(colCondition);
                        }
                        builderCol.Append(",").Append("TC.").Append(col);
                    }
                    firstColumn = false;
                }
            }
            builderQry.Append(" )");

            if (!existsPKCol && !existsNPCol)
            {
                return "";
            }

            builderSql.Append(builderCol);
            builderSql.Append(" FROM ");
            builderSql.Append(catalogName1).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderSql.Append(" TC ");
            builderSql.Append(builderQry);

            return builderSql.ToString();
        }

        public override string GetTableRowsPKDataExist(string catalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes, IList<Tuple<string, string>> dataCollPKs)
        {
            bool firstColumn = true;

            bool existsPKCol = (columnsPKs.Count != 0);
            bool existsNPCol = (columnsDat.Count != 0);

            StringBuilder builderSql = new StringBuilder("SELECT  ");
            StringBuilder builderQry = new StringBuilder(" WHERE ");
            StringBuilder builderCol = new StringBuilder();

            foreach (var col in columnsPKs)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                var colData = dataCollPKs.First((c) => (c.Item1 == col));
                string colCondition = GetWhereDataCondition("=", "TT.", col, colType.Item2, colData.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        builderQry.Append(colCondition);
                        builderCol.Append("TT.").Append(col);
                    }
                    else
                    {
                        builderQry.Append(" AND ").Append(colCondition);
                        builderCol.Append(",").Append("TT.").Append(col);
                    }
                    firstColumn = false;
                }
            }
            foreach (var col in columnsDat)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                if (colType.Item2 != DatabaseDef.DB_LONGBINARY && colType.Item2 != DatabaseDef.DB_MEMO)
                {
                    if (firstColumn)
                    {
                        builderCol.Append("TT.").Append(col);
                    }
                    else
                    {
                        builderCol.Append(",").Append("TT.").Append(col);
                    }
                    firstColumn = false;
                }
            }

            if (!existsPKCol)
            {
                return "";
            }

            builderSql.Append(builderCol);
            builderSql.Append(" FROM ");
            builderSql.Append(catalogName).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderSql.Append(" TT ");
            builderSql.Append(builderQry);

            return builderSql.ToString();
        }

        public override string GetTableRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            bool firstColumn = true;
            bool firstColQry = true;

            bool existsPKCol = (columnsPKs.Count != 0);
            bool existsNPCol = (columnsDat.Count != 0);

            if (!existsNPCol)
            {
                return "";
            }

            StringBuilder builderSql = new StringBuilder("SELECT  ");
            StringBuilder builderQry = new StringBuilder(" WHERE EXISTS (SELECT * FROM ");
            StringBuilder builderCol = new StringBuilder();

            builderQry.Append(catalogName2).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderQry.Append(" TT WHERE ");

            foreach (var col in columnsPKs)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("=", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColumn)
                    {
                        builderQry.Append(colCondition);
                        builderCol.Append("TC.").Append(col);
                    }
                    else
                    {
                        builderQry.Append(" AND ").Append(colCondition);
                        builderCol.Append(",").Append("TC.").Append(col);
                    }
                    firstColumn = false;
                }
            }
            if (firstColumn)
            {
                builderQry.Append(" (");
            }
            else
            {
                builderQry.Append(" AND (");
            }
            firstColQry = true;
            foreach (var col in columnsDat)
            {
                var colType = columnTypes.First((c) => (c.Item1 == col));
                string colCondition = GetWhereCondition("<>", "TC.", "TT.", col, colType.Item2);
                if (colCondition.Length > 0)
                {
                    if (firstColQry)
                    {
                        builderQry.Append(colCondition);
                    }
                    else
                    {
                        builderQry.Append(" OR ").Append(colCondition);
                    }
                    firstColQry = false;
                    if (firstColumn)
                    {
                        builderCol.Append("TC.").Append(col);
                    }
                    else
                    {
                        builderCol.Append(",").Append("TC.").Append(col);
                    }
                    firstColumn = false;
                }
            }
            builderQry.Append(" ) )");

            if (!existsPKCol)
            {
                return "";
            }

            builderSql.Append(builderCol);
            builderSql.Append(" FROM ");
            builderSql.Append(catalogName1).Append(".").Append(schemaName).Append(".").Append(tableName);
            builderSql.Append(" TC ");
            builderSql.Append(builderQry);

            return builderSql.ToString();
        }

    }
}