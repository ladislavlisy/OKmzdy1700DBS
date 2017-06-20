using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Generator;
using MigrateDataLib.Schema.DefCopyItems;

namespace MigrateDataLib.Schema.Builder
{
    public interface ISqlScriptBuilder
    {
        string CreateDefaultSQL(UInt32 versCreate);
        string CreateTableSQL(TableDefInfo tableInfo, bool createRels, UInt32 versCreate);
        string CreateTableSEQ(TableDefInfo tableInfo);
        string CreateSequeSYN(TableDefInfo tableInfo, IGeneratorWriter processWriter);
        string CreateTableSYN(TableDefInfo tableInfo, IGeneratorWriter processWriter);
        string CreateSequeSEC(TableDefInfo tableInfo);
        string CreateTableSEC(TableDefInfo tableInfo);
        string CreateTableBND(TableDefInfo tableInfo, UInt32 versCreate);
        string CreateSelectCommand(TableDefInfo tableInfo, UInt32 versCreate);
        string CreateSelectCommand(TableDefPipe tableInfo, UInt32 versCreate);
        string CreateTableFromPart(TableDefPipe tableInfo, UInt32 versCreate);
        bool DbXPIndexInCreateTable();
        string AlterXPKIndexSQL(IndexDefInfo indexInfo);
        string CreateIndexSQL(IndexDefInfo indexInfo);
        string CreateDbTriggerUpd(TableDefInfo tableInfo);
        string CreateDbTriggerIns(TableDefInfo tableInfo);
        string CreateQuerySQL(QueryDefInfo queryInfo, UInt32 versCreate);
        string CreateAlterTableRelationSQL(TableDefInfo tableInfo, RelationDefInfo relatInfo);
        string CreateSwitchIndentityInsertOn(TableDefInfo tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);
        string CreateSwitchIndentityInsertOff(TableDefInfo tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);
        string CreateSwitchIndentityInsertOn(TableDefPipe tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);
        string CreateSwitchIndentityInsertOff(TableDefPipe tableDef, bool bIdentityOn, IGeneratorWriter scriptWriter);
    }
}
