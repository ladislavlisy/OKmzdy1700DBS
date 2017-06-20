using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Generator;
using MigrateDataLib.Constants;
using MigrateDataLib.Utils;
using MigrateDataLib.Schema.DefCopyItems;

namespace MigrateDataLib.Schema.Builder
{
    public abstract class ScriptBuilderBase : ISqlScriptBuilder
    {
        protected const string NEW_LINE_STR = DatabaseDef.NEW_LINE_STR;

        protected DbsDataConfig _config;

        protected UInt32 PlatformType { get; set; }
        public ScriptBuilderBase(DbsDataConfig config)
        {
            this._config = config;

            this.PlatformType = _config.PlatformType;
        }

        public abstract string CreateDefaultSQL(UInt32 versCreate);
        public string CreateTableSQL(TableDefInfo tableInfo, bool createRels, UInt32 versCreate)
        {
            if (tableInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string strFieldNames = TableColsDefinition(tableInfo, createRels, versCreate);

            StringBuilder strSQL = new StringBuilder("CREATE TABLE ");

            strSQL.Append(tableInfo.TableName())
                .Append(" ")
                .Append(strFieldNames);

            return strSQL.ToString();
        }
        public virtual string CreateTableSEQ(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public virtual string CreateSequeSYN(TableDefInfo tableInfo, IGeneratorWriter processWriter)
        {
            if (DatabaseSynonym() == false)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string countsSequeSYN = QuerySynonymCount(tableInfo.TableSeqName());

            long nSeqSynonymCount = processWriter.GetScriptCount(countsSequeSYN);

            bool bSeqSynonymFound = (nSeqSynonymCount == 1);

            if (bSeqSynonymFound)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string createSequeSYN = CreateSequenceSynonym(tableInfo);

            return createSequeSYN;
        }
        public virtual string CreateTableSYN(TableDefInfo tableInfo, IGeneratorWriter processWriter)
        {
            if (DatabaseSynonym() == false)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string countsTableSYN = QuerySynonymCount(tableInfo.TableName());

            long nTabSynonymCount = processWriter.GetScriptCount(countsTableSYN);

            bool bTabSynonymFound = (nTabSynonymCount == 1);

            if (bTabSynonymFound)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string createTableSYN = CreateTableSynonym(tableInfo);

            return createTableSYN;
        }

        public abstract string CreateSequeSEC(TableDefInfo tableInfo);
        public abstract string CreateTableSEC(TableDefInfo tableInfo);
        public abstract string CreateTableBND(TableDefInfo tableInfo, UInt32 versCreate);
        public string CreateSelectCommand(TableDefInfo tableInfo, UInt32 versCreate)
        {
            string sqlCommand = "SELECT ";

            sqlCommand += tableInfo.CreateColumnList(versCreate);

            sqlCommand += " FROM ";
            sqlCommand += tableInfo.TableName();

            return sqlCommand;
        }
        public string CreateTableFromPart(TableDefPipe tableDupl, UInt32 versCreate)
        {
            string strConstraintQuote = TableConstraintDefinition(tableDupl, "\nWHERE ");
            string strEndsClauseQuote = TableEndClausesDefinition(tableDupl, "\n ");

            StringBuilder strBegin = new StringBuilder("FROM ");

            strBegin.Append(strConstraintQuote);

            string sqlCommand = StringUtils.JoinNonEmpty(NEW_LINE_STR, strBegin.ToString(), strEndsClauseQuote);
            sqlCommand += NEW_LINE_STR;

            return sqlCommand;
        }
        public string CreateSelectCommand(TableDefPipe tableInfo, UInt32 versCreate)
        {
            string strTableFieldNames = TableColsAliasDefinition(tableInfo);
            string strConstraintQuote = TableConstraintDefinition(tableInfo, "\nWHERE ");
            string strEndsClauseQuote = TableEndClausesDefinition(tableInfo, "\n ");

            StringBuilder strBegin = new StringBuilder("SELECT ");

            strBegin.Append(strTableFieldNames)
                .Append(NEW_LINE_STR)
                .Append("FROM ")
                .Append(strConstraintQuote);

            string sqlCommand = StringUtils.JoinNonEmpty(NEW_LINE_STR, strBegin.ToString(), strEndsClauseQuote);
            sqlCommand += NEW_LINE_STR;

            return sqlCommand;
        }
        public abstract string QuerySynonymCount(string synonymTarget);
        public abstract string CreateSequenceSynonym(TableDefInfo tableDef);
        public abstract string CreateTableSynonym(TableDefInfo tableDef);
        public abstract string CreateSwitchIndentityInsertOn(TableDefInfo tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);
        public abstract string CreateSwitchIndentityInsertOff(TableDefInfo tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);
        public abstract string CreateSwitchIndentityInsertOn(TableDefPipe tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);
        public abstract string CreateSwitchIndentityInsertOff(TableDefPipe tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);

        private string TableColsDefinition(TableDefInfo tableInfo, bool createRels, UInt32 versCreate)
        {
            if (tableInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            StringBuilder strFieldNames = new StringBuilder("(");

            foreach (var field in tableInfo.TableColumnsForVersion(versCreate))
            {
                IList<string> columnNames = field.AllColumnNames();

                foreach (string columnName in columnNames)
                {
                    StringBuilder strFieldName = new StringBuilder(columnName);

                    strFieldName.Append(" ")
                        .Append(DbConvertDataType(field))
                        .Append(" ")
                        .Append(DbIdentity(field))
                        .Append(DbNullAndDefault(field));

                    strFieldNames.Append(NEW_LINE_STR)
                        .Append(strFieldName.ToString().Trim())
                        .Append(",");

                }
            }

            string retFieldSql = strFieldNames.ToString();

            if (createRels && DbRelationInCreateTable())
            {
                retFieldSql = StringUtils.JoinNonEmpty(NEW_LINE_STR, retFieldSql, DbRelsDefinition(tableInfo));
            }
            if (DbXPIndexInCreateTable())
            {
                retFieldSql = StringUtils.JoinNonEmpty(NEW_LINE_STR, retFieldSql, DbPKsDefinition(tableInfo));
            }

            string retTableSql = retFieldSql.TrimEnd(DatabaseDef.TRIM_CHARS);
            retTableSql += ")";
            retTableSql += NEW_LINE_STR;
            return retTableSql;
        }

        protected string DbIdentity(TableFieldInfo fieldInfo)
        {
            bool identityColumn = DBPlatform.AutoIncrField(fieldInfo.Attributes);
            string strDbIdentity = DatabaseDef.EMPTY_STRING;
            if (identityColumn)
            {
                strDbIdentity = DbIdentitySQL();
            }
            return strDbIdentity;

        }
        protected string DbNullAndDefault(TableFieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            bool bColumnDefault = DbColumnDefault();
            bool bFieldsDefault = fieldInfo.IsDefaultValue();

            bool bRequiredDefault = fieldInfo.IsBindColumnInfo();
            bool bAutoIncremField = fieldInfo.IsAutoIncrement();

            string strFieldNames = DatabaseDef.EMPTY_STRING;

            if (bColumnDefault)
            {
                if (fieldInfo.Required && bRequiredDefault)
                {
                    strFieldNames = StringUtils.JoinNonEmpty(" ", strFieldNames, DbTypeDefault(fieldInfo.ColumnType));
                }
            }
            if (bAutoIncremField == false)
            {
                if (fieldInfo.Required)
                {
                    strFieldNames = StringUtils.JoinNonEmpty(" ", strFieldNames, "NOT NULL");
                }
                else
                {
                    strFieldNames = StringUtils.JoinNonEmpty(" ", strFieldNames, "NULL");
                }
            }
            return strFieldNames;
        }
        protected string GDateDefault()
        {
            return DBPlatform.GDateDefault(PlatformType);
        }

        protected string NumberDefault()
        {
            return DBPlatform.NumberDefault(PlatformType);
        }

        protected string DbTypeDefault(int nType)
        {
            if (DBPlatform.TypeIsNumber(nType))
            {
                return NumberDefault();
            }
            else if (DBPlatform.TypeIsDate(nType))
            {
                return GDateDefault();
            }
            return DatabaseDef.EMPTY_STRING;
        }

        private string DbPKsDefinition(TableDefInfo tableInfo)
        {
            if (tableInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string strFieldNames = CreateConstraintSQL(tableInfo.IndexPK(), true, true);

            return strFieldNames;
        }

        private string DbAKsDefinition(TableDefInfo tableInfo)
        {
            if (tableInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string strFieldNames = CreateConstraintSQL(tableInfo.IndexAK(), true, true);

            return strFieldNames;
        }

        private string CreateConstraintSQL(IndexDefInfo indexInfo, bool addComma, bool addNewLine)
        {
            if (indexInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string sqlCommand = DatabaseDef.EMPTY_STRING;

            string sqlFieldsList = CreateConstraintFiledList(indexInfo.IndexFields(), indexInfo.Primary);

            sqlCommand = PlatformCreateContraintSQL(sqlFieldsList, indexInfo.IndexName, indexInfo.Primary);

            if (sqlCommand != DatabaseDef.EMPTY_STRING)
            {
                if (addComma)
                {
                    sqlCommand += ",";
                }
                if (addNewLine)
                {
                    sqlCommand += DatabaseDef.NEW_LINE_STR;
                }
            }
            return sqlCommand;
        }
        protected virtual string PlatformCreateTableRelationSQL(RelationDefInfo relInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        protected virtual string PlatformCreateContraintSQL(string fieldsList, string indexName, bool indexPrimary)
        {
            string strSQL = DatabaseDef.EMPTY_STRING;
            if (indexPrimary)
            {
                strSQL = ("CONSTRAINT ");
                strSQL += indexName;
                strSQL += (" PRIMARY KEY (");
                strSQL += fieldsList;
                strSQL += (")");
            }
            return strSQL;
        }

        private string CreateConstraintFiledList(IList<IndexFieldInfo> indexFields, bool indexPrimary)
        {
            string strNames = "";

            foreach (var field in indexFields)
            {
                strNames += field.FieldInfo(indexPrimary);
                strNames += (",");
            }
            return strNames.TrimEnd(DatabaseDef.TRIM_CHARS);
        }

        private string DbRelsDefinition(TableDefInfo tableInfo)
        {
            if (tableInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string strFieldNames = DatabaseDef.EMPTY_STRING;

            foreach (var item in tableInfo.Relations())
            {
                strFieldNames += CreateTableRelationSQL(item, true, true);
            }
            return strFieldNames;
        }

        private string CreateTableRelationSQL(RelationDefInfo relInfo, bool addComma, bool addNewLine)
        {
            if (relInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string strFieldName = PlatformCreateTableRelationSQL(relInfo);
            if (strFieldName != DatabaseDef.EMPTY_STRING)
            {
                if (addComma)
                {
                    strFieldName += ",";
                }
                if (addNewLine)
                {
                    strFieldName += DatabaseDef.NEW_LINE_STR;
                }
            }
            return strFieldName;
        }

        public string AlterXPKIndexSQL(IndexDefInfo indexInfo)
        {
            string strNames = indexInfo.CreateFieldsNameLnList();

            StringBuilder strSQL = new StringBuilder("ALTER TABLE ");

            strSQL.Append(indexInfo.TableName)
                .Append(" ADD ")
                .Append(PlatformCreateContraintSQL(strNames, indexInfo.IndexName, indexInfo.Primary));

            return strSQL.ToString();
        }
        public string CreateIndexSQL(IndexDefInfo indexInfo)
        {
            string strNames = indexInfo.CreateFieldsNameLnList();

            StringBuilder strSQL = new StringBuilder("CREATE INDEX ");
            strSQL.Append(indexInfo.IndexName)
                .Append(" ON ")
                .Append(indexInfo.TableName)
                .Append(" (")
                .Append(strNames)
                .Append(")");

            return strSQL.ToString();
        }

        public string CreateQuerySQL(QueryDefInfo queryInfo, uint versCreate)
        {
            if (queryInfo == null)
            {
                return DatabaseDef.EMPTY_STRING;
            }

            string strTableFieldNames = TableColsDefinition(queryInfo, versCreate);
            string strQueryFieldNames = QueryColsDefinition(queryInfo, versCreate);
            string strConstraintQuote = QueryConstraintDefinition(queryInfo, "\nWHERE ");
            string strEndsClauseQuote = QueryEndClausesDefinition(queryInfo, "\n ");

            StringBuilder strBegin = new StringBuilder("CREATE VIEW ");

            strBegin.Append(queryInfo.QueryName)
                .Append(NEW_LINE_STR)
                .Append("(")
                .Append(strQueryFieldNames)
                .Append(")")
                .Append(NEW_LINE_STR)
                .Append("AS SELECT")
                .Append(NEW_LINE_STR)
                .Append(strTableFieldNames)
                .Append(NEW_LINE_STR)
                .Append("FROM ")
                .Append(strConstraintQuote);

            string strSQL = StringUtils.JoinNonEmpty(NEW_LINE_STR, strBegin.ToString(), strEndsClauseQuote);
            strSQL += NEW_LINE_STR;

            return strSQL;
        }

        private string QueryColsDefinition(QueryDefInfo queryInfo, uint versCreate)
        {
            string strFieldNames = "";
            foreach (var field in queryInfo.QueryAliasNamesForVersion(versCreate))
            {
                strFieldNames += field;
                strFieldNames += ",";
                strFieldNames += NEW_LINE_STR;
            }

            string retTableSql = strFieldNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retTableSql;
        }
        private string TableColsDefinition(QueryDefInfo queryInfo, uint versCreate)
        {
            string strFieldNames = "";
            foreach (var field in queryInfo.TableColumnsSourceForVersion(versCreate))
            {
                strFieldNames += field;
                strFieldNames += ",";
                strFieldNames += NEW_LINE_STR;
            }

            string retTableSql = strFieldNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retTableSql;
        }
        private string TableColsAliasDefinition(TableDefPipe tableInfo)
        {
            string strFieldNames = "";
            foreach (var field in tableInfo.TableColumnList())
            {
                if (field.GetSourceInfo()!=null)
                {
                    IList<string> columnNames = field.SourceAllColumnAliasNames();

                    foreach (string columnName in columnNames)
                    {
                        strFieldNames += columnName;
                        strFieldNames += ",";
                        strFieldNames += NEW_LINE_STR;
                    }
                }
            }

            string retTableSql = strFieldNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retTableSql;
        }
        private string QueryConstraintDefinition(QueryDefInfo queryInfo, string delimClause)
        {
            var constraintAllArray = new string[] { QueryInnerJoinDefinition(queryInfo), QueryWhereConsDefinition(queryInfo) };
            var constraintNonEmpty = constraintAllArray.Where((s) => (s.CompareNoCase("") == false)).ToList();

            return string.Join(delimClause, constraintNonEmpty);
        }
        private string QueryEndClausesDefinition(QueryDefInfo queryInfo, string delimClause)
        {
            var endClausesAllArray = queryInfo.QueryCloseList();
            var endClausesNonEmpty = endClausesAllArray.Where((s) => (s.NonEmpty())).Select((c) => (c.CloseInfo)).ToList();

            return string.Join(delimClause, endClausesNonEmpty);
        }

        private string QueryInnerJoinDefinition(QueryDefInfo queryInfo)
        {
            bool bTbJoinConds = true;
            bool bFilterConds = DbJoinWithWhere() == false;

            string strParenthesis = DatabaseDef.EMPTY_STRING;

            string strFieldNames = "";

            var queryTableJoins = queryInfo.QueryTableJoins();
            if (queryTableJoins.Count == 0)
            {
                var queryTables = queryInfo.QueryTableFroms();
                strFieldNames += string.Join(",", queryTables);

                strFieldNames += NEW_LINE_STR;
            }
            else
            {
                foreach (var tableJoin in queryTableJoins)
                {
                    strParenthesis += DbJoinOpenParenthesis();

                    strFieldNames += tableJoin.QueryTableJoinConditions(queryInfo, bTbJoinConds, bFilterConds);

                    strFieldNames += DbJoinCloseParenthesis();

                    strFieldNames += NEW_LINE_STR;
                }
            }

            string retTableSql = strParenthesis + strFieldNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retTableSql;
        }

        private string QueryWhereConsDefinition(QueryDefInfo queryInfo)
        {
            bool bTbJoinConds = false;
            bool bFilterConds = DbJoinWithWhere() == true;

            var queryWhereJoins = queryInfo.QueryWhereJoins(bFilterConds).Select((t) => (t.QueryTableFilterConditions(bTbJoinConds, bFilterConds))).ToList();
            var queryFiltrJoins = queryInfo.FiltrConsDefinition().ToList();

            string retTableSql = string.Join(" AND \n", queryWhereJoins.Concat(queryFiltrJoins));

            return retTableSql;
        }

        private string TableConstraintDefinition(TableDefPipe tableInfo, string delimClause)
        {
            var constraintAllArray = new string[] { TableInnerJoinDefinition(tableInfo), TableWhereConsDefinition(tableInfo) };
            var constraintNonEmpty = constraintAllArray.Where((s) => (s.CompareNoCase("") == false)).ToList();

            return string.Join(delimClause, constraintNonEmpty);
        }
        private string TableEndClausesDefinition(TableDefPipe tableInfo, string delimClause)
        {
            var endClausesAllArray = tableInfo.SourceQueryCloseList();
            var endClausesNonEmpty = endClausesAllArray.Where((s) => (s.NonEmpty())).Select((c) => (c.CloseInfo)).ToList();

            return string.Join(delimClause, endClausesNonEmpty);
        }

        private string TableInnerJoinDefinition(TableDefPipe tableInfo)
        {
            bool bTbJoinConds = true;
            bool bFilterConds = DbJoinWithWhere() == false;

            string strParenthesis = DatabaseDef.EMPTY_STRING;

            string strFieldNames = "";

            var queryTableJoins = tableInfo.SourceTableJoins();
            if (queryTableJoins.Count == 0)
            {
                var queryTables = tableInfo.SourceTableFroms();
                strFieldNames += string.Join(",", queryTables);

                strFieldNames += NEW_LINE_STR;
            }
            else
            {
                foreach (var tableJoin in queryTableJoins)
                {
                    strParenthesis += DbJoinOpenParenthesis();

                    strFieldNames += tableJoin.QueryTableJoinConditions(tableInfo, bTbJoinConds, bFilterConds);

                    strFieldNames += DbJoinCloseParenthesis();

                    strFieldNames += NEW_LINE_STR;
                }
            }

            string retTableSql = strParenthesis + strFieldNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retTableSql;
        }

        private string TableWhereConsDefinition(TableDefPipe tableInfo)
        {
            bool bTbJoinConds = false;
            bool bFilterConds = DbJoinWithWhere() == true;

            var queryWhereJoins = tableInfo.SourceWhereJoins(bFilterConds).Select((t) => (t.QueryTableFilterConditions(bTbJoinConds, bFilterConds))).ToList();
            var queryFiltrJoins = tableInfo.FiltrConsDefinition().ToList();

            string retTableSql = string.Join(" AND \n", queryWhereJoins.Concat(queryFiltrJoins));

            return retTableSql;
        }

        protected string CreateRelationSQL(RelationDefInfo relatInfo, string addBegin, string addClose)
        {
            string strCNames = relatInfo.SourceFieldNameColumnLnList();
            string strFNames = relatInfo.ForeignFieldNameColumnLnList();

            StringBuilder strSQL = new StringBuilder("ALTER TABLE ");

            strSQL.Append(relatInfo.ForeignTableName)
                .Append(" ADD ")
                .Append(addBegin)
                .Append("CONSTRAINT ")
                .Append(relatInfo.RelationName)
                .Append(" FOREIGN KEY (")
                .Append(strFNames)
                .Append(") REFERENCES ")
                .Append(relatInfo.SourceTableName)
                .Append(" (")
                .Append(strCNames)
                .Append(")")
                .Append(addClose);

            return strSQL.ToString();
        }

        protected abstract string DbConvertDataType(TableFieldInfo fieldInfo);
        protected abstract string DbIdentitySQL();
        protected abstract bool DatabaseSynonym();
        protected abstract bool DbColumnDefault();
        protected abstract bool DbJoinWithWhere();
        protected abstract string DbJoinOpenParenthesis();
        protected abstract string DbJoinCloseParenthesis();
        public abstract bool DbXPIndexInCreateTable();
        public abstract bool DbRelationInCreateTable();
        public abstract string CreateDbTriggerUpd(TableDefInfo tableInfo);
        public abstract string CreateDbTriggerIns(TableDefInfo tableInfo);
        public abstract string CreateAlterTableRelationSQL(TableDefInfo tableInfo, RelationDefInfo relatInfo);
    }
}
