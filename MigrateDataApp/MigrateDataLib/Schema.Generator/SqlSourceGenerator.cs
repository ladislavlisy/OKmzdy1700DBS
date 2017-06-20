﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Schema.Builder;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Database;
using MigrateDataLib.Constants;
using MigrateDataLib.Schema.DefCopyItems;
using MigrateDataLib.Utils;
using MigrateDataLib.Schema.Transformation;

namespace MigrateDataLib.Schema.Generator
{
    class SqlSourceGenerator : BaseSchemaGenerator
    {
        public SqlSourceGenerator(IDatabaseSchema schema, DbsDataConfig config, ISqlScriptBuilder builder, UInt32 version)
            : base(schema, config, builder, version)
        {
        }

        public override void PrepareSchema(IGeneratorWriter writer, MigrateOptions buildOptions)
        {
        }

        protected override void TryProcessStartCreate(IGeneratorWriter processWriter)
        {
        }

        protected override void TryProcessCast1InitDb(IGeneratorWriter processWriter)
        {
            string scriptPart = Builder.CreateDefaultSQL(Version);

            processWriter.DefaultCodeLine(scriptPart, "Database Defaults");
        }

        protected override void TryProcessCast2Tables(IList<TableDefInfo> tableList, bool createRels, IGeneratorWriter processWriter)
        {
            foreach (TableDefInfo tableInfo in tableList)
            {
                string scriptPartTbl = Builder.CreateTableSQL(tableInfo, createRels, Version);

                processWriter.DefaultCodeLine(scriptPartTbl, tableInfo.InfoName());

                string scriptPartSeq = Builder.CreateTableSEQ(tableInfo);

                processWriter.DefaultCodeLine(scriptPartSeq, tableInfo.InfoName());

                string scriptSynsSeq = Builder.CreateSequeSYN(tableInfo, processWriter);

                processWriter.DefaultCodeLine(scriptSynsSeq, tableInfo.InfoName());

                string scriptSynsTab = Builder.CreateTableSYN(tableInfo, processWriter);

                processWriter.DefaultCodeLine(scriptSynsTab, tableInfo.InfoName());

                string scriptSeqsSEC = Builder.CreateSequeSEC(tableInfo);

                processWriter.DefaultCodeLine(scriptSeqsSEC, tableInfo.InfoName());

                string scriptPartSec = Builder.CreateTableSEC(tableInfo);

                processWriter.DefaultCodeLine(scriptPartSec, tableInfo.InfoName());

                string scriptPartBnd = Builder.CreateTableBND(tableInfo, Version);

                processWriter.DefaultCodeLine(scriptPartBnd, tableInfo.InfoName());

            }
        }

        protected override void TryProcessCast3Indexs(IList<TableDefInfo> tableList, IGeneratorWriter processWriter)
        {
            bool bCreatePKs = (Builder.DbXPIndexInCreateTable() == false);
            foreach (TableDefInfo tableDef in tableList)
            {
                if (bCreatePKs)
                {
                    IndexDefInfo indexPK = tableDef.IndexPK();
                    if (indexPK != null)
                    {
                        string scriptPart = Builder.AlterXPKIndexSQL(indexPK);

                        scriptPart += DatabaseDef.NEW_LINE_STR;

                        processWriter.DefaultCodeLine(scriptPart, indexPK.InfoName());
                    }
                }

                IList<IndexDefInfo> indexes = tableDef.IndexesNonPK();
                foreach (var indexIF in indexes)
                {
                    string scriptPart = Builder.CreateIndexSQL(indexIF);

                    scriptPart += DatabaseDef.NEW_LINE_STR;

                    processWriter.DefaultCodeLine(scriptPart, indexIF.InfoName());
                }
            }
        }

        protected override void TryProcessCast4Trigger(IList<TableDefInfo> trigUList, IList<TableDefInfo> trigIList, IGeneratorWriter processWriter)
        {
            string scriptPart = DatabaseDef.EMPTY_STRING;

            foreach (TableDefInfo tableDef in trigUList)
            {
                scriptPart = Builder.CreateDbTriggerUpd(tableDef);
                processWriter.DefaultCodeLine(scriptPart, tableDef.InfoName());
            }
            foreach (TableDefInfo tableDef in trigIList)
            {
                scriptPart = Builder.CreateDbTriggerIns(tableDef);
                processWriter.DefaultCodeLine(scriptPart, tableDef.InfoName());
            }
        }

        protected override void TryProcessCast5Tviews(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter)
        {
            foreach (QueryDefInfo queryInfo in queryList)
            {
                string scriptPartTbl = Builder.CreateQuerySQL(queryInfo, Version);

                processWriter.DefaultCodeLine(scriptPartTbl, queryInfo.InfoName());
            }
        }

        protected override void TryProcessCast6Insert(IList<TableDefInfo> tableList, IGeneratorWriter processWriter)
        {
            string scriptTemp = "INSERT INTO STAV_DATABAZE (verze, heslo) VALUES ({0}, \'2C1A313A0F528234\')";

            string scriptPart = string.Format(scriptTemp, Version);

            processWriter.DefaultCodeLine(scriptPart, "STAV_DATABAZE");
        }

        protected override void TryProcessCast7Refint(IList<TableDefInfo> tableList, IGeneratorWriter processWriter)
        {
            foreach (TableDefInfo tableDef in tableList)
            {
                IList<RelationDefInfo> relations = tableDef.Relations();
                foreach (var relation in relations)
                {
                    string scriptPart = Builder.CreateAlterTableRelationSQL(tableDef, relation);

                    processWriter.DefaultCodeLine(scriptPart, relation.InfoName());
                }
            }
        }

        protected override void TryProcessStopsCreate(IList<TableDefInfo> tableList, IGeneratorWriter processWriter)
        {
        }
    }
}
