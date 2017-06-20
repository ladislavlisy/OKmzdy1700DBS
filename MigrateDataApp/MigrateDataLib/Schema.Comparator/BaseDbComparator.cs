using MigrateDataLib.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.Comparator
{
    public abstract class BaseDbComparator : IDbComparator
    {
        public abstract string GetDiffTablesCompareSql();
        public abstract string[] GetDiffTablesCompareNames();

        public abstract string GetDiffTableColumnsCompareSql();
        public abstract string[] GetDiffTableColumnsCompareNames();

        public abstract string GetDiffViewsCompareSql();
        public abstract string[] GetDiffViewsCompareNames();

        public abstract string GetDiffViewTablesCompareSql();
        public abstract string[] GetDiffViewTablesCompareNames();

        public abstract string GetDiffViewColumnsCompareSql();
        public abstract string[] GetDiffViewColumnsCompareNames();

        public abstract string GetDiffTableConstaintsCompareSql();
        public abstract string[] GetDiffTableConstaintsCompareNames();

        public abstract string GetDiffReferConstaintsCompareSql();
        public abstract string[] GetDiffReferConstaintsCompareNames();

        public abstract string GetDiffKeyConstaintsCompareSql();
        public abstract string[] GetDiffKeyConstaintsCompareNames();

        public abstract string GetDiffCheckConstaintsCompareSql();
        public abstract string[] GetDiffCheckConstaintsCompareNames();

        public abstract string GetTableNamesSelectSql();

        public virtual string GetTableRowsCountSql(string tableName)
        {
            StringBuilder builderSql = new StringBuilder("SELECT count(*) AS ROWS_COUNT FROM ");

            builderSql.Append(tableName);

            return builderSql.ToString();
        }
        public virtual string GetWhereCondition(string op, string compAlias, string tempAlias, string collName, Int32 collType)
        {
            StringBuilder builderCol = new StringBuilder();
            string collPrev = "";
            string collPost = "";
            bool appendPart = true;
            switch (collType)
            {
                case DatabaseDef.DB_BOOLEAN:
                case DatabaseDef.DB_LONG:
                case DatabaseDef.DB_INTEGER:
                case DatabaseDef.DB_BYTE:
                case DatabaseDef.DB_DOUBLE:
                    collPost = "";
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
                    collPost = "";
                    break;
                case DatabaseDef.DB_DATE:
                    collPost = "";
                    break;
            }
            if (appendPart)
            {
                builderCol.Append(collPrev).Append(compAlias).Append(collName).Append(collPost).Append(op).Append(collPrev).Append(tempAlias).Append(collName).Append(collPost);
            }
            return builderCol.ToString();
        }

        public virtual string GetWhereDataCondition(string op, string tempAlias, string collName, Int32 collType, string compData)
        {
            StringBuilder builderCol = new StringBuilder();
            string collPrev = "";
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
                    collPost = "";
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
                    collPost = "";
                    collData = "'" + compData + "'";
                    break;
                case DatabaseDef.DB_DATE:
                    collPost = "";
                    collData = "'" + compData + "'";
                    break;
            }
            if (appendPart)
            {
                builderCol.Append(collData).Append(op).Append(collPrev).Append(tempAlias).Append(collName).Append(collPost);
            }
            return builderCol.ToString();
        }
        public abstract string GetCountRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
        public abstract string GetCountRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
        public abstract string GetTableRowsPKNonExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
        public abstract string GetTableRowsPKDataExist(string catalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes, IList<Tuple<string, string>> dataCollPKs);
        public abstract string GetTableRowsPKExists(string catalogName1, string catalogName2, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes);
    }
}
