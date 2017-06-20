using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Schema.Builder;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Database;

namespace MigrateDataLib.Schema.Generator
{
    public abstract class BaseSchemaGenerator : ISchemaGenerator
    {
        protected DbsDataConfig Config;
        protected UInt32 Version;
        protected ISqlScriptBuilder Builder;
        protected IDatabaseSchema Schema;

        protected IList<TableDefInfo> m_TableList = null;
        protected IList<TableDefInfo> m_IndexList = null;
        protected IList<TableDefInfo> m_TrigUList = null;
        protected IList<TableDefInfo> m_TrigIList = null;
        protected IList<TableDefInfo> m_RelatList = null;
        protected IList<QueryDefInfo> m_QueryList = null;

        public BaseSchemaGenerator(IDatabaseSchema schema, DbsDataConfig config, ISqlScriptBuilder builder, UInt32 version)
        {
            this.Schema = schema;
            this.Config = config;
            this.Builder = builder;
            this.Version = version;
        }

        public void CreateTableList(MigrateSubsets options)
        {
            if (options.FilterTableList != null)
            {
                m_TableList = Schema.CreateFilteredTableCloneList(options.FilterTableList);
            }
            else if (options.SubsetTableList != null)
            {
                m_TableList = Schema.CreateSubsetTableCloneList(options.SubsetTableList);
            }
        }

        public void CreateIndexList(MigrateSubsets options)
        {
            if (options.FilterIndexList != null)
            {
                m_IndexList = Schema.CreateFilteredTableCloneList(options.FilterTableList);
            }
            else if (options.SubsetIndexList != null)
            {
                m_IndexList = Schema.CreateSubsetTableCloneList(options.SubsetTableList);
            }
        }

        public void CreateRelatList(MigrateSubsets options)
        {
            if (options.FilterRelatList != null)
            {
                m_RelatList = Schema.CreateFilteredTableCloneList(options.FilterTableList);
            }
            else if (options.SubsetRelatList != null)
            {
                m_RelatList = Schema.CreateSubsetTableCloneList(options.SubsetTableList);
            }
        }

        public void CreateQueryList(MigrateSubsets options)
        {
            if (options.FilterQueryList != null)
            {
                m_QueryList = Schema.CreateFilteredQueryCloneList(options.FilterQueryList);
            }
            else if (options.SubsetQueryList != null)
            {
                m_QueryList = Schema.CreateSubsetQueryCloneList(options.SubsetQueryList);
            }
        }

        public abstract void PrepareSchema(IGeneratorWriter writer, MigrateOptions buildOptions);

        public IGeneratorWriter CreateWriter(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            return new ScriptWritter(appRootFolder, appDataConfig, outBase64);
        }

        public IGeneratorWriter CreateExecutor(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            return new ScriptExecutor(appRootFolder, appDataConfig);
        }

        public bool Generate(IGeneratorWriter processWriter, MigrateOptions buildOptions)
        {
            bool createDataFiles = buildOptions.CreateDataFiles;
            bool createTableInit = buildOptions.CreateTableInit;
            bool createTableStru = buildOptions.CreateTableStru;
            bool createIndexKeys = buildOptions.CreateIndexKeys;
            bool createIUTrigger = buildOptions.CreateIUTrigers;
            bool insertTableData = buildOptions.InsertTableData;
            bool insertTableSest = buildOptions.InsertTableSest;
            bool createQueryView = buildOptions.CreateQueryView;
            bool createRelations = buildOptions.CreateRelations;

            bool successProcess = false;

            processWriter.PrepareInfo(buildOptions);
            processWriter.PrepareCode(buildOptions);

            processWriter.WriteInfoLine("Process started ...");

            if (CREATEDB_START_CREATE(processWriter, createDataFiles) == false)
                return successProcess;

            if (CREATEDB_CAST1_INITDB(processWriter, createTableInit) == false)
                return successProcess;

            if (CREATEDB_CAST2_TABLES(m_TableList, processWriter, createTableStru, createRelations) == false)
                return successProcess;

            if (CREATEDB_CAST2_INDEXS(m_IndexList, processWriter, createIndexKeys) == false)
                return successProcess;

            if (CREATEDB_CAST3_TRIGGER(m_TrigUList, m_TrigIList, processWriter, createIUTrigger) == false)
                return successProcess;

            if (CREATEDB_CAST3_TVIEWS(m_QueryList, processWriter, createQueryView) == false)
                return successProcess;

            if (CREATEDB_CAST4_INSERT(m_TableList, processWriter, insertTableData) == false)
                return successProcess;

            if (CREATEDB_CAST5_REFINT(m_TableList, processWriter, createRelations) == false)
                return successProcess;

            if (CREATEDB_STOPS_CREATE(m_TableList, processWriter, createDataFiles) == false)
                return successProcess;

            processWriter.WriteInfoLine("Process finished ...");

            return true;
        }

        private bool CREATEDB_START_CREATE(IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of schema started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessStartCreate(processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }


        private bool CREATEDB_CAST1_INITDB(IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of DEFAULTS started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast1InitDb(processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        private bool CREATEDB_CAST2_TABLES(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart, bool createRels)
        {
            processWriter.WriteInfoLine("Creating of TABLES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast2Tables(tableList, createRels, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }


        private bool CREATEDB_CAST2_INDEXS(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of INDEXES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast3Indexs(tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }


        private bool CREATEDB_CAST3_TRIGGER(IList<TableDefInfo> trigUList, IList<TableDefInfo> trigIList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of TRIGGERS started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast4Trigger(trigUList, trigIList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }


        private bool CREATEDB_CAST3_TVIEWS(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of VIEWS started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast5Tviews(queryList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }


        private bool CREATEDB_CAST4_INSERT(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Inserting of INITIAL rows started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast6Insert(tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }


        private bool CREATEDB_CAST5_REFINT(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of RELATIONS started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast7Refint(tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }


        private bool CREATEDB_STOPS_CREATE(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Finishing of schema started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessStopsCreate(tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        protected abstract void TryProcessStartCreate(IGeneratorWriter processWriter);
        protected abstract void TryProcessCast1InitDb(IGeneratorWriter processWriter);
        protected abstract void TryProcessCast2Tables(IList<TableDefInfo> tableList, bool createRels, IGeneratorWriter processWriter);
        protected abstract void TryProcessCast3Indexs(IList<TableDefInfo> tableList, IGeneratorWriter processWriter);
        protected abstract void TryProcessCast4Trigger(IList<TableDefInfo> trigUList, IList<TableDefInfo> trigIList, IGeneratorWriter processWriter);
        protected abstract void TryProcessCast5Tviews(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter);
        protected abstract void TryProcessCast6Insert(IList<TableDefInfo> tableList, IGeneratorWriter processWriter);
        protected abstract void TryProcessCast7Refint(IList<TableDefInfo> tableList, IGeneratorWriter processWriter);
        protected abstract void TryProcessStopsCreate(IList<TableDefInfo> tableList, IGeneratorWriter processWriter);

        private void ExceptionDiagnostics(Exception ex)
        {
            string errorDiagnostics = string.Format("Exception in command: {0}", ex.ToString());
            System.Diagnostics.Debug.Print(errorDiagnostics);
        }
    }
}
