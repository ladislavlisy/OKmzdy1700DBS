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
    class SourceBuilderRuby : SourceBuilderBase
    {
        public SourceBuilderRuby(DbsDataConfig config) : base(config)
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
        private IList<string> AllClassColumnNames(TableFieldInfo columnInfo)
        {
            return columnInfo.AllColumnNames();
        }

        public override void CreateTableClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = EntityClassName(tableInfo) + Extension();

            scriptWriter.OpenCode(1, clazzFileName);

            string namespaceName = ContextProj().ConvertNameToCamel();

            CreateCodeClassesImports(scriptWriter);

            CreateCodeNamespaceClassesOpens(scriptWriter, namespaceName);

            string className = ClassName(EntityName(tableInfo));

            string blokIndent = "";

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionOpens(scriptWriter, tableInfo, className, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionBody(scriptWriter, tableInfo, buildVersion, className, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionClose(scriptWriter, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeNamespaceClose(scriptWriter);

            scriptWriter.CloseCode();
        }

        public override void CreateTableHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {

        }
        public override void CreateQueryClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = EntityClassName(tableInfo) + Extension();

            scriptWriter.OpenCode(1, clazzFileName);

            string namespaceName = ContextProj().ConvertNameToCamel();

            CreateCodeClassesImports(scriptWriter);

            CreateCodeNamespaceClassesOpens(scriptWriter, namespaceName);

            string className = ClassName(EntityName(tableInfo));

            string blokIndent = "";

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionOpens(scriptWriter, tableInfo, className, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionBody(scriptWriter, tableInfo, buildVersion, className, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionClose(scriptWriter, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeNamespaceClose(scriptWriter);

            scriptWriter.CloseCode();
        }

        public override void CreateQueryHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {

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

            scriptWriter.WriteCodeLine(blokIndent + "# " + tableName + " : Declaration of the " + className);
            scriptWriter.WriteCodeLine(blokIndent + "class " + className + " < ActiveRecord::Base");
        }

        private void CreateCodeClassDefinitionClose(IGeneratorWriter scriptWriter, string blokIndent)
        {
            scriptWriter.WriteCodeLine(blokIndent + "end");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }

        private void CreateCodeClassDefinitionBody(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, UInt32 buildVersion, string className, string blokIndent)
        {
            string tableName = tableInfo.TableName();

            IList<TableFieldInfo> columnList = tableInfo.TableColumnsForVersion(buildVersion);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                IList<string> columnNames = AllClassColumnNames(columnInfo);

                foreach (string columnName in columnNames)
                {
                    int columnType = columnInfo.ColumnType;

                    int columnMaxx = columnInfo.DbColumnSize();

                    bool columnNull = columnInfo.DbColumnNull();

                    string propertyName = columnName.ConvertNameToCamel();

                    string propertyType = DBPlatform.EntityConvertDataType(columnType, columnMaxx, !columnNull);

                    if (columnNull == false)
                    {
                        scriptWriter.WriteCode(blokIndent + "validates :" + propertyName);
                        if (columnNull == false)
                        {
                            scriptWriter.WriteCode(", :presence => true");
                        }
                        if (columnType == DatabaseDef.DB_TEXT && columnMaxx != 0)
                        {
                            scriptWriter.WriteCode(", :length => {{ :maximum => {0} }}", columnMaxx);
                        }
                        scriptWriter.WriteCodeLine(EMPTY_SPACES);
                    }
                }
            }

            foreach (TableFieldInfo columnInfo in columnList)
            {
                IList<string> columnNames = AllClassColumnNames(columnInfo);

                foreach (string columnName in columnNames)
                {
                    int columnType = columnInfo.ColumnType;

                    int columnMaxx = columnInfo.DbColumnSize();

                    bool columnNull = columnInfo.DbColumnNull();

                    string propertyName = columnName.ConvertNameToCamel();

                    string propertyType = DBPlatform.EntityConvertDataType(columnType, columnMaxx, !columnNull);

                    scriptWriter.WriteCodeLine(blokIndent + "attr_accessible :" + propertyName);
                }
            }


        }

        public override void CreateTableConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
        }

        public override void CreateQueryConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
        }

        public override void CreateBeginConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
        }

        public override void CreateCloseConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
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
            return ".rb";
        }

        public override string Prefix()
        {
            return "Ruby";
        }
    }
}
