using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Source.Builder;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Database;
using MigrateDataLib.Constants;
using MigrateDataLib.Schema.DefCopyItems;
using MigrateDataLib.Utils;
using MigrateDataLib.Schema.Transformation;

namespace MigrateDataLib.Schema.Generator
{
    class SrcSourceGenerator : BaseSourceGenerator
    {
        public SrcSourceGenerator(IDatabaseSchema schema, DbsDataConfig config, ISrcCodebaseBuilder builder, UInt32 version)
            : base(schema, config, builder, version)
        {
        }

        public override void PrepareSchema(IGeneratorWriter writer, MigrateOptions buildOptions)
        {
            IList<TableDefCopy> cloneTableList = new List<TableDefCopy>();
            if (m_TableList != null)
            {
                cloneTableList = m_TableList.Select((t) => (new TableDefCopy(t, Version))).ToList();
            }

            m_TableList = cloneTableList.Select((t) => (t.GetTargetInfo())).ToList();

            IList<TableDefCopy> cloneTrigUList = new List<TableDefCopy>();
            if (m_TrigUList != null)
            {
                cloneTrigUList = m_TrigUList.Select((t) => (cloneTableList.SingleOrDefault((c) => (c.TableName().CompareNoCase(t.TableName()))))).ToList();
            }

            m_TrigUList = cloneTrigUList.Select((t) => (t.GetTargetInfo())).ToList();

            IList<TableDefCopy> cloneTrigIList = new List<TableDefCopy>();
            if (m_TrigIList != null)
            {
                cloneTrigIList = m_TrigIList.Select((t) => (cloneTableList.SingleOrDefault((c) => (c.TableName().CompareNoCase(t.TableName()))))).ToList();
            }

            m_TrigIList = cloneTrigIList.Select((t) => (t.GetTargetInfo())).ToList();

            IList<TableDefCopy> cloneIndexList = new List<TableDefCopy>();
            if (m_IndexList != null)
            {
                cloneIndexList = m_IndexList.Select((t) => (cloneTableList.SingleOrDefault((c) => (c.TableName().CompareNoCase(t.TableName()))))).ToList();
            }

            m_IndexList = cloneIndexList.Select((t) => (t.GetTargetInfo())).ToList();

            IList<TableDefCopy> cloneRelatList = new List<TableDefCopy>();
            if (m_RelatList != null)
            {
                cloneRelatList = m_RelatList.Select((t) => (cloneTableList.SingleOrDefault((c) => (c.TableName().CompareNoCase(t.TableName()))))).ToList();
            }

            m_RelatList = cloneRelatList.Select((t) => (t.GetTargetInfo())).ToList();

            IList<QueryDefCopy> cloneQueryList = new List<QueryDefCopy>();
            if (m_QueryList != null)
            {
                cloneQueryList = m_QueryList.Select((t) => (new QueryDefCopy(t, Version))).ToList();
            }

            m_QueryList = cloneQueryList.Select((t) => (t.GetTargetInfo())).ToList();
        }

        protected override void TryProcessClazzesTable(IList<TableDefInfo> tableList, IGeneratorWriter processWriter)
        {
            foreach (TableDefInfo tableDef in tableList)
            {
                Builder.CreateTableClazzFile(tableDef, Version, processWriter);
            }
        }

        protected override void TryProcessHeadersTable(IList<TableDefInfo> tableList, IGeneratorWriter processWriter)
        {
            foreach (TableDefInfo tableDef in tableList)
            {
                Builder.CreateTableHeadzFile(tableDef, Version, processWriter);
            }
        }

        protected override void TryProcessClazzesQuery(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter)
        {
            foreach (QueryDefInfo queryDef in queryList)
            {
                TableDefInfo tableDef = queryDef.GetTableDef();

                Builder.CreateQueryClazzFile(tableDef, Version, processWriter);
            }
        }

        protected override void TryProcessHeadersQuery(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter)
        {
            foreach (QueryDefInfo queryDef in queryList)
            {
                TableDefInfo tableDef = queryDef.GetTableDef();

                Builder.CreateQueryHeadzFile(tableDef, Version, processWriter);
            }
        }

        protected override void TryProcessConfigsTable(IList<TableDefInfo> tableList, IGeneratorWriter processWriter)
        {
            processWriter.OpenCode(3, EMPTY_STRING);

            Builder.CreateBeginConfxCode(tableList, Version, processWriter);

            foreach (TableDefInfo tableDef in tableList)
            {
                Builder.CreateTableConfxCode(tableDef, Version, processWriter);
            }

            Builder.CreateCloseConfxCode(tableList, Version, processWriter);

            processWriter.CloseCode();
        }

        protected override void TryProcessConfigsQuery(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter)
        {
            processWriter.OpenCode(4, EMPTY_STRING);

            foreach (QueryDefInfo queryDef in queryList)
            {
                TableDefInfo tableDef = queryDef.GetTableDef();

                Builder.CreateQueryConfxCode(tableDef, Version, processWriter);
            }

            processWriter.CloseCode();
        }

        protected override void TryProcessContextDbSet(IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, IGeneratorWriter processWriter)
        {
            Builder.CreateDbSetContxFile(tableList, queryList, Version, processWriter);
        }
    }
}
