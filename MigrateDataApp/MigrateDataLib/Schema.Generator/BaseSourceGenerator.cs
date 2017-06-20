using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Database;
using MigrateDataLib.Source.Builder;

namespace MigrateDataLib.Schema.Generator
{
    public abstract class BaseSourceGenerator : ISchemaGenerator
    {
        protected const string EMPTY_STRING = "";

        protected DbsDataConfig Config;
        protected UInt32 Version;
        protected ISrcCodebaseBuilder Builder;
        protected IDatabaseSchema Schema;

        protected IList<TableDefInfo> m_TableList = null;
        protected IList<TableDefInfo> m_IndexList = null;
        protected IList<TableDefInfo> m_TrigUList = null;
        protected IList<TableDefInfo> m_TrigIList = null;
        protected IList<TableDefInfo> m_RelatList = null;
        protected IList<QueryDefInfo> m_QueryList = null;

        public BaseSourceGenerator(IDatabaseSchema schema, DbsDataConfig config, ISrcCodebaseBuilder builder, UInt32 version)
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
            return new SourceWritter(appRootFolder, appDataConfig, Builder.Prefix(), Builder.Extension());
        }

        public IGeneratorWriter CreateExecutor(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            return new SourceWritter(appRootFolder, appDataConfig, Builder.Prefix(), Builder.Extension());
        }

        public bool Generate(IGeneratorWriter processWriter, MigrateOptions buildOptions)
        {
            bool createContx = buildOptions.CreateTableInit;
            bool createTable = buildOptions.CreateTableStru;
            bool createViews = buildOptions.CreateQueryView;

            bool successProcess = false;

            processWriter.PrepareInfo(buildOptions);

            processWriter.WriteInfoLine("Process started ...");

            if (CREATESRC_CLAZZES_TABLES(m_TableList, processWriter, createTable) == false)
                return successProcess;
            if (CREATESRC_HEADERS_TABLES(m_TableList, processWriter, createTable) == false)
                return successProcess;
            if (CREATESRC_CONFIGS_TABLES(m_TableList, processWriter, createTable) == false)
                return successProcess;
            if (CREATESRC_CLAZZES_TVIEWS(m_QueryList, processWriter, createViews) == false)
                return successProcess;
            if (CREATESRC_HEADERS_TVIEWS(m_QueryList, processWriter, createTable) == false)
                return successProcess;
            if (CREATESRC_CONFIGS_TVIEWS(m_QueryList, processWriter, createViews) == false)
                return successProcess;
            if (CREATESRC_CONTEXT_ENTITY(m_TableList, m_QueryList, processWriter, createTable, createViews) == false)
                return successProcess;

            processWriter.WriteInfoLine("Process finished ...");

            return true;
        }
        private bool CREATESRC_CONTEXT_ENTITY(IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, IGeneratorWriter processWriter, bool executeTable, bool executeQuery)
        {
            processWriter.WriteInfoLine("Creating of context for ENTITIES started ...");

            if (executeTable == false && executeQuery == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                processWriter.OpenCode(5, EMPTY_STRING);

                IList<TableDefInfo> tableCtxList = new List<TableDefInfo>();
                IList<QueryDefInfo> queryCtxList = new List<QueryDefInfo>();

                if (executeTable)
                {
                    tableCtxList = tableList;
                }
                if (executeQuery)
                {
                    queryCtxList = queryList;
                }

                TryProcessContextDbSet(tableCtxList, queryCtxList, processWriter);

                processWriter.CloseCode();

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }
        private bool CREATESRC_CLAZZES_TABLES(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of classes for TABLES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessClazzesTable(tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }
        private bool CREATESRC_HEADERS_TABLES(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of headers for TABLES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessHeadersTable(tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }
        private bool CREATESRC_CONFIGS_TABLES(IList<TableDefInfo> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of configs for TABLES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessConfigsTable(tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }
        private bool CREATESRC_CLAZZES_TVIEWS(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of classes for QUERIES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessClazzesQuery(queryList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }
        private bool CREATESRC_HEADERS_TVIEWS(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of headers for QUERIES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessHeadersQuery(queryList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }
        private bool CREATESRC_CONFIGS_TVIEWS(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Creating of configs for QUERIES started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessConfigsQuery(queryList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        protected abstract void TryProcessClazzesTable(IList<TableDefInfo> tableList, IGeneratorWriter processWriter);
        protected abstract void TryProcessHeadersTable(IList<TableDefInfo> tableList, IGeneratorWriter processWriter);
        protected abstract void TryProcessConfigsTable(IList<TableDefInfo> tableList, IGeneratorWriter processWriter);
        protected abstract void TryProcessClazzesQuery(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter);
        protected abstract void TryProcessHeadersQuery(IList<QueryDefInfo> tableList, IGeneratorWriter processWriter);
        protected abstract void TryProcessConfigsQuery(IList<QueryDefInfo> queryList, IGeneratorWriter processWriter);
        protected abstract void TryProcessContextDbSet(IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, IGeneratorWriter processWriter);
        private void ExceptionDiagnostics(Exception ex)
        {
            string errorDiagnostics = string.Format("Exception in command: {0}", ex.ToString());
            System.Diagnostics.Debug.Print(errorDiagnostics);
        }
    }
}
