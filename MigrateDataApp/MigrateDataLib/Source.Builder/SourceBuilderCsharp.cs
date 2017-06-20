using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Generator;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Source.Builder
{
    public class SourceBuilderCsharp : SourceBuilderBase
    {
        public SourceBuilderCsharp(DbsDataConfig config) : base(config)
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

        #region CLAZZ_ENTITY_FILES
        public override void CreateTableClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = EntityClassName(tableInfo) + Extension();

            scriptWriter.OpenCode(1, clazzFileName);

            string namespaceName = ProjectNamespace;

            CreateCodeClassesImports(scriptWriter);

            CreateCodeNamespaceClassesOpens(scriptWriter, namespaceName);

            CreateTableClazzCode(tableInfo, buildVersion, scriptWriter);

            CreateCodeNamespaceClose(scriptWriter);

            scriptWriter.CloseCode();
        }

        public override void CreateTableHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {

        }
        public override void CreateQueryClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string clazzFileName = EntityClassName(tableInfo) + Extension();

            scriptWriter.OpenCode(2, clazzFileName);

            string namespaceName = ProjectNamespace;

            CreateCodeClassesImports(scriptWriter);

            CreateCodeNamespaceClassesOpens(scriptWriter, namespaceName);

            CreateTableClazzCode(tableInfo, buildVersion, scriptWriter);

            CreateCodeNamespaceClose(scriptWriter);

            scriptWriter.CloseCode();
        }

        public override void CreateQueryHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {

        }
        private void CreateCodeClassesImports(IGeneratorWriter scriptWriter)
        {
            scriptWriter.WriteCodeLine("using System;");
            scriptWriter.WriteCodeLine("using System.Linq;");
            scriptWriter.WriteCodeLine("using System.Text;");
            scriptWriter.WriteCodeLine("using System.Threading.Tasks;");
#if REPOSITORY_PATTERN
            scriptWriter.WriteCodeLine("using Repository.Pattern.Ef6;");
#endif
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }
        private void CreateCodeNamespaceClassesOpens(IGeneratorWriter scriptWriter, string namespaceName)
        {
            scriptWriter.WriteCodeLine("namespace " + namespaceName + ".EntityModel");
            scriptWriter.WriteCodeLine("{");
        }
        private void CreateCodeNamespaceClose(IGeneratorWriter scriptWriter)
        {
            scriptWriter.WriteCodeLine("}");
        }
        private void CreateTableClazzCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string className = EntityClassName(tableInfo);

            string blokIndent = "";

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionOpens(scriptWriter, tableInfo, className, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionBody(scriptWriter, tableInfo, buildVersion, className, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeClassDefinitionClose(scriptWriter, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

        }
        private void CreateCodeClassDefinitionOpens(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, string className, string blokIndent)
        {
            string tableName = tableInfo.TableName();

            scriptWriter.WriteCodeLine(blokIndent + "// " + tableName + " : Declaration of the " + className);
#if REPOSITORY_PATTERN
            scriptWriter.WriteCodeLine(blokIndent + "public class " + className + " : Entity");
#else
            scriptWriter.WriteCodeLine(blokIndent + "public class " + className);
#endif
            scriptWriter.WriteCodeLine(blokIndent + "{");
        }

        private void CreateCodeClassDefinitionClose(IGeneratorWriter scriptWriter, string blokIndent)
        {
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }

        private void CreateCodeClassDefinitionBody(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, UInt32 buildVersion, string className, string blokIndent)
        {
            scriptWriter.WriteCodeLine(blokIndent + "public " + className + "()");
            scriptWriter.WriteCodeLine(blokIndent + "{");
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);

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

                    scriptWriter.WriteCodeLine(blokIndent + "public " + propertyType + " " + propertyName + " { get; set; }");
                }
            }
        }
        
        #endregion

        #region CONFIG_ENTITY_FILES

        public override void CreateTableConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string blokIndent = "";

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassConfigsOpens(scriptWriter, tableInfo, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassConfigsTableBody(scriptWriter, tableInfo, buildVersion, blokIndent);

            CreateCodeClassConfigsClose(scriptWriter, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

        }
        public override void CreateQueryConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string blokIndent = "";

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassConfigsOpens(scriptWriter, tableInfo, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassConfigsQueryBody(scriptWriter, tableInfo, buildVersion, blokIndent);

            CreateCodeClassConfigsClose(scriptWriter, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

        }
        public override void CreateBeginConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string namespaceName = ProjectNamespace;

            CreateCodeConfigsImports(scriptWriter, namespaceName);

            CreateCodeNamespaceConfigsOpens(scriptWriter, namespaceName);
        }

        public override void CreateCloseConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            CreateCodeNamespaceClose(scriptWriter);
        }

        private void CreateCodeConfigsImports(IGeneratorWriter scriptWriter, string namespaceName)
        {
            scriptWriter.WriteCodeLine("using System;");
            scriptWriter.WriteCodeLine("using System.Linq;");
            scriptWriter.WriteCodeLine("using System.Text;");
            scriptWriter.WriteCodeLine("using System.Threading.Tasks;");
            scriptWriter.WriteCodeLine("using System.Data.Entity;");
            scriptWriter.WriteCodeLine("using System.Data.Entity.ModelConfiguration;");
            scriptWriter.WriteCodeLine("using " + namespaceName + ".EntityModel;");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }
        private void CreateCodeNamespaceConfigsOpens(IGeneratorWriter scriptWriter, string namespaceName)
        {
            scriptWriter.WriteCodeLine("namespace " + namespaceName + ".EntityConfiguration");
            scriptWriter.WriteCodeLine("{");
        }

        private void CreateCodeClassConfigsOpens(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, string blokIndent)
        {
            string tableName = tableInfo.TableName();

            string className = ClassName(EntityName(tableInfo));

            scriptWriter.WriteCodeLine(blokIndent + "// " + tableName + " : Declaration of the " + className);
            scriptWriter.WriteCodeLine(blokIndent + "public class " + className + "Configuration : EntityTypeConfiguration<" + className + ">");
            scriptWriter.WriteCodeLine(blokIndent + "{");

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            scriptWriter.WriteCodeLine(blokIndent + "public " + className + "Configuration()");
            scriptWriter.WriteCodeLine(blokIndent + "{");
        }

        private void CreateCodeClassConfigsTableBody(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, UInt32 buildVersion, string blokIndent)
        {
            string tableName = tableInfo.TableName();

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            scriptWriter.WriteCodeLine(blokIndent + "ToTable(\"" + tableName + "\");");

            IList<string> xpkcolList = tableInfo.PrimaryKeyCamelColumnList();

            string keyNames = "";

            foreach (string columnName in xpkcolList)
            {
                string propertyName = columnName.ConvertNameToCamel();

                keyNames += "p." + propertyName + ", ";
            }
            scriptWriter.WriteCodeLine(blokIndent + "HasKey(p => new { " + keyNames.TrimEnd(TRIM_CHARS) + " });");

            IList<TableFieldInfo> columnList = tableInfo.TableColumnsForVersion(buildVersion);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                IList<string> columnNames = AllClassColumnNames(columnInfo);

                foreach (string columnName in columnNames)
                {
                    int columnType = columnInfo.ColumnType;

                    int columnMaxx = columnInfo.DbColumnSize();

                    string propertyName = columnName.ConvertNameToCamel();

                    scriptWriter.WriteCodeLine(blokIndent + "Property(d => d." + propertyName + ").HasColumnName(\"" + columnName + "\");");
                }
            }

            bool bRelationsHas = false;

            if (bRelationsHas)
            {
                IList<RelationDefInfo> relationList = tableInfo.Relations();

                foreach (RelationDefInfo relationInfo in relationList)
                {
                    string relationName = relationInfo.InfoName();

                    scriptWriter.WriteCodeLine(blokIndent + "Has" + relationName);
                }
            }
        }

        private void CreateCodeClassConfigsQueryBody(IGeneratorWriter scriptWriter, TableDefInfo tableInfo, UInt32 buildVersion, string blokIndent)
        {
            string tableName = tableInfo.TableName();

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            scriptWriter.WriteCodeLine(blokIndent + "ToTable(\"" + tableName + "\");");

            IList<TableFieldInfo> columnList = tableInfo.TableColumnsForVersion(buildVersion);

            foreach (TableFieldInfo columnInfo in columnList)
            {
                IList<string> columnNames = AllClassColumnNames(columnInfo);

                foreach (string columnName in columnNames)
                {
                    int columnType = columnInfo.ColumnType;

                    int columnMaxx = columnInfo.DbColumnSize();

                    string propertyName = columnName.ConvertNameToCamel();

                    scriptWriter.WriteCodeLine(blokIndent + "Property(d => d." + propertyName + ").HasColumnName(\"" + columnName + "\");");
                }
            }
        }

        private static void CreateCodeClassConfigsClose(IGeneratorWriter scriptWriter, string blokIndent)
        {
            scriptWriter.WriteCodeLine(blokIndent + "}");

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            scriptWriter.WriteCodeLine(blokIndent + "}");

            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }

        #endregion
       
        #region CONTEXT_ENTITY_FILES
        public override void CreateDbSetContxFile(IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, UInt32 buildVersion, IGeneratorWriter scriptWriter)
        {
            string namespaceName = ContextProj();

            CreateCodeContextImports(scriptWriter, namespaceName);

            CreateCodeNamespaceContextOpens(scriptWriter, namespaceName);

            string contextClassName = ContextName().ConvertNameToCamel();

            string contextPartyName = ContextPart().ConvertNameToCamel();

            string blokIndent = "";

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassContextOpens(scriptWriter, contextClassName, blokIndent);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            CreateCodeClassContextBody(scriptWriter, tableList, queryList, contextClassName, contextPartyName, blokIndent);

            blokIndent = IndentBack(blokIndent, TAB_INDENT1);

            CreateCodeClassContextClose(scriptWriter, blokIndent);

            CreateCodeNamespaceClose(scriptWriter);
        }

        private void CreateCodeContextImports(IGeneratorWriter scriptWriter, string namespaceName)
        {
            scriptWriter.WriteCodeLine("using System;");
            scriptWriter.WriteCodeLine("using System.Linq;");
            scriptWriter.WriteCodeLine("using System.Text;");
            scriptWriter.WriteCodeLine("using System.Threading.Tasks;");
            scriptWriter.WriteCodeLine("using System.Data.Entity;");
            scriptWriter.WriteCodeLine("using " + namespaceName + ".EntityModel;");
            scriptWriter.WriteCodeLine("using " + namespaceName + ".EntityConfiguration;");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }
        private void CreateCodeNamespaceContextOpens(IGeneratorWriter scriptWriter, string namespaceName)
        {
            scriptWriter.WriteCodeLine("namespace " + namespaceName + ".EntityContext");
            scriptWriter.WriteCodeLine("{");
        }

        private void CreateCodeClassContextOpens(IGeneratorWriter scriptWriter, string className, string blokIndent)
        {
            scriptWriter.WriteCodeLine(blokIndent + "public class " + className + "Context : DbContext");
            scriptWriter.WriteCodeLine(blokIndent + "{");
        }

        private void CreateCodeClassContextClose(IGeneratorWriter scriptWriter, string blokIndent)
        {
            scriptWriter.WriteCodeLine(blokIndent + "}");
            scriptWriter.WriteCodeLine(EMPTY_SPACES);
        }

        private void CreateCodeClassContextBody(IGeneratorWriter scriptWriter, IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, string contextName, string contextPart, string blokIndent)
        {
            scriptWriter.WriteCodeLine("#region " + contextName + "." + contextPart);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            foreach (var tableInfo in tableList)
            {
                string tableName = tableInfo.TableName();

                string className = ClassName(EntityName(tableInfo));

                scriptWriter.WriteCodeLine(blokIndent + "public DbSet<" + className + "> " + className + " { get; set; }");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }

            scriptWriter.WriteCodeLine("#endregion");

            scriptWriter.WriteCodeLine("#region " + contextName + "." + contextPart);

            blokIndent = IndentPlus(blokIndent, TAB_INDENT1);

            foreach (var queryInfo in queryList)
            {
                var tableInfo = queryInfo.GetTableDef();

                string tableName = tableInfo.TableName();

                string className = ClassName(EntityName(tableInfo));

                scriptWriter.WriteCodeLine(blokIndent + "public DbSet<" + className + "> " + className + " { get; set; }");
                scriptWriter.WriteCodeLine(EMPTY_SPACES);
            }

            scriptWriter.WriteCodeLine("#endregion");

            scriptWriter.WriteCodeLine(blokIndent + "protected override void OnModelCreating");
            scriptWriter.WriteCodeLine(blokIndent + TAB_INDENT1 + "(DbModelBuilder modelBuilder)");
            scriptWriter.WriteCodeLine(blokIndent + "{");

            scriptWriter.WriteCodeLine("#region " + contextName + "." + contextPart);

            foreach (TableDefInfo tableInfo in tableList)
            {
                string className = ClassName(EntityName(tableInfo));

                scriptWriter.WriteCodeLine(blokIndent + "modelBuilder.Configurations.Add(new " + className + "Configuration());");
            }

            scriptWriter.WriteCodeLine("#endregion");

            scriptWriter.WriteCodeLine("#region " + contextName + "." + contextPart);

            foreach (var queryInfo in queryList)
            {
                var tableInfo = queryInfo.GetTableDef();

                string className = ClassName(EntityName(tableInfo));

                scriptWriter.WriteCodeLine(blokIndent + "modelBuilder.Configurations.Add(new " + className + "Configuration());");
            }

            scriptWriter.WriteCodeLine("#endregion");

            scriptWriter.WriteCodeLine(blokIndent + "}");
        }

        #endregion

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
            return ".cs";
        }

        public override string Prefix()
        {
            return "CSharp";
        }
    }
}
