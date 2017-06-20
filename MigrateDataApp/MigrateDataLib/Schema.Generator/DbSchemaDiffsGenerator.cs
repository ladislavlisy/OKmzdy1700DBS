using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Utils;
using MigrateDataLib.SqlData.Adapters;
using System.Data.Common;
using MigrateDataLib.Schema.Comparator;
using System.Data;
using System.Runtime.InteropServices;

namespace MigrateDataLib.Schema.Generator
{
    public class CompareResult
    {
        public bool ResultSuccess { get; private set; }
        public int ResultCount  { get; private set; }

        public CompareResult(bool success, int count)
        {
            ResultCount = 0;
            ResultSuccess = success;
            if (ResultSuccess == true)
            {
                ResultCount = count;
            }
        }
        public static CompareResult Diff(CompareResult fir, CompareResult sec)
        {
            return new CompareResult((fir.ResultSuccess && sec.ResultSuccess), (sec.ResultCount - fir.ResultCount));
        }
        public static CompareResult Sum(CompareResult fir, CompareResult sec)
        {
            return new CompareResult((fir.ResultSuccess && sec.ResultSuccess), (sec.ResultCount + fir.ResultCount));
        }

        public override string ToString()
        {
            if (ResultSuccess)
            {
                return ResultCount.ToString();
            }
            return "failed";
        }

    }
    public class DbSchemaDiffsGenerator : ISchemaGenerator
    {
        protected const string EMPTY_STRING = "";

        protected DbsDataConfig SrcConfig;
        protected IDbComparator SrcCompar;
        protected DbsDataConfig TrgConfig;
        protected IDbComparator TrgCompar;
        protected UInt32 Version;

        public DbSchemaDiffsGenerator(DbsDataConfig srcConfig, DbsDataConfig trgConfig, uint version)
        {
            this.SrcConfig = srcConfig;
            this.SrcCompar = DbComparatorFactory.CreateComparator(this.SrcConfig);
            this.TrgConfig = trgConfig;
            this.TrgCompar = DbComparatorFactory.CreateComparator(this.TrgConfig);
            this.Version = version;
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
            throw new NotImplementedException();
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

                        processWriter.WriteInfoLine("comparing TABLES ... ");
                        DiffTablesCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        processWriter.WriteInfoLine("comparing TABLE COLUMNS ... ");
                        DiffTableColumnsCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        processWriter.WriteInfoLine("comparing VIEWS ... ");
                        DiffViewsCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        processWriter.WriteInfoLine("comparing VIEW COLUMNS ... ");
                        DiffViewColumnsCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        processWriter.WriteInfoLine("comparing CONSTRAINTS ... ");
                        DiffTableConstaintsCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        processWriter.WriteInfoLine("comparing REFERENTIAL ... ");
                        DiffReferConstaintsCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        processWriter.WriteInfoLine("comparing KEYS ... ");
                        DiffKeysConstaintsCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

                        processWriter.WriteInfoLine("comparing CHECK ... ");
                        DiffCheckConstaintsCompare(processWriter, xmlWriter, srcAdapter, trgAdapter, compareDetails);

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

        private CompareResult DiffTablesCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            string sourceCommand = SrcCompar.GetDiffTablesCompareSql();

            string targetCommand = TrgCompar.GetDiffTablesCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target, 
                ADODB.SchemaEnum.adSchemaTables, sourceCommand, targetCommand, "tables", "TABLES", null, null, null, "TABLE");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffTablesCompareNames();

                string[] targetFldName = TrgCompar.GetDiffTablesCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    ADODB.SchemaEnum.adSchemaTables, sourceCommand, targetCommand, sourceFldName, targetFldName,
                    compareAttrib, "tables", "TABLES", null, null, null, "TABLE");
            }
            xmlWriter.WriteEndElement();

            return diffCount01;
        }

        private CompareResult DiffTableColumnsCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            string sourceColumnCommand = SrcCompar.GetDiffTableColumnsCompareSql();

            string targetColumnCommand = TrgCompar.GetDiffTableColumnsCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                ADODB.SchemaEnum.adSchemaColumns, sourceColumnCommand, targetColumnCommand, "columns", "COLUMNS");

            if (diffCount01.ResultSuccess && details)
            {
                string sourceTableCommand = SrcCompar.GetDiffTablesCompareSql();

                string targetTableCommand = TrgCompar.GetDiffTablesCompareSql();

                string[] sourceTableFldName = SrcCompar.GetDiffTablesCompareNames();

                string[] targetTableFldName = TrgCompar.GetDiffTablesCompareNames();

                var tablesAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                IList<Tuple<string, string>> sourceTables = GetDatabaseItemsPlatform(txtWriter, source, 
                ADODB.SchemaEnum.adSchemaTables, sourceTableCommand, sourceTableFldName, tablesAttrib, "TABLES");

                IList<Tuple<string, string>> targetTables = GetDatabaseItemsPlatform(txtWriter, target, 
                    ADODB.SchemaEnum.adSchemaTables, targetTableCommand, targetTableFldName, tablesAttrib, "TABLES");

                string[] sourceColumnFldName = SrcCompar.GetDiffTableColumnsCompareNames();

                string[] targetColumnFldName = TrgCompar.GetDiffTableColumnsCompareNames();

                var columnAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItemsWithLists(txtWriter, xmlWriter, source, target,
                    ADODB.SchemaEnum.adSchemaColumns, sourceTables, targetTables, sourceColumnCommand, targetColumnCommand, 
                    sourceColumnFldName, targetColumnFldName, columnAttrib, "table_columns", "COLUMNS");
            }
            xmlWriter.WriteEndElement();

            return diffCount01;
        }

        private CompareResult DiffViewsCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            string sourceCommand = SrcCompar.GetDiffViewsCompareSql();

            string targetCommand = TrgCompar.GetDiffViewsCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                ADODB.SchemaEnum.adSchemaTables, sourceCommand, targetCommand, "views", "TABLES", null, null, null, "VIEW");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffViewsCompareNames();

                string[] targetFldName = TrgCompar.GetDiffViewsCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    ADODB.SchemaEnum.adSchemaTables, sourceCommand, targetCommand, sourceFldName, targetFldName,
                    compareAttrib, "views", "TABLES", null, null, null, "VIEW");
            }
            xmlWriter.WriteEndElement();

            return diffCount01;
        }

        private CompareResult DiffViewColumnsCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            string sourceCommand01 = SrcCompar.GetDiffViewTablesCompareSql();

            string targetCommand01 = TrgCompar.GetDiffViewTablesCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                ADODB.SchemaEnum.adSchemaViewTableUsage, sourceCommand01, targetCommand01, "view_tables", "VIEW_TABLE_USAGE");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffViewTablesCompareNames();

                string[] targetFldName = TrgCompar.GetDiffViewTablesCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    ADODB.SchemaEnum.adSchemaViewTableUsage, sourceCommand01, targetCommand01, sourceFldName, targetFldName,
                    compareAttrib, "views", "TABLES");
            }

            xmlWriter.WriteEndElement();

            string sourceCommand02 = SrcCompar.GetDiffViewColumnsCompareSql();

            string targetCommand02 = TrgCompar.GetDiffViewColumnsCompareSql();

            CompareResult diffCount02 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                ADODB.SchemaEnum.adSchemaViewColumnUsage, sourceCommand02, targetCommand02, "view_columns", "VIEW_COLUMN_USAGE");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffViewColumnsCompareNames();

                string[] targetFldName = TrgCompar.GetDiffViewColumnsCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    ADODB.SchemaEnum.adSchemaViewColumnUsage, sourceCommand02, targetCommand02, sourceFldName, targetFldName,
                    compareAttrib, "view_columns", "VIEW_COLUMN_USAGE");
            }

            xmlWriter.WriteEndElement();

            return (CompareResult.Sum(diffCount01, diffCount02));
        }

        private CompareResult DiffTableConstaintsCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            // ADODB.SchemaEnum.adSchemaTableConstraints
            ADODB.SchemaEnum openSchema = ADODB.SchemaEnum.adSchemaProviderSpecific;

            string sourceCommand = SrcCompar.GetDiffTableConstaintsCompareSql();

            string targetCommand = TrgCompar.GetDiffTableConstaintsCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                openSchema, sourceCommand, targetCommand, "table_constraints", "TABLE_CONSTRAINTS");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffTableConstaintsCompareNames();

                string[] targetFldName = TrgCompar.GetDiffTableConstaintsCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    openSchema, sourceCommand, targetCommand, sourceFldName, targetFldName,
                    compareAttrib, "table_constraints", "TABLE_CONSTRAINTS");
            }
            xmlWriter.WriteEndElement();

            return diffCount01;
        }

        private CompareResult DiffReferConstaintsCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            // ADODB.SchemaEnum.adSchemaReferentialConstraints
            ADODB.SchemaEnum openSchema = ADODB.SchemaEnum.adSchemaProviderSpecific;

            string sourceCommand = SrcCompar.GetDiffReferConstaintsCompareSql();

            string targetCommand = TrgCompar.GetDiffReferConstaintsCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                openSchema, sourceCommand, targetCommand, "refer_constraints", "REFERENTIAL_CONSTRAINTS");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffReferConstaintsCompareNames();

                string[] targetFldName = TrgCompar.GetDiffReferConstaintsCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    openSchema, sourceCommand, targetCommand, sourceFldName, targetFldName,
                    compareAttrib, "refer_constraints", "REFERENTIAL_CONSTRAINTS");
            }
            xmlWriter.WriteEndElement();

            return diffCount01;
        }

        private CompareResult DiffKeysConstaintsCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            // ADODB.SchemaEnum.adSchemaKeyColumnUsage
            ADODB.SchemaEnum openSchema = ADODB.SchemaEnum.adSchemaProviderSpecific;

            string sourceCommand = SrcCompar.GetDiffKeyConstaintsCompareSql();

            string targetCommand = TrgCompar.GetDiffKeyConstaintsCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                openSchema, sourceCommand, targetCommand, "keys_constraints", "KEY_COLUMN_USAGE");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffKeyConstaintsCompareNames();

                string[] targetFldName = TrgCompar.GetDiffKeyConstaintsCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    openSchema, sourceCommand, targetCommand, sourceFldName, targetFldName,
                    compareAttrib, "keys_constraints", "KEY_COLUMN_USAGE");
            }
            xmlWriter.WriteEndElement();

            return diffCount01;
        }

        private CompareResult DiffCheckConstaintsCompare(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, bool details)
        {
            // ADODB.SchemaEnum.adSchemaCheckConstraints
            ADODB.SchemaEnum openSchema = ADODB.SchemaEnum.adSchemaProviderSpecific;

            string sourceCommand = SrcCompar.GetDiffCheckConstaintsCompareSql();

            string targetCommand = TrgCompar.GetDiffCheckConstaintsCompareSql();

            CompareResult diffCount01 = CompareDatabaseCount(txtWriter, xmlWriter, source, target,
                openSchema, sourceCommand, targetCommand, "check_constraints", "CHECK_CONSTRAINTS");

            if (diffCount01.ResultSuccess && details)
            {
                string[] sourceFldName = SrcCompar.GetDiffCheckConstaintsCompareNames();

                string[] targetFldName = TrgCompar.GetDiffCheckConstaintsCompareNames();

                var compareAttrib = new Dictionary<string, string> { { "type", "COMP_TYPE" } };

                CompareDatabaseItems(txtWriter, xmlWriter, source, target,
                    openSchema, sourceCommand, targetCommand, sourceFldName, targetFldName,
                    compareAttrib, "check_constraints", "CHECK_CONSTRAINTS");
            }
            xmlWriter.WriteEndElement();

            return diffCount01;
        }


        private CompareResult CompareDatabaseCount(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, 
            ADODB.SchemaEnum openSchema, string sqlSource, string sqlTarget,
            string elementText, string datasetText, params object[] queryArray)
        {
            CompareResult sourceTableCount = CompareDatabaseCountPlatform(txtWriter, source, openSchema, sqlSource, datasetText, queryArray);
            CompareResult targetTableCount = CompareDatabaseCountPlatform(txtWriter, target, openSchema, sqlTarget, datasetText, queryArray);

            CompareResult diffTableCount = CompareResult.Diff(sourceTableCount, targetTableCount);

            txtWriter.WriteInfoLine("comparing {0} found {1} differences ... ", elementText, diffTableCount.ToString());

            xmlWriter.WriteElementWithAttributeNoEnd(elementText, "diff_count", diffTableCount.ToString());

            xmlWriter.WriteElementWithAttribute("source", "count", sourceTableCount.ToString());

            xmlWriter.WriteElementWithAttribute("target", "count", targetTableCount.ToString());

            return diffTableCount;
        }

        private CompareResult CompareDatabaseCountPlatform(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema, string sqlCommand, string datasetText, params object[] queryArray)
        {
            if (DBPlatform.IsMsJetType(conAdapter.PlatformType()) && openSchema != ADODB.SchemaEnum.adSchemaProviderSpecific)
            {
                return CompareDatabaseCountADO(txtWriter, conAdapter, openSchema, sqlCommand, datasetText, queryArray);
            }
            else
            {
                return CompareDatabaseCountSql(txtWriter, conAdapter, openSchema, sqlCommand, datasetText);
            }
        }

        private CompareResult CompareDatabaseCountSql(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema, string sqlCommand, string datasetText)
        {
            CompareResult resultCount = new CompareResult(false, 0);

            Int32 schemaItemsCount = 0;

            if (sqlCommand != "")
            {
                string schemaFiltr = String.Format(sqlCommand, conAdapter.DatabaseName(), "dbo", conAdapter.DatabaseOwnr());

                DbDataAdapter schemaCmd = conAdapter.GetAdapter(schemaFiltr);

                DataSet schemaDS = new DataSet();

                try
                {
                    schemaCmd.Fill(schemaDS, datasetText);
                }
                catch (DbException e)
                {
                    txtWriter.WriteInfoLine(e.ToString());
                    return resultCount;
                }

                DataRowCollection schemaRows = schemaDS.Tables[datasetText].Rows;

                schemaItemsCount = schemaRows.Count;
            }
            return new CompareResult(true, schemaItemsCount);
        }

        private CompareResult CompareDatabaseCountADO(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema, string sqlCommand, string datasetText, params object[] queryArray)
        {
            CompareResult resultCount = new CompareResult(false, 0);

            Int32 schemaItemsCount = 0;

            object dummy = Type.Missing;

            ADODB.Connection cnADODB = new ADODB.Connection();

            try
            {
                string schemaConnStr = conAdapter.GetShortConnectString();

                cnADODB.Open(schemaConnStr, conAdapter.Config().OwnerName, conAdapter.Config().PlainOwnerPsw(), 0);

                ADODB.Recordset schemaRows = null;
                if (queryArray.Length != 0)
                {
                    schemaRows = cnADODB.OpenSchema(openSchema, queryArray);
                }
                else
                {
                    schemaRows = cnADODB.OpenSchema(openSchema);
                }

                while (!schemaRows.EOF)
                {
                    schemaItemsCount++;
                    schemaRows.MoveNext();
                }
            }
            catch (DbException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
                return resultCount;
            }
            catch (COMException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
                return resultCount;
            }
            catch (TypeLoadException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
                return resultCount;
            }
            return new CompareResult(true, schemaItemsCount);
        }

        private IList<Tuple<string, string>> GetDatabaseItemsPlatform(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema, 
            string sqlCommand, string[] fieldNames, IDictionary<string, string> fieldTypes, string datasetText, params object[] queryArray)
        {
            string fieldName1 = fieldNames[0];
            string fieldName2 = fieldNames[1];
            string fieldType1 = fieldNames[2];

            IList<Tuple<string, string>> tempDataRows = CompareDatabaseItemsPlatform(txtWriter, conAdapter, openSchema, sqlCommand, datasetText,
                fieldName1, fieldName2, fieldType1, fieldTypes, queryArray);

            return tempDataRows;
        }

        private IList<Tuple<string, string>> CompareDatabaseItemsPlatform(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema, 
            string sqlCommand, string datasetText, string fieldName1, string fieldName2, string fieldType1, IDictionary<string, string> fieldTypes, params object[] queryArray)
        {
            if (DBPlatform.IsMsJetType(conAdapter.PlatformType()) && openSchema != ADODB.SchemaEnum.adSchemaProviderSpecific)
            {
                return CompareDatabaseItemsADO(txtWriter, conAdapter, openSchema, sqlCommand, datasetText, fieldName1, fieldName2, fieldType1, fieldTypes, queryArray);
            }
            else
            {
                return CompareDatabaseItemsSql(txtWriter, conAdapter, openSchema, sqlCommand, datasetText, fieldName1, fieldName2, fieldType1, fieldTypes);
            }
        }

        private IList<Tuple<string, string>> CompareDatabaseItemsSql(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema, 
            string sqlCommand, string datasetText, string fieldName1, string fieldName2, string fieldType1, IDictionary<string, string> fieldTypes)
        {
            List<Tuple<string, string>> schemaDataRows = new List<Tuple<string, string>>();

            if (sqlCommand != "")
            {
                string schemaFiltr = String.Format(sqlCommand, conAdapter.DatabaseName(), "dbo", conAdapter.DatabaseOwnr());

                DbDataAdapter schemaCmd = conAdapter.GetAdapter(schemaFiltr);

                DataSet schemaDS = new DataSet();

                try
                {
                    schemaCmd.Fill(schemaDS, datasetText);
                }
                catch (DbException e)
                {
                    txtWriter.WriteInfoLine(e.ToString());
                }

                DataRowCollection schemaRows = schemaDS.Tables[datasetText].Rows;

                foreach (DataRow row in schemaRows)
                {
                    string fieldType = row[fieldType1].ToString();
                    string fieldName = row[fieldName1].ToString();
                    if (fieldName2 != null)
                    {
                        fieldName += ".";
                        fieldName += row[fieldName2].ToString();
                    }
                    schemaDataRows.Add(Tuple.Create(fieldName, fieldType));
                }
                schemaDataRows.OrderBy((t) => (t.Item1));
            }
            return schemaDataRows;
        }

        private IList<Tuple<string, string>> CompareDatabaseItemsADO(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema, 
            string sqlCommand, string datasetText, string fieldName1, string fieldName2, string fieldType1, IDictionary<string, string> fieldTypes, params object[] queryArray)
        {
            object dummy = Type.Missing;

            ADODB.Connection cnADODB = new ADODB.Connection();

            List<Tuple<string, string>> schemaDataRows = new List<Tuple<string, string>>();

            try
            {
                string schemaConnStr = conAdapter.GetShortConnectString();

                cnADODB.Open(schemaConnStr, conAdapter.Config().OwnerName, conAdapter.Config().PlainOwnerPsw(), 0);

                ADODB.Recordset schemaRows = null;
                if (queryArray.Length != 0)
                {
                    schemaRows = cnADODB.OpenSchema(openSchema, queryArray);
                }
                else
                {
                    schemaRows = cnADODB.OpenSchema(openSchema);
                }

                while (!schemaRows.EOF)
                {
                    string fieldType = schemaRows.Fields[fieldType1].Value.ToString();
                    string fieldName = schemaRows.Fields[fieldName1].Value.ToString();
                    if (fieldName2 != null)
                    {
                        fieldName += ".";
                        fieldName += schemaRows.Fields[fieldName2].Value.ToString();
                    }
                    schemaDataRows.Add(Tuple.Create(fieldName, fieldType));
                    schemaRows.MoveNext();
                }
                schemaDataRows.OrderBy((t) => (t.Item1));
            }
            catch (DbException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
            }
            catch (COMException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
            }
            catch (TypeLoadException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
            }
            return schemaDataRows;
        }

        private IList<Tuple<string, string, string>> GetDatabaseItems3Platform(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema,
            string sqlCommand, string[] fieldNames, IDictionary<string, string> fieldTypes, string datasetText, params object[] queryArray)
        {
            string fieldName1 = fieldNames[0];
            string fieldName2 = fieldNames[1];
            string fieldType1 = fieldNames[2];

            IList<Tuple<string, string, string>> tempDataRows = CompareDatabaseItems3Platform(txtWriter, conAdapter, openSchema, sqlCommand, datasetText, 
                fieldName1, fieldName2, fieldType1, fieldTypes, queryArray);

            return tempDataRows;
        }

        private IList<Tuple<string, string, string>> CompareDatabaseItems3Platform(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema,
            string sqlCommand, string datasetText, string fieldName1, string fieldName2, string fieldType1, IDictionary<string, string> fieldTypes, params object[] queryArray)
        {
            if (DBPlatform.IsMsJetType(conAdapter.PlatformType()) && openSchema != ADODB.SchemaEnum.adSchemaProviderSpecific)
            {
                return CompareDatabaseItems3ADO(txtWriter, conAdapter, openSchema, sqlCommand, datasetText, fieldName1, fieldName2, fieldType1, fieldTypes, queryArray);
            }
            else
            {
                return CompareDatabaseItems3Sql(txtWriter, conAdapter, openSchema, sqlCommand, datasetText, fieldName1, fieldName2, fieldType1, fieldTypes);
            }
        }

        private IList<Tuple<string, string, string>> CompareDatabaseItems3Sql(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema,
            string sqlCommand, string datasetText, string fieldName1, string fieldName2, string fieldType1, IDictionary<string, string> fieldTypes)
        {
            List<Tuple<string, string, string>> schemaDataRows = new List<Tuple<string, string, string>>();

            if (sqlCommand != "")
            {
                string schemaFiltr = String.Format(sqlCommand, conAdapter.DatabaseName(), "dbo", conAdapter.DatabaseOwnr());

                DbDataAdapter schemaCmd = conAdapter.GetAdapter(schemaFiltr);

                DataSet schemaDS = new DataSet();

                try
                {
                    schemaCmd.Fill(schemaDS, datasetText);
                }
                catch (DbException e)
                {
                    txtWriter.WriteInfoLine(e.ToString());
                }

                DataRowCollection schemaRows = schemaDS.Tables[datasetText].Rows;

                foreach (DataRow row in schemaRows)
                {
                    string fieldTabs = row[fieldName1].ToString();
                    string fieldType = row[fieldType1].ToString();
                    string fieldName = row[fieldName1].ToString();
                    if (fieldName2 != null)
                    {
                        fieldName += ".";
                        fieldName += row[fieldName2].ToString();
                    }
                    schemaDataRows.Add(Tuple.Create(fieldName, fieldType, fieldTabs));
                }
                schemaDataRows.OrderBy((t) => (t.Item1));
            }
            return schemaDataRows;
        }

        private IList<Tuple<string, string, string>> CompareDatabaseItems3ADO(IGeneratorWriter txtWriter, ISqlAdapter conAdapter, ADODB.SchemaEnum openSchema,
            string sqlCommand, string datasetText, string fieldName1, string fieldName2, string fieldType1, IDictionary<string, string> fieldTypes, params object[] queryArray)
        {
            object dummy = Type.Missing;

            ADODB.Connection cnADODB = new ADODB.Connection();

            List<Tuple<string, string, string>> schemaDataRows = new List<Tuple<string, string, string>>();

            try
            {
                string schemaConnStr = conAdapter.GetShortConnectString();

                cnADODB.Open(schemaConnStr, conAdapter.Config().OwnerName, conAdapter.Config().PlainOwnerPsw(), 0);

                ADODB.Recordset schemaRows = null;
                if (queryArray.Length != 0)
                {
                    schemaRows = cnADODB.OpenSchema(openSchema, queryArray);
                }
                else
                {
                    schemaRows = cnADODB.OpenSchema(openSchema);
                }

                while (!schemaRows.EOF)
                {
                    string fieldTabs = schemaRows.Fields[fieldType1].Value.ToString();
                    string fieldType = schemaRows.Fields[fieldType1].Value.ToString();
                    string fieldName = schemaRows.Fields[fieldName1].Value.ToString();
                    if (fieldName2 != null)
                    {
                        fieldName += ".";
                        fieldName += schemaRows.Fields[fieldName2].Value.ToString();
                    }
                    schemaDataRows.Add(Tuple.Create(fieldName, fieldType, fieldTabs));
                    schemaRows.MoveNext();
                }
                schemaDataRows.OrderBy((t) => (t.Item1));
            }
            catch (DbException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
            }
            catch (COMException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
            }
            catch (TypeLoadException e)
            {
                txtWriter.WriteInfoLine("ADODB.SchemaEnum = {0}", openSchema);
                txtWriter.WriteInfoLine(e.ToString());
            }
            return schemaDataRows;
        }

        private void CompareDatabaseItems(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target, 
            ADODB.SchemaEnum openSchema, string sqlSource, string sqlTarget,
            string[] sourceFields, string[] targetFields, IDictionary<string, string> fieldTypes, string elementText, string datasetText, params object[] queryArray)
        {
            txtWriter.WriteInfoLine("comparing details of {0} ... ", datasetText);

            IList<Tuple<string, string>> sourceRows = GetDatabaseItemsPlatform(txtWriter, source, openSchema, sqlSource, sourceFields, fieldTypes, datasetText, queryArray);

            IList<Tuple<string, string>> targetRows = GetDatabaseItemsPlatform(txtWriter, target, openSchema, sqlTarget, targetFields, fieldTypes, datasetText, queryArray);

            StringT2Enumerator enumSource = new StringT2Enumerator(sourceRows);
            StringT2Enumerator enumTarget = new StringT2Enumerator(targetRows);

            xmlWriter.WriteStartElement(elementText + "_diff");

            string[] fieldTypeKeys = fieldTypes.Keys.OrderBy(key => key).ToArray();

            string[] sourceFieldType = new string[fieldTypeKeys.Length];
            string sourceFieldName = enumSource.NextValues(sourceFieldType, fieldTypeKeys, fieldTypes);

            string[] targetFieldType = new string[fieldTypeKeys.Length];
            string targetFieldName = enumTarget.NextValues(targetFieldType, fieldTypeKeys, fieldTypes);

            while (enumSource.RowEnumIsValid || enumTarget.RowEnumIsValid)
            {
                if (CompareWithDifference(sourceFieldName, targetFieldName))
                {
                    xmlWriter.WriteElementWithAttributeNoEnd("add", "name", targetFieldName);
                    xmlWriter.WriteFieldTypeValues(targetFieldType, fieldTypeKeys, fieldTypes);
                    xmlWriter.WriteEndElement();

                    targetFieldName = enumTarget.NextValues(targetFieldType, fieldTypeKeys, fieldTypes);
                }
                else if (CompareWithDifference(targetFieldName, sourceFieldName))
                {
                    xmlWriter.WriteElementWithAttributeNoEnd("drop", "name", sourceFieldName);
                    xmlWriter.WriteFieldTypeValues(sourceFieldType, fieldTypeKeys, fieldTypes);
                    xmlWriter.WriteEndElement();

                    sourceFieldName = enumSource.NextValues(sourceFieldType, fieldTypeKeys, fieldTypes);
                }
                else
                {
                    sourceFieldName = enumSource.NextValues(sourceFieldType, fieldTypeKeys, fieldTypes);

                    targetFieldName = enumTarget.NextValues(targetFieldType, fieldTypeKeys, fieldTypes);
                }
            }
            xmlWriter.WriteEndElement();
        }

        private void CompareDatabaseItemsWithLists(IGeneratorWriter txtWriter, IXmlWritter xmlWriter, ISqlAdapter source, ISqlAdapter target,
            ADODB.SchemaEnum openSchema, IList<Tuple<string, string>> sourceTables, IList<Tuple<string, string>> targetTables, 
            string sqlSource, string sqlTarget, string[] sourceFields, string[] targetFields, 
            IDictionary<string, string> fieldTypes, string elementText, string datasetText, params object[] queryArray)
        {
            txtWriter.WriteInfoLine("comparing details of {0} ... ", datasetText);

            IList<Tuple<string, string, string>> sourceDataRows = GetDatabaseItems3Platform(txtWriter, source, openSchema, sqlSource,
                sourceFields, fieldTypes, datasetText, queryArray);

            IList<Tuple<string, string, string>> targetDataRows = GetDatabaseItems3Platform(txtWriter, target, openSchema, sqlTarget,
                targetFields, fieldTypes, datasetText, queryArray);

            StringT3Enumerator enumSource = new StringT3Enumerator(sourceDataRows);
            StringT3Enumerator enumTarget = new StringT3Enumerator(targetDataRows);

            xmlWriter.WriteStartElement(elementText + "_diff");

            string[] fieldTypeKeys = fieldTypes.Keys.OrderBy(key => key).ToArray();
            string[] sourceFieldType = new string[fieldTypeKeys.Length];
            Tuple<string, string> sourceNames = enumSource.NextValues(sourceFieldType, fieldTypeKeys, fieldTypes);

            string[] targetFieldType = new string[fieldTypeKeys.Length];
            Tuple<string, string> targetNames = enumTarget.NextValues(targetFieldType, fieldTypeKeys, fieldTypes);

            while (enumSource.RowEnumIsValid || enumTarget.RowEnumIsValid)
            {
                if (CompareWithDifference(sourceNames.Item2, targetNames.Item2))
                {
                    if (sourceTables.SingleOrDefault((t) => (t.Item1.CompareTo(targetNames.Item1) == 0)) != null)
                    {
                        xmlWriter.WriteElementWithAttributeNoEnd("add", "name", targetNames.Item2);
                        xmlWriter.WriteFieldTypeValues(targetFieldType, fieldTypeKeys, fieldTypes);
                        xmlWriter.WriteEndElement();
                    }

                    targetNames = enumTarget.NextValues(targetFieldType, fieldTypeKeys, fieldTypes);
                }
                else if (CompareWithDifference(targetNames.Item2, sourceNames.Item2))
                {
                    if (targetTables.SingleOrDefault((t) => (t.Item1.CompareTo(sourceNames.Item1) == 0)) != null)
                    {
                        xmlWriter.WriteElementWithAttributeNoEnd("drop", "name", sourceNames.Item2);
                        xmlWriter.WriteFieldTypeValues(sourceFieldType, fieldTypeKeys, fieldTypes);
                        xmlWriter.WriteEndElement();
                    }

                    sourceNames = enumSource.NextValues(sourceFieldType, fieldTypeKeys, fieldTypes);
                }
                else
                {
                    sourceNames = enumSource.NextValues(sourceFieldType, fieldTypeKeys, fieldTypes);

                    targetNames = enumTarget.NextValues(targetFieldType, fieldTypeKeys, fieldTypes);
                }
            }
            xmlWriter.WriteEndElement();
        }

        private static bool CompareWithDifference(string fieldName1, string fieldName2)
        {
            return (fieldName1 == "") || (fieldName2 != "" && fieldName1.CompareTo(fieldName2) > 0);
        }

    }
}
