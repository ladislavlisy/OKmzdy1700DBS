using System;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Constants;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;
using MigrateDataLib.Schema.Generator;
using MigrateDataLib.Schema.DefCopyItems;

namespace MigrateDataLib.Schema.Builder
{
    public class ScriptBuilderSqlite : ScriptBuilderBase
    {
        public ScriptBuilderSqlite(DbsDataConfig config) : base(config)
        {
        }

        protected override string DbConvertDataType(TableFieldInfo fieldInfo)
        {
            string strFieldType = DBPlatform.SqliteConvertDataType(fieldInfo.ColumnType, fieldInfo.ColumnSize);

            return strFieldType;
        }

        protected override string DbIdentitySQL()
        {
            return DatabaseDef.EMPTY_STRING;
        }

        protected override bool DbJoinWithWhere()
        {
            return false;
        }
        protected override string DbJoinOpenParenthesis()
        {
            return DatabaseDef.EMPTY_STRING;
        }
        protected override string DbJoinCloseParenthesis()
        {
            return DatabaseDef.EMPTY_STRING;
        }
        protected override bool DbColumnDefault()
        {
            return true;
        }
        protected override bool DatabaseSynonym()
        {
            return false;
        }
        public override string CreateDefaultSQL(UInt32 versCreate)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override bool DbXPIndexInCreateTable()
        {
            return true;
        }
        public override bool DbRelationInCreateTable()
        {
            return true;
        }
        public override string CreateDbTriggerUpd(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override string CreateDbTriggerIns(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override string CreateTableSEQ(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override string CreateSequeSEC(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override string CreateTableSEC(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override string CreateTableBND(TableDefInfo tableInfo, UInt32 versCreate)
        {
            return DatabaseDef.EMPTY_STRING;
        }

        public override string QuerySynonymCount(string synonymName)
        {
            return DatabaseDef.EMPTY_STRING;
        }

        public override string CreateSequenceSynonym(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }

        public override string CreateTableSynonym(TableDefInfo tableInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }

        public override string CreateSwitchIndentityInsertOn(TableDefInfo tableInfo, bool bIdentityOn, IGeneratorWriter scriptWriter)
        {
            return DatabaseDef.EMPTY_STRING;
        }

        public override string CreateSwitchIndentityInsertOff(TableDefInfo tableInfo, bool bIdentityOn, IGeneratorWriter scriptWriter)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override string CreateSwitchIndentityInsertOn(TableDefPipe tableInfo, bool bIdentityOn, IGeneratorWriter scriptWriter)
        {
            return DatabaseDef.EMPTY_STRING;
        }

        public override string CreateSwitchIndentityInsertOff(TableDefPipe tableInfo, bool bIdentityOn, IGeneratorWriter scriptWriter)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        public override string CreateAlterTableRelationSQL(TableDefInfo tableInfo, RelationDefInfo relatInfo)
        {
            return DatabaseDef.EMPTY_STRING;
        }
        protected override string PlatformCreateTableRelationSQL(RelationDefInfo relInfo)
        {
            string strCNames = relInfo.SourceFieldNameColumnLnList();
            string strFNames = relInfo.ForeignFieldNameColumnLnList();

            string strSQL = ("");
            strSQL += ("FOREIGN KEY(");
            strSQL += strFNames;
            strSQL += (") REFERENCES ");
            strSQL += relInfo.SourceTableName;
            strSQL += ("(");
            strSQL += strCNames;
            strSQL += (")");
            return strSQL;
        }
    }
}