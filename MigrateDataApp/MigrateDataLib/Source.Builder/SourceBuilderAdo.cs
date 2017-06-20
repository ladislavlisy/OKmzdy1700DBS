using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Generator;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Utils;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Source.Builder
{
    class SourceBuilderAdo : SourceBuilderBase
    {
        public SourceBuilderAdo(DbsDataConfig config) : base(config)
        {
        }
        private string EntityName(TableDefInfo tableInfo)
        {
            return tableInfo.TableName().ConvertNameToCamel();
        }

        private string EntityClassName(TableDefInfo tableInfo)
        {
            return ClassName(EntityName(tableInfo));
        }

        private string ClassName(string tableName)
        {
            return m_ChangeNames.Aggregate(tableName, (agr, x) => StringUtils.ReplaceIgnoreCase(agr, x.Item1, x.Item2));
        }

        private string ClassColumnName(TableFieldInfo columnInfo)
        {
            return string.Format("m_{0}", columnInfo.ColumnName);
        }

        private string ClassColumnName(TableFieldInfo columnInfo, int multiIdx)
        {
            return string.Format("m_{0}[{1}]", columnInfo.ColumnName, multiIdx.ToString("0#"));
        }

        public override void CreateTableClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = "Ado" + EntityClassName(tableInfo) + Extension();

            scriptWriter.OpenCode(1, clazzFileName);

            string className = ClassName(EntityName(tableInfo));

            string blokIndent = "";

            CreateCodeClassDefinitionOpens(scriptWriter, tableInfo, className, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionBody(scriptWriter, tableInfo, buildVersion, className, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionClose(scriptWriter, tableInfo, buildVersion, className, blokIndent);

            scriptWriter.CloseCode();
        }

        public override void CreateTableHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = "Ado" + EntityClassName(tableInfo) + ExtensionCpp();

            scriptWriter.OpenCode(1, clazzFileName);

            string className = ClassName(EntityName(tableInfo));

            string blokIndent = "";

            CreateHeadClassDefinitionOpens(scriptWriter, tableInfo, className, blokIndent);

            scriptWriter.CloseCode();
        }
        public override void CreateQueryClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = "Ado" + EntityClassName(tableInfo) + Extension();

            scriptWriter.OpenCode(1, clazzFileName);

            string className = ClassName(EntityName(tableInfo));

            string blokIndent = "";

            CreateCodeClassDefinitionOpens(scriptWriter, tableInfo, className, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionBody(scriptWriter, tableInfo, buildVersion, className, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionClose(scriptWriter, tableInfo, buildVersion, className, blokIndent);

            scriptWriter.CloseCode();
        }

        public override void CreateQueryHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = "Ado" + EntityClassName(tableInfo) + ExtensionCpp();

            scriptWriter.OpenCode(1, clazzFileName);

            string className = ClassName(EntityName(tableInfo));

            string blokIndent = "";

            CreateHeadClassDefinitionOpens(scriptWriter, tableInfo, className, blokIndent);

            scriptWriter.CloseCode();
        }
        private void CreateCodeClassesImports(IGeneratorWriter scriptWriter)
        {
        }
        private void CreateCodeNamespaceClassesOpens(IGeneratorWriter scriptWriter, string namespaceName)
        {
        }
        private void CreateCodeNamespaceClose(IGeneratorWriter scriptWriter)
        {
        }
        private void CreateCodeClassDefinitionOpens(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, string className, string blokIndent)
        {
            string tableName = tableInfo.TableName();
            scriptWriter.WriteCodeLine(blokIndent + "// Ado" + className + ".h : Declaration of the Accessor" + className);
            scriptWriter.WriteCodeLine(blokIndent + "// code generated on " + System.DateTime.Now.ToString());
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "#pragma once");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "class Accessor" + className);
            scriptWriter.WriteCodeLine(blokIndent + "{ ");
            scriptWriter.WriteCodeLine(blokIndent + "public:");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "static CString m_table_name;");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "public:");
        }

        private void CreateHeadClassDefinitionOpens(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, string className, string blokIndent)
        {
            string tableName = tableInfo.TableName();
            scriptWriter.WriteCodeLine(blokIndent + "// Ado" + className + ".cpp implementation of the Accessor" + className);
            scriptWriter.WriteCodeLine(blokIndent + "// code generated on " + System.DateTime.Now.ToString());
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "#include \"StdAfx.h\"");
            scriptWriter.WriteCodeLine(blokIndent + "//#include \"Ado" + className + ".h\"");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "CString Accessor" + className + "::m_table_name = _T(\""+ tableName +"\");");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }

        private void CreateCodeClassDefinitionColumn(IGeneratorWriter scriptWriter, TableFieldInfo columnInfo, UInt32 buildVersion, string columnName, string columnMult, string blokIndent)
        {
            int columnType = columnInfo.ColumnType;

            if (columnType == DatabaseDef.DB_BOOLEAN)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = sizeof(VARIANT_BOOL);");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_BYTE)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = sizeof(BYTE);");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_INTEGER)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = sizeof(SHORT);");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_LONG)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = sizeof(LONG);");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_TEXT)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "[0]      = _T('\\0');");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = 0;");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_DATE)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + ".year     = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + ".month    = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + ".day      = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + ".hour     = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + ".minute   = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + ".second   = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + ".fraction = 0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + "  = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + "  = sizeof(DBTIMESTAMP);");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_CURRENCY)
            {

            }
            else if (columnType == DatabaseDef.DB_SINGLE)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = 0.0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = sizeof(double);");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_DOUBLE)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = 0.0;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = sizeof(double);");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_LONGBINARY)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = NULL;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = 0;");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_MEMO)
            {
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + columnMult + "         = NULL;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status" + columnMult + " = DBSTATUS_S_OK;");
                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Length" + columnMult + " = 0;");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }
            else if (columnType == DatabaseDef.DB_GUID)
            {

            }
        }
        private void CreateCodeClassDefinitionClose(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, UInt32 buildVersion, string className, string blokIndent)
        {
            scriptWriter.WriteCodeLine(blokIndent + "};");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "typedef CAccessor<Accessor" + className + "> CAdo" + className + "Acc;");
            scriptWriter.WriteCodeLine(blokIndent + "typedef CAdoCmdRecordset<CAdo" + className + "Acc> CAdo" + className + ";");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "inline CString Accessor" + className + "::GetTableName()");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "return m_table_name;");
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::GetRowsetProperties(CDBPropSet* pPropSet)");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "pPropSet->AddProperty(DBPROP_CANFETCHBACKWARDS, true, DBPROPOPTIONS_OPTIONAL);");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "pPropSet->AddProperty(DBPROP_CANSCROLLBACKWARDS, true, DBPROPOPTIONS_OPTIONAL);");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "pPropSet->AddProperty(DBPROP_IRowsetChange, true, DBPROPOPTIONS_OPTIONAL);");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "pPropSet->AddProperty(DBPROP_SERVERDATAONINSERT, true);");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "pPropSet->AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);");
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::ClearRecord()");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "memset(this, 0, sizeof(*this));");
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            IList<TableFieldInfo> columnList = tableInfo.TableColumnsForVersion(buildVersion);

            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::Init()");
            scriptWriter.WriteCodeLine(blokIndent + "{");

            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                Int32 multiCount = columnInfo.Multiple();
                if (multiCount > 0)
                {
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "for (int m = 0; m < " + columnName + "Multi; m++)");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "{");

                    CreateCodeClassDefinitionColumn(scriptWriter, columnInfo, buildVersion, columnName, "[m]", blokIndent + TAB_INDENT1);

                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "}");
                }
                else
                {
                    CreateCodeClassDefinitionColumn(scriptWriter, columnInfo, buildVersion, columnName, "", blokIndent);
                }
            }
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::SetStatusToAllColumns(const DBSTATUS& _status)");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                Int32 multiCount = columnInfo.Multiple();
                if (multiCount > 0)
                {
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "for (int m = 0; m < " + columnName + "Multi; m++) {");

                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT2 + columnName + "Status[m] = _status;");

                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "}");
                }
                else
                {
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status = _status;");
                }
            }
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::SetOkStatusToAllColumns()");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "SetStatusToAllColumns(DBSTATUS_S_OK);");
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::SetIgnoreStatusToAllColumns()");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "SetStatusToAllColumns(DBSTATUS_S_IGNORE);");
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::SetIgnoreStatusToAutoincrementColumns()");
            scriptWriter.WriteCodeLine(blokIndent + "{");

            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                if (columnInfo.IsAutoIncrement())
                {
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status = DBSTATUS_S_IGNORE;");
                }
            }

            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::SetAutoincrementColumn(LONG lValue)");
            scriptWriter.WriteCodeLine(blokIndent + "{");

            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                if (columnInfo.IsAutoIncrement())
                {
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + " = lValue;");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status = DBSTATUS_S_OK;");
                }
            }

            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "inline void Accessor" + className + "::SetIgnoreStatusToPrimaryKeys()");
            scriptWriter.WriteCodeLine(blokIndent + "{");

            IList<TableFieldInfo> xpkcolList = tableInfo.XPKIdxColumnsForVersion(buildVersion);

            foreach (TableFieldInfo columnInfo in xpkcolList)
            {
                string columnName = ClassColumnName(columnInfo);

                scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + columnName + "Status = DBSTATUS_S_IGNORE;");
            }

            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }

        private void CreateCodeClassDefinitionBody(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, UInt32 buildVersion, string className, string blokIndent)
        {
            string prevIndent = IndentBack(blokIndent, TAB_INDENT1);

            string tableName = tableInfo.TableName();

            IList<TableFieldInfo> columnList = tableInfo.TableColumnsForVersion(buildVersion);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                int columnType = columnInfo.ColumnType;

                int columnMaxx = columnInfo.DbColumnSize();

                int columnMult = columnInfo.Multiple();

                Int32 int2Str = columnMaxx + 1;

                Int32 mul2Str = columnMult;

                if (columnMult > 0)
                {
                    scriptWriter.WriteCodeLine(blokIndent + "static const size_t " + columnName + "Multi = " + mul2Str.ToString() + ";");
                }
                if (columnType == DatabaseDef.DB_TEXT)
                {
                    scriptWriter.WriteCodeLine(blokIndent + "static const size_t " + columnName + "Count = " + int2Str.ToString() + ";");
                }
                else if (columnType == DatabaseDef.DB_LONGBINARY)
                {
                    scriptWriter.WriteCodeLine(blokIndent + "static const size_t " + columnName + "Count = " + int2Str.ToString() + ";");
                }
                else if (columnType == DatabaseDef.DB_MEMO)
                {
                    scriptWriter.WriteCodeLine(blokIndent + "static const size_t " + columnName + "Count = " + int2Str.ToString() + ";");
                }
            }

            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                int columnType = columnInfo.ColumnType;

                int columnMaxx = columnInfo.DbColumnSize();

                bool columnNull = columnInfo.DbColumnNull();

                string propertyType = DBPlatform.AdoCppConvertDataType(columnType, columnMaxx, !columnNull);

                string propertyAttr = DBPlatform.AdoCppConvertDataAttr(columnType, columnMaxx, !columnNull, columnName);

                Int32 multiCount = columnInfo.Multiple();
                if (multiCount > 0)
                {
                    scriptWriter.WriteCodeLine(blokIndent + propertyType + " " + columnName + "[" + columnName + "Multi]" + propertyAttr + ";");
                }
                else
                {
                    scriptWriter.WriteCodeLine(blokIndent + propertyType + " " + columnName + propertyAttr + ";");
                }
            }

            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                Int32 multiCount = columnInfo.Multiple();
                if (multiCount > 0)
                {
                    scriptWriter.WriteCodeLine(blokIndent + "DBSTATUS " + columnName + "Status[" + columnName + "Multi];");
                }
                else
                {
                    scriptWriter.WriteCodeLine(blokIndent + "DBSTATUS " + columnName + "Status;");
                }
            }

            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                Int32 multiCount = columnInfo.Multiple();
                if (multiCount > 0)
                {
                    scriptWriter.WriteCodeLine(blokIndent + "DBLENGTH " + columnName + "Length[" + columnName + "Multi];");
                }
                else
                {
                    scriptWriter.WriteCodeLine(blokIndent + "DBLENGTH " + columnName + "Length;");
                }
            }

            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(prevIndent + "public:");

            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(prevIndent + "public:");

            scriptWriter.WriteCodeLine(blokIndent + "inline CString GetTableName();");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "inline void GetRowsetProperties(CDBPropSet* pPropSet);");
            scriptWriter.WriteCodeLine(blokIndent + "inline void ClearRecord();");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "inline void Init();");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
            scriptWriter.WriteCodeLine(blokIndent + "inline void SetStatusToAllColumns(const DBSTATUS& _status);");
            scriptWriter.WriteCodeLine(blokIndent + "inline void SetOkStatusToAllColumns();");
            scriptWriter.WriteCodeLine(blokIndent + "inline void SetIgnoreStatusToAllColumns();");
            scriptWriter.WriteCodeLine(blokIndent + "inline void SetIgnoreStatusToAutoincrementColumns();");
            scriptWriter.WriteCodeLine(blokIndent + "inline void SetAutoincrementColumn(LONG lValue);");
            scriptWriter.WriteCodeLine(blokIndent + "inline void SetIgnoreStatusToPrimaryKeys();");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "DEFINE_COMMAND_EX(Accessor" + className + ", \\");
            scriptWriter.WriteCodeLine(blokIndent + "L\"SELECT \\");

            Int32 nCol = 0;
            foreach (TableFieldInfo columnInfo in columnList)
            {
                IList<string> columnNames = columnInfo.AllColumnNames();

                foreach (string columnName in columnNames)
                {
                    if (nCol == 0)
                    {
                        scriptWriter.WriteCode(blokIndent + TAB_INDENT1 + columnName);
                        nCol++;
                    }
                    else
                    {
                        scriptWriter.WriteCodeLine(", \\");
                        scriptWriter.WriteCode(blokIndent + TAB_INDENT1 + columnName);
                    }
                }
            }
            scriptWriter.WriteCodeLine(" \\");
            scriptWriter.WriteCodeLine(blokIndent + "FROM " + tableName + "\")");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

            scriptWriter.WriteCodeLine(blokIndent + "BEGIN_COLUMN_MAP(Accessor" + className + ")");

            nCol = 0;
            foreach (TableFieldInfo columnInfo in columnList)
            {
                string columnName = ClassColumnName(columnInfo);

                Int32 multiCount = columnInfo.Multiple();
                if (multiCount == 0)
                {

                    nCol++;
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "COLUMN_ENTRY_LENGTH_STATUS(" + nCol.ToString() + ", " + columnName + ", " + 
                        columnName + "Length, " + columnName + "Status)");
                }
                else
                {
                    for (Int32 multiIdx = 0; multiIdx < multiCount; multiIdx++)
                    {
                        nCol++;
                        scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "COLUMN_ENTRY_LENGTH_STATUS(" + nCol.ToString() + ", " + columnName + "[" + multiIdx.ToString() + "], " + 
                            columnName + "Length[" + multiIdx.ToString() + "], " + columnName + "Status[" + multiIdx.ToString() + "])");

                    }
                }
            }
            scriptWriter.WriteCodeLine(blokIndent + "END_COLUMN_MAP()");
        }

        public override void CreateTableConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string blokIndent = "";

            string tableName = tableInfo.TableName();

            scriptWriter.WriteCodeLine(blokIndent + "// -------------------------------------------------------------------------------------------");
            scriptWriter.WriteCodeLine(blokIndent + "m_pProgDlg->AddInfoTxt(0, \"" + tableName + "\");");
            scriptWriter.WriteCodeLine(blokIndent + "try");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "// -------------------------------------------------------------------------------------------");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "//  CREATE TABLE \"" + tableName + "\"");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinDatabase.CreateTableDef(ERwinTable, \"" + tableName + "\");");

            IList<TableFieldInfo> columnList = tableInfo.TableColumnsForVersion(buildVersion);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                IList<string> columnNames = columnInfo.AllColumnNames();

                foreach (string columnName in columnNames)
                {
                    int columnType = columnInfo.ColumnType;

                    int columnMaxx = columnInfo.DbColumnSize();

                    Int32 int2Str = columnMaxx;

                    bool columnNull = columnInfo.DbColumnNull();

                    if (columnInfo.IsAutoIncrement())
                    {
                        scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinField = ERwinTable.CreateFAUTO(\"" + columnName + "\", DB_LONG);");
                    }
                    else if (columnType == DatabaseDef.DB_TEXT)
                    {
                        if (columnNull)
                        {
                            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinField = ERwinTable.CreateFTEXT(\"" + columnName + "\", DB_TEXT, " + int2Str.ToString() + ");");
                        }
                        else
                        {
                            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinField = ERwinTable.CreateFTEXT(\"" + columnName + "\", DB_TEXT, " + int2Str.ToString() + ", dbNotNullFieldOption);");
                        }
                    }
                    else
                    {
                        string columnTDao = DBPlatform.DaoCppConvertDataType(columnType, columnMaxx, !columnNull);
                        if (columnNull)
                        {
                            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinField = ERwinTable.CreateField(\"" + columnName + "\", " + columnTDao + ", dbNullFieldOption);");
                        }
                        else
                        {
                            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinField = ERwinTable.CreateField(\"" + columnName + "\", " + columnTDao + ", dbNotNullFieldOption);");
                        }
                    }
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinTable.FieldsAppend(ERwinField);");
                }
            }

            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinTable.Append();");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "// -------------------------------------------------------------------------------------------");
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(blokIndent + "UPGR_EXCEPTION_THROW(m_verzePuv, m_verzePrg)");
            scriptWriter.WriteCodeLine(blokIndent + "// -------------------------------------------------------------------------------------------");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }

        public override void CreateQueryConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
        }

        public override void CreateBeginConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string blokIndent = "";

            foreach (TableDefInfo tableInfo in tableList)
            {
                string tableName = tableInfo.TableName();
                scriptWriter.WriteCodeLine(blokIndent + "UPGR_EXECUTE(ERwinDatabase.TableDefsDelete(\"" + tableName + "\"))");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }

        }

        public override void CreateCloseConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string blokIndent = "";

            foreach (TableDefInfo tableInfo in tableList)
            {
                string tableName = tableInfo.TableName();

                IList<TableFieldInfo> xpkcolList = tableInfo.XPKIdxColumnsForVersion(buildVersion);

                if (xpkcolList.Count > 0)
                {
                    scriptWriter.WriteCodeLine(blokIndent + "// -------------------------------------------------------------------------------------------");
                    scriptWriter.WriteCodeLine(blokIndent + "m_pProgDlg->AddInfoTxt(0, \"" + tableName + "\");");
                    scriptWriter.WriteCodeLine(blokIndent + "try");
                    scriptWriter.WriteCodeLine(blokIndent + "{");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "// -------------------------------------------------------------------------------------------");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "//  CREATE INDEX \"XPK" + tableName + "\"");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinDatabase.TableDefs(ERwinTable, \"" + tableName + "\");");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinIndex = ERwinTable.CreateIndex(\"XPK" + tableName + "\");");

                    foreach (TableFieldInfo columnInfo in xpkcolList)
                    {
                        IList<string> columnNames = columnInfo.AllColumnNames();

                        foreach (string columnName in columnNames)
                        {
                            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinField = ERwinIndex.CreateField(\"" + columnName + "\");");
                            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinIndex.FieldsAppend(ERwinField);");
                        }
                    }

                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinIndex.m_bPrimary = TRUE;");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "ERwinTable.IndexesAppend(ERwinIndex);");
                    scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "// -------------------------------------------------------------------------------------------");
                    scriptWriter.WriteCodeLine(blokIndent + "}");
                    scriptWriter.WriteCodeLine(blokIndent + "UPGR_EXCEPTION_THROW(m_verzePuv, m_verzePrg)");
                    scriptWriter.WriteCodeLine(blokIndent + "// -------------------------------------------------------------------------------------------");
                    scriptWriter.WriteCodeLine(EMPTY_SPACES);
                }
            }
        }

        public override void CreateDbSetContxFile(IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
        }

        private static string IndentPlus(string strBlokIndent, string strIndent)
        {
            string strNewIndent = (strBlokIndent + strIndent);

            return strNewIndent;
        }

        private static string IndentBack(string strBlokIndent, string strIndent)
        {
            int blokIndentLen = strBlokIndent.Length;
            int onesIndentLen = strIndent.Length;

            string strNewIndent = strBlokIndent.Substring(0, blokIndentLen - onesIndentLen);

            return strNewIndent;
        }

        public override string Extension()
        {
            return ".h";
        }

        public string ExtensionCpp()
        {
            return ".cpp";
        }

        public override string Prefix()
        {
            return "Ado";
        }
    }
}
