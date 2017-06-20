using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Utils;
using MigrateDataLib.SqlData.Adapters;
using MigrateDataLib.Schema.Comparator;
using MigrateDataLib.Schema.Database;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.Schema.Generator
{
    public class DbDataDiffsGenerator : ISchemaGenerator
    {
        protected const string EMPTY_STRING = "";

        protected DbsDataConfig SrcConfig;
        protected IDbComparator SrcCompar;
        protected DbsDataConfig TrgConfig;
        protected IDbComparator TrgCompar;
        protected UInt32 Version;
        protected IDatabaseSchema Schema;

        protected IList<TableDefInfo> m_DataTableList = null;
        protected IList<TableDefInfo> m_SestTableList = null;


        public DbDataDiffsGenerator(IDatabaseSchema schemaInfo, DbsDataConfig srcConfig, DbsDataConfig trgConfig, uint version)
        {
            this.SrcConfig = srcConfig;
            this.SrcCompar = DbComparatorFactory.CreateComparator(this.SrcConfig);
            this.TrgConfig = trgConfig;
            this.TrgCompar = DbComparatorFactory.CreateComparator(this.TrgConfig);
            this.Version = version;
            this.Schema = schemaInfo;
        }

        private bool IsPlatformImplemented(UInt32 dbPlatform)
        {
            if (DBPlatform.IsMsJetType(dbPlatform))
            {
                return true;
            }
            else if (DBPlatform.IsMsSQLType(dbPlatform))
            {
                return true;
            }
            else if (DBPlatform.IsOracleType(dbPlatform))
            {
                return true;
            }
            else if (DBPlatform.IsSQLiteType(dbPlatform))
            {
            }
            return false;
        }
        public IGeneratorWriter CreateWriter(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            return new XmlDiffWritter(appRootFolder, appDataConfig);
        }
        public IGeneratorWriter CreateExecutor(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            throw new NotImplementedException();
        }
        public void CreateTableList(MigrateSubsets options)
        {
            IList<TableDefInfo> optionTableList = new List<TableDefInfo>();
            if (options.FilterTableList != null)
            {
                optionTableList = Schema.CreateFilteredTableCloneList(options.FilterTableList);
            }
            else if (options.SubsetTableList != null)
            {
                optionTableList = Schema.CreateSubsetTableCloneList(options.SubsetTableList);
            }
            m_DataTableList = optionTableList.Where((t) => (!t.TableName().StartsWith("ZSEST_"))).ToList();
            m_SestTableList = optionTableList.Where((t) => (t.TableName().StartsWith("ZSEST_"))).ToList();
        }
        public void CreateQueryList(MigrateSubsets options)
        {
            throw new NotImplementedException();
        }

        public void CreateIndexList(MigrateSubsets options)
        {
            throw new NotImplementedException();
        }
        public void CreateRelatList(MigrateSubsets options)
        {
            throw new NotImplementedException();
        }
        public void PrepareSchema(IGeneratorWriter writer, MigrateOptions buildOptions)
        {
        }
        public bool Generate(IGeneratorWriter processWriter, MigrateOptions buildOptions)
        {
            processWriter.PrepareInfo(buildOptions);

            processWriter.PrepareCode(buildOptions);

            bool exportsDetails = buildOptions.CompareXDetails;

            bool compareDetails = true;

            IXmlWritter xmlWriter = processWriter as IXmlWritter;

            if (IsPlatformImplemented(SrcConfig.PlatformType) && IsPlatformImplemented(TrgConfig.PlatformType))
            {
                processWriter.WriteInfoLine(">>> RESULTS file: " + processWriter.InfoFileName());

                xmlWriter.WriteElementWithAttribute("source", "database", SrcConfig.ConfigName);
                xmlWriter.WriteElementWithAttribute("target", "database", TrgConfig.ConfigName);

                try
                {
                    processWriter.WriteInfoLine("comparing Source: {0} / Target: {1} ... ", SrcConfig.DatabaseName, TrgConfig.DatabaseName);

                    using (ISqlAdapter srcAdapter = SqlAdapterFactory.CreateSqlAdapter(SrcConfig), trgAdapter = SqlAdapterFactory.CreateSqlAdapter(TrgConfig))
                    {
                        processWriter.WriteInfoLine("connecting database Source: {0} ... ", SrcConfig.ConfigName);
                        srcAdapter.CreateConnection();

                        srcAdapter.OpenConnection();

                        processWriter.WriteInfoLine("connecting database Target: {0} ... ", TrgConfig.ConfigName);
                        trgAdapter.CreateConnection();

                        trgAdapter.OpenConnection();

                        DiffTablesDataCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        DiffTablesSestCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        srcAdapter.CloseConnection();
                        trgAdapter.CloseConnection();
                    }
                }
                catch (DbException e)
                {
                    processWriter.WriteInfoLine(e.ToString());
                }

                xmlWriter.XmlFileToHtmlFile(exportsDetails);
                return true;
            }
            return false;
        }

        private int DiffTablesSestCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            int retCompareDiff = 0;

            txtWriter.WriteInfoLine("comparing <ZSEST_> TABLE DATA ... ");

            xmlWriter.WriteStartElement("table_data");

            string sourceTablesCommand = SrcCompar.GetTableNamesSelectSql();
            var sourceTableList = GetTableNames(txtWriter, source, sourceTablesCommand, m_SestTableList);

            string targetTablesCommand = TrgCompar.GetTableNamesSelectSql();
            var targetTableList = GetTableNames(txtWriter, target, targetTablesCommand, m_SestTableList);

            IList<TableDefInfo> schemaTables = sourceTableList.Intersect(targetTableList).ToList();

            if (source.IsMsSQLType() && target.IsMsSQLType())
            {
                foreach (var tableInfo in schemaTables)
                {
                    txtWriter.WriteInfoLine("comparing TABLE {0} ... ", tableInfo.TableName());

                    int parCompareDiff = ProcessTableSestDiff(txtWriter, xmlWriter, source, target, tableInfo, details);

                    retCompareDiff += Math.Abs(parCompareDiff);
                }
                xmlWriter.WriteEndElement();
            }
            return retCompareDiff;
        }

        private int DiffTablesDataCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            int retCompareDiff = 0;

            txtWriter.WriteInfoLine("comparing TABLE DATA ... ");

            xmlWriter.WriteStartElement("table_data");

            string sourceTablesCommand = SrcCompar.GetTableNamesSelectSql();
            var sourceTableList = GetTableNames(txtWriter, source, sourceTablesCommand, m_DataTableList);

            string targetTablesCommand = TrgCompar.GetTableNamesSelectSql();
            var targetTableList = GetTableNames(txtWriter, target, targetTablesCommand, m_DataTableList);

            IList<TableDefInfo> schemaTables = sourceTableList.Intersect(targetTableList).ToList();

            foreach (var tableInfo in schemaTables)
            {
                txtWriter.WriteInfoLine("comparing TABLE {0} ... ", tableInfo.TableName());

                int parCompareDiff = ProcessTableDataDiff(txtWriter, xmlWriter, source, target, tableInfo, details);

                retCompareDiff += Math.Abs(parCompareDiff);
            }
            xmlWriter.WriteEndElement();

            return retCompareDiff;
        }

        private IList<TableDefInfo> GetTableNames(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, string sqlTablesCommand, IList<TableDefInfo> tableList)
        {
            if (DBPlatform.IsMsJetType(conAdapter.PlatformType()))
            {
                return GetTableNamesADO(txtWriter, conAdapter, sqlTablesCommand, tableList);
            }
            else
            {
                return GetTableNamesSql(txtWriter, conAdapter, sqlTablesCommand,  tableList);
            }
        }


        private IList<TableDefInfo> GetTableNamesSql(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, string sqlCommand, IList<TableDefInfo> tableList)
        {
            string schemaFiltr = String.Format(sqlCommand, conAdapter.DatabaseName(), "dbo", conAdapter.DatabaseOwnr());

            IList<TableDefInfo> tablesReturn = new List<TableDefInfo>(); 

            try
            {
                DbCommand schemaCmd = conAdapter.GetCommand(schemaFiltr);

                using (var reader = schemaCmd.ExecuteReader())
                {
                    var tableNames = (from IDataRecord emp in reader select emp["TABLE_NAME"].ToString()).ToList();

                    tablesReturn = tableList.Where((t) => (tableNames.Contains(t.TableName()))).ToList();
                }
            }
            catch (Exception e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            return tablesReturn;
        }

        private IList<TableDefInfo> GetTableNamesADO(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, string sqlTablesCommand, IList<TableDefInfo> tableList)
        {
            object dummy = Type.Missing;

            IList<TableDefInfo> tablesReturn = new List<TableDefInfo>();

            ADODB.Connection cnADODB = new ADODB.Connection();

            try
            {
                string schemaConnStr = conAdapter.GetShortConnectString();

                cnADODB.Open(schemaConnStr, conAdapter.Config().OwnerName, conAdapter.Config().PlainOwnerPsw(), 0);

                ADODB.Recordset schemaRows = cnADODB.OpenSchema(ADODB.SchemaEnum.adSchemaTables);

                List<string> tableNames = new List<string>();

                while (!schemaRows.EOF)
                {
                    string fieldName = schemaRows.Fields["TABLE_NAME"].Value.ToString();

                    tableNames.Add(fieldName);

                    schemaRows.MoveNext();
                }

                tablesReturn = tableList.Where((t) => (tableNames.Contains(t.TableName()))).ToList();
            }
            catch (DbException e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            catch (COMException e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            catch (TypeLoadException e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            return tablesReturn;
        }
        private int ProcessTableDataDiff(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, TableDefInfo tableInfo, bool details)
        {
            int retCompareDiff = 0;

            xmlWriter.WriteStartElement(tableInfo.TableName());

            Int32 sourceTableCount = source.SelectRowCountTableData(tableInfo);

            Int32 targetTableCount = target.SelectRowCountTableData(tableInfo);

            Int32 diffTableCount = sourceTableCount - targetTableCount;

            xmlWriter.WriteAttribute("diff_count", diffTableCount.ToString());

            xmlWriter.WriteCountBlock("source", sourceTableCount);
            xmlWriter.WriteCountBlock("target", targetTableCount);

            IList<string> tablePKColumns = tableInfo.PrimaryKeyColumnList();

            IList<string> tabDataColumns = tableInfo.TableDatasColumnList(Version);

            IList<Tuple<string, Int32>> tabColumnTypes = tableInfo.TableColumnTypesList(Version);
            
            if (source.IsMsSQLType() && target.IsMsSQLType())
            {
                string sourceCatalogName = source.DatabaseName();

                string targetCatalogName = target.DatabaseName();

                string schemaName = "dbo";

                Int32 tableColumnsCount = (tablePKColumns.Count + tabDataColumns.Count);

                Int32 diffCount = GetTableRowsCountDiffs(txtWriter, source, sourceCatalogName, targetCatalogName, schemaName,
                    tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);

                xmlWriter.WriteStartElement("diff_rows");

                retCompareDiff += Math.Abs(diffCount);

                xmlWriter.WriteCountElement(true, diffCount);

                if (details)
                {
                    if (diffCount != 0)
                    {
                        GetTableRowsTDataDiffs(txtWriter, xmlWriter, source, sourceCatalogName, targetCatalogName, schemaName, 
                            tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);
                    }
                }
                xmlWriter.WriteEndElement();

                Int32 xpksCount = GetXPKTableRowsCountDiffs(txtWriter, source, sourceCatalogName, targetCatalogName, schemaName,
                    tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);

                if (diffCount > 0 || xpksCount > 0)
                {
                    txtWriter.WriteInfo("found {0} ", diffCount);
                    txtWriter.WriteInfoLine("+ {0} differences.", xpksCount);
                }

                retCompareDiff += Math.Abs(xpksCount);

                xmlWriter.WriteStartElement("xpk_diff_rows");

                xmlWriter.WriteCountElement(true, xpksCount);

                if (details)
                {
                    if (xpksCount != 0)
                    {
                        GetXPKTableRowsVDataDiffs(txtWriter, xmlWriter, source, sourceCatalogName, targetCatalogName, schemaName,
                            tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);
                    }
                }
                xmlWriter.WriteEndElement();

            }
            xmlWriter.WriteEndElement();

            return retCompareDiff;
        }

        private int ProcessTableSestDiff(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, TableDefInfo tableInfo, bool details)
        {
            int retCompareDiff = 0;
            xmlWriter.WriteStartElement(tableInfo.TableName());

            Int32 sourceTableCount = source.SelectRowCountTableData(tableInfo);

            Int32 targetTableCount = target.SelectRowCountTableData(tableInfo);

            Int32 diffTableCount = sourceTableCount - targetTableCount;

            xmlWriter.WriteAttribute("diff_count", diffTableCount.ToString());

            xmlWriter.WriteCountBlock("source", sourceTableCount);
            xmlWriter.WriteCountBlock("target", targetTableCount);

            if (source.IsMsSQLType() && target.IsMsSQLType())
            {
                string sourceCatalogName = source.DatabaseName();

                string targetCatalogName = target.DatabaseName();

                string schemaName = "dbo";

                IList<string> tablePKColumns = tableInfo.OrdinalColumnList();

                IList<string> tabDataColumns = tableInfo.TableDatasColumnList(Version, tablePKColumns);

                IList<Tuple<string, Int32>> tabColumnTypes = tableInfo.TableColumnTypesList(Version);

                Int32 tableColumnsCount = (tablePKColumns.Count + tabDataColumns.Count);

                Int32 diffCount = GetTableRowsCountDiffs(txtWriter, source, sourceCatalogName, targetCatalogName, schemaName,
                    tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);

                xmlWriter.WriteStartElement("diff_rows");

                if (diffCount > 0)
                {
                    txtWriter.WriteInfoLine("found {0} differences.", diffCount);
                }

                retCompareDiff += Math.Abs(diffCount);

                xmlWriter.WriteCountElement(true, diffCount);

                if (details)
                {
                    if (diffCount != 0)
                    {
                        GetTableRowsTDataDiffs(txtWriter, xmlWriter, source, sourceCatalogName, targetCatalogName, schemaName,
                            tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);
                    }
                }
                xmlWriter.WriteEndElement();

                Int32 xpksCount = GetXPKTableRowsCountDiffs(txtWriter, source, sourceCatalogName, targetCatalogName, schemaName,
                    tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);

                xmlWriter.WriteStartElement("xpk_diff_rows");

                xmlWriter.WriteCountElement(true, xpksCount);

                retCompareDiff += Math.Abs(xpksCount);

                if (details)
                {
                    if (xpksCount != 0)
                    {
                        GetXPKTableRowsVDataDiffs(txtWriter, xmlWriter, source, sourceCatalogName, targetCatalogName, schemaName,
                            tableInfo.TableName(), tablePKColumns, tabDataColumns, tabColumnTypes);
                    }
                }
                xmlWriter.WriteEndElement();

            }
            xmlWriter.WriteEndElement();

            return retCompareDiff;
        }

        private Int32 GetTableRowsCountDiffs(IGeneratorWriter txtWriter, ISqlAdapter source, string sourceCatalogName, string targetCatalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string sqlTabRowCount = SrcCompar.GetCountRowsPKNonExists(sourceCatalogName, targetCatalogName, schemaName, tableName, columnsPKs, columnsDat, columnTypes);

            Int32 diffCount = 0;

            if (sqlTabRowCount == "")
            {
                return diffCount;
            }


            try
            {
                diffCount = source.SelectDiffCount(sqlTabRowCount);
            }
            catch (Exception e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            return diffCount;
        }

        private Int32 GetXPKTableRowsCountDiffs(IGeneratorWriter txtWriter, ISqlAdapter source, string sourceCatalogName, string targetCatalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string sqlTabRowCount = SrcCompar.GetCountRowsPKExists(sourceCatalogName, targetCatalogName, schemaName, tableName, columnsPKs, columnsDat, columnTypes);


            Int32 diffCount = 0;

            if (sqlTabRowCount == "")
            {
                return diffCount;
            }


            try
            {
                diffCount = source.SelectDiffCount(sqlTabRowCount);
            }
            catch (Exception e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            return diffCount;
        }

        private void GetTableRowsTDataDiffs(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, string sourceCatalogName, string targetCatalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string sqlCompareQuery = SrcCompar.GetTableRowsPKNonExists(sourceCatalogName, targetCatalogName, schemaName, tableName, columnsPKs, columnsDat, columnTypes);

            if (sqlCompareQuery == "")
            {
                return;
            }

            try
            {
                DbDataAdapter compareQueryAdapter = source.GetAdapter(sqlCompareQuery);

                DataSet compareDataSet = new DataSet();

                compareQueryAdapter.Fill(compareDataSet);

                DataRowCollection compareQueryRows = compareDataSet.Tables[0].Rows;

                xmlWriter.WriteStartElement("diff_data");

                xmlWriter.WriteStartElement("data_cols");

                foreach (string col in columnsPKs)
                {
                    xmlWriter.WriteElement("TD", col);
                }

                foreach (string col in columnsDat)
                {
                    xmlWriter.WriteElement("TD", col);
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("data_rows");

                foreach (DataRow row in compareQueryRows)
                {
                    xmlWriter.WriteStartElement("data_row");
                    foreach (string col in columnsPKs)
                    {
                        xmlWriter.WriteElement("TD", row[col].ToString());
                    }
                    foreach (string col in columnsDat)
                    {
                        xmlWriter.WriteElement("TD", row[col].ToString());
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                return;
            }
            catch (Exception e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            return;
        }

        private void GetXPKTableRowsVDataDiffs(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, string sourceCatalogName, string targetCatalogName, string schemaName, string tableName, IList<string> columnsPKs, IList<string> columnsDat, IList<Tuple<string, Int32>> columnTypes)
        {
            string sqlSourceCompareQuery = SrcCompar.GetTableRowsPKExists(sourceCatalogName, targetCatalogName, schemaName, tableName, columnsPKs, columnsDat, columnTypes);

            if (sqlSourceCompareQuery == "")
            {
                return;
            }

            try
            {
                DbDataAdapter sourceQueryAdapter = source.GetAdapter(sqlSourceCompareQuery);

                DataSet sourceDataSet = new DataSet();

                sourceQueryAdapter.Fill(sourceDataSet);

                DataRowCollection sourceQueryRows = sourceDataSet.Tables[0].Rows;

                xmlWriter.WriteStartElement("diff_data");

                IList<Tuple<string, string>> datacollPKs = new List<Tuple<string, string>>();

                xmlWriter.WriteStartElement("data_rows");

                foreach (DataRow row in sourceQueryRows)
                {
                    datacollPKs = columnsPKs.Select((pk) => (new Tuple<string, string>(pk, row[pk].ToString()))).ToList();

                    string sqlTargetCompareQuery = SrcCompar.GetTableRowsPKDataExist(targetCatalogName, schemaName, tableName, columnsPKs, columnsDat, columnTypes, datacollPKs);

                    DbDataAdapter targetQueryAdapter = source.GetAdapter(sqlTargetCompareQuery);

                    DataSet targetDataSet = new DataSet();

                    targetQueryAdapter.Fill(targetDataSet);

                    DataRowCollection targetQueryRows = targetDataSet.Tables[0].Rows;

                    DataRow tempRow = targetQueryRows[0];

                    xmlWriter.WriteStartElement("data_row");
                    foreach (string col in columnsPKs)
                    {
                        xmlWriter.WriteStartElement("TR");
                        xmlWriter.WriteElement("TD", col);
                        xmlWriter.WriteElement("TD", row[col].ToString());
                        xmlWriter.WriteElement("TD", "");
                        xmlWriter.WriteEndElement();
                    }
                    foreach (string col in columnsDat)
                    {
                        string compData = row[col].ToString();
                        string tempData = tempRow[col].ToString();

                        int compLength = compData.Trim().Length;
                        int tempLength = tempData.Trim().Length;

                        if (compLength != 0 || tempLength != 0)
                        {
                            xmlWriter.WriteStartElement("TR");
                            if (compData.CompareTo(tempData) != 0)
                            {
                                xmlWriter.WriteAttribute("diff_colls", "1");
                            }
                            xmlWriter.WriteElement("TD", col);
                            xmlWriter.WriteElement("TD", row[col].ToString());
                            xmlWriter.WriteElement("TD", tempRow[col].ToString());
                            xmlWriter.WriteEndElement();
                        }
                    }

                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

                return;
            }
            catch (Exception e)
            {
                txtWriter.WriteInfoLine(e.ToString());
            }
            return;
        }

    }
}
