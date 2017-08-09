//#define __NEXT_DATABASE_VERSION__

using System;
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
using System.Data.Common;
using MigrateDataLib.SqlData.Adapters;
using System.Data;

namespace MigrateDataLib.Schema.Generator
{
    class DbCopyDataGenerator : ISchemaGenerator
    {
        protected const int DB_LONG = DatabaseDef.DB_LONG;
        protected static bool dbNullFieldOption = DatabaseDef.dbNullFieldOption;
        public const string COLUMN_NAME_AUTOID = DatabaseDef.COLUMN_NAME_AUTOID;
        public const string NAMEAUTO_REF_ID = DatabaseDef.NAMEAUTO_REF_ID;

        const int LOGGER_COUNT = 1000;

        protected DbsDataConfig SrcConfig;
        protected UInt32 Version;
        protected ISqlScriptBuilder SrcBuilder;
        protected ISqlScriptBuilder DupBuilder;
        protected IDatabaseSchema Schema;

        protected IList<TableDefInfo> m_TableList = null;

        protected IList<TableDefPipe> m_TableCopy = null;

        public DbCopyDataGenerator(IDatabaseSchema schema, DbsDataConfig config, ISqlScriptBuilder sourceBuilder, ISqlScriptBuilder targetBuilder, UInt32 version)
        {
            this.Schema = schema;
            this.SrcConfig = config;
            this.SrcBuilder = sourceBuilder;
            this.DupBuilder = targetBuilder;
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
        }

        public void CreateRelatList(MigrateSubsets options)
        {
        }

        public void CreateQueryList(MigrateSubsets options)
        {
        }

        public void PrepareSchema(IGeneratorWriter writer, MigrateOptions buildOptions)
        {
            writer.PrepareInfo(buildOptions);
            writer.PrepareCode(buildOptions);

            m_TableCopy = new List<TableDefPipe>();

            if (m_TableList != null)
            {
                IDictionary<string, IList<string>> tableRelationInfo = SortedRelationsList(m_TableList);

                IList<TableDefInfo> sortedTableList = SortTableListToClone(m_TableList, tableRelationInfo);

                m_TableCopy = sortedTableList.Select((t) => (new TableDefPipe(t, Version))).ToList();
            }

#if __NEXT_DATABASE_VERSION__
            NextSchemaTransformation.ConvertTablesAutoIdFieldToId(m_TableCopy);
            NextSchemaTransformation.ConvertTablesRelationsMxToId(m_TableCopy);
#endif
        }

        public IGeneratorWriter CreateWriter(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            return new ScriptWritter(appRootFolder, appDataConfig, outBase64);
        }

        public IGeneratorWriter CreateExecutor(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            return new ConsoleExecutor(appRootFolder, appDataConfig);
        }

        private bool NullColumnInRelation(RelationDefInfo newRelation, TableDefPipe newRefTable)
        {
            foreach (var relationField in newRelation.RelationFields())
            {
                TableFieldPipe tableField = newRefTable.ColumnByTargetName(relationField.ForeignName);

                if (tableField != null)
                {
                    if (tableField.GetSourceInfo() == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DifferenceInRelation(RelationDefInfo oldRelation, RelationDefInfo newRelation)
        {
            bool sameTables = oldRelation.SourceTableName.CompareTo(newRelation.SourceTableName) == 0 &&
                oldRelation.ForeignTableName.CompareTo(newRelation.ForeignTableName) == 0;

            if (sameTables)
            {
                int oldFieldsLength = oldRelation.RelationFields().Count;
                int newFieldsLength = newRelation.RelationFields().Count;

                bool sameColumns = (oldFieldsLength == newFieldsLength);
                if (sameColumns)
                {
                    for (int index = 0; index < newFieldsLength; index++)
                    {
                        RelationFieldInfo oldField = oldRelation.RelationFields()[index];
                        RelationFieldInfo newField = newRelation.RelationFields()[index];

                        bool sameFields = (oldField.SourceName.CompareTo(newField.SourceName) == 0 &&
                            oldField.ForeignName.CompareTo(newField.ForeignName) == 0);
                        if (sameFields == false)
                        {
                            sameColumns = false;
                        }
                    }
                }
                return !sameColumns;
            }
            return !sameTables;
        }

        public bool Generate(IGeneratorWriter processWriter, MigrateOptions buildOptions)
        {
            bool insertTableData = buildOptions.InsertTableData;
            bool createRelations = buildOptions.CreateRelations;

            bool successProcess = false;

            processWriter.WriteInfoLine("Process started ...");

            try
            {
                using (ISqlAdapter srcAdapter = SqlAdapterFactory.CreateSqlAdapter(SrcConfig))
                {
                    processWriter.WriteInfoLine("connecting database Source: {0} ... ", SrcConfig.ConfigName);
                    srcAdapter.CreateConnection();

                    srcAdapter.OpenConnection();

                    if (CLONEDB_START_CREATE(srcAdapter, m_TableCopy, processWriter) == false)
                        return successProcess;

                    if (CLONEDB_TABLES_DISABLE_REL(srcAdapter, m_TableCopy, processWriter, createRelations) == false)
                        return successProcess;

                    if (CLONEDB_TABLES_INSERTS_DAT(srcAdapter, m_TableCopy, processWriter, insertTableData) == false)
                        return successProcess;

                    if (CLONEDB_TABLES_UPDATES_REF(srcAdapter, m_TableCopy, processWriter, insertTableData) == false)
                        return successProcess;

                    if (CLONEDB_TABLES_RENABLE_REL(srcAdapter, m_TableCopy, processWriter, createRelations) == false)
                        return successProcess;

                    if (CLONEDB_STOPS_CREATE(srcAdapter, m_TableCopy, processWriter) == false)
                        return successProcess;

                    srcAdapter.CloseConnection();
                }
            }
            catch (DbException e)
            {
                processWriter.WriteInfoLine(e.ToString());
            }


            processWriter.WriteInfoLine("Process finished ...");

            return true;
        }

        private bool CLONEDB_START_CREATE(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
            processWriter.WriteInfoLine("Begining of duplication started ...");

            bool uspesnaCast = true;

            try
            {
                TryProcessStartCreate(source, tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        private bool CLONEDB_TABLES_DISABLE_REL(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Disabling relations of schema started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast1Disable(source, tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        private bool CLONEDB_TABLES_INSERTS_DAT(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Duplication data rows started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast2Insert(source, tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        private bool CLONEDB_TABLES_UPDATES_REF(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Update foreign relations started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast3FxRefer(source, tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        private bool CLONEDB_TABLES_RENABLE_REL(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter, bool executePart)
        {
            processWriter.WriteInfoLine("Renabling relations of schema started ...");

            if (executePart == false)
            {
                processWriter.WriteInfoLine("- skipped ...");
                return true;
            }
            bool uspesnaCast = true;

            try
            {
                TryProcessCast4Renable(source, tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        private bool CLONEDB_STOPS_CREATE(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
            processWriter.WriteInfoLine("Finishing duplication started ...");

            bool uspesnaCast = true;

            try
            {
                TryProcessStopsCreate(source, tableList, processWriter);

                processWriter.WriteInfoLine("- finished successfuly ...");
            }
            catch (Exception ex)
            {
                ExceptionDiagnostics(ex);
                uspesnaCast = false;
            }
            return uspesnaCast;
        }

        protected void TryProcessStartCreate(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
        }

        protected void TryProcessCast1Disable(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
            foreach (TableDefPipe tableDef in tableList)
            {
                IList<RelationDefInfo> relations = tableDef.SourceRelations();
                foreach (var relation in relations)
                {
                }
            }
        }
        protected void TryProcessCast2Insert(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
            foreach (TableDefPipe tableDef in tableList)
            {
                TableProcessCast2Insert(tableDef, source, SrcBuilder, DupBuilder, processWriter);
            }
        }
        protected void TryProcessCast3FxRefer(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
            IList<Tuple<RelationDefInfo,RelationDefInfo>> relations = CreateRelationUpdateList(tableList);
            foreach (var relationDef in relations)
            {
                RelationDefInfo oldRelation = relationDef.Item1;

                RelationDefInfo newRelation = relationDef.Item2;

                bool bStopByName = false;
                if (oldRelation.ForeignTableName.CompareNoCase("ADRESA"))
                {
                    bStopByName = true;
                }

                bool relationChanged = DifferenceInRelation(oldRelation, newRelation);

                TableDefPipe newRefTable = tableList.SingleOrDefault((t) => (t.TableName().CompareNoCase(newRelation.ForeignTableName)));


                if (relationChanged)
                {
                    bool relationRfEmpty = NullColumnInRelation(newRelation, newRefTable);

                    if (relationRfEmpty)
                    {
                        // UPDATE STAV_MES set stav_konfig_refid = (SELECT A.id FROM STAV_KONFIG A WHERE A.firma_id = STAV_MES.firma_id)

                        // UPDATE X SET X.stav_konfig_refid = A.id FROM STAV_MES X, STAV_KONFIG A WHERE A.firma_id = X.firma_id

                        string updateCommand = CrateUpdateRelationCommand(oldRelation, newRelation);

                        processWriter.DefaultCodeLine(updateCommand, newRelation.SourceTableName);
                    }
                }
            }
        }

        private string CrateUpdateRelationCommand(RelationDefInfo oldRelation, RelationDefInfo newRelation)
        {
            string setsColumnList = newRelation.FieldNamesWithAlias("X", "A", ",");
            string condColumnList = oldRelation.FieldNamesWithAlias("X", "A", " AND ");

            StringBuilder commandBuilder = new StringBuilder("UPDATE X SET ").
                Append(setsColumnList).
                AppendFormat(" FROM {0} A, {1} X", newRelation.SourceTableName, newRelation.ForeignTableName).
                AppendFormat(" WHERE {0}", condColumnList);

            return commandBuilder.ToString();
        }

        protected void TryProcessCast4Renable(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
            foreach (TableDefPipe tableDef in tableList)
            {
                IList<RelationDefInfo> relations = tableDef.SourceRelations();
                foreach (var relation in relations)
                {
                }
            }
        }
        protected void TryProcessStopsCreate(ISqlAdapter source, IList<TableDefPipe> tableList, IGeneratorWriter processWriter)
        {
        }

        private void TableProcessCast2Insert(TableDefPipe tableCopy, ISqlAdapter source, 
            ISqlScriptBuilder sourceBuilder, ISqlScriptBuilder targetBuilder, IGeneratorWriter processWriter)
        {
            Int32 pocetInsertedLines = 0;

            string TABLE_NAME = tableCopy.TableName();

            processWriter.WriteInfoLine("> {0}: (BEGIN) ... migrating data", TABLE_NAME);

            string selectCountSql = sourceBuilder.CreateTableFromPart(tableCopy, Version);

            Int32 tableRowsCount = 0;

            try
            {
                tableRowsCount = source.SelectRowCountTableData(TABLE_NAME, selectCountSql);
            }
            catch (OverflowException ex)
            {
                processWriter.WriteInfoLine(ex.ToString());
            }
            catch (DbException ex)
            {
                processWriter.WriteInfoLine(ex.ToString());
            }
            catch (Exception ex)
            {
                processWriter.WriteInfoLine(ex.ToString());
            }

            string selectTableSql = sourceBuilder.CreateSelectCommand(tableCopy, Version);

            DbCommand countCD = source.GetCommand(selectTableSql);

            try
            {
                bool bIdentityInsertOn = false;

                foreach (TableFieldPipe targetColumn in tableCopy.TableColumnList())
                {
                    if (targetColumn.IsSourceToTargetAutoIncrement())
                    {
                        bIdentityInsertOn = true;
                    }
                }

                targetBuilder.CreateSwitchIndentityInsertOn(tableCopy, bIdentityInsertOn, processWriter);

                using (var sourceReader = countCD.ExecuteReader())
                {
                    while (sourceReader.Read())
                    {
                        string insertCommandSql = CreateInsertCommandWithValues(tableCopy, Version, sourceReader, processWriter);

                        pocetInsertedLines++;

                        if (pocetInsertedLines == 0 || (pocetInsertedLines % LOGGER_COUNT) == 0)
                        {
                            processWriter.WriteInfoLine("> {0}: ({1}) ... migrating row: {2} ... ", TABLE_NAME, tableRowsCount, pocetInsertedLines.ToString());
                        }
                    }
                }

                targetBuilder.CreateSwitchIndentityInsertOff(tableCopy, bIdentityInsertOn, processWriter);

                processWriter.WriteInfoLine("> {0}: ({1}) ... migrating data completed", TABLE_NAME, tableRowsCount);
            }
            catch (OverflowException ex)
            {
                processWriter.WriteInfoLine(ex.ToString());
            }
            catch (DbException ex)
            {
                processWriter.WriteInfoLine(ex.ToString());
            }
        }

        public string CreateInsertCommandWithValues(TableDefPipe tableCopy, uint versCreate, DataRow values, IGeneratorWriter scriptWriter)
        {
            IList<TableFieldPipe> columnCopy = tableCopy.TableColumnList();

            string sqlCommand = "INSERT INTO ";

            sqlCommand += tableCopy.TableName();

            sqlCommand += " (";
            sqlCommand += tableCopy.CreateSourceToTargetColumnList();
            sqlCommand += " ) VALUES (";
            sqlCommand += CreateValueList(columnCopy, values, scriptWriter.PlatformType());
            sqlCommand += " )";

            scriptWriter.DefaultCodeLine(sqlCommand, tableCopy.TableName());

            return sqlCommand;
        }

        private string CreateValueList(IList<TableFieldPipe> columns, DataRow values, UInt32 platformType)
        {
            string columnsList = "";

            foreach (TableFieldPipe field in columns)
            {
                TableFieldInfo source = field.GetSourceInfo();

                TableFieldInfo target = field.GetTargetInfo();

                if (source != null && target != null)
                {
                    IList<string> columnNames = source.AllColumnNames();

                    foreach (string columnName in columnNames)
                    {
                        int typeColumn = target.ColumnType;

                        string dataItem = values[columnName].ToString();

                        bool appendValue = target.IncludeColumnType();

                        string dataColumn = target.StringColumnData(dataItem, platformType);

                        if (appendValue)
                        {
                            columnsList += dataColumn;
                            columnsList += ",";
                        }
                    }
                }
            }
            string columnListRet = columnsList.TrimEnd(DatabaseDef.TRIM_CHARS);

            return columnListRet;
        }
        public string CreateInsertCommandWithValues(TableDefPipe tableCopy, uint versCreate, DbDataReader sourceReader, IGeneratorWriter scriptWriter)
        {
            IList<TableFieldPipe> columnCopy = tableCopy.TableColumnList();

            string sqlCommand = "INSERT INTO ";

            sqlCommand += tableCopy.TableName();

            sqlCommand += " (";
            sqlCommand += tableCopy.CreateSourceToTargetColumnList();
            sqlCommand += " ) VALUES (";
            sqlCommand += CreateValueList(columnCopy, sourceReader, scriptWriter.PlatformType());
            sqlCommand += " )";

            scriptWriter.DefaultCodeLine(sqlCommand, tableCopy.TableName());

            return sqlCommand;
        }

        private string CreateValueList(IList<TableFieldPipe> columns, DbDataReader sourceReader, UInt32 platformType)
        {
            string columnsList = "";

            foreach (TableFieldPipe field in columns)
            {
                TableFieldInfo source = field.GetSourceInfo();

                TableFieldInfo target = field.GetTargetInfo();

                if (source != null && target != null)
                {
                    IList<string> columnNames = source.AllColumnNames();

                    foreach (string columnName in columnNames)
                    {
                        int typeColumn = target.ColumnType;

                        string dataItem = sourceReader[columnName].ToString();

                        bool appendValue = target.IncludeColumnType();

                        string dataColumn = target.StringColumnData(dataItem, platformType);

                        if (appendValue)
                        {
                            columnsList += dataColumn;
                            columnsList += ",";
                        }
                    }
                }
            }
            string columnListRet = columnsList.TrimEnd(DatabaseDef.TRIM_CHARS);

            return columnListRet;
        }
        public static IDictionary<string, IList<string>> SortedRelationsList(IList<TableDefInfo> tablesInfo)
        {
            IDictionary<string, IList<string>> mapaRelationList = new Dictionary<string, IList<string>>();

            foreach (TableDefInfo tableDef in tablesInfo)
            {
                IList<string> initRelationList = new List<string>();

                IList<string> retsRelationList = tableDef.DeepRelationsList(initRelationList, tablesInfo, false);

                mapaRelationList.Add(tableDef.TableName(), retsRelationList.Distinct().ToList());
            }
            return mapaRelationList;
        }

        public static IList<TableDefInfo> SortTableListToClone(IList<TableDefInfo> sourceInfo, IDictionary<string, IList<string>> relationList)
        {
            List<TableDefInfo> sortInfo = sourceInfo.ToList();

            sortInfo.Sort(delegate (TableDefInfo x, TableDefInfo y)
            {
                if (x.TableName() == null && y.TableName() == null)
                {
                    return 0;
                }
                else if (x.TableName() == null)
                {
                    return -1;
                }
                else if (y.TableName() == null)
                {
                    return 1;
                }
                else if (x.Equals(y))
                {
                    return 0;
                }
                else if (RelateTo(x, y, relationList))
                {
                    return 1;
                }
                else if (RelateTo(y, x, relationList))
                {
                    return -1;
                }
                return CompareToRelate(x, y, relationList);
            });
            return sortInfo;
        }

        private static int CompareToRelate(TableDefInfo tableX, TableDefInfo tableY, IDictionary<string, IList<string>> relationList)
        {
            IList<string> relationsX = null;

            IList<string> relationsY = null;

            relationList.TryGetValue(tableX.TableName(), out relationsX);

            relationList.TryGetValue(tableY.TableName(), out relationsY);

            if (relationsX == null && relationsY == null)
            {
                return 0;
            }
            else if (relationsX == null)
            {
                return -1;
            }
            else if (relationsY == null)
            {
                return 1;
            }
            else
            {
                int compareCount = relationsX.Count.CompareTo(relationsY.Count);
                if (compareCount == 0)
                {
                    return tableX.CompareTo(tableY);
                }
                else
                {
                    return compareCount;
                }
            }
        }

        public static bool RelateTo(TableDefInfo tableX, TableDefInfo tableY, IDictionary<string, IList<string>> relationList)
        {
            IList<string> relations = null;

            relationList.TryGetValue(tableX.TableName(), out relations);

            if (relations == null) return false;

            string relatedTable = relations.FirstOrDefault((r) => (r.Equals(tableY.TableName())));

            return (relatedTable != null);
        }

        IList<Tuple<RelationDefInfo, RelationDefInfo>> CreateRelationUpdateList(IList<TableDefPipe> tableList)
        {
            return tableList.SelectMany((t) => CollectUpdateRelations(t, tableList)).ToList();
        }

        public IList<Tuple<RelationDefInfo, RelationDefInfo>> CollectUpdateRelations(TableDefPipe tableInfo, IList<TableDefPipe> listTables)
        {
            IList<RelationDefInfo> foreignRelations = tableInfo.SourceForeignRelations(listTables);

            IList<string> foreignRelNames = foreignRelations.Select((r) => (r.RelationUniqueForeignAllNames())).ToList();

            return foreignRelations.Select((r) => (RelationsMakePair(tableInfo, r, foreignRelNames, listTables))).ToList();
        }

        public Tuple<RelationDefInfo, RelationDefInfo> RelationsMakePair(TableDefPipe tableInfo, RelationDefInfo relation, IList<string> relationNames, IList<TableDefPipe> listTables)
        {
            RelationDefInfo relationOld = (RelationDefInfo)relation.Clone();
            RelationDefInfo relationNew = (RelationDefInfo)relation.Clone();

            relationOld.ReNameSourceColumnList(tableInfo.TableColumnList());

            var relationTable = listTables.SingleOrDefault((t) => (t.TableName().CompareTo(relation.ForeignTableName) == 0));

            if (relationTable != null)
            {
                string foreignColName = NextSchemaTransformation.CreateForeignNameMxToId(tableInfo, relation, relationTable, relationNames);

                string constraintName = NextSchemaTransformation.CreateRelationNameMxToId(foreignColName, relationTable.TableName());

                relationNew = relationTable.TargetRelations().SingleOrDefault((r) => (r.RelationName.CompareNoCase(constraintName)));
            }

            return new Tuple<RelationDefInfo, RelationDefInfo>(relationOld, relationNew);
        }


        private void ExceptionDiagnostics(Exception ex)
        {
            string errorDiagnostics = string.Format("Exception in command: {0}", ex.ToString());
            System.Diagnostics.Debug.Print(errorDiagnostics);
        }
    }
}
